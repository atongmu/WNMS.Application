using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WNMS.Application.Utility.Filters;
using WNMS.IService;
using WNMS.Model.DataModels;

namespace WNMS.Application.Areas.Wo.Controllers
{
    [Area("Wo")]
    public class WorkExtensionController : Controller
    {
        private IWO_WorkOrderService _wO_WorkOrderService = null;
        private ISysUserService _sysUserService = null;
        private IWO_WOExtensionService _wO_WOExtensionService = null;
        public WorkExtensionController(IWO_WorkOrderService wO_WorkOrderService, ISysUserService sysUserService,
            IWO_WOExtensionService wO_WOExtensionService

            )
        {
            _wO_WorkOrderService = wO_WorkOrderService;
            _sysUserService = sysUserService;
            _wO_WOExtensionService = wO_WOExtensionService;
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

            var datalist = _wO_WorkOrderService.GetWoEInfoTable(pageindex, pagesize, ordertems, filter, ref Totalcount);
            return Json(new { total = Totalcount, rows = Newtonsoft.Json.JsonConvert.SerializeObject(datalist) });
        }

        //审核延期信息
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult ReviewExtension(long ID, short State)
        {
            //获取当前登录用户
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = _sysUserService.Query<SysUser>(u => u.UserId == userID)?.FirstOrDefault();
            var exInfo = _wO_WOExtensionService.Query<WoWoextension>(r => r.Id == ID).FirstOrDefault();
            var woInfo = _wO_WorkOrderService.Query<WoWorkOrder>(r => r.Woid == exInfo.Woid).FirstOrDefault();
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
                WoWooperation woWooperation = new WoWooperation();
                woWooperation.Id = ConvertDateTimeInt(DateTime.Now);
                woWooperation.Pid = exInfo.Woid;
                woWooperation.OperationTime = DateTime.Now;
                woWooperation.UserId = user == null ? 0 : user.UserId;
                woWooperation.State = (short)Model.CustomizedClass.WOExtensionReview.审核通过;
                woWooperation.Type = (short)Model.CustomizedClass.WOOperationType.审核;
                woWooperation.Description = user.NickName + "-审核通过";
                //修改工单完成时间
                woInfo.CompleteTime = (DateTime)exInfo.CompleteTime;
                if (_wO_WorkOrderService.EditWoExtension(exInfo, woInfo, woWooperation) > 0)
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
                WoWooperation woWooperation = new WoWooperation();
                woWooperation.Id = ConvertDateTimeInt(DateTime.Now);
                woWooperation.Pid = exInfo.Woid;
                woWooperation.OperationTime = DateTime.Now;
                woWooperation.UserId = user == null ? 0 : user.UserId;
                woWooperation.State = (short)Model.CustomizedClass.WOExtensionReview.审核未通过;
                woWooperation.Type = (short)Model.CustomizedClass.WOOperationType.审核;
                woWooperation.Description = user.NickName + "-审核未通过";
                if (_wO_WorkOrderService.EditNoWoExtension(exInfo, woWooperation) > 0)
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