using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WNMS.Application.Utility;
using WNMS.Application.Utility.Filters;
using WNMS.IService;
using WNMS.Model.DataModels;
using WNMS.Utility;
using Microsoft.EntityFrameworkCore;
using WNMS.Application.Utility.JsonHelper;
using Microsoft.AspNetCore.Hosting;
using WNMS.Model.CustomizedClass;

namespace WNMS.Application.Areas.Sws.Controllers
{
    [Area("Sws")]
    public class Sws_GPSModuleController : Controller
    {
        #region 属性 构造函数
        private ISws_GPSModuleService gpsMouleService = null;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public Sws_GPSModuleController(ISws_GPSModuleService sws_gpsMouleService, IWebHostEnvironment webHostEnvironment)
        {
            gpsMouleService = sws_gpsMouleService;
            _webHostEnvironment = webHostEnvironment;
        }
        #endregion
        #region 数据查询

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [TypeFilter(typeof(IgonreActionFilter))]
        public async Task<IActionResult> GetData(string name, string order, string sortName, int pageSize=20, int pageIndex=1)
        {
            #region 查询条件
            var list = gpsMouleService.Query<SwsGpsmodule>(r => true);
            if (!string.IsNullOrEmpty(name))
            {
                list = gpsMouleService.Query<SwsGpsmodule>(r => r.GpSnumber.Contains(name));
            }

            int total = list.Count();
            if (string.IsNullOrEmpty(sortName))
            {
                sortName = "GpsName";
            }
            var gpslist = list.OrderBy(sortName);
            if (order== "desc")
            {
                gpslist = list.OrderByDescending(sortName);
            }
            #endregion
            List<SwsGpsmodule> gpsList = gpslist.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            PartialView("_GPSTable", gpsList);
            string dataTable = await ViewToString.RenderPartialViewToString(this, "_GPSTable");
            return Json(new
            {
                total = total,
                pageIndex = pageIndex,
                pageSize = pageSize,
                order = order,
                sortName = sortName ?? "",
                dataTable = dataTable
            });
        }

        #endregion

        #region 用户编辑
        public IActionResult AddGPSPage()
        {
            return View("SetGPS", new SwsGpsmodule());
        }

        public IActionResult EditGPSPage(int? id)
        {
            SwsGpsmodule gps = new SwsGpsmodule();
            if (id != null)
            {
                gps = this.gpsMouleService.Find<SwsGpsmodule>((int)id) ?? new SwsGpsmodule();
            }
            ViewBag.Gpsnumber = id;
            return View("SetGPS", gps);
        }

        [HttpPost]
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult SetGpsInfo(SwsGpsmodule gps)
        {
            if (gps.Gpsid == 0)    //插入
            {
                List<SwsGpsmodule> gpsList = this.gpsMouleService.Query<SwsGpsmodule>(u => u.GpSnumber == gps.GpSnumber).ToList();
                if (gpsList.Count > 0)
                {
                    return Content("has");
                }
                else
                {
                    gps.Gpsid = ConvertDateTimeInt(DateTime.Now);
                    SwsGpsmodule newgps = this.gpsMouleService.Insert(gps);
                    if (newgps != null)
                    {
                        var path = _webHostEnvironment.ContentRootPath;
                        JsonFileHelper jsonFileHelper = new JsonFileHelper(path + "/Utility/ProjectJson/project.json");
                        SystemInfo st = jsonFileHelper.Read<SystemInfo>("SystemInfo");
                        if (gpsMouleService.CreateGPSTable(gps, st.Lng, st.Lat) == 1)
                        {
                            return Content("ok");
                        }
                        else
                        {
                            return Content("cerror");
                        }                        
                    }
                    else
                    {
                        return Content("no");
                    }
                }
            }
            else    //修改
            {
                SwsGpsmodule gpsDB = this.gpsMouleService.Query<SwsGpsmodule>(r=>r.Gpsid==gps.Gpsid).AsNoTracking().FirstOrDefault();
                if (gpsDB == null)
                {
                    return Content("false");
                }
                else
                {
                    List<SwsGpsmodule> gpsList = this.gpsMouleService.Query<SwsGpsmodule>(u => u.GpSnumber == gps.GpSnumber&&u.GpSnumber!=gpsDB.GpSnumber).ToList();
                    if (gpsList.Count > 0)
                    {
                        return Content("has");
                    }
                    else
                    {
                        if (this.gpsMouleService.Update(gps))
                        {
                            return Content("ok");
                        }
                        else
                        {
                            return Content("no");
                        }
                    }
                }
            }
        }

        //时间戳
        public static int ConvertDateTimeInt(System.DateTime time)
        {
            System.DateTime startTime = System.TimeZoneInfo.ConvertTimeToUtc(new System.DateTime(1970, 1, 1));
            return (int)(time - startTime).TotalSeconds;
        }


        [HttpPost]
        public IActionResult DeleteGps(int gpsId)
        {
            if (this.gpsMouleService.Delete<SwsGpsmodule>(gpsId))
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }       
        }
        #endregion
    }
}
