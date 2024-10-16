using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WNMS.Application.Utility;
using WNMS.Application.Utility.Filters;
using WNMS.IService;
using WNMS.Model.CustomizedClass;
using WNMS.Model.DataModels;
using WNMS.Utility;

namespace WNMS.Application.Areas.Sws.Controllers
{
    [Area("Sws")]
    public class Sws_CameraInfoController : Controller
    {
        private ISws_CameraService cameraService = null;
        private ISws_StationService stationService = null;
        private ISws_AccessControlService accessControlService = null;
        private ISys_DataItemDetailService dataItemDetailServices = null;
        private ISws_GUIInfoService guiInfoService = null;
        public Sws_CameraInfoController(ISws_CameraService sws_CameraService, ISws_AccessControlService sws_AccessControlService,
           ISws_StationService sws_StationService)
        {
            cameraService = sws_CameraService;
            accessControlService = sws_AccessControlService;
            stationService = sws_StationService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [TypeFilter(typeof(IgonreActionFilter))]
        public async Task<IActionResult> LoadCameraInfo(string sortName, string sortOrder, string cameraName, int pageSize = 10, int pageIndex = 1)
        {
            int userID = int.Parse(User.FindFirstValue("UserID"));
            #region 查询条件
            Expression<Func<CameraInfo, bool>> funcWhere = null;
            if (!string.IsNullOrWhiteSpace(cameraName))
            {
                funcWhere = funcWhere.And(r => r.CameraName.Contains(cameraName));
            }
            #endregion

            #region  排序
            bool flag = true;
            if (sortOrder == "desc") flag = false;
            string sort = string.IsNullOrWhiteSpace(sortName) ? "CameraID" : sortName;
            #endregion

            PageResult<CameraInfo> cameraList = this.cameraService.LoadInfoList(funcWhere, pageSize, pageIndex, sort, userID, true, flag);
            PartialView("_CameraTable", cameraList.DataList);
            string dataTable = await ViewToString.RenderPartialViewToString(this, "_CameraTable");
            return Json(new
            {
                total = cameraList.TotalCount,
                pageIndex = cameraList.PageIndex,
                pageSize = cameraList.PageSize,
                order = sortOrder,
                sortName = sortName ?? "",
                dataTable = dataTable
            });
        }

        #region 摄像头信息编辑
        public IActionResult AddCameraPage()
        {
            ViewBag.StationName = "";
            return View("SetCamera", new SwsCamera());
        }

        public IActionResult EditCameraPage(int id)
        {
            SwsCamera swsCamera = cameraService.Find<SwsCamera>(id);
            swsCamera.PassWord = WNMS.Encrypt.Decode(swsCamera.PassWord);
            SwsStation station = stationService.Find<SwsStation>(swsCamera.StationId);
            HttpContext.Session.SetInt32("stationID", swsCamera.StationId);
            if (station != null)
            {
                ViewBag.StationName = station.StationName;
            }
            else
            {
                ViewBag.StationName = "";
            }
            return View("SetCamera", swsCamera);
        }

        [HttpPost]
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult GetDoorID(int stationID)
        {
            List<SwsAccessControl> list = new List<SwsAccessControl>();
            list = accessControlService.Query<SwsAccessControl>(r => r.StationId == stationID).ToList();
            return Json(new
            {
                list = list
            });
        }
        [HttpPost]
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult SetCameraInfo(SwsCamera camera)
        {
            if (camera.CameraId == 0)
            {
                string passWord = WNMS.Encrypt.Encode(camera.PassWord);   //加密;
                                                                          //构造userID

                int? cId = this.cameraService.Query<SwsCamera>(c => true).Select(c => c.CameraId).OrderByDescending(c => c).FirstOrDefault();

                //int? cId = this.cameraService.Query<SwsCamera>(c => true)?.Max(c => c.CameraId);
                camera.CameraId = cId + 1 ?? 1;
                camera.PassWord = passWord;
                if (cameraService.AddCameraEntity(camera) > 0)
                {
                    return Content("ok");
                }
                else
                {
                    return Content("false");
                }
            }
            else
            {
                int stationId = HttpContext.Session.GetInt32("stationID") == null ? 0 : (int)(int)HttpContext.Session.GetInt32("stationID");
                string passWord = WNMS.Encrypt.Encode(camera.PassWord);   //加密;
                camera.PassWord = passWord;
                if (cameraService.EditCameraEntity(camera, stationId) > 0)
                {
                    return Content("ok");
                }
                else
                {
                    return Content("false");
                }
            }
        }

        [HttpPost]
        public IActionResult DeleteCamera(string cameraId)
        {
            List<int> detaids = new List<string>(cameraId.Split(',')).ConvertAll(r => int.Parse(r));


            if (this.cameraService.DeleteCameraEntity(detaids) > 0)
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }
        }
        #endregion

        #region 选择泵房
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult StationInfo()
        {
            return View();
        }
        #endregion

    }
}