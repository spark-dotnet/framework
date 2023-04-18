using BlazorSpark.Example.Application.Models;
using Coravel.Events.Interfaces;

namespace BlazorSpark.Example.Application.Events
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
