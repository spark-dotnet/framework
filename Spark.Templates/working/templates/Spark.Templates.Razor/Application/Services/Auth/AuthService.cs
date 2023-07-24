using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Spark.Templates.Razor.Application.Models;
using System.Globalization;
using System.Security.Claims;

namespace Spark.Templates.Razor.Application.Services.Auth
{
    public class AuthService
    {
        private readonly UsersService _usersService;
        private readonly RolesService _rolesService;
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _contextAccessor;
        private HttpContext _context;

        public HttpContext Context
        {
            get
            {
                var context = _context ?? _contextAccessor?.HttpContext;
                if (context == null)
                {
                    throw new InvalidOperationException("HttpContext must not be null.");
                }
                return context;
            }
            set
            {
                _context = value;
            }
        }

        public AuthService(RolesService rolesService, IConfiguration config, IHttpContextAccessor contextAccessor, UsersService usersService)
        {
            _rolesService = rolesService;
            _config = config;
            _contextAccessor = contextAccessor;
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

        public async Task RefreshSignIn(User user)
        {
            await Context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            var cookieClaims = await this.CreateCookieClaims(user);
            var loginCookieExpirationDays = _config.GetValue("Spark:Auth:CookieExpirationDays", 5);
            await Context.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                cookieClaims,
                new AuthenticationProperties
                {
                    IsPersistent = true,
                    IssuedUtc = DateTimeOffset.UtcNow,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddDays(loginCookieExpirationDays)
                });
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
