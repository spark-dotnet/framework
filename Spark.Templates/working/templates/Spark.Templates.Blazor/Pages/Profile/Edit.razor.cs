using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Spark.Library.Extensions;
using Spark.Templates.Blazor.Application.Database;
using Spark.Templates.Blazor.Application.Models;
using Spark.Templates.Blazor.Application.Services.Auth;
using Spark.Templates.Blazor.Pages.Shared;
using System.ComponentModel.DataAnnotations;

namespace Spark.Templates.Blazor.Pages.Profile;

public partial class Edit
{
    [CascadingParameter]
    public MainLayout? Layout { get; set; }
    private User? user => Layout.User;
    private UpdateProfileModel profileInformationForm { get; set; } = new();
    private string profileFormMessage = "";
    private UpdatePasswordModel passwordForm { get; set; } = new();
    private string passwordFormMessage = "";

    [Inject]
    IDbContextFactory<DatabaseContext> Factory { get; set; } = default!;
    [Inject]
    NavigationManager NavManager { get; set; } = default!;
    [Inject]
    UsersService UsersService { get; set; } = default!;


    protected override void OnInitialized()
    {
        // get user profile
        profileInformationForm.Name = user.Name;
        profileInformationForm.Email = user.Email;
    }

    private async Task SaveProfileInformation()
    {
        using var db = Factory.CreateDbContext();
        var currentUser = db.Users.Find(user.Id);

        if (currentUser != null)
        {
            var existingUser = await UsersService.FindUserByEmailAsync(profileInformationForm.Email);

            if (existingUser != null && user.Id != existingUser.Id)
            {
                profileFormMessage = "Email already in use.";
                return;
            }

            currentUser.Email = profileInformationForm.Email;
            currentUser.Name = profileInformationForm.Name;

            db.Users.Save(currentUser);
            StateHasChanged();
            NavManager.NavigateTo("profile/edit", true);
        }
    }

    private async Task UpdatePassword()
    {
        using var db = Factory.CreateDbContext();
        var existingUser = await UsersService.FindUserAsync(user.Email, UsersService.GetSha256Hash(passwordForm.CurrentPassword));

        if (existingUser == null)
        {
            passwordFormMessage = "Current password was incorrect.";
            return;
        }

        existingUser.Password = UsersService.GetSha256Hash(passwordForm.NewPassword);

        db.Users.Save(existingUser);
        StateHasChanged();
        NavManager.NavigateTo("profile/edit", true);
    }

    public class UpdateProfileModel
    {
        [Required]
        public string Name { get; set; } = default!;

        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [Required(ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; } = default!;
    }

    public class UpdatePasswordModel
    {
        [Required(ErrorMessage = "Current Password is required")]
        public string CurrentPassword { get; set; } = default!;

        [Required(ErrorMessage = "New Password is required")]
        public string NewPassword { get; set; } = default!;

        [Required(ErrorMessage = "Confirm password is required")]
        [Compare("NewPassword", ErrorMessage = "The New Password and Confirm Password do not match.")]
        public string ConfirmPassword { get; set; } = default!;
    }
}
