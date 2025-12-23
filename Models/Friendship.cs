using System.ComponentModel.DataAnnotations.Schema;

namespace projec.Models
{
    public class Friendship
    {
        public int Id { get; set; }

        public int RequesterId { get; set; } // İsteği atan kişi
        
        [ForeignKey("RequesterId")]
        public User Requester { get; set; }

        public int ReceiverId { get; set; } // İsteği alan kişi

        [ForeignKey("ReceiverId")]
        public User Receiver { get; set; }

        public FriendshipStatus Status { get; set; } = FriendshipStatus.Pending;
    }
}
