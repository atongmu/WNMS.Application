using System;
using System.Collections.Generic;

namespace WNMS.Model.DataModels
{
    public partial class WoInspectPlanCheck
    {
        public long Id { get; set; }
        public long PlanId { get; set; }
        public long EquipmentId { get; set; }
        public DateTime ModifyTime { get; set; }
        public bool ReachState { get; set; }
        public bool FeedBackState { get; set; }
        public string Gislocation { get; set; }
        public string InspectImage { get; set; }
        public string DetailContent { get; set; }
        public byte ObjectType { get; set; }
    }
}
