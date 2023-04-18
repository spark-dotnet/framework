using BlazorSpark.Default.Application.Models;
using Coravel.Events.Interfaces;

namespace BlazorSpark.Default.Application.Events
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
