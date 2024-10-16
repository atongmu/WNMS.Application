using System;
using System.Collections.Generic;

namespace WNMS.Model.DataModels
{
    public partial class GdRepair
    {
        public int RepairId { get; set; }
        public string Num { get; set; }
        public int StationId { get; set; }
        public string FaultContent { get; set; }
        public string FaultDescription { get; set; }
        public bool IsFeedback { get; set; }
        public long? DeviceId { get; set; }
        public DateTime? ReportTime { get; set; }
        public DateTime CreateTime { get; set; }
        public byte? RepairState { get; set; }
        public string RepairDescription { get; set; }
        public int RepairUser { get; set; }
    }
}
