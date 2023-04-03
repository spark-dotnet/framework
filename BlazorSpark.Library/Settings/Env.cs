using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSpark.Library.Settings
{
	public static class Env
	{
		public static void Load()
		{
			DotNetEnv.Env.Load();
		}

		public static string? Get(string name)
		{
			return System.Environment.GetEnvironmentVariable(name);
		}
	}
}
