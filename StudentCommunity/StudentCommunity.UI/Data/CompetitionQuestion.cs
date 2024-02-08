using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentCommunity.UI.Data
{
	public class CompetitionQuestion
	{
		[Key]
		[Column(Order = 0)]
		[ForeignKey("Competition")]
		public int CompetitionId { get; set; }

		[Key]
		[Column(Order = 1)]
		[ForeignKey("Question")]
		public int QuestionId { get; set; }

		public AnnouncedCompetition Competition { get; set; }
		public Questions Question { get; set; }

	}
}
