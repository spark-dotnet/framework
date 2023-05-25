using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSpark.Console.Shared
{
    public static class ConsoleOutput
    {
        public static void StartAlert(List<string> outputs)
        {
            System.Console.WriteLine("");
            System.Console.ForegroundColor = ConsoleColor.Yellow;
            WriteStringsToConsole(outputs);
            System.Console.WriteLine("--------------------------------------------------------------------------------------------");
            System.Console.ResetColor();
            System.Console.WriteLine("");
        }

        public static void StandardAlert(List<string> outputs)
        {
            System.Console.WriteLine("");
            System.Console.ForegroundColor = ConsoleColor.White;
            WriteStringsToConsole(outputs);
            System.Console.ResetColor();
            System.Console.WriteLine("");
        }

        public static void GenerateAlert(List<string> outputs)
        {
            System.Console.WriteLine("");
            System.Console.ForegroundColor = ConsoleColor.Blue;
            WriteStringsToConsole(outputs);
            System.Console.ResetColor();
            System.Console.WriteLine("");
        }

        public static void SuccessAlert(List<string> outputs)
        {
            System.Console.WriteLine("");
            System.Console.ForegroundColor = ConsoleColor.Green;
            System.Console.WriteLine("--------------------------------------------------------------------------------------------");
            WriteStringsToConsole(outputs);
            System.Console.ResetColor();
            System.Console.WriteLine("");
        }

        public static void ErrorAlert(List<string> outputs)
        {
            System.Console.WriteLine("");
            System.Console.ForegroundColor = ConsoleColor.Red;
            System.Console.WriteLine("--------------------------------------------------------------------------------------------");
            WriteStringsToConsole(outputs);
            System.Console.ResetColor();
            System.Console.WriteLine("");
        }

        private static void WriteStringsToConsole(List<string> outputs)
        {
            foreach (var output in outputs)
            {
                System.Console.WriteLine(output);
            }
        }
    }
}
