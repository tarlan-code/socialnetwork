namespace Zust.Domain.Entities
{
    public class PostReaction:BaseEntity
    {
        //Relations
        public int PostId { get; set; }
        public Post Post { get; set; }
        public int ReactionId { get; set; }
        public Reaction Reaction { get; set; }
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}
