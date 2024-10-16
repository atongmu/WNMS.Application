using System;
using System.Collections.Generic;
using System.Text;
using WNMS.Model.DataModels;

namespace WNMS.Model.CustomizedClass
{
    public class ActionCompare : IEqualityComparer<SysModule>
    { 
        public bool Equals(SysModule x, SysModule y)
        {
            if (x == null)
                return y == null;
            return x.ModuleNum == y.ModuleNum;
        }

        public int GetHashCode(SysModule obj)
        {
            if (obj == null)
                return 0;
            return obj.ModuleNum.GetHashCode();
        }
    }
    public class ActionCompareBtn : IEqualityComparer<SysModuleButton>
    {
        public bool Equals(SysModuleButton x, SysModuleButton y)
        {
            if (x == null)
                return y == null;
            return x.ModuleButtonId == y.ModuleButtonId;
        }

        public int GetHashCode(SysModuleButton obj)
        {
            if (obj == null)
                return 0;
            return obj.ModuleButtonId.GetHashCode();
        }
    }
}
