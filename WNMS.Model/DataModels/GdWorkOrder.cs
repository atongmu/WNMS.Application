using System;
using System.Collections.Generic;

namespace WNMS.Model.DataModels
{
    public partial class GdWorkOrder
    {
        public long Woid { get; set; }
        public short? Degree { get; set; }
        public short? HandleLevel { get; set; }
        public DateTime? ReleaseTime { get; set; }
        public byte? IsAuditing { get; set; }
        public string AuditingContent { get; set; }
        public long EventId { get; set; }
        public short? CurrentState { get; set; }
        public long ReleaseUser { get; set; }
        public DateTime CompleteTime { get; set; }
        public long? Pid { get; set; }
        public int UserId { get; set; }
        public string Num { get; set; }
    }
}
