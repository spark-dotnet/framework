using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Spark.Library.Auth;
using System.Globalization;
using System.Security.Claims;
using Spark.Templates.Blazor.Application.Models;

namespace Spark.Templates.Blazor.Application.Services.Auth
{
    public class AuthService : ICookieService
    {

        private readonly RolesService _rolesService;
        private readonly UsersService _usersService;
        private readonly AuthenticationStateProvider _stateProvider;

        public AuthService(RolesService rolesService, UsersService usersService, AuthenticationStateProvider stateProvider)
        {
            _rolesService = rolesService;
            _usersService = usersService;
            _stateProvider = stateProvider;
        }

        public async Task ValidateAsync(CookieValidatePrincipalContext context)
        {
            var claimsIdentity = context.Principal.Identity as ClaimsIdentity;
            if (claimsIdentity?.Claims == null || !claimsIdentity.Claims.Any())
            {
                // this is not our issued cookie
                await handleUnauthorizedRequest(context);
                return;
            }

            var userIdString = claimsIdentity.FindFirst(ClaimTypes.UserData).Value;
            if (!int.TryParse(userIdString, out int userId))
            {
                await handleUnauthorizedRequest(context);
                return;
            }

            var user = await _usersService.FindUserAsync(userId);
            if (user == null)
            {
                await handleUnauthorizedRequest(context);
            }
        }

        public async Task<User?> GetAuthenticatedUser()
        {
            var userId = await GetAuthenticatedUserId();
            if (userId != null)
            {
                var id = userId ?? default(int);
                return await _usersService.FindUserAsync(id);
            }
            return null;
        }

        public async Task<int?> GetAuthenticatedUserId()
        {
            var authState = await _stateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            if (user.Identity != null && user.Identity.IsAuthenticated)
            {
                var userIdString = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!int.TryParse(userIdString, out var userId))
                {
                    return null;
                }
                return userId;
            }
            return null;
        }

        public async Task<ClaimsPrincipal> CreateCookieClaims(User user)
        {
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString(CultureInfo.InvariantCulture)));
            identity.AddClaim(new Claim(ClaimTypes.Name, user.Email));
            identity.AddClaim(new Claim(ClaimTypes.UserData, user.Id.ToString(CultureInfo.InvariantCulture)));

            // add roles
            var roles = await _rolesService.FindUserRolesAsync(user.Id);
            foreach (var role in roles)
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, role.Name));
            }

            return new ClaimsPrincipal(identity);
        }

        private Task handleUnauthorizedRequest(CookieValidatePrincipalContext context)
        {
            context.RejectPrincipal();
            return context.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}
