

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WNMS.Utility;
using System.Linq;
using WNMS.Model.CustomizedClass;
using WNMS.Model.DataModels;
using WNMS.Utility.Jpush;
using Jiguang.JPush.Model;

namespace WNMS.Service
{
    public partial class Sws_EventInfoService : BaseService, IService.ISws_EventInfoService
    {
        public IEnumerable<AlarmDeviceInfo> LoadCurrentEventInfo()
        {
            var query = this.Context.Database.SqlQuery<AlarmDeviceInfo>("exec GetCurrentEventInfo");
            return query;
        }
        #region 报警监控
        public IEnumerable<dynamic> QueryEventInfo_JK(int pageindex, int pagesize, string filter, string countfilter, int userID, bool IsAdmin, int f_itemID, int f_itemIDStationType, ref int Totalcount, ref int AttendNum, ref int ValueAlarmNum, ref int CommiNum, ref int DataNum)
        {
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@IsAdmin",IsAdmin),
                new SqlParameter("@UserID",userID),
                new SqlParameter("@f_itemID",f_itemID),
                new SqlParameter("@f_itemIDStationType",f_itemIDStationType),
                new SqlParameter("@startindex",pageindex),
                new SqlParameter("@pageSize",pagesize),
                new SqlParameter("@filterString",filter),
                new SqlParameter("@countfilter",countfilter),
                new SqlParameter("@Totalcount",Totalcount){Direction=System.Data.ParameterDirection.Output,DbType=System.Data.DbType.Int32},
                new SqlParameter("@AttendNum",AttendNum){Direction=System.Data.ParameterDirection.Output,DbType=System.Data.DbType.Int32},
                new SqlParameter("@ValueAlarmNum",ValueAlarmNum){Direction=System.Data.ParameterDirection.Output,DbType=System.Data.DbType.Int32},
                new SqlParameter("@CommiNum",CommiNum){Direction=System.Data.ParameterDirection.Output,DbType=System.Data.DbType.Int32},
                new SqlParameter("@dataCount",DataNum){Direction=System.Data.ParameterDirection.Output,DbType=System.Data.DbType.Int32}

            };
            var query = this.Context.Database.SqlQuery_Dic(@"exec QueryEventJKInfo @IsAdmin,@UserID,@f_itemID,@f_itemIDStationType,
@startindex,@pageSize,@filterString,@countfilter,@Totalcount Output,@AttendNum Output,@ValueAlarmNum Output,@CommiNum Output,@dataCount Output", sp).ToList();

            Totalcount = Convert.ToInt32(sp[8].Value);
            AttendNum = Convert.ToInt32(sp[9].Value);
            ValueAlarmNum = Convert.ToInt32(sp[10].Value);
            CommiNum = Convert.ToInt32(sp[11].Value);
            DataNum = Convert.ToInt32(sp[12].Value);
            return query;

        }
        public IEnumerable<dynamic> QueryEventHandleInfo(DateTime eventtime, int rtuid, int eventsouce)
        {
            SqlParameter[] sp = new SqlParameter[] {

                new SqlParameter("@eventtime",eventtime),
                new SqlParameter("@rtuid",rtuid),
                new SqlParameter("@eventsouce",eventsouce)
            };
            string sql = @"select e.*,Account,NickName from [dbo].[Sws_EventHandle] e
  left join [dbo].[Sys_User] u on e.UserID=u.UserID where EventTime=@eventtime and RTUID=@rtuid and EventSource=@eventsouce";
            var query = this.Context.Database.SqlQuery_Dic(sql, sp);
            return query;
        }
        #endregion
        //工艺图获取实时报警信息
        public IEnumerable<dynamic> LoadEventList(string rtuids)
        {
            SqlParameter[] sp = new SqlParameter[] {

                new SqlParameter("@rtuids",rtuids),
            };
            string sql = @"select distinct e.* ,sd.ItemName,case when h.ID is null then 0 else 1 end IsHandle from Sws_EventInfo e
                           left join Sys_DataItemDetail sd on e.EventLevel = sd.ItemValue and sd.F_ItemId = 3
                           left join Sws_EventHandle h on e.Rtuid =h.RTUID AND e.EventSource = h.EventSource
                           where e.EventLevel <>0 and CHARINDEX(','+LTRIM(e.RTUID)+',',','+@rtuids+',')>0";


            var query = this.Context.Database.SqlQuery_Dic(sql, sp);
            return query;
        }
        /// <summary>
        /// 查询报警泵房信息
        /// </summary>
        /// <param name="rtuIDs">设备ID</param>
        /// <returns></returns>
        public IEnumerable<dynamic> GetPumData(List<int> rtuIDs)
        {
            var query = (from s in Context.Set<SwsStation>()
                         join st in Context.Set<SwsRtuinfo>() on s.StationId equals st.StationId into str
                         from stra in str.DefaultIfEmpty()
                         join r in Context.Set<SwsEventInfo>() on stra.Rtuid equals r.Rtuid into rs
                         from rsa in rs.DefaultIfEmpty()
                         where rtuIDs.Contains(rsa.Rtuid) && rsa.EventLevel != 0
                         select new
                         {
                             s.StationId,
                             s.StationName,
                             s.Lat,
                             s.Lng
                         }).ToList();
            return query;
        }
        /// <summary>
        /// 查询泵房报警
        /// </summary>
        /// <param name="rtuids">设备ID</param>
        /// <returns></returns>
        public IEnumerable<dynamic> LoadEventsInfo(string rtuids)
        {
            SqlParameter[] sp = new SqlParameter[] {

                new SqlParameter("@rtuids",rtuids),
            };
            string sql = @"select distinct sde.DeviceName, e.EventMessage,e.EventTime,e.EventLevel ,sd.ItemName,case when h.ID is null then 0 else 1 end IsHandle from Sws_EventInfo e
                           left join Sys_DataItemDetail sd on e.EventLevel = sd.ItemValue and sd.F_ItemId = 3
                           left join Sws_EventHandle h on e.Rtuid =h.RTUID AND e.EventSource = h.EventSource
						   left join Sws_RTUInfo sr on sr.RTUID = e.RTUID 
						   left join Sws_DeviceInfo01 sde on sde.RTUID = e.RTUID
                           where e.EventLevel <>0 and  CHARINDEX(','+LTRIM(e.RTUID)+',',','+@rtuids+',')>0";


            var query = this.Context.Database.SqlQuery_Dic(sql, sp);
            return query;
        }
        #region 报警推送
        //报警列表
        public IEnumerable<dynamic> GetAlarmList(int userid, bool isadmin, string filter, int pageindex, int pagesize, int itemid_partion, ref int TotalCount)
        {
            StringBuilder sb = new StringBuilder();
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@userid",userid),
                new SqlParameter("@filter",filter),
                new SqlParameter("@pageindex",pageindex),
                new SqlParameter("@pagesize",pagesize),
                new SqlParameter("@itemid_partion",itemid_partion)
            };
            string countsql = "";
            if (isadmin == true)
            {
                sb.Append(@"with base as(  select e.*,InType,StaitonType, s.StationName,s.StationID from [dbo].[Sws_EventInfo] e
inner join [dbo].[Sws_DeviceInfo01] f on e.RTUID=f.RTUID
 left join [dbo].[Sws_RTUInfo] r on e.RTUID=r.RTUID
  left join [dbo].[Sws_Station] s on r.StationID=s.StationID where " + @filter + "  and (IsConfirm=0 or IsConfirm is null) " +
 @" and State=1 and EventLevel!=0 and s.StationID is not null  ),");
            }
            else
            {
                sb.Append(@"with base as( select e.*,InType,StaitonType, s.StationName,s.StationID from [dbo].[Sws_EventInfo] e
 inner join [dbo].[Sws_DeviceInfo01] f on e.RTUID=f.RTUID
 left join [dbo].[Sws_RTUInfo] r on e.RTUID=r.RTUID
  left join [dbo].[Sws_Station] s on r.StationID=s.StationID
  left join [dbo].[Sws_UserStation] u on s.StationID=u.StationID
   where " + @filter + " and (IsConfirm=0 or IsConfirm is null) " +
  @"and State=1 and EventLevel!=0 and s.StationID is not null and u.UserID=@userid),");
            }
            countsql = sb.ToString().Substring(0, sb.ToString().Length - 1) + " select count(*) as Num from base";


            sb.Append(@"ta as(
  select Row_number() over(order by EventLevel asc, EventTime desc) as rownumber,* from base ),
  tp as(
  select * from ta where rownumber>=(@pageindex-1)*@pagesize and rownumber<=@pagesize*@pageindex),");

            sb.Append(@" t as(
   select tp.*,Unit,Region,m.ItemName from tp
  left join [dbo].[Sws_DataInfo] d on tp.EventSource=d.DataID and d.DeviceType=tp.InType
  left join (select * from [dbo].[Sys_DataItemDetail] where F_ItemId=@itemid_partion and IsEnable=1) m on d.Region=m.ItemValue),");
            sb.Append(@" r as(select * from t where Region!=0),");//非通讯报警
            #region 通讯报警
            sb.Append(@"h as(
  select * from t where Region=0),
  dd as(select DeviceID,Partition, StationID,RTUID,1 type from [dbo].[Sws_DeviceInfo01]
  union all( select DeviceID,Partition, StationID,RTUID,2 type from [dbo].[Sws_DeviceInfo02])
  ),
  d1 as(
  select dd.StationID,Partition from dd
  left join h on dd.type=h.InType and dd.StationID=h.StationID and dd.RTUID=h.RTUID
  where h.ID is not null),
  d2 as(
  select StationID,Partition from d1 group by StationID,Partition),
  d3 as(
  select d2.*,ItemName from d2
  left join (select * from [dbo].[Sys_DataItemDetail] where F_ItemId=@itemid_partion and IsEnable=1) as m on d2.Partition=m.ItemValue),
  d4 as(
  SELECT StationID, STUFF((SELECT  '#'+ItemName from  d3  where StationID=a.StationID  for xml path('')),1,1,'') as names from d3 as a group by 
  StationID),
   d as(
  select rownumber,ID, EventTime, RTUID, EventSource, EventMessage, EventType, EventLevel, State, CurrentValue, LimitValue, IsConfirm,InType,StaitonType,h.StationName,h.StationID, Unit,Region,names as ItemName from h 
  left join d4 on h.StationID=d4.StationID),");
            #endregion
            sb.Append(@" result as (
  select * from r union select * from d )
  select * from result order by rownumber");
            var query = this.Context.Database.SqlQuery_Dic(sb.ToString(), sp);
            TotalCount = this.Context.Database.SqlQuery<QueryIDNum>(countsql, sp)[0].Num;
            return query;

        }
        class QueryIDNum
        {
            public int Num { get; set; }

        }
        //报警标记为已读
        public int OperateReadAlarm(string eventid)
        {
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@eventid",eventid)
          };
            StringBuilder sb = new StringBuilder();
            sb.Append(@"  update [dbo].[Sws_EventInfo] set IsConfirm=1 where ID=@eventid");


            var sql = sb.ToString();
            var query = this.Context.Database.ExecuteSqlRaw(sb.ToString(), sp);
            return query;
        }
        //全部标记为已读
        public int OperateReadAlarm_All(int userid, bool isadmin)
        {
            StringBuilder sb = new StringBuilder();
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@userid",userid)

            };
            string sql = "";
            if (isadmin == true)
            {
                sb.Append(@"with base as(  select e.*,InType,StaitonType, s.StationName,s.StationID from [dbo].[Sws_EventInfo] e
 left join [dbo].[Sws_RTUInfo] r on e.RTUID=r.RTUID
  left join [dbo].[Sws_Station] s on r.StationID=s.StationID where  (IsConfirm=0 or IsConfirm is null) " +
 @" and State=1 and EventLevel!=0 and s.StationID is not null  )");
            }
            else
            {
                sb.Append(@"with base as( select e.*,InType,StaitonType, s.StationName,s.StationID from [dbo].[Sws_EventInfo] e
 left join [dbo].[Sws_RTUInfo] r on e.RTUID=r.RTUID
  left join [dbo].[Sws_Station] s on r.StationID=s.StationID
  left join [dbo].[Sws_UserStation] u on s.StationID=u.StationID
   where  (IsConfirm=0 or IsConfirm is null) " +
  @"and State=1 and EventLevel!=0 and s.StationID is not null and u.UserID=@userid)");
            }
            sql = sb.ToString() + "  update [dbo].[Sws_EventInfo] set IsConfirm=1 where ID IN (SELECT ID from base)";
            var query = this.Context.Database.ExecuteSqlRaw(sql, sp);
            return query;
        }
        //查询报警数量
        public int GetAlarmCount(int userid, bool isadmin)
        {
            StringBuilder sb = new StringBuilder();
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@userid",userid)

            };
            string countsql = "";
            if (isadmin == true)
            {
                sb.Append(@"with base as(  select e.*,InType,StaitonType, s.StationName,s.StationID from [dbo].[Sws_EventInfo] e
 inner join [dbo].[Sws_DeviceInfo01] f on e.RTUID=f.RTUID
 left join [dbo].[Sws_RTUInfo] r on e.RTUID=r.RTUID
  left join [dbo].[Sws_Station] s on r.StationID=s.StationID where  (IsConfirm=0 or IsConfirm is null) " +
 @" and State=1 and EventLevel!=0 and s.StationID is not null  )");
            }
            else
            {
                sb.Append(@"with base as( select e.*,InType,StaitonType, s.StationName,s.StationID from [dbo].[Sws_EventInfo] e
inner join [dbo].[Sws_DeviceInfo01] f on e.RTUID=f.RTUID
 left join [dbo].[Sws_RTUInfo] r on e.RTUID=r.RTUID
  left join [dbo].[Sws_Station] s on r.StationID=s.StationID
  left join [dbo].[Sws_UserStation] u on s.StationID=u.StationID
   where  (IsConfirm=0 or IsConfirm is null) " +
  @"and State=1 and EventLevel!=0 and s.StationID is not null and u.UserID=@userid)");
            }
            countsql = sb.ToString() + " select count(*) as Num from base";
            var TotalCount = this.Context.Database.SqlQuery<QueryIDNum>(countsql, sp)[0].Num;
            return TotalCount;

        }
        #endregion
        #region 泵房监控 报警查询
        public IEnumerable<dynamic> QueryEventByRtuID(string rtuid, int intype)
        {
            var f_itemid = (int)Model.CustomizedClass.Enum.设备分区;
            SqlParameter[] sp = new SqlParameter[] {
            new SqlParameter("@rtuid",rtuid),
            new SqlParameter("@intype",intype),
            new SqlParameter("@f_itemid",f_itemid)
            };

            string sql = @"with ta as(  
  select * from [dbo].[Sws_EventInfo] where CHARINDEX(','+LTRIM(RTUID)+',',',' + @rtuid + ',')>0  and EventLevel != 0 and State= 1)," +
 @" t as(
  select ta.*,Region,m.ItemName  as RegionName,DataType,Unit from ta
   left join [dbo].[Sws_DataInfo] d on ta.EventSource=d.DataID and d.DeviceType=@intype
  left join (select * from [dbo].[Sys_DataItemDetail] where F_ItemId=@f_itemid and IsEnable=1) m on d.Region=m.ItemValue),
  r as( select ID, EventTime, RTUID as Rtuid, EventSource, EventMessage, CurrentValue, LimitValue,RegionName,Unit,DataType,EventLevel from t where Region!=0)
  ,h as(
  select * from t where Region=0),
  d1 as(
  select dd.Partition,dd.RTUID,m.ItemName as RegionName from [dbo].[Sws_DeviceInfo0" + @intype + "] dd " +
  @"left join h on   dd.RTUID=h.RTUID
  left join (select * from [dbo].[Sys_DataItemDetail] where F_ItemId=@f_itemid and IsEnable=1) as m on dd.Partition=m.ItemValue
  where h.ID is not null),
  d2 as(
   SELECT RTUID, STUFF((SELECT  '#'+RegionName from  d1  where RTUID=a.RTUID  for xml path('')),1,1,'') as RegionName from d1 as a group by 
  RTUID),
  d as(
  select ID, EventTime, h.RTUID as Rtuid, EventSource, EventMessage, CurrentValue, LimitValue,d2.RegionName,Unit,DataType,EventLevel from h 
  left join d2 on h.RTUID=d2.RTUID)
  select * from r union select * from d";
            var query = this.Context.Database.SqlQuery_Dic(sql, sp);
            return query;
        }
        #endregion
        #region 报警监控  报警处理  自动生成工单
        public int HandlerEventInfo(int? evid, SwsEventHandle e, string alarmMessage, byte? eventLevel)
        {
            if (e.IsConvertOrder == true)//转工单处理
            {
                //GdRepair rr = new GdRepair();
                //var gdRepairMax = this.Context.Set<GdRepair>().Where(r => true);
                //if (gdRepairMax.Count() == 0)
                //{
                //    rr.RepairId = 1;
                //    rr.Num = "WX-" + DateTime.Now.Year + "-" + 1.ToString("0000000");
                //}
                //else
                //{

                //    rr.RepairId = gdRepairMax.OrderByDescending(r => r.RepairId).FirstOrDefault().RepairId + 1;

                //    rr.Num = "WX-" + DateTime.Now.Year + "-" + (rr.RepairId).ToString("0000000");
                //}
                ////rr.Num = "WX-" + DateTime.Now.ToString("yyyyMMddHHmmssSSS");
                //rr.StationId = (int)this.Context.Set<SwsRtuinfo>().Where(r => r.Rtuid == e.Rtuid).FirstOrDefault()?.StationId;
                //rr.RepairState = 0;
                //rr.RepairUser = 0;
                //rr.CreateTime = DateTime.Now;
                //rr.FaultContent = alarmMessage;

                //GdEvents ge = new GdEvents();
                //ge.IncidentId = ConvertDateTimeInt(DateTime.Now);
                //ge.IncidentNum = "WX" + DateTime.Now.ToString("yyyyMMddHHmmss");
                //ge.IncidentState = 0;
                //ge.IncidentType = 2;
                //ge.IncidentContent = alarmMessage;
                //ge.IncidentSource = 2;
                //ge.ReportTime = DateTime.Now;
                //ge.ReportUser = e.UserId;
                //ge.DisposeState = 1;
                //ge.AuditState = false;
                //ge.TaskId = rr.RepairId;

                //this.Context.Set<GdRepair>().Add(rr);
                //this.Context.Set<GdEvents>().Add(ge);

                //转工单  先生成维修事件，再生成工单
                Random random = new Random();
                //生成事件
                WoEvents we = new WoEvents();
                we.IncidentId = ConvertDateTimeInt(DateTime.Now);
                we.IncidentNum = "WX" + DateTime.Now.ToString("yyyyMMddHHmmss");
                we.IncidentState = 0;
                we.IncidentType = 2;
                we.IncidentContent = alarmMessage;
                we.IncidentSource = 2;
                we.ReportTime = DateTime.Now;
                we.ReportUser = e.UserId;
                we.DisposeState = 1;
                we.EquipmentId = e.Rtuid;
                we.StationId = Query<SwsDeviceInfo01>(r => r.Rtuid == e.Rtuid).FirstOrDefault()?.StationId;
                we.EventId = evid;

                WoWorkOrder woWorkOrder = new WoWorkOrder();
                woWorkOrder.Woid = ConvertDateTimeInt(DateTime.Now);
                woWorkOrder.CurrentState = (short)WoState.未分派;
                woWorkOrder.Num = "WO_WX_" + ConvertDateTimeInt(DateTime.Now) + random.Next(1, 100);
                woWorkOrder.EventId = we.IncidentId;
                woWorkOrder.CurrentState = -1;
                if (eventLevel == 1)
                {
                    //非常紧急
                    woWorkOrder.HandleLevel = (byte)ProcessingLevel.十二小时;
                    woWorkOrder.CompleteTime = DateTime.Now.AddHours(12);
                    woWorkOrder.Degree = (byte)EmergencyDegree.非常紧急;
                }
                else if (eventLevel == 2)
                {
                    //紧急
                    woWorkOrder.HandleLevel = (byte)ProcessingLevel.二十四小时;
                    woWorkOrder.CompleteTime = DateTime.Now.AddHours(24);
                    woWorkOrder.Degree = (byte)EmergencyDegree.紧急;
                }
                else
                {
                    //一般
                    woWorkOrder.HandleLevel = (byte)ProcessingLevel.两天;
                    woWorkOrder.CompleteTime = DateTime.Now.AddHours(48);
                    woWorkOrder.Degree = (byte)EmergencyDegree.一般;
                }
                woWorkOrder.IsAuditing = (byte)WOExtensionReview.未审核;
                woWorkOrder.AuditingContent = "未审核";
                woWorkOrder.ReleaseTime = DateTime.Now;
                woWorkOrder.Pid = 0;
                this.Context.Set<WoEvents>().Add(we);
                this.Context.Set<WoWorkOrder>().Add(woWorkOrder);
            }
            this.Context.Set<SwsEventHandle>().Add(e);
            return this.Context.SaveChanges();
        }
        public static long ConvertDateTimeInt(System.DateTime time)
        {
            System.DateTime startTime = System.TimeZoneInfo.ConvertTimeToUtc(new System.DateTime(1970, 1, 1));
            return (long)(time - startTime).TotalMilliseconds;
        }

        //自动生成工单
        public int CreatOrder(int evid, int rtuid, int userid, string message, byte eventLevel, ref long woid)
        {
            //转工单  先生成维修事件，再生成工单
            Random random = new Random();
            //生成事件
            WoEvents we = new WoEvents();
            we.IncidentId = ConvertDateTimeInt(DateTime.Now);
            we.IncidentNum = "WX" + DateTime.Now.ToString("yyyyMMddHHmmss");
            we.IncidentState = 0;
            we.IncidentType = 2;
            we.IncidentContent = message;
            we.IncidentSource = 2;
            we.ReportTime = DateTime.Now;
            we.ReportUser = userid;
            we.DisposeState = 1;
            we.EquipmentId = rtuid;
            we.StationId = Query<SwsDeviceInfo01>(r => r.Rtuid == rtuid).FirstOrDefault()?.StationId;
            we.EventId = evid;

            WoWorkOrder woWorkOrder = new WoWorkOrder();
            woWorkOrder.Woid = ConvertDateTimeInt(DateTime.Now);
            woWorkOrder.CurrentState = (short)WoState.未分派;
            woWorkOrder.Num = "WO_WX_" + ConvertDateTimeInt(DateTime.Now) + random.Next(1, 100);
            woWorkOrder.EventId = we.IncidentId;
            woWorkOrder.CurrentState = -1;
            if (eventLevel == 1)
            {
                //非常紧急
                woWorkOrder.HandleLevel = (byte)ProcessingLevel.十二小时;
                woWorkOrder.CompleteTime = DateTime.Now.AddHours(12);
                woWorkOrder.Degree = (byte)EmergencyDegree.非常紧急;
            }
            else if (eventLevel == 2)
            {
                //紧急
                woWorkOrder.HandleLevel = (byte)ProcessingLevel.二十四小时;
                woWorkOrder.CompleteTime = DateTime.Now.AddHours(24);
                woWorkOrder.Degree = (byte)EmergencyDegree.紧急;
            }
            else
            {
                //一般
                woWorkOrder.HandleLevel = (byte)ProcessingLevel.两天;
                woWorkOrder.CompleteTime = DateTime.Now.AddHours(48);
                woWorkOrder.Degree = (byte)EmergencyDegree.一般;
            }
            woWorkOrder.IsAuditing = (byte)WOExtensionReview.未审核;
            woWorkOrder.AuditingContent = "未审核";
            woWorkOrder.ReleaseTime = DateTime.Now;
            woWorkOrder.Pid = 0;
            woid = woWorkOrder.Woid;
            this.Context.Set<WoEvents>().Add(we);
            this.Context.Set<WoWorkOrder>().Add(woWorkOrder);
            return this.Context.SaveChanges();
        }

        //极光推送
        public void PushTask(List<TaskPush> ts)
        {
            Jpush jpush = new Jpush();
            try
            {
                foreach (var item in ts)
                {
                    PushPayload pushPayload = new PushPayload()
                    {
                        Platform = new List<string> { "android", "ios" },
                        Audience = new Audience
                        {
                            Alias = new List<string> { item.UserID.ToString() }
                        },
                        Message = new Message
                        {
                            Title = item.PlanID.ToString(),
                            Content = item.Content,
                        }
                    };
                    jpush.ExcutePush(pushPayload);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region 删除报警信息
        /// <summary>
        /// 删除报警
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <returns></returns>
        public int DeleteEvents(int eventId, DateTime eventTime, short eventSource, int rtuId)
        {
            SwsEventInfo sev = this.Query<SwsEventInfo>(r => r.Id == eventId).FirstOrDefault();
            if (sev == null)
            {
                throw new Exception("报警不存在");
            }
            else
            {
                //删除历史表               
                List<SwsEventHistory> sehistory = this.Query<SwsEventHistory>(r => r.EventTime == eventTime && r.EventSource == eventSource && r.Rtuid == rtuId).ToList();
                if (sehistory.Count() > 0)
                {
                    foreach (var se in sehistory)
                    {
                        this.Context.Attach(se);
                    }
                    this.Context.RemoveRange(sehistory);
                }

                //删除报警处理表
                List<SwsEventHandle> ehandle = this.Query<SwsEventHandle>(r => r.EventTime == eventTime && r.EventSource == eventSource && r.Rtuid == rtuId).ToList();
                if (ehandle.Count() > 0)
                {
                    foreach (var eh in ehandle)
                    {
                        this.Context.Attach(eh);
                    }
                    this.Context.RemoveRange(ehandle);
                }

                //删除事件表  工单表
                WoEvents wevent = this.Query<WoEvents>(m => m.EventId == eventId).FirstOrDefault();
                if (wevent != null)
                {
                    WoWorkOrder worder = this.Query<WoWorkOrder>(m => m.EventId == wevent.IncidentId).FirstOrDefault();
                    if (worder != null)
                    {
                        this.Context.Attach(worder);
                        this.Context.Remove(worder);
                    }
                    this.Context.Attach(wevent);
                    this.Context.Remove(wevent);
                }

                //删除实时报警
                this.Context.Attach(sev);
                this.Context.Remove(sev);
                return this.Context.SaveChanges();
            }
        }
        #endregion
    }
}
