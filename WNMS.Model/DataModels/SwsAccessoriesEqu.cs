using System;
using System.Collections.Generic;

namespace WNMS.Model.DataModels
{
    public partial class SwsAccessoriesEqu
    {
        public string AccessoriesId { get; set; }
        public long EquipmentId { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime MchanicalDate { get; set; }
        public DateTime ElectricalDate { get; set; }
        public DateTime? GuaranteePeriod { get; set; }
        public DateTime CommissioningDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string AccessoriesNo { get; set; }
        public string Reason { get; set; }
        public decimal? Cost { get; set; }
        public long Id { get; set; }
        public int Quantity { get; set; }
        public byte EquType { get; set; }
        public DateTime MaintenanceDate { get; set; }
    }
}
