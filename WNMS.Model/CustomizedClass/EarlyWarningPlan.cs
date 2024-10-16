using System;
using System.Collections.Generic;
using System.Text;

namespace WNMS.Model.CustomizedClass
{
    public class EarlyWarningPlan
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Contents { get; set; }
        public string Solution { get; set; }
        public byte? Type { get; set; }
        public string ItemName { get; set; }
        public bool IsEnable { get; set; }

    }
}
