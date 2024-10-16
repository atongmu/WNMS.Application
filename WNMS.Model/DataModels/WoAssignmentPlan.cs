using System;
using System.Collections.Generic;

namespace WNMS.Model.DataModels
{
    public partial class WoAssignmentPlan
    {
        public long PlanId { get; set; }
        public string PlanName { get; set; }
        public int Inspector { get; set; }
        public byte InspectCycle { get; set; }
        public int Creater { get; set; }
        public int Dmaid { get; set; }
        public byte Travel { get; set; }
        public bool State { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Remark { get; set; }
        public byte PlanType { get; set; }
        public DateTime CreateTime { get; set; }
        public long? TemplateId { get; set; }
        public byte Type { get; set; }
        public string Gis { get; set; }
        public long? TemplatePlanId { get; set; }
        public byte? IsFinish { get; set; }
        public DateTime? UniqueTime { get; set; }
        public int? IsForward { get; set; }
        public int? IsChargeback { get; set; }
    }
}
