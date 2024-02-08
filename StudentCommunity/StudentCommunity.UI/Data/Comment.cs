using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentCommunity.UI.Data
{
	public class Comment
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int CommentId { get; set; }
		[Required]
		public string Content { get; set; }

		public DateTime CommentTime { get; set; }

		public string UserId { get; set; }

		[ForeignKey("UserId")]
		public ApplicationUser User { get; set; }

		public int PostId { get; set; }

		[ForeignKey("PostId")]
		public Post Post { get; set; }
	}
}
