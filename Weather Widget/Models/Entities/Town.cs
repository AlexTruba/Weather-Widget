using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Weather_Widget.Models.Entities
{
    public class Town
    {
        [Key]
        public int Id { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }

        public virtual ICollection<WeatherInfo> WeatherInfo { get; set; } = new List<WeatherInfo>();
    }
}