using System;
using System.Collections.Generic;

namespace WNMS.Model.DataModels
{
    public partial class WarnData1
    {
        public long DeviceId { get; set; }
        public int StationId { get; set; }
        public short DataId { get; set; }
        public int RuleId { get; set; }
        public string Value { get; set; }
        public int Id { get; set; }
    }
}
