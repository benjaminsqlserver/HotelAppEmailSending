namespace DollyHotel.Server.Services
{
    public interface IEmailSender
    {
        Task SendHTMLEmailAsync(string fromEmail, string toEmail, string subject, string body, string smtpAddress, string portNo, string username, string password);
    }
}