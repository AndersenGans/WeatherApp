using System.Collections.Generic;

namespace WeatherApp_OKopot.BLL.BusinessModels
{
    public class ListAPI
    {
        public int dt { get; set; }
        public TempAPI temp { get; set; }
        public double pressure { get; set; }
        public int humidity { get; set; }
        public List<WeatherAPI> weather { get; set; }
        public double speed { get; set; }
        public int deg { get; set; }
        public int clouds { get; set; }
        public double? rain { get; set; }
        public double? snow { get; set; }
    }
}