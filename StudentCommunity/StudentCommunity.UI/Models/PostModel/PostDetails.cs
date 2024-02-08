using StudentCommunity.UI.Data;

namespace StudentCommunity.UI.Models.PostModel
{
    public class PostDetails
    {
        public int Id { get; set; }
        public bool IsLike { get; set; }
        public bool IsComment { get; set; }
        public bool IsDislike { get; set; }
        public string UserId { get; set; }
        public int PostId { get; set; } // Corrected property name
        public ApplicationUser User { get; set; } // Navigation property to the User
        public Post Post { get; set; } // Navigation property to the Post
    }
}
