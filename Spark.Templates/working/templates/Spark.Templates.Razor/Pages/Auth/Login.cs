using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Globalization;
using System.Security.Claims;
using System.Threading.Tasks;
using System;
using Spark.Templates.Razor.Application.Services.Auth;
using Spark.Templates.Razor.Application.ViewModels;
using Spark.Templates.Razor.Application.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace Spark.Templates.Razor.Pages.Auth
{
	[AllowAnonymous]
	public class LoginModel : PageModel
	{
		private readonly RolesService _rolesService;
		private readonly UsersService _usersService;
		private readonly IConfiguration _configuration;

		[BindProperty]
		public Login Login { set; get; }

		public LoginModel(UsersService usersService, RolesService rolesService, IConfiguration configuration)
		{
			_usersService = usersService;
			_rolesService = rolesService;
			_configuration = configuration;
		}

		public void OnGet()
		{
		}

		public async Task<IActionResult> OnPost()
		{
			if (!ModelState.IsValid)
				return new PageResult();

			if (Login == null)
			{
				ModelState.AddModelError("FailedLogin", "Login Failed: user is not set.");
				return new PageResult();
			}

			var user = await _usersService.FindUserAsync(Login.Email, _usersService.GetSha256Hash(Login.Password));

			if (user == null)
			{
				ModelState.AddModelError("FailedLogin", "Login Failed: Your email or password was incorrect");
				return new PageResult();
			}

			var loginCookieExpirationDays = _configuration.GetValue("LoginCookieExpirationDays", 30);
			var cookieClaims = await createCookieClaimsAsync(user);

			await HttpContext.SignInAsync(
				CookieAuthenticationDefaults.AuthenticationScheme,
				cookieClaims,
				new AuthenticationProperties
				{
					IsPersistent = Login.RememberMe,
					IssuedUtc = DateTimeOffset.UtcNow,
					ExpiresUtc = DateTimeOffset.UtcNow.AddDays(loginCookieExpirationDays)
				});

			return Redirect("/dashboard");
		}

		private async Task<ClaimsPrincipal> createCookieClaimsAsync(User user)
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
	}
}
