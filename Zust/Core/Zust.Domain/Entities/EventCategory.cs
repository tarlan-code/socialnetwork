namespace Zust.Domain.Entities
{
    public class EventCategory:BaseEntity
    {
        public string Name { get; set; }
        public ICollection<Event>? Events{ get; set; }
    }
}
