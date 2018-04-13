using AutoMapper;
using CarHelp.AppLayer.Models.DTO;
using CarHelp.DAL.DTO;
using CarHelp.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarHelp.AppLayer.Models
{
    internal class MappingProfileAppLayer : Profile
    {
        public MappingProfileAppLayer()
        {
            CreateMap<UserSignUpDTO, UserProfile>();
            CreateMap<OrderCreateDTO, DALOrderCreateDTO>();
        }
    }
}
