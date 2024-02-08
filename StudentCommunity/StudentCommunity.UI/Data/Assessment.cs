using System.ComponentModel.DataAnnotations.Schema;

namespace StudentCommunity.UI.Data
{
	public class Assessment
	{
        public int Id { get; set; }

        public string  Text { get; set; }

        public string? photoPath { get; set; }

        public DateTime CreateTime { get; set; }
        public string userId { get; set; }

        [ForeignKey("userId")]
        public ApplicationUser User { get; set; }   

    }
}
