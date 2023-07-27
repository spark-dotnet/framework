using Spark.Console.Shared;
using System.Collections.Generic;

namespace Spark.Console.Commands.Jobs
{
    public class CreateJobCommand
    {
        private readonly static string JobPath = $"./Application/Jobs";

        public void Execute(string jobName)
        {
            string appName = UserApp.GetAppName();

            ConsoleOutput.GenerateAlert(new List<string>() { $"Creating a new Job" });

            bool wasGenerated = CreateJobFile(appName, jobName);

            if (!wasGenerated)
            {
                ConsoleOutput.WarningAlert(new List<string>() { $"{JobPath}/{jobName}.cs already exists. Nothing done." });
            }
            else
            {
                ConsoleOutput.SuccessAlert(new List<string>() { $"{JobPath}/{jobName}.cs generated!" });
            }
        }

        private bool CreateJobFile(string appName, string jobName)
        {
            string content = $@"using Coravel.Invocable;

namespace {appName}.Application.Jobs
{{
    public class {jobName} : IInvocable
    {{

        public {jobName}()
        {{
        }}

        public Task Invoke()
        {{
            Console.WriteLine(""Do something in the background."");
            return Task.CompletedTask;
        }}

    }}
}}";
            return Files.WriteFileIfNotCreatedYet(JobPath, jobName + ".cs", content);
        }
    }
}
