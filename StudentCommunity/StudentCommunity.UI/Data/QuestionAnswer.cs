using System.ComponentModel.DataAnnotations.Schema;

namespace StudentCommunity.UI.Data
{
	public class QuestionAnswer
	{
		public int Id { get; set; }
		public string Answer { get; set; }

		public int QuestionId { get; set; }
		[ForeignKey("QuestionId")]
		public Questions CompetitionQuestion { get; set; }

		public int TeamId { get; set; }
		[ForeignKey("TeamId")]
		public CompetitionTeam Team { get; set; }
		public int? Mark { get; set; }
	}
}
