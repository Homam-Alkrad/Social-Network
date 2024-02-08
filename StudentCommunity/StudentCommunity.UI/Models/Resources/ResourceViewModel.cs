using StudentCommunity.UI.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentCommunity.UI.Models.Resources
{
	public class ResourceViewModel
	{
		public int Id { get; set; }
		public string Name { get; set; }

		public string FilePath { get; set; }
		public string PhotoPath { get; set; }
		public string Description { get; set; }
		public int likes { get; set; }
		public int Reports { get; set; }
		public string PublisherId { get; set; }
		public ApplicationUser Publisher { get; set; }
		public int typeId { get; set; }
		public College type { get; set; }
		public DateTime publishedTime { get; set; }
        public bool IsDelete { get; set; }
        public ICollection<ResourcesIntractions> UserIntraction { get; set; }
	}
}
