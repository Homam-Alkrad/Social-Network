using Microsoft.AspNetCore.Mvc;
using StudentCommunity.UI.Data;

namespace StudentCommunity.UI.ViewComponents
{
	public class CommentViewComponent : ViewComponent
	{
		private readonly CommunityContext context;

		public CommentViewComponent(CommunityContext context)
		{
			this.context = context;
		}
		public IViewComponentResult Invoke(int Id)
		{
			List<Comment> comments = new List<Comment>();

			foreach (var _comment in context.Comments.Where(comment => comment.PostId == Id).ToList().Take(50))
			{
				Comment comment= new Comment
				{
					CommentId = _comment.CommentId,
					Content= _comment.Content,
					CommentTime = _comment.CommentTime,
					User=context.Users.Where(user=>user.Id==_comment.UserId).FirstOrDefault(),
					UserId=_comment.UserId,
				};
				comments.Add(comment);
			}
			comments.Reverse();
			return View(comments);
		}

	}
}
