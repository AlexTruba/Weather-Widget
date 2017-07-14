using NUnit.Framework;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Weather_Widget.Models;

namespace WeatherWidget.Tests
{
    [TestFixture]
    public class WeatherApiTests
    {
        private WeatherAPI _weather;
        [SetUp]
        public void WeatherSetUp()
        {
            _weather = new WeatherAPI() { ApiKey = "039d992b6267c9d758a6ded74467c1c9",BaseUrl = "http://api.openweathermap.org/data/2.5/forecast/daily"};
 
        }
        [Test]
        public void GetInfoAsync_When_request_wrong_city_Then_return_error_state()
        {
            RootObject rt = _weather.GetInfoAsync(",,,,,,,,,,,,,,,").Result;
            Assert.That(_weather.IsError,Is.True);
            Assert.IsNull(rt);
        }
        [Test]
        public void GetInfoAsync_When_request_real_city_Then_return_weather_for_city()
        {
            RootObject rt = _weather.GetInfoAsync("Лондон").Result;
            Assert.That(_weather.IsError, Is.False);
            Assert.IsNotNull(rt);
        }
    }
}
