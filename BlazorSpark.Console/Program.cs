using BlazorSpark.Console.Commands;
using BlazorSpark.Console.Commands.Migrations;
using BlazorSpark.Console.Commands.Project;
using McMaster.Extensions.CommandLineUtils;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;

namespace BlazorSpark.Console
{
    class Program
    {
        public static void Main(string[] args)
        {
            // TODO:
            // spark open (start MyApp.sln)
            // code generation
            // code generation
            var app = new CommandLineApplication
            {
                Name = "spark"
            };

            app.HelpOption(inherited: true);

            app.OnExecute(() =>
            {
                app.ShowHelp();
                return 1;
            });

            app.Command("install", config =>
                config.OnExecute(() =>
                    new InstallSparkCommand().Execute()
                )
            );

            app.Command("update", config =>
                config.OnExecute(() =>
                    new UpdateSparkCommand().Execute()
                )
            );

            app.Command("new", config =>
            {
                config.OnExecute(() =>
                {
                    config.ShowHelp();
                    return 1;
                });

                config.Description = "Create a new Blazor Spark project.";
                var projectName = config.Argument<string>("name", "Name of the project to generate.");
                config.OnExecute(() =>
                {
                    string project = projectName.Value ?? null;
                    new CreateProjectCommand().Execute(project);
                });
            });

            app.Command("open", config =>
                config.OnExecute(() =>
                    new OpenSolutionCommand().Execute()
                )
            );

            app.Command("make:migration", config =>
            {
                config.Description = "Create a new migration.";
                var migrationName = config.Argument<string>("name", "Name of the Migration to generate.");
                config.OnExecute(() =>
                {
                    string migration = migrationName.Value ?? Guid.NewGuid().ToString();
                    new CreateMigration().Execute(migration);
                });
            });

            app.Command("migrate", config =>
                config.OnExecute(() =>
                    new RunMigrations().Execute()
                )
            );


            try
            {
                int code = app.Execute(args);
                Environment.Exit(code);
            }
            catch (Exception e)
            {
                System.Console.WriteLine("Blazor Spark had some trouble... try again.");
                System.Console.WriteLine(e.Message);
            }


            if (args.Length == 0)
            {
                System.Console.WriteLine($"spark requires a command to do something");
                return;
            }

            // spark install
            //if (command == "install")
            //{
            //	await InstallTemplate();
            //	return;
            //}


            //// spark update
            //if (command == "update")
            //{
            //    await Update();
            //    return;
            //}

            //// spark new <ProjectName>
            //if (command == "new")
            //{
            //	if (args.Length != 2)
            //	{
            //		Console.WriteLine($"spark new requires a project name. Ex: spark new <ProjectName>");
            //		return;
            //	}
            //	var projectName = args[1];
            //	await CreateProject(projectName);
            //	return;
            //}

            //// spark make:migration <MigrationName>
            //if (command == "make:migration")
            //{
            //	if (args.Length != 2)
            //	{
            //		Console.WriteLine($"spark make:migration requires a migration name. Ex: spark make:migration <MigrationName>");
            //		return;
            //	}
            //	var name = args[1];
            //	await CreateMigration(name);
            //	return;
            //}

            //if (command == "run:migration")
            //{
            //	await RunMigration();
            //	return;
            //}

            //async Task InstallTemplate()
            //{
            //	var psi = new ProcessStartInfo
            //	{
            //		FileName = "dotnet",
            //		Arguments = "new install BlazorSpark.Templates"
            //	};

            //	using var proc = Process.Start(psi)!;
            //	await proc.WaitForExitAsync();
            //}

            //async Task Update()
            //{
            //    Console.WriteLine($"Updating Spark Templates");
            //    var psi2 = new ProcessStartInfo
            //    {
            //        FileName = "dotnet",
            //        Arguments = "new install BlazorSpark.Templates"
            //    };

            //    using var proc2 = Process.Start(psi2)!;
            //    await proc2.WaitForExitAsync();
            //}

            //async Task CreateProject(string projectName)
            //{
            //	Console.WriteLine($"Creating a Spark project at \"./{projectName}\"");
            //	var psi = new ProcessStartInfo
            //	{
            //		FileName = "dotnet",
            //		Arguments = $"new blazorspark -n {projectName} -o {projectName}"
            //	};

            //	using var proc = Process.Start(psi)!;
            //	await proc.WaitForExitAsync();
            //	Console.WriteLine($"Spark project {projectName} successfully created.");
            //	Console.WriteLine($"Application ready! Build something amazing.");
            //}

            //async Task CreateMigration(string name)
            //{
            //	var psi = new ProcessStartInfo
            //	{
            //		FileName = "dotnet",
            //		Arguments = $"ef migrations add {name} -o Application/Database/Migrations"
            //	};

            //    using var proc = Process.Start(psi)!;
            //	await proc.WaitForExitAsync();
            //}
            //async Task RunMigration()
            //{
            //	var psi = new ProcessStartInfo
            //	{
            //		FileName = "dotnet",
            //		Arguments = $"ef database update"
            //	};

            //	using var proc = Process.Start(psi)!;
            //	await proc.WaitForExitAsync();
            //}
        }
    }
}