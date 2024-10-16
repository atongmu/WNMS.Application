using System;
using System.Collections.Generic;

namespace WNMS.Model.DataModels
{
    public partial class SysRole
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public int Sort { get; set; }
        public bool IsEnable { get; set; }
        public string Remark { get; set; }
    }
}
