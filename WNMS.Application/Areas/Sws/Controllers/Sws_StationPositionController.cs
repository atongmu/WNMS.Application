using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WNMS.Application.Utility;
using WNMS.IService;
using WNMS.Model.DataModels;
using WNMS.Model.CustomizedClass;
using System.Security.Claims;
using WNMS.Application.Utility.Filters;
using Microsoft.AspNetCore.Hosting;
using WNMS.Application.Utility.JsonHelper;
using WNMS.Utility;

namespace WNMS.Application.Areas.Sws.Controllers
{
    [Area("Sws")]
    public class Sws_StationPositionController : Controller
    {
        private ISws_StationService stationService = null;
        private ISys_DataItemDetailService dataItemDetailServices = null;
        private readonly IWebHostEnvironment webHostEnvironment;
        string arcgisUrl = StaticConstraint.ArcgisUrl;
        public Sws_StationPositionController(ISws_StationService sws_StationService, ISys_DataItemDetailService sys_DataItemDetailService,
            IWebHostEnvironment _webHostEnvironment)
        {
            stationService = sws_StationService;
            dataItemDetailServices = sys_DataItemDetailService;
            webHostEnvironment = _webHostEnvironment;
        }
        public IActionResult Index()
        {
            int userID = int.Parse(User.FindFirstValue("UserID"));
            //泵房类型
            var deviceType = stationService.GetStationType(userID);
            ViewBag.deviceType = deviceType;

            //获取地图级别
            var path = webHostEnvironment.ContentRootPath;
            JsonFileHelper jsonFileHelper = new JsonFileHelper(path + "/Utility/ProjectJson/project.json");
            SystemInfo st = jsonFileHelper.Read<SystemInfo>("SystemInfo");
            string zoom = st.MapLevel == "" ? "11" : st.MapLevel;
            ViewBag.Zoom = Convert.ToInt32(zoom);
            ViewBag.lng = st.Lng;
            ViewBag.lat = st.Lat;
            ViewBag.AreaName = st.AreaName == "" ? "兰山区" : st.AreaName;
            ViewBag.arcgisUrl = arcgisUrl;
            return View();
        }

        //获取泵房列表信息
        [HttpPost]
        [TypeFilter(typeof(IgonreActionFilter))]
        public async Task<IActionResult> GetStationData(string stationName,int stationtype)
        {
            if (stationName == null) stationName = "";
            int userID = int.Parse(User.FindFirstValue("UserID"));
            List<dynamic> stationList = this.stationService.GetMapStationInfo(stationName,stationtype,userID).ToList();
            PartialView("_StationTable", stationList);
            string dataTable = await ViewToString.RenderPartialViewToString(this, "_StationTable");
            return Json(new
            {
                dataTable = dataTable,
                dataList=stationList
            });
        }

        [HttpPost]
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult GetMapWindowInfo(int stationID)
        {
            //获取泵房信息
             List<dynamic> stationInfo = this.stationService.GetStationByID(stationID).ToList();
            //获取泵房下设备信息
            int? type = this.stationService.Find<SwsStation>(stationID)?.InType;
            List<DeviceInfo01Info> device01 = new List<DeviceInfo01Info>();
            List<DeviceInfo02Info> device02 = new List<DeviceInfo02Info>();
            if (type==1)
            {
                device01 = this.stationService.GetDevice01ByID(stationID).ToList();
            }
            if (type == 2)
            {
                device02 = this.stationService.GetDevice02ByID(stationID).ToList();
            }
            //获取泵房资产信息
            List<dynamic> property = this.stationService.GetPropertyInfoByID(stationID).ToList();

            //获取附件信息
            List<string> suff = new List<string> { ".jpg", ".png", "jpeg" };
            List<Attachment> attachments = this.stationService.Query<Attachment>(r => r.Affiliation == stationID&& !suff.Contains(r.Suffix)).ToList();

            //泵房样貌
            var imgList = stationService.Query<Attachment>(r => r.Affiliation == stationID && suff.Contains(r.Suffix)).ToList();
            
            return Json(new
            {
                station=stationInfo,
                device01=device01,
                device02=device02,
                type=type,
                property= property,
                attach =attachments,
                imgList=imgList
            });
        }
    }
}