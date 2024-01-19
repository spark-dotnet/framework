namespace Spark.Library.Environment;

public static class Env
{
    public static string? Get(string name)
    {
        return System.Environment.GetEnvironmentVariable(name);
    }
}
