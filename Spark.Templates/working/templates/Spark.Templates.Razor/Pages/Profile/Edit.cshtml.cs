using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Spark.Library.Environment;
using Spark.Library.Extensions;
using Spark.Templates.Razor.Application.Database;
using Spark.Templates.Razor.Application.Services.Auth;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace Spark.Templates.Razor.Pages.Profile;

[Authorize]
public class Edit : PageModel
{
	private readonly DatabaseContext _db;
	private readonly UsersService _usersService;
	private readonly AuthService _authService;
	public string PasswordErrorMessage = "";
	public string ProfileErrorMessage = "";

	[BindProperty]
	public PasswordForm PasswordForm { get; set; }

	[BindProperty]
	public ProfileForm ProfileForm { get; set; }

	public Edit(DatabaseContext db, UsersService usersService, AuthService authService)
	{
		_db = db;
		_usersService = usersService;
		_authService = authService;
	}

	public async Task OnGet()
	{
		await Setup();
	}

	public async Task<IActionResult> OnPostProfileAsync()
	{
		if (ModelState.GetFieldValidationState("ProfileForm") != ModelValidationState.Valid)
		{
			await Setup();
			return Page();
		}

		var currentUser = await _authService.GetAuthenticatedUser(HttpContext.User);

		// Verify another user isn't using the new email address
		var existingUser = await _usersService.FindUserByEmailAsync(ProfileForm.Email);
		if (existingUser != null && currentUser!.Id != existingUser.Id)
		{
			ProfileErrorMessage = "Email already in use.";
			await Setup();
			return Page();
		}
		// update user info
		currentUser!.Email = ProfileForm.Email;
		currentUser.Name = ProfileForm.Name;
		_db.Users.Save(currentUser);

		// re-login user so cookie info reflects updated values
		var cookieClaims = await _authService.CreateCookieClaims(currentUser);
		await HttpContext.SignInAsync(
			CookieAuthenticationDefaults.AuthenticationScheme,
			cookieClaims,
			new AuthenticationProperties
			{
				IsPersistent = true,
				IssuedUtc = DateTimeOffset.UtcNow,
				ExpiresUtc = DateTimeOffset.UtcNow.AddDays(5)
			}
		);
		HttpContext.SetFlash("success", "Profile info updated.");
		return RedirectToPage("/profile/edit");
	}

	public async Task<IActionResult> OnPostPasswordAsync()
	{
		if (ModelState.GetFieldValidationState("PasswordForm") != ModelValidationState.Valid)
		{
			await Setup();
			return Page();
		}

		var currentUser = await _authService.GetAuthenticatedUser(HttpContext.User);

		var existingUser = await _usersService.FindUserByCredsAsync(currentUser!.Email, _usersService.GetSha256Hash(PasswordForm.CurrentPassword));
		if (existingUser == null)
		{
			PasswordErrorMessage = "Current password was incorrect.";
			await Setup();
			return Page();
		}

		existingUser.Password = _usersService.GetSha256Hash(PasswordForm.NewPassword);
		_db.Users.Save(existingUser);
		HttpContext.SetFlash("success", "Password updated.");
		return RedirectToPage("/profile/edit");
	}


	private async Task Setup()
	{
		var currentUser = await _authService.GetAuthenticatedUser(HttpContext.User);
		ProfileForm = new()
		{
			Name = currentUser!.Name,
			Email = currentUser.Email
		};
	}
}

public class PasswordForm
{
	[Required(ErrorMessage = "Current Password is required")]
	public string CurrentPassword { get; set; } = default!;

	[Required(ErrorMessage = "New Password is required")]
	public string NewPassword { get; set; } = default!;

	[Required(ErrorMessage = "Confirm password is required")]
	[Compare("NewPassword", ErrorMessage = "The New Password and Confirm Password do not match.")]
	public string ConfirmPassword { get; set; } = default!;
}

public class ProfileForm
{
	[Required]
	public string Name { get; set; } = default!;

	[EmailAddress(ErrorMessage = "Invalid email address.")]
	[Required(ErrorMessage = "Please enter a valid email address")]
	public string Email { get; set; } = default!;
}