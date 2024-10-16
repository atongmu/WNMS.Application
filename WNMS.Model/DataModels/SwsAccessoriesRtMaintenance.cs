using System;
using System.Collections.Generic;

namespace WNMS.Model.DataModels
{
    public partial class SwsAccessoriesRtMaintenance
    {
        public long AccessoriesId { get; set; }
        public DateTime MaintenanceDate { get; set; }
        public int? DayCount { get; set; }
    }
}
