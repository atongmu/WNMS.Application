using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using WNMS.IService;
using WNMS.Model.DataModels;
using WNMS.Utility;
using Microsoft.Data.SqlClient;
namespace WNMS.Service
{
    public partial class WO_TeamInfoService : BaseService, IWO_TeamInfoService
    {
        public IEnumerable<SysUser> GetUserByTeamIDOfPage(long teamid, int pageindex, int pagesize, ref int TotalCount)
        {
            var query = from tu in Query<WoTeamUser>(t => t.TeamId == teamid)
                        join u in Query<SysUser>(q => true) on tu.UserId equals u.UserId into u1
                        from uu in u1.DefaultIfEmpty()
                        where uu != null
                        orderby uu.NickName
                        select uu;
            TotalCount = query.Count();
            query = query.Skip((pageindex - 1) * pagesize).Take(pagesize);
            return query;
        }


        //提交表单
        public int SetTeamUser(WoTeamInfo t, List<WoTeamUser> list)
        {
            List<WoTeamUser> tuser = this.Query<WoTeamUser>(r => r.TeamId == t.TeamId).AsNoTracking().ToList();
            if (tuser != null && tuser.Count > 0)
            {
                foreach (var item in tuser)
                {
                    this.Context.Attach(item);
                }

                this.Context.RemoveRange(tuser);
            }

            if (list.Count > 0)
            {
                foreach (var item in list)
                {
                    this.Context.Attach(item);
                }

                this.Context.AddRange(list);
            }

            return this.Context.SaveChanges();




        }
        //添加
        public int AddTeamInfo(WoTeamInfo t, List<WoTeamUser> list)
        {
            this.Context.Set<WoTeamInfo>().Add(t);
            if (list.Count > 0)
            {
                this.Context.Set<WoTeamUser>().AddRange(list);
            }
            return this.Context.SaveChanges();
        }
        //修改
        public int EditeTeamInfo(WoTeamInfo t, List<WoTeamUser> list)
        {
            var old_tu = this.Context.Set<WoTeamUser>().Where(r => r.TeamId == t.TeamId);
            if (old_tu.Count() > 0)
            {
                this.Context.Set<WoTeamUser>().RemoveRange(old_tu);
            }
            this.Context.Set<WoTeamUser>().AddRange(list);
            this.Context.Set<WoTeamInfo>().Update(t);
            return this.Context.SaveChanges();
        }

        public IEnumerable<dynamic> GetTeamTreeNode()
        {
            SqlParameter[] sp = new SqlParameter[]
            {

            };
            var sql =
                @"  select ID,AreaName,PID,0 as IsTeam from [dbo].WO_AreaInfo where ID in (select RegionID from [dbo].WO_TeamInfo)
  union all(
  select TeamID, TeamName, RegionID,1 as IsTeam from [dbo].WO_TeamInfo)";
            var query = this.Context.Database.SqlQuery_Dic(sql, sp);
            return query;
        }


        //删除 
        public int DeleteTeamInfos(List<long> ids)
        {
            var teamlist = this.Context.Set<WoTeamInfo>().Where(r => ids.Contains(r.TeamId)).ToList();
            var teamUser = this.Context.Set<WoTeamUser>().Where(r => ids.Contains(r.TeamId)).ToList();
            if (teamlist.Count > 0)
            {
                this.Context.Set<WoTeamInfo>().RemoveRange(teamlist);
            }
            if (teamUser.Count > 0)
            {
                this.Context.Set<WoTeamUser>().RemoveRange(teamUser);
            }
            return this.Context.SaveChanges();
        }

        public IEnumerable<dynamic> GetTeamUserInfo(int teamid, string username, string sort, string order, int pageSize, int pageIndex, ref int totalcount)
        {
            var query = from t in this.Context.Set<WoTeamUser>()
                        join a in this.Context.Set<SysUser>() on t.UserId equals a.UserId into dd
                        from dt in dd.DefaultIfEmpty()
                        where t.TeamId == teamid && dt != null
                        select new
                        {
                            UserID = t.UserId,
                            UserName = dt.Account,
                            UserFullName = dt.NickName,
                            Phone = dt.Phone,
                            WeChatID = dt.WeChatKey,
                            HeadIcon = dt.HeadIcon
                        };
            if (!string.IsNullOrEmpty(username))
            {
                query = query.Where(r => r.UserFullName.Contains(username));
            }
            if (sort == "asc")
            {
                query = query.OrderBy(r => order);
            }
            else
            {
                query = query.OrderByDescending(r => order);
            }
            totalcount = query.Count();
            query = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            return query;
        }


        //部门人员树查询
        public IEnumerable<dynamic> GetUserTree(string searchtext, int teamid)
        {
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@searchtext","%"+searchtext+"%"),
                new SqlParameter("@TeamID", teamid)
            };
            //            string sql = @" with t as (select u.UserID,NickName,d.* ,case when TeamID is null then 0 else 1 end Ischeack from [dbo].[Sys_User] u
            //  left join [dbo].[Sys_DepartMent] d on u.DepartMent=d.DepartmentID
            //  left join [dbo].[WO_TeamUser] t on u.UserID=t.UserID
            // where u.UserID not in (select UserID from[dbo].[WO_TeamUser] where TeamID!=@TeamID)),
            // depart as (
            // select * from t where DptName like '%" + @searchtext + "%')," +
            //                         @"parentd as(
            // select [DepartmentID],[PID],[DptName] from depart 
            //    union all
            //    select d.[DepartmentID],d.[PID],d.[DptName] from Sys_DepartMent d join parentd as d2 on d.DepartmentID=d2.PID
            //),
            //chd as (
            //select [DepartmentID],[PID],[DptName] from depart
            //    union all
            //    select d.[DepartmentID],d.[PID],d.[DptName] from Sys_DepartMent d join chd as d2 on d.PID=d2.DepartmentID
            //)
            //select DepartmentID ,PID ,DptName,0 as UserID,'' as  NickName,1  as type,0 as Ischeack from parentd 
            //union all(
            //select DepartmentID,PID,DptName, UserID,NickName,2 as type,Ischeack from t where DepartmentID in (select DepartmentID  from chd)
            //)
            //union all(
            //select DepartmentID,PID,DptName, UserID,NickName,2 as type,Ischeack from t where NickName like '%" + @searchtext + "%') ";
            string sql = @"  with t as (select u.UserID,NickName,d.*  from [dbo].[Sys_User] u
  left join [dbo].[Sys_DepartMent] d on u.DepartMent=d.DepartmentID
  left join [dbo].[WO_TeamUser] t on u.UserID=t.UserID
 where u.UserID not in (select UserID from[dbo].[WO_TeamUser] where TeamID!=@TeamID)),
 depart as (
 select * from t where DptName like @searchtext )," +
 @"parentd as(
 select [DepartmentID],[PID],[DptName] from depart 
    union all
    select d.[DepartmentID],d.[PID],d.[DptName] from Sys_DepartMent d join parentd as d2 on d.DepartmentID=d2.PID
),
chd as (
select [DepartmentID],[PID],[DptName] from depart
    union all
    select d.[DepartmentID],d.[PID],d.[DptName] from Sys_DepartMent d join chd as d2 on d.PID=d2.DepartmentID
),
users as (
select DepartmentID,PID,DptName, UserID,NickName from t where NickName like @searchtext "+
@")
,
udepart as (
select [DepartmentID],[PID],[DptName] from users 
    union all
    select d.[DepartmentID],d.[PID],d.[DptName] from Sys_DepartMent d join udepart as d2 on d.DepartmentID=d2.PID
),
r as(
select DepartmentID ,PID ,DptName,0 as UserID,'' as  NickName,1  as type from parentd 
union all(
select DepartmentID,PID,DptName, UserID,NickName,2 as type from t where DepartmentID in (select DepartmentID  from chd)
)
union all(
select DepartmentID ,PID ,DptName,0 as UserID,'' as  NickName,1  as type from udepart
)
union all(
select DepartmentID,PID,DptName, UserID,NickName,2 as type from users))select distinct * from r";
            var query = this.Context.Database.SqlQuery_Dic(sql, sp);
            return query;
        }


        //获取未分配的人员
        public IEnumerable<dynamic> GetUnallotUser(int teamid)
        {
            //           SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@TeamID", teamid) };

            //           string sql = @" select u.UserID,u.NickName,d.* ,case when TeamID is null then 0 else 1 end Ischeack from [dbo].[Sys_User] u
            // left join [dbo].[Sys_DepartMent] d on u.Department=d.DepartmentID 
            // left join [dbo].[WO_TeamUser] t on u.UserID=t.UserID
            //where u.UserID not in (select UserID from[dbo].[WO_TeamUser] where TeamID!=@TeamID)";
            //           var query = this.Context.Database.SqlQuery_Dic(sql, sp);
            //           return query;
            SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@TeamID", teamid) };

            string sql = @" select u.UserID,u.NickName,d.*  from [dbo].[Sys_User] u
  left join [dbo].[Sys_DepartMent] d on u.Department=d.DepartmentID 
  left join [dbo].[WO_TeamUser] t on u.UserID=t.UserID
 where u.UserID not in (select UserID from[dbo].[WO_TeamUser] where TeamID!=@TeamID)";
            var query = this.Context.Database.SqlQuery_Dic(sql, sp);
            return query;
        }
        //获取已分配的人员
        public IEnumerable<dynamic> GetAllotUser(int teamid)
        {
            SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@TeamID", teamid) };
            string sql = @"select u.UserID,u.NickName,d.DepartmentID,d.DptName from [dbo].[Sys_User] u
  left join [dbo].[Sys_DepartMent] d on u.Department=d.DepartmentID 
  where UserID  in (select UserID from [dbo].[WO_TeamUser]  WHERE TeamID=@TeamID ) ";
            var query = this.Context.Database.SqlQuery_Dic(sql, sp);
            return query;
        }

        //班组管理树查询
        public IEnumerable<dynamic> SearchTreeTeam(string teamname)
        {
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@teamname","%"+teamname+"%")
            };
            //t 查出区域含有teamname的数据列，ct 查出包含搜索内容的所有区域以及其子集，cteam查出ct的所有班组，pt查出ct的所有父级，
            //m查出包含搜索内容的所有班组，mt查出所有m的区域,mtt查出mt父级区域，data 返回最终的结果
            string sql = @"with t as (select a.* from [dbo].[WO_TeamInfo] f
                        left join [dbo].[WO_AreaInfo] a on a.ID=f.RegionID where AreaName like @teamname)," +
                            @" base as (select a.* from [dbo].[WO_TeamInfo] f
                        left join [dbo].[WO_AreaInfo] a on a.ID=f.RegionID),
                        ct as (
                        select *,0 as IsTeam from base where ID in (select ID FROM t) or PID in (select ID from t)),
                        cteam as(
                        select * ,1 as IsTeam from [dbo].[WO_TeamInfo] where RegionID in (select ID from ct)),
                        pt as (select *,0 as IsTeam  from base where PID in (select ID from t)),
                        m as (select *,1 as IsTeam from [dbo].[WO_TeamInfo]  where TeamName like @teamname)," +
                            @"mt as (
                        select *,0 as IsTeam from base where ID in (select RegionID from m) ),
                        mtt as(select *,0 as IsTeam from base where ID in (select PID FROM mt)),
                        data as (
                        select ID,AreaName,PID,IsTeam from ct
                        union all(
                        select TeamID,TeamName,RegionID,IsTeam from cteam
                        )
                        union all(
                        select ID,AreaName,PID,IsTeam from pt
                        )
                        union all(select TeamID,TeamName,RegionID,IsTeam from m)
                        union all(
                        select ID,AreaName,PID,IsTeam from mt
                        )
                        union all(
                        select ID,AreaName,PID,IsTeam from mtt
                        ))
                        select distinct * from data
                        ";
            var query = this.Context.Database.SqlQuery_Dic(sql, sp);
            return query;
        }
        public IEnumerable<dynamic> GetUserByTeamID(int teamid)
        {
            SqlParameter[] sp = new SqlParameter[] {
            new SqlParameter("@teamid",teamid)
            };
            string sql = @"  select u.UserID,u.NickName from [dbo].[WO_TeamUser] tu
                      left join [dbo].[Sys_User] u on tu.UserID=u.UserID
                      where tu.TeamID=@teamid and u.UserID is not null";
            var query = this.Context.Database.SqlQuery_Dic(sql, sp);
            return query;
        }
    }
}
