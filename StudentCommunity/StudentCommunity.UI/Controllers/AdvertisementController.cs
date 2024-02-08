using Humanizer.Localisation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudentCommunity.UI.Data;
using StudentCommunity.UI.Models.AdvertisingModel;

namespace StudentCommunity.UI.Controllers
{
	public class AdvertisementController : Controller
	{
		private readonly IHostEnvironment env;
		private readonly CommunityContext context;
		private readonly UserManager<ApplicationUser> userManager;
		public AdvertisementController(CommunityContext context,UserManager<ApplicationUser> userManager, IHostEnvironment env)
		{
			this.context = context;
			this.userManager = userManager;
			this.env = env;
		}

		// Renders the view for creating advertising.
		public IActionResult CreateAdvertising()
		{
			return View();
		}

		// Handles the creation of advertising.
		[HttpPost]
		public async Task<IActionResult> CreateAdvertising(CreateAdvertisement model)
		{
			if (ModelState.IsValid)
			{
			ApplicationUser user = await userManager.GetUserAsync(User);		
				var MyFile = HttpContext.Request.Form.Files;
				foreach (var image in MyFile)
				{
					var file = image;
					if (file.Length > 0)
					{
						var imageName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
						var upload = Path.Combine(env.ContentRootPath, "wwwroot", "Custom", "images", "Advertising");
						using (var StreamFile = new FileStream(Path.Combine(upload, imageName), FileMode.Create))
						{
							await file.CopyToAsync(StreamFile);
							model.ImagePhath = "/Custom/images/Advertising/" + imageName;
						}
					}
				}
				Advertising _advertising = new Advertising
				{
					Text = model.Text,
					Title = model.Title,
					PhoneNumber = model.PhoneNumber,
					ImagePhath = model.ImagePhath,
				};
				_advertising.UserId = user.Id;
				_advertising.IsAccept = false;

				context.Advertisings.Add(_advertising);
				context.SaveChanges();
				return RedirectToAction("Index", "Community");
			}
			return View(model);
		}

		// Renders the view for accepting advertisements.
		public IActionResult AcceptAdvertisings()
		{
			return View(context.Advertisings.Where(x=>x.IsAccept==false).ToList());
		}

		// Renders the view for managing advertisements.
		public IActionResult AdvertisingsManagment()
		{
			return View(context.Advertisings.Where(x=>x.IsAccept==true).ToList());
		}

		// Handles the acceptance of an advertisement.
		public IActionResult Accept(int Id)
		{
			Advertising advertising = context.Advertisings.Where(x => x.Id == Id).FirstOrDefault();
			if(advertising != null)
			{
				if (advertising != null)
				{
					Notification notification = new Notification
					{
						Text = advertising.Title + " has been Accepted by admin.",
						dateTime = DateTime.Now,
						StdId = advertising.UserId
					};

					advertising.IsAccept = true;

					context.Notifications.Add(notification);
					context.Advertisings.Update(advertising);
					context.SaveChanges();

				}
			}
			return RedirectToAction("AcceptAdvertisings");
		}

		// Handles the decline of an advertisement.
		[HttpGet]
		public IActionResult Decline(int Id)
		{

			Advertising advertising = context.Advertisings.Where(x => x.Id == Id).FirstOrDefault();
			if (advertising != null)
			{
				Notification notification = new Notification
				{
					Text = advertising.Title + " has been removed by admin.",
					dateTime = DateTime.Now,
					StdId = advertising.UserId
				};
				context.Notifications.Add(notification);
				context.Advertisings.Remove(advertising);
				context.SaveChanges();
			}
			return RedirectToAction("AcceptAdvertisings");
		}

		// Handles the deletion of an advertisement.
		[HttpGet]
		public IActionResult Delete(int Id)
		{

			Advertising advertising = context.Advertisings.Where(x => x.Id == Id).FirstOrDefault();
			if (advertising != null)
			{
				Notification notification = new Notification
				{
					Text = advertising.Title + " has been Delete by admin.",
					dateTime = DateTime.Now,
					StdId = advertising.UserId
				};
				context.Notifications.Add(notification);
				context.Advertisings.Remove(advertising);
				context.SaveChanges();
			}
			return RedirectToAction("AdvertisingsManagment");
		}


	}
}
