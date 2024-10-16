using System;
using System.Collections.Generic;

namespace WNMS.Model.DataModels
{
    public partial class WoInsExtension
    {
        public long Id { get; set; }
        public long? UserId { get; set; }
        public DateTime? ExtensionTime { get; set; }
        public DateTime? CompleteTime { get; set; }
        public long? Auditor { get; set; }
        public DateTime? AuditingTime { get; set; }
        public short? State { get; set; }
        public string Reason { get; set; }
        public long PlanId { get; set; }
        public DateTime? OldCompleteTime { get; set; }
    }
}
