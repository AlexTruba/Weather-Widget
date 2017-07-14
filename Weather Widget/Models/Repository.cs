using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using Weather_Widget.Infrastructure;

namespace Weather_Widget.Models
{
    public class Repository<T> : IRepository<T> where T:class
    {
        private readonly DbContext _dbContext;

        private readonly DbSet<T> _dbSet;
        public IQueryable<T> Data => _dbSet;
        public Repository(LogContext log)
        {
            _dbContext = log;
            _dbSet = _dbContext.Set<T>();
        }
        public void Add(T entry)
        {
            _dbSet.Add(entry);
            _dbContext.SaveChanges();
        }

        public void Edit(T entry)
        {
            //_dbContext.Set<T>().AddOrUpdate(entry);
            try
            {
                _dbSet.Attach(entry);
                _dbContext.Entry<T>(entry).State = EntityState.Modified;
                _dbContext.SaveChanges();
            }
            catch (Exception e) { }
           
        }

        public T Get(Func<T, bool> predicate)
        {
            return _dbSet.FirstOrDefault(predicate);
        }

        public void Remove(T entry)
        {
            if (entry!=null)
            {
                _dbSet.Remove(entry);
                _dbContext.SaveChanges();
            }
        }
    }
}