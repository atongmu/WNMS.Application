using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WNMS.Application.Utility.Filters;
using WNMS.IService;
using WNMS.Model.DataModels;

namespace WNMS.Application.Areas.Com.Controllers
{
    [Area("Com")]
    [TypeFilter(typeof(IgonreActionFilter))]
    public class MaintenanceRtController : Controller
    {
        public ISws_AccessoriesMaintenanceService _sws_AccessoriesMaintenanceService = null;
        public ISysUserService _sysUserService = null; 
        public ISws_AccessoriesService _sws_AccessoriesService = null;

        public MaintenanceRtController(ISws_AccessoriesMaintenanceService sws_AccessoriesMaintenanceService, ISysUserService sysUserService,
           ISws_AccessoriesService sws_AccessoriesService
            )
        {
            _sws_AccessoriesMaintenanceService = sws_AccessoriesMaintenanceService;
            _sysUserService = sysUserService; 
            _sws_AccessoriesService = sws_AccessoriesService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult QueryMaintenanceRtTable(int pagesize, int pageindex, string SearchText, string order, string sort)
        {
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = _sysUserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();

            int Totalcount = 0;
            string ordertems = sort + " " + order;
            string filter = " 1=1 ";


            if (SearchText != null)
            {
                filter += " and (Name like '%" + SearchText + "%' or DeviceName like '%" + SearchText + "%')";
            } 
            if (user.IsAdmin == false)//非管理员
            {
                filter += " and UserID= " + userID + "";
            }
            var datalist = _sws_AccessoriesMaintenanceService.QueryMaintenanceRtTable(user.IsAdmin, user.UserId, pageindex, pagesize, ordertems, filter, ref Totalcount);
            return Json(new { total = Totalcount, rows = Newtonsoft.Json.JsonConvert.SerializeObject(datalist) });
        }
        //标记已读
        [HttpPost]
        public IActionResult DeleteMaintenance(string requestids)
        {
            List<long> Ids = requestids.Split(',').Select(x => Convert.ToInt64(x)).ToList();
            if (_sws_AccessoriesMaintenanceService.ReadMaintenances(Ids) > 0)
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