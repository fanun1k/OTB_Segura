using System;
using System.Collections.Generic;
using System.Text;

namespace OTB_SEGURA.Models
{
    //Modelo de Actividades
    public class ActivityModel
    {
        public string Message { get; set; }
        public string Type { get; set; }
        public string UserId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime DateTime { get; set; }
    }
}
