using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentCommunity.UI.Data;
using StudentCommunity.UI.Models.Resources;
using StudentCommunity.UI.Models.Resources.RecommendedToYou;

namespace StudentCommunity.UI.Controllers
{
	[Authorize]
	public class LibraryController : Controller
	{
		#region ctro
		private readonly CommunityContext context;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly IHostEnvironment env;
		public LibraryController(CommunityContext context, UserManager<ApplicationUser> _userManager, IHostEnvironment env)
		{
			this.context = context;
			this._userManager = _userManager;
			this.env = env;
		}
		#endregion
		// Displays the index view for managing library resources.
		public async Task<IActionResult> Index()
		{
			ApplicationUser user = await _userManager.GetUserAsync(User);
			var resources = await GetResourcesAsync(user, "asc"); 
			ViewBag.Categories = context.Majors.ToList();
			return View(resources);
		}

		// Retrieves data for the index view using AJAX.
		public async Task<IActionResult> retrivedata(string sortOrder)
		{
			try
			{
				ApplicationUser user = await _userManager.GetUserAsync(User);
				var resources = await GetResourcesAsync(user, sortOrder);
				ViewBag.Categories = context.Majors.ToList();
				return PartialView("_PostsPartial", resources);
			}
			catch (Exception ex)
			{
				Console.Error.WriteLine($"Exception in retrivedata action: {ex.Message}");
				return StatusCode(500, "Internal Server Error");
			}
		}

		// Retrieves resources asynchronously based on user and sorting order.
		private async Task<List<ResourceViewModel>> GetResourcesAsync(ApplicationUser user, string sortOrder)
		{
			user.Major = await context.Majors.FirstOrDefaultAsync(x => x.Id == user.MajorId);

			var libraryResourcesQuery = context.LibraryResources
				.Where(x => x.CategoryId == user.Major.CollegeId && x.IsAccepted == true);

			switch (sortOrder)
			{
				case "asc":
					libraryResourcesQuery = libraryResourcesQuery.OrderBy(x => x.Name);
					break;
				case "desc":
					libraryResourcesQuery = libraryResourcesQuery.OrderByDescending(x => x.Name);
					break;
				default:
					libraryResourcesQuery = libraryResourcesQuery.OrderBy(x => x.Name);
					break;
			}

			var libraryResources = await libraryResourcesQuery.ToListAsync();

			var resources = libraryResources.Select(item => new ResourceViewModel
			{
				Id = item.Id,
				Name = item.Name,
				Description = item.Description,
				FilePath = item.FilePath,
				PhotoPath = item.PhotoPath,
				likes = item.likes,
				Reports = item.Reports,
				publishedTime = item.publishedTime,
				Publisher = context.Users.FirstOrDefault(x => x.Id == item.PublisherId),
				type = context.Colleges.FirstOrDefault(x => x.Id == item.CategoryId),
				IsDelete = item.IsDelete,
			}).ToList();

			return resources;
		}

		// Displays recommended resources for the user.
		public async Task<IActionResult> RecommendedToYou()
		{
			ApplicationUser user = await _userManager.GetUserAsync(User);
			user.Major = context.Majors.Where(x => x.Id == user.MajorId).FirstOrDefault();
			List<RecommendedToYou> linkes = context.RecommendedToYou.Where(rq => rq.CategoryId == user.Major.CollegeId).ToList();
			return View(linkes);
		}

		// Displays the form for adding a website.
		public IActionResult AddWebsite()
		{
			ViewBag.Categories = context.Colleges.ToList();

			return View();
		}

		// Handles the process of adding a website.
		[HttpPost]
		public async Task<IActionResult> AddWebsite(AddLinkViewModel model)
		{
			if (ModelState.IsValid)
			{
				var MyFile = HttpContext.Request.Form.Files;
				foreach (var image in MyFile)
				{
					var file = image;
					if (file.Length > 0)
					{
						var imageName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
						var upload = Path.Combine(env.ContentRootPath, "wwwroot", "Custom", "images", "websites");
						using (var StreamFile = new FileStream(Path.Combine(upload, imageName), FileMode.Create))
						{
							await file.CopyToAsync(StreamFile);
							model.PhotoPath = "/Custom/images/websites/" + imageName;
						}
					}
				}
				RecommendedToYou link = new RecommendedToYou
				{
					Name = model.Name,
					url = model.url,
					Description = model.Description,
					CategoryId = model.CategoryId,
					PhotoPath = model.PhotoPath,
					publishedTime = model.publishedTime
				};
				context.RecommendedToYou.Add(link);
				context.SaveChanges();
				return RedirectToAction("RecommendedToYou");
			}
			return View(model);
		}

		// Displays the form for creating a resource.
		[HttpGet]
		public IActionResult Create()
		{
			ViewBag.Categories = context.Colleges.ToList();
			return View();
		}

		// Handles the process of creating a resource.
		[HttpPost]
		public async Task<IActionResult> Create(CreateResourceViewModel model)
		{
			ApplicationUser user = await _userManager.GetUserAsync(User);
			var MyFile = HttpContext.Request.Form.Files;

			foreach (var image in MyFile)
			{
				var file = image;
				if (file.Length > 0)
				{
					var imageName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
					string upload;

					if (image.Name == "File")
					{
						upload = Path.Combine(env.ContentRootPath, "wwwroot", "Custom", "images", "files");
					}
					else
					{
						upload = Path.Combine(env.ContentRootPath, "wwwroot", "Custom", "images", "books");
					}

					using (var StreamFile = new FileStream(Path.Combine(upload, imageName), FileMode.Create))
					{
						await file.CopyToAsync(StreamFile);

						if (image.Name == "File")
						{
							model.FilePath = "/Custom/images/files/" + imageName;
						}
						else
						{
							model.ImgPath = "/Custom/images/books/" + imageName;
						}
					}

				}
			}
			LibraryResource resource = new LibraryResource
			{
				FilePath = model.FilePath,
				PhotoPath = model.ImgPath,
				Description = model.Description,
				CategoryId = model.typeId,
				Name = model.Name,
				publishedTime = DateTime.Now,
				PublisherId = user.Id,
				IsAccepted = false,
			};

			context.LibraryResources.Add(resource);
			context.SaveChanges();
			TempData["CreateMessage"] = "Please Wait Until The Admin Accepts Your Request";
			return RedirectToAction("Index");
		}

		// Handles the process of liking a resource.
		[HttpPost]
		public async Task<IActionResult> Like(int Id)
		{
			var user = await _userManager.GetUserAsync(User);

			if (user == null)
			{
				return BadRequest("User not found.");
			}

			var hasLiked = context.Intractions
				.FirstOrDefault(x => x.UserId == user.Id && x.ResourceId == Id && x.IsLike);

			LibraryResource resource = context.LibraryResources.Find(Id);

			if (hasLiked != null)
			{
				context.Intractions.Remove(hasLiked);
				resource.likes--;
			}
			else
			{
				var newLike = new ResourcesIntractions
				{
					UserId = user.Id,
					ResourceId = Id,
					IsLike = true,
					IsReport = false
				};

				context.Intractions.Add(newLike);
				resource.likes++;
			}

			context.LibraryResources.Update(resource);
			context.SaveChanges();

			return Json(new { likes = resource.likes });
		}

		// Handles the process of reporting a resource.
		[HttpPost]
		public async Task<IActionResult> Report(int Id)
		{
			var user = await _userManager.GetUserAsync(User);

			if (user == null)
			{
				return BadRequest("User not found.");
			}

			var hasReport = context.Intractions
				.FirstOrDefault(x => x.UserId == user.Id && x.ResourceId == Id && x.IsReport);

			LibraryResource resource = context.LibraryResources.Find(Id);

			if (hasReport != null)
			{
				context.Intractions.Remove(hasReport);
				resource.Reports--;
			}
			else
			{
				var newLike = new ResourcesIntractions
				{
					UserId = user.Id,
					ResourceId = Id,
					IsLike = false,
					IsReport = true
				};

				context.Intractions.Add(newLike);
				resource.Reports++;
			}

			context.LibraryResources.Update(resource);
			context.SaveChanges();

			return Json(new { reports = resource.Reports });
		}

		[Authorize(Roles = "Admin")]

		// Displays pending resources for admin approval.
		public IActionResult Accept()
		{
			IEnumerable<LibraryResource> resources = context.LibraryResources.Where(rc=>rc.IsAccepted==false).ToList();
			return View(resources);
		}
		[Authorize(Roles = "Admin")]

		// Accepts a pending resource by admin.
		public IActionResult AcceptResource(int Id)
		{
			if (Id != 0)
			{

				LibraryResource resource = context.LibraryResources.FirstOrDefault(x => x.Id == Id);
				if (resource != null)
				{
					Notification notification = new Notification
					{
						Text = resource.Name + " has been Accepted by admin.",
						dateTime = DateTime.Now,
						StdId = resource.PublisherId
					};
					context.Notifications.Add(notification);
					resource.IsAccepted = true;
					context.LibraryResources.Update(resource);
					context.SaveChanges();
				}
			}

			return RedirectToAction("Accept");
		}
		// Declines a pending resource by admin.
		[Authorize(Roles = "Admin")]
		[HttpGet]
		public IActionResult Decline(int Id)
		{
			LibraryResource resource = context.LibraryResources.FirstOrDefault(x => x.Id == Id);
			if (resource != null)
			{
				Notification notification = new Notification
				{
					Text = resource.Name + " has been removed by admin.",
					dateTime = DateTime.Now,
					StdId = resource.PublisherId
				};
				context.Notifications.Add(notification);
				context.LibraryResources.Remove(resource);
				context.SaveChanges();
			}

			return RedirectToAction("Accept");
		}

		// Deletes a resource by admin.
		[Authorize(Roles = "Admin")]
		public IActionResult Delete(int Id)
		{
			LibraryResource resource = context.LibraryResources.FirstOrDefault(x => x.Id == Id);
			if (resource != null)
			{
				resource.IsDelete = true;
				context.SaveChanges();
			}
			return RedirectToAction("Index");
		}

	}
}
