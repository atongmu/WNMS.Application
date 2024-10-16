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
    public class InsExtensionController : Controller
    {
       
        private ISysUserService _sysUserService = null; 
        private IWO_InspectionPlanService _wO_InspectionPlanService = null;
        private IWO_AssignmentPlanService _wO_AssignmentPlanService = null;
        public InsExtensionController(  ISysUserService sysUserService,
         
            IWO_InspectionPlanService wO_InspectionPlanService,
            IWO_AssignmentPlanService wO_AssignmentPlanService
            )
        { 
            _sysUserService = sysUserService; 
            _wO_InspectionPlanService = wO_InspectionPlanService;
            _wO_AssignmentPlanService = wO_AssignmentPlanService;
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
                filter += " and PlanName like '%" + SearchText + "%'";
            }

            var datalist = _wO_InspectionPlanService.GetInsEInfoTable(pageindex, pagesize, ordertems, filter, ref Totalcount);
            return Json(new { total = Totalcount, rows = Newtonsoft.Json.JsonConvert.SerializeObject(datalist) });
        }

        //审核延期信息
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult ReviewExtension(long ID, short State)
        {
            //获取当前登录用户
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = _sysUserService.Query<SysUser>(u => u.UserId == userID)?.FirstOrDefault();
            var exInfo = _wO_InspectionPlanService.Query<WoInsExtension>(r => r.Id == ID).FirstOrDefault();
            var planInfo = _wO_AssignmentPlanService.Query<WoAssignmentPlan>(r => r.PlanId == exInfo.PlanId).FirstOrDefault();
            if (exInfo.State != 3)
            {
                return Content("Exit");
            }
            exInfo.State = (byte)State;
            exInfo.Auditor = user == null ? 0 : user.UserId;
            exInfo.AuditingTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            if (State == 1)
            {
                planInfo.EndDate = (DateTime)exInfo.CompleteTime;
            }
            //修改巡检完成时间 
            if (_wO_InspectionPlanService.EditInsExtension(exInfo, planInfo) > 0)
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
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