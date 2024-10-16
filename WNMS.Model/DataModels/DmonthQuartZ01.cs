using System;
using System.Collections.Generic;

namespace WNMS.Model.DataModels
{
    public partial class DmonthQuartZ01
    {
        public long Id { get; set; }
        public long DeviceId { get; set; }
        public decimal EnergyCon { get; set; }
        public decimal FlowCon { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}
