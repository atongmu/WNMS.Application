using System;
using System.Collections.Generic;

namespace WNMS.Model.DataModels
{
    public partial class GdEvents
    {
        public long IncidentId { get; set; }
        public string IncidentNum { get; set; }
        public byte IncidentState { get; set; }
        public int IncidentType { get; set; }
        public string IncidentContent { get; set; }
        public byte IncidentSource { get; set; }
        public DateTime ReportTime { get; set; }
        public int ReportUser { get; set; }
        public string Picture { get; set; }
        public string Recording { get; set; }
        public string Description { get; set; }
        public byte DisposeState { get; set; }
        public bool AuditState { get; set; }
        public string AuditContent { get; set; }
        public int TaskId { get; set; }
    }
}
