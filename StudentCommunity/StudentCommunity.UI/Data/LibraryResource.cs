using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentCommunity.UI.Data
{
	public class LibraryResource
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }	
        public string Name { get; set; }
        public string FilePath { get; set; }	
        public string PhotoPath { get; set; }	
        public string Description { get; set; }
        public int likes { get; set; }	
        public int Reports { get; set; }
		public string PublisherId { get; set; }
		[ForeignKey("PublisherId")]
		public ApplicationUser Publisher { get; set; }
		public int CategoryId { get; set; }

        public bool IsDelete { get; set; }

		public bool IsAccepted { get; set; }
        public DateTime publishedTime { get; set; }

		public ICollection<ResourcesIntractions> UserIntraction { get; set; }

	}
}
