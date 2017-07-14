using System.Collections.Generic;

namespace WeatherApp_OKopot.DAL.Entities
{
    public class City
    {
        public int CityId { get; set; }
        public string Name { get; set; }
        public string AlternativeName { get; set; }
        public bool AddToMainList { get; set; }

        public virtual ICollection<Weather> Weathers { get; set; }
        public virtual ICollection<History> Histories { get; set; }

        public City()
        {
            Weathers = new HashSet<Weather>();
            Histories = new HashSet<History>();
        }
    }
}
