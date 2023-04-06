using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSpark.Library.Environment
{
    public static class Env
    {
        public static string? Get(string name)
        {
            return System.Environment.GetEnvironmentVariable(name);
        }
    }
}
