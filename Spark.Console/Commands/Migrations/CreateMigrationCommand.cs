using Spark.Console.Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Spark.Console.Commands.Migrations;

public class CreateMigrationCommand
{
    private readonly static string MigrationPath = $"./Application/Database/Migrations";
    public void Execute(string migrationName)
    {
        ConsoleOutput.GenerateAlert(new List<string>() { $"Creating a new migration" });
        //Process.Start("dotnet", $"ef migrations add {migrationName} -o {MigrationPath}").WaitForExit();
		ProcessStartInfo startInfo = new ProcessStartInfo();
		startInfo.FileName = "dotnet"; 
		startInfo.Arguments = $"ef migrations add {migrationName} -o {MigrationPath}";
		startInfo.RedirectStandardOutput = true;
		startInfo.RedirectStandardError = true;
		startInfo.UseShellExecute = false;
		startInfo.CreateNoWindow = true;
		using (Process process = Process.Start(startInfo))
		{
			using (StreamReader outputReader = process.StandardOutput)
			using (StreamReader errorReader = process.StandardError)
			{
				string output = outputReader.ReadToEnd();
				string error = errorReader.ReadToEnd();

				if (error.Contains("Could not execute because the specified command or file was not found"))
				{
					ConsoleOutput.ErrorAlert(new List<string>() { error });
					ConsoleOutput.ErrorAlert(new List<string>() { "Miration was not created. Make sure  Entity Framework Core tools is installed." });
					ConsoleOutput.ErrorAlert(new List<string>() { "You can install this by running - dotnet tool install --global dotnet-ef" });
				}
				else
				{
					ConsoleOutput.StandardAlert(new List<string>() { output });
					ConsoleOutput.SuccessAlert(new List<string>() { $"Spark migration {migrationName} created" });
				}
			}

			process.WaitForExit();
		}
	}
}
