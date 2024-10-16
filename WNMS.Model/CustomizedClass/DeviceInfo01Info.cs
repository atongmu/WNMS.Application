using System;
using System.Collections.Generic;
using System.Text;

namespace WNMS.Model.CustomizedClass
{
    public class DeviceInfo01Info
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
        public string DeviceTypeName { get; set; }
        public string FrequencyName { get; set; }
        public string ManufacturerName { get; set; }
        public string PartitionName { get; set; }
        public string StationName { get; set; }
        public Nullable<int> RTUID { get; set; }
        public string RDeviceID { get; set; }
    }
}
