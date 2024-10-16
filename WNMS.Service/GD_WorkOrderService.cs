using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;
using WNMS.IService;
using WNMS.Utility;
using System.Linq;
using WNMS.Model.DataModels;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace WNMS.Service
{
    public partial class GD_WorkOrderService : BaseService, IGD_WorkOrderService
    {
        public IEnumerable<dynamic> GetWorkTable(string beginDate, string endDate, string State, int pageSize, int pageIndex, string field, string order, string eventsType, long UserID, string message, ref int totalCount)
        {
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@beginDate",beginDate),
                new SqlParameter("@endDate",endDate),
                new SqlParameter("@State",State),
                new SqlParameter("@pageSize",pageSize),
                new SqlParameter("@pageIndex",pageIndex),
                new SqlParameter("@field",field),
                new SqlParameter("@order",order),
                new SqlParameter("@eventsType",eventsType),
                new SqlParameter("@UserID",UserID),
                new SqlParameter("@message",message),
                new SqlParameter("@totalCount",totalCount){Direction=System.Data.ParameterDirection.Output,DbType=System.Data.DbType.Int32}
            };
            var query = this.Context.Database.SqlQuery_Dic("exec QueryGD_WorkOrderData @beginDate,@endDate, @State,@pageSize,@pageIndex,@field,@order,@eventsType,@UserID,@message,@totalCount Output", sp).ToList();
            totalCount = Convert.ToInt32(sp[10].Value);
            return query;
        }

        public dynamic GetWO_EventData(long WOID)
        {
            SqlParameter[] sp = new SqlParameter[] {
              new SqlParameter("@WOID",WOID)
            };
            var sql = @"select wo.WOID,wo.Degree,wo.HandleLevel,us.Account,wo.CompleteTime, usu.Account AS ReleaseUser,wo.ReleaseTime,wo.IsAuditing,wo.Num,
                        wo.EventID,wo.CurrentState,de.IncidentNum,de.IncidentState,de.IncidentType,de.IncidentContent,de.IncidentSource,de.ReportTime,de.ReportUser, de.Picture,de.Description,dpt.TeamName,
                        (select count(*) from [dbo].[GD_Resource] where PID=de.TaskID AND  Type=1 and ResourceType=1) as ImgCount,
                        (select count(*) from [dbo].[GD_Resource] where PID=de.TaskID AND   Type=2 and ResourceType=1) as recordCount 
                        from GD_WorkOrder wo left join GD_Events de on wo.EventID = de.IncidentID 
                        left join  [Sys_User] us  on de.ReportUser =us.UserID
                        LEFT OUTER JOIN  [Sys_User] AS usu ON wo.ReleaseUser = usu.UserID
                        LEFT OUTER JOIN GD_TeamUser dp on usu.userid = dp.userid
                        LEFT OUTER JOIN GD_TeamInfo dpt on dp.TeamID = dpt.TeamID
                        where wo.WOID=@WOID";
            var query = this.Context.Database.SqlQuery_Dic(sql, sp).FirstOrDefault();
            return query;
        }
        //获取工单操作历史 
        public List<dynamic> GetWO_OpData(long WOID, int oType)
        {
            SqlParameter[] sp = new SqlParameter[] {
              new SqlParameter("@WOID",WOID),
              new SqlParameter("@oType",oType)
            };
            var sql = @"select o.ID, o.UserID, o.OperationTime, o.Description,us.Account,o.PID, dpt.TeamName,o.Type,o.State, 
                        r.Path, r.ResourceType,  (SELECT   COUNT(*) AS Expr1 FROM      GD_Resource dr 
                        left join GD_WOOperation oo on dr.ID = oo.ID
                        WHERE (dr.PID = o.ID) AND  (dr.Type = 1) AND (dr.ResourceType = 2)) AS ImgCount,
                        (select count(*) from [dbo].[GD_Resource] dr 
                        left join GD_WOOperation oo on dr.ID = oo.ID  where (dr.PID = o.ID) AND   (dr.Type = 2) AND (dr.ResourceType = 2)) as recordCount  
                        from GD_WOOperation o left join GD_Resource r on  o.PID = r.ID
                        LEFT OUTER JOIN
                        [Sys_User] AS us ON o.UserID = us.UserID 
			            LEFT OUTER JOIN GD_TeamUser dp on us.userid = dp.userid
	                    LEFT OUTER JOIN GD_TeamInfo dpt on dp.TeamID = dpt.TeamID
                        where o.PID=@WOID   ";

            if (oType != 1)
            {
                sql += " and o.Type=@oType ";
            }
            sql += " order by OperationTime asc";
            var query = this.Context.Database.SqlQuery_Dic(sql, sp);
            return query.ToList();
        }

        //获取工单延期信息
        public IEnumerable<dynamic> LoadExtensionData(long woid)
        {
            var allList = from d in Query<GdWoextension>(s => s.Woid == woid)
                          join m in Query<SysUser>(s => true) on d.UserId equals m.UserId into dm
                          from dmt in dm
                          join u in Query<SysUser>(u => true) on d.Auditor equals u.UserId into su
                          from sut in su.DefaultIfEmpty()
                          select new
                          {
                              WOID = d.Woid,
                              Exuser = dmt.Account,
                              ExtensionTime = d.ExtensionTime,
                              CompleteTime = d.CompleteTime,
                              Auditor = sut.Account,
                              AuditingTime = d.AuditingTime,
                              State = d.State == 1 ? "审核通过" : "审核未通过",
                              Reason = d.Reason
                          };
            return allList;
        }
        //退单修改
        public int EditWo(GdWorkOrder gdWorkOrder, GdWooperation gdWooperation)
        {
            try
            {
                this.Context.Update(gdWorkOrder);
                this.Context.Add(gdWooperation);
                this.Context.SaveChanges();
            }
            catch (Exception ex)
            {
                return -1;
            }
            return 1;

        }

        //移交工单
        public int AddWOTransfer(GdWoextension gD_WOReview, GdWorkOrder newWo, GdWooperation wo, GdWorkOrder oldWo, GdWooperation newwoop)
        {
            try
            {
                //添加新工单
                this.Context.Add(newWo);
                //添加工单历史表
                this.Context.Add(wo);
                //修改父工单状态
                this.Context.Update(oldWo);
                //添加新工单历史
                this.Context.Add(newwoop);
                //更新审核表状态
                if (gD_WOReview.Id != 0)
                {
                    this.Context.Update(gD_WOReview);
                }
                this.Context.SaveChanges();
            }
            catch (Exception ex)
            {
                return -1;
            }
            return 1;
        }

        //延期申请
        public IEnumerable<dynamic> GetWoEInfoTable(int pageindex, int pagesize, string orderby, string filter, ref int Totalcount)
        {
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@startindex",pageindex),
                new SqlParameter("@pageSize",pagesize),
                new SqlParameter("@orderItems",orderby),
                new SqlParameter("@filterString",filter),
                new SqlParameter("@Totalcount",Totalcount){Direction=System.Data.ParameterDirection.Output,DbType=System.Data.DbType.Int32}
            };
            var query = this.Context.Database.SqlQuery_Dic("exec QueryGD_WOExtension @startindex,@pageSize,@orderItems,@filterString,@Totalcount Output", sp).ToList();
            Totalcount = Convert.ToInt32(sp[4].Value);
            return query;
        }

        //审核延期申请
        public int EditWoExtension(GdWoextension gdWoextension, GdWorkOrder gdWorkOrder, GdWooperation gdWooperation)
        {
            try
            {
                this.Context.Update(gdWoextension);
                this.Context.Update(gdWorkOrder);
                this.Context.Add(gdWooperation);
                this.Context.SaveChanges();
            }
            catch (Exception ex)
            {
                return -1;
            }
            return 1;
        }
        //审核延期申请未通过
        public int EditNoWoExtension(GdWoextension gdWoextension, GdWooperation gdWooperation)
        {
            try
            {
                this.Context.Update(gdWoextension);
                this.Context.Add(gdWooperation);
                this.Context.SaveChanges();
            }
            catch (Exception ex)
            {
                return -1;
            }
            return 1;
        }

        //移交申请
        public IEnumerable<dynamic> GetWoReInfoTable(int pageindex, int pagesize, string orderby, string filter, ref int Totalcount)
        {
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@startindex",pageindex),
                new SqlParameter("@pageSize",pagesize),
                new SqlParameter("@orderItems",orderby),
                new SqlParameter("@filterString",filter),
                new SqlParameter("@Totalcount",Totalcount){Direction=System.Data.ParameterDirection.Output,DbType=System.Data.DbType.Int32}
            };
            var query = this.Context.Database.SqlQuery_Dic("exec QueryGD_WOReview @startindex,@pageSize,@orderItems,@filterString,@Totalcount Output", sp).ToList();
            Totalcount = Convert.ToInt32(sp[4].Value);
            return query;
        }
        //审核退单申请通过
        public int EditWoOkReview(GdWoreview gdWoreview, GdWorkOrder gdWorkOrder, GdWooperation gdWooperation)
        {
            try
            {
                this.Context.Update(gdWoreview);
                this.Context.Update(gdWorkOrder);
                this.Context.Add(gdWooperation);
                this.Context.SaveChanges();
            }
            catch (Exception ex)
            {
                return -1;
            }
            return 1;
        }
        //审核退单申请未通过
        public int EditWoNoReview(GdWoreview gdWoreview, GdWooperation gdWooperation)
        {
            try
            {
                this.Context.Update(gdWoreview);
                this.Context.Add(gdWooperation);
                this.Context.SaveChanges();
            }
            catch (Exception ex)
            {
                return -1;
            }
            return 1;
        }

        //审核移交申请通过
        public int AddWOTransfer(GdWoreview gdWoreview, GdWorkOrder gdWorkOrder, GdWooperation woop, GdWorkOrder woInfo, GdWooperation newwoop)
        {
            try
            {
                this.Context.Add(gdWorkOrder);
                this.Context.Update(gdWoreview);
                this.Context.Add(woop);
                this.Context.Update(woInfo);
                this.Context.Add(newwoop);
                this.Context.SaveChanges();
            }
            catch (Exception ex)
            {
                return -1;
            }
            return 1;
        }

        //审核移交申请未通过
        public int AddWONoTransfer(GdWoreview gdWoreview, GdWooperation gdWooperation)
        {
            try
            {
                this.Context.Update(gdWoreview);
                this.Context.Add(gdWooperation);
                this.Context.SaveChanges();
            }
            catch (Exception ex)
            {
                return -1;
            }
            return 1;
        }

        #region 工单处理接口方法
        public IEnumerable<dynamic> LoadWoEvent(long userId)
        {
            var allList = from d in Query<GdWorkOrder>(s => s.UserId == userId)
                          join m in Query<GdEvents>(s => true) on d.EventId equals m.IncidentId into dm
                          from dmt in dm
                          where d.CurrentState < 4
                          select new
                          {
                              d.Woid,
                              d.ReleaseTime,
                              d.CompleteTime,
                              dmt.IncidentContent,
                              Degree = Enum.GetName(typeof(WNMS.Model.CustomizedClass.EmergencyDegree), int.Parse(d.Degree.ToString())),
                              HandleLevel = Enum.GetName(typeof(WNMS.Model.CustomizedClass.ProcessingLevel), int.Parse(d.HandleLevel.ToString())),
                              dmt.IncidentId
                          };
            return allList.ToList();

        }
        /// <summary>
        /// 获取单个工单信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> LoadWoById(long Id)
        {

            var allList = from d in Query<GdWorkOrder>(s => s.Woid == Id)
                          join m in Query<GdEvents>(s => true) on d.EventId equals m.IncidentId into dm
                          from dmt in dm
                          where d.CurrentState < 4
                          select new
                          {
                              d.Woid,
                              d.ReleaseTime,
                              d.CompleteTime,
                              dmt.IncidentContent,
                              Degree = Enum.GetName(typeof(WNMS.Model.CustomizedClass.EmergencyDegree), int.Parse(d.Degree.ToString())),
                              HandleLevel = Enum.GetName(typeof(WNMS.Model.CustomizedClass.ProcessingLevel), int.Parse(d.HandleLevel.ToString())),
                              dmt.IncidentId
                          };
            return allList.ToList();

        }
        //添加工单操作信息
        public int AddWoOp(GdWooperation gd_WOOperation, GdWorkOrder gdWorkOrder, GdEvents gdEvents)
        {
            try
            {
                this.Context.Add(gd_WOOperation);
                this.Context.Update(gdWorkOrder);
                if (gdEvents != null)
                {
                    this.Context.Update(gdEvents);
                }
                this.Context.SaveChanges();
            }
            catch (Exception ex)
            {
                return -1;
            }
            return 1;
        }
        #endregion
    }
}
