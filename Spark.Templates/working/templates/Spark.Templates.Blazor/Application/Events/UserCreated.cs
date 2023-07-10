using Spark.Templates.Blazor.Application.Models;
using Coravel.Events.Interfaces;

namespace Spark.Templates.Blazor.Application.Events
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
