using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Spark.Console.Shared
{
    public class ModelExtractor
    {
        public static ModelInfo ExtractModelInfo(string pathToFile)
        {
            ModelInfo modelInfo = new ModelInfo();
            modelInfo.Name = ExtractClassName(pathToFile);
            modelInfo.Path = pathToFile;
            modelInfo.FileName = Path.GetFileName(pathToFile);
            modelInfo.Properties = ExtractProperties(pathToFile);

            return modelInfo;
        }

        public static string ExtractClassName(string pathToFile)
        {
            var code = File.ReadAllText(pathToFile);
            var syntaxTree = CSharpSyntaxTree.ParseText(code);
            var root = syntaxTree.GetRoot();

            var classDeclaration = root.DescendantNodes().OfType<ClassDeclarationSyntax>().FirstOrDefault();
            if (classDeclaration != null)
            {
                return classDeclaration.Identifier.ValueText;
            }

            return "";
        }

        private static List<ModelProperty> ExtractProperties(string pathToFile)
        {
            List<ModelProperty> modelProperties = new List<ModelProperty>();
            var code = File.ReadAllText(pathToFile);
            var syntaxTree = CSharpSyntaxTree.ParseText(code);
            var root = syntaxTree.GetRoot();

            foreach (var property in root.DescendantNodes().OfType<PropertyDeclarationSyntax>())
            {
                ModelProperty mp = new ModelProperty()
                {
                    Name = property.Identifier.ValueText,
                    Type = property.Type.ToString(),
                    Accessors = property.AccessorList.ToString(),
                    Modifiers = property.Modifiers.ToString()
                };

                modelProperties.Add(mp);
            }
            return modelProperties;
        }
    }

    public class ModelInfo
    {
        public string Name { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public List<ModelProperty> Properties { get; set; }
    }

    public class ModelProperty
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Accessors { get; set; }
        public string Modifiers { get; set; }
    }
}
