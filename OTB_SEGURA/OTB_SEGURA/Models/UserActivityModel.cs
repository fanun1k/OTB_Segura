using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace OTB_SEGURA.Models
{
    // Modelo de las actividades de los usuarios
    public class UserActivityModel
    {
        [PrimaryKey, AutoIncrement]
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Message { get; set; }
        public string Type { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime DateTime { get; set; }
    }
}
