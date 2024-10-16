using System;
using System.Collections.Generic;

namespace WNMS.Model.DataModels
{
    public partial class SwsAccessHistory
    {
        public long Id { get; set; }
        public int DoorId { get; set; }
        public DateTime PoliceTime { get; set; }
        public string Information { get; set; }
        public string OperatingUser { get; set; }
        public string InOutWay { get; set; }
        public string UserName { get; set; }
    }
}
