using System.ComponentModel.DataAnnotations.Schema;

namespace StudentCommunity.UI.Data
{
	public class Room
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public ApplicationUser Admin { get; set; }
		public ICollection<Message> Messages { get; set; }
		public CompetitionTeam CompetitionTeam { get; set; }

	}
}
