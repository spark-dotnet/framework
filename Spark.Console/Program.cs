using Spark.Console.Commands;
using Spark.Console.Commands.Events;
using Spark.Console.Commands.Mail;
using Spark.Console.Commands.Migrations;
using Spark.Console.Commands.Models;
using Spark.Console.Commands.Pages;
using Spark.Console.Commands.Project;
using Spark.Console.Commands.Services;
using Spark.Console.Commands.Jobs;
using Spark.Console.Shared;
using McMaster.Extensions.CommandLineUtils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;

namespace Spark.Console;

class Program
{
    public static void Main(string[] args)
    {
        var app = new CommandLineApplication
        {
            AllowArgumentSeparator = true,
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

        app.Command("dev", config =>
            config.OnExecute(() =>
                new DevCommand().Execute()
            )
        );

        app.Command("new", config =>
        {
            config.OnExecute(() =>
            {
                config.ShowHelp();
                return 1;
            });

            config.Description = "Create a new Spark project.";
            var projectName = config.Argument<string>("name", "Name of the project to generate.");
            var projectType = config.Option("-t|--type <ProjectType>", "Projec type of the new Spark project", CommandOptionType.SingleValue);
            config.OnExecute(() =>
            {
                string project = projectName.Value ?? null;
                new CreateProjectCommand().Execute(project, projectType.Value());
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

            config.Command("job", jobConfig =>
            {
                jobConfig.Description = "Create a new Job.";
                var jobName = jobConfig.Argument<string>("jobName", "Name of the Job to generate.").IsRequired();
                jobConfig.OnExecute(() =>
                {
                    new CreateJobCommand().Execute(jobName.Value);
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
            System.Console.WriteLine("Spark had some trouble... try again.");
            System.Console.WriteLine(e.Message);
        }


        if (args.Length == 0)
        {
            System.Console.WriteLine($"spark requires a command to do something");
            return;
        }
    }
}