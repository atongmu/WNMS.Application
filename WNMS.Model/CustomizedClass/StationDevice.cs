using System;
using System.Collections.Generic;
using System.Text;

namespace WNMS.Model.CustomizedClass
{
    public class StationDevice
    {
        public int StationId { get; set; }
        public string StationName { get; set; }
        public long DeviceId { get; set; }
        public string DeviceName { get; set; }
        public int EquType { get; set; }
        public int Total { get; set; }
    }
}
