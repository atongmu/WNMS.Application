using System;
using System.Collections.Generic;

namespace WNMS.Model.DataModels
{
    public partial class DhourQuartZ02
    {
        public long Id { get; set; }
        public long DeviceId { get; set; }
        public decimal EnergyCon { get; set; }
        public decimal FlowCon { get; set; }
        public decimal? PhAver { get; set; }
        public decimal? ClAver { get; set; }
        public decimal? TurbidityAver { get; set; }
        public decimal? OrpAver { get; set; }
        public decimal? SalinityAver { get; set; }
        public decimal? OxygenAver { get; set; }
        public decimal? ConductivityAver { get; set; }
        public decimal? HardnessAver { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}
