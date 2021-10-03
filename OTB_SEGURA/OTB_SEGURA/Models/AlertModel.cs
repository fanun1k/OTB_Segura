using SQLite;
using OTB_SEGURA.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace OTB_SEGURA.Models
{
    public class AlertModel
    {
        public int Alert_ID { get; set; }
        [MaxLength(30)]
        public DateTime Date { get; set; }
        [MaxLength(1)]
        public int State { get; set; }
        [MaxLength(20)]
        public int Alert_type_ID { get; set; }
        [MaxLength(50)]
        public double Latitude { get; set; }
        [MaxLength(50)]
        public double Longitude { get; set; }
        [MaxLength(50)]
        public int Otb_ID { get; set; }
        [MaxLength(100)]
        public int User_ID { get; set; }
        [MaxLength(100)]
        public string Message { get; set; }
    }
}
