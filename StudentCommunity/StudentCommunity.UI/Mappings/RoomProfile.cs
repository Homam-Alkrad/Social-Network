using AutoMapper;
using StudentCommunity.UI.Models;
using Chat.Web.ViewModels;
using StudentCommunity.UI.Data;
using StudentCommunity.UI.ViewModels;

namespace Chat.Web.Mappings
{
    public class RoomProfile : Profile
    {
        public RoomProfile()
        {
            CreateMap<Room, RoomViewModel>()
                .ForMember(dst => dst.Admin, opt => opt.MapFrom(x => x.Admin.UserName));

            CreateMap<RoomViewModel, Room>();
        }
    }
}
