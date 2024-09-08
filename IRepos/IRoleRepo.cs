using Shield.Models;

namespace Shield.IRepos
{
	public interface IRoleRepo
	{
		Task<IEnumerable<Role>> GetRoles();
		Task<string> CreateRole(string Rolename);
		Task<string> DeleteRole(string roleId);


		// User Roles
		Task<IList<string>> GetUserRole(string userid);
		Task<string> AddUserToRole(string UserId, string RoleName);

		Task<string> RemoveUserFromRole(string UserId, string RoleName);

		Task<string> ChangeUserRole(string userId , string  OldRoleName,string NewRoleNmae);

	}
}
