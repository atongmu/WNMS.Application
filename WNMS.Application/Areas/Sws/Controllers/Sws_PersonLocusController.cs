using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using WNMS.Application.Utility.Filters;
using WNMS.IService;
using WNMS.Model.CustomizedClass;

namespace WNMS.Application.Areas.Sws.Controllers
{
    [Area("Sws")]
    public class Sws_PersonLocusController : Controller
    {
        private ISws_ConsumpSettingService _ConsumpSettingService = null;
        public Sws_PersonLocusController(ISws_ConsumpSettingService sws_ConsumpSettingService) {
            _ConsumpSettingService = sws_ConsumpSettingService;
        }
        public IActionResult Index()
        {
           
            ViewBag.beginDate = DateTime.Now.ToString("yyyy-MM-dd");
            //ViewBag.endDate = DateTime.Now;
            return View();
        }
        
        [HttpPost]
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult QueryPersonGps(string gpsnum,int pagesize,int pageindex,string begindate,string endate)
        {
            var infoList = _ConsumpSettingService.GetPersonLocus(gpsnum, pageindex, pagesize, begindate, endate);
            return Json(new { totalpage =Math.Ceiling((double)infoList.TotalCount/ pagesize), rows = infoList.DataList.ToList()});
        }
        public string GetPersonTree(string searchtext)
        {
            var list = _ConsumpSettingService.PersonLocusTree(searchtext);
            var araeainfo = list.Select(r => new treestring
            {
                id = "a"+r.ID.ToString(),
                pId ="a"+ r.PID.ToString(),
                name = "<i class='fa fa-sitemap'></i>" + r.AreaName,
                isUser = false
            });
            var userlist = list.Where(r => r.type == 2).Select(r => new treestring
            {
                id = r.UserID.ToString(),
                pId = "a" + r.ID.ToString(),
                name = "<i class='iconfont  icon-yonghu'></i>" + r.NickName.ToString(),
                nickname= r.NickName,
                isUser = true,
                serialNumber = r.SerialNumber.ToString()
            });
            var data = araeainfo.Concat(userlist).Distinct(new treestringCompare());
            if (data.Count() > 0)
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(data);
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
            public string nickname { get; set; }
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
        //人员列表
        [HttpPost]
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult QueryUserTable(int pagesize,int pageindex,string searchTime,string userName,string order,string sort)
        {
           
            bool flag = true;
            if (order == "desc")
            {
                flag = false;
            }
           
            var infoList = _ConsumpSettingService.LocusUserList(userName, searchTime, pagesize, pageindex, sort, flag);
            return Json(new { total = infoList.TotalCount, rows = Newtonsoft.Json.JsonConvert.SerializeObject(infoList.DataList.ToList()) });
        }
    }
}