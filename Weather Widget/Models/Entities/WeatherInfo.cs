using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Weather_Widget.Models.Entities
{
    public class WeatherInfo
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Log")]
        public int LogId { get; set; }
        [ForeignKey("Town")]
        public int TownId { get; set; }
        public double Temp { get; set; }
        public string Weather { get; set; }
        public DateTime Time { get; set; } = DateTime.Now;

        public virtual Town Town { get; set; }
        public virtual Log Log { get; set; }
    }
}