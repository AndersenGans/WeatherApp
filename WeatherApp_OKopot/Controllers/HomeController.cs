using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using WeatherApp_OKopot.Infrastructure;
using WeatherApp_OKopot.Services;

namespace WeatherApp_OKopot.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWeatherControl weatherControl;
        private WeatherDBContext dbContext;
        
        public HomeController(IWeatherControl weatherControl)
        {
            this.weatherControl = weatherControl;
            dbContext = new WeatherDBContext();
            var listOfCities = dbContext.CityEntities.Where(item => item.AddToMainList);
            ViewBag.ListOfCities = new SelectList(listOfCities, "Name", "Name");
        }
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> ShowDailyWeather(string search, bool addToMainList)
        {
            await weatherControl.DailyWeather(search, addToMainList);
            var listOfWeathers = dbContext.WeatherEntities.Where(item => item.CityEntity.Name == search);
            return PartialView(listOfWeathers.ToList());
        }

        public async Task<ActionResult> ShowThreeDaysWeather(string search)
        {
            await weatherControl.ManyDaysWeather(search, 3);
            var listOfWeathers = dbContext.WeatherEntities.Where(item => item.CityEntity.Name == search);
            return PartialView("ShowDailyWeather", listOfWeathers.ToList());
        }

        public async Task<ActionResult> ShowWeekWeather(string search)
        {
            await weatherControl.ManyDaysWeather(search, 7);
            var listOfWeathers = dbContext.WeatherEntities.Where(item => item.CityEntity.Name == search);
            return PartialView("ShowDailyWeather", listOfWeathers.ToList());
        }

        public ActionResult HistoryOfWeather()
        {
            return View(dbContext.HistoryEntities.ToList());
        }

        public ActionResult DeleteCity(string cityName)
        {
            var delCity = dbContext.CityEntities.Where(item => item.Name == cityName);
            dbContext.CityEntities.RemoveRange(delCity);
            dbContext.SaveChanges();
            return View("Index");
        }

        public ActionResult ClearHistory()
        {
            dbContext.HistoryEntities.RemoveRange(dbContext.HistoryEntities);
            dbContext.SaveChanges();
            return View("HistoryOfWeather",dbContext.HistoryEntities.ToList());
        }
        

    }
}