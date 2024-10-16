using System;
using System.Collections.Generic;
using System.Text;

namespace WNMS.Model.CustomizedClass
{
    public class DeviceInfo02Info
    {
        public long DeviceID { get; set; }//FullLoadCurrent
        public string DeviceNum { get; set; }
        public string DeviceName { get; set; }
        public int DeviceType { get; set; }
        public DateTime? ProductionDate { get; set; }
        public byte Partition { get; set; }
        public int Manufacturer { get; set; }
        public string StationName { get; set; }
        public string DeviceTypeName { get; set; }
        public string ManufacturerName { get; set; }
        public string PartitionName { get; set; }
        public int StationID { get; set; }
        public string ImageUrl { get; set; }
        public int Gui { get; set; }
        public string RtuNum { get; set; }
    }
}
