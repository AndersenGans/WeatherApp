using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WeatherApp_OKopot.BLL.DTO;

namespace WeatherApp_OKopot.BLL.Interfaces
{
    public interface IService
    {
        IEnumerable<CityDTO> FindCitiesToAddToMainList();
        IEnumerable<WeatherDTO> FindWeathers(string cityName);
        Task GetDailyWeathers(string cityName, string key, bool addToMainList);
        Task GetManyDaysWeathers(string cityName, string key, int countOfDays);
        IEnumerable<HistoryDTO> GetHistories();
        CityDTO GetCityByName(string cityName);
        void DeleteCitiesFromMainList(string cityName);
        void DeleteHistories();
        void Dispose();
    }
}