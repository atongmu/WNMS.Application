using System;
using System.Collections.Generic;

namespace WNMS.Model.DataModels
{
    public partial class WarnRule
    {
        public int RuleId { get; set; }
        public string RuleText { get; set; }
        public string RuleSql { get; set; }
        public byte Partition { get; set; }
    }
}
