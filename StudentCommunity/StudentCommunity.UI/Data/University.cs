namespace StudentCommunity.UI.Data
{
	public class University
	{
        public int universityId { get; set; }
        public string universityName { get; set; }
        public ICollection<ApplicationUser> users { get; set; }

    }
}
