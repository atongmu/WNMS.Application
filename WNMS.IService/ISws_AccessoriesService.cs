using System;
using System.Collections.Generic;
using System.Text;
using WNMS.Model.DataModels;

namespace WNMS.IService
{
    public partial interface ISws_AccessoriesService : IBaseService
    { //查询信息
        IEnumerable<dynamic> QueryAccessoriesTable( int pageindex, int pagesize, string order, string filter, ref int Totalcount);

        //删除信息
        int DeleteAccessories(List<string> ids);


        //导入数据
        int AccessoriesImport(List<SwsAccessories> list);
    }
}
