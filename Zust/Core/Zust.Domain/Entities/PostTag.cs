namespace Zust.Domain.Entities
{
    public class PostTag:BaseEntity
    {

        //Relations
        public int PostId { get; set; }
        public Post Post { get; set; }

        public string AppUserId { get; set; }
        public AppUser AppUser{ get; set; }
    }
}
