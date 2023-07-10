using Spark.Console.Shared;
using McMaster.Extensions.CommandLineUtils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spark.Console.Commands.Project
{
    public class CreateProjectCommand
    {
        private readonly static string ProjectPath = $"./";

        public void Execute(string projectName, string cssFramework)
        {
            if (String.IsNullOrEmpty(projectName))
            {
                ConsoleOutput.ErrorAlert(new List<string>() { $"spark new requires a project name. Ex: spark new [projectName]" });
                return;
            }
            var command = $"new sparkblazor -n {projectName} -o {projectName}";
            if (!String.IsNullOrEmpty(cssFramework))
            {
                if (!CssFrameworks.IsValid(cssFramework))
                {
                    ConsoleOutput.ErrorAlert(new List<string>() { $"Invalid css framework. Valid values: tailwind, bootstrap, pico" });
                    return;
                }
                command += $" --Css {cssFramework}";
            }
            ConsoleOutput.StartAlert(new List<string>() { $"Creating a Spark project at \"./{projectName}\"" });
            Process.Start("dotnet", command).WaitForExit();
            ConsoleOutput.SuccessAlert(new List<string>() {
                $"Spark project {projectName} created",
                $"Application ready! Build something amazing..."
            });
        }
    }
}
