using System;
using System.Collections.Generic;
using System.Text;

namespace WNMS.Model.CustomizedClass
{
    public class TreeAreaCompare: IEqualityComparer<TreeAction>
    {
        public bool Equals(TreeAction x, TreeAction y)
        {
            if (x == null)
                return y == null;
            return x!=null && y!=null&& x.id == y.id;
        }

        public int GetHashCode(TreeAction obj)
        {
            if (obj == null)
                return 0;
            return obj.id.GetHashCode();
        }
    }
}
