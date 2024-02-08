using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using StudentCommunity.UI.Data;
using StudentCommunity.UI.Models.AccountModel;
using StudentCommunity.UI.Models.PostModel;
using StudentCommunity.UI.Models.UniversityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace StudentCommunity.UI.Controllers
{
	[Authorize(Roles = "SuperAdmin,Admin")]
	public class AccountController : Controller
	{


		#region ctro
		private readonly UserManager<ApplicationUser> userManager;
		private readonly SignInManager<ApplicationUser> signInManager;
		private readonly CommunityContext context;
		private readonly RoleManager<IdentityRole> roleManager;
		private readonly IHostEnvironment env;
		public AccountController(IHostEnvironment env, UserManager<ApplicationUser> _userManager,
			SignInManager<ApplicationUser> _signInManager, CommunityContext context, RoleManager<IdentityRole> roleManager)
		{
			userManager = _userManager;
			signInManager = _signInManager;
			this.roleManager = roleManager;
			this.env = env;
			this.context = context;
		}
		#endregion

		#region User
		[AllowAnonymous]
		// Renders the welcome view.
		public IActionResult Welcome()
		{
			return View();
		}

		// Renders the registration view.
		[AllowAnonymous]
		[HttpGet]
		public IActionResult Register()
		{
			ViewBag.Universities = context.Universities.OrderBy(u => u.universityName).ToList();
			ViewBag.Colleges = context.Colleges.OrderBy(c => c.Name).ToList();


			if (ViewBag.Universities != null && ViewBag.Colleges != null)
			{
				return View();
			}
			return RedirectToAction("NotFound");

		}

		// Handles user registration.
		[AllowAnonymous]
		[HttpPost]
		public async Task<IActionResult> Register(RegisterViewModel model)
		{

			if (ModelState.IsValid)
			{
				ApplicationUser user = new ApplicationUser
				{
					FullName = model.FirstName + "  " + model.LastName,
					universityId = model.universityId,
					UserName = model.Email,
					Email = model.Email,
					MajorId = model.majorId,
					ProfileImg = "/custom/images/profile.jpg",
					Major = context.Majors.Where(major => major.Id == model.majorId).FirstOrDefault(),
					university = context.Universities.Where(university => university.universityId.ToString() == model.universityId.ToString()).FirstOrDefault(),
				};
				var register = await userManager.CreateAsync(user, model.Password);

				if (register.Succeeded)
				{
					await signInManager.SignInAsync(user, isPersistent: false);
					return RedirectToAction("Login", "Account");
				}
				foreach (var error in register.Errors)
				{
					ModelState.AddModelError(string.Empty, error.Description);
					ViewBag.Errors = error.Description;
				}

			}
			return RedirectToAction("Register", model);
		}

		// Renders the login view.
		[AllowAnonymous]
		[HttpGet]
		public IActionResult Login()
		{
			return View();
		}

		// Handles user login.
		[AllowAnonymous]
		[HttpPost]
		public async Task<IActionResult> Login(LoginViewModel model)
		{
			if (ModelState.IsValid)
			{
				var user = await userManager.FindByEmailAsync(model.Email);
				if (user != null)
				{	
					var CheckUser = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

					if (CheckUser.Succeeded)
					{
						return RedirectToAction("Index", "Community");
					}
				}
				ModelState.AddModelError(string.Empty, "Invalid user Name / Password");

			}
			return View(model);
		}	
		//[AllowAnonymous]
		//[HttpPost]
		//public async Task<IActionResult> Login(LoginViewModel model)
		//{
		//	if (ModelState.IsValid)
		//	{
		//		var user = await userManager.FindByEmailAsync(model.Email);
		//		if (user != null)
		//		{
					
		//			user.university = context.Universities.Where(uni => uni.universityId == user.universityId).FirstOrDefault();

		//			var result = await userManager.UpdateAsync(user);

		//			var CheckUser = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

		//			if (CheckUser.Succeeded)
		//			{
		//				ViewBag.UserUniversity = context.Universities.Where(uni => uni.universityId == user.universityId).FirstOrDefault().universityName;
		//				return RedirectToAction("Index", "Community");
		//			}
		//		}
		//		ModelState.AddModelError(string.Empty, "Invalid user Name / Password");

		//	}
		//	return View(model);
		//}

		// Logs out the current user.
		[AllowAnonymous]
		public async Task<IActionResult> Logout()
		{
			HttpContext.Session.Clear();
			await signInManager.SignOutAsync();
			return RedirectToAction("Login", "Account");
		}

		// Handles the user profile photo editing.
		[AllowAnonymous]
		public async Task<IActionResult> editphoto(UserViewModel model)
		{
			var MyFile = HttpContext.Request.Form.Files;
			foreach (var image in MyFile)
			{
				var file = image;
				if (file.Length > 0)
				{
					var imageName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
					var upload = Path.Combine(env.ContentRootPath, "wwwroot", "Custom", "images", "persons");
					using (var StreamFile = new FileStream(Path.Combine(upload, imageName), FileMode.Create))
					{
						await file.CopyToAsync(StreamFile);
						model.ProfileImg = "/Custom/images/persons/" + imageName;
					}
				}
				ApplicationUser user = await userManager.GetUserAsync(User);
				if (user != null)
				{
					user.ProfileImg = model.ProfileImg;
					await userManager.UpdateAsync(user);
					return RedirectToAction("MyProfile", "Community");
				}

			}
			return RedirectToAction("Inbox", "Community");
		}

		// Renders the access denied view.
		[AllowAnonymous]
		public IActionResult AccessDenied()
		{
			return View();
		}

		// Renders the not found view.
		[AllowAnonymous]
		#endregion
		public IActionResult NotFound()
		{
			return View();
		}

		// Retrieves the current user.
		[HttpGet]
		public async Task<IActionResult> GetCurrentUser()
		{
			if (User.Identity.IsAuthenticated)
			{
				var user = await userManager.GetUserAsync(User);
				if (user != null)
				{
					return Json(new { FullName = user.FullName });
				}
			}
			return Json(new { FullName = "Unknown User" });
		}

		// Retrieves all majors for a college.
		[AllowAnonymous]
		[HttpGet]
		public IActionResult GetAllMajor(int Id)
		{
			var majors = context.Majors.Where(m => m.College.Id == Id).OrderBy(m => m.Name).ToList();
			return Json(majors);
		}

		// Renders the view for creating a role.
		public IActionResult CreateRole()
		{
			return View();
		}

		// Handles the creation of a role.
		[HttpPost]
		public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
		{
			if (ModelState.IsValid)
			{
				var IdentityRole = new IdentityRole
				{
					Name = model.RoleName
				};
				var result = await roleManager.CreateAsync(IdentityRole);

				if (result.Succeeded)
				{
					return RedirectToAction("RoleList", "Account");
				}

				foreach (var error in result.Errors)
				{
					ModelState.AddModelError(string.Empty, error.Description);
				}
				return View(model);



			}
			return View();
		}

		// Renders the role list view.
		[HttpGet]
		public IActionResult RoleList()
		{
			var result = roleManager.Roles;
			return View(result);
		}

		// Renders the view for editing a role.
		[HttpGet]
		public async Task<IActionResult> EditRole(string Id)
		{
			var role = await roleManager.FindByIdAsync(Id);

			if (role == null)
			{
				ViewBag.ErrorMsg("The Role Not Fount");
				return View("Not Fount");
			}
			var model = new EditRoleViewModel
			{
				Id = role.Id,
				RoleName = role.Name
			};

			foreach (var user in userManager.Users)
			{
				if (await userManager.IsInRoleAsync(user, role.Name))
				{
					model.Users.Add(user.UserName);
				}
			}
			return View(model);

		}
		[HttpPost]
		public async Task<IActionResult> EditRole(EditRoleViewModel model)
		{
			var role = await roleManager.FindByIdAsync(model.Id);
			if (model == null)
			{
				ViewBag.ErrorMsg("The Role Not Fount");
				return View("Not Fount");
			}
			else
			{
				role.Name = model.RoleName;

				var result = await roleManager.UpdateAsync(role);

				if (result.Succeeded)
					return RedirectToAction("RoleList");
				return View(model);
			}
		}
		public async Task<IActionResult> AddRemoveRole(string Id)
		{

			var role = await roleManager.FindByIdAsync(Id);
			HttpContext.Session.SetString("RoleID", Id);
			if (role == null)
			{
				RedirectToRoute("RoleList");
			}

			var model = new List<UserRoleViewModel>();

			foreach (var user in userManager.Users)
			{
				var userRoleViewModels = new UserRoleViewModel
				{
					UserID = user.Id,
					UserName = user.UserName
				};
				if (await userManager.IsInRoleAsync(user, role.Name))
				{
					userRoleViewModels.IsSelected = true;
				}
				else
				{
					userRoleViewModels.IsSelected = false;

				}
				model.Add(userRoleViewModels);


			}
			return View(model);
		}
		[HttpPost]
		public async Task<IActionResult> AddRemoveRoleAsync(List<UserRoleViewModel> usersInRole, string RoleID)
		{
			RoleID = HttpContext.Session.GetString("RoleID");
			var role = await roleManager.FindByIdAsync(RoleID);

			if (role == null)
			{
				return View("Not Found");
			}
			for (int i = 0; i < usersInRole.Count; i++)
			{
				var user = await userManager.FindByIdAsync(usersInRole[i].UserID);
				IdentityResult identityResult = null;

				if (usersInRole[i].IsSelected && !(await userManager.IsInRoleAsync(user, role.Name)))
				{
					identityResult = await userManager.AddToRoleAsync(user, role.Name);
				}
				else if (!(usersInRole[i].IsSelected) && await userManager.IsInRoleAsync(user, role.Name))
				{
					identityResult = await userManager.RemoveFromRoleAsync(user, role.Name);
				}
				else
					continue;

			}
			return RedirectToAction("RoleList");
		}

	}
}
