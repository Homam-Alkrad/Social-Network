using Microsoft.AspNetCore.Mvc;
using StudentCommunity.UI.Data;

namespace StudentCommunity.UI.ViewComponents
{
    public class FriendPageViewComponent:ViewComponent
    {
        private readonly CommunityContext context;

        public FriendPageViewComponent(CommunityContext context)
        {
            this.context = context;
        }

        public IViewComponentResult Invoke(string Id)
        {
            List<ApplicationUser> users = new List<ApplicationUser>();

            foreach (var item in context.FriendRequests
                .Where(fr => (fr.SenderUserId == Id || fr.ReceiverUserId == Id) && fr.Status == true)
                .ToList())
            {
                ApplicationUser user = new ApplicationUser();

                if (item.SenderUserId == Id)
                {
                    user = context.Users.Where(x => x.Id == item.ReceiverUserId).FirstOrDefault();
                }
                else
                {
                    user = context.Users.Where(x => x.Id == item.SenderUserId).FirstOrDefault();
                }

                users.Add(user);
            }

            return View(users);
        }
    }
}
