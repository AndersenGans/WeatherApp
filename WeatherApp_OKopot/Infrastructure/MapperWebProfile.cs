using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using WeatherApp_OKopot.BLL.DTO;
using WeatherApp_OKopot.Models;

namespace WeatherApp_OKopot.Infrastructure
{
    public class MapperWebProfile:Profile
    {
        public MapperWebProfile()
        {
            CreateMap<CityDTO, CityModel>()
                .ForMember(c => c
                    .Weathers,opt => opt
                        .MapFrom(s => Mapper.Map<ICollection<WeatherDTO>, ICollection<WeatherModel>>(s.Weathers)));

            CreateMap<CityModel, CityDTO>()
                .ForMember(c => c
                    .Weathers, opt => opt
                    .MapFrom(s => Mapper.Map<ICollection<WeatherModel>, ICollection<WeatherDTO>>(s.Weathers)));

            CreateMap<WeatherDTO, WeatherModel>()
                .ReverseMap();

            CreateMap<HistoryDTO, HistoryModel>()
                .ForMember(c => c
                    .City, opt => opt
                        .MapFrom(s => Mapper.Map<CityDTO, CityModel>(s.City)));

            CreateMap<HistoryModel, HistoryDTO>()
                .ForMember(c => c
                    .City, opt => opt
                    .MapFrom(s => Mapper.Map<CityModel, CityDTO>(s.City)));


        }
    }
}