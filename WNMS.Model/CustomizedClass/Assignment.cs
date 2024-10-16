using System;
using System.Collections.Generic;
using System.Text;

namespace WNMS.Model.CustomizedClass
{
    public class Assignment
    {
        public long PlanID { get; set; }
        public string PlanName { get; set; }
        public long Inspector { get; set; }
        public string InspectorName { get; set; }
        public byte InspectCycle { get; set; }
        public string InspectCycleName { get; set; }
        public long Creater { get; set; }
        public string CreaterName { get; set; }
        public string DMAName { get; set; }
        public byte Travel { get; set; }
        public string TravelName { get; set; }
        public bool State { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Remark { get; set; }
        public byte PlanType { get; set; }
        public DateTime CreateTime { get; set; }
        public byte? IsFinish { get; set; }
        public long TemplatePlanID { get; set; }
        public int? IsForward { get; set; }
        public int? IsChargeback { get; set; }
    }
}
