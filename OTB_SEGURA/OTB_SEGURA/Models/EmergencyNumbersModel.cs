using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace OTB_SEGURA.Models
{
    public class EmergencyNumbersModel
    {
        public Guid NumberId { get; set; }
        public string Name { get; set; }
        public int Number { get; set; }
        public byte State { get; set; }
        public string Imagen { get; set; }

     
    }
}
