using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WeatherApp_OKopot.Models;
using WeatherApp_OKopot.Services;

namespace WeatherApp_OKopot.Controllers
{
    public class HomeController : Controller
    {
        private static SelectList listOfCities;
        private static WeatherControl weatherControl;
        private List<WeatherModel> listWeathers;

        public HomeController()
        {
            if (listOfCities == null)
            {
                listOfCities = new SelectList(new List<String> { "Киев", "Львов", "Харьков", "Днепропетровск", "Одесса" });
            }
            if (weatherControl == null)
            {
                weatherControl = new WeatherControl();
            }
            ViewBag.ListOfCities = listOfCities;
            listWeathers = new List<WeatherModel>();
        }
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> ShowDailyWeather(string search)
        {
            listWeathers = await weatherControl.DailyWeather(search);
            return PartialView(listWeathers);
        }

        public async Task<ActionResult> ShowThreeDaysWeather(string search)
        {
            listWeathers = await weatherControl.ManyDaysWeather(search, 3);
            return PartialView("ShowDailyWeather", listWeathers);
        }

        public async Task<ActionResult> ShowWeekWeather(string search)
        {
            listWeathers = await weatherControl.ManyDaysWeather(search, 7);
            return PartialView("ShowDailyWeather", listWeathers);
        }


    }
}