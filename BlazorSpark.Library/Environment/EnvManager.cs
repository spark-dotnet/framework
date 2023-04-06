using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSpark.Library.Environment
{
    public static class EnvManager
    {
        public static void Setup()
        {
            DotNetEnv.Env.Load();
        }
    }
}
