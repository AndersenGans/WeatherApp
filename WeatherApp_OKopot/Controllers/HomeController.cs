using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WeatherApp_OKopot.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.ListOfCities = new SelectList(new List<String> { "Киев", "Львов", "Харьков", "Днепропетровск", "Одесса" });
                //new List<String>{"Киев","Львов","Харьков","Днепропетровск","Одесса"};
            return View();
        }

        public ActionResult ShowWeather(string search)
        {

            return View("Index");
        }

        
    }
}