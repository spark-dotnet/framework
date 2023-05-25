using BlazorSpark.Console.Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSpark.Console.Commands.Project
{
    public class CreateProjectCommand
    {
        private readonly static string ProjectPath = $"./";
        public void Execute(string projectName)
        {
            if (String.IsNullOrEmpty(projectName))
            {
                ConsoleOutput.ErrorAlert(new List<string>() { $"spark new requires a project name. Ex: spark new <ProjectName>" });
                return;
            }
            ConsoleOutput.StartAlert(new List<string>() { $"Creating a Spark project at \"./{projectName}\"" });
            Process.Start("dotnet", $"new blazorspark -n {projectName} -o {projectName}").WaitForExit();
            ConsoleOutput.SuccessAlert(new List<string>() {
                $"Blazor Spark project {projectName} created",
                $"Application ready! Build something amazing..."
            });
            // todo
            // to get started
            // cd <ProjectName>
            // spark open
        }
    }
}
