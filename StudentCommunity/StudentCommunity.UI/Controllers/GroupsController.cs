using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using StudentCommunity.UI.Data;
using StudentCommunity.UI.Models.AccountModel;
using StudentCommunity.UI.Models.Groups;
using StudentCommunity.UI.Models.PostModel;

namespace StudentCommunity.UI.Controllers
{
    public class GroupsController : Controller
	{
		private readonly UserManager<ApplicationUser> userManager;
		private readonly CommunityContext context;
		private readonly RoleManager<IdentityRole> roleManager;

		public GroupsController(UserManager<ApplicationUser> _userManager, CommunityContext context,
			IConfiguration configuration, RoleManager<IdentityRole> roleManager)
		{
			userManager = _userManager;
			this.context = context;
			this.roleManager = roleManager;
			this.context = context;
			this.roleManager = roleManager;
		}

		// Displays the index view for managing groups.
		public async Task<IActionResult> Index()
		{
			ApplicationUser user = await userManager.GetUserAsync(User);
			Major major = context.Majors.Where(mj => mj.Id == user.MajorId).FirstOrDefault();
			CreateGroupModel model = new CreateGroupModel
			{
				CollageId = major.CollegeId
			};
			ViewBag.Categories = context.Colleges.ToList();
			return View(model);
		}

		// Handles the creation of new groups.
		public async Task<IActionResult> CreateGroups(CreateGroupModel model)
		{
			ApplicationUser user=await userManager.GetUserAsync(User);
			if(ModelState.IsValid)
			{
				Group group = new Group
				{
					Name = model.Name,
					typeId=model.typeId
				};
				context.Groups.Add(group);
				context.SaveChanges();
				if (user != null)
				{
					UserGroup userGroup = new UserGroup
					{
						GroupId = group.Id,
						UserId = user.Id
					};
					context.UserGroups.Add(userGroup);
				}
				context.SaveChanges();
				return RedirectToAction("Index");
			}
			return View();
		}

		// Displays details about a specific group.
		public async Task<IActionResult> Group(int groupId)
		{
			var user = await userManager.GetUserAsync(User);

			if (groupId == 0)
			{
				groupId=int.Parse(HttpContext.Session.GetString("groupId"));
			}
			UserGroup group=context.UserGroups.Where(uc=>uc.UserId==user.Id && uc.GroupId==groupId).FirstOrDefault();
			if(group == null)
			{
				return RedirectToAction("AccessDenied","Account");
			}
			var PostModel = new PostViewModel();
			PostModel.groupId=groupId;
			var UserModel = new UserViewModel();
			UserModel = new UserViewModel
			{
				UserId = user.Id,
				Email = user.Email,
				FullName = user.FullName,
				Phone = user.PhoneNumber,
				UniversityId = user.universityId,
				major = context.Majors.Where(Major => Major.Id == user.MajorId).FirstOrDefault(),
				university = context.Universities.Where(University => University.universityId == user.universityId).FirstOrDefault(),
				ProfileImg = user.ProfileImg
			};

			var viewModel = new PostUserViewModel
			{
				userView = UserModel,
				postView = PostModel,
				GroupName=context.Groups.Where(x=>x.Id==groupId).FirstOrDefault().Name
			};
			return View(viewModel);
		}
		// Handles the process of joining a group.
		public async Task<IActionResult> JoinGroup(int groupId)
        {
            var user = await userManager.GetUserAsync(User);
            bool isMember = context.UserGroups.Any(ug => ug.UserId == user.Id && ug.GroupId == groupId);
            if (!isMember)
            {
                UserGroup userGroup = new UserGroup
                {
                    GroupId = groupId,
                    UserId = user.Id
                };
                context.UserGroups.Add(userGroup);
                context.SaveChanges();
                return RedirectToAction("Group", new { groupId = groupId });
            }
            return RedirectToAction("Group", new { groupId = groupId });
        }

    }
}
