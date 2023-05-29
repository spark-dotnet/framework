using BlazorSpark.Console.Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSpark.Console.Commands.Migrations
{
    public class RunMigrationsCommand
    {
        public void Execute()
        {
            ConsoleOutput.StartAlert(new List<string>() { "Running Blazor Spark migrations" });
            Process.Start("dotnet", "ef database update").WaitForExit();
            ConsoleOutput.SuccessAlert(new List<string>() { "Blazor Spark migrations ran" });
        }
    }
}
