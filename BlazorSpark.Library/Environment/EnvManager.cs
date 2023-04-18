using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSpark.Library.Environment
{
    public static class EnvManager
    {
        public static void LoadConfig()
        {
            var root = Directory.GetCurrentDirectory();
            var filePath = Path.Combine(root, ".env");
            if (!File.Exists(filePath))
                return;

            foreach (var line in File.ReadAllLines(filePath))
            {
                var parts = line.Split(
                    '=',
                    StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length != 2)
                    continue;

                System.Environment.SetEnvironmentVariable(parts[0], parts[1]);
            }
        }
    }
}
