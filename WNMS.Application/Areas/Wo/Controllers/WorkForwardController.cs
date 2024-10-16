using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Jiguang.JPush.Model;
using Microsoft.AspNetCore.Mvc;
using WNMS.Application.Utility.Filters;
using WNMS.Application.Utility.Jpush;
using WNMS.IService;
using WNMS.Model.DataModels;
using WNMS.Utility.Jpush;

namespace WNMS.Application.Areas.Wo.Controllers
{
    [Area("Wo")]
    public class WorkForwardController : Controller
    {
        private IWO_WorkOrderService _wO_WorkOrderService = null;
        private ISysUserService _sysUserService = null;
        private IWO_ForwardService _wO_ForwardService = null;
        public WorkForwardController(IWO_WorkOrderService wO_WorkOrderService, ISysUserService sysUserService,
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
            string filter = "1=1 and m.Type = 2 ";
            if (!string.IsNullOrEmpty(SearchText))
            {
                filter += " and Num like '%" + SearchText + "%'";
            }

            var datalist = _wO_WorkOrderService.GetWoForwardTable(pageindex, pagesize, ordertems, filter, ref Totalcount);
            return Json(new { total = Totalcount, rows = Newtonsoft.Json.JsonConvert.SerializeObject(datalist) });
        }

        /// <summary>
        /// 提醒客服接收工单
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult Notice(long ID)
        {
            //获取当前登录用户
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = _sysUserService.Query<SysUser>(u => u.UserId == userID)?.FirstOrDefault();
            var exInfo = _wO_ForwardService.Query<WoForward>(r => r.Id == ID).FirstOrDefault();
            //派发工单
            Random random = new Random();
            var woInfo = _wO_WorkOrderService.Find<WoWorkOrder>(exInfo.Woid);
            woInfo.CurrentState = (short)Model.CustomizedClass.WoState.移交;
            WoWooperation woWooperation = new WoWooperation();
            woWooperation.Id = ConvertDateTimeInt(DateTime.Now) + random.Next(1, 10000);
            woWooperation.Pid = exInfo.Woid;
            woWooperation.OperationTime = DateTime.Now;
            woWooperation.UserId = user == null ? 0 : user.UserId;
            woWooperation.State = 0;
            woWooperation.Type = (short)Model.CustomizedClass.WOOperationType.移交;
            if (exInfo.State != 3)
            {
                return Content("Exit");
            }
            exInfo.State = 1;
            exInfo.Auditor = user == null ? 0 : user.UserId;
            exInfo.AuditingTime = DateTime.Now;
            //生成新工单
            var beginTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 00:00:00"));
            var endTime = DateTime.Parse(DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00"));
            var woCount = _wO_WorkOrderService.Query<WoWorkOrder>(r => r.ReleaseTime >= beginTime && r.ReleaseTime <= endTime).ToList().Count();
            string num = "0";
            if (woCount == 0)
            {
                num = "001";
            }
            else if (woCount < 10)
            {
                int tenCount = woCount + 1;
                num = "00" + tenCount;
            }
            else if (woCount >= 10 && woCount < 100)
            {
                num = "0" + woCount;
            }
            WoWorkOrder woWorkOrder = new WoWorkOrder();
            woWorkOrder.Woid = ConvertDateTimeInt(DateTime.Now) + random.Next(1, 10000);
            woWorkOrder.CompleteTime = (DateTime)exInfo.CompleteTime;
            woWorkOrder.Degree = woInfo.Degree;
            woWorkOrder.HandleLevel = woInfo.HandleLevel;
            woWorkOrder.ReleaseTime = woInfo.ReleaseTime;
            woWorkOrder.IsAuditing = woInfo.IsAuditing;
            woWorkOrder.AuditingContent = woInfo.AuditingContent;
            woWorkOrder.EventId = woInfo.EventId;
            woWorkOrder.CurrentState = (short)Model.CustomizedClass.WoState.待接收;
            woWorkOrder.ReleaseUser = woInfo.ReleaseUser; 
            woWorkOrder.Pid = exInfo.Woid;
            woWorkOrder.ReceiveUser = exInfo.RecipientId;
            woWorkOrder.Num = "WO_WX_" + DateTime.Now.ToString("yyyyMMdd") + num; 
            woWorkOrder.EarlyWarningPlanId = woInfo.EarlyWarningPlanId;
            //生成新工单操作
            WoWooperation woWooperation1 = new WoWooperation();
            woWooperation1.Id = ConvertDateTimeInt(DateTime.Now) + random.Next(1, 10000);
            woWooperation1.Pid = woWorkOrder.Woid;
            woWooperation1.OperationTime = DateTime.Now;
            woWooperation1.UserId = user == null ? 0 : user.UserId;
            woWooperation1.State = 0;
            woWooperation1.Type = (short)Model.CustomizedClass.WOOperationType.派发;


            if (_wO_WorkOrderService.EditWoForward(woInfo, woWooperation, exInfo, woWorkOrder, woWooperation1) > 0)
            {
                Jpush jpush = new Jpush();
                try
                {
                    PushPayload pushPayload = new PushPayload()
                    {
                        Platform = new List<string> { "android", "ios" },
                        Audience = new Audience
                        {
                            Alias = new List<string> { woInfo.ReceiveUser.ToString() }//接收人的ID
                        },
                        Message = new Message
                        {
                            Title = "有待收转发工单",
                            Content = "有待收转发工单，请尽快处理",
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
        //时间戳
        public static long ConvertDateTimeInt(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (long)(time - startTime).TotalMilliseconds;
        }
    }
}