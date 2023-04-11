using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSpark.Library.Mail
{
    public class Mailable<T>
    {

        private MailRecipient from;

        private IEnumerable<MailRecipient> to;

        private MailRecipient replyTo;

        private IEnumerable<MailRecipient> cc;

        private IEnumerable<MailRecipient> bcc;

        private string subject;

        private string html;

        private List<Attachment> _attachments;

        public Mailable<T> From(MailRecipient recipient)
        {
            this.from = recipient;
            return this;
        }

        public Mailable<T> From(string email) =>
            this.From(new MailRecipient(email));

        public Mailable<T> To(IEnumerable<MailRecipient> recipients)
        {
            this.to = recipients;
            return this;
        }

        public Mailable<T> To(MailRecipient recipient) =>
            this.To(new MailRecipient[] { recipient });

        public Mailable<T> To(IEnumerable<string> addresses) =>
            this.To(addresses.Select(address => new MailRecipient(address)));

        public Mailable<T> To(string email) =>
            this.To(new MailRecipient(email));

        public Mailable<T> Cc(IEnumerable<MailRecipient> recipients)
        {
            this.cc = recipients;
            return this;
        }

        public Mailable<T> Cc(IEnumerable<string> addresses) =>
            this.Cc(addresses.Select(address => new MailRecipient(address)));

        public Mailable<T> Bcc(IEnumerable<MailRecipient> recipients)
        {
            this.bcc = recipients;
            return this;
        }

        public Mailable<T> Bcc(IEnumerable<string> addresses) =>
            this.Bcc(addresses.Select(address => new MailRecipient(address)));

        public Mailable<T> ReplyTo(MailRecipient replyTo)
        {
            this.replyTo = replyTo;
            return this;
        }

        public Mailable<T> ReplyTo(string address)
        {
            this.replyTo = new MailRecipient(address);
            return this;
        }

        public Mailable<T> Subject(string subject)
        {
            this.subject = subject;
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
            this.html = html;
            return this;
        }

        public virtual void Build() { }

        internal async Task SendAsync(RazorRenderer renderer, IMailer mailer)
        {
            this.Build();

            string message = await this.BuildMessage(renderer, mailer).ConfigureAwait(false);

            await mailer.SendAsync(
                message,
                this.subject,
                this.to,
                this.from,
                this.replyTo,
                this.cc,
                this.bcc,
                this._attachments
            ).ConfigureAwait(false);
        }

        internal async Task<string> RenderAsync(RazorRenderer renderer, IMailer mailer)
        {
            this.Build();
            return await this.BuildMessage(renderer, mailer).ConfigureAwait(false);
        }

        private async Task<string> BuildMessage(RazorRenderer renderer, IMailer mailer)
        {
            this.BindDynamicProperties();

            if (this.html != null)
            {
                return this.html;
            }

            if (this.viewPath != null)
            {
                return await renderer
                    .RenderViewToStringAsync<T>(this.viewPath, this.viewModel)
                    .ConfigureAwait(false);
            }

            throw new NoMailRendererFound(NoRenderFoundMessage);
        }

    }
}
