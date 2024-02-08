using Microsoft.AspNetCore.Identity;
using StudentCommunity.UI.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentCommunity.UI.Data
{
	public class ApplicationUser : IdentityUser
	{
		public string FullName { get; set; }    
		public string? ProfileImg { get; set; }
		public int universityId { get; set; }
		[ForeignKey("universityId")]
		public University university { get; set; }
		public int MajorId { get; set; }
		[ForeignKey("MajorId")]
		public Major Major { get; set; }
		public ICollection<Advertising> advertisings { get; set; }
		public ICollection<Post> Posts { get; set; }
		public ICollection<PostsIntractions> PostUser { get; set; }
		public ICollection<Room> Rooms { get; set; }
		public ICollection<Message> Messages { get; set; }
		public ICollection<LibraryResource> resources { get; set; }
		public ICollection<FriendRequest> SentFriendRequests { get; set; }
		public ICollection<FriendRequest> ReceivedFriendRequests { get; set; }
		public ICollection<UserGroup> Groups { get; set; }
		public ICollection<Assessment> feadbacks { get; set; }
	}
}
