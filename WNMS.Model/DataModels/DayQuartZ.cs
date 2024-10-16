using System;
using System.Collections.Generic;

namespace WNMS.Model.DataModels
{
    public partial class DayQuartZ
    {
        public int Id { get; set; }
        public DateTime? UpdateTime { get; set; }
        public int? StationId { get; set; }
        public double? EnergyCon { get; set; }
        public double? FlowCon { get; set; }
        public double? PhAver { get; set; }
        public double? ClAver { get; set; }
        public double? TurbidityAver { get; set; }
        public double? OrpAver { get; set; }
        public double? SalinityAver { get; set; }
        public double? OxygenAver { get; set; }
        public double? ConductivityAver { get; set; }
    }
}
