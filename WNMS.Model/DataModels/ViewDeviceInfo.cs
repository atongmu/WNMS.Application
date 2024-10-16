using System;
using System.Collections.Generic;

namespace WNMS.Model.DataModels
{
    public partial class ViewDeviceInfo
    {
        public long DeviceId { get; set; }
        public int StationId { get; set; }
        public string DeviceName { get; set; }
        public string DeviceNum { get; set; }
        public int? Rtuid { get; set; }
        public byte Partition { get; set; }
        public int PumpNum { get; set; }
        public int DeviceType { get; set; }
        public int Frequency { get; set; }
        public int EquipmentType { get; set; }
        public int DeviceTypeZys { get; set; }
        public int IsSingle { get; set; }
    }
}
