using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Spark.Library.Extensions;
using Spark.Templates.Api.Application.Database;
using Spark.Templates.Api.Application.Services.Auth;
using Spark.Templates.Api.Application.ViewModels;

namespace Spark.Templates.Api.Application.Controllers
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

        [HttpGet, Authorize]
        [Route("")]
        public async Task<IActionResult> ProfileInfo()
        {
            var user = await _authService.GetAuthenticatedUser(User);

            if (user == null) 
            {
                return NotFound();
            }

            return Ok(new User { Name = user.Name, Email = user.Email, EmailVerifiedAt = user.EmailVerifiedAt });
		}

        [HttpPost, Authorize]
        [Route("edit")]
        public async Task<IActionResult> Update(ProfileInfoEditor request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var currentUser = await _authService.GetAuthenticatedUser(User);

            if (currentUser != null)
            {
                var existingUser = await _usersService.FindUserByEmailAsync(request.Email);

                if (existingUser != null && currentUser.Id != existingUser.Id)
                {
                    ModelState.AddModelError("Email", "Email already in use.");
                    return BadRequest(ModelState);
                }

                currentUser.Email = request.Email;
                currentUser.Name = request.Name;

                _db.Users.Save(currentUser);

                //await _authService.RefreshSignIn(currentUser);
            }

            return Ok("Profile updated!");
        }

		[HttpPost, Authorize]
        [Route("edit/password")]
        public async Task<IActionResult> UpdatePassword(ProfilePasswordEditor request)
        {
            var user = await _authService.GetAuthenticatedUser(User);

            if (!ModelState.IsValid)
            {   
                return BadRequest(ModelState);
            }

            var existingUser = await _usersService.FindUserAsync(user.Email, _usersService.GetSha256Hash(request.CurrentPassword));

            if (existingUser == null)
            {   
                ModelState.AddModelError("CurrentPassword", "Current password was incorrect.");
                return BadRequest(ModelState);
            }

            existingUser.Password = _usersService.GetSha256Hash(request.NewPassword);

            _db.Users.Save(existingUser);

            return Ok("Password was updated");
        }
    }
}
