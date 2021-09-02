using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace OTB_SEGURA.Models
{
    public class UserModel
    {
        [PrimaryKey, AutoIncrement]
        public Guid UserId { get; set; }//eliminar despues cuando se deje de utilizar firebase
        public int User_ID { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }//eliminar despues cuando se deje de utilizar firebase
        public string Password { get; set; }
        public int Cell_phone { get; set; }
        public byte[] Photo { get; set; }
        public byte State { get; set; }
        public int Ci { get; set; }
        public string StateColor { get; set; }
        public string StateBorderColor { get; set; }
        public int Type { get; set; }
        public string Email { get; set; }
    }
}
