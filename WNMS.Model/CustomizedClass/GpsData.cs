using System;
using System.Collections.Generic;
using System.Text;

namespace WNMS.Model.CustomizedClass
{
   public class GpsData
    {
        public long UserID { get; set; }
        public DateTime UpdateTime { get; set; }
        public double Lng { get; set; }
        public double Lat { get; set; }
    }
}
