using StudentCommunity.UI.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentCommunity.UI.Models.Assessment
{
	public class AssessmentModel
	{

		[Required(AllowEmptyStrings = false, ErrorMessage = "Feadback Text Is Required"), MinLength(5),MaxLength(500)]
		public string Text { get; set; }

		public string? photoPath { get; set; }
		public IFormFile FeadbackImg { get; set; }

		public DateTime? CreateTime { get; set; }
		public string? userId { get; set; }

		[ForeignKey("userId")]
		public ApplicationUser? User { get; set; }
	}
}
