using BlazorSpark.Default.Application.Mail;
using BlazorSpark.Library.Mail;
using Coravel.Events.Interfaces;

namespace BlazorSpark.Default.Application.Events.Listeners
{
    public class EmailNewUser : IListener<UserCreated>
    {
        private readonly IMailer _mailer;
        private readonly IConfiguration _config;

        public EmailNewUser(IMailer mailer, IConfiguration config)
        {
            this._mailer = mailer;
            _config = config;
        }

        public async Task HandleAsync(UserCreated broadcasted)
        {
            var user = broadcasted.User;
            var mail = new GenericMailable()
                .To(user.Email)
                .Subject($"Welcome to {_config.GetValue<string>("APP_NAME")}")
                .Html(@"
<h1>Thanks for signing up!</h1>
");
            await this._mailer.SendAsync(mail);
        }
    }
}
