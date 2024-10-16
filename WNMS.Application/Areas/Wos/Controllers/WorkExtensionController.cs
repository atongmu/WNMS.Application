using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WNMS.Application.Utility.Filters;
using WNMS.IService;
using WNMS.Model.DataModels;

namespace WNMS.Application.Areas.Wos.Controllers
{
    [Area("Wos")]
    public class WorkExtensionController : Controller
    {
        private readonly IGD_WorkOrderService _gD_WorkOrderService = null;
        private ISysUserService _UserService = null;
        private IGD_WOExtensionService _gD_WOExtensionService = null;

        public WorkExtensionController(IGD_WorkOrderService gD_WorkOrderService, ISysUserService sysUserService, IGD_WOExtensionService gD_WOExtensionService)
        {
            _gD_WorkOrderService = gD_WorkOrderService;
            _UserService = sysUserService;
            _gD_WOExtensionService = gD_WOExtensionService;
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

            var datalist = _gD_WorkOrderService.GetWoEInfoTable(pageindex, pagesize, ordertems, filter, ref Totalcount);
            return Json(new { total = Totalcount, rows = Newtonsoft.Json.JsonConvert.SerializeObject(datalist) });
        }

        //审核延期信息
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult ReviewExtension(long ID, short State)
        {
            //获取当前登录用户
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = _UserService.Query<SysUser>(u => u.UserId == userID)?.FirstOrDefault();
            var exInfo = _gD_WOExtensionService.Query<GdWoextension>(r => r.Id == ID).FirstOrDefault();
            var woInfo = _gD_WorkOrderService.Query<GdWorkOrder>(r => r.Woid == exInfo.Woid).FirstOrDefault();
            if (exInfo.State != 3)
            {
                return Content("Exit");
            }
            exInfo.State = (byte)State;
            exInfo.Auditor = user == null ? 0 : user.UserId;
            exInfo.AuditingTime = DateTime.Now;
            //通过 添加工单操作信息  修改工单完成时间
            if (State == 1)
            {
                exInfo.OldCompleteTime = woInfo.CompleteTime;
                GdWooperation gd_WOOperation = new GdWooperation();
                gd_WOOperation.Id = ConvertDateTimeInt(DateTime.Now);
                gd_WOOperation.Pid = exInfo.Woid;
                gd_WOOperation.OperationTime = DateTime.Now;
                gd_WOOperation.UserId = user == null ? 0 : user.UserId;
                gd_WOOperation.State = (short)Model.CustomizedClass.WOExtensionReview.审核通过;
                gd_WOOperation.Type = (short)Model.CustomizedClass.WOOperationType.审核;
                gd_WOOperation.Description = user.NickName + "-审核通过";
                //修改工单完成时间
                woInfo.CompleteTime = (DateTime)exInfo.CompleteTime;
                if (_gD_WorkOrderService.EditWoExtension(exInfo, woInfo, gd_WOOperation) > 0)
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
                gd_WOOperation.Description = user.NickName + "-审核未通过";
                if (_gD_WorkOrderService.EditNoWoExtension(exInfo, gd_WOOperation) > 0)
                {
                    return Content("ok");
                }
                else
                {
                    return Content("no");
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