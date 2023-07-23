using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Spark.Library.Auth;
using System.Globalization;
using System.Security.Claims;
using Spark.Templates.Blazor.Application.Models;

namespace Spark.Templates.Blazor.Application.Services.Auth
{
    public class CookieService
    {

        private readonly RolesService _rolesService;

        public CookieService(RolesService rolesService)
        {
            _rolesService = rolesService;
        }

        public async Task<ClaimsPrincipal> CreateCookieClaims(User user)
        {
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString(CultureInfo.InvariantCulture)));
            identity.AddClaim(new Claim(ClaimTypes.Email, user.Email));
            identity.AddClaim(new Claim(ClaimTypes.Name, user.Name));
            identity.AddClaim(new Claim(ClaimTypes.UserData, user.Id.ToString(CultureInfo.InvariantCulture)));

            // add roles
            var roles = await _rolesService.FindUserRolesAsync(user.Id);
            foreach (var role in roles)
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, role.Name));
            }

            return new ClaimsPrincipal(identity);
        }
    }
}
