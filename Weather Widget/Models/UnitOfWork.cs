using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Weather_Widget.Infrastructure;

namespace Weather_Widget.Models
{
    public class UnitOfWork : IDisposable
    {
        private bool disposed = false;
        private LogContext db = new LogContext();
        private LogRepository _log;
        private ElectCityRepository _elect;

        public LogRepository LogRepository
        {
            get
            {
                if (_log == null) _log = new LogRepository(db);
                return _log;
            }
        }

        public ElectCityRepository ElectCityRepository
        {
            get
            {
                if (_elect == null) _elect = new ElectCityRepository(db);
                return _elect;
            }
        }

        public void Save() => db.SaveChanges();

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing) db.Dispose();
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}