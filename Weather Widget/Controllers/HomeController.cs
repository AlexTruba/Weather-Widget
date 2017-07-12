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
        UnitOfWork _uw;

        public HomeController(IWeather weather)
        {
            _weather = weather;
            _uw = new UnitOfWork();
        }

        public ActionResult Index()
        {
            return View(_uw.ElectCityRepository.Data.ToList());
        }

        public ActionResult History()
        {
            var a = _uw.LogRepository.Data.ToList();
            return View(a.FirstOrDefault(t => t.Session == (string)Session["history"]));
        }

        [HttpGet]
        public async Task<ActionResult> Detail(string cityName)
        {
            if (ControllerContext.HttpContext.Session["history"] == null)
            {
                ControllerContext.HttpContext.Session["history"] = Guid.NewGuid().ToString("N");
            }
            var rt = await _weather.GetInfoAsync(cityName);
            LogChangeHelp lh = new LogChangeHelp();
            lh.ChangeDB(cityName, rt, (string)Session["history"], _uw.LogRepository);
            ViewBag.City = cityName;
            ViewBag.IsError = _weather.IsError;
            return View(rt);
        }

        [HttpPost]
        public  ActionResult RemoveOneLog(int id)
        {
            _uw.LogRepository.RemoveWeather(id);
            return Json(new {status = 200 });
        }
    }
}