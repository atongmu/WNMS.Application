using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using NPOI.HSSF.UserModel;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using WNMS.Application.Utility.Filters;
using WNMS.IService;
using WNMS.Model.DataModels;

namespace WNMS.Application.Areas.Com
{
    [Area("Com")]
    [TypeFilter(typeof(IgonreActionFilter))]
    public class AccessoriesController : Controller
    {
    
        public ISysUserService _sysUserService = null;
        private ISys_DataItemDetailService _DataItemDetailService = null;
        public ISws_AccessoriesService _sws_AccessoriesService = null;
        public ISws_AccessoriesEquService _ISws_AccessoriesEquService = null;
        public AccessoriesController(ISysUserService sysUserService, 
            ISws_AccessoriesService sws_AccessoriesService, 
            ISys_DataItemDetailService sys_DataItemDetailService,
            ISws_AccessoriesEquService _AccessoriesEquService)
        {
            
            _sysUserService = sysUserService;
            _sws_AccessoriesService = sws_AccessoriesService;
            _DataItemDetailService = sys_DataItemDetailService;
            _ISws_AccessoriesEquService = _AccessoriesEquService;
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
        /// <returns></returns>
        public IActionResult QueryAccessoriesTable(int pagesize, int pageindex, string SearchText, string order, string sort)
        {
            //int userID = int.Parse(User.FindFirstValue("UserID"));
            //var user = _sysUserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
            var MaintenancePeriod = (int)Model.CustomizedClass.Enum.保养周期;
            int Totalcount = 0;
            string ordertems = sort + " " + order;
            string filter = "1=1";

            filter += "and F_ItemId= " + MaintenancePeriod + "";
            if (SearchText != null)
            {
                filter += " and (Name like '%" + SearchText + "%' or Type like '%" + SearchText + "%')";
            }

       
            //if (user.IsAdmin == false)//非管理员
            //{
            //    filter += "and UserID= " + userID + "";
            //}
            var datalist = _sws_AccessoriesService.QueryAccessoriesTable( pageindex, pagesize, ordertems, filter, ref Totalcount);
            return Json(new { total = Totalcount, rows = Newtonsoft.Json.JsonConvert.SerializeObject(datalist) });
        }

        #region 页面操作
        //添加
        public IActionResult AddPage()
        {
            var MaintenancePeriod = (int)Model.CustomizedClass.Enum.保养周期;
            ViewBag.MaintenancePeriodlist = _DataItemDetailService.Query<SysDataItemDetail>(r => r.FItemId == MaintenancePeriod).ToList();

            return View("_SetInfoPage",new SwsAccessories());
        }
        //编辑
        public IActionResult EditePage(string id)
        {
            SwsAccessories swsAccessories = _sws_AccessoriesService.Query<SwsAccessories>(s=>s.Id==id).FirstOrDefault();
            var MaintenancePeriod = (int)Model.CustomizedClass.Enum.保养周期;
            ViewBag.MaintenancePeriodlist = _DataItemDetailService.Query<SysDataItemDetail>(r => r.FItemId == MaintenancePeriod).ToList();
            ViewBag.id_M = id;
           
            return View("_SetInfoPage", swsAccessories);
        }
        //删除
        [HttpPost]
        public IActionResult DeleteAccessories(string requestids)
        {
            List<string> Ids = requestids.Split(',').ToList();
 
            int count = _sws_AccessoriesService.DeleteAccessories(Ids);
            if (count > 0)
            {
                return Content("ok");
            }
            else if(count==-1)
            {
                return Content("存在已在用的设备无法删除");
            }
            else
            {
                return Content("no");
            }

        }

        //提交表单
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult SetInfo(SwsAccessories swsAccessories, string id_M)
        {
            
            if (String.IsNullOrEmpty(id_M))
            {
                var swsAccessory = _sws_AccessoriesService.Query<SwsAccessories>(S => S.Id == swsAccessories.Id)
                    .FirstOrDefault();
                if (swsAccessory != null)
                {
                    return Content("已经存在相同的器件编号");
                   // return Content("has");
                }
                else
                {
                    
                    if (_sws_AccessoriesService.Insert(swsAccessories) != null)
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
                swsAccessories.Id = id_M;
                var hasmodel = _sws_AccessoriesService.Query<SwsAccessories>(r => r.Id == swsAccessories.Id &&r.Id!=id_M).FirstOrDefault();
                if (hasmodel != null)
                {
                    return Content("已经存在相同的器件编号");
                }
                if (_sws_AccessoriesService.Update(swsAccessories))
                {
                    return Content("ok");
                }
                else
                {
                    return Content("no");
                }
            }

        }


        //查看在用量情况
        public IActionResult ViewEqu(string AccessoriesId)
        {
            ViewBag.id_M = AccessoriesId;
            return View();
        }
        [HttpPost]
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult QueryAccess_EquTable(int pagesize, int pageindex, string SearchText, string order, string sort, string time, string beginDate, string endDate,string id_M)
        {
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = _sysUserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();

            int Totalcount = 0;
            string ordertems = sort + " " + order;
            string filter = "1=1";

            if (id_M != null)
            {
                filter += " and AccessoriesID = '" + id_M + "'";
            }
            if (SearchText != null)
            {
                filter += " and (Name like '%" + SearchText + "%' or AccessoriesNo like '%" + SearchText + "%' or DeviceName like '%" + SearchText + "%')";
            }

            if (!string.IsNullOrEmpty(time))
            {
                GetBeginDate(time, ref beginDate, ref endDate);
                if (!string.IsNullOrEmpty(beginDate) || !string.IsNullOrEmpty(endDate))
                {

                    if (!string.IsNullOrEmpty(beginDate))
                    {
                        filter += " and  DeliveryDate>='" + beginDate + "'";
                    }
                    if (!string.IsNullOrEmpty(endDate))
                    {
                        filter += " and  DeliveryDate<'" + endDate + "'";
                    }
                }

            }

            var datalist = _ISws_AccessoriesEquService.QueryAccessoryEquTable(user.IsAdmin, user.UserId, pageindex, pagesize, ordertems, filter, ref Totalcount);
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
            List<SwsAccessories> list = new List<SwsAccessories> { };

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
                    SwsAccessories obj = new SwsAccessories();
                    if (row.GetCell(0) != null)
                    {
                      var acc =   _sws_AccessoriesService.Query<SwsAccessories>(a=>a.Id== row.GetCell(0).ToString()).FirstOrDefault();
                        if (acc == null)
                        {
                            obj.Id = row.GetCell(0)?.ToString();
                            obj.Name = row.GetCell(1).ToString();
                            obj.Type = row.GetCell(2).ToString();
                            obj.Material = row.GetCell(3).ToString();
                            obj.Model = row.GetCell(4).ToString();
                            obj.Place = row.GetCell(5).ToString();
                            obj.Manufacturer = row.GetCell(6).ToString();
                            obj.Inventory = Int32.Parse(row.GetCell(7)?.ToString());
                            obj.ReplacementCycle = Int32.Parse(row.GetCell(8)?.ToString());
                            obj.Unit = row.GetCell(9)?.ToString();
                            
                            obj.IsConsumable = row.GetCell(10)?.ToString() == "1" || row.GetCell(10)?.ToString() == "是" ? true : false;
                            obj.MaintenancePeriod = Int32.Parse(row.GetCell(11)?.ToString());
                        }
                        else
                        {
                            continue;
                        }
                        //Regex reg = new Regex(@"[\u4e00-\u9fa5]");
                        //string md = reg.Replace(row.GetCell(3).ToString(), "");

                    }
                    else
                    {
                        continue;
                    }
                    list.Add(obj);
                }
                ms.Dispose();
                if (_sws_AccessoriesService.AccessoriesImport(list) > 0)
                {
                    return Content("ok");
                }
                else
                {
                    return Content("no");
                }
            }
            catch (Exception ex)
            {
                return Content("exception");
            }

        }
        #endregion


 
    }
}
