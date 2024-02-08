using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentCommunity.UI.Data
{
	public class CompetitionTeam
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }	
		public string Name { get; set; }
        public int competationId { get; set; }
		[ForeignKey("competationId")]
        public AnnouncedCompetition competition { get; set; }
        public ICollection<CompetitionMember> students { get; set; }
		public int RoomId { get; set; }

		[ForeignKey("RoomId")]
		public Room Room { get; set; }
	}
}
