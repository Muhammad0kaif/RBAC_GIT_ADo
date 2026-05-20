using System.Net;
using System.Net.Mail;

namespace AdoApi2.Services
{
    public class EmailService(IConfiguration configuration)
    {
        public async Task SendEmailAsync( string to, string subject,string body)
        {
            var host = configuration["Email:SmtpHost"];
            var port = Convert.ToInt32(configuration["Email:SmtpPort"]);
            var username = configuration["Email:Username"];
            var password = configuration["Email:Password"];
            var from = configuration["Email:From"];

            using var client = new SmtpClient(host, port)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(username, password)
            };

            var mail = new MailMessage
            {
                From = new MailAddress(from),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            mail.To.Add(to);

            await client.SendMailAsync(mail);
        }
    }
}