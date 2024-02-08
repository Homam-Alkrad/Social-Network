using StudentCommunity.UI.Data;
using StudentCommunity.UI.Models.AccountModel;
using StudentCommunity.UI.Models.Message;

namespace StudentCommunity.UI.Models.PostModel
{
    public class PostUserViewModel
    {
        public UserViewModel? userView { get; set; }
        public PostViewModel? postView { get; set; }
        public string? GroupName { get; set; }
    }
}
