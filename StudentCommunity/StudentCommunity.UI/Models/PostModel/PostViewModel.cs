using System;
using System.Collections.Generic;
using StudentCommunity.UI.Data;

namespace StudentCommunity.UI.Models.PostModel
{
    public class PostViewModel
    {
        public int PostId { get; set; }
        public string? Type { get; set; }
        public string? UserId { get; set; }
        public string? Content { get; set; }
        public IFormFile? PhotoFile { get; set; }
        public string? PhtotPath { get; set; }
        public DateTime PublicationTime { get; set; }
        public ApplicationUser User { get; set; }
        public PostDetails details { get; set; }
        public string returnUrl { get; set; }
        public bool? IsNews { get; set; }
        public bool? IsForLanguage { get; set; }
        public int?Likes { get; set; }
        public int? Dislikes { get; set; }
		public int? majorId { get; set; }
		public int? universityId { get; set; }
        public int? groupId { get; set;}
        public Group? group { get; set;}
	}
}
