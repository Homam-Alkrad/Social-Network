using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StudentCommunity.UI.Data
{
	public class ResourcesIntractions
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[MaxLength(255)] 
		public int Id { get; set; }

		public bool IsLike { get; set; }
		public bool IsReport { get; set; }

		public string UserId { get; set; }
		public int ResourceId { get; set; } 

		public ApplicationUser User { get; set; } 
		public LibraryResource resource { get; set; } 
	}
}
