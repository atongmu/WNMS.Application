using System;
using System.Collections.Generic;
using System.Text;

namespace WNMS.Model.CustomizedClass
{
    public class CandlestickData
    {
        public DateTime UpdateTime { get; set; }
        public double? FirstData { get; set; }
        public double? LastData { get; set; }
        public double? MinData { get; set; }
        public double? MaxData { get; set; }
    }
}
