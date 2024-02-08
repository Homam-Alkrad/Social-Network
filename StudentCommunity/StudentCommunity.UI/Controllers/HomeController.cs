using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentCommunity.UI.Data;
using StudentCommunity.UI.Models;
using StudentCommunity.UI.Models.AccountModel;
using System.Diagnostics;

namespace StudentCommunity.UI.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly UserManager<ApplicationUser> userManager;
		private readonly CommunityContext context;

		public HomeController(ILogger<HomeController> logger, UserManager<ApplicationUser> userManager, CommunityContext context)
		{
			this._logger = logger;
			this.userManager = userManager;
			this.context = context; 
		}


		public  IActionResult Index()
		{
			
			return View();

		}	

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}