using AutoMapper;
using CarHelp.DAL.Entities;

namespace CarHelp.ViewModels
{
    internal class MappingProfileVM: Profile
    {
        public MappingProfileVM()
        {
            CreateMap<UserProfile, UserProfileVM>();

            CreateMap<((string refresh, string access) tokens, User user), TokenVM>()
                .ForMember(vm => vm.RefreshToken, t => t.MapFrom(o => o.tokens.refresh))
                .ForMember(vm => vm.AccessToken, t => t.MapFrom(o => o.tokens.access))
                .ForMember(vm => vm.Id, t => t.MapFrom(o => o.user.Id))
                .ForMember(vm => vm.Roles, t => t.MapFrom(o => o.user.Roles));

            CreateMap<Order, CreatedOrderVM>();
        }
    }
}
