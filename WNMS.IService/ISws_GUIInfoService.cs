using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using WNMS.Model.CustomizedClass;

namespace WNMS.IService
{
    public partial interface ISws_GUIInfoService:IBaseService
    {
        PageResult<GUIInfo> GetGUIData(Expression<Func<GUIInfo, bool>> funcWhere, int pageSize, int pageIndex, string sortName, string order);
        int DeleteGUI(int guiId);
    }
}
