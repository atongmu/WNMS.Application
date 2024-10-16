using System;
using System.Collections.Generic;

namespace WNMS.Model.CustomizedClass
{
    public partial class EventHistoryInfo
    {
        public long EquipID { get; set; }
        public string DeviceID { get; set; }
        public string DeviceName { get; set; }
        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }
        public int EventSource { get; set; }
        public string EventMessage { get; set; }
        public string EventLevel { get; set; }
      
    }
}
