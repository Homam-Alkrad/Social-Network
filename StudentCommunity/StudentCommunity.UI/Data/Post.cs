using StudentCommunity.UI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentCommunity.UI.Data
{
	public class Post
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		//[MaxLength(255)] // Adjust the length as needed
		public int PostId { get; set; }
		public int Likes { get; set; }
		public int Comments { get; set; }
		public int Dislikes { get; set; }
		public string? Content { get; set; }
		public string? PhotoPath { get; set; }
		public int? majorId { get; set; }
		public int? universityId { get; set; }
		public int? groupId { get; set; }
		public bool? IsNews {get;set;}
		public bool? IsForLanguage {get;set;}
		public DateTime PublicationTime { get; set; }
		public string? UserId { get; set; }

		[ForeignKey("UserId")]
		public ApplicationUser User { get; set; }
		public ICollection<PostsIntractions> UserPost { get; set; }
		public ICollection<Comment> Commentlst { get; set; }
	}
}
