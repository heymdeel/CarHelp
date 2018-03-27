using AutoMapper;
using CarHelp.DAL.Entities;
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
            CreateMap<User, TokenVM>();
            CreateMap<UserProfile, UserProfileVM>();
            CreateMap<(double price, double distance, UserProfile worker), ClosestWorkersVM>()
                .ForMember(vm => vm.Distance, r => r.MapFrom(cw => cw.distance))
                .ForMember(vm => vm.Price, r => r.MapFrom(cw => cw.price))
                .ForMember(vm => vm.Worker, r => r.MapFrom(cw => cw.worker));
        }
    }
}
