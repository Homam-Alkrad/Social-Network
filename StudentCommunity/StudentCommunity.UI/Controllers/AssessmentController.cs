using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudentCommunity.UI.Data;
using StudentCommunity.UI.Models.Assessment;

namespace StudentCommunity.UI.Controllers
{
	[Authorize(Roles = "Admin")]
	public class AssessmentController : Controller
	{
		 private readonly IHostEnvironment environment;
		 private readonly UserManager<ApplicationUser> userManager;
		private readonly CommunityContext context;
		public AssessmentController(IHostEnvironment environment, UserManager<ApplicationUser> userManager, CommunityContext context)
		{
			this.environment = environment;
			this.userManager = userManager;
			this.context = context;
		}

		// Action method to display the index view with feedbacks.
		public IActionResult Index()
		{
			return View(context.Feadbacks.ToList());	
		}

		// Action method to display the create view.
		public IActionResult Create()
		{
			return View();
		}

		// Action method to handle the creation of a new assessment.
		[HttpPost]
		public async Task<IActionResult> CreateAsync(AssessmentModel model)
		{
			ApplicationUser me = await userManager.GetUserAsync(User);

			if (ModelState.IsValid)
			{
				// جيبلي الملفات او الصور الي تم رفعها و خزنها بهذا المتغير 
				var MyFile = HttpContext.Request.Form.Files;

				// لفلي على كل الملفات الي تم رفعها 
				foreach (var image in MyFile)
				{
					var file = image;
					if (file.Length > 0)
					{
						var imageName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
						var upload = Path.Combine(environment.ContentRootPath, "wwwroot", "Custom", "images", "feadback");
						using (var StreamFile = new FileStream(Path.Combine(upload, imageName), FileMode.Create))
						{
							await file.CopyToAsync(StreamFile);
							model.photoPath = "/Custom/images/feadback/" + imageName;
						}
					}
				}
				Assessment obj = new Assessment
				{
					Text = model.Text,
					photoPath= model.photoPath,
					CreateTime = DateTime.Now,
					userId=me.Id
				};
				context.Feadbacks.Add(obj);
				context.SaveChanges();
				return RedirectToAction("Index","Community");
			}
			return View(model);
		}

		// Action method to remove an assessment.
		public IActionResult Remove(int Id)
		{
			var feedbackToRemove = context.Feadbacks.Find(Id);

			if (feedbackToRemove != null)
			{
				context.Feadbacks.Remove(feedbackToRemove);
				context.SaveChanges();
			}

			return RedirectToAction("Index", "Assessment");
		}
	}
}
