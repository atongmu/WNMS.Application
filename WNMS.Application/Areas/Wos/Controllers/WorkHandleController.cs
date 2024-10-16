using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WNMS.Application.Utility;
using WNMS.Application.Utility.Filters;
using WNMS.Application.Utility.Jpush;
using WNMS.IService;
using WNMS.Model.CustomizedClass;
using WNMS.Model.DataModels;

namespace WNMS.Application.Areas.Wos.Controllers
{
    [Area("Wos")]
    public class WorkHandleController : Controller
    {
        private ISysUserService _UserService = null;
        private IGD_WorkOrderService _gD_WorkOrderService = null;
        private ISys_DataItemDetailService _DataItemDetailService = null;
        private IGD_WOOperationService _gD_WOOperationService = null;
        private IGD_EventsService _gD_EventsService = null;
        private IGD_InspectionService _gD_InspectionService = null;
        private IGD_MaintainService _gD_MaintainService = null;
        private IGD_RepairService _gD_RepairService = null;
        private ISws_StationService _sws_StationService = null;
        private IGD_ResourceService _gD_ResourceService = null;
        private IGD_EventsService eventService = null;
        private IGD_TeamUserService _gD_TeamUserService = null;
        private IGD_TeamInfoService _gD_TeamInfoService = null;
        public WorkHandleController(
            ISysUserService sysUserService,
            IGD_WorkOrderService gD_WorkOrderService,
            ISys_DataItemDetailService sys_DataItemDetailService, IGD_WOOperationService gD_WOOperationService,
            IGD_EventsService gD_EventsService,
            IGD_InspectionService gD_InspectionService,
            IGD_MaintainService gD_MaintainService,
            IGD_RepairService gD_RepairService,
            ISws_StationService sws_StationService,
            IGD_ResourceService gD_ResourceService,
            IGD_EventsService gd_EventService,
            IGD_TeamUserService gD_TeamUserService,
            IGD_TeamInfoService gD_TeamInfoService
            )
        {
            _UserService = sysUserService;
            _gD_WorkOrderService = gD_WorkOrderService;
            _DataItemDetailService = sys_DataItemDetailService;
            _gD_WOOperationService = gD_WOOperationService;
            _gD_EventsService = gD_EventsService;
            _gD_InspectionService = gD_InspectionService;
            _gD_MaintainService = gD_MaintainService;
            _gD_RepairService = gD_RepairService;
            _sws_StationService = sws_StationService;
            _gD_ResourceService = gD_ResourceService;
            eventService = gd_EventService;
            _gD_TeamUserService = gD_TeamUserService;
            _gD_TeamInfoService = gD_TeamInfoService;
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
            var wolist = _gD_WorkOrderService.GetWorkTable(beginDate, endDate, State, pageSize, pageIndex, field, order, eventsType, UserID, message, ref totalCount).ToList();
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
            var wo_eventInfo = _gD_WorkOrderService.GetWO_EventData(id);
            ViewBag.wo_event = wo_eventInfo;


            //获取当前登录用户
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = _UserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();


            ViewBag.User = user;

            //返回工单ID
            ViewBag.WOID = id;

            //返回事件图片,缩略图使用
            var woinfo = _gD_WorkOrderService.Query<GdWorkOrder>(r => r.Woid == id).FirstOrDefault();
            var eventinfo = eventService.Query<GdEvents>(R => R.IncidentId == woinfo.EventId).FirstOrDefault()?.TaskId;
            List<GdResource> img = _gD_ResourceService.Query<GdResource>(r => r.Pid == eventinfo && r.Type == 1 && r.ResourceType == 1).ToList();
            ViewBag.Img = img;
            //获取工单操作历史 
            var opData = _gD_WorkOrderService.GetWO_OpData(id, 1);
            ViewBag.opData = opData;
            //获取维修历史 
            var opcData = _gD_WorkOrderService.GetWO_OpData(id, 3);
            ViewBag.opcData = opcData;
            //延期申请
            var extension = _gD_WorkOrderService.LoadExtensionData(id).ToList();
            ViewBag.extension = extension;
            //获取退单信息
            var tdData = _gD_WorkOrderService.GetWO_OpData(id, 6);
            ViewBag.tdData = tdData;
            //获取维修负责人信息
            var userid = woinfo.UserId;
            var userInfo = _UserService.Query<SysUser>(r => r.UserId == userid).FirstOrDefault();
            ViewBag.userInfo = userInfo.Account;
            //班组
            var teamid = _gD_TeamUserService.Query<GdTeamUser>(r => r.UserId == userid).FirstOrDefault()?.TeamId;
            var depInfo = _gD_TeamInfoService.Query<GdTeamInfo>(r => r.TeamId == teamid).FirstOrDefault();
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

        //退单
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult ChargebackWO(long id)
        {
            var WOInfo = _gD_WorkOrderService.Query<GdWorkOrder>(r => r.Woid == id).FirstOrDefault();
            //if (WOInfo.CurrentState != 0 && WOInfo.CurrentState != (short)Model.CustomizedClass.Enum.WoState.已退单)
            if (WOInfo.CurrentState != (short)Model.CustomizedClass.WoState.已退单)
            {
                WOInfo.CurrentState = (short)Model.CustomizedClass.WoState.已退单;
                //WOInfo.CurrentState = (short)Model.CustomizedClass.Enum.WoState.待接收;
                DateTime? dt = WOInfo.ReleaseTime == null ? DateTime.Now : WOInfo.ReleaseTime;
                WOInfo.CompleteTime = (DateTime)dt;
                //返回当前登录用户
                int userID = int.Parse(User.FindFirstValue("UserID"));
                var user = _UserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
                GdWooperation gd_WOOperation = new GdWooperation();
                gd_WOOperation.Id = ConvertDateTimeInt(DateTime.Now);
                gd_WOOperation.Pid = id;
                gd_WOOperation.OperationTime = DateTime.Now;
                gd_WOOperation.UserId = user == null ? 0 : user.UserId;
                gd_WOOperation.State = (short)Model.CustomizedClass.WoState.已退单;
                gd_WOOperation.Type = (short)Model.CustomizedClass.WOOperationType.退单;
                gd_WOOperation.Description = "退单";
                if (_gD_WorkOrderService.EditWo(WOInfo, gd_WOOperation) > 0)
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

        //移交工单
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult TransferWo(long id)
        {
            //获取班组树
            List<TeamUser> tu = eventService.GetTeamInfo().ToList();
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

            ViewBag.EventID = id;

            ViewBag.CTime = "";
            return View();
        }
        //获取班组树
        [TypeFilter(typeof(IgonreActionFilter))]
        public string GetUserTree()
        {
            //获取班组树
            List<TeamUser> tu = eventService.GetTeamInfo().ToList();
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
                    name = r.Account,
                    icon = "../../../images/renyuan.png",
                    @checked = false,
                    type = 1
                });
                var dataList = Team.Distinct(new TreeAreaCompare()).Union<TreeAction>(CsrInfo).OrderBy(r => r.name);
                return JsonConvert.SerializeObject(dataList);
            }
            else
            {
                return "";
            }
        }
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult AddWOTransfer(long WOID, int mid, string principal, string CompDate, string Description)
        {
            //获取当前登录用户
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = _UserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
            var woInfo = _gD_WorkOrderService.Query<GdWorkOrder>(r => r.Woid == WOID).FirstOrDefault();
            var historyInfo = _gD_WOOperationService.Query<GdWooperation>(r => r.Pid == WOID && r.Type == (short)Model.CustomizedClass.WOOperationType.移交).FirstOrDefault();
            //查询该工单是否移交过
            if (woInfo.CurrentState == (short)Model.CustomizedClass.WoState.移交 || historyInfo != null || woInfo.CurrentState == (short)Model.CustomizedClass.WoState.已退单)
            {
                return Content("cant");
            }
            Random random = new Random();
            //添加新工单
            string year = DateTime.Now.Year.ToString();
            var ebeninfo = _gD_EventsService.Query<GdEvents>(r => r.IncidentId == woInfo.EventId).FirstOrDefault();
            string str = System.Enum.GetName(typeof(IncidentTypePY), ebeninfo.IncidentType);
            int count = _gD_WorkOrderService.Query<GdWorkOrder>(r => true).Count();
            GdWorkOrder order = new GdWorkOrder();
            order.Num = "GD" + str + "-" + year + "-" + count.ToString().PadLeft(7, '0');
            order.Woid = ConvertDateTimeInt(DateTime.Now);
            order.EventId = woInfo.EventId;
            order.Degree = woInfo.Degree;
            order.HandleLevel = woInfo.HandleLevel;
            order.AuditingContent = woInfo.AuditingContent;
            order.CurrentState = 0;
            order.IsAuditing = (byte)Model.CustomizedClass.WOExtensionReview.未审核;
            order.ReleaseTime = DateTime.Now;
            order.ReleaseUser = user == null ? 0 : user.UserId;
            order.CompleteTime = Convert.ToDateTime(CompDate);
            order.Pid = WOID;
            order.UserId = mid;
            //修改当前工单状态
            woInfo.CurrentState = (short)Model.CustomizedClass.WoState.移交;
            woInfo.CompleteTime = (DateTime)woInfo.ReleaseTime;

            //工单历史数据
            GdWooperation woop = new GdWooperation();
            woop.Description = Description;
            woop.Id = ConvertDateTimeInt(DateTime.Now) + random.Next(1, 100);
            woop.OperationTime = DateTime.Now;
            woop.Pid = WOID;
            woop.State = 0;
            woop.Type = (short)Model.CustomizedClass.WOOperationType.移交;
            woop.UserId = user == null ? 0 : user.UserId;

            //新工单历史操作表
            GdWooperation newwoop = new GdWooperation();
            newwoop.Description = Description;
            newwoop.Id = ConvertDateTimeInt(DateTime.Now) + random.Next(1, 100);
            newwoop.OperationTime = DateTime.Now;
            newwoop.Pid = order.Woid;
            newwoop.State = 0;
            newwoop.Type = (short)Model.CustomizedClass.WOOperationType.派发;
            newwoop.UserId = user == null ? 0 : user.UserId;
            GdWoextension gD_WOReview = new GdWoextension();
            ////提交
            if (_gD_WorkOrderService.AddWOTransfer(gD_WOReview, order, woop, woInfo, newwoop) > 0)
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
        #region 定位
        [TypeFilter(typeof(IgonreActionFilter))]
        public ActionResult Positioning(long id)
        {
            //工单信息
            // var woinfo = _gD_WorkOrderService.Query<GdWorkOrder>(r => r.Woid == id).FirstOrDefault();
            var eventinfo = _gD_EventsService.Query<GdEvents>(r => r.IncidentId == id).FirstOrDefault();
            int? stationid = 0;
            if (eventinfo?.IncidentType == 0)
            {
                stationid = _gD_InspectionService.Query<GdInspection>(r => r.InspectionId == eventinfo.TaskId).FirstOrDefault()?.StationId;
            }
            else if (eventinfo?.IncidentType == 1)
            {
                stationid = _gD_MaintainService.Query<GdMaintain>(r => r.MaintainId == eventinfo.TaskId).FirstOrDefault()?.StationId;
            }
            else if (eventinfo?.IncidentType == 2)
            {
                stationid = _gD_RepairService.Query<GdRepair>(r => r.RepairId == eventinfo.TaskId).FirstOrDefault()?.StationId;
            }
            var stationoinfo = _sws_StationService.Query<SwsStation>(r => r.StationId == stationid).FirstOrDefault();
            ViewBag.Lat = stationoinfo.Lat;
            ViewBag.Lng = stationoinfo.Lng;
            ViewBag.StationNum = stationoinfo.StationNum;
            ViewBag.StationName = stationoinfo.StationName;
            return View();
        }
        //工单详情 图片信息
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult ShowImage(long id, int type, int ResourceType)
        {
            List<GdResource> resource = _gD_ResourceService.Query<GdResource>(r => r.Pid == id && r.Type == type && r.ResourceType == ResourceType).ToList();
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
            List<GdResource> resource = _gD_ResourceService.Query<GdResource>(r => r.Pid == id && r.Type == type && r.ResourceType == ResourceType).ToList();
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(resource));
        }
        #endregion
        //时间戳
        public static long ConvertDateTimeInt(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (long)(time - startTime).TotalMilliseconds;
        }
    }
}