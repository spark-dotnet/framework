using BlazorSpark.Default.Data;
using BlazorSpark.Default.Helpers;
using BlazorSpark.Library.Database;
using BlazorSpark.Library.Settings;
using Microsoft.EntityFrameworkCore;

namespace BlazorSpark.Default.Startup
{
	public static class Database
	{
		public static IServiceCollection Setup(IServiceCollection services)
		{
			var dbType = ConnectionHelper.GetDatabaseType();
			var dbName = ConnectionHelper.GetDatabaseName();
			var connectionString = ConnectionHelper.GetConnectionString();

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
