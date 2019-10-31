namespace Abstractions.Services
{
    public interface IEmailSender
    {
        void Send(string address, string subject, string message);
    }
}
