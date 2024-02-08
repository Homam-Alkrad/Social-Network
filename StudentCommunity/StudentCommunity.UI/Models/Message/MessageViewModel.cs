using StudentCommunity.UI.Data;

namespace StudentCommunity.UI.Models.Message
{
	public class MessageViewModel
	{
		public int MessageId { get; set; }

		public string SenderId { get; set; }
		public string ReceiverId { get; set; }
		public ApplicationUser Sender { get; set; }
		public ApplicationUser Receiver { get; set; }	
		public string Content { get; set; }
		public DateTime Date { get; set; }


	}
}
