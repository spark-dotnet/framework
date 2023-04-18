using BlazorSpark.Library.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace BlazorSpark.Default.Application.Services.Auth
{
    public class CookieService : ICookieService
    {
        private readonly UsersService usersService;
        public CookieService(UsersService usersService)
        {
            this.usersService = usersService;
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

            var user = await usersService.FindUserAsync(userId);
            if (user == null)
            {
                await handleUnauthorizedRequest(context);
            }
        }

        private Task handleUnauthorizedRequest(CookieValidatePrincipalContext context)
        {
            context.RejectPrincipal();
            return context.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }

}
