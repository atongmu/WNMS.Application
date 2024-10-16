using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WNMS.Application.Utility.Filters;
using WNMS.Model.DataModels;
using WNMS.IService;
using WNMS.Service;
using Microsoft.AspNetCore.Hosting;
using WNMS.Application.Utility.JsonHelper;
using WNMS.Model.CustomizedClass;
using WNMS.Utility;

namespace WNMS.Application.Areas.Com.Controllers
{
    [Area("Com")]
    public class AccessoryMapController : Controller
    {
        private ISysUserService _ISysUserService = null;
        private ISws_AccessoriesEquService _ISws_AccessoriesEquService = null;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public AccessoryMapController(ISysUserService sysUserService,
            ISws_AccessoriesEquService _AccessoriesEquService,
            IWebHostEnvironment webHostEnvironment) {
            _ISysUserService = sysUserService;
            _ISws_AccessoriesEquService = _AccessoriesEquService;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            var data = GetStationList("");
            ViewBag.stationState = data;
            var path = _webHostEnvironment.ContentRootPath;
            JsonFileHelper jsonFileHelper = new JsonFileHelper(path + "/Utility/ProjectJson/project.json");
            SystemInfo st = jsonFileHelper.Read<SystemInfo>("SystemInfo");
            ViewBag.lng = st.Lng;
            ViewBag.lat = st.Lat;
            ViewBag.zoom = st.MapLevel;
            ViewBag.AreaName = st.AreaName;
            ViewBag.ArcgisUrl = StaticConstraint.ArcgisUrl;
            return View("Index_let");
        }
        #region 左侧泵房树查询
        private IEnumerable<dynamic> GetStationList(string stationName)
        {
            int userID = int.Parse(User.FindFirstValue("UserID"));

            var user = _ISysUserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
            if (stationName==null)
            {
                stationName = "";
            }
            var query = _ISws_AccessoriesEquService.GetStationByAccessory( user.IsAdmin, user.UserId, stationName);
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
        //根据泵房id查询相关器件
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult QueryAccessBYStationid(int stationid)
        {
            var f_itemid = (int)Model.CustomizedClass.Enum.设备分区;
            var data = _ISws_AccessoriesEquService.GetAccessBySId(stationid, f_itemid);
            return Json(new { 
                data=data
            });
        }
    }
}