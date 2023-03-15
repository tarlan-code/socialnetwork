using System.ComponentModel.DataAnnotations;

namespace Zust.Domain.Entities
{
    public class Country:BaseEntity
    {
        [MinLength(2),MaxLength(50)]
        public string Name { get; set; }

        //Many Relations
        public ICollection<AppUser>? AppUsers { get; set; }
        public ICollection<City>? Cities{ get; set; }
    }
}
