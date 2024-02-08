using StudentCommunity.UI.Data;

namespace StudentCommunity.UI.Models.Competition
{
	public class TeamsViewModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public List<ApplicationUser> students { get; set; }

		public bool IsUserInThisTeam { get; set; }
	}
}
