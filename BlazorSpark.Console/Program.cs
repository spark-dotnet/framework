using BlazorSpark.Console.Commands;
using BlazorSpark.Console.Commands.Events;
using BlazorSpark.Console.Commands.Mail;
using BlazorSpark.Console.Commands.Migrations;
using BlazorSpark.Console.Commands.Models;
using BlazorSpark.Console.Commands.Pages;
using BlazorSpark.Console.Commands.Project;
using BlazorSpark.Console.Commands.Services;
using BlazorSpark.Console.Commands.Tasks;
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

            app.Command("make", config =>
            {
                config.OnExecute(() =>
                {
                    config.ShowHelp();
                    return 1;
                });

                config.Command("migration", migrationConfig =>
                {

                    migrationConfig.Description = "Create a new migration.";
                    var migrationName = migrationConfig.Argument<string>("name", "Name of the Migration to generate.");
                    migrationConfig.OnExecute(() =>
                    {
                        string migration = migrationName.Value ?? Guid.NewGuid().ToString();
                        new CreateMigrationCommand().Execute(migration);
                    });
                });

                config.Command("event", eventConfig =>
                {
                    eventConfig.Description = "Create a new Event and Listener.";
                    var eventName = eventConfig.Argument<string>("eventName", "Name of the Event to generate.").IsRequired();
                    var listenerName = eventConfig.Argument<string>("listenerName", "Name of the Listener to generate.").IsRequired();
                    eventConfig.OnExecute(() =>
                    {
                        new CreateEventCommand().Execute(eventName.Value, listenerName.Value);
                    });
                });

                config.Command("mail", mailConfig =>
                {
                    mailConfig.Description = "Create a new Mailable.";
                    var mailableName = mailConfig.Argument<string>("mailableName", "Name of the Mailable to generate.").IsRequired();
                    mailConfig.OnExecute(() =>
                    {
                        new CreateMailableCommand().Execute(mailableName.Value);
                    });
                });

                config.Command("model", modelConfig =>
                {
                    modelConfig.Description = "Create a new Model.";
                    var modelName = modelConfig.Argument<string>("modelName", "Name of the Model to generate.").IsRequired();
                    modelConfig.OnExecute(() =>
                    {
                        new CreateModelCommand().Execute(modelName.Value);
                    });
                });

                config.Command("service", serviceConfig =>
                {
                    serviceConfig.Description = "Create a new Service.";
                    var serviceName = serviceConfig.Argument<string>("serviceName", "Name of the Service to generate.").IsRequired();
                    serviceConfig.OnExecute(() =>
                    {
                        new CreateServiceCommand().Execute(serviceName.Value);
                    });
                });

                config.Command("task", taskConfig =>
                {
                    taskConfig.Description = "Create a new Task.";
                    var taskName = taskConfig.Argument<string>("taskName", "Name of the Task to generate.").IsRequired();
                    taskConfig.OnExecute(() =>
                    {
                        new CreateTaskCommand().Execute(taskName.Value);
                    });
                });

                config.Command("page", pageConfig =>
                {
                    pageConfig.Description = "Create a new Blazor Page.";
                    var pageName = pageConfig.Argument<string>("pageName", "Name of the Page to generate.").IsRequired();
                    pageConfig.OnExecute(() =>
                    {
                        new CreatePageCommand().Execute(pageName.Value);
                    });
                });

                config.Command("component", componentConfig =>
                {
                    componentConfig.Description = "Create a new Blazor Component.";
                    var componentName = componentConfig.Argument<string>("pageName", "Name of the Component to generate.").IsRequired();
                    componentConfig.OnExecute(() =>
                    {
                        new CreateComponentCommand().Execute(componentName.Value);
                    });
                });

                // make migration DONE
                // make event DONE
                // make mail DONE
                // make model DONE
                // make service DONE
                // make task DONE
                // make page
                // make component
            });

            app.Command("migrate", config =>
                config.OnExecute(() =>
                    new RunMigrationsCommand().Execute()
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