using SQLite;
using System;

namespace OTB_SEGURA.Models
{
    public class SessionModel
    {
        [PrimaryKey, AutoIncrement,NotNull]
        public int User_ID { get; set; }
        [MaxLength(50), NotNull]
        public string Name { get; set; }
        [NotNull]
        public int Cell_phone { get; set; }
        public byte[] Photo { get; set; }
        [MaxLength(1), NotNull]
        public byte State { get; set; }
        [MaxLength(1), NotNull]
        public int Type { get; set; }
        [MaxLength(50), NotNull]
        public string Email { get; set; }
        public Nullable<int> Otb_ID { get; set; }
        [NotNull]
        public string Token { get; set; }
    }
}
