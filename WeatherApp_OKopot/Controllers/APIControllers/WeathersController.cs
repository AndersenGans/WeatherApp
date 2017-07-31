using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Http;
using WeatherApp_OKopot.BLL.Infrastructure;
using WeatherApp_OKopot.BLL.Interfaces;

namespace WeatherApp_OKopot.Controllers.APIControllers
{
    public class WeathersController : ApiController
    {
        private readonly string key = WebConfigurationManager.AppSettings["WeatherKey"];

        private readonly IService service;

        public WeathersController(IService service)
        {
            this.service = service;
        }

        // GET: api/Weathers/Kharkiv/3
        public async Task<IHttpActionResult> Get(string cityName, int countOfDays)
        {
            try
            {
                await service.GetManyDaysWeathers(cityName, key, countOfDays);
                var result = service.FindWeathers(cityName);
                return Json(result);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //// POST: api/Weathers
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT: api/Weathers/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE: api/Weathers/5
        //public void Delete(int id)
        //{
        //}
    }
}
