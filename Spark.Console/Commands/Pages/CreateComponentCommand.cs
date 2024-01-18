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
    private readonly static string ComponentPath = $"./Pages/Components";

    public void Execute(string componentName)
    {
        string appName = UserApp.GetAppName();

        ConsoleOutput.GenerateAlert(new List<string>() { $"Creating a new Blazor Component" });

        bool wasGenerated = CreateComponentFileInline(appName, componentName);

        if (!wasGenerated)
        {
            ConsoleOutput.WarningAlert(new List<string>() { $"{ComponentPath}/{componentName}.razor already exists. Nothing done." });
        }
        else
        {
            ConsoleOutput.SuccessAlert(new List<string>() { $"{ComponentPath}/{componentName}.razor generated!" });
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

        string content = $@"@code {{
    protected override void OnInitialized()
    {{
        
    }}
}}
<div>
</div>
";
        return Files.WriteFileIfNotCreatedYet($"{finalPath}", fileName.ToUpperFirst() + ".razor", content);
    }
    
}
