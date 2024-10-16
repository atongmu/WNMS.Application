using System;
using System.Collections.Generic;
using System.Text;

namespace WNMS.Model.CustomizedClass
{
    public class Api_SinWoEvent
    {
        public long WOID { get; set; }
        public string Num { get; set; } 
        public DateTime CompleteTime { get; set; } 
        public string IncidentContent { get; set; }
        public string DeviceName { get; set; }
        public short? CurrentState { get; set; }
        public DateTime? ReleaseTime { get; set; }
        public int State { get; set; }
    }
}
