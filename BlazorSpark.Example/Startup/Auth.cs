using BlazorSpark.Example.Data;
using BlazorSpark.Example.Services.Auth;
using BlazorSpark.Library.Database;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

namespace BlazorSpark.Example.Startup
{
	public static class Auth
	{
		public static IServiceCollection Setup(IServiceCollection services)
		{
			services.AddAuthorization(options =>
			{
				options.AddPolicy(CustomRoles.Admin, policy => policy.RequireRole(CustomRoles.Admin));
				options.AddPolicy(CustomRoles.User, policy => policy.RequireRole(CustomRoles.User));
			});
			services
				.AddAuthentication(options =>
				{
					options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
					options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
					options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
				})
				.AddCookie(options =>
				{
					options.SlidingExpiration = false;
					options.LoginPath = "/login";
					options.LogoutPath = "/logout";
					//options.AccessDeniedPath = new PathString("/Home/Forbidden/");
					options.Cookie.Name = ".blazor.spark.cookie";
					options.Cookie.HttpOnly = true;
					options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
					options.Cookie.SameSite = SameSiteMode.Lax;
					options.Events = new CookieAuthenticationEvents
					{
						OnValidatePrincipal = context =>
						{
							var cookieValidatorService = context.HttpContext.RequestServices.GetRequiredService<ICookieService>();
							return cookieValidatorService.ValidateAsync(context);
						}
					};
				});
			return services;
		}
	}
}
