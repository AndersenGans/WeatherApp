using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Mvc;
using AutoMapper;
using WeatherApp_OKopot.BLL.DTO;
using WeatherApp_OKopot.BLL.Infrastructure;
using WeatherApp_OKopot.BLL.Interfaces;
using WeatherApp_OKopot.Models;

namespace WeatherApp_OKopot.Controllers
{
    public class HomeController : Controller
    {
        private readonly IService service;
        private readonly string key = WebConfigurationManager.AppSettings["WeatherKey"];
        private readonly ModelForView modelForView;


        public HomeController(IService service)
        {
            this.service = service;
            modelForView = new ModelForView
            {
                ListOfCities = new SelectList(service.FindCitiesToAddToMainList(), "Name", "Name")
            };
        }

        public ActionResult Index()
        {
            return View(modelForView);
        }

        public async Task<ActionResult> ShowDailyWeatherPartial(string search)
        {
            try
            {
                await service.GetDailyWeathers(search, key, false);
                GetModelForView(search);
                return PartialView("ShowDailyWeatherPartial", modelForView);
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelError(ex.Property, ex.Message);
                return View("Index", modelForView);
            }
        }

        public async Task<ActionResult> ShowDailyWeather(string search, bool addToMainList)
        {
            try
            {
                await service.GetDailyWeathers(search, key, addToMainList);
                GetModelForView(search);
                return View("Index", modelForView);
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelError(ex.Property, ex.Message);
                return View("Index", modelForView);
            }
        }

        public async Task<ActionResult> ShowThreeDaysWeather(string search)
        {
            try
            {
                await service.GetManyDaysWeathers(search, key, 3);
                GetModelForView(search);
                return PartialView("ShowDailyWeatherPartial", modelForView);
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelError(ex.Property, ex.Message);
                return PartialView("ShowDailyWeatherPartial", modelForView);
            }
        }

        public async Task<ActionResult> ShowWeekWeather(string search)
        {
            try
            {
                await service.GetManyDaysWeathers(search, key, 7);
                GetModelForView(search);
                return PartialView("ShowDailyWeatherPartial", modelForView);
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelError(ex.Property, ex.Message);
                return PartialView("ShowDailyWeatherPartial", modelForView);
            }
        }

        public ActionResult HistoryOfWeather()
        {
            var historiesDTO = service.GetHistories();
            var histories = Mapper.Map<IEnumerable<HistoryDTO>, List<HistoryModel>>(historiesDTO);
            return View("HistoryOfWeather", histories);
        }

        public ActionResult DeleteCity(string ListOfCities)
        {
            try
            {
                service.DeleteCitiesFromMainList(ListOfCities);
                modelForView.ListOfCities = new SelectList(service.FindCitiesToAddToMainList(), "Name", "Name");
                return View("Index", modelForView);
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelError(ex.Property, ex.Message);
                return View("Index", modelForView);
            }
        }

        public ActionResult ClearHistory()
        {
            service.DeleteHistories();
            var historiesDTO = service.GetHistories();
            var histories = Mapper.Map<IEnumerable<HistoryDTO>, List<HistoryModel>>(historiesDTO);
            return View("HistoryOfWeather", histories);
        }

        protected override void Dispose(bool disposing)
        {
            service.Dispose();
            base.Dispose(disposing);
        }


        internal void GetModelForView(string search)
        {
            try
            {
                var weathersDTO = service.FindWeathers(search);
                var weathers = Mapper.Map<IEnumerable<WeatherDTO>, List<WeatherModel>>(weathersDTO);
                modelForView.Weathers = weathers;
                modelForView.ListOfCities = new SelectList(service.FindCitiesToAddToMainList(), "Name", "Name");
                modelForView.CityName = service.GetCityByName(search).AlternativeName;
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelError(ex.Property, ex.Message);
            }
        }
    }
}