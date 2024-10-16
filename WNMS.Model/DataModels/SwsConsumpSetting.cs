using System;
using System.Collections.Generic;

namespace WNMS.Model.DataModels
{
    public partial class SwsConsumpSetting
    {
        public int UserId { get; set; }
        public long DeviceId { get; set; }
        public short Type { get; set; }
    }
}
