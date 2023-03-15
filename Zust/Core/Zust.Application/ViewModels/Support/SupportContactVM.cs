using System.ComponentModel.DataAnnotations;

namespace Zust.Application.ViewModels
{
    public class SupportContactVM
    {
        [MaxLength(30)]
        public string Subject { get; set; }
        [MaxLength(500)]
        public string Message { get; set; }
    }
}
