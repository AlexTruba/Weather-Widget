using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using Weather_Widget.Models;
using Weather_Widget.Models.Entities;

namespace Weather_Widget.Controllers.Api
{
    public class ElectCityApiController : ApiController
    {
        readonly IUnitOfWork _unit = new UnitOfWork();

        // GET: api/ElectCities
        [HttpGet]
        public IEnumerable<ElectCity> Get()
        {
            return _unit.Repository<ElectCity>().Data.ToList();
        }
        
        // GET: api/ElectCities/5
        [HttpGet]
        public HttpResponseMessage GetItem(int id)
        {
            HttpResponseMessage response;
            var city = _unit.Repository<ElectCity>().Get(t => t.Id == id);
            if (city == null)
            {
                response = this.Request.CreateResponse(HttpStatusCode.NotFound);
                return response;
            }
            response = this.Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(JsonConvert.SerializeObject(city), Encoding.UTF8,"application/json");

            return response;
        }

        // POST: api/ElectCities
        [HttpPost]
        public HttpResponseMessage Additem([FromBody]ElectCity town)
        {
            if (!String.IsNullOrEmpty(town.Name))
            {
                var electRepo = _unit.Repository<ElectCity>();
                if (electRepo.Get(t => t.Name.Equals(town.Name, StringComparison.OrdinalIgnoreCase)) == null )
                {
                    electRepo.Add(new ElectCity() { Name = town.Name });
                    return this.Request.CreateResponse(HttpStatusCode.OK);
                }
                return this.Request.CreateResponse(HttpStatusCode.Accepted);
            }
            return this.Request.CreateResponse(HttpStatusCode.BadRequest);
        }

        // PUT: api/ElectCities/5
        [HttpPut]
        public HttpResponseMessage EditItem(int id, [FromBody]ElectCity town)
        {
            if (!String.IsNullOrEmpty(town.Name))
            {
                var city = _unit.Repository<ElectCity>().Get(t => t.Id == id);
                if (city != null)
                {
                    city.Name = town.Name;
                    _unit.Repository<ElectCity>().Edit(city);
                    return this.Request.CreateResponse(HttpStatusCode.OK);
                }
                return this.Request.CreateResponse(HttpStatusCode.NotFound);
            }
            return this.Request.CreateResponse(HttpStatusCode.BadRequest);
        }


        // DELETE: api/ElectCities/5
        [HttpDelete]
        public HttpResponseMessage RemoveItem(int id)
        {
            var city = _unit.Repository<ElectCity>().Get(t => t.Id == id);

            if (city != null)
            {
                _unit.Repository<ElectCity>().Remove(city);
                return this.Request.CreateResponse(HttpStatusCode.OK);
            }
            return this.Request.CreateResponse(HttpStatusCode.NotFound);
        }
    }
}
