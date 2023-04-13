using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using BlazorSpark.Example.Services;
using BlazorSpark.Example.Services.Auth;
using BlazorSpark.Library.Logging;
using Serilog;
using BlazorSpark.Library.Scheduling;

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
			services = Log.Setup(services);
            services = Database.Setup(services);
			services = Auth.Setup(services);
			services = ScheduleManager.Setup(services);
            return services;
		}
	}
}
