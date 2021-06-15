using System;
using System.Collections.Generic;
using System.Text;

namespace OTB_SEGURA.Models
{
    class EmergencyModel
    {
        public string UserId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime DateTime { get; set; }
    }
}
