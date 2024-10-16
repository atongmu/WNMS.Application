using System;
using System.Collections.Generic;

namespace WNMS.Model.DataModels
{
    public partial class WoEvents
    {
        public long IncidentId { get; set; }
        public byte IncidentState { get; set; }
        public int IncidentType { get; set; }
        public string IncidentContent { get; set; }
        public int IncidentSource { get; set; }
        public DateTime ReportTime { get; set; }
        public int ReportUser { get; set; }
        public string Address { get; set; }
        public decimal? Lng { get; set; }
        public decimal? Lat { get; set; }
        public string Description { get; set; }
        public byte DisposeState { get; set; }
        public byte Type { get; set; }
        public long? EquipmentId { get; set; }
        public int? EventId { get; set; }
        public long? StationId { get; set; }
        public string IncidentNum { get; set; }
    }
}
