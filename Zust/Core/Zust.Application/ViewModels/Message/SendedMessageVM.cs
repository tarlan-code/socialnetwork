using Microsoft.AspNetCore.Http;

namespace Zust.Application.ViewModels
{
    public class SendedMessageVM
    {
        public string? Content { get; set; }
        public IFormFile? File{ get; set; }
    }
}
