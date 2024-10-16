using System;
using System.Collections.Generic;

namespace WNMS.Model.DataModels
{
    public partial class SwsEventHistory
    {
        public long Id { get; set; }
        public DateTime EventTime { get; set; }
        public int Rtuid { get; set; }
        public short EventSource { get; set; }
        public string EventMessage { get; set; }
        public byte EventType { get; set; }
        public byte EventLevel { get; set; }
        public byte State { get; set; }
        public float CurrentValue { get; set; }
        public float LimitValue { get; set; }
        public string EventDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? EventMonth { get; set; }
        public int? EventYear { get; set; }
    }
}
