using Coravel.Events.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Globalization;
using System.Security.Claims;
using Spark.Templates.Razor.Application.Events;
using Spark.Templates.Razor.Application.Models;
using Spark.Templates.Razor.Application.Services.Auth;
using Spark.Templates.Razor.Application.ViewModels;
using System.Threading.Tasks;
using System;

namespace Spark.Templates.Razor.Pages.Auth
{
	[AllowAnonymous]
	public class RegisterModel : PageModel
	{
		private readonly RolesService _rolesService;
		private readonly UsersService _usersService;
		private IDispatcher _dispatcher;

		[BindProperty]
		public Register Register { get; set; }

		public RegisterModel(UsersService usersService, RolesService rolesService, IDispatcher dispatcher)
		{
			_usersService = usersService;
			_rolesService = rolesService;
			_dispatcher = dispatcher;
		}

		public void OnGet()
		{
		}

		public async Task<IActionResult> OnPost()
		{
			if (!ModelState.IsValid)
				return new PageResult();

			if (Register == null)
			{
				ModelState.AddModelError("FailedLogin", "Login Failed: user is not set.");
				return new PageResult();
			}

			var existingUser = await _usersService.FindUserByEmailAsync(Register.Email);

			if (existingUser != null)
			{
				ModelState.AddModelError("EmailExists", "Email already in use by another account.");
				return new PageResult();
			}

			var userForm = new User()
			{
				Name = Register.Name,
				Email = Register.Email,
				Password = _usersService.GetSha256Hash(Register.Password),
				CreatedAt = DateTime.UtcNow
			};
			var newUser = await _usersService.CreateUserAsync(userForm);

			// broadcast user created event
			var userCreated = new UserCreated(newUser);
			await _dispatcher.Broadcast(userCreated);

			var user = await _usersService.FindUserAsync(newUser.Email, newUser.Password);

			var loginCookieExpirationDays = 30;
			var cookieClaims = await createCookieClaimsAsync(user);
			await HttpContext.SignInAsync(
				CookieAuthenticationDefaults.AuthenticationScheme,
				cookieClaims,
				new AuthenticationProperties
				{
					IsPersistent = true,
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
