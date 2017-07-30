using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace WeatherApp_OKopot
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();
            var corsAttr = new EnableCorsAttribute("http://localhost:4200", "*", "*");
            config.EnableCors(corsAttr);

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
