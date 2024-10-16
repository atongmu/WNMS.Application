using System;
using System.Collections.Generic;

namespace WNMS.Model.CustomizedClass
{
    public partial class DeviceAndRtuinfo
    {
        public int Rtuid { get; set; }
        public string DeviceId { get; set; }
        public byte ComAddress { get; set; }
        public string ComType { get; set; }
        public string DeviceType { get; set; }
        public string Priority { get; set; }
        public byte[] PluginFile { get; set; }
        public bool ActivelySent { get; set; }
        public int? Ipport { get; set; }
        public int StationId { get; set; }

        public long EquipID { get; set; }
        public string DeviceName { get; set; }
        public string EquipmentType { get; set; }
        public int iEquipmentType { get; set; }
        public string Partition { get; set; }
        public int PumpNum { get; set; }
        public int Frequency { get; set; }
        public int IsSingle { get; set; }
        public string DeviceType_ZYS { get; set; }
    }
}
