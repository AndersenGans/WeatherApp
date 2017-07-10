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
            ViewBag.ListOfCities = new SelectList(dbContext.CityEntities, "Name", "Name");
        }
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> ShowDailyWeather(string search)
        {
            await weatherControl.DailyWeather(search);
            var listOfWeathers = dbContext.WeatherEntities.Where(item => item.CityEntity.Name == search);
            return PartialView(listOfWeathers.ToList());
        }

        public async Task<ActionResult> ShowThreeDaysWeather(string search)
        {
            await weatherControl.ManyDaysWeather(search, 3);
            return PartialView("ShowDailyWeather", dbContext.WeatherEntities.ToList());
        }

        public async Task<ActionResult> ShowWeekWeather(string search)
        {
            await weatherControl.ManyDaysWeather(search, 7);
            return PartialView("ShowDailyWeather", dbContext.WeatherEntities.ToList());
        }


    }
}