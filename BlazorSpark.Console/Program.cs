using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;

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


// spark update
if (command == "update")
{
    await Update();
    return;
}

// spark new <ProjectName>
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

// spark make:migration <MigrationName>
if (command == "make:migration")
{
	if (args.Length != 2)
	{
		Console.WriteLine($"spark make:migration requires a migration name. Ex: spark make:migration <MigrationName>");
		return;
	}
	var name = args[1];
	await CreateMigration(name);
	return;
}

if (command == "run:migration")
{
	await RunMigration();
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

async Task Update()
{
    Console.WriteLine($"Updating Spark Templates");
    var psi2 = new ProcessStartInfo
    {
        FileName = "dotnet",
        Arguments = "new install BlazorSpark.Templates"
    };

    using var proc2 = Process.Start(psi2)!;
    await proc2.WaitForExitAsync();
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

async Task CreateMigration(string name)
{
	var psi = new ProcessStartInfo
	{
		FileName = "dotnet",
		Arguments = $"ef migrations add {name} -o Application/Database/Migrations"
	};

    using var proc = Process.Start(psi)!;
	await proc.WaitForExitAsync();
}
async Task RunMigration()
{
	var psi = new ProcessStartInfo
	{
		FileName = "dotnet",
		Arguments = $"ef database update"
	};

	using var proc = Process.Start(psi)!;
	await proc.WaitForExitAsync();
}