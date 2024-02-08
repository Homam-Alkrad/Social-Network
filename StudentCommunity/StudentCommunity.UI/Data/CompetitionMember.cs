using System.ComponentModel.DataAnnotations.Schema;

namespace StudentCommunity.UI.Data
{
	public class CompetitionMember
	{

        public int  Id {  get; set; }
        public string MemberId { get; set; }
        [ForeignKey("MemberId")]
        public ApplicationUser Member { get; set; }
        public int TeamId { get; set; }
        [ForeignKey("TeamId")]
        public CompetitionTeam Team { get; set; }
        public int?  CompetitionId { get; set; }
        public bool? IsLeader { get; set; }
    }
}
