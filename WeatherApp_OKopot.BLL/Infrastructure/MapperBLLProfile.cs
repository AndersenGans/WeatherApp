using System.Collections.Generic;
using AutoMapper;
using WeatherApp_OKopot.BLL.DTO;
using WeatherApp_OKopot.DAL.Entities;

namespace WeatherApp_OKopot.BLL.Infrastructure
{
    public class MapperBLLProfile:Profile
    {
        public MapperBLLProfile()
        {
            CreateMap<CityDTO, City>()
                .ForMember(c => c
                    .Weathers, opt => opt
                    .MapFrom(s => Mapper.Map<ICollection<WeatherDTO>, ICollection<Weather>>(s.Weathers)));

            CreateMap<City, CityDTO>()
                .ForMember(c => c
                    .Weathers, opt => opt
                    .MapFrom(s => Mapper.Map<ICollection<Weather>, ICollection<WeatherDTO>>(s.Weathers)));

            CreateMap<WeatherDTO, Weather>()
                .ForMember(c  => c
                    .City, opt => opt
                        .Ignore());

            CreateMap<Weather, WeatherDTO>()
                .ForMember(c => c
                    .City, opt => opt
                    .MapFrom(s => s.City.AlternativeName));

            CreateMap<HistoryDTO, History>()
                .ForMember(c => c
                    .City, opt => opt
                    .MapFrom(s => Mapper.Map<CityDTO, City>(s.City)));

            CreateMap<History, HistoryDTO>()
                .ForMember(c => c
                    .City, opt => opt
                    .MapFrom(s => Mapper.Map<City, CityDTO>(s.City)));


        }
    }
}