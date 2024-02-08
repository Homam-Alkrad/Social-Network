using System.ComponentModel.DataAnnotations.Schema;

namespace StudentCommunity.UI.Data
{
	public class Advertising
	{
		public int Id { get; set; }
		public string? Title { get; set; }
		public string Text { get; set; }
		public string? ImagePhath { get; set; }
		public string? PhoneNumber { get; set; }
		public bool IsAccept { get; set; }
		public string UserId { get; set; }
		[ForeignKey("UserId")]
		public ApplicationUser User { get; set; }
	}
}
