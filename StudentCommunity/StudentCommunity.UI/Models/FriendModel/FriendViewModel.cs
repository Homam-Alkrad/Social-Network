using StudentCommunity.UI.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentCommunity.UI.Models.FrindModel
{
    public class FriendViewModel
    {
        public int Id { get; set; }

        public string SenderUserId { get; set; }

        public string ReceiverUserId { get; set; }

        public DateTime RequestDateTime { get; set; }

        public bool Status { get; set; }

        public ApplicationUser SenderUser { get; set; }

        public Major major;
        public ApplicationUser ReceiverUser { get; set; }
    }
}
