using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using WeatherApp_OKopot.Models;

namespace WeatherApp_OKopot.Services
{
    public interface IWeatherControl
    {
        Task<List<WeatherModel>> DailyWeather(string cityName);
        Task<List<WeatherModel>> ManyDaysWeather(string cityName, int countOfDays);
    }

    public class WeatherControl:IWeatherControl
    {
        public async Task<List<WeatherModel>> DailyWeather(string cityName)
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

                    List<WeatherModel> listWeather = new List<WeatherModel>()
                    {

                        new WeatherModel()
                        {
                            CityName = rootObj.city.name,
                            Cloudiness = rootObj.list[0].clouds,
                            DayAvgTemp = rootObj.list[0].temp.day,
                            DayMaxTemp = rootObj.list[0].temp.max,
                            DayMinTemp = rootObj.list[0].temp.min,
                            DescWeather = rootObj.list[0].weather[0].description,
                            EveTemp = rootObj.list[0].temp.eve,
                            Humidity = rootObj.list[0].humidity,
                            MainWeather = rootObj.list[0].weather[0].main,
                            MonTemp = rootObj.list[0].temp.morn,
                            WindSpeed = rootObj.list[0].speed,
                            Pressure = rootObj.list[0].pressure,
                            NightTemp = rootObj.list[0].temp.night,
                            IconId = rootObj.list[0].weather[0].icon + ".png",
                            Day = DateTime.Today
                        }
                    };
                    return listWeather;
                }
            }
            return null;
        }

        public async Task<List<WeatherModel>> ManyDaysWeather(string cityName, int countOfDays)
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

                    List<WeatherModel> listWeather = new List<WeatherModel>();

                    for (int i = 0; i < rootObj.list.Count; i++)
                    {
                        listWeather.Add(
                            new WeatherModel()
                            {
                                CityName = rootObj.city.name,
                                Cloudiness = rootObj.list[i].clouds,
                                DayAvgTemp = rootObj.list[i].temp.day,
                                DayMaxTemp = rootObj.list[i].temp.max,
                                DayMinTemp = rootObj.list[i].temp.min,
                                DescWeather = rootObj.list[i].weather[0].description,
                                EveTemp = rootObj.list[i].temp.eve,
                                Humidity = rootObj.list[i].humidity,
                                MainWeather = rootObj.list[i].weather[0].main,
                                MonTemp = rootObj.list[i].temp.morn,
                                WindSpeed = rootObj.list[i].speed,
                                Pressure = rootObj.list[i].pressure,
                                NightTemp = rootObj.list[i].temp.night,
                                IconId = rootObj.list[i].weather[0].icon + ".png",
                                Day = DateTime.Today.AddDays(i)
                            }
                        );
                    }


                    return listWeather;
                }
            }
            return null;
        }
    }
}