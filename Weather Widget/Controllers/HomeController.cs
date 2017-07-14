using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Services;
using Weather_Widget.Infrastructure;
using Weather_Widget.Models;
using Weather_Widget.Models.DBHelper;
using Weather_Widget.Models.Entities;

namespace Weather_Widget.Controllers
{
    public class HomeController : Controller
    {
        IWeather _weather;
        IUnitOfWork _uw;

        public HomeController(IUnitOfWork unit,IWeather weather)
        {
            _weather = weather;
            _uw = unit;
        }

        public ViewResult Index()
        {
            return View(_uw.Repository<ElectCity>().Data.ToList());
        }

        public ActionResult History()
        {
            var a = _uw.Repository<Log>().Data.ToList();
            return View(a.FirstOrDefault(t => t.Session == (string)ControllerContext.HttpContext.Session["history"]));
        }

        [HttpGet]
        public async Task<ViewResult> Detail(string cityName)
        {
            if (ControllerContext.HttpContext.Session["history"] == null)
            {
                ControllerContext.HttpContext.Session["history"] = Guid.NewGuid().ToString("N");
            }
            var rt = await _weather.GetInfoAsync(cityName);

            if (!_weather.IsError)
            {
                LogChangeHelp lh = new LogChangeHelp();
                lh.ChangeDB(cityName, rt, (string)ControllerContext.HttpContext.Session["history"], _uw.Repository<Log>());
            }
            ViewBag.City = cityName;
            ViewBag.IsError = _weather.IsError;
            return View(rt);
        }

        [HttpPost]
        public  ActionResult RemoveOneLog(int id)
        {
            _uw.Repository<WeatherInfo>().Remove(_uw.Repository<WeatherInfo>().Get(t=>t.Id==id));
            return Json(new {status = 200 });
        }
    }
}