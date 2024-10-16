using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using WNMS.Application.Utility.Filters;
using WNMS.IService;
using WNMS.Model.DataModels;

namespace WNMS.Application.Areas.Com
{
    [Area("Com")]
    [TypeFilter(typeof(IgonreActionFilter))]
    public class MaintenanceController : Controller
    {
        public ISws_AccessoriesMaintenanceService _sws_AccessoriesMaintenanceService = null;
        public ISysUserService _sysUserService = null;
        public ISws_AccessoriesEquService _ISws_AccessoriesEquService = null;
        public ISws_AccessoriesService _sws_AccessoriesService = null;
        public MaintenanceController(ISws_AccessoriesMaintenanceService sws_AccessoriesMaintenanceService, ISysUserService sysUserService, ISws_AccessoriesEquService _AccessoriesEquService,
           ISws_AccessoriesService sws_AccessoriesService
            )
        {
            _sws_AccessoriesMaintenanceService = sws_AccessoriesMaintenanceService;
            _sysUserService = sysUserService;
            _ISws_AccessoriesEquService = _AccessoriesEquService;
            _sws_AccessoriesService = sws_AccessoriesService;
        }
        public IActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 数据查询
        /// </summary>
        /// <param name="pagesize">条数</param>
        /// <param name="pageindex">页码</param>
        /// <param name="SearchText">查询条件</param>
        /// <param name="order">排序</param>
        /// <param name="sort"></param>
        /// <param name="time">时间</param>
        /// <param name="beginDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <returns></returns>
        public IActionResult QueryMaintenanceTable(int pagesize, int pageindex, string SearchText, string order, string sort, string time, string beginDate, string endDate)
        {
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = _sysUserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();

            int Totalcount = 0;
            string ordertems = sort + " " + order;
            string filter = "1=1";


            if (SearchText != null)
            {
                filter += " and (Name like '%" + SearchText + "%' or DeviceName like '%" + SearchText + "%')";
            }

            if (!string.IsNullOrEmpty(time))
            {
                GetBeginDate(time, ref beginDate, ref endDate);
                if (!string.IsNullOrEmpty(beginDate) || !string.IsNullOrEmpty(endDate))
                {

                    if (!string.IsNullOrEmpty(beginDate))
                    {
                        filter += " and  a.MaintenanceDate>='" + beginDate + "'";
                    }
                    if (!string.IsNullOrEmpty(endDate))
                    {
                        filter += " and  a.MaintenanceDate<'" + endDate + "'";
                    }
                }

            }
            if (user.IsAdmin == false)//非管理员
            {
                filter += "and UserID= " + userID + "";
            }
            var datalist = _sws_AccessoriesMaintenanceService.QueryMaintenanceTable(user.IsAdmin, user.UserId, pageindex, pagesize, ordertems, filter, ref Totalcount);
            return Json(new { total = Totalcount, rows = Newtonsoft.Json.JsonConvert.SerializeObject(datalist) });
        }
        //获取查询开始日期
        void GetBeginDate(string executeTime, ref string BeginTime, ref string EndTime)
        {
            switch (executeTime)
            {
                case "昨天":
                    BeginTime = string.Format("{0:yyyy-MM-dd}", DateTime.Now.AddDays(-1));
                    EndTime = string.Format("{0:yyyy-MM-dd}", DateTime.Now);
                    break;
                case "上周":
                    var dayofWeek = (int)DateTime.Now.DayOfWeek == 0 ? 7 : (int)DateTime.Now.DayOfWeek;
                    BeginTime = string.Format("{0:yyyy-MM-dd}", DateTime.Now.AddDays(-(dayofWeek - 1) - 7));
                    EndTime = string.Format("{0:yyyy-MM-dd}", DateTime.Now.AddDays(-(dayofWeek - 1)));
                    break;
                case "本周":
                    var dayofWeek1 = (int)DateTime.Now.DayOfWeek == 0 ? 7 : (int)DateTime.Now.DayOfWeek;
                    BeginTime = string.Format("{0:yyyy-MM-dd}", DateTime.Now.AddDays(-(dayofWeek1 - 1)));
                    EndTime = string.Format("{0:yyyy-MM-dd}", DateTime.Now.AddDays(-(dayofWeek1 - 1) + 7));
                    break;
                case "下周":
                    var dayofWeek2 = (int)DateTime.Now.DayOfWeek == 0 ? 7 : (int)DateTime.Now.DayOfWeek;
                    BeginTime = string.Format("{0:yyyy-MM-dd}", DateTime.Now.AddDays(-(dayofWeek2 - 1) + 7));
                    EndTime = string.Format("{0:yyyy-MM-dd}", DateTime.Now.AddDays(-(dayofWeek2 - 1) + 14));
                    break;
                case "上月":
                    BeginTime = string.Format("{0:yyyy-MM}", DateTime.Now.AddMonths(-1)) + "-01";
                    EndTime = string.Format("{0:yyyy-MM}", DateTime.Now) + "-01";
                    break;
                case "本月":
                    BeginTime = string.Format("{0:yyyy-MM}", DateTime.Now) + "-01";
                    EndTime = string.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(string.Format("{0:yyyy-MM-01}", DateTime.Now.AddMonths(1))));
                    break;
                case "下月":
                    BeginTime = string.Format("{0:yyyy-MM}", DateTime.Now.AddMonths(1)) + "-01";
                    EndTime = string.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(string.Format("{0:yyyy-MM-01}", DateTime.Now.AddMonths(2))));
                    break;
                case "自定义":
                    if (EndTime != null)
                    {
                        EndTime = string.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(EndTime).AddDays(1));
                    }
                    break;
                default:
                    break;

            }

        }

        #region 页面操作
        //添加
        public IActionResult AddPage(long id)
        {
            if (id == 0)
            {
                ViewBag.Name = "";
                ViewBag.ID = 0;
                ViewBag.MaintenanceDate = DateTime.Now.ToString("yyyy-MM-dd");
            }
            else
            {
                var info = _ISws_AccessoriesEquService.Find<SwsAccessoriesEqu>(id);
                var accessinfo = _sws_AccessoriesService.Query<SwsAccessories>(r => r.Id == info.AccessoriesId).FirstOrDefault();
                if (accessinfo != null)
                {
                    ViewBag.Name = accessinfo.Name;
                    ViewBag.ID = info.Id;
                }
                
                ViewBag.MaintenanceDate = DateTime.Now.ToString("yyyy-MM-dd");
            } 
            return View("_SetInfoPage", new SwsAccessoriesMaintenance());
        }
        //编辑
        public IActionResult EditePage(int id)
        {
            SwsAccessoriesMaintenance swsAccessoriesMaintenance = _sws_AccessoriesMaintenanceService.Find<SwsAccessoriesMaintenance>(id);
            var saq = _ISws_AccessoriesEquService.Query<SwsAccessoriesEqu>(r => r.Id == swsAccessoriesMaintenance.AccessoriesDetailId).FirstOrDefault();
            var swsAccessorie = _sws_AccessoriesService.Query<SwsAccessories>(t => t.Id == saq.AccessoriesId).FirstOrDefault();
            if (swsAccessorie != null)
            {
                ViewBag.Name = swsAccessorie.Name;
            }
            else
            {
                ViewBag.Name = "";
            }
            ViewBag.MaintenanceDate = swsAccessoriesMaintenance.MaintenanceDate;
            return View("_SetInfoPage", swsAccessoriesMaintenance);
        }
        //删除
        [HttpPost]
        public IActionResult DeleteMaintenance(string requestids)
        {
            List<int> Ids = requestids.Split(',').Select(x => Convert.ToInt32(x)).ToList();
            if (_sws_AccessoriesMaintenanceService.DeleteMaintenances(Ids) > 0)
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }
        }

        //提交表单
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult SetInfo(SwsAccessoriesMaintenance swsAccessoriesMaintenance, int id_M)
        {
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = _sysUserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
            if (id_M == 0)
            {
                if (_sws_AccessoriesMaintenanceService.Find<SwsAccessoriesMaintenance>(swsAccessoriesMaintenance.Id) != null)
                {
                    return Content("has");
                }
                else
                {
                    //添加保养时，更新期间详情表的保养时间，删除保养提醒表
                    if (_sws_AccessoriesMaintenanceService.SetMaintenances(swsAccessoriesMaintenance, 0) > 0)
                    {
                        return Content("ok");
                    }
                    else
                    {
                        return Content("no");
                    }
                }
            }
            else
            {
                swsAccessoriesMaintenance.Id = id_M;
                if (_sws_AccessoriesMaintenanceService.SetMaintenances(swsAccessoriesMaintenance, 1) > 0)
                {
                    return Content("ok");
                }
                else
                {
                    return Content("no");
                }
            }

        }
        //选择器件
        public IActionResult SelectAccessories()
        {
            return View();

        }
        //获取器件信息
        [HttpPost]
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult QueryStationTable(int pagesize, int pageindex, string SearchText, string order, string sort, string beginDate, string endDate)
        {
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = _sysUserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();

            int Totalcount = 0;
            string ordertems = sort + " " + order;
            string filter = "1=1";


            if (SearchText != null)
            {
                filter += " and (Name like '%" + SearchText + "%' or AccessoriesNo like '%" + SearchText + "%' or DeviceName like '%" + SearchText + "%')";
            }
            if (user.IsAdmin == false)//非管理员
            {
                filter += "and UserID= " + userID + "";
            }
            var datalist = _ISws_AccessoriesEquService.QueryAccessoryEquTable(user.IsAdmin, user.UserId, pageindex, pagesize, ordertems, filter, ref Totalcount);
            return Json(new { total = Totalcount, rows = Newtonsoft.Json.JsonConvert.SerializeObject(datalist) });
        }

        #endregion

        #region 导入
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult Import(IFormFile excelfile)
        {
            string ExcelKzm = ".xls|.xlsx";
            string OldName = string.Empty;
            OldName = "." + excelfile.FileName.Split('.')[1];
            if (ExcelKzm.IndexOf(OldName) < 0)
            {
                return Content("typeno");
            }
            List<SwsAccessoriesMaintenance> list = new List<SwsAccessoriesMaintenance> { };

            MemoryStream ms = new MemoryStream();
            excelfile.CopyTo(ms);
            ms.Seek(0, SeekOrigin.Begin);
            IWorkbook workbook = null;
            if (OldName == ".xls")
            {
                workbook = new HSSFWorkbook(ms);

            }
            else
            {
                workbook = new XSSFWorkbook(ms);

            } 
            ISheet sheet = workbook.GetSheetAt(0);
            try
            {
                for (int i = 1; i <= sheet.LastRowNum; i++)
                {
                    IRow row = sheet.GetRow(i);
                    SwsAccessoriesMaintenance obj = new SwsAccessoriesMaintenance();
                    if (row.GetCell(3) != null)
                    {
                        //Regex reg = new Regex(@"[\u4e00-\u9fa5]");
                        //string md = reg.Replace(row.GetCell(3).ToString(), "");
                        obj.AccessoriesDetailId = long.Parse(row.GetCell(0)?.ToString());
                        obj.MaintenanceDate = DateTime.ParseExact(row.GetCell(3).ToString(), "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture, DateTimeStyles.NoCurrentDateDefault);
                        obj.Cost = decimal.Parse(row.GetCell(4)?.ToString());
                        obj.MaintenanceContent = row.GetCell(5)?.ToString();
                        obj.MaintenanceUser = row.GetCell(6)?.ToString();
                    }
                    else
                    {
                        continue;
                    }
                    list.Add(obj);
                }
                ms.Dispose();
                if (_sws_AccessoriesMaintenanceService.MaintenanceImport(list) > 0)
                {
                    return Content("ok");
                }
                else
                {
                    return Content("no");
                }
            }
            catch (Exception ex) {
                return Content("exception");
            }
            
        }
        #endregion
    }
}