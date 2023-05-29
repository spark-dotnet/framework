using BlazorSpark.Console.Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSpark.Console.Commands.Events
{
    public class CreateEventCommand
    {
        private readonly static string EventsPath = $"./Application/Events";
        private readonly static string ListenersPath = $"./Application/Events/Listeners";
        public void Execute(string eventName, string listenerName)
        {
            string appName = UserApp.GetAppName();

            ConsoleOutput.GenerateAlert(new List<string>() { $"Creating a new event and listener" });
            bool eventWasCreated = GenerateEventFile(appName, eventName);
            bool listenerWasCreated = GenerateListenerFile(appName, eventName, listenerName);

            if (!eventWasCreated && !listenerWasCreated)
            {
                ConsoleOutput.WarningAlert(new List<string>() { $"Both files already exist! Nothing done." });
            }
            else
            {
                if (eventWasCreated)
                {
                    ConsoleOutput.SuccessAlert(new List<string>() { $"{EventsPath}/{eventName}.cs generated!" });
                }
                if (listenerWasCreated)
                {
                    ConsoleOutput.SuccessAlert(new List<string>() { $"{ListenersPath}/{listenerName}.cs generated!" });
                }

                ConsoleOutput.WarningAlert(new List<string>() { "Note: Remember to add your listeners to the Application/Startup/AppServiceRegistration.cs AddEventServices() method and register your events and listeners in Application/Startup/Events.cs." });
            }
        }

        private bool GenerateListenerFile(string appName, string eventName, string listenerName)
        {
            string content = $@"using System.Threading.Tasks;
using Coravel.Events.Interfaces;
using {appName}.Application.Events;

namespace {appName}.Application.Events.Listeners
{{
    public class {listenerName} : IListener<{eventName}>
    {{
        public Task HandleAsync({eventName} broadcasted)
        {{
            return Task.CompletedTask;
        }}
    }}
}}";
            return Files.WriteFileIfNotCreatedYet(ListenersPath, listenerName + ".cs", content);
        }

        private bool GenerateEventFile(string appName, string eventName)
        {
            string eventContent = $@"using Coravel.Events.Interfaces;

namespace {appName}.Application.Events
{{
    public class {eventName} : IEvent
    {{
        public string Message {{ get; set; }}

        public {eventName}(string message)
        {{
            this.Message = message;
        }}
    }}
}}";

            return Files.WriteFileIfNotCreatedYet(EventsPath, eventName + ".cs", eventContent);
        }
    }
}
