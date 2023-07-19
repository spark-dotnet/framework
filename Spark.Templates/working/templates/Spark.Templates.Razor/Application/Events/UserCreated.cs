using Spark.Templates.Razor.Application.Models;
using Coravel.Events.Interfaces;

namespace Spark.Templates.Razor.Application.Events
{

    public class UserCreated : IEvent
    {
        public User User { get; set; }

        public UserCreated(User user)
        {
            this.User = user;
        }
    }
}
