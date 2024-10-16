using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.Data.SqlClient;
using System.Text;
using WNMS.IService;
using WNMS.Model.CustomizedClass;
using WNMS.Model.DataModels;
using WNMS.Utility;

namespace WNMS.Service
{
    public partial class GD_EventsService : BaseService, IGD_EventsService
    {
        /// <summary>
        /// 查询事件信息
        /// </summary>
        /// <param name="funcWhere">查询条件</param>
        /// <param name="pageSize">分页每页多少条</param>
        /// <param name="pageIndex">第几页</param>
        /// <param name="funcOrderby">排序</param>
        /// <param name="userID"></param>
        /// <param name="ispage">是否需要分页</param>
        /// <param name="isAsc">是否为正序</param>
        /// <returns></returns>
        public PageResult<SEvents> LoadEventInfoList(Expression<Func<SEvents, bool>> funcWhere, string state, int pageSize, int pageIndex, string funcOrderby, int userID, bool ispage = true, bool isAsc = true)
        {
            SysUser user = this.Find<SysUser>(userID);

            SqlParameter[] sqlparameter = new SqlParameter[] {
                new SqlParameter("@userID",userID),
                new SqlParameter("@state",state)
            };
            string sql = @" with t as (
                 select e.*,t.StationID from GD_Events e left join [dbo].[GD_Inspection] t on e.TaskID=t.InspectionID where e.incidentType=0
                 union
                 select e.*,t.StationID from GD_Events e left join [dbo].[GD_Maintain] t on e.TaskID=t.MaintainID where e.incidentType=1
                 union 
                 select e.*,t.StationID from GD_Events e left join [dbo].[GD_Repair] t on e.TaskID=t.RepairID where e.incidentType=2)
                 select t.*,s.StationName,u.Account from t left join [dbo].[Sws_Station] s on t.stationID=s.StationID left join [dbo].[Sys_User] u on t.ReportUser=u.UserID
                 where t.StationID is not null and s.StationID is not null";

            if (!user.IsAdmin)     //非admin查询
            {
                sql += " and t.StationID in (select StationID from Sws_UserStation where UserID=@userID) or t.StationID=0";
            }

            if (!string.IsNullOrEmpty(state) && state != "8")
            {
                if (state == "6" || state == "7")
                {
                    sql += " and t.IncidentID in (select EventID from[GD_WorkOrder] where CurrentState = @state)";
                }
                else
                {
                    sql += " and t.IncidentState=@state";
                }

            }

            PageResult<SEvents> presult = new PageResult<SEvents>();
            if (ispage)   //分页查询
            {
                presult = this.ExcuteQueryPage(funcWhere, pageSize, pageIndex, funcOrderby, sql, sqlparameter, isAsc);
            }
            else     //查询所有
            {
                List<SEvents> list = this.Context.Database.SqlQuery<SEvents>(sql, sqlparameter);
                presult.DataList = list;
            }
            return presult;
        }

        /// <summary>
        /// 根据ID查询事件信息
        /// </summary>
        /// <param name="id">事件id</param>
        /// <returns></returns>
        public IEnumerable<SEvents> GetIncidentByID(long id)
        {
            SqlParameter[] sqlparameter = new SqlParameter[] {
                new SqlParameter("@id",id)
            };
            string sql = @" with t as (
                 select e.*,t.StationID from GD_Events e left join [dbo].[GD_Inspection] t on e.TaskID=t.InspectionID where e.incidentType=0
                 union
                 select e.*,t.StationID from GD_Events e left join [dbo].[GD_Maintain] t on e.TaskID=t.MaintainID where e.incidentType=1
                 union 
                 select e.*,t.StationID from GD_Events e left join [dbo].[GD_Repair] t on e.TaskID=t.RepairID where e.incidentType=2)
                 select t.*,s.StationName,u.Account from t left join [dbo].[Sws_Station] s on t.stationID=s.StationID left join [dbo].[Sys_User] u on t.ReportUser=u.UserID
                 where t.StationID is not null and s.StationID is not null and t.IncidentID=@id";

            var query = this.Context.Database.SqlQuery<SEvents>(sql, sqlparameter);
            return query;
        }

        /// <summary>
        /// 派发数据查询
        /// </summary>
        /// <param name="eventID">事件ID</param>
        /// <returns></returns>
        public IEnumerable<dynamic> GetTreateTaskData(string eventID)
        {
            SqlParameter[] sp = new SqlParameter[] {
              new SqlParameter("@eventID",eventID)
            };
            string sql = @"select w.WOID,w.Num,CurrentState,c.UserID,c.Account,min(o.OperationTime) as ComeTime,min(p.OperationTime) as TreateTime,min(e.OperationTime) as CompleteTime from [dbo].[GD_WorkOrder] w
                        left join   [dbo].[Sys_User] c on w.UserID=c.UserID
                        left join  [dbo].[GD_WOOperation] o on w.WOID=o.PID and o.Type=2
                        left join  [dbo].[GD_WOOperation] p on w.WOID=p.PID and p.Type=3
                        left join  [dbo].[GD_WOOperation] e on w.WOID=e.PID and e.Type=4
                        where w.EventID=@eventID group by w.WOID,w.Num,CurrentState,c.UserID,c.Account";
            var query = this.Context.Database.SqlQuery_Dic(sql, sp);
            return query;
        }

        /// <summary>
        /// 任务派发时获取班组信息
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TeamUser> GetTeamInfo()
        {
            SqlParameter[] sqlparameter = new SqlParameter[] {
            };
            string sql = @"select tu.*,t.TeamName,u.Account from GD_TeamUser tu left join GD_TeamInfo t
                            on tu.TeamID=t.TeamID left join Sys_User u on tu.UserID=u.UserID";

            var query = this.Context.Database.SqlQuery<TeamUser>(sql, sqlparameter);
            return query;
        }

        #region 任务派发
        public int AddWorkerOrder(GdWorkOrder order, GdWooperation wo, GdEvents e)
        {
            //添加工单
            this.Context.Add<GdWorkOrder>(order);
            this.Context.Add<GdWooperation>(wo);
            this.Context.Update<GdEvents>(e);

            return this.Context.SaveChanges();
        }
        #endregion

        #region 工单审核
        //工单审核
        public int EditWorkOrderAudit(GdWorkOrder w, GdWooperation woo)
        {
            //添加工单
            this.Context.Update<GdWorkOrder>(w);
            this.Context.Add<GdWooperation>(woo);
            return this.Context.SaveChanges();
        }
        #endregion
    }
}
