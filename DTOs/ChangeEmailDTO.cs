using System.ComponentModel.DataAnnotations;

namespace Shield.DTOs
{
    public class ChangeEmailDTO
    {
        [EmailAddress]
    public string newEmail { get; set; }
    }
}
