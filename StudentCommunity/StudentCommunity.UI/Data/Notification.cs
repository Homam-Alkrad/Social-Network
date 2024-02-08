using System.ComponentModel.DataAnnotations.Schema;

namespace StudentCommunity.UI.Data
{
	public class Notification
	{
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime dateTime { get; set; }  
        public string StdId {  get; set; }
        [ForeignKey("StdId")]
        public ApplicationUser User { get; set; }

    }
}
