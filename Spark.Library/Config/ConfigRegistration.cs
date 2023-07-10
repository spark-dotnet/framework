using Spark.Library.Environment;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spark.Library.Config
{
    public static class ConfigRegistration
    {
        public static ConfigurationManager SetupSparkConfig(this ConfigurationManager manager)
        {
            // Load env file into config
            manager.AddEnvironmentVariables();

            // Get Spark section from appsettings
            var sparkConfig = manager.GetSection("Spark").AsEnumerable();

            if (!sparkConfig.Any())
            {
                throw new Exception("No \"Spark\" section detected in appsettings.json.");
            }

            // Update ENV_ prefixes with environment variables in appsettings
            foreach (var item in sparkConfig)
            {
                if (!String.IsNullOrEmpty(item.Value) && item.Value.Contains("ENV_"))
                {
                    var envVar = item.Value.Replace("ENV_", "");
                    manager[item.Key] = Env.Get(envVar);
                }
            }
            return manager;
        }
    }
}
