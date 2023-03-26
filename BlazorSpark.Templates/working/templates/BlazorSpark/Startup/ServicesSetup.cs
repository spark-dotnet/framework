using BlazorSpark.Data;
using BlazorSpark.Helpers;
using BlazorSpark.Lib.Auth;
using BlazorSpark.Services;
using BlazorSpark.Lib.Database;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using System.IO;
using BlazorSpark.Shared;

namespace BlazorSpark.Startup
{
    public static class ServicesSetup
	{
		public static IServiceCollection RegisterServices(this IServiceCollection services)
		{
			var connectionString = ConnectionHelper.GetConnectionString();
			var databaseType = Environment.GetEnvironmentVariable("DB_CONNECTION");

			services.AddRazorPages();
			services.AddServerSideBlazor();
			services.AddScoped<UsersService>();
			services.AddScoped<UserInfoService>();
			services.AddScoped<RolesService>();
			services.AddScoped<ITestService, TestService>();
			services.AddScoped<ICookieService, CookieService>();
			services.AddScoped<AuthenticationStateProvider, SparkAuthenticationStateProvider>();
			if (databaseType== DatabaseTypes.sqlite)
			{
				var folder = Environment.SpecialFolder.LocalApplicationData;
				var databaseName = Environment.GetEnvironmentVariable("DB_DATABASE");
				var path = Environment.GetFolderPath(folder);
				var dbPath = System.IO.Path.Join(path, databaseName);
				services.AddDbContextFactory<ApplicationDbContext>(options =>
				{
					options.UseSqlite(
						$"Data Source={dbPath}"
					);
				});
			}
			else if (databaseType == DatabaseTypes.mysql)
			{
				throw new NotImplementedException("MySQL is not yet implemented");
				//services.AddDbContextFactory<ApplicationDbContext>(options =>
				//{
				//	options.UseMySql(
				//		connectionString,
				//		ServerVersion.AutoDetect(connectionString),
				//		serverDbContextOptionsBuilder =>
				//		{
				//			var minutes = (int)TimeSpan.FromMinutes(3).TotalSeconds;
				//			serverDbContextOptionsBuilder.CommandTimeout(minutes);
				//			serverDbContextOptionsBuilder.EnableRetryOnFailure();
				//		});
				//});
			}
			else if (databaseType == DatabaseTypes.mssql)
			{
				throw new NotImplementedException("MSSQL is not yet implemented");
			}
			else
			{
				throw new Exception("Invalid database driver. Check your .env file and make sure the DB_CONNECTION variable is set to mysql or mssql.");
			}
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
