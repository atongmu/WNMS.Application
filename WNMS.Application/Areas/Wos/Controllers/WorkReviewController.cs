using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WNMS.Application.Utility.Filters;
using WNMS.IService;
using WNMS.Model.DataModels;

namespace WNMS.Application.Areas.Wos.Controllers
{
    [Area("Wos")]
    public class WorkReviewController : Controller
    {
        private readonly IGD_WorkOrderService _gD_WorkOrderService = null;
        private ISysUserService _UserService = null;
        private IGD_WOReviewService _gD_WOReviewService = null;
        private IGD_EventsService _gD_EventsService = null;
        public WorkReviewController(IGD_WorkOrderService gD_WorkOrderService,
            ISysUserService sysUserService,
            IGD_WOReviewService gD_WOReviewService,
            IGD_EventsService gD_EventsService)
        {
            _gD_WorkOrderService = gD_WorkOrderService;
            _UserService = sysUserService;
            _gD_WOReviewService = gD_WOReviewService;
            _gD_EventsService = gD_EventsService;
        }
        public IActionResult Index()
        {
            return View();
        }
        //表格数据获取
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult QueryTable(int pagesize, int pageindex, string SearchText, string order, string sort)
        {
            int Totalcount = 0;
            string ordertems = sort + " " + order;
            string filter = "1=1";
            if (!string.IsNullOrEmpty(SearchText))
            {
                filter += " and Num like '%" + SearchText + "%'";
            }

            var datalist = _gD_WorkOrderService.GetWoReInfoTable(pageindex, pagesize, ordertems, filter, ref Totalcount);
            return Json(new { total = Totalcount, rows = Newtonsoft.Json.JsonConvert.SerializeObject(datalist) });
        }
        //审核
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult ReviewExtension(long ID, short State)
        {
            //获取当前登录用户
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = _UserService.Query<SysUser>(u => u.UserId == userID)?.FirstOrDefault();
            var exInfo = _gD_WOReviewService.Query<GdWoreview>(r => r.Id == ID).FirstOrDefault();
            var woInfo = _gD_WorkOrderService.Query<GdWorkOrder>(r => r.Woid == exInfo.Woid).FirstOrDefault();

            if (exInfo.State != 3)
            {
                return Content("Exit");
            }
            //随机数
            Random random = new Random();
            exInfo.State = State;
            exInfo.Auditor = user == null ? 0 : user.UserId;
            exInfo.AuditingTime = DateTime.Now;
            //通过 添加工单操作信息  修改工单完成时间
            if (exInfo.Type == 1)//退单
            {
                if (State == 1)
                {
                    GdWooperation gd_WOOperation = new GdWooperation();
                    gd_WOOperation.Id = ConvertDateTimeInt(DateTime.Now);
                    gd_WOOperation.Pid = exInfo.Woid;
                    gd_WOOperation.OperationTime = DateTime.Now;
                    gd_WOOperation.UserId = user == null ? 0 : user.UserId;
                    gd_WOOperation.State = (short)Model.CustomizedClass.WOExtensionReview.审核通过;
                    gd_WOOperation.Type = (short)Model.CustomizedClass.WOOperationType.审核;
                    gd_WOOperation.Description = user.Account + "-审核通过";

                    woInfo.CurrentState = (short)Model.CustomizedClass.WoState.已退单;
                    //修改工单完成时间
                    woInfo.CompleteTime = (DateTime)woInfo.ReleaseTime;
                    if (_gD_WorkOrderService.EditWoOkReview(exInfo, woInfo, gd_WOOperation) > 0)
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
                    GdWooperation gd_WOOperation = new GdWooperation();
                    gd_WOOperation.Id = ConvertDateTimeInt(DateTime.Now);
                    gd_WOOperation.Pid = exInfo.Woid;
                    gd_WOOperation.OperationTime = DateTime.Now;
                    gd_WOOperation.UserId = user == null ? 0 : user.UserId;
                    gd_WOOperation.State = (short)Model.CustomizedClass.WOExtensionReview.审核未通过;
                    gd_WOOperation.Type = (short)Model.CustomizedClass.WOOperationType.审核;
                    gd_WOOperation.Description = user.Account + "-审核未通过";
                    if (_gD_WorkOrderService.EditWoNoReview(exInfo, gd_WOOperation) > 0)
                    {
                        return Content("ok");
                    }
                    else
                    {
                        return Content("no");
                    }

                }
            }
            else
            {
                if (State == 1)
                {

                    //查询该工单是否移交过
                    if (woInfo.CurrentState == 0)
                    {
                        return Content("cant");
                    }
                    //添加新工单
                    GdWorkOrder order = new GdWorkOrder();
                    order.Woid = ConvertDateTimeInt(DateTime.Now);
                    order.EventId = woInfo.EventId;
                    order.Degree = woInfo.Degree;
                    order.HandleLevel = woInfo.HandleLevel;
                    order.AuditingContent = woInfo.AuditingContent;
                    order.CurrentState = 0;
                    order.IsAuditing = (byte)Model.CustomizedClass.WOExtensionReview.未审核;
                    order.ReleaseTime = DateTime.Now;
                    order.ReleaseUser = user == null ? 0 : user.UserId;
                    order.CompleteTime = (DateTime)exInfo.CompleteTime;
                    order.Pid = exInfo.Woid;

                    string year = DateTime.Now.Year.ToString();
                    var ebeninfo = _gD_EventsService.Query<GdEvents>(r => r.IncidentId == woInfo.EventId).FirstOrDefault();
                    string str = System.Enum.GetName(typeof(Model.CustomizedClass.IncidentTypePY), ebeninfo.IncidentType);
                    int count = _gD_WorkOrderService.Query<GdWorkOrder>(r => true).Count();

                    order.Num = "GD" + str + "-" + year + "-" + count.ToString().PadLeft(7, '0');

                    //修改当前工单状态
                    woInfo.CurrentState = (short)Model.CustomizedClass.WoState.移交;
                    woInfo.CompleteTime = (DateTime)woInfo.ReleaseTime;

                    //工单历史数据
                    GdWooperation woop = new GdWooperation();
                    woop.Description = "工单移交";
                    woop.Id = ConvertDateTimeInt(DateTime.Now) + random.Next(1, 100);
                    woop.OperationTime = DateTime.Now;
                    woop.Pid = woInfo.Woid;
                    woop.State = 0;
                    woop.Type = (short)Model.CustomizedClass.WOOperationType.移交;
                    woop.UserId = user == null ? 0 : user.UserId;
                    //新工单历史操作表
                    GdWooperation newwoop = new GdWooperation();
                    newwoop.Description = "";
                    newwoop.Id = ConvertDateTimeInt(DateTime.Now) + random.Next(1, 100);
                    newwoop.OperationTime = DateTime.Now;
                    newwoop.Pid = order.Woid;
                    newwoop.State = 0;
                    newwoop.Type = (short)Model.CustomizedClass.WOOperationType.派发;
                    newwoop.UserId = user == null ? 0 : user.UserId;
                    //工单人员中间表数据处理

                    order.UserId = (int)exInfo.RecipientId;
                    //List<GD_WOUser> woUereList = new List<GD_WOUser>();
                    //if (exInfo.CollaborationID != "")
                    //{
                    //    string[] ids = exInfo.CollaborationID.Split(',');
                    //    foreach (var item in ids)
                    //    {
                    //        GD_WOUser wo = new GD_WOUser();
                    //        wo.Role = false;
                    //        wo.UserID = long.Parse(item);
                    //        wo.WOID = order.WOID;
                    //        woUereList.Add(wo);
                    //    }
                    //}
                    ////负责人
                    //GD_WOUser w = new GD_WOUser();
                    //w.WOID = order.WOID;
                    //w.UserID = (long)exInfo.RecipientID;
                    //w.Role = true;
                    //woUereList.Add(w);
                    //提交
                    if (_gD_WorkOrderService.AddWOTransfer(exInfo, order, woop, woInfo, newwoop) > 0)
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
                    GdWooperation gd_WOOperation = new GdWooperation();
                    gd_WOOperation.Id = ConvertDateTimeInt(DateTime.Now);
                    gd_WOOperation.Pid = exInfo.Woid;
                    gd_WOOperation.OperationTime = DateTime.Now;
                    gd_WOOperation.UserId = user == null ? 0 : user.UserId;
                    gd_WOOperation.State = (short)Model.CustomizedClass.WOExtensionReview.审核未通过;
                    gd_WOOperation.Type = (short)Model.CustomizedClass.WOOperationType.审核;
                    gd_WOOperation.Description = user.Account + "-审核未通过";
                    if (_gD_WorkOrderService.AddWONoTransfer(exInfo, gd_WOOperation) > 0)
                    {
                        return Content("ok");
                    }
                    else
                    {
                        return Content("no");
                    }
                }
            }

        }

        //时间戳
        public static long ConvertDateTimeInt(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (long)(time - startTime).TotalMilliseconds;
        }
    }
}