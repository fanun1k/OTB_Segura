using SQLite;
using System;
using OTB_SEGURA.Models;

namespace OTB_SEGURA.Models
{
    public class UserModel
    {
        public Guid UserId { get; set; }//eliminar despues cuando se deje de utilizar firebase
        [PrimaryKey]
        public int User_ID { get; set; }
        [MaxLength(60)]
        public string Name { get; set; }
        public string UserName { get; set; }//eliminar despues cuando se deje de utilizar firebase
        [MaxLength(16)]
        public string Password { get; set; }
        [MaxLength(15)]
        public int Cell_phone { get; set; }
        [MaxLength(100)]
        public string Photo { get; set; }
        [MaxLength(1)]
        public byte State { get; set; }
        [MaxLength(15)]
        public string Ci { get; set; }
        [MaxLength(50)]
        public int Type { get; set; }
        [MaxLength(50)]
        public string Email { get; set; }
        public Nullable<int> Otb_ID { get; set; }
        public string Token { get; set; }



    }
}
