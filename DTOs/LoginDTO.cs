using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Shield.DTOs
{
    public class LoginDTO
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        [PasswordPropertyText]
        [Required]
        public string Password { get; set; }
    }
}
