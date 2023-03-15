namespace Zust.Domain.Entities
{
    public class PrivacySetting : BaseEntity
    {
        public bool? WhoCanSeeYourProfile { get; set; } = true;
        public bool? WhoCanSendFollow { get; set; } = true;
        public bool? WhoCanSeeYourPhone { get; set; } = true;
        public bool? WhoCanSeeMyBirthday { get; set; } = true;
      
        //Relations
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }



    }
}
