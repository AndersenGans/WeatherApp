using System.Collections.Generic;

namespace WeatherApp_OKopot.BLL.BusinessModels
{
    public class RootObject
    {
        public string cod { get; set; }
        public double message { get; set; }
        public CityAPI city { get; set; }
        public int cnt { get; set; }
        public List<ListAPI> list { get; set; }
    }
}