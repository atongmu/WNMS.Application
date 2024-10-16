using System;
using System.Collections.Generic;
using System.Text;
using WNMS.Model.DataModels;

namespace WNMS.IService
{
    public partial interface ISws_AccessoriesMaintenanceService : IBaseService
    {
        //查询信息
        IEnumerable<dynamic> QueryMaintenanceTable(bool IsAdmin, int UserID, int pageindex, int pagesize, string order, string filter, ref int Totalcount);
        //删除信息
        int DeleteMaintenances(List<int> ids);
        //查询保养提醒信息
        IEnumerable<dynamic> QueryMaintenanceRtTable(bool IsAdmin, int UserID, int pageindex, int pagesize, string order, string filter, ref int Totalcount);
        //标记已读
        int ReadMaintenances(List<long> ids);
        //添加保养及更新器件表
        int SetMaintenances(SwsAccessoriesMaintenance swsAccessoriesMaintenance,int isInsert);
        //查询保养数量
        int GetMaintenanceRtCount(int userid, bool isadmin);
        //导入数据
        int MaintenanceImport(List<SwsAccessoriesMaintenance> list);
    }
}
