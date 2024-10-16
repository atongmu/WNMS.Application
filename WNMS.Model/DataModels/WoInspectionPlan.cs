using System;
using System.Collections.Generic;

namespace WNMS.Model.DataModels
{
    public partial class WoInspectionPlan
    {
        public long Id { get; set; }
        public string PlanName { get; set; }
        public int Dmaid { get; set; }
        public byte Travel { get; set; }
        public byte Cycle { get; set; }
        public int Inspector { get; set; }
        public int CreateUser { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? ModifyDate { get; set; }
        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Remark { get; set; }
        public bool EnabledMark { get; set; }
        public byte Type { get; set; }
        public int? WeekNum { get; set; }
        public string DayNums { get; set; }
        public long? TemplateId { get; set; }
        public string ScheduleId { get; set; }
    }
}
