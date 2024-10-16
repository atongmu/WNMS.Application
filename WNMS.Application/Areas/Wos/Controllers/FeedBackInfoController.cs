using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WNMS.IService;
using WNMS.Service;
using WNMS.Utility;
using WNMS.Model.DataModels;
using MongoDBHelper;
using WNMS.Model.CustomizedClass;
using WNMS.Application.Utility.Filters;
using Microsoft.AspNetCore.Html;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.Extensions.FileProviders;

namespace WNMS.Application.Areas.Wos.Controllers
{
    [Area("Wos")]
    public class FeedBackInfoController : Controller
    {
        private ISws_AccessControlService _AccessControlService = null;
        private ISysUserService _UserService = null;
        private IGD_InspectionService _InspectionService = null;
        private ISws_StationService _StationService = null;
        private ISys_DataItemDetailService _DataItemDetailService = null;
        private IGD_RepairService _RepairService = null;
        int timevalue = int.Parse(StaticConstraint.TimeValue);
        private readonly IWebHostEnvironment _webHostEnvironment;
        private IGD_ResourceService _ResourceService = null;
        private IGD_MaintainService _MaintainService = null;
        public FeedBackInfoController(ISws_AccessControlService sws_AccessControlService,
            ISysUserService sysUserService,
            IGD_InspectionService gD_InspectionService,
            ISws_StationService sws_StationService,
            ISys_DataItemDetailService sys_DataItemDetailService,
            IGD_RepairService gD_RepairService,
            IWebHostEnvironment webHostEnvironment,
            IGD_ResourceService gD_ResourceService,
            IGD_MaintainService gD_MaintainService)
        {
            _AccessControlService = sws_AccessControlService;
            _UserService = sysUserService;
            _InspectionService = gD_InspectionService;
            _StationService = sws_StationService;
            _DataItemDetailService = sys_DataItemDetailService;
            _RepairService = gD_RepairService;
            _webHostEnvironment = webHostEnvironment;
            _ResourceService = gD_ResourceService;
            _MaintainService = gD_MaintainService;
        }
        public IActionResult Index()
        {
            var data = GetStationList("");
            ViewBag.stationState = data;
            return View();
        }
        #region 左侧泵房状态
        
        private IEnumerable<dynamic> GetStationList(string stationName)
        {
            int userID = int.Parse(User.FindFirstValue("UserID"));

            var user = _UserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
            var offtime = DateTime.Now.AddMinutes(-timevalue);//掉线时间
            string macJson = "{'DigitalValues.1001':true,'DigitalValues.1000':true,'UpdateTime':{'$gte':ISODate('" + offtime.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'") + "')}}";

            var collections = new MongoDBHelper<RtuJKInfo>(DateTime.Now.Year.ToString());
            var datalist = collections.Find("Sws_DeviceJKInfo", macJson, "UpdateTime").Select(r => r.RTUID).ToList();
            string onlinertuIds = datalist.Count > 0 ? string.Join(",", datalist) : "0";//在线的rtuid字符串

            var query = _AccessControlService.GetStationTreeOfState(onlinertuIds, user.IsAdmin, user.UserId, stationName == null ? "" : stationName);
            return query;
        }

        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult SearchStationList(string stationName)
        {
            var query = GetStationList(stationName);
            return Json(new
            {
                data = query
            });
        }
        #endregion
        #region 巡检反馈
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult XJ_Page()
        {
            ViewBag.datemin = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM") + "-01").AddMonths(-2).ToString("yyyy-MM-dd");
            ViewBag.datemax = DateTime.Now.ToString("yyyy-MM-dd");
           
            return View();
        }
        //巡检表格查询
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult QueryXJTable(int pagesize,int pageindex,string beginDate,string endDate,string order,string sort,int stationid)
        {
            string filter = "IsFeedback=1 ";
            int userID = int.Parse(User.FindFirstValue("UserID"));

            var user = _UserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
            if (stationid != 0)
            {
                filter += " and s.StationID=" + stationid + "";
            }
            if (!string.IsNullOrEmpty(beginDate))
            {
                filter += " and s.InspectionTime>='"+ beginDate + "'";
            }
            if (!string.IsNullOrEmpty(endDate))
            {
                var enddate1 = Convert.ToDateTime(endDate).AddDays(1);
                filter += " and s.InspectionTime<'"+ enddate1.ToString()+ "'";
            }
            if (user.IsAdmin != true)
            {
                filter += "  and su.UserID=" + userID + "";
            }
            string orderby = sort + " " + order;

            int Totalcount = 0;
            var datalist = _InspectionService.GetInspectionTable(user.IsAdmin,pageindex, pagesize, orderby, filter, ref Totalcount);
           return Json(new { total = Totalcount, rows = Newtonsoft.Json.JsonConvert.SerializeObject(datalist) });
        }
        //添加
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult AddXJPage(int id)
        {
            var model = new GdInspection();
            model.StationId = id;
            var stationmodel = _StationService.Query<SwsStation>(r => r.StationId == id).FirstOrDefault();
            if (stationmodel != null)
            {
                ViewBag.StationName = stationmodel.StationName;
            }
            else
            {
                ViewBag.StationName = "";
            }
            var stateList = _DataItemDetailService.Query<SysDataItemDetail>(r => r.FItemId == 18 && r.IsEnable == true);
            ViewBag.stateList = stateList;
            //卫生状态
            var healthSate= _DataItemDetailService.Query<SysDataItemDetail>(r => r.FItemId == 21 && r.IsEnable == true);
            ViewBag.healthSate = healthSate;
            //巡检人员树
            var datalist = _InspectionService.GetTeamUserTree();
            var userTree = "[]";
            if (datalist.Count() > 0)
            { 
               var teaminfo= datalist.Select(r => new treestring
               {
                   id = r.TeamID.ToString(),
                   pId = "0",
                   name = r.TeamName,
                   nocheck = true,
                   iddata= r.TeamID.ToString()
               });
                var userinfo= datalist.Select(r => new treestring
                {
                    id = r.UserID.ToString()+"u",
                    pId = r.TeamID.ToString(),
                    name = r.Account,
                    nocheck = false,
                    iddata = r.UserID.ToString()
                });
                userTree = Newtonsoft.Json.JsonConvert.SerializeObject(teaminfo.Concat(userinfo).Distinct(new treestringCompare()));
            }
            ViewBag.treenodes = new HtmlString(userTree);
            return View("SetXJPage", model);
        }
        //编辑
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult EditeXJPage(int id)//id为InspectionID
        {
            var model = _InspectionService.Query<GdInspection>(r => r.InspectionId == id).FirstOrDefault();
            var stationmodel = _StationService.Query<SwsStation>(r => r.StationId == model.StationId).FirstOrDefault();
            if (stationmodel != null)
            {
                ViewBag.StationName = stationmodel.StationName;
            }
            else
            {
                ViewBag.StationName = "";
            }
            var stateList = _DataItemDetailService.Query<SysDataItemDetail>(r => r.FItemId == 18 && r.IsEnable == true);
            ViewBag.stateList = stateList;
            //卫生状态
            var healthSate = _DataItemDetailService.Query<SysDataItemDetail>(r => r.FItemId == 21 && r.IsEnable == true);
            ViewBag.healthSate = healthSate;

            //巡检人员树
            var datalist = _InspectionService.GetTeamUserTree();
            var userTree = "[]";
            if (datalist.Count() > 0)
            {
                var teaminfo = datalist.Select(r => new treestring
                {
                    id = r.TeamID.ToString(),
                    pId = "0",
                    name = r.TeamName,
                    nocheck = true,
                    iddata = r.TeamID.ToString()
                });
                var userinfo = datalist.Select(r => new treestring
                {
                    id = r.UserID.ToString() + "u",
                    pId = r.TeamID.ToString(),
                    name = r.Account,
                    nocheck = false,
                    iddata = r.UserID.ToString()
                });
                userTree = Newtonsoft.Json.JsonConvert.SerializeObject(teaminfo.Concat(userinfo).Distinct(new treestringCompare()));
            }
            ViewBag.treenodes = new HtmlString(userTree);
            //巡检人员
            var inspectUserName = _UserService.Query<SysUser>(r => r.UserId == model.InspectionUser).FirstOrDefault()?.Account;
            ViewBag.inspectUserName = inspectUserName;
            return View("SetXJPage", model);
        }
        //巡检编号查询 根据泵房id
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult GetXJNumListByStationid(int id)
        {
            var inspects = _InspectionService.Query<GdInspection>(r => r.StationId == id && r.IsFeedback == false);
            var XJnum = "[]";
            if (inspects.Count() > 0)
            {
               var data = inspects.Select(r=>new {
                    id = r.Num,
                    pId =0,
                    name = r.Num,
                });
                XJnum = Newtonsoft.Json.JsonConvert.SerializeObject(data);
            }
            return Content(XJnum);
        }
        //提交表单
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult SetXJInfo(GdInspection p,string Edite_Num)
        {
            if (string.IsNullOrEmpty(p.Num))
            {
                p.Num = Edite_Num;
            }
            var oldmodel = _InspectionService.Query<GdInspection>(r => r.Num == p.Num).AsNoTracking().FirstOrDefault();
            p.InspectionId = oldmodel.InspectionId;
            p.CreateTime = oldmodel.CreateTime;
            p.InspectionTime = DateTime.Now;
            p.TaskDescription = oldmodel.TaskDescription;
            p.IsFeedback = true; 
            string result = "";
            try {
                _InspectionService.Update(p);
                result = "ok";
            }
            catch (Exception e) {
                result = "no";
            }

            return Content(result);
        }
        //删除
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult DeleteXJFeedBack(string id)
        {
            List<int> inspectIDList = new List<string>(id.Split(",")).ConvertAll(r=>int.Parse(r));
            
            if (_InspectionService.DeleteXJFeedBack(inspectIDList) > 0)
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }
        }
        public class treestring
        {
            public string id { get; set; }
            public string pId { get; set; }
            public string name { get; set; }
            public bool nocheck { get; set; }
            public string iddata { get; set; }
            
        }
        public class treestringCompare : IEqualityComparer<treestring>
        {
            public bool Equals(treestring x, treestring y)
            {
                if (x == null)
                    return y == null;
                return x.id == y.id;
            }

            public int GetHashCode(treestring obj)
            {
                if (obj == null)
                    return 0;
                return obj.id.GetHashCode();
            }
        }
        #endregion
        #region 维修反馈
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult WX_Page()
        {
            ViewBag.datemin = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM") + "-01").AddMonths(-2).ToString("yyyy-MM-dd");
            ViewBag.datemax = DateTime.Now.ToString("yyyy-MM-dd");
            return View();
        }
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult QueryRepareTable(int pagesize, int pageindex, string beginDate, string endDate, string order, string sort, int stationid)
        {
            string filter = "IsFeedback=1 ";
            int userID = int.Parse(User.FindFirstValue("UserID"));

            var user = _UserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
            if (stationid != 0)
            {
                filter += " and s.StationID=" + stationid + "";
            }
            if (!string.IsNullOrEmpty(beginDate))
            {
                filter += " and s.ReportTime>='" + beginDate + "'";
            }
            if (!string.IsNullOrEmpty(endDate))
            {
                var enddate1 = Convert.ToDateTime(endDate).AddDays(1);
                filter += " and s.ReportTime<'" + enddate1.ToString() + "'";
            }
            if (user.IsAdmin != true)
            {
                filter += "  and su.UserID=" + userID + "";
            }
            string orderby = sort + " " + order;

            int Totalcount = 0;
            var datalist = _InspectionService.GetRepairTable(user.IsAdmin, pageindex, pagesize, orderby, filter, ref Totalcount);
            return Json(new { total = Totalcount, rows = Newtonsoft.Json.JsonConvert.SerializeObject(datalist) });
        }
        #region 页面操作
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult AddWXPage(int id)
        {
            var model = new GdRepair();
            model.StationId = id;
            var stationmodel = _StationService.Query<SwsStation>(r => r.StationId == id).FirstOrDefault();
            if (stationmodel != null)
            {
                ViewBag.StationName = stationmodel.StationName;
            }
            else
            {
                ViewBag.StationName = "";
            }
            var handleState = _DataItemDetailService.Query<SysDataItemDetail>(r => r.FItemId == 19 && r.IsEnable == true);
            ViewBag.handleState = handleState;
            //巡检人员树
            var datalist = _InspectionService.GetTeamUserTree();
            var userTree = "[]";
            if (datalist.Count() > 0)
            {
                var teaminfo = datalist.Select(r => new treestring
                {
                    id = r.TeamID.ToString(),
                    pId = "0",
                    name = r.TeamName,
                    nocheck = true,
                    iddata = r.TeamID.ToString()
                });
                var userinfo = datalist.Select(r => new treestring
                {
                    id = r.UserID.ToString() + "u",
                    pId = r.TeamID.ToString(),
                    name = r.Account,
                    nocheck = false,
                    iddata = r.UserID.ToString()
                });
                userTree = Newtonsoft.Json.JsonConvert.SerializeObject(teaminfo.Concat(userinfo).Distinct(new treestringCompare()));
            }
            ViewBag.treenodes = new HtmlString(userTree);
            return View("SetWXPage", model);
        }
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult GetWXNumListByStationid(int id)
        {
            var inspects = _RepairService.Query<GdRepair>(r => r.StationId == id && r.IsFeedback == false);
            var XJnum = "[]";
            if (inspects.Count() > 0)
            {
                var data = inspects.Select(r => new {
                    id = r.Num,
                    pId = 0,
                    name = r.Num,
                });
                XJnum = Newtonsoft.Json.JsonConvert.SerializeObject(data);
            }
            return Content(XJnum);
        }
        //根据泵房查询泵房下的设备列表
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult GetDeviceList_ByStationid(int id)
        {
            var stationModel = _StationService.Query<SwsStation>(r => r.StationId == id).FirstOrDefault();
            string tablename = "";
            if (stationModel.InType == 1)
            {
                tablename = "Sws_DeviceInfo01";
            }
            else if (stationModel.InType == 2)
            {
                tablename = "Sws_DeviceInfo02";
            }
            else
            {
                tablename = "Sws_DeviceInfo03";
            }
            var datalist = _InspectionService.GetDeviceList_ByStationID(tablename,id,0);
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(datalist));
        }
        //编辑
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult EditeWXPage(int id)//id为repairid
        {
            var model = _RepairService.Query<GdRepair>(r => r.RepairId == id).FirstOrDefault();
            var stationmodel = _StationService.Query<SwsStation>(r => r.StationId == model.StationId).FirstOrDefault();
            if (stationmodel != null)
            {
                ViewBag.StationName = stationmodel.StationName;
                
            }
            else
            {
                ViewBag.StationName = "";
                ViewBag.deviceName = "";
            }
            var handleState = _DataItemDetailService.Query<SysDataItemDetail>(r => r.FItemId == 19 && r.IsEnable == true).ToList();
            ViewBag.handleState = handleState;
            //巡检人员树
            var datalist = _InspectionService.GetTeamUserTree().ToList();
            var userTree = "[]";
            if (datalist.Count() > 0)
            {
                var teaminfo = datalist.Select(r => new treestring
                {
                    id = r.TeamID.ToString(),
                    pId = "0",
                    name = r.TeamName,
                    nocheck = true,
                    iddata = r.TeamID.ToString()
                });
                var userinfo = datalist.Select(r => new treestring
                {
                    id = r.UserID.ToString() + "u",
                    pId = r.TeamID.ToString(),
                    name = r.Account,
                    nocheck = false,
                    iddata = r.UserID.ToString()
                });
                userTree = Newtonsoft.Json.JsonConvert.SerializeObject(teaminfo.Concat(userinfo).Distinct(new treestringCompare()));
            }
            ViewBag.treenodes = new HtmlString(userTree);
            //巡检人员
            var RepairUserName = _UserService.Query<SysUser>(r => r.UserId == model.RepairUser).FirstOrDefault()?.Account;
            ViewBag.RepairUserName = RepairUserName;
            if (stationmodel != null)
            {
                if (model.DeviceId != null && model.DeviceId != 0)
                {
                    string tablename = "";
                    if (stationmodel.InType == 1)
                    {
                        tablename = "Sws_DeviceInfo01";
                    }
                    else if (stationmodel.InType == 2)
                    {
                        tablename = "Sws_DeviceInfo02";
                    }
                    else
                    {
                        tablename = "Sws_DeviceInfo03";
                    }
                    var devicemodel = _InspectionService.GetDeviceList_ByStationID(tablename, id, (long)model.DeviceId).FirstOrDefault();
                    ViewBag.deviceName = devicemodel?.deviceName;
                }
                else
                {
                    ViewBag.deviceName = "";
                }
            }

           return View("SetWXPage", model);
        }
        //表单提交
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult SetWXInfo(GdRepair p, string Edite_Num,string[] url,string[] size,string[] suffix,string[] names)
        {
            if (string.IsNullOrEmpty(p.Num))
            {
                p.Num = Edite_Num;
            }
            var oldmodel = _RepairService.Query<GdRepair>(r => r.Num == p.Num).AsNoTracking().FirstOrDefault();
            p.CreateTime = oldmodel.CreateTime;
            p.FaultContent = oldmodel.FaultContent;
            p.FaultDescription = oldmodel.FaultDescription;
            p.RepairId = oldmodel.RepairId;
            p.IsFeedback = true;
            p.ReportTime = DateTime.Now;
            #region 附件
            List<GdResource> resourceList = new List<GdResource>() { };
            if (url.Length > 0)
            {
                long id = 0;
                GdResource resouce = _ResourceService.Query<GdResource>(r => true).OrderByDescending(r => r.Id).FirstOrDefault();
                if (resouce != null)
                {
                    id = resouce.Id;
                }
                for (var i = 0; i < url.Length; i++)
                {
                    GdResource g = new GdResource();
                    g.Id = id + i + 1;
                    g.Pid = p.RepairId;
                    g.Path = url[i];
                    g.ResourceType = 3;
                    g.Suffix = suffix[i];
                    g.FileName = names[i];
                    g.Type = (byte)GetFileType(suffix[i]);
                    resourceList.Add(g);
                }
            }
            if (_InspectionService.UpdateRepairInfo(p, resourceList) > 0)
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }
            #endregion
        }
        //删除维修
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult DeleteWXFeedBack(string id)
        {
            List<long> repaireIDList = new List<string>(id.Split(",")).ConvertAll(r => long.Parse(r));
            string dicPath = _webHostEnvironment.WebRootPath;
            if (_InspectionService.DeleteWXFeedBack(repaireIDList, dicPath) > 0)
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
        #region 保养反馈
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult BY_Page()
        {
            ViewBag.datemin = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM") + "-01").AddMonths(-2).ToString("yyyy-MM-dd");
            ViewBag.datemax = DateTime.Now.ToString("yyyy-MM-dd");
            return View();
        }
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult QueryBYTable(int pagesize, int pageindex, string beginDate, string endDate, string order, string sort, int stationid)
        {
            string filter = "IsFeedback=1 ";
            int userID = int.Parse(User.FindFirstValue("UserID"));

            var user = _UserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
            if (stationid != 0)
            {
                filter += " and s.StationID=" + stationid + "";
            }
            if (!string.IsNullOrEmpty(beginDate))
            {
                filter += " and s.MaintainTime>='" + beginDate + "'";
            }
            if (!string.IsNullOrEmpty(endDate))
            {
                var enddate1 = Convert.ToDateTime(endDate).AddDays(1);
                filter += " and s.MaintainTime<'" + enddate1.ToString() + "'";
            }
            if (user.IsAdmin != true)
            {
                filter += "  and su.UserID=" + userID + "";
            }
            string orderby = sort + " " + order;

            int Totalcount = 0;
            var datalist = _InspectionService.GetMainTainTable(user.IsAdmin,pageindex, pagesize, orderby, filter, ref Totalcount);
            return Json(new { total = Totalcount, rows = Newtonsoft.Json.JsonConvert.SerializeObject(datalist) });
        }
        #region 页面操作
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult AddBYPage(int id)
        {
            var model = new GdMaintain();
            model.StationId = id;
            var stationmodel = _StationService.Query<SwsStation>(r => r.StationId == id).FirstOrDefault();
            if (stationmodel != null)
            {
                ViewBag.StationName = stationmodel.StationName;
            }
            else
            {
                ViewBag.StationName = "";
            }
            var handleState = _DataItemDetailService.Query<SysDataItemDetail>(r => r.FItemId == 19 && r.IsEnable == true);
            ViewBag.handleState = handleState;
            //保养项目
            var projects = _DataItemDetailService.Query<SysDataItemDetail>(r => r.FItemId == 20 && r.IsEnable == true);
            ViewBag.projects = projects;
            //巡检人员树
            var datalist = _InspectionService.GetTeamUserTree();
            var userTree = "[]";
            if (datalist.Count() > 0)
            {
                var teaminfo = datalist.Select(r => new treestring
                {
                    id = r.TeamID.ToString(),
                    pId = "0",
                    name = r.TeamName,
                    nocheck = true,
                    iddata = r.TeamID.ToString()
                });
                var userinfo = datalist.Select(r => new treestring
                {
                    id = r.UserID.ToString() + "u",
                    pId = r.TeamID.ToString(),
                    name = r.Account,
                    nocheck = false,
                    iddata = r.UserID.ToString()
                });
                userTree = Newtonsoft.Json.JsonConvert.SerializeObject(teaminfo.Concat(userinfo).Distinct(new treestringCompare()));
            }
            ViewBag.treenodes = new HtmlString(userTree);
            return View("SetBYPage", model);
        }
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult GetBYNumListByStationid(int id)
        {
            var inspects = _MaintainService.Query<GdMaintain>(r => r.StationId == id && r.IsFeedback == false);
            var XJnum = "[]";
            if (inspects.Count() > 0)
            {
                var data = inspects.Select(r => new {
                    id = r.Num,
                    pId = 0,
                    name = r.Num,
                });
                XJnum = Newtonsoft.Json.JsonConvert.SerializeObject(data);
            }
            return Content(XJnum);
        }
        //编辑
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult EditeBYPage(int id)//MaintainId
        {
            var model = _MaintainService.Query<GdMaintain>(r => r.MaintainId == id).FirstOrDefault();
            var stationmodel = _StationService.Query<SwsStation>(r => r.StationId == model.StationId).FirstOrDefault();
            if (stationmodel != null)
            {
                ViewBag.StationName = stationmodel.StationName;

            }
            else
            {
                ViewBag.StationName = "";
                ViewBag.deviceName = "";
            }
            var handleState = _DataItemDetailService.Query<SysDataItemDetail>(r => r.FItemId == 19 && r.IsEnable == true).ToList();
            ViewBag.handleState = handleState;
            //保养项目
            var projects = _DataItemDetailService.Query<SysDataItemDetail>(r => r.FItemId == 20 && r.IsEnable == true);
            ViewBag.projects = projects;
            //巡检人员树
            var datalist = _InspectionService.GetTeamUserTree().ToList();
            var userTree = "[]";
            if (datalist.Count() > 0)
            {
                var teaminfo = datalist.Select(r => new treestring
                {
                    id = r.TeamID.ToString(),
                    pId = "0",
                    name = r.TeamName,
                    nocheck = true,
                    iddata = r.TeamID.ToString()
                });
                var userinfo = datalist.Select(r => new treestring
                {
                    id = r.UserID.ToString() + "u",
                    pId = r.TeamID.ToString(),
                    name = r.Account,
                    nocheck = false,
                    iddata = r.UserID.ToString()
                });
                userTree = Newtonsoft.Json.JsonConvert.SerializeObject(teaminfo.Concat(userinfo).Distinct(new treestringCompare()));
            }
            ViewBag.treenodes = new HtmlString(userTree);
            //巡检人员
            var RepairUserName = _UserService.Query<SysUser>(r => r.UserId == model.MaintainUser).FirstOrDefault()?.Account;
            ViewBag.RepairUserName = RepairUserName;
          

            return View("SetBYPage", model);
        }
        //表单提交
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult SetBYInfo(GdMaintain p, string Edite_Num, string[] url, string[] size, string[] suffix, string[] names)
        {
            if (string.IsNullOrEmpty(p.Num))
            {
                p.Num = Edite_Num;
            }
            var oldmodel = _MaintainService.Query<GdMaintain>(r => r.Num == p.Num).AsNoTracking().FirstOrDefault();
            p.CreateTime = oldmodel.CreateTime;
            p.TaskDescription = oldmodel.TaskDescription;
            p.MaintainId = oldmodel.MaintainId;
            p.IsFeedback = true;
            p.MaintainTime = DateTime.Now;
            #region 附件
            List<GdResource> resourceList = new List<GdResource>() { };
            if (url.Length > 0)
            {
                long id = 0;
                GdResource resouce = _ResourceService.Query<GdResource>(r => true).OrderByDescending(r => r.Id).FirstOrDefault();
                if (resouce != null)
                {
                    id = resouce.Id;
                }
                for (var i = 0; i < url.Length; i++)
                {
                    GdResource g = new GdResource();
                    g.Id = id + i + 1;
                    g.Pid = p.MaintainId;
                    g.Path = url[i];
                    g.ResourceType = 4;
                    g.Suffix = suffix[i];
                    g.FileName = names[i];
                    g.Type = (byte)GetFileType(suffix[i]);
                    resourceList.Add(g);
                }
            }
            if (_InspectionService.UpdateMainTainInfo(p, resourceList) > 0)
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }
            #endregion
        }
        //删除保养
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult DeleteBYFeedBack(string id)
        {
            List<long> repaireIDList = new List<string>(id.Split(",")).ConvertAll(r => long.Parse(r));
            string dicPath = _webHostEnvironment.WebRootPath;
            if (_InspectionService.DeleteBYFeedBack(repaireIDList, dicPath) > 0)
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

        #region 泵房选择
        [TypeFilter(typeof(IgonreActionFilter))]
        public ActionResult SelectStationInfo()
        {
            return View();
        }

        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult LoadStationList(int pageSize, int pageIndex, string sort, string sortOrder, string searchText, int stationid)
        {
            int Totalcount = 0;
            int userID = int.Parse(User.FindFirstValue("UserID"));

            var user = _UserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
            string filter = " 1=1";
            if (!string.IsNullOrEmpty(searchText))
            {
                filter += " and (StationNum like '%" + searchText + "%' or StationName like '%" + searchText + "%')";
            }
            if (user.IsAdmin != true)
            {
                filter += " and su.UserID="+ userID + "";
            }
            string orderby = sort + " " + sortOrder;
            var f_stationTypeID = (int)Model.CustomizedClass.Enum.泵房类型;
            var datalist = _InspectionService.GetGD_SelectStationTable(user.IsAdmin,stationid, f_stationTypeID, pageIndex, pageSize, orderby, filter, ref Totalcount);
            return Json(new { total = Totalcount, rows = Newtonsoft.Json.JsonConvert.SerializeObject(datalist) });
        }
        #endregion

        #region 附件处理
        #region 上传文件处理

        [TypeFilter(typeof(IgonreActionFilter))]

        public IActionResult UpLoad(IFormFile file,string positionName)
        {
            string filePathName = string.Empty;
            
            string fileExt = file.FileName.Substring(file.FileName.IndexOf('.')); //文件扩展名，不含“.”
            int index = Convert.ToInt32(base.HttpContext.Request.Form["chunk"]);//当前分块序号
            

            ////上传文件是否为空的判断
            if (file.Length == 0)
            {
                return Json(new { jsonrpc = 2.0, error = new { code = 102, message = "保存失败" }, id = "id" });
            }

            //路径处理

            string dicPath = _webHostEnvironment.WebRootPath;
            string localPath = Path.Combine(dicPath, "UploadFile\\" + positionName +  "\\" + file.FileName);
            if (!System.IO.Directory.Exists(localPath))
            {
                System.IO.Directory.CreateDirectory(localPath);
            }
            //var filePath = localPath + "/" + index + fileExt;
            var pathfile = Path.Combine(localPath, index + fileExt);
            using (FileStream fs = System.IO.File.Create(pathfile))
            {
                file.CopyTo(fs);
                fs.Flush();
            }

            return Content("");

        }

        
        [HttpPost]
        [TypeFilter(typeof(IgonreActionFilter))]
        public ActionResult MergeFile( string fileName, string positionName)
        {
            string dicPath = _webHostEnvironment.WebRootPath;
            var provider = new PhysicalFileProvider(dicPath);
            string localPath = "wwwroot\\UploadFile\\" + positionName + "\\" + fileName;
            //var files1 = provider.(localPath);
            var files = Directory.GetFiles(localPath);//获得下面的所有文件
            string paths = Guid.NewGuid().ToString("N");
            string realPath = Path.Combine(dicPath, "UploadFile\\" + positionName + "\\" + paths);
            var finalPath = Path.Combine(realPath, fileName);
            if (!System.IO.Directory.Exists(realPath))
            {
                System.IO.Directory.CreateDirectory(realPath);
            }

            var fs = new FileStream(finalPath, FileMode.Create);
            foreach (var part in files.OrderBy(x => x.Length).ThenBy(x => x))//排一下序，保证从0-N Write
            {
                var bytes = System.IO.File.ReadAllBytes(part);
                fs.Write(bytes, 0, bytes.Length);
                bytes = null;
                System.IO.File.Delete(part);//删除分块
            }
            fs.Close();

            System.IO.Directory.Delete(localPath);//删除文件夹           
            return Json(new
            {
                path = paths
            });
        }

        /// <summary>
        /// 获取各功能模块下 上传的文件信息（包括问文件名称、大小、地址、类型等）
        /// </summary>
        /// <param name="id">模块ID</param>
        /// <param name="classifyName">类型名称（枚举获取类型值时使用，例如公告管理模块，类型值为“通知公告”）</param>
        /// <returns></returns>
        [TypeFilter(typeof(IgonreActionFilter))]
        public ActionResult GetFileInfo(int id, string classifyName)
        {
            ////获取分类
            var classify =int.Parse(classifyName);
            string dicPath = _webHostEnvironment.WebRootPath;
            var file = _ResourceService.Query<GdResource>(r => r.Pid == id && r.ResourceType == classify).ToList();
            List<GdResource> att = new List<GdResource>();
            foreach (var item in file)
            {
                string localPath = Path.Combine(dicPath, item.Path.Substring(1));
                
                if (System.IO.File.Exists(localPath))
                {
                    att.Add(item);
                }
            }
            return Json(new
            {
                data = att
            });
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="fileId">文件ID</param>
        /// <returns></returns>
        [HttpPost]
        [TypeFilter(typeof(IgonreActionFilter))]
        public ActionResult DeleteFile(string fileId)
        {
            string dicPath = _webHostEnvironment.WebRootPath;
            long id = Convert.ToInt64(fileId);
            GdResource fileInfo = _ResourceService.Query<GdResource>(r => r.Id == id).FirstOrDefault();
            if (fileInfo != null)
            {
                _ResourceService.Delete(fileInfo);
                var indexlast= fileInfo.Path.LastIndexOf("\\");
                string localPath = Path.Combine(dicPath, fileInfo.Path.Substring(1, indexlast-1));
                //删除文件
                if (Directory.Exists(localPath))
                {
                    Directory.Delete(localPath, true);
                }
                return Content("ok");
            }
            else
            {
                return Content("no");
            }

        }
        //删除未添加到数据库的附件（上传后立即删除）
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult DeleteFileLocal(string path)
        {
            string dicPath = _webHostEnvironment.WebRootPath;
            try
            {
                
                string localPath = Path.Combine(dicPath, path.Substring(1));
                //删除文件
                if (Directory.Exists(localPath))
                {
                    Directory.Delete(localPath, true);
                }
                return Content("ok");
            }
            catch (Exception e)
            {
                return Content("no");
            }
        }

        //删除未添加到数据库的附件(取消表单)
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult DeleteFileLocal_cancle(string urlstring)
        {
            string dicPath = _webHostEnvironment.WebRootPath;
            string[] urllist = urlstring.Split("|");
            try
            {
                foreach (var item in urllist)
                {
                    var indexlast = item.LastIndexOf("\\");
                    string localPath = Path.Combine(dicPath, item.Substring(1, indexlast - 1));
                    //删除文件
                    if (Directory.Exists(localPath))
                    {
                        Directory.Delete(localPath, true);
                    }
                }
                return Content("ok");
            }
            catch (Exception e)
            {
                return Content("no");
            }
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="fileId">文件ID</param>
        /// <returns></returns>
        [TypeFilter(typeof(IgonreActionFilter))]
        public FileStreamResult DownloadFile(long fileId)
        {
            string dicPath = _webHostEnvironment.WebRootPath;
            var file = _ResourceService.Query<GdResource>(r => r.Id == fileId).FirstOrDefault();
            string realPath = Path.Combine(dicPath, file.Path.Substring(1));
            FileStream fs = new FileStream(realPath, FileMode.Open);
            return File(fs, "text/plain", file.FileName);
           
        }
        #endregion
        #region 数据处理方法
        //分解ID
        public List<int> GetIDs(string ids)
        {
            string[] idList = ids.Split(',');
            List<int> nIds = new List<int>();
            foreach (var item in idList)
            {
                nIds.Add(int.Parse(item));
            }
            return nIds;
        }
        //上传文件大小处理
        public static double FileSizeData(int size, ref string fileunit)
        {
            double fileSize = 0.0;
            if (size > 1000)
            {
                fileSize = Math.Round((double)size / 1024, 2);
                fileunit = "KB";
                if (fileSize > 1000)
                {
                    fileSize = Math.Round((double)fileSize / 1024, 2);
                    fileunit = "MB";
                    if (fileSize > 1000)
                    {
                        fileSize = Math.Round((double)fileSize / 1024, 2);
                        fileunit = "GB";
                    }
                }
            }
            else
            {
                fileSize = Math.Round((double)size, 2);
                fileunit = "B";
            }
            return fileSize;
        }

        //图片类型处理
        public static int GetFileType(string suffix)//附件只能上传图片，视频，音频  三种
        {
            var soundList = new List<string>() { ".mp3",".wma",".rm",".wav",".mid", ".ape",".flac" };//音频后缀
            if (suffix == ".jpg" || suffix == ".png")
            {
                return 1;
            }
            else
            {
                if (suffix == ".rmvb" || suffix == ".mp4" || suffix == ".flv" || suffix == ".wmv" || suffix == ".mkv" || suffix == ".mov")
                {
                    return 3;
                }
                else if (soundList.Contains(suffix))
                {
                    return 2;
                }
                else
                {
                    return 0;
                }
            }
        }

        #endregion
        #endregion
    }
}