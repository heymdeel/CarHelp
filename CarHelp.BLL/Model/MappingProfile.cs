using AutoMapper;
using CarHelp.BLL.Model.DTO;
using CarHelp.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarHelp.BLL.Model
{
    internal class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserSignUpDTO, UserProfile>();
        }
    }
}
