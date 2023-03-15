using System.ComponentModel.DataAnnotations;

namespace Zust.Domain.Entities
{
    public class Event:BaseEntity
    {
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        [MaxLength(20)]
        public string Name { get; set; }
        [MaxLength(40)]
        public string Location { get; set; }
        [DataType(DataType.Date)]
        public DateTime EventDate { get; set; }
        public string Image { get; set; }
        public int EventCategoryId { get; set; }
        public EventCategory EventCategory { get; set; }
        public ICollection<EventAttend>? EventAttends { get; set; }
    }
}
