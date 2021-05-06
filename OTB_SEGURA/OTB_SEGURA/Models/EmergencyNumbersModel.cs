using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace OTB_SEGURA.Models
{
    public class EmergencyNumbersModel
    {
        [PrimaryKey, AutoIncrement]
        public Guid NumberId { get; set; }
        public string Name { get; set; }
        public int Number { get; set; }
        public byte State { get; set; }

    }
}
