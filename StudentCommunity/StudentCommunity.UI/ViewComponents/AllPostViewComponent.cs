using Microsoft.AspNetCore.Mvc;
using StudentCommunity.UI.Data;

namespace StudentCommunity.UI.ViewComponents
{
	public class AllPostViewComponent : ViewComponent
	{
		private readonly CommunityContext context;

		public AllPostViewComponent(CommunityContext context)
		{
			this.context = context;
		}
		public IViewComponentResult Invoke(int numberOfPosts)
		{
			List<Post> posts = new List<Post>();

			foreach (var post in context.Posts.Where(Post=>Post.majorId==null && Post.universityId==null
			&& Post.groupId==null && Post.IsNews==null &&Post.IsForLanguage==null).ToList().Take(numberOfPosts))
			{
				Post post1 = new Post
				{
					PostId=post.PostId,
					Likes = post.Likes,
					Comments=post.Comments,
					Dislikes = post.Dislikes,
					Content = post.Content,
					PhotoPath =post.PhotoPath,
					PublicationTime = post.PublicationTime,
					IsNews=post.IsNews,
					IsForLanguage=post.IsForLanguage,
					User = context.Users.ToList().Find(x => x.Id == post.UserId),
				};
				post1.User.university = context.Universities.Where(x => x.universityId == post1.User.universityId).FirstOrDefault();
				posts.Add(post1);
			}
			posts.Reverse();
			return View(posts);
		}


	}
}
