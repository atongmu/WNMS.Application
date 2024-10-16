using System;
using System.Collections.Generic;
using System.Text;

namespace WNMS.Model.CustomizedClass
{
   public class UserPosition
    {
        public string UpdateTime { get; set; }
        public double Lng { get; set; }
        public double Lat { get; set; }
        public String Name { get; set; }
        public String SerialNumber { get; set; }
        public DateTime RemandTime { get; set; }
    }
}
