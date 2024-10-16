using System;
using System.Collections.Generic;

namespace WNMS.Model.DataModels
{
    public partial class SwsAccessoriesMaintenance
    {
        public int Id { get; set; }
        public long AccessoriesDetailId { get; set; }
        public string MaintenanceContent { get; set; }
        public decimal Cost { get; set; }
        public DateTime MaintenanceDate { get; set; }
        public string MaintenanceUser { get; set; }
    }
}
