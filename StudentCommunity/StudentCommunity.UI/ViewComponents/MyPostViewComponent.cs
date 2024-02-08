using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudentCommunity.UI.Data;
using StudentCommunity.UI.Models.PostModel;

namespace StudentCommunity.UI.ViewComponents
{
	public class MyPostViewComponent:ViewComponent
	{

		private readonly CommunityContext context;

		public MyPostViewComponent(CommunityContext context)
		{
			this.context = context;
		}
		public  IViewComponentResult Invoke(string userId)
		{
			var posts = context.Posts
				   .Where(post => post.UserId == userId)
				   .ToList();
			var processedPosts = new List<PostViewModel>(); 
			foreach (var post in posts)
			{
				var postViewModel = new PostViewModel
				{
					PostId=post.PostId,
					universityId = post.universityId,
					majorId = post.majorId,
					groupId=post.groupId,
					Likes=post.Likes,
					Dislikes = post.Dislikes,
					Content = post.Content,
					PhtotPath=post.PhotoPath,
					PublicationTime = post.PublicationTime,
					IsNews=post.IsNews,
					IsForLanguage=post.IsForLanguage,
					User = context.Users.FirstOrDefault(u => u.Id == post.UserId),
				};
				if(postViewModel.groupId !=null)
				{
					postViewModel.group=context.Groups.Where(gr=>gr.Id==postViewModel.groupId).FirstOrDefault();	
				}

				processedPosts.Add(postViewModel);
			}

			processedPosts.Reverse();

			return View(processedPosts);
		}
	}
}
