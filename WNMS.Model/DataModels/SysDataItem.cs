using System;
using System.Collections.Generic;

namespace WNMS.Model.DataModels
{
    public partial class SysDataItem
    {
        public int ItemId { get; set; }
        public int Pid { get; set; }
        public string ItemName { get; set; }
        public string ItemValue { get; set; }
        public int Sort { get; set; }
        public string Reamrk { get; set; }
    }
}
