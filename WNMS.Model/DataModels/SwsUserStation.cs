using System;
using System.Collections.Generic;

namespace WNMS.Model.DataModels
{
    public partial class SwsUserStation
    {
        public int UserId { get; set; }
        public int StationId { get; set; }
        public bool FocusOn { get; set; }
    }
}
