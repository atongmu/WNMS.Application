using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Jiguang.JPush.Model;
using Microsoft.AspNetCore.Mvc;
using WNMS.Application.Utility.Filters;
using WNMS.IService;
using WNMS.Model.DataModels;
using WNMS.Utility.Jpush;

namespace WNMS.Application.Areas.Wo.Controllers
{
    [Area("Wo")]
    public class InsForwardController : Controller
    {
        private ISysUserService _sysUserService = null;
        private IWO_InspectionPlanService _wO_InspectionPlanService = null;
        private IWO_AssignmentPlanService _wO_AssignmentPlanService = null;
        private IWO_InsForwardService _wO_InsForwardService = null;
        private IWO_PlanInspectOService _wO_PlanInspectOService = null;

        public InsForwardController(ISysUserService sysUserService,
              IWO_InspectionPlanService wO_InspectionPlanService,
              IWO_AssignmentPlanService wO_AssignmentPlanService,
              IWO_InsForwardService wO_InsForwardService,
              IWO_PlanInspectOService wO_PlanInspectOService
              )
        {
            _sysUserService = sysUserService;
            _wO_InspectionPlanService = wO_InspectionPlanService;
            _wO_AssignmentPlanService = wO_AssignmentPlanService;
            _wO_InsForwardService = wO_InsForwardService;
            _wO_PlanInspectOService = wO_PlanInspectOService;
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
            string filter = "1=1 and m.Type = 2 ";
            if (!string.IsNullOrEmpty(SearchText))
            {
                filter += " and PlanName like '%" + SearchText + "%'";
            }
            var datalist = _wO_InspectionPlanService.GetInsForwardTable(pageindex, pagesize, ordertems, filter, ref Totalcount);
            return Json(new { total = Totalcount, rows = Newtonsoft.Json.JsonConvert.SerializeObject(datalist) });
        }


        /// <summary>
        /// 提醒客服接收工单
        /// </summary>
        /// <param name="ID">转发ID</param>
        /// <param name="State">状态</param>
        /// <returns></returns>
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult Notice(long ID, short State)
        {
            //获取当前登录用户
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = _sysUserService.Query<SysUser>(u => u.UserId == userID)?.FirstOrDefault();
            var exInfo = _wO_InsForwardService.Query<WoInsForward>(r => r.Id == ID).FirstOrDefault();
            var assPlan = _wO_AssignmentPlanService.Query<WoAssignmentPlan>(r => r.PlanId == exInfo.PlanId).FirstOrDefault();
            if (exInfo.State != 3)
            {
                return Content("Exit");
            }
            exInfo.State = State;
            exInfo.Auditor = user == null ? 0 : user.UserId;
            exInfo.AuditingTime = DateTime.Now;
            assPlan.State = true;
            assPlan.IsForward = 0;
            //更新原巡检设备信息
            var oldEq = _wO_PlanInspectOService.Query<WoPlanInspectO>(r => r.PlanId == exInfo.OldPlanId && r.ForwardState == 1).ToList();

            //更新巡检计划的状态
            if (_wO_InspectionPlanService.EditInsForward(exInfo, assPlan, oldEq) > 0)
            {
                Jpush jpush = new Jpush();
                try
                {

                    PushPayload pushPayload = new PushPayload()
                    {
                        Platform = new List<string> { "android", "ios" },
                        Audience = new Audience
                        {
                            Alias = new List<string> { exInfo.RecipientId.ToString() }//接收人的ID
                        },
                        Message = new Message
                        {
                            Title = "有待收转发巡检",
                            Content = "有待收转发巡检单，请尽快处理",
                        }
                    };
                    jpush.ExcutePush(pushPayload);
                }
                catch (Exception)
                {
                    throw;
                }
                return Content("ok");
            }
            else
            {
                return Content("no");
            }

        }
        //审核驳回转发信息
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult AuditForward(long ID, short State)
        {
            //获取当前登录用户
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = _sysUserService.Query<SysUser>(u => u.UserId == userID)?.FirstOrDefault();
            var exInfo = _wO_InsForwardService.Query<WoInsForward>(r => r.Id == ID).FirstOrDefault();

            if (exInfo.State != 3)
            {
                return Content("Exit");
            }
            //驳回工单为删除新添加的巡检信息和巡检设备信息，修改原巡检设备的状态
            exInfo.State = State;
            exInfo.Auditor = user == null ? 0 : user.UserId;
            exInfo.AuditingTime = DateTime.Now;

            //更新原巡检设备信息
            var oldEq = _wO_PlanInspectOService.Query<WoPlanInspectO>(r => r.PlanId == exInfo.OldPlanId && r.ForwardState == 1).ToList();
            var assPlan = _wO_AssignmentPlanService.Query<WoAssignmentPlan>(r => r.PlanId == exInfo.PlanId).FirstOrDefault();
            var newEq = _wO_PlanInspectOService.Query<WoPlanInspectO>(r => r.PlanId == exInfo.PlanId && r.ForwardState == null).ToList();
            assPlan.IsForward = 1;
            if (_wO_InspectionPlanService.TrunInsForward(exInfo, assPlan, oldEq, newEq) > 0)
            {
                Jpush jpush = new Jpush();
                try
                {
                    PushPayload pushPayload = new PushPayload()
                    {
                        Platform = new List<string> { "android", "ios" },
                        Audience = new Audience
                        {
                            Alias = new List<string> { exInfo.UserId.ToString() }//申请者的ID
                        },
                        Message = new Message
                        {
                            Title = "驳回转发工单",
                            Content = "你的工单转发请求被驳回，请重新操作",
                        }
                    };
                    jpush.ExcutePush(pushPayload);

                }
                catch (Exception)
                {
                    throw;
                }
                return Content("ok");
            }
            else
            {
                return Content("no");
            }
        }
    }
}