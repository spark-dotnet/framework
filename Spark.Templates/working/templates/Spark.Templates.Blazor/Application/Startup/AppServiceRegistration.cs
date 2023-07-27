using Spark.Templates.Blazor.Application.Database;
using Spark.Templates.Blazor.Application.Events.Listeners;
using Spark.Templates.Blazor.Application.Models;
using Spark.Templates.Blazor.Application.Services.Auth;
using Spark.Templates.Blazor.Application.Services;
using Spark.Library.Database;
using Spark.Library.Logging;
using Coravel;
using Microsoft.AspNetCore.Components.Authorization;
using Spark.Library.Auth;
using Spark.Templates.Blazor.Application.Jobs;
using Spark.Library.Mail;

namespace Spark.Templates.Blazor.Application.Startup
{
    public static class AppServiceRegistration
    {
        public static IServiceCollection AddAppServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddCustomServices();
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddDatabase<DatabaseContext>(config);
            services.AddLogger(config);
            services.AddAuthorization(config, new string[] { CustomRoles.Admin, CustomRoles.User });
            services.AddAuthentication<IAuthValidator>(config);
            services.AddScoped<AuthenticationStateProvider, SparkAuthenticationStateProvider>();
            services.AddTaskServices();
            services.AddScheduler();
            services.AddQueue();
            services.AddEventServices();
            services.AddEvents();
            services.AddMailer(config);
            return services;
        }

        private static IServiceCollection AddCustomServices(this IServiceCollection services)
        {
            // add custom services
            services.AddScoped<UsersService>();
            services.AddScoped<RolesService>();
			services.AddScoped<IAuthValidator, AuthValidator>();
			services.AddScoped<AuthService>();
			return services;
        }

        private static IServiceCollection AddEventServices(this IServiceCollection services)
        {
            // add custom events here
            services.AddTransient<EmailNewUser>();
            return services;
        }

        private static IServiceCollection AddTaskServices(this IServiceCollection services)
        {
            // add custom background tasks here
            services.AddTransient<ExampleJob>();
            return services;
        }
    }
}
