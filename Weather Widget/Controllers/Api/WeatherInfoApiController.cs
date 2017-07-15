using Newtonsoft.Json;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Weather_Widget.Models;

namespace Weather_Widget.Controllers.Api
{
    public class WeatherInfoApiController : ApiController
    {
        // GET: api/ElectCities/5
        [HttpGet]
        public async Task<HttpResponseMessage> GetItem(string city,int day)
        {
            if (city == null || (day<0)) return this.Request.CreateResponse(HttpStatusCode.BadRequest);

            var weather = new WeatherAPI();
            var rt = await weather.GetInfoAsync(city);
            if (weather.IsError) return this.Request.CreateResponse(HttpStatusCode.NotFound);
            
            //I assume that the application can find the weather for 7 days 
            int dayCount = day > 7 ? 7 : day;
            rt.list = rt.list.Take(dayCount).ToList();

            var response = this.Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(JsonConvert.SerializeObject(rt), Encoding.UTF8, "application/json");
            return response;
        }
    }
}
