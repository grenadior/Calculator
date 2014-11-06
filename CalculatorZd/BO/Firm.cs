using System;

namespace BO
{
    public class Firm
    {
        //[MapField("ID"), PrimaryKey, NonUpdatable]
        public Guid ID { get; set; }

        public string Login { get; set; }

        public string AdminEmail { get; set; }
        public string MD5 { get; set; }
        public string FirmName { get; set; }
        public string OKPO { get; set; }
        public string INN { get; set; }
        public string Address { get; set; }
        public string PostAddress { get; set; }
        public string Phone { get; set; }
        public string Phone2 { get; set; }
        public string Fax { get; set; }
        public string Fax2 { get; set; }
        public string FIODirector { get; set; }
        public string FIOBuh { get; set; }
        public string FIOContact { get; set; }
        public string ContactPhone { get; set; }
        public string ContactMobile { get; set; }
        public string ContactEmail { get; set; }
        public DateTime PasswordExpDate { get; set; }
        public string PwdSalt { get; set; }
        public bool IsNonActivated { get; set; }
    }
}