using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Weather_Widget.Models
{
    public interface IRepository<T> where T: class
    {
        IQueryable<T> Data { get; }
        void Add(T entry);
        void Edit(T entry);
        void Remove(T entry);
        T Get(Func<T, bool> predicate);
    }
}