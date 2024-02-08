using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentCommunity.UI.Data
{
	public class FriendRequest
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		public string SenderUserId { get; set; }

		public string ReceiverUserId { get; set; }

		public DateTime RequestDateTime { get; set; }

		public bool Status { get; set; }

		public ApplicationUser SenderUser { get; set; }

		public ApplicationUser ReceiverUser { get; set; }
	}

}
