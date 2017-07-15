using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Newtonsoft.Json;
using WeatherApp_OKopot.BLL.BusinessModels;
using WeatherApp_OKopot.BLL.DTO;
using WeatherApp_OKopot.BLL.Infrastructure;
using WeatherApp_OKopot.BLL.Interfaces;
using WeatherApp_OKopot.DAL.Entities;
using WeatherApp_OKopot.DAL.Interfaces;

namespace WeatherApp_OKopot.BLL.Services
{
    public class Service:IService
    {
        private IUnitOfWork Database;

        public Service(IUnitOfWork uow)
        {
            Database = uow;
        }

        public IEnumerable<CityDTO> FindCitiesToAddToMainList()
        {
            return Mapper.Map<IEnumerable<City>, List<CityDTO>>(Database.Cities.Find(item => item.AddToMainList));
        }

        public IEnumerable<WeatherDTO> FindWeathers(string cityName)
        {
            if(cityName == "")
                throw new ValidationException("There's no city like in the field", "city");
            
            var result = Mapper.Map<IEnumerable<Weather>, List<WeatherDTO>>(Database.Weathers.Find(item => item.City.Name == cityName));

            return result;
        }

        public async Task GetDailyWeathers(string cityName, string key, bool addToMainList)
        {
            RootObject rootObj = await RequestingFromAPI(cityName, 1, key);
            var cityFromDb = CheckingCity(cityName, rootObj, addToMainList);

            if (cityFromDb == null)
            {
                return;
            }

            //если есть запись погоды по такому городу, то удаляем запись
            var deleteWeather = Database.Weathers.Find(
                item => item.CityId == cityFromDb.CityId);

            if (deleteWeather.Any())
            {
                Database.Weathers.Delete(deleteWeather);
            }

            Database.Weathers.Create(new Weather
            {
                CityId = cityFromDb.CityId,
                Cloudiness = rootObj.list[0].clouds,
                DayAvgTemp = rootObj.list[0].temp.day,
                DayMinTemp = rootObj.list[0].temp.min,
                DescWeather = rootObj.list[0].weather[0].description,
                Humidity = rootObj.list[0].humidity,
                MainWeather = rootObj.list[0].weather[0].main,
                WindSpeed = rootObj.list[0].speed,
                Pressure = rootObj.list[0].pressure,
                IconId = rootObj.list[0].weather[0].icon + ".png",
                Day = DateTime.Today
            });
            Database.Histories.Create(new History
            {
                Time = DateTime.Now,
                CityId = cityFromDb.CityId
            });
            Database.Save();
        }

        public async Task GetManyDaysWeathers(string cityName, string key, int countOfDays)
        {
            var rootObj = await RequestingFromAPI(cityName, countOfDays, key);
            var cityFromDb = CheckingCity(cityName, rootObj, false);

            if (cityFromDb == null)
            {
                return;
            }

            //если есть запись погоды по такому городу, то удаляем запись
            var deleteWeather = Database.Weathers.Find(
                item => item.CityId == cityFromDb.CityId);

            if (deleteWeather.Any())
            {
                Database.Weathers.Delete(deleteWeather);
            }

            for (int i = 0; i < rootObj.list.Count; i++)
            {
                Database.Weathers.Create(new Weather
                {
                    CityId = cityFromDb.CityId,
                    Cloudiness = rootObj.list[i].clouds,
                    DayAvgTemp = rootObj.list[i].temp.day,
                    DayMinTemp = rootObj.list[i].temp.min,
                    DescWeather = rootObj.list[i].weather[0].description,
                    Humidity = rootObj.list[i].humidity,
                    MainWeather = rootObj.list[i].weather[0].main,
                    WindSpeed = rootObj.list[i].speed,
                    Pressure = rootObj.list[i].pressure,
                    IconId = rootObj.list[i].weather[0].icon + ".png",
                    Day = DateTime.Today.AddDays(i)
                });
            }
            Database.Save();
        }

        public IEnumerable<HistoryDTO> GetHistories()
        {
            var result = Mapper.Map<IEnumerable<History>, List<HistoryDTO>>(Database.Histories.GetAll());

            return result;
        }

        public void DeleteCitiesFromMainList(string cityName)
        {
            if (String.IsNullOrEmpty(cityName))
                throw new ValidationException("There's no city like in the field", "ListOfCities");

            var city = Database.Cities.GetAll().First(item => item.Name == cityName);
            city.AddToMainList = false;
            Database.Cities.Update(city);
            Database.Save();
        }

        public CityDTO GetCityByName(string cityName)
        {
            if(cityName == "")
                throw new ValidationException("There's no city like in the field", "city");

            var result = Mapper.Map<City, CityDTO>(Database.Cities.GetAll().First(item => item.Name == cityName));

            return result;
        }

        public void DeleteHistories()
        {
            var histories = Database.Histories.GetAll();
            Database.Histories.Delete(histories);
            Database.Save();
        }

        public void Dispose()
        {
            Database.Dispose();
        }





        internal City CheckingCity(string cityName, RootObject rootObj, bool addToMainList)
        {
            //есть ли в базе город с таким названием
            var cityFromDb = Database
                .Cities
                .Find
                (item => item.Name == cityName).FirstOrDefault();
            //если нет, но API вернул какую-то информацию

            if (cityFromDb == null)
            {

                if (rootObj != null)
                {
                    //создаем новую запись "CityEntity"
                    cityFromDb = new City
                    {
                        Name = cityName,
                        AlternativeName = rootObj.city.name,
                        AddToMainList = false
                    };

                    if (addToMainList)
                    {
                        cityFromDb.AddToMainList = true;
                    }
                    Database.Cities.Create(cityFromDb);
                    Database.Save();

                    return cityFromDb;
                }
                throw new ValidationException("There's no city like in the field", "city");
            }
            //если такой город есть, то добавляем ему альтернативное имя, которое вернул API
            cityFromDb.AlternativeName = rootObj.city.name;

            if (addToMainList)
            {
                cityFromDb.AddToMainList = true;
            }

            Database.Cities.Update(cityFromDb);
            Database.Save();

            return cityFromDb;
        }

        internal async Task<RootObject> RequestingFromAPI(string cityName, int countOfDays, string key)
        {
            var client = new HttpClient();
            var response = client.GetAsync("http://api.openweathermap.org/data/2.5/forecast/daily?q=" + cityName +
                                      "&units=metric&cnt=" + countOfDays + "&APPID=" + key).Result;

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var rootObj = JsonConvert.DeserializeObject<RootObject>(result);
                return rootObj;
            }

            return null;
        }
    }
}