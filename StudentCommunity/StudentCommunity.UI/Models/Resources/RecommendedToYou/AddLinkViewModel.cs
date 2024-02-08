using StudentCommunity.UI.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentCommunity.UI.Models.Resources.RecommendedToYou
{
	public class AddLinkViewModel
	{
		public string Name { get; set; }
		public IFormFile PhotoFile { get; set; }
		public string? PhotoPath { get; set; }
		public string Description { get; set; }
		public string url { get; set; }
		public int CategoryId { get; set; }
		public DateTime publishedTime { get; set; }
	}
}
