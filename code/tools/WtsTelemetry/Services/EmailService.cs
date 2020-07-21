using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace WtsTelemetry.Services
{
    public class EmailService
    {
        private readonly Regex emailRegex = new Regex(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");

        private readonly string from;
        private readonly IEnumerable<string> to;
        private readonly string subject;

        public EmailService(ConfigurationService configService)
        {
            var config = configService.GetSendGridConfig();
            from = config.From;
            to = config.To;
            subject = config.Subject;
        }
        public SendGridMessage CreateMail(string content, string date)
        {
            var mail = new SendGridMessage();
            mail.From = new EmailAddress(from);
            mail.AddTos(GetEmailFromList(to));
            mail.Subject = string.Format(subject, date);
            mail.AddContent("text", content);

            return mail;
        }

        private List<EmailAddress> GetEmailFromList(IEnumerable<string> emails)
        {
            return emails
                    .Where(email => IsValidEmail(email))
                    .Select(email => new EmailAddress(email))
                    .ToList();
        }

        private bool IsValidEmail(string emailAdress) => emailRegex.IsMatch(emailAdress.Trim());

        private void AddAttachment(SendGridMessage mail, string filename, string content)
        {
            var bytes = Encoding.ASCII.GetBytes(content);
            var base64Content = Convert.ToBase64String(bytes);
            mail.AddAttachment(filename, base64Content);
        }
    }
}
