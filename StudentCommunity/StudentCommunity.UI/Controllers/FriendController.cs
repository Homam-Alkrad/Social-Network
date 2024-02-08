using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.ML;
using StudentCommunity.UI.Data;
using StudentCommunity.UI.Models.AccountModel;
using StudentCommunity.UI.Models.FriendModel;
using StudentCommunity.UI.Models.FrindModel;

namespace StudentCommunity.UI.Controllers
{
	[Authorize]
	public class FriendController : Controller
	{
		private readonly CommunityContext context;
		private readonly UserManager<ApplicationUser> _userManager;
		public FriendController(CommunityContext context, UserManager<ApplicationUser> _userManager)
		{
			this.context = context;
			this._userManager = _userManager;
		}
		// Action method to display the index view.
		public async Task<IActionResult> Index()
		{
			ApplicationUser user = await _userManager.GetUserAsync(User);
			List<FriendRequest> friends = context.FriendRequests.ToList();
			List<FriendViewModel> model = new List<FriendViewModel>();
			foreach (FriendRequest friend in friends.Where(fr => fr.ReceiverUserId == user.Id).ToList())
			{
				FriendViewModel obj = new FriendViewModel
				{
					Id = friend.Id,
					SenderUserId = friend.SenderUserId,
					ReceiverUserId = friend.ReceiverUserId,
					Status = friend.Status,
					RequestDateTime = friend.RequestDateTime,
					SenderUser = context.Users.Where(x => x.Id == friend.SenderUserId).FirstOrDefault(),
					major = context.Majors.Where(m => m.Id == context.Users.Where(x => x.Id == friend.SenderUserId).FirstOrDefault().MajorId).FirstOrDefault(),
					ReceiverUser = user
				};
				model.Add(obj);
			}
			var UserModel = new UserViewModel();

			UserModel = new UserViewModel
			{
				UserId = user.Id,
				Email = user.Email,
				FullName = user.FullName,
				major = context.Majors.Where(Major => Major.Id == user.MajorId).FirstOrDefault(),
				university = context.Universities.Where(University => University.universityId == user.universityId).FirstOrDefault(),
				ProfileImg = user.ProfileImg
			};
			FrindPageViewModel Model = new FrindPageViewModel
			{
				FrindsView = model,
				userView = UserModel
			};
			return View(Model);
		}

		// Action method to add a friend.
		[HttpPost]
		public async Task<IActionResult> AddFriend(string userId)
		{
			if (string.IsNullOrEmpty(userId))
			{
				return Json(new { success = false, message = "User ID is required." });
			}

			ApplicationUser currentUser = await _userManager.GetUserAsync(User);

			if (currentUser == null)
			{
				return Json(new { success = false, message = "User not found." });
			}

			bool friendRequestExists = context.FriendRequests
				.Any(fr => fr.SenderUserId == currentUser.Id && fr.ReceiverUserId == userId);

			if (!friendRequestExists)
			{
				FriendRequest friendRequest = new FriendRequest
				{
					SenderUserId = currentUser.Id,
					ReceiverUserId = userId,
					RequestDateTime = DateTime.Now,
					Status = false
				};

				context.FriendRequests.Add(friendRequest);
				await context.SaveChangesAsync();

				return Json(new { success = true, message = "Friend request sent successfully." });
			}
			else
			{
				return Json(new { success = false, message = "Friend request already sent." });
			}
		}
		// Action method to handle friend request actions (accept/decline).
		[HttpPost]
		public async Task<IActionResult> FriendRequestAction(string userId, string action)
		{
			ApplicationUser currentUser = await _userManager.GetUserAsync(User);

			if (currentUser == null)
			{
				return Json(new { success = false, message = "User not found." });
			}

			FriendRequest friendRequest = context.FriendRequests
				.FirstOrDefault(fr => fr.SenderUserId == userId && fr.ReceiverUserId == currentUser.Id);

			if (friendRequest == null)
			{
				return Json(new { success = false, message = "Friend request not found." });
			}

			if (action.ToLower() == "accept")
			{
				friendRequest.Status = true;
				await context.SaveChangesAsync();

				return Json(new { success = true, message = "Friend request accepted successfully." });
			}
			else if (action.ToLower() == "decline")
			{
				context.FriendRequests.Remove(friendRequest);

				await context.SaveChangesAsync();

				return Json(new { success = true, message = "Friend request declined successfully." });
			}
			else
			{
				return Json(new { success = false, message = "Invalid action." });
			}
		}

	}
}
