using System;
using System.Collections.Generic;

namespace WNMS.Model.DataModels
{
    public partial class SwsStation
    {
        public int StationId { get; set; }
        public string StationNum { get; set; }
        public string StationName { get; set; }
        public byte StaitonType { get; set; }
        public double Lng { get; set; }
        public double Lat { get; set; }
        public byte InstallationPosition { get; set; }
        public DateTime InstallationDate { get; set; }
        public DateTime AcceptanceDate { get; set; }
        public DateTime QualityEndDate { get; set; }
        public string Remark { get; set; }
        public byte InspectionCycle { get; set; }
        public byte MaintenanceCycle { get; set; }
        public byte CleaningCycle { get; set; }
        public byte WaterTankNum { get; set; }
        public byte SwitchingCycle { get; set; }
        public bool WaterDisinfection { get; set; }
        public bool WaterTankPublic { get; set; }
        public bool WaterQualityMonitor { get; set; }
        public bool CameraMonitor { get; set; }
        public bool ControlMonitor { get; set; }
        public int InType { get; set; }
        public DateTime CreateTime { get; set; }
        public long? Manager { get; set; }
        public string ManagerPhone { get; set; }
        public double OccupancyRate { get; set; }
        public bool DoorInsert { get; set; }
        public int DoorId { get; set; }
        public int GuiNum { get; set; }
        public int Gui3dnum { get; set; }
        public bool ControlMonitor_bengfang { get; set; }
    }
}
