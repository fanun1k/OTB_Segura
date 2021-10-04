using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace OTB_SEGURA.Models
{
    public class AlertTypeModel
    {
        [PrimaryKey]
        public int Alert_type_ID { get; set; }
        [MaxLength(60)]
        public string Name { get; set; }
        [MaxLength(1)]
        public int State { get; set; }
        public int Obt_ID { get; set; }
    }
}
