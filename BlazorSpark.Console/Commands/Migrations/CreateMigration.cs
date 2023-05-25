using BlazorSpark.Console.Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BlazorSpark.Console.Commands.Migrations
{
    public class CreateMigration
    {
        private readonly static string MigrationPath = $"./Application/Database/Migrations";
        public void Execute(string migrationName)
        {
            ConsoleOutput.StartAlert(new List<string>() { $"Creating a new migration" });
            Process.Start("dotnet", $"ef migrations add {migrationName} -o {MigrationPath}").WaitForExit();
            ConsoleOutput.SuccessAlert(new List<string>() {
                $"Blazor Spark migration {migrationName} created",
            });
        }
    }
}
