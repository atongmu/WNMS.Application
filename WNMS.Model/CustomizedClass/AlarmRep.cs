using System;
using System.Collections.Generic;
using System.Text;

namespace WNMS.Model.CustomizedClass
{
    public class AlarmRep
    {
        public long DeviceId { get; set; }
        public string DeviceName { get; set; }
        public int Num { get; set; }
        public string EventTime { get; set; }
        public string ItemValue { get; set; }

        public string ItemName { get; set; }
 
    }
}
