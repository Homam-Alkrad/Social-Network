using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudentCommunity.UI.Data;
using StudentCommunity.UI.Models.AccountModel;
using StudentCommunity.UI.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;

namespace StudentCommunity.UI.Controllers
{
	[Authorize]
	public class ChatSystemController : Controller
	{
		#region
		private readonly ILogger<HomeController> _logger;
		private readonly UserManager<ApplicationUser> userManager;
		private readonly CommunityContext context;

		public ChatSystemController(ILogger<HomeController> logger, UserManager<ApplicationUser> userManager, CommunityContext context)
		{
			this._logger = logger;
			this.userManager = userManager;
			this.context = context;
		}

		#endregion
		// Action method to display the chat view.
		public async Task<IActionResult> ChatAsync()
		{
			if (User.Identity.IsAuthenticated)
			{
				var user = await userManager.GetUserAsync(User);

				if (user != null)
				{
					var UserModel = new UserViewModel();

					UserModel = new UserViewModel
					{
						UserId = user.Id,
						Email = user.Email,
						FullName = user.FullName,
						major = context.Majors.FirstOrDefault(Major => Major.Id == user.MajorId),
						university = context.Universities.FirstOrDefault(University => University.universityId == user.universityId),
						ProfileImg = user.ProfileImg
					};

					return View(UserModel);
				}
			}

			return RedirectToAction("Login");
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
