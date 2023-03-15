using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace Zust.Domain.Entities
{
    public class Friend:BaseEntity
    {
        public bool IsConfirmed { get; set; } //If user confirm request add identity user followers and add sender user followings

        //Relations

        [ForeignKey(nameof(Receiver))]
        public string ReceiveId { get; set; }
        [ForeignKey(nameof(Sender))]
        public string SenderId { get; set; }

        public AppUser Receiver { get; set; }
        public AppUser Sender { get; set; }
    }
}
