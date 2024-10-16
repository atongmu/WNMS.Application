using System;
using System.Collections.Generic;

namespace WNMS.Model.DataModels
{
    public partial class SwsDeviceInfo01
    {
        public long DeviceId { get; set; }
        public string DeviceName { get; set; }
        public string DeviceNum { get; set; }
        public byte Partition { get; set; }
        public int StationId { get; set; }
        public int DeviceType { get; set; }
        public int Frequency { get; set; }
        public string ImageUrl { get; set; }
        public int Manufacturer { get; set; }
        public int PumpNum { get; set; }
        public string PumpType { get; set; }
        public int Gui { get; set; }
        public string ImportDn { get; set; }
        public string ExportDn { get; set; }
        public DateTime? ManufactureDate { get; set; }
        public int? Rtuid { get; set; }
        public DateTime? CreateTime { get; set; }
    }
}
