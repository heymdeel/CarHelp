using AutoMapper;
using CarHelp.DAL.DTO;
using CarHelp.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarHelp.DAL
{
    internal class MappingProfileDAL : Profile
    {
        public MappingProfileDAL()
        {
            CreateMap<DALOrderCreateDTO, Order>();
        }
    }
}
