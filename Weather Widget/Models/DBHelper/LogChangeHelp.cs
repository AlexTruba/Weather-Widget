using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Weather_Widget.Models.Entities;

namespace Weather_Widget.Models.DBHelper
{
    public class LogChangeHelp
    {
        public void ChangeDB(string cityName, RootObject rt, string session,IRepository<Log> log)
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
            if (log.Data.FirstOrDefault(t => t.Session == session) == null)
            {
                currLog.Session = session;
                currLog.WeatherInfo.Add(weather);
                log.Add(currLog);
            }
            else
            {
                currLog = log.Data.First(t => t.Session == session);
                currLog.WeatherInfo.Add(weather);
                log.Edit(currLog);
            }
        }
    }
}