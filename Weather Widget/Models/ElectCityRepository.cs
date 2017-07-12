using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Weather_Widget.Infrastructure;
using Weather_Widget.Models.Entities;

namespace Weather_Widget.Models
{
    public class ElectCityRepository : IRepository<ElectCity>
    {
        private LogContext _context;
        public IQueryable<ElectCity> Data => _context.ElectCities;
        
        public ElectCityRepository(LogContext lg)
        {
            _context = lg;
        }
        public void Add(ElectCity entry)
        {
            try
            {
                _context.ElectCities.Add(entry);
                _context.SaveChanges();

            }
            catch { throw; }
        }

        public void Edit(ElectCity entry)
        {
            var city = Data.FirstOrDefault(t => t.Id == entry.Id);
            city.Name = entry.Name;
            if (city != null)
            {
                _context.Entry(city).State = System.Data.Entity.EntityState.Modified;
                _context.SaveChanges();
            }
        }

        public void Remove(int id)
        {
            try
            {
                _context.ElectCities.Remove(_context.ElectCities.First(t => t.Id == id));
                _context.SaveChanges();
            }
            catch { throw; }
        }
    }
}