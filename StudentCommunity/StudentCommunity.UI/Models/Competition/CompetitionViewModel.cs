using StudentCommunity.UI.Data;
using System.Collections.ObjectModel;

namespace StudentCommunity.UI.Models.Competition
{
	public class CompetitionViewModel
	{
		public int Id { get; set; }
		public int InitialCompetitionId { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public Collection<CompetitionTeam>? teams { get; set; }
		public ICollection<Questions>? questions { get; set; }
		public List<int> SelectedQuestions { get; set; }
		public DateTime? CompetitionTime { get; set; }
		public bool IsDelete { get; set; }
		public bool IsFinshed { get; set; }
		public bool? ShowResult { get; set; }

	}
}
