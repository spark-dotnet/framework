using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Spark.Templates.Razor.Pages.Auth
{
	[AllowAnonymous]
	public class LogoutModel : PageModel
	{
		public async Task<IActionResult> OnGet()
		{
			if (HttpContext.User != null)
			{
				await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			}
			return RedirectToPage("/Index");
		}
	}
}
