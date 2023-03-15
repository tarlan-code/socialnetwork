using System.ComponentModel.DataAnnotations.Schema;

namespace Zust.Domain.Entities
{
    public class Message:BaseEntity
    {
        public string? Content { get; set; }
        public string? Media { get; set; }


        //Relations

        [ForeignKey("Sender")]
        public string SenderId { get; set; }
        [ForeignKey("Receiver")]
        public string ReceiverId { get; set; }

        public AppUser Sender { get; set; }
        public AppUser Receiver { get; set; }

    }
}
