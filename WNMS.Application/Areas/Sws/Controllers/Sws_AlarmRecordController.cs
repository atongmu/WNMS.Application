using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using WNMS.Application.Utility;
using WNMS.Application.Utility.Filters;
using WNMS.IService;
using WNMS.Model.DataModels;

namespace WNMS.Application.Areas.Sws.Controllers
{
    [Area("Sws")]
    public class Sws_AlarmRecordController : Controller
    {
        private readonly ISws_EventHistoryService _sws_EventHistoryService;
        private readonly ISws_StationService _sws_StationService;
        private readonly ISws_DeviceInfo01Service _sws_DeviceInfo01Service;
        private readonly ISysUserService _sysUserService;

        public object Session { get; private set; }

        public Sws_AlarmRecordController(ISws_EventHistoryService sws_EventHistoryService,
            ISws_StationService sws_StationService,
            ISws_DeviceInfo01Service sws_DeviceInfo01Service,
            ISysUserService sysUserService)
        {
            _sws_EventHistoryService = sws_EventHistoryService;
            _sws_StationService = sws_StationService;
            _sws_DeviceInfo01Service = sws_DeviceInfo01Service;
            _sysUserService = sysUserService;
        }
        public IActionResult Index()
        {
            ViewBag.beginDate = string.Format("{0:yyyy-MM-dd}", DateTime.Now.AddMonths(-1));
            ViewBag.endDate = string.Format("{0:yyyy-MM-dd}", DateTime.Now);
            return View();
        }
        //查询table
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult LoadEventData(int? pageindex, int? pagesize, string beginTime, string endTime, string eventLevel, string stationType, string eventType, string searchtext, string sortName = "EventTime", string order = "desc", string type = "1")
        {
            string filer = "where 1=1 ";
            if (type == "1")
            {
                beginTime = DateTime.Now.AddHours(-1).ToString("yyyy-MM-dd HH:mm:ss");
                endTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }
            else if (type == "2")
            {
                beginTime = DateTime.Now.AddHours(-4).ToString("yyyy-MM-dd HH:mm:ss");
                endTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }
            else if (type == "3")
            {
                beginTime = DateTime.Now.AddHours(-12).ToString("yyyy-MM-dd HH:mm:ss");
                endTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }
            else if (type == "4")
            {
                beginTime = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd HH:mm:ss");
                endTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }
            else {
                
            }
            if (!string.IsNullOrEmpty(eventLevel))
            {
                filer += "    and EventLevel in (" + eventLevel + ")";
            }
            if (!string.IsNullOrEmpty(eventType))
            {
                filer += "  and   EventType in (" + eventType + ")";
            }
            if (!string.IsNullOrEmpty(beginTime) && !string.IsNullOrEmpty(endTime))
            {
                filer += "    and EventTime >'" + beginTime + "' and EventTime < '"+ endTime + "'";
            }
            if (!string.IsNullOrEmpty(searchtext) && type != "4")
            {
                filer += " and (StationName like '%" + searchtext + "%' or EventMessage like '%" + searchtext + "%')"; 
            }
            int Totalcount = 0;
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var sysUser = _sysUserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
            int size = pagesize ?? 10;
            int index = pageindex ?? 1;
            var dataList = _sws_EventHistoryService.QueryEventTable(sysUser.IsAdmin, index, size, beginTime, endTime, filer, sortName, order, sysUser.UserId, ref Totalcount).ToList();
            //string json = JsonConvert.SerializeObject(dataList);           
            
            //HttpContext.Session.SetString("DataList", "12344567");

            return Json(new { total = Totalcount, rows = dataList.ToList() });
        }

        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult LoadData(int pageIndex, int pageSize, string eventLevel, string stationType, string eventType, string beginTime, string endTime)
        {
            return Content("");
        }
        #region 导出报警历史数据
        [TypeFilter(typeof(IgonreActionFilter))]
        //public ActionResult ExportEventHistoryData(string equID, string beginDate, string endDate, int templateID)
        public ActionResult ExportEventHistoryData( string beginTime, string endTime, string eventLevel, string stationType, string eventType, string searchtext,  int dateid )
        {
            string sortName = "EventTime"; 
            string order = "asc";
            //变量处理
            //long equipmentID = Convert.ToInt64(equID);
            //string year = Convert.ToDateTime(beginTime).Year.ToString();

            string filer = "where 1=1 ";
            if (dateid == 1)
            {
                beginTime = DateTime.Now.AddHours(-1).ToString("yyyy-MM-dd HH:mm:ss");
                endTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }
            else if (dateid == 2)
            {
                beginTime = DateTime.Now.AddHours(-4).ToString("yyyy-MM-dd HH:mm:ss");
                endTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }
            else if (dateid == 3)
            {
                beginTime = DateTime.Now.AddHours(-12).ToString("yyyy-MM-dd HH:mm:ss");
                endTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }
            else if (dateid == 4)
            {
                beginTime = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd HH:mm:ss");
                endTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }
            else
            {

            }
            if (!string.IsNullOrEmpty(eventLevel))
            {
                filer += "    and EventLevel in (" + eventLevel + ")";
            }
            if (!string.IsNullOrEmpty(eventType))
            {
                filer += "  and   EventType in (" + eventType + ")";
            }
            if (!string.IsNullOrEmpty(beginTime) && !string.IsNullOrEmpty(endTime))
            {
                filer += "    and EventTime >'" + beginTime + "' and EventTime < '" + endTime + "'";
            }
            if (!string.IsNullOrEmpty(searchtext) && dateid != 4)
            {
                filer += " and (StationName like '%" + searchtext + "%' or EventMessage like '%" + searchtext + "%')";
            }
            int Totalcount = 0;
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var sysUser = _sysUserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
            int size =  100;
            int index =  0;
            var dataList = _sws_EventHistoryService.QueryEventTable(sysUser.IsAdmin, index, size, beginTime, endTime, filer, sortName, order, sysUser.UserId, ref Totalcount).ToList();

           
           
            //数据导出
            HSSFWorkbook workbook = new HSSFWorkbook();
            ISheet sheet = workbook.CreateSheet();

            #region 内容样式
            IFont font1 = workbook.CreateFont(); //创建一个字体样式对象
            font1.FontName = "Microsoft YaHei"; //和excel里面的字体对应
                                                //font1.Boldweight = short.MaxValue;//字体加粗
            font1.FontHeightInPoints = 12;//字体大小
            ICellStyle style = workbook.CreateCellStyle();//创建样式对象
            style.BorderBottom = BorderStyle.Thin;
            style.BorderLeft = BorderStyle.Thin;
            style.BorderRight = BorderStyle.Thin;
            style.BorderTop = BorderStyle.Thin;
            style.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
            style.VerticalAlignment = VerticalAlignment.Center;
            style.SetFont(font1); //将字体样式赋给样式对象 
            #endregion

            #region 标题样式
            IFont font = workbook.CreateFont(); //创建一个字体样式对象
            font.FontName = "Microsoft YaHei"; //和excel里面的字体对应
            font.Boldweight = (short)FontBoldWeight.Bold;//字体加粗
            font.FontHeightInPoints = 12;//字体大小
            ICellStyle style1 = workbook.CreateCellStyle();//创建样式对象
            style1.BorderBottom = BorderStyle.Thin;
            style1.BorderLeft = BorderStyle.Thin;
            style1.BorderRight = BorderStyle.Thin;
            style1.BorderTop = BorderStyle.Thin;
            style1.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
            style1.VerticalAlignment = VerticalAlignment.Center;
            style1.SetFont(font); //将字体样式赋给样式对象 
            #endregion

            //填充表头
            IRow dataRow = sheet.CreateRow(0);
            dataRow.CreateCell(0).SetCellValue("序号");
            dataRow.CreateCell(1).SetCellValue("泵房");
            dataRow.CreateCell(2).SetCellValue("设备名称");
            dataRow.CreateCell(3).SetCellValue("状态");
            dataRow.CreateCell(4).SetCellValue("报警级别");
            dataRow.CreateCell(5).SetCellValue("报警信息");
            dataRow.CreateCell(6).SetCellValue("报警时间");
            dataRow.CreateCell(7).SetCellValue("结束时间");
            dataRow.CreateCell(8).SetCellValue("持续时间");
            dataRow.CreateCell(9).SetCellValue("报警值");
            dataRow.CreateCell(10).SetCellValue("限定值");
            dataRow.Height = 20 * 20;
            //int i = 1;
            //foreach (var item in dataInfo)
            //{
            //    dataRow.CreateCell(i).SetCellValue(item.Cnname);
            //    i++;
            //}
            for (int s = 0; s < 11 ; s++)
            {
                sheet.SetColumnWidth(s, 25 * 256);
                dataRow.Cells[s].CellStyle = style1;
            }
           

            int j = 1;
            foreach (var item in dataList.ToList())
            {
                dataRow = sheet.CreateRow(j);
                dataRow.CreateCell(0).SetCellValue(item.rownumber);
                dataRow.CreateCell(1).SetCellValue(item.StationName);
                dataRow.CreateCell(2).SetCellValue(item.DeviceName);
                dataRow.CreateCell(3).SetCellValue(item.State1);
                dataRow.CreateCell(4).SetCellValue(item.ItemName);
                dataRow.CreateCell(5).SetCellValue(item.EventMessage);
                dataRow.CreateCell(6).SetCellValue(Convert.ToDateTime(item.EventTime).ToString("yyyy-MM-dd HH:mm:ss"));
                dataRow.CreateCell(7).SetCellValue(Convert.ToDateTime(item.EndDate1).ToString("yyyy-MM-dd HH:mm:ss"));
                dataRow.CreateCell(8).SetCellValue(item.diffm);
                dataRow.CreateCell(9).SetCellValue(item.CurrentValue);
                dataRow.CreateCell(10).SetCellValue(item.LimitValue);

               
                   
                    sheet.GetRow(j).Height = 20 * 20;
                    for (int s = 0; s < 11; s++)
                    {
                        sheet.GetRow(j).Cells[s].CellStyle = style;
                    }
               
                j++;
            }

           
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            workbook.Write(ms);
            workbook = null;
            ms.Close();
            ms.Dispose();
            return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",   "报警历史数据" + beginTime.Substring(0, 10) + ".xls");
            //return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",   "历史数据" +   ".xls");
        }

        
        #endregion

    }
}