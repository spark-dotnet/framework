using Spark.Templates.Razor.Application.Database;
using Spark.Templates.Razor.Application.Events.Listeners;
using Spark.Templates.Razor.Application.Services.Auth;
using Spark.Library.Database;
using Coravel;
using Spark.Library.Auth;
using Spark.Templates.Razor.Application.Jobs;
using Spark.Library.Mail;
using Vite.AspNetCore.Extensions;
using FluentValidation;
using Spark.Templates.Razor.Pages.Auth;
using Spark.Templates.Razor.Application.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Spark.Templates.Razor.Application.Startup;

public static class AppServicesRegistration
{
    public static IServiceCollection AddAppServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddViteServices();
        services.AddCustomServices();
        services.AddDatabase<DatabaseContext>(config);
        services.AddAuthorization(config, new string[] { CustomRoles.Admin, CustomRoles.User });
        services.AddAuthentication<IAuthValidator>(config);
        services.AddJobServices();
        services.AddScheduler();
        services.AddQueue();
        services.AddEventServices();
        services.AddEvents();
        services.AddMailer(config);
        services.AddRazorPages();
        services.AddDistributedMemoryCache();
        services.AddSession(options => {
            options.Cookie.Name = ".Spark.Templates.Razor";
            options.IdleTimeout = TimeSpan.FromMinutes(1);
        });
        return services;
    }

    private static IServiceCollection AddCustomServices(this IServiceCollection services)
    {
        // add custom services
        services.AddScoped<UsersService>();
        services.AddScoped<RolesService>();
        services.AddScoped<IAuthValidator, SparkAuthValidator>();
        services.AddScoped<AuthService>();
        services.AddScoped<IValidator<RegisterForm>, RegisterFormValidator>();
		services.AddScoped<IValidator<LoginForm>, LoginFormValidator>();
		return services;
    }

    private static IServiceCollection AddEventServices(this IServiceCollection services)
    {
        // add custom events here
        services.AddTransient<EmailNewUser>();
        return services;
    }

    private static IServiceCollection AddJobServices(this IServiceCollection services)
    {
        // add custom background tasks here
        services.AddTransient<ExampleJob>();
        return services;
    }
}
