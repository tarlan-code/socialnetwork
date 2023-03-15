using System.ComponentModel.DataAnnotations;

namespace Zust.Domain.Entities
{
    public class Post:BaseEntity
    {
        [MaxLength(300)]
        public string? Content { get; set; }
        [Range(0,int.MaxValue)]
        public int ShareCount { get; set; }

        //Relations
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }


        //Many Relations
        public ICollection<PostReaction>? PostReactions { get; set; }
        public ICollection<PostMedia>? PostMedias{ get; set; }
        public ICollection<PostTag>? PostTags { get; set; }
        public ICollection<Comment>? Comments { get; set; }
    }
}
