using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using wolftrack.DTO;
using wolftrack.Models;

namespace wolftrack
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Pack, GetPackDTO>();
            CreateMap<AddPackDTO, Pack>();
            CreateMap<Wolf, GetWolfDTO>();
            CreateMap<AddWolfDTO, Wolf>();
        }
    }
}
