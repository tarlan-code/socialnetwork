using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Zust.Domain.Entities
{
    public class AppUser: IdentityUser,IBaseEntity
    {
        [MinLength(3),MaxLength(25)]
        public string Name { get; set; }
        [MinLength(3),MaxLength(30)]
        public string? Surname { get; set; }
        [MaxLength(1000)]
        public string? About { get; set; }
        [MaxLength(10000)]
        public string? EducationOrWorks { get; set; }

        public string ProfilImage { get; set; } = "default.png";
        public string BannerImage { get; set; } = "defaultbanner.png";
        [DataType(DataType.Date)]
        public DateTime Birthday { get; set; }
        public bool IsDeleted { get; set; } = false; //If user delete account IsDelete set true.30 days after if user don't login account delete  
        public DateTime? DeletedDate { get; set; }

        //Relations
        public int? CountryId { get; set; }
        public Country? Country { get; set; }

        public int? CityId { get; set; }
        public City? City { get; set; }

        public int? RelationId { get; set; }
        public Relation? Relation { get; set; }

        public int? GenderId { get; set; }
        public Gender? Gender { get; set; }

        public PrivacySetting PrivacySetting { get; set; }


        //Many Relations

        public ICollection<Post>? Posts { get; set; }
        public ICollection<PostTag>? PostTags { get; set; }
        public ICollection<Comment>? Comments { get; set; }
        public ICollection<PostReaction>? PostReactions { get; set; }
        [InverseProperty("Receiver")]
        public ICollection<Notification>? Notifications { get; set; }

        [InverseProperty("Sender")]
        public ICollection<Message>? SenderMessages{ get; set; }
        [InverseProperty("Receiver")]
        public ICollection<Message>? ReceiverMessages{ get; set; }

        [InverseProperty("Receiver")]
        public ICollection<Friend>? ReceiverFriends{ get; set; }
        [InverseProperty("Sender")]
        public ICollection<Friend>? SenderFriends { get; set; }
        public ICollection<Event>? Events { get; set; }
        public ICollection<EventAttend>? EventAttends { get; set; }
    }
}
