using BlazorSpark.Console.Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSpark.Console.Commands
{
    public class UpdateSparkCommand
    {
        public void Execute()
        {
            ConsoleOutput.StartAlert(new List<string>() { "Updating Blazor Spark Templates" });
            Process.Start("dotnet", "new install BlazorSpark.Templates").WaitForExit();
            ConsoleOutput.SuccessAlert(new List<string>() { "Blazor Spark Templates successfully updated" });
        }
    }
}
