using System;
using System.Collections.Generic;
using System.Text;

namespace WNMS.Model.CustomizedClass
{
    public class WebWaterQuality
    {
        public DateTime? UpdateTime { get; set; }
        public int? StationId { get; set; }
        public decimal PhAver { get; set; }
        public decimal ClAver { get; set; }
        public decimal TurbidityAver { get; set; }
        public decimal OrpAver { get; set; }
        public decimal SalinityAver { get; set; }
        public decimal OxygenAver { get; set; }
        public decimal ConductivityAver { get; set; }
    }
}
