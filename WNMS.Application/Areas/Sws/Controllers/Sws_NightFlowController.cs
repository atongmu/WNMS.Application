using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using WNMS.Application.Utility;
using WNMS.Application.Utility.Filters;
using WNMS.IService;
using WNMS.Model.DataModels;
namespace WNMS.Application.Areas.Sws.Controllers
{
    [Area("Sws")]
    public class Sws_NightFlowController : Controller
    {
        private ISysUserService _userService = null;
        private ISws_StationService _StationService = null;
        public Sws_NightFlowController(ISws_StationService sws_StationService,ISysUserService sysUserService)
        {
            _userService = sysUserService;
            _StationService = sws_StationService;
        }
        public IActionResult Index()
        {
            ViewBag.treenodes = new HtmlString(GetStationTree(""));
            ViewBag.BeginTimeDay = DateTime.Now.ToString("yyyy-MM-dd");
            ViewBag.BeginTimeMonth = DateTime.Now.ToString("yyyy-MM");
            ViewBag.BeginTimeYear = DateTime.Now.ToString("yyyy");
            ViewBag.BeginTime = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd");
            ViewBag.EndTime = DateTime.Now.ToString("yyyy-MM-dd");
            return View();
        }
        #region 获取表格数据
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult GetTable_Father(int pageindex,int pagesize,string stationids,string priodTimeBegin,string priodTimeEnd,string time)
        {
            var innertype = 1;
            var thisBegin = time + " " + priodTimeBegin;
            var thisEnd= time + " " + priodTimeEnd;
            var beginTime = Convert.ToDateTime(thisBegin);
            var pastBegin = beginTime.AddDays(-1).ToString();
            var endTime = Convert.ToDateTime(thisEnd);
            var pastEnd= endTime.AddDays(-1).ToString();
            //表头时间
            #region 表头
            List<string> tabledate = new List<string>();
            List<string> tableheads = new List<string>();
            tableheads.Add("StationName");
            string dates = "";
            while (beginTime<= endTime)
            {
                var hour = beginTime.Hour.ToString();
                tabledate.Add(hour + "时");
                tableheads.Add(hour);
                dates +="["+ hour + "],";

                beginTime= beginTime.AddHours(1);
            }
            if (dates != "")
            {
                dates = dates.Substring(0, dates.Length - 1);
            }
            tableheads.Add("maxFlowCon");
            tableheads.Add("maxTime");
            tableheads.Add("minFlowCon");
            tableheads.Add("minTime");
            tableheads.Add("pastAverge");
            tableheads.Add("thisAverge");
            tableheads.Add("circleRate");
            #endregion
            int totalcount = 0;
            var datalist = _StationService.GetNightFlow_Day(innertype, stationids,pageindex,pagesize, pastBegin, pastEnd,thisBegin,thisEnd,dates,ref totalcount);
            ViewBag.tableheads = tableheads;
            ViewBag.tabledate = tabledate;
            PartialView("_nightFlow_father", datalist);
            var dataTable = ViewToString.RenderPartialViewToString(this, "_nightFlow_father");
            
            return Json(new
            {
                dataTable= dataTable,
                datalist = datalist,
                total = totalcount,
                pageSize=pagesize,
                pageIndex=pageindex
            });
        }

        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult GetTable_Father_Month(int pageindex, int pagesize, string stationids, string priodTimeBegin, string priodTimeEnd, string time)
        {
            string time2 = time + "-01";
            DateTime begindate = Convert.ToDateTime(time2);
            var time3 = begindate.AddMonths(1);
            if (time3 > DateTime.Now)
            {
                time3 = DateTime.Now.Date.AddDays(1);
            }
            var time1 = begindate.AddMonths(-1);

            var minhour =int.Parse(priodTimeBegin.Split(':')[0]);
            var maxhour = int.Parse(priodTimeEnd.Split(':')[0]);
            #region 表头
            List<string> tabledate = new List<string>();
            List<string> tableheads = new List<string>();
            tableheads.Add("StationName");
            string dates = "";
            while (begindate < time3)
            {
                var day = begindate.Day.ToString();
                tabledate.Add(day + "日");
                tableheads.Add(day);
                dates += "[" + day + "],";

                begindate = begindate.AddDays(1);
            }
            if (dates != "")
            {
                dates = dates.Substring(0, dates.Length - 1);
            }
            tableheads.Add("maxFlowCon");
            tableheads.Add("maxTime");
            tableheads.Add("minFlowCon");
            tableheads.Add("minTime");
            tableheads.Add("pastAverge");
            tableheads.Add("thisAverge");
            tableheads.Add("circleRate");
            #endregion
            int totalcount = 0;
            var innertype = 1;
            var datalist = _StationService.GetNightFlow_Month(innertype, stationids, pageindex, pagesize, time1.ToString(), time2, time3.ToString(), minhour,maxhour, dates, ref totalcount);
            ViewBag.tableheads = tableheads;
            ViewBag.tabledate = tabledate;
            PartialView("_nightFlow_fatherMonth", datalist);
            var dataTable = ViewToString.RenderPartialViewToString(this, "_nightFlow_fatherMonth");

            return Json(new
            {
                dataTable = dataTable,
                total = totalcount,
                pageSize = pagesize,
                pageIndex = pageindex
            });
        }

        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult GetTable_Father_Year(int pageindex, int pagesize, string stationids, string priodTimeBegin, string priodTimeEnd, string time)
        {
            string time2 = time + "-01-01";
            DateTime begindate = Convert.ToDateTime(time2);
            var time3 = begindate.AddYears(1);
            if (time3 > DateTime.Now)
            {
                time3 = DateTime.Now.Date.AddDays(1);
            }
            var time1 = begindate.AddYears(-1);

            var minhour = int.Parse(priodTimeBegin.Split(':')[0]);
            var maxhour = int.Parse(priodTimeEnd.Split(':')[0]);
            #region 表头
            List<string> tabledate = new List<string>();
            List<string> tableheads = new List<string>();
            tableheads.Add("StationName");
            string dates = "";
            while (begindate < time3)
            {
                var month = begindate.Month.ToString();
                tabledate.Add(month + "月");
                tableheads.Add(month);
                dates += "[" + month + "],";

                begindate = begindate.AddMonths(1);
            }
            if (dates != "")
            {
                dates = dates.Substring(0, dates.Length - 1);
            }
            tableheads.Add("maxFlowCon");
            tableheads.Add("maxTime");
            tableheads.Add("minFlowCon");
            tableheads.Add("minTime");
            tableheads.Add("pastAverge");
            tableheads.Add("thisAverge");
            tableheads.Add("circleRate");
            #endregion
            int totalcount = 0;
            var innertype = 1;
            var datalist = _StationService.GetNightFlow_Year(innertype, stationids, pageindex, pagesize, time1.ToString(), time2, time3.ToString(), minhour, maxhour, dates, ref totalcount);
            ViewBag.tableheads = tableheads;
            ViewBag.tabledate = tabledate;
            PartialView("_nightFlow_fatherYear", datalist);
            var dataTable = ViewToString.RenderPartialViewToString(this, "_nightFlow_fatherYear");

            return Json(new
            {
                dataTable = dataTable,
                total = totalcount,
                pageSize = pagesize,
                pageIndex = pageindex
            });
        }
        #endregion
        #region 获取图标信息
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult GetEcharts_Father(int pageindex, int pagesize, string stationids, string priodTimeBegin, string priodTimeEnd, string time,int hasTable)
        {
            var innertype = 1;
            var thisBegin = time + " " + priodTimeBegin;
            var thisEnd = time + " " + priodTimeEnd;
            var beginTime = Convert.ToDateTime(thisBegin);
            var pastBegin = beginTime.AddDays(-1).ToString();
            var endTime = Convert.ToDateTime(thisEnd);
            var pastEnd = endTime.AddDays(-1).ToString();
            
            //表头时间
            #region 表头
            List<string> tabledate = new List<string>();
            List<string> tableheads = new List<string>();
            tableheads.Add("StationName");
            string dates = "";
            while (beginTime <= endTime)
            {
                var hour = beginTime.Hour.ToString();
                tabledate.Add(hour + "时");
                tableheads.Add(hour);
                dates += "[" + hour + "],";

                beginTime = beginTime.AddHours(1);
            }
            if (dates != "")
            {
                dates = dates.Substring(0, dates.Length - 1);
            }
            tableheads.Add("maxFlowCon");
            tableheads.Add("maxTime");
            tableheads.Add("minFlowCon");
            tableheads.Add("minTime");
            tableheads.Add("pastAverge");
            tableheads.Add("thisAverge");
            tableheads.Add("circleRate");
            #endregion
            int totalcount = 0;
            var datalist = _StationService.GetNightFlow_Day(innertype, stationids, pageindex, pagesize, pastBegin, pastEnd, thisBegin, thisEnd, dates, ref totalcount);
            ViewBag.tableheads = tableheads;
            ViewBag.tabledate = tabledate;
            PartialView("_nightFlow_father", datalist);
            var dataTable = ViewToString.RenderPartialViewToString(this, "_nightFlow_father");

            return Json(new
            {
                dataTable = dataTable,
                datalist= datalist,
                total = totalcount,
                hasTable = hasTable,
                pageSize = pagesize,
                pageIndex = pageindex
            });
        }
        #endregion
        #region 导出
        //日报导出
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult ExportTable(string stationids, string priodTimeBegin, string priodTimeEnd, string time)
        {
            var innertype = 1;
            var thisBegin = time + " " + priodTimeBegin;
            var thisEnd = time + " " + priodTimeEnd;
            var beginTime = Convert.ToDateTime(thisBegin);
            var pastBegin = beginTime.AddDays(-1).ToString();
            var endTime = Convert.ToDateTime(thisEnd);
            var pastEnd = endTime.AddDays(-1).ToString();
            //表头时间
            #region 表头
            List<string> tabledate = new List<string>();
            List<string> tableheads = new List<string>();
            tableheads.Add("StationName");
            string dates = "";
            while (beginTime <= endTime)
            {
                var hour = beginTime.Hour.ToString();
                tabledate.Add(hour + "时");
                tableheads.Add(hour);
                dates += "[" + hour + "],";

                beginTime = beginTime.AddHours(1);
            }
            if (dates != "")
            {
                dates = dates.Substring(0, dates.Length - 1);
            }
            tableheads.Add("maxFlowCon");
            tableheads.Add("maxTime");
            tableheads.Add("minFlowCon");
            tableheads.Add("minTime");
            tableheads.Add("pastAverge");
            tableheads.Add("thisAverge");
            tableheads.Add("circleRate");
            #endregion
            var list = _StationService.ExportNightFlow_Day(innertype, stationids, pastBegin, pastEnd, thisBegin, thisEnd, dates);
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
            #region 填充表头
            IRow dataRow = sheet.CreateRow(0);
            dataRow.CreateCell(0).SetCellValue("泵房名称");
            List<CellRangeAddress> regionList = new List<CellRangeAddress>();
            CellRangeAddress region = new CellRangeAddress(0, 1, 0, 0);
            sheet.AddMergedRegion(region);
            regionList.Add(region);
            for (var i = 0; i < tabledate.Count; i++)
            {

                dataRow.CreateCell(i+1).SetCellValue(tabledate[i]);
                 region = new CellRangeAddress(0,1, i+1, i+1);
                sheet.AddMergedRegion(region);
                regionList.Add(region);
            }
            dataRow.CreateCell(tabledate.Count+1).SetCellValue("最大值");
            region = new CellRangeAddress(0, 0, tabledate.Count + 1, tabledate.Count + 2);
            sheet.AddMergedRegion(region);
            regionList.Add(region);
            dataRow.CreateCell(tabledate.Count + 3).SetCellValue("最小值");
            region = new CellRangeAddress(0, 0, tabledate.Count + 3, tabledate.Count + 4);
            sheet.AddMergedRegion(region);
            regionList.Add(region);
            dataRow.CreateCell(tabledate.Count+5).SetCellValue("平均值");
            region = new CellRangeAddress(0, 0, tabledate.Count + 5, tabledate.Count + 7);
            sheet.AddMergedRegion(region);
            regionList.Add(region);

            IRow dataRow1 = sheet.CreateRow(1);
            dataRow1.CreateCell(tabledate.Count + 1).SetCellValue("流量");
            dataRow1.CreateCell(tabledate.Count + 2).SetCellValue("时间");
            dataRow1.CreateCell(tabledate.Count + 3).SetCellValue("流量");
            dataRow1.CreateCell(tabledate.Count + 4).SetCellValue("时间");
            dataRow1.CreateCell(tabledate.Count + 5).SetCellValue("昨日");
            dataRow1.CreateCell(tabledate.Count + 6).SetCellValue("今日");
            dataRow1.CreateCell(tabledate.Count + 7).SetCellValue("同比");
            #endregion
            #region 表头样式
            for (var i = 0; i <7; i++)
            {
                //sheet.SetColumnWidth(i, 25 * 256);
                dataRow1.Cells[i].CellStyle = style1;

            }
            for (var i = 0; i < tabledate.Count + 4; i++)
            {
                dataRow.Cells[i].CellStyle = style1;
            }
            foreach (var item in regionList)
            {
                ((HSSFSheet)sheet).SetEnclosedBorderOfRegion(item, BorderStyle.Thin, NPOI.HSSF.Util.HSSFColor.Black.Index);
            }
            #endregion
            #region 内容
            int j = 2;
            if (list.Count() > 0)
            {
                foreach (var item in list)
                {
                    var itemjson = JsonConvert.SerializeObject(item);
                    var dy = JsonConvert.DeserializeObject(itemjson);
                    var ii = 0;
                    dataRow = sheet.CreateRow(j);
                    dataRow.Height = 100 * 4;
                    foreach (var h in tableheads)
                    {

                        
                        var data = dy[h];
                        if (h == "StationName")
                        {
                            dataRow.CreateCell(ii).SetCellValue(data.ToString());
                   
                        }
                        else if (h == "maxTime" || h == "minTime")
                        {
                            if (data.ToString() == "")
                            {
                                dataRow.CreateCell(ii).SetCellValue("--");
                        
                            }
                            else
                            {
                                dataRow.CreateCell(ii).SetCellValue(data.ToString() + "时");
                               
                            }
                        }
                        else
                        {
                            if (data.ToString() == "")
                            {
                                dataRow.CreateCell(ii).SetCellValue("--");
                            }
                            else
                            {
                                dataRow.CreateCell(ii).SetCellValue(Math.Round(Convert.ToDouble(data), 2).ToString());
                        
                            }

                        }
                        ii++;
                    }

                    j++;
                }

                for (int ro = 2; ro < list.Count() + 2; ro++)
                {
                    sheet.GetRow(ro).Height = 20 * 20;
                    for (int s = 0; s < tableheads.Count; s++)
                    {
                        sheet.GetRow(ro).Cells[s].CellStyle = style;
                    }
                }
            }
            #endregion
            
            var filename = time + " " + priodTimeBegin + "至" + priodTimeEnd + "夜间流量";
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            workbook.Write(ms);
            workbook = null;
            ms.Close();
            ms.Dispose();
            return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "" + filename + ".xls");
        }
        //月报导出
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult ExportTable_Month(string stationids, string priodTimeBegin, string priodTimeEnd, string time)
        {
            string time2 = time + "-01";
            DateTime begindate = Convert.ToDateTime(time2);
            var time3 = begindate.AddMonths(1);
            if (time3 > DateTime.Now)
            {
                time3 = DateTime.Now.Date.AddDays(1);
            }
            var time1 = begindate.AddMonths(-1);

            var minhour = int.Parse(priodTimeBegin.Split(':')[0]);
            var maxhour = int.Parse(priodTimeEnd.Split(':')[0]);
            #region 表头
            List<string> tabledate = new List<string>();
            List<string> tableheads = new List<string>();
            tableheads.Add("StationName");
            string dates = "";
            while (begindate < time3)
            {
                var day = begindate.Day.ToString();
                tabledate.Add(day + "日");
                tableheads.Add(day);
                dates += "[" + day + "],";

                begindate = begindate.AddDays(1);
            }
            if (dates != "")
            {
                dates = dates.Substring(0, dates.Length - 1);
            }
            tableheads.Add("maxFlowCon");
            tableheads.Add("maxTime");
            tableheads.Add("minFlowCon");
            tableheads.Add("minTime");
            tableheads.Add("pastAverge");
            tableheads.Add("thisAverge");
            tableheads.Add("circleRate");
            #endregion
            var innertype = 1;
            var list = _StationService.ExportNightFlow_Month(innertype, stationids,time1.ToString(), time2, time3.ToString(), minhour, maxhour, dates);

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
            #region 填充表头
            IRow dataRow = sheet.CreateRow(0);
            dataRow.CreateCell(0).SetCellValue("泵房名称");
            List<CellRangeAddress> regionList = new List<CellRangeAddress>();
            CellRangeAddress region = new CellRangeAddress(0, 1, 0, 0);
            sheet.AddMergedRegion(region);
            regionList.Add(region);
            for (var i = 0; i < tabledate.Count; i++)
            {

                dataRow.CreateCell(i + 1).SetCellValue(tabledate[i]);
                region = new CellRangeAddress(0, 1, i + 1, i + 1);
                sheet.AddMergedRegion(region);
                regionList.Add(region);
            }
            dataRow.CreateCell(tabledate.Count + 1).SetCellValue("最大值");
            region = new CellRangeAddress(0, 0, tabledate.Count + 1, tabledate.Count + 2);
            sheet.AddMergedRegion(region);
            regionList.Add(region);
            dataRow.CreateCell(tabledate.Count + 3).SetCellValue("最小值");
            region = new CellRangeAddress(0, 0, tabledate.Count + 3, tabledate.Count + 4);
            sheet.AddMergedRegion(region);
            regionList.Add(region);
            dataRow.CreateCell(tabledate.Count + 5).SetCellValue("平均值");
            region = new CellRangeAddress(0, 0, tabledate.Count + 5, tabledate.Count + 7);
            sheet.AddMergedRegion(region);
            regionList.Add(region);

            IRow dataRow1 = sheet.CreateRow(1);
            dataRow1.CreateCell(tabledate.Count + 1).SetCellValue("流量");
            dataRow1.CreateCell(tabledate.Count + 2).SetCellValue("时间");
            dataRow1.CreateCell(tabledate.Count + 3).SetCellValue("流量");
            dataRow1.CreateCell(tabledate.Count + 4).SetCellValue("时间");
            dataRow1.CreateCell(tabledate.Count + 5).SetCellValue("上月");
            dataRow1.CreateCell(tabledate.Count + 6).SetCellValue("本月");
            dataRow1.CreateCell(tabledate.Count + 7).SetCellValue("同比");
            #endregion
            #region 表头样式
            for (var i = 0; i < 7; i++)
            {
                //sheet.SetColumnWidth(i, 25 * 256);
                dataRow1.Cells[i].CellStyle = style1;

            }
            for (var i = 0; i < tabledate.Count + 4; i++)
            {
                dataRow.Cells[i].CellStyle = style1;
            }
            foreach (var item in regionList)
            {
                ((HSSFSheet)sheet).SetEnclosedBorderOfRegion(item, BorderStyle.Thin, NPOI.HSSF.Util.HSSFColor.Black.Index);
            }
            #endregion
            #region 内容
            int j = 2;
            if (list.Count() > 0)
            {
                foreach (var item in list)
                {
                    var itemjson = JsonConvert.SerializeObject(item);
                    var dy = JsonConvert.DeserializeObject(itemjson);
                    var ii = 0;
                    dataRow = sheet.CreateRow(j);
                    dataRow.Height = 100 * 4;
                    foreach (var h in tableheads)
                    {


                        var data = dy[h];
                        if (h == "StationName")
                        {
                            dataRow.CreateCell(ii).SetCellValue(data.ToString());

                        }
                        else if (h == "maxTime" || h == "minTime")
                        {
                            if (data.ToString() == "")
                            {
                                dataRow.CreateCell(ii).SetCellValue("--");

                            }
                            else
                            {
                                dataRow.CreateCell(ii).SetCellValue(data.ToString() + "日");

                            }
                        }
                        else
                        {
                            if (data.ToString() == "")
                            {
                                dataRow.CreateCell(ii).SetCellValue("--");
                            }
                            else
                            {
                                dataRow.CreateCell(ii).SetCellValue(Math.Round(Convert.ToDouble(data), 2).ToString());

                            }

                        }
                        ii++;
                    }

                    j++;
                }

                for (int ro = 2; ro < list.Count() + 2; ro++)
                {
                    sheet.GetRow(ro).Height = 20 * 20;
                    for (int s = 0; s < tableheads.Count; s++)
                    {
                        sheet.GetRow(ro).Cells[s].CellStyle = style;
                    }
                }
            }
            #endregion

            var filename = time + " " + priodTimeBegin + "至" + priodTimeEnd + "夜间流量";
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            workbook.Write(ms);
            workbook = null;
            ms.Close();
            ms.Dispose();
            return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "" + filename + ".xls");
        }
        //年报导出
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult ExportTable_Year(string stationids, string priodTimeBegin, string priodTimeEnd, string time)
        {
            string time2 = time + "-01-01";
            DateTime begindate = Convert.ToDateTime(time2);
            var time3 = begindate.AddYears(1);
            if (time3 > DateTime.Now)
            {
                time3 = DateTime.Now.Date.AddDays(1);
            }
            var time1 = begindate.AddYears(-1);

            var minhour = int.Parse(priodTimeBegin.Split(':')[0]);
            var maxhour = int.Parse(priodTimeEnd.Split(':')[0]);
            #region 表头
            List<string> tabledate = new List<string>();
            List<string> tableheads = new List<string>();
            tableheads.Add("StationName");
            string dates = "";
            while (begindate < time3)
            {
                var month = begindate.Month.ToString();
                tabledate.Add(month + "月");
                tableheads.Add(month);
                dates += "[" + month + "],";

                begindate = begindate.AddMonths(1);
            }
            if (dates != "")
            {
                dates = dates.Substring(0, dates.Length - 1);
            }
            tableheads.Add("maxFlowCon");
            tableheads.Add("maxTime");
            tableheads.Add("minFlowCon");
            tableheads.Add("minTime");
            tableheads.Add("pastAverge");
            tableheads.Add("thisAverge");
            tableheads.Add("circleRate");
            #endregion
          
            var innertype = 1;
            var list = _StationService.ExportNightFlow_Year(innertype, stationids,time1.ToString(), time2, time3.ToString(), minhour, maxhour, dates);
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
            #region 填充表头
            IRow dataRow = sheet.CreateRow(0);
            dataRow.CreateCell(0).SetCellValue("泵房名称");
            List<CellRangeAddress> regionList = new List<CellRangeAddress>();
            CellRangeAddress region = new CellRangeAddress(0, 1, 0, 0);
            sheet.AddMergedRegion(region);
            regionList.Add(region);
            for (var i = 0; i < tabledate.Count; i++)
            {

                dataRow.CreateCell(i + 1).SetCellValue(tabledate[i]);
                region = new CellRangeAddress(0, 1, i + 1, i + 1);
                sheet.AddMergedRegion(region);
                regionList.Add(region);
            }
            dataRow.CreateCell(tabledate.Count + 1).SetCellValue("最大值");
            region = new CellRangeAddress(0, 0, tabledate.Count + 1, tabledate.Count + 2);
            sheet.AddMergedRegion(region);
            regionList.Add(region);
            dataRow.CreateCell(tabledate.Count + 3).SetCellValue("最小值");
            region = new CellRangeAddress(0, 0, tabledate.Count + 3, tabledate.Count + 4);
            sheet.AddMergedRegion(region);
            regionList.Add(region);
            dataRow.CreateCell(tabledate.Count + 5).SetCellValue("平均值");
            region = new CellRangeAddress(0, 0, tabledate.Count + 5, tabledate.Count + 7);
            sheet.AddMergedRegion(region);
            regionList.Add(region);

            IRow dataRow1 = sheet.CreateRow(1);
            dataRow1.CreateCell(tabledate.Count + 1).SetCellValue("流量");
            dataRow1.CreateCell(tabledate.Count + 2).SetCellValue("时间");
            dataRow1.CreateCell(tabledate.Count + 3).SetCellValue("流量");
            dataRow1.CreateCell(tabledate.Count + 4).SetCellValue("时间");
            dataRow1.CreateCell(tabledate.Count + 5).SetCellValue("上年");
            dataRow1.CreateCell(tabledate.Count + 6).SetCellValue("本年");
            dataRow1.CreateCell(tabledate.Count + 7).SetCellValue("同比");
            #endregion
            #region 表头样式
            for (var i = 0; i < 7; i++)
            {
                //sheet.SetColumnWidth(i, 25 * 256);
                dataRow1.Cells[i].CellStyle = style1;

            }
            for (var i = 0; i < tabledate.Count + 4; i++)
            {
                dataRow.Cells[i].CellStyle = style1;
            }
            foreach (var item in regionList)
            {
                ((HSSFSheet)sheet).SetEnclosedBorderOfRegion(item, BorderStyle.Thin, NPOI.HSSF.Util.HSSFColor.Black.Index);
            }
            #endregion
            #region 内容
            int j = 2;
            if (list.Count() > 0)
            {
                foreach (var item in list)
                {
                    var itemjson = JsonConvert.SerializeObject(item);
                    var dy = JsonConvert.DeserializeObject(itemjson);
                    var ii = 0;
                    dataRow = sheet.CreateRow(j);
                    dataRow.Height = 100 * 4;
                    foreach (var h in tableheads)
                    {


                        var data = dy[h];
                        if (h == "StationName")
                        {
                            dataRow.CreateCell(ii).SetCellValue(data.ToString());

                        }
                        else if (h == "maxTime" || h == "minTime")
                        {
                            if (data.ToString() == "")
                            {
                                dataRow.CreateCell(ii).SetCellValue("--");

                            }
                            else
                            {
                                dataRow.CreateCell(ii).SetCellValue(data.ToString() + "月");

                            }
                        }
                        else
                        {
                            if (data.ToString() == "")
                            {
                                dataRow.CreateCell(ii).SetCellValue("--");
                            }
                            else
                            {
                                dataRow.CreateCell(ii).SetCellValue(Math.Round(Convert.ToDouble(data), 2).ToString());

                            }

                        }
                        ii++;
                    }

                    j++;
                }

                for (int ro = 2; ro < list.Count() + 2; ro++)
                {
                    sheet.GetRow(ro).Height = 20 * 20;
                    for (int s = 0; s < tableheads.Count; s++)
                    {
                        sheet.GetRow(ro).Cells[s].CellStyle = style;
                    }
                }
            }
            #endregion

            var filename = time + " " + priodTimeBegin + "至" + priodTimeEnd + "夜间流量";
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            workbook.Write(ms);
            workbook = null;
            ms.Close();
            ms.Dispose();
            return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "" + filename + ".xls");
        }
        #endregion
        #region 树查询
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult SearchTree(string stationName)
        {
            return Content(GetStationTree(stationName));
        }
        private string GetStationTree(string stationname)
        {
            int userID = int.Parse(User.FindFirstValue("UserID"));

            var user = _userService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
            var data = _StationService.GetStation_consumption(user.IsAdmin, userID, stationname, 1);

            if (stationname == "")
            {
                
                var firstnode = new
                {
                    id = 0,
                    pId = -1,
                    name = "<em class='iconfont icon-bengfang'></em>"+"全部",
                    isStation = false,
                    icon = "/images/stationTree.png"
                };

                if (data.Count() > 0)
                {
                    var list = data.Select(r => new
                    {
                        id = (int)r.StationID,
                        pId = 0,
                        name = "<em class='iconfont icon-bengfang'></em>"+(string)r.StationName,
                        isStation = true,
                        icon = "/images/stationTree.png"
                    }).OrderBy(r => r.name).ToList();
                    list.Add(firstnode);
                    return Newtonsoft.Json.JsonConvert.SerializeObject(list);
                }
                else
                {
                    return Newtonsoft.Json.JsonConvert.SerializeObject(firstnode);

                }
            }
            else
            {
                if (data.Count() > 0)
                {
                    var list = data.Select(r => new
                    {
                        id = r.StationID,
                        pId = 0,
                        name = r.StationName,
                        isStation = true,
                        icon = "/images/stationTree.png"
                    }).OrderBy(r => r.name);
                    return Newtonsoft.Json.JsonConvert.SerializeObject(list);
                }
                else
                {
                    return "[]";
                }

            }
          
        }
        #endregion
    }
}