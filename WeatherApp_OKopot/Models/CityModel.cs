using System.Collections;
using System.Collections.Generic;

namespace WeatherApp_OKopot.Models
{
    public class CityModel
    {
        public int CityId { get; set; }
        public string Name { get; set; }
        public string AlternativeName { get; set; }
        public bool AddToMainList { get; set; }

        public virtual ICollection<WeatherModel> Weathers { get; set; }
    }
}