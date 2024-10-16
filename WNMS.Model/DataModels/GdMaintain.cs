using System;
using System.Collections.Generic;

namespace WNMS.Model.DataModels
{
    public partial class GdMaintain
    {
        public int MaintainId { get; set; }
        public string Num { get; set; }
        public int StationId { get; set; }
        public DateTime? CreateTime { get; set; }
        public DateTime? MaintainTime { get; set; }
        public string TaskDescription { get; set; }
        public string FeedbackMsg { get; set; }
        public bool IsFeedback { get; set; }
        public byte Project { get; set; }
        public int MaintainUser { get; set; }
        public byte MaintainState { get; set; }
    }
}
