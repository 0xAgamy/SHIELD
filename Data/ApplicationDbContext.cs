using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shield.Models;

namespace Shield.Data
{
	public class ApplicationDbContext : IdentityDbContext<User,Role,string>
	{
		public ApplicationDbContext(DbContextOptions options) : base(options)
		{
			
		}

		public DbSet<User>users { get; set; }
		public DbSet<Role> roles { get; set; }
		public DbSet<EmailVerificationToken> emailVerificationTokens { get; set; }
		public DbSet<PasswordVerificationToken> passwordVerificationTokens { get; set; }
	}



}
