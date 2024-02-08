using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentCommunity.UI.Data
{
	public class RecommendedToYou
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		public string Name { get; set; }
		public string PhotoPath { get; set; }
		public string Description { get; set; }

        public string url { get; set; }
        public int CategoryId { get; set; }
		[ForeignKey("CategoryId")]
		[Required]
		public Major category { get; set; }
		public DateTime publishedTime { get; set; }
	}
}
