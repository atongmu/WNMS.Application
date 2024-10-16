using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using WNMS.Application.Utility;
using WNMS.Application.Utility.Filters;
using WNMS.IService;
using WNMS.Model.CustomizedClass;
using WNMS.Model.DataModels;

namespace WNMS.Application.Areas.Sws.Controllers
{
    [Area("Sws")]
    public class Sws_WaterReportController : Controller
    {
        #region 属性构造函数
        private IHourQuartZService hourService = null;
        private IDayQuartZService dayService = null;
        private IMonthQuartZService monthService = null;
        private ISysUserService userService = null;
        private ISws_DeviceInfo02Service deviceInfo02Service = null;
        private ISws_RTUInfoService rtuService = null;
        private ISws_UserStationService userstationService = null;
        private ISws_StationService stationService = null;
        public Sws_WaterReportController(IHourQuartZService sws_hourService, ISysUserService sys_UserService, IDayQuartZService dayQuartZService,
            IMonthQuartZService monthQuartZService, ISws_DeviceInfo02Service sws_DeviceInfo02Service, ISws_RTUInfoService sws_RtuInfoService,
            ISws_UserStationService sws_UserStationService, ISws_StationService sws_StationService)
        {
            hourService = sws_hourService;
            userService = sys_UserService;
            dayService = dayQuartZService;
            monthService = monthQuartZService;
            deviceInfo02Service = sws_DeviceInfo02Service;
            rtuService = sws_RtuInfoService;
            userstationService = sws_UserStationService;
            stationService = sws_StationService;
        }
        #endregion

        #region 页面加载
        public IActionResult Index()
        {
            //树形
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = userService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
            var treeNodes = TreesOfStationDevice(user, "");
            if (!string.IsNullOrEmpty(treeNodes))
            {
                ViewBag.TreeNodes = new HtmlString(treeNodes);
            }
            else
            {
                ViewBag.TreeNodes = "[]";
            }
            return View();
        }
        #endregion

        #region 日报
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult WaterDayReport()
        {
            //初始化时间
            ViewBag.DateTime = string.Format("{0:yyyy-MM-dd}", DateTime.Now);
            return View();
        }
        [HttpPost]
        //获取日报数据
        [TypeFilter(typeof(IgonreActionFilter))]
        public async Task<IActionResult> GetDayReportData(string ids, string beginDate, string endDate)
        {
            List<int> idList = new List<int>();
            if (!string.IsNullOrEmpty(ids))
            {
                string[] StationIDs = ids.Split(",");

                foreach (var item in StationIDs)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        idList.Add(int.Parse(item));
                    }
                }
            }

            //时间处理
            DateTime begindate = Convert.ToDateTime(beginDate);
            DateTime enddate = Convert.ToDateTime(endDate);
            if (begindate == enddate)
            {
                enddate=enddate.AddDays(1);
            }
            //List<IGrouping<DateTime?, WebWaterQuality>> data = new List<IGrouping<DateTime?, WebWaterQuality>>();
            List<WebWaterQuality> data = new List<WebWaterQuality>();
            List<SwsStation> station = new List<SwsStation>();
            if (!string.IsNullOrEmpty(ids))
            {
                data = deviceInfo02Service.GetWaterData(ids, "HourQuartZ02", begindate.ToString(), enddate.ToString()).OrderBy(r => r.StationId).ToList();
                station = stationService.Query<SwsStation>(r => idList.Contains(r.StationId)).OrderBy(r => r.StationId).ToList();
            }
            ViewBag.Station = station;
            PartialView("_DayReportTable", data);
            string dataTable = await ViewToString.RenderPartialViewToString(this, "_DayReportTable");
            return Json(new
            {
                dataTable = dataTable
            });
        }

        //曲线
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult GetDayLineData(string beginDate, string endDate, string ids, string typeName)
        {
            List<int> idList = new List<int>();
            if (!string.IsNullOrEmpty(ids))
            {
                string[] StationIDs = ids.Split(",");

                foreach (var item in StationIDs)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        idList.Add(int.Parse(item));
                    }
                }
            }

            //时间处理
            DateTime begindate = Convert.ToDateTime(beginDate);
            DateTime enddate = Convert.ToDateTime(endDate);
            if (begindate == enddate)
            {
                enddate=enddate.AddDays(1);
            }
            List<WebWaterQuality> data = new List<WebWaterQuality>();
            List<SwsStation> station = new List<SwsStation>();
            if (idList.Count > 0)
            {
                data = deviceInfo02Service.GetWaterData(ids, "HourQuartZ02", begindate.ToString(), enddate.ToString()).OrderBy(r => r.StationId).ToList();
                station = stationService.Query<SwsStation>(r => idList.Contains(r.StationId)).OrderBy(r => r.StationId).ToList();
            }

            List<string> xix = new List<string>();
            List<string> lenged = new List<string>();
            List<PreChar> charData = new List<PreChar>();
            if (data.Count > 0)
            {
                List<IGrouping<DateTime?, WebWaterQuality>> tdata = data.OrderBy(r => r.UpdateTime).GroupBy(r => r.UpdateTime).ToList();
                foreach (var item in tdata)
                {
                    xix.Add(Convert.ToDateTime(item.Key).ToString("HH:mm"));
                }
                List<IGrouping<int?, WebWaterQuality>> sdata = data.OrderBy(r => r.UpdateTime).GroupBy(r => r.StationId).ToList();
                foreach (var item in station)
                {
                    PreChar pr = new PreChar();
                    pr.Name = item.StationName + typeName;
                    lenged.Add(item.StationName + typeName);
                    IGrouping<int?, WebWaterQuality> infodata = sdata.Where(r => r.Key == item.StationId).FirstOrDefault();
                    List<string> cc = new List<string>();
                    if (infodata != null && infodata.Count() > 0)
                    {
                        foreach (var info in infodata)
                        {
                            switch (typeName)
                            {
                                case "余氯":
                                    cc.Add(info.ClAver.ToString());
                                    break;
                                case "浊度":
                                    cc.Add(info.TurbidityAver.ToString());
                                    break;
                                case "PH":
                                    cc.Add(info.PhAver.ToString());
                                    break;
                                case "ORP":
                                    cc.Add(info.OrpAver.ToString());
                                    break;
                                case "盐度":
                                    cc.Add(info.SalinityAver.ToString());
                                    break;
                                case "溶解氧":
                                    cc.Add(info.OxygenAver.ToString());
                                    break;
                                case "电导率":
                                    cc.Add(info.ConductivityAver.ToString());
                                    break;
                            }
                        }
                    }
                    pr.Data = cc;
                    charData.Add(pr);
                }
            }
            //数据返回
            var rel = new
            {
                xAxis = xix,
                data = charData,
                legend = lenged
            };
            string result = Newtonsoft.Json.JsonConvert.SerializeObject(rel);
            return Content(result);
        }

        //数据导出
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult GetDayExportData(string beginDate, string endDate, string ids)
        {
            List<int> idList = new List<int>();
            if (!string.IsNullOrEmpty(ids))
            {
                string[] StationIDs = ids.Split(",");

                foreach (var item in StationIDs)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        idList.Add(int.Parse(item));
                    }
                }
            }

            //时间处理
            DateTime begindate = Convert.ToDateTime(beginDate);
            DateTime enddate = Convert.ToDateTime(endDate);
            if (begindate == enddate)
            {
                enddate=enddate.AddDays(1);
            }
            List<WebWaterQuality> data = new List<WebWaterQuality>();
            List<SwsStation> station = new List<SwsStation>();
            if (idList.Count > 0)
            {
                data = deviceInfo02Service.GetWaterData(ids, "HourQuartZ02", begindate.ToString(), enddate.ToString()).OrderBy(r => r.StationId).ToList();
                station = stationService.Query<SwsStation>(r => idList.Contains(r.StationId)).OrderBy(r => r.StationId).ToList();
            }

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

            #region 填充表头
            IRow head = sheet.CreateRow(0);
            IRow headrow = sheet.CreateRow(1);
            for (int i = 0; i <= station.Count * 7; i++)
            {
                head.CreateCell(i);
                head.Cells[i].CellStyle = style1;
            }
            head.Height = 31 * 20;
            head.Cells[0].SetCellValue("时间");
            sheet.SetColumnWidth(0, 20 * 256);
            for (var i = 0; i < station.Count; i++)
            {
                head.Cells[1 + i * 7].SetCellValue(station[i].StationName);

                headrow.CreateCell(1 + 7 * i).SetCellValue("余氯(mg/L)");
                sheet.SetColumnWidth(1 + 7 * i, 15 * 256);
                headrow.CreateCell(2 + 7 * i).SetCellValue("浊度(NTU)");
                sheet.SetColumnWidth(2 + 7 * i, 15 * 256);
                headrow.CreateCell(3 + 7 * i).SetCellValue("PH");
                sheet.SetColumnWidth(3 + 7 * i, 15 * 256);
                headrow.CreateCell(4 + 7 * i).SetCellValue("ORP(MV)");
                sheet.SetColumnWidth(4 + 7 * i, 15 * 256);
                headrow.CreateCell(5 + 7 * i).SetCellValue("盐度(ppm)");
                sheet.SetColumnWidth(5 + 7 * i, 15 * 256);
                headrow.CreateCell(6 + 7 * i).SetCellValue("溶解氧(mg/L)");
                sheet.SetColumnWidth(6 + 7 * i, 15 * 256);
                headrow.CreateCell(7 + 7 * i).SetCellValue("电导率");
                sheet.SetColumnWidth(7 + 7 * i, 15 * 256);
            }
            //表头第二行样式
            for (int i = 0; i < station.Count * 7; i++)
            {
                headrow.Cells[i].CellStyle = style1;
            }
            //合并单元格(最后合并单元格)
            sheet.AddMergedRegion(new CellRangeAddress(0, 1, 0, 0));
            for (var i = 0; i < station.Count; i++)
            {
                sheet.AddMergedRegion(new CellRangeAddress(0, 0, 1 + i * 7, 7 + i * 7));
            }
            #endregion
            List<IGrouping<DateTime?, WebWaterQuality>> sdata = data.GroupBy(r => r.UpdateTime).ToList();
            int j = 0;
            //填充内容
            foreach (var item in sdata)
            {
                IRow dataRow = sheet.CreateRow(j + 2);
                dataRow.CreateCell(0).SetCellValue(Convert.ToDateTime(item.Key).ToString("HH:mm"));
                int k = 1;
                foreach (var st in station)
                {
                    WebWaterQuality stdata = item.Where(r => r.StationId == st.StationId).FirstOrDefault();
                    if (stdata != null)
                    {
                        dataRow.CreateCell(k).SetCellValue(/*stdata.ClAver == null ? "--" :*/ Math.Round((double)stdata.ClAver, 2).ToString());
                        dataRow.CreateCell(k + 1).SetCellValue(Math.Round((double)stdata.TurbidityAver, 2).ToString());
                        dataRow.CreateCell(k + 2).SetCellValue(/*stdata.PhAver == null ? "--" :*/ Math.Round((double)stdata.PhAver, 2).ToString());
                        dataRow.CreateCell(k + 3).SetCellValue(Math.Round((double)stdata.OrpAver, 2).ToString());
                        dataRow.CreateCell(k + 4).SetCellValue(Math.Round((double)stdata.SalinityAver, 2).ToString());
                        dataRow.CreateCell(k + 5).SetCellValue(Math.Round((double)stdata.OxygenAver, 2).ToString());
                        dataRow.CreateCell(k + 6).SetCellValue(Math.Round((double)stdata.ConductivityAver, 2).ToString());

                    }
                    else
                    {
                        dataRow.CreateCell(k).SetCellValue("--");
                        dataRow.CreateCell(k + 1).SetCellValue("--");
                        dataRow.CreateCell(k + 2).SetCellValue("--");
                        dataRow.CreateCell(k + 3).SetCellValue("--");
                        dataRow.CreateCell(k + 4).SetCellValue("--");
                        dataRow.CreateCell(k + 5).SetCellValue("--");
                        dataRow.CreateCell(k + 6).SetCellValue("--");
                    }
                    k = k + 7;
                }
                //foreach (var info in item)
                //{
                //    dataRow.CreateCell(k).SetCellValue(info.ClAver==null?"--": Math.Round((double)info.ClAver, 2).ToString());
                //    dataRow.CreateCell(k+1).SetCellValue(info.TurbidityAver==null?"--": Math.Round((double)info.TurbidityAver, 2).ToString());
                //    dataRow.CreateCell(k+2).SetCellValue(info.PhAver==null?"--": Math.Round((double)info.PhAver,2).ToString());

                //}
                j = j + 1;
            }
            List<IGrouping<int?, WebWaterQuality>> stationData = data.GroupBy(r => r.StationId).ToList();
            #region 平均值
            IRow dataRow1 = sheet.CreateRow(j + 2);
            dataRow1.CreateCell(0).SetCellValue("平均值");
            for (int i = 0; i < station.Count; i++)
            {
                IGrouping<int?, WebWaterQuality> sd = stationData.Where(r => r.Key == station[i].StationId).FirstOrDefault();
                if (sd == null)
                {
                    dataRow1.CreateCell(7 * i + 1).SetCellValue("--");
                    dataRow1.CreateCell(7 * i + 2).SetCellValue("--");
                    dataRow1.CreateCell(7 * i + 3).SetCellValue("--");
                    dataRow1.CreateCell(7 * i + 4).SetCellValue("--");
                    dataRow1.CreateCell(7 * i + 5).SetCellValue("--");
                    dataRow1.CreateCell(7 * i + 6).SetCellValue("--");
                    dataRow1.CreateCell(7 * i + 7).SetCellValue("--");
                }
                else
                {
                    var cl = sd.Average(r => r.ClAver);
                    dataRow1.CreateCell(7 * i + 1).SetCellValue(/*cl == null ? "--" :*/ Math.Round((double)cl, 2).ToString());
                    var tb = sd.Average(r => r.TurbidityAver);
                    dataRow1.CreateCell(7 * i + 2).SetCellValue(Math.Round((double)tb, 2).ToString());
                    var ph = sd.Average(r => r.PhAver);
                    dataRow1.CreateCell(7 * i + 3).SetCellValue(Math.Round((double)ph, 2).ToString());
                    var or = sd.Average(r => r.OrpAver);
                    dataRow1.CreateCell(7 * i + 4).SetCellValue(Math.Round((double)or, 2).ToString());
                    var sa = sd.Average(r => r.SalinityAver);
                    dataRow1.CreateCell(7 * i + 5).SetCellValue(Math.Round((double)sa, 2).ToString());
                    var ox = sd.Average(r => r.OxygenAver);
                    dataRow1.CreateCell(7 * i + 6).SetCellValue(Math.Round((double)ox, 2).ToString());
                    var cu = sd.Average(r => r.ConductivityAver);
                    dataRow1.CreateCell(7 * i + 7).SetCellValue(Math.Round((double)cu, 2).ToString());
                }
            }
            #endregion

            #region 最大值
            IRow dataRow2 = sheet.CreateRow(j + 3);
            dataRow2.CreateCell(0).SetCellValue("最大值");
            for (int i = 0; i < station.Count; i++)
            {
                IGrouping<int?, WebWaterQuality> sd = stationData.Where(r => r.Key == station[i].StationId).FirstOrDefault();
                if (sd == null)
                {
                    dataRow2.CreateCell(7 * i + 1).SetCellValue("--");
                    dataRow2.CreateCell(7 * i + 2).SetCellValue("--");
                    dataRow2.CreateCell(7 * i + 3).SetCellValue("--");
                    dataRow2.CreateCell(7 * i + 4).SetCellValue("--");
                    dataRow2.CreateCell(7 * i + 5).SetCellValue("--");
                    dataRow2.CreateCell(7 * i + 6).SetCellValue("--");
                    dataRow2.CreateCell(7 * i + 7).SetCellValue("--");
                }
                else
                {
                    var cl = sd.Max(r => r.ClAver);
                    dataRow2.CreateCell(7 * i + 1).SetCellValue(/*cl == null ? "--" : */Math.Round((double)cl, 2).ToString());
                    var tb = sd.Max(r => r.TurbidityAver);
                    dataRow2.CreateCell(7 * i + 2).SetCellValue( Math.Round((double)tb, 2).ToString());
                    var ph = sd.Max(r => r.PhAver);
                    dataRow2.CreateCell(7 * i + 3).SetCellValue(Math.Round((double)ph, 2).ToString());
                    var or = sd.Max(r => r.OrpAver);
                    dataRow2.CreateCell(7 * i + 4).SetCellValue(Math.Round((double)or, 2).ToString());
                    var sa = sd.Max(r => r.SalinityAver);
                    dataRow2.CreateCell(7 * i + 5).SetCellValue(Math.Round((double)sa, 2).ToString());
                    var ox = sd.Max(r => r.OxygenAver);
                    dataRow2.CreateCell(7 * i + 6).SetCellValue(Math.Round((double)ox, 2).ToString());
                    var cu = sd.Max(r => r.ConductivityAver);
                    dataRow2.CreateCell(7 * i + 7).SetCellValue(Math.Round((double)cu, 2).ToString());
                }
            }
            #endregion

            #region 最大值时间
            IRow dataRow3 = sheet.CreateRow(j + 4);
            dataRow3.CreateCell(0).SetCellValue("最大值时间");
            for (int i = 0; i < station.Count; i++)
            {
                IGrouping<int?, WebWaterQuality> sd = stationData.Where(r => r.Key == station[i].StationId).FirstOrDefault();
                if (sd == null)
                {
                    dataRow3.CreateCell(7 * i + 1).SetCellValue("--");
                    dataRow3.CreateCell(7 * i + 2).SetCellValue("--");
                    dataRow3.CreateCell(7 * i + 3).SetCellValue("--");
                    dataRow3.CreateCell(7 * i + 4).SetCellValue("--");
                    dataRow3.CreateCell(7 * i + 5).SetCellValue("--");
                    dataRow3.CreateCell(7 * i + 6).SetCellValue("--");
                    dataRow3.CreateCell(7 * i + 7).SetCellValue("--");
                }
                else
                {
                    var cl = ((DateTime)sd.Where(r => r.ClAver == sd.Max(r => r.ClAver))?.Select(r => r.UpdateTime).FirstOrDefault()).ToString("HH:mm");
                    dataRow3.CreateCell(7 * i + 1).SetCellValue(cl == null ? "--" : cl.ToString());
                    var tb = ((DateTime)sd.Where(r => r.TurbidityAver == sd.Max(r => r.TurbidityAver))?.Select(r => r.UpdateTime).FirstOrDefault()).ToString("HH:mm");
                    dataRow3.CreateCell(7 * i + 2).SetCellValue(tb == null ? "--" : tb.ToString());
                    var ph = ((DateTime)sd.Where(r => r.PhAver == sd.Max(r => r.PhAver))?.Select(r => r.UpdateTime).FirstOrDefault()).ToString("HH:mm");
                    dataRow3.CreateCell(7 * i + 3).SetCellValue(ph == null ? "--" : ph.ToString());
                    var or = ((DateTime)sd.Where(r => r.OrpAver == sd.Max(r => r.OrpAver))?.Select(r => r.UpdateTime).FirstOrDefault()).ToString("HH:mm");
                    dataRow3.CreateCell(7 * i + 4).SetCellValue(or == null ? "--" : or.ToString());
                    var sa = ((DateTime)sd.Where(r => r.SalinityAver == sd.Max(r => r.SalinityAver))?.Select(r => r.UpdateTime).FirstOrDefault()).ToString("HH:mm");
                    dataRow3.CreateCell(7 * i + 5).SetCellValue(sa == null ? "--" : sa.ToString());
                    var ox = ((DateTime)sd.Where(r => r.OxygenAver == sd.Max(r => r.OxygenAver))?.Select(r => r.UpdateTime).FirstOrDefault()).ToString("HH:mm");
                    dataRow3.CreateCell(7 * i + 6).SetCellValue(ox == null ? "--" : ox.ToString());
                    var cu = ((DateTime)sd.Where(r => r.ConductivityAver == sd.Max(r => r.ConductivityAver))?.Select(r => r.UpdateTime).FirstOrDefault()).ToString("HH:mm");
                    dataRow3.CreateCell(7 * i + 7).SetCellValue(cu == null ? "--" : cu.ToString());
                }
            }
            #endregion
            #region 最小值
            IRow dataRow4 = sheet.CreateRow(j + 5);
            dataRow4.CreateCell(0).SetCellValue("最小值");
            for (int i = 0; i < station.Count; i++)
            {
                IGrouping<int?, WebWaterQuality> sd = stationData.Where(r => r.Key == station[i].StationId).FirstOrDefault();
                if (sd == null)
                {
                    dataRow4.CreateCell(7 * i + 1).SetCellValue("--");
                    dataRow4.CreateCell(7 * i + 2).SetCellValue("--");
                    dataRow4.CreateCell(7 * i + 3).SetCellValue("--");
                    dataRow4.CreateCell(7 * i + 4).SetCellValue("--");
                    dataRow4.CreateCell(7 * i + 5).SetCellValue("--");
                    dataRow4.CreateCell(7 * i + 6).SetCellValue("--");
                    dataRow4.CreateCell(7 * i + 7).SetCellValue("--");
                }
                else
                {
                    var cl = sd.Min(r => r.ClAver);
                    dataRow4.CreateCell(7 * i + 1).SetCellValue(/*cl == null ? "--" :*/ Math.Round((double)cl, 2).ToString());
                    var tb = sd.Min(r => r.TurbidityAver);
                    dataRow4.CreateCell(7 * i + 2).SetCellValue(Math.Round((double)tb, 2).ToString());
                    var ph = sd.Min(r => r.PhAver);
                    dataRow4.CreateCell(7 * i + 3).SetCellValue(Math.Round((double)ph, 2).ToString());
                    var or = sd.Min(r => r.OrpAver);
                    dataRow4.CreateCell(7 * i + 4).SetCellValue(Math.Round((double)or, 2).ToString());
                    var sa = sd.Min(r => r.SalinityAver);
                    dataRow4.CreateCell(7 * i + 5).SetCellValue(Math.Round((double)sa, 2).ToString());
                    var ox = sd.Min(r => r.OxygenAver);
                    dataRow4.CreateCell(7 * i + 6).SetCellValue(Math.Round((double)ox, 2).ToString());
                    var cu = sd.Min(r => r.ConductivityAver);
                    dataRow4.CreateCell(7 * i + 7).SetCellValue(Math.Round((double)cu, 2).ToString());

                }
            }
            #endregion
            #region 最小值时间
            IRow dataRow5 = sheet.CreateRow(j + 6);
            dataRow5.CreateCell(0).SetCellValue("最小值时间");
            for (int i = 0; i < station.Count; i++)
            {
                IGrouping<int?, WebWaterQuality> sd = stationData.Where(r => r.Key == station[i].StationId).FirstOrDefault();
                if (sd == null)
                {
                    dataRow5.CreateCell(7 * i + 1).SetCellValue("--");
                    dataRow5.CreateCell(7 * i + 2).SetCellValue("--");
                    dataRow5.CreateCell(7 * i + 3).SetCellValue("--");
                    dataRow5.CreateCell(7 * i + 4).SetCellValue("--");
                    dataRow5.CreateCell(7 * i + 5).SetCellValue("--");
                    dataRow5.CreateCell(7 * i + 6).SetCellValue("--");
                    dataRow5.CreateCell(7 * i + 7).SetCellValue("--");
                }
                else
                {
                    var cl = ((DateTime)sd.Where(r => r.ClAver == sd.Min(r => r.ClAver))?.Select(r => r.UpdateTime).FirstOrDefault()).ToString("HH:mm");
                    dataRow5.CreateCell(7 * i + 1).SetCellValue(cl == null ? "--" : cl.ToString());
                    var tb = ((DateTime)sd.Where(r => r.TurbidityAver == sd.Min(r => r.TurbidityAver))?.Select(r => r.UpdateTime).FirstOrDefault()).ToString("HH:mm");
                    dataRow5.CreateCell(7 * i + 2).SetCellValue(tb == null ? "--" : tb.ToString());
                    var ph = ((DateTime)sd.Where(r => r.PhAver == sd.Min(r => r.PhAver))?.Select(r => r.UpdateTime).FirstOrDefault()).ToString("HH:mm");
                    dataRow5.CreateCell(7 * i + 3).SetCellValue(ph == null ? "--" : ph.ToString());
                    var or = ((DateTime)sd.Where(r => r.OrpAver == sd.Min(r => r.OrpAver))?.Select(r => r.UpdateTime).FirstOrDefault()).ToString("HH:mm");
                    dataRow5.CreateCell(7 * i + 4).SetCellValue(or == null ? "--" : or.ToString());
                    var sa = ((DateTime)sd.Where(r => r.SalinityAver == sd.Min(r => r.SalinityAver))?.Select(r => r.UpdateTime).FirstOrDefault()).ToString("HH:mm");
                    dataRow5.CreateCell(7 * i + 5).SetCellValue(sa == null ? "--" : sa.ToString());
                    var ox = ((DateTime)sd.Where(r => r.OxygenAver == sd.Min(r => r.OxygenAver))?.Select(r => r.UpdateTime).FirstOrDefault()).ToString("HH:mm");
                    dataRow5.CreateCell(7 * i + 6).SetCellValue(ox == null ? "--" : ox.ToString());
                    var cu = ((DateTime)sd.Where(r => r.ConductivityAver == sd.Min(r => r.ConductivityAver))?.Select(r => r.UpdateTime).FirstOrDefault()).ToString("HH:mm");
                    dataRow5.CreateCell(7 * i + 7).SetCellValue(cu == null ? "--" : cu.ToString());
                }
            }
            #endregion
            for (int ro = 2; ro < sdata.Count + 7; ro++)
            {
                sheet.GetRow(ro).Height = 20 * 20;
                for (int s = 0; s <= station.Count * 7; s++)
                {
                    sheet.GetRow(ro).Cells[s].CellStyle = style;
                }
            }
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            workbook.Write(ms);
            workbook = null;
            ms.Close();
            ms.Dispose();
            return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "水质报表" + beginDate.Substring(0, 10) + ".xls");
        }
        #endregion

        #region 月报
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult WaterMonthReport()
        {
            string time = DateTime.Now.ToString("yyyy-MM-dd");
            ViewBag.endDate = time;
            ViewBag.beginDate = DateTime.Now.ToString("yyyy-MM-01");
            return View();
        }
        [HttpPost]
        [TypeFilter(typeof(IgonreActionFilter))]
        //获取日报数据
        public async Task<IActionResult> GetMonthReportData(string ids, string beginDate, string endDate)
        {
            List<int> idList = new List<int>();
            if (!string.IsNullOrEmpty(ids))
            {
                string[] StationIDs = ids.Split(",");

                foreach (var item in StationIDs)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        idList.Add(int.Parse(item));
                    }
                }
            }

            //时间处理
            DateTime begindate = Convert.ToDateTime(beginDate);
            DateTime enddate = Convert.ToDateTime(endDate);
            //List<IGrouping<DateTime?, WebWaterQuality>> data = new List<IGrouping<DateTime?, WebWaterQuality>>();
            List<WebWaterQuality> data = new List<WebWaterQuality>();
            List<SwsStation> station = new List<SwsStation>();
            if (idList.Count > 0)
            {
                data = deviceInfo02Service.GetWaterData(ids, "DayQuartZ02", begindate.ToString(), enddate.ToString()).OrderBy(r => r.StationId).ToList();
                station = stationService.Query<SwsStation>(r => idList.Contains(r.StationId)).OrderBy(r => r.StationId).ToList();
            }
            ViewBag.Station = station;
            PartialView("_MonthReportTable", data);
            string dataTable = await ViewToString.RenderPartialViewToString(this, "_MonthReportTable");
            return Json(new
            {
                dataTable = dataTable
            });
        }

        //曲线
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult GetMonthLineData(string beginDate, string endDate, string ids, string typeName)
        {
            List<int> idList = new List<int>();
            if (!string.IsNullOrEmpty(ids))
            {
                string[] StationIDs = ids.Split(",");

                foreach (var item in StationIDs)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        idList.Add(int.Parse(item));
                    }
                }
            }

            //时间处理
            DateTime begindate = Convert.ToDateTime(beginDate);
            DateTime enddate = Convert.ToDateTime(endDate);
            List<WebWaterQuality> data = new List<WebWaterQuality>();
            List<SwsStation> station = new List<SwsStation>();
            if (idList.Count > 0)
            {
                data = deviceInfo02Service.GetWaterData(ids, "DayQuartZ02", begindate.ToString(), enddate.ToString()).OrderBy(r => r.StationId).ToList();
                station = stationService.Query<SwsStation>(r => idList.Contains(r.StationId)).OrderBy(r => r.StationId).ToList();
            }

            List<string> xix = new List<string>();
            List<string> lenged = new List<string>();
            List<PreChar> charData = new List<PreChar>();
            if (data.Count > 0)
            {
                List<IGrouping<DateTime?, WebWaterQuality>> tdata = data.OrderBy(r => r.UpdateTime).GroupBy(r => r.UpdateTime).ToList();
                foreach (var item in tdata)
                {
                    xix.Add(Convert.ToDateTime(item.Key).ToString("yyyy-MM-dd"));
                }
                List<IGrouping<int?, WebWaterQuality>> sdata = data.OrderBy(r => r.UpdateTime).GroupBy(r => r.StationId).ToList();
                foreach (var item in station)
                {
                    PreChar pr = new PreChar();
                    pr.Name = item.StationName + typeName;
                    lenged.Add(item.StationName + typeName);
                    IGrouping<int?, WebWaterQuality> infodata = sdata.Where(r => r.Key == item.StationId).FirstOrDefault();
                    List<string> cc = new List<string>();
                    if (infodata != null && infodata.Count() > 0)
                    {
                        foreach (var info in infodata)
                        {
                            switch (typeName)
                            {
                                case "余氯":
                                    cc.Add(info.ClAver.ToString());
                                    break;
                                case "浊度":
                                    cc.Add(info.TurbidityAver.ToString());
                                    break;
                                case "PH":
                                    cc.Add(info.PhAver.ToString());
                                    break;
                                case "ORP":
                                    cc.Add(info.OrpAver.ToString());
                                    break;
                                case "盐度":
                                    cc.Add(info.SalinityAver.ToString());
                                    break;
                                case "溶解氧":
                                    cc.Add(info.OxygenAver.ToString());
                                    break;
                                case "电导率":
                                    cc.Add(info.ConductivityAver.ToString());
                                    break;
                            }
                        }
                    }
                    pr.Data = cc;
                    charData.Add(pr);
                }
            }
            //数据返回
            var rel = new
            {
                xAxis = xix,
                data = charData,
                legend = lenged
            };
            string result = Newtonsoft.Json.JsonConvert.SerializeObject(rel);
            return Content(result);
        }

        //数据导出
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult GetMonthExportData(string beginDate, string endDate, string ids)
        {
            List<int> idList = new List<int>();
            if (!string.IsNullOrEmpty(ids))
            {
                string[] StationIDs = ids.Split(",");

                foreach (var item in StationIDs)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        idList.Add(int.Parse(item));
                    }
                }
            }

            //时间处理
            DateTime begindate = Convert.ToDateTime(beginDate);
            DateTime enddate = Convert.ToDateTime(endDate);
            List<WebWaterQuality> data = new List<WebWaterQuality>();
            List<SwsStation> station = new List<SwsStation>();
            if (idList.Count > 0)
            {
                data = deviceInfo02Service.GetWaterData(ids, "DayQuartZ02", begindate.ToString(), enddate.ToString()).OrderBy(r => r.StationId).ToList();
                station = stationService.Query<SwsStation>(r => idList.Contains(r.StationId)).OrderBy(r => r.StationId).ToList();
            }
             
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

            #region 填充表头
            IRow head = sheet.CreateRow(0);
            IRow headrow = sheet.CreateRow(1);
            for (int i = 0; i <= station.Count * 7; i++)
            {
                head.CreateCell(i);
                head.Cells[i].CellStyle = style1;
            }
            head.Height = 31 * 20;
            head.Cells[0].SetCellValue("时间");
            sheet.SetColumnWidth(0, 20 * 256);
            for (var i = 0; i < station.Count; i++)
            {
                head.Cells[1 + i * 7].SetCellValue(station[i].StationName);
                
                headrow.CreateCell(1 + 7 * i).SetCellValue("余氯(mg/L)");
                sheet.SetColumnWidth(1 + 7 * i, 15 * 256);
                headrow.CreateCell(2 + 7 * i).SetCellValue("浊度(NTU)");
                sheet.SetColumnWidth(2 + 7 * i, 15 * 256);
                headrow.CreateCell(3 + 7 * i).SetCellValue("PH");
                sheet.SetColumnWidth(3 + 7 * i, 15 * 256);
                headrow.CreateCell(4 + 7 * i).SetCellValue("ORP(MV)");
                sheet.SetColumnWidth(4 + 7 * i, 15 * 256);
                headrow.CreateCell(5 + 7 * i).SetCellValue("盐度(ppm)");
                sheet.SetColumnWidth(5 + 7 * i, 15 * 256);
                headrow.CreateCell(6 + 7 * i).SetCellValue("溶解氧(mg/L)");
                sheet.SetColumnWidth(6 + 7 * i, 15 * 256);
                headrow.CreateCell(7 + 7 * i).SetCellValue("电导率");
                sheet.SetColumnWidth(7 + 7 * i, 15 * 256);
            }
            //表头第二行样式
            for (int i = 0; i < station.Count * 7; i++)
            {
                headrow.Cells[i].CellStyle = style1;
            }
            //合并单元格(最后合并单元格)
            sheet.AddMergedRegion(new CellRangeAddress(0, 1, 0, 0));
            for (var i = 0; i < station.Count; i++)
            {
                sheet.AddMergedRegion(new CellRangeAddress(0, 0, 1 + i * 7, 7 + i * 7));
            }
            #endregion
            List<IGrouping<DateTime?, WebWaterQuality>> sdata = data.GroupBy(r => r.UpdateTime).ToList();
            int j = 0;
            //填充内容
            foreach (var item in sdata)
            {
                IRow dataRow = sheet.CreateRow(j + 2);
                dataRow.CreateCell(0).SetCellValue(Convert.ToDateTime(item.Key).ToString("yyyy-MM-dd"));
                int k = 1;
                foreach (var st in station)
                {
                    WebWaterQuality stdata = item.Where(r => r.StationId == st.StationId).FirstOrDefault();
                    if (stdata != null)
                    {
                        dataRow.CreateCell(k).SetCellValue(/*stdata.ClAver == null ? "--" :*/ Math.Round((double)stdata.ClAver, 2).ToString());
                        dataRow.CreateCell(k + 1).SetCellValue(Math.Round((double)stdata.TurbidityAver, 2).ToString());
                        dataRow.CreateCell(k + 2).SetCellValue(Math.Round((double)stdata.PhAver, 2).ToString());
                        dataRow.CreateCell(k + 3).SetCellValue(Math.Round((double)stdata.OrpAver, 2).ToString());
                        dataRow.CreateCell(k + 4).SetCellValue(Math.Round((double)stdata.SalinityAver, 2).ToString());
                        dataRow.CreateCell(k + 5).SetCellValue(Math.Round((double)stdata.OxygenAver, 2).ToString());
                        dataRow.CreateCell(k + 6).SetCellValue(Math.Round((double)stdata.ConductivityAver, 2).ToString());

                    }
                    else
                    {
                        dataRow.CreateCell(k).SetCellValue("--");
                        dataRow.CreateCell(k + 1).SetCellValue("--");
                        dataRow.CreateCell(k + 2).SetCellValue("--");
                        dataRow.CreateCell(k + 3).SetCellValue("--");
                        dataRow.CreateCell(k + 4).SetCellValue("--");
                        dataRow.CreateCell(k + 5).SetCellValue("--");
                        dataRow.CreateCell(k + 6).SetCellValue("--");
                    }
                    k = k + 7;
                }
                //foreach (var info in item)
                //{
                //    dataRow.CreateCell(k).SetCellValue(info.ClAver==null?"--": Math.Round((double)info.ClAver, 2).ToString());
                //    dataRow.CreateCell(k+1).SetCellValue(info.TurbidityAver==null?"--": Math.Round((double)info.TurbidityAver, 2).ToString());
                //    dataRow.CreateCell(k+2).SetCellValue(info.PhAver==null?"--": Math.Round((double)info.PhAver,2).ToString());

                //}
                j = j + 1;
            }
            List<IGrouping<int?, WebWaterQuality>> stationData = data.GroupBy(r => r.StationId).ToList();
            #region 平均值
            IRow dataRow1 = sheet.CreateRow(j + 2);
            dataRow1.CreateCell(0).SetCellValue("平均值");
            for (int i = 0; i < station.Count; i++)
            {
                IGrouping<int?, WebWaterQuality> sd = stationData.Where(r => r.Key == station[i].StationId).FirstOrDefault();
                if (sd == null)
                {
                    dataRow1.CreateCell(7 * i + 1).SetCellValue("--");
                    dataRow1.CreateCell(7 * i + 2).SetCellValue("--");
                    dataRow1.CreateCell(7 * i + 3).SetCellValue("--");
                    dataRow1.CreateCell(7 * i + 4).SetCellValue("--");
                    dataRow1.CreateCell(7 * i + 5).SetCellValue("--");
                    dataRow1.CreateCell(7 * i + 6).SetCellValue("--");
                    dataRow1.CreateCell(7 * i + 7).SetCellValue("--");
                }
                else
                {
                    var cl = sd.Average(r => r.ClAver);
                    dataRow1.CreateCell(7 * i + 1).SetCellValue(/*cl == null ? "--" : */Math.Round((double)cl, 2).ToString());
                    var tb = sd.Average(r => r.TurbidityAver);
                    dataRow1.CreateCell(7 * i + 2).SetCellValue(Math.Round((double)tb, 2).ToString());
                    var ph = sd.Average(r => r.PhAver);
                    dataRow1.CreateCell(7 * i + 3).SetCellValue(Math.Round((double)ph, 2).ToString());
                    var or = sd.Average(r => r.OrpAver);
                    dataRow1.CreateCell(7 * i + 4).SetCellValue(Math.Round((double)or, 2).ToString());
                    var sa = sd.Average(r => r.SalinityAver);
                    dataRow1.CreateCell(7 * i + 5).SetCellValue(Math.Round((double)sa, 2).ToString());
                    var ox = sd.Average(r => r.OxygenAver);
                    dataRow1.CreateCell(7 * i + 6).SetCellValue(Math.Round((double)ox, 2).ToString());
                    var cu = sd.Average(r => r.ConductivityAver);
                    dataRow1.CreateCell(7 * i + 7).SetCellValue(Math.Round((double)cu, 2).ToString());
                }
            }
            #endregion

            #region 最大值
            IRow dataRow2 = sheet.CreateRow(j + 3);
            dataRow2.CreateCell(0).SetCellValue("最大值");
            for (int i = 0; i < station.Count; i++)
            {
                IGrouping<int?, WebWaterQuality> sd = stationData.Where(r => r.Key == station[i].StationId).FirstOrDefault();
                if (sd == null)
                {
                    dataRow2.CreateCell(7 * i + 1).SetCellValue("--");
                    dataRow2.CreateCell(7 * i + 2).SetCellValue("--");
                    dataRow2.CreateCell(7 * i + 3).SetCellValue("--");
                    dataRow2.CreateCell(7 * i + 4).SetCellValue("--");
                    dataRow2.CreateCell(7 * i + 5).SetCellValue("--");
                    dataRow2.CreateCell(7 * i + 6).SetCellValue("--");
                    dataRow2.CreateCell(7 * i + 7).SetCellValue("--");
                }
                else
                {
                    var cl = sd.Max(r => r.ClAver);
                    dataRow2.CreateCell(7 * i + 1).SetCellValue(/*cl == null ? "--" : */Math.Round((double)cl, 2).ToString());
                    var tb = sd.Max(r => r.TurbidityAver);
                    dataRow2.CreateCell(7 * i + 2).SetCellValue(Math.Round((double)tb, 2).ToString());
                    var ph = sd.Max(r => r.PhAver);
                    dataRow2.CreateCell(7 * i + 3).SetCellValue(Math.Round((double)ph, 2).ToString());
                    var or = sd.Max(r => r.OrpAver);
                    dataRow2.CreateCell(7 * i + 4).SetCellValue(Math.Round((double)or, 2).ToString());
                    var sa = sd.Max(r => r.SalinityAver);
                    dataRow2.CreateCell(7 * i + 5).SetCellValue(Math.Round((double)sa, 2).ToString());
                    var ox = sd.Max(r => r.OxygenAver);
                    dataRow2.CreateCell(7 * i + 6).SetCellValue(Math.Round((double)ox, 2).ToString());
                    var cu = sd.Max(r => r.ConductivityAver);
                    dataRow2.CreateCell(7 * i + 7).SetCellValue(Math.Round((double)cu, 2).ToString());
                }
            }
            #endregion

            #region 最大值时间
            IRow dataRow3 = sheet.CreateRow(j + 4);
            dataRow3.CreateCell(0).SetCellValue("最大值时间");
            for (int i = 0; i < station.Count; i++)
            {
                IGrouping<int?, WebWaterQuality> sd = stationData.Where(r => r.Key == station[i].StationId).FirstOrDefault();
                if (sd == null)
                {
                    dataRow3.CreateCell(7 * i + 1).SetCellValue("--");
                    dataRow3.CreateCell(7 * i + 2).SetCellValue("--");
                    dataRow3.CreateCell(7 * i + 3).SetCellValue("--");
                    dataRow3.CreateCell(7 * i + 4).SetCellValue("--");
                    dataRow3.CreateCell(7 * i + 5).SetCellValue("--");
                    dataRow3.CreateCell(7 * i + 6).SetCellValue("--");
                    dataRow3.CreateCell(7 * i + 7).SetCellValue("--");
                }
                else
                {
                    var cl = ((DateTime)sd.Where(r => r.ClAver == sd.Max(r => r.ClAver))?.Select(r => r.UpdateTime).FirstOrDefault()).ToString("yyyy-MM-dd");
                    dataRow3.CreateCell(7 * i + 1).SetCellValue(cl == null ? "--" : cl.ToString());
                    var tb = ((DateTime)sd.Where(r => r.TurbidityAver == sd.Max(r => r.TurbidityAver))?.Select(r => r.UpdateTime).FirstOrDefault()).ToString("yyyy-MM-dd");
                    dataRow3.CreateCell(7 * i + 2).SetCellValue(tb == null ? "--" : tb.ToString());
                    var ph = ((DateTime)sd.Where(r => r.PhAver == sd.Max(r => r.PhAver))?.Select(r => r.UpdateTime).FirstOrDefault()).ToString("yyyy-MM-dd");
                    dataRow3.CreateCell(7 * i + 3).SetCellValue(ph == null ? "--" : ph.ToString());
                    var or = ((DateTime)sd.Where(r => r.OrpAver == sd.Max(r => r.OrpAver))?.Select(r => r.UpdateTime).FirstOrDefault()).ToString("yyyy-MM-dd");
                    dataRow3.CreateCell(7 * i + 4).SetCellValue(or == null ? "--" : or.ToString());
                    var sa = ((DateTime)sd.Where(r => r.SalinityAver == sd.Max(r => r.SalinityAver))?.Select(r => r.UpdateTime).FirstOrDefault()).ToString("yyyy-MM-dd");
                    dataRow3.CreateCell(7 * i + 5).SetCellValue(sa == null ? "--" : sa.ToString());
                    var ox = ((DateTime)sd.Where(r => r.OxygenAver == sd.Max(r => r.OxygenAver))?.Select(r => r.UpdateTime).FirstOrDefault()).ToString("yyyy-MM-dd");
                    dataRow3.CreateCell(7 * i + 6).SetCellValue(ox == null ? "--" : ox.ToString());
                    var cu = ((DateTime)sd.Where(r => r.ConductivityAver == sd.Max(r => r.ConductivityAver))?.Select(r => r.UpdateTime).FirstOrDefault()).ToString("yyyy-MM-dd");
                    dataRow3.CreateCell(7 * i + 7).SetCellValue(cu == null ? "--" : cu.ToString());
                }
            }
            #endregion
            #region 最小值
            IRow dataRow4 = sheet.CreateRow(j + 5);
            dataRow4.CreateCell(0).SetCellValue("最小值");
            for (int i = 0; i < station.Count; i++)
            {
                IGrouping<int?, WebWaterQuality> sd = stationData.Where(r => r.Key == station[i].StationId).FirstOrDefault();
                if (sd == null)
                {
                    dataRow4.CreateCell(7 * i + 1).SetCellValue("--");
                    dataRow4.CreateCell(7 * i + 2).SetCellValue("--");
                    dataRow4.CreateCell(7 * i + 3).SetCellValue("--");
                    dataRow4.CreateCell(7 * i + 4).SetCellValue("--");
                    dataRow4.CreateCell(7 * i + 5).SetCellValue("--");
                    dataRow4.CreateCell(7 * i + 6).SetCellValue("--");
                    dataRow4.CreateCell(7 * i + 7).SetCellValue("--");
                }
                else
                {
                    var cl = sd.Min(r => r.ClAver);
                    dataRow4.CreateCell(7 * i + 1).SetCellValue(/*cl == null ? "--" : */Math.Round((double)cl, 2).ToString());
                    var tb = sd.Min(r => r.TurbidityAver);
                    dataRow4.CreateCell(7 * i + 2).SetCellValue(Math.Round((double)tb, 2).ToString());
                    var ph = sd.Min(r => r.PhAver);
                    dataRow4.CreateCell(7 * i + 3).SetCellValue(Math.Round((double)ph, 2).ToString());
                    var or = sd.Min(r => r.OrpAver);
                    dataRow4.CreateCell(7 * i + 4).SetCellValue(Math.Round((double)or, 2).ToString());
                    var sa = sd.Min(r => r.SalinityAver);
                    dataRow4.CreateCell(7 * i + 5).SetCellValue(Math.Round((double)sa, 2).ToString());
                    var ox = sd.Min(r => r.OxygenAver);
                    dataRow4.CreateCell(7 * i + 6).SetCellValue(Math.Round((double)ox, 2).ToString());
                    var cu = sd.Min(r => r.ConductivityAver);
                    dataRow4.CreateCell(7 * i + 7).SetCellValue(Math.Round((double)cu, 2).ToString());

                }
            }
            #endregion
            #region 最小值时间
            IRow dataRow5 = sheet.CreateRow(j + 6);
            dataRow5.CreateCell(0).SetCellValue("最小值时间");
            for (int i = 0; i < station.Count; i++)
            {
                IGrouping<int?, WebWaterQuality> sd = stationData.Where(r => r.Key == station[i].StationId).FirstOrDefault();
                if (sd == null)
                {
                    dataRow5.CreateCell(7 * i + 1).SetCellValue("--");
                    dataRow5.CreateCell(7 * i + 2).SetCellValue("--");
                    dataRow5.CreateCell(7 * i + 3).SetCellValue("--");
                    dataRow5.CreateCell(7 * i + 4).SetCellValue("--");
                    dataRow5.CreateCell(7 * i + 5).SetCellValue("--");
                    dataRow5.CreateCell(7 * i + 6).SetCellValue("--");
                    dataRow5.CreateCell(7 * i + 7).SetCellValue("--");
                }
                else
                {
                    var cl = ((DateTime)sd.Where(r => r.ClAver == sd.Min(r => r.ClAver))?.Select(r => r.UpdateTime).FirstOrDefault()).ToString("yyyy-MM-dd");
                    dataRow5.CreateCell(7 * i + 1).SetCellValue(cl == null ? "--" : cl.ToString());
                    var tb = ((DateTime)sd.Where(r => r.TurbidityAver == sd.Min(r => r.TurbidityAver))?.Select(r => r.UpdateTime).FirstOrDefault()).ToString("yyyy-MM-dd");
                    dataRow5.CreateCell(7 * i + 2).SetCellValue(tb == null ? "--" : tb.ToString());
                    var ph = ((DateTime)sd.Where(r => r.PhAver == sd.Min(r => r.PhAver))?.Select(r => r.UpdateTime).FirstOrDefault()).ToString("yyyy-MM-dd");
                    dataRow5.CreateCell(7 * i + 3).SetCellValue(ph == null ? "--" : ph.ToString());
                    var or = ((DateTime)sd.Where(r => r.OrpAver == sd.Min(r => r.OrpAver))?.Select(r => r.UpdateTime).FirstOrDefault()).ToString("yyyy-MM-dd");
                    dataRow5.CreateCell(7 * i + 4).SetCellValue(or == null ? "--" : or.ToString());
                    var sa = ((DateTime)sd.Where(r => r.SalinityAver == sd.Min(r => r.SalinityAver))?.Select(r => r.UpdateTime).FirstOrDefault()).ToString("yyyy-MM-dd");
                    dataRow5.CreateCell(7 * i + 5).SetCellValue(sa == null ? "--" : sa.ToString());
                    var ox = ((DateTime)sd.Where(r => r.OxygenAver == sd.Min(r => r.OxygenAver))?.Select(r => r.UpdateTime).FirstOrDefault()).ToString("yyyy-MM-dd");
                    dataRow5.CreateCell(7 * i + 6).SetCellValue(ox == null ? "--" : ox.ToString());
                    var cu = ((DateTime)sd.Where(r => r.ConductivityAver == sd.Min(r => r.ConductivityAver))?.Select(r => r.UpdateTime).FirstOrDefault()).ToString("yyyy-MM-dd");
                    dataRow5.CreateCell(7 * i + 7).SetCellValue(cu == null ? "--" : cu.ToString());
                }
            }
            #endregion
            for (int ro = 2; ro < sdata.Count + 7; ro++)
            {
                sheet.GetRow(ro).Height = 20 * 20;
                for (int s = 0; s <= station.Count * 7; s++)
                {
                    sheet.GetRow(ro).Cells[s].CellStyle = style;
                }
            }
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            workbook.Write(ms);
            workbook = null;
            ms.Close();
            ms.Dispose();
            return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "水质月表" + beginDate + ".xls");
        }
        #endregion

        #region 年报
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult WaterYearReport()
        {
            string time = DateTime.Now.ToString("yyyy-MM");
            ViewBag.endDate = time;
            ViewBag.beginDate = DateTime.Now.ToString("yyyy-01");
            return View();
        }
        [HttpPost]
        //获取日报数据
        [TypeFilter(typeof(IgonreActionFilter))]
        public async Task<IActionResult> GetYearReportData(string ids, string beginDate, string endDate)
        {
            List<int> idList = new List<int>();
            if (!string.IsNullOrEmpty(ids))
            {
                string[] StationIDs = ids.Split(",");

                foreach (var item in StationIDs)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        idList.Add(int.Parse(item));
                    }
                }
            }

            //时间处理
            DateTime begindate = Convert.ToDateTime(beginDate);
            DateTime enddate = Convert.ToDateTime(endDate);
            //List<IGrouping<DateTime?, YearQuartZ>> data = new List<IGrouping<DateTime?, YearQuartZ>>();
            List<WebWaterQuality> data = new List<WebWaterQuality>();
            List<SwsStation> station = new List<SwsStation>();
            if (idList.Count > 0)
            {
                data = deviceInfo02Service.GetWaterData(ids, "MonthQuartZ02", begindate.ToString(), enddate.ToString()).OrderBy(r => r.StationId).ToList(); 
                station = stationService.Query<SwsStation>(r => idList.Contains(r.StationId)).OrderBy(r => r.StationId).ToList();
            }
            ViewBag.Station = station;
            PartialView("_YearReportTable", data);
            string dataTable = await ViewToString.RenderPartialViewToString(this, "_YearReportTable");
            return Json(new
            {
                dataTable = dataTable
            });
        }

        //曲线
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult GetYearLineData(string beginDate, string endDate, string ids, string typeName)
        {
            List<int> idList = new List<int>();
            if (!string.IsNullOrEmpty(ids))
            {
                string[] StationIDs = ids.Split(",");

                foreach (var item in StationIDs)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        idList.Add(int.Parse(item));
                    }
                }
            }

            //时间处理
            DateTime begindate = Convert.ToDateTime(beginDate);
            DateTime enddate = Convert.ToDateTime(endDate);
            List<WebWaterQuality> data = new List<WebWaterQuality>();
            List<SwsStation> station = new List<SwsStation>();
            if (idList.Count > 0)
            {
                data = deviceInfo02Service.GetWaterData(ids, "MonthQuartZ02", begindate.ToString(), enddate.ToString()).OrderBy(r => r.StationId).ToList();
                station = stationService.Query<SwsStation>(r => idList.Contains(r.StationId)).OrderBy(r => r.StationId).ToList();
            }

            List<string> xix = new List<string>();
            List<string> lenged = new List<string>();
            List<PreChar> charData = new List<PreChar>();
            if (data.Count > 0)
            {
                List<IGrouping<DateTime?, WebWaterQuality>> tdata = data.OrderBy(r => r.UpdateTime).GroupBy(r => r.UpdateTime).ToList();
                foreach (var item in tdata)
                {
                    xix.Add(Convert.ToDateTime(item.Key).ToString("yyyy-MM"));
                }
                List<IGrouping<int?, WebWaterQuality>> sdata = data.OrderBy(r => r.UpdateTime).GroupBy(r => r.StationId).ToList();
                foreach (var item in station)
                {
                    PreChar pr = new PreChar();
                    pr.Name = item.StationName + typeName;
                    lenged.Add(item.StationName + typeName);
                    IGrouping<int?, WebWaterQuality> infodata = sdata.Where(r => r.Key == item.StationId).FirstOrDefault();
                    List<string> cc = new List<string>();
                    if (infodata != null && infodata.Count() > 0)
                    {
                        foreach (var info in infodata)
                        {
                            switch (typeName)
                            {
                                case "余氯":
                                    cc.Add(info.ClAver.ToString());
                                    break;
                                case "浊度":
                                    cc.Add(info.TurbidityAver.ToString());
                                    break;
                                case "PH":
                                    cc.Add(info.PhAver.ToString());
                                    break;
                                case "ORP":
                                    cc.Add(info.OrpAver.ToString());
                                    break;
                                case "盐度":
                                    cc.Add(info.SalinityAver.ToString());
                                    break;
                                case "溶解氧":
                                    cc.Add(info.OxygenAver.ToString());
                                    break;
                                case "电导率":
                                    cc.Add(info.ConductivityAver.ToString());
                                    break;
                            }
                        }
                    }
                    pr.Data = cc;
                    charData.Add(pr);
                }
            }
            //数据返回
            var rel = new
            {
                xAxis = xix,
                data = charData,
                legend = lenged
            };
            string result = Newtonsoft.Json.JsonConvert.SerializeObject(rel);
            return Content(result);
        }

        //数据导出
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult GetYearExportData(string beginDate, string endDate, string ids)
        {
            List<int> idList = new List<int>();
            if (!string.IsNullOrEmpty(ids))
            {
                string[] StationIDs = ids.Split(",");

                foreach (var item in StationIDs)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        idList.Add(int.Parse(item));
                    }
                }
            }

            //时间处理
            DateTime begindate = Convert.ToDateTime(beginDate);
            DateTime enddate = Convert.ToDateTime(endDate);
            List<WebWaterQuality> data = new List<WebWaterQuality>();
            List<SwsStation> station = new List<SwsStation>();
            if (idList.Count > 0)
            {
                data = deviceInfo02Service.GetWaterData(ids, "MonthQuartZ02", begindate.ToString(), enddate.ToString()).OrderBy(r => r.StationId).ToList();
                station = stationService.Query<SwsStation>(r => idList.Contains(r.StationId)).OrderBy(r => r.StationId).ToList();
            }

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

            #region 填充表头
            IRow head = sheet.CreateRow(0);
            IRow headrow = sheet.CreateRow(1);
            for (int i = 0; i <= station.Count * 7; i++)
            {
                head.CreateCell(i);
                head.Cells[i].CellStyle = style1;
            }
            head.Height = 31 * 20;
            head.Cells[0].SetCellValue("时间");
            sheet.SetColumnWidth(0, 20 * 256);
            for (var i = 0; i < station.Count; i++)
            {
                head.Cells[1 + i * 7].SetCellValue(station[i].StationName);

                headrow.CreateCell(1 + 7 * i).SetCellValue("余氯(mg/L)");
                sheet.SetColumnWidth(1 + 7 * i, 15 * 256);
                headrow.CreateCell(2 + 7 * i).SetCellValue("浊度(NTU)");
                sheet.SetColumnWidth(2 + 7 * i, 15 * 256);
                headrow.CreateCell(3 + 7 * i).SetCellValue("PH");
                sheet.SetColumnWidth(3 + 7 * i, 15 * 256);
                headrow.CreateCell(4 + 7 * i).SetCellValue("ORP(MV)");
                sheet.SetColumnWidth(4 + 7 * i, 15 * 256);
                headrow.CreateCell(5 + 7 * i).SetCellValue("盐度(ppm)");
                sheet.SetColumnWidth(5 + 7 * i, 15 * 256);
                headrow.CreateCell(6 + 7 * i).SetCellValue("溶解氧(mg/L)");
                sheet.SetColumnWidth(6 + 7 * i, 15 * 256);
                headrow.CreateCell(7 + 7 * i).SetCellValue("电导率");
                sheet.SetColumnWidth(7 + 7 * i, 15 * 256);
            }
            //表头第二行样式
            for (int i = 0; i < station.Count * 7; i++)
            {
                headrow.Cells[i].CellStyle = style1;
            }
            //合并单元格(最后合并单元格)
            sheet.AddMergedRegion(new CellRangeAddress(0, 1, 0, 0));
            for (var i = 0; i < station.Count; i++)
            {
                sheet.AddMergedRegion(new CellRangeAddress(0, 0, 1 + i * 7, 7 + i * 7));
            }
            #endregion
            List<IGrouping<DateTime?, WebWaterQuality>> sdata = data.GroupBy(r => r.UpdateTime).ToList();
            int j = 0;
            //填充内容
            foreach (var item in sdata)
            {
                IRow dataRow = sheet.CreateRow(j + 2);
                dataRow.CreateCell(0).SetCellValue(Convert.ToDateTime(item.Key).ToString("yyyy-MM"));
                int k = 1;
                foreach (var st in station)
                {
                    WebWaterQuality stdata = item.Where(r => r.StationId == st.StationId).FirstOrDefault();
                    if (stdata != null)
                    {
                        dataRow.CreateCell(k).SetCellValue(/*stdata.ClAver == null ? "--" : */Math.Round((double)stdata.ClAver, 2).ToString());
                        dataRow.CreateCell(k + 1).SetCellValue(Math.Round((double)stdata.TurbidityAver, 2).ToString());
                        dataRow.CreateCell(k + 2).SetCellValue(Math.Round((double)stdata.PhAver, 2).ToString());
                        dataRow.CreateCell(k + 3).SetCellValue(Math.Round((double)stdata.OrpAver, 2).ToString());
                        dataRow.CreateCell(k + 4).SetCellValue(Math.Round((double)stdata.SalinityAver, 2).ToString());
                        dataRow.CreateCell(k + 5).SetCellValue(Math.Round((double)stdata.OxygenAver, 2).ToString());
                        dataRow.CreateCell(k + 6).SetCellValue(Math.Round((double)stdata.ConductivityAver, 2).ToString());

                    }
                    else
                    {
                        dataRow.CreateCell(k).SetCellValue("--");
                        dataRow.CreateCell(k + 1).SetCellValue("--");
                        dataRow.CreateCell(k + 2).SetCellValue("--");
                        dataRow.CreateCell(k + 3).SetCellValue("--");
                        dataRow.CreateCell(k + 4).SetCellValue("--");
                        dataRow.CreateCell(k + 5).SetCellValue("--");
                        dataRow.CreateCell(k + 6).SetCellValue("--");
                    }
                    k = k + 7;
                }
                //foreach (var info in item)
                //{
                //    dataRow.CreateCell(k).SetCellValue(info.ClAver==null?"--": Math.Round((double)info.ClAver, 2).ToString());
                //    dataRow.CreateCell(k+1).SetCellValue(info.TurbidityAver==null?"--": Math.Round((double)info.TurbidityAver, 2).ToString());
                //    dataRow.CreateCell(k+2).SetCellValue(info.PhAver==null?"--": Math.Round((double)info.PhAver,2).ToString());

                //}
                j = j + 1;
            }
            List<IGrouping<int?, WebWaterQuality>> stationData = data.GroupBy(r => r.StationId).ToList();
            #region 平均值
            IRow dataRow1 = sheet.CreateRow(j + 2);
            dataRow1.CreateCell(0).SetCellValue("平均值");
            for (int i = 0; i < station.Count; i++)
            {
                IGrouping<int?, WebWaterQuality> sd = stationData.Where(r => r.Key == station[i].StationId).FirstOrDefault();
                if (sd == null)
                {
                    dataRow1.CreateCell(7 * i + 1).SetCellValue("--");
                    dataRow1.CreateCell(7 * i + 2).SetCellValue("--");
                    dataRow1.CreateCell(7 * i + 3).SetCellValue("--");
                    dataRow1.CreateCell(7 * i + 4).SetCellValue("--");
                    dataRow1.CreateCell(7 * i + 5).SetCellValue("--");
                    dataRow1.CreateCell(7 * i + 6).SetCellValue("--");
                    dataRow1.CreateCell(7 * i + 7).SetCellValue("--");
                }
                else
                {
                    var cl = sd.Average(r => r.ClAver);
                    dataRow1.CreateCell(7 * i + 1).SetCellValue(/*cl == null ? "--" : */Math.Round((double)cl, 2).ToString());
                    var tb = sd.Average(r => r.TurbidityAver);
                    dataRow1.CreateCell(7 * i + 2).SetCellValue(Math.Round((double)tb, 2).ToString());
                    var ph = sd.Average(r => r.PhAver);
                    dataRow1.CreateCell(7 * i + 3).SetCellValue(Math.Round((double)ph, 2).ToString());
                    var or = sd.Average(r => r.OrpAver);
                    dataRow1.CreateCell(7 * i + 4).SetCellValue(Math.Round((double)or, 2).ToString());
                    var sa = sd.Average(r => r.SalinityAver);
                    dataRow1.CreateCell(7 * i + 5).SetCellValue(Math.Round((double)sa, 2).ToString());
                    var ox = sd.Average(r => r.OxygenAver);
                    dataRow1.CreateCell(7 * i + 6).SetCellValue(Math.Round((double)ox, 2).ToString());
                    var cu = sd.Average(r => r.ConductivityAver);
                    dataRow1.CreateCell(7 * i + 7).SetCellValue(Math.Round((double)cu, 2).ToString());
                }
            }
            #endregion

            #region 最大值
            IRow dataRow2 = sheet.CreateRow(j + 3);
            dataRow2.CreateCell(0).SetCellValue("最大值");
            for (int i = 0; i < station.Count; i++)
            {
                IGrouping<int?, WebWaterQuality> sd = stationData.Where(r => r.Key == station[i].StationId).FirstOrDefault();
                if (sd == null)
                {
                    dataRow2.CreateCell(7 * i + 1).SetCellValue("--");
                    dataRow2.CreateCell(7 * i + 2).SetCellValue("--");
                    dataRow2.CreateCell(7 * i + 3).SetCellValue("--");
                    dataRow2.CreateCell(7 * i + 4).SetCellValue("--");
                    dataRow2.CreateCell(7 * i + 5).SetCellValue("--");
                    dataRow2.CreateCell(7 * i + 6).SetCellValue("--");
                    dataRow2.CreateCell(7 * i + 7).SetCellValue("--");
                }
                else
                {
                    var cl = sd.Max(r => r.ClAver);
                    dataRow2.CreateCell(7 * i + 1).SetCellValue(Math.Round((double)cl, 2).ToString());
                    var tb = sd.Max(r => r.TurbidityAver);
                    dataRow2.CreateCell(7 * i + 2).SetCellValue(Math.Round((double)tb, 2).ToString());
                    var ph = sd.Max(r => r.PhAver);
                    dataRow2.CreateCell(7 * i + 3).SetCellValue(/*ph == null ? "--" : */Math.Round((double)ph, 2).ToString());
                    var or = sd.Max(r => r.OrpAver);
                    dataRow2.CreateCell(7 * i + 4).SetCellValue(Math.Round((double)or, 2).ToString());
                    var sa = sd.Max(r => r.SalinityAver);
                    dataRow2.CreateCell(7 * i + 5).SetCellValue(Math.Round((double)sa, 2).ToString());
                    var ox = sd.Max(r => r.OxygenAver);
                    dataRow2.CreateCell(7 * i + 6).SetCellValue(Math.Round((double)ox, 2).ToString());
                    var cu = sd.Max(r => r.ConductivityAver);
                    dataRow2.CreateCell(7 * i + 7).SetCellValue(Math.Round((double)cu, 2).ToString());
                }
            }
            #endregion

            #region 最大值时间
            IRow dataRow3 = sheet.CreateRow(j + 4);
            dataRow3.CreateCell(0).SetCellValue("最大值时间");
            for (int i = 0; i < station.Count; i++)
            {
                IGrouping<int?, WebWaterQuality> sd = stationData.Where(r => r.Key == station[i].StationId).FirstOrDefault();
                if (sd == null)
                {
                    dataRow3.CreateCell(7 * i + 1).SetCellValue("--");
                    dataRow3.CreateCell(7 * i + 2).SetCellValue("--");
                    dataRow3.CreateCell(7 * i + 3).SetCellValue("--");
                    dataRow3.CreateCell(7 * i + 4).SetCellValue("--");
                    dataRow3.CreateCell(7 * i + 5).SetCellValue("--");
                    dataRow3.CreateCell(7 * i + 6).SetCellValue("--");
                    dataRow3.CreateCell(7 * i + 7).SetCellValue("--");
                }
                else
                {
                    var cl = ((DateTime)sd.Where(r => r.ClAver == sd.Max(r => r.ClAver))?.Select(r => r.UpdateTime).FirstOrDefault()).ToString("yyyy-MM");
                    dataRow3.CreateCell(7 * i + 1).SetCellValue(cl == null ? "--" : cl.ToString());
                    var tb = ((DateTime)sd.Where(r => r.TurbidityAver == sd.Max(r => r.TurbidityAver))?.Select(r => r.UpdateTime).FirstOrDefault()).ToString("yyyy-MM");
                    dataRow3.CreateCell(7 * i + 2).SetCellValue(tb == null ? "--" : tb.ToString());
                    var ph = ((DateTime)sd.Where(r => r.PhAver == sd.Max(r => r.PhAver))?.Select(r => r.UpdateTime).FirstOrDefault()).ToString("yyyy-MM");
                    dataRow3.CreateCell(7 * i + 3).SetCellValue(ph == null ? "--" : ph.ToString());
                    var or = ((DateTime)sd.Where(r => r.OrpAver == sd.Max(r => r.OrpAver))?.Select(r => r.UpdateTime).FirstOrDefault()).ToString("yyyy-MM");
                    dataRow3.CreateCell(7 * i + 4).SetCellValue(or == null ? "--" : or.ToString());
                    var sa = ((DateTime)sd.Where(r => r.SalinityAver == sd.Max(r => r.SalinityAver))?.Select(r => r.UpdateTime).FirstOrDefault()).ToString("yyyy-MM");
                    dataRow3.CreateCell(7 * i + 5).SetCellValue(sa == null ? "--" : sa.ToString());
                    var ox = ((DateTime)sd.Where(r => r.OxygenAver == sd.Max(r => r.OxygenAver))?.Select(r => r.UpdateTime).FirstOrDefault()).ToString("yyyy-MM");
                    dataRow3.CreateCell(7 * i + 6).SetCellValue(ox == null ? "--" : ox.ToString());
                    var cu = ((DateTime)sd.Where(r => r.ConductivityAver == sd.Max(r => r.ConductivityAver))?.Select(r => r.UpdateTime).FirstOrDefault()).ToString("yyyy-MM");
                    dataRow3.CreateCell(7 * i + 7).SetCellValue(cu == null ? "--" : cu.ToString());
                }
            }
            #endregion
            #region 最小值
            IRow dataRow4 = sheet.CreateRow(j + 5);
            dataRow4.CreateCell(0).SetCellValue("最小值");
            for (int i = 0; i < station.Count; i++)
            {
                IGrouping<int?, WebWaterQuality> sd = stationData.Where(r => r.Key == station[i].StationId).FirstOrDefault();
                if (sd == null)
                {
                    dataRow4.CreateCell(7 * i + 1).SetCellValue("--");
                    dataRow4.CreateCell(7 * i + 2).SetCellValue("--");
                    dataRow4.CreateCell(7 * i + 3).SetCellValue("--");
                    dataRow4.CreateCell(7 * i + 4).SetCellValue("--");
                    dataRow4.CreateCell(7 * i + 5).SetCellValue("--");
                    dataRow4.CreateCell(7 * i + 6).SetCellValue("--");
                    dataRow4.CreateCell(7 * i + 7).SetCellValue("--");
                }
                else
                {
                    var cl = sd.Min(r => r.ClAver);
                    dataRow4.CreateCell(7 * i + 1).SetCellValue(/*cl == null ? "--" : */Math.Round((double)cl, 2).ToString());
                    var tb = sd.Min(r => r.TurbidityAver);
                    dataRow4.CreateCell(7 * i + 2).SetCellValue(Math.Round((double)tb, 2).ToString());
                    var ph = sd.Min(r => r.PhAver);
                    dataRow4.CreateCell(7 * i + 3).SetCellValue(Math.Round((double)ph, 2).ToString());
                    var or = sd.Min(r => r.OrpAver);
                    dataRow4.CreateCell(7 * i + 4).SetCellValue(Math.Round((double)or, 2).ToString());
                    var sa = sd.Min(r => r.SalinityAver);
                    dataRow4.CreateCell(7 * i + 5).SetCellValue(Math.Round((double)sa, 2).ToString());
                    var ox = sd.Min(r => r.OxygenAver);
                    dataRow4.CreateCell(7 * i + 6).SetCellValue(Math.Round((double)ox, 2).ToString());
                    var cu = sd.Min(r => r.ConductivityAver);
                    dataRow4.CreateCell(7 * i + 7).SetCellValue(Math.Round((double)cu, 2).ToString());

                }
            }
            #endregion
            #region 最小值时间
            IRow dataRow5 = sheet.CreateRow(j + 6);
            dataRow5.CreateCell(0).SetCellValue("最小值时间");
            for (int i = 0; i < station.Count; i++)
            {
                IGrouping<int?, WebWaterQuality> sd = stationData.Where(r => r.Key == station[i].StationId).FirstOrDefault();
                if (sd == null)
                {
                    dataRow5.CreateCell(7 * i + 1).SetCellValue("--");
                    dataRow5.CreateCell(7 * i + 2).SetCellValue("--");
                    dataRow5.CreateCell(7 * i + 3).SetCellValue("--");
                    dataRow5.CreateCell(7 * i + 4).SetCellValue("--");
                    dataRow5.CreateCell(7 * i + 5).SetCellValue("--");
                    dataRow5.CreateCell(7 * i + 6).SetCellValue("--");
                    dataRow5.CreateCell(7 * i + 7).SetCellValue("--");
                }
                else
                {
                    var cl = ((DateTime)sd.Where(r => r.ClAver == sd.Min(r => r.ClAver))?.Select(r => r.UpdateTime).FirstOrDefault()).ToString("yyyy-MM");
                    dataRow5.CreateCell(7 * i + 1).SetCellValue(cl == null ? "--" : cl.ToString());
                    var tb = ((DateTime)sd.Where(r => r.TurbidityAver == sd.Min(r => r.TurbidityAver))?.Select(r => r.UpdateTime).FirstOrDefault()).ToString("yyyy-MM");
                    dataRow5.CreateCell(7 * i + 2).SetCellValue(tb == null ? "--" : tb.ToString());
                    var ph = ((DateTime)sd.Where(r => r.PhAver == sd.Min(r => r.PhAver))?.Select(r => r.UpdateTime).FirstOrDefault()).ToString("yyyy-MM");
                    dataRow5.CreateCell(7 * i + 3).SetCellValue(ph == null ? "--" : ph.ToString());
                    var or = ((DateTime)sd.Where(r => r.OrpAver == sd.Min(r => r.OrpAver))?.Select(r => r.UpdateTime).FirstOrDefault()).ToString("yyyy-MM");
                    dataRow5.CreateCell(7 * i + 4).SetCellValue(or == null ? "--" : or.ToString());
                    var sa = ((DateTime)sd.Where(r => r.SalinityAver == sd.Min(r => r.SalinityAver))?.Select(r => r.UpdateTime).FirstOrDefault()).ToString("yyyy-MM");
                    dataRow5.CreateCell(7 * i + 5).SetCellValue(sa == null ? "--" : sa.ToString());
                    var ox = ((DateTime)sd.Where(r => r.OxygenAver == sd.Min(r => r.OxygenAver))?.Select(r => r.UpdateTime).FirstOrDefault()).ToString("yyyy-MM");
                    dataRow5.CreateCell(7 * i + 6).SetCellValue(ox == null ? "--" : ox.ToString());
                    var cu = ((DateTime)sd.Where(r => r.ConductivityAver == sd.Min(r => r.ConductivityAver))?.Select(r => r.UpdateTime).FirstOrDefault()).ToString("yyyy-MM");
                    dataRow5.CreateCell(7 * i + 7).SetCellValue(cu == null ? "--" : cu.ToString());
                }
            }
            #endregion
            for (int ro = 2; ro < sdata.Count + 7; ro++)
            {
                sheet.GetRow(ro).Height = 20 * 20;
                for (int s = 0; s <= station.Count * 7; s++)
                {
                    sheet.GetRow(ro).Cells[s].CellStyle = style;
                }
            }
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            workbook.Write(ms);
            workbook = null;
            ms.Close();
            ms.Dispose();
            return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "水质年表" + beginDate + ".xls");
        }
        #endregion

        #region 树查询
        //泵房树形
        public string TreesOfStationDevice(SysUser sysUser, string stationName)
        {
            List<StationAndDevice> treelist = deviceInfo02Service.QueryZtreeInfo(sysUser, stationName).ToList();

            IEnumerable<TreeAction> ztreeStation = treelist.Select(t => new TreeAction
            {
                id = t.StationId,
                pId = 0,
                name = "<em class='iconfont icon-bengfang'></em>" + t.StationName,
                @checked = false,
                isDevice = false
                //icon = "../../Content/zTree/css/zTreeStyle/area.png"
            });

            //IEnumerable<TreeAction> ztreeDevice = treelist.Select(t => new TreeAction
            //{
            //    id = t.DeviceId,
            //    pId = t.StationId,
            //    name = t.DeviceName,
            //    @checked = false,
            //    isDevice = true
            //    //icon = "../../Content/zTree/css/zTreeStyle/area.png"
            //});
            var treeList = ztreeStation.Distinct(new TreeAreaCompare());
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(treeList);
            return json;
        }
        //树形查询
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult SelectTree(string stationName)
        {
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var sysUser = userService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();

            var json = TreesOfStationDevice(sysUser, stationName);
            var str = new HtmlString(json);
            return Content(str.ToString());
        }
        #endregion
    }
    public class PreChar
    {
        public string Name { get; set; }
        public List<string> Data { get; set; }
    }
}