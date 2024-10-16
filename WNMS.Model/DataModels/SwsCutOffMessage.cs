using System;
using System.Collections.Generic;

namespace WNMS.Model.DataModels
{
    public partial class SwsCutOffMessage
    {
        public int Id { get; set; }
        public int? StationId { get; set; }
        public string CutOffTime { get; set; }
        public DateTime? SupplyTime { get; set; }
        public string CutOffReason { get; set; }
    }
}
