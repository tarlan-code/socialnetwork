using System.ComponentModel.DataAnnotations;

namespace Zust.Domain.Entities
{
    public class Comment:BaseEntity
    {

        public string? Content { get; set; }
        public string? Image { get; set; }

        [Range(0,int.MaxValue)]
        public int LikeCount { get; set; }


        //Relations
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        public int PostId { get; set; }
        public Post Post { get; set; }


        public int? RepliedId { get; set; }
        public Comment? Replied { get; set; }
    }
}
