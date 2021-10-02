using System;
using System.Collections.Generic;
using System.Text;

namespace OTB_SEGURA.Models
{
    public class CompleteAlertModel
    {
        public int Alert_ID { get; set; }
        public DateTime Date { get; set; }
        public int State { get; set; }
        public string Alert_type_Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int Otb_ID { get; set; }
        public string User_Name { get; set; }
        public string Message { get; set; }
    }
}
