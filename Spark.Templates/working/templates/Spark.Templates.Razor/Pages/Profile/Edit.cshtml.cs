using Coravel.Events.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Spark.Library.Extensions;
using Spark.Templates.Razor.Application.Database;
using Spark.Templates.Razor.Application.Services.Auth;
using Spark.Templates.Razor.Pages.Auth;
using System.ComponentModel.DataAnnotations;

namespace Spark.Templates.Razor.Pages.Profile
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly UsersService _usersService;
        private readonly AuthService _authService;
        private readonly IConfiguration _configuration;
        private readonly DatabaseContext _db;
        private IDispatcher _dispatcher;

        [BindProperty]
        public ProfileInfoEditor ProfileInfo { set; get; } = new();

        [BindProperty]
        public ProfilePasswordEditor ProfilePassword { set; get; } = new();

        public EditModel(UsersService usersService, IConfiguration configuration, IDispatcher dispatcher, AuthService authService, DatabaseContext db)
        {
            _usersService = usersService;
            _configuration = configuration;
            _dispatcher = dispatcher;
            _authService = authService;
            _db = db;
        }

        public async Task OnGet()
        {
            var user = await _authService.GetAuthenticatedUser(User);
            ProfileInfo.Name = user.Name;
            ProfileInfo.Email = user.Email;
        }

        public async Task<IActionResult> OnPostUpdateInfo()
        {
            if (ModelState.GetFieldValidationState("ProfileInfo") != ModelValidationState.Valid)
            {
                return new PageResult();
            }

            var currentUser = await _authService.GetAuthenticatedUser(User);

            if (currentUser != null)
            {
                var existingUser = await _usersService.FindUserByEmailAsync(ProfileInfo.Email);

                if (existingUser != null && currentUser.Id != existingUser.Id)
                {
                    ModelState.AddModelError("ProfileInfo.Email", "Email already in use.");
                    return new PageResult();
                }

                currentUser.Email = ProfileInfo.Email;
                currentUser.Name = ProfileInfo.Name;

                _db.Users.Save(currentUser);

                await _authService.RefreshSignIn(currentUser);
            }
            return Redirect("/profile/edit");
        }

        public async Task<IActionResult> OnPostUpdatePassword()
        {
            if (ModelState.GetFieldValidationState("ProfilePassword") != ModelValidationState.Valid)
            {
                return new PageResult();
            }
            var user = await _authService.GetAuthenticatedUser(User);

            var existingUser = await _usersService.FindUserAsync(user.Email, _usersService.GetSha256Hash(ProfilePassword.CurrentPassword));

            if (existingUser == null)
            {
                ModelState.AddModelError("ProfilePassword.CurrentPassword", "Current password was incorrect.");
                return new PageResult();
            }

            existingUser.Password = _usersService.GetSha256Hash(ProfilePassword.NewPassword);

            _db.Users.Save(existingUser);
            return Redirect("/profile/edit");
        }
    }

    public class ProfileEditor
    {
        public ProfileInfoEditor ProfileInfoEditor { get; set; } = new();
        public ProfilePasswordEditor ProfilePasswordEditor { get; set; } = new();
    }

    public class ProfileInfoEditor
    {
        [Required]
        public string Name { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [Required(ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; }
    }

    public class ProfilePasswordEditor
    {
        [Required(ErrorMessage = "Current Password is required")]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "New Password is required")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Confirm password is required")]
        [Compare("NewPassword", ErrorMessage = "The New Password and Confirm Password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
