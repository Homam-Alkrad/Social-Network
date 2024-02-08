using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing.Printing;

namespace StudentCommunity.UI.Data
{
	public class AnnouncedCompetition
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		public string Name { get; set; }
		public bool IsDeleted { get; set; }
		public DateTime? CompetitionTime { get; set; }
		public int InitialCompetitionId { get; set; }
		[ForeignKey("InitialCompetitionId")]
		public Competition InitialCompetition { get; set; }
		public Collection<CompetitionTeam>? teams { get; set; }
		public ICollection<CompetitionQuestion> CompetitionQuestions { get; set; }
		public bool? ShowResult { get; set; }	
	}
}
