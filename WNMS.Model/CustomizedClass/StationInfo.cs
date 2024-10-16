using System;
using System.Collections.Generic;

namespace WNMS.Model.CustomizedClass
{
    public partial class StationInfo
    {
        public int StationId { get; set; }
        public string StationNum { get; set; }
        public string StationName { get; set; }
        public string StaitonTypeName { get; set; }
        public string Lng_Lat { get; set; }
        public string StationPostion { get; set; }
        public DateTime InstallationDate { get; set; }
        public DateTime AcceptanceDate { get; set; }
        public DateTime QualityEndDate { get; set; }
        public byte InspectionCycle { get; set; }
        public byte MaintenanceCycle { get; set; }
        public byte CleaningCycle { get; set; }
        public byte WaterTankNum { get; set; }
        public string WaterTankPublic { get; set; }
       
    }
}
