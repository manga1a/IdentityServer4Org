using System;
using Abstractions.Services;
using MailKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Services.Email
{
    public class SmtpConfig
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class SmtpEmailSender : IEmailSender
    {
        private readonly SmtpConfig config;

        public SmtpEmailSender(IOptions<SmtpConfig> config)
        {
            this.config = config.Value;
        }

        public void Send(string address, string subject, string message)
        {
            //TODO: send an email
            //System.IO.File.WriteAllText(@"C:\temp\comfirmation_link.txt", message);

            using (var client = new SmtpClient())
            {
                client.Connect(config.Host, config.Port, SecureSocketOptions.Auto);
                client.Authenticate(config.Username, config.Password);
                client.Send(CreateMessage(address, subject, message));
                client.Disconnect(true);
            }
        }

        private MimeMessage CreateMessage(string address, string subject, string text)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Joey", config.Username));
            message.To.Add(new MailboxAddress("Alice", address));
            message.Subject = subject;

            message.Body = new TextPart("plain")
            {
                Text = @"Hey Alice,

What are you up to this weekend? Monica is throwing one of her parties on
    Saturday and I was hoping you could make it.

Will you be my +1?

-- Joey
"
            };

            return message;
        }
    }
}
