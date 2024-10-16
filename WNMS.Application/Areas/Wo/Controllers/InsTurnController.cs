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

namespace WNMS.Application.Areas.Wo.Controllers
{
    [Area("Wo")]
    public class InsTurnController : Controller
    {
        private ISysUserService _sysUserService = null;
        private IWO_InspectionPlanService _wO_InspectionPlanService = null;
        private IWO_AssignmentPlanService _wO_AssignmentPlanService = null;
        private IWO_InsForwardService _wO_InsForwardService = null;
        private IWO_PlanInspectOService _wO_PlanInspectOService = null;
        private IWO_AssignmentPlanService _assignmentService = null;

        public InsTurnController(ISysUserService sysUserService,
              IWO_InspectionPlanService wO_InspectionPlanService,
              IWO_AssignmentPlanService wO_AssignmentPlanService,
              IWO_InsForwardService wO_InsForwardService,
              IWO_PlanInspectOService wO_PlanInspectOService,
              IWO_AssignmentPlanService assignmentService
              )
        {
            _sysUserService = sysUserService;
            _wO_InspectionPlanService = wO_InspectionPlanService;
            _wO_AssignmentPlanService = wO_AssignmentPlanService;
            _wO_InsForwardService = wO_InsForwardService;
            _wO_PlanInspectOService = wO_PlanInspectOService;
            _assignmentService = assignmentService;
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
            string filter = "1=1 and m.Type = 1 ";
            if (!string.IsNullOrEmpty(SearchText))
            {
                filter += " and PlanName like '%" + SearchText + "%'";
            }
            var datalist = _wO_InspectionPlanService.GetInsTurnTable(pageindex, pagesize, ordertems, filter, ref Totalcount);
            return Json(new { total = Totalcount, rows = Newtonsoft.Json.JsonConvert.SerializeObject(datalist) });
        }
        //审核退单信息
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult ReviewTurn(long ID, short State)
        {
            //获取当前登录用户
            Random random = new Random();
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = _sysUserService.Query<SysUser>(u => u.UserId == userID)?.FirstOrDefault();
            if (State == 1)
            {
                var exInfo = _wO_InsForwardService.Query<WoInsForward>(r => r.Id == ID).FirstOrDefault();
                if (exInfo.State != 3)
                {
                    return Content("Exit");
                }
                exInfo.State = (byte)State;
                exInfo.Auditor = user == null ? 0 : user.UserId;
                exInfo.AuditingTime = DateTime.Now;
                
                //查询原巡检计划信息
                //var assInfo = _wO_AssignmentPlanService.Query<WoAssignmentPlan>(r => r.PlanId == exInfo.OldPlanId).FirstOrDefault();
                var assInfo = _wO_AssignmentPlanService.Query<WoAssignmentPlan>(r => r.PlanId == exInfo.PlanId).FirstOrDefault();
                //var assignment = _wO_AssignmentPlanService.Query<WoAssignmentPlan>(r => r.PlanId == exInfo.PlanId).FirstOrDefault();
                //查询原巡检计划的设备信息
                var assEq = _wO_PlanInspectOService.Query<WoPlanInspectO>(r => r.PlanId == exInfo.PlanId).ToList();
                //将原来的巡检工单标注成退回
                WoAssignmentPlan woAssignment = new WoAssignmentPlan();
                if(woAssignment.PlanId == 0)
                {
                    woAssignment = assInfo;
                    woAssignment.State = true;
                    woAssignment.IsChargeback = 1;
                    woAssignment.IsFinish = 2;
                    var num = _wO_AssignmentPlanService.Update<WoAssignmentPlan>(woAssignment);
                }
                    



             //生成新的巡检计划
             WoAssignmentPlan woAssignmentPlan = new WoAssignmentPlan();
                woAssignmentPlan = assInfo;
                woAssignmentPlan.PlanId = ConvertDateTimeInt(DateTime.Now) + random.Next(1, 10000);//
                woAssignmentPlan.IsFinish = 0;
                woAssignmentPlan.Inspector = 0;
                woAssignmentPlan.CreateTime = DateTime.Now;
                woAssignmentPlan.State = false;
                woAssignmentPlan.IsChargeback = 0;
                List<WoPlanInspectO> objlist = new List<WoPlanInspectO>();
                
                
                //objlist.Add(woPlanInspectO);
                foreach (var item in assEq)
                {
                    WoPlanInspectO woPlanInspectO = new WoPlanInspectO();
                    woPlanInspectO.PlanId = woAssignmentPlan.PlanId;
                    woPlanInspectO.InspectObject = item.InspectObject;
                    woPlanInspectO.TemplateId = item.TemplateId;
                    woPlanInspectO.ObjectName = item.ObjectName;
                    woPlanInspectO.IsTemplate = item.IsTemplate;
                    woPlanInspectO.PumpStationId = item.PumpStationId;
                    objlist.Add(woPlanInspectO);
                }
                if (_wO_InspectionPlanService.EditInsTurn(exInfo, woAssignmentPlan, objlist) > 0  )
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
                var exInfo = _wO_InsForwardService.Query<WoInsForward>(r => r.Id == ID).FirstOrDefault();
                if (exInfo.State != 3)
                {
                    return Content("Exit");
                }
                exInfo.State = (byte)State;
                exInfo.Auditor = user == null ? 0 : user.UserId;
                exInfo.AuditingTime = DateTime.Now;
                if (_wO_InsForwardService.Update(exInfo))
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
            //System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            System.DateTime startTime = System.TimeZoneInfo.ConvertTimeToUtc(new System.DateTime(1970, 1, 1));
            return (long)(time - startTime).TotalMilliseconds;
        }
    }
}