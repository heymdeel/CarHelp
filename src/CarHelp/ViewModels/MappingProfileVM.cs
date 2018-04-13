using AutoMapper;
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
            CreateMap<User, TokenVM>();
            CreateMap<UserProfile, UserProfileVM>();
            CreateMap<ClosestWorkerInfoDTO, ClosestWorkersVM>()
                .ForMember(vm => vm.Worker, r => r.MapFrom(cw => cw));

            CreateMap<Order, CreatedOrderVM>();
        }
    }
}
