using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Security.Claims;
using BlazorSpark.Default.Application.Models;
using BlazorSpark.Default.Application.Services.Auth;
using Coravel.Events.Interfaces;
using Microsoft.Extensions.Configuration;
using BlazorSpark.Default.Application.Events;

namespace BlazorSpark.Pages.Auth
{
    public class RegisterModel : PageModel
	{
		private readonly RolesService _rolesService;
		private readonly UsersService _usersService;
        private IDispatcher _dispatcher;

        [BindProperty]
        public InputModel Input { get; set; }
        public string ReturnUrl { get; set; }

        public RegisterModel(
            UsersService usersService,
            RolesService rolesService,
            IDispatcher dispatcher)
        {
            _usersService = usersService ?? throw new ArgumentNullException(nameof(usersService));
            _rolesService = rolesService ?? throw new ArgumentNullException(nameof(rolesService));
            _dispatcher = dispatcher ?? throw new ArgumentNullException(nameof(dispatcher));
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
			var userForm = new User()
			{
				Name = Input.Name,
				Email = Input.Email,
				Password = _usersService.GetSha256Hash(Input.Password),
				CreatedAt = DateTime.Now
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
					IsPersistent = true, // "Remember Me"
					IssuedUtc = DateTimeOffset.UtcNow,
					ExpiresUtc = DateTimeOffset.UtcNow.AddDays(loginCookieExpirationDays)
				});

			return Redirect("~/");
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
