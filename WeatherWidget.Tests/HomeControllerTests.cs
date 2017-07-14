using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Moq;
using NUnit.Framework.Internal;
using NUnit.Framework;
using Weather_Widget.Controllers;
using Weather_Widget.Models;
using Weather_Widget.Models.DBHelper;
using Weather_Widget.Models.Entities;
using List = Weather_Widget.Models.List;


namespace WeatherWidget.Tests
{
    [TestFixture]
    class HomeControllerTests
    {
        private HomeController _home;
        private IUnitOfWork _unit;
        private string cityName = "Kiev";
        private WeatherAPI _weather;
        [SetUp]
        public void SetUp()
        {
            _unit = new UnitOfWork();
            var _root = new RootObject()
            {
                city = new City() { id = 200, country = "Ua" },
                list = new List<List>()
                {
                    new List()
                    {
                        temp = new Temp() {day = 435053},
                        weather = new List<Weather>() {new Weather() {icon = "10d"}}
                    }
                }
            };
            _weather = new WeatherAPI()
            {
                ApiKey = "039d992b6267c9d758a6ded74467c1c9",
                BaseUrl = "http://api.openweathermap.org/data/2.5/forecast/daily"
            };
            //Create fake HttpContext for controller
            var HttpContextBaseMock = new Mock<HttpContextBase>();
            var HttpRequestMock = new Mock<HttpRequestBase>();
            var HttpResponseMock = new Mock<HttpResponseBase>();
            var HttpSessionStateBase = new Mock<HttpSessionStateBase>();
            HttpSessionStateBase.Setup(t => t["history"]).Returns("gsdgsdhsd");
            HttpContextBaseMock.SetupGet(x => x.Request).Returns(HttpRequestMock.Object);
            HttpContextBaseMock.SetupGet(x => x.Response).Returns(HttpResponseMock.Object);
            HttpContextBaseMock.SetupGet(x => x.Session).Returns(HttpSessionStateBase.Object);

            //Setup home controller
            _home = new HomeController(_unit, _weather);
            _home.ControllerContext = new ControllerContext(HttpContextBaseMock.Object, new RouteData(),_home);
            _unit.Repository<ElectCity>().Remove(_unit.Repository<ElectCity>().Get(t => t.Name == cityName));
            _unit.Repository<Log>().Remove(_unit.Repository<Log>().Get(t => t.Session == "gsdgsdhsd"));
        }
        [Test]
        public void Detail_When_request_wrong_city_Then_catch_error()
        {
            var detailView = _home.Detail(",,,,,,,,").Result;
            Assert.IsNotNull(detailView);
            Assert.That(detailView.ViewBag.IsError, Is.True);
            Assert.That(_unit.Repository<Log>().Get(t=>t.Session== "gsdgsdhsd"),Is.Null);
        }
        [Test]
        public void Detail_When_request_real_city_Then_add_to_db()
        {
            var detailView = _home.Detail(cityName).Result;
            var session = (string)_home.HttpContext.Session["history"];
            var a = detailView.Model;
            Assert.That(detailView.ViewBag.IsError, Is.False);
            var root = detailView.Model as RootObject;
            Assert.NotNull(root);
            Assert.NotNull(session);
            Assert.NotNull(_unit.Repository<Log>().Get(t => t.Session == "gsdgsdhsd"));
           
        }
        [Test]
        public void Detail_When_request_real_city_Then_add_log_to_history()
        {
            _home.Detail(cityName).Wait();
            _home.Detail("London").Wait();
            var session = (string)_home.HttpContext.Session["history"];
            var history = _home.History() as ViewResult;
            Assert.NotNull(history);
            Assert.That((history.Model as Log).WeatherInfo.Count, Is.EqualTo(2));
        }
        [Test]
        public void Index_When_add_city_to_elected_list_Then_show_elected_city_list_in_page()
        {
            string newCity = "Berlin";
            var elected = new ElectCityController(_unit);

            elected.AddElectTown(newCity);
            var detailView = _home.Index();
            var list = detailView.Model as IEnumerable<ElectCity>;

            Assert.IsNotNull(list);
            Assert.IsNotNull(list.FirstOrDefault(t=>t.Name==newCity));
        }

        [Test]
        public void Index_When_delete_city_to_elected_list_Then_change_elected_city_list_in_page()
        {
            string newCity = "Berlin";
            var elected = new ElectCityController(_unit);

            var id = _unit.Repository<ElectCity>().Get(t => t.Name == newCity).Id;
            elected.RemoveElectTown(id);

            var detailView = _home.Index();
            var list = detailView.Model as IEnumerable<ElectCity>;

            Assert.IsNotNull(list);
            Assert.IsNull(list.FirstOrDefault(t => t.Name == newCity));
        }

        [Test]
        public void Index_When_edit_city_to_elected_list_Then_update_elected_city_list_in_page()
        {
            string oldCity = "Berlin";
            string editCity = "Pekin";

            var elected = new ElectCityController(_unit);
            elected.AddElectTown(oldCity);

            var id = _unit.Repository<ElectCity>().Get(t => t.Name == oldCity).Id;
            elected.EditElectTown(id, editCity);

            var detailView = _home.Index();
            var list = detailView.Model as IEnumerable<ElectCity>;
            var town = list.FirstOrDefault(t => t.Id == id);

            Assert.IsNotNull(town);
            Assert.That(town.Name,Is.EqualTo(editCity));
        }
    }
}
