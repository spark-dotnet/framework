using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSpark.Library.Logging
{
	public class Logger : ILogger
	{
		public void Fatal(string message)
		{
			Serilog.Log.Fatal(message);
		}

		public void Error(string message)
		{
			Serilog.Log.Error(message);
		}

		public void Warning(string message)
		{
			Serilog.Log.Warning(message);
		}

		public void Information(string message)
		{
			Serilog.Log.Information(message);
		}

		public void Debug(string message)
		{
			Serilog.Log.Debug(message);
		}
	}
}
