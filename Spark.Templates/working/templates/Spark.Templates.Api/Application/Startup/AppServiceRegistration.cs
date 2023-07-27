using Spark.Templates.Api.Application.Database;
using Spark.Templates.Api.Application.Events.Listeners;
using Spark.Templates.Api.Application.Models;
using Spark.Templates.Api.Application.Services.Auth;
using Spark.Templates.Api.Application.Services;
using Spark.Library.Database;
using Spark.Library.Logging;
using Coravel;
using Microsoft.AspNetCore.Components.Authorization;
using Spark.Library.Auth;
using Spark.Templates.Api.Application.Tasks;
using Spark.Library.Mail;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using Spark.Library.Environment;
using Microsoft.AspNetCore.Authorization;

namespace Spark.Templates.Api.Application.Startup
{
    public static class AppServiceRegistration
    {
        public static IServiceCollection AddAppServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddAuthentication(options => 
			{	
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
			})
            .AddJwtBearer(options =>
            {	
				options.TokenValidationParameters = new TokenValidationParameters()
				{
					ClockSkew = TimeSpan.Zero,
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidateLifetime = true,
					ValidateIssuerSigningKey = true,
					ValidIssuer = config.GetValue("Spark:Jwt:Issuer", "https://spark-framework.net"),
					ValidAudience = config.GetValue("Spark:Jwt:Audience", "https://spark-framework.net"),
					IssuerSigningKey = new SymmetricSecurityKey(
				    Encoding.UTF8.GetBytes(config.GetValue("Spark:Jwt:Key", "SomthingSecret!"))
			        ),
				};
			});
			services.AddAuthorization(options =>
			{
				options.DefaultPolicy = new AuthorizationPolicyBuilder()
				   .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
				   .RequireAuthenticatedUser()
				   .Build();
			});
			services.AddSwaggerGen(option =>
			{
				option.SwaggerDoc("v1", new OpenApiInfo { Title = Env.Get("APP_NAME"), Version = "v1" });
				option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
				{
					In = ParameterLocation.Header,
					Description = "Please enter a valid token",
					Name = "Authorization",
					Type = SecuritySchemeType.Http,
					BearerFormat = "JWT",
					Scheme = "Bearer"
				});
				option.AddSecurityRequirement(new OpenApiSecurityRequirement
				{
					{
						new OpenApiSecurityScheme
						{
							Reference = new OpenApiReference
							{
								Type=ReferenceType.SecurityScheme,
								Id="Bearer"
							}
						},
						new string[]{}
					}
				});
			});
			services.AddControllers();
			services.AddEndpointsApiExplorer();
			services.AddSwaggerGen();
			services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
			services.AddCustomServices();
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddDatabase<DatabaseContext>(config);
            services.AddLogger(config);
            services.AddAuthorization(config, new string[] { CustomRoles.Admin, CustomRoles.User });
            services.AddAuthentication<IAuthValidator>(config);
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
            services.AddScoped<TokenService>();
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
