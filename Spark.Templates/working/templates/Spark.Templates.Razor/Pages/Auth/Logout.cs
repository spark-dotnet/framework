using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Spark.Library.Routing;

namespace Spark.Templates.Razor.Pages.Auth;

public class Logout : IRoute
{
	public void Map(WebApplication app)
	{
		app.MapPost("/logout", async (HttpContext HttpContext) =>
		{
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			return Results.Redirect("/");
		});
	}
}