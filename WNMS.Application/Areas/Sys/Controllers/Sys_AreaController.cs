using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using WNMS.Application.Models;
using WNMS.Application.Utility.Filters;
using WNMS.IService;
using WNMS.Model.DataModels;

namespace WNMS.Application.Areas.Sys.Controllers
{
    [Area("Sys")]
    public class Sys_AreaController : Controller
    {
        private readonly ISys_AreaService _sys_AreaService = null;
        public Sys_AreaController(ISys_AreaService sys_AreaService)
        {
            _sys_AreaService = sys_AreaService;
        }
        public IActionResult Index()
        {
            var treeNodes = new TreesWithAction(_sys_AreaService).TreesOfArea(a => true);
            if (treeNodes != "")
            {
                ViewBag.TreeNodes = new HtmlString(treeNodes);
            }
            else
            {
                ViewBag.TreeNodes = "[]";
            }
            return View();
        }

        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult LoadInfoList(int pid, string sort, string order, int pageSize, int pageIndex)
        {
            int totalCount = 0; 
            Expression<Func<SysArea, bool>> funcWhere = r => r.Parents == pid;
            var dataList = _sys_AreaService.QueryPage(funcWhere, pageSize, pageIndex, r => r.Id, true);
            totalCount = dataList.TotalCount;

            return Json(new
            {
                table = dataList.DataList,
                totalCount = totalCount
            }); 
        }
    }
}