using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Text;
using WNMS.IService;
using WNMS.Utility;
using System.Linq;
using WNMS.Model.DataModels;
namespace WNMS.Service
{
    public partial class GD_TeamInfoService : BaseService, IGD_TeamInfoService
    {

        public IEnumerable<dynamic> GetTeamInfoTable(int pageindex, int pagesize, string orderby, string filter, ref int Totalcount)
        {
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@startindex",pageindex),
                new SqlParameter("@pageSize",pagesize),
                new SqlParameter("@orderItems",orderby),
                new SqlParameter("@filterString",filter),
                new SqlParameter("@Totalcount",Totalcount){Direction=System.Data.ParameterDirection.Output,DbType=System.Data.DbType.Int32}
            };
            var query = this.Context.Database.SqlQuery_Dic("exec QueryGD_TeamInfoTable @startindex,@pageSize,@orderItems,@filterString,@Totalcount Output", sp).ToList();
            Totalcount = Convert.ToInt32(sp[4].Value);
            return query;
        }
        //部门人员树(功能未启用，注销2021.9.9)
        public IEnumerable<dynamic> GetDepartUserNode(string searchText, int teamid, string userids)
        {
            //            SqlParameter[] sp = new SqlParameter[] {
            //                new SqlParameter("@searchText",searchText),
            //                new SqlParameter("@teamid",teamid),
            //                new SqlParameter("@userids",userids)
            //             };
            //            string sql = @"  with t as (select u.UserID,Account,d.DepartmentID,d.DptName,d.PID ,case when CHARINDEX(','+LTRIM(u.UserID)+',','," + @userids + ",')<=0 then 0 else 1 end Ischeack from [dbo].[Sys_User] u " +
            // @" left join [dbo].[Sys_DepartMent] d on u.Department=d.DepartmentID
            //  left join [dbo].[GD_TeamUser] t on u.UserID=t.UserID
            //  left join (select UserID from [dbo].[GD_TeamUser] where TeamID!=@teamid) g on u.UserID=g.UserID
            // where g.UserID is null),
            // depart as (
            // select * from t where DptName like '%" + @searchText + "%'),parentd as(" +
            // @"select DepartmentID, PID, DptName from depart 
            //    union all
            //    select d.DepartmentID, d.PID, d.DptName from [Sys_DepartMent] d join parentd as d2 on d.DepartmentID=d2.PID
            //),
            //chd as (
            //select DepartmentID, PID, DptName from depart
            //    union all
            //    select d.DepartmentID, d.PID, d.DptName from [Sys_DepartMent] d join chd as d2 on d.PID=d2.DepartmentID
            //)
            //select DepartmentID ,PID ,DptName,0 as UserID,'' as  Account,1  as type,0 as Ischeack from parentd 
            //union (
            //select t.DepartmentID ,PID ,DptName, UserID,Account,2 as type,Ischeack from t 
            //left join (select DepartmentID  from chd) n on t.DepartmentID=n.DepartmentID
            //where n.DepartmentID is not null
            //)
            //union (
            //select DepartmentID ,PID ,DptName, UserID,Account,2 as type,Ischeack from t where Account like '%" + @searchText + "%')";
            //            var query = this.Context.Database.SqlQuery_Dic(sql, sp);
            //            return query;
            return null;

        }
        //获取已分配的人员
        public IEnumerable<dynamic> GetAllotUser(int teamid)
        {
            SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@TeamID", teamid) };
            string sql = @"    select  u.UserID,u.Account,u.Department,u.Phone,u.Email,u.HeadIcon from [dbo].[Sys_User] u
  left join [dbo].[Sys_DepartMent] d on u.Department=d.DepartmentID
  left join (select UserID from [dbo].[GD_TeamUser] where TeamID=@TeamID) g on u.UserID=g.UserID
  where g.UserID is not null ";
            var query = this.Context.Database.SqlQuery_Dic(sql, sp);
            return query;
        }


        //添加班组
        public int AddTeamInfo(GdTeamInfo t, List<int> userids)
        {
            this.Context.Set<GdTeamInfo>().Add(t);
            List<GdTeamUser> list = new List<GdTeamUser>() { };
            foreach (var item in userids)
            {
                GdTeamUser a = new GdTeamUser();
                a.TeamId = t.TeamId;
                a.UserId = item;
                list.Add(a);
            }
            this.Context.Set<GdTeamUser>().AddRange(list);
            return this.Context.SaveChanges();
        }
        //修改班组
        public int EditeTeamInfo(GdTeamInfo t, List<int> userids)
        {
            this.Context.Set<GdTeamInfo>().Update(t);
            var oldUsers = this.Context.Set<GdTeamUser>().Where(r => r.TeamId == t.TeamId);
            if (oldUsers.Count() > 0)
            {
                this.Context.Set<GdTeamUser>().RemoveRange(oldUsers);
            }
            List<GdTeamUser> list = new List<GdTeamUser>() { };
            foreach (var item in userids)
            {
                GdTeamUser a = new GdTeamUser();
                a.TeamId = t.TeamId;
                a.UserId = item;
                list.Add(a);
            }
            this.Context.Set<GdTeamUser>().AddRange(list);
            return this.Context.SaveChanges();
        }
        //删除班组
        public int DeleteTeamInfo(string teamids)
        {
            List<int> teamidList = new List<string>(teamids.Split(",")).ConvertAll(r => int.Parse(r));
            var teaminfos = this.Context.Set<GdTeamInfo>().Where(r => teamidList.Contains(r.TeamId));
            var tamusers = this.Context.Set<GdTeamUser>().Where(r => teamidList.Contains(r.TeamId));
            this.Context.Set<GdTeamInfo>().RemoveRange(teaminfos);
            this.Context.Set<GdTeamUser>().RemoveRange(tamusers);
            return this.Context.SaveChanges();
        }
    }
}
