using StudentCommunity.UI.Data;

namespace StudentCommunity.UI.Models.Competition
{
	public enum LevelEnum
	{
		Easy,
		Medium,
		Hard
	}
	public class EvaluateViewMode
	{
		public int Id { get; set; }
		public int TeamId { get; set; }
		public CompetitionTeam Team { get; set; }
		public string answer {  get; set; } 
		public Questions question {  get; set; } 
		public int? mark { get; set; }
	}
}
