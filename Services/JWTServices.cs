using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Shield.Models;
using Shield.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Shield.Services
{
	public class JWTServices
	{
		private readonly JWTOptions _jWTOptions;
        private readonly UserManager<User> _userManager;

        public JWTServices(JWTOptions jWTOptions , UserManager<User> userManager )
        {
			_jWTOptions = jWTOptions;
            _userManager = userManager;
        }

		public async Task<string> GenerateJwtToken(string userid)
		{
			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jWTOptions.Key));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
			 
			var user = await _userManager.FindByIdAsync(userid);
			var userRoles = await _userManager.GetRolesAsync(user);

			var claims = new List<Claim>
			{
				new Claim(JwtRegisteredClaimNames.Sid,userid),
			};
            claims.AddRange(userRoles.Select(role => new Claim("role", role)));


            var token = new JwtSecurityToken(
			  issuer: _jWTOptions.Issuer,
			  audience: _jWTOptions.Audience,
			  claims: claims,
			  expires: DateTime.Now.AddMinutes(Convert.ToDouble(_jWTOptions.DurationInMinutes)),
			  signingCredentials: creds

			 );
			return new JwtSecurityTokenHandler().WriteToken(token);
				

		}
	}
}
