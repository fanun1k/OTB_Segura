using OTB_SEGURA.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace OTB_SEGURA.Models
{
    public class AlertModel:BaseViewModel
    {
        public int Alert_ID { get; set; }
        public DateTime Date { get; set; }
        public int State { get; set; }
        public int Alert_type_ID { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int Otb_ID { get; set; }
        public int User_ID { get; set; }
        public string Message { get; set;}
    }
}
