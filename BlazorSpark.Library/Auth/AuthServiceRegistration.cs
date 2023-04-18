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

namespace BlazorSpark.Library.Auth
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

        public static IServiceCollection AddAuthentication<T>(this IServiceCollection services, IConfiguration config) where T : ICookieService
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
                            var cookieValidatorService = context.HttpContext.RequestServices.GetRequiredService<T>();
                            return cookieValidatorService.ValidateAsync(context);
                        }
                    };
                });
            return services;
        }
    }
}
