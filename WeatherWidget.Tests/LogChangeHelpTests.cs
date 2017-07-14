using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Weather_Widget.Infrastructure;
using Weather_Widget.Models;
using Weather_Widget.Models.DBHelper;
using Weather_Widget.Models.Entities;
using List = Weather_Widget.Models.List;

namespace WeatherWidget.Tests
{

    [TestFixture]
    class LogChangeHelpTests
    {
        LogChangeHelp _logChange;
        RootObject _root;
        string session = "123523tgsdhger634t";
        LogContext context = new LogContext();

        [SetUp]
        public void WeatherSetUp()
        {
            _logChange = new LogChangeHelp();
            _root = new RootObject()
            {
                city = new City() {id = 200, country = "Ua"},
                list = new List<List>()
                {
                    new List()
                    {
                        temp = new Temp() {day = 435053},
                        weather = new List<Weather>() {new Weather() {icon = "10d"}}
                    }
                }
            };

            context.Log.Remove(context.Log.First(t => t.Session == "123523tgsdhger634t"));
            context.SaveChanges();
        }

        [Test]
        public void ChangeDB_When_user_first_access_Then_create_new_log()
        {
           _logChange.ChangeDB("kiev",_root,session,new Repository<Log>(context));

            var dbLog = context.Log.FirstOrDefault(t => t.Session == session);
            Assert.NotNull(dbLog);
            Assert.That(dbLog.WeatherInfo.Count,Is.EqualTo(1));
        }
        [Test]
        public void ChangeDB_When_user_not_fisrt_access_Then_update_log()
        {
            _logChange.ChangeDB("kiev", _root, session, new Repository<Log>(context));
            _root.list.First().temp.day = 535;
            _logChange.ChangeDB("kiev", _root, session, new Repository<Log>(context));
            var dbLog = context.Log.FirstOrDefault(t => t.Session == session);
            Assert.NotNull(dbLog);
            Assert.That(context.Log.Count(t => t.Session == session), Is.EqualTo(1));
            Assert.That(dbLog.WeatherInfo.Count, Is.EqualTo(2));
        }
    }
}
