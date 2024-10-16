using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;
using WNMS.IService;
using WNMS.Utility;
using System.Linq;
using WNMS.Model.DataModels;
using WNMS.Model.CustomizedClass;
using Enum = System.Enum;

namespace WNMS.Service
{
    public partial class WO_WorkOrderService : BaseService, IWO_WorkOrderService
    {
        //查询工单信息
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
            var query = this.Context.Database.SqlQuery_Dic("exec QueryWO_WorkOrderData @beginDate,@endDate, @State,@pageSize,@pageIndex,@field,@order,@eventsType,@UserID,@message,@totalCount Output", sp).ToList();
            totalCount = Convert.ToInt32(sp[10].Value);
            return query;
        }
        //查询工单事件详情
        public dynamic GetWO_EventData(long WOID)
        {
            SqlParameter[] sp = new SqlParameter[] {
              new SqlParameter("@WOID",WOID)
            };
            var sql = @"select wo.WOID,wo.Degree,wo.HandleLevel,us.NickName as Account,wo.CompleteTime, usu.NickName AS ReleaseUser,wo.ReleaseTime,wo.IsAuditing,wo.Num,de.Address,sde.DeviceName,
                        wo.EventID,wo.CurrentState,de.IncidentState,de.IncidentType,de.IncidentNum,de.IncidentContent,de.IncidentSource,de.ReportTime,de.ReportUser, de.Description,dpt.TeamName,
                        (select count(*) from [dbo].[WO_Resource] where PID=wo.EventID AND  Type=1 and ResourceType=5) as ImgCount,
                        (select count(*) from [dbo].[WO_Resource] where PID=wo.EventID AND   Type=2 and ResourceType=5) as recordCount 
                        from WO_WorkOrder wo left join WO_Events de on wo.EventID = de.IncidentID 
                        left join  [Sys_User] us  on de.ReportUser =us.UserID
                        LEFT OUTER JOIN  [Sys_User] AS usu ON wo.ReleaseUser = usu.UserID
                        LEFT OUTER JOIN WO_TeamUser dp on us.userid = dp.userid
                        LEFT OUTER JOIN WO_TeamInfo dpt on dp.TeamID = dpt.TeamID
                        left join Sws_DeviceInfo01 sde on de.EquipmentID = sde.DeviceID
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
            var sql = @" select o.ID, o.UserID, o.OperationTime, o.Description,us.NickName as Account,o.PID, dpt.TeamName,o.Type,o.State,  r.Path, r.ResourceType,
                         (SELECT   COUNT(*) AS Expr1 FROM   WO_Resource dr left join WO_WOOperation oo on dr.ID = oo.ID
                          WHERE (dr.PID = o.ID) AND  (dr.Type = 1) AND (dr.ResourceType = 6)) AS ImgCount,
                         (select count(*) from [dbo].[WO_Resource] dr 
                          left join WO_WOOperation oo on dr.ID = oo.ID  where (dr.PID = o.ID) AND   (dr.Type = 2) AND (dr.ResourceType = 6)) as recordCount  
                          from WO_WOOperation o left join WO_Resource r on  o.PID = r.ID LEFT OUTER JOIN
                          [Sys_User] AS us ON o.UserID = us.UserID 
						  LEFT OUTER JOIN WO_TeamUser dp on us.userid = dp.userid
	                      LEFT OUTER JOIN WO_TeamInfo dpt on dp.TeamID = dpt.TeamID
                          where o.PID=@WOID   ";
            if (oType != 1)
            {
                sql += " and o.Type=@oType ";
            }
            //sql += " and o.Type=@oType ";
            sql += " order by OperationTime asc";
            var query = this.Context.Database.SqlQuery_Dic(sql, sp);
            return query.ToList();
        }
        /// <summary>
        /// 获取工单操作历史
        /// </summary>
        /// <param name="WOID"></param>
        /// <returns></returns>
        public List<dynamic> GetWO_OpInfo(long WOID)
        {
            SqlParameter[] sp = new SqlParameter[] {
              new SqlParameter("@WOID",WOID),
            };
            var sql = @" select o.ID, o.UserID, o.OperationTime, o.Description,us.NickName as Account,o.PID, dpt.TeamName,o.Type,o.State,  r.Path, r.ResourceType,
                         (SELECT   COUNT(*) AS Expr1 FROM   WO_Resource dr left join WO_WOOperation oo on dr.ID = oo.ID
                          WHERE (dr.PID = o.ID) AND  (dr.Type = 1) AND (dr.ResourceType = 6)) AS ImgCount,
                         (select count(*) from [dbo].[WO_Resource] dr 
                          left join WO_WOOperation oo on dr.ID = oo.ID  where (dr.PID = o.ID) AND   (dr.Type = 2) AND (dr.ResourceType = 6)) as recordCount  
                          from WO_WOOperation o left join WO_Resource r on  o.PID = r.ID LEFT OUTER JOIN
                          [Sys_User] AS us ON o.UserID = us.UserID 
						  LEFT OUTER JOIN WO_TeamUser dp on us.userid = dp.userid
	                      LEFT OUTER JOIN WO_TeamInfo dpt on dp.TeamID = dpt.TeamID
                          where o.Type > 0 and o.Type < 5 and  o.PID=@WOID   ";
            sql += " order by OperationTime asc";
            var query = this.Context.Database.SqlQuery_Dic(sql, sp);
            return query.ToList();
        }

        /// <summary>
        /// 获取工单延期信息
        /// </summary>
        /// <param name="woid"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> LoadExtensionData(long woid)
        {
            var allList = from d in Query<WoWoextension>(s => s.Woid == woid)
                          join m in Query<SysUser>(s => true) on d.UserId equals m.UserId into dm
                          from dmt in dm
                          join u in Query<SysUser>(u => true) on d.Auditor equals u.UserId into su
                          from sut in su.DefaultIfEmpty()
                          select new
                          {
                              WOID = d.Woid,
                              //Exuser = dmt.Account,
                              Exuser = dmt.NickName,
                              ExtensionTime = d.ExtensionTime,
                              CompleteTime = d.CompleteTime,
                              //Auditor = sut.Account,
                              Auditor = sut.NickName,
                              AuditingTime = d.AuditingTime,
                              State = d.State == 1 ? "审核通过" : "审核未通过",
                              Reason = d.Reason
                          };
            return allList;
        }

        /// <summary>
        /// 任务派发时获取班组信息
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TeamUser> GetTeamInfo()
        {
            SqlParameter[] sqlparameter = new SqlParameter[] {
            };
            string sql = @"select tu.*,t.TeamName,u.Account,u.NickName from WO_TeamUser tu left join WO_TeamInfo t
                            on tu.TeamID=t.TeamID left join Sys_User u on tu.UserID=u.UserID";

            var query = this.Context.Database.SqlQuery<TeamUser>(sql, sqlparameter);
            return query;
        }
        //派工单
        public int AddDispatchWo(WoWorkOrder newWo, WoWooperation wo, WoForward woForward)
        {
            try
            {
                //添加工单历史表
                this.Context.Add(wo);
                //更新工单数据
                this.Context.Update(newWo);
                if (woForward != null)
                {
                    //更新转发数据信息
                    this.Context.Update(woForward);
                }
                this.Context.SaveChanges();
            }
            catch (Exception ex)
            {
                return -1;
            }
            return 1;
        }
        /// <summary>
        /// 退单
        /// </summary>
        /// <param name="woWorkOrder">工单信息</param>
        /// <param name="woWooperation">工单操作</param>
        /// <returns></returns>
        public int ChargebackWO(WoWorkOrder woWorkOrder, WoWooperation woWooperation)
        {
            try
            {
                this.Context.Update(woWorkOrder);
                this.Context.Add(woWooperation);
                this.Context.SaveChanges();
            }
            catch (Exception ex)
            {
                return -1;
            }
            return 1;

        }
        /// <summary>
        /// 退单申请
        /// </summary>
        /// <param name="woWorkOrder">工单信息</param>
        /// <param name="woWooperation">工单操作</param>
        ///  <param name="woForward">退单申请</param>
        /// <returns></returns>
        public int ChargebackWOF(WoWorkOrder woWorkOrder, WoWooperation woWooperation, WoForward woForward)
        {
            try
            {
                this.Context.Update(woWorkOrder);
                this.Context.Add(woWooperation);
                if (woForward != null)
                {
                    this.Context.Update(woForward);
                }
                this.Context.SaveChanges();
            }
            catch (Exception ex)
            {
                return -1;
            }
            return 1;

        }
        /// <summary>
        /// 审核工单
        /// </summary>
        /// <param name="woWorkOrder">工单信息</param>
        /// <param name="woWooperation">工单操作</param>
        /// <returns></returns>
        public int ReviewWO(WoWorkOrder woWorkOrder, WoWooperation woWooperation)
        {
            try
            {
                this.Context.Update(woWorkOrder);
                this.Context.Add(woWooperation);
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
            var query = this.Context.Database.SqlQuery_Dic("exec QueryWO_WOExtension @startindex,@pageSize,@orderItems,@filterString,@Totalcount Output", sp).ToList();
            Totalcount = Convert.ToInt32(sp[4].Value);
            return query;
        }
        //审核延期申请
        public int EditWoExtension(WoWoextension woWoextension, WoWorkOrder woWorkOrder, WoWooperation woWooperation)
        {
            try
            {
                this.Context.Update(woWoextension);
                this.Context.Update(woWorkOrder);
                this.Context.Add(woWooperation);
                this.Context.SaveChanges();
            }
            catch (Exception ex)
            {
                return -1;
            }
            return 1;
        }
        //审核延期申请未通过
        public int EditNoWoExtension(WoWoextension woWoextension, WoWooperation woWooperation)
        {
            try
            {
                this.Context.Update(woWoextension);
                this.Context.Add(woWooperation);
                this.Context.SaveChanges();
            }
            catch (Exception ex)
            {
                return -1;
            }
            return 1;
        }

        //工单转发
        public IEnumerable<dynamic> GetWoForwardTable(int pageindex, int pagesize, string orderby, string filter, ref int Totalcount)
        {
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@startindex",pageindex),
                new SqlParameter("@pageSize",pagesize),
                new SqlParameter("@orderItems",orderby),
                new SqlParameter("@filterString",filter),
                new SqlParameter("@Totalcount",Totalcount){Direction=System.Data.ParameterDirection.Output,DbType=System.Data.DbType.Int32}
            };
            var query = this.Context.Database.SqlQuery_Dic("exec QueryWO_Forward @startindex,@pageSize,@orderItems,@filterString,@Totalcount Output", sp).ToList();
            Totalcount = Convert.ToInt32(sp[4].Value);
            return query;
        }
        /// <summary>
        /// 获取设备负责人信息
        /// </summary>
        /// <param name="rtuid">设备ID</param>
        /// <returns></returns>
        public IEnumerable<TeamUser> LoadUserInfoByRtuid(long rtuid)
        {
            var info = from wtu in Query<WoTeamUser>(r => true)
                       join sus in Query<SwsUserStation>(r => true) on wtu.UserId equals sus.UserId into susw
                       from sw in susw
                       join rtu in Query<SwsRtuinfo>(r => r.Rtuid == rtuid) on sw.StationId equals rtu.StationId into rts
                       from rs in rts.DefaultIfEmpty()
                       select new TeamUser
                       {
                           UserID = sw.UserId,
                           TeamID = wtu.TeamId,
                           Account = sw.StationId.ToString()
                       };
            return info;
        }
        /// <summary>
        /// 插入事件和工单信息
        /// </summary>
        /// <param name="woWorkOrder">工单信息</param>
        /// <param name="woEvents">事件信息</param>
        /// <returns></returns>
        public int InsertWoandEvent(WoWorkOrder woWorkOrder, WoEvents woEvents)
        {
            try
            {
                this.Context.Add(woWorkOrder);
                this.Context.Add(woEvents);
                this.Context.SaveChanges();
            }
            catch (Exception ex)
            {
                return -1;
            }
            return 1;
        }
        /// <summary>
        /// 工单退单
        /// </summary>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <param name="orderby"></param>
        /// <param name="filter"></param>
        /// <param name="Totalcount"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> GetWoTrunTable(int pageindex, int pagesize, string orderby, string filter, ref int Totalcount)
        {
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@startindex",pageindex),
                new SqlParameter("@pageSize",pagesize),
                new SqlParameter("@orderItems",orderby),
                new SqlParameter("@filterString",filter),
                new SqlParameter("@Totalcount",Totalcount){Direction=System.Data.ParameterDirection.Output,DbType=System.Data.DbType.Int32}
            };
            var query = this.Context.Database.SqlQuery_Dic("exec QueryWO_WoTurn @startindex,@pageSize,@orderItems,@filterString,@Totalcount Output", sp).ToList();
            Totalcount = Convert.ToInt32(sp[4].Value);
            return query;
        }
        /// <summary>
        /// 审核转发申请通过
        /// </summary>
        /// <param name="woWorkOrder"></param>
        /// <param name="woWooperation"></param>
        /// <param name="woForward"></param>
        /// <returns></returns>
        public int EditWoForward(WoWorkOrder woWorkOrder, WoWooperation woWooperation, WoForward woForward, WoWorkOrder woWorkOrder1, WoWooperation woWooperation1)
        {
            try
            {
                this.Context.Update(woWorkOrder);
                this.Context.Add(woWooperation);
                this.Context.Update(woForward);
                this.Context.Add(woWorkOrder1);
                this.Context.Add(woWooperation1);
                this.Context.SaveChanges();
            }
            catch (Exception ex)
            {
                return -1;
            }
            return 1;
        }
        /// <summary>
        /// 手动派发工单
        /// </summary>
        /// <param name="woEvents">事件信息</param>
        /// <param name="woWorkOrder">工单信息</param>
        /// <param name="woWooperation">工单操作信息</param>
        /// <returns></returns>
        public int AddManualDispatch(WoEvents woEvents, WoWorkOrder woWorkOrder, WoWooperation woWooperation)
        {
            try
            {
                this.Context.Add(woEvents);
                this.Context.Add(woWorkOrder);
                this.Context.Add(woWooperation);
                this.Context.SaveChanges();
            }
            catch (Exception ex)
            {
                return -1;
            }
            return 1;

        }

        /// <summary>
        /// 网站退单操作
        /// </summary>
        /// <param name="woWorkOrder">工单信息</param>
        /// <param name="woWooperation">工单操作</param>
        /// <param name="woForward">退单申请</param>
        /// <param name="woWoextension">延期申请</param>
        /// <returns></returns>
        public int ChargebackWOW(WoWorkOrder woWorkOrder, WoWooperation woWooperation, WoForward woForward, WoWoextension woWoextension)
        {
            try
            {
                this.Context.Update(woWorkOrder);
                this.Context.Add(woWooperation);
                if (woForward != null)
                {
                    this.Context.Update(woForward);
                }
                if (woWoextension != null)
                {
                    this.Context.Update(woWoextension);
                }
                this.Context.SaveChanges();
            }
            catch (Exception ex)
            {
                return -1;
            }
            return 1;

        }
        #region 新工单接口
        /// <summary>
        /// 根据用户id获取用户处理工单
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> LoadWoListByUserID(long userId)
        {

            SqlParameter[] sqlparameter = new SqlParameter[] {
                  new SqlParameter("@userId",userId)
            };
            string sql = @"select  distinct wo.WOID,wo.ReleaseTime, wo.Num ,wo.HandleLevel,wo.Degree,wo.CompleteTime,we.IncidentContent,su.NickName,we.ReportTime,we.Address,sd.DeviceNum,
                           case when we.EquipmentID is not null then  ((select DeviceName from Sws_DeviceInfo01 where DeviceID = we.EquipmentID)  ) end  as DeviceName,
                           case when we.StationID is not null then  ((select Convert(nvarchar(20),Lng)  +','+Convert(nvarchar(20),Lat)  from Sws_Station where StationID = we.StationID)  ) end  as point,
                           wo.CurrentState ,
						   (select top(1) State from WO_WOExtension where WOID = wo.WOID order by ExtensionTime desc ) as ExtensionState,
                           (select top(1) State from WO_Forward wis where WOID = wo.WOID and wis.Type=2 order by ExtensionTime desc ) as ForwardState,
                           (select top(1) State from WO_Forward wis where WOID = wo.WOID and wis.Type=1 order by ExtensionTime desc ) as TurnState,
                           (select top(1) Reason from WO_WOExtension where WOID = wo.WOID order by ExtensionTime desc ) as ExtensionContent,
                           (select top(1) Remake from WO_Forward wis where WOID = wo.WOID and wis.Type=2 order by ExtensionTime desc ) as ForwardContent,
                           (select top(1) Remake from WO_Forward wis where WOID = wo.WOID and wis.Type=1 order by ExtensionTime desc ) as TurnContent
                           from WO_WorkOrder wo
                           left join WO_Events we on wo.EventID = we.IncidentID
                           left join Sys_User su on we.ReportUser =  su.UserID
                           left join Sws_DeviceInfo01 sd on we.EquipmentID = sd.DeviceID 
                           where wo.CurrentState < 5  and wo.ReceiveUser =@userId";
            //var query = this.Context.Database.SqlQuery<Api_SinWoEvent>(sql, sqlparameter);
            var query = this.Context.Database.SqlQuery_Dic(sql, sqlparameter).ToList();
            return query;
            //var allList = from d in Query<WoWorkOrder>(s => s.ReceiveUser == userId)
            //              join m in Query<WoEvents>(s => true) on d.EventId equals m.IncidentId into dm
            //              from dmt in dm
            //              where d.CurrentState < 4
            //              select new
            //              {
            //                  d.Woid,
            //                  d.ReleaseTime,
            //                  d.CompleteTime,
            //                  dmt.IncidentContent,
            //                  Degree = d.Degree,
            //                  HandleLevel = d.HandleLevel,
            //                  dmt.IncidentId,
            //                  d.CurrentState
            //              };
            //return allList.ToList();

        }
        /// <summary>
        /// 获取单个工单信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IEnumerable<Api_WoEvent> LoadWoById(long Id)
        {
            SqlParameter[] sqlparameter = new SqlParameter[] {
                 new SqlParameter("@Id",Id)
            };
            string sql = @"select  distinct wo.WOID ,wo.Num ,wo.ReleaseTime,wo.HandleLevel,wo.Degree,wo.CompleteTime,we.IncidentType,we.IncidentContent,we.Description,wo.EventID,su.NickName,we.ReportTime,we.Address,sd.DeviceNum,ss.StationName,ss.StationID,sd.DeviceID,sus.NickName as ReceiveUser,
                           (select top(1) Type  from WO_WOOperation wwo where wwo.PID = wo.WOID order by OperationTime desc ) AS StepState,
                            (select ItemName from   Sys_DataItemDetail where F_ItemId =5 AND ItemValue = (select DeviceType from Sws_DeviceInfo01 where DeviceID = we.EquipmentID)) as DeviceType,
                           case when we.EquipmentID is not null then  ((select DeviceName from Sws_DeviceInfo01 where DeviceID = we.EquipmentID)  ) end  as DeviceName,
                           case when we.StationID is not null then  ((select Convert(nvarchar(20),Lng)  +','+Convert(nvarchar(20),Lat)  from Sws_Station where StationID = we.StationID)  ) end  as point,
                           wo.CurrentState  ,
						   (select count(ID) from WO_WOExtension where wo.WOID = WOID) AS ExtensionCount,
						   (select count(ID) from WO_Forward wis where WOID = wo.WOID and wis.Type=2   ) as ForwardCount,
                           (select count(ID) from WO_Forward wis where WOID = wo.WOID and wis.Type=1  ) as TurnCount,
						   (select top(1) State from WO_WOExtension where WOID = wo.WOID order by ExtensionTime desc ) as ExtensionState,
                           (select top(1) State from WO_Forward wis where WOID = wo.WOID and wis.Type=2 order by ExtensionTime desc ) as ForwardState,
                           (select top(1) State from WO_Forward wis where WOID = wo.WOID and wis.Type=1 order by ExtensionTime desc ) as TurnState
                           from WO_WorkOrder wo
                           left join WO_Events we on wo.EventID = we.IncidentID
                           left join Sys_User su on we.ReportUser =  su.UserID
                           left join Sys_User sus on wo.ReceiveUser =  sus.UserID
                           left join Sws_DeviceInfo01 sd on we.EquipmentID = sd.DeviceID  
                           left join Sws_Station ss on sd.StationID = ss.StationID
                           where wo.WOID = @Id";
            var query = this.Context.Database.SqlQuery<Api_WoEvent>(sql, sqlparameter);
            return query;

        }
        /// <summary>
        /// 添加工单操作信息
        /// </summary>
        /// <param name="woWooperation"></param>
        /// <param name="woWorkOrder"></param>
        /// <param name="woEvents"></param>
        /// <returns></returns>
        public int AddWoOpInfo(WoWooperation woWooperation, WoWorkOrder woWorkOrder, WoEvents woEvents)
        {
            try
            {
                this.Context.Add(woWooperation);
                this.Context.Update(woWorkOrder);
                if (woEvents.IncidentId != 0)
                {
                    this.Context.Update(woEvents);
                }
                this.Context.SaveChanges();
            }
            catch (Exception ex)
            {
                return -1;
            }
            return 1;
        }
        /// <summary>
        /// 获取工单转发次数
        /// </summary>
        /// <param name="ID">工单ID</param>
        /// <returns></returns>
        public IEnumerable<dynamic> GetWOForwardCount(long ID)
        {
            SqlParameter[] sqlparameter = new SqlParameter[] {
                new SqlParameter("@WOID",ID),
            };
            string sql = @"with cte as
                      (select PID from WO_WorkOrder where WOID=@WOID
                      union all
                      select a.PID from WO_WorkOrder a join cte b on a.WOID=b.PID where a.PID <> 0) 
                      select * from cte";
            var query = this.Context.Database.SqlQuery<dynamic>(sql, sqlparameter);
            return query;
        }
        /// <summary>
        /// 转发工单接口
        /// </summary>
        /// <param name="newWo">新工单信息</param>
        /// <param name="oldWo">转发工单信息</param>
        /// <param name="Newwoop">新工单操作数据</param>
        /// <param name="oldwoop">转发工单操作数据</param>
        /// <returns></returns>
        public int AddWOForward(WoWorkOrder newWo, WoWorkOrder oldWo, WoWooperation Newwoop, WoWooperation oldwoop)
        {
            try
            {
                this.Context.Add(newWo);
                this.Context.Update(oldWo);
                this.Context.Add(Newwoop);
                this.Context.Add(oldwoop);
                this.Context.SaveChanges();
            }
            catch (Exception ex)
            {
                return -1;
            }
            return 1;
        }
        /// <summary>
        /// 生成事件和工单接口
        /// </summary>
        /// <param name="woWorkOrder">工单信息</param>
        /// <param name="woEvents">事件信息</param>
        /// <param name="woResource">资源信息</param>
        /// <returns></returns>
        public int AddWOAndEvent(WoWorkOrder woWorkOrder, WoEvents woEvents, WoResource woResource)
        {
            try
            {
                this.Context.Add(woWorkOrder);
                this.Context.Add(woEvents);
                if (woResource.Id != 0)
                {
                    this.Context.Add(woResource);
                }
                this.Context.SaveChanges();
            }
            catch (Exception ex)
            {
                return -1;
            }
            return 1;
        }
        /// <summary>
        /// 生成事件和工单接口
        /// </summary>
        /// <param name="woWorkOrder">工单信息</param>
        /// <param name="woEvents">事件信息</param> 
        /// <returns></returns>
        public int AddWOAndEvent(WoWorkOrder woWorkOrder, WoEvents woEvents)
        {
            try
            {
                this.Context.Add(woWorkOrder);
                this.Context.Add(woEvents);
                this.Context.SaveChanges();
            }
            catch (Exception ex)
            {
                return -1;
            }
            return 1;
        }
        /// <summary>
        /// 根据用户id获取用户历史工单
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="beginDate"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> LoadHisWoListByUserID(long userId, string beginDate)
        {
            SqlParameter[] sqlparameter = new SqlParameter[] {
                  new SqlParameter("@userId",userId),
                  new SqlParameter("@beginDate",beginDate)
            };
            string sql = @"select  distinct wo.WOID,wo.ReleaseTime, wo.Num ,wo.HandleLevel,wo.Degree,wo.CompleteTime,we.IncidentContent,su.NickName,we.ReportTime,we.Address,sd.DeviceNum,
                           case when we.EquipmentID is not null then  ((select DeviceName from Sws_DeviceInfo01 where DeviceID = we.EquipmentID)  ) end  as DeviceName,
                           case when we.StationID is not null then  ((select Convert(nvarchar(20),Lng)  +','+Convert(nvarchar(20),Lat)  from Sws_Station where StationID = we.StationID)  ) end  as point,
                           wo.CurrentState ,
						   (select top(1) State from WO_WOExtension where WOID = wo.WOID order by ExtensionTime desc ) as ExtensionState,
                           (select top(1) State from WO_Forward wis where WOID = wo.WOID and wis.Type=2 order by ExtensionTime desc ) as ForwardState,
                           (select top(1) State from WO_Forward wis where WOID = wo.WOID and wis.Type=1 order by ExtensionTime desc ) as TurnState
                           from WO_WorkOrder wo
                           left join WO_Events we on wo.EventID = we.IncidentID
                           left join Sys_User su on we.ReportUser =  su.UserID
                           left join Sws_DeviceInfo01 sd on we.EquipmentID = sd.DeviceID 
                           where wo.CurrentState > 4  and wo.ReceiveUser =@userId  and wo.CompleteTime > @beginDate";
            //var query = this.Context.Database.SqlQuery<Api_SinWoEvent>(sql, sqlparameter);
            var query = this.Context.Database.SqlQuery_Dic(sql, sqlparameter).ToList();
            return query;
            //var allList = from d in Query<WoWorkOrder>(s => s.ReceiveUser == userId)
            //              join m in Query<WoEvents>(s => true) on d.EventId equals m.IncidentId into dm
            //              from dmt in dm
            //              where d.CurrentState > 4
            //              select new
            //              {
            //                  d.Woid,
            //                  d.ReleaseTime,
            //                  d.CompleteTime,
            //                  dmt.IncidentContent,
            //                  Degree = d.Degree,
            //                  HandleLevel = d.HandleLevel,
            //                  dmt.IncidentId
            //              };
            //return allList.ToList();

        }
        #endregion

    }
}
