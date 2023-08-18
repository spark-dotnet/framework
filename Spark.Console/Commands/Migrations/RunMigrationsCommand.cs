using Spark.Console.Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spark.Console.Commands.Migrations;

public class RunMigrationsCommand
{
    public void Execute()
    {
        ConsoleOutput.StartAlert(new List<string>() { "Running Spark migrations" });
        Process.Start("dotnet", "ef database update").WaitForExit();
        ConsoleOutput.SuccessAlert(new List<string>() { "Spark migrations ran" });
    }
}
