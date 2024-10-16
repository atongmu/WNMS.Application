using System;
using System.Collections.Generic;

namespace WNMS.Model.CustomizedClass
{
    public partial class AlarmDeviceInfo
    {
        public long EquipID { get; set; }
        public string DeviceID { get; set; }
        public int RTUID { get; set; }
        public string DeviceName { get; set; }
        public string EquipmentType { get; set; }
        public string Partition { get; set; }
        public int EventSource { get; set; }
        public byte State { get; set; }
        public DateTime EventTime { get; set; }
        public string EventMessage { get; set; }
        public string EventLevel { get; set; }
        public DateTime EndDate { get; set; }

    }

    /// <summary>
    /// 实时报警
    /// </summary>
    public partial class AlarmDeviceInfo_SS
    {
        public int RTUID { get; set; }
        public string DeviceName { get; set; }
        public string Partition { get; set; }
        public int EventSource { get; set; }
        public DateTime EventTime { get; set; }
        public string EventMessage { get; set; }
        public string EventLevel { get; set; }

    }

}
