using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Zust.Domain.Entities
{
    public class Notification:BaseEntity
    {
        [MaxLength(50)]
        public string Title { get; set; }
        public bool IsReaded { get; set; } = false;
        //Relations

        [ForeignKey("Sender")]
        public string SenderId { get; set; }
        [ForeignKey("Receiver")]
        public string ReceiverId { get; set; }

        public AppUser Sender { get; set; }
        public AppUser Receiver { get; set; }
    }
}
