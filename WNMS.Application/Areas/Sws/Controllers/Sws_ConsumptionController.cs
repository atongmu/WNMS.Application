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
    public class Sws_ConsumptionController : Controller
    {
        private ISws_StationService _StationService = null;
        private ISysUserService _userService = null;
        public Sws_ConsumptionController(ISws_StationService sws_StationService,ISysUserService sysUserService)
        {
            _StationService = sws_StationService;
            _userService = sysUserService;
        }
        public IActionResult Index()
        {
            ViewBag.treenodes = new HtmlString(GetStationTree(""));
            ViewBag.BeginTimeDay = DateTime.Now.ToString("yyyy-MM-dd");
            ViewBag.BeginTimeMonth= DateTime.Now.ToString("yyyy-MM");
            ViewBag.BeginTimeYear= DateTime.Now.ToString("yyyy");
            ViewBag.BeginTime = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd");
            ViewBag.EndTime = DateTime.Now.ToString("yyyy-MM-dd");
            return View();
        }
        #region 数据查询
        [TypeFilter(typeof(IgonreActionFilter))]
        public async Task<IActionResult> QueryConsumpData(string type,string begindate,string enddate,int stationid)
        {
            DateTime beginTime=new DateTime(), endTime=new DateTime();
            string dataTable = "";
            IEnumerable<dynamic> datalist;
            string tablename = "";
            string timeformate = "";
            var deciceList = _StationService.GetDeviceNameOfCon(stationid);
            if (deciceList.Count() > 0)
            {
                var deviceidList = deciceList.Select(r => r.DeviceID);
                string deviceids = string.Join(",", deviceidList);
                if (type == "day")
                {
                    beginTime = Convert.ToDateTime(begindate);
                    endTime = beginTime.AddDays(1);
                    tablename = "DHourQuartZ01";
                    timeformate = "HH:mm";

                   
                   

                }
                else if (type == "month")
                {
                    beginTime = Convert.ToDateTime(begindate + "-01");
                    endTime = beginTime.AddMonths(1);
                    tablename = "DDayQuartZ01";
                    timeformate = "yyyy-MM-dd";
                    //datalist = _StationService.GetConDataByDeviceID(beginTime.ToString(), endTime.ToString(), deviceids, tablename);
                }
                else if (type == "year")
                {
                    beginTime = Convert.ToDateTime(begindate + "-01-01");
                    endTime = beginTime.AddYears(1);
                    tablename = "DMonthQuartZ01";
                    timeformate = "yyyy-MM";
                    //datalist = _StationService.GetConDataByDeviceID(beginTime.ToString(), endTime.ToString(), deviceids, tablename);

                }
                else//自定义
                {
                    beginTime = Convert.ToDateTime(begindate);
                    endTime = Convert.ToDateTime(enddate).AddDays(1);
                    tablename = "DDayQuartZ01";
                    timeformate = "yyyy-MM-dd";
                    //datalist = _StationService.GetConDataByDeviceID(beginTime.ToString(), endTime.ToString(), deviceids, tablename);
                }
                datalist = _StationService.GetConDataByDeviceID(beginTime.ToString(), endTime.ToString(), deviceids, tablename);
                if (type == "year")
                {
                    //只需要查询本月的数据
                    if (Convert.ToInt32(begindate) == DateTime.Now.Year)
                    {
                        var begintimethis = DateTime.Now.ToString("yyyy-MM") + "-01";
                        //if (datalist.Count() > 0)
                        //{
                        //    begintimethis = datalist.LastOrDefault().UpdateTime.AddMonths(1).ToString();
                        //}
                        var endtimethis = Convert.ToDateTime(begintimethis).AddMonths(1).ToString();
                        var thismonthData = _StationService.GetConDataByDeviceID_thisMonth(begintimethis, endtimethis, deviceids);
                        if (thismonthData.Count() > 0)
                        {
                            datalist = datalist.Union(thismonthData);
                        }
                    }
                }
                if (datalist.Count() > 0)
                {
                    ViewBag.timeformate = timeformate;
                    ViewBag.deviceidList = deciceList;
                    PartialView("_consumpData_New", datalist);
                    dataTable = await ViewToString.RenderPartialViewToString(this, "_consumpData_New");
                }
                else
                {
                    dataTable = "";
                }
               
            }
            else
            {
                dataTable = "";
            }
            return Json(new
            {
                dataTable = dataTable

            });
        }

        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult GetEchartData(string type, string begindate, string enddate, int stationid)
        {
            List<string> timelist = new List<string>();
            List<string> flowlist = new List<string>();
            List<string> energylist = new List<string>();
            List<string> consumplist = new List<string>();
            DateTime beginTime = new DateTime(), endTime = new DateTime();
            IEnumerable<dynamic> datalist;
            string tablename = "";
            string timeformate = "";
            var deciceList = _StationService.GetDeviceNameOfCon(stationid);
            List<PreChar> seriesList = new List<PreChar>();
            List<string> lengnds = new List<string>();
            if (deciceList.Count() > 0)
            {
                var deviceidList = deciceList.Select(r => r.DeviceID);
                string deviceids = string.Join(",", deviceidList);
                if (type == "day")
                {
                    beginTime = Convert.ToDateTime(begindate);
                    endTime = beginTime.AddDays(1);
                    tablename = "HourQuartZ";
                    timeformate = "HH:mm";
                    tablename = "DHourQuartZ01";        

                    

                }
                else if (type == "month")
                {
                    beginTime = Convert.ToDateTime(begindate + "-01");
                    endTime = beginTime.AddMonths(1);
                    tablename = "DDayQuartZ01";
                    timeformate = "yyyy-MM-dd";
                }
                else if (type == "year")
                {
                    beginTime = Convert.ToDateTime(begindate + "-01-01");
                    endTime = beginTime.AddYears(1);
                    tablename = "DMonthQuartZ01";
                    timeformate = "yyyy-MM";

                }
                else//自定义
                {
                    beginTime = Convert.ToDateTime(begindate);
                    endTime = Convert.ToDateTime(enddate).AddDays(1);
                    tablename = "DDayQuartZ01";
                    timeformate = "yyyy-MM-dd";
                }
                
                    datalist = _StationService.GetConDataByDeviceID(beginTime.ToString(), endTime.ToString(), deviceids, tablename);
               
                if (type == "year")
                {
                    var begintimethis = DateTime.Now.ToString("yyyy-MM") + "-01";
                    if (datalist.Count() > 0)
                    {
                        begintimethis = datalist.LastOrDefault().UpdateTime.AddMonths(1).ToString();
                    }
                    var endtimethis = Convert.ToDateTime(begintimethis).AddMonths(1).ToString();
                    var thismonthData = _StationService.GetConDataByDeviceID_thisMonth(begintimethis, endtimethis, deviceids);
                    if (thismonthData.Count() > 0)
                    {
                        datalist = datalist.Union(thismonthData);
                    }

                }
                if (datalist.Count() > 0)
                {
                    var tdata = datalist.GroupBy(r => r.UpdateTime).OrderBy(r => r.Key).ToList();
                   
                    foreach (var item in tdata)
                    {
                        timelist.Add(item.Key.ToString(timeformate));
                       
                    }
                    foreach (var item in deciceList)
                    {
                        PreChar p_f = new PreChar();
                        p_f.Name = item.deviceName + "用水量(m³)";
                        p_f.Data = new List<string>();
                        lengnds.Add(item.deviceName + "用水量(m³)");

                        PreChar p_e = new PreChar();
                        p_e.Name = item.deviceName + "用电量(kW·h)";
                        p_e.Data = new List<string>();
                        lengnds.Add(item.deviceName + "用电量(kW·h)");


                        PreChar p_c = new PreChar();
                        p_c.Name = item.deviceName + "吨水电耗(kW·h/m³)";
                        p_c.Data = new List<string>();
                        lengnds.Add(item.deviceName + "吨水电耗(kW·h/m³)");


                        foreach (var it in tdata)
                        {
                            var datat = it.Where(r => r.DeviceID == item.DeviceID).FirstOrDefault();
                            if (datat == null)
                            {
                                p_f.Data.Add("");
                                p_e.Data.Add("");
                                p_c.Data.Add("");
                            }
                            else
                            {
                                p_f.Data.Add(datat.FlowCon.ToString() == "" ? "" : Math.Round((double)datat.FlowCon, 2).ToString());
                                p_e.Data.Add(datat.EnergyCon.ToString() == "" ? "" : Math.Round((double)datat.EnergyCon, 2).ToString());
                                p_c.Data.Add(datat.consump.ToString() == "" ? "" : Math.Round((double)datat.consump, 2).ToString());

                            }
                        
                        }
                        seriesList.Add(p_f);
                        seriesList.Add(p_e);
                        seriesList.Add(p_c);
                    }


                    //foreach (var item in datalist)
                    //{
                    //    timelist.Add(item.UpdateTime.ToString(timeformate));
                    //    flowlist.Add(item.FlowCon.ToString() == "" ? null : Math.Round((double)item.FlowCon, 2).ToString());
                    //    energylist.Add(item.EnergyCon.ToString() == "" ? null : Math.Round((double)item.EnergyCon, 2).ToString());
                    //    consumplist.Add(item.consump.ToString() == "" ? null : Math.Round((double)item.consump, 2).ToString());
                    //}
                }
                else
                {

                }
            }
            else
            { 
            
            }
            return Json(new
            {
                timelist = timelist,
                seriesList = seriesList,
                lengnds= lengnds
            }); 
        }

        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult ExportTable(string type, string begindate, string enddate, int stationid,string stationname)
        {
            DateTime beginTime = new DateTime(), endTime = new DateTime();
            IEnumerable<dynamic> list;
            string tablename = "";
            string timeformate = "";
            string filename = "";
            var index = stationname.IndexOf("</em>");
            var deciceList = _StationService.GetDeviceNameOfCon(stationid).ToList();
            if (deciceList.Count> 0)
            {
                var deviceidList = deciceList.Select(r => r.DeviceID);
                string deviceids = string.Join(",", deviceidList);
                if (type == "day")
                {
                    beginTime = Convert.ToDateTime(begindate);
                    endTime = beginTime.AddDays(1);
                    tablename = "DHourQuartZ01";
                    timeformate = "HH:mm";
                    filename = stationname.Substring(index + 5) + " " + begindate + "单吨电耗日报";

                }
                else if (type == "month")
                {
                    beginTime = Convert.ToDateTime(begindate + "-01");
                    endTime = beginTime.AddMonths(1);
                    tablename = "DDayQuartZ01";
                    timeformate = "yyyy-MM-dd";
                    filename = stationname.Substring(index + 5) + " " + begindate + "单吨电耗月报";
                }
                else if (type == "year")
                {
                    beginTime = Convert.ToDateTime(begindate + "-01-01");
                    endTime = beginTime.AddYears(1);
                    tablename = "DMonthQuartZ01";
                    timeformate = "yyyy-MM";
                    filename = stationname.Substring(index + 5) + " " + begindate + "单吨电耗年报";
                }
                else//自定义
                {
                    beginTime = Convert.ToDateTime(begindate);
                    endTime = Convert.ToDateTime(enddate).AddDays(1);
                    tablename = "DDayQuartZ01";
                    timeformate = "yyyy-MM-dd";
                    filename = stationname.Substring(index + 5) + " " + begindate + "到" + enddate + "单吨电耗报表";
                }
               
                    list = _StationService.GetConDataByDeviceID(beginTime.ToString(), endTime.ToString(), deviceids, tablename);
               
                if (type == "year")
                {
                    if (DateTime.Now.Year.ToString() == begindate)
                    {
                        var begintimethis = DateTime.Now.ToString("yyyy-MM") + "-01";
                        //if (list.Count() > 0)
                        //{
                        //    begintimethis = list.LastOrDefault().UpdateTime.AddMonths(1).ToString();
                        //}
                        var endtimethis = Convert.ToDateTime(begintimethis).AddMonths(1).ToString();
                        var thismonthData = _StationService.GetConDataByDeviceID_thisMonth(begintimethis, endtimethis, deviceids);
                        if (thismonthData.Count() > 0)
                        {
                            list = list.Union(thismonthData);
                        }
                    }

                }
                if (list.Count() > 0)
                {
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
                    IRow head = sheet.CreateRow(0);
                    IRow headrow = sheet.CreateRow(1);
                    for (int i = 0; i <= deciceList.Count * 3; i++)
                    {
                        head.CreateCell(i);
                        head.Cells[i].CellStyle = style1;
                    }
                    head.Height = 31 * 20;
                    head.Cells[0].SetCellValue("时间");
                    sheet.SetColumnWidth(0, 20 * 256);
                    for (var i = 0; i < deciceList.Count; i++)
                    {
                        head.Cells[1 + i * 3].SetCellValue(deciceList[i].deviceName);

                        headrow.CreateCell(1 + 3 * i).SetCellValue("用水量(m³)");
                        sheet.SetColumnWidth(1 + 3 * i, 15 * 256);
                        headrow.CreateCell(2 + 3 * i).SetCellValue("用电量(kW·h)");
                        sheet.SetColumnWidth(2 + 3 * i, 15 * 256);
                        headrow.CreateCell(3 + 3 * i).SetCellValue("吨水电耗(kW·h/m³)");
                        sheet.SetColumnWidth(3 + 3 * i, 15 * 256);
                       
                    }
                    //表头第二行样式
                    for (int i = 0; i < deciceList.Count * 3; i++)
                    {
                        headrow.Cells[i].CellStyle = style1;
                    }
                    //合并单元格(最后合并单元格)
                    sheet.AddMergedRegion(new CellRangeAddress(0, 1, 0, 0));
                    for (var i = 0; i < deciceList.Count; i++)
                    {
                        sheet.AddMergedRegion(new CellRangeAddress(0, 0, 1 + i * 3, 3 + i * 3));
                    }
                    #region 最值填充
                    var DeviceData_Head = list.GroupBy(r => r.DeviceID).ToList();
                    var  dataRow = sheet.CreateRow(2);
                    dataRow.Height = 100 * 4;
                    dataRow.CreateCell(0).SetCellValue("最大值");
                    for (int i = 0; i < deciceList.Count; i++)
                    {
                        var  sd = DeviceData_Head.Where(r => r.Key == deciceList[i].DeviceID).FirstOrDefault();
                        if (sd == null)
                        {
                            dataRow.CreateCell(3 * i + 1).SetCellValue("--");
                            dataRow.CreateCell(3 * i + 2).SetCellValue("--");
                            dataRow.CreateCell(3 * i + 3).SetCellValue("--");
                            
                        }
                        else
                        {
                            var maxflow = sd.Max(r => r.FlowCon);
                            var maxenergy = sd.Max(r => r.EnergyCon);
                            var maxconsump = sd.Where(r => r.consump.ToString() != "")?.Max(r => r.consump);
                            dataRow.CreateCell(1+3*i).SetCellValue(maxflow == null ? "--" : Math.Round((double)maxflow, 2).ToString());
                            dataRow.CreateCell(2+3*i).SetCellValue(maxenergy == null ? "--" : Math.Round((double)maxenergy, 2).ToString());
                            dataRow.CreateCell(3+3*i).SetCellValue(maxconsump == null ? "--" : Math.Round((double)maxconsump, 2).ToString());
                           
                        }
                    }

                    dataRow = sheet.CreateRow(3);
                    dataRow.Height = 100 * 4;
                    dataRow.CreateCell(0).SetCellValue("最大时间");
                    for (int i = 0; i < deciceList.Count; i++)
                    {
                        var sd = DeviceData_Head.Where(r => r.Key == deciceList[i].DeviceID).FirstOrDefault();
                        if (sd == null)
                        {
                            dataRow.CreateCell(3 * i + 1).SetCellValue("--");
                            dataRow.CreateCell(3 * i + 2).SetCellValue("--");
                            dataRow.CreateCell(3 * i + 3).SetCellValue("--");

                        }
                        else
                        {
                            var maxflow = sd.Max(r => r.FlowCon);
                            var maxenergy = sd.Max(r => r.EnergyCon);
                            var maxconsump = sd.Where(r => r.consump.ToString() != "").Max(r => r.consump);
                            var maxflowtime = sd.Where(r => r.FlowCon == maxflow).FirstOrDefault();
                            var maxenergytime = sd.Where(r => r.EnergyCon == maxenergy).FirstOrDefault();
                            var maxconsumptime = sd.Where(r => r.consump.ToString() != "" && r.consump == maxconsump).FirstOrDefault();
                            dataRow.CreateCell(3 * i + 1).SetCellValue(maxflowtime == null ? "--" : maxflowtime.UpdateTime.ToString(timeformate));
                            dataRow.CreateCell(3 * i + 2).SetCellValue(maxenergytime == null ? "--" : maxenergytime.UpdateTime.ToString(timeformate));
                            dataRow.CreateCell(3 * i + 3).SetCellValue(maxconsumptime == null ? "--" : maxconsumptime.UpdateTime.ToString(timeformate));

                        }
                    }
                   

                    dataRow = sheet.CreateRow(4);
                    dataRow.Height = 100 * 4;
                    dataRow.CreateCell(0).SetCellValue("最小值");
                    for (int i = 0; i < deciceList.Count; i++)
                    {
                        var sd = DeviceData_Head.Where(r => r.Key == deciceList[i].DeviceID).FirstOrDefault();
                        if (sd == null)
                        {
                            dataRow.CreateCell(3 * i + 1).SetCellValue("--");
                            dataRow.CreateCell(3 * i + 2).SetCellValue("--");
                            dataRow.CreateCell(3 * i + 3).SetCellValue("--");

                        }
                        else
                        {
                            var minflow = sd.Min(r => r.FlowCon);
                            var minenergy = sd.Min(r => r.EnergyCon);
                            var minconsump = sd.Where(r => r.consump.ToString() != "")?.Min(r => r.consump);
                            dataRow.CreateCell(3 * i + 1).SetCellValue(minflow == null ? "--" : Math.Round((double)minflow, 2).ToString());
                            dataRow.CreateCell(3 * i + 2).SetCellValue(minenergy == null ? "--" : Math.Round((double)minenergy, 2).ToString());
                            dataRow.CreateCell(3 * i + 3).SetCellValue(minconsump == null ? "--" : Math.Round((double)minconsump, 2).ToString());

                        }
                    }


                    dataRow = sheet.CreateRow(5);
                    dataRow.Height = 100 * 4;
                    dataRow.CreateCell(0).SetCellValue("最小时间");
                    for (int i = 0; i < deciceList.Count; i++)
                    {
                        var sd = DeviceData_Head.Where(r => r.Key == deciceList[i].DeviceID).FirstOrDefault();
                        if (sd == null)
                        {
                            dataRow.CreateCell(3 * i + 1).SetCellValue("--");
                            dataRow.CreateCell(3 * i + 2).SetCellValue("--");
                            dataRow.CreateCell(3 * i + 3).SetCellValue("--");

                        }
                        else
                        {
                            var minflow = sd.Min(r => r.FlowCon);
                            var minenergy = sd.Min(r => r.EnergyCon);
                            var minconsump = sd.Where(r => r.consump.ToString() != "").Min(r => r.consump);
                            var minflowtime = sd.Where(r => r.FlowCon == minflow).FirstOrDefault();
                            var minenergytime = sd.Where(r => r.EnergyCon == minenergy).FirstOrDefault();
                            var minconsumptime = sd.Where(r => r.consump.ToString() != "" && r.consump == minconsump).FirstOrDefault();
                            dataRow.CreateCell(1+ 3 * i).SetCellValue(minflowtime == null ? "--" : minflowtime.UpdateTime.ToString(timeformate));
                            dataRow.CreateCell(2+ 3 * i).SetCellValue(minenergytime == null ? "--" : minenergytime.UpdateTime.ToString(timeformate));
                            dataRow.CreateCell(3+ 3 * i).SetCellValue(minconsumptime == null ? "--" : minconsumptime.UpdateTime.ToString(timeformate));

                        }
                    }

                    

                    dataRow = sheet.CreateRow(6);
                    dataRow.Height = 100 * 4;
                    dataRow.CreateCell(0).SetCellValue("平均值");
                    for (int i = 0; i < deciceList.Count; i++)
                    {
                        var sd = DeviceData_Head.Where(r => r.Key == deciceList[i].DeviceID).FirstOrDefault();
                        if (sd == null)
                        {
                            dataRow.CreateCell(3 * i + 1).SetCellValue("--");
                            dataRow.CreateCell(3 * i + 2).SetCellValue("--");
                            dataRow.CreateCell(3 * i + 3).SetCellValue("--");

                        }
                        else
                        {
                            var aveFlist = sd.Where(r => r.FlowCon.ToString() != "");
                            var aveElist = sd.Where(r => r.EnergyCon.ToString() != "");
                            var aveClist = sd.Where(r => r.consump.ToString() != "");

                           
                            dataRow.CreateCell(3 * i + 1).SetCellValue(aveFlist.Count() == 0 ? "--" : Math.Round((double)aveFlist.Average(r => (double)r.FlowCon), 2).ToString());
                            dataRow.CreateCell(3 * i + 2).SetCellValue(aveElist.Count() == 0 ? "--" : Math.Round((double)aveElist.Average(r => (double)r.EnergyCon), 2).ToString());
                            dataRow.CreateCell(3 * i + 3).SetCellValue(aveClist.Count() == 0 ? "--" : Math.Round((double)aveClist.Average(r => (double)r.consump), 2).ToString());

                        }
                    }
                    

                    #endregion
                    int j = 7;
                    var data = list.GroupBy(r => r.UpdateTime).OrderBy(r => r.Key).ToList();
                    foreach (var item in data)
                    {
                        dataRow = sheet.CreateRow(j);
                        dataRow.Height = 100 * 4;
                        dataRow.CreateCell(0).SetCellValue(item.Key.ToString(timeformate));
                        for (int i = 0; i < deciceList.Count; i++)
                        {
                            var sd = item.Where(r => r.DeviceID == deciceList[i].DeviceID).FirstOrDefault();
                            if (sd == null)
                            {
                                dataRow.CreateCell(3 * i + 1).SetCellValue("--");
                                dataRow.CreateCell(3 * i + 2).SetCellValue("--");
                                dataRow.CreateCell(3 * i + 3).SetCellValue("--");

                            }
                            else
                            {
                             
                                dataRow.CreateCell(3 * i + 1).SetCellValue(sd.FlowCon.ToString() == "" ? "--" : Math.Round((double)sd.FlowCon, 2).ToString());
                                dataRow.CreateCell(3 * i + 2).SetCellValue(sd.EnergyCon.ToString() == "" ? "--" : Math.Round((double)sd.EnergyCon, 2).ToString());
                                dataRow.CreateCell(3 * i + 3).SetCellValue(sd.consump.ToString() == "" ? "--" : Math.Round((double)sd.consump, 2).ToString());

                            }
                        }

                        

                        j++;
                    }

                    for (int ro = 2; ro < data.Count + 7; ro++)
                    {
                        sheet.GetRow(ro).Height = 20 * 20;
                        for (int s = 0; s <=deciceList.Count * 3; s++)
                        {
                            sheet.GetRow(ro).Cells[s].CellStyle = style;
                        }
                    }
                    System.IO.MemoryStream ms = new System.IO.MemoryStream();
                    workbook.Write(ms);
                    workbook = null;
                    ms.Close();
                    ms.Dispose();
                    return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "" + filename + ".xls");
                }
                else
                {
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
                    IRow head = sheet.CreateRow(0);
                    IRow headrow = sheet.CreateRow(1);
                    for (int i = 0; i <= deciceList.Count * 3; i++)
                    {
                        head.CreateCell(i);
                        head.Cells[i].CellStyle = style1;
                    }
                    head.Height = 31 * 20;
                    head.Cells[0].SetCellValue("时间");
                    sheet.SetColumnWidth(0, 20 * 256);
                    for (var i = 0; i < deciceList.Count; i++)
                    {
                        head.Cells[1 + i * 3].SetCellValue(deciceList[i].deviceName);

                        headrow.CreateCell(1 + 3 * i).SetCellValue("用水量(m³)");
                        sheet.SetColumnWidth(1 + 3 * i, 15 * 256);
                        headrow.CreateCell(2 + 3 * i).SetCellValue("用电量(kW·h)");
                        sheet.SetColumnWidth(2 + 3 * i, 15 * 256);
                        headrow.CreateCell(3 + 3 * i).SetCellValue("吨水电耗(kW·h/m³)");
                        sheet.SetColumnWidth(3 + 3 * i, 15 * 256);

                    }
                    //表头第二行样式
                    for (int i = 0; i < deciceList.Count * 3; i++)
                    {
                        headrow.Cells[i].CellStyle = style1;
                    }
                    //合并单元格(最后合并单元格)
                    sheet.AddMergedRegion(new CellRangeAddress(0, 1, 0, 0));
                    for (var i = 0; i < deciceList.Count; i++)
                    {
                        sheet.AddMergedRegion(new CellRangeAddress(0, 0, 1 + i * 3, 3 + i * 3));
                    }
                    System.IO.MemoryStream ms = new System.IO.MemoryStream();
                    workbook.Write(ms);
                    workbook = null;
                    ms.Close();
                    ms.Dispose();
                    return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "" + filename + ".xls");
                }
            }
            else
            {
                HSSFWorkbook workbook = new HSSFWorkbook();
                ISheet sheet = workbook.CreateSheet();
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                workbook.Write(ms);
                workbook = null;
                ms.Close();
                ms.Dispose();
                return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "" + filename + ".xls");
            }
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
            var data = _StationService.GetStation_consumption(user.IsAdmin, userID, stationname,1);
            if (data.Count() > 0)
            {
                var list = data.Select(r => new
                {
                    id = r.StationID,
                    pId = 0,
                    name = "<em class='iconfont icon-bengfang'></em>"+ r.StationName,
                    icon = "/images/stationTree.png"
                }).OrderBy(r => r.name);
                return Newtonsoft.Json.JsonConvert.SerializeObject(list);
            }
            else
            {
                return "[]";
            }
        }
        #endregion
        public class PreChar
        {
            public string Name { get; set; }
            public List<string> Data { get; set; }
        }
    }
}