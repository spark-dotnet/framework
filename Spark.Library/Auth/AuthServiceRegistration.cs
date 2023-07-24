using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spark.Library.Auth
{
    public static class AuthServiceRegistration
    {
        public static IServiceCollection AddAuthorization(this IServiceCollection services, IConfiguration config, string[] roles)
        {
            services.AddAuthorization(options =>
            {
                foreach (var role in roles)
                {
                    options.AddPolicy(role, policy => policy.RequireRole(role));
                }
            });
            return services;
        }

        public static IServiceCollection AddAuthentication<T>(this IServiceCollection services, IConfiguration config) where T : IAuthValidator
        {
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
                    options.LoginPath = config.GetValue<string>("Spark:Auth:LoginPath", "/login");
                    options.LogoutPath = config.GetValue<string>("Spark:Auth:LogoutPath", "/logout");
                    options.AccessDeniedPath = config.GetValue<string>("Spark:Auth:AccessDeniedPath", "/access-denied");
                    options.Cookie.Name = ".blazor.spark.cookie";
                    options.Cookie.HttpOnly = true;
                    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                    options.Cookie.SameSite = SameSiteMode.Lax;
                    options.Events = new CookieAuthenticationEvents
                    {
                        OnValidatePrincipal = context =>
                        {
                            var cookieValidatorService = context.HttpContext.RequestServices.GetRequiredService<T>();
                            return cookieValidatorService.ValidateAsync(context);
                        }
                    };
                });
            return services;
        }
    }
}
