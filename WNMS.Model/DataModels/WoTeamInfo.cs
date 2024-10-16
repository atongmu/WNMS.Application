using System;
using System.Collections.Generic;

namespace WNMS.Model.DataModels
{
    public partial class WoTeamInfo
    {
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public int? RegionId { get; set; }
    }
}
