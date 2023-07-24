using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Security.Claims;
using Coravel.Events.Interfaces;
using Microsoft.Extensions.Configuration;
using Spark.Templates.Blazor.Application.Events;
using Spark.Templates.Blazor.Application.Models;
using Spark.Templates.Blazor.Application.Services.Auth;

namespace Spark.Templates.Blazor.Pages.Auth
{
	public class RegisterModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private readonly UsersService _usersService;
        private readonly AuthService _cookieService;
        private IDispatcher _dispatcher;

        [BindProperty]
        public InputModel Input { get; set; }
        public string ReturnUrl { get; set; }

        public RegisterModel(
            IConfiguration configuration,
            UsersService usersService,
            AuthService cookieService,
            IDispatcher dispatcher)
        {
            _configuration = configuration;
            _usersService = usersService;
            _cookieService = cookieService;
            _dispatcher = dispatcher;
        }

        public void OnGet()
        {
            ReturnUrl = Url.Content("~/");
        }

        public async Task<IActionResult> OnPost()
        {

            if (!ModelState.IsValid)
                return Page();

            if (Input == null)
            {
                return BadRequest("user is not set.");
            }

            var existingUser = await _usersService.FindUserByEmailAsync(Input.Email);

            if (existingUser != null)
            {
                ModelState.AddModelError("EmailExists", "Email already in use by another account.");
                return Page();
            }

            var userForm = new User()
            {
                Name = Input.Name,
                Email = Input.Email,
                Password = _usersService.GetSha256Hash(Input.Password),
                CreatedAt = DateTime.UtcNow
            };

            var newUser = await _usersService.CreateUserAsync(userForm);

            // Broadcast user created event. Sends welcome email
            var userCreated = new UserCreated(newUser);
            await _dispatcher.Broadcast(userCreated);

            var user = await _usersService.FindUserAsync(newUser.Email, newUser.Password);

            var cookieExpirationDays = _configuration.GetValue("Spark:Auth:CookieExpirationDays", 5);
            var cookieClaims = await _cookieService.CreateCookieClaims(user);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                cookieClaims,
                new AuthenticationProperties
                {
                    IsPersistent = true,
                    IssuedUtc = DateTimeOffset.UtcNow,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddDays(cookieExpirationDays)
                });

            return Redirect("~/");
        }
    }

    public class InputModel
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
