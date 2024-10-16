using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WNMS.Utility;
using System.Linq;
using WNMS.Model.DataModels;
using WNMS.Model.CustomizedClass;
using System.IO;
using System.Data;

namespace WNMS.Service
{
    public partial class Sws_StationService : BaseService, IService.ISws_StationService
    {
        public IEnumerable<dynamic> QueryStationTable(bool IsAdmin, int pageindex, int pagesize, int F_itemid, int f_StationInstall, string order, string filter, ref int Totalcount)
        {
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@IsAdmin",IsAdmin),
                new SqlParameter("@f_StationInstall",f_StationInstall),
                new SqlParameter("@f_itemID",F_itemid),
                new SqlParameter("@startindex",pageindex),
                new SqlParameter("@pageSize",pagesize),
                new SqlParameter("@orderItems",order),
                new SqlParameter("@filterString",filter),
                new SqlParameter("@Totalcount",Totalcount){Direction=System.Data.ParameterDirection.Output,DbType=System.Data.DbType.Int32}
            };
            var query = this.Context.Database.SqlQuery_Dic("exec QueryStationTable @IsAdmin,@f_StationInstall,@f_itemID,@startindex,@pageSize,@orderItems,@filterString,@Totalcount Output", sp).ToList();

            Totalcount = Convert.ToInt32(sp[7].Value);
            return query;
        }


        public IEnumerable<dynamic> LoadStationInfo(int StationID)
        {
            SqlParameter[] sp = new SqlParameter[]
             {
                new SqlParameter("@StationID",StationID)
             };

            var query = this.Context.Database.SqlQuery_Dic("exec GetStationInfo @StationID", sp);
            return query;
        }


        public int AddStation(SwsStation ss, List<Attachment> attachlist, bool isadmin, int userid)
        {
            if (isadmin != true)//非管理员,添加userstation中间表
            {
                SwsUserStation us = new SwsUserStation();
                us.StationId = ss.StationId;
                us.UserId = userid;
                us.FocusOn = false;

                this.Context.Set<SwsUserStation>().Add(us);

            }
            this.Context.Set<SwsStation>().Add(ss);
            if (attachlist.Count > 0)
            {
                this.Context.Set<Attachment>().AddRange(attachlist);
            }
            return this.Context.SaveChanges();
        }

        //批量导入
        public int AddStationList(List<SwsStation> slist, bool isadmin, int userid)
        {
            if (isadmin == true)//管理员
            {
                this.Context.Set<SwsStation>().AddRange(slist);
            }
            else
            {
                List<SwsUserStation> userStations = new List<SwsUserStation>() { };
                foreach (var item in slist)
                {
                    SwsUserStation u = new SwsUserStation();
                    u.StationId = item.StationId;
                    u.UserId = userid;
                    u.FocusOn = false;
                    userStations.Add(u);
                }
                this.Context.Set<SwsStation>().AddRange(slist);
                this.Context.Set<SwsUserStation>().AddRange(userStations);
            }
            return this.Context.SaveChanges();
        }
        public int EditeStation(SwsStation ss, List<Attachment> attachlist, int classify)
        {
            ////查找原有的附件
            //IEnumerable<Attachment> oldAttachList = this.Context.Set<Attachment>().Where(r => r.Affiliation == ss.StationId && r.Classify == classify);
            //this.Context.Set<Attachment>().RemoveRange(oldAttachList);//删除原有附件
            this.Context.Set<SwsStation>().Update(ss);
            if (attachlist.Count > 0)
            {
                this.Context.Set<Attachment>().AddRange(attachlist);
            }
            return this.Context.SaveChanges();
        }
        public int DeleteStation(string RootPath, List<int> stationIDs, int classify, ref string useStationName, ref string deleteStationName)
        {
            //查询被占用的泵房id
            //var useStationIDs = this.Context.Set<SwsRtuinfo>().Where(r => stationIDs.Contains(r.StationId)).Select(r => r.StationId).ToList();
            var useStationIDs = new List<int>() { };
            var useStationID1s = this.Context.Set<SwsDeviceInfo01>().Where(r => stationIDs.Contains(r.StationId)).Select(r => r.StationId).ToList();
            var useStaionid2 = this.Context.Set<SwsDeviceInfo02>().Where(r => stationIDs.Contains(r.StationId)).Select(r => r.StationId).ToList();
            useStationIDs = useStationIDs.Concat(useStationID1s).Concat(useStaionid2).Distinct().ToList();
            //查询被资产信息表占用的泵房id
            var propertSID = this.Context.Set<SwsPropertyInfo>().Where(r => stationIDs.Contains(r.StationId)).Select(r => r.StationId).ToList();
            useStationIDs = useStationIDs.Concat(propertSID).Distinct().ToList();//SwsRtuinfo和SwsPropertyInfo占用的泵房id并集
            var unUseStationIDs = stationIDs;
            if (useStationIDs.Count > 0)//被占用的泵房
            {
                unUseStationIDs = stationIDs.Except(useStationIDs).ToList();
                var useStations = this.Context.Set<SwsStation>().Where(r => useStationIDs.Contains(r.StationId));
                foreach (var item in useStations)
                {
                    if (propertSID.Contains(item.StationId))
                    {
                        useStationName += item.StationName + "被资产占用,";
                    }
                    else
                    {
                        useStationName += item.StationName + "被设备占用,";
                    }
                }
                useStationName = useStationName.Substring(0, useStationName.Length - 1);
            }
            if (unUseStationIDs.Count > 0)//未被占用的泵房
            {
                //相关附件
                var attachList = this.Context.Set<Attachment>().Where(r => unUseStationIDs.Contains(r.Affiliation) && r.Classify == classify);

                var stationList = this.Context.Set<SwsStation>().Where(r => unUseStationIDs.Contains(r.StationId));
                this.Context.Set<Attachment>().RemoveRange(attachList);//删除原有附件
                this.Context.Set<SwsStation>().RemoveRange(stationList);//删除泵房 
                foreach(var i in stationList)
                {
                    deleteStationName += i.StationName + ",";  
                }
                deleteStationName = deleteStationName.Substring(0, deleteStationName.Length - 1);
                //门禁表中的stationid清0
                var doorlist = this.Context.Set<SwsAccessControl>().Where(r => unUseStationIDs.Contains(r.StationId));
                foreach (var item in doorlist)
                {
                    item.StationId = 0;
                    this.Context.Set<SwsAccessControl>().Update(item);
                }
                //视频表中的stationid清0
                var cameralist = this.Context.Set<SwsCamera>().Where(r => unUseStationIDs.Contains(r.StationId));
                foreach (var item in cameralist)
                {
                    item.StationId = 0;
                    this.Context.Set<SwsCamera>().Update(item);

                }

                //删除stationUser中间表
                var stationusers = this.Context.Set<SwsUserStation>().Where(r => unUseStationIDs.Contains(r.StationId));
                this.Context.Set<SwsUserStation>().RemoveRange(stationusers);

                #region 泵房下的上传的附件本地删除
                foreach (var item in stationList)
                {
                    string localPath = Path.Combine(RootPath, "UploadFile\\Sws_Station\\" + item.StationId + "");
                    //删除文件
                    if (Directory.Exists(localPath))
                    {
                        Directory.Delete(localPath, true);
                    }
                }
                #endregion

                #region 删除工单相关数据
                //相关任务，维修，保养，巡检，并根据任务查找相应的事件
                #region 维修
                var repairTask = this.Context.Set<GdRepair>().Where(r => unUseStationIDs.Contains(r.StationId));
                IEnumerable<GdEvents> gd_events = new List<GdEvents>() { };
                IEnumerable<GdResource> gd_resource = new List<GdResource>() { };
                if (repairTask.Count() > 0)
                {
                    var repairTaskID = repairTask.Select(r => r.RepairId);
                    var repaireIDS= repairTaskID.ToList().ConvertAll(r=>Convert.ToInt64(r));
                   var gd_events1 = this.Context.Set<GdEvents>().Where(r=>r.IncidentType==2 && repairTaskID.Contains(r.TaskId));
                   var gd_resource1 = this.Context.Set<GdResource>().Where(r =>( r.ResourceType == 3 || r.ResourceType==1) && repaireIDS.Contains(r.Pid));
                    gd_events = gd_events.Union(gd_events1);
                    gd_resource = gd_resource.Union(gd_resource1);
                }
                #endregion
                #region 保养
                var maintainTask = this.Context.Set<GdMaintain>().Where(r=>unUseStationIDs.Contains(r.StationId));
                if (maintainTask.Count() > 0)
                {
                    var mainTaskID = maintainTask.Select(r => r.MaintainId);
                    var event_maintain = this.Context.Set<GdEvents>().Where(r => r.IncidentType == 1 && mainTaskID.Contains(r.TaskId));
                    gd_events = gd_events.Union(event_maintain);

                    var mainIDS = mainTaskID.ToList().ConvertAll(r => Convert.ToInt64(r));
                    var resouce_maintain = this.Context.Set<GdResource>().Where(r => r.ResourceType == 4 && mainIDS.Contains(r.Pid));
                    gd_resource = gd_resource.Union(resouce_maintain);
                }
                #endregion
                #region 巡检
                var inspectTask = this.Context.Set<GdInspection>().Where(r => unUseStationIDs.Contains(r.StationId));
                if (inspectTask.Count() > 0)
                {
                    var inspectTaskID = inspectTask.Select(r => r.InspectionId);
                    var event_inspect = this.Context.Set<GdEvents>().Where(r => r.IncidentType == 0 && inspectTaskID.Contains(r.TaskId));
                    gd_events = gd_events.Union(event_inspect);
                }
                #endregion
                //工单 GD_WorkOrder
                IEnumerable<long> EventIDList = new List<long>() { };
                if (gd_events.Count() > 0)
                {
                    EventIDList = gd_events.Select(r => r.IncidentId);
                }
                var gd_WorkOrder = this.Context.Set<GdWorkOrder>().Where(r => EventIDList.Contains(r.EventId));
                IEnumerable<long> WorkIDS = new List<long>() { };
                if (gd_WorkOrder.Count() > 0)
                {
                    WorkIDS = gd_WorkOrder.Select(r => r.Woid);
                }
                //延期 GD_WOExtension
                var gd_WoExtension = this.Context.Set<GdWoextension>().Where(r => WorkIDS.Contains(r.Woid));
                //移交 GD_WOReview
                var gd_WoReview = this.Context.Set<GdWoreview>().Where(r=>WorkIDS.Contains(r.Woid));
                //工单历史 GD_WOOperation
                var gd_woOperate = this.Context.Set<GdWooperation>().Where(r=> WorkIDS.Contains(r.Pid));
               
                    if (repairTask.Count() > 0)
                    {
                        foreach (var t in repairTask)
                        {
                            this.Context.Set<GdRepair>().Attach(t);
                        }
                        this.Context.Set<GdRepair>().RemoveRange(repairTask);
                       
                    }
                    if (maintainTask.Count() > 0)
                    {
                        foreach (var t in maintainTask)
                        {
                            this.Context.Set<GdMaintain>().Attach(t);
                        }
                        this.Context.Set<GdMaintain>().RemoveRange(maintainTask);
                    }
                    if (inspectTask.Count() > 0)
                    {
                        foreach (var t in inspectTask)
                        {
                            this.Context.Set<GdInspection>().Attach(t);
                        }
                        this.Context.Set<GdInspection>().RemoveRange(inspectTask);
                    }
                    if (gd_events.Count() > 0)
                    {
                        foreach (var t in gd_events)
                        {
                            this.Context.Set<GdEvents>().Attach(t);
                        }
                        this.Context.Set<GdEvents>().RemoveRange(gd_events);
                    }
                    if (gd_resource.Count() > 0)
                    {
                        foreach (var t in gd_resource)
                        {
                            this.Context.Set<GdResource>().Attach(t);
                        }
                        this.Context.Set<GdResource>().RemoveRange(gd_resource);
                    }
                    if (gd_WorkOrder.Count() > 0)
                    {
                        foreach (var t in gd_WorkOrder)
                        {
                            this.Context.Set<GdWorkOrder>().Attach(t);
                        }
                        this.Context.Set<GdWorkOrder>().RemoveRange(gd_WorkOrder);
                    }
                    if (gd_WoReview.Count() > 0)
                    {
                        foreach (var t in gd_WoReview)
                        {
                            this.Context.Set<GdWoreview>().Attach(t);
                        }
                        this.Context.Set<GdWoreview>().RemoveRange(gd_WoReview);
                    }
                    if (gd_WoExtension.Count() > 0)
                    {
                        foreach (var t in gd_WoExtension)
                        {
                            this.Context.Set<GdWoextension>().Attach(t);
                        }
                        this.Context.Set<GdWoextension>().RemoveRange(gd_WoExtension);
                    }
                    if (gd_woOperate.Count() > 0)
                    {
                        foreach (var t in gd_woOperate)
                        {
                            this.Context.Set<GdWooperation>().Attach(t);
                        }
                        this.Context.Set<GdWooperation>().RemoveRange(gd_woOperate);
                    }

                    
                #endregion
            }
            return this.Context.SaveChanges();
        }


        #region 泵房分布
        /// <summary>
        /// 泵房分布模块，泵房信息列表
        /// </summary>
        /// <param name="stationName">泵房名称</param>
        /// <param name="stationtype">泵房类型</param>
        /// <param name="userID">用户ID</param>
        /// <returns></returns>
        public IEnumerable<dynamic> GetMapStationInfo(string stationName, int stationtype, int userID)
        {
            SysUser user = this.Find<SysUser>(userID);
            StringBuilder sb = new StringBuilder();

            SqlParameter[] sp = new SqlParameter[]
            {
                new SqlParameter("@userID",userID),
                new SqlParameter("@stationName","%"+stationName+"%"),
                new SqlParameter("@stationtype",stationtype)
            };
            string sql = @"select s.StationID,StationName,StaitonType,Lng,Lat,d.ItemName as TypeName,InstallationDate from Sws_Station s left join Sys_DataItemDetail d on s.StaitonType=d.ItemValue and d.F_ItemId=10 and d.IsEnable=1 where 1=1";
            sb.Append(sql);
            if (!user.IsAdmin)
            {
                sb.Append(@" and s.StationID in (select StationID from Sws_UserStation where UserID=@userID)");
            }
            if (!string.IsNullOrEmpty(stationName))
            {
                sb.Append(@" and s.StationName like @stationName");
            }
            if (stationtype != 0)
            {
                sb.Append(@" and s.StaitonType=@stationtype");
            }



            var query = this.Context.Database.SqlQuery_Dic(sb.ToString(), sp).ToList();
            return query;
        }

        /// <summary>
        /// 泵房分布获取泵房类型及数量
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public IEnumerable<StationType> GetStationType(int userID)
        {
            SysUser user = this.Find<SysUser>(userID);
            SqlParameter[] sp = new SqlParameter[]
            {
                new SqlParameter("@userID",userID)
            };

            string sql = string.Empty;
            if (!user.IsAdmin)
            {
                sql = @"with t as (select StaitonType, count(StaitonType) as num from Sws_Station s left join Sws_UserStation u on s.StationID=u.StationID  where u.UserID=@userID  group by StaitonType)
                        select d.ItemValue,d.ItemName,isnull(t.num,0) as Num from Sys_DataItemDetail d left join t on d.ItemValue=t.StaitonType where d.F_ItemId=10 and d.IsEnable=1";
            }
            else
            {
                sql = @"with t as (select StaitonType, count(StaitonType) as num from Sws_Station  group by StaitonType)
                       select d.ItemValue,d.ItemName,isnull(t.num,0) as Num from Sys_DataItemDetail d left join t on d.ItemValue=t.StaitonType where d.F_ItemId=10 and d.IsEnable=1";
            }

            var query = this.Context.Database.SqlQuery<StationType>(sql, sp);
            return query;
        }

        /// <summary>
        /// 根据泵房ID查询泵房信息
        /// </summary>
        /// <param name="stationID"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> GetStationByID(int stationID)
        {
            //获取泵房类型，泵房安装位置字典值
            int type = (int)Model.CustomizedClass.Enum.泵房类型;
            //int position = (int)Model.CustomizedClass.Enum.泵房安装位置;
            SqlParameter[] sp = new SqlParameter[]
            {
                new SqlParameter("@stationID",stationID),
                new SqlParameter("@type",type),
                //new SqlParameter("@position",position)
            };
            string sql = @"select s.*,t.ItemName as StaitonTypeName  from [dbo].[Sws_Station] s
                        left join (select * from  [dbo].[Sys_DataItemDetail] where  F_ItemId=@type and  IsEnable=1)t on s.StaitonType=t.ItemValue                         
                        where StationID=@stationID";

            var query = this.Context.Database.SqlQuery_Dic(sql, sp).ToList();
            return query;
        }

        /// <summary>
        /// 根据泵房ID查询设备信息(二供设备)
        /// </summary>
        /// <param name="stationID"></param>
        /// <returns></returns>
        public IEnumerable<DeviceInfo01Info> GetDevice01ByID(int stationID)
        {
            SqlParameter[] sqlparameter = new SqlParameter[] {
                new SqlParameter("@stationID",stationID)
            };
            string sql = @"SELECT   d.DeviceID, d.DeviceName, d.DeviceNum, d.Partition, d.StationID, d.DeviceType, d.Frequency, d.ImageURL, d.Manufacturer, d.RTUID,
                           d.PumpNum, d.PumpType, d.GUI, d.ImportDN, d.ExportDN, d.ManufactureDate, d.RTUID,s.StationName,sd.ItemName as DeviceTypeName,sd1.ItemName as FrequencyName,
                           sd2.ItemName as ManufacturerName,sd3.ItemName as  PartitionName
                           FROM sws_DeviceInfo01 d  
                           left join Sws_Station s on d.StationID = s.StationID
                           left join Sys_DataItemDetail sd on  d.DeviceType = sd.ItemValue and sd.F_ItemId in 
                           (select F_ItemId from Sys_DataItemDetail  where [F_ItemId] =5 and IsEnable=1)
                           
                           left join Sys_DataItemDetail sd1 on  d.Frequency = sd1.ItemValue and sd1.F_ItemId in 
                           (select F_ItemId from Sys_DataItemDetail  where [F_ItemId] =6 and IsEnable=1)
                           
                           left join Sys_DataItemDetail sd2 on  d.Manufacturer = sd2.ItemValue and sd2.F_ItemId in 
                           (select F_ItemId from Sys_DataItemDetail  where [F_ItemId] =7 and IsEnable=1)

                           left join Sys_DataItemDetail sd3 on  d.Partition = sd3.ItemValue and sd3.F_ItemId in 
                           (select F_ItemId from Sys_DataItemDetail  where [F_ItemId] =8 and IsEnable=1) where d.StationID=@stationID";
            var query = this.Context.Database.SqlQuery<DeviceInfo01Info>(sql, sqlparameter);
            return query;
        }

        /// <summary>
        /// 根据泵房ID查询设备信息（直饮水设备）
        /// </summary>
        /// <param name="stationID"></param>
        /// <returns></returns>
        public IEnumerable<DeviceInfo02Info> GetDevice02ByID(int stationID)
        {
            SqlParameter[] sp = new SqlParameter[]
            {
                new SqlParameter("@stationID",stationID)
            };
            string sql = @"SELECT   d.DeviceID, d.DeviceName, d.DeviceNum, d.Partition, d.StationID, d.DeviceType, d.Manufacturer, 
                    d.ProductionDate,s.StationName,sd.ItemName as DeviceTypeName,d.ImageUrl,d.Gui,r.DeviceID as RtuNum,
                    sd2.ItemName as ManufacturerName,sd3.ItemName as  PartitionName
                    FROM sws_DeviceInfo02 d  
                    left join Sws_Station s on d.StationID = s.StationID
                    left join Sws_RTUInfo r on d.RTUID=r.RTUID
                    left join Sys_DataItemDetail sd on  d.DeviceType = sd.ItemValue and sd.F_ItemId in 
                    (select F_ItemId from Sys_DataItemDetail  where [F_ItemId] =14 and IsEnable=1)
                           
                    left join Sys_DataItemDetail sd2 on  d.Manufacturer = sd2.ItemValue and sd2.F_ItemId in 
                    (select F_ItemId from Sys_DataItemDetail  where [F_ItemId] =7 and IsEnable=1)

                    left join Sys_DataItemDetail sd3 on  d.Partition = sd3.ItemValue and sd3.F_ItemId in 
                    (select F_ItemId from Sys_DataItemDetail  where [F_ItemId] =8 and IsEnable=1)  where d.StationID=@stationID";
            var query = this.Context.Database.SqlQuery<DeviceInfo02Info>(sql, sp);
            return query;
        }

        /// <summary>
        /// 根据泵房ID获取资产信息
        /// </summary>
        /// <param name="stationID"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> GetPropertyInfoByID(int stationID)
        {
            int type = (int)Model.CustomizedClass.Enum.资产类型;
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@stationID",stationID),
                new SqlParameter("@type",type)
            };
            string sql = @" select s.*,t.ItemName as PropertyTypeName from [dbo].[Sws_PropertyInfo] s
                  left join (select * from  [dbo].[Sys_DataItemDetail] where  F_ItemId=@type and  IsEnable=1)t on s.Type=t.ItemValue where s.StationID=@stationID";
            var query = this.Context.Database.SqlQuery_Dic(sql, sp).ToList();
            return query;
        }
        #endregion
        #region 泵房监控(供水泵房)  
        //报警泵房ID
        public IEnumerable<dynamic> GetAlarmStion(bool isadmin, int userID, int stationtype)
        {
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@userID",userID),
                new SqlParameter("@stationtype",stationtype)
          };
            string sql = "";
            if (isadmin == true)
            {
                sql = @"WITH  t as (SELECT StationID from [dbo].[Sws_DeviceInfo01] where RTUID in (select RTUID from [dbo].[Sws_EventInfo] where EventLevel!=0))
  select distinct s.StationID from [dbo].[Sws_Station] s
  left join t on t.StationID=s.StationID
  where t.StationID is not null  and s.InType=@stationtype";
            }
            else
            {
                sql = @"  WITH  t as (SELECT StationID from [dbo].[Sws_DeviceInfo01] where RTUID in (select RTUID from [dbo].[Sws_EventInfo] where EventLevel!=0))
  select distinct s.StationID from [dbo].[Sws_Station] s
  left join  [dbo].[Sws_UserStation] u on u.StationID=s.StationID
  left join t on t.StationID=s.StationID
  where t.StationID is not null and u.UserID=@userID and s.InType=@stationtype";
            }
            var query = this.Context.Database.SqlQuery_Dic(sql, sp).ToList();
            return query;
        }

        //已弃用   2021.09.10
        public IEnumerable<dynamic> GetAlarmStion_New(bool isadmin, int userID, int stationtype, string onlineRtu)
        {
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@userID",userID),
                new SqlParameter("@stationtype",stationtype),
                new SqlParameter("@onlineRtu",onlineRtu)
          };
            string sql = "";
            if (isadmin == true)
            {
                sql = @"WITH  t as (SELECT StationID from [dbo].[Sws_DeviceInfo0" + @stationtype + "] where RTUID in (select RTUID     from [dbo].[Sws_EventInfo] where EventLevel!=0" +
                      @" ) and CHARINDEX(','+LTRIM(RTUID)+',',@onlineRtu)>0)" +
                      @" select distinct s.StationID from [dbo].[Sws_Station] s
                      left join t on t.StationID=s.StationID
                      where t.StationID is not null  and s.InType=@stationtype";
            }
            else
            {
                sql = @"  WITH  t as (SELECT StationID from [dbo].[Sws_DeviceInfo0" + @stationtype + "] where RTUID in (select        RTUID from [dbo].[Sws_EventInfo] where EventLevel!=0" +
                     @" )  and CHARINDEX(','+LTRIM(RTUID)+',',@onlineRtu)>0)" +
                     @" select distinct s.StationID from [dbo].[Sws_Station] s
                      left join  [dbo].[Sws_UserStation] u on u.StationID=s.StationID
                      left join t on t.StationID=s.StationID
                      where t.StationID is not null and u.UserID=@userID and s.InType=@stationtype";
            }
            var query = this.Context.Database.SqlQuery_Dic(sql, sp).ToList();
            return query;
        }
        //直饮水  //已弃用   2021.09.10
        public IEnumerable<dynamic> GetAlarmZStion_New(bool isadmin, int userID, int stationtype, string onlineRtu)
        {
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@userID",userID),
                new SqlParameter("@stationtype",stationtype),
                new SqlParameter("@onlineRtu",onlineRtu)
          };
            string sql = "";
            if (isadmin == true)
            {
                sql = @"WITH  t as (SELECT StationID from [dbo].[Sws_DeviceInfo02] where RTUID in (select RTUID from [dbo].             [Sws_EventInfo] where EventLevel!=0
                     ) and CHARINDEX(','+LTRIM(RTUID)+',',@onlineRtu)>0)" +
                     @" select distinct s.StationID from [dbo].[Sws_Station] s
                      left join t on t.StationID=s.StationID
                      where t.StationID is not null  and s.InType=@stationtype";
            }
            else
            {
                sql = @"  WITH  t as (SELECT StationID from [dbo].[Sws_DeviceInfo02] where RTUID in (select RTUID from [dbo].          [Sws_EventInfo] where EventLevel!=0
                     )  and CHARINDEX(','+LTRIM(RTUID)+',',@onlineRtu)>0)" +
                     @" select distinct s.StationID from [dbo].[Sws_Station] s
                      left join  [dbo].[Sws_UserStation] u on u.StationID=s.StationID
                      left join t on t.StationID=s.StationID
                      where t.StationID is not null and u.UserID=@userID and s.InType=@stationtype";
            }
            var query = this.Context.Database.SqlQuery_Dic(sql, sp).ToList();
            return query;
        }

        //直饮水报警泵房ID
        public IEnumerable<dynamic> GetAlarmZStion(bool isadmin, int userID, int stationtype)
        {
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@userID",userID),
                new SqlParameter("@stationtype",stationtype)
          };
            string sql = "";
            if (isadmin == true)
            {
                sql = @"WITH  t as (SELECT StationID from [dbo].[Sws_DeviceInfo02] where RTUID in (select RTUID from [dbo].[Sws_EventInfo] where EventLevel!=0))
                  select distinct s.StationID from [dbo].[Sws_Station] s
                  left join t on t.StationID=s.StationID
                  where t.StationID is not null and s.InType=@stationtype";
            }
            else
            {
                sql = @"  WITH  t as (SELECT StationID from [dbo].[Sws_DeviceInfo02] where RTUID in (select RTUID from [dbo].[Sws_EventInfo] where EventLevel!=0))
                  select distinct s.StationID from [dbo].[Sws_Station] s
                  left join  [dbo].[Sws_UserStation] u on u.StationID=s.StationID
                  left join t on t.StationID=s.StationID
                  where t.StationID is not null and u.UserID=@userID and s.InType=@stationtype";
            }
            var query = this.Context.Database.SqlQuery_Dic(sql, sp).ToList();
            return query;
        }
        //根据用户id来查询rtuid
        public IEnumerable<dynamic> GetRtuIDByUserID(int userid, int stationtype)
        {
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@userID",userid),
                new SqlParameter("@stationtype",stationtype)
           };
            string sql = @"  
                select distinct RTUID  from [dbo].[Sws_DeviceInfo01] where StationID in
(select u.StationID from [dbo].[Sws_UserStation] u 
  left join [dbo].[Sws_Station] s on u.StationID=s.StationID where UserID=@userID and s.InType=@stationtype) and RTUID is not null";
            var query = this.Context.Database.SqlQuery_Dic(sql, sp).ToList();
            return query;
        }
        //直饮水根据用户id来查询rtuid
        public IEnumerable<dynamic> GetZRtuIDByUserID(int userid, int stationtype)
        {
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@userID",userid),
                new SqlParameter("@stationtype",stationtype)
           };
            string sql = @"  
                select distinct RTUID  from [dbo].[Sws_DeviceInfo02] where StationID in
(select u.StationID from [dbo].[Sws_UserStation] u 
  left join [dbo].[Sws_Station] s on u.StationID=s.StationID where UserID=@userID and s.InType=@stationtype) and RTUID is not null";
            var query = this.Context.Database.SqlQuery_Dic(sql, sp).ToList();
            return query;
        }
        //泵房总数、在线数量 //已弃用   2021.09.10
        public IEnumerable<dynamic> GetSationNumMany(string alarmSationids, string onlineRtuid, string filter, int userid, bool isadmin, int stationtype)
        {
            int partid = (int)Model.CustomizedClass.Enum.设备分区;
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@userid",userid),
                new SqlParameter("@alarmSationids",alarmSationids),
                new SqlParameter("@onlineRtuid",onlineRtuid),
                new SqlParameter("@filter","%"+filter+"%"),
                new SqlParameter("@stationtype",stationtype),
                new SqlParameter("@partitonid",partid)
            };
            StringBuilder builder = new StringBuilder();
            if (filter == "")
            {
                builder.Append("with t as (select * from [dbo].[Sws_Station] where InType=@stationtype),");
            }
            else
            {
                builder.Append(@"
                 with t1 as ( select distinct StationID from [dbo].[Sws_DeviceInfo0" + @stationtype + "] d " +
                 @" left join (select * from [dbo].[Sys_DataItemDetail] where F_ItemId=@partitonid and IsEnable=1) a1 on d.Partition=a1.ItemValue
                where ItemName like @filter or DeviceNum like @filter)," +
                @"t11 as(
                select s.* from [dbo].[Sws_Station] s
                left join t1 on s.StationID=t1.StationID where t1.StationID is not null and s.InType=@stationtype),
                t2 as (
                select * from [dbo].[Sws_Station] where InType=@stationtype and StationName like @filter  or StationNum like  @filter)," +
                @"t as(
                select * from t2
                union select * from t11),");

            }
            if (isadmin == true)//泵房总数
            {
                builder.Append(@"alls as (select count(*) as allNum from(
                select distinct t.* from[dbo].[Sws_DeviceInfo0" + @stationtype + "] d " +
                @"left join t on d.StationID = t.StationID where t.StationID is not null) as alla1),");
            }
            else
            {
                builder.Append(@"alls as (select count(*) as allNum from(
                select distinct t.* from [dbo].[Sws_DeviceInfo0" + @stationtype + "] d " +
                @"left join t on d.StationID=t.StationID
                left join [dbo].[Sws_UserStation] u on t.StationID=u.StationID where u.UserID=@userid and t.StationID is not null) as alla1),");
            }
            //报警泵房数量
            builder.Append(@"alarms as(select count(*) as alarmNum  from t where   CHARINDEX(','+LTRIM(StationID)+',',@alarmSationids)>0),");
            //在线
            builder.Append(@"onlines as (select count(*) as onlineNum from(
            select distinct t.* from [dbo].[Sws_DeviceInfo0" + @stationtype + "] d " +
            @"left join t on d.StationID=t.StationID
            where t.StationID is not null and  CHARINDEX(','+LTRIM(RTUID)+',',@onlineRtuid)>0  and not  CHARINDEX(','+LTRIM(t.StationID)+',', @alarmSationids)>0) as g),");
            //关注
            builder.Append(@" attentions as(select count (*) as attentionNum from( select t.* from t
            left join [dbo].[Sws_UserStation] u on t.StationID=u.StationID
            where UserID=@userid and FocusOn=1 and u.StationID is not null) as g)");
            builder.Append("select allNum,alarmNum,onlineNum,attentionNum from alls,alarms,onlines,attentions");
            var sql = builder.ToString();
            var query = this.Context.Database.SqlQuery_Dic(sql, sp).ToList();
            return query;
        }


        //获取泵房以及设备数据 //已弃用   2021.09.10
        public IEnumerable<dynamic> GetStaionDataByPage(int stationtype, string type, string onlineRtuid, string alarmSids, string filter, int pageIndex, int pageSize, bool IsAdmin, long UserID, ref int totalcount)
        {
            var partitonid = (int)Model.CustomizedClass.Enum.设备分区;
            string sql = "";
            string sqlcount = "";
            SqlParameter[] sp = sp = new SqlParameter[] {
                    new SqlParameter("@filter","%"+filter+"%"),
                    new SqlParameter("@pageIndex",pageIndex),
                    new SqlParameter("@pageSize",pageSize),
                    new SqlParameter("@UserID",UserID),
                    new SqlParameter("@onlineRtuid",onlineRtuid),
                    new SqlParameter("@alarmSids",alarmSids),
                    new SqlParameter("@stationtype",stationtype),
                    new SqlParameter("@partitonid",partitonid)
                };
            StringBuilder sb = new StringBuilder();
            if (filter == "")//查询的是有设备的泵房
            {
                sb.Append("with t1 as (select distinct StationID from [dbo].[Sws_DeviceInfo0" + @stationtype + "]),t as (select s.* from [dbo].[Sws_Station] s" +
                    " left join t1 on s.StationID=t1.StationID where t1.StationID is not null and s.InType=@stationtype),");
            }
            else//查询的是有设备的泵房
            {
                sb.Append(@"
                 with t1 as ( select distinct StationID from [dbo].[Sws_DeviceInfo0" + @stationtype + "] d1 " +
                @"left join (select * from [dbo].[Sys_DataItemDetail] where F_ItemId=@partitonid and IsEnable=1) a1 on d1.Partition=a1.ItemValue " +
                @"where ItemName like @filter or DeviceNum like  @filter)," +
                @"t11 as(
                select s.* from [dbo].[Sws_Station] s
                left join t1 on s.StationID=t1.StationID where t1.StationID is not null and s.InType=@stationtype),
                t12 as(select distinct StationID from [dbo].[Sws_DeviceInfo0" + @stationtype + "])," +
                @"t2 as (
                select s.* from [dbo].[Sws_Station] s 
                left join t12 on s.StationID=t12.StationID where s.InType=@stationtype and t12.StationID is not null and   (StationName like @filter or StationNum like @filter))," +
                @"t as(
                select * from t2
                union select * from t11),");

            }
            string appends = "";
            if (type == "all")
            {
                appends = getAllStationByPage(IsAdmin);
                sb.Append(appends);
            }
            else if (type == "attention")
            {
                appends = getAttentionSByPage();
                sb.Append(appends);
            }
            else if (type == "online")
            {
                appends = getOnlineStationPage(IsAdmin, alarmSids, onlineRtuid, stationtype);
                sb.Append(appends);
            }
            else if (type == "alarm")
            {
                appends = getAlarmSPage(alarmSids);
                sb.Append(appends);
            }
            else
            {
                appends = getOffStionPage(IsAdmin, alarmSids, onlineRtuid, stationtype);
                sb.Append(appends);
            }
            sqlcount = @"" + sb.ToString().Substring(0, sb.ToString().Length - 1) + "" +
                    "select count(*) as totalcount from tt";
                                sb.Append(@"dt as(select tt.*,case when FocusOn=1 then 1 else 0 end FocusOn  from tt 
                      left join(select * from[dbo].[Sws_UserStation] where UserID =@UserID) u on tt.StationID = u.StationID),
                    data1 as(select row_number() over(order by FocusOn desc) as number ,* from dt),
                    data as( select * from data1 where number>(@pageIndex-1)*@pageSize and number<=@pageIndex*@pageSize)
                    ");
            if (stationtype == 1)
            {
                sb.Append(@" select data.StationID,data.StationNum,data.StationName,data.FocusOn,d.DeviceID,tail.ItemName as  DeviceName,d.RTUID,Partition,Frequency,PumpNum,ControlMonitor,CameraMonitor from data
                  left join[dbo].[Sws_DeviceInfo0" + @stationtype + "] d on data.StationID = d.StationID " +
                  @"left join (select * from [dbo].[Sys_DataItemDetail] where F_ItemId=@partitonid and IsEnable=1) tail on d.Partition=tail.ItemValue");
            }
            else if (stationtype == 2)
            {
                sb.Append(@" select data.StationID,data.StationNum,data.StationName,data.FocusOn,d.DeviceID,tail.ItemName as  DeviceName,d.RTUID,Partition,ControlMonitor,CameraMonitor from data
                  left join[dbo].[Sws_DeviceInfo0" + @stationtype + "] d on data.StationID = d.StationID " +
                     @"left join (select * from [dbo].[Sys_DataItemDetail] where F_ItemId=@partitonid and IsEnable=1) tail on d.Partition=tail.ItemValue");
            }
            sql = sb.ToString();
            var query = this.Context.Database.SqlQuery_Dic(sql, sp);
            totalcount = this.Context.Database.SqlQuery<QueryNum>(sqlcount, sp)[0].totalcount;

            return query;
        }

        class QueryNum
        {
            public int totalcount { get; set; }

        }


        public string getAllStationByPage(bool IsAdmin)
        {
            StringBuilder s = new StringBuilder();

            if (IsAdmin == true)
            {
                s.Append("tt as (select * from t ),");
            }
            else
            {
                s.Append("tt as (select t.* from [dbo].[Sws_UserStation] u " +
                    "left join t on t.StationID=u.StationID where UserID=@UserID and t.StationID is not null),");
            }

            return s.ToString();
        }


        //关注泵房
        public string getAttentionSByPage()
        {
            string s = @"tt as (select t.* from [dbo].[Sws_UserStation] u " +
                    "left join t on t.StationID=u.StationID where UserID=@UserID and t.StationID is not null and u.FocusOn=1),";
            return s;
        }
        //在线泵房 //已弃用   2021.09.10
        public string getOnlineStationPage(bool IsAdmin, string alarmSationids, string onlineRtuid, int stationtype)
        {
            StringBuilder s = new StringBuilder();
            //if (IsAdmin == true)
            //{
            s.Append(@"tt as (
              select distinct t.* from [dbo].[Sws_DeviceInfo0" + @stationtype + "] d " +
            @"  left join t on d.StationID=t.StationID where CHARINDEX(','+LTRIM(RTUID)+',',@onlineRtuid)>0 and not  CHARINDEX(','+LTRIM(t.StationID)+',',@alarmSationids)>0 and t.StationID is not null),");
            //            }
            //            else
            //            {
            //                s.Append(@"m as (select distinct t.* from [dbo].[Sws_DeviceInfo01] d
            //  left join t on d.StationID=t.StationID
            //  left join [dbo].[Sws_UserStation] u on t.StationID=u.StationID
            //where u.UserID=@UserID and  CHARINDEX(','+LTRIM(RTUID)+',','," + @onlineRtuid + ",')>0 and not  CHARINDEX(','+LTRIM(t.StationID)+',','," + @alarmSationids + ",')>0 and t.StationID is not null),"+
            //"tt as(select row_number() over(order by StationName) as number ,m.* from m),");
            //            }
            return s.ToString();
        }


        //报警泵房//已弃用   2021.09.10
        public string getAlarmSPage(string alarmSationids)
        {
            string s = @"tt as (select t.* from t where CHARINDEX(','+LTRIM(t.StationID)+',',@alarmSationids)>0),";
            return s;
        }

        //离线泵房//已弃用   2021.09.10
        public string getOffStionPage(bool IsAdmin, string alarmSationids, string onlineRtuid, int stationtype)
        {
            StringBuilder s = new StringBuilder();
            if (IsAdmin == true)
            {
                s.Append(@"  m as(select StationID from [dbo].[Sws_DeviceInfo0" + @stationtype + "] where CHARINDEX(','+LTRIM(RTUID)+',',@onlineRtuid)>0" +
                  @"union (select StationID from [dbo].[Sws_Station] where CHARINDEX(','+LTRIM(StationID)+',',@alarmSationids)>0 ))," +
                  @"tt as(select t.* from t
                  left join m on t.StationID=m.StationID where m.StationID is null),");
            }
            else
            {

                s.Append(@"  m as(select StationID from [dbo].[Sws_DeviceInfo0" + @stationtype + "] where CHARINDEX(','+LTRIM(RTUID)+',',@onlineRtuid )>0" +
                  @"union (select StationID from [dbo].[Sws_Station] where CHARINDEX(','+LTRIM(StationID)+',',@alarmSationids)>0 ))," +
                  @"tt as(select t.* from t
                left join [dbo].[Sws_UserStation] u on u.StationID=t.StationID
                  left join m on t.StationID=m.StationID where m.StationID is null and u.UserID=@UserID),");
            }
            return s.ToString();

        }

        #region 精简模式
        //已弃用   2021.09.10
        public IEnumerable<dynamic> GetStaionDataByPage_simple(int stationtype, string type, string onlineRtuid, string alarmSids, string filter, int pageIndex, int pageSize, bool IsAdmin, long UserID, ref int totalcount)
        {
            string sql = "";
            string sqlcount = "";
            var partitonid = (int)Model.CustomizedClass.Enum.设备分区;
            SqlParameter[] sp = sp = new SqlParameter[] {
                    new SqlParameter("@filter","%"+filter+"%"),
                    new SqlParameter("@pageIndex",pageIndex),
                    new SqlParameter("@pageSize",pageSize),
                    new SqlParameter("@UserID",UserID),
                    new SqlParameter("@onlineRtuid",onlineRtuid),
                    new SqlParameter("@alarmSids",alarmSids),
                    new SqlParameter("@stationtype",stationtype),
                    new SqlParameter("@partitonid",partitonid)

                };
            StringBuilder sb = new StringBuilder();
            if (filter == "")//查询的是有设备的泵房
            {
                sb.Append("with t1 as (select distinct StationID from [dbo].[Sws_DeviceInfo0" + @stationtype + "]),t as (select s.* from [dbo].[Sws_Station] s" +
                    " left join t1 on s.StationID=t1.StationID where t1.StationID is not null and s.InType=@stationtype),");

            }
            else//查询的是有设备的泵房
            {
                sb.Append(@"
                 with t1 as ( select distinct StationID from [dbo].[Sws_DeviceInfo0" + @stationtype + "] d1 " +
                @"left join (select * from [dbo].[Sys_DataItemDetail] where F_ItemId=@partitonid and IsEnable=1) a1 on d1.Partition=a1.ItemValue 
                  where ItemName  like  @filter  or DeviceNum like  @filter )," +
                @"t11 as(
                select s.* from [dbo].[Sws_Station] s
                left join t1 on s.StationID=t1.StationID where t1.StationID is not null and s.InType=@stationtype),
                t12 as(select distinct StationID from [dbo].[Sws_DeviceInfo0" + @stationtype + "])," +
                @"t2 as (
                select s.* from [dbo].[Sws_Station] s 
                left join t12 on s.StationID=t12.StationID where s.InType=@stationtype and t12.StationID is not null and   (StationName like  @filter or StationNum like @filter))," +
                @"t as(
                select * from t2
                union select * from t11),");

            }
            string appends = "";
            if (type == "all")
            {
                appends = getAllStationByPage(IsAdmin);
                sb.Append(appends);
                sqlcount = @"" + sb.ToString().Substring(0, sb.ToString().Length - 1) + "" +
                "select count(*) as totalcount from tt";


                sb.Append(@"data1 as (select tt.*,case when FocusOn=1 then 1 else  0 end FocusOn from tt
                left join (select * from [dbo].[Sws_UserStation] where UserID=@UserID) u on  tt.StationID=u.StationID ),");
                sb.Append(@"data2 as(select row_number() over(order by FocusOn desc) as number,* from data1),");
                sb.Append(@"base as(select * from data2 where number>(@pageIndex-1)*@pageSize and number<=@pageIndex*@pageSize),");

                sb.Append(@"online as (
  select distinct base.*,'正常' as type from [dbo].[Sws_DeviceInfo0" + @stationtype + "] d " +
  @"left join base on d.StationID=base.StationID where CHARINDEX(','+LTRIM(RTUID)+',',@onlineRtuid)>0 and not  CHARINDEX(','+LTRIM(base.StationID)+',',@alarmSids)>0 and base.StationID is not null),");
                sb.Append(@"alarm as (select *,'故障' as type from base  where CHARINDEX(','+LTRIM(StationID)+',',@alarmSids )>0),");
                sb.Append(@"off1 as (select StationID from [dbo].[Sws_DeviceInfo0" + stationtype + "] where CHARINDEX(','+LTRIM(RTUID)+',',@onlineRtuid)>0" +
                    @" union (select StationID from [dbo].[Sws_Station] where CHARINDEX(','+LTRIM(StationID)+',',@alarmSids)>0 ))," +
                    @"offline as(select base.*,'离线' as type from base
  left join off1 on base.StationID=off1.StationID where off1.StationID is null),");
                sb.Append(@"r as (select * from online
union select * from alarm
union select * from offline) select * from r order by FocusOn desc ");

            }

            else if (type == "attention")
            {
                appends = getAttentionSByPage();
                sb.Append(appends);
                sqlcount = @"" + sb.ToString().Substring(0, sb.ToString().Length - 1) + "" +
                    "select count(*) as totalcount from tt";
                sb.Append(@"data1 as (select tt.*,1 as FocusOn from tt ),");
                sb.Append(@"data2 as(select row_number() over(order by FocusOn desc) as number,* from data1),");

                sb.Append(@"base as (select * from data2 where number>(@pageIndex-1)*@pageSize and number<=@pageIndex*@pageSize),");

                sb.Append(@"online as (
                  select distinct base.*,'正常' as type from [dbo].[Sws_DeviceInfo0" + @stationtype + "] d " +
                 @" left join base on d.StationID=base.StationID where CHARINDEX(','+LTRIM(RTUID)+',',@onlineRtuid )>0 and not  CHARINDEX(','+LTRIM(base.StationID)+',',@alarmSids)>0 and base.StationID is not null),");
                sb.Append(@"alarm as (select *,'故障' as type from base  where CHARINDEX(','+LTRIM(StationID)+',',@alarmSids)>0),");
                sb.Append(@"off1 as (select StationID from [dbo].[Sws_DeviceInfo0" + stationtype + "] where CHARINDEX(','+LTRIM(RTUID)+',',@onlineRtuid )>0" +
                    @" union (select StationID from [dbo].[Sws_Station] where CHARINDEX(','+LTRIM(StationID)+',',@alarmSids)>0 ))," +
                    @"offline as(select base.*,'离线' as type from base
                left join off1 on base.StationID=off1.StationID where off1.StationID is null),");
                sb.Append(@"r as (select * from online
                union select * from alarm
                union select * from offline) select * from r order by StationName ");

            }
            else if (type == "online")
            {
                appends = getOnlineStationPage(IsAdmin, alarmSids, onlineRtuid, stationtype);
                sb.Append(appends);
                sqlcount = @"" + sb.ToString().Substring(0, sb.ToString().Length - 1) + "" +
                "select count(*) as totalcount from tt";

                sb.Append(@"data1 as (select tt.*,case when FocusOn=1 then 1 else  0 end FocusOn from tt
                left join (select * from [dbo].[Sws_UserStation] where UserID=@UserID) u on  tt.StationID=u.StationID ),");
                sb.Append(@"data2 as(select row_number() over(order by FocusOn desc) as number,* from data1),");
                sb.Append(@"data as(select * from data2 where number>(@pageIndex-1)*@pageSize and number<=@pageIndex*@pageSize)");

                sb.Append(@"select data.StationID,data.StationNum,data.StationName,data.FocusOn,'正常' as type from data");
            }
            else if (type == "alarm")
            {
                appends = getAlarmSPage(alarmSids);
                sb.Append(appends);
                sqlcount = @"" + sb.ToString().Substring(0, sb.ToString().Length - 1) + "" +
                "select count(*) as totalcount from tt";

                sb.Append(@"data1 as (select tt.*,case when FocusOn=1 then 1 else  0 end FocusOn from tt
                left join (select * from [dbo].[Sws_UserStation] where UserID=@UserID) u on  tt.StationID=u.StationID ),");
                sb.Append(@"data2 as(select row_number() over(order by FocusOn desc) as number,* from data1),");
                sb.Append(@"data as(select * from data2 where number>(@pageIndex-1)*@pageSize and number<=@pageIndex*@pageSize)");
                sb.Append(@"select data.StationID,data.StationNum,data.StationName,data.FocusOn,'故障' as type from data");
            }
            else
            {
                appends = getOffStionPage(IsAdmin, alarmSids, onlineRtuid, stationtype);
                sb.Append(appends);
                sqlcount = @"" + sb.ToString().Substring(0, sb.ToString().Length - 1) + "" +
                "select count(*) as totalcount from tt";

                sb.Append(@"data1 as (select tt.*,case when FocusOn=1 then 1 else  0 end FocusOn from tt
                left join (select * from [dbo].[Sws_UserStation] where UserID=@UserID) u on  tt.StationID=u.StationID ),");
                sb.Append(@"data2 as(select row_number() over(order by FocusOn desc) as number,* from data1),");
                sb.Append(@"data as(select * from data2 where number>(@pageIndex-1)*@pageSize and number<=@pageIndex*@pageSize)");

                sb.Append(@"
                select data.StationID,data.StationNum,data.StationName,data.FocusOn,'离线' as type from data
   ");
            }

            sql = sb.ToString();
            var query = this.Context.Database.SqlQuery_Dic(sql, sp);
            totalcount = this.Context.Database.SqlQuery<QueryNum>(sqlcount, sp)[0].totalcount;

            return query;
        }

        #endregion

        //查询报警
        public IEnumerable<Eventinfo> GetEventinfos(int devicetype, List<int> rtuids)
        {
            var query = from e in Query<SwsEventInfo>(r => rtuids.Contains(r.Rtuid) && r.EventLevel != 0 && r.State == 1)
                        join d in Query<SwsDataInfo>(r => r.DeviceType == devicetype) on e.EventSource equals d.DataId into d1
                        from dd in d1.DefaultIfEmpty()
                        select new Eventinfo
                        {
                            ID = e.Id,
                            CurrentValue = e.CurrentValue,
                            EventMessage = e.EventMessage,
                            EventTime = e.EventTime,
                            LimitValue = e.LimitValue,
                            Unit = dd.Unit,
                            DataType = dd.DataType,
                            Rtuid = e.Rtuid,
                            EventSource = e.EventSource
                        };
            return query;
        }
        #endregion
        #region 泵房监控 表变量
        //新的泵房总数、在线数量
        public IEnumerable<dynamic> GetSationNumMany_tvp(DataTable tvpDt, string filter, int userid, bool isadmin, int stationtype)
        {
            int partid = (int)Model.CustomizedClass.Enum.设备分区;

            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@isadmin",isadmin),
                new SqlParameter("@userid",userid),
                 new SqlParameter("@stationtype",stationtype),
                new SqlParameter("@filter",filter),
                new SqlParameter("@partitonid",partid),
                new SqlParameter("@onlineRtu", SqlDbType.Structured) { Value = tvpDt, TypeName = "OnlineRtuIDTVP" }
            };
            var query = this.Context.Database.SqlQuery_Dic("exec GetSationNumMany @isadmin,@userID,@stationtype,@filter,@partitonid,@onlineRtu", sp).ToList();
            return query;
        }
        //泵房监控过滤掉智能泵房（2021.09.14）
        public IEnumerable<dynamic> GetSationNumMany_StationJK(DataTable tvpDt, string filter, int userid, bool isadmin, int stationtype)
        {
            int partid = (int)Model.CustomizedClass.Enum.设备分区;

            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@isadmin",isadmin),
                new SqlParameter("@userid",userid),
                 new SqlParameter("@stationtype",stationtype),
                new SqlParameter("@filter",filter),
                new SqlParameter("@partitonid",partid),
                new SqlParameter("@onlineRtu", SqlDbType.Structured) { Value = tvpDt, TypeName = "OnlineRtuIDTVP" }
            };
            var query = this.Context.Database.SqlQuery_Dic("exec GetSationNumMany_staionJK @isadmin,@userID,@stationtype,@filter,@partitonid,@onlineRtu", sp).ToList();
            return query;
        }
        public IEnumerable<dynamic> GetStaionDataByPage_tvp(DataTable tvpDt, int stationtype, string type, string filter, int pageIndex, int pageSize, bool IsAdmin, long UserID, ref int totalcount)
        {
            var partitonid = (int)Model.CustomizedClass.Enum.设备分区;
            string sql = "";
            string sqlcount = "";
            SqlParameter[] sp = sp = new SqlParameter[] {
                    new SqlParameter("@filter","%"+filter+"%"),
                    new SqlParameter("@pageIndex",pageIndex),
                    new SqlParameter("@pageSize",pageSize),
                    new SqlParameter("@UserID",UserID),
                    new SqlParameter("@stationtype",stationtype),
                    new SqlParameter("@partitonid",partitonid),
                    new SqlParameter("@onlineRtu", SqlDbType.Structured) { Value = tvpDt, TypeName = "OnlineRtuIDTVP" }

                };
            StringBuilder sb = new StringBuilder();
            //报警泵房
            if (IsAdmin == true)
            {
                sb.Append(@"
                with tf as(SELECT distinct RTUID,StationID from [dbo].[Sws_DeviceInfo0" + @stationtype + "] where RTUID in (select RTUID from [dbo].[Sws_EventInfo] where EventLevel!=0))," +
                @"n as (select * from @onlineRtu),
                alarmIds as( select tf.StationID from tf
                left join n on tf.RTUID=n.RTUID
                where n.RTUID is not null),
                ");
            }
            else
            {
                sb.Append(@"
                with tf as(SELECT distinct RTUID,StationID from [dbo].[Sws_DeviceInfo0" + @stationtype + "] where RTUID in (select RTUID from [dbo].[Sws_EventInfo] where EventLevel!=0))," +
                @"n as (select * from @onlineRtu),
                alarmIds as ( select tf.StationID from tf
                left join n on tf.RTUID=n.RTUID
                left join [dbo].[Sws_UserStation] u on u.StationID=tf.StationID
                where n.RTUID is not null and u.UserID=@UserID),");
            }
            if (filter == "")//查询的是有设备的泵房
            {
                sb.Append(" t1 as (select distinct StationID from [dbo].[Sws_DeviceInfo0" + @stationtype + "] where Partition!=6),t as (select s.* from [dbo].[Sws_Station] s" +
                    " left join t1 on s.StationID=t1.StationID where t1.StationID is not null and s.InType=@stationtype),");
            }
            else//查询的是有设备的泵房
            {
                sb.Append(@"
                 t1 as ( select distinct StationID from [dbo].[Sws_DeviceInfo0" + @stationtype + "] d1 " +
                @"left join (select * from [dbo].[Sys_DataItemDetail] where F_ItemId=@partitonid and IsEnable=1) a1 on d1.Partition=a1.ItemValue " +
                @"where (ItemName like @filter or DeviceNum like @filter) and Partition!=6)," +
                @"t11 as(
                select s.* from [dbo].[Sws_Station] s
                left join t1 on s.StationID=t1.StationID where t1.StationID is not null and s.InType=@stationtype),
                t12 as(select distinct StationID from [dbo].[Sws_DeviceInfo0" + @stationtype + "] where Partition!=6)," +
                @"t2 as (
                select s.* from [dbo].[Sws_Station] s 
                left join t12 on s.StationID=t12.StationID where s.InType=@stationtype and t12.StationID is not null and   (StationName like @filter or StationNum like  @filter))," +
                @"t as(
                select * from t2
                union select * from t11),");

            }
            string appends = "";
            if (type == "all")
            {
                appends = getAllStationByPage(IsAdmin);
                sb.Append(appends);
            }
            else if (type == "attention")
            {
                appends = getAttentionSByPage();
                sb.Append(appends);
            }
            else if (type == "online")
            {
                appends = getOnlineStationPage_tvp(stationtype);
                sb.Append(appends);
            }
            else if (type == "alarm")
            {
                appends = getAlarmSPage_tvp();
                sb.Append(appends);
            }
            else
            {
                appends = getOffStionPage_tvp(IsAdmin, stationtype);
                sb.Append(appends);
            }
            sqlcount = @"" + sb.ToString().Substring(0, sb.ToString().Length - 1) + "" +
            "select count(*) as totalcount from tt";
                        sb.Append(@"dt as(select tt.*,case when FocusOn=1 then 1 else 0 end FocusOn  from tt 
              left join(select * from[dbo].[Sws_UserStation] where UserID =@UserID) u on tt.StationID = u.StationID),
            data1 as(select row_number() over(order by FocusOn desc) as number ,* from dt),
            data as( select * from data1 where number>(@pageIndex-1)*@pageSize and number<=@pageIndex*@pageSize)
            ");
            if (stationtype == 1)
            {
                sb.Append(@" select data.StationID,data.StationNum,data.StationName,data.FocusOn,d.DeviceID,tail.ItemName as  DeviceName,d.RTUID,Partition,Frequency,PumpNum,ControlMonitor,CameraMonitor from data
              left join[dbo].[Sws_DeviceInfo0" + @stationtype + "] d on data.StationID = d.StationID " +
              @"left join (select * from [dbo].[Sys_DataItemDetail] where F_ItemId=@partitonid and IsEnable=1) tail on d.Partition=tail.ItemValue where d.Partition!=6");
            }
            else if (stationtype == 2)
            {
                sb.Append(@" select data.StationID,data.StationNum,data.StationName,data.FocusOn,d.DeviceID,tail.ItemName as  DeviceName,d.RTUID,Partition,ControlMonitor,CameraMonitor from data
                  left join[dbo].[Sws_DeviceInfo0" + @stationtype + "] d on data.StationID = d.StationID " +
                     @"left join (select * from [dbo].[Sys_DataItemDetail] where F_ItemId=@partitonid and IsEnable=1) tail on d.Partition=tail.ItemValue where d.Partition!=6");
            }
            sql = sb.ToString();
            var query = this.Context.Database.SqlQuery_Dic(sql, sp);
            totalcount = this.Context.Database.SqlQuery<QueryNum>(sqlcount, sp)[0].totalcount;

            return query;
        }
        public string getOnlineStationPage_tvp(int stationtype)
        {
            StringBuilder s = new StringBuilder();

            s.Append(@"tt as (
              select distinct t.* from [dbo].[Sws_DeviceInfo0" + @stationtype + "] d " +
            @"  left join t on d.StationID=t.StationID where RTUID in (select * from @onlineRtu)  and t.StationID not in (select * from alarmIds) and t.StationID is not null),");

            return s.ToString();
        }
        //地图查询数据
        public IEnumerable<dynamic> GetStaionDataByMap_tvp(DataTable tvpDt, int stationtype, string type, string filter,  bool IsAdmin, long UserID)
        {
            var partitonid = (int)Model.CustomizedClass.Enum.设备分区;
            string sql = "";
            string sqlcount = "";
            SqlParameter[] sp = sp = new SqlParameter[] {
                    new SqlParameter("@filter","%"+filter+"%"), 
                    new SqlParameter("@UserID",UserID),
                    new SqlParameter("@stationtype",stationtype),
                    new SqlParameter("@partitonid",partitonid),
                    new SqlParameter("@onlineRtu", SqlDbType.Structured) { Value = tvpDt, TypeName = "OnlineRtuIDTVP" }

                };
            StringBuilder sb = new StringBuilder();
            //报警泵房
            if (IsAdmin == true)
            {
                sb.Append(@"
                with tf as(SELECT distinct RTUID,StationID from [dbo].[Sws_DeviceInfo0" + @stationtype + "] where RTUID in (select RTUID from [dbo].[Sws_EventInfo] where EventLevel!=0))," +
                @"n as (select * from @onlineRtu),
                alarmIds as( select tf.StationID from tf
                left join n on tf.RTUID=n.RTUID
                where n.RTUID is not null),
                ");
            }
            else
            {
                sb.Append(@"
                with tf as(SELECT distinct RTUID,StationID from [dbo].[Sws_DeviceInfo0" + @stationtype + "] where RTUID in (select RTUID from [dbo].[Sws_EventInfo] where EventLevel!=0))," +
                @"n as (select * from @onlineRtu),
                alarmIds as ( select tf.StationID from tf
                left join n on tf.RTUID=n.RTUID
                left join [dbo].[Sws_UserStation] u on u.StationID=tf.StationID
                where n.RTUID is not null and u.UserID=@UserID),");
            }
            if (string.IsNullOrEmpty(filter))//查询的是有设备的泵房
            {
                sb.Append(" t1 as (select distinct StationID from [dbo].[Sws_DeviceInfo0" + @stationtype + "]),t as (select s.* from [dbo].[Sws_Station] s" +
                    " left join t1 on s.StationID=t1.StationID where t1.StationID is not null and s.InType=@stationtype),");
            }
            else//查询的是有设备的泵房
            {
                sb.Append(@"
                 t1 as ( select distinct StationID from [dbo].[Sws_DeviceInfo0" + @stationtype + "] d1 " +
                @"left join (select * from [dbo].[Sys_DataItemDetail] where F_ItemId=@partitonid and IsEnable=1) a1 on d1.Partition=a1.ItemValue " +
                @"where ItemName like @filter or DeviceNum like  @filter)," +
                @"t11 as(
                select s.* from [dbo].[Sws_Station] s
                left join t1 on s.StationID=t1.StationID where t1.StationID is not null and s.InType=@stationtype),
                t12 as(select distinct StationID from [dbo].[Sws_DeviceInfo0" + @stationtype + "])," +
                @"t2 as (
                select s.* from [dbo].[Sws_Station] s 
                left join t12 on s.StationID=t12.StationID where s.InType=@stationtype and t12.StationID is not null and   (StationName like @filter or StationNum like @filter))," +
                @"t as(
                select * from t2
                union select * from t11),");

            }
            string appends = "";
            if (type == "all")
            {
                appends = getAllStationByPage(IsAdmin);
                sb.Append(appends);
            }
            else if (type == "attention")
            {
                appends = getAttentionSByPage();
                sb.Append(appends);
            }
            else if (type == "online")
            {
                appends = getOnlineStationPage_tvp(stationtype);
                sb.Append(appends);
            }
            else if (type == "alarm")
            {
                appends = getAlarmSPage_tvp();
                sb.Append(appends);
            }
            else
            {
                appends = getOffStionPage_tvp(IsAdmin, stationtype);
                sb.Append(appends);
            }
            sqlcount = @"" + sb.ToString().Substring(0, sb.ToString().Length - 1) + "" +
            "select count(*) as totalcount from tt";
                        sb.Append(@"dt as(select tt.*,case when FocusOn=1 then 1 else 0 end FocusOn  from tt 
              left join(select * from[dbo].[Sws_UserStation] where UserID =@UserID) u on tt.StationID = u.StationID),
            data1 as(select row_number() over(order by FocusOn desc) as number ,* from dt),
            data as( select * from data1)
            ");
            if (stationtype == 1)
            {
                sb.Append(@" select data.StationID,data.StationNum,data.StationName,data.FocusOn,d.DeviceID,tail.ItemName as  DeviceName,d.RTUID,Partition,Lat,Lng,Frequency,PumpNum,ControlMonitor,CameraMonitor from data
                  left join[dbo].[Sws_DeviceInfo0" + @stationtype + "] d on data.StationID = d.StationID " +
                  @"left join (select * from [dbo].[Sys_DataItemDetail] where F_ItemId=@partitonid and IsEnable=1) tail on d.Partition=tail.ItemValue");
            }
            else if (stationtype == 2)
            {
                sb.Append(@" select data.StationID,data.StationNum,data.StationName,data.FocusOn,d.DeviceID,tail.ItemName as  DeviceName,d.RTUID,Partition,Lat,Lng,ControlMonitor,CameraMonitor from data
              left join[dbo].[Sws_DeviceInfo0" + @stationtype + "] d on data.StationID = d.StationID " +
                 @"left join (select * from [dbo].[Sys_DataItemDetail] where F_ItemId=@partitonid and IsEnable=1) tail on d.Partition=tail.ItemValue");
            }
            sql = sb.ToString();
            var query = this.Context.Database.SqlQuery_Dic(sql, sp);
            return query;
        }
        public string getAlarmSPage_tvp()
        {
            string s = @"tt as (select t.* from t where t.StationID in (select * from alarmIds)),";
            return s;
        }
        public string getOffStionPage_tvp(bool IsAdmin, int stationtype)
        {
            StringBuilder s = new StringBuilder();
            if (IsAdmin == true)
            {
                s.Append(@"  m as(select StationID from [dbo].[Sws_DeviceInfo0" + @stationtype + "] where  RTUID in (select * from @onlineRtu) " +
              @"union (select StationID from [dbo].[Sws_Station] where StationID in (select * from alarmIds)))," +
              @"tt as(select t.* from t
              left join m on t.StationID=m.StationID where m.StationID is null),");
            }
            else
            {

                s.Append(@"  m as(select StationID from [dbo].[Sws_DeviceInfo0" + @stationtype + "] where RTUID in (select * from @onlineRtu) " +
                  @"union (select StationID from [dbo].[Sws_Station] where StationID in (select * from alarmIds) ))," +
                  @"tt as(select t.* from t
                left join [dbo].[Sws_UserStation] u on u.StationID=t.StationID
                left join m on t.StationID=m.StationID where m.StationID is null and u.UserID=@UserID),");
            }
            return s.ToString();

        }
        //精简模式
        public IEnumerable<dynamic> GetStaionDataByPage_simple_tvp(DataTable tvpDt, int stationtype, string type, string filter, int pageIndex, int pageSize, bool IsAdmin, long UserID, ref int totalcount)
        {
            string sql = "";
            string sqlcount = "";
            var partitonid = (int)Model.CustomizedClass.Enum.设备分区;
            SqlParameter[] sp = sp = new SqlParameter[] {
                    new SqlParameter("@filter","%"+filter+"%"),
                    new SqlParameter("@pageIndex",pageIndex),
                    new SqlParameter("@pageSize",pageSize),
                    new SqlParameter("@UserID",UserID),

                    new SqlParameter("@stationtype",stationtype),
                    new SqlParameter("@partitonid",partitonid),
                    new SqlParameter("@onlineRtu", SqlDbType.Structured) { Value = tvpDt, TypeName = "OnlineRtuIDTVP" }

                };
            StringBuilder sb = new StringBuilder();
            //报警泵房
            if (IsAdmin == true)
            {
                sb.Append(@"
                with tf as(SELECT distinct RTUID,StationID from [dbo].[Sws_DeviceInfo0" + @stationtype + "] where RTUID in (select RTUID from [dbo].[Sws_EventInfo] where EventLevel!=0))," +
                @"n as (select * from @onlineRtu),
                alarmIds as( select tf.StationID from tf
                left join n on tf.RTUID=n.RTUID
                where n.RTUID is not null),
                ");
            }
            else
            {
                sb.Append(@"
                with tf as(SELECT distinct RTUID,StationID from [dbo].[Sws_DeviceInfo0" + @stationtype + "] where RTUID in (select RTUID from [dbo].[Sws_EventInfo] where EventLevel!=0))," +
                @"n as (select * from @onlineRtu),
                alarmIds as ( select tf.StationID from tf
                left join n on tf.RTUID=n.RTUID
                left join [dbo].[Sws_UserStation] u on u.StationID=tf.StationID
                where n.RTUID is not null and u.UserID=@UserID),");
            }

            if (filter == "")//查询的是有设备的泵房
            {
                sb.Append("t1 as (select distinct StationID from [dbo].[Sws_DeviceInfo0" + @stationtype + "] where Partition!=6),t as (select s.* from [dbo].[Sws_Station] s" +
                    " left join t1 on s.StationID=t1.StationID where t1.StationID is not null and s.InType=@stationtype),");

            }
            else//查询的是有设备的泵房
            {
                sb.Append(@"
                  t1 as ( select distinct StationID from [dbo].[Sws_DeviceInfo0" + @stationtype + "] d1 " +
                @"left join (select * from [dbo].[Sys_DataItemDetail] where F_ItemId=@partitonid and IsEnable=1) a1 on d1.Partition=a1.ItemValue 
                  where (ItemName  like @filter or DeviceNum like  @filter) and Partition!=6 )," +
                @"t11 as(
                select s.* from [dbo].[Sws_Station] s
                left join t1 on s.StationID=t1.StationID where t1.StationID is not null and s.InType=@stationtype),
                t12 as(select distinct StationID from [dbo].[Sws_DeviceInfo0" + @stationtype + "] where Partition!=6)," +
                @"t2 as (
                select s.* from [dbo].[Sws_Station] s 
                left join t12 on s.StationID=t12.StationID where s.InType=@stationtype and t12.StationID is not null and   (StationName like @filter  or StationNum like  @filter ))," +
                @"t as(
                select * from t2
                union select * from t11),");

            }
            string appends = "";
            if (type == "all")
            {
                appends = getAllStationByPage(IsAdmin);
                sb.Append(appends);
                sqlcount = @"" + sb.ToString().Substring(0, sb.ToString().Length - 1) + "" +
                "select count(*) as totalcount from tt";


                sb.Append(@"data1 as (select tt.*,case when FocusOn=1 then 1 else  0 end FocusOn from tt
                left join (select * from [dbo].[Sws_UserStation] where UserID=@UserID) u on  tt.StationID=u.StationID ),");
                sb.Append(@"data2 as(select row_number() over(order by FocusOn desc) as number,* from data1),");
                sb.Append(@"base as(select * from data2 where number>(@pageIndex-1)*@pageSize and number<=@pageIndex*@pageSize),");

                sb.Append(@"online as (
                  select distinct base.*,'正常' as type from [dbo].[Sws_DeviceInfo0" + @stationtype + "] d " +
                  @"left join base on d.StationID=base.StationID where RTUID in (select * from @onlineRtu)  and  base.StationID not in (select * from alarmIds)  and base.StationID is not null),");
                sb.Append(@"alarm as (select *,'故障' as type from base  where StationID in (select * from alarmIds)),");
                sb.Append(@"off1 as (select StationID from [dbo].[Sws_DeviceInfo0" + stationtype + "] where RTUID in (select * from @onlineRtu) " +
                    @" union (select StationID from [dbo].[Sws_Station] where StationID in (select * from alarmIds)))," +
                    @"offline as(select base.*,'离线' as type from base
                      left join off1 on base.StationID=off1.StationID where off1.StationID is null),");
                                    sb.Append(@"r as (select * from online
                    union select * from alarm
                    union select * from offline) select * from r order by FocusOn desc ");

            }

            else if (type == "attention")
            {
                appends = getAttentionSByPage();
                sb.Append(appends);
                sqlcount = @"" + sb.ToString().Substring(0, sb.ToString().Length - 1) + "" +
                "select count(*) as totalcount from tt";
                sb.Append(@"data1 as (select tt.*,1 as FocusOn from tt ),");
                sb.Append(@"data2 as(select row_number() over(order by FocusOn desc) as number,* from data1),");

                sb.Append(@"base as (select * from data2 where number>(@pageIndex-1)*@pageSize and number<=@pageIndex*@pageSize),");

                sb.Append(@"online as (
                  select distinct base.*,'正常' as type from [dbo].[Sws_DeviceInfo0" + @stationtype + "] d " +
                 @" left join base on d.StationID=base.StationID where RTUID in (select * from @onlineRtu)  and  base.StationID not in (select * from alarmIds)  and base.StationID is not null),");
                sb.Append(@"alarm as (select *,'故障' as type from base  where StationID in (select * from alarmIds) ),");
                sb.Append(@"off1 as (select StationID from [dbo].[Sws_DeviceInfo0" + stationtype + "] where RTUID in (select * from @onlineRtu) " +
                    @" union (select StationID from [dbo].[Sws_Station] where StationID in (select * from alarmIds) ))," +
                    @"offline as(select base.*,'离线' as type from base
                  left join off1 on base.StationID=off1.StationID where off1.StationID is null),");
                                sb.Append(@"r as (select * from online
                union select * from alarm
                union select * from offline) select * from r order by StationName ");

            }
            else if (type == "online")
            {
                appends = getOnlineStationPage_tvp(stationtype);
                sb.Append(appends);
                sqlcount = @"" + sb.ToString().Substring(0, sb.ToString().Length - 1) + "" +
                "select count(*) as totalcount from tt";

                sb.Append(@"data1 as (select tt.*,case when FocusOn=1 then 1 else  0 end FocusOn from tt
                left join (select * from [dbo].[Sws_UserStation] where UserID=@UserID) u on  tt.StationID=u.StationID ),");
                sb.Append(@"data2 as(select row_number() over(order by FocusOn desc) as number,* from data1),");
                sb.Append(@"data as(select * from data2 where number>(@pageIndex-1)*@pageSize and number<=@pageIndex*@pageSize)");

                sb.Append(@"select data.StationID,data.StationNum,data.StationName,data.FocusOn,'正常' as type from data");
            }
            else if (type == "alarm")
            {
                appends = getAlarmSPage_tvp();
                sb.Append(appends);
                sqlcount = @"" + sb.ToString().Substring(0, sb.ToString().Length - 1) + "" +
                "select count(*) as totalcount from tt";

                sb.Append(@"data1 as (select tt.*,case when FocusOn=1 then 1 else  0 end FocusOn from tt
                left join (select * from [dbo].[Sws_UserStation] where UserID=@UserID) u on  tt.StationID=u.StationID ),");
                sb.Append(@"data2 as(select row_number() over(order by FocusOn desc) as number,* from data1),");
                sb.Append(@"data as(select * from data2 where number>(@pageIndex-1)*@pageSize and number<=@pageIndex*@pageSize)");
                sb.Append(@"select data.StationID,data.StationNum,data.StationName,data.FocusOn,'故障' as type from data");
            }
            else
            {
                appends = getOffStionPage_tvp(IsAdmin, stationtype);
                sb.Append(appends);
                sqlcount = @"" + sb.ToString().Substring(0, sb.ToString().Length - 1) + "" +
                "select count(*) as totalcount from tt";

                sb.Append(@"data1 as (select tt.*,case when FocusOn=1 then 1 else  0 end FocusOn from tt
                 left join (select * from [dbo].[Sws_UserStation] where UserID=@UserID) u on  tt.StationID=u.StationID ),");
                sb.Append(@"data2 as(select row_number() over(order by FocusOn desc) as number,* from data1),");
                sb.Append(@"data as(select * from data2 where number>(@pageIndex-1)*@pageSize and number<=@pageIndex*@pageSize)");

                sb.Append(@"
                 select data.StationID,data.StationNum,data.StationName,data.FocusOn,'离线' as type from data
                   ");
            }

            sql = sb.ToString();
            var query = this.Context.Database.SqlQuery_Dic(sql, sp);
            totalcount = this.Context.Database.SqlQuery<QueryNum>(sqlcount, sp)[0].totalcount;

            return query;
        }

        #endregion
        #region  数据监测获取泵房信息
        /// <summary>
        /// 数据监测获取泵房数据 （二供泵房）
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public IEnumerable<StationData> GetStationInfo(int userID, string stationName)
        {
            SysUser user = this.Find<SysUser>(userID);
            StringBuilder sb = new StringBuilder();
            stationName = stationName ?? "";
            SqlParameter[] sp = new SqlParameter[]
            {
                new SqlParameter("@userID",userID),
                new SqlParameter("@stationName","%"+stationName+"%")
            };

            string sql = @"select StationID,StationName from Sws_Station  where InType=1";
            sb.Append(sql);
            if (!string.IsNullOrEmpty(stationName))
            {
                sb.Append(" and StationName like @stationName ");
            }
            if (!user.IsAdmin)
            {
                sb.Append(@" and stationID in (select StationID from Sws_UserStation where UserID=@userID)");
            }


            var query = this.Context.Database.SqlQuery<StationData>(sb.ToString(), sp);
            return query;
        }
        #endregion
        #region 单吨能耗
        public IEnumerable<dynamic> GetStation_consumption(bool isadmin, int userid, string stationName, int innertype)
        {
            SqlParameter[] sp = new SqlParameter[] {
            new SqlParameter("@userid",userid),
            new SqlParameter("@stationName","%"+stationName+"%"),
            new SqlParameter("@innertype",innertype)
            };
            string sql = "";
            if (isadmin == true)
            {
                sql = @"with t as(  select distinct StationID  from [dbo].[Sws_DeviceInfo01])
                select s.StationID,StationName from [dbo].[Sws_Station] s
                left join t on s.StationID=t.StationID 
                where t.StationID is not null and InType=@innertype ";
            }
            else
            {
                sql = @"with t as(  select distinct StationID  from [dbo].[Sws_DeviceInfo01])
                select s.StationID,StationName from [dbo].[Sws_Station] s
                left join t on s.StationID=t.StationID 
                left join [dbo].[Sws_UserStation] u on s.StationID=u.StationID
                where t.StationID is not null and InType=@innertype  and u.UserID=@userid";
            }
            if (!string.IsNullOrEmpty(stationName))
            {
                sql += "and StationName like @stationName";
            }
            var query = this.Context.Database.SqlQuery_Dic(sql, sp).ToList();
            return query;
        }

        //日报数据查询 //已弃用   2021.09.10
        public IEnumerable<dynamic> GetConsumpBy_Day(string begindate, string enddate, int stationID, string tablename)
        {
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@stationID",stationID),
                new SqlParameter("@begindate",begindate),
                new SqlParameter("@enddate",enddate),
                new SqlParameter("@tablename",tablename)
        };
            string sql = @" select ID, UpdateTime, StationID, EnergyCon, FlowCon,case when  FlowCon is null or FlowCon=0 or  EnergyCon  is null then null
  else (EnergyCon/FlowCon) end consump from [dbo].[" + @tablename + "] where StationID=@stationID and UpdateTime>=@begindate  and UpdateTime<@enddate order by UpdateTime";
            //var query = this.Context.Database.SqlQuery<ConsumpInfo>(sql, sp).ToList();
            var query = this.Context.Database.SqlQuery_Dic(sql, sp).ToList();
            return query;
        }
        //月，年报数据查询 //已弃用   2021.09.10
        public IEnumerable<dynamic> GetConsumpBy_MY(string begindate, string enddate, int stationID, string tablename)
        {
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@stationID",stationID),
                new SqlParameter("@begindate",begindate),
                new SqlParameter("@enddate",enddate),
                 new SqlParameter("@tablename",tablename)
             };
            string sql = @"with t as(SELECT StationID, UpdateTime, DataKey, DataValue
      FROM [dbo].[" + @tablename + "] where StationID=@stationID and UpdateTime>=@begindate and UpdateTime<@enddate and DataKey in ('EnergyCon','FlowCon'))," +
      @"r as(
	  select StationID,UpdateTime,EnergyCon,FlowCon  from t PIVOT(max(DataValue) FOR DataKey IN(EnergyCon,FlowCon)) as APVT)
	  select *,case when  FlowCon is null or FlowCon=0 or  EnergyCon  is null then null
      else (EnergyCon/FlowCon) end consump from r  order by UpdateTime";
            //var query = this.Context.Database.SqlQuery<ConsumpInfo>(sql, sp).ToList();
            var query = this.Context.Database.SqlQuery_Dic(sql, sp).ToList();
            return query;
        }


        //查询本月数据//已弃用   2021.09.10
        public IEnumerable<dynamic> GetConsumpBy_ThisMonth(string begindate, string enddate, int stationID)
        {
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@stationID",stationID),
                new SqlParameter("@begindate",begindate),
                new SqlParameter("@enddate",enddate)
             };
            string sql = @"with t as(SELECT StationID, UpdateTime, DataKey, DataValue
      FROM [dbo].[DayQuartZ01] where StationID=@stationID and UpdateTime>=@begindate and UpdateTime<@enddate and DataKey in ('EnergyCon','FlowCon'))," +
      @"r as(
	  select StationID,UpdateTime,EnergyCon,FlowCon  from t PIVOT(max(DataValue) FOR DataKey IN(EnergyCon,FlowCon)) as APVT),
      g as( select cast(@begindate  as datetime) as UpdateTime,StationID,sum(isnull(EnergyCon,0)) as EnergyCon," +
      @"sum(isnull(FlowCon, 0)) as FlowCon from r group by StationID)
      select *,case when  FlowCon is null or FlowCon=0 or  EnergyCon  is null then null
      else (EnergyCon/FlowCon) end consump from g
      ";
            //var query = this.Context.Database.SqlQuery<ConsumpInfo>(sql, sp).ToList();
            var query = this.Context.Database.SqlQuery_Dic(sql, sp).ToList();
            return query;

        }

        #region 单吨能耗数据查询（以设备为单位）
        public IEnumerable<dynamic> GetConDataByDeviceID(string begindate, string enddate, string deviceids, string tablename)
        {

            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@deviceids",deviceids),
                new SqlParameter("@begindate",begindate),
                new SqlParameter("@enddate",enddate),
                new SqlParameter("@tablename",tablename)
        };
            string sql = @" select UpdateTime, DeviceID, EnergyCon, FlowCon,case when  FlowCon is null or FlowCon=0 or  EnergyCon  is null then null
  else (EnergyCon/FlowCon) end consump from [dbo].[" + @tablename + "] where CHARINDEX(','+LTRIM(DeviceID)+',',','+@deviceids+',')>0 and UpdateTime>= @begindate  and UpdateTime<@enddate order by UpdateTime";

            var query = this.Context.Database.SqlQuery_Dic(sql, sp).ToList();
            return query;
        }

        public IEnumerable<dynamic> GetDeviceNameOfCon(int stationid)
        {
            var f_itemid = (int)Model.CustomizedClass.Enum.设备分区;
            SqlParameter[] sp = new SqlParameter[] {
            new SqlParameter("@stationid",stationid),
            new SqlParameter("@f_itemid",f_itemid)
            };

            //          string sql = @"  select d.DeviceID,ItemName as  deviceName,StationName from  [dbo].[Sws_DeviceInfo01] d 
            //left join [dbo].[Sws_Station] s on d.StationID=s.StationID
            //left join (select * from  [dbo].[Sys_DataItemDetail] where F_ItemId=@f_itemid and IsEnable=1) g on d.Partition=g.ItemValue
            //where   CHARINDEX(','+LTRIM(DeviceID)+',','," + @deviceid + ",')>0";
            string sql = @"  select d.DeviceID,ItemName as  deviceName,StationName from  [dbo].[Sws_DeviceInfo01] d 
  left join [dbo].[Sws_Station] s on d.StationID=s.StationID
  left join (select * from  [dbo].[Sys_DataItemDetail] where F_ItemId=@f_itemid and IsEnable=1) g on d.Partition=g.ItemValue
  where   d.StationID=@stationid and d.Partition<>6  order by d.Partition";
            var query = this.Context.Database.SqlQuery_Dic(sql, sp).ToList();
            return query;
        }
        public IEnumerable<dynamic> GetConDataByDeviceID_thisMonth(string begindate, string enddate, string deviceids)
        {
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@deviceids",deviceids),
                new SqlParameter("@begindate",begindate),
                new SqlParameter("@enddate",enddate)

        };
            string sql = @" with t as( select * from [dbo].[DDayQuartZ01] 
where CHARINDEX(','+LTRIM(DeviceID)+',',','+@deviceids+',' )>0 and UpdateTime>= @begindate  and UpdateTime<@enddate " +
  @" ),
 g as(
 select cast(@begindate as datetime) as UpdateTime,DeviceID,sum(isnull(EnergyCon,0)) as EnergyCon,sum(isnull(FlowCon, 0)) as FlowCon from t  group by DeviceID)" +
 @" select *,case when  FlowCon is null or FlowCon=0 or  EnergyCon  is null then null
      else (EnergyCon/FlowCon) end consump from g";
            var query = this.Context.Database.SqlQuery_Dic(sql, sp).ToList();
            return query;
        }
        #endregion
        #endregion
        #region 夜间流量
        //日报
        public IEnumerable<dynamic> GetNightFlow_Day(int innertype, string stationids, int pageindex, int pagesize, string pastbegin, string pastend, string thisbegin, string thisend, string dates, ref int totalcount)
        {
           
            var stationidList = new List<string>(stationids.Split(",")).ConvertAll(r => int.Parse(r));
            DataTable tvpDt = new DataTable();
            tvpDt.Columns.Add("RTUID", typeof(int));//RTUID 代表Stationid
            foreach (var item in stationidList)
            {
                tvpDt.Rows.Add(item);
            }
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@pageIndex",pageindex),
                new SqlParameter("@pageSize",pagesize),
                new SqlParameter("@innertype",innertype),
                new SqlParameter("@pastbegin",pastbegin),
                new SqlParameter("@pastend",pastend),
                new SqlParameter("@thisbegin",thisbegin),
                new SqlParameter("@thisend",thisend),
                new SqlParameter("@dates",dates),
                new SqlParameter("@stationIDTable", SqlDbType.Structured) { Value = tvpDt, TypeName = "OnlineRtuIDTVP" }
            };

            string sqlcount = "select count(*) as totalcount from [dbo].[Sws_Station] s" +
                " left join @stationIDTable temp on s.StationID=temp.RTUID  where  temp.RTUID is not null and InType=@innertype";

            string sql = @"with t as( select row_number() over(order by StationName) as number,StationID,StationName from [dbo].[Sws_Station] s
left join @stationIDTable temp on s.StationID=temp.RTUID
where temp.RTUID is not null  and InType=@innertype)," +
 @"tt as (select * from t where number>(@pageIndex-1)*@pageSize and number<=@pageIndex*@pageSize),
 r as(
 select UpdateTime,h.StationID,DataValue as FlowCon from [dbo].[HourQuartZ01] h
 left join tt on h.StationID=tt.StationID where UpdateTime>=@thisbegin  and UpdateTime<= @thisend  and DataKey='FlowCon')," +
 @"rr as(select datename(hour,UpdateTime) as UpdateTime,StationID,FlowCon from r),
 rpast as(
  select UpdateTime,h.StationID,DataValue as FlowCon from [dbo].[HourQuartZ01] h
 left join tt on h.StationID=tt.StationID where UpdateTime>= @pastbegin  and UpdateTime<= @pastend  and DataKey='FlowCon')," +
 @"m as(
 select max(FlowCon) as maxFlowCon,min(FlowCon) as minFlowCon, StationID from r group by StationID),
 mTime as(
select m.StationID,m.maxFlowCon,m.minFlowCon,min(r.UpdateTime) as maxTime,min(r1.UpdateTime) as minTime from m
left join r on m.StationID=r.StationID and m.maxFlowCon=r.FlowCon
left join r r1 on m.StationID=r.StationID and m.minFlowCon=r1.FlowCon
group by m.StationID,m.maxFlowCon,m.minFlowCon),
thisdata as(
select StationID,AVG(FlowCon) as thisAverge from r group by StationID),
pastdata as(
select StationID,AVG(FlowCon) as pastAverge from rpast group by StationID),
avgdata as(
select thisdata.StationID,thisAverge, pastAverge,case when pastAverge is null or pastAverge=0 then 0 else ((thisAverge-pastAverge)/pastAverge)*100 end circleRate from thisdata
left join pastdata on thisdata.StationID=pastdata.StationID),
data as(
 select StationID," + @dates + "  from rr PIVOT(max(FlowCon) FOR UpdateTime IN(" + @dates + ")) as APVT)" +
@" select tt.StationName,tt.StationID," + @dates + ",maxFlowCon,datename(hour,maxTime) as  maxTime,minFlowCon,datename(hour,minTime) as minTime,pastAverge,thisAverge,circleRate from tt " +
@" left  join data on tt.StationID=data.StationID
 left join mTime on data.StationID=mTime.StationID
 left join avgdata on data.StationID=avgdata.StationID
";
            var query = this.Context.Database.SqlQuery_Dic(sql, sp);
            totalcount = this.Context.Database.SqlQuery<QueryNum>(sqlcount, sp)[0].totalcount;
            return query;

        }
        //日报导出数据
        public IEnumerable<dynamic> ExportNightFlow_Day(int innertype, string stationids, string pastbegin, string pastend, string thisbegin, string thisend, string dates)
        {
            var stationidList = new List<string>(stationids.Split(",")).ConvertAll(r => int.Parse(r));
            DataTable tvpDt = new DataTable();
            tvpDt.Columns.Add("RTUID", typeof(int));//RTUID 代表Stationid
            foreach (var item in stationidList)
            {
                tvpDt.Rows.Add(item);
            }
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@innertype",innertype),
                new SqlParameter("@pastbegin",pastbegin),
                new SqlParameter("@pastend",pastend),
                new SqlParameter("@thisbegin",thisbegin),
                new SqlParameter("@thisend",thisend),
                new SqlParameter("@dates",dates),
                 new SqlParameter("@stationIDTable", SqlDbType.Structured) { Value = tvpDt, TypeName = "OnlineRtuIDTVP" }
            };
            string sql = @"with t as( select row_number() over(order by StationName) as number,StationID,StationName from [dbo].[Sws_Station] s
left join @stationIDTable temp on s.StationID=temp.RTUID
where temp.RTUID is not null  and InType=@innertype)," +
 @"tt as (select * from t ),
 r as(
 select UpdateTime,h.StationID,DataValue as FlowCon from [dbo].[HourQuartZ01] h
 left join tt on h.StationID=tt.StationID where UpdateTime>= @thisbegin  and UpdateTime<= @thisend and DataKey='FlowCon')," +
 @"rr as(select datename(hour,UpdateTime) as UpdateTime,StationID,FlowCon from r),
 rpast as(
  select UpdateTime,h.StationID,DataValue as FlowCon from [dbo].[HourQuartZ01] h
 left join tt on h.StationID=tt.StationID where UpdateTime>= @pastbegin and UpdateTime<= @pastend  and DataKey='FlowCon')," +
 @"m as(
 select max(FlowCon) as maxFlowCon,min(FlowCon) as minFlowCon, StationID from r group by StationID),
 mTime as(
select m.StationID,m.maxFlowCon,m.minFlowCon,min(r.UpdateTime) as maxTime,min(r1.UpdateTime) as minTime from m
left join r on m.StationID=r.StationID and m.maxFlowCon=r.FlowCon
left join r r1 on m.StationID=r.StationID and m.minFlowCon=r1.FlowCon
group by m.StationID,m.maxFlowCon,m.minFlowCon),
thisdata as(
select StationID,AVG(FlowCon) as thisAverge from r group by StationID),
pastdata as(
select StationID,AVG(FlowCon) as pastAverge from rpast group by StationID),
avgdata as(
select thisdata.StationID,thisAverge, pastAverge,case when pastAverge is null or pastAverge=0 then 0 else ((thisAverge-pastAverge)/pastAverge)*100 end circleRate from thisdata
left join pastdata on thisdata.StationID=pastdata.StationID),
data as(
 select StationID," + @dates + "  from rr PIVOT(max(FlowCon) FOR UpdateTime IN(" + @dates + ")) as APVT)" +
@" select tt.StationName,tt.StationID," + @dates + ",maxFlowCon,datename(hour,maxTime) as  maxTime,minFlowCon,datename(hour,minTime) as minTime,pastAverge,thisAverge,circleRate from tt " +
@" left  join data on tt.StationID=data.StationID
 left join mTime on data.StationID=mTime.StationID
 left join avgdata on data.StationID=avgdata.StationID order by StationName
";
            var query = this.Context.Database.SqlQuery_Dic(sql, sp);
            return query;
        }
        //月报数据查询
        public IEnumerable<dynamic> GetNightFlow_Month(int innertype, string stationids, int pageindex, int pagesize, string time1, string time2, string time3, int minhour, int maxhour, string dates, ref int totalcount)
        {
            var stationidList = new List<string>(stationids.Split(",")).ConvertAll(r => int.Parse(r));
            DataTable tvpDt = new DataTable();
            tvpDt.Columns.Add("RTUID", typeof(int));//RTUID 代表Stationid
            foreach (var item in stationidList)
            {
                tvpDt.Rows.Add(item);
            }
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@pageIndex",pageindex),
                new SqlParameter("@pageSize",pagesize),
                new SqlParameter("@innertype",innertype),
                new SqlParameter("@time1",time1),
                new SqlParameter("@time2",time2),
                new SqlParameter("@time3",time3),
                new SqlParameter("@minhour",minhour),
                new SqlParameter("@maxhour",maxhour),
                new SqlParameter("@dates",dates),
                new SqlParameter("@stationIDTable", SqlDbType.Structured) { Value = tvpDt, TypeName = "OnlineRtuIDTVP" }
            };
            string sqlcount = "select count(*) as totalcount from [dbo].[Sws_Station] s" +
                " left join @stationIDTable temp on s.StationID=temp.RTUID where temp.RTUID is not null  and InType=@innertype";

            string sql = @"with t as( select row_number() over(order by StationName) as number,StationID,StationName from [dbo].[Sws_Station] s
left join @stationIDTable temp on s.StationID=temp.RTUID
where temp.RTUID is not null and InType=@innertype)," +
@"tt as (select * from t where number>(@pageIndex-1)*@pageSize and number<=@pageIndex*@pageSize),
 base as(select UpdateTime,h.StationID,DataValue as FlowCon, datename(hour,UpdateTime) as hourdate, cast(datename(day,UpdateTime) as int) as daydate from [dbo].[HourQuartZ01] h
 left join tt on h.StationID=tt.StationID where UpdateTime>= @time1  and UpdateTime< @time3  and DataKey='FlowCon')," +
 @"r as(
 select StationID,sum(FlowCon) as FlowCon,daydate from base  where UpdateTime>= @time2  and UpdateTime< @time3  and hourdate>=@minhour and hourdate<=@maxhour  group by  daydate,StationID)," +
 @"rpast as(select StationID,sum(FlowCon) as FlowCon,daydate from base  where  UpdateTime>=  @time1  and UpdateTime< @time2  and hourdate>= @minhour and hourdate<= @maxhour  group by  daydate,StationID" +
 @"),
 m as(
 select max(FlowCon) as maxFlowCon,min(FlowCon) as minFlowCon, StationID from r group by StationID),
 mTime as(
select m.StationID,m.maxFlowCon,m.minFlowCon,min(r.daydate) as maxTime,min(r1.daydate) as minTime from m
left join r on m.StationID=r.StationID and m.maxFlowCon=r.FlowCon
left join r r1 on m.StationID=r.StationID and m.minFlowCon=r1.FlowCon
group by m.StationID,m.maxFlowCon,m.minFlowCon),
thisdata as(
select StationID,AVG(FlowCon) as thisAverge from r group by StationID),
pastdata as(
select StationID,AVG(FlowCon) as pastAverge from rpast group by StationID),
avgdata as(
select thisdata.StationID,thisAverge, pastAverge,case when pastAverge is null or pastAverge=0 then 0 else ((thisAverge-pastAverge)/pastAverge)*100 end circleRate from thisdata
left join pastdata on thisdata.StationID=pastdata.StationID),
data as(
 select StationID," + @dates + "  from r PIVOT(max(FlowCon) FOR daydate IN(" + @dates + ")) as APVT)" +
 @"select tt.StationName,tt.StationID," + @dates + ", maxFlowCon, maxTime,minFlowCon,minTime,pastAverge,thisAverge,circleRate from tt " +
@" left  join data on tt.StationID=data.StationID
 left join mTime on data.StationID=mTime.StationID
 left join avgdata on data.StationID=avgdata.StationID
 ";
            var query = this.Context.Database.SqlQuery_Dic(sql, sp);
            totalcount = this.Context.Database.SqlQuery<QueryNum>(sqlcount, sp)[0].totalcount;
            return query;
        }
        //月报导出
        public IEnumerable<dynamic> ExportNightFlow_Month(int innertype, string stationids, string time1, string time2, string time3, int minhour, int maxhour, string dates)
        {
            var stationidList = new List<string>(stationids.Split(",")).ConvertAll(r => int.Parse(r));
            DataTable tvpDt = new DataTable();
            tvpDt.Columns.Add("RTUID", typeof(int));//RTUID 代表Stationid
            foreach (var item in stationidList)
            {
                tvpDt.Rows.Add(item);
            }
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@innertype",innertype),
                new SqlParameter("@time1",time1),
                new SqlParameter("@time2",time2),
                new SqlParameter("@time3",time3),
                new SqlParameter("@minhour",minhour),
                new SqlParameter("@maxhour",maxhour),
                new SqlParameter("@dates",dates),
                new SqlParameter("@stationIDTable", SqlDbType.Structured) { Value = tvpDt, TypeName = "OnlineRtuIDTVP" }
            };
            string sql = @"with t as( select row_number() over(order by StationName) as number,StationID,StationName from [dbo].[Sws_Station] s
left join @stationIDTable temp on s.StationID=temp.RTUID
where temp.RTUID is not null  and InType=@innertype)," +
@"tt as (select * from t),
 base as(select UpdateTime,h.StationID,DataValue as FlowCon, datename(hour,UpdateTime) as hourdate, cast(datename(day,UpdateTime) as int) as daydate from [dbo].[HourQuartZ01] h
 left join tt on h.StationID=tt.StationID where UpdateTime>=@time1  and UpdateTime< @time3  and DataKey='FlowCon')," +
 @"r as(
 select StationID,sum(FlowCon) as FlowCon,daydate from base  where UpdateTime>= @time2  and UpdateTime< @time3  and hourdate>=@minhour and hourdate<=@maxhour  group by  daydate,StationID)," +
 @"rpast as(select StationID,sum(FlowCon) as FlowCon,daydate from base  where UpdateTime>=  @time1  and UpdateTime< @time2  and hourdate>= @minhour and hourdate<= @maxhour  group by  daydate,StationID" +
 @"),
 m as(
 select max(FlowCon) as maxFlowCon,min(FlowCon) as minFlowCon, StationID from r group by StationID),
 mTime as(
select m.StationID,m.maxFlowCon,m.minFlowCon,min(r.daydate) as maxTime,min(r1.daydate) as minTime from m
left join r on m.StationID=r.StationID and m.maxFlowCon=r.FlowCon
left join r r1 on m.StationID=r.StationID and m.minFlowCon=r1.FlowCon
group by m.StationID,m.maxFlowCon,m.minFlowCon),
thisdata as(
select StationID,AVG(FlowCon) as thisAverge from r group by StationID),
pastdata as(
select StationID,AVG(FlowCon) as pastAverge from rpast group by StationID),
avgdata as(
select thisdata.StationID,thisAverge, pastAverge,case when pastAverge is null or pastAverge=0 then 0 else ((thisAverge-pastAverge)/pastAverge)*100 end circleRate from thisdata
left join pastdata on thisdata.StationID=pastdata.StationID),
data as(
 select StationID," + @dates + "  from r PIVOT(max(FlowCon) FOR daydate IN(" + @dates + ")) as APVT)" +
 @"select tt.StationName,tt.StationID," + @dates + ", maxFlowCon, maxTime,minFlowCon,minTime,pastAverge,thisAverge,circleRate from tt " +
@" left  join data on tt.StationID=data.StationID
 left join mTime on data.StationID=mTime.StationID
 left join avgdata on data.StationID=avgdata.StationID order by StationName
 ";
            var query = this.Context.Database.SqlQuery_Dic(sql, sp);
            return query;
        }
        //年报数据查询
        public IEnumerable<dynamic> GetNightFlow_Year(int innertype, string stationids, int pageindex, int pagesize, string time1, string time2, string time3, int minhour, int maxhour, string dates, ref int totalcount)
        {
            var stationidList = new List<string>(stationids.Split(",")).ConvertAll(r => int.Parse(r));
            DataTable tvpDt = new DataTable();
            tvpDt.Columns.Add("RTUID", typeof(int));//RTUID 代表Stationid
            foreach (var item in stationidList)
            {
                tvpDt.Rows.Add(item);
            }
            
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@pageIndex",pageindex),
                new SqlParameter("@pageSize",pagesize),
                new SqlParameter("@innertype",innertype),
                new SqlParameter("@time1",time1),
                new SqlParameter("@time2",time2),
                new SqlParameter("@time3",time3),
                new SqlParameter("@minhour",minhour),
                new SqlParameter("@maxhour",maxhour),
                new SqlParameter("@dates",dates),
                new SqlParameter("@stationIDTable", SqlDbType.Structured) { Value = tvpDt, TypeName = "OnlineRtuIDTVP" }
            };
            string sqlcount = @"select count(*) as totalcount from [dbo].[Sws_Station] s
left join @stationIDTable temp on s.StationID=temp.RTUID
where temp.RTUID is not null  and InType=@innertype";

            string sql = @"with t as( select row_number() over(order by StationName) as number,StationID,StationName from [dbo].[Sws_Station] s
left join @stationIDTable temp on s.StationID=temp.RTUID
where temp.RTUID is not null  and InType=@innertype)," +
@"tt as (select * from t where number>(@pageIndex-1)*@pageSize and number<=@pageIndex*@pageSize),
 base as(select UpdateTime,h.StationID,DataValue as FlowCon,  datename(hour,UpdateTime) as hourdate, cast(datename(month,UpdateTime) as int) as monthdate from [dbo].[HourQuartZ01] h
 left join tt on h.StationID=tt.StationID where UpdateTime>= @time1  and UpdateTime< @time3  and DataKey='FlowCon')," +
 @"r as(
 select StationID,sum(FlowCon) as FlowCon,monthdate from base  where UpdateTime>= @time2  and UpdateTime< @time3  and hourdate>@minhour and hourdate<@maxhour  group by  monthdate,StationID)," +
 @"rpast as(select StationID,sum(FlowCon) as FlowCon,monthdate from base  where UpdateTime>=  @time1  and UpdateTime< @time2  and hourdate>@minhour and hourdate<@maxhour  group by  monthdate,StationID" +
 @"),
 m as(
 select max(FlowCon) as maxFlowCon,min(FlowCon) as minFlowCon, StationID from r group by StationID),
 mTime as(
select m.StationID,m.maxFlowCon,m.minFlowCon,min(r.monthdate) as maxTime,min(r1.monthdate) as minTime from m
left join r on m.StationID=r.StationID and m.maxFlowCon=r.FlowCon
left join r r1 on m.StationID=r.StationID and m.minFlowCon=r1.FlowCon
group by m.StationID,m.maxFlowCon,m.minFlowCon),
thisdata as(
select StationID,AVG(FlowCon) as thisAverge from r group by StationID),
pastdata as(
select StationID,AVG(FlowCon) as pastAverge from rpast group by StationID),
avgdata as(
select thisdata.StationID,thisAverge, pastAverge,case when pastAverge is null or pastAverge=0 then 0 else ((thisAverge-pastAverge)/pastAverge)*100 end circleRate from thisdata
left join pastdata on thisdata.StationID=pastdata.StationID),
data as(
 select StationID," + @dates + "  from r PIVOT(max(FlowCon) FOR monthdate IN(" + @dates + ")) as APVT)" +
 @"select tt.StationName,tt.StationID," + @dates + ", maxFlowCon, maxTime,minFlowCon,minTime,pastAverge,thisAverge,circleRate from tt " +
 @"left  join data on tt.StationID=data.StationID
 left join mTime on data.StationID=mTime.StationID
 left join avgdata on data.StationID=avgdata.StationID 
 ";
            var query = this.Context.Database.SqlQuery_Dic(sql, sp);
            totalcount = this.Context.Database.SqlQuery<QueryNum>(sqlcount, sp)[0].totalcount;
            return query;
        }
        //年报导出数据查询
        public IEnumerable<dynamic> ExportNightFlow_Year(int innertype, string stationids, string time1, string time2, string time3, int minhour, int maxhour, string dates)
        {
            var stationidList = new List<string>(stationids.Split(",")).ConvertAll(r => int.Parse(r));
            DataTable tvpDt = new DataTable();
            tvpDt.Columns.Add("RTUID", typeof(int));//RTUID 代表Stationid
            foreach (var item in stationidList)
            {
                tvpDt.Rows.Add(item);
            }
            SqlParameter[] sp = new SqlParameter[] {

                new SqlParameter("@innertype",innertype),
                new SqlParameter("@time1",time1),
                new SqlParameter("@time2",time2),
                new SqlParameter("@time3",time3),
                new SqlParameter("@minhour",minhour),
                new SqlParameter("@maxhour",maxhour),
                new SqlParameter("@dates",dates),
                new SqlParameter("@stationIDTable", SqlDbType.Structured) { Value = tvpDt, TypeName = "OnlineRtuIDTVP" }
            };
            string sql = @"with t as( select row_number() over(order by StationName) as number,StationID,StationName from [dbo].[Sws_Station] s
left join @stationIDTable temp on s.StationID=temp.RTUID
where temp.RTUID is not null  and InType=@innertype)," +
@"tt as (select * from t ),
 base as(select UpdateTime,h.StationID,DataValue as FlowCon,  datename(hour,UpdateTime) as hourdate, cast(datename(month,UpdateTime) as int) as monthdate from [dbo].[HourQuartZ01] h
 left join tt on h.StationID=tt.StationID where UpdateTime>= @time1  and UpdateTime< @time3  and DataKey='FlowCon')," +
 @"r as(
 select StationID,sum(FlowCon) as FlowCon,monthdate from base  where UpdateTime>= @time2  and UpdateTime< @time3  and hourdate>@minhour and hourdate<@maxhour  group by  monthdate,StationID)," +
 @"rpast as(select StationID,sum(FlowCon) as FlowCon,monthdate from base  where UpdateTime>=  @time1  and UpdateTime< @time2  and hourdate>@minhour and hourdate<@maxhour  group by  monthdate,StationID" +
 @"),
 m as(
 select max(FlowCon) as maxFlowCon,min(FlowCon) as minFlowCon, StationID from r group by StationID),
 mTime as(
select m.StationID,m.maxFlowCon,m.minFlowCon,min(r.monthdate) as maxTime,min(r1.monthdate) as minTime from m
left join r on m.StationID=r.StationID and m.maxFlowCon=r.FlowCon
left join r r1 on m.StationID=r.StationID and m.minFlowCon=r1.FlowCon
group by m.StationID,m.maxFlowCon,m.minFlowCon),
thisdata as(
select StationID,AVG(FlowCon) as thisAverge from r group by StationID),
pastdata as(
select StationID,AVG(FlowCon) as pastAverge from rpast group by StationID),
avgdata as(
select thisdata.StationID,thisAverge, pastAverge,case when pastAverge is null or pastAverge=0 then 0 else ((thisAverge-pastAverge)/pastAverge)*100 end circleRate from thisdata
left join pastdata on thisdata.StationID=pastdata.StationID),
data as(
 select StationID," + @dates + "  from r PIVOT(max(FlowCon) FOR monthdate IN(" + @dates + ")) as APVT)" +
 @"select tt.StationName,tt.StationID," + @dates + ", maxFlowCon, maxTime,minFlowCon,minTime,pastAverge,thisAverge,circleRate from tt " +
 @"left  join data on tt.StationID=data.StationID
 left join mTime on data.StationID=mTime.StationID
 left join avgdata on data.StationID=avgdata.StationID order by StationName
 ";
            var query = this.Context.Database.SqlQuery_Dic(sql, sp);

            return query;
        }

        #endregion
        #region 直饮水数据监测
        //左侧树查询（泵房+设备）//已弃用   2021.09.10
        public IEnumerable<dynamic> GetZDataMonitoringTree(int F_ItemId, int userid, bool isadmin, string searchtext)
        {
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@F_ItemId",F_ItemId),
                new SqlParameter("@userid",userid),
                new SqlParameter("@searchtext","%"+searchtext+"%")
            };
            StringBuilder sb = new StringBuilder();
            sb.Append(@"with t as (
  select distinct StationID  from [dbo].[Sws_DeviceInfo02] where RTUID is not null),");
            if (isadmin)
            {
                sb.Append(@"ss as(
  select s.StationID as id,StationName as name,0 as pid,1 as type,0 as Partition,0 as RTUID from [dbo].[Sws_Station] s 
  left join t on s.StationID=t.StationID where 
  t.StationID is not null and StationName like  @searchtext ),");
            }
            else
            {
                sb.Append(@" ss as(
  select s.StationID as id,StationName as name,0 as pid,1 as type,0 as Partition,0 as RTUID from [dbo].[Sws_Station] s 
  left join t on s.StationID=t.StationID
  left join [dbo].[Sws_UserStation] u on s.StationID=u.StationID where 
  t.StationID is not null and StationName like  @searchtext  and UserID=@userid),");
            }
            sb.Append(@" dd as(
  select d.DeviceID as id,(name+'#'+ItemName) as name,d.StationID as pid,2 as type,d.Partition,d.RTUID from [dbo].[Sws_DeviceInfo02] d
  left join ss on d.StationID=ss.id 
  left join [dbo].[Sys_DataItemDetail] e on d.Partition=e.ItemValue
  where ss.id  is not null and d.RTUID is not null and F_ItemId=@F_ItemId)
  select * from ss union select* from dd");

            var query = this.Context.Database.SqlQuery_Dic(sb.ToString(), sp).ToList();
            return query;
        }
        #endregion

        #region 地图监测数据
        #region 二供泵房数据监测
        //泵房总数、在线数量 //已弃用   2021.09.10
        public IEnumerable<dynamic> GetSationNumMap(string alarmSationids, string onlineRtuid, int userid, bool isadmin)
        {
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@userid",userid),
                new SqlParameter("@alarmSationids",alarmSationids),
                new SqlParameter("@onlineRtuid",onlineRtuid)
            };
            StringBuilder builder = new StringBuilder();
            builder.Append("with t as (select * from [dbo].[Sws_Station]),");


            if (isadmin == true)//泵房总数
            {
                //存在设备通讯的泵房
                //                builder.Append(@"alls as (select count(*) as allNum from(
                //select distinct t.* from[dbo].[Sws_DeviceInfo01] d
                //left join t on d.StationID = t.StationID where t.StationID is not null) as alla1),");
                //所有泵房
                builder.Append(@"alls as (select count(*) as allNum from(
                select distinct t.* from t
                ) as alla1),");
            }
            else
            {
                //存在设备的泵房
                //                builder.Append(@"alls as (select count(*) as allNum from(
                //select distinct t.* from [dbo].[Sws_DeviceInfo01] d
                //left join t on d.StationID=t.StationID
                //left join [dbo].[Sws_UserStation] u on t.StationID=u.StationID where u.UserID=@userid and t.StationID is not null) as alla1),");

                builder.Append(@"alls as (select count(*) as allNum from(
select distinct a.* from t a 
left join [dbo].[Sws_UserStation] u on a.StationID=u.StationID where u.UserID=@userid and a.StationID is not null) as alla1),");
            }
            //报警泵房数量
            builder.Append(@"alarms as(select count(*) as alarmNum  from t where   CHARINDEX(','+LTRIM(StationID)+',',@alarmSationids)>0),");
            //二供在线
            builder.Append(@"onlines as (select count(*) as onlineNum from(
select distinct t.* from [dbo].[Sws_DeviceInfo01] d
left join t on d.StationID=t.StationID
where t.StationID is not null and  CHARINDEX(','+LTRIM(RTUID)+',',@onlineRtuid )>0  and not  CHARINDEX(','+LTRIM(t.StationID)+',', @alarmSationids )>0) as g),");

            //直饮水在线
            builder.Append(@"onlines1 as (select count(*) as onlineNum1 from(
select distinct t.* from [dbo].[Sws_DeviceInfo02] d
left join t on d.StationID=t.StationID
where t.StationID is not null and  CHARINDEX(','+LTRIM(RTUID)+',',@onlineRtuid )>0  and not  CHARINDEX(','+LTRIM(t.StationID)+',',@alarmSationids)>0) as g)");
            //            //关注
            //            builder.Append(@" attentions as(select count (*) as attentionNum from( select t.* from t
            //left join [dbo].[Sws_UserStation] u on t.StationID=u.StationID
            //where UserID=@userid and FocusOn=1 and u.StationID is not null) as g)");
            builder.Append("select allNum,alarmNum,onlineNum,onlineNum1 from alls,alarms,onlines,onlines1");
            var sql = builder.ToString();
            var query = this.Context.Database.SqlQuery_Dic(sql, sp).ToList();
            return query;
        }
        //报警泵房ID
        public IEnumerable<dynamic> GetAlarmStionMap(bool isadmin, int userID)
        {
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@userID",userID)
          };
            string sql = "";
            if (isadmin == true)
            {
                sql = @"WITH  t as (SELECT StationID from [dbo].[Sws_DeviceInfo01] where RTUID in (select RTUID from [dbo].[Sws_EventInfo] where EventLevel!=0))
  select distinct s.StationID from [dbo].[Sws_Station] s
  left join t on t.StationID=s.StationID
  where t.StationID is not null";
            }
            else
            {
                sql = @"  WITH  t as (SELECT StationID from [dbo].[Sws_DeviceInfo01] where RTUID in (select RTUID from [dbo].[Sws_EventInfo] where EventLevel!=0)),
a as (SELECT StationID from [dbo].[Sws_DeviceInfo02] where RTUID in (select RTUID from [dbo].[Sws_EventInfo] where EventLevel!=0))
  select distinct s.StationID from [dbo].[Sws_Station] s
  left join  [dbo].[Sws_UserStation] u on u.StationID=s.StationID
  left join t on t.StationID=s.StationID
  left join a on a.StationID=s.StationID
  where t.StationID is not null and u.UserID=@userID";
            }
            var query = this.Context.Database.SqlQuery_Dic(sql, sp).ToList();
            return query;
        }
        //根据用户id来查询rtuid
        public IEnumerable<dynamic> GetRtuIDByUserIDMap(int userid)
        {
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@userID",userid)
           };
            string sql = @"  
                select distinct RTUID  from [dbo].[Sws_DeviceInfo01] where StationID in
(select u.StationID from [dbo].[Sws_UserStation] u 
  left join [dbo].[Sws_Station] s on u.StationID=s.StationID where UserID=@userID ) and RTUID is not null
  union all(select distinct RTUID  from [dbo].[Sws_DeviceInfo02] where StationID in
(select distinct u.StationID from [dbo].[Sws_UserStation] u 
  left join [dbo].[Sws_Station] s on u.StationID=s.StationID where UserID=@userID  ) and RTUID is not null)";
            var query = this.Context.Database.SqlQuery_Dic(sql, sp).ToList();
            return query;
        }
        //查询机组数量 
        public IEnumerable<dynamic> GetDeviceNumMap(bool isadmin, int userID)
        {
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@userID",userID)
          };
            string sql = "";
            if (isadmin == true)
            {
                sql = @"with t as (select count(*) as ty1 from Sws_DeviceInfo01 d ),
a as (select count(*) as ty2 from Sws_DeviceInfo02 d )
select * from a,t";
            }
            else
            {
                sql = @"  with t as (select count(*) as ty1 from Sws_DeviceInfo01 d where d.StationID in (select StationID from Sws_UserStation where UserID = @userID)),
a as (select count(*) as ty2 from Sws_DeviceInfo02 d where d.StationID in (select StationID from Sws_UserStation where UserID = @userID))
select * from a,t";
            }
            var query = this.Context.Database.SqlQuery_Dic(sql, sp).ToList();
            return query;
        }
        //获取泵房以及设备数据地图//已弃用   2021.09.10
        public IEnumerable<dynamic> GetStaionDataMap(string type, string onlineRtuid, string alarmSids, bool IsAdmin, long UserID, string stationName)
        {
            string sql = "";
            stationName = stationName ?? "";
            SqlParameter[] sp = sp = new SqlParameter[] {
                    new SqlParameter("@UserID",UserID),
                    new SqlParameter("@onlineRtuid",onlineRtuid),
                    new SqlParameter("@alarmSids",alarmSids),
                    new SqlParameter("@stationName","%"+stationName+"%")
                };
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(stationName))
            {
                sb.Append("with t1 as (select distinct StationID from [dbo].[Sws_Station] where (InType =1 or InType = 0) and StationName like  @stationName ),t as (select s.* from [dbo].[Sws_Station] s" +
               " left join t1 on s.StationID=t1.StationID where t1.StationID is not null ),");
            }
            else
            {
                sb.Append("with t1 as (select distinct StationID from [dbo].[Sws_Station] where InType =1 or InType = 0),t as (select s.* from [dbo].[Sws_Station] s" +
               " left join t1 on s.StationID=t1.StationID where t1.StationID is not null ),");
            }


            string appends = "";
            if (type == "all")
            {
                StringBuilder s = new StringBuilder();

                if (IsAdmin == true)
                {
                    s.Append("tt as (select row_number() over(order by StationName) as number ,* from t ),");
                }
                else
                {
                    s.Append("tt as (select row_number() over(order by StationName) as number ,t.* from [dbo].[Sws_UserStation] u " +
                        "left join t on t.StationID=u.StationID where UserID=@UserID and t.StationID is not null),");
                }

                appends = s.ToString();

                //appends = getAllStationByPage(IsAdmin);
                sb.Append(appends);
            }

            sb.Append(@"data1 as(select * from tt ),
data as( select data1.*,case when FocusOn=1 then 1 else 0 end FocusOn  from data1 
  left join(select * from[dbo].[Sws_UserStation] where UserID =@UserID) u on data1.StationID = u.StationID)
 select data.StationID,data.StationNum,data.StationName,data.FocusOn,d.DeviceID,d.DeviceName,d.RTUID,Partition,Lat,Lng,Frequency,PumpNum,ControlMonitor,CameraMonitor from data
  left join [dbo].[Sws_DeviceInfo01] d on data.StationID = d.StationID  ");
            sql = sb.ToString();
            var query = this.Context.Database.SqlQuery_Dic(sql, sp);

            return query;
        }
        //查询泵房下的RTUID
        public IEnumerable<dynamic> GetStationRtuid(int id)
        {
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@id",id)
          };
            string sql = @" (select distinct d.RtuID from Sws_DeviceInfo01 d where d.StationID = @id  and RtuID is not null)
union (select distinct d.RtuID from Sws_DeviceInfo02 d where d.StationID = @id  and RtuID is not null)";
            var query = this.Context.Database.SqlQuery_Dic(sql, sp).ToList();
            return query;
        }
        //查询泵房设备信息
        public IEnumerable<dynamic> GetStaionById(string type, int stationId)
        {
            string sql = "";
            SqlParameter[] sp = sp = new SqlParameter[] {
                    new SqlParameter("@stationId",stationId),
                };
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(type))
            {
                sb.Append("with t1 as (select distinct StationID from [dbo].[Sws_Station] where StationID = @stationId),t as (select s.* from [dbo].[Sws_Station] s" +
                " left join t1 on s.StationID=t1.StationID where t1.StationID is not null ),");


                sb.Append("tt as (select  * from t ),");


                sb.Append(@"data1 as(select * from tt ),
data as( select data1.*,case when FocusOn=1 then 1 else 0 end FocusOn  from data1 
  left join(select * from[dbo].[Sws_UserStation]  ) u on data1.StationID = u.StationID)
 select distinct data.StationID,data.StationNum,data.StationName,d.DeviceID,d.DeviceName,d.RTUID,Partition,Lat,Lng from data
  left join [dbo].[Sws_DeviceInfo02] d on data.StationID = d.StationID  ");
            }
            else
            {
                sb.Append("with t1 as (select distinct StationID from [dbo].[Sws_Station] where StationID = @stationId),t as (select s.* from [dbo].[Sws_Station] s" +
                " left join t1 on s.StationID=t1.StationID where t1.StationID is not null ),");


                sb.Append("tt as (select  * from t ),");


                sb.Append(@"data1 as(select * from tt ),
data as( select data1.*,case when FocusOn=1 then 1 else 0 end FocusOn  from data1 
  left join(select * from[dbo].[Sws_UserStation]  ) u on data1.StationID = u.StationID)
 select distinct data.StationID,data.StationNum,data.StationName,d.DeviceID,d.DeviceName,d.RTUID,Partition,Lat,Lng,Frequency,PumpNum from data
  left join [dbo].[Sws_DeviceInfo01] d on data.StationID = d.StationID  ");
            }

            sql = sb.ToString();
            var query = this.Context.Database.SqlQuery_Dic(sql, sp);

            return query;
        }
        #endregion
        #region 直饮水泵房数据监测
        //获取直饮水泵房以及设备数据地图 //已弃用   2021.09.10
        public IEnumerable<dynamic> GetZStaionDataMap(string type, string onlineRtuid, string alarmSids, bool IsAdmin, long UserID, string stationName)
        {
            string sql = "";
            SqlParameter[] sp = sp = new SqlParameter[] {
                    new SqlParameter("@UserID",UserID),
                    new SqlParameter("@onlineRtuid",onlineRtuid),
                    new SqlParameter("@alarmSids",alarmSids),
                    new SqlParameter("@stationName","%"+stationName+"%")
                };
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(stationName))
            {
                sb.Append("with t1 as (select distinct StationID from [dbo].[Sws_Station] where InType =2 and StationName like   @stationName ),t as (select s.* from [dbo].[Sws_Station] s" +
               " left join t1 on s.StationID=t1.StationID where t1.StationID is not null and  InType =2),");

            }
            else
            {
                sb.Append("with t1 as (select distinct StationID from [dbo].[Sws_Station] where InType =2),t as (select s.* from [dbo].[Sws_Station] s" +
               " left join t1 on s.StationID=t1.StationID where t1.StationID is not null and  InType =2),");

            }

            string appends = "";
            if (type == "all")
            {
                StringBuilder s = new StringBuilder();

                if (IsAdmin == true)
                {
                    s.Append("tt as (select row_number() over(order by StationName) as number ,* from t ),");
                }
                else
                {
                    s.Append("tt as (select row_number() over(order by StationName) as number ,t.* from [dbo].[Sws_UserStation] u " +
                        "left join t on t.StationID=u.StationID where UserID=@UserID and t.StationID is not null),");
                }

                appends = s.ToString();

                //appends = getAllStationByPage(IsAdmin);
                sb.Append(appends);
            }

            sb.Append(@"data1 as(select * from tt ),
data as( select data1.*,case when FocusOn=1 then 1 else 0 end FocusOn  from data1 
  left join(select * from[dbo].[Sws_UserStation] where UserID =@UserID) u on data1.StationID = u.StationID)
 select data.StationID,data.StationNum,data.StationName,data.FocusOn,d.DeviceID,d.DeviceName,d.RTUID,Partition,Lat,Lng from data
  left join [dbo].[Sws_DeviceInfo02] d on data.StationID = d.StationID  ");
            sql = sb.ToString();
            var query = this.Context.Database.SqlQuery_Dic(sql, sp);

            return query;
        }
        //查询泵房设备信息
        public IEnumerable<dynamic> GetZStaionById(string type, int stationId)
        {
            string sql = "";
            SqlParameter[] sp = sp = new SqlParameter[] {
                    new SqlParameter("@stationId",stationId),
                };
            StringBuilder sb = new StringBuilder();

            sb.Append("with t1 as (select distinct StationID from [dbo].[Sws_Station] where StationID = @stationId),t as (select s.* from [dbo].[Sws_Station] s" +
                " left join t1 on s.StationID=t1.StationID where t1.StationID is not null ),");


            sb.Append("tt as (select  * from t ),");


            sb.Append(@"data1 as(select * from tt ),
data as( select data1.*,case when FocusOn=1 then 1 else 0 end FocusOn  from data1 
  left join(select * from[dbo].[Sws_UserStation]  ) u on data1.StationID = u.StationID)
 select distinct data.StationID,data.StationNum,data.StationName,d.DeviceID,d.DeviceName,d.RTUID,Partition,Lat,Lng from data
  left join [dbo].[Sws_DeviceInfo02] d on data.StationID = d.StationID  ");
            sql = sb.ToString();
            var query = this.Context.Database.SqlQuery_Dic(sql, sp);

            return query;
        }
        #endregion
        #region 查询二供泵房和直饮水泵房的报警
        //查询二供泵房和直饮水泵房的报警
        public IEnumerable<dynamic> GetAllAlarmStionMap(bool isadmin, int userID)
        {
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@userID",userID)
          };
            string sql = "";
            if (isadmin == true)
            {
                sql = @"WITH  t as (select StationID from [dbo].[Sws_DeviceInfo01] where StationID is not null and RTUID in (select RTUID from [dbo].[Sws_EventInfo] where EventLevel!=0)
 union select StationID from [dbo].[Sws_DeviceInfo02] where StationID is not null and RTUID in (select RTUID from [dbo].[Sws_EventInfo] where EventLevel!=0))
  select distinct s.StationID from [dbo].[Sws_Station] s
  left join t on t.StationID=s.StationID
  where t.StationID is not null";
            }
            else
            {
                sql = @"  WITH  t as ( select StationID from [dbo].[Sws_DeviceInfo01] where StationID is not null and RTUID in (select RTUID from [dbo].[Sws_EventInfo] where EventLevel!=0)
 union select StationID from [dbo].[Sws_DeviceInfo02] where StationID is not null and RTUID in (select RTUID from [dbo].[Sws_EventInfo] where EventLevel!=0))
  select distinct s.StationID from [dbo].[Sws_Station] s
  left join t on t.StationID=s.StationID
  left join Sws_UserStation su on su.StationID = t.StationID
  where t.StationID is not null and su.UserID = @userID";
            }
            var query = this.Context.Database.SqlQuery_Dic(sql, sp).ToList();
            return query;
        }
        #endregion
        #endregion

        #region 获取所有泵房及设备地图信息  不区分直饮水 二供
        //获取泵房以及设备数据地图 不区分直饮水 二供//已弃用   2021.09.10
        public IEnumerable<dynamic> GetStaionAllDataMap(string onlineRtuid, string alarmSids, bool IsAdmin, long UserID, string stationName)
        {
            string sql = "";
            SqlParameter[] sp = new SqlParameter[] {
                    new SqlParameter("@UserID",UserID),
                    new SqlParameter("@onlineRtuid",onlineRtuid),
                    new SqlParameter("@alarmSids",alarmSids),
                    new SqlParameter("@stationName","%"+stationName+"%")
                };
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(stationName))
            {
                sb.Append("with t1 as (select distinct StationID from [dbo].[Sws_Station] where StationName like @stationName  or InType = 0),t as (select s.* from [dbo].[Sws_Station] s" +
               " left join t1 on s.StationID=t1.StationID where t1.StationID is not null ),");
            }
            else
            {
                sb.Append("with t1 as (select distinct StationID from [dbo].[Sws_Station]),t as (select s.* from [dbo].[Sws_Station] s" +
               " left join t1 on s.StationID=t1.StationID where t1.StationID is not null ),");
            }



            if (IsAdmin == true)
            {
                sb.Append("tt as (select row_number() over(order by StationName) as number ,* from t ),");
            }
            else
            {
                sb.Append("tt as (select row_number() over(order by StationName) as number ,t.* from [dbo].[Sws_UserStation] u " +
                        "left join t on t.StationID=u.StationID where UserID=@UserID and t.StationID is not null),");
            }

            sb.Append(@"data1 as(select * from tt ),
  data as( select data1.*  from data1 
  left join(select * from[dbo].[Sws_UserStation] where UserID =@UserID) u on data1.StationID = u.StationID)
 select data.StationID,data.StationNum,data.StationName,data.FocusOn,d.DeviceID,d.DeviceName,d.RTUID,d.Partition,Lat,Lng,d.Frequency,d.PumpNum,d1.DeviceID as zDeviceID,d1.DeviceName as zDeviceName,d1.RTUID as zRTUID,d1.Partition as zPartition from data
  left join [dbo].[Sws_DeviceInfo01] d on data.StationID = d.StationID
 left join [dbo].[Sws_DeviceInfo02] d1 on data.StationID = d1.StationID 
");
            sql = sb.ToString();
            var query = this.Context.Database.SqlQuery_Dic(sql, sp);

            return query;
        }
        //查询泵房下的RTUID
        #endregion
        #region 泵房接入
        #region 门禁接入
        public IEnumerable<dynamic> QueryStationAccessTable(int pageindex, int pagesize, int F_itemid, string order, string filter, ref int Totalcount)
        {
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@f_itemID",F_itemid),
                new SqlParameter("@startindex",pageindex),
                new SqlParameter("@pageSize",pagesize),
                new SqlParameter("@orderItems",order),
                new SqlParameter("@filterString",filter),
                new SqlParameter("@Totalcount",Totalcount){Direction=System.Data.ParameterDirection.Output,DbType=System.Data.DbType.Int32}
            };
            var query = this.Context.Database.SqlQuery_Dic("exec QueryStationAccessTable @f_itemID,@startindex,@pageSize,@orderItems,@filterString,@Totalcount Output", sp).ToList();

            Totalcount = Convert.ToInt32(sp[5].Value);
            return query;
        }
        public IEnumerable<dynamic> GetAccessByStationid(int pageindex, int pagesize, int stationid, int brand_itemid, string search, ref int Totalcount)
        {
            search = search ?? "";
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@pageindex",pageindex),
                new SqlParameter("@pagesize",pagesize),
                new SqlParameter("@stationid",stationid),
                new SqlParameter("@brand_itemid",brand_itemid),
                new SqlParameter("@search","%"+search+"%"),
           };
            StringBuilder sb = new StringBuilder();
            sb.Append(@"with t as ( select *,case when StationID=0 then 0 else 1 end State from [dbo].[Sws_AccessControl] where (StationID=@stationid or StationID=0)");
            if (!string.IsNullOrEmpty(search))
            {
                sp.Append(new SqlParameter("@search", search));
                sb.Append(@" and (Num like  @search  or AccessName like  @search ))");
            }
            else
            {
                sb.Append(")");
            }
            string sqlcount = sb.ToString() + "select count(*) as totalcount from t";
            Totalcount = this.Context.Database.SqlQuery<QueryNum>(sqlcount, sp)[0].totalcount;
            sb.Append(@",r as(
 select Row_number() over(order by State desc) as rownumber, * from t ),
 base as(
 select * from r where rownumber>(@pageindex-1)*@pagesize and rownumber<=@pageindex*@pagesize)
 select base.*,g.ItemName as brandName from base
 left join (select * from [dbo].[Sys_DataItemDetail] where F_ItemId=@brand_itemid and IsEnable=1) g on base.Brand=g.ItemValue");
            var query = this.Context.Database.SqlQuery_Dic(sb.ToString(), sp);
            return query;
        }
        //门禁挂接页面提交
        public int SetStationAccess_Door(string doorids, int stationid)
        {
            var doorlist_old = this.Context.Set<SwsAccessControl>().Where(r => r.StationId == stationid);
            if (doorlist_old.Count() > 0)
            {
                foreach (var item in doorlist_old)
                {
                    item.StationId = 0;
                    this.Context.Set<SwsAccessControl>().Update(item);
                }

            }
            if (!string.IsNullOrEmpty(doorids))
            {
                var doorid = new List<string>(doorids.Split(',')).ConvertAll(r => int.Parse(r));
                var doorlist = this.Context.Set<SwsAccessControl>().Where(r => doorid.Contains(r.DoorId));
                foreach (var item in doorlist)
                {
                    item.StationId = stationid;
                    this.Context.Set<SwsAccessControl>().Update(item);
                }
                var stationModel = this.Context.Set<SwsStation>().Where(r => r.StationId == stationid).FirstOrDefault();
                if (stationModel != null)
                {
                    if (stationModel.DoorInsert == false)
                    {
                        stationModel.DoorInsert = true;
                        this.Context.Set<SwsStation>().Update(stationModel);
                    }
                }
            }
            else
            {
                var stationModel = this.Context.Set<SwsStation>().Where(r => r.StationId == stationid).FirstOrDefault();
                if (stationModel != null)
                {
                    if (stationModel.DoorInsert == true)
                    {
                        stationModel.DoorInsert = false;
                        this.Context.Set<SwsStation>().Update(stationModel);
                    }
                }

            }
            return this.Context.SaveChanges();
        }
        #endregion
        #region 通讯接入
        public IEnumerable<dynamic> GetDeviceByStationID(int stationid, string tablename)
        {
            var f_itemid = (int)Model.CustomizedClass.Enum.设备分区;
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@stationid",stationid),
                new SqlParameter("@tablename",tablename),
                new SqlParameter("@f_itemid",f_itemid)
              };
            string sql = @"  select d.DeviceID,d.Partition,ItemName as RegionName,r.DeviceID as comiID,ComAddress from [dbo].[" + @tablename + "] d " +
  @"left join [dbo].[Sws_RTUInfo] r on d.RTUID=r.RTUID
  left join (select * from [dbo].[Sys_DataItemDetail] where F_ItemId=@f_itemid and IsEnable=1) g on d.Partition=g.ItemValue
   where d.StationID=@stationid order by d.Partition";
            var query = this.Context.Database.SqlQuery_Dic(sql, sp);
            return query;
        }
        //设备取消通讯
        public int CancleRtu_Access(string Deviceid, int Intype)
        {
            List<long> deviceidlist = new List<string>(Deviceid.Split(",")).ConvertAll(r => long.Parse(r));
            if (Intype == 1)//二供
            {
                var models = this.Context.Set<SwsDeviceInfo01>().Where(r => deviceidlist.Contains(r.DeviceId));
                foreach (var item in models)
                {
                    item.Rtuid = 0;
                    this.Context.Set<SwsDeviceInfo01>().Update(item);
                }
                var rtuids = models.Select(r => r.Rtuid).Distinct();//相关的所有的rtuid
                var hasSameRtu = this.Context.Set<SwsDeviceInfo01>().Where(r => !deviceidlist.Contains(r.DeviceId) && rtuids.Contains(r.Rtuid)).Select(r => r.Rtuid);
                var updateRtuId = rtuids.Except(hasSameRtu);
                if (updateRtuId.Count() > 0)
                {
                    var swsRtu = this.Context.Set<SwsRtuinfo>().Where(r => updateRtuId.Contains(r.Rtuid));
                    foreach (var item in swsRtu)
                    {
                        item.StationId = 0;
                        this.Context.Set<SwsRtuinfo>().Update(item);
                    }
                }

            }
            else
            {
                var models = this.Context.Set<SwsDeviceInfo02>().Where(r => deviceidlist.Contains(r.DeviceId));
                foreach (var item in models)
                {
                    item.Rtuid = 0;
                    this.Context.Set<SwsDeviceInfo02>().Update(item);
                }
                var rtuids = models.Select(r => r.Rtuid).Distinct();//相关的所有的rtuid
                var hasSameRtu = this.Context.Set<SwsDeviceInfo02>().Where(r => !deviceidlist.Contains(r.DeviceId) && rtuids.Contains(r.Rtuid)).Select(r => r.Rtuid);
                var updateRtuId = rtuids.Except(hasSameRtu);
                if (updateRtuId.Count() > 0)
                {
                    var swsRtu = this.Context.Set<SwsRtuinfo>().Where(r => updateRtuId.Contains(r.Rtuid));
                    foreach (var item in swsRtu)
                    {
                        item.StationId = 0;
                        this.Context.Set<SwsRtuinfo>().Update(item);
                    }
                }

            }

            return this.Context.SaveChanges();
        }
        //设备通讯挂接
        public int SetDeviceRtu_Access(int rtuid, string deviceID, int intype)
        {
            List<long> deviceidlist = new List<string>(deviceID.Split(",")).ConvertAll(r => long.Parse(r));
            if (intype == 1)//二供
            {

                var deviceModels = this.Context.Set<SwsDeviceInfo01>().Where(r => deviceidlist.Contains(r.DeviceId));
                foreach (var item in deviceModels)
                {
                    item.Rtuid = rtuid;
                    this.Context.Set<SwsDeviceInfo01>().Update(item);
                }

                //查询所选设备原先的rtuid(除了传入的rtuid)    对应的通讯表的stationid是否需要置0
                var rtuidList = deviceModels.Where(r => r.Rtuid != rtuid).Select(r => r.Rtuid);
                var haveSameRtuid = this.Context.Set<SwsDeviceInfo01>().Where(r => rtuidList.Contains(r.Rtuid) && !deviceidlist.Contains(r.DeviceId)).Select(r => r.Rtuid);
                var handRtu = rtuidList.Except(haveSameRtuid);
                if (handRtu.Count() > 0)
                {
                    var rtuHands = this.Context.Set<SwsRtuinfo>().Where(r => handRtu.Contains(r.Rtuid));
                    foreach (var item in rtuHands)
                    {
                        item.StationId = 0;
                        this.Context.Set<SwsRtuinfo>().Update(item);
                    }
                }




                var rtumodel = this.Context.Set<SwsRtuinfo>().Where(r => r.Rtuid == rtuid).FirstOrDefault();
                if (rtumodel != null)
                {
                    if (rtumodel.StationId == 0)
                    {
                        rtumodel.StationId = deviceModels.FirstOrDefault().StationId;
                        this.Context.Set<SwsRtuinfo>().Update(rtumodel);
                    }
                }

            }
            else if (intype == 2)
            {
                var deviceModels = this.Context.Set<SwsDeviceInfo02>().Where(r => deviceidlist.Contains(r.DeviceId));
                foreach (var item in deviceModels)
                {
                    item.Rtuid = rtuid;
                    this.Context.Set<SwsDeviceInfo02>().Update(item);
                }
                //查询所选设备原先的rtuid(除了传入的rtuid)    对应的通讯表的stationid是否需要置0
                var rtuidList = deviceModels.Where(r => r.Rtuid != rtuid).Select(r => r.Rtuid);
                var haveSameRtuid = this.Context.Set<SwsDeviceInfo02>().Where(r => rtuidList.Contains(r.Rtuid) && !deviceidlist.Contains(r.DeviceId)).Select(r => r.Rtuid);
                var handRtu = rtuidList.Except(haveSameRtuid);
                if (handRtu.Count() > 0)
                {
                    var rtuHands = this.Context.Set<SwsRtuinfo>().Where(r => handRtu.Contains(r.Rtuid));
                    foreach (var item in rtuHands)
                    {
                        item.StationId = 0;
                        this.Context.Set<SwsRtuinfo>().Update(item);
                    }
                }


                var rtumodel = this.Context.Set<SwsRtuinfo>().Where(r => r.Rtuid == rtuid).FirstOrDefault();
                if (rtumodel != null)
                {
                    if (rtumodel.StationId == 0)
                    {
                        rtumodel.StationId = deviceModels.FirstOrDefault().StationId;
                        this.Context.Set<SwsRtuinfo>().Update(rtumodel);
                    }
                }
            }
            return this.Context.SaveChanges();
        }
        #endregion

        #endregion

        #region 查询泵房工艺图
        public IEnumerable<StationGUI> GetGUIImg(int sid)
        {
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@sid",sid)
              };
            var sql = @"select S.StationName,g.PageURL from Sws_Station s left join Sws_GUIInfo g on s.GuiNum=g.Num where s.StationID=@sid ";
            var query = this.Context.Database.SqlQuery<StationGUI>(sql, sp);
            return query;
        }
        //获取某泵房的分区和通讯id
        public IEnumerable<dynamic> GetStationOfRtu(int sttaionid)
        {
            SqlParameter[] sp = new SqlParameter[] { 
            new SqlParameter("@sttaionid",sttaionid)
            };
            var sql = @"  Select d.Partition,d.RTUID,a.ItemName from [dbo].[Sws_DeviceInfo01] d
  inner join [dbo].[Sws_Station] s on d.StationID=s.StationID
  left join (select * from [dbo].[Sys_DataItemDetail] where F_ItemId=8 and IsEnable=1) a on d.Partition=a.ItemValue
  where d.StationID=@sttaionid order by d.Partition";
            var query = this.Context.Database.SqlQuery_Dic(sql, sp);
            return query;
        }

        #endregion

        #region 实时轨迹跟踪
        public IEnumerable<GpsData> GetGPSMarkerData(string userid)
        {
            string year = DateTime.Now.Year.ToString();
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@year",year),
                new SqlParameter("@userid",userid)
            };
            string sql = @"select * from [WNMS_PhoneGps" + @year + "].[dbo].[HistoryPosition] where Lng!=0 and UserID=@userid order by UpdateTime desc";
            var query= this.Context.Database.SqlQuery<GpsData>(sql, sp).ToList();
            return query;
        }

        public IEnumerable<GpsData> GetGPSData(string userid, string beginDate,string endDate)
        {
            string year = DateTime.Now.Year.ToString();
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@year",year),
                new SqlParameter("@userid",userid),
                new SqlParameter("@beginDate",beginDate),
                new SqlParameter("@endDate",endDate)
            };
            string sql = @"select * from [WNMS_PhoneGps" + @year + "].[dbo].[HistoryPosition] where Lng!=0 and UserID=@userid and UpdateTime between  @beginDate  and @endDate  order by UpdateTime desc";
            var query = this.Context.Database.SqlQuery<GpsData>(sql, sp).ToList();
            return query;
        }
        #endregion
    }
}
