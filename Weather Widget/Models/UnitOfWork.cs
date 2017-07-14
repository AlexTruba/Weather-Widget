using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Weather_Widget.Infrastructure;

namespace Weather_Widget.Models
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly LogContext _context = new LogContext();

        public IRepository<T> Repository<T>() where T : class
        {
            return new Repository<T>(_context);
        }

        public void Save() => _context.SaveChanges();
        public void Dispose() => _context.Dispose();
    }
}