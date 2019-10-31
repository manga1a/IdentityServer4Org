using Abstractions.Services;

namespace Services.Email
{
    public class SmtpEmailSender : IEmailSender
    {
        public void Send(string address, string subject, string message)
        {
            //TODO: send an email
            System.IO.File.WriteAllText(@"C:\temp\comfirmation_link.txt", message);
        }
    }
}
