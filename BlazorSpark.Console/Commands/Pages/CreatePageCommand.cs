using BlazorSpark.Console.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BlazorSpark.Console.Commands.Pages
{
    public class CreatePageCommand
    {
        private readonly static string PagePath = $"./Pages";

        public void Execute(string pageName)
        {
            string appName = UserApp.GetAppName();

            ConsoleOutput.GenerateAlert(new List<string>() { $"Creating a new Page" });

            bool wasGenerated = CreatePageFile(appName, pageName);

            if (!wasGenerated)
            {
                ConsoleOutput.WarningAlert(new List<string>() { $"{PagePath}/{pageName}.cs already exists. Nothing done." });
            }
            else
            {
                ConsoleOutput.SuccessAlert(new List<string>() { $"{PagePath}/{pageName}.cs generated!" });
            }
        }

        private bool CreatePageFile(string appName, string pageFilePath)
        {
            var paths = pageFilePath.Split('/');
            var fileName = paths.Last();
            var finalPath = PagePath;
            if (paths.Count() > 1)
            {
                var justFolders = pageFilePath.Replace($"/{fileName}", "");
                finalPath += $"/{justFolders}";
            }
            var pageKebab = pageFilePath.PascalToKebabCase();

            string content = $@"@page ""/{pageKebab}""

<h1>My Page</h1>

@code {{

    protected override void OnInitialized()
    {{
        
    }}
}}";
            return Files.WriteFileIfNotCreatedYet($"{finalPath}", fileName + ".razor", content);
        }
    }
}
