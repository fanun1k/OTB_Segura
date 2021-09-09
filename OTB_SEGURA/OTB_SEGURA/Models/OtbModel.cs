using System;
using System.Collections.Generic;
using System.Text;

namespace OTB_SEGURA.Models
{
    public class OtbModel
    {
        public int Otb_ID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int User_ID { get; set; } //campo encesario para la creacion de la otb
    }
}
