
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shield.Data;
using Shield.IRepos;
using Shield.Models;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Shield.Repos
{
	public class RoleRepo : IRoleRepo
	{
        private readonly ApplicationDbContext _context;
		private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<User> _userManager;

        public RoleRepo(ApplicationDbContext context, RoleManager<Role> roleManager , UserManager<User> userManager  )
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }


        public async Task<string> CreateRole(string Rolename)
		{
			if(string.IsNullOrWhiteSpace(Rolename)) throw new ArgumentNullException(nameof(Rolename));
			if(string.IsNullOrEmpty(Rolename)) throw new ArgumentNullException(nameof(Rolename));
	
			string pattern = @"^[a-zA-Z\s]+$";
			Regex regex = new Regex(pattern);
			bool isValid = regex.IsMatch(Rolename);
			if (!isValid) throw new Exception("the Role name Must be String");

			var role = new Role
			{
				Name = Rolename.ToLower(),
				
			};
			if (findRole(role)) throw new Exception("The Role name Already Exist");

			
			await _roleManager.CreateAsync(role);

            await _context.SaveChangesAsync();
			return role.Id;
		
		}

		public async Task<IEnumerable<Role>> GetRoles()
		{
			return await _roleManager.Roles.ToListAsync();
		}

		public async Task<string> DeleteRole(string roleId)
		{
			var role = await _roleManager.FindByIdAsync(roleId);
			if (role == null) throw new Exception("The Role Doesn't Exist");
			await _roleManager.DeleteAsync(role);
			return role.Id;
			
		}

        // User-Roles


        public async Task<IList<string>> GetUserRole(string userid)
		{
			var user = await _userManager.FindByIdAsync(userid);
			if (user == null) throw new Exception("user not exist");
			return await _userManager.GetRolesAsync(user);

		}

        public async Task<string> AddUserToRole(string UserId, string RoleName)
		{
			var user = await _userManager.FindByIdAsync(UserId);
			if (user is null) throw new Exception("User not found");
            if (await _userManager.IsInRoleAsync(user, RoleName)) throw new Exception("User Alread in Role");

            await _userManager.AddToRoleAsync(user, RoleName);
			return RoleName;
		}

       public async Task<string> RemoveUserFromRole(string UserId, string RoleName) 
		{
            var user = await _userManager.FindByIdAsync(UserId);
            if (user is null) throw new Exception("User not found");
			if (await  _userManager.IsInRoleAsync(user, RoleName) ==false ) throw new Exception("User not in Role");
			await _userManager.RemoveFromRoleAsync(user, RoleName);
			return RoleName;
		}


        public async Task<string> ChangeUserRole(string userId, string OldRoleName, string NewRoleNmae)
		{

			await RemoveUserFromRole(userId, OldRoleName);
			await AddUserToRole(userId, NewRoleNmae);
			return NewRoleNmae;

			//var user = await _userManager.FindByIdAsync(userId);
			//if (user is null) throw new Exception("The User Doesn't Exist");
			//var alluserRoles = await _userManager.GetRolesAsync(user);
			//var OldRole = alluserRoles.FirstOrDefault();

			//var NewRole=_roleManager.FindByNameAsync(roleName);

			//if (OldRole == NewRole.ToString()) throw new Exception("User Already in Role");
			//await _userManager.RemoveFromRoleAsync(user,OldRole);
			//await _userManager.AddToRoleAsync(user, NewRole.ToString());
			//return NewRole.Result!;
		}








        private bool findRole(Role role)
		{
			return _context.roles.Where(a => a.Name == role.Name).Any() ? true : false;
		}
	}
}
