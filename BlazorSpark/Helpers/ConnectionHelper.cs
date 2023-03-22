namespace BlazorSpark.Helpers
{
	public static class ConnectionHelper
	{
		public static string GetConnectionString()
		{
			var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");
			return connectionString;

        }
	}
}
