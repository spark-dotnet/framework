using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSpark.Library.Database
{
    public static class DatabaseServiceRegistration
    {
        public static IServiceCollection AddDatabase<T>(this IServiceCollection services, IConfiguration config) where T : DbContext
        {
            var dbType = config.GetValue<string>("DB_CONNECTION");
            var dbHost = config.GetValue<string>("DB_HOST");
            var dbPort = config.GetValue<string>("DB_PORT");
            var dbName = config.GetValue<string>("DB_DATABASE");
            var dbUser = config.GetValue<string>("DB_USERNAME");
            var dbPassword = config.GetValue<string>("DB_PASSWORD");
            if (dbType == DatabaseTypes.sqlite)
            {
                var folder = Directory.GetCurrentDirectory();
                var dbPath = System.IO.Path.Join(folder, dbName);
                services.AddDbContextFactory<T>(options =>
                {
                    options.UseSqlite(
                        $"Data Source={dbPath}"
                    ).UseSnakeCaseNamingConvention();
                });
            }
            else if (dbType == DatabaseTypes.mysql)
            {
                var connectionString = $"server={dbHost};port={dbPort};database={dbName};user={dbUser};password={dbPassword};";
                services.AddDbContextFactory<T>(options =>
                {
                    options.UseMySql(
                        connectionString,
                        ServerVersion.AutoDetect(connectionString)
                    ).UseSnakeCaseNamingConvention();
                });
            }
            else if (dbType == DatabaseTypes.postgres)
            {
                var connectionString = $"Server={dbHost};Port={dbPort};Database={dbName};Username={dbUser};Password={dbPassword};";
                services.AddDbContextFactory<T>(options =>
                {
                    options.UseNpgsql(
                        connectionString
                    ).UseSnakeCaseNamingConvention();
                });
            }
            else if (dbType == DatabaseTypes.sqlserver)
            {
                var dbTrustCertificate = config.GetValue<bool>("DB_TRUST_CERTIFICATE");
                var dbIntegratedSecurity = config.GetValue<bool>("DB_INTEGRATED_SECURITY");
                string connectionString = string.Empty;

                if (dbIntegratedSecurity)
                {
                    connectionString = $"Server={dbHost};Database={dbName};Integrated Security={dbIntegratedSecurity}";

                    if (dbTrustCertificate)                    
                        connectionString = $"Server={dbHost};Database={dbName};Integrated Security={dbIntegratedSecurity};TrustServerCertificate={dbTrustCertificate}";                    
                }
                else if (dbTrustCertificate)
                {
                    connectionString = $"Server={dbHost};Database={dbName};User Id={dbUser};Password={dbPassword};TrustServerCertificate={dbTrustCertificate}";
                }
                else 
                {
                    connectionString = $"Server={dbHost};Database={dbName};User Id={dbUser};Password={dbPassword};";
                }

                services.AddDbContextFactory<T>(options =>
                {
                    options.UseSqlServer(
                        connectionString
                    ).UseSnakeCaseNamingConvention();
                });
            }
            else
            {
                throw new Exception("Invalid database driver. Check your .env file and make sure the DB_CONNECTION variable is set to mysql, sqlite, or postgres.");
            }

            return services;
        }
    }
}
