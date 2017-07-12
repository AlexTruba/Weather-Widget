using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Timers;
using System.Web;
using Weather_Widget.Models.Entities;

namespace Weather_Widget.Infrastructure
{
    public class LogContext:DbContext
    {
        private static Timer _removeSession = new Timer();
        public LogContext():base("History")
        {
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
          
            base.OnModelCreating(modelBuilder);
            Database.SetInitializer<LogContext>(new DropCreateDatabaseIfModelChanges<LogContext>());
            //Remove Session from DB after 1 hour
            _removeSession.Interval = TimeSpan.FromHours(1).TotalMilliseconds;
            _removeSession.AutoReset = false;
            _removeSession.Elapsed += (sender, e) =>
            {
                foreach (var item in Log)
                {
                    if (item.SessionStart.AddHours(1) > DateTime.Now) Log.Remove(item);
                }
            };


        }
        public DbSet<Log> Log { get; set; }
        public DbSet<WeatherInfo> WeatherInfo { get; set; }
        public DbSet<Town> Towns { get; set; }
        public DbSet<ElectCity> ElectCities { get; set; }
    }
}