using Microsoft.AspNetCore.Mvc.ActionConstraints;

namespace StudentCommunity.UI.Models.Groups
{
	public class CreateGroupModel
	{
		public string Name { get; set; }		
		public int typeId { get; set; }
		public int? CollageId { get; set; }
	}
}
