using Spark.Library.Mail.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spark.Library.Mail.Mailers
{
    public class FileMailer : IMailer
	{
		private string _filePath;
		private MailRecipient _globalFrom;

		public FileMailer(MailRecipient globalFrom, string filePath)
		{
			this._globalFrom = globalFrom;
			this._filePath = filePath;
        }

		public async Task SendAsync(string message, string subject, IEnumerable<MailRecipient> to, MailRecipient from, MailRecipient replyTo, IEnumerable<MailRecipient> cc, IEnumerable<MailRecipient> bcc, IEnumerable<Attachment> attachments = null)
		{
			from = this._globalFrom ?? from;

			using (var writer = File.AppendText(_filePath))
			{
				await writer.WriteAsync($@"
{DateTime.Now}
---------------------------------------------
Subject: {subject}
To: {CommaSeparated(to)}    
From: {DisplayAddress(from)}
ReplyTo: {DisplayAddress(replyTo)}
Cc: {CommaSeparated(cc)}
Bcc: {CommaSeparated(bcc)}
Attachment: {(attachments is null ? "N/A" : string.Join(";", attachments.Select(a => a.Name)))}
---------------------------------------------

{message}
                ").ConfigureAwait(false);
			}
		}

		public async Task SendAsync<T>(Mailable<T> mailable)
		{
			await mailable.SendAsync(this);
		}

        private static string CommaSeparated(IEnumerable<MailRecipient> recipients) =>
            (recipients ?? Enumerable.Empty<MailRecipient>())
                .Select(r => DisplayAddress(r))
                .CommaSeparated();

        private static string DisplayAddress(MailRecipient recipient)
        {
            if (recipient == null)
                return string.Empty;
            else
                return $"{recipient.Name} <{recipient.Email}>";
        }
	}
}
