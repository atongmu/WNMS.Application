using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using WNMS.Application.Utility;
using WNMS.Application.Utility.Filters;
using WNMS.IService;
using WNMS.Model.CustomizedClass;
using WNMS.Model.DataModels;

namespace WNMS.Application.Areas.Wo.Controllers
{
    [Area("Wo")]
    public class WorkHandleController : Controller
    {
        private IWO_WorkOrderService _wO_WorkOrderService = null;
        private ISysUserService _sysUserService = null;
        private IWO_EventsService _wO_EventsService = null;
        private IWO_ResourceService _wO_ResourceService = null;
        private IWO_TeamInfoService _wO_TeamInfoService = null;
        private IWO_TeamUserService _wO_TeamUserService = null;
        private ISws_StationService _sws_StationService = null;
        private IWO_ForwardService _wO_ForwardService = null;
        private ISws_DeviceInfo01Service _sws_DeviceInfo01Service = null;
        private IWO_WOExtensionService _wO_WOExtensionService = null;
        public WorkHandleController(IWO_WorkOrderService wO_WorkOrderService, ISysUserService sysUserService, IWO_EventsService wO_EventsService,
            IWO_ResourceService wO_ResourceService, IWO_TeamInfoService wO_TeamInfoService, IWO_TeamUserService wO_TeamUserService, ISws_StationService sws_StationService,
            IWO_ForwardService wO_ForwardService, ISws_DeviceInfo01Service sws_DeviceInfo01Service, IWO_WOExtensionService wO_WOExtensionService
            )
        {
            _wO_WorkOrderService = wO_WorkOrderService;
            _sysUserService = sysUserService;
            _wO_EventsService = wO_EventsService;
            _wO_ResourceService = wO_ResourceService;
            _wO_TeamInfoService = wO_TeamInfoService;
            _wO_TeamUserService = wO_TeamUserService;
            _sws_StationService = sws_StationService;
            _wO_ForwardService = wO_ForwardService;
            _sws_DeviceInfo01Service = sws_DeviceInfo01Service;
            _wO_WOExtensionService = wO_WOExtensionService;
        }
        public IActionResult Index()
        {
            return View();
        }
        //查询待办工单
        [TypeFilter(typeof(IgonreActionFilter))]
        [HttpPost]
        public async Task<IActionResult> GetWorkOrderData(string date, string State, int pageSize, int pageIndex, string field, string order, string eventsType, long UserID, string message, string BeginTime, string EndTime)
        {
            string endDate = "";
            string beginDate = GetDate(date, ref endDate);
            int totalCount = 0;
            int d = Convert.ToInt32(date);
            if (d == 6)
            {
                beginDate = BeginTime;
                endDate = EndTime;
            }
            if (string.IsNullOrEmpty(message))
            {
                message = "";
            }
            var wolist = _wO_WorkOrderService.GetWorkTable(beginDate, endDate, State, pageSize, pageIndex, field, order, eventsType, UserID, message, ref totalCount).ToList();
            PartialView("_WorkOrderTable", wolist);
            string dataTable = await ViewToString.RenderPartialViewToString(this, "_WorkOrderTable");
            double totalPage = Math.Ceiling((double)totalCount / pageSize);
            return Json(new
            {
                total = totalCount,
                totalPage = totalPage,
                pageIndex = pageIndex,
                pageSize = pageSize,
                dataTable = dataTable
            });
        }
        [TypeFilter(typeof(IgonreActionFilter))]
        public string GetDate(string date, ref string endDate)
        {
            string beginDate = "";
            if (!string.IsNullOrEmpty(date))
            {
                int d = Convert.ToInt32(date);
                DateTime dt = DateTime.Now;
                switch (d)
                {
                    case 1:
                        beginDate = dt.Date.AddDays(-1).ToString("yyyy-MM-dd");
                        endDate = dt.Date.ToString("yyyy-MM-dd");
                        break;
                    case 2:
                        beginDate = dt.Date.AddDays(1 - Convert.ToInt32(dt.DayOfWeek.ToString("d"))).ToString("yyyy-MM-dd");
                        endDate = dt.Date.AddDays(1).ToString("yyyy-MM-dd");
                        break;
                    case 3:
                        beginDate = dt.Date.AddDays(Convert.ToInt32(1 - Convert.ToInt32(DateTime.Now.DayOfWeek)) - 7).ToString("yyyy-MM-dd");
                        endDate = dt.Date.AddDays(Convert.ToInt32(1 - Convert.ToInt32(DateTime.Now.DayOfWeek)) - 7).AddDays(7).ToString("yyyy-MM-dd");
                        break;
                    case 4:
                        beginDate = dt.ToString("yyyy-MM-01");
                        endDate = dt.AddDays(1).ToString("yyyy-MM-dd");
                        break;
                    case 5:
                        beginDate = DateTime.Parse(dt.ToString("yyyy-MM-01")).AddMonths(-1).ToShortDateString();
                        endDate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-01")).ToShortDateString();
                        break;
                    default:
                        beginDate = "";
                        endDate = "";
                        break;
                }
            }
            else
            {
                beginDate = "";
                endDate = "";
            }
            return beginDate;
        }

        //工单详情
        [TypeFilter(typeof(IgonreActionFilter))]
        public ActionResult LoadWODetails(long id)
        {
            //根据ID获取并返回待处理事件信息
            var wo_eventInfo = _wO_WorkOrderService.GetWO_EventData(id);
            ViewBag.wo_event = wo_eventInfo;


            //获取当前登录用户
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = _sysUserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();


            ViewBag.User = user;

            //返回工单ID
            ViewBag.WOID = id;

            //返回事件图片,缩略图使用
            var woinfo = _wO_WorkOrderService.Query<WoWorkOrder>(r => r.Woid == id).FirstOrDefault();
            var eventinfo = _wO_EventsService.Query<WoEvents>(R => R.IncidentId == woinfo.EventId).FirstOrDefault()?.IncidentId;
            List<WoResource> img = _wO_ResourceService.Query<WoResource>(r => r.Pid == eventinfo && r.Type == 1 && r.ResourceType == 5).ToList();
            ViewBag.Img = img;
            //获取工单操作历史 
            var opData = _wO_WorkOrderService.GetWO_OpData(id, 1);
            ViewBag.opData = opData;
            //获取维修历史 
            var opcData = _wO_WorkOrderService.GetWO_OpInfo(id);
            ViewBag.opcData = opcData;
            //延期申请
            var extension = _wO_WorkOrderService.LoadExtensionData(id).ToList();
            ViewBag.extension = extension;
            //获取退单信息
            var tdData = _wO_WorkOrderService.GetWO_OpData(id, 6);
            ViewBag.tdData = tdData;
            //获取维修负责人信息
            var userid = woinfo.ReceiveUser;
            var userInfo = _sysUserService.Query<SysUser>(r => r.UserId == userid).FirstOrDefault();
            //ViewBag.userInfo = userInfo?.Account;
            ViewBag.userInfo = userInfo?.NickName;
            //班组
            var teamid = _wO_TeamUserService.Query<WoTeamUser>(r => r.UserId == userid).FirstOrDefault()?.TeamId;
            var depInfo = _wO_TeamInfoService.Query<WoTeamInfo>(r => r.TeamId == teamid).FirstOrDefault();
            if (depInfo != null)
            {
                ViewBag.depName = depInfo.TeamName;
            }
            else
            {
                ViewBag.depName = "";
            }
            return View();
        }

        [TypeFilter(typeof(IgonreActionFilter))]
        public ActionResult Positioning(long id)
        {
            //工单信息
            // var woinfo = _gD_WorkOrderService.Query<GdWorkOrder>(r => r.Woid == id).FirstOrDefault();
            var eventinfo = _wO_EventsService.Query<WoEvents>(r => r.IncidentId == id).FirstOrDefault();
            if (eventinfo.StationId != null && eventinfo.StationId != 0)
            {
                var stationoinfo = _sws_StationService.Query<SwsStation>(r => r.StationId == eventinfo.StationId).FirstOrDefault();
                ViewBag.StationNum = stationoinfo.StationNum;
                ViewBag.StationName = stationoinfo.StationName;
            }
            else
            {
                ViewBag.StationNum = "";
                ViewBag.StationName = "";
            }
            ViewBag.Lat = eventinfo.Lat;
            ViewBag.Lng = eventinfo.Lng;
            return View();
        }

        //工单详情 图片信息
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult ShowImage(long id, int type, int ResourceType)
        {
            List<WoResource> resource = _wO_ResourceService.Query<WoResource>(r => r.Pid == id && r.Type == type && r.ResourceType == ResourceType).ToList();
            ViewBag.Resource = resource;
            return View();
        }
        //音频
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult ShowAudio(long id, int type, int ResourceType)
        {
            ViewBag.ID = id;
            ViewBag.type = type;
            ViewBag.ResourceType = ResourceType;
            return View();
        }
        /// <summary>
        /// 加载音频文件
        /// </summary>
        /// <param name="id">事件/工单ID</param>
        /// <param name="ResourceType">音频来源</param>
        /// <returns></returns>
        [TypeFilter(typeof(IgonreActionFilter))]
        public ActionResult LoadAudio(long id, short type, short ResourceType)
        {
            List<WoResource> resource = _wO_ResourceService.Query<WoResource>(r => r.Pid == id && r.Type == type && r.ResourceType == ResourceType).ToList();
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(resource));
        }

        //时间戳
        public static long ConvertDateTimeInt(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (long)(time - startTime).TotalMilliseconds;
        }
        /// <summary>
        /// 退单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult ChargebackWO(long id)
        {
            var WOInfo = _wO_WorkOrderService.Query<WoWorkOrder>(r => r.Woid == id).FirstOrDefault();
            if (WOInfo.CurrentState != (short)Model.CustomizedClass.WoState.已退单 && WOInfo.CurrentState < (short)Model.CustomizedClass.WoState.已到场)
            {
                WOInfo.CurrentState = (short)Model.CustomizedClass.WoState.已退单;
                //WOInfo.CurrentState = (short)Model.CustomizedClass.Enum.WoState.待接收;
                DateTime? dt = WOInfo.ReleaseTime == null ? DateTime.Now : WOInfo.ReleaseTime;
                WOInfo.CompleteTime = (DateTime)dt;
                //返回当前登录用户
                int userID = int.Parse(User.FindFirstValue("UserID"));
                var user = _sysUserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
                WoWooperation woWooperation = new WoWooperation();
                woWooperation.Id = ConvertDateTimeInt(DateTime.Now);
                woWooperation.Pid = id;
                woWooperation.OperationTime = DateTime.Now;
                woWooperation.UserId = user == null ? 0 : user.UserId;
                woWooperation.State = (short)Model.CustomizedClass.WoState.已退单;
                woWooperation.Type = (short)Model.CustomizedClass.WOOperationType.退单;
                woWooperation.Description = "退单";
                //查询一下有没有用户申请的退单信息，如果有，直接通过审核
                var returnWo = _wO_ForwardService.Query<WoForward>(r => r.Woid == id && r.Type == 1).OrderByDescending(r => r.ExtensionTime).FirstOrDefault();
                if (returnWo != null)
                {
                    returnWo.State = 1;
                    returnWo.Auditor = user == null ? 0 : user.UserId;
                    returnWo.AuditingTime = DateTime.Now;
                }
                //查询一下有没有延期申请，如果有
                var exInfo = _wO_WOExtensionService.Query<WoWoextension>(r => r.Woid == id).OrderByDescending(r => r.ExtensionTime).FirstOrDefault();
                if (exInfo != null)
                {
                    exInfo.State = 2;
                    exInfo.Auditor = user == null ? 0 : user.UserId;
                    exInfo.AuditingTime = DateTime.Now;
                }
                //if (_wO_WorkOrderService.ChargebackWO(WOInfo, woWooperation) > 0)
                if (_wO_WorkOrderService.ChargebackWOW(WOInfo, woWooperation, returnWo, exInfo) > 0)
                {
                    return Content("ok");
                }
                else
                {
                    return Content("no");
                }
            }
            else
            {
                return Content("repeat");
            }

        }



        /// <summary>
        /// 驳回重做
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult TurnDown(long id)
        {
            /*工单驳回，工单状态修改为驳回状态，操作历史表插入一条驳回的记录
             手机端需要重新接单，然后再继续操作
             */
            var WOInfo = _wO_WorkOrderService.Query<WoWorkOrder>(r => r.Woid == id).FirstOrDefault();
            if (WOInfo.CurrentState == (short)Model.CustomizedClass.WoState.已审核)
            {
                return Content("cannot");
            }
            if (WOInfo.CurrentState == (short)Model.CustomizedClass.WoState.已完工)
            {
                //WOInfo.CurrentState = (short)Model.CustomizedClass.WoState.驳回; 
                WOInfo.CurrentState = (short)Model.CustomizedClass.WoState.待接收;
                //返回当前登录用户
                int userID = int.Parse(User.FindFirstValue("UserID"));
                var user = _sysUserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
                WoWooperation woWooperation = new WoWooperation();
                woWooperation.Id = ConvertDateTimeInt(DateTime.Now);
                woWooperation.Pid = id;
                woWooperation.OperationTime = DateTime.Now;
                woWooperation.UserId = user == null ? 0 : user.UserId;
                woWooperation.State = (short)Model.CustomizedClass.WoState.驳回;
                woWooperation.Type = (short)Model.CustomizedClass.WOOperationType.驳回;
                woWooperation.Description = "驳回";
                if (_wO_WorkOrderService.ChargebackWO(WOInfo, woWooperation) > 0)
                {
                    return Content("ok");
                }
                else
                {
                    return Content("no");
                }
            }
            else
            {
                return Content("cannot");
            }

        }



        /// <summary>
        /// 审核工单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult ReviewWO(long id)
        {
            var WOInfo = _wO_WorkOrderService.Query<WoWorkOrder>(r => r.Woid == id).FirstOrDefault();
            if (WOInfo.CurrentState != (short)Model.CustomizedClass.WoState.已审核)
            {
                WOInfo.CurrentState = (short)Model.CustomizedClass.WoState.已审核;
                WOInfo.IsAuditing = 1;
                WOInfo.AuditingContent = "";
                //返回当前登录用户
                int userID = int.Parse(User.FindFirstValue("UserID"));
                var user = _sysUserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
                WoWooperation woWooperation = new WoWooperation();
                woWooperation.Id = ConvertDateTimeInt(DateTime.Now);
                woWooperation.Pid = id;
                woWooperation.OperationTime = DateTime.Now;
                woWooperation.UserId = user == null ? 0 : user.UserId;
                woWooperation.State = (short)Model.CustomizedClass.WoState.已审核;
                woWooperation.Type = (short)Model.CustomizedClass.WOOperationType.审核;
                woWooperation.Description = "审核通过";
                if (_wO_WorkOrderService.ChargebackWO(WOInfo, woWooperation) > 0)
                {
                    return Content("ok");
                }
                else
                {
                    return Content("no");
                }
            }
            else
            {
                return Content("repeat");
            }

        }

        /// <summary>
        /// 工单审核页面
        /// </summary>
        /// <param name="id"></param> 
        /// <returns></returns>
        [TypeFilter(typeof(IgonreActionFilter))]
        public ActionResult WorkOrderAudit(long id)
        {
            ViewBag.WOID = id;
            ViewBag.State = WOExtensionReview.审核通过;
            return View();
        }
        /// <summary>
        /// 工单审核操作
        /// </summary>
        /// <param name="woID"></param>
        /// <param name="state"></param>
        /// <param name="treatidea"></param>
        /// <returns></returns>
        [TypeFilter(typeof(IgonreActionFilter))]
        public ActionResult SetOrderAudit(long woID, byte state, string treatidea)
        {
            //返回当前登录用户
            int userID = int.Parse(User.FindFirstValue("UserID"));

            WoWorkOrder w = _wO_WorkOrderService.Query<WoWorkOrder>(r => r.Woid == woID).FirstOrDefault();
            if (w.CurrentState != (byte)WOOperationType.维修完工)
            {
                return Content("notFinish");
            }
            if (w != null)
            {
                //工单数据 处理
                if (state == 1)
                {
                    w.IsAuditing = (byte)WOExtensionReview.审核通过;
                }
                else
                {
                    w.IsAuditing = (byte)WOExtensionReview.审核未通过;
                }
                w.CurrentState = (byte)WoState.已审核;
                w.AuditingContent = treatidea;

                //工单历史操作
                WoWooperation woo = new WoWooperation();
                woo.Description = treatidea;
                woo.Id = ConvertDateTimeInt(DateTime.Now);
                woo.OperationTime = DateTime.Now;
                woo.Pid = woID;
                woo.State = 0;
                woo.Type = (short)WOOperationType.审核;
                woo.UserId = userID;

                if (_wO_WorkOrderService.ChargebackWO(w, woo) > 0)
                {
                    return Content("ok");
                }
                else
                {
                    return Content("no");
                }
            }
            else
            {
                return Content("false");
            }
        }
        //分派工单
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult DispatchWo(long id)
        {
            //班组树 
            string crs_Tree = GetUserTree();
            if (crs_Tree != "")
            {
                ViewBag.TreeNode = new HtmlString(crs_Tree);
            }
            else
            {
                ViewBag.TreeNode = new HtmlString("[]");
            }

            ViewBag.WoID = id;

            ViewBag.CTime = "";
            return View();
        }

        //获取班组树
        [TypeFilter(typeof(IgonreActionFilter))]
        public string GetUserTree()
        {
            //获取班组树
            List<TeamUser> tu = _wO_WorkOrderService.GetTeamInfo().ToList();
            if (tu.Count() > 0)
            {
                var Team = tu.Select(r => new TreeAction
                {
                    id = r.TeamID,
                    pId = 0,
                    name = r.TeamName,
                    icon = "../../../images/quyu.png",
                    @checked = false,
                    type = 0
                });
                var CsrInfo = tu.Select(r => new TreeAction
                {
                    id = r.UserID,
                    pId = r.TeamID,
                    //name = r.Account,
                    name = r.NickName,
                    icon = "../../../images/renyuan.png",
                    @checked = false,
                    type = 1
                });
                var dataList = Team.Distinct(new TreeAreaCompare()).Union<TreeAction>(CsrInfo).OrderBy(r => r.name);
                return Newtonsoft.Json.JsonConvert.SerializeObject(dataList);
            }
            else
            {
                return "";
            }
        }
        /// <summary>
        /// 派工单
        /// </summary>
        /// <param name="WOID"></param>
        /// <param name="mid"></param>
        /// <param name="CompDate"></param>
        /// <param name="Description"></param>
        /// <returns></returns>

        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult AddDispatchWo(long WOID, int mid, string CompDate, string Description)
        {
            //获取当前登录用户
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = _sysUserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
            var woInfo = _wO_WorkOrderService.Query<WoWorkOrder>(r => r.Woid == WOID).FirstOrDefault();
            //如果工单已接收，不能进行再次分派
            if (woInfo.CurrentState >= 0)
            {
                return Content("isIng");
            }
            //修改当前工单状态
            woInfo.CurrentState = (short)Model.CustomizedClass.WoState.待接收;
            //woInfo.CompleteTime = (DateTime)woInfo.ReleaseTime;
            woInfo.ReceiveUser = mid;
            woInfo.CompleteTime = DateTime.Parse(CompDate);
            woInfo.ReleaseUser = userID;
            woInfo.ReleaseTime = DateTime.Now;
            Random random = new Random();
            //工单历史数据
            WoWooperation woop = new WoWooperation();
            woop.Description = Description;
            woop.Id = ConvertDateTimeInt(DateTime.Now) + random.Next(1, 100);
            woop.OperationTime = DateTime.Now;
            woop.Pid = WOID;
            woop.State = 0;
            woop.Type = (short)Model.CustomizedClass.WOOperationType.派发;
            woop.UserId = user == null ? 0 : user.UserId;
            WoForward woForward = new WoForward();
            woForward = _wO_ForwardService.Query<WoForward>(r => r.Woid == WOID).FirstOrDefault();
            if (woForward != null)
            {
                woForward.RecipientId = mid;
            }

            //提交
            if (_wO_WorkOrderService.AddDispatchWo(woInfo, woop, woForward) > 0)
            {
                //WorkOrderJpush jp = new WorkOrderJpush();
                //jp.GetTageList();
                //string[] userIDs = { order.UserId.ToString() };
                //string JpushContent = "收到新的工单(" + order.Woid + "),请注意查收！";
                //string JpushMark = "新增";
                //jp.Jpush(userIDs, JpushContent, JpushMark, ebeninfo.IncidentType);
                return Content("ok");
            }
            else
            {
                return Content("no");
            }
        }
        /// <summary>
        /// 查询工单是否已经分派
        /// </summary>
        /// <param name="WOID"></param>
        /// <returns></returns>
        public IActionResult WoIsDispatch(long WOID)
        {
            var woInfo = _wO_WorkOrderService.Query<WoWorkOrder>(r => r.Woid == WOID).FirstOrDefault();
            //如果工单已接收，不能进行再次分派
            if (woInfo.CurrentState >= 0)
            {
                return Content("isIng");
            }
            else
            {
                return Content("ok");
            }
        }
        #region 手动派发工单
        /// <summary>
        /// 手动派发工单
        /// </summary>
        /// <returns></returns>
        public IActionResult ManualDispatch()
        {
            //班组树 
            string crs_Tree = GetUserTree();
            if (crs_Tree != "")
            {
                ViewBag.TreeNode = new HtmlString(crs_Tree);
            }
            else
            {
                ViewBag.TreeNode = new HtmlString("[]");
            }

            ViewBag.CTime = "";
            //泵房
            var Station = _sws_StationService.Query<SwsStation>(r => true).ToList();
            ViewBag.station = Station;
            return View();
        }
        /// <summary>
        /// 根据泵房ID
        /// </summary>
        /// <param name="id">泵房ID</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult LoadEq(long id)
        {
            var eqList = _sws_DeviceInfo01Service.Query<SwsDeviceInfo01>(r => r.StationId == id).ToList();
            return Json(new
            {
                eqList
            });
        }
        /// <summary>
        ///  手动派工单
        /// </summary>
        /// <param name="mid">接收人</param>
        /// <param name="CompDate">完成时间</param>
        /// <param name="Description">备注</param>
        /// <param name="level">处理级别</param>
        /// <param name="degree">紧急程度</param>
        /// <param name="stationid">泵房ID</param>
        /// <param name="eqid">设备ID</param>
        /// <param name="eventContent">事件描述</param>
        /// <returns></returns>
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult AddManualDispatch(int mid, string CompDate, string Description, short level, short degree, int stationid, long eqid, string eventContent)
        {
            //获取当前登录用户
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = _sysUserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
            Random random = new Random();
            var Station = _sws_StationService.Query<SwsStation>(r => r.StationId == stationid).FirstOrDefault();
            //生成事件
            WoEvents woEvents = new WoEvents();
            woEvents.IncidentId = ConvertDateTimeInt(DateTime.Now) + random.Next(1, 10000);
            woEvents.IncidentNum = "BX_" + woEvents.IncidentId;
            woEvents.ReportUser = userID;
            woEvents.ReportTime = DateTime.Now;
            woEvents.StationId = stationid;
            woEvents.EquipmentId = eqid;
            woEvents.IncidentContent = eventContent;
            woEvents.Description = eventContent;
            woEvents.DisposeState = 1;
            woEvents.IncidentType = 2;
            woEvents.IncidentSource = 1;
            woEvents.Lng = (decimal)Station.Lng;
            woEvents.Lat = (decimal)Station.Lat;
            woEvents.Type = 1;
            //生成工单
            var beginTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 00:00:00"));
            var endTime = DateTime.Parse(DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00"));
            var woCount = _wO_WorkOrderService.Query<WoWorkOrder>(r => r.ReleaseTime >= beginTime && r.ReleaseTime <= endTime).ToList().Count();
            string num = "0";
            if (woCount == 0)
            {
                num = "001";
            }
            else if (woCount < 10)
            {
                int tenCount = woCount + 1;
                num = "00" + tenCount;
            }
            else if (woCount >= 10 && woCount < 100)
            {
                num = "0" + woCount;
            }
            WoWorkOrder woWorkOrder = new WoWorkOrder();
            woWorkOrder.Woid = ConvertDateTimeInt(DateTime.Now) + random.Next(1, 10000);
            woWorkOrder.Degree = degree;
            woWorkOrder.HandleLevel = level;
            woWorkOrder.ReleaseTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            woWorkOrder.EventId = woEvents.IncidentId;
            woWorkOrder.CurrentState = (short)WoState.待接收;
            woWorkOrder.Num = "WO_WX_" + DateTime.Now.ToString("yyyyMMdd") + num;
            woWorkOrder.IsAuditing = (byte)WOExtensionReview.未审核;
            woWorkOrder.ReleaseUser = userID;
            woWorkOrder.CompleteTime = DateTime.Parse(CompDate);
            woWorkOrder.ReceiveUser = mid;
            //生成工单操作记录
            WoWooperation woWooperation = new WoWooperation();
            woWooperation.Id = ConvertDateTimeInt(DateTime.Now) + random.Next(1, 10000);
            woWooperation.UserId = userID;
            woWooperation.OperationTime = DateTime.Now;
            woWooperation.Pid = woWorkOrder.Woid;
            woWooperation.Type = (short)WOOperationType.派发;
            woWooperation.State = 0;
            woWooperation.Description = Description;
            if (_wO_WorkOrderService.AddManualDispatch(woEvents, woWorkOrder, woWooperation) > 0)
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }


        }
        #endregion
    }
}