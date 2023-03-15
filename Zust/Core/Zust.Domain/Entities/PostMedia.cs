using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zust.Domain.Entities
{
    public class PostMedia:BaseEntity
    {
        public string Media { get; set; }
        //Relations
        public int PostId { get; set; }
        public Post Post { get; set; }
    }
}
