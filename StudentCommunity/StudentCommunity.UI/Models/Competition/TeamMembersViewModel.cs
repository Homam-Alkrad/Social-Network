using StudentCommunity.UI.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentCommunity.UI.Models.Competition
{
	public class TeamMembersViewModel
	{
		public int Id { get; set; }
		public ApplicationUser Student { get; set; }
		public int TeamId { get; set; }
		[ForeignKey("TeamId")]
		public CompetitionTeam Team { get; set; }
	}
}
