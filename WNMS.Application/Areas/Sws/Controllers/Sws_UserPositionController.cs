using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WNMS.IService;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Html;
using WNMS.Application.Utility.Filters;
using WNMS.Application.Utility.JsonHelper;
using WNMS.Model.CustomizedClass;
using Microsoft.AspNetCore.Hosting;
using WNMS.Model.DataModels;

namespace WNMS.Application.Areas.Sws.Controllers
{
    [Area("Sws")]
    public class Sws_UserPositionController : Controller
    {
        private ISws_UserPositionService _UserPositionService = null;

        private ISysUserService _UserService = null;
        private readonly IWebHostEnvironment webHostEnvironment;
        private ISws_GPSModuleService _gpsmoduleService = null;
        public Sws_UserPositionController(ISysUserService sysUserService, ISws_UserPositionService sws_UserPositionService, IWebHostEnvironment _webHostEnvironment, ISws_GPSModuleService gpsmoduleService)
        {
            _UserPositionService = sws_UserPositionService;
            _UserService = sysUserService;
            webHostEnvironment = _webHostEnvironment;
            _gpsmoduleService = gpsmoduleService;
        }
        public IActionResult Index()
        {          //获取地图级别
            var path = webHostEnvironment.ContentRootPath;
            JsonFileHelper jsonFileHelper = new JsonFileHelper(path + "/Utility/ProjectJson/project.json");
            SystemInfo st = jsonFileHelper.Read<SystemInfo>("SystemInfo");
            string zoom = st.MapLevel == "" ? "11" : st.MapLevel;
            ViewBag.Zoom = Convert.ToInt32(zoom);
            ViewBag.lng = st.Lng;
            ViewBag.lat = st.Lat;
            ViewBag.TreeNodes = new HtmlString(GetPersonTree(""));
            return View();
        }
        /// <summary>
        /// 获取用户定位数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult LoadUserPositionInfo()
        {
            //string filter = " and 1=1";
            //var info = _UserService.LoadUserPositionInfo(filter);
            List<UserPositionPhone> info = _gpsmoduleService.GetPhoneGpsDevices(0);
            //foreach (var item in info)
            //{
            //    var us = _gpsmoduleService.GetUserName(item.SerialNumber, item.UpdateTime);
            //    if (string.IsNullOrEmpty(us))
            //    {
            //        item.Name = "未使用";
            //    }
            //    else { item.Name = us; }
            //}
            return Json(new
            {
                info
            });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mid"></param>
        /// <returns></returns>
        [HttpPost]
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult GetUserPositionInfoById(long mid)
        {
            UserPositionPhone info = _gpsmoduleService.GetPhoneGpsDevices(mid).FirstOrDefault();
            //if (info != null)
            //{
            //    var us = _gpsmoduleService.GetUserNamePhone(info.UserID, info.UpdateTime);
            //    if (string.IsNullOrEmpty(us))
            //    {
            //        info.NickName = "未使用";
            //    }
            //    else { info.NickName = us; }

            //}
            //else
            //{
                
            //}
            
            return Json(new
            {
                info
            });
        }


        private string GetPersonTree(string UserName)
        {
            //var data = _UserService.Query<SysUser>(u => true).ToList();
            var data = _UserService.GetCusUser(UserName).ToList();
            if (data.Count() > 0)
            {
                var list = data.Select(r => new
                {
                    id = r.UserID.ToString(),
                    pId = 0,
                    name = "<i class='fa fa-sitemap'></i>" + r.NickName,
                    icon = "/images/stationTree.png",
                    serialNumber = r.UserID.ToString()
                });
                return Newtonsoft.Json.JsonConvert.SerializeObject(list);
            }
            else
            {
                return "[]";
            }
        }
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult SearchPersonTree(string searchtxt)
        {
            return Content(GetPersonTree(searchtxt));
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
    }
}