using BlazorSpark.Library.Database;
using BlazorSpark.Library.Environment;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSpark.Library.Logging
{
	public static class LogManager
	{
		public static void Setup()
		{
			string? logLevel = Env.Get("LOG_LEVEL");
            string? logChannel = Env.Get("LOG_CHANNEL");

			var logConfig = new LoggerConfiguration();
			
			switch (logChannel)
            {
                case LogChannels.file:
                    logConfig.WriteTo.File("Storage/Logging/spark.log", rollingInterval: RollingInterval.Day);
                    break;
                case LogChannels.console:
					logConfig.WriteTo.Console();
                    break;
                default:
                    logConfig.WriteTo.File("Storage/Logging/spark.log", rollingInterval: RollingInterval.Day);
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

		public static ILoggingBuilder AddLogger(ILoggingBuilder loggingBuilder)
		{
			return loggingBuilder.AddSerilog(dispose: true);

        }
	}
}
