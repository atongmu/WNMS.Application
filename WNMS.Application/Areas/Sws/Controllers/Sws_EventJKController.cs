using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Jiguang.JPush.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WNMS.Application.Controllers;
using WNMS.Application.Utility;
using WNMS.Application.Utility.Filters;
using WNMS.IService;
using WNMS.Model.CustomizedClass;
using WNMS.Model.DataModels;
using WNMS.Utility.Jpush;
using WNMS.Utility.PanGu;

namespace WNMS.Application.Areas.Sws.Controllers
{
    [Area("Sws")]
    public class Sws_EventJKController : Controller
    {
        private readonly ILogger<LoginController> logger = null;
        private ISysUserService _UserService = null;
        private ISws_EventInfoService _EventInfoService = null;
        private ISys_DataItemDetailService _DataItemDetailService = null;
        private ISws_EventHandleService _EventHandleService = null;
        private ISws_EventAttentionService _EventAttentionService = null;
        private ISws_RTUInfoService _RTUInfoService = null;
        private ISws_UserStationService _UserStationService = null;
        private IWO_WorkOrderService _wO_WorkOrderService = null;
        private IWO_EventsService _wO_EventsService = null;
        private ISys_EarlyWarningPlanService _sys_EarlyWarningPlanService = null;
        public Sws_EventJKController(ISysUserService sysUserService,
            ISws_EventInfoService sws_EventInfoService,
            ISys_DataItemDetailService sys_DataItemDetailService,
            ISws_EventHandleService sws_EventHandleService,
            ISws_EventAttentionService sws_EventAttentionService,
            ISws_RTUInfoService sws_RTUInfoService,
            ISws_UserStationService sws_UserStationService,
            IWO_WorkOrderService wO_WorkOrderService,
            IWO_EventsService wO_EventsService,
            ISys_EarlyWarningPlanService sys_EarlyWarningPlanService,
            ILogger<LoginController> myLogger)
        {
            _UserService = sysUserService;
            _EventInfoService = sws_EventInfoService;
            _DataItemDetailService = sys_DataItemDetailService;
            _EventHandleService = sws_EventHandleService;
            _EventAttentionService = sws_EventAttentionService;
            _RTUInfoService = sws_RTUInfoService;
            _UserStationService = sws_UserStationService;
            _wO_WorkOrderService = wO_WorkOrderService;
            _wO_EventsService = wO_EventsService;
            _sys_EarlyWarningPlanService = sys_EarlyWarningPlanService;
            logger = myLogger;
        }
        public IActionResult Index()
        {
            var f_itemid = (int)Model.CustomizedClass.Enum.泵房类型;
            var stationTypeList = _DataItemDetailService.Query<SysDataItemDetail>(r => r.FItemId == f_itemid);
            ViewBag.stationTypeList = stationTypeList;

            return View();
        }
        //数据查询
        [TypeFilter(typeof(IgonreActionFilter))]
        public async Task<IActionResult> QueryEventJKInfo(string alarmLevel, string stationType, string searchTpe, int pageindex, int pagesize, string searchtext)
        {
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = _UserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
            string filter = " 1=1";
            string countfilter = " 1=1";
            if (!string.IsNullOrEmpty(alarmLevel))
            {
                filter += " and EventLevel in (" + alarmLevel + ")";
                countfilter += " and EventLevel in (" + alarmLevel + ")";

            }
            if (!string.IsNullOrEmpty(stationType))
            {
                filter += " and StaitonType in (" + stationType + ")";
                countfilter += "and StaitonType in (" + stationType + ")";
            }
            if (!string.IsNullOrEmpty(searchtext))
            {
                filter += " and (StationName like '%" + searchtext + "%' or EventMessage like '%" + searchtext + "%')";
                countfilter += "and (StationName like '%" + searchtext + "%' or EventMessage like '%" + searchtext + "%')";
            }
            if (searchTpe == "attend")//查询关注报警
            {
                filter += " and IsAttend=1";
            }
            if (searchTpe == "valueAlarm")//阈值报警
            {
                filter += " and EventType!=0";
            }
            if (searchTpe == "commAlarm")//通讯报警
            {
                filter += " and EventSource in (1000,1001)";
            }
            var f_itemID = (int)Model.CustomizedClass.Enum.设备分区;//分区
            var f_itemIDStationType = (int)Model.CustomizedClass.Enum.泵房类型;//泵房分类
            int Totalcount = 0;//总数量
            int AttendNum = 0;//关注报警
            int ValueAlarmNum = 0;//阈值报警数量
            int CommiNum = 0;//通讯报警数量
            int DataNum = 0;//
            var datalist = _EventInfoService.QueryEventInfo_JK(pageindex, pagesize, filter, countfilter, userID, user.IsAdmin, f_itemID, f_itemIDStationType, ref Totalcount, ref AttendNum, ref ValueAlarmNum, ref CommiNum, ref DataNum);
            double TotalPage = Math.Ceiling((float)DataNum / (float)pagesize);

            PartialView("_eventJKTable", datalist);
            string dataTable = await ViewToString.RenderPartialViewToString(this, "_eventJKTable");

            return Json(new
            {
                TotalCount = Totalcount,
                TotalPage = TotalPage,
                dataTable = dataTable,
                AttendNum = AttendNum,
                ValueAlarmNum = ValueAlarmNum,
                CommiNum = CommiNum

            });

        }

        #region 报警处理
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult HandleEventPage(bool ishandle, bool isrefresh, string alarmMessage, string eventtime, int rtuid, int eventsouce)
        {
            if (ishandle == true)
            {
                var model = _EventInfoService.QueryEventHandleInfo(Convert.ToDateTime(eventtime), rtuid, eventsouce).FirstOrDefault();
                if (model != null)
                {
                    ViewBag.IsConvertOrder = model.IsConvertOrder;
                    ViewBag.FeedBackInfo = model.FeedBackInfo;
                    ViewBag.UserName = model.NickName;

                }
                else
                {
                    ViewBag.IsConvertOrder = false;
                    ViewBag.FeedBackInfo = "";
                    ViewBag.UserName = "";
                }

            }
            else
            {
                ViewBag.IsConvertOrder = false;
                ViewBag.FeedBackInfo = "";
                ViewBag.UserName = "";
            }
            ViewBag.alarmMessage = alarmMessage;
            ViewBag.eventtime = eventtime;
            ViewBag.rtuid = rtuid;
            ViewBag.eventsouce = eventsouce;
            ViewBag.ishandle = ishandle;
            ViewBag.isrefresh = isrefresh;
            return View();
        }

        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult SetEventHandle(string eventtime, int rtuid, short eventsouce, string feedbackInfo, bool IsConvertOrder, string alarminfo)
        {
            int userID = int.Parse(User.FindFirstValue("UserID"));
            SwsEventHandle e = new SwsEventHandle();
            e.EventSource = eventsouce;
            e.EventTime = Convert.ToDateTime(eventtime);
            e.FeedBackInfo = feedbackInfo;
            e.IsConvertOrder = IsConvertOrder;
            e.Rtuid = rtuid;
            e.UserId = userID;
            var eventInfo = _EventInfoService.Query<SwsEventInfo>(r => r.Rtuid == rtuid && r.EventSource == eventsouce).FirstOrDefault();
            var eventLevel = eventInfo?.EventLevel;
            //try {
            //    _EventHandleService.Insert<SwsEventHandle>(e);
            //    return Content("ok");
            //}
            //catch (Exception ee)
            //{

            //    return Content("no");
            //}
            //先判断是否已经反馈
            var hasModle = _EventHandleService.Query<SwsEventHandle>(r => r.EventTime == e.EventTime && r.Rtuid == rtuid && r.EventSource == eventsouce).FirstOrDefault();
            if (hasModle != null)
            {
                return Content("have");
            }
            if (_EventInfoService.HandlerEventInfo(eventInfo?.Id, e, alarminfo, eventLevel) > 0)
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }
        }

        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult DeleteEvents(string eventtime, int rtuid, short eventsouce, int eventid)
        {
            DateTime eventTime = Convert.ToDateTime(eventtime);
            var eventLevel = _EventInfoService.Query<SwsEventInfo>(r => r.Id == eventid).FirstOrDefault();
            if (eventLevel == null)
            {
                return Content("false");
            }
            else
            {
                if (_EventInfoService.DeleteEvents(eventid, eventTime, eventsouce, rtuid) > 0)
                {
                    return Content("ok");
                }
                else
                {
                    return Content("no");
                }
            }
        }
        #endregion
        #region 泵房关注
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult OperateAttend(bool operate, short datasrouce)
        {
            int userID = int.Parse(User.FindFirstValue("UserID"));
            if (operate == true)//关注报警
            {
                SwsEventAttention a = new SwsEventAttention();
                a.DataId = datasrouce;
                a.UserId = userID;
                try
                {
                    _EventAttentionService.Insert<SwsEventAttention>(a);
                    return Content("ok");
                }
                catch (Exception e)
                {
                    return Content("no");
                }
            }
            else//取消报警
            {
                SwsEventAttention a = new SwsEventAttention();
                a.DataId = datasrouce;
                a.UserId = userID;
                try
                {
                    _EventAttentionService.Delete<SwsEventAttention>(a);
                    return Content("ok");
                }
                catch (Exception e)
                {
                    return Content("no");
                }
            }
        }
        #endregion
        #region 报警推送
        [TypeFilter(typeof(IgonreActionFilter))]
        public async Task<IActionResult> GetDeviceAlarm(string ids)
        {
            int TotalCount = 0;
            var idflag = ids.Split(',');
            var rtuid = long.Parse(idflag[0]);
            var eventSource = short.Parse(idflag[1]);
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = _UserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
            string filter = "e.RTUID=" + rtuid + " and e.EventSource=" + eventSource + "";
            var f_itemid = (int)Model.CustomizedClass.Enum.设备分区;
            var data = _EventInfoService.GetAlarmList(userID, user.IsAdmin, filter, 1, 100, f_itemid, ref TotalCount);
            new Task(() =>
            {
                SendWo(rtuid, eventSource); //发送工单
            });
            return Json(new
            {
                data = data,
                TotalCount = TotalCount
            });
        }
        //[TypeFilter(typeof(IgonreActionFilter))]
        //public IActionResult GetDeviceAlarm(string ids)
        //{
        //    int TotalCount = 0;
        //    var idflag = ids.Split(',');
        //    var rtuid = long.Parse(idflag[0]);
        //    var eventSource = short.Parse(idflag[1]);
        //    int userID = int.Parse(User.FindFirstValue("UserID"));
        //    var user = _UserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
        //    string filter = "e.RTUID=" + rtuid + " and e.EventSource=" + eventSource + "";
        //    var f_itemid = (int)Model.CustomizedClass.Enum.设备分区;
        //    var data = _EventInfoService.GetAlarmList(userID, user.IsAdmin, filter, 1, 100, f_itemid, ref TotalCount); 
        //    return Json(new
        //    {
        //        data = data,
        //        TotalCount = TotalCount
        //    });
        //}
        /// <summary>
        /// 报警生成工单  先查看在报警方案中是否存在解决方案， 如果有则生成工单，如果没有则不生成，可以在报警监控中手动派发工单
        /// 工单派发人员，先看是否存在身份是客服的设备所属人员，有即可直接生成工单推送，没有则生成无人员工单
        /// </summary>
        /// <param name="rtuid">设备id</param>
        /// <param name="eventSource">报警来源</param>
        /// <returns></returns>
        public async Task SendWo(long rtuid, short eventSource)
        {
            await Task.Run(() =>
            {
                //State=1  发生
                var eventInfo = _EventInfoService.Query<SwsEventInfo>(r => r.Rtuid == rtuid && r.EventSource == eventSource).FirstOrDefault();
                //查询是否存在解决方案
                string filter = "1=1";
                string[] strList = null;
                if (!string.IsNullOrEmpty(eventInfo?.EventMessage))
                {
                    filter += " and ( ";
                    strList = SplitContent.SplitWords(eventInfo?.EventMessage);
                    foreach (var item in strList)
                    {
                        filter += " Title like '%" + item + "%' or Contents like '%" + item + "%' or Solution like '%" + item + "%' OR";
                    }
                    filter = filter.Substring(0, filter.Length - 2);
                    filter += " ) ";
                }
                var datalist = _sys_EarlyWarningPlanService.QueryEventPlans(filter);
                if (datalist.Count() > 0)
                {
                    List<EarlyWarningPlan> listplan = new List<EarlyWarningPlan>();
                    string ids = "";
                    foreach (var item in datalist)
                    {
                        EarlyWarningPlan plan = new EarlyWarningPlan();
                        plan = SplitContent.SetHighlighter(strList, item);
                        listplan.Add(plan);
                        ids += plan.Id + ',';
                    }
                    if (listplan.Count > 0)
                    {
                        var info = _wO_WorkOrderService.LoadUserInfoByRtuid(rtuid).ToList().FirstOrDefault();
                        int userId = int.Parse(info?.UserID.ToString());
                        if (eventInfo != null)
                        {
                            //转工单  先生成维修事件，再生成工单
                            Random random = new Random();
                            //生成事件
                            WoEvents we = new WoEvents();
                            we.IncidentId = ConvertDateTimeInt(DateTime.Now);
                            we.IncidentNum = "WX" + DateTime.Now.ToString("yyyyMMddHHmmss");
                            we.IncidentState = 0;
                            we.IncidentType = 2;
                            we.IncidentContent = eventInfo.EventMessage;
                            we.IncidentSource = 2;
                            we.ReportTime = DateTime.Now;
                            if (userId != 0)
                            {
                                we.ReportUser = userId;
                            }

                            we.DisposeState = 1;
                            we.EquipmentId = rtuid;
                            we.StationId = long.Parse(info?.Account);

                            WoWorkOrder woWorkOrder = new WoWorkOrder();
                            woWorkOrder.Woid = ConvertDateTimeInt(DateTime.Now);
                            woWorkOrder.CurrentState = (short)WoState.未分派;
                            woWorkOrder.Num = "WO_WX_" + ConvertDateTimeInt(DateTime.Now) + random.Next(1, 100);
                            woWorkOrder.EventId = we.IncidentId;
                            woWorkOrder.CurrentState = -1;
                            if (eventInfo.EventLevel == 1)
                            {
                                //非常紧急
                                woWorkOrder.HandleLevel = (byte)ProcessingLevel.十二小时;
                                woWorkOrder.CompleteTime = DateTime.Now.AddHours(12);
                                woWorkOrder.Degree = (byte)EmergencyDegree.非常紧急;
                            }
                            else if (eventInfo.EventLevel == 2)
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
                            woWorkOrder.EarlyWarningPlanId = ids.Substring(0, ids.Length - 1);
                            _wO_WorkOrderService.InsertWoandEvent(woWorkOrder, we);
                        }
                    }

                }

            });
        }

        public static long ConvertDateTimeInt(System.DateTime time)
        {
            System.DateTime startTime = System.TimeZoneInfo.ConvertTimeToUtc(new System.DateTime(1970, 1, 1));
            return (long)(time - startTime).TotalMilliseconds;
        }
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult GetDeviceAlarmList(int pagesize)
        {
            int TotalCount = 0;

            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = _UserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
            string filter = "1=1";
            var f_itemid = (int)Model.CustomizedClass.Enum.设备分区;
            var data = _EventInfoService.GetAlarmList(userID, user.IsAdmin, filter, 1, pagesize, f_itemid, ref TotalCount);
            return Json(new
            {
                data = data,
                TotalCount = TotalCount
            });
        }
        //标记为已读
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult OperateRead(string eventid)
        {
            var query = _EventInfoService.OperateReadAlarm(eventid);
            if (query > 0)
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }
        }
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult OperateRead_all()
        {
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = _UserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
            var query = _EventInfoService.OperateReadAlarm_All(userID, user.IsAdmin);
            if (query > 0)
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }
        }
        //判断是否可以接收该报警
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult CheckAction(string ids)
        {
            var idflag = ids.Split(',');
            var rtuid = long.Parse(idflag[0]);
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = _UserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
            if (user.IsAdmin == true)
            {
                return Content("ok");
            }
            else
            {
                var reslut = "no";
                var swsrtu = _RTUInfoService.Query<SwsRtuinfo>(r => r.Rtuid == rtuid).FirstOrDefault();
                if (swsrtu != null)
                {
                    var usestation = _UserStationService.Query<SwsUserStation>(r => r.UserId == userID && r.StationId == swsrtu.StationId).FirstOrDefault();
                    if (usestation != null)
                    {
                        reslut = "ok";
                    }
                }
                return Content(reslut);
            }

        }
        #endregion

        #region 报警自动生成工单
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult CreatWorkOrder(int rtuid, short source, string message, string eventtime, byte eventLevel)
        {
            //查询当前用户
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = _UserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
            //查询是否为自动生成的报警
            SwsEventScheme es = _EventInfoService.Query<SwsEventScheme>(r => r.DataId == source && r.Rtuid == rtuid).FirstOrDefault();
            //查询当前报警是否已经生成工单
            SwsEventInfo einfo = _EventInfoService.Query<SwsEventInfo>(r => r.EventSource == source && r.Rtuid == rtuid && r.EventTime == Convert.ToDateTime(eventtime)).FirstOrDefault();
            List<WoEvents> we = _wO_EventsService.Query<WoEvents>(r => r.EventId == einfo.Id).ToList();

            //查询设备信息
            SwsDeviceInfo01 dinfo = _RTUInfoService.Query<SwsDeviceInfo01>(r => r.Rtuid == rtuid).FirstOrDefault();
            //查询泵房信息
            int stid = dinfo == null ? 0 : dinfo.StationId;
            SwsStation sta = _RTUInfoService.Query<SwsStation>(r => r.StationId == stid).FirstOrDefault();

            if (es != null && we.Count == 0)
            {
                long woid = 0;
                if (_EventInfoService.CreatOrder(einfo.Id, rtuid, user.UserId, message, eventLevel,ref woid) > 0)
                {
                    if (sta != null && sta.Manager != null)
                    {
                        PushOrder(sta.Manager, message, woid);
                    }
                    return Content("ok");
                }
                else
                {
                    logger.LogInformation("报警自动生成工单失败，【设备名称】："+dinfo?.DeviceName+"【报警信息】："+ message + "，【报警字段】："+source+",【报警时间】："+ eventtime + "");
                    return Content("no");
                }
            }
            else
            {
                return Content("no");
            }
        }
        public void PushOrder(long? userid,string message,long woid)
        {
            Jpush jpush = new Jpush();
            try
            {
                    PushPayload pushPayload = new PushPayload()
                    {
                        Platform = new List<string> { "android", "ios" },
                        Audience = new Audience
                        {
                            Alias = new List<string> { userid.ToString() }
                        },
                        Message = new Message
                        {
                            Title = woid.ToString(),
                            Content = "报警生成的维修工工单，报警内容："+message+""
                        }
                    };
                    jpush.ExcutePush(pushPayload);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
