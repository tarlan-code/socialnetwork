using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zust.Domain.Entities
{
    public class Reaction:BaseEntity
    {
        public string IconName { get; set; }

       //Many Relations
        public ICollection<PostReaction>? PostReactions { get; set; }
    }
}
