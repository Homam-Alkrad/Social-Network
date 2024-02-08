using Microsoft.AspNetCore.Mvc;
using StudentCommunity.UI.Data;

namespace StudentCommunity.UI.ViewComponents
{
	public class AdvertisementImagesViewComponent:ViewComponent
	{
		private readonly CommunityContext context;

		public AdvertisementImagesViewComponent(CommunityContext context)
		{
			this.context = context;
		}
		public IViewComponentResult Invoke(int numberOfPosts)
		{
			return View(context.Advertisings.Where(adv=>adv.IsAccept==true && adv.ImagePhath !=null).ToList());
		}
	}
}
