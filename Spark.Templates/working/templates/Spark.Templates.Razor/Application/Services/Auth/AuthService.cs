using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Spark.Library.Auth;
using System.Globalization;
using System.Security.Claims;
using Spark.Templates.Razor.Application.Models;
using Spark.Templates.Razor.Application.Services.Auth;

namespace Spark.Templates.Razor.Application.Services.Auth;

public class AuthService
{
    private readonly UsersService _usersService;
    private readonly RolesService _rolesService;

    public AuthService(RolesService rolesService, UsersService usersService)
    {
        _rolesService = rolesService;
        _usersService = usersService;
    }

    public async Task<User?> GetAuthenticatedUser(ClaimsPrincipal User)
    {
        if (User.Identity != null && User.Identity.IsAuthenticated)
        {
            var userId = this.GetAuthenticatedUserId(User);
            if (userId != null)
            {
                var id = userId ?? default(int);
                return await _usersService.FindUserAsync(id);
            }
            return null;
        }
        return null;
    }

    public int? GetAuthenticatedUserId(ClaimsPrincipal User)
    {
        var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!int.TryParse(userIdString, out var userId))
        {
            return null;
        }
        return userId;
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
