using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Zust.Application.ViewModels
{
    public class CommentVM
    {
        public int? CommentId { get; set; }
        public int PostId { get; set; }
        [MaxLength(200)]
        public string? Text { get; set; }
        public IFormFile? Image { get; set; }
    }
}
