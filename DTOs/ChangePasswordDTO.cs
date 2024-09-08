using System.ComponentModel;

namespace Shield.DTOs
{
    public class ChangePasswordDTO
    {
        [PasswordPropertyText]
        public string CurrentPassword { get; set; }
        [PasswordPropertyText]

        public string NewPassword { get; set; }
    }
}
