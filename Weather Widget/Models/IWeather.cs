using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weather_Widget.Models
{
    public interface IWeather
    {
        bool IsError { get; set; }
        Task<RootObject> GetInfoAsync(string cityName);
    }
}
