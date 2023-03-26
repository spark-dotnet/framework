using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

if (args.Length == 0)
{
	Console.WriteLine($"spark requires a command to do something");
	return;
}

var command = args[0];

// spark install
if (command == "install")
{
	await InstallTemplate();
	return;
}

// spark create project
if (command == "new")
{
	if (args.Length != 2)
	{
		Console.WriteLine($"spark new requires a project name. Ex: spark new <ProjectName>");
		return;
	}
	var projectName = args[1];
	await CreateProject(projectName);
	return;
}

async Task InstallTemplate()
{
	var psi = new ProcessStartInfo
	{
		FileName = "dotnet",
		Arguments = "new install BlazorSpark.Templates"
	};

	using var proc = Process.Start(psi)!;
	await proc.WaitForExitAsync();
}

async Task CreateProject(string projectName)
{
	Console.WriteLine($"Creating a Spark project at \"./{projectName}\"");
	var psi = new ProcessStartInfo
	{
		FileName = "dotnet",
		Arguments = $"new blazorspark -n {projectName} -o {projectName}"
	};

	using var proc = Process.Start(psi)!;
	await proc.WaitForExitAsync();
	Console.WriteLine($"Spark project {projectName} successfully created.");
	Console.WriteLine($"Application ready! Build something amazing.");
}