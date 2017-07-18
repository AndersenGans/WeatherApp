using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WeatherApp_OKopot.BLL.Interfaces;

namespace WeatherApp_OKopot.Controllers.APIControllers
{
    public class HistoriesController : ApiController
    {
        private readonly IService service;
        public HistoriesController(IService service)
        {
            this.service = service;
        }
        // GET: api/Histories
        public IHttpActionResult Get()
        {
            var result = service.GetHistories();
            if (!result.Any())
                return Ok("There are no history in the database");
            return Json(result);
        }
        
        // POST: api/Histories
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Histories/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Histories/5
        public void Delete(int id)
        {
        }
    }
}
