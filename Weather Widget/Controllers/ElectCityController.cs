using System.Linq;
using System.Web.Mvc;
using Weather_Widget.Models;
using Weather_Widget.Models.Entities;

namespace Weather_Widget.Controllers
{
    public class ElectCityController : Controller
    {
        IUnitOfWork _uw;
        public ElectCityController(IUnitOfWork unit)
        {
            _uw = unit;
        }

        public ActionResult Index()
        {
            return View(_uw.Repository<ElectCity>().Data.ToList());
        }

        [HttpPost]
        public ActionResult RemoveElectTown(int id)
        {
            _uw.Repository<ElectCity>().Remove(_uw.Repository<ElectCity>().Get(t => t.Id == id));
            return Json(new { status = 200 });
        }

        [HttpPost]
        public ActionResult AddElectTown(string name)
        {
            var city = _uw.Repository<ElectCity>().Data.FirstOrDefault(t => t.Name.ToLower() == name.ToLower());
            if (city==null)
            {
                _uw.Repository<ElectCity>().Add(new ElectCity { Name = name });
                return Json(new { status = 200,id = _uw.Repository<ElectCity>().Data.First(t => t.Name==name).Id });
            }
            return Json(new { status = 302});
        }
        [HttpPost]
        public ActionResult EditElectTown(int id,string name)
        {
            var town = _uw.Repository<ElectCity>().Get(t => t.Id == id);
            town.Name = name;
            _uw.Repository<ElectCity>().Edit(town);
            return Json(new { status = 200 });
        }
    }
}