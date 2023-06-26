using BlazorSpark.Console.Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSpark.Console.Commands.Project
{
    internal class OpenSolutionCommand
    {
        public void Execute()
        {
            string appName = UserApp.GetAppName();
            char dirSep = Path.DirectorySeparatorChar;
            Process.Start(new ProcessStartInfo($@"{Directory.GetCurrentDirectory()}{dirSep}{appName}.sln") { UseShellExecute = true });
        }
    }
}
