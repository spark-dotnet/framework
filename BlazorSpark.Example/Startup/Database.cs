using BlazorSpark.Example.Data;
using BlazorSpark.Library.Database;
using BlazorSpark.Library.Environment;
using Microsoft.EntityFrameworkCore;

namespace BlazorSpark.Example.Startup
{
	public static class Database
	{
		public static IServiceCollection Setup(IServiceCollection services)
		{
			var dbType = Env.Get("DB_CONNECTION");
			var dbHost = Env.Get("DB_HOST");
			var dbPort = Env.Get("DB_PORT");
			var dbName = Env.Get("DB_DATABASE");
			var dbUser = Env.Get("DB_USERNAME");
			var dbPassword = Env.Get("DB_PASSWORD");
			var connectionString = $"server={dbHost};port={dbPort};database={dbName};user={dbUser};password={dbPassword};";
			if (dbType == DatabaseTypes.sqlite)
			{
				var folder = Directory.GetCurrentDirectory();
				var dbPath = System.IO.Path.Join(folder, dbName);
				services.AddDbContextFactory<ApplicationDbContext>(options =>
				{
					options.UseSqlite(
						$"Data Source={dbPath}"
					);
				});
			}
			else if (dbType == DatabaseTypes.mysql)
			{
				services.AddDbContextFactory<ApplicationDbContext>(options =>
				{
					options.UseMySql(
						connectionString,
						ServerVersion.AutoDetect(connectionString)
					);
				});
			}
			else if (dbType == DatabaseTypes.postgres)
			{
				services.AddDbContextFactory<ApplicationDbContext>(options =>
				{
					options.UseNpgsql(
						connectionString
					);
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
