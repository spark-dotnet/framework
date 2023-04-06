using BlazorSpark.Library.Environment;

namespace BlazorSpark.Default.Helpers
{
	public static class ConnectionHelper
	{
		public static string GetDatabaseType()
		{
			string? databaseConnection = Env.Get("DB_CONNECTION");
			if (String.IsNullOrEmpty(databaseConnection))
			{
				throw new Exception("Database driver not found. Check your .env file and make sure the DB_CONNECTION variable is set to mysql, sqlite, or postgres.");
			}
			return databaseConnection;
		}

		public static string GetDatabaseName()
		{
			string? databaseName = Env.Get("DB_DATABASE");
			if (String.IsNullOrEmpty(databaseName))
			{
				throw new Exception("Database name not found. Check your .env file and make sure the DB_DATABASE is set to your database name.");
			}
			return databaseName;
		}

		public static string GetConnectionString()
		{
			var dbHost = Env.Get("DB_HOST");
			var dbPort = Env.Get("DB_PORT");
			var dbName = Env.Get("DB_DATABASE");
			var dbUser = Env.Get("DB_USERNAME");
			var dbPassword = Env.Get("DB_PASSWORD");
			var connectionString = $"server={dbHost};port={dbPort};database={dbName};user={dbUser};password={dbPassword};";
			return connectionString;

        }
	}
}
