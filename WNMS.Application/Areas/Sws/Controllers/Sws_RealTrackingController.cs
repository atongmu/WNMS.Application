using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WNMS.Application.Utility.Filters;
using WNMS.Application.Utility.JsonHelper;
using WNMS.IService;
using WNMS.Model.CustomizedClass;
using WNMS.Model.DataModels;

namespace WNMS.Application.Areas.Sws.Controllers
{
    [Area("Sws")]
    public class Sws_RealTrackingController : Controller
    {
        private ISws_StationService _StationService = null;
        private ISysUserService _userService = null;
        private ISws_ConsumpSettingService _ConsumpSettingService = null;
        private ISws_GPSModuleService _gpsmoduleService = null;
        private readonly IWebHostEnvironment webHostEnvironment;
        public Sws_RealTrackingController(ISws_StationService sws_StationService, ISysUserService sysUserService, ISws_ConsumpSettingService sws_ConsumpSettingService, ISws_GPSModuleService gpsmoduleService, IWebHostEnvironment _webHostEnvironment)
        {
            _StationService = sws_StationService;
            _userService = sysUserService;
            _ConsumpSettingService = sws_ConsumpSettingService;
            _gpsmoduleService = gpsmoduleService;
            webHostEnvironment = _webHostEnvironment;
        }
        public IActionResult Index()
        {
            ViewBag.treenodes = new HtmlString(GetUserTree(""));

            //获取地图级别
            var path = webHostEnvironment.ContentRootPath;
            JsonFileHelper jsonFileHelper = new JsonFileHelper(path + "/Utility/ProjectJson/project.json");
            SystemInfo st = jsonFileHelper.Read<SystemInfo>("SystemInfo");
            string zoom = st.MapLevel == "" ? "11" : st.MapLevel;
            ViewBag.Zoom = Convert.ToInt32(zoom);
            ViewBag.lng = st.Lng;
            ViewBag.lat = st.Lat;

            return View();
        }

        [HttpPost]
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult GetGPSMarkerDatas(string mid)
        { 
            GpsData gs = new GpsData();
            gs = _StationService.GetGPSMarkerData(mid).FirstOrDefault();
            if (gs != null)
            {
                HttpContext.Session.SetString("UpdateTime",gs.UpdateTime.ToString());
                //HttpContext.Session.SetString("UpdateTime", "2021-07-15 17:09   :00");
            }
            return Json(new
            {
                gs = gs
            });
        }

        [HttpPost]
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult GetGPSDatas(string mid)
        {
            string beginDate = HttpContext.Session.GetString("UpdateTime");
            if (string.IsNullOrEmpty(beginDate))
            {
                beginDate = DateTime.Now.AddSeconds(-60).ToString();
            }
            //string endDate = DateTime.Now.ToString();
            string endDate = Convert.ToDateTime(beginDate).AddSeconds(60).ToString();

            List<GpsData> gs = new List<GpsData>();
            gs = _StationService.GetGPSData(mid, beginDate, endDate).ToList();
            if (gs.Count > 0)
            {
                HttpContext.Session.SetString("UpdateTime", gs.First().UpdateTime.ToString());
            }

            var us = _gpsmoduleService.GetGpsUser(mid).FirstOrDefault();
            string state = "";
            if (us != null)
            {
                if (us.RemandTime != null && us.RemandTime < DateTime.Now)
                {
                    state = "暂未借出";
                }
                else
                {
                    state = "使用中";
                }
            }
            else
            {
                state = "暂未使用";
            }
            return Json(new
            {
                gs = gs,
                state=state,
                username=us.UserName
            });
        }

        #region 树查询
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult SearchUser(string gpsName)
        {
            return Content(GetUserTree(gpsName));
        }
        private string GetUserTree(string gpsName)
        {
            var data = _gpsmoduleService.Query<SwsGpsmodule>(u => true).ToList();
            if (!string.IsNullOrEmpty(gpsName))
            {
                data = data.Where(r => r.GpsName.Contains(gpsName)).ToList();
            }
            if (data.Count() > 0)
            {
                var list = data.Select(r => new
                {
                    id = r.Gpsid,
                    pId = 0,
                    name = "<i class='fa fa-sitemap'></i>" + r.GpsName,
                    icon = "/images/stationTree.png",
                    serialNumber = r.GpSnumber.ToString()
                });
                return Newtonsoft.Json.JsonConvert.SerializeObject(list);
            }
            else
            {
                return "[]";
            }

            //var list = _ConsumpSettingService.PersonLocusTree(userName);
            //var araeainfo = list.Select(r => new treestring
            //{
            //    id = "a" + r.ID.ToString(),
            //    pId = "a" + r.PID.ToString(),
            //    name = "<i class='fa fa-sitemap'></i>" + r.AreaName,
            //    isUser = false
            //});
            //var userlist = list.Where(r => r.type == 2).Select(r => new treestring
            //{
            //    id = r.UserID.ToString(),
            //    pId = "a" + r.ID.ToString(),
            //    name = "<i class='iconfont  icon-yonghu'></i>" + r.NickName.ToString(),
            //    isUser = true,
            //    serialNumber = r.SerialNumber.ToString()
            //});
            //var data = araeainfo.Concat(userlist).Distinct(new treestringCompare());
            //if (data.Count() > 0)
            //{
            //    return Newtonsoft.Json.JsonConvert.SerializeObject(data);
            //}
            //else
            //{
            //    return "[]";
            //}
        }

        public class treestring
        {
            public string id { get; set; }
            public string pId { get; set; }
            public string name { get; set; }

            public string icon { get; set; }
            public bool isUser { get; set; }
            public string serialNumber { get; set; }
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
    }
}
