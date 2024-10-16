using System;
using System.Collections.Generic;

namespace WNMS.Model.DataModels
{
    public partial class SysEarlyWarningPlan
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Contents { get; set; }
        public string Solution { get; set; }
        public byte? Type { get; set; }
        public bool? IsEnable { get; set; }
    }
}
