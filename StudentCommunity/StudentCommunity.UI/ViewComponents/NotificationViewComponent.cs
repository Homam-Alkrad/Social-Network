using Microsoft.AspNetCore.Mvc;
using StudentCommunity.UI.Data;

namespace StudentCommunity.UI.ViewComponents
{
	public class NotificationViewComponent : ViewComponent
	{
		private readonly CommunityContext context;

		public NotificationViewComponent(CommunityContext context)
		{
			this.context = context;
		}
		public IViewComponentResult Invoke(string userId)
		{
			return View(context.Notifications.Where(x => x.StdId == userId).ToList().Take(15)); ;
		}
	}
}
