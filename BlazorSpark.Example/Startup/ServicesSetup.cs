using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using BlazorSpark.Example.Services;
using BlazorSpark.Example.Services.Auth;
using BlazorSpark.Library.Database;
using BlazorSpark.Example.Helpers;
using BlazorSpark.Example.Data;

namespace BlazorSpark.Example.Startup
{
	public static class ServicesSetup
	{
		public static IServiceCollection RegisterServices(this IServiceCollection services)
		{
			services.AddRazorPages();
			services.AddServerSideBlazor();
			services.AddScoped<UsersService>();
			services.AddScoped<UserInfoService>();
			services.AddScoped<RolesService>();
			services.AddScoped<ITestService, TestService>();
			services.AddScoped<ICookieService, CookieService>();
			services.AddScoped<AuthenticationStateProvider, SparkAuthenticationStateProvider>();
			services = Database.Setup(services);
			services = Auth.Setup(services);
			return services;
		}
	}
}
