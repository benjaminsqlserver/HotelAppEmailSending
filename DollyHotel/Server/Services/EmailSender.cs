using MailKit.Security;
using MimeKit.Text;
using MimeKit;
using MailKit.Net.Smtp;

namespace DollyHotel.Server.Services
{
    public class EmailSender : IEmailSender
    {
        public async Task SendHTMLEmailAsync(string fromEmail, string toEmail, string subject, string body, string smtpAddress, string portNo, string username, string password)
        {
            try
            {
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(fromEmail));
                email.To.Add(MailboxAddress.Parse(toEmail));
                email.Subject = subject;
                email.Body = new TextPart(TextFormat.Html) { Text = body };

                // send email
                using var smtp = new SmtpClient();
                smtp.Connect(smtpAddress, Convert.ToInt32(portNo), SecureSocketOptions.StartTls);
                smtp.Authenticate(username, password);
                smtp.Send(email);
                smtp.Disconnect(true);

                //snippet to make method run asynchronously

                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw;
            }

        }
    }
}
