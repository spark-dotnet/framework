namespace BlazorSpark.Library.Mail
{
	public class MailRecipient
	{
		public string Email { get; set; }
		public string Name { get; set; }

		public MailRecipient(string email)
		{
			this.Email = email;
		}

		public MailRecipient(string email, string name)
		{
			this.Email = email;
			this.Name = name;
		}
	}
}
