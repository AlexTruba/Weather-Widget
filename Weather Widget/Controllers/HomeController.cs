using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Weather_Widget.Models;
using Weather_Widget.Models.Entities;

namespace Weather_Widget.Controllers
{
    public class HomeController : Controller
    {
        IWeather _weather;
        IRepository<Log> _log;
        public HomeController(IWeather weather, IRepository<Log> log)
        {
            _weather = weather;
            _log = log;
           
        }
        public ActionResult Index()
        {
            if (ControllerContext.HttpContext.Session["history"] == null)
            {
                ControllerContext.HttpContext.Session["history"] = Guid.NewGuid().ToString("N");
            }
            return View();
        }
        public ActionResult History()
        {
            var a = _log.Data.ToList();
            return View(a.FirstOrDefault(t => t.Session == (string)Session["history"]));
        }
        [HttpGet]
        public async Task<ActionResult> Detail(string cityName)
        {

            var rt = await _weather.GetInfoAsync(cityName);
            ChangeDB(cityName, rt,(string)Session["history"]);
            ViewBag.City = cityName;
            ViewBag.IsError = _weather.IsError;
            return View(rt);
        }

        [HttpPost]
        public  ActionResult RemoveOneLog(int id)
        {
            _log.RemoveWeather(id);
            return Json(new {status = 200 });
        }
        private void ChangeDB(string cityName, RootObject rt, string session)
        {
            Log currLog = new Log();
            var newCity = new Town()
            {
                Code = rt.city.id,
                Country = rt.city.country,
                Name = cityName
            };
            var weather = new WeatherInfo()
            {
                Temp = rt.list.First().temp.day,
                Weather = rt.list.First().weather.First().icon,
                Town = newCity
            };
            if (_log.Data.FirstOrDefault(t => t.Session == session) == null)
            {
                currLog.Session = session;
                currLog.WeatherInfo.Add(weather);
                _log.Add(currLog);
            }
            else
            {
                currLog = _log.Data.First(t => t.Session == session);
                currLog.WeatherInfo.Add(weather);
                _log.Edit(currLog);
            }
        }
    }
}