using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Zust.Application.ViewModels
{
    public class PostVM
    {
        [MaxLength(500)]
        public string? Text { get; set; }

        public List<IFormFile>? Images { get; set; }
        public List<IFormFile>? Videos { get; set; }
        public List<string>? Tags { get; set; }
    }
}
