using System;
using System.Collections.Generic;

namespace WNMS.Model.DataModels
{
    public partial class SwsAccessories
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Material { get; set; }
        public string Manufacturer { get; set; }
        public int MaintenancePeriod { get; set; }
        public int ReplacementCycle { get; set; }
        public string Unit { get; set; }
        public int Inventory { get; set; }
        public bool IsConsumable { get; set; }
        public string Model { get; set; }
        public string Place { get; set; }
    }
}
