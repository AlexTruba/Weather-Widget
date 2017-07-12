using System.Linq;
using Weather_Widget.Infrastructure;
using Weather_Widget.Models.Entities;

namespace Weather_Widget.Models
{
    public class LogRepository : IRepository<Log>
    {
        private LogContext _context;
        public IQueryable<Log> Data => _context.Log;
        public LogRepository(LogContext lg)
        {
            _context = lg;
        }

        public void Add(Log entry)
        {
            try
            {
                _context.Log.Add(entry);
                _context.SaveChanges();
               
            }
            catch { throw; }
        }
        
        public void Edit(Log entry)
        {
            var log = Data.FirstOrDefault(t => t.Id == entry.Id);
            if (log!= null)
            {
                _context.Entry(entry).State = System.Data.Entity.EntityState.Modified;
                _context.SaveChanges();
            }
        }

        public void Remove(int id)
        {
            try
            {
                _context.Log.Remove(_context.Log.First(t => t.Id == id));
                _context.SaveChanges();
            }
            catch { throw; }
        }
        public void RemoveWeather(int id)
        {
            try
            {
                //_context.Database.ExecuteSqlCommand($"DELETE FROM WeatherInfoes WHERE Id={id}");
                _context.WeatherInfo.Remove(_context.WeatherInfo.FirstOrDefault(t => t.Id == id));
                _context.SaveChanges();
            }
            catch { throw; }
        }
    }
}