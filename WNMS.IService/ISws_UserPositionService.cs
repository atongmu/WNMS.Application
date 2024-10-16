using System;
using System.Collections.Generic;
using System.Text;
using WNMS.Model.CustomizedClass;

namespace WNMS.IService
{
    public partial interface ISws_UserPositionService : IBaseService

    {
        PageResult<UserPosition> GetUserPosition(int pageindex, int pagesize);


        IEnumerable<dynamic> UserPositionTree(string text);


        dynamic GetUserPositionInfo(string mid);
    }
}
