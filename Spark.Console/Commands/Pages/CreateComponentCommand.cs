using Spark.Console.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Spark.Console.Commands.Pages;

public class CreateComponentCommand
{
    private readonly static string ComponentPath = $"./Pages/Shared";

    public void Execute(string componentName, bool isInline = false)
    {
        string appName = UserApp.GetAppName();

        ConsoleOutput.GenerateAlert(new List<string>() { $"Creating a new Blazor Component" });

        bool wasGenerated;
        if (isInline)
        {
            wasGenerated = CreateComponentFileInline(appName, componentName);

            if (!wasGenerated)
            {
                ConsoleOutput.WarningAlert(new List<string>() { $"{ComponentPath}/{componentName}.razor already exists. Nothing done." });
            }
            else
            {
                ConsoleOutput.SuccessAlert(new List<string>() { $"{ComponentPath}/{componentName}.razor generated!" });
            }
        }
        else
        {
            wasGenerated = CreateComponentFile(appName, componentName);

            if (!wasGenerated)
            {
                ConsoleOutput.WarningAlert(new List<string>() { $"{ComponentPath}/{componentName}.razor already exists. Nothing done." });
            }
            else
            {
                ConsoleOutput.SuccessAlert(new List<string>() { $"{ComponentPath}/{componentName}.razor and {ComponentPath}/{componentName}.cs generated!" });
            }
        }
        
    }

    private bool CreateComponentFileInline(string appName, string componentFilePath)
    {
        var paths = componentFilePath.Split('/');
        var fileName = paths.Last();
        var finalPath = ComponentPath;
        if (paths.Count() > 1)
        {
            var justFolders = componentFilePath.Replace($"/{fileName}", "");
            finalPath += $"/{justFolders}";
        }

        string content = $@"<div>
</div>

@code {{
    protected override void OnInitialized()
    {{
        
    }}
}}
";
        return Files.WriteFileIfNotCreatedYet($"{finalPath}", fileName.ToUpperFirst() + ".razor", content);
    }
    
    private bool CreateComponentFile(string appName, string componentFilePath)
    {
        var paths = componentFilePath.Split('/');
        var fileName = paths.Last();
        var finalPath = ComponentPath;
        if (paths.Count() > 1)
        {
            var justFolders = componentFilePath.Replace($"/{fileName}", "");
            finalPath += $"/{justFolders}";
        }

        string content = $@"<div>
</div>
";
        var success = Files.WriteFileIfNotCreatedYet($"{finalPath}", fileName.ToUpperFirst() + ".razor", content);
        
        if (!success) return false;
        
        string namespacePath = finalPath.Replace(".", "").Replace("/", ".");
        string codeBehindContent = $@"namespace {appName}{namespacePath};

public partial class {fileName.ToUpperFirst()}
{{

	protected override void OnInitialized()
	{{
	}}

}}
";
        return Files.WriteFileIfNotCreatedYet($"{finalPath}", fileName.ToUpperFirst() + ".cs", codeBehindContent);
    }
}
