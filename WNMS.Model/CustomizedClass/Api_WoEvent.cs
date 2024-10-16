using System;
using System.Collections.Generic;
using System.Text;

namespace WNMS.Model.CustomizedClass
{
    public class Api_WoEvent
    {
        public long WOID { get; set; }
        public string Num { get; set; }
        public short? Degree { get; set; }
        public short? HandleLevel { get; set; }
        public int IncidentType { get; set; }
        public DateTime CompleteTime { get; set; }
        public DateTime ReleaseTime { get; set; }
        public string point { get; set; }
        public string IncidentContent { get; set; }
        public string DeviceName { get; set; }
        public short? CurrentState { get; set; }
        public string NickName { get; set; }
        public DateTime ReportTime { get; set; }
        public string Address { get; set; }
        public string DeviceNum { get; set; }
        public int? State { get; set; }
        public long EventID { get; set; }
        public short? StepState { get; set; }
        public string DeviceType { get; set; }
        public int? ExtensionCount { get; set; }
        public short? ExtensionState { get; set; }
        public short? ForwardState { get; set; }
        public short? TurnState { get; set; }
        public string ReceiveUser { get; set; }
        public int? ForwardCount { get; set; }
        public int? TurnCount { get; set; }
        public string Description { get; set; }
        public int StationID { get; set; }
        public string StationName { get; set; }
        public long DeviceID { get; set; } 

    }
}
