using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Spark.Library.Auth;
using System.Globalization;
using System.Security.Claims;
using Spark.Templates.Blazor.Application.Models;

namespace Spark.Templates.Blazor.Application.Services.Auth;

public class AuthService
{
    private readonly UsersService _usersService;
    private readonly RolesService _rolesService;
    private readonly AuthenticationStateProvider _stateProvider;

    public AuthService(RolesService rolesService, UsersService usersService, AuthenticationStateProvider stateProvider)
    {
        _rolesService = rolesService;
        _usersService = usersService;
        _stateProvider = stateProvider;
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
