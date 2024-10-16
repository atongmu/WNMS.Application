using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WNMS.Application.Utility;
using WNMS.Application.Utility.Filters;
using WNMS.IService;
using WNMS.Model.DataModels;

namespace WNMS.Application.Areas.Sws.Controllers
{
    [Area("Sws")]
    [TypeFilter(typeof(IgonreLoginFilter))]
    [TypeFilter(typeof(IgonreActionFilter))]
    public class AppScreen_ConsumptionController : Controller
    {
        private ISws_StationService _StationService = null;
        private ISysUserService _userService = null;
        public AppScreen_ConsumptionController(ISws_StationService sws_StationService, ISysUserService sysUserService)
        {
            _StationService = sws_StationService;
            _userService = sysUserService;
        }
        public IActionResult Index()
        {
            return View();
        }

        #region  能耗分析
        public IActionResult EnergyConsumption(int stationId,string token)
        {
            SysUser user = _userService.Query<SysUser>(r => r.Token == token).FirstOrDefault();
            if (user != null)
            {
                ViewBag.BeginTimeDay = DateTime.Now.ToString("yyyy-MM-dd");
                ViewBag.BeginTimeMonth = DateTime.Now.ToString("yyyy-MM");
                ViewBag.BeginTimeYear = DateTime.Now.ToString("yyyy");
                ViewBag.BeginTime = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd");
                ViewBag.EndTime = DateTime.Now.ToString("yyyy-MM-dd");
                ViewBag.StationId = stationId;
                return View();
            }
            else
            {
                return View("Index");
            }
        }

        //表格数据获取
        [TypeFilter(typeof(IgonreActionFilter))]
        public async Task<IActionResult> QueryConsumpData(string type, string begindate, string enddate, int stationid)
        {
            DateTime beginTime = new DateTime(), endTime = new DateTime();
            string dataTable = "";
            IEnumerable<dynamic> datalist;
            string tablename = "";
            string timeformate = "";
            var deciceList = _StationService.GetDeviceNameOfCon(stationid);
            if (deciceList.Count() > 0)
            {
                var deviceidList = deciceList.Select(r => r.DeviceID);
                string deviceids = string.Join(",", deviceidList);
                if (type == "按天")
                {
                    beginTime = Convert.ToDateTime(begindate);
                    endTime = beginTime.AddDays(1);
                    tablename = "DHourQuartZ01";
                    timeformate = "HH:mm";
                }
                else if (type == "按月")
                {
                    beginTime = Convert.ToDateTime(begindate + "-01");
                    endTime = beginTime.AddMonths(1);
                    tablename = "DDayQuartZ01";
                    timeformate = "yyyy-MM-dd";
                }
                else if (type == "按年")
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
                if (type == "按年")
                  {
                    //只需要查询本月的数据
                    if (Convert.ToInt32(begindate) == DateTime.Now.Year)
                    {
                        var begintimethis = DateTime.Now.ToString("yyyy-MM") + "-01";
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
                    PartialView("_energyTable", datalist);
                    dataTable = await ViewToString.RenderPartialViewToString(this, "_energyTable");
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
                if (type == "按天")
                {
                    beginTime = Convert.ToDateTime(begindate);
                    endTime = beginTime.AddDays(1);
                    tablename = "HourQuartZ";
                    timeformate = "HH:mm";
                    tablename = "DHourQuartZ01";
                }
                else if (type == "按月")
                {
                    beginTime = Convert.ToDateTime(begindate + "-01");
                    endTime = beginTime.AddMonths(1);
                    tablename = "DDayQuartZ01";
                    timeformate = "yyyy-MM-dd";
                }
                else if (type == "按年")
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

                if (type == "按年")
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
                        PreChar p_e = new PreChar();
                        p_e.Name = item.deviceName + "用电量(kW·h)";
                        p_e.Data = new List<string>();
                        lengnds.Add(item.deviceName + "用电量(kW·h)");
                        foreach (var it in tdata)
                        {
                            var datat = it.Where(r => r.DeviceID == item.DeviceID).FirstOrDefault();
                            if (datat == null)
                            {
                                p_e.Data.Add(null);
                            }
                            else
                            {
                                p_e.Data.Add(datat.EnergyCon.ToString() == "" ? null : Math.Round((double)datat.EnergyCon, 2).ToString());
                            }

                        }
                        seriesList.Add(p_e);
                    }
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
                lengnds = lengnds
            });
        }
        #endregion

        #region  能耗分析
        public IActionResult WaterConsumption(int stationId,string token)
        {
            SysUser user = _userService.Query<SysUser>(r => r.Token == token).FirstOrDefault();
            if (user != null)
            {
                ViewBag.BeginTimeDay = DateTime.Now.ToString("yyyy-MM-dd");
                ViewBag.BeginTimeMonth = DateTime.Now.ToString("yyyy-MM");
                ViewBag.BeginTimeYear = DateTime.Now.ToString("yyyy");
                ViewBag.BeginTime = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd");
                ViewBag.EndTime = DateTime.Now.ToString("yyyy-MM-dd");
                ViewBag.StationId = stationId;
                return View();
            }
            else
            {
                return View("Index");
            }
        }

        //表格数据获取
        [TypeFilter(typeof(IgonreActionFilter))]
        public async Task<IActionResult> QueryWaterConsumpData(string type, string begindate, string enddate, int stationid)
        {
            DateTime beginTime = new DateTime(), endTime = new DateTime();
            string dataTable = "";
            IEnumerable<dynamic> datalist;
            string tablename = "";
            string timeformate = "";
            var deciceList = _StationService.GetDeviceNameOfCon(stationid);
            if (deciceList.Count() > 0)
            {
                var deviceidList = deciceList.Select(r => r.DeviceID);
                string deviceids = string.Join(",", deviceidList);
                if (type == "按天")
                {
                    beginTime = Convert.ToDateTime(begindate);
                    endTime = beginTime.AddDays(1);
                    tablename = "DHourQuartZ01";
                    timeformate = "HH:mm";
                }
                else if (type == "按月")
                {
                    beginTime = Convert.ToDateTime(begindate + "-01");
                    endTime = beginTime.AddMonths(1);
                    tablename = "DDayQuartZ01";
                    timeformate = "yyyy-MM-dd";
                }
                else if (type == "按年")
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
                if (type == "按年")
                {
                    //只需要查询本月的数据
                    if (Convert.ToInt32(begindate) == DateTime.Now.Year)
                    {
                        var begintimethis = DateTime.Now.ToString("yyyy-MM") + "-01";
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
                    PartialView("_waterTable", datalist);
                    dataTable = await ViewToString.RenderPartialViewToString(this, "_waterTable");
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

        //[TypeFilter(typeof(IgonreActionFilter))]
        //public IActionResult GetWaterEchartData(string type, string begindate, string enddate, int stationid)
        //{
        //    List<string> timelist = new List<string>();
        //    List<string> flowlist = new List<string>();
        //    DateTime beginTime = new DateTime(), endTime = new DateTime();
        //    IEnumerable<dynamic> datalist;
        //    string tablename = "";
        //    string timeformate = "";
        //    var deciceList = _StationService.GetDeviceNameOfCon(stationid);
        //    List<PreChar> seriesList = new List<PreChar>();
        //    List<string> lengnds = new List<string>();
        //    if (deciceList.Count() > 0)
        //    {
        //        var deviceidList = deciceList.Select(r => r.DeviceID);
        //        string deviceids = string.Join(",", deviceidList);
        //        if (type == "day")
        //        {
        //            beginTime = Convert.ToDateTime(begindate);
        //            endTime = beginTime.AddDays(1);
        //            tablename = "HourQuartZ";
        //            timeformate = "HH:mm";
        //            tablename = "DHourQuartZ01";
        //        }
        //        else if (type == "month")
        //        {
        //            beginTime = Convert.ToDateTime(begindate + "-01");
        //            endTime = beginTime.AddMonths(1);
        //            tablename = "DDayQuartZ01";
        //            timeformate = "yyyy-MM-dd";
        //        }
        //        else if (type == "year")
        //        {
        //            beginTime = Convert.ToDateTime(begindate + "-01-01");
        //            endTime = beginTime.AddYears(1);
        //            tablename = "DMonthQuartZ01";
        //            timeformate = "yyyy-MM";

        //        }
        //        else//自定义
        //        {
        //            beginTime = Convert.ToDateTime(begindate);
        //            endTime = Convert.ToDateTime(enddate).AddDays(1);
        //            tablename = "DDayQuartZ01";
        //            timeformate = "yyyy-MM-dd";
        //        }

        //        datalist = _StationService.GetConDataByDeviceID(beginTime.ToString(), endTime.ToString(), deviceids, tablename);

        //        if (type == "year")
        //        {
        //            var begintimethis = DateTime.Now.ToString("yyyy-MM") + "-01";
        //            if (datalist.Count() > 0)
        //            {
        //                begintimethis = datalist.LastOrDefault().UpdateTime.AddMonths(1).ToString();
        //            }
        //            var endtimethis = Convert.ToDateTime(begintimethis).AddMonths(1).ToString();
        //            var thismonthData = _StationService.GetConDataByDeviceID_thisMonth(begintimethis, endtimethis, deviceids);
        //            if (thismonthData.Count() > 0)
        //            {
        //                datalist = datalist.Union(thismonthData);
        //            }

        //        }
        //        if (datalist.Count() > 0)
        //        {
        //            var tdata = datalist.GroupBy(r => r.UpdateTime).OrderBy(r => r.Key).ToList();

        //            foreach (var item in tdata)
        //            {
        //                timelist.Add(item.Key.ToString(timeformate));

        //            }
        //            foreach (var item in deciceList)
        //            {
        //                PreChar p_e = new PreChar();
        //                p_e.Name = item.deviceName + "用电量(kW·h)";
        //                p_e.Data = new List<string>();
        //                lengnds.Add(item.deviceName + "用电量(kW·h)");
        //                foreach (var it in tdata)
        //                {
        //                    var datat = it.Where(r => r.DeviceID == item.DeviceID).FirstOrDefault();
        //                    if (datat == null)
        //                    {
        //                        p_e.Data.Add(null);
        //                    }
        //                    else
        //                    {
        //                        p_e.Data.Add(datat.EnergyCon.ToString() == "" ? null : Math.Round((double)datat.EnergyCon, 2).ToString());
        //                    }

        //                }
        //                seriesList.Add(p_e);
        //            }
        //        }
        //        else
        //        {

        //        }
        //    }
        //    else
        //    {

        //    }
        //    return Json(new
        //    {
        //        timelist = timelist,
        //        seriesList = seriesList,
        //        lengnds = lengnds
        //    });
        //}

        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult GetWaterEchartData(string type, string begindate, string enddate, int stationid)
        {
            List<string> timelist = new List<string>();
            List<string> flowlist = new List<string>();
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
                if (type == "按天")
                {
                    beginTime = Convert.ToDateTime(begindate);
                    endTime = beginTime.AddDays(1);
                    tablename = "HourQuartZ";
                    timeformate = "HH:mm";
                    tablename = "DHourQuartZ01";
                }
                else if (type == "按月")
                {
                    beginTime = Convert.ToDateTime(begindate + "-01");
                    endTime = beginTime.AddMonths(1);
                    tablename = "DDayQuartZ01";
                    timeformate = "yyyy-MM-dd";
                }
                else if (type == "按年")
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

                if (type == "按年")
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

                        foreach (var it in tdata)
                        {
                            var datat = it.Where(r => r.DeviceID == item.DeviceID).FirstOrDefault();
                            if (datat == null)
                            {
                                p_f.Data.Add(null);
                            }
                            else
                            {
                                p_f.Data.Add(datat.FlowCon.ToString() == "" ? null : Math.Round((double)datat.FlowCon, 2).ToString());
                            }
                        }
                        seriesList.Add(p_f);
                    }
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
                lengnds = lengnds
            });
        }
        #endregion
    }
}
