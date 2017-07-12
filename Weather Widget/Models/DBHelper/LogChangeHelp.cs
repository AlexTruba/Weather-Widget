using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Weather_Widget.Models.Entities;

namespace Weather_Widget.Models.DBHelper
{
    public class LogChangeHelp
    {
        public void ChangeDB(string cityName, RootObject rt, string session,LogRepository _log)
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