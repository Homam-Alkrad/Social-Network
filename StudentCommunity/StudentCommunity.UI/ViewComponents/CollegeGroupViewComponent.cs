using Microsoft.AspNetCore.Mvc;
using StudentCommunity.UI.Data;

namespace StudentCommunity.UI.ViewComponents
{
	public class CollegeGroupViewComponent: ViewComponent
	{
		private readonly CommunityContext context;
		public CollegeGroupViewComponent(CommunityContext context)
		{
			this.context = context;
		}
		public IViewComponentResult Invoke(int typeId)
		{
			List<Group> groups = new List<Group>();

			foreach (var Group in context.Groups.Where(Group => Group.typeId == typeId).ToList())
			{
				Group group = new Group
				{
					Id = Group.Id,
					Name = Group.Name,
					typeId = typeId
				};
				groups.Add(group);
			}
			groups.Reverse();
			return View(groups);
		}

	}
}
