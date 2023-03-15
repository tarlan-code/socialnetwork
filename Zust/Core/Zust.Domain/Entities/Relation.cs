using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zust.Domain.Entities
{
    public class Relation:BaseEntity
    {
        [MinLength(1),MaxLength(20)]
        public string Name { get; set; }

        //Many Relations
        public ICollection<AppUser>? AppUsers { get; set; }
    }
}
