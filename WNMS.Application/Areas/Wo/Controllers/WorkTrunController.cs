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
    public class WorkTrunController : Controller
    {
        private IWO_WorkOrderService _wO_WorkOrderService = null;
        private ISysUserService _sysUserService = null;
        private IWO_ForwardService _wO_ForwardService = null;
        public WorkTrunController(IWO_WorkOrderService wO_WorkOrderService, ISysUserService sysUserService,
            IWO_ForwardService wO_ForwardService)
        {
            _wO_WorkOrderService = wO_WorkOrderService;
            _sysUserService = sysUserService;
            _wO_ForwardService = wO_ForwardService;
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
                filter += " and Num like '%" + SearchText + "%'";
            }

            var datalist = _wO_WorkOrderService.GetWoTrunTable(pageindex, pagesize, ordertems, filter, ref Totalcount);
            return Json(new { total = Totalcount, rows = Newtonsoft.Json.JsonConvert.SerializeObject(datalist) });
        }
        /// <summary>
        /// 退单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult ChargebackWO(long id)
        {
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = _sysUserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
            var exInfo = _wO_ForwardService.Query<WoForward>(r => r.Id == id).FirstOrDefault();
            var WOInfo = _wO_WorkOrderService.Query<WoWorkOrder>(r => r.Woid == exInfo.Woid).FirstOrDefault();
            if (WOInfo.CurrentState != (short)Model.CustomizedClass.WoState.已退单)
            {
                WOInfo.CurrentState = (short)Model.CustomizedClass.WoState.已退单;
                //WOInfo.CurrentState = (short)Model.CustomizedClass.Enum.WoState.待接收;
                DateTime? dt = WOInfo.ReleaseTime == null ? DateTime.Now : WOInfo.ReleaseTime;
                WOInfo.CompleteTime = (DateTime)dt; 
                WOInfo.IsAuditing = 1; 
                //返回当前登录用户 
                WOInfo.AuditingContent = user.NickName +"-审核通过";
                WoWooperation woWooperation = new WoWooperation();
                woWooperation.Id = ConvertDateTimeInt(DateTime.Now);
                woWooperation.Pid = id;
                woWooperation.OperationTime = DateTime.Now;
                woWooperation.UserId = user == null ? 0 : user.UserId;
                woWooperation.State = (short)Model.CustomizedClass.WoState.已退单;
                woWooperation.Type = (short)Model.CustomizedClass.WOOperationType.退单;
                woWooperation.Description = "退单";
                exInfo.State = 1;
                exInfo.Auditor = user == null ? 0 : user.UserId;
                exInfo.AuditingTime = DateTime.Now;
                if (_wO_WorkOrderService.ChargebackWOF(WOInfo, woWooperation, exInfo) > 0)
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

        //审核驳回转发信息
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult AuditTurn(long ID, short State)
        {
            //获取当前登录用户
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = _sysUserService.Query<SysUser>(u => u.UserId == userID)?.FirstOrDefault();
            var exInfo = _wO_ForwardService.Query<WoForward>(r => r.Id == ID).FirstOrDefault();

            if (exInfo.State != 3)
            {
                return Content("Exit");
            }

            exInfo.State = State;
            exInfo.Auditor = user == null ? 0 : user.UserId;
            exInfo.AuditingTime = DateTime.Now;
            if (_wO_ForwardService.Update<WoForward>(exInfo))
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
                            Title = "驳回退单申请",
                            Content = "你的工单退单请求被驳回，请重新操作",
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

        //时间戳
        public static long ConvertDateTimeInt(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (long)(time - startTime).TotalMilliseconds;
        }
    }
}