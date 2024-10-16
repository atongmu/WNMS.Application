using System;
using System.Collections.Generic;
using System.Text;

namespace WNMS.Model.CustomizedClass
{
    public class StationAndDevice
    {
        public int StationId { get; set; } 
        public string StationName { get; set; }
        public long DeviceId { get; set; }
        public string DeviceName { get; set; }
        public byte Partition { get; set; }
        public int RTUID { get; set; }
    }
}
