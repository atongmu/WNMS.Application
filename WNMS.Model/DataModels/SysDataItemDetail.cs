using System;
using System.Collections.Generic;

namespace WNMS.Model.DataModels
{
    public partial class SysDataItemDetail
    {
        public int ItemDetailId { get; set; }
        public int FItemId { get; set; }
        public string ItemName { get; set; }
        public string ItemValue { get; set; }
        public int Sort { get; set; }
        public bool IsEnable { get; set; }
        public string Remark { get; set; }
    }
}
