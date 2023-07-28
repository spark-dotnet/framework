using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Spark.Templates.Api.Application.Services.Auth;
using Spark.Templates.Api.Application.Events;
using Coravel.Events.Interfaces;
using Spark.Templates.Api.Application.Models;
using Spark.Templates.Api.Application.ViewModels;

namespace Spark.Templates.Api.Application.Controllers
{
    public class AuthController : Controller
    {        
        private readonly UsersService _usersService;
		private readonly AuthService _authService;
        private IDispatcher _dispatcher;

        public AuthController(UsersService usersService, 
            IDispatcher dispatcher,
			AuthService authService)
        {
            _usersService = usersService;
            _dispatcher = dispatcher;
			_authService = authService;
		}

        [HttpPost, AllowAnonymous]
        [Route("login")]
        public async Task<IActionResult> Login(Login request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (request == null)
            {
                return BadRequest("user is not set.");
            }

            var user = await _usersService.FindUserAsync(request.Email, _usersService.GetSha256Hash(request.Password));

            if (user == null)
            {
                ModelState.AddModelError("FailedLogin", "Login Failed: Your email or password was incorrect");
                return View();
            }
            
            var token = await _authService.CreateJwtToken(user);

			return Ok(new { user.Name, user.Email, token });
        }

        [HttpPost, AllowAnonymous]
        [Route("register")]
        public async Task<IActionResult> Register(Register request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (request == null)
            {
                return BadRequest("user is not set.");
            }

            var existingUser = await _usersService.FindUserByEmailAsync(request.Email);

            if (existingUser != null)
            {
                ModelState.AddModelError("EmailExists", "Email already in use by another account.");
                return BadRequest(ModelState);
            }

            var userForm = new User()
            {
                Name = request.Name,
                Email = request.Email,
                Password = _usersService.GetSha256Hash(request.Password),
                CreatedAt = DateTime.UtcNow
            };

            var newUser = await _usersService.CreateUserAsync(userForm);

            // broadcast user created event
            var userCreated = new UserCreated(newUser);
            await _dispatcher.Broadcast(userCreated);

            return Ok("User registered!");
		}
    }
}
