using System;
using System.Collections.Generic;

namespace WNMS.Model.DataModels
{
    public partial class WoPlanInspectO
    {
        public long PlanId { get; set; }
        public long InspectObject { get; set; }
        public long TemplateId { get; set; }
        public string ObjectName { get; set; }
        public bool IsTemplate { get; set; }
        public int PumpStationId { get; set; }
        public int? ForwardState { get; set; }
        public int ForwardCount { get; set; }
        public byte? IsAuditing { get; set; }
        public int? AuditUser { get; set; }
    }
}
