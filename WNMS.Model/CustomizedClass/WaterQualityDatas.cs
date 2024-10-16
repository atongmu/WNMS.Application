using System;
using System.Collections.Generic;
using System.Text;

namespace WNMS.Model.CustomizedClass
{
    public class WaterQualityDatas
    {
        public string DeviceName { get; set; }
        public decimal? PhAver { get; set; }
        public decimal? ClAver { get; set; }
        public decimal? TurbidityAver { get; set; }
    }
}
