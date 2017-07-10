using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Weather_Widget.Models.Entities
{
    public class Log
    {
        [Key]
        public int Id { get; set; }
        public string Session { get; set; }
        public DateTime SessionStart { get; set; } = DateTime.Now;

        public virtual ICollection<WeatherInfo> WeatherInfo { get; set; } = new List<WeatherInfo>();
    }
}