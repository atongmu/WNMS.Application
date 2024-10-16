using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WNMS.Application.Utility.Filters;
using WNMS.IService;
using WNMS.Model.DataModels;

namespace WNMS.Application.Areas.Sws.Controllers
{
    [Area("Sws")]
    public class Sws_GpsborrowingController : Controller
    {

        private ISws_RTUInfoService _sws_RTUInfoService = null;
        private ISysUserService _sysUserService = null;
        private IWO_TeamUserService _wO_TeamUserService = null;
        private ISws_GpsborrowingService _sws_GpsborrowingService = null;
        private ISws_GPSModuleService _sws_GPSModuleService = null;

        public Sws_GpsborrowingController(ISws_RTUInfoService sws_RTUInfoService, ISysUserService sysUserService,
            IWO_TeamUserService wO_TeamUserService, ISws_GpsborrowingService sws_GpsborrowingService, ISws_GPSModuleService sws_GPSModuleService)
        {
            _sws_RTUInfoService = sws_RTUInfoService;
            _sysUserService = sysUserService;
            _wO_TeamUserService = wO_TeamUserService;
            _sws_GpsborrowingService = sws_GpsborrowingService;
            _sws_GPSModuleService = sws_GPSModuleService;
        }
        public IActionResult Index()
        {
            //ViewBag.beginDate = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now.AddDays(-7));
            //ViewBag.endDate = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now.AddDays(1));
            ViewBag.beginDate =  DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd HH:mm:ss");
            ViewBag.endDate = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd HH:mm:ss");
            return View();
        }
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult LoadGpsborrowingList(int pageSize, int pageIndex, string sort, string sortOrder, DateTime beginTime, DateTime endTime, string userName, string depName)
        {
            int Totalcount = 0;
            string ordertems = sort + " " + sortOrder;
            string filter = "1=1";
            if (beginTime != null)
            {
                filter += " and BorrowTime between '" + beginTime + "'  and   '" + endTime + "'";
            }
            if (!string.IsNullOrEmpty(userName))
            {
                filter += " and NickName like '%" + userName + "%'";
            }
            if (!string.IsNullOrEmpty(depName))
            {
                filter += " and DptName like '%" + depName + "%'";
            }

            var datalist = _sws_RTUInfoService.GetGpsborrowingTable(pageIndex, pageSize, ordertems, filter, ref Totalcount);
            return Json(new { total = Totalcount, rows = Newtonsoft.Json.JsonConvert.SerializeObject(datalist) });
        }

        #region 增删改 

        //添加页面
        public IActionResult AddPage()
        {
            ViewBag.UserInfo = _sws_RTUInfoService.LoadKF();
            //过滤设备
            var info = _sws_GpsborrowingService.Query<SwsGpsborrowing>(t=>t.RemandTime > DateTime.Now).Select(r=>r.SerialNumber).Distinct();
            ViewBag.GpsInfo = _sws_GPSModuleService.Query<SwsGpsmodule>(T => !info.Contains(T.GpSnumber));
            ViewBag.BorrowTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            ViewBag.RemandTime = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd HH:mm:ss");
            return View("SetInfo", new SwsGpsborrowing());
        }
        //编辑页面
        public IActionResult EditPage(int id)
        {
            SwsGpsborrowing swsGpsborrowing = _sws_GpsborrowingService.Find<SwsGpsborrowing>(id);
            ViewBag.UserInfo = _sws_RTUInfoService.LoadKF();
            var info = _sws_GpsborrowingService.Query<SwsGpsborrowing>(t => t.RemandTime < DateTime.Now).Select(r => r.SerialNumber).Distinct();
            ViewBag.GpsInfo = _sws_GPSModuleService.Query<SwsGpsmodule>(T => info.Contains(T.GpSnumber) || T.GpSnumber == swsGpsborrowing.SerialNumber);
            ViewBag.BorrowTime = swsGpsborrowing.BorrowTime?.ToString("yyyy-MM-dd HH:mm:ss");
            ViewBag.RemandTime = swsGpsborrowing.RemandTime?.ToString("yyyy-MM-dd HH:mm:ss");
            return View("SetInfo", swsGpsborrowing);
        }
        //表单提交
        [HttpPost]
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult SetInfo(SwsGpsborrowing swsGpsborrowing, int id_M)
        {
            if (id_M == 0)
            {
                swsGpsborrowing.Id = _sysUserService.QueryID("ID", "Sws_Gpsborrowing");
                if (_sws_GpsborrowingService.Insert(swsGpsborrowing) != null)
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
                swsGpsborrowing.Id = id_M;
                if (_sws_GpsborrowingService.Update(swsGpsborrowing))
                {
                    return Content("ok");
                }
                else
                {
                    return Content("no");
                }
            }
        }

        //批量删除
        [HttpPost]
        public ActionResult DeleteInfo(string requestids)
        {
            List<int> Ids = requestids.Split(',').Select(x => Convert.ToInt32(x)).ToList();
            if (_sws_RTUInfoService.DeleteGpsborrowing(Ids) > 0)
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