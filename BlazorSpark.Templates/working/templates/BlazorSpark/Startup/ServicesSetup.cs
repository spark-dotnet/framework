using BlazorSpark.Default.Services;
using Microsoft.AspNetCore.Components.Authorization;
using BlazorSpark.Default.Services.Auth;

namespace BlazorSpark.Default.Startup
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
