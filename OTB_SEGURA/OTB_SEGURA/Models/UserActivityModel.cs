using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace OTB_SEGURA.Models
{
    // Modelo de las actividades de los usuarios
    public class UserActivityModel
    {
        public Guid UserId { get; set; }
        [MaxLength(60)]
        public string Name { get; set; }
        [MaxLength(100)]
        public string Message { get; set; }
        [MaxLength(20)]
        public string Type { get; set; }
        [MaxLength(50)]
        public double Latitude { get; set; }
        [MaxLength(50)]
        public double Longitude { get; set; }
        [MaxLength(50)]
        public DateTime DateTime { get; set; }
    }
}
