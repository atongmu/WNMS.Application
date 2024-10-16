using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using WNMS.Model.CustomizedClass;
using WNMS.Model.DataModels;

namespace WNMS.IService
{
    public partial interface IWarnRuleService:IBaseService
    {
         IEnumerable<dynamic> GetDetailRuleByID(int ruleid);
         int SetRuleInfo(WarnRuleDetail rd, int RuleId,byte  partition);
         IEnumerable<dynamic> GetRuleByDetailID(int detailid);
         int Del_DetailRule(int detailID, int ruleID);
         int DeleteRules(List<int> ruleIDs);
         PageResult<warnReport> LoadWarnDataList(int userid, Expression<Func<warnReport, bool>> funcWhere, int pageSize, int pageIndex, string funcOrderby, bool isAsc = true);
        int GetWarnCount(int userid);

    }
}
