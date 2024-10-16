using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using WNMS.IService;
using WNMS.Model.CustomizedClass;
using WNMS.Model.DataModels;
using WNMS.Utility;
using System.Linq;
using Jiguang.JPush.Model;
using WNMS.Utility.Jpush;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Jiguang.JPush;

namespace WNMS.Service
{
    public partial class WO_AssignmentPlanService : BaseService, IWO_AssignmentPlanService
    {
        #region 增删改查
        public IEnumerable<dynamic> GetCreaterAndInspector()
        {
            SqlParameter[] sp = new SqlParameter[] {
            };
            string sql = @"select distinct UserID,NickName ,1 as type from [dbo].[WO_AssignmentPlan] ap
            left join [dbo].[Sys_User] u on ap.Creater=u.UserID
            union all(
            select distinct UserID,NickName ,2 as type from [dbo].[WO_AssignmentPlan] ap
            left join [dbo].[Sys_User] u on ap.Inspector=u.UserID)";
            var query = this.Context.Database.SqlQuery_Dic(sql, sp);
            return query;
        }

        public PageResult<Assignment> LoadAssignmentList(Expression<Func<Assignment, bool>> funcWhere, int pageSize, int pageIndex, string funcOrderby, bool isAsc = true)
        {
            SqlParameter[] sqlparameter = new SqlParameter[] {
            };
            string sql = @" select p.TemplatePlanID,p.IsForward, p.PlanID,p.PlanName,p.Travel,p.BeginDate,p.CreateTime,p.InspectCycle,p.EndDate,p.Remark,p.State,p.PlanType,p.IsFinish,p.IsChargeback,p.Inspector,p.Creater,s.ItemName as InspectCycleName,st.ItemName as TravelName,             u1.NickName as InspectorName, u.NickName as CreaterName,AreaName as DMAName	  
                FROM [dbo].[WO_AssignmentPlan] p
               left join [dbo].[Sys_User] u on p.Creater=u.UserID
              left join [dbo].[Sys_User] u1 on p.Inspector=u1.UserID
              left join [dbo].[WO_AreaInfo] a on p.DMAID=a.ID
			  left join [dbo].[Sys_DataItemDetail] s on s.F_ItemId=24 and s.ItemValue=p.InspectCycle
			  left join [dbo].[Sys_DataItemDetail] st on st.F_ItemId=25 and st.ItemValue=p.Travel";

            PageResult<Assignment> presult = new PageResult<Assignment>();
            presult = this.ExcuteQueryPage(funcWhere, pageSize, pageIndex, funcOrderby, sql, sqlparameter, isAsc);
            return presult;
        }

        //根据任务id 获取任务的部分信息
        public IEnumerable<dynamic> GetAssignmentPlanInfo(long id)
        {
            SqlParameter[] sqlparameter = new SqlParameter[] {
                new SqlParameter("@id",id)
            };
            string sql = @"select AreaName as DMAName,d.DeviceName,u1.NickName as InspectorName,po.InspectObject,po.PumpStationID from [dbo].[WO_AssignmentPlan] p 
                        left join [dbo].[WO_AreaInfo] ar on p.DMAID=ar.ID
                        left join [dbo].[Sys_User] u1 on p.Inspector=u1.UserID 
                        left join [dbo].[WO_PlanInspectO] po on po.PlanID=p.PlanID
                        left join [dbo].[Sws_DeviceInfo01] d  on po.InspectObject=d.DeviceID where p.PlanID=@id";
            var query = this.Context.Database.SqlQuery_Dic(sql, sqlparameter);
            return query;
        }
        //获取客服人员列表
        public IEnumerable<dynamic> GetUserInfoList(long DMAID)
        {
            SqlParameter[] sqlparameter = new SqlParameter[] {
                new SqlParameter("@DMAID", DMAID)
            };
            string sql = "";
            if (DMAID == 0)
            {
                sql = @"select UserID as ID,Account,NickName,Phone,2 as type from [dbo].[Sys_User]";
            }
            else
            {
                sql = @"SELECT tu.UserID as ID,u.Account,u.NickName,u.Phone,t.TeamID,t.TeamName,2 as type
                        FROM [dbo].[WO_TeamUser] tu
                        inner join [dbo].[Sys_User] u on u.UserID=tu.UserID
                        left join [dbo].[WO_TeamInfo] t on t.TeamID=tu.TeamID
                        where t.RegionID=@DMAID order by t.TeamID";
            }
            var query = this.Context.Database.SqlQuery_Dic(sql, sqlparameter);
            return query;
        }
        #endregion

        #region 增删改查

        //任务添加
        public int AddPlan(WoAssignmentPlan plan, List<WoPlanInspectO> planInspectO)
        {
            List<Model.CustomizedClass.TaskPush> taskpushs = new List<Model.CustomizedClass.TaskPush>();


            if (plan != null)
            {
                this.Context.Add(plan);

                Model.CustomizedClass.TaskPush task = new Model.CustomizedClass.TaskPush();
                task.UserID = plan.Inspector;
                task.PlanID = plan.PlanId;
                task.Content = "收到一条临时任务，请注意查收";
                taskpushs.Add(task);
                PushTask(taskpushs);
            }

            if (planInspectO.Count > 0)
            {
                this.Context.AddRange(planInspectO);
            }
            return this.Context.SaveChanges();
        }
        //任务修改
        public int EditePlan(WoAssignmentPlan plan, List<WoPlanInspectO> planInspectO)
        {
            if (plan != null)
            {
                plan.State = true;
                plan.IsFinish = 0;
                this.Context.Update(plan);
            }

            List<WoPlanInspectO> dma_ObjectList = this.Query<WoPlanInspectO>(r => r.PlanId == plan.PlanId).ToList();
            if (dma_ObjectList.Count > 0)
            {
                this.Context.RemoveRange(dma_ObjectList);
            }

            if (planInspectO.Count > 0)
            {
                this.Context.AddRange(planInspectO);
            }
            return this.Context.SaveChanges();
        }

        //删除任务
        public int DeletePlan(string planIDs)
        {
            //删除任务巡检对象中间表
            var planIDList = new List<string>(planIDs.Split(',')).ConvertAll(r => long.Parse(r));
            List<WoPlanInspectO> planInspectO = this.Query<WoPlanInspectO>(r => planIDList.Contains(r.PlanId) && r.IsTemplate == false).ToList();
            if (planInspectO.Count > 0)
            {
                this.Context.RemoveRange(planInspectO);
            }

            //删除任务表
            List<WoAssignmentPlan> planlist = this.Query<WoAssignmentPlan>(r => planIDList.Contains(r.PlanId)).ToList();
            if (planlist.Count > 0)
            {
                this.Context.RemoveRange(planlist);
            }

            //删除任务反馈表
            var plancheck = this.Query<WoInspectPlanCheck>(r => planIDList.Contains(r.PlanId)).ToList();
            if (plancheck.Count > 0)
            {
                this.Context.RemoveRange(plancheck);
            }

            return this.Context.SaveChanges();
        }
        #endregion

        #region  查看
        /// <summary>
        /// 查看页面页面加载
        /// </summary>
        /// <param name="planID">任务ID</param>
        /// <returns></returns>
        public IEnumerable<dynamic> GetWatchData(long planID)
        {
            SqlParameter[] sqlparameter = new SqlParameter[] {
                new SqlParameter("@planID", planID)
            };
            string sql = @"select PlanID,convert(varchar(10),BeginDate,102)+'-'+convert(varchar(10),EndDate,102) as time,
                            (select count(*) from [dbo].[WO_PlanInspectO] po where po.PlanID=@planID) as allNum,
                            (select count(*) from [dbo].[WO_InspectPlanCheck] where PlanID=@planID and FeedBackState=1) as feedNum 
                            from [dbo].[WO_AssignmentPlan] where PlanID=@planID";

            var query = this.Context.Database.SqlQuery_Dic(sql, sqlparameter).ToList();
            return query;
        }

        /// <summary>
        /// 查询任务巡检对象列表
        /// </summary>
        /// <param name="planID">任务ID</param>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        public IEnumerable<dynamic> GetWatchDetailData(long planID, int type)
        {
            SqlParameter[] sqlparameter = new SqlParameter[] {
                new SqlParameter("@planID", planID),
                new SqlParameter("@type",type)
            };

            string str = "1=1";
            if (type != 8)
            {
                str += " and FeedBackState=@type ";
            }

            string sql = @"select row_number() over (order by ModifyTime desc) as rownum,* from(
                        select po.InspectObject,po.ForwardState, d.DeviceID as EquipmentID,  DeviceNum, cast(t.Lng as varchar(20))+','+cast(t.Lat as varchar(20)) as Gis,DeviceName,isnull(ID,0)as ID,ModifyTime,isnull(FeedBackState,0) as FeedBackState  from [dbo].[WO_PlanInspectO] po 
                        left join [dbo].[WO_InspectPlanCheck] c on po.InspectObject=c.EquipmentID and po.PlanID=c.PlanID
                        left join (select DeviceID,DeviceName,DeviceNum,StationID from [dbo].[Sws_DeviceInfo01] union select DeviceID,DeviceName,DeviceNum,StationID from [dbo].[Sws_DeviceInfo02]) d on po.InspectObject=d.DeviceID
                        left join [dbo].[Sws_Station] t on t.StationID=d.StationID where po.PlanID=@planID   
                        ) as aa where  " + str + "";

            var query = this.Context.Database.SqlQuery_Dic(sql, sqlparameter);
            return query;
        }

        //根据设备id和任务id获取相应的返回模板
        public IEnumerable<dynamic> GetFeedDataInfo(long planID)
        {
            SqlParameter[] sqlparameter = new SqlParameter[] {
                new SqlParameter("@planID", planID),
            };

            string sql = @"select fb.* from [dbo].[WO_FeedBackInfo] fb
                        left join [dbo].[WO_FbOfTemplate]  fo on fb.ID=fo.FeedBackID
                        left join [dbo].[WO_AssignmentPlan] ap on fo.TemplateID=ap.TemplateID
                        where ap.PlanID=@planID";

            var query = this.Context.Database.SqlQuery_Dic(sql, sqlparameter);
            return query;
        }

        //获取设备信息
        public IEnumerable<dynamic> GetDeviceData(long deviceID)
        {
            SqlParameter[] sp = new SqlParameter[]
            {
                new SqlParameter("@deviceID",deviceID)
            };
            string sql = @"with t as (select DeviceID,DeviceName,DeviceNum,StationID from [dbo].[Sws_DeviceInfo01] union select                  DeviceID,DeviceName,DeviceNum,StationID from [dbo].[Sws_DeviceInfo02])
                select t.*,s.StationName,cast(s.Lng as varchar(20))+','+cast(s.Lat as varchar(20)) as Gis,s.InstallationPosition,r.[ComAddress],r.[DeviceID] as ComNum,r.[IPPort]  from t left join [dbo].[Sws_Station] s on t.StationID=s.StationID
                left join [dbo].[Sws_RTUInfo] r on s.StationID=r.StationID where t. DeviceID=@deviceID";
            var query = this.Context.Database.SqlQuery_Dic(sql, sp);
            return query;
        }

        //任务分派
        public int AssignPlan(string planIDs)
        {
            //更新任务状态
            var planIDList = new List<string>(planIDs.Split(',')).ConvertAll(r => long.Parse(r));
            var planList = this.Query<WoAssignmentPlan>(r => planIDList.Contains(r.PlanId)).AsNoTracking().ToList();

            List<Model.CustomizedClass.TaskPush> taskpushs = new List<Model.CustomizedClass.TaskPush>();
            var inspects = planList.Select(r => r.Inspector).Distinct();
            foreach (var item in inspects)
            {
                #region 派发集合
                Model.CustomizedClass.TaskPush task = new Model.CustomizedClass.TaskPush();
                task.UserID = item;
                var tasklist = planList.Where(r => r.Inspector == item).ToList();
                if (tasklist.Count > 1)
                {
                    task.PlanID = 0;
                }
                else
                {
                    task.PlanID = tasklist.FirstOrDefault().PlanId;
                }
                task.Content = "收到" + tasklist.Count + "条任务,请注意查收！";
                taskpushs.Add(task);
                #endregion

            }
            if (taskpushs.Count() > 0)
            {
                PushTask(taskpushs);
            }
            foreach (var item in planList)
            {
                item.State = true;
                this.Context.Update(item);
            }

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

        #region 定时分派任务
        /// <summary>
        /// 创建定时任务  先创建计划，再创建定jpush时任务
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string CreateSchedule(long id)
        {
            WoInspectionPlan isp = this.Query<WoInspectionPlan>(r => r.Id == id).FirstOrDefault();
            string scheduleId = string.Empty;
            int cycle = isp.Cycle;
            string time = "";
            string unite = "";
            int fre = 1;
            string week = "Mon";
            if (isp.DayNums != null)
            {
                week = System.Enum.GetName(typeof(Weeks),Convert.ToInt32(isp.DayNums)).ToString();
               
            }

            List<string> str = new List<string>();
            switch (cycle)
            {
                case 1:
                    time = "23:05:00";
                    unite = "day";
                    break;
                case 2:
                    time = "08:00:00";
                    unite = "week";
                    str.Add(week);
                    break;
                case 3:
                    time = "08:00:00";
                    unite = "month";
                    str.Add(Convert.ToInt32(isp.DayNums).ToString("00"));
                    break;
                case 4:
                    time = "08:00:00";
                    unite = "month";
                    str.Add(Convert.ToInt32(isp.DayNums).ToString("00"));
                    fre = 12;
                    break;

                default:
                    time = "08:00:00";
                    unite = "day";
                    break;
            }
            if (cycle == 60)
            {
                time = "08:00:00";
                unite = "day";
            }
            PushPayload pushPayload = new PushPayload()
            {
                Platform = new List<string> { "android", "ios" },
                Audience = new Audience
                {
                    Alias = new List<string> { isp.Inspector.ToString() }
                },
                Message = new Message
                {
                    Title = isp.Id.ToString(),
                    Content = isp.Remark,
                    Extras = new Dictionary<string, string>
                    {
                        ["PlanType"] = "1",
                        ["Remark"] = "计划任务"
                    }
                }
            };
            var trigger = new Trigger
            {
                StartDate = isp.BeginTime.ToString("yyyy-MM-dd HH:mm:ss"),
                EndDate = isp.EndTime.ToString("yyyy-MM-dd HH:mm:ss"),
                TriggerTime = time,
                TimeUnit = unite,
                Frequency = fre,
                TimeList = str
            };
            Jpush jpush = new Jpush();
            try
            {
                var result = jpush.ExecuteSchedule(pushPayload, trigger, isp.PlanName);
                var aa = Newtonsoft.Json.JsonConvert.DeserializeObject(result.Content);
                Dictionary<string, string> cc = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(result.Content);
                JArray jar = Newtonsoft.Json.JsonConvert.DeserializeObject(result.Content) as JArray;
                scheduleId = cc["schedule_id"].ToString();
            }
            catch (Exception)
            {
                throw;
            }
            return scheduleId;
        }

        /// <summary>
        /// 修改定时任务，若定时任务存在则修改，若不存在则添加，并修改数据库中的ScheduleID
        /// </summary>
        /// <param name="isp">计划任务类</param>
        public string EditSchedule(WoInspectionPlan isp)
        {

            //参数处理
            string scheduleId = string.Empty;
            int cycle = isp.Cycle;
            string time = "";
            string unite = "";
            int fre = 1;
            string week = "Mon";
            if (isp.DayNums != null)
            {
                week = System.Enum.GetName(typeof(Weeks), isp.DayNums).ToString();
            }

            List<string> str = new List<string>();
            switch (cycle)
            {
                case 1:
                    time = "23:05:00";
                    unite = "day";
                    break;
                case 2:
                    time = "08:00:00";
                    unite = "week";
                    str.Add(week);
                    break;
                case 3:
                    time = "08:00:00";
                    unite = "month";
                    str.Add(Convert.ToInt32(isp.DayNums).ToString("00"));
                    break;
                case 4:
                    time = "08:00:00";
                    unite = "month";
                    str.Add(Convert.ToInt32(isp.DayNums).ToString("00"));
                    fre = 12;
                    break;

                default:
                    time = "08:00:00";
                    unite = "day";
                    break;
            }
            if (cycle == 60)
            {
                time = "08:00:00";
                unite = "day";
            }
            PushPayload pushPayload = new PushPayload()
            {
                Platform = new List<string> { "android", "ios" },
                Audience = new Audience
                {
                    Alias = new List<string> { isp.Inspector.ToString() }
                },
                Message = new Message
                {
                    Title = isp.Id.ToString(),
                    Content = isp.Remark,
                    Extras = new Dictionary<string, string>
                    {
                        ["PlanType"] = "1",
                        ["Remark"] = "计划任务"
                    }
                }
            };
            var trigger = new Trigger
            {
                StartDate = isp.BeginTime.ToString("yyyy-MM-dd HH:mm:ss"),
                EndDate = isp.EndTime.ToString("yyyy-MM-dd HH:mm:ss"),
                TriggerTime = time,
                TimeUnit = unite,
                Frequency = fre,
                TimeList = str
            };

            //定时任务修改/创建
            Jpush jpush = new Jpush();
            string stask = jpush.GetSchedule(isp.ScheduleId).Content;  //获取当前Jpush定时任务
            if (stask.IndexOf("error") > 0)     //若定时任务不存在  添加
            {
                var result = jpush.ExecuteSchedule(pushPayload, trigger, isp.PlanName);
                Dictionary<string, string> jar = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(result.Content);
                scheduleId = jar["schedule_id"].ToString();
            }
            else        //若定时任务存在，修改定时任务
            {
                jpush.EditPeriodicalSchedule(isp.ScheduleId, isp.PlanName, isp.EnabledMark, trigger, pushPayload);
            }
            return scheduleId;
        }

        /// <summary>
        /// 删除定时任务   先删除Jpush定时任务，再删除计划
        /// </summary>
        /// <param name="id">定时任务id</param>
        /// <returns></returns>
        //public bool DeleteSchedule(string id)
        //{
        //    bool flag = true;
        //    Jpush jpush = new Jpush();
        //    var result = jpush.DeleteSchedule(id).ToString();
        //    if (result == "404")
        //    {
        //        flag = false;
        //    }
        //    return flag;
        //}

        public void DeleteSchedule(List<long> planids)
        {
            Jpush jpush = new Jpush();

            var oldPlans = this.Context.Set<WoInspectionPlan>().Where(r => planids.Contains(r.Id)).ToList();
            foreach (var item in oldPlans)
            {
                if (!string.IsNullOrEmpty(item.ScheduleId))
                {
                    jpush.DeleteSchedule(item.ScheduleId);
                }
            }
        }
        #endregion

        #region 巡检接口
        /// <summary>
        /// 根据巡检任务ID获取模板巡检项
        /// </summary>
        /// <param name="planID">巡检任务ID</param>
        /// <returns></returns>
        public IEnumerable<dynamic> GetTemplateDetailData(long planID)
        {
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@planID",planID)
            };
            string sql = @"select TemplateID,FeedBackID,Type,FeedBackName,Unit from [dbo].[WO_FbOfTemplate] t 
                        left join [dbo].[WO_FeedBackInfo] f on t.[FeedBackID]=f.[ID]
                        where t.TemplateID in (select TemplateID from [dbo].[WO_AssignmentPlan] where PlanID=@planID) order by Type";
            var query = this.Context.Database.SqlQuery_Dic(sql, sp);
            return query;
        }
        /// <summary>
        /// 更新上传反馈
        /// </summary>
        /// <param name="woInspectPlanCheck">反馈信息</param>
        /// <param name="woAssignmentPlan">巡检信息</param>
        /// <param name="woPlanInspectO">巡检设备信息</param>
        /// <returns></returns>
        public int AddInspectPlanCheckInfo(WoInspectPlanCheck woInspectPlanCheck, WoAssignmentPlan woAssignmentPlan, WoPlanInspectO woPlanInspectO)
        {
            try
            {

                this.Context.Add(woInspectPlanCheck);
                this.Context.Update(woAssignmentPlan);
                this.Context.Update(woPlanInspectO);
                this.Context.SaveChanges();
            }
            catch (Exception ex)
            {
                return -1;
            }
            return 1;
        }
        /// <summary>
        /// 审核巡检工单
        /// </summary>
        /// <param name="woAssignmentPlan">巡检单信息</param>
        /// <param name="woPlanInspectOs">巡检设备信息</param>
        /// <param name="woInsExtension">巡检延期申请信息</param>
        /// <returns></returns>
        public int AuditAss(WoAssignmentPlan woAssignmentPlan, List<WoPlanInspectO> woPlanInspectOs, List<WoInsExtension> woInsExtension)
        {
            try
            {
                this.Context.Update(woAssignmentPlan);
                if (woPlanInspectOs.Count > 0)
                {
                    foreach(var item in woPlanInspectOs)
                    {
                        item.ForwardState = null;
                        this.Context.Update(item);
                    }
                }
                if (woInsExtension.Count > 0)
                {
                    foreach (var item in woInsExtension)
                    {
                        this.Context.Remove(item);
                    }
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
        /// 审核反馈设备
        /// </summary>
        /// <param name="woPlanInspectO">设备表</param>
        /// <param name="woInspectPlanCheck">设备反馈表</param>
        /// <returns></returns>
        public int AuditEq(WoPlanInspectO woPlanInspectO,WoInspectPlanCheck woInspectPlanCheck)
        {
            try
            {
                this.Context.Update(woPlanInspectO);
                this.Context.Remove(woInspectPlanCheck); 
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
