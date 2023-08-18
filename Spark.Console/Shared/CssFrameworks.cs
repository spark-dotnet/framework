using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spark.Console.Shared;

public class CssFrameworks
{
    public const string tailwind = nameof(tailwind);
    public const string bootstrap = nameof(bootstrap);
    public const string pico = nameof(pico);

    public static bool IsValid(string framework)
    {
        if (framework == tailwind) return true;
        if (framework == bootstrap) return true;
        if (framework == pico) return true;
        return false;
    }
}
