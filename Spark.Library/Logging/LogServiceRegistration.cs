using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spark.Library.Logging
{
    public static class LogServiceRegistration
    {
        public static IServiceCollection AddLogger(this IServiceCollection services, IConfiguration config)
        {
            SetupLogger(config);
            services.AddScoped<ILogger, Logger>();
            services.AddLogging(loggingBuilder =>
                loggingBuilder.AddSerilog(dispose: true));
            return services;
        }

        private static void SetupLogger(IConfiguration config)
        {
            string logChannel = config.GetValue<string>("Spark:Log:Default")!;
            string logLevel = config.GetValue<string>("Spark:Log:Level")!;

            var logConfig = new LoggerConfiguration();

            switch (logChannel)
            {
                case LogChannels.file:
                    logConfig.WriteTo.File(
                        config.GetValue<string>("Spark:Log:Channels:File:Path","Storage/Logging/Spark.log")!, 
                        rollingInterval: RollingInterval.Day
                    );
                    break;
                case LogChannels.console:
                    logConfig.WriteTo.Console();
                    break;
                default:
                    logConfig.WriteTo.File(
                        config.GetValue<string>("Spark:Log:Channels:File:Path", "Storage/Logging/Spark.log")!,
                        rollingInterval: RollingInterval.Day
                    );
                    break;
            }

            switch (logLevel)
            {
                case LogLevels.debug:
                    logConfig.MinimumLevel.Debug();
                    break;
                case LogLevels.information:
                    logConfig.MinimumLevel.Information();
                    break;
                case LogLevels.warning:
                    logConfig.MinimumLevel.Warning();
                    break;
                case LogLevels.error:
                    logConfig.MinimumLevel.Error();
                    break;
                case LogLevels.fatal:
                    logConfig.MinimumLevel.Fatal();
                    break;
                default:
                    logConfig.MinimumLevel.Error();
                    break;
            }

            Serilog.Log.Logger = logConfig.CreateLogger();
        }
    }
}
