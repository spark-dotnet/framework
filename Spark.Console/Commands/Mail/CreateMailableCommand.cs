using Spark.Console.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spark.Console.Commands.Mail
{
    public class CreateMailableCommand
    {
        private readonly static string MailablePath = $"./Application/Mail";
        public void Execute(string mailableName)
        {
            string appName = UserApp.GetAppName();

            ConsoleOutput.GenerateAlert(new List<string>() { $"Creating a new mailable" });

            bool wasGenerated = GenerateMailableFile(appName, mailableName);

            if (!wasGenerated)
            {
                ConsoleOutput.WarningAlert(new List<string>() { $"{MailablePath}/{mailableName}.cs already exists. Nothing done." });
            }
            else
            {
                ConsoleOutput.SuccessAlert(new List<string>() { $"{MailablePath}/{mailableName}.cs generated!" });
            }
        }

        private bool GenerateMailableFile(string appName, string mailableName)
        {
            string content = $@"using Spark.Library.Mail;

namespace {appName}.Application.Mail
{{
    public class {mailableName} : Mailable<string>
    {{
        private string _email;

        public {mailableName}(string email)
        {{
            this._email = email;
        }}

        public override void Build()
        {{
			var recipient = new MailRecipient(this._email);
            this.To(recipient)
                .From(""hello@example.com"")
				.Subject(""Example subject"")
				.Html(@""
					<p>Example body</p>
				"");
        }}
    }}
}}";
            return Files.WriteFileIfNotCreatedYet(MailablePath, mailableName + ".cs", content);
        }
    }
}
