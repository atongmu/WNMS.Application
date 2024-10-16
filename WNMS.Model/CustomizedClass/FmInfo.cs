using System;
using System.Collections.Generic;
using System.Text;

namespace WNMS.Model.CustomizedClass
{
    public class FmInfo
    {
        public int ValveId { get; set; }
        public long DeviceId { get; set; }
        public string ValveName { get; set; }
        public int? ValveNum { get; set; }
        public bool IsAdjusted { get; set; }
        public bool IsRemote { get; set; }
        public string DeciceName { get; set; }
        public int   RtuId { get; set; }
        public int Partition { get; set; }
    }
}
