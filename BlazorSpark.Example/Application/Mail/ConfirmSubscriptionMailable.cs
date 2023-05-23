using BlazorSpark.Example.Application.Models;
using BlazorSpark.Library.Mail;

namespace BlazorSpark.Example.Application.Mail
{
	public class ConfirmSubscriptionMailable : Mailable<string>
    {
        private string _email;
        public ConfirmSubscriptionMailable(string email) => this._email = email;

        public override void Build()
		{
			var recipient = new MailRecipient(this._email);
			this.To(recipient)
				.Subject("Confirm your Blazor Spark Subscription")
				.Html(@"
					<p>Confirm your subscription by clicking the link</p>
					<a href=""blazorspark.com/confirm""></a>
				");
		}
	}
}
