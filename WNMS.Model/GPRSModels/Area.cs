using System;
using System.Collections.Generic;

namespace WNMS.Model.GPRSModels
{
    public partial class Area
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Remark { get; set; }
        public byte[] Map { get; set; }
        public string RegionCode { get; set; }
    }
}
