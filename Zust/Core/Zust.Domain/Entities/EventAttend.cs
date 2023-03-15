namespace Zust.Domain.Entities
{
    public class EventAttend:BaseEntity
    {
        public AppUser AppUser { get; set; }
        public string AppUserId { get; set; }
        public Event? Event { get; set; }
        public int? EventId { get; set; }
    }
}
