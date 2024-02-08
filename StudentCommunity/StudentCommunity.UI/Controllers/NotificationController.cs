using Microsoft.AspNetCore.Mvc;
using StudentCommunity.UI.Data;

namespace StudentCommunity.UI.Controllers
{
    public class NotificationController : Controller
    {
        private readonly CommunityContext context;
        public NotificationController(CommunityContext context) 
        { 
            this.context = context;
        }
        [HttpPost]
        public IActionResult Remove(int Id)
        {
            try
            {
                if (Id != 0)
                {
                    Notification notification = context.Notifications.Find(Id);

                    if (notification != null)
                    {
                        context.Notifications.Remove(notification);
                        context.SaveChanges();
                        return Json(new { success = true });
                    }
                    else
                    {
                        return Json(new { success = false, error = "Notification not found" });
                    }
                }

                return Json(new { success = false, error = "Invalid Id" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }
    }
}
