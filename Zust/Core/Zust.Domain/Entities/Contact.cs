using System.ComponentModel.DataAnnotations;

namespace Zust.Domain.Entities
{
    public class Contact:BaseEntity
    {
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        [MaxLength(30)]
        public string Subject { get; set; }
        [MaxLength(500)]
        public string Message { get; set; }
    }
}
