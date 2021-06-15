using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace OTB_SEGURA.Models
{
    public class UserModel
    {
        [PrimaryKey, AutoIncrement]
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int Phone { get; set; }
        public byte[] Photo { get; set; }
        public byte State { get; set; }
        public int Ci { get; set; }
        public string StateColor { get; set; }
        public string StateBorderColor { get; set; }
        public int UserType { get; set; }
    }
}
