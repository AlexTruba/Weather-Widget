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
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> Detail(string cityName)
        {
            var rt =  await WeatherAPI.GetInfo(cityName);
            @ViewBag.City = cityName;
            ViewBag.IsError = WeatherAPI.IsError;
            return View(rt);
        }
    }
}