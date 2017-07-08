using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Weather_Widget.Models;

namespace Weather_Widget.Controllers
{
    public class HomeController : Controller
    {
        IWeather _weather;

        public HomeController(IWeather weather)
        {
            _weather = weather;
        }
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> Detail(string cityName)
        {
            var rt = await _weather.GetInfoAsync(cityName);
            @ViewBag.City = cityName;
            ViewBag.IsError = _weather.IsError;
            return View(rt);
        }
    }
}