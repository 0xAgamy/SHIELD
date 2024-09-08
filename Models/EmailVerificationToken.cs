using System.ComponentModel.DataAnnotations.Schema;

namespace Shield.Models
{
	public class EmailVerificationToken
	{
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public string id {  get; set; }
		public string token {  get; set; }

		public User User { get; set; }
		public string UserId { get; set; }

		public DateTime ExpirationDate { get; set; }
	}
}
