using Spark.Console.Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spark.Console.Commands;

public class DevCommand
{
    public void Execute()
    {
        ConsoleOutput.StartAlert(new List<string>() { "Starting dev server" });
        Process.Start("dotnet", "watch");
        //var psiNpmRunDist = new ProcessStartInfo
        //{
        //    FileName = "cmd",
        //    RedirectStandardInput = true,
        //    WorkingDirectory = "./"
        //};
        //var pNpmRunDist = Process.Start(psiNpmRunDist);
        //pNpmRunDist.StandardInput.WriteLine("npx.cmd tailwindcss -i ./Assets/input.css -o ./wwwroot/output.css --watch");
        //pNpmRunDist.WaitForExit();
        
        string? npx = Where("npx.cmd") != null ? "npx.cmd" 
                    : Where("npx") != null     ? "npx"
                    : null;
        
        if (npx != null)
        {
            Process.Start(npx, "tailwindcss -i ./Assets/input.css -o ./wwwroot/output.css --watch");
        } 
        else
        {
            Console.Error.WriteLine("Command npx or npx.cmd is not found in the environment PATH"); 
        }
    }

    private string? Where(string executableName)
    {
        var directories = (Environment.GetEnvironmentVariable("PATH") ?? ".").Split(Path.DirectorySeparatorChar);
        foreach (var directory in directories)
        {
            var fileName = Path.Combine(Path.GetFullPath(directory), executableName);
            if (File.Exists(fileName))
            {
                return fileName;
            }
        }
        return null;
    }

}
