using BlazorSpark.Library.Settings;
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
                case "file":
                    logConfig.WriteTo.File("Storage/Logging/spark.log", rollingInterval: RollingInterval.Day);
                    break;
                case "console":
					logConfig.WriteTo.Console();
                    break;
                default:
                    logConfig.WriteTo.File("Storage/Logging/spark.log", rollingInterval: RollingInterval.Day);
                    break;
            }

			switch (logLevel) 
			{
				case "verbose":
					logConfig.MinimumLevel.Verbose();
					break;
				case "debug":
					logConfig.MinimumLevel.Debug();
					break;
				case "information":
					logConfig.MinimumLevel.Information();
					break;
				case "warning":
					logConfig.MinimumLevel.Warning();
					break;
				case "error":
					logConfig.MinimumLevel.Error();
					break;
				case "fatal":
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
