using FluentValidation;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Spark.Templates.Razor.Application.Services.Auth;
using FluentValidation.Results;
using FluentValidation.AspNetCore;

namespace Spark.Templates.Razor.Pages.Auth;

public class Login : PageModel
{
	[BindProperty]
	public LoginForm Form { get; set; }
	public string LoginFailedMessage = "";

	private readonly IConfiguration _configuration;
	private readonly UsersService _usersService;
	private readonly AuthService _authService;
	private readonly IValidator<LoginForm> _validator;

	public Login(IConfiguration configuration, UsersService usersService, AuthService authService, IValidator<LoginForm> validator)
	{
		_configuration = configuration;
		_usersService = usersService;
		_authService = authService;
		_validator = validator;
	}

	public async Task<IActionResult> OnPostAsync()
	{
		ValidationResult result = await _validator.ValidateAsync(Form);
		if (!result.IsValid)
		{
			result.AddToModelState(ModelState, nameof(Form));
			return Page();
		}

		var user = await _usersService.FindUserByCredsAsync(Form.Email, _usersService.GetSha256Hash(Form.Password));
		if (user == null)
		{
			LoginFailedMessage = "Login Failed: Your email or password was incorrect";
			return Page();
		}

		var cookieExpirationDays = _configuration.GetValue("Spark:Auth:CookieExpirationDays", 5);
		var cookieClaims = await _authService.CreateCookieClaims(user);

		await HttpContext.SignInAsync(
			CookieAuthenticationDefaults.AuthenticationScheme,
			cookieClaims,
			new AuthenticationProperties
			{
				IsPersistent = Form.RememberMe,
				IssuedUtc = DateTimeOffset.UtcNow,
				ExpiresUtc = DateTimeOffset.UtcNow.AddDays(cookieExpirationDays)
			});
		return RedirectToPage("/dashboard");
	}
}

public class LoginForm
{
	public string Email { get; set; }
	public string Password { get; set; }
	public bool RememberMe { get; set; }
}

public class LoginFormValidator : AbstractValidator<LoginForm>
{
	public LoginFormValidator()
	{
		RuleFor(p => p.Email)
			.EmailAddress().WithMessage("Please enter a valid email address");

		RuleFor(p => p.Password)
			.NotEmpty().WithMessage("Password is required");
	}
}