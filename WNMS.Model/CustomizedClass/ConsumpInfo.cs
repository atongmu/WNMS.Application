using System;
using System.Collections.Generic;
using System.Text;

namespace WNMS.Model.CustomizedClass
{
   public class ConsumpInfo
    {
        public DateTime UpdateTime { get; set; }
        public int StationID { get; set; }
        public decimal? EnergyCon { get; set; }//电耗
        public decimal? FlowCon { get; set;}//流量
        public decimal? consump { get; set; }//单吨能耗
    }
}
