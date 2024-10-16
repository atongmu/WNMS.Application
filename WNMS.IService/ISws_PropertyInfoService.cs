using System;
using System.Collections.Generic;
using System.Text;

namespace WNMS.IService
{
    public partial interface ISws_PropertyInfoService:IBaseService
    {
       IEnumerable<dynamic> QueryPropertyTable(bool IsAdmin, int pageindex, int pagesize, int ptypeID, string order, string filter, ref int Totalcount);
    }
}
