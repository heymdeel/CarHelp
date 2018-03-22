using AutoMapper;
using CarHelp.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarHelp.ViewModels
{
    internal class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<User, TokenVM>();
        }
    }
}
