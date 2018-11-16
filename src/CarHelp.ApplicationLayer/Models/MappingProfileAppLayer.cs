using AutoMapper;
using CarHelp.AppLayer.Models.DTO;
using CarHelp.DAL.DTO;
using CarHelp.DAL.Entities;
using GeoAPI.Geometries;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarHelp.AppLayer.Models
{
    public class MappingProfileAppLayer : Profile
    {
        public MappingProfileAppLayer()
        {
            CreateMap<SignUpInput, User>();
            CreateMap<ProfileInput, UserProfile>();

            CreateMap<CreateOrderInput, Order>()
                .ForMember(o => o.Location, s => s.MapFrom(x => new Point(new Coordinate(x.Location.Longitude, x.Location.Latitude)) { SRID = 4326 }));
            CreateMap<SearchOrderInput, DALSearchOrderDTO>();
            CreateMap<WorkerRespondOrderInput, RespondedWorkers>()
                .ForMember(rw => rw.Location,
                            w => w.MapFrom(x => new Point(new Coordinate(x.Location.Longitude, x.Location.Latitude)) { SRID = 4326 }));
           
        }
    }
}
