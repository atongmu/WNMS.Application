using System;
using System.Collections.Generic;

namespace WNMS.Model.DataModels
{
    public partial class SwsValveWith01
    {
        public int ValveId { get; set; }
        public long DeviceId { get; set; }
        public string ValveName { get; set; }
        public int? ValveNum { get; set; }
        public bool IsAdjusted { get; set; }
        public bool IsRemote { get; set; }
    }
}
