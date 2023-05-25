using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSpark.Console.Shared
{
    public class UserApp
    {
        public static string GetAppName() =>
            Directory.GetCurrentDirectory().Replace("/", "\\").Split('\\').Last();

        public static string FindSolutionName()
        {
            var currentDir = Directory.GetCurrentDirectory();
            return "";
        }
    }
}
