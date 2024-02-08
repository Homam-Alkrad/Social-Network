using StudentCommunity.UI.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentCommunity.UI.Models.Competition
{
	public class QuestionsViewModel
	{
		public int Id { get; set; }

		public string QuestionTitle { get; set; }

		public string QuestionText { get; set; }

		public LevelEnum QuestionLevel { get; set; }

		public int InitialCompetitionId { get; set; }

		public Data.Competition InitialCompetition { get; set; }

		public bool IsSelected { get; set; }
	}
}
