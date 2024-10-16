using System;
using System.Collections.Generic;

namespace WNMS.Model.DataModels
{
    public partial class WarnData
    {
        public long DeviceId { get; set; }
        public int StationId { get; set; }
        public string Data { get; set; }
        public int RuleId { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}
