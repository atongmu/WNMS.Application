using System;
using System.Collections.Generic;

namespace WNMS.Model.DataModels
{
    public partial class SwsPropertyInfo
    {
        public int PropertyId { get; set; }
        public string Name { get; set; }
        public byte Type { get; set; }
        public string Manufacturer { get; set; }
        public string BrandName { get; set; }
        public string Size { get; set; }
        public DateTime BuyDate { get; set; }
        public decimal BuyMonery { get; set; }
        public string WarrantyPeriod { get; set; }
        public string Custodian { get; set; }
        public string StorePosition { get; set; }
        public string Remark { get; set; }
        public int StationId { get; set; }
    }
}
