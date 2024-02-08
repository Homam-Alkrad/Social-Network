using StudentCommunity.UI.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentCommunity.UI.Models.Competition
{
	public class CreateQuestionViewModel
	{
		public int Id { get; set; }

		public string QuestionTitle { get; set; }

		public string QuestionText { get; set; }

		public LevelEnum QuestionLevel { get; set; }

		public int CompetitionId { get; set; }
		[ForeignKey("Id")]

		public QuestionAnswer Answer { get; set; }
	}
}
