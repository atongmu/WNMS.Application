using System;
using System.Collections.Generic;
using System.Text;

namespace WNMS.Model.CustomizedClass
{
    public  class warnReport
    {
        public long DeviceID { get; set; }
        public int StationID { get; set; }
        public string Data { get; set; }
        public string DeviceName { get; set; }
        public string RuleText { get; set; }
        public DateTime UpdateTime { get; set; }
        public int RuleID { get; set; }
    }
}
