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
    public class Sws_RealTrackingUserController : Controller
    {
        private ISws_StationService _StationService = null;
        private ISysUserService _userService = null;
        private ISws_ConsumpSettingService _ConsumpSettingService = null;
        private ISws_GPSModuleService _gpsmoduleService = null;
        private readonly IWebHostEnvironment webHostEnvironment;
        public Sws_RealTrackingUserController(ISws_StationService sws_StationService, ISysUserService sysUserService, ISws_ConsumpSettingService sws_ConsumpSettingService, ISws_GPSModuleService gpsmoduleService, IWebHostEnvironment _webHostEnvironment)
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
        public IActionResult GetGPSMarkerDatas(string userid)
        {
            GpsData gs = new GpsData();
            gs = _StationService.GetGPSMarkerData(userid).FirstOrDefault();
            if (gs != null)
            {
                HttpContext.Session.SetString("UpdateTime", gs.UpdateTime.ToString());
                //HttpContext.Session.SetString("UpdateTime", "2022-08-11 15:59:59");
            }
            return Json(new
            {
                gs = gs
            });
        }

        [HttpPost]
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult GetGPSDatas(string userid,int num)
        {
             string beginDate = HttpContext.Session.GetString("UpdateTime");
            if (string.IsNullOrEmpty(beginDate))
            {
                beginDate = DateTime.Now.AddSeconds(-60).ToString();
            }
            //string endDate = DateTime.Now.ToString();
            string endDate = Convert.ToDateTime(beginDate).AddSeconds(10*num).ToString();

            List<GpsData> gs = new List<GpsData>();
            gs = _StationService.GetGPSData(userid, beginDate, endDate).ToList();
            if (gs.Count > 1)
            {
                HttpContext.Session.SetString("UpdateTime", gs.First().UpdateTime.ToString());
            }
            else
            {
                if (num > 59)
                {
                    HttpContext.Session.SetString("UpdateTime", Convert.ToDateTime(beginDate).AddSeconds(10 * num).ToString());
                }
            }
            return Json(new
            {
                gs = gs
            });
        }

        #region 树查询
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult SearchUser(string userName)
        {
            return Content(GetUserTree(userName));
        }
        private string GetUserTree(string userName)
        {
            var data = _userService.GetRoleUserData().ToList();
            if (!string.IsNullOrEmpty(userName))
            {
                data = data.Where(r => r.NickName.Contains(userName)).ToList();
            }
            if (data.Count() > 0)
            {
                var list = data.Select(r => new
                {
                    id = r.UserId,
                    pId = 0,
                    name = "<i class='fa fa-sitemap'></i>" + r.NickName,
                    icon = "/images/stationTree.png"
                });
                return Newtonsoft.Json.JsonConvert.SerializeObject(list);
            }
            else
            {
                return "[]";
            }
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
