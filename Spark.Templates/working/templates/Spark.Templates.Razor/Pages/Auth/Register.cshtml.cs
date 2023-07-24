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
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Configuration;

namespace Spark.Templates.Razor.Pages.Auth
{
	[AllowAnonymous]
	public class RegisterModel : PageModel
	{
		private readonly AuthService _authService;
		private readonly UsersService _usersService;
		private readonly IConfiguration _configuration;
		private IDispatcher _dispatcher;

		[BindProperty]
		public Register Register { get; set; }

		public RegisterModel(UsersService usersService, AuthService authService, IDispatcher dispatcher, IConfiguration configuration)
		{
			_usersService = usersService;
			_authService = authService;
			_dispatcher = dispatcher;
			_configuration = configuration;
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

			var loginCookieExpirationDays = _configuration.GetValue("Spark:Auth:CookieExpirationDays", 5);
			var cookieClaims = await _authService.CreateCookieClaims(user);
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
	}

    public class Register
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email address")]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm password is required")]
        [Compare("Password", ErrorMessage = "The Password and Confirm Password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
