using System;
using System.Collections.Generic;
using System.Text;

namespace WNMS.Model.CustomizedClass
{
    public class EventStation
    {
        public int Rtuid { get; set; }
        public string EventTime { get; set; }
        public string EventMessage { get; set; }
        public byte EventType { get; set; }
        public byte EventLevel { get; set; }
        public byte State { get; set; }
        public float CurrentValue { get; set; }
        public float LimitValue { get; set; }
        public DateTime? EndDate { get; set; }
        public string StationName { get; set; }
        public string Manufacturer { get; set; }
        public string DeviceType { get; set; }
        public string Duration { get; set; }
    }
}
