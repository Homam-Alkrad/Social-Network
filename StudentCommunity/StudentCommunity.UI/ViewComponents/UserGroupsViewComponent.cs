using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentCommunity.UI.Data;

namespace StudentCommunity.UI.ViewComponents
{
	public class UserGroupsViewComponent:ViewComponent
	{
		private readonly CommunityContext context;
		private readonly UserManager<ApplicationUser> userManager;
		ApplicationUser user=new ApplicationUser();
		public UserGroupsViewComponent(CommunityContext context, UserManager<ApplicationUser> userManager)
		{
			this.context = context;
			this.userManager = userManager; 
		}

		public async Task<ApplicationUser> GetUserEmailAsync()
		{
			var user = await userManager.GetUserAsync(HttpContext.User); 
			return user;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			var users = await context.Users.ToListAsync(); 
			return View(users);
		}

	}
}
