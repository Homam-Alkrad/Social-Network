namespace StudentCommunity.UI.Models.Competition
{
	public class QuestionsCompetitionModel
	{
		public	List<QuestionsViewModel> Questions { get; set; }=new List<QuestionsViewModel>();
		public int competitionId {  get; set; }
	}
}
