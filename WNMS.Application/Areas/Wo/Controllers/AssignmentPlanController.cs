using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WNMS.IService;
using WNMS.Service;
using WNMS.Model;
using System.Security.Claims;
using WNMS.Application.Utility;
using System.Linq.Expressions;
using WNMS.Model.CustomizedClass;
using WNMS.Utility;
using WNMS.Model.DataModels;
using Microsoft.AspNetCore.Html;
using WNMS.Application.Utility.Filters;

namespace WNMS.Application.Areas.Wo.Controllers
{
    [Area("Wo")]
    public class AssignmentPlanController : Controller
    {
        private IWO_AssignmentPlanService _assignmentService = null;
        private IWO_AreaInfoService _areaInfoService = null;
        private IWO_TemplateInfoService _templateInfoService = null;
        private IWO_InspectPlanCheckService _isnpectPlanCheckService = null;
        private ISys_DataItemDetailService sys_DataItemDetailService = null;
        private ISysUserService _sysUserService = null;
        private IWO_PlanInspectOService _wO_PlanInspectOService = null;
        private IWO_InsExtensionService _wO_InsExtensionService = null;
        public AssignmentPlanController(IWO_AssignmentPlanService assignmentService, IWO_AreaInfoService areaInfoService,
            IWO_TemplateInfoService templateInfoService, IWO_InspectPlanCheckService inspectPlanCheckService,
            ISys_DataItemDetailService _dataItemDetailService, ISysUserService sysUserService,
            IWO_PlanInspectOService wO_PlanInspectOService, IWO_InsExtensionService wO_InsExtensionService
            )
        {
            _assignmentService = assignmentService;
            _areaInfoService = areaInfoService;
            _templateInfoService = templateInfoService;
            _isnpectPlanCheckService = inspectPlanCheckService;
            sys_DataItemDetailService = _dataItemDetailService;
            _sysUserService = sysUserService;
            _wO_PlanInspectOService = wO_PlanInspectOService;
            _wO_InsExtensionService = wO_InsExtensionService;
        }

        #region 页面加载，数据查询
        //0是未完成1是待审核2是已审核
        public IActionResult Index()
        {
            //int aa = _assignmentService.AssignPlan("2");
            //返回初始化时间
            ViewBag.datemin = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now.AddDays(-7));
            ViewBag.datemax = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now);

            //返回类型数量
            var planList = _assignmentService.Query<Model.DataModels.WoAssignmentPlan>(r => true).ToList();
            ViewBag.TotalCount = planList.Count();
            ViewBag.UnAssign = planList.Where(r => r.State == false).Count();   //未派发
            ViewBag.Assign = planList.Where(r => r.State == true && r.IsFinish == 0).Count();  //已派发
            ViewBag.Reviewed = planList.Where(r => r.State == true && r.IsFinish == 1).Count();   //待审核
            ViewBag.Finish = planList.Where(r => r.State == true && r.IsFinish == 2).Count();   //已完成

            //返回巡检人员和派发人员
            var list = _assignmentService.GetCreaterAndInspector();
            ViewBag.inspectors = list;


            return View();
        }
        [HttpPost]
        [TypeFilter(typeof(IgonreActionFilter))]
        public async Task<IActionResult> LoadAssignmentPlan(string beginDate, string endDate, string message, string state, string date, string type, long creator, long inspector, string sortName, string sortOrder, int pageSize = 10, int pageIndex = 1)
        {
            //int userID = int.Parse(User.FindFirstValue("UserID"));
            string enddate = "";
            string begindate = GetDate(date, beginDate, endDate, ref enddate);
            #region 查询条件
            Expression<Func<Assignment, bool>> funcWhere = null;
            if (!string.IsNullOrWhiteSpace(message))
            {
                funcWhere = funcWhere.And(r => r.DMAName.Contains(message) || r.PlanName.Contains(message) || (r.PlanName != null && r.Remark.Contains(message)));
            }
            if (!string.IsNullOrWhiteSpace(begindate) && !string.IsNullOrWhiteSpace(enddate))
            {
                DateTime beginTime = Convert.ToDateTime(begindate);
                DateTime endTime = Convert.ToDateTime(enddate);
                funcWhere = funcWhere.And(r => r.BeginDate >= beginTime && r.BeginDate < endTime);
            }
            if (!string.IsNullOrEmpty(type) && type != "8")
            {
                funcWhere = funcWhere.And(r => r.PlanType == int.Parse(type));
            }
            if (!string.IsNullOrEmpty(state) && state != "8")
            {
                if (int.Parse(state) == 0)
                {
                    funcWhere = funcWhere.And(r => r.State == false);
                }
                if (int.Parse(state) == 1)
                {
                    funcWhere = funcWhere.And(r => r.State == true && r.IsFinish == 0);
                }
                if (int.Parse(state) == 2)
                {
                    funcWhere = funcWhere.And(r => r.State == true && r.IsFinish == 2);
                }
                if (int.Parse(state) == 3)
                {
                    funcWhere = funcWhere.And(r => r.State == true && r.IsFinish == 1);
                }
            }
            if (creator != 0)
            {
                funcWhere = funcWhere.And(r => r.Creater == creator);
            }
            if (inspector != 0)
            {
                funcWhere = funcWhere.And(r => r.Inspector == inspector);
            }
            funcWhere = funcWhere.And(r => r.IsForward != 1);
            #endregion

            #region  排序
            bool flag = true;
            if (sortOrder == "desc") flag = false;
            string sort = string.IsNullOrWhiteSpace(sortName) ? "CreateTime" : sortName;
            #endregion

            PageResult<Assignment> eventsList = this._assignmentService.LoadAssignmentList(funcWhere, pageSize, pageIndex, sort, flag);
            PartialView("_AssignmentTable", eventsList.DataList);
            string dataTable = await ViewToString.RenderPartialViewToString(this, "_AssignmentTable");
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

        //时间取值
        public static string GetDate(string date, string beginTime, string endTime, ref string endDate)
        {
            string beginDate = "";
            if (!string.IsNullOrEmpty(date))
            {
                int d = Convert.ToInt32(date);
                DateTime dt = DateTime.Now;
                var dayofWeek = (int)DateTime.Now.DayOfWeek == 0 ? 7 : (int)DateTime.Now.DayOfWeek;
                switch (d)
                {
                    case 1:
                        beginDate = dt.Date.AddDays(-1).ToString("yyyy-MM-dd");
                        endDate = dt.Date.ToString("yyyy-MM-dd");
                        break;
                    case 2:   //本周
                        beginDate = string.Format("{0:yyyy-MM-dd}", DateTime.Now.AddDays(-(dayofWeek - 1)));
                        endDate = string.Format("{0:yyyy-MM-dd}", DateTime.Now.AddDays(-(dayofWeek - 1) + 7)); ;
                        break;
                    case 3:   //上周
                        beginDate = string.Format("{0:yyyy-MM-dd}", DateTime.Now.AddDays(-dayofWeek - 7));
                        endDate = string.Format("{0:yyyy-MM-dd}", DateTime.Now.AddDays(-(dayofWeek - 1)));
                        break;
                    case 4: //下周
                        beginDate = string.Format("{0:yyyy-MM-dd}", DateTime.Now.AddDays(-(dayofWeek - 1) + 7));
                        endDate = string.Format("{0:yyyy-MM-dd}", DateTime.Now.AddDays(-(dayofWeek - 1) + 14));
                        break;
                    case 5:  //本月
                        beginDate = dt.ToString("yyyy-MM-01");
                        endDate = dt.AddDays(1).ToString("yyyy-MM-dd");
                        break;
                    case 6:   //上月
                        beginDate = DateTime.Parse(dt.ToString("yyyy-MM-01")).AddMonths(-1).ToShortDateString();
                        endDate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-01")).ToShortDateString();
                        break;
                    case 7:   //下月
                        beginDate = DateTime.Parse(dt.ToString("yyyy-MM-01")).AddMonths(1).ToShortDateString();
                        endDate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-01")).AddMonths(2).ToShortDateString();
                        break;
                    case 8:
                        beginDate = beginTime;
                        endDate = endTime;
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

        #region 新增计划、编辑、删除
        //添加
        public IActionResult AddPlan()
        {
            //区域tree
            var treenode = _areaInfoService.Query<WoAreaInfo>(r => true).Select(r => new
            {
                id = r.Id,
                name = r.AreaName,
                pId = r.Pid,
                icon = "/images/stationTree.png"
            }).ToList();


            treenode.Add(new
            {
                id = 0,
                name = "所有区域",
                pId = -1,
                icon = "/images/stationTree.png"
            }
             );
            ViewBag.TreeNode = new HtmlString(Newtonsoft.Json.JsonConvert.SerializeObject(treenode));

            var templatelist = _templateInfoService.Query<WoTemplateInfo>(t => true).ToList();
            ViewBag.templateinfo = templatelist;

            var Travel_fitemid = (int)WNMS.Model.CustomizedClass.WorkOrder_enum.行走方式;
            var Cycle_fitemid = (int)WNMS.Model.CustomizedClass.WorkOrder_enum.巡检周期;
            var dictory_data = sys_DataItemDetailService.Query<SysDataItemDetail>(r => r.FItemId == Travel_fitemid || r.FItemId == Cycle_fitemid);
            ViewBag.dictory_data = dictory_data;

            ViewBag.inspector = "";
            ViewBag.planObjects = "";
            ViewBag.DmaArea = "";

            ViewBag.type = 0;
            return View("_SetAssignmentPlan", new WoAssignmentPlan());
        }
        public IActionResult EditePlan(long id)
        {
            //区域tree
            var treenode = _areaInfoService.Query<WoAreaInfo>(r => true).Select(r => new
            {
                id = r.Id,
                name = r.AreaName,
                pId = r.Pid,
                icon = "/images/stationTree.png"
            }).ToList();


            treenode.Add(new
            {
                id = 0,
                name = "所有区域",
                pId = -1,
                icon = "/images/stationTree.png"
            }
             );
            ViewBag.TreeNode = new HtmlString(Newtonsoft.Json.JsonConvert.SerializeObject(treenode));

            var Travel_fitemid = (int)WNMS.Model.CustomizedClass.WorkOrder_enum.行走方式;
            var Cycle_fitemid = (int)WNMS.Model.CustomizedClass.WorkOrder_enum.巡检周期;
            var dictory_data = sys_DataItemDetailService.Query<SysDataItemDetail>(r => r.FItemId == Travel_fitemid || r.FItemId == Cycle_fitemid);
            ViewBag.dictory_data = dictory_data;

            var model = _assignmentService.Query<WoAssignmentPlan>(r => r.PlanId == id).FirstOrDefault();

            //返回巡检对象，区域名称、巡检人员
            var data = _assignmentService.GetAssignmentPlanInfo(model.PlanId).ToList();
            string devicelist = string.Empty;
            string devicename = string.Empty;
            if (data != null)
            {
                ViewBag.inspector = data[0].InspectorName;
                ViewBag.DmaArea = data[0].DMAName;
                foreach (var item in data)
                {
                    devicelist += item.InspectObject + "," + item.PumpStationID + ";";
                    devicename += item.DeviceName + ",";
                }
                if (devicelist != "")
                {
                    devicelist = devicelist.Substring(0, devicelist.Length - 1);
                }
                if (devicename != "")
                {
                    devicename = devicename.Substring(0, devicename.Length - 1);
                }
                ViewBag.planObjects = devicelist;
                ViewBag.DeviceNames = devicename;
            }
            else
            {

                ViewBag.inspector = "";
                ViewBag.planObjects = "";
                ViewBag.DmaArea = "";
            }

            var templatelist = _templateInfoService.Query<WoTemplateInfo>(t => true).ToList();
            ViewBag.templateinfo = templatelist;

            ViewBag.type = model.Type;
            return View("_SetAssignmentPlan", model);

        }
        //表单提交
        [HttpPost]
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult SetAssignmentInfo(WoAssignmentPlan plan, String InspectObjectID)
        {

            long planID = 0;
            if (plan.PlanId == 0)
            {
                planID = ConvertDateTimeInt(DateTime.Now);
            }

            var inspectObject = InspectObjectID.Split(';');
            List<WoPlanInspectO> planInspectO = new List<WoPlanInspectO>();
            foreach (var item in inspectObject)
            {
                var itArray = item.Split(',');
                WoPlanInspectO inspectO = new WoPlanInspectO();
                inspectO.PlanId = plan.PlanId == 0 ? planID : plan.PlanId;
                inspectO.InspectObject = Convert.ToInt64(itArray[0]);
                inspectO.IsTemplate = false;//是计划模板
                inspectO.TemplateId = (long)plan.TemplateId;
                inspectO.ObjectName = "设备";
                inspectO.PumpStationId = Convert.ToInt32(itArray[1]);
                planInspectO.Add(inspectO);
            }

            if (plan.PlanId == 0)//添加
            {

                plan.PlanId = planID;
                plan.State = true;
                //获取当前用户信息
                int userID = int.Parse(User.FindFirstValue("UserID"));
                plan.Creater = userID;
                plan.PlanType = (byte)PlanType.常规;
                plan.CreateTime = DateTime.Now;
                if (_assignmentService.AddPlan(plan, planInspectO) > 0)
                {
                    return Content("ok");
                }
                else
                {
                    return Content("no");
                }
            }
            else
            {//修改
                //plan.EndDate = Convert.ToDateTime(string.Format("{0:yyyy-MM-dd}", plan.EndDate.Date) + " 23:59:59");
                if (_assignmentService.EditePlan(plan, planInspectO) > 0)
                {
                    return Content("ok");
                }
                else
                {
                    return Content("no");
                }
            }
        }

        //删除
        [HttpPost]
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult DeletePlans(string id)
        {
            if (_assignmentService.DeletePlan(id) > 0)
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }
        }

        //时间戳
        public static long ConvertDateTimeInt(DateTime time)
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            return (long)(time - startTime).TotalMilliseconds;
        }
        #endregion

        #region 查看
        //页面返回
        public IActionResult WatchPlan(long id)
        {
            ViewBag.PlanID = id;
            ViewBag.PlanData = _assignmentService.GetWatchData(id).FirstOrDefault();
            return View();
        }

        //查询列表数据
        [HttpPost]
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult WatchPlanDetail(long PlanID, int type)
        {
            var data = _assignmentService.GetWatchDetailData(PlanID, type);
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(data));
        }

        //查询反馈详情
        [HttpPost]
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult GetTaskDetail(long feedID)
        {
            string feedbackData = "";
            string GISLocation = "";
            var feedback = _isnpectPlanCheckService.Query<WoInspectPlanCheck>(r => r.Id == feedID).FirstOrDefault();

            //获取设备巡检图片
            List<string> imgelist = new List<string>();
            if (feedback != null)
            {
                if (!string.IsNullOrEmpty(feedback.InspectImage))
                {
                    if (feedback.InspectImage.Contains(','))
                    {
                        List<string> LstInspectImage = feedback.InspectImage.Split(',').ToList();
                        foreach (var item in LstInspectImage)
                        {
                            string InspectImageFile = "/UploadImg/设备巡检/" + item;
                            imgelist.Add(InspectImageFile);
                        }
                    }
                    else
                    {
                        string InspectImageFile = "/UploadImg/设备巡检/" + feedback.InspectImage;
                        imgelist.Add(InspectImageFile);
                    }
                }
            }

            IEnumerable<dynamic> dataInfos = null;
            if (feedback != null)
            {

                feedbackData = feedback.DetailContent;
                GISLocation = feedback.Gislocation;
                //根据设备id以及planid查出模板包含的反馈项
                dataInfos = _assignmentService.GetFeedDataInfo(feedback.PlanId);
            }
            return Json(new
            {
                imgelist = imgelist,
                GISLocation = GISLocation,
                feedbackData = feedbackData,
                dataInfos = Newtonsoft.Json.JsonConvert.SerializeObject(dataInfos)
            });
        }
        //反馈设备详情
        [HttpPost]
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult GetDeviceDetail(string deviceId)
        {
            dynamic data = null;
            if (deviceId != "")
            {
                long ldeviceID = Convert.ToInt64(deviceId);
                data = _assignmentService.GetDeviceData(ldeviceID).FirstOrDefault();
            }
            return Json(new
            {
                data = Newtonsoft.Json.JsonConvert.SerializeObject(data)
            });
        }

        //图片预览查看
        [TypeFilter(typeof(IgonreActionFilter))]
        public ActionResult ImagePreview(string Url)
        {
            ViewBag.ImageUrl = Url;
            return View();
        }
        #endregion

        #region 任务分派
        [HttpPost]
        [TypeFilter(typeof(IgonreActionFilter))]
        public ActionResult AssignmentPlan(string PlanIDs)
        {
            if (_assignmentService.AssignPlan(PlanIDs) > 0)
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }
        }
        #endregion

        #region 审核巡检工单
        //public IActionResult AuditAss(long planid, byte state)
        //{
        //    int userID = int.Parse(User.FindFirstValue("UserID"));
        //    var user = _sysUserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
        //    // 审核通过 = 1，审核未通过 = 2 ，未审核 = 3 
        //    var assInfo = _assignmentService.Find<WoAssignmentPlan>(planid);
        //    if (assInfo.IsAuditing == 1)
        //    {
        //        return Content("isAudit");
        //    }
        //    if (state == 1)
        //    {
        //        //审核通过，直接更新审核信息 
        //        if (assInfo != null)
        //        {
        //            assInfo.IsAuditing = state;
        //            assInfo.AuditUser = user.UserId;
        //            if (_assignmentService.Update(assInfo))
        //            {
        //                return Content("ok");
        //            }
        //            else
        //            {
        //                return Content("no");
        //            }
        //        }
        //        else
        //        {
        //            return Content("no");
        //        }
        //    }
        //    else
        //    {
        //        //审核不通过 重置该工单下的设备信息 删除延期申请（防止巡检时间超期，无法再申请延期）
        //        //查询该工单的所有未转发设备信息
        //        //
        //        var eqInfo = _wO_PlanInspectOService.Query<WoPlanInspectO>(r => r.PlanId == planid && r.ForwardState == null).ToList();
        //        var extensionInfo = _wO_InsExtensionService.Query<WoInsExtension>(r => r.PlanId == planid).ToList();
        //        if (assInfo.IsFinish == true)
        //        {
        //            assInfo.IsFinish = false;
        //            assInfo.IsAuditing = state;
        //            assInfo.AuditUser = user.UserId;
        //        }
        //        if (_assignmentService.AuditAss(assInfo, eqInfo, extensionInfo) > 0)
        //        {
        //            return Content("ok");
        //        }
        //        else
        //        {
        //            return Content("no");
        //        }
        //    }
        //}
        //审核设备

        /// <summary>
        /// 审核设备
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult AuditEq(long id)
        {
            var feedback = _isnpectPlanCheckService.Query<WoInspectPlanCheck>(r => r.Id == id).FirstOrDefault();
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = _sysUserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
            // 审核通过 = 1，审核未通过 = 2 ，未审核 = 3 
            //查询巡检设备表设备信息  上传反馈信息的时候 要修改一下设备表的设备审核状态
            var eqInfo = _wO_PlanInspectOService.Query<WoPlanInspectO>(r => r.PlanId == feedback.PlanId && r.InspectObject == feedback.EquipmentId).FirstOrDefault();
            eqInfo.IsAuditing = 2;
            eqInfo.AuditUser = user.UserId;
            //驳回设备  即删除改设备的反馈信息
            if (_assignmentService.AuditEq(eqInfo, feedback) > 0)
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }
        }
        /// <summary>
        /// 标记巡检完成
        /// </summary>
        /// <param name="planid"></param>
        /// <returns></returns>
        public IActionResult AuditAssFinish(long planid)
        {
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = _sysUserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
            // 审核通过 = 1，审核未通过 = 2 ，未审核 = 3 
            //0是未完成1是待审核2是已审核
            var assInfo = _assignmentService.Find<WoAssignmentPlan>(planid);
            if (assInfo.IsFinish == 2)
            {
                return Content("isFinish");
            }
            else
            {
                //查询一下是否有未巡检的设备
                int eQInfo = _wO_PlanInspectOService.Query<WoPlanInspectO>(r => r.PlanId == planid && r.ForwardState == null).Count();
                int checkEq = _isnpectPlanCheckService.Query<WoInspectPlanCheck>(r => r.PlanId == planid).Count();

                if (eQInfo != checkEq)
                {
                    return Content("isnoChecked");
                }
                assInfo.IsFinish = 2;
                if (_assignmentService.Update(assInfo))
                {
                    return Content("ok");
                }
                else
                {
                    return Content("no");
                }
            }

        }

        #endregion
    }
}
