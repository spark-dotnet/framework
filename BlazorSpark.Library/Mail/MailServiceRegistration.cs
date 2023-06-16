using BlazorSpark.Library.Mail.Mailers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

// Credit to James, the author of Coravel Mailer, for a lot of the inspiration and code for Sparks Mailer library
// Link to Coravel Mailer: https://github.com/jamesmh/coravel/tree/master/Src/Coravel.Mailer
namespace BlazorSpark.Library.Mail
{
    public static class MailServiceRegistration
	{
		public static IServiceCollection AddMailer(this IServiceCollection services, IConfiguration config)
		{
			string mailDriver = config.GetValue<string>("Spark:Mail:Default", "file")!;

			if (mailDriver == MailerTypes.file) 
			{
				services.AddFileMailer(config);
			}
			else if (mailDriver == MailerTypes.smtp)
			{
				services.AddSmtpMailer(config);
			}
			else
			{
				throw new Exception("Invalid mail driver. Check your .env file and make sure the MAIL_MAILER variable is set to file or smtp.");
			}
			return services;
		}

		public static IServiceCollection AddFileMailer(this IServiceCollection services, IConfiguration config)
		{
			var globalFrom = GetGlobalFromRecipient(config);
			var mailer = new FileMailer(globalFrom, config.GetValue<string>("Spark:Mail:Mailers:File:Path", "Storage/Mail/mail.log")!);
			services.AddSingleton<IMailer>(mailer);
			return services;
		}

		public static IServiceCollection AddSmtpMailer(this IServiceCollection services, IConfiguration config)
		{
			var globalFrom = GetGlobalFromRecipient(config);
			IMailer mailer = new SmtpMailer(
                config.GetValue<string>("Spark:Mail:Mailers:Smtp:Host", "")!,
				config.GetValue<int>("Spark:Mail:Mailers:Smtp:Port", 587),
				config.GetValue<string>("Spark:Mail:Mailers:Smtp:Username", null)!,
				config.GetValue<string>("Spark:Mail:Mailers:Smtp:Password", null)!,
				globalFrom
			);
			services.AddSingleton<IMailer>(mailer);
			return services;
		}

		private static MailRecipient GetGlobalFromRecipient(IConfiguration config)
		{
			string globalFromAddress = config.GetValue<string>("Spark:Mail:From:Address", null);
			string globalFromName = config.GetValue<string>("Spark:Mail:From:Name", null);

			if (globalFromAddress != null)
			{
				return new MailRecipient(globalFromAddress, globalFromName);
			}
			else
			{
				return null;
			}
		}
	}
}
