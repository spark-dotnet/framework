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
using System.ComponentModel.DataAnnotations;

namespace Spark.Templates.Razor.Pages.Auth
{
	[AllowAnonymous]
	public class LoginModel : PageModel
	{
		private readonly UsersService _usersService;
		private readonly AuthService _authService;
		private readonly IConfiguration _configuration;

		[BindProperty]
		public Login Login { set; get; }

		public LoginModel(UsersService usersService, IConfiguration configuration, AuthService authService)
		{
			_usersService = usersService;
			_configuration = configuration;
			_authService = authService;
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

			var loginCookieExpirationDays = _configuration.GetValue("Spark:Auth:CookieExpirationDays", 5);
			var cookieClaims = await _authService.CreateCookieClaims(user);

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
	}

    public class Login
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; } = false;
    }
}
