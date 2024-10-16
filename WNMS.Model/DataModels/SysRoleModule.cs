using System;
using System.Collections.Generic;

namespace WNMS.Model.DataModels
{
    public partial class SysRoleModule
    {
        public int RoleId { get; set; }
        public int ModuleId { get; set; }
        public byte Type { get; set; }
    }
}
