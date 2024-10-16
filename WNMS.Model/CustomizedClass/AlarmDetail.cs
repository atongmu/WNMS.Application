using System;
using System.Collections.Generic;
using System.Text;

namespace WNMS.Model.CustomizedClass
{
   public class AlarmDetail
    {
         
        public string EventTime { get; set; }
        public string EventMessage { get; set; }
        public byte EventType { get; set; }
        public byte EventLevel { get; set; }
        public byte State { get; set; }
        public float CurrentValue { get; set; }
        public float LimitValue { get; set; }
        public string EndDate { get; set; }
        public string DeviceName { get; set; }
        public string DeviceType { get; set; }
        public string Duration { get; set; }
    }
}
