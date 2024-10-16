using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WNMS.Application.Utility;
using WNMS.Application.Utility.Filters;
using WNMS.IService;
using WNMS.Model.CustomizedClass;
using WNMS.Utility;
using WNMS.Model.DataModels;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Html;
using WNMS.Application.Utility.Jpush;

namespace WNMS.Application.Areas.Wos.Controllers
{
    [Area("Wos")]
    public class EventsController : Controller
    {
        #region 属性，构造函数
        private IGD_EventsService eventService = null;
        private IGD_WorkOrderService workOrderService = null;
        private ISysUserService userService = null;
        private IGD_ResourceService resourceService = null;
        public EventsController(IGD_EventsService gd_EventService, IGD_WorkOrderService gd_WorkOrderService,
            ISysUserService sys_UserService, IGD_ResourceService gd_ResourceService)
        {
            eventService = gd_EventService;
            workOrderService = gd_WorkOrderService;
            userService = sys_UserService;
            resourceService = gd_ResourceService;
        }
        #endregion

        #region 页面加载 数据查询
        public IActionResult Index()
        {
            int userID = int.Parse(User.FindFirstValue("UserID"));
            List<SEvents> elist = this.eventService.LoadEventInfoList(null, "8", 10, 1, "ReportTime", userID, false, false).DataList.ToList();
            List<GdWorkOrder> wo = workOrderService.Query<GdWorkOrder>(r => r.CurrentState == 6).ToList();
            List<GdWorkOrder> wod = workOrderService.Query<GdWorkOrder>(r => r.CurrentState == 7).ToList();

            ViewBag.TotalCount = elist.Count();
            ViewBag.untreated = elist.Where(r => r.IncidentState == 0).Count();
            ViewBag.treated = elist.Where(r => r.IncidentState == 1).Count();
            ViewBag.invalid = elist.Where(r => r.IncidentState == 4).Count();

            //包含撤回的数量
            int recall = 0;
            if (wo.Count > 0)
            {
                recall = elist.Where(r => wo.Select(e => e.EventId).Contains(r.IncidentID)).Count();
            }
            ViewBag.recall = recall;

            //包含退单的数量
            int back = 0;
            if (wod.Count > 0)
            {
                back = elist.Where(r => wod.Select(e => e.EventId).Contains(r.IncidentID)).Count();
            }
            ViewBag.back = back;

            //返回初始化时间
            ViewBag.datemin = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now.AddDays(-7));
            ViewBag.datemax = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now);
            return View();
        }

        [HttpPost]
        [TypeFilter(typeof(IgonreActionFilter))]
        public async Task<IActionResult> LoadEventsInfo(string beginDate, string endDate, string message, string state, string date, string type, string sortName, string sortOrder, int pageSize = 10, int pageIndex = 1)
        {
            int userID = int.Parse(User.FindFirstValue("UserID"));
            string enddate = "";
            string begindate = GetDate(date, beginDate, endDate, ref enddate);
            #region 查询条件
            Expression<Func<SEvents, bool>> funcWhere = null;
            if (!string.IsNullOrWhiteSpace(message))
            {
                funcWhere = funcWhere.And(r => r.IncidentNum.Contains(message) || (r.IncidentContent != null && r.IncidentContent.Contains(message)));
            }
            if (!string.IsNullOrWhiteSpace(begindate) && !string.IsNullOrWhiteSpace(enddate))
            {
                DateTime beginTime = Convert.ToDateTime(begindate);
                DateTime endTime = Convert.ToDateTime(enddate);
                funcWhere = funcWhere.And(r => r.ReportTime >= beginTime && r.ReportTime <= endTime);
            }
            if (!string.IsNullOrEmpty(type) && type != "8")
            {
                funcWhere = funcWhere.And(r => r.IncidentType == int.Parse(type));
            }
            #endregion

            #region  排序
            bool flag = true;
            if (sortOrder == "desc") flag = false;
            string sort = string.IsNullOrWhiteSpace(sortName) ? "ReportTime" : sortName;
            #endregion

            PageResult<SEvents> eventsList = this.eventService.LoadEventInfoList(funcWhere, state, pageSize, pageIndex, sort, userID, true, flag);
            PartialView("_EventsTable", eventsList.DataList);
            string dataTable = await ViewToString.RenderPartialViewToString(this, "_EventsTable");
            return Json(new
            {
                total = eventsList.TotalCount,
                pageIndex = eventsList.PageIndex,
                pageSize = eventsList.PageSize,
                order = sortOrder,
                sortName = sortName ?? "",
                dataTable = dataTable
            });
        }
        public static string GetDate(string date, string beginTime, string endTime, ref string endDate)
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
                    case 6:
                        beginDate = beginTime;
                        endDate = endTime;
                        break;
                    case 8:
                        beginDate = dt.Date.ToString("yyyy-MM-dd");
                        endDate = dt.Date.AddDays(1).ToString("yyyy-MM-dd");
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

        #endregion

        #region 任务派发
        /// <summary>
        /// 派发页面 数据获取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult TreatEvent(long id)
        {
            //根据ID获取并返回待处理事件信息
            SEvents elist = eventService.GetIncidentByID(id).FirstOrDefault();
            ViewBag.Events = elist;

            //返回当前登录用户
            int userID = int.Parse(User.FindFirstValue("UserID"));
            Model.DataModels.SysUser user = userService.Query<SysUser>(r => r.UserId == userID).FirstOrDefault();
            ViewBag.User = user;

            //返回事件图片,缩略图使用
            List<GdResource> img = new List<GdResource>();
            List<GdResource> record = new List<GdResource>();
            if (elist != null)
            {
                if (elist.IncidentType == 2)
                {
                    img = resourceService.Query<GdResource>(r => r.Pid == elist.TaskId && r.Type == 1 && r.ResourceType == 1).ToList();
                    record = resourceService.Query<GdResource>(r => r.Pid == elist.TaskId && r.Type == 2 && r.ResourceType == 1).ToList();
                }
            }           
            ViewBag.Img = img;

            //返回事件音频
            ViewBag.Record =record.Count();

            //返回事件ID
            ViewBag.IncidentID = id;

            //返回分派详情
            List<dynamic> tasklist = eventService.GetTreateTaskData(id.ToString()).ToList();
            ViewBag.WorkOrder = tasklist;
            return View();
        }

        [TypeFilter(typeof(IgonreActionFilter))]
        //待处理事件图片展示
        public IActionResult EventsImg(long id)
        {
            List<GdResource> resource = new List<GdResource>();
            GdEvents e = eventService.Query<GdEvents>(r => r.IncidentId == id).FirstOrDefault();
            if (e != null)
            {
                if (e.IncidentType == 2)
                {
                    resource = resourceService.Query<GdResource>(r => r.Pid == e.TaskId && r.Type == 1 && r.ResourceType == 1).ToList();
                }
            }
            ViewBag.Resource = resource;
            return View();
        }

        [TypeFilter(typeof(IgonreActionFilter))]
        //分派
        public ActionResult TaskTreat(long id, int degree, int level)
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
            string cTime = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now.AddHours(level));
            if (level == 6)
            {
                cTime = "";
            }
            ViewBag.EventID = id;
            ViewBag.Degree = degree;
            ViewBag.Level = level;
            ViewBag.CTime = cTime;
            return View();
        }
        //获取班组树
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
        //提交任务分派
        [HttpPost]
        [TypeFilter(typeof(IgonreActionFilter))]
        public ActionResult SetTaskTreat(long eventID, byte level, byte degree, string mid, string treatidea, string endDate)
        {
            //返回当前登录用户
            int userID = int.Parse(User.FindFirstValue("UserID"));

            //事件处理（状态设置为已分派）
            GdEvents e = eventService.Query<GdEvents>(r => r.IncidentId == eventID).FirstOrDefault();
            e.IncidentState = (byte)IncidentState.已派发;

            //工单数据处理
            string year = DateTime.Now.Year.ToString();
            string str = System.Enum.GetName(typeof(IncidentTypePY), e.IncidentType);
            int count = workOrderService.Query<GdWorkOrder>(r => true).Count();

            GdWorkOrder order = new GdWorkOrder();
            order.Num = "GD" + str + "-" + year + "-" + count.ToString().PadLeft(7, '0');
            order.Woid = ConvertDateTimeInt(DateTime.Now);
            order.EventId = eventID;
            order.Degree = degree;
            order.HandleLevel = level;
            order.AuditingContent = "";
            order.CurrentState = 0;
            order.IsAuditing = (byte)WOExtensionReview.未审核;
            order.ReleaseTime = DateTime.Now;
            order.ReleaseUser = userID;
            order.CompleteTime = Convert.ToDateTime(endDate);
            order.UserId = string.IsNullOrEmpty(mid) ? 0 : int.Parse(mid);

            //工单历史数据处理
            GdWooperation woo = new GdWooperation();
            woo.Description = treatidea;
            woo.Id = ConvertDateTimeInt(DateTime.Now);
            woo.OperationTime = DateTime.Now;
            woo.Pid = order.Woid;
            woo.State = 0;
            woo.Type = 0;
            woo.UserId = userID;
            WorkOrderJpush jp = new WorkOrderJpush();
            jp.GetTageList();

            if (eventService.AddWorkerOrder(order, woo, e) > 0)
            {
                string[] userIDs = { userID.ToString() };
                string JpushContent = "收到新的工单(" + woo.Id + "),请注意查收！";
                string JpushMark = "新增";
                jp.Jpush(userIDs, JpushContent, JpushMark, e.IncidentType);
                return Content("ok");
            }
            else
            {
                return Content("no");
            }
        }
        public static long ConvertDateTimeInt(System.DateTime time)
        {
            System.DateTime startTime = System.TimeZoneInfo.ConvertTimeToUtc(new System.DateTime(1970, 1, 1));
            return (long)(time - startTime).TotalMilliseconds;
        }

        //设置事件为无效
        [TypeFilter(typeof(IgonreActionFilter))]
        public ActionResult EventsInvalid(long id)
        {
            GdEvents e = eventService.Query<GdEvents>(r => r.IncidentId == id).FirstOrDefault();
            if (e != null)
            {
                e.IncidentState = (byte)IncidentState.无效;
                if (eventService.Update<GdEvents>(e))
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
        //恢复无效事件
        [TypeFilter(typeof(IgonreActionFilter))]
        public ActionResult EventsRenew(long id)
        {
            GdEvents e = eventService.Query<GdEvents>(r => r.IncidentId == id).FirstOrDefault();
            if (e != null)
            {
                if (workOrderService.Query<GdWorkOrder>(r => r.EventId == id).Count() > 0)
                {
                    e.IncidentState = (byte)IncidentState.已派发;
                }
                else
                {
                    e.IncidentState = (byte)IncidentState.未处理;
                }
                if (eventService.Update(e))
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
        #endregion

        #region 音频
        [TypeFilter(typeof(IgonreActionFilter))]
        public ActionResult ShowAudio(long id)
        {
            ViewBag.ID = id;
            return View();
        }
        /// <summary>
        /// 事件/工单获取音频
        /// </summary>
        /// <param name="id">事件或者工单ID</param>
        /// <returns></returns>
        [TypeFilter(typeof(IgonreActionFilter))]
        public ActionResult LoadAudio(long id)
        {
            List<GdResource> resource = new List<GdResource>();
            GdEvents e = eventService.Query<GdEvents>(r => r.IncidentId == id).FirstOrDefault();
            if (e != null)
            {
                if (e.IncidentType == 2)
                {
                    resource = resourceService.Query<GdResource>(r => r.Pid == e.TaskId && r.Type == 2 && r.ResourceType == 1).ToList();
                }
            }
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(resource));
        }
        #endregion
    }
}