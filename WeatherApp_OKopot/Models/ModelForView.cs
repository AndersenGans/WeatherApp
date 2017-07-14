using System.Collections.Generic;
using System.Web.Mvc;

namespace WeatherApp_OKopot.Models
{
    public class ModelForView
    {
        public List<WeatherModel> Weathers { get; set; }
        public string CityName { get; set; }
        public SelectList ListOfCities { get; set; }
    }
}