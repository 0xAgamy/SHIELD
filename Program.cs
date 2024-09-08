using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Shield.Data;
using Shield.IRepos;
using Shield.Models;
using Shield.Options;
using Shield.Repos;
using Shield.Services;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

var connectionstring = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(o => o.UseSqlServer(connectionstring));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//Identity

builder.Services.AddIdentity<User,Role>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
// JWT
var jwtOptions = builder.Configuration.GetSection("Jwt").Get<JWTOptions>();
builder.Services.AddSingleton(jwtOptions);

var emailOptions = builder.Configuration.GetSection("SmtpSettings").Get<EmailOptions>();
builder.Services.AddSingleton(emailOptions);

builder.Services.AddAuthentication(options =>
	{

		options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
		options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;


	}
)
	.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, option =>
	{
		option.SaveToken = true;
		option.TokenValidationParameters = new TokenValidationParameters
		{
			//ValidateIssuer = true,
			ValidateIssuer = false,
			ValidIssuer = jwtOptions?.Issuer,

			//ValidateAudience = true,			
			ValidateAudience = false,
			ValidAudience = jwtOptions?.Audience,

			

            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions!.Key)),
			ValidateIssuerSigningKey = true,

            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
        };
        option.Events = new JwtBearerEvents
        {
            OnTokenValidated = async context =>
            {
                var userManager = context.HttpContext.RequestServices.GetRequiredService<UserManager<User>>();
                var user = await userManager.GetUserAsync(context.Principal);

                if (user != null)
                {
                    var roles = await userManager.GetRolesAsync(user);
                    var claims = roles.Select(role => new Claim(ClaimTypes.Role, role));

                    var identity = (ClaimsIdentity)context.Principal.Identity;
                    identity.AddClaims(claims);
                }
            }
        };

    });

//builder.Services.AddAuthorization(options =>
//{
//	options.AddPolicy("AdminPolicy", policy =>
//	{
//		policy.RequireRole("admin"); // Ensure this role is correctly assigned to users
//	});
//});

// register Servicse
builder.Services.AddScoped<IRoleRepo,RoleRepo>();

builder.Services.AddScoped<JWTServices>();
builder.Services.AddScoped<EmailServices>();

builder.Services.AddScoped<IUserRepo,UserRepo>();













var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
