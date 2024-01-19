using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spark.Console.Shared;

public class UserApp
{
    public static string GetAppName() =>
        Directory.GetCurrentDirectory().Replace("/", "\\").Split('\\').Last();

    public static string GetProjectName()
    {
        string[] csprojFiles = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.csproj");
        if (csprojFiles.Length > 0)
        {
            return Path.GetFileName(csprojFiles[0]).Replace(".csproj", "");
        }
        else
        {
            return null;
        }
    }

    public static string FindSolutionName()
    {
        var currentDir = Directory.GetCurrentDirectory();
        return "";
    }
}
