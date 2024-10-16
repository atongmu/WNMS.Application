using System;
using System.Collections.Generic;

namespace WNMS.Model.DataModels
{
    public partial class HourQuartZtest
    {
        public int Id { get; set; }
        public int? StationId { get; set; }
        public DateTime? UpdateTime { get; set; }
        public string DataKey { get; set; }
        public double? DataValue { get; set; }
    }
}
