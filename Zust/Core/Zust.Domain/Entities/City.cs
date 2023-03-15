using System.ComponentModel.DataAnnotations;

namespace Zust.Domain.Entities
{
    public class City:BaseEntity
    {
        public string Name { get; set; }


        //Relation
        public int CountryId { get; set; }
        public Country Country  { get; set; }
        //Many Relations
        public ICollection<AppUser>? AppUsers { get; set; }
    }
}
