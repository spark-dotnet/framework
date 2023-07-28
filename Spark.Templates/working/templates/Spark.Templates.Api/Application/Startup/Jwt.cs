using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Spark.Templates.Api.Application.Models;
using System.Text;

namespace Spark.Templates.Api.Application.Startup
{
	public static class Jwt
	{
		public static IServiceCollection AddJwt(this IServiceCollection services, IConfiguration config)
		{
			services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(options =>
			{
				options.TokenValidationParameters = new TokenValidationParameters()
				{
					ClockSkew = TimeSpan.Zero,
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidateLifetime = true,
					ValidateIssuerSigningKey = true,
					ValidIssuer = config.GetValue("Spark:Jwt:Issuer", "https://spark-framework.net"),
					ValidAudience = config.GetValue("Spark:Jwt:Audience", "https://spark-framework.net"),
					IssuerSigningKey = new SymmetricSecurityKey(
					Encoding.UTF8.GetBytes(config.GetValue("Spark:Jwt:Key", "SomthingSecret!"))),
				};
			});
			
			services.AddAuthorization(options =>
			{
				options.AddPolicy(CustomRoles.User, policy =>
				{
					policy.RequireAuthenticatedUser();
					policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
					policy.RequireRole(CustomRoles.User);
				});

				options.AddPolicy(CustomRoles.Admin, policy =>
				{
					policy.RequireAuthenticatedUser();
					policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
					policy.RequireRole(CustomRoles.Admin);
				});

				options.DefaultPolicy = new AuthorizationPolicyBuilder()
					   .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
					   .RequireAuthenticatedUser()
					   .Build();
			});

			return services;
		}
	}
}
