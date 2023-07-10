using Spark.Console.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spark.Console.Commands.Tasks
{
    public class CreateTaskCommand
    {
        private readonly static string TaskPath = $"./Application/Tasks";

        public void Execute(string taskName)
        {
            string appName = UserApp.GetAppName();

            ConsoleOutput.GenerateAlert(new List<string>() { $"Creating a new Task" });

            bool wasGenerated = CreateTaskFile(appName, taskName);

            if (!wasGenerated)
            {
                ConsoleOutput.WarningAlert(new List<string>() { $"{TaskPath}/{taskName}.cs already exists. Nothing done." });
            }
            else
            {
                ConsoleOutput.SuccessAlert(new List<string>() { $"{TaskPath}/{taskName}.cs generated!" });
            }
        }

        private bool CreateTaskFile(string appName, string taskName)
        {
            string content = $@"using Coravel.Invocable;

namespace {appName}.Application.Tasks
{{
    public class {taskName} : IInvocable
    {{

        public {taskName}()
        {{
        }}

        public Task Invoke()
        {{
            Console.WriteLine(""Do something in the background."");
            return Task.CompletedTask;
        }}

    }}
}}";
            return Files.WriteFileIfNotCreatedYet(TaskPath, taskName + ".cs", content);
        }
    }
}
