using System;
using System.Collections.Generic;

namespace WNMS.Model.DataModels
{
    public partial class GdWoextension
    {
        public long Id { get; set; }
        public int? UserId { get; set; }
        public DateTime? ExtensionTime { get; set; }
        public DateTime? CompleteTime { get; set; }
        public int? Auditor { get; set; }
        public DateTime? AuditingTime { get; set; }
        public byte? State { get; set; }
        public string Reason { get; set; }
        public long Woid { get; set; }
        public DateTime? OldCompleteTime { get; set; }
    }
}
