using Microsoft.AspNetCore.Mvc;
using StudentCommunity.UI.Data;

namespace StudentCommunity.UI.ViewComponents
{
	public class UserViewComponent:ViewComponent
	{
		private readonly CommunityContext context;

		public UserViewComponent(CommunityContext context)
		{
			this.context = context;	
		}
		public IViewComponentResult Invoke(string Id)
		{
			return View(context.Users.ToList());
		}
	}
}
