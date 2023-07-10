using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spark.Library.Mail.Helpers
{
	public static class StringHelpers
	{
		public static string CommaSeparated(this IEnumerable<string> str)
		{
			if (str == null)
			{
				return string.Empty;
			}

			return string.Join(",", str);
		}
	}
}
