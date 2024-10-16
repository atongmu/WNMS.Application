using System;
using System.Collections.Generic;

namespace WNMS.Model.DataModels
{
    public partial class SwsDeviceInfo03
    {
        public long DeviceId { get; set; }
        public string DeviceNum { get; set; }
        public string DeviceName { get; set; }
        public int? Rtuid { get; set; }
        public byte Partition { get; set; }
        public int StationId { get; set; }
    }
}
