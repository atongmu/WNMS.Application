using System;
using System.Collections.Generic;

namespace WNMS.Model.DataModels
{
    public partial class GdInspection
    {
        public int InspectionId { get; set; }
        public string Num { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? InspectionTime { get; set; }
        public int InspectionUser { get; set; }
        public byte PumpState { get; set; }
        public int StationId { get; set; }
        public byte DoorState { get; set; }
        public byte Penetration { get; set; }
        public byte Temperature { get; set; }
        public byte Health { get; set; }
        public byte Noise { get; set; }
        public byte ControlCabinet { get; set; }
        public byte PipeState { get; set; }
        public byte ValveParts { get; set; }
        public byte Pressure { get; set; }
        public byte Electricity { get; set; }
        public byte ElectricQuantity { get; set; }
        public byte Voltage { get; set; }
        public byte Frequency { get; set; }
        public byte LiquidLevel { get; set; }
        public string TaskDescription { get; set; }
        public string FeedbackMsg { get; set; }
        public bool IsFeedback { get; set; }
    }
}
