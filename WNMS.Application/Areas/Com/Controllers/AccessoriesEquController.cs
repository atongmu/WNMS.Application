using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WNMS.IService;
using WNMS.Model.DataModels;
using WNMS.Application.Utility.Filters;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;

namespace WNMS.Application.Areas.Com.Controllers
{
    [Area("Com")]
    public class AccessoriesEquController : Controller
    {
        public ISysUserService _ISys_UserService = null;
        public ISws_AccessoriesEquService _ISws_AccessoriesEquService = null;
        public ISws_AccessoriesService _ISws_AccessoriesService = null;
        public ISws_DeviceInfo01Service _ISws_DeviceInfo01Service = null;
        public ISws_DeviceInfo02Service _ISws_DeviceInfo02Service = null;
        public ISws_AccessoriesMaintenanceService _ISwsAccessMaintenService = null;
        public AccessoriesEquController(ISysUserService _userservice,
            ISws_AccessoriesEquService _AccessoriesEquService,
            ISws_AccessoriesService _AccessoriesService,
            ISws_DeviceInfo01Service _DeviceInfo01Service,
            ISws_DeviceInfo02Service _DeviceInfo02Service,
            ISws_AccessoriesMaintenanceService _AccessoriesMaintenanceService)
        {
            _ISys_UserService = _userservice;
            _ISws_AccessoriesEquService = _AccessoriesEquService;
            _ISws_AccessoriesService = _AccessoriesService;
            _ISws_DeviceInfo01Service = _DeviceInfo01Service;
            _ISws_DeviceInfo02Service = _DeviceInfo02Service;
            _ISwsAccessMaintenService = _AccessoriesMaintenanceService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult QueryAccess_EquTable(int pagesize, int pageindex, string SearchText, string order, string sort, string time, string beginDate, string endDate)
        {
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = _ISys_UserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();

            int Totalcount = 0;
            string ordertems = sort + " " + order;
            string filter = "1=1";


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
        #region 页面操作
        //添加页面
        public IActionResult AddAccessEquPage()
        {
            ViewBag.IsConsumable = false;
            var model = new SwsAccessoriesEqu();
            model.Quantity = 1;
            ViewBag.ComponentName = "";
            ViewBag.deviceName = "";
            return View("_setAccessEqu", model);
        }
        //修改页面
        public IActionResult EditeAccessEquPage(long id)
        {
            var accessEqu = _ISws_AccessoriesEquService.Query<SwsAccessoriesEqu>(r => r.Id == id).FirstOrDefault();
            var accessory = _ISws_AccessoriesService.Query<SwsAccessories>(r => r.Id == accessEqu.AccessoriesId).FirstOrDefault();
            ViewBag.ComponentName = accessory.Name;
            ViewBag.IsConsumable = accessory.IsConsumable;
            if (accessEqu.EquType == 1)
            {
                var devicemodel = _ISws_DeviceInfo01Service.Query<SwsDeviceInfo01>(r => r.DeviceId == accessEqu.EquipmentId).FirstOrDefault();
                ViewBag.deviceName = devicemodel.DeviceName;
            }
            if (accessEqu.EquType == 2)
            {
                var devicemodel = _ISws_DeviceInfo02Service.Query<SwsDeviceInfo02>(r => r.DeviceId == accessEqu.EquipmentId).FirstOrDefault();
                ViewBag.deviceName = devicemodel.DeviceName;
            }
            return View("_setAccessEqu", accessEqu);
        }
        //选择器件
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult SelectAccessory()
        {
            return View();
        }
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult LoadAccessoryList(int pageSize, int pageIndex, string sort, string sortOrder, string searchText, string componentid)
        {
            int Totalcount = 0;

            string filter = " Inventory>0 ";
            if (!string.IsNullOrEmpty(searchText))
            {
                filter += " and (Name like '%" + searchText + "%' or Type like '%" + searchText + "%')";
            }
            string orderby = sort + " " + sortOrder;
            
            if (string.IsNullOrEmpty(componentid))
            {
                componentid = "";
            }
            var datalist = _ISws_AccessoriesEquService.GetAccessoryTable(componentid, pageIndex, pageSize, orderby, filter, ref Totalcount);
            return Json(new { total = Totalcount, rows = Newtonsoft.Json.JsonConvert.SerializeObject(datalist) });
        }
        //设备选择
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult SelectDevice()
        {
            return View();
        }
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult LoadDeviceList(int pageSize, int pageIndex, string sort, string sortOrder, string searchText)
        {
            int Totalcount = 0;
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = _ISys_UserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();

            string filter = "";
            if (!string.IsNullOrEmpty(searchText))
            {
                filter = "(DeviceName like '%" + searchText + "%' or DeviceNum like '%" + searchText + "%')";
            }
            string orderby = sort + " " + sortOrder;

            var datalist = _ISws_AccessoriesEquService.GetAcce_DeviceTable(user.IsAdmin,user.UserId, pageIndex, pageSize, orderby, filter, ref Totalcount);
            return Json(new { total = Totalcount, rows = Newtonsoft.Json.JsonConvert.SerializeObject(datalist) });
        }
        //表单提交
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult SetAccessEqu(SwsAccessoriesEqu a)
        {
            if (!string.IsNullOrEmpty(a.AccessoriesNo))
            {
                var hasmodel = _ISws_AccessoriesEquService.Query<SwsAccessoriesEqu>(r => r.AccessoriesNo == a.AccessoriesNo && r.AccessoriesId == a.AccessoriesId && r.Id != a.Id).FirstOrDefault();
                if (hasmodel != null)
                {
                    return Content("已经存在相同的器件编号");
                }
            }
           
            if (a.Id == 0)//新增
            {
                //判断是否够用
                var sws_accessory = _ISws_AccessoriesService.Query<SwsAccessories>(r => r.Id == a.AccessoriesId).FirstOrDefault();
                if (sws_accessory.Inventory >= a.Quantity)
                {
                    a.MaintenanceDate = a.DeliveryDate;
                    a.Id = ConvertDateTimeInt(DateTime.Now);
                    if (_ISws_AccessoriesEquService.AddAccessoryEqu(a, sws_accessory) > 0)
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
                    return Content("" + sws_accessory.Name + "库存量不足");
                }
            }
            else//修改
            {
                //判断库存量是否够用
                var sws_accessory = _ISws_AccessoriesService.Query<SwsAccessories>(r => r.Id == a.AccessoriesId).FirstOrDefault();
                var Acce_Equ = _ISws_AccessoriesEquService.Query<SwsAccessoriesEqu>(r => r.Id == a.Id).AsNoTracking().FirstOrDefault();
                if ((sws_accessory.Inventory + Acce_Equ.Quantity) >= a.Quantity)
                {
                    //判断是否有保养记录
                    var maintanRecord = _ISwsAccessMaintenService.Query<SwsAccessoriesMaintenance>(r => r.AccessoriesDetailId == a.Id).ToList();
                    if (maintanRecord.Count == 0)
                    {
                        a.MaintenanceDate = a.DeliveryDate;
                    }
                    if (_ISws_AccessoriesEquService.EditeAccessoryEqu(a, sws_accessory, Acce_Equ.Quantity) > 0)
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
                    return Content("" + sws_accessory.Name + "库存量不足");
                }

            }

        }
        //删除记录
        public IActionResult DeleteAcc_Equ(string ids)
        {
            List<long> Acce_Equids = new List<string>(ids.Split(",")).ConvertAll(r => long.Parse(r));
            if (_ISws_AccessoriesEquService.DeleteAccessoryEqu(Acce_Equids) > 0)
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }
        }
        #endregion

        //导出器件模板
        //导出关键数据
        [TypeFilter(typeof(IgonreActionFilter))]
        public ActionResult AccessImportExport(string SearchText, string order, string sort, string time, string beginDate, string endDate)
        {
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = _ISys_UserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();

            int Totalcount = 0;
            string ordertems = sort + " " + order;
            string filter = "1=1";


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

            var datalist = _ISws_AccessoriesEquService.QueryAccessoryEquTable(user.IsAdmin, user.UserId, 0, 0, ordertems, filter, ref Totalcount);

            //数据导出
            HSSFWorkbook workbook = new HSSFWorkbook();
            ISheet sheet = workbook.CreateSheet();
            #region 内容样式
            //IFont font1 = workbook.CreateFont(); //创建一个字体样式对象
            //font1.FontName = "Microsoft YaHei"; //和excel里面的字体对应
            //                                    //font1.Boldweight = short.MaxValue;//字体加粗
            //font1.FontHeightInPoints = 12;//字体大小
            //ICellStyle style = workbook.CreateCellStyle();//创建样式对象
            //style.BorderBottom = BorderStyle.Thin;
            //style.BorderLeft = BorderStyle.Thin;
            //style.BorderRight = BorderStyle.Thin;
            //style.BorderTop = BorderStyle.Thin;
            //style.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
            //style.VerticalAlignment = VerticalAlignment.Center;
            //style.SetFont(font1); //将字体样式赋给样式对象 
            #endregion

            #region 标题样式
            //IFont font = workbook.CreateFont(); //创建一个字体样式对象
            //font.FontName = "Microsoft YaHei"; //和excel里面的字体对应
            //font.Boldweight = (short)FontBoldWeight.Bold;//字体加粗
            //font.FontHeightInPoints = 12;//字体大小
            //ICellStyle style1 = workbook.CreateCellStyle();//创建样式对象
            //style1.BorderBottom = BorderStyle.Thin;
            //style1.BorderLeft = BorderStyle.Thin;
            //style1.BorderRight = BorderStyle.Thin;
            //style1.BorderTop = BorderStyle.Thin;
            //style1.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
            //style1.VerticalAlignment = VerticalAlignment.Center;
            //style1.SetFont(font); //将字体样式赋给样式对象 
            #endregion
           

            //填充表头
            IRow dataRow = sheet.CreateRow(0);
            dataRow.Height = 20 * 20;
            dataRow.CreateCell(0).SetCellValue("器件ID");
            dataRow.CreateCell(1).SetCellValue("器件名称");
            dataRow.CreateCell(2).SetCellValue("设备名称");
            dataRow.CreateCell(3).SetCellValue("保养时间");
            dataRow.CreateCell(4).SetCellValue("保养费用");
            dataRow.CreateCell(5).SetCellValue("内容");
            dataRow.CreateCell(6).SetCellValue("保养人");
            
            for (int i = 0; i < 7; i++)
            {
                sheet.SetColumnWidth(i, 15 * 256);
            }
            //for (int s = 0; s < 3; s++)
            //{
            //    sheet.SetColumnWidth(s, 25 * 256);
            //    dataRow.Cells[s].CellStyle = style1;
            //}
            int j = 1;
            foreach (var item in datalist)
            {
                dataRow = sheet.CreateRow(j);
                //dataRow.Height = 100 * 4;
                dataRow.CreateCell(0).SetCellValue(item.ID?.ToString());
                dataRow.CreateCell(1).SetCellValue(item.Name?.ToString());
                dataRow.CreateCell(2).SetCellValue(item.DeviceName?.ToString());
                j++;
            }

            //for (int ro = 1; ro < list.Count + 1; ro++)
            //{
            //    sheet.GetRow(ro).Height = 20 * 20;
            //    for (int s = 0; s < 3; s++)
            //    {
            //        sheet.GetRow(ro).Cells[s].CellStyle = style;
            //    }
            //}
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            workbook.Write(ms);
            workbook = null;
            ms.Close();
            ms.Dispose();
            return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "器件保养信息模板.xls");
        }
        //时间戳
        public static long ConvertDateTimeInt(System.DateTime time)
        {
            System.DateTime startTime = System.TimeZoneInfo.ConvertTimeToUtc(new System.DateTime(1970, 1, 1));
            return (long)(time - startTime).TotalMilliseconds;
        }
    }
}