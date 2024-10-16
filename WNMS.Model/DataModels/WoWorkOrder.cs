using System;
using System.Collections.Generic;

namespace WNMS.Model.DataModels
{
    public partial class WoWorkOrder
    {
        public long Woid { get; set; }
        public short? Degree { get; set; }
        public short? HandleLevel { get; set; }
        public DateTime? ReleaseTime { get; set; }
        public byte? IsAuditing { get; set; }
        public string AuditingContent { get; set; }
        public long EventId { get; set; }
        public short? CurrentState { get; set; }
        public int ReleaseUser { get; set; }
        public DateTime CompleteTime { get; set; }
        public long? Pid { get; set; }
        public int? ReceiveUser { get; set; }
        public string Num { get; set; }
        public string EarlyWarningPlanId { get; set; }
    }
}
