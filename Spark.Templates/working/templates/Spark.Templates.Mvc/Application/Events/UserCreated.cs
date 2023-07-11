using Spark.Templates.Mvc.Application.Models;
using Coravel.Events.Interfaces;

namespace Spark.Templates.Mvc.Application.Events
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
