using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Shield.DTOs
{
	public class RegisterDTO
	{
		[Required]
		[MaxLength(25)]
		public string Username { get; set; }
		[EmailAddress]
		[Required]
		public string Email { get; set; }
		[Required]
		public string Password { get; set; }
		[Required]
		public string ConfirmPassword { get; set; }
	}
}
