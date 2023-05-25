using BlazorSpark.Console.Shared;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using static System.Net.WebRequestMethods;

namespace BlazorSpark.Console.Commands
{
    public struct InstallSparkCommand
    {
        public void Execute()
        {
            ConsoleOutput.StartAlert(new List<string>() { "Installing Spark" });
            Process.Start("dotnet", "new install BlazorSpark.Templates").WaitForExit();
            ConsoleOutput.SuccessAlert(new List<string>() { "Blazor Spark was installed! To learn more visit our offical docs - https://blazorspark.com/" });
        }
    }
}
