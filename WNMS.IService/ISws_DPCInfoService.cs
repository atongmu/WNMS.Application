using System;
using System.Collections.Generic;
using System.Text;

namespace WNMS.IService
{
    public partial interface ISws_DPCInfoService : IBaseService
    {
        //删除
        int DeleteDpcs(List<string> ids, ref List<string> dpcIdsList);
    }
}
