using BlazorSpark.Library.Environment;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSpark.Library.Mail
{
    public static class MailServiceRegistration
    {
        public static IServiceCollection AddMailer(this IServiceCollection services, IConfiguration config)
        {
            string driver = Env.Get("MAIL_DRIVER");

            if (driver == MailDrivers.FileLog)
            {
                AddFileLogMailer(services, config);
            }
            else if (driver == MailDrivers.SMPT)
            {
                // AddSmtpMailer(services, config);
            }
            else
            {
                throw new Exception("Invalid mail driver.");
            }
            return services;
        }


        public static IServiceCollection AddFileLogMailer(this IServiceCollection services, IConfiguration config)
        {
            var globalFrom = GetRecipient(config);
            RazorRenderer renderer = RazorRendererFactory.MakeInstance(config);
            var mailer = new FileLogMailer(renderer, globalFrom);
            services.AddSingleton<IMailer>(mailer);
            return services;
        }

        public static IServiceCollection AddSmtpMailer(this IServiceCollection services, IConfiguration config)
        {
            var globalFrom = GetRecipient(config);
            RazorRenderer renderer = RazorRendererFactory.MakeInstance(config);
            IMailer mailer = new SmtpMailer(
                renderer,
                config.GetValue<string>("Coravel:Mail:Host", ""),
                config.GetValue<int>("Coravel:Mail:Port", 0),
                config.GetValue<string>("Coravel:Mail:Username", null),
                config.GetValue<string>("Coravel:Mail:Password", null),
                globalFrom,
                null
            );
            services.AddSingleton<IMailer>(mailer);
            return services;
        }

        private static MailRecipient GetRecipient(IConfiguration config)
        {
            string fromAddress = Env.Get("MAIL_FROM_ADDRESS");
            string fromName = Env.Get("MAIL_FROM_NAME");

            if (fromAddress != null)
            {
                return new MailRecipient(fromAddress, fromName);
            }
            else
            {
                return null;
            }
        }
    }
}
