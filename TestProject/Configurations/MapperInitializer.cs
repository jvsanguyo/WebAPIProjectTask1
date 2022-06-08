using AutoMapper;
using TestProject.Data;
using TestProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestProject.Configurations
{
    public class MapperInitializer : Profile
    {
        public MapperInitializer()
        {
            CreateMap<Summary, CovidSummary>().ReverseMap();
            CreateMap<TestProject.Data.Global, TestProject.Models.Global>().ReverseMap();
            CreateMap<Country, TestProject.Models.Countries>().ReverseMap().ForMember(dest => dest.CountryName, opt => opt.MapFrom(src => src.Country));
            CreateMap<History, CovidHistory>().ReverseMap();
            CreateMap<ApiUser, UserDTO>().ReverseMap();
        }
    }
}