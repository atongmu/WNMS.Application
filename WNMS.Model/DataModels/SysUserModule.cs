using System;
using System.Collections.Generic;

namespace WNMS.Model.DataModels
{
    public partial class SysUserModule
    {
        public int UserId { get; set; }
        public int ModuleId { get; set; }
        public byte Type { get; set; }
    }
}
