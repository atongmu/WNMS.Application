using System;
using System.Collections.Generic;

namespace WNMS.Model.DataModels
{
    public partial class WoForward
    {
        public long Id { get; set; }
        public long Woid { get; set; }
        public int UserId { get; set; }
        public int? RecipientId { get; set; }
        public DateTime? ExtensionTime { get; set; }
        public DateTime? CompleteTime { get; set; }
        public short? State { get; set; }
        public short? Type { get; set; }
        public int? Auditor { get; set; }
        public string Remake { get; set; }
        public DateTime? AuditingTime { get; set; }
        public DateTime ForwardTime { get; set; }
        public bool IsReceive { get; set; }
    }
}
