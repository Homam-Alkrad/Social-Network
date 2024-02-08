using Microsoft.AspNetCore.Mvc;
using StudentCommunity.UI.Data;

namespace StudentCommunity.UI.ViewComponents
{
	public class GroupPostViewComponent : ViewComponent
	{
		private readonly CommunityContext context;

		public GroupPostViewComponent(CommunityContext context)
		{
			this.context = context;
		}
		public IViewComponentResult Invoke(int groupId)
		{
			List<Post> posts = new List<Post>();

			foreach (var post in context.Posts.Where(Post => Post.groupId == groupId).ToList())
			{
				Post post1 = new Post
				{
					PostId = post.PostId,
					Likes = post.Likes,
					Comments = post.Comments,
					Dislikes = post.Dislikes,
					Content = post.Content,
					PhotoPath = post.PhotoPath,
					PublicationTime = post.PublicationTime,
					User = context.Users.ToList().Find(x => x.Id == post.UserId),
				};
				posts.Add(post1);
			}
			posts.Reverse();
			return View(posts);
		}
	}
}
