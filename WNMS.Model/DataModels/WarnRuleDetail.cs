using System;
using System.Collections.Generic;

namespace WNMS.Model.DataModels
{
    public partial class WarnRuleDetail
    {
        public int Id { get; set; }
        public short DataId { get; set; }
        public string CompareSymbol { get; set; }
        public double Value { get; set; }
        public int ParentId { get; set; }
        public int Num { get; set; }
        public string RelateSymbol { get; set; }
    }
}
