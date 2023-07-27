using Microsoft.IdentityModel.Tokens;
using Spark.Templates.Api.Application.Models;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Spark.Templates.Api.Application.Services.Auth
{
	public class TokenService
	{
		private readonly IConfiguration _configuration;
		private readonly RolesService _rolesService;

		public TokenService(IConfiguration configuration, RolesService rolesService)
        {
			_configuration = configuration;
			_rolesService = rolesService;
		}

        public async Task<string> CreateJwtToken(User user)
		{
			var tokenExpirationDays = _configuration.GetValue("Spark:Jwt:ExpirationDays", 5);
			var expiration = DateTime.UtcNow.AddDays(tokenExpirationDays);
			var token = CreateJwtToken(
				await CreateJwtClaimsAsync(user),
				CreateJwtSigningCredentials(),
				expiration
			);
			var tokenHandler = new JwtSecurityTokenHandler();
			return tokenHandler.WriteToken(token);
		}

		private JwtSecurityToken CreateJwtToken(List<Claim> claims, SigningCredentials credentials,
			DateTime expiration) =>
			new(
				_configuration.GetValue("Spark:Jwt:Issuer", "https://spark-framework.net"),
				_configuration.GetValue("Spark:Jwt:Audience", "https://spark-framework.net"),
				claims,
				expires: expiration,
				signingCredentials: credentials
			);

		private async Task<List<Claim>> CreateJwtClaimsAsync(User user)
		{
			var claims = new List<Claim>
				{	
					new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
					new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)),
					new Claim(ClaimTypes.NameIdentifier, user.Id.ToString(CultureInfo.InvariantCulture)),
					new Claim(ClaimTypes.Email, user.Email),
					new Claim(ClaimTypes.Name, user.Name),
					new Claim(ClaimTypes.UserData, user.Id.ToString(CultureInfo.InvariantCulture))
				};

			// add roles
			var roles = await _rolesService.FindUserRolesAsync(user.Id);
			foreach (var role in roles)
			{
				claims.Add(new Claim(ClaimTypes.Role, role.Name));
			}

			return claims;
		}
		private SigningCredentials CreateJwtSigningCredentials()
		{
			return new SigningCredentials(
				new SymmetricSecurityKey(
					Encoding.UTF8.GetBytes(_configuration.GetValue("Spark:Jwt:Key", "SomthingSecret!"))
				),
				SecurityAlgorithms.HmacSha256
			);
		}
	}
}
