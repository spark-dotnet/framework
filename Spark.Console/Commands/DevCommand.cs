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
        Process.Start("npx.cmd", "tailwindcss -i ./Assets/input.css -o ./wwwroot/output.css --watch");
    }
}
