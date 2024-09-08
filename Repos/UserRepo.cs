using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Shield.Controllers;
using Shield.Data;
using Shield.DTOs;
using Shield.IRepos;
using Shield.Models;
using Shield.Services;
using System;
using System.Net.Mail;

namespace Shield.Repos
{
	public class UserRepo : IUserRepo
	{
		private readonly JWTServices _jwtServices;
		private readonly UserManager<User> _userManager;

		private readonly SignInManager<User> _signInManager;

		private readonly EmailServices _emailServices;

		private readonly IHttpContextAccessor _httpContextAccessor;

		//  private readonly RoleManager<IdentityRole> _roleManager;

		public UserRepo(JWTServices jwtServices, UserManager<User> userManager, SignInManager<User> signInManager, EmailServices emailServices, IHttpContextAccessor httpContextAccessor)
		{
			_jwtServices = jwtServices;
			_userManager = userManager;
			_signInManager = signInManager;
			_emailServices = emailServices;
			_httpContextAccessor = httpContextAccessor;
		}

		// Auth Methods
		public async Task<string> RegisterAsync(RegisterDTO registerDTO)
		{

			if (registerDTO.Password != registerDTO.ConfirmPassword)
				throw new Exception("password and confirm password doesn't match");
			// Check if E-mail Exist
			var u = await _userManager.FindByEmailAsync(registerDTO.Email);
			if (u is not null) throw new Exception("The Email Already exist");


			var user = new User
			{
				Email = registerDTO.Email,
				PasswordHash = registerDTO.Password,
				UserName = registerDTO.Username,



			};
			var result = await _userManager.CreateAsync(user, registerDTO.Password);
			await _userManager.AddToRoleAsync(user, "user");
			//

			var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

			var confirmationLink = GenerateConfirmationLink(user.Id, token);


			_emailServices.SendEmail(user.Email, "SHIELD Email Confirmation",
		   $"Please confirm your email by clicking here: <a href='{confirmationLink}'>Confirm</a>");
			//
			var errorMsg = result.Errors.FirstOrDefault()?.Description.ToString();
			if (!result.Succeeded)
				throw new Exception(errorMsg);

			return "Registration successful. Please check your email to confirm your account.";



		}

		public async Task<string> LoginAsync(LoginDTO model)
		{
			var user = await _userManager.FindByEmailAsync(model.Email);
			if (user is null) throw new UnauthorizedAccessException("Invalid Email or Password");

			var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

			if (!result.Succeeded) return "Cannot Login";
			return await _jwtServices.GenerateJwtToken(user.Id);
			// Should check if the user Confirm his e-mail

		}



		// User Profile Data

		public async Task<UserProfileDTO> GetUserProfileAsync(string userId)
		{
			var user = await _userManager.FindByIdAsync(userId);
			if (user is null) throw new Exception("user Doesn't exist ");

			var userRoles = await _userManager.GetRolesAsync(user);

			var usrProfile = new UserProfileDTO
			{
				Email = user.Email,
				Usernmae = user.UserName,
				roles = userRoles
			};
			return usrProfile;



		}




		// User Email Confirm

		private string GenerateConfirmationLink(string userId, string token)
		{
			var request = _httpContextAccessor.HttpContext.Request;
			var confirmationLink = new Uri($"{request.Scheme}://{request.Host}/api/User/comfirmEmail?userId={userId}&token={Uri.EscapeDataString(token)}");
			return confirmationLink.ToString();
		}

		public async Task<string> ConfirmUserEmail(string userId, string Token)
		{
			if (userId == null || Token == null)
			{
				throw new BadHttpRequestException("Invalid Email Confirm Request");
			}

			var user = await _userManager.FindByIdAsync(userId);
			if (user is null) throw new Exception("User Not found");

			var result = await _userManager.ConfirmEmailAsync(user, Token);
			var message = "Email Confirmed Successful";
			if (!result.Succeeded) throw new Exception("Email not Confirmed");
			return message;

		}

        private string GenerateEmailChangeConfirmationLink(string userId,string newEmail, string token)
        {
            var request = _httpContextAccessor.HttpContext.Request;
            var confirmationLink = new Uri($"{request.Scheme}://{request.Host}/api/User/ConfirmEmailChange?userId={userId}&newEmail={Uri.EscapeDataString(newEmail)}&token={Uri.EscapeDataString(token)}");
            return confirmationLink.ToString();
        }

        // User Chagne Settings

        public async Task<string> ChangeUserPassword(string userId, string currentPass, string newPass)
		{
			var user = await _userManager.FindByIdAsync(userId);
			if (user is null) throw new Exception("User Not found");

			var result = await _userManager.ChangePasswordAsync(user, currentPass, newPass);
			if (!result.Succeeded)
			{
				var errorMsg = result.Errors.FirstOrDefault()?.Description ?? "Error changing password.";
				throw new Exception(errorMsg);
			}
			return "Password change successfuly";
		}
		public async Task<string> ChangeUserEmail(string userId, string newEmail)
		{
			var user =await  _userManager.FindByIdAsync(userId);
			if (user is null)
				throw new Exception("user Not Found");

			var emailExist = await _userManager.FindByEmailAsync(newEmail);
			if (emailExist != null)
				throw new Exception("The new Email already in use");

			var token = await _userManager.GenerateChangeEmailTokenAsync(user,newEmail);

			var confirmationLink = GenerateEmailChangeConfirmationLink(user.Id, newEmail, token);

             _emailServices.SendEmail(newEmail, "Confirm your new email",
			 $"Please confirm your new email by clicking here: <a href='{confirmationLink}'>link</a>");

            return "Email change requested. Please check your new email to confirm.";
        }

       public async Task<bool> ConfirmEmailChagen(string userId, string newEmail, string Token)
		{
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new Exception("User not found.");

            var result = await _userManager.ChangeEmailAsync(user, newEmail, Token);
            if (result.Succeeded)
            {
                await _userManager.SetUserNameAsync(user, newEmail); // Update the username if it's based on email
                return true;
            }

            return false;
        }


        //

        public async Task<string> DeleteUserAccount(string UserId)
		{
			var user = await _userManager.FindByIdAsync(UserId);
			if (user is null) throw new Exception("user not found");
			var result = await _userManager.DeleteAsync(user);
			if (!result.Succeeded)
			{
                var errorMsg = result.Errors.FirstOrDefault()?.Description ?? "Error deleting user.";
                throw new Exception(errorMsg);
            }
			return "Account Deleted Successfully";

		}


    }
}
