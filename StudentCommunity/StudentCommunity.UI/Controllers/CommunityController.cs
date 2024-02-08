using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StudentCommunity.UI.Data;
using StudentCommunity.UI.Models;
using StudentCommunity.UI.Models.AccountModel;
using StudentCommunity.UI.Models.NewsModel;
using StudentCommunity.UI.Models.PostModel;

namespace StudentCommunity.UI.Controllers
{
	[Authorize]
	public class CommunityController : Controller
	{
		#region Constructor
		private readonly UserManager<ApplicationUser> userManager;
		private readonly CommunityContext context;

		public CommunityController(UserManager<ApplicationUser> _userManager, CommunityContext context, IConfiguration configuration)
		{
			userManager = _userManager;
			this.context = context;
			this.context = context;
		}
		#endregion
		// Action method to display the index view.
		public async Task<IActionResult> Index()
		{
			var user = await userManager.GetUserAsync(User);

			var PostModel = new PostViewModel();
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

			var viewModel = new PostUserViewModel
			{
				userView = UserModel,
				postView = PostModel,
			};

			return View(viewModel);
		}

		// Action method to display the user's university.
		public async Task<IActionResult> UserUniversity()
		{
			var user = await userManager.GetUserAsync(User);

			var PostModel = new PostViewModel();
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
			};
			return View(viewModel);
		}

		// Action method to display the major community.
		public async Task<IActionResult> MajorCommunity()
		{
			var user = await userManager.GetUserAsync(User);

			var PostModel = new PostViewModel();
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
			};
			return View(viewModel);
		}

		// Action method to display the news.
		public async Task<IActionResult> News()
		{
			HttpClient client = new HttpClient();
			var response = await client.GetAsync("https://newsdata.io/api/1/news?country=jo&apikey=pub_371129f14959925b4ff50019d7baccc09a89b");

			if (response.IsSuccessStatusCode)
			{
				var ApiString = await response.Content.ReadAsStringAsync();
				JObject jsonObject = JsonConvert.DeserializeObject<JObject>(ApiString);

				if (jsonObject.ContainsKey("results"))
				{
					List<Article> articles = jsonObject["results"].ToObject<List<Article>>();
					List<NewsResponse> newsResponses = articles.Select(article => new NewsResponse { results = new List<Article> { article } }).ToList();
					return View(newsResponses);
				}
				else
				{
					return View("NotFound");
				}
			}
			else
			{
				return View("NotFound");
			}
		}

		// Action method to display the user's profile.
		public async Task<IActionResult> MyProfile()
		{
			var user = await userManager.GetUserAsync(User);

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
			return View(UserModel);
		}

		// Action method to display the news community.
		public async Task<IActionResult> NewsCommunity()
		{
			var user = await userManager.GetUserAsync(User);

			var PostModel = new PostViewModel();
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

			var viewModel = new PostUserViewModel
			{
				userView = UserModel,
				postView = PostModel,
			};

			return View(viewModel);
		}

		// Action method to display the language community.
		public async Task<IActionResult> LanguageCommunity()
		{
			var user = await userManager.GetUserAsync(User);

			var PostModel = new PostViewModel();
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

			var viewModel = new PostUserViewModel
			{
				userView = UserModel,
				postView = PostModel,
			};

			return View(viewModel);
		}

	}
}
