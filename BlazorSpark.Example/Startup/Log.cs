using BlazorSpark.Library.Logging;
using Serilog;

namespace BlazorSpark.Example.Startup
{
    public static class Log
    {
        public static IServiceCollection Setup(IServiceCollection services)
        {
            services.AddScoped<ISparkLogger, SparkLogger>();
            services.AddLogging(loggingBuilder =>
                loggingBuilder.AddSerilog(dispose: true));

            return services;
        }
    }
}
