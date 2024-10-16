using System;
using System.Collections.Generic;

namespace WNMS.Model.DataModels
{
    public partial class SwsEventInfo
    {
        public int Id { get; set; }
        public DateTime EventTime { get; set; }
        public int Rtuid { get; set; }
        public short EventSource { get; set; }
        public string EventMessage { get; set; }
        public byte EventType { get; set; }
        public byte EventLevel { get; set; }
        public byte State { get; set; }
        public float CurrentValue { get; set; }
        public float LimitValue { get; set; }
        public bool? IsConfirm { get; set; }
    }
}
