using StudentCommunity.UI.Models.AccountModel;
using StudentCommunity.UI.Models.FrindModel;
using StudentCommunity.UI.Models.Message;

namespace StudentCommunity.UI.Models.FriendModel
{
	public class FrindPageViewModel
	{
		public List<FriendViewModel> FrindsView { get; set; }
		public UserViewModel userView { get; set; }
	}
}
