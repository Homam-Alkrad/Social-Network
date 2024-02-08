using Hope.Infrastructure.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StudentCommunity.UI.Data;
using StudentCommunity.UI.Models.AccountModel;
using StudentCommunity.UI.Models.PostModel;

namespace StudentCommunity.UI.Controllers
{
	[Authorize]
	public class PostController : BaseController
	{
		#region ctro	
		private readonly UserManager<ApplicationUser> userManager;
		private readonly CommunityContext context;
		private readonly IHostEnvironment env;
		public PostController(IHostEnvironment env, UserManager<ApplicationUser> _userManager,
			RoleManager<IdentityRole> _roleManager, CommunityContext context, IConfiguration configuration) : base(configuration)
		{
			userManager = _userManager;
			this.env = env;
			this.context = context;
		}
		#endregion

		[HttpPost]
		public async Task<IActionResult> CreatePostAsync(PostUserViewModel model)
		{
			UserViewModel user = model.userView;

			PostViewModel post = model.postView;

			HttpClient client = new HttpClient();
			string url = configuration.GetSection("BaseAPIURL").Value.ToString();
			var response = await client.GetAsync(url + "Home?text=" + post.Content);

			if (response.IsSuccessStatusCode)
			{
				var apiResponse = await response.Content.ReadAsStringAsync();
				var IsOffensive = JsonConvert.DeserializeObject<bool>(apiResponse);
				if (!IsOffensive)
				{
					var MyFile = HttpContext.Request.Form.Files;

					foreach (var image in MyFile)
					{
						var file = image;
						if (file.Length > 0)
						{
							var imageName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
							string upload = Path.Combine(env.ContentRootPath, "wwwroot", "Custom", "images", "posts");

							using (var StreamFile = new FileStream(Path.Combine(upload, imageName), FileMode.Create))
							{
								await file.CopyToAsync(StreamFile);

								post.PhtotPath = "/Custom/images/posts/" + imageName;
							}

						}
					}
					if (post != null)
					{
						Post _post = new Post
						{
							PostId = post.PostId,
							Content = post.Content,
							PhotoPath = post.PhtotPath,
							PublicationTime = DateTime.Now,
							UserId = post.UserId,
						};
						if (post.majorId != null)
						{
							_post.majorId = post.majorId;
						}
						else if (post.universityId != null)
						{
							_post.universityId = post.universityId;
						}
						else if(post.IsNews==true)
						{
							_post.IsNews = true;
						}
						else if(post.IsForLanguage==true)
						{
							_post.IsForLanguage = true;
						}
						else if (post.groupId != null)
						{
							_post.groupId = post.groupId;
							context.Posts.Add(_post);
							context.SaveChanges();
							HttpContext.Session.SetString("groupId", post.groupId.ToString());
							return RedirectToAction("Group", "Groups", post.groupId);
						}
						context.Posts.Add(_post);
						context.SaveChanges();
						string previousView = post.returnUrl;
						return RedirectToAction(previousView, "Community");
					}
					return RedirectToAction("NotFound", "Account");
				}
				else
				{
					string previousView = post.returnUrl;
					TempData["OffensiveMessage"] = "Your post may be offensive";
					return RedirectToAction(previousView, "Community");
				}

			}
			else
			{
				return View("AccessDenied", "Account");
			}

		}

		[HttpPost]
		public async Task<IActionResult> Like(int postId)
		{
			var user = await userManager.GetUserAsync(User);

			if (user == null)
			{
				return BadRequest("User not found.");
			}

			var hasLiked = context.PostsIntractions
				.FirstOrDefault(x => x.UserId == user.Id && x.PostId == postId && x.IsLike);

			Post post = context.Posts.Find(postId);

			if (hasLiked != null)
			{
				context.PostsIntractions.Remove(hasLiked);
				post.Likes--;
			}
			else
			{
				var newLike = new PostsIntractions
				{
					UserId = user.Id,
					PostId = postId,
					IsLike = true,
					IsComment = false,
					IsDislike = false
				};

				context.PostsIntractions.Add(newLike);
				post.Likes++;
			}

			context.Posts.Update(post);
			context.SaveChanges();

			return Json(new { likes = post.Likes });
		}
		[HttpPost]
		public async Task<IActionResult> Dislike(int postId)
		{
			var user = await userManager.GetUserAsync(User);

			if (user == null)
			{
				return BadRequest("User not found.");
			}

			Post post = context.Posts.Find(postId);

			var hasDisliked = context.PostsIntractions
				.FirstOrDefault(x => x.UserId == user.Id && x.PostId == postId && x.IsDislike);

			if (hasDisliked != null)
			{
				context.PostsIntractions.Remove(hasDisliked);
				post.Dislikes--;
			}
			else
			{
				var newDislike = new PostsIntractions
				{
					UserId = user.Id,
					PostId = postId,
					IsLike = false,
					IsComment = false,
					IsDislike = true
				};

				context.PostsIntractions.Add(newDislike);
				post.Dislikes++;
			}

			context.Posts.Update(post);
			context.SaveChanges();

			return Json(new { dislikes = post.Dislikes });
		}
		public async Task<IActionResult> CreateCommentAsync([FromBody] Comment _comment)
		{
			try
			{
				var user = await userManager.GetUserAsync(User);

				Comment comment = new Comment
				{
					CommentTime = DateTime.Now,
					Content = _comment.Content,
					PostId = _comment.PostId,
					UserId = user.Id,
					User = user,
				};

				context.Comments.Add(comment);
				context.SaveChanges();
				return Json(new { success = true, comment = comment });
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return StatusCode(500, "An error occurred while processing the request.");
			}
		}
		public IActionResult Delete(int PostId)
		{
			if (PostId != 0)
			{
				Post DbPost = context.Posts.ToList().Find(_Post => _Post.PostId == PostId);
				PostsIntractions postDetails = context.PostsIntractions.ToList().Find(p => p.PostId == PostId);
				if (DbPost != null)
				{
					context.Posts.Remove(DbPost);
				}
				if (postDetails != null)
				{
					context.PostsIntractions.Remove(postDetails);
				}
				context.SaveChanges();

				return Json(new { success = true });
			}

			return Json(new { success = false });
		}
		public IActionResult DeleteComment(int CommentId)
		{
			if (CommentId != 0)
			{
				Comment comment = context.Comments.Where(comment => comment.CommentId == CommentId).FirstOrDefault();

				if (comment != null)
				{
					context.Comments.Remove(comment);
				}
				context.SaveChanges();

				return Json(new { success = true });
			}

			return Json(new { success = false });
		}
		public async Task<IActionResult> EditPostAsync([FromBody] Post post)
		{
			HttpClient client = new HttpClient();
			string url = configuration.GetSection("BaseAPIURL").Value.ToString();
			var response = await client.GetAsync(url + "Home?text=" + post.Content);

			if (response != null)
			{
				var apiResponse = await response.Content.ReadAsStringAsync();
				var IsOffensive = JsonConvert.DeserializeObject<bool>(apiResponse);
				if (!IsOffensive)
				{
					Post _post = context.Posts.Where(p => p.PostId == post.PostId).FirstOrDefault();
					if (_post != null)
					{
						_post.Content = post.Content;
						context.Posts.Update(_post);
						context.SaveChanges();
						return Json(new { success = true });
					}
				}
				return Json(new { success = false });
			}
			return RedirectToAction("MyProfile", "Community");
		}
	}
}
