using System;
using System.Collections.Generic;

namespace WNMS.Model.DataModels
{
    public partial class WoAreaInfo
    {
        public int Id { get; set; }
        public string AreaName { get; set; }
        public int Pid { get; set; }
        public string FillColor { get; set; }
    }
}
