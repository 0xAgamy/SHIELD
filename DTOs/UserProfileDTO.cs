namespace Shield.DTOs
{
	public class UserProfileDTO
	{
		public string Email { get; set; }
		public string Usernmae { get; set; }
		public IList<string> roles { get; set; }
	}
}
