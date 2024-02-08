using Microsoft.AspNetCore.Mvc;
using StudentCommunity.UI.Data;
using StudentCommunity.UI.Models.PostModel;

namespace StudentCommunity.UI.ViewComponents
{
	public class LanguagePostViewComponent : ViewComponent
	{
		private readonly CommunityContext context;

		public LanguagePostViewComponent(CommunityContext context)
		{
			this.context = context;
		}
		public IViewComponentResult Invoke()
		{
			List<Post> posts = new List<Post>();

			foreach (var post in (context.Posts.Where(post => post.IsForLanguage == true)).ToList())
			{
				Post post1 = new Post
				{
					PostId = post.PostId,
					Likes = post.Likes,
					Comments = post.Comments,
					Dislikes = post.Dislikes,
					Content = post.Content,
					majorId = post.majorId,
					PhotoPath = post.PhotoPath,
					PublicationTime = post.PublicationTime,
					User = context.Users.ToList().Find(x => x.Id == post.UserId),
				};
				if (post1.User != null)
				{
					post1.User.Major = context.Majors.Where(x => x.Id == post1.User.MajorId).FirstOrDefault();
					post1.User.university = context.Universities.Where(x => x.universityId == post1.User.universityId).FirstOrDefault();
				}
				posts.Add(post1);
			}
			posts.Reverse();
			return View(posts);
		}
	}
}
