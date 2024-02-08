using StudentCommunity.UI.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentCommunity.UI.Models.Competition
{
	public class CreateQuestionModel
	{
		[Required(AllowEmptyStrings = false, ErrorMessage = " Question Title Is Required")]
		public string QuestionTitle { get; set; }
		[Required(AllowEmptyStrings = false, ErrorMessage = " Question Text Is Required")]
		public string QuestionText { get; set; }
		[Required(AllowEmptyStrings = false, ErrorMessage = " Question Level Is Required")]

		public LevelEnum QuestionLevel { get; set; }
		[Required]
		public int InitialCompetitionId { get; set; }
	}
}
