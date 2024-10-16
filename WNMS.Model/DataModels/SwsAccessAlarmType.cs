using System;
using System.Collections.Generic;

namespace WNMS.Model.DataModels
{
    public partial class SwsAccessAlarmType
    {
        public string Id { get; set; }
        public string AlarmName { get; set; }
        public int? ParentId { get; set; }
    }
}
