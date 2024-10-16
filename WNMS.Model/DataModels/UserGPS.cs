using System;
using System.Collections.Generic;
using System.Text;

namespace WNMS.Model.DataModels
{
    public class UserGPS
    {
        public string UserName { get; set; }
        public DateTime? BorrowTime { get; set; }
        public DateTime? RemandTime { get; set; }
        public string SerialNumber { get; set; }
    }
}
