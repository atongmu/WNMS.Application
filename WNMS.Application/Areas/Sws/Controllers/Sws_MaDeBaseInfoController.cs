using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WNMS.Application.Utility.Filters;
using WNMS.Model.CustomizedClass;
using WNMS.IService;
using WNMS.Model.DataModels;

namespace WNMS.Application.Areas.Sws.Controllers
{
    [Area("Sws")]
    public class Sws_MaDeBaseInfoController : Controller
    {
        private ISws_ConsumpSettingService _ConsumpSettingService;
        private ISys_DataItemDetailService _DataItemDetailService;
        private ISws_MaDeBaseInfoService _MaDeBaseInfoService;
        private ISysUserService _UserService;
        public Sws_MaDeBaseInfoController(ISws_ConsumpSettingService sws_ConsumpSettingService,
            ISys_DataItemDetailService sys_DataItemDetailService,
            ISws_MaDeBaseInfoService sws_MaDeBaseInfoService,
            ISysUserService sysUserService)
        {
            _ConsumpSettingService = sws_ConsumpSettingService;
            _DataItemDetailService = sys_DataItemDetailService;
            _MaDeBaseInfoService = sws_MaDeBaseInfoService;
            _UserService = sysUserService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult QueryTable(int pagesize, int pageindex, string SearchText, string order, string sort)
        {
            Expression<Func<ManuDeviceBase, bool>> funcWhere = r => true;
            bool flag = true;
            //查询条件

            if (!string.IsNullOrEmpty(SearchText))
            {
                funcWhere = r => (r.ManufacterName.Contains(SearchText)||r.PumpBrandName.Contains(SearchText));
            }
            if (order == "desc")
            {
                flag = false;
            }
            var infoList = _ConsumpSettingService.GetMaDeviceBaseTable(funcWhere, pagesize, pageindex, sort, flag);
            return Json(new { total = infoList.TotalCount, rows = Newtonsoft.Json.JsonConvert.SerializeObject(infoList.DataList.ToList()) });
        }
        public IActionResult AddInfo()
        {
            var manuid = (int)Extra_enum.厂商;
            var pumpbrand = (int)Extra_enum.水泵品牌;
            var DataItemDetail = _DataItemDetailService.Query<SysDataItemDetail>(r => (r.FItemId == manuid || r.FItemId == pumpbrand) && r.IsEnable == true);
            ViewBag.DataItemDetail = DataItemDetail;
            var model = new SwsMaDeBaseInfo();
            model.WaterFlow = 1;
            model.HighLift = 100;

            return View("_SetInfo", model);
        }
        public IActionResult EditeInfo(int id)
        {
            var manuid = (int)Extra_enum.厂商;
            //var devicetype = (int)Extra_enum.设备型号;
            var pumpbrand = (int)Extra_enum.水泵品牌;
            var DataItemDetail = _DataItemDetailService.Query<SysDataItemDetail>(r => (r.FItemId == manuid || r.FItemId == pumpbrand) && r.IsEnable == true);
            ViewBag.DataItemDetail = DataItemDetail;
            var model = _MaDeBaseInfoService.Query<SwsMaDeBaseInfo>(r => r.Id== id).FirstOrDefault();
           
            return View("_SetInfo",model);
        }
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult SetBaseInfo(SwsMaDeBaseInfo b)
        {
            
            if (b.Id == 0)//添加
            {
                var ishave = _MaDeBaseInfoService.Query<SwsMaDeBaseInfo>(r => r.Manufacturer == b.Manufacturer && r.PumpBrand == b.PumpBrand).FirstOrDefault();
                if (ishave!=null)
                { 
                    return Content("have");
                }
                b.Id = _UserService.QueryID("ID", "Sws_MaDeBaseInfo");
                if (b.MotorEff != 0 && b.PumpEff != 0)
                {
                    b.ValueData = Math.Round((double)(b.WaterFlow * 2.7222 * b.HighLift) / (double)(b.MotorEff * b.PumpEff * 0.01 * 0.01), 2);
                }
                else
                {
                    b.ValueData = 0;
                }
                if (_ConsumpSettingService.AddInfo(b)>0)
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
                var ishave = _MaDeBaseInfoService.Query<SwsMaDeBaseInfo>(r => r.Manufacturer == b.Manufacturer && r.PumpBrand == b.PumpBrand && r.Id!=b.Id).FirstOrDefault();
                if (ishave != null)
                {
                    return Content("have");
                }
                if (b.MotorEff != 0 && b.PumpEff != 0)
                {
                    b.ValueData = Math.Round((double)(b.WaterFlow * 2.7222 * b.HighLift) / (double)(b.MotorEff * b.PumpEff * 0.01 * 0.01), 2);
                }
                else
                {
                    b.ValueData = 0;
                }
                if (_ConsumpSettingService.EditeInfo(b) > 0)
                {
                    return Content("ok");
                }
                else
                {
                    return Content("no");
                }
            }
            
        }
        [HttpPost]
        public IActionResult DeleteInfos(string ids)
        {
            List<int> idlist = new List<string>(ids.Split(",")).ConvertAll(r => int.Parse(r));
            if (_ConsumpSettingService.DeleteInfo(idlist)>0)
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