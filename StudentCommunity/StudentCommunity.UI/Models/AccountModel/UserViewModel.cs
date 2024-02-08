using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using StudentCommunity.UI.Data;
using StudentCommunity.UI.Models.PostModel;

namespace StudentCommunity.UI.Models.AccountModel
{
    public class UserViewModel
    {
        public string? UserId { get; set; }
        public string? FullName { get; set; }
        public string? ProfileImg { get; set; }
        public IFormFile? ImgFile { get; set; }
        public string? Phone { get; set; }
        public int? UniversityId { get; set; }
        public University university { get; set; }
        public Major major { get; set; }
        public string? Email { get; set; }

        public ICollection<Post> Posts { get; set; }

        public ICollection<PostDetails> PostUserModels { get; set; }
		public string? CurrentRoom { get; internal set; }
	}
}
