using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Moq;
using Weather_Widget.Controllers;
using Weather_Widget.Models;
using Weather_Widget.Models.Entities;

namespace WeatherWidget.Tests
{
    [TestFixture]
    class ElectCityControllerTest
    {
        private IUnitOfWork _unit;
        private ElectCityController _electCity;
        private string cityName = "Mexoki";
        [SetUp]
        public void SetUp()
        {
            _unit = new UnitOfWork();
            _unit.Repository<ElectCity>();
            _electCity = new ElectCityController(_unit);
            _unit.Repository<ElectCity>().Remove(_unit.Repository<ElectCity>().Get(t => t.Name == cityName));
        }

        [Test]
        public void AddElectTown_When_city_is_new_Then_status_200()
        {
            var a = (JsonResult)_electCity.AddElectTown(cityName);
            dynamic result = a.Data;
            Assert.That(result.status,Is.EqualTo(200));
            Assert.That(a,Is.TypeOf(typeof(JsonResult)));
        }
        [Test]
        public void AddElectTown_When_db_contain_city_Then_status_302()
        {
            _electCity.AddElectTown(cityName);
            var a = (JsonResult)_electCity.AddElectTown(cityName);
            dynamic result = a.Data;
            Assert.That(result.status, Is.EqualTo(302));
            Assert.That(a, Is.TypeOf(typeof(JsonResult)));
        }
    }
}
