using Serilog;
using BlazorSpark.Library.Logging;

namespace BlazorSpark.Default.Startup
{
    public static class Log
    {
        public static IServiceCollection Setup(IServiceCollection services)
        {
            services.AddScoped<BlazorSpark.Library.Logging.ILogger, Logger>();
            services.AddLogging(loggingBuilder =>
                loggingBuilder.AddSerilog(dispose: true));

            return services;
        }
    }
}
