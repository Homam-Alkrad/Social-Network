using StudentCommunity.UI.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentCommunity.UI.Models.Competition
{
	public class ExamViewModel
	{
		public List<string>? Answer { get; set; }=new List<string>();
		public Data.AnnouncedCompetition competition { get; set; }
		public	CompetitionTeam team { get; set; }
		public string TeamLeaderId { get; set; }
		public ICollection<QuestionAnswerTeamModel> questions { get; set; }=new List<QuestionAnswerTeamModel>();
		
	}
}
