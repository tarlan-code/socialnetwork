using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zust.Application.Abstractions.Services
{
    public interface IMailService
    {
        public Task SendMailAsync(string ToEmail, string subject, string body, bool IsBodyHtml = true);
    }
}
