using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentCommunity.UI.Data;

namespace StudentCommunity.UI.ViewComponents
{
	public class GroupViewComponent:ViewComponent
	{
		private readonly CommunityContext context;
		private readonly UserManager<ApplicationUser> userManager;

		public GroupViewComponent(CommunityContext context, UserManager<ApplicationUser> userManager)
		{
			this.context = context;
			this.userManager = userManager;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			ApplicationUser user = await userManager.GetUserAsync(HttpContext.User);

			if (user != null)
			{
				var userGroups = await context.UserGroups
					.Include(ug => ug.Group)
					.Where(ug => ug.UserId == user.Id)
					.Select(ug => ug.Group)
					.ToListAsync();

				return View(userGroups);
			}
			return View(Enumerable.Empty<Group>());
		}
	}
}
