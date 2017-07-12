using System;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.ModelBinding;
using Newtonsoft.Json;
using WeatherApp_OKopot.Entities;
using WeatherApp_OKopot.Infrastructure;
using WeatherApp_OKopot.Models;
using ModelState = System.Web.Mvc.ModelState;

namespace WeatherApp_OKopot.Services
{
    public interface IWeatherControl
    {
        Task DailyWeather(string cityName, bool addToMainList);
        Task ManyDaysWeather(string cityName, int countOfDays);
    }

    public class WeatherControl:IWeatherControl
    {
        WeatherDBContext weatherDbContext = new WeatherDBContext();
        public async Task DailyWeather(string cityName, bool addToMainList)
        {
            try
            {
                using (var client = new HttpClient())
                using (HttpResponseMessage response =
                    await client.GetAsync("http://api.openweathermap.org/data/2.5/forecast/daily?q=" + cityName +
                                          "&units=metric&cnt=1&APPID=" +
                                          System.Configuration.ConfigurationManager.AppSettings["WeatherKey"]))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        var rootObj = JsonConvert.DeserializeObject<RootObject>(result);
                        var cityFromDb = CheckingCity(cityName, rootObj, addToMainList);
                        if (cityFromDb == null)
                        {
                            return;
                        }
                        //если есть запись погоды по такому городу, то удаляем запись
                        var deleteWeather = weatherDbContext.WeatherEntities.Where(
                            item => item.CityEntityId == cityFromDb.CityEntityId);
                        if (deleteWeather.Any())
                        {
                            weatherDbContext.WeatherEntities.RemoveRange(deleteWeather);
                        }

                        weatherDbContext.WeatherEntities.Add(new WeatherEntity
                        {
                            CityEntityId = cityFromDb.CityEntityId,
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

                        //var historyEntity = weatherDbContext.HistoryEntities.Where(
                        //    item => item.CityEntityId == cityFromDb.CityEntityId );
                        //if (!historyEntity.Any())
                        //{
                            weatherDbContext.HistoryEntities.Add(new HistoryEntity
                            {
                                Time = DateTime.Now,
                                CityEntityId = cityFromDb.CityEntityId
                            });
                        //}
                        
                        
                        weatherDbContext.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task ManyDaysWeather(string cityName, int countOfDays)
        {
            using (var client = new HttpClient())
            using (HttpResponseMessage response =
                await client.GetAsync("http://api.openweathermap.org/data/2.5/forecast/daily?q=" + cityName +
                                      "&units=metric&cnt=" + countOfDays + "&APPID=a0e092f89087b55a736a66d0519de7cb"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var rootObj = JsonConvert.DeserializeObject<RootObject>(result);
                    var cityFromDb = CheckingCity(cityName, rootObj, false);
                    if (cityFromDb == null)
                    {
                        return;
                    }

                    var deleteWeather = weatherDbContext.WeatherEntities.Where(
                        item => item.CityEntityId == cityFromDb.CityEntityId);
                    if (deleteWeather.Count() != 0)
                    {
                        weatherDbContext.WeatherEntities.RemoveRange(deleteWeather);
                    }

                    for (int i = 0; i < rootObj.list.Count; i++)
                    {
                        weatherDbContext.WeatherEntities.Add(
                            new WeatherEntity
                            {
                                CityEntityId = cityFromDb.CityEntityId,
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
                            }
                        );
                    }
                    weatherDbContext.SaveChanges();
                }
            }
        }

        //провераяем наличие города в базе
        public CityEntity CheckingCity(string cityName, RootObject rootObj, bool addToMainList)
        {
            //есть ли в базе город с таким названием
            var cityFromDb = weatherDbContext
                .CityEntities
                .FirstOrDefault
                (item => item.Name == cityName);
            //если нет, но API вернул какую-то информацию
            if (cityFromDb == null)
            {
                if (rootObj.city.name != "")
                {
                    //создаем новую запись "CityEntity"
                    cityFromDb = new CityEntity
                    {
                        Name = cityName,
                        AlternativeName = rootObj.city.name,
                        AddToMainList = false
                    };
                    if (addToMainList)
                    {
                        cityFromDb.AddToMainList = true;
                    }
                    weatherDbContext.CityEntities.Add(cityFromDb);
                    weatherDbContext.SaveChanges();
                        
                    return cityFromDb;
                }
                return null;

            }
            //если такой город есть, то добавляем ему альтернативное имя, которое вернул API
            cityFromDb.AlternativeName = rootObj.city.name;
            if (addToMainList)
            {
                cityFromDb.AddToMainList = true;
            }
            weatherDbContext.Entry(cityFromDb).State = EntityState.Modified;
            weatherDbContext.SaveChanges();
            return cityFromDb;
        }
    }
}