using System;
using System.Collections.Generic;

namespace WNMS.Model.DataModels
{
    public partial class HourQuartZ01
    {
        public int Id { get; set; }
        public int? StationId { get; set; }
        public DateTime? UpdateTime { get; set; }
        public string DataKey { get; set; }
        public decimal? DataValue { get; set; }
    }
}
