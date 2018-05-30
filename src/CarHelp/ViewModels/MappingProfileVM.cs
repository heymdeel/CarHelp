using AutoMapper;
using CarHelp.AppLayer.Models.DTO;
using CarHelp.DAL.Entities;
using CarHelp.DAL.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarHelp.ViewModels
{
    internal class MappingProfileVM: Profile
    {
        public MappingProfileVM()
        {
            CreateMap<UserProfile, UserProfileVM>();
            CreateMap<ClosestWorkerkDTO, ClosestWorkersVM>()
                .ForMember(vm => vm.Worker, r => r.MapFrom(cw => cw));

            CreateMap<((string refresh, string access) tokens, User user), TokenVM>()
                .ForMember(vm => vm.RefreshToken, t => t.MapFrom(o => o.tokens.refresh))
                .ForMember(vm => vm.AccessToken, t => t.MapFrom(o => o.tokens.access))
                .ForMember(vm => vm.Id, t => t.MapFrom(o => o.user.Id))
                .ForMember(vm => vm.Roles, t => t.MapFrom(o => o.user.Roles));

            CreateMap<Order, CreatedOrderVM>();
        }
    }
}
