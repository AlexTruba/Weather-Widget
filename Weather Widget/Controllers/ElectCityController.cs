using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Weather_Widget.Models;
using Weather_Widget.Models.Entities;

namespace Weather_Widget.Controllers
{
    public class ElectCityController : Controller
    {
        UnitOfWork _uw;
        public ElectCityController()
        {
            _uw = new UnitOfWork();
        }

        public ActionResult Index()
        {
            return View(_uw.ElectCityRepository.Data.ToList());
        }

        [HttpPost]
        public ActionResult RemoveElectTown(int id)
        {
            _uw.ElectCityRepository.Remove(id);
            return Json(new { status = 200 });
        }

        [HttpPost]
        public ActionResult AddElectTown(string name)
        {
            var city = _uw.ElectCityRepository.Data.FirstOrDefault(t => t.Name.ToLower() == name.ToLower());
            if (city==null)
            {
                _uw.ElectCityRepository.Add(new ElectCity() { Name = name });
                return Json(new { status = 200,id = _uw.ElectCityRepository.Data.First(t => t.Name==name).Id });
            }
            else
            {
                return Json(new { status = 302});
            }
            
        }
        [HttpPost]
        public ActionResult EditElectTown(int id,string name)
        {
            _uw.ElectCityRepository.Edit(new ElectCity() { Id = id,Name = name});
            return Json(new { status = 200 });
        }
    }
}