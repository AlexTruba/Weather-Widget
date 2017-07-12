using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Weather_Widget.Models.Entities
{
    public class ElectCity
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}