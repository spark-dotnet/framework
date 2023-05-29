using BlazorSpark.Console.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSpark.Console.Commands.Models
{
    public class CreateModelCommand
    {
        private readonly static string ModelPath = $"./Application/Models";
        public void Execute(string modelName)
        {
            string appName = UserApp.GetAppName();

            ConsoleOutput.GenerateAlert(new List<string>() { $"Creating a new model" });

            bool wasGenerated = CreateModelFile(appName, modelName);

            if (!wasGenerated)
            {
                ConsoleOutput.WarningAlert(new List<string>() { $"{ModelPath}/{modelName}.cs already exists. Nothing done." });
            }
            else
            {
                ConsoleOutput.SuccessAlert(new List<string>() { $"{ModelPath}/{modelName}.cs generated!" });
            }
        }

        private bool CreateModelFile(string appName, string modelName)
        {
            string content = $@"namespace {appName}.Application.Models
{{
    public class {modelName} : BaseModel
    {{
        public int Id {{ get; set; }}
    }}
}}";
            return Files.WriteFileIfNotCreatedYet(ModelPath, modelName + ".cs", content);
        }
    }
}
