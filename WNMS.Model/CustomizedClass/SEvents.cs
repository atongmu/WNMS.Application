using System;
using System.Collections.Generic;
using System.Text;

namespace WNMS.Model.CustomizedClass
{
    public class SEvents
    {
        public long IncidentID { get; set; }
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
        public int StationID { get; set; }
        public string StationName { get; set; }
        public string Account { get; set; }
    }
}
