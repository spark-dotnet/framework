using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Spark.Library.Auth;
using Spark.Library.Environment;
using Spark.Library.Extensions;
using Spark.Templates.Mvc.Application.Database;
using Spark.Templates.Mvc.Application.Models;
using Spark.Templates.Mvc.Application.Services.Auth;
using Spark.Templates.Mvc.Application.ViewModels;
using System.Security.Claims;

namespace Spark.Templates.Mvc.Application.Controllers
{
	[Route("profile")]
	public class ProfileController : Controller
	{
		private readonly UsersService _usersService;
        private readonly DatabaseContext _db;
		private readonly AuthService _authService;

		public ProfileController(UsersService usersService, DatabaseContext db, AuthService authService) 
		{
			_usersService = usersService;
			_db = db;
			_authService = authService;
        }

        [Authorize]
        [Route("edit")]
        public async Task<IActionResult> Edit()
        {
            var user = await _authService.GetAuthenticatedUser(User);
            var model = new ProfileEditor();
            model.ProfileInfoEditor.Name = user.Name;
            model.ProfileInfoEditor.Email = user.Email;
            return View(model);
        }

        [Authorize]
        [HttpPost]
        [Route("edit")]
        public async Task<IActionResult> Update(ProfileInfoEditor request)
        {
            if (!ModelState.IsValid)
            {
                var model = new ProfileEditor();
                model.ProfileInfoEditor = request;
                return View("Edit", model);
            }

            var currentUser = await _authService.GetAuthenticatedUser(User);

            if (currentUser != null)
            {
                var existingUser = await _usersService.FindUserByEmailAsync(request.Email);

                if (existingUser != null && currentUser.Id != existingUser.Id)
                {
                    ModelState.AddModelError("Email", "Email already in use.");
                    var model = new ProfileEditor();
                    model.ProfileInfoEditor = request;
                    return View("Edit", model);
                }

                currentUser.Email = request.Email;
                currentUser.Name = request.Name;

                _db.Users.Save(currentUser);

                await _authService.RefreshSignIn(currentUser);
            }
            return RedirectToAction("Edit");
        }

        [Authorize]
        [HttpPost]
        [Route("edit/password")]
        public async Task<IActionResult> UpdatePassword(ProfilePasswordEditor request)
        {
            var user = await _authService.GetAuthenticatedUser(User);

            if (!ModelState.IsValid)
            {
                var model = new ProfileEditor();
                model.ProfileInfoEditor.Name = user.Name;
                model.ProfileInfoEditor.Email = user.Email;
                return View("Edit", model);
            }

            var existingUser = await _usersService.FindUserAsync(user.Email, _usersService.GetSha256Hash(request.CurrentPassword));

            if (existingUser == null)
            {
                var model = new ProfileEditor();
                model.ProfileInfoEditor.Name = user.Name;
                model.ProfileInfoEditor.Email = user.Email;
                ModelState.AddModelError("CurrentPassword", "Current password was incorrect.");
                return View("Edit", model);
            }

            existingUser.Password = _usersService.GetSha256Hash(request.NewPassword);

            _db.Users.Save(existingUser);
            return RedirectToAction("Edit", "Profile");
        }
    }
}
