using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using Newtonsoft.Json;
using Weather_Widget.Models;
using Weather_Widget.Models.Entities;
using System.Web.Http.Cors;

namespace Weather_Widget.Controllers.Api
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class HistoryApiController : ApiController
    {
        readonly IUnitOfWork _unit = new UnitOfWork();
        
        // GET: api/ElectCities/5
        [HttpGet]
        public HttpResponseMessage GetItem(int id)
        {
            HttpResponseMessage response;
            var log = _unit.Repository<Log>().Get(t => t.Id == id);
            if (log == null)
            {
                response = this.Request.CreateResponse(HttpStatusCode.NotFound);
                return response;
            }
            response = this.Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(JsonConvert.SerializeObject(log), Encoding.UTF8, "application/json");
            return response;
        }
        //session string for test: "905d32dacd4e46a1bf81f49e959bad48"
        [HttpPost]
        public HttpResponseMessage GetItem(string session)
        {
            HttpResponseMessage response;
            var log = _unit.Repository<Log>().Get(t => t.Session == session);
            if (log == null)
            {
                response = this.Request.CreateResponse(HttpStatusCode.NotFound);
                return response;
            }
            response = this.Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(JsonConvert.SerializeObject(log), Encoding.UTF8, "application/json");
            return response;
        }
    }
}
