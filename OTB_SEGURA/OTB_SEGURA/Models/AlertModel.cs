using System;
using System.Collections.Generic;
using System.Text;

namespace OTB_SEGURA.Models
{
    public class AlertModel
    {
        public int Alert_ID { get; set; }
        public DateTime Date { get; set; }
        public int State { get; set; }
        public int Alert_type_ID { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public int Otb_ID { get; set; }
        public int User_ID { get; set; }
        public string Message { get; set; }
    }
}
