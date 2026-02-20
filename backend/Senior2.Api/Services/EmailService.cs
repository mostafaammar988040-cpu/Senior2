using System.Net;
using System.Net.Mail;

namespace Senior2.Api.Services
{
    public class EmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var smtpServer = _config["EmailSettings:SmtpServer"];
            var port = int.Parse(_config["EmailSettings:Port"]);
            var senderEmail = _config["EmailSettings:SenderEmail"];
            var username = _config["EmailSettings:Username"];
            var password = _config["EmailSettings:Password"];

            var client = new SmtpClient(smtpServer, port)
            {
                Credentials = new NetworkCredential(username, password),
                EnableSsl = true
            };

            var mail = new MailMessage(senderEmail, to, subject, body);
            mail.IsBodyHtml = true;

            await client.SendMailAsync(mail);
        }
    }
}
