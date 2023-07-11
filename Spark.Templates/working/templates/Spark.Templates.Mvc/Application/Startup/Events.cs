using Spark.Templates.Mvc.Application.Events.Listeners;
using Spark.Templates.Mvc.Application.Events;
using Coravel.Events.Interfaces;
using Coravel;

namespace Spark.Templates.Mvc.Application.Startup
{
    public static class Events
    {
        public static IServiceProvider RegisterEvents(this IServiceProvider services)
        {
            IEventRegistration registration = services.ConfigureEvents();

            // add events and listeners here
            registration
                .Register<UserCreated>()
                .Subscribe<EmailNewUser>();

            return services;
        }
    }
}
