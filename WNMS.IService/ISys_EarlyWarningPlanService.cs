using System;
using System.Collections.Generic;
using System.Text;
using WNMS.Model.DataModels;

namespace WNMS.IService
{
    public partial interface ISys_EarlyWarningPlanService : IBaseService
    {
        //查询信息
        IEnumerable<dynamic> QueryPlansTable(int pageindex, int pagesize, string order, string filter, ref int Totalcount);

        //删除信息
        int DeletePlans(List<string> ids);

        //添加编辑信息
        int AddPlan(SysEarlyWarningPlan sysEarlyWarningPlan);
        /// <summary>
        /// 查询报警方案
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        IEnumerable<dynamic> QueryEventPlans(string filter);

    }
}
