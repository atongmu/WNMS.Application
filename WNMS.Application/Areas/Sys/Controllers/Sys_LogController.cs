using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WNMS.Application.Utility.Filters;
using WNMS.IService;
using WNMS.Model.DataModels;
using WNMS.Utility;

namespace WNMS.Application.Areas.Sys.Controllers
{
    [Area("Sys")]
    public class Sys_LogController : Controller
    {
        private readonly ISys_LogService _sys_LogService = null;
        private ISysUserService _sysUserService = null;

        public Sys_LogController(ISys_LogService sys_LogService, ISysUserService sysUserService)
        {
            _sys_LogService = sys_LogService;
            _sysUserService = sysUserService;
        }
        public IActionResult Index()
        {
            ViewBag.beginDate = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now.AddHours(-12));
            ViewBag.endDate = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now);
            return View();
        }
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult LoadInfoList(int pageSize, int pageIndex, string sort, string sortOrder, DateTime beginTime, DateTime endTime, string username)
        {
            int totalCount = 0;
            bool flag = true;
            //查询条件
            Expression<Func<SysLog, bool>> expr1 = r => true;
            Expression<Func<SysLog, bool>> expr2 = r => true;
            if (!string.IsNullOrEmpty(beginTime.ToString()) && !string.IsNullOrEmpty(endTime.ToString()))
            {
                expr1 = r => r.LogDate >= beginTime && r.LogDate <= endTime;
            }
            if (!string.IsNullOrEmpty(username))
            {
                expr2 = r => r.UserName.Contains(username);
            }
            if (sortOrder == "desc")
            {
                flag = false;
            }
            var funcWhere = ExpressionExt.And(expr1, expr2);
            var dataList = _sys_LogService.QueryPage(funcWhere, pageSize, pageIndex, r => r.LogDate, flag);
            totalCount = dataList.TotalCount;
            return Json(new
            {
                rows = dataList.DataList,
                total = totalCount
            });
        }

        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult LoadInfosList(int pageSize, int pageIndex, string sort, string sortOrder, DateTime beginTime, DateTime endTime, string username)
        {
            int Totalcount = 0;
            string ordertems = sort + " " + sortOrder;
            string filter = "1=1";
            if (beginTime != null)
            {
                filter += " and LogDate between '"+ beginTime+ "'  and   '" + endTime+"'";
            }
            if (!string.IsNullOrEmpty(username))
            {
                filter += " and NickName like '%" + username + "%'";
            }
             
            var datalist = _sysUserService.GetLogInfoTable(pageIndex, pageSize, ordertems, filter, ref Totalcount);
            return Json(new { total = Totalcount, rows = Newtonsoft.Json.JsonConvert.SerializeObject(datalist) });
        }
    }
}