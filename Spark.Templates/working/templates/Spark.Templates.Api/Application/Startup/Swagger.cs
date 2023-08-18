using Microsoft.OpenApi.Models;

namespace Spark.Templates.Api.Application.Startup
{
	public static class Swagger
	{ 
		public static IServiceCollection AddSwagger(this IServiceCollection services)
		{	
			services.AddSwaggerGen(option =>
			{
				option.SwaggerDoc("v1", new OpenApiInfo { Title = "Spark.Net (API)", Version = "v1" });
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

			return services;
		}
	}
}
