using Coravel.Events.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Spark.Templates.Razor.Application.Events;
using Spark.Templates.Razor.Application.Models;
using Spark.Templates.Razor.Application.Services.Auth;
using FluentValidation.Results;
using FluentValidation.AspNetCore;

namespace Spark.Templates.Razor.Pages.Auth;

public class Register : PageModel
{
	[BindProperty]
	public RegisterForm Form { get; set; }
	public string UserExistsMessage = "";

	private readonly IConfiguration _configuration;
	private readonly UsersService _usersService;
	private readonly AuthService _authService;
	private readonly IDispatcher _dispatcher;
	private readonly IValidator<RegisterForm> _validator;

	public Register(IConfiguration configuration, UsersService usersService, AuthService authService, IDispatcher dispatcher, IValidator<RegisterForm> validator)
	{
		_configuration = configuration;
		_usersService = usersService;
		_authService = authService;
		_dispatcher = dispatcher;
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

		var existingUser = await _usersService.FindUserByEmailAsync(Form.Email);
		if (existingUser != null)
		{
			UserExistsMessage = "Email already in use by another account.";
			return Page();
		}

		var userForm = new User()
		{
			Name = Form.Name,
			Email = Form.Email,
			Password = _usersService.GetSha256Hash(Form.Password),
			CreatedAt = DateTime.UtcNow
		};

		var newUser = await _usersService.CreateUserAsync(userForm);

		// Broadcast user created event. Sends welcome email
		var userCreated = new UserCreated(newUser);
		await _dispatcher.Broadcast(userCreated);

		var user = await _usersService.FindUserByCredsAsync(newUser.Email, newUser.Password);

		var cookieExpirationDays = _configuration.GetValue("Spark:Auth:CookieExpirationDays", 5);
		var cookieClaims = await _authService.CreateCookieClaims(user);

		await HttpContext.SignInAsync(
			CookieAuthenticationDefaults.AuthenticationScheme,
			cookieClaims,
			new AuthenticationProperties
			{
				IsPersistent = true,
				IssuedUtc = DateTimeOffset.UtcNow,
				ExpiresUtc = DateTimeOffset.UtcNow.AddDays(cookieExpirationDays)
			});

		return RedirectToPage("/dashboard");
	}
}

public class RegisterForm
{
	public string Name { get; set; } = String.Empty;
	public string Email { get; set; } = String.Empty;
	public string Password { get; set; } = String.Empty;
	public string ConfirmPassword { get; set; } = String.Empty;
}

public class RegisterFormValidator : AbstractValidator<RegisterForm>
{
	private readonly IConfiguration _config;

	public RegisterFormValidator(IConfiguration config)
	{
		_config = config;
		var minPasswordLength = _config.GetValue<int>("Spark:Auth:Password:MinimumLength", 8);
		var maxPasswordLength = _config.GetValue<int>("Spark:Auth:Password:MaximumLength", 32);
		var requireDigit = _config.GetValue<bool>("Spark:Auth:Password:RequireDigit", false);
		var requireUpper = _config.GetValue<bool>("Spark:Auth:Password:RequireUppercase", false);
		var requireNonAlphanumeric = _config.GetValue<bool>("Spark:Auth:Password:RequireNonAlphanumeric", false);

		RuleFor(p => p.Name)
			.NotEmpty().WithMessage("Name is required");

		RuleFor(p => p.Email)
			.NotEmpty().WithMessage("Email address is required")
			.EmailAddress().WithMessage("Please enter a valid email address");

		RuleFor(p => p.Password)
			.NotEmpty().WithMessage("Password is required")
			.MinimumLength(minPasswordLength).WithMessage($"Password length must be at least {minPasswordLength}.")
			.MaximumLength(maxPasswordLength).WithMessage($"Password length must not exceed {maxPasswordLength}.");

		if (requireDigit)
		{
			RuleFor(p => p.Password)
				.Matches(@"[0-9]+").WithMessage("Password must contain at least one number.");
		}
		if (requireUpper)
		{
			RuleFor(p => p.Password)
				.Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.");
		}
		if (requireNonAlphanumeric)
		{
			RuleFor(p => p.Password)
				.Matches(@"[^a-zA-Z0-9]").WithMessage("Password must contain at least one non-alphanumeric character.");
		}

		RuleFor(p => p.ConfirmPassword)
			.NotEmpty().WithMessage("Confirm password is required")
			.Equal(model => model.Password).WithMessage("The Password and Confirm Password do not match.");
	}
}
