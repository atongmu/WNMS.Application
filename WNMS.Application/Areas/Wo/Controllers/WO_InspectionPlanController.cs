using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WNMS.IService;
using WNMS.Service;
using WNMS.Model.DataModels;
using Microsoft.AspNetCore.Html;
using System.Data;
using WNMS.Model.CustomizedClass;
using System.Security.Claims;
using System.Linq.Expressions;
using WNMS.Utility;
using WNMS.Application.Utility.Filters;

namespace WNMS.Application.Areas.Wo.Controllers
{
    [Area("Wo")]
    public class WO_InspectionPlanController : Controller
    {
        private IWO_TemplateInfoService templateInfoService= null;
        private IWO_InspectionPlanService wO_InspectionPlanService = null;
        private ISys_DataItemDetailService sys_DataItemDetailService = null;
        private ISysUserService sysUserService = null;
        private IWO_FeedBackInfoService WO_FeedBackInfoService = null;
        private IWO_AreaInfoService wO_AreaInfoService = null;
        private IWO_AssignmentPlanService wO_AssignmentPlanService = null;
        private IWO_PlanInspectOService wO_PlanInspectOService = null;
        public WO_InspectionPlanController(IWO_TemplateInfoService _TemplateInfoService,
            IWO_InspectionPlanService _InspectionPlanService,
            ISys_DataItemDetailService _DataItemDetailService,
            ISysUserService userService,
            IWO_FeedBackInfoService _FeedBackInfoService,
            IWO_AreaInfoService _AreaInfoService,
            IWO_AssignmentPlanService _AssignmentPlanService,
            IWO_PlanInspectOService _PlanInspectOService) {
            templateInfoService = _TemplateInfoService;
            wO_InspectionPlanService = _InspectionPlanService;
            sys_DataItemDetailService = _DataItemDetailService;
            sysUserService = userService;
            WO_FeedBackInfoService = _FeedBackInfoService;
            wO_AreaInfoService = _AreaInfoService;
            wO_AssignmentPlanService = _AssignmentPlanService;
            wO_PlanInspectOService = _PlanInspectOService;
        }
        public IActionResult Index()
        {
            var list = wO_InspectionPlanService.LoadCreaterAndInspector();
            ViewBag.inspectors = list;
            return View();
        }
        //查询计划表格
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult QueryInspectPlanTable(int pagesize, int pageindex, string SearchText, string order, string sort, string time, string beginDate, string endDate,int creater,int inspector,int? isEnabled) {
            int Totalcount = 0;
            string ordertems = sort + " " + order;
            string filter = "";
            
            if (!string.IsNullOrEmpty(time))
            {
                GetBeginDate(time, ref beginDate, ref endDate);
                if (!string.IsNullOrEmpty(beginDate) && !string.IsNullOrEmpty(endDate))
                {


                    filter += " ((BeginTime<'" + endDate + "' and BeginTime>='" + beginDate + "' ) or (EndTime>='" + beginDate + "' and EndTime<'" + endDate + "') or (BeginTime<='" + beginDate + "' and EndTime>'" + endDate + "'))";


                }
                else if (!string.IsNullOrEmpty(beginDate))
                {
                    filter += "EndTime>='" + beginDate + "'";
                }
                else if (!string.IsNullOrEmpty(endDate))
                {
                    filter += "BeginTime<'" + endDate + "'";
                }

            }
            if (!string.IsNullOrEmpty(SearchText))
            {
                if (filter != "")
                {
                    filter += " and ";
                }
                filter += " ( PlanName like '%"+ SearchText + "%' or AreaName like '%"+ SearchText + "%' or TemplateName like '%" + SearchText + "%')";
            }
            if (creater != 0)
            {
                if (filter != "")
                {
                    filter += " and ";
                }
                filter += " CreateUser="+ creater + "";
            
            }
            if (inspector != 0)
            {
                if (filter != "")
                {
                    filter += " and ";
                }
                filter += " Inspector=" + inspector + "";
            }
            if (isEnabled != null)
            {
                if (filter != "")
                {
                    filter += " and ";
                }
                filter += " EnabledMark=" + isEnabled + "";
            }
            var Travel_fitemid = (int)WNMS.Model.CustomizedClass.WorkOrder_enum.行走方式;
            var Cycle_fitemid= (int)WNMS.Model.CustomizedClass.WorkOrder_enum.巡检周期;
            var datalist = wO_InspectionPlanService.QueryInspectPlanTable(pageindex, pagesize, ordertems, filter, Travel_fitemid.ToString(), Cycle_fitemid.ToString(), ref Totalcount);
            return Json(new { total = Totalcount, rows = Newtonsoft.Json.JsonConvert.SerializeObject(datalist) });
        }
        //获取查询开始日期
        void GetBeginDate(string executeTime, ref string BeginTime, ref string EndTime)
        {
            switch (executeTime)
            {
                case "昨天":
                    BeginTime = string.Format("{0:yyyy-MM-dd}", DateTime.Now.AddDays(-1));
                    EndTime = string.Format("{0:yyyy-MM-dd}", DateTime.Now);
                    break;
                case "上周":
                    var dayofWeek = (int)DateTime.Now.DayOfWeek == 0 ? 7 : (int)DateTime.Now.DayOfWeek;
                    BeginTime = string.Format("{0:yyyy-MM-dd}", DateTime.Now.AddDays(-(dayofWeek - 1) - 7));
                    EndTime = string.Format("{0:yyyy-MM-dd}", DateTime.Now.AddDays(-(dayofWeek - 1)));
                    break;
                case "本周":
                    var dayofWeek1 = (int)DateTime.Now.DayOfWeek == 0 ? 7 : (int)DateTime.Now.DayOfWeek;
                    BeginTime = string.Format("{0:yyyy-MM-dd}", DateTime.Now.AddDays(-(dayofWeek1 - 1)));
                    EndTime = string.Format("{0:yyyy-MM-dd}", DateTime.Now.AddDays(-(dayofWeek1 - 1) + 7));
                    break;
                case "下周":
                    var dayofWeek2 = (int)DateTime.Now.DayOfWeek == 0 ? 7 : (int)DateTime.Now.DayOfWeek;
                    BeginTime = string.Format("{0:yyyy-MM-dd}", DateTime.Now.AddDays(-(dayofWeek2 - 1) + 7));
                    EndTime = string.Format("{0:yyyy-MM-dd}", DateTime.Now.AddDays(-(dayofWeek2 - 1) + 14));
                    break;
                case "上月":
                    BeginTime = string.Format("{0:yyyy-MM}", DateTime.Now.AddMonths(-1)) + "-01";
                    EndTime = string.Format("{0:yyyy-MM}", DateTime.Now) + "-01";
                    break;
                case "本月":
                    BeginTime = string.Format("{0:yyyy-MM}", DateTime.Now) + "-01";
                    EndTime = string.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(string.Format("{0:yyyy-MM-01}", DateTime.Now.AddMonths(1))));
                    break;
                case "下月":
                    BeginTime = string.Format("{0:yyyy-MM}", DateTime.Now.AddMonths(1)) + "-01";
                    EndTime = string.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(string.Format("{0:yyyy-MM-01}", DateTime.Now.AddMonths(2))));
                    break;
                case "自定义":
                    if (EndTime != null)
                    {
                        EndTime = string.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(EndTime).AddDays(1));
                    }
                    break;
                default:
                    break;

            }

        }
        //更改停止启用功能
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult UpdateEnable(long planid,bool state)
        {
            var model = wO_InspectionPlanService.Query<WoInspectionPlan>(r => r.Id == planid).FirstOrDefault();
            model.EnabledMark = state;
            try {
                wO_InspectionPlanService.Update(model);
                return Content("ok");
            }
            catch (Exception e)
            {
                return Content("no");
            }
        }
        #region 根据巡检计划id 获取任务
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult QueryAssignTable(long id) 
        {
            ViewBag.planid = id;
            return View();
        }
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult QueryAssignTableByID(int pagesize,int pageindex,string order,string sort,string beginDate,string endDate,long planid)
        {
            Expression<Func<Assignment, bool>> funcWhere = (r => r.TemplatePlanID == planid);
           
            if (!string.IsNullOrWhiteSpace(beginDate))
            {
                DateTime beginTime = Convert.ToDateTime(beginDate);
                
                funcWhere = funcWhere.And(r => r.BeginDate >= beginTime);
            }
            if (!string.IsNullOrWhiteSpace(endDate))
            {
                
                DateTime endTime = Convert.ToDateTime(endDate);
                funcWhere = funcWhere.And(r =>r.BeginDate <= endTime);
            }
            #region  排序
            bool flag = true;
            if (order == "desc") flag = false;
            sort = string.IsNullOrWhiteSpace(sort) ? "CreateTime" : sort;
            PageResult<Assignment> eventsList = wO_AssignmentPlanService.LoadAssignmentList(funcWhere, pagesize, pageindex, sort, flag);
            return Json(new { total = eventsList.TotalCount, rows = Newtonsoft.Json.JsonConvert.SerializeObject(eventsList.DataList) });
            #endregion
        }
        #endregion
        #region 添加、修改、删除巡检计划
        public IActionResult AddPlan()
        {
            var Travel_fitemid = (int)WNMS.Model.CustomizedClass.WorkOrder_enum.行走方式;
            var Cycle_fitemid = (int)WNMS.Model.CustomizedClass.WorkOrder_enum.巡检周期;
            var dictory_data = sys_DataItemDetailService.Query<SysDataItemDetail>(r => r.FItemId == Travel_fitemid|| r.FItemId== Cycle_fitemid);
            ViewBag.dictory_data = dictory_data;
            ViewBag.templateinfo= templateInfoService.Query<WoTemplateInfo>(r => true);//测试为空

            //区域tree
            var treenode = wO_AreaInfoService.Query<WoAreaInfo>(r => true).Select(r => new
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
            ViewBag.inspector = "";
            ViewBag.planObjects = "";
            ViewBag.DmaArea = "";
            return View("_SetInspectPlan",new WoInspectionPlan());
        }
        //修改
        public IActionResult EditePlan(long id)
        {
            var Travel_fitemid = (int)WNMS.Model.CustomizedClass.WorkOrder_enum.行走方式;
            var Cycle_fitemid = (int)WNMS.Model.CustomizedClass.WorkOrder_enum.巡检周期;
            var dictory_data = sys_DataItemDetailService.Query<SysDataItemDetail>(r => r.FItemId == Travel_fitemid || r.FItemId == Cycle_fitemid);
            ViewBag.dictory_data = dictory_data;
            ViewBag.templateinfo = templateInfoService.Query<WoTemplateInfo>(r => true);//测试为空

            //区域tree
            var treenode = wO_AreaInfoService.Query<WoAreaInfo>(r => true).Select(r => new
            {
                id = r.Id,
                name = r.AreaName,
                pId = r.Pid,
                icon = "/images/bf.png"
            }).ToList();


            treenode.Add(new
            {
                id = 0,
                name = "所有区域",
                pId = -1,
                icon = "/images/bf.png"
            }
             );
            ViewBag.TreeNode = new HtmlString(Newtonsoft.Json.JsonConvert.SerializeObject(treenode));
            var model = wO_InspectionPlanService.Query<WoInspectionPlan>(r => r.Id == id).FirstOrDefault();
            //巡检人员
            ViewBag.inspector = sysUserService.Query<SysUser>(r => r.UserId == model.Inspector).FirstOrDefault()?.NickName;
            //巡检设备
            var PlanO = wO_PlanInspectOService.Query<WoPlanInspectO>(r => r.PlanId == model.Id && r.IsTemplate == true);
            var planObjects = "";
            if (PlanO.Count() > 0)
            {
                foreach (var item in PlanO)
                {
                    planObjects += item.InspectObject + "," + item.PumpStationId + ";";

                }
                if (planObjects != "")
                {
                    planObjects = planObjects.Substring(0, planObjects.Length-1);
                }
            }
            ViewBag.planObjects = planObjects;
            ViewBag.DmaArea=treenode.Where(r => r.id == model.Dmaid).FirstOrDefault()?.name;
            return View("_SetInspectPlan", model);
        }
        //删除
        public IActionResult DeletePlan(string ids)
        {
            List<long> planids = new List<string>(ids.Split(',')).ConvertAll(r => long.Parse(r));
           // wO_AssignmentPlanService.DeleteSchedule(planids);    //删除jpush定时任务
            if (wO_InspectionPlanService.DeleteInspectPlan(planids) > 0)
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }
        }
        //根据区域id获取区域信息以及区域中设备数量
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult GetAreaInfoByID(int areaid) {
            var query = wO_InspectionPlanService.GetAreaInfoByID(areaid);
         return Content(Newtonsoft.Json.JsonConvert.SerializeObject(query));
        }
        #region 添加巡检设备
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult AllotDevice(int areaid,string devicelist_Sel)
        {
            IEnumerable<AllotDevice> allotDevice = null ;
            string nodes="";
            GetNodeOfTree("",areaid,devicelist_Sel,ref nodes,ref allotDevice);
            ViewBag.ztreenode = new HtmlString(nodes);
            
            ViewBag.allotDevice = allotDevice;
            return View();
        }
        class treeNode { 
            public long id { get; set; }
            public long pId { get; set; }
            public string name { get; set; }
            public bool isDevice { get; set; }
            public bool nocheck { get; set; }
            public bool? @checked { get; set; }
            public string icon { get; set; }
        }
        //查询树
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult GetTreeNodes(string searchtxt,int areaid,string devicelist_Sel)
        {
            IEnumerable<AllotDevice> allotDevice = null;
            string nodes = "";
            GetNodeOfTree(searchtxt, areaid,devicelist_Sel, ref nodes, ref allotDevice);
            return Content(new HtmlString(nodes).ToString());
        }
        void GetNodeOfTree(string searchtxt, int areaid, string devicelist_Sel, ref string nodes, ref IEnumerable<AllotDevice> allotDevice)
        {
          
            //获取已经分配的设备
            DataTable tvpDt = new DataTable();
            tvpDt.Columns.Add("DeviceID", typeof(long));
            tvpDt.Columns.Add("StationID", typeof(int));
            if (!string.IsNullOrEmpty(devicelist_Sel))
            {
                var dataArray = devicelist_Sel.Split(';');
                foreach (var item in dataArray)
                {
                    var it = item.Split(',');
                    tvpDt.Rows.Add(long.Parse(it[0]), int.Parse(it[1]));
                }
            }
            searchtxt = searchtxt == null ? "" : searchtxt;
            //根据区域id 获取所有的设备
            var alldevice = wO_InspectionPlanService.GetAllDeviceByID(areaid, searchtxt);
            if (tvpDt.Rows.Count > 0)
            {
                allotDevice = wO_InspectionPlanService.GetAllotDevice(tvpDt);
            }
            if (alldevice.Count() > 0)
            {
                
                
                var unAllotDevice = alldevice;
                if (allotDevice!=null &&allotDevice.Count() > 0)
                {
                    unAllotDevice = alldevice.Except(allotDevice,new AllotDeviceCompare());//待测试
                }
                var stationNode = alldevice.Select(r => new { r.StationID, r.StationName }).Distinct().Select(r => new treeNode
                {
                    id = r.StationID,
                    pId = 0,
                    name = "<em class='iconfont icon-bengfang'></em>"+ r.StationName,
                    isDevice = false,
                    nocheck = true,
                    icon = ""
                }).ToList();
                var checknodes = allotDevice?.Where(r=>r.DeviceName.Contains(searchtxt)|| r.StationName.Contains(searchtxt)).Select(r => new treeNode
                {
                    id = r.DeviceID,
                    pId = r.StationID,
                    name = r.DeviceName,
                    isDevice = true,
                    nocheck = false,
                    @checked = true,
                    icon = ""
                });
                var unchecknodes = unAllotDevice.Select(r => new treeNode
                {
                    id = r.DeviceID,
                    pId = r.StationID,
                    name = r.DeviceName,
                    isDevice = true,
                    nocheck = false,
                    @checked = false,
                    icon = ""
                });
                if (checknodes!=null&&checknodes.Count() > 0)
                {
                    stationNode = stationNode.Union(checknodes).ToList();
                }
                if (unchecknodes.Count() > 0)
                {
                    stationNode = stationNode.Union(unchecknodes).ToList();
                }
                nodes = Newtonsoft.Json.JsonConvert.SerializeObject(stationNode);
            }
            else
            {
                nodes = "[]";
            }
        }
        public class AllotDeviceCompare : IEqualityComparer<AllotDevice>
        {
            public bool Equals(AllotDevice x, AllotDevice y)
            {
                if (x == null)
                    return y == null;
                return x.DeviceID == y.DeviceID && x.StationID==y.StationID;
            }

            public int GetHashCode(AllotDevice obj)
            {
                if (obj == null)
                    return 0;

                return obj.DeviceID.GetHashCode()^obj.StationID.GetHashCode();
                
            }
        }

        #endregion
        //选择巡检人员
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult SelectInspector(long areaid,int userID)
        {
            var datalist = wO_AssignmentPlanService.GetUserInfoList(areaid);
            ViewBag.userlist = datalist;
            ViewBag.UserID = userID;
            return View();
        }
        //提交表单
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult SetInspectionPlan(WoInspectionPlan p,string InspectObjectID)
        {
            long planID = p.Id;
            if (p.Id == 0)
            {
                planID = ConvertDateTimeInt(DateTime.Now);
            }

            List<WoPlanInspectO> planInspectO = new List<WoPlanInspectO>();
            var DS_array = InspectObjectID.Split(';');
            foreach (var item in DS_array)
            {
                var itArray = item.Split(',');
                WoPlanInspectO po = new WoPlanInspectO();
                po.PlanId = planID;
                po.IsTemplate = true;
                po.InspectObject = long.Parse(itArray[0]);
                po.TemplateId = 0;
                po.PumpStationId = int.Parse(itArray[1]);
                po.ObjectName = "设备";
                planInspectO.Add(po);
            }
            p.ModifyDate = DateTime.Now;
            try
            {
                if (p.Id == 0)//添加
                {
                    p.Id = planID;
                    p.CreateDate = DateTime.Now;
                    p.EnabledMark = true;
                    p.CreateUser =int.Parse(User.FindFirstValue("UserID"));
                    if (wO_InspectionPlanService.AddInspectPlan(p, planInspectO) > 0)
                    {
                        CreateScheduleTask(p.Id);  //创建定时任务
                        return Content("ok");
                    }
                    else
                    {
                        return Content("no");
                    }
                }
                else
                {
                    if (wO_InspectionPlanService.EditeInspectPlan(p, planInspectO) > 0)
                    {
                        if (string.IsNullOrEmpty(p.ScheduleId))
                        {
                            CreateScheduleTask(p.Id);  //创建定时任务
                        }
                        else
                        {
                            EditScheduleTask(p);     //修改定时任务
                        }
                        return Content("ok");
                    }
                    else
                    {
                        return Content("no");
                    }
                }
            }
            catch (Exception e)
            {
                var aa = e;
                return Content("no");
            }
        }

        //创建定时任务
        public void CreateScheduleTask(long id)
        {
            string sid=wO_AssignmentPlanService.CreateSchedule(id);
            WoInspectionPlan wp = wO_InspectionPlanService.Query<WoInspectionPlan>(r => r.Id == id).FirstOrDefault();
            if (wp != null)
            {
                wp.ScheduleId = sid;
                wO_InspectionPlanService.Update<WoInspectionPlan>(wp);
            }
        }

        //修改定时任务
        public void EditScheduleTask(WoInspectionPlan wp)
        {
            string sid = wO_AssignmentPlanService.EditSchedule(wp);
            if (!string.IsNullOrEmpty(sid))
            {
                wp.ScheduleId = sid;
                wO_InspectionPlanService.Update<WoInspectionPlan>(wp);
            }
        }


        //删除定时任务

        #endregion


        #region 巡检模板
        public IActionResult SetTemplate()
        {
            var templatelist = templateInfoService.Query<WoTemplateInfo>(t => true).ToList();
            ViewBag.templatelist = templatelist;
            return View();
        }
        //查询所有子项
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult QueryFeedBackItem(string TemplateID)
        {
            long wo_templateID = 0;
            if (!string.IsNullOrEmpty(TemplateID))
            {
                wo_templateID =long.Parse(TemplateID);
            }
            var feedback_fitem = (int)WNMS.Model.CustomizedClass.WorkOrder_enum.反馈项类型;
            var datalist = wO_InspectionPlanService.GetFeedBackItems(wo_templateID, feedback_fitem).ToList();
            return Json(new {  rows = Newtonsoft.Json.JsonConvert.SerializeObject(datalist) });
        }
        #region 模板
        //添加修改模板
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult SetTemplateInfo(long tid,string tname,string fb_ids) 
        {
            long templateid = tid;
            var result = "";
            if (tid == 0)
            {
                templateid = ConvertDateTimeInt(DateTime.Now);
            }
            //判断模板名称是否重复
            var hasmodel = templateInfoService.Query<WoTemplateInfo>(r => r.TemplateName == tname && r.Id != tid).FirstOrDefault();
            if (hasmodel != null)
            {
                return Content("该模板名称已存在，请重新输入");
            }
            List<WoFbOfTemplate> fbTemplateList = new List<WoFbOfTemplate>() { };
            var fbid_array = fb_ids.Split(',');
            foreach (var item in fbid_array) {
                WoFbOfTemplate w = new WoFbOfTemplate();
                w.FeedBackId = int.Parse(item);
                w.TemplateId = templateid;
                fbTemplateList.Add(w);

            }
            WoTemplateInfo tt = new WoTemplateInfo();
            tt.Id = templateid;
            tt.TemplateName = tname;
            if (tid == 0)//添加
            {
               
                if (wO_InspectionPlanService.AddTemplate(tt, fbTemplateList) > 0)
                {
                    result = "ok";
                   
                }
                else
                {
                    result = "no";
                    
                }
            }
            else//修改
            {
                if (wO_InspectionPlanService.EditeTemplate(tt, fbTemplateList) > 0)
                {
                    result = "ok";
                    
                }
                else
                {
                    result = "no";
                }
            }
            return Json(new
            {
                result,
                templateid
            });
        }
        //时间戳
        public static long ConvertDateTimeInt(DateTime time)
        {
            System.DateTime startTime = System.TimeZoneInfo.ConvertTimeToUtc(new System.DateTime(1970, 1, 1));
            return (long)(time - startTime).TotalMilliseconds;
        }
        //模板刷新
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult TemplateRefresh()
        {
            var data = templateInfoService.Query<WoTemplateInfo>(r => true);
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(data));
        }
        //删除模板
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult Deletetemp(long templateid)
        {
            //先判断是否被占用
            var plan = wO_InspectionPlanService.Query<WoInspectionPlan>(r => r.TemplateId == templateid).FirstOrDefault();
            var assign = wO_AssignmentPlanService.Query<WoAssignmentPlan>(r => r.TemplateId == templateid).FirstOrDefault();
            if (plan == null &&assign == null)
            {
                if (wO_InspectionPlanService.DeleteTemplate(templateid) > 0)
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
                return Content("have");
            }
        }
        //自定义模板
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult NewTemplate()
        {
            return View();
        }
        #endregion
        #region 反馈项
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult AddGD_FeedBackInfo()
        {
            var f_itemid = (int)WNMS.Model.CustomizedClass.WorkOrder_enum.反馈项类型;
            var feedbackType = sys_DataItemDetailService.Query<SysDataItemDetail>(r=>r.FItemId==f_itemid);
            ViewBag.feedbackType = feedbackType;
            return View("_SetFeedBackInfo",new WoFeedBackInfo());
        }
        //修改反馈项
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult EditeGD_FeedBackInfo(int id) {
            var model = WO_FeedBackInfoService.Query<WoFeedBackInfo>(r => r.Id == id).FirstOrDefault();
            var f_itemid = (int)WNMS.Model.CustomizedClass.WorkOrder_enum.反馈项类型;
            var feedbackType = sys_DataItemDetailService.Query<SysDataItemDetail>(r => r.FItemId == f_itemid);
            ViewBag.feedbackType = feedbackType;
            return View("_SetFeedBackInfo", model);
        }
        //提交表单
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult SetFeedBackInfo(WoFeedBackInfo fb) 
        {
            if (fb.Id == 0)//添加
            {
                fb.Id = sysUserService.QueryID("ID", "WO_FeedBackInfo");
                try
                {
                    WO_FeedBackInfoService.Insert<WoFeedBackInfo>(fb);
                    return Content("ok");
                }
                catch (Exception e)
                {
                    return Content("no");
                }
            }
            else//修改
            {
                try {
                    WO_FeedBackInfoService.Update<WoFeedBackInfo>(fb);
                    return Content("ok");
                }
                catch (Exception e)
                {
                    return Content("no");
                }
            }
        }
        //删除
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult DeleteFB(int id)
        {
            if (wO_InspectionPlanService.DeleteFeedBack(id) > 0)
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }
        }
        #endregion
        #endregion
    }
}