using Spark.Console.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spark.Console.Commands.Services;

public class CreateServiceCommand
{
    private readonly static string ServicePath = $"./Application/Services";

    public void Execute(string serviceName)
    {
        string appName = UserApp.GetAppName();

        ConsoleOutput.GenerateAlert(new List<string>() { $"Creating a new service" });

        bool wasGenerated = CreateServiceFile(appName, serviceName);

        if (!wasGenerated)
        {
            ConsoleOutput.WarningAlert(new List<string>() { $"{ServicePath}/{serviceName}.cs already exists. Nothing done." });
        }
        else
        {
            ConsoleOutput.SuccessAlert(new List<string>() { $"{ServicePath}/{serviceName}.cs generated!" });
        }
    }

    private bool CreateServiceFile(string appName, string serviceName)
    {
        string content = $@"namespace {appName}.Application.Services;

public class {serviceName}
{{

    public {serviceName}()
    {{
    }}

}}";
        return Files.WriteFileIfNotCreatedYet(ServicePath, serviceName + ".cs", content);
    }
}
