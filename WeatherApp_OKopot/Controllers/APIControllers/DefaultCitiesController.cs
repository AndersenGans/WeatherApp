using System.Web.Http;
using WeatherApp_OKopot.BLL.Infrastructure;
using WeatherApp_OKopot.BLL.Interfaces;

namespace WeatherApp_OKopot.Controllers.APIControllers
{
    public class DefaultCitiesController : ApiController
    {
        private readonly IService service;
        
        public DefaultCitiesController(IService service)
        {
            this.service = service;
        }

        // GET: api/DefaultCities
        public IHttpActionResult Get()
        {
            var result = service.FindCitiesToAddToMainList();
            return Json(result);
        }

        // POST: api/DefaultCities
        //public IHttpActionResult Post([FromBody] FormData data)
        //{
        //}

        // PUT: api/DefaultCities/Kharkiv
        public IHttpActionResult Put(string cityName)
        {
            try
            {
                service.AddCityToMainList(cityName);
                var city = service.GetCityByName(cityName);
                return Ok(city);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/DefaultCities/Kharkiv
        public IHttpActionResult Delete(string cityName)
        {
            try
            {
                service.DeleteCitiesFromMainList(cityName);
                var city = service.GetCityByName(cityName);
                return Ok(city);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
