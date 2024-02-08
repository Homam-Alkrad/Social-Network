using System.ComponentModel.DataAnnotations.Schema;

namespace StudentCommunity.UI.Models.Competition
{
	public class QuestionAnswerTeamModel
	{
		public int Id { get; set; }

		public string QuestionTitle { get; set; }

		public string QuestionText { get; set; }

		public LevelEnum QuestionLevel { get; set; }

		public string answer { get; set; }
	}
}
