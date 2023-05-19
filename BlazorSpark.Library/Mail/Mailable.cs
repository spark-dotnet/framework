using BlazorSpark.Library.Mail.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSpark.Library.Mail
{
	public class Mailable<T>
	{
		private MailRecipient _from;

		private IEnumerable<MailRecipient> _to;

		private IEnumerable<MailRecipient> _cc;

		private IEnumerable<MailRecipient> _bcc;

		private MailRecipient _replyTo;

		private string _subject;

		private List<Attachment> _attachments;

		private string _html;

		public Mailable<T> From(MailRecipient recipient)
		{
			this._from = recipient;
			return this;
		}

		public Mailable<T> From(string email)
		{
			this.From(new MailRecipient(email));
			return this;
		}

		public Mailable<T> To(IEnumerable<MailRecipient> recipients)
		{
			this._to = recipients;
			return this;
		}

		public Mailable<T> To(MailRecipient recipient)
		{
			this.To(new MailRecipient[] { recipient });
			return this;
		}

		public Mailable<T> To(IEnumerable<string> addresses)
		{
			this.To(addresses.Select(address => new MailRecipient(address)));
			return this;
		}

		public Mailable<T> To(string email)
		{
			this.To(new MailRecipient(email));
			return this;
		}

		public Mailable<T> Cc(IEnumerable<MailRecipient> recipients)
		{
			this._cc = recipients;
			return this;
		}

		public Mailable<T> Cc(IEnumerable<string> addresses)
		{
			this.Cc(addresses.Select(address => new MailRecipient(address)));
			return this;
		}
		public Mailable<T> Bcc(IEnumerable<MailRecipient> recipients)
		{
			this._bcc = recipients;
			return this;
		}

		public Mailable<T> Bcc(IEnumerable<string> addresses)
		{
			this.Bcc(addresses.Select(address => new MailRecipient(address)));
			return this;
		}

		public Mailable<T> ReplyTo(MailRecipient replyTo)
		{
			this._replyTo = replyTo;
			return this;
		}

		public Mailable<T> ReplyTo(string address)
		{
			this._replyTo = new MailRecipient(address);
			return this;
		}

		public Mailable<T> Subject(string subject)
		{
			this._subject = subject;
			return this;
		}

		public Mailable<T> Attach(Attachment attachment)
		{
			if (this._attachments is null)
			{
				this._attachments = new List<Attachment>();
			}
			this._attachments.Add(attachment);
			return this;
		}

		public Mailable<T> Html(string html)
		{
			this._html = html;
			return this;
		}

		internal async Task SendAsync(IMailer mailer)
		{
			if (this._to == null)
			{
				throw new Exception("No to email set.");
			}

			await mailer.SendAsync(
				this._html,
				this._subject,
				this._to,
				this._from,
				this._replyTo,
				this._cc,
				this._bcc,
				this._attachments
			).ConfigureAwait(false);
		}
	}
}
