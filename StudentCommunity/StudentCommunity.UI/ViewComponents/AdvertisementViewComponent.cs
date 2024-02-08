using Microsoft.AspNetCore.Mvc;
using StudentCommunity.UI.Data;

namespace StudentCommunity.UI.ViewComponents
{
	public class AdvertisementViewComponent:ViewComponent
	{
		private readonly CommunityContext context;

		public AdvertisementViewComponent(CommunityContext context)
		{
			this.context = context;
		}
		public IViewComponentResult Invoke()
		{
			return View(context.Advertisings.Where(adv=>adv.IsAccept==true && adv.ImagePhath==null).ToList());
		}

	}
}
