using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace WeatherApp_OKopot
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "WeathersApi",
                routeTemplate: "api/{controller}/{cityName}/{countOfDays}"
            );

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{cityName}",
                defaults: new { cityName = RouteParameter.Optional } 
            );
        }
    }
}
