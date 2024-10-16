using System;
using System.Collections.Generic;
using System.Text;

namespace WNMS.Model.CustomizedClass
{
    public class StationEvaluation
    {
        public int StationId { get; set; }
        public string StationNum { get; set; }
        public string StationName { get; set; }
        public double TonsWater { get; set; }
        public double SupplyWater { get; set; }
        public double SupplyPower { get; set; }
        public int Manufacturer { get; set; }
    }
}
