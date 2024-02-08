using Microsoft.EntityFrameworkCore;
using StudentCommunity.UI.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentCommunity.UI.Data
{
	public class PostsIntractions
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[MaxLength(255)] // Adjust the length as needed
		public int Id { get; set; }

		public bool IsLike { get; set; }
		public bool IsComment { get; set; }
		public bool IsDislike { get; set; }

		public string UserId { get; set; }
		public int PostId { get; set; }

		public ApplicationUser User { get; set; } 
		public Post Post { get; set; }
	}
}
