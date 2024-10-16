using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using WNMS.Model.CustomizedClass;
using WNMS.Model.DataModels;

namespace WNMS.IService
{
    public interface ISysUserService : IBaseService
    {
        int QueryID(string PK, string TableName);
        IEnumerable<dynamic> GetUserInfos();
        PageResult<DptUser> GetUserData(Expression<Func<DptUser, bool>> funcWhere, int pageSize, int pageIndex, string sortName, string order);
        int DeleteUser(int id);
        int SetModule(List<SysUserModule> list, int userID);
        IEnumerable<AllotStation> GetStation(int id);
        /// <summary>
        /// 查询用户的定位信息
        /// </summary>
        /// <param name="fliter">过滤条件</param>
        /// <returns></returns>
        IEnumerable<dynamic> LoadUserPositionInfo(string fliter);
        //查询日志信息
        IEnumerable<dynamic> GetLogInfoTable(int pageindex, int pagesize, string orderby, string filter, ref int Totalcount);
        //查询客服人员
        IEnumerable<TeamUser> GetCusUser(string nickname);
        IEnumerable<SysUser> GetRoleUserData();
    }
}
