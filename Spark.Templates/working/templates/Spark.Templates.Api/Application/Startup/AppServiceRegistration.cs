using Coravel;
using Spark.Library.Database;
using Spark.Library.Logging;
using Spark.Library.Mail;
using Spark.Templates.Api.Application.Database;
using Spark.Templates.Api.Application.Events.Listeners;
using Spark.Templates.Api.Application.Services.Auth;
using Spark.Templates.Api.Application.Tasks;

namespace Spark.Templates.Api.Application.Startup
{
	public static class AppServiceRegistration
    {
        public static IServiceCollection AddAppServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddJwt(config);
            services.AddSwagger();
			services.AddControllers();
			services.AddEndpointsApiExplorer();
			services.AddSwaggerGen();
			services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
			services.AddCustomServices();
            services.AddDatabase<DatabaseContext>(config);
            services.AddLogger(config);
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
            services.AddTransient<ExampleTask>();
            return services;
        }
    }
}
