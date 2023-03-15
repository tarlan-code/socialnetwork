using System.ComponentModel.DataAnnotations;

namespace Zust.Domain.Entities
{
    public class Gender:BaseEntity
    {
        [MinLength(1),MaxLength(20)]
        public string Name { get; set; }

        //Many Relations
        public ICollection<AppUser>? AppUsers { get; set; }
    }
}
