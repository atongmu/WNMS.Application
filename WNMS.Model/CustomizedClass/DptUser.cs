using System;
using System.Collections.Generic;
using System.Text;

namespace WNMS.Model.CustomizedClass
{
    public class DptUser
    {
        public int UserId { get; set; }
        public string Account { get; set; }
        public string NickName { get; set; }
        public int Department { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime? CreateTime { get; set; }
        public bool Gender { get; set; }
        public bool IsEnable { get; set; }
        public string Remark { get; set; }
        public string DptName { get; set; }
        public string SerialNumber { get; set; }
    }
}
