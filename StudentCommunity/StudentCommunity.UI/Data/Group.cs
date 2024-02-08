namespace StudentCommunity.UI.Data
{
	public class Group
	{
        public int Id { get; set; }
        public string Name { get; set; }
        public int typeId { get; set; }
		public virtual ICollection<UserGroup> Users { get; set; }
	}
}
