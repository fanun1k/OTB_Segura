using System;
using System.Collections.Generic;
using System.Text;

namespace OTB_SEGURA.Models
{
    public class AlarmModel
    {
        public int Alarm_ID { get; set; }
        public string Name { get; set; }
        public int State { get; set; }
        public int Otb_ID { get; set; }
    }
}
