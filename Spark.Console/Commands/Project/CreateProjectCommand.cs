using Spark.Console.Shared;
using McMaster.Extensions.CommandLineUtils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spark.Console.Commands.Project;

public class CreateProjectCommand
{
	private readonly static string ProjectPath = $"./";

	public void Execute(string projectName, string cssFramework, string projectType)
	{
		if (String.IsNullOrEmpty(projectName))
		{
			ConsoleOutput.ErrorAlert(new List<string>() { $"spark new requires a project name. Ex: spark new [projectName]" });
			return;
		}
		var template = "sparkblazor"; // default is blazor
		if (!String.IsNullOrEmpty(projectType))
		{
			if (!ProjectTypes.IsValid(projectType))
			{
				ConsoleOutput.ErrorAlert(new List<string>() { $"Invalid project type. Valid values: blazor, mvc, razor, api" });
				return;
			}
			template = ProjectTypes.TranslateProjectTypeToTemplate(projectType);
		}
		var command = $"new {template} -n {projectName} -o {projectName}";
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
