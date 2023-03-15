using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using Zust.Application.Abstractions.Services;

namespace Zust.Infrastructure.Services
{
    internal class MailService : IMailService
    {

        IConfiguration _conf;

        public MailService(IConfiguration conf)
        {
            _conf = conf;
        }

        public async Task SendMailAsync(string ToEmail, string subject, string body, bool IsBodyHtml = true)
        {
            SmtpClient smtp = new();
            smtp.Credentials = new NetworkCredential(_conf["Mail:Email"], _conf["Mail:Password"]);
            smtp.Port = Convert.ToInt32(_conf["Mail:Port"]);
            smtp.Host = _conf["Mail:Host"];
            smtp.EnableSsl = true;

            MailAddress from = new MailAddress(_conf["Mail:Email"], "Zust", System.Text.Encoding.UTF8);
            MailAddress to = new MailAddress(ToEmail);
            MailMessage mail = new(from, to);
            mail.IsBodyHtml = true;
            mail.Subject = subject; 
            mail.Body = body;
            await smtp.SendMailAsync(mail);
        }
    }
}
