using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Shield.DTOs;
using Shield.IRepos;
using Shield.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Shield.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private readonly IUserRepo _userRepo;

		public UserController(IUserRepo userRepo)
		{
			_userRepo = userRepo;
		}


		[HttpPost("Register")]
		public async Task<IActionResult> Register(RegisterDTO model)
		{
			try
			{
				return Ok(new {messege = await _userRepo.RegisterAsync(model) } );
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });

			}


		}

		[HttpPost("Login")]
		public async Task<IActionResult> Login(LoginDTO model)
		{
			try
			{
				return Ok(new {token = await _userRepo.LoginAsync(model) });
			}
			catch (UnauthorizedAccessException ex)
			{
				return Unauthorized(new { message = ex.Message });
			}
			catch (Exception ex)
			{
				return BadRequest(new {message = ex.Message });
			}




		}

		[HttpPut("ChangePassword")]
		public async Task<IActionResult> ChangeUserPassword(ChangePasswordDTO model)
		{
            if (User is null) return Unauthorized();
            var userid = User.FindFirst(JwtRegisteredClaimNames.Sid)?.Value;
            if (userid is null) return Unauthorized();
			try
			{
				return Ok(new { message = await _userRepo.ChangeUserPassword(userid, model.CurrentPassword, model.NewPassword) });
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}


		[HttpPut("ChangeEmail")]
		public async Task<IActionResult>ChnageUserEmail(ChangeEmailDTO model)
		{
            if (User is null) return Unauthorized();
            var userid = User.FindFirst(JwtRegisteredClaimNames.Sid)?.Value;
            if (userid is null) return Unauthorized();
			try
			{
				return Ok(new { message = await _userRepo.ChangeUserEmail(userid, model.newEmail) });
			}
			catch (Exception ex) {

				return BadRequest(new { error = ex.Message });
			}
        }


		[HttpDelete("DeleteAccount")]
		public async Task<IActionResult> DeleterUsrAccount()
		{
            if (User is null) return Unauthorized();
            var userid = User.FindFirst(JwtRegisteredClaimNames.Sid)?.Value;
            if (userid is null) return Unauthorized();
			try
			{
				return Ok(new { message = await _userRepo.DeleteUserAccount(userid) });
			}
			catch (Exception ex) { return BadRequest(new {message = ex.Message }); }


        }
		[HttpGet("Profile")]
		
		public async Task<IActionResult> GetUserProfile()
		{
			if (User is null) return Unauthorized();
			var userid = User.FindFirst(JwtRegisteredClaimNames.Sid)?.Value;
			if (userid is null) return Unauthorized();
			return Ok (await _userRepo.GetUserProfileAsync(userid));
		}

		[HttpGet("comfirmEmail")]
		public async Task<IActionResult> Confrim(string userId, string token)
		{
			try
			{
				return Ok(await _userRepo.ConfirmUserEmail(userId, token));
			}
			catch (Exception ex)
			{

				return BadRequest(ex.Message);
			}
		}

        [HttpGet("ConfirmEmailChange")]
        public async Task<IActionResult> ConfirmEmailChange(string userId, string newEmail, string token)
        {
            try
            {
                var result = await _userRepo.ConfirmEmailChagen(userId, newEmail, token);
                if (result)
                    return Ok("Email change confirmed successfully!");

                return BadRequest("Error confirming email change.");
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

    }
}
