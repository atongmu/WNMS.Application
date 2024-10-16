using System;
using System.Collections.Generic;
using System.Text;

namespace WNMS.Model.CustomizedClass
{
    public class Device01Data
    {
        public int StationID { get; set; }
        public string StationName { get; set; }
        public Nullable<decimal>  EnergyCon { get; set; }
        public Nullable<decimal> FlowCon { get; set; }
        public Nullable<int> Num { get; set; }
    }
}
