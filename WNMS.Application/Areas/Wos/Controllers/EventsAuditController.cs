using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WNMS.Application.Utility.Filters;
using WNMS.IService;
using WNMS.Model.CustomizedClass;
using WNMS.Model.DataModels;

namespace WNMS.Application.Areas.Wos.Controllers
{
    [Area("Wos")]
    public class EventsAuditController : Controller
    {
        #region 属性，构造函数
        private IGD_EventsService eventService = null;
        private IGD_WorkOrderService workOrderService = null;
        private ISysUserService userService = null;
        private IGD_ResourceService resourceService = null;
        private IGD_TeamUserService teamUserService = null;
        private IGD_TeamInfoService teamInfoService = null;
        public EventsAuditController(IGD_EventsService gd_EventService, IGD_WorkOrderService gd_WorkOrderService, 
            ISysUserService sys_UserService, IGD_ResourceService gd_ResourceService, IGD_TeamUserService gD_TeamUserService,
            IGD_TeamInfoService gD_TeamInfoService)
        {
            eventService = gd_EventService;
            workOrderService = gd_WorkOrderService;
            userService = sys_UserService;
            resourceService = gd_ResourceService;
            teamUserService = gD_TeamUserService;
            teamInfoService = gD_TeamInfoService;
        }
        #endregion
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult Index()
        {
            return View();
        }

        #region 事件详情
        [TypeFilter(typeof(IgonreActionFilter))]
        //事件详情页面
        public ActionResult EventDetail(long id)
        {
            //根据ID获取并返回待处理事件信息
            SEvents elist = eventService.GetIncidentByID(id).FirstOrDefault();
            ViewBag.Events = elist;

            //返回事件ID
            ViewBag.IncidentID = elist.IncidentID;

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
            ViewBag.Record = record.Count();


            //返回分派详情
            List<dynamic> tasklist = eventService.GetTreateTaskData(id.ToString()).ToList();
            ViewBag.WorkOrder = tasklist;
            return View();
        }
        #endregion
        #region 事件审核
        [TypeFilter(typeof(IgonreActionFilter))]
        public ActionResult EventAudit(long id)
        {
            ViewBag.IncidentID = id;
            return View();
        }

        //提交事件审核意见
        [TypeFilter(typeof(IgonreActionFilter))]
        public ActionResult SetEventAudit(long eventID, string treatidea)
        {
            GdEvents e = eventService.Query<GdEvents>(r => r.IncidentId == eventID).FirstOrDefault();
            if (e != null)
            {
                e.AuditState = true;
                e.AuditContent = treatidea;
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
        #region 工单审核
        //获取工单详情
        [TypeFilter(typeof(IgonreActionFilter))]
        public ActionResult WorkOrderDetail(long id)
        {
            //根据ID获取并返回待处理事件信息
            var wo_eventInfo = workOrderService.GetWO_EventData(id);
            ViewBag.wo_event = wo_eventInfo;


            //获取当前登录用户
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = userService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();


            ViewBag.User = user;

            //返回工单ID
            ViewBag.WOID = id;

            //返回事件图片,缩略图使用
            var woinfo = workOrderService.Query<GdWorkOrder>(r => r.Woid == id).FirstOrDefault();
            var eventinfo = eventService.Query<GdEvents>(R => R.IncidentId == woinfo.EventId).FirstOrDefault()?.TaskId;
            List<GdResource> img = resourceService.Query<GdResource>(r => r.Pid == eventinfo && r.Type == 1 && r.ResourceType == 1).ToList();
            ViewBag.Img = img;
            //获取工单操作历史 
            var opData = workOrderService.GetWO_OpData(id, 1);
            ViewBag.opData = opData;
            //获取维修历史 
            var opcData = workOrderService.GetWO_OpData(id, 3);
            ViewBag.opcData = opcData;
            //延期申请
            var extension = workOrderService.LoadExtensionData(id).ToList();
            ViewBag.extension = extension;
            //获取退单信息
            var tdData = workOrderService.GetWO_OpData(id, 6);
            ViewBag.tdData = tdData;
            //获取维修负责人信息
            var userid = woinfo.UserId;
            var userInfo = userService.Query<SysUser>(r => r.UserId == userid).FirstOrDefault();
            ViewBag.userInfo = userInfo.Account;
            //班组
            var teamid = teamUserService.Query<GdTeamUser>(r => r.UserId == userid).FirstOrDefault()?.TeamId;
            var depInfo = teamInfoService.Query<GdTeamInfo>(r => r.TeamId == teamid).FirstOrDefault();
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

        //工单审核页面
        [TypeFilter(typeof(IgonreActionFilter))]
        public ActionResult WorkOrderAudit(long id, byte state)
        {
            ViewBag.WOID = id;
            ViewBag.State = state;
            return View();
        }
        [TypeFilter(typeof(IgonreActionFilter))]
        public ActionResult SetOrderAudit(long woID, byte state, string treatidea)
        {
            //返回当前登录用户
            int userID = int.Parse(User.FindFirstValue("UserID"));

            GdWorkOrder w = workOrderService.Query<GdWorkOrder>(r => r.Woid == woID).FirstOrDefault();
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
                GdWooperation woo = new GdWooperation();
                woo.Description = treatidea;
                woo.Id = ConvertDateTimeInt(DateTime.Now);
                woo.OperationTime = DateTime.Now;
                woo.Pid = woID;
                woo.State = 0;
                woo.Type = (short)WOOperationType.审核;
                woo.UserId = userID;

                if (eventService.EditWorkOrderAudit(w, woo) > 0)
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

        public static long ConvertDateTimeInt(System.DateTime time)
        {
            System.DateTime startTime = System.TimeZoneInfo.ConvertTimeToUtc(new System.DateTime(1970, 1, 1));
            return (long)(time - startTime).TotalMilliseconds;
        }
        #endregion
    }
}