using Shield.DTOs;

namespace Shield.IRepos
{
	public interface IUserRepo
	{
		// Auth methods
		Task<string> RegisterAsync(RegisterDTO registerDTO);
		Task<string> LoginAsync(LoginDTO user);
		// User Profile Data
		Task<UserProfileDTO> GetUserProfileAsync(string userId);
		
		// User Email Confirm
		Task<string> ConfirmUserEmail(string userId, string Token);

		// Change User settings
		Task<string> ChangeUserPassword(string userId, string currentPass, string newPass);
		Task<string> ChangeUserEmail(string userId, string newEmail);
		Task<string> DeleteUserAccount(string UserId);
		Task<bool> ConfirmEmailChagen(string userId, string newEmail, string Toekn);

	}
}
