using SQLite;
using System;
using OTB_SEGURA.Models;

namespace OTB_SEGURA.Models
{
    public class UserModel
    {
        public Guid UserId { get; set; }//eliminar despues cuando se deje de utilizar firebase
        [PrimaryKey, AutoIncrement]
        public int User_ID { get; set; }//--Ss
        [MaxLength(50)]
        public string Name { get; set; }//--ss
        public string UserName { get; set; }//eliminar despues cuando se deje de utilizar firebase
        public string Password { get; set; }
        public int Cell_phone { get; set; }
        public byte[] Photo { get; set; }
        [MaxLength(1)]
        public byte State { get; set; }//--ss
        public int Ci { get; set; }
        [MaxLength(50)]
        public int Type { get; set; }//--ss
        [MaxLength(50)]
        public string Email { get; set; }//--ss
        public Nullable<int> Otb_ID { get; set; }
        public string Token { get; set; }



    }
}
