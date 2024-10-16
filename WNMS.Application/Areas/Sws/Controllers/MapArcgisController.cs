using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WNMS.Application.Utility.JsonHelper;
using WNMS.IService;
using WNMS.Model.CustomizedClass;
using WNMS.Utility;

namespace WNMS.Application.Areas.Sws.Controllers
{
    [Area("Sws")]
    public class MapArcgisController : Controller
    {
        private ISws_StationService _StationService = null;
        private ISysUserService _UserService = null;
        private ISws_EventInfoService _EventInfoService = null;
        private readonly IWebHostEnvironment _webHostEnvironment;
        int timevalue = int.Parse(StaticConstraint.TimeValue);
        public MapArcgisController(ISws_StationService sws_StationService,
            ISysUserService sysUserService, ISws_EventInfoService sws_EventInfoService,
             IWebHostEnvironment webHostEnvironment
           )
        {
            _StationService = sws_StationService;
            _UserService = sysUserService;
            _EventInfoService = sws_EventInfoService;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            var path = _webHostEnvironment.ContentRootPath;
            JsonFileHelper jsonFileHelper = new JsonFileHelper(path + "/Utility/ProjectJson/project.json");
            SystemInfo st = jsonFileHelper.Read<SystemInfo>("SystemInfo");
            ViewBag.lng = st.Lng;
            ViewBag.lat = st.Lat;
            ViewBag.zoom = st.MapLevel;
            ViewBag.AreaName = st.AreaName;
            ViewBag.ArcgisUrl = StaticConstraint.ArcgisUrl;
            return View();
        }
    }
}
