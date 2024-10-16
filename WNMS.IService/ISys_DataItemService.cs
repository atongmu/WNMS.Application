using System;
using System.Collections.Generic;
using System.Text;
using WNMS.Model.DataModels;

namespace WNMS.IService
{
    public partial interface ISys_DataItemService : IBaseService
    {
       IEnumerable<SysDataItem> QueyExtentSelfAndChirldren(int itemid);
       IEnumerable<SysDataItem> QuerySelfAndChirldren(string ids);
    }
}
