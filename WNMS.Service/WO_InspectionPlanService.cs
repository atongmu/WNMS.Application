using System;
using System.Collections.Generic;
using System.Text;
using WNMS.IService;
using Microsoft.Data.SqlClient;
using WNMS.Utility;
using WNMS.Model.DataModels;
using System.Linq;
using System.Data;
using WNMS.Model.CustomizedClass;

namespace WNMS.Service
{
    public partial class WO_InspectionPlanService : BaseService, IWO_InspectionPlanService
    {
        #region 模板管理
        public IEnumerable<dynamic> GetFeedBackItems(long templateID, int feedback_fitem)
        {
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@templateID",templateID),
                new SqlParameter("@feedback_fitem",feedback_fitem)
            };
            string sql = @"  select f.*,d.ItemName,case when t.TemplateID is not null then 1 else 0 end state from [dbo].[WO_FeedBackInfo] f
  left join ( select * from  [dbo].[Sys_DataItemDetail] where F_ItemId=@feedback_fitem and IsEnable=1) d on f.Type=d.ItemValue
  left join (select * from [dbo].[WO_FbOfTemplate] where TemplateID=@templateID) t on f.ID=t.FeedBackID";
            var query = this.Context.Database.SqlQuery_Dic(sql, sp);
            return query;

        }
        //添加模板
        public int AddTemplate(WoTemplateInfo tt, List<WoFbOfTemplate> fbTemplateList)
        {
            this.Context.Set<WoTemplateInfo>().Add(tt);
            this.Context.Set<WoFbOfTemplate>().AddRange(fbTemplateList);
            return this.Context.SaveChanges();
        }
        //修改模板
        public int EditeTemplate(WoTemplateInfo tt, List<WoFbOfTemplate> fbTemplateList)
        {
            var old_fbOft = this.Context.Set<WoFbOfTemplate>().Where(r => r.TemplateId == tt.Id);
            this.Context.Set<WoFbOfTemplate>().RemoveRange(old_fbOft);
            this.Context.Set<WoFbOfTemplate>().AddRange(fbTemplateList);
            this.Context.Set<WoTemplateInfo>().Update(tt);
            return this.Context.SaveChanges();

        }
        //删除模板
        public int DeleteTemplate(long templateid)
        {
            var temModel = this.Context.Set<WoTemplateInfo>().Where(r => r.Id == templateid).FirstOrDefault();
            var fb_tem = this.Context.Set<WoFbOfTemplate>().Where(r => r.TemplateId == templateid);
            this.Context.Set<WoTemplateInfo>().Remove(temModel);
            this.Context.Set<WoFbOfTemplate>().RemoveRange(fb_tem);
            return this.Context.SaveChanges();
        }
        //删除反馈项
        public int DeleteFeedBack(int id)
        {
            var fb_model = this.Context.Set<WoFeedBackInfo>().Where(r => r.Id == id).FirstOrDefault();
            var fb_template = this.Context.Set<WoFbOfTemplate>().Where(r => r.FeedBackId == id);
            if (fb_template.Count() > 0)
            {
                this.Context.Set<WoFbOfTemplate>().RemoveRange(fb_template);
            }
            this.Context.Set<WoFeedBackInfo>().Remove(fb_model);
            return this.Context.SaveChanges();
        }
        #endregion
        //获取创建人以及巡检员列表
        public IEnumerable<dynamic> LoadCreaterAndInspector()
        {
            SqlParameter[] sp = new SqlParameter[] { };
            string sql = @"
              select distinct UserID,NickName ,1 as type from [dbo].[WO_InspectionPlan] ap
            left join [dbo].[Sys_User] u on ap.CreateUser=u.UserID where UserID is not null
            union all(
            select distinct u.UserID, NickName ,2 as type from [dbo].[WO_InspectionPlan] ap
            left join [dbo].[Sys_User] u on ap.Inspector=u.UserID where UserID is not null )
            ";
            var query = this.Context.Database.SqlQuery_Dic(sql, sp).ToList();
            return query;
        }
        public IEnumerable<dynamic> QueryInspectPlanTable(int pageindex, int pagesize, string ordertems, string filter, string Travel_fitemid, string Cycle_fitemid, ref int Totalcount)
        {
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@startindex",pageindex),
                new SqlParameter("@pageSize",pagesize),
                new SqlParameter("@orderItems",ordertems),
                new SqlParameter("@filterString",filter),
                new SqlParameter("@Travel_fitemid",Travel_fitemid),
                new SqlParameter("@Cycle_fitemid",Cycle_fitemid),
                new SqlParameter("@Totalcount",Totalcount){Direction=System.Data.ParameterDirection.Output,DbType=System.Data.DbType.Int32}
            };
            var query = this.Context.Database.SqlQuery_Dic("exec QueryInspectPlanTable @startindex,@pageSize,@orderItems,@filterString,@Travel_fitemid,@Cycle_fitemid,@Totalcount Output", sp).ToList();

            Totalcount = Convert.ToInt32(sp[6].Value);
            return query;
        }
        //获取区域信息以及设备数量
        public IEnumerable<dynamic> GetAreaInfoByID(int areaid)
        {
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@areaid",areaid)
            };
            string sql = @"with t as(select DeviceID,StationID from [dbo].[Sws_DeviceInfo01]
union select DeviceID,StationID from [dbo].[Sws_DeviceInfo02]),
g as(
select AreaID,count(*) as countnum from [dbo].[WO_AreaRTU] ar
left join t on ar.EquipmentID=t.DeviceID and ar.StationID=t.StationID
where t.DeviceID is not null and AreaID=@areaid group by AreaID)
select ID, AreaName, GISPoints.ToString() as GISPoints, PID, FillColor ,case when countnum is null then 0 else countnum end countnum from  [dbo].[WO_AreaInfo] a
left join g on a.ID=G.AreaID
where a.ID=@areaid";
            var query = this.Context.Database.SqlQuery_Dic(sql, sp).ToList();
            return query;
        }
        //获取区域的所有设备
        public IEnumerable<AllotDevice> GetAllDeviceByID(int areaid, string searchtext)
        {
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@areaid",areaid),
                new SqlParameter("@searchtext", "%" + searchtext + "%")
            };
            String sql = @"with t as(select DeviceID,StationID,DeviceName from [dbo].[Sws_DeviceInfo01]
union select DeviceID,StationID,DeviceName from [dbo].[Sws_DeviceInfo02])
select t.*,StationName from [dbo].[WO_AreaRTU] ar
left join t on ar.EquipmentID=t.DeviceID and ar.StationID=t.StationID
left join [dbo].[Sws_Station] s on ar.StationID=s.StationID
";

            sql += " where t.DeviceID is not null and AreaID=@areaid and s.StationID is not null";
            if (!string.IsNullOrEmpty(searchtext))
            {
                sql += " and (StationName like @searchtext  or DeviceName like @searchtext)";
            }
            var query = this.Context.Database.SqlQuery<AllotDevice>(sql, sp).ToList();
            return query;
        }


        //获取已经被分配的设备
        public IEnumerable<AllotDevice> GetAllotDevice(DataTable tvpDt)
        {
            try
            {
                //if (tvpDt.Rows.Count > 0)
                //{
                SqlParameter[] sp = new SqlParameter[] {
                     new SqlParameter("@tvpDt", SqlDbType.Structured) { Value = tvpDt, TypeName = "DeviceTVP" }
                };
                string sql = @"  with t as (select DeviceID,DeviceName,StationID from [dbo].[Sws_DeviceInfo01]
  union select DeviceID,DeviceName,StationID from [dbo].[Sws_DeviceInfo02] )
  select t.*,StationName from @tvpDt p
  left join t on p.DeviceID=t.DeviceID and p.StationID=t.StationID
  left join [dbo].[Sws_Station] s on p.StationID=s.StationID
  where t.DeviceID is not null and s.StationID is not null";
                var query = this.Context.Database.SqlQuery<AllotDevice>(sql, sp).ToList();
                return query;
            }
            catch (Exception e)
            {
                var qq = e;
                return null;
            }
            //}
            //           else
            //           {
            //               SqlParameter[] sp = new SqlParameter[] { 
            //               new SqlParameter("@AreaId",AreaId),
            //               new SqlParameter("@PlanId",PlanId)
            //               };
            //               string sql = @"with t as (select DeviceID,DeviceName,StationID from [dbo].[Sws_DeviceInfo01]
            // union select DeviceID,DeviceName,StationID from [dbo].[Sws_DeviceInfo02] )
            //select t.* from [dbo].[WO_PlanInspectO] po
            //left join [dbo].[WO_AreaRTU] ar on po.InspectObject=ar.EquipmentID and po.PumpStationID=ar.StationID
            //left join t on po.InspectObject=t.DeviceID and po.PumpStationID=t.StationID
            //where PlanID=@PlanId and IsTemplate=1 and AreaID=@AreaId and t.DeviceID is not null";
            //               var query = this.Context.Database.SqlQuery_Dic(sql, sp).ToList();
            //               return query;
            //           }

        }
        #region 巡检计划添加、修改、删除
        public int AddInspectPlan(WoInspectionPlan p, List<WoPlanInspectO> planInspectO)
        {
            this.Context.Set<WoInspectionPlan>().Add(p);
            this.Context.Set<WoPlanInspectO>().AddRange(planInspectO);
            return this.Context.SaveChanges();
        }
        public int EditeInspectPlan(WoInspectionPlan p, List<WoPlanInspectO> planInspectO)
        {
            var oldPlanO = this.Context.Set<WoPlanInspectO>().Where(r => r.PlanId == p.Id && r.IsTemplate == true);
            if (oldPlanO.Count() > 0)
            {
                this.Context.Set<WoPlanInspectO>().RemoveRange(oldPlanO);
            }
            this.Context.Set<WoPlanInspectO>().AddRange(planInspectO);
            this.Context.Set<WoInspectionPlan>().Update(p);
            return this.Context.SaveChanges();
        }
        public int DeleteInspectPlan(List<long> planids)
        {
            var oldPlanO = this.Context.Set<WoPlanInspectO>().Where(r => planids.Contains(r.PlanId) && r.IsTemplate == true);
            var oldPlans = this.Context.Set<WoInspectionPlan>().Where(r => planids.Contains(r.Id));
            if (oldPlanO.Count() > 0)
            {
                this.Context.Set<WoPlanInspectO>().RemoveRange(oldPlanO);
            }
            this.Context.Set<WoInspectionPlan>().RemoveRange(oldPlans);
            return this.Context.SaveChanges();
        }

        /// <summary>
        /// 巡检延期申请
        /// </summary>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <param name="orderby"></param>
        /// <param name="filter"></param>
        /// <param name="Totalcount"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> GetInsEInfoTable(int pageindex, int pagesize, string orderby, string filter, ref int Totalcount)
        {
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@startindex",pageindex),
                new SqlParameter("@pageSize",pagesize),
                new SqlParameter("@orderItems",orderby),
                new SqlParameter("@filterString",filter),
                new SqlParameter("@Totalcount",Totalcount){Direction=System.Data.ParameterDirection.Output,DbType=System.Data.DbType.Int32}
            };
            var query = this.Context.Database.SqlQuery_Dic("exec QueryWO_InsExtension @startindex,@pageSize,@orderItems,@filterString,@Totalcount Output", sp).ToList();
            Totalcount = Convert.ToInt32(sp[4].Value);
            return query;
        }
        /// <summary>
        /// 审核巡检延期申请
        /// </summary>
        /// <param name="woInsExtension"></param>
        /// <param name="woAssignmentPlan"></param>
        /// <returns></returns>
        public int EditInsExtension(WoInsExtension woInsExtension, WoAssignmentPlan woAssignmentPlan)
        {
            try
            {
                this.Context.Update(woInsExtension);
                this.Context.Update(woAssignmentPlan);
                this.Context.SaveChanges();
            }
            catch (Exception ex)
            {
                return -1;
            }
            return 1;
        }
        /// <summary>
        /// 巡检转发
        /// </summary>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <param name="orderby"></param>
        /// <param name="filter"></param>
        /// <param name="Totalcount"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> GetInsForwardTable(int pageindex, int pagesize, string orderby, string filter, ref int Totalcount)
        {
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@startindex",pageindex),
                new SqlParameter("@pageSize",pagesize),
                new SqlParameter("@orderItems",orderby),
                new SqlParameter("@filterString",filter),
                new SqlParameter("@Totalcount",Totalcount){Direction=System.Data.ParameterDirection.Output,DbType=System.Data.DbType.Int32}
            };
            var query = this.Context.Database.SqlQuery_Dic("exec QueryWO_InsForward @startindex,@pageSize,@orderItems,@filterString,@Totalcount Output", sp).ToList();
            Totalcount = Convert.ToInt32(sp[4].Value);
            return query;
        }
        /// <summary>
        /// 审核巡检转发申请
        /// </summary>
        /// <param name="woInsExtension"></param>
        /// <param name="woAssignmentPlan"></param>
        /// <param name="woPlanInspectOs"></param>
        /// <returns></returns>
        public int EditInsForward(WoInsForward woInsForward, WoAssignmentPlan woAssignmentPlan, List<WoPlanInspectO> woPlanInspectOs)
        {
            try
            {
                this.Context.Update(woInsForward);
                this.Context.Update(woAssignmentPlan);
                foreach (var i in woPlanInspectOs)
                {
                    i.ForwardState = 2;
                    this.Context.Update(i);
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
        /// 审核驳回巡检转发申请
        /// </summary>
        /// <param name="woInsExtension"></param>
        /// <param name="woAssignmentPlan"></param>
        /// <param name="woPlanInspectOs"></param>
        /// <param name="newwoPlanInspectOs"></param>
        /// <returns></returns>
        public int TrunInsForward(WoInsForward woInsForward, WoAssignmentPlan woAssignmentPlan, List<WoPlanInspectO> woPlanInspectOs, List<WoPlanInspectO> newwoPlanInspectOs)
        {
            try
            {
                this.Context.Update(woInsForward);
                this.Context.Update(woAssignmentPlan);
                foreach (var i in woPlanInspectOs)
                {
                    i.ForwardState = null;
                    this.Context.Update(i);
                }
                foreach (var j in newwoPlanInspectOs)
                {
                    this.Context.Remove(j);
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
        /// 巡检退单
        /// </summary>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <param name="orderby"></param>
        /// <param name="filter"></param>
        /// <param name="Totalcount"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> GetInsTurnTable(int pageindex, int pagesize, string orderby, string filter, ref int Totalcount)
        {
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@startindex",pageindex),
                new SqlParameter("@pageSize",pagesize),
                new SqlParameter("@orderItems",orderby),
                new SqlParameter("@filterString",filter),
                new SqlParameter("@Totalcount",Totalcount){Direction=System.Data.ParameterDirection.Output,DbType=System.Data.DbType.Int32}
            };
            var query = this.Context.Database.SqlQuery_Dic("exec QueryWO_InsTurn @startindex,@pageSize,@orderItems,@filterString,@Totalcount Output", sp).ToList();
            Totalcount = Convert.ToInt32(sp[4].Value);
            return query;
        }
        /// <summary>
        /// 审核巡检退单申请
        /// </summary>
        /// <param name="woInsForward"></param>
        /// <param name="woAssignmentPlan"></param>
        /// <param name="woPlanInspectOs"></param>
        /// <returns></returns>
        public int EditInsTurn(WoInsForward woInsForward, WoAssignmentPlan woAssignmentPlan, List<WoPlanInspectO> woPlanInspectOs)
        {
            try
            {
                this.Context.Update(woInsForward);
                //this.Context.Update(woAssignment);
                this.Context.Add(woAssignmentPlan);
                foreach (var i in woPlanInspectOs)
                {
                    i.ForwardState = null;
                    this.Context.Add(i);
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
        #region 接口 
        public IEnumerable<dynamic> GetAssignList(int Travel_fitemid, int Cycle_fitemid, long userId)
        {
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@Travel_fitemid",Travel_fitemid),
                new SqlParameter("@Cycle_fitemid",Cycle_fitemid),
                new SqlParameter("@userId",userId)
            };
            string sql = @"select  PlanID, PlanName, Inspector, DMAID, State, BeginDate, EndDate,case when   PlanType=1 then '常规' else  '临时' end PlanType, CreateTime,
                            (select Top 1  State from WO_InsExtension where PlanID = a.PlanID order by ExtensionTime desc ) as ExtensionState,
                           (select Top 1 State from WO_InsForward wis where OldPlanID = a.PlanID and wis.Type=2 order by ExtensionTime desc ) as ForwardState,
                           (select Top 1 State from WO_InsForward wis where OldPlanID = a.PlanID and wis.Type=1 order by ExtensionTime desc ) as TurnState,
                           (select count(ID) from WO_InsExtension where PlanID = a.PlanID) AS ExtensionCount,
						   (select count(ID) from WO_InsForward wis where OldPlanID = a.PlanID and wis.Type=2   ) as ForwardCount,
                           (select count(ID) from WO_InsForward wis where OldPlanID = a.PlanID and wis.Type=1  ) as TurnCount,
                           (select count(*) from WO_PlanInspectO where PlanID = a.PlanID) as allcount,
                           (select count(*) from WO_InspectPlanCheck where PlanID = a.PlanID) as okcount,
                           (select count(*) from WO_PlanInspectO o  where PlanID = a.PlanID and o.ForwardState is not NULL) as isForwardcount,
                           IsFinish,d.ItemName as TravelName,d1.ItemName as CycleName,AreaName from [dbo].[WO_AssignmentPlan] a
                           left join (select * from  [dbo].[Sys_DataItemDetail] where F_ItemId=@Travel_fitemid and IsEnable=1 ) d  on a.Travel=d.ItemValue
                           left join (select * from  [dbo].[Sys_DataItemDetail] where F_ItemId=@Cycle_fitemid and IsEnable=1) d1 on a.InspectCycle=d1.ItemValue
                           left join [dbo].[WO_AreaInfo] e on a.DMAID=e.ID
                           where State=1 and (IsFinish =0 or IsFinish is null) and Inspector= @userId";
            var query = this.Context.Database.SqlQuery_Dic(sql, sp).ToList();
            return query;
        }

        //根据巡检ID获取巡检详情
        public IEnumerable<dynamic> GetAssignListByID(int Travel_fitemid, int Cycle_fitemid, long planid)
        {
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@Travel_fitemid",Travel_fitemid),
                new SqlParameter("@Cycle_fitemid",Cycle_fitemid),
                new SqlParameter("@planid",planid)
            };
            string sql = @"select  PlanID, PlanName, Inspector, DMAID, State, BeginDate, EndDate,case when   PlanType=1 then '常规' else  '临时' end PlanType, CreateTime,
                            (select Top 1  State from WO_InsExtension where PlanID = a.PlanID order by ExtensionTime desc ) as ExtensionState,
                           (select Top 1 State from WO_InsForward wis where OldPlanID = a.PlanID and wis.Type=2 order by ExtensionTime desc ) as ForwardState,
                           (select Top 1 State from WO_InsForward wis where OldPlanID = a.PlanID and wis.Type=1 order by ExtensionTime desc ) as TurnState,
                           (select top 1 OldPlanID from WO_InsForward wis where PlanID = @planid and wis.Type=1 order by ExtensionTime desc ) as OldPlanID,
                           (select count(ID) from WO_InsExtension where PlanID = a.PlanID) AS ExtensionCount,
						   (select count(ID) from WO_InsForward wis where OldPlanID = a.PlanID and wis.Type=2   ) as ForwardCount,
                           (select count(ID) from WO_InsForward wis where OldPlanID = a.PlanID and wis.Type=1  ) as TurnCount,                          
                           (select count(*) from WO_PlanInspectO where PlanID = a.PlanID) as allcount,
                           (select count(*) from WO_InspectPlanCheck where PlanID = a.PlanID) as okcount,
                           (select count(*) from WO_PlanInspectO o  where PlanID = a.PlanID and o.ForwardState is not NULL) as isForwardcount,
                           IsFinish,d.ItemName as TravelName,d1.ItemName as CycleName,AreaName from [dbo].[WO_AssignmentPlan] a
                           left join (select * from  [dbo].[Sys_DataItemDetail] where F_ItemId=@Travel_fitemid and IsEnable=1 ) d  on a.Travel=d.ItemValue
                           left join (select * from  [dbo].[Sys_DataItemDetail] where F_ItemId=@Cycle_fitemid and IsEnable=1) d1 on a.InspectCycle=d1.ItemValue
                           left join [dbo].[WO_AreaInfo] e on a.DMAID=e.ID
                           where PlanID = @planid";
            var query = this.Context.Database.SqlQuery_Dic(sql, sp).ToList();
            return query;
        }


        //获取历史工单
        public IEnumerable<dynamic> GetAssignListHistory(int Travel_fitemid, int Cycle_fitemid,string beginDate, long userId)
        {
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@Travel_fitemid",Travel_fitemid),
                new SqlParameter("@Cycle_fitemid",Cycle_fitemid),
                new SqlParameter("@userId",userId),
                new SqlParameter("@beginDate",beginDate)
            };
            string sql = @"select  PlanID, PlanName, Inspector, DMAID, State, BeginDate, EndDate,case when   PlanType=1 then '常规' else  '临时' end PlanType, CreateTime,
                            (select Top 1  State from WO_InsExtension where PlanID = a.PlanID order by ExtensionTime desc ) as ExtensionState,
                           (select Top 1 State from WO_InsForward wis where OldPlanID = a.PlanID and wis.Type=2 order by ExtensionTime desc ) as ForwardState,
                           (select Top 1 State from WO_InsForward wis where OldPlanID = a.PlanID and wis.Type=1 order by ExtensionTime desc ) as TurnState,
                           (select count(ID) from WO_InsExtension where PlanID = a.PlanID) AS ExtensionCount,
						   (select count(ID) from WO_InsForward wis where OldPlanID = a.PlanID and wis.Type=2   ) as ForwardCount,
                           (select count(ID) from WO_InsForward wis where OldPlanID = a.PlanID and wis.Type=1  ) as TurnCount,
                           (select count(*) from WO_PlanInspectO where PlanID = a.PlanID) as allcount,
                           (select count(*) from WO_InspectPlanCheck where PlanID = a.PlanID) as okcount,
                           (select count(*) from WO_PlanInspectO o  where PlanID = a.PlanID and o.ForwardState is not NULL) as isForwardcount,
                           IsFinish,d.ItemName as TravelName,d1.ItemName as CycleName,AreaName from [dbo].[WO_AssignmentPlan] a
                           left join (select * from  [dbo].[Sys_DataItemDetail] where F_ItemId=@Travel_fitemid and IsEnable=1 ) d  on a.Travel=d.ItemValue
                           left join (select * from  [dbo].[Sys_DataItemDetail] where F_ItemId=@Cycle_fitemid and IsEnable=1) d1 on a.InspectCycle=d1.ItemValue
                           left join [dbo].[WO_AreaInfo] e on a.DMAID=e.ID
                           where IsFinish = 1 and EndDate > @beginDate and Inspector=@userId";
            var query = this.Context.Database.SqlQuery_Dic(sql, sp).ToList();
            return query;
        }

        /// <summary>
        /// 获取简单的巡检信息 用于列表展示
        /// </summary>
        /// <param name="Travel_fitemid"></param>
        /// <param name="Cycle_fitemid"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> GetSinAssignList(string filter)
        {
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@filter",filter)
            };
            string sql = @"select  PlanID, PlanName,  State,   IsFinish, AreaName from [dbo].[WO_AssignmentPlan] a 
                           left join [dbo].[WO_AreaInfo] e on a.DMAID=e.ID
                           where " + @filter + "";
            var query = this.Context.Database.SqlQuery_Dic(sql, sp).ToList();
            return query;
        }
        public IEnumerable<dynamic> GetEqui_Assign(long planid)
        {
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@planid",planid)
            };
            string sql = @"with t as (select DeviceID,DeviceName,DeviceNum,StationID from [dbo].[Sws_DeviceInfo01]
                           union select DeviceID,DeviceName,DeviceNum,StationID from [dbo].[Sws_DeviceInfo02])
                           select t.*,StationName,Lng,Lat,ForwardCount,ForwardState ,(select count(ID) from WO_InspectPlanCheck where planid = @planid and EquipmentID =t.DeviceID) as checked ,
						    (select top(1) Reason from WO_InsExtension where PlanID = @planid order by ExtensionTime desc ) as ExtensionContent,
                           (select top(1) Remake from WO_InsForward wis where PlanID = @planid and wis.Type=2 order by ExtensionTime desc ) as ForwardContent,
                           (select top(1) Remake from WO_InsForward wis where PlanID = @planid and wis.Type=1 order by ExtensionTime desc ) as TurnContent                          
						   from [dbo].[WO_PlanInspectO] p
                           left join t on p.InspectObject=t.DeviceID and p.PumpStationID=t.StationID 
                           left join [dbo].[Sws_Station] s on p.PumpStationID=s.StationID 
						   where p.PlanID=@planid and IsTemplate=0
                           and t.DeviceID is not null and s.StationID is not null";
            var query = this.Context.Database.SqlQuery_Dic(sql, sp).ToList();
            return query;
        }

        /// <summary>
        /// 更新巡检转发设备状态
        /// </summary>
        /// <param name="woAssignmentPlan">新巡检信息</param>
        /// <param name="woPlanInspectO">巡检设备</param>
        /// <param name="oldwoPlanInspectO">旧设备信息</param>
        /// <param name="woInsForward">转发申请信息</param>
        /// <returns></returns>
        public int EditObjectForward(WoAssignmentPlan woAssignmentPlan, List<WoPlanInspectO> woPlanInspectO, List<WoPlanInspectO> oldwoPlanInspectO, WoInsForward woInsForward)

        {
            try
            {
                this.Context.Add(woAssignmentPlan);
                this.Context.Add(woInsForward);
                foreach (var i in woPlanInspectO)
                {
                    this.Context.Add(i);
                }
                foreach (var j in oldwoPlanInspectO)
                {
                    this.Context.Update(j);
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
