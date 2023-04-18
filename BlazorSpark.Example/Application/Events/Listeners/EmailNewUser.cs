using Coravel.Events.Interfaces;

namespace BlazorSpark.Example.Application.Events.Listeners
{
    public class EmailNewUser : IListener<UserCreated>
    {
        public EmailNewUser()
        {
        }

        public async Task HandleAsync(UserCreated broadcasted)
        {
            var user = broadcasted.User;
            // TODO: email the user something
            Console.WriteLine($"New user created : {user.Name}");
        }
    }
}
