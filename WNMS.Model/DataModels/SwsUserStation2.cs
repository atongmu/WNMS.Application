using System;
using System.Collections.Generic;

namespace WNMS.Model.DataModels
{
    public partial class SwsUserStation2
    {
        public int UserId { get; set; }
        public int StationId { get; set; }
        public bool FocusOn { get; set; }
    }
}
