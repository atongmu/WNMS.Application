using System;
using System.Collections.Generic;
using System.Text;

namespace WNMS.Model.CustomizedClass
{
    public class LocusUser
    {
        public int UserID { get; set; }
        public string NickName { get; set; }
        public string DptName { get; set; }
        public string SerialNumber { get; set; }
        public DateTime BorrowTime { get; set; }
        public DateTime RemandTime { get; set; }
        public string Phone { get; set; }
    }
}
