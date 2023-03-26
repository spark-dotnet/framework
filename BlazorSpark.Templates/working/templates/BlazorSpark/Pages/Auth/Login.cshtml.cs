using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Globalization;
using System.Security.Claims;
using System.ComponentModel.DataAnnotations;
using BlazorSpark.Lib.Auth;
using BlazorSpark.Data;

namespace BlazorSpark.Pages.Auth
{
    public class LoginModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private readonly RolesService _rolesService;
        private readonly UsersService _usersService;

        public LoginModel(
            UsersService usersService,
            RolesService rolesService,
            IConfiguration configuration)
        {
            _usersService = usersService ?? throw new ArgumentNullException(nameof(usersService));
            _rolesService = rolesService ?? throw new ArgumentNullException(nameof(rolesService));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        [BindProperty]
        public LoginForm loginUser { get; set; }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPost()
        {

            if (!ModelState.IsValid)
                return Page();

            if (loginUser == null)
            {
                return BadRequest("user is not set.");
            }

            var user = await _usersService.FindUserAsync(loginUser.Email, _usersService.GetSha256Hash(loginUser.Password));

            var loginCookieExpirationDays = _configuration.GetValue("LoginCookieExpirationDays", 30);
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

            return Redirect("~/dashboard");
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

        public class LoginForm
        {
            [Display(Name = "Email")]
            [Required(ErrorMessage = "Please enter a valid email address")]
            public string Email { get; set; }

            [Display(Name = "Password")]
            [Required(ErrorMessage = "Invalid password")]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }
    }
}
