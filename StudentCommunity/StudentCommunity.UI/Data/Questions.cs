using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentCommunity.UI.Data
{
	public enum LevelEnum
	{
		Easy,
		Medium,
		Hard
	}

	public class Questions
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		public string QuestionTitle { get; set; }

		public string QuestionText { get; set; }
		public LevelEnum QuestionLevel { get; set; }
		public int InitialCompetitionId { get; set; }

		[ForeignKey("InitialCompetitionId")]
		public Competition InitialCompetition { get; set; }

		public ICollection<CompetitionQuestion> CompetitionQuestions { get; set; }
	}
}
