using AutoMapper;
using CarHelp.BLL.Model.DTO;
using CarHelp.DAL.DTO;
using CarHelp.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarHelp.BLL.Model
{
    internal class MappingProfileBLL : Profile
    {
        public MappingProfileBLL()
        {
            CreateMap<UserSignUpDTO, UserProfile>();
            CreateMap<OrderCreateDTO, DALOrderCreateDTO>();
        }
    }
}
