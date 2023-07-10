namespace Spark.Library.Environment
{
    public static class EnvManager
    {
        public static void LoadConfig()
        {
            DotNetEnv.Env.Load();
        }
    }
}
