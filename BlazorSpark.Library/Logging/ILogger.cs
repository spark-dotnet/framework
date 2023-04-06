using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSpark.Library.Logging
{
	public interface ILogger
	{
		void Fatal(string message);
		void Error(string message);
		void Warning(string message);
		void Information(string message);
		void Debug(string message);
	}
}
