using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WNMS.Application.Utility;
using WNMS.Application.Utility.Filters;
using WNMS.IService;
using WNMS.Model.CustomizedClass;
using WNMS.Model.DataModels;

namespace WNMS.Application.Areas.Sws.Controllers
{
    [Area("Sws")]
    #region 初始化页面 构造函数
    public class Sws_EnergyConsumpController : Controller
    {
        private IDHourQuartZ01Service _dHourQuartZ01Service = null;
        private IDDayQuartZ01Service _dDayQuartZ01Service = null;
        private IDMonthQuartZ01Service _dMonthQuartZ01Service = null;
        private ISws_StationService _stationService = null;
        private ISysUserService _userService = null;
        private ISws_DeviceInfo01Service _deviceInfo01Service = null;
        public Sws_EnergyConsumpController(IDHourQuartZ01Service dHourQuartZ01Service,
            IDDayQuartZ01Service dDayQuartZ01Service, ISws_DeviceInfo01Service deviceInfo01Service,
            IDMonthQuartZ01Service dMonthQuartZ01Service, ISws_StationService sws_StationService, ISysUserService sysUserService)
        {
            _dHourQuartZ01Service = dHourQuartZ01Service;
            _dDayQuartZ01Service = dDayQuartZ01Service;
            _dMonthQuartZ01Service = dMonthQuartZ01Service;
            _stationService = sws_StationService;
            _userService = sysUserService;
            _deviceInfo01Service = deviceInfo01Service;
        }
        public IActionResult Index()
        {
            ViewBag.treenodes = new HtmlString(GetStationTree(""));
            ViewBag.BeginTimeDay = DateTime.Now.ToString("yyyy-MM-dd");
            //ViewBag.BeginTimeDay = "2021-07-22";
            ViewBag.BeginTime = DateTime.Now.AddDays(-10).ToString("yyyy-MM-dd");
            ViewBag.EndTime = DateTime.Now.ToString("yyyy-MM-dd");
            return View();
        }
        #endregion

        #region 今日昨日本周本月能耗查询
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult GetTotalData(int type, int stationID,long deviceId)
        {
            //查询设备id
            var deciceList = _deviceInfo01Service.Query<SwsDeviceInfo01>(r => r.StationId == stationID).ToList();
            if (type == 1)
            {
                deciceList = _deviceInfo01Service.Query<SwsDeviceInfo01>(r => r.DeviceId == deviceId).ToList();
            }
            List<long> ids = new List<long>();
            if (deciceList.Count() > 0)
            {
                ids = deciceList.Select(r => r.DeviceId).ToList();
            }

            //今日能耗
            DateTime beginTime = DateTime.Now.Date;
            DateTime endTime = DateTime.Now;
            double? todayNH = _dHourQuartZ01Service.Query<DhourQuartZ01>(r => ids.Contains(r.DeviceId) && r.UpdateTime >= beginTime && r.UpdateTime < endTime)?.Sum(r => (double)r.EnergyCon);

            //昨日能耗
            beginTime = DateTime.Now.AddDays(-1).Date;
            endTime = DateTime.Now.Date;
            double? yestodayNH = _dDayQuartZ01Service.Query<DdayQuartZ01>(r => ids.Contains(r.DeviceId) && r.UpdateTime == beginTime)?.Sum(r => (double)r.EnergyCon);

            //本月能耗
            beginTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-01"));
            endTime = DateTime.Now.Date;
            double? mNH = _dDayQuartZ01Service.Query<DdayQuartZ01>(r => ids.Contains(r.DeviceId) && r.UpdateTime >= beginTime && r.UpdateTime < endTime)?.Sum(r => (double)r.EnergyCon);
            //double monthNH = mNH ?? 0.0 + todayNH ?? 0.0;
            double monthNH = mNH ?? 0.0;

            //本周能耗 
            int num = Convert.ToInt32(DateTime.Now.DayOfWeek.ToString("d"));
            beginTime = DateTime.Now.AddDays(1 - num).Date;
            endTime = DateTime.Now;
            double? wNH = _dDayQuartZ01Service.Query<DdayQuartZ01>(r => ids.Contains(r.DeviceId) && r.UpdateTime >= beginTime && r.UpdateTime < endTime)?.Sum(r => (double)r.EnergyCon);
            double weekNH = (wNH ?? 0.0) + (todayNH ?? 0.0);

            return Json(new
            {
                todayNH = Math.Round(todayNH ?? 0.0, 2),
                yestodayNH = Math.Round(yestodayNH ?? 0.0, 2),
                monthNH = Math.Round(monthNH, 2),
                weekNH = Math.Round(weekNH, 2)
            });
        }
        #endregion

        #region  查询能耗统计
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult GetDayChartData(int type,string beginDate, string endDate,int stationID,long deviceId)
        {
            //定义变量
            List<string> timelist = new List<string>();
            List<string> energylist = new List<string>();
            List<PreChar> seriesList = new List<PreChar>();
            List<string> lengnds = new List<string>();

            //时间处理
            DateTime beginTime = DateTime.Now.AddDays(-7).Date;   //如果时间为空  默认查七天内数据
            DateTime endTime = DateTime.Now.AddDays(1).Date;
            if (!string.IsNullOrEmpty(beginDate) && !string.IsNullOrEmpty(endDate))     //如果时间不为空，查时间段内数据
            {
                beginTime = Convert.ToDateTime(beginDate);
                endTime = Convert.ToDateTime(endDate);
            }

            //查询泵房设备
            List<long> ids = new List<long>();
            var deviceList = _stationService.GetDeviceNameOfCon(stationID).ToList();
            if (type == 1)
            {
                deviceList = deviceList.Where(r=>r.DeviceID== deviceId).ToList();
            }

            //数据查询及处理
            if (deviceList.Count > 0)
            {
                ids = deviceList.Select(r => (long)r.DeviceID).ToList();
                var datalist = _dDayQuartZ01Service.Query<DdayQuartZ01>(r => r.UpdateTime >= beginTime && r.UpdateTime <= endTime && ids.Contains(r.DeviceId)).ToList();

                if (datalist.Count() > 0)
                {
                    var tdata = datalist.GroupBy(r => r.UpdateTime).OrderBy(r => r.Key).ToList();

                    foreach (var item in tdata)
                    {
                        timelist.Add(item.Key.ToString("yyyy-MM-dd"));
                    }
                    foreach (var item in deviceList)        //循环分区
                    {
                        PreChar p_e = new PreChar();
                        p_e.Name = item.deviceName;
                        p_e.Data = new List<string>();
                        lengnds.Add(item.deviceName);
                        foreach (var it in tdata)             //循环分区设备数据
                        {
                            var datat = it.Where(r => r.DeviceId == item.DeviceID).FirstOrDefault();
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
            }

            return Json(new
            {
                timelist = timelist,
                seriesList = seriesList,
                lengnds = lengnds
            });
        }
        #endregion

        #region 表格统计数据
        [TypeFilter(typeof(IgonreActionFilter))]
        public async Task<IActionResult> GetTableDayData(int type,string beginDate, string endDate, int stationID,long deviceId)
        {
            //定义变量
            List<DdayQuartZ01> datalist = new List<DdayQuartZ01>();
            string dataTable = "";

            //时间处理
            DateTime beginTime = DateTime.Now.AddDays(-7).Date;   //如果时间为空  默认查七天内数据
            DateTime endTime = DateTime.Now.AddDays(1).Date;
            if (!string.IsNullOrEmpty(beginDate) && !string.IsNullOrEmpty(endDate))     //如果时间不为空，查时间段内数据
            {
                beginTime = Convert.ToDateTime(beginDate);
                endTime = Convert.ToDateTime(endDate);
            }

            //查询泵房设备
            List<long> ids = new List<long>();
            var deviceList = _stationService.GetDeviceNameOfCon(stationID).ToList();

            if (type == 1)
            {
                deviceList = deviceList.Where(r => r.DeviceID == deviceId).ToList();
            }

            //数据查询及处理
            if (deviceList.Count > 0)
            {
                ids = deviceList.Select(r => (long)r.DeviceID).ToList();
                datalist = _dDayQuartZ01Service.Query<DdayQuartZ01>(r => r.UpdateTime >= beginTime && r.UpdateTime < endTime && ids.Contains(r.DeviceId)).ToList();
                if (datalist.Count > 0)
                {
                    ViewBag.deviceidList = deviceList;
                    PartialView("_energyDayData", datalist);
                    dataTable = await ViewToString.RenderPartialViewToString(this, "_energyDayData");
                }
            }
            ViewBag.Type = type;
            return Json(new
            {
                dataTable = dataTable
            });
        }
        #endregion

        #region 分段查询
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult GetEnergySectionData(int type,string date, int timevalue, int stationId, long deviceId)
        {
            //定义变量
            List<string> timelist = new List<string>();
            List<string> energylist = new List<string>();
            List<PreChar> seriesList = new List<PreChar>();
            List<string> lengnds = new List<string>();

            //时间处理
            DateTime beginTime = DateTime.Now.AddDays(-1).Date;   //如果时间为空  默认查七天内数据
            DateTime endTime = DateTime.Now.AddDays(1).Date;
            if (!string.IsNullOrEmpty(date) && !string.IsNullOrEmpty(date))     //如果时间不为空，查时间段内数据
            {
                beginTime = Convert.ToDateTime(date);
                if (beginTime == DateTime.Now.Date)
                {
                    endTime = DateTime.Now;
                }
                else
                {
                    endTime = beginTime.AddDays(1).Date;
                }
            }

            int length = (int)(endTime - beginTime).TotalHours;
            //查询泵房设备
            List<long> ids = new List<long>();
            var deviceList = _stationService.GetDeviceNameOfCon(stationId).ToList();
            if (type == 1)
            {
                deviceList = deviceList.Where(r => r.DeviceID == deviceId).ToList();
            }

            //数据查询及处理
            if (deviceList.Count > 0)
            {
                ids = deviceList.Select(r => (long)r.DeviceID).ToList();
                var datalist = _dHourQuartZ01Service.Query<DhourQuartZ01>(r => r.UpdateTime >= beginTime && r.UpdateTime <= endTime && ids.Contains(r.DeviceId)).ToList();

                if (datalist.Count() > 0)
                {
                    for (int i = 0; i <length; i = i + timevalue)
                    {
                        beginTime = beginTime.AddHours(timevalue);
                        timelist.Add(beginTime.ToString());
                    }

                    foreach (var item in deviceList)        //循环分区
                    {
                        PreChar p_e = new PreChar();
                        p_e.Name = item.deviceName;
                        p_e.Data = new List<string>();
                        lengnds.Add(item.deviceName);
                        DateTime startTime = Convert.ToDateTime(date).AddHours(timevalue);
                        DateTime enddate = beginTime.AddHours(1);
                        for (int i = 0; i < length; i = i + timevalue)
                        {
                            enddate = startTime.AddHours(timevalue);
                            var datat = datalist.Where(r => r.DeviceId == item.DeviceID & r.UpdateTime >= startTime && r.UpdateTime < enddate).OrderBy(r=>r.UpdateTime).ToList();
                            if (datat == null)
                            {
                                p_e.Data.Add(null);
                            }
                            else
                            {
                                double value = Math.Round(datat.Sum(r => (double)r.EnergyCon), 2);
                                p_e.Data.Add(value.ToString());
                            }
                            startTime = enddate;
                        }
                        seriesList.Add(p_e);
                    }
                }
            }

            return Json(new
            {
                timelist = timelist,
                seriesList = seriesList,
                lengnds = lengnds
            });
        }

        //表格
        [TypeFilter(typeof(IgonreActionFilter))]
        public async Task<IActionResult> GetEnergySectionTable(int type,string date, int timevalue, int stationId, long deviceId )
        {
            //定义变量
            ViewBag.TimeValue = timevalue;
            string dataTable = "";

            //时间处理
            DateTime beginTime = DateTime.Now.AddDays(-1).Date;   //如果时间为空  默认查七天内数据
            DateTime endTime = DateTime.Now.AddDays(1).Date;
            if (!string.IsNullOrEmpty(date) && !string.IsNullOrEmpty(date))     //如果时间不为空，查时间段内数据
            {
                beginTime = Convert.ToDateTime(date);
                endTime = beginTime.AddDays(1).Date;
            }

            //查询泵房设备
            List<long> ids = new List<long>();
            var deviceList = _stationService.GetDeviceNameOfCon(stationId).ToList();
            if (type == 1)
            {
                deviceList = deviceList.Where(r => r.DeviceID == deviceId).ToList();
            }


            //数据查询及处理
            if (deviceList.Count > 0)
            {
                ids = deviceList.Select(r => (long)r.DeviceID).ToList();
                var datalist = _dHourQuartZ01Service.Query<DhourQuartZ01>(r => r.UpdateTime >= beginTime && r.UpdateTime < endTime && ids.Contains(r.DeviceId)).ToList();

                if (datalist.Count > 0)
                {
                    ViewBag.deviceidList = deviceList;
                    PartialView("EnergySectionTable", datalist);
                    dataTable = await ViewToString.RenderPartialViewToString(this, "EnergySectionTable");
                }
            }
            ViewBag.Type = type;
            return Json(new
            {
                dataTable = dataTable
            });
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
            //var data = _stationService.GetStation_consumption(user.IsAdmin, userID, stationname, 1);
            //if (data.Count() > 0)
            //{
            //    var list = data.Select(r => new
            //    {
            //        id = r.StationID,
            //        pId = 0,
            //        name = "<em class='iconfont icon-bengfang'></em>" + r.StationName,
            //        icon = "/images/stationTree.png"
            //    });
            //    return Newtonsoft.Json.JsonConvert.SerializeObject(list);
            //}
            List<StationAndDevice> treelist = _deviceInfo01Service.QueryZtreeInfo(user, stationname).ToList();
            if (treelist.Count() > 0)
            {
                IEnumerable<TreeAction> ztreeStation = treelist.Select(t => new TreeAction
                {
                    id = t.StationId,
                    pId = 0,
                    name = "<em class='iconfont icon-bengfang'></em>" + t.StationName,
                    @checked = false,
                    isDevice = false
                    //icon = "../../Content/zTree/css/zTreeStyle/area.png"
                }).OrderBy(r => r.name);

                IEnumerable<TreeAction> ztreeDevice = treelist.Where(r=>r.Partition!=6).Select(t => new TreeAction
                {
                    id = t.DeviceId,
                    pId = t.StationId,
                    name = t.DeviceName,
                    @checked = false,
                    isDevice = true,
                    Partition = t.Partition
                    //icon = "../../Content/zTree/css/zTreeStyle/area.png"
                });
                var treeList = ztreeStation.Union<TreeAction>(ztreeDevice).Distinct(new TreeAreaCompare());
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(treeList);
                return json;
            }
            else
            {
                return "[]";
            }
        }
        #endregion
    }
    public class StationEnergy
    {
        public string UpdateTime { get; set; }
        public double DataL { get; set; }//地区
        public double DataM { get; set; }//中区
        public double DataH { get; set; }//高区
        public double DataG { get; set; }//超高区
        public double DataCG { get; set; }//超超高区
        public double Total { get; set; }
    }
}
