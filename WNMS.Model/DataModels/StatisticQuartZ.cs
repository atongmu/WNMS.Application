using System;
using System.Collections.Generic;

namespace WNMS.Model.DataModels
{
    public partial class StatisticQuartZ
    {
        public int Id { get; set; }
        public string DataKey { get; set; }
        public string DataName { get; set; }
        public string DataId { get; set; }
        public bool IsStatistic { get; set; }
        public int? DeviceType { get; set; }
    }
}
