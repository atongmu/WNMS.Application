using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using WNMS.IService;
using WNMS.Model.CustomizedClass;
using WNMS.Utility;


namespace WNMS.Service
{
    public partial class Sws_UserPositionService : BaseService, ISws_UserPositionService
    {
        public Sws_UserPositionService(DbContext content) : base(content)
        {

        }
        #region 人员轨迹
        public PageResult<UserPosition> GetUserPosition(int pageindex, int pagesize)
        {
            string year = DateTime.Now.Year.ToString();
            SqlParameter[] sqlparameter = new SqlParameter[] {
                new SqlParameter("@year",year)
            };
            string sql = @"select * from dbo.Sys_User a LEFT JOIN [WNMS_Gps" + @year + "].[dbo].[RealTimePosition] b ON a.SerialNumber=b.SerialNumber  where a.SerialNumber is not null";

            PageResult<UserPosition> presult = new PageResult<UserPosition>();
            Expression<Func<UserPosition, bool>> funcWhere = r => true;
            presult = this.ExcuteQueryPage(funcWhere, pagesize, pageindex, "UpdateTime", sql, sqlparameter, true);
            return presult;
        }
        //未使用
        public IEnumerable<dynamic> UserPositionTree(string text)
        {
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@text","%"+text+"%")
           };
            string sql = @" with t as ( select u.UserID,NickName,SerialNumber,case when a.ID is null then -1 else a.ID end ID, case when a.AreaName is null then '未知区域' else a.AreaName end AreaName,case when a.PID is null then 0 else a.PID END PID from [dbo].[Sys_User] u
  left join [dbo].[WO_TeamUser] tu on u.UserID=tu.UserID
  left join [dbo].[WO_TeamInfo] t on tu.TeamID=t.TeamID
  left join [dbo].[WO_AreaInfo] a on t.RegionID=a.ID
  where SerialNumber is not null),
  area as(
  select * from t where AreaName like @text)," +
  @"parentd as(
 select ID,PID,AreaName from area
    union all
    select d.ID,d.PID,d.AreaName from [dbo].[WO_AreaInfo] d join parentd as d2 on d.ID=d2.PID
),
chd as (
select ID,PID,AreaName from area
    union all
    select d.ID,d.PID,d.AreaName from [dbo].[WO_AreaInfo] d join chd as d2 on d.PID=d2.ID
),
users as (
select ID,PID,AreaName, UserID,NickName,SerialNumber from t where NickName like @text" +
@"),
udepart as (
select ID,PID,AreaName from users 
    union all
    select d.ID,d.PID,d.AreaName from [dbo].[WO_AreaInfo] d join udepart as d2 on d.ID=d2.PID
),
r as(
select ID,PID,AreaName,'' as  NickName,1  as type,'' as SerialNumber,0  as UserID from parentd 
union all(
select ID,PID,AreaName,NickName,2 as type,SerialNumber,UserID from t where ID in (select ID  from chd)
)
union all(
select ID,PID,AreaName,'' as  NickName,1  as type,'' as SerialNumber,0  as UserID from udepart
)
union all(
select ID,PID,AreaName,NickName,2 as type,SerialNumber,UserID from users))select distinct * from r

";
            var query = this.Context.Database.SqlQuery_Dic(sql, sp);
            return query;
        }

        //没用到
        public dynamic GetUserPositionInfo(string mid)
        {
            string sql = @"select su.UserID,su.NickName,su.SerialNumber,rt.Lng,rt.Lat,CONVERT(varchar(20),rt.UpdateTime,120) UpdateTime from Sys_User su
                           left join [WNMS_Gps2021].[dbo].RealTimePosition rt on su.SerialNumber = rt.SerialNumber
                           where su.SerialNumber is not NULL AND su.UserID=" + mid;
            SqlParameter[] sqlparameter = new SqlParameter[] { };
            var user = this.Context.Database.SqlQuery_Dic(sql, sqlparameter).FirstOrDefault();
            return user;
        }
        #endregion
    }
}
