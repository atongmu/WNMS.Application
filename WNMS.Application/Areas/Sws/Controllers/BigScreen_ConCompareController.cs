using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WNMS.IService;
using WNMS.Service;
using WNMS.Model.DataModels;
using Microsoft.AspNetCore.Html;
using WNMS.Application.Utility.Filters;
using System.Security.Claims;
using MongoDBHelper;
using WNMS.Model.CustomizedClass;
using Microsoft.AspNetCore.Hosting;
using WNMS.Application.Utility.JsonHelper;

namespace WNMS.Application.Areas.Sws.Controllers
{
    [Area("Sws")]
    public class BigScreen_ConCompareController : Controller
    {
        private ISws_DeviceInfo01Service _Sws_DeviceInfo01 = null;
        private ISysUserService _userService = null;
        private ISws_RTUInfoService _RTUInfoService = null;
        private IDHourQuartZ01Service _dHourQuartZ01Service = null;
        private IDDayQuartZ01Service _dDayQuartZ01Service = null;
        private IDMonthQuartZ01Service _DMonthQuartZ01Service = null;
        private ISws_ConsumpSettingService _ConsumpSettingService = null;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public BigScreen_ConCompareController(ISws_DeviceInfo01Service sws_DeviceInfo01Service,
            ISws_RTUInfoService sws_RTUInfoService,
            ISysUserService sysUserService,
            IDHourQuartZ01Service dHourQuartZ01Service,
             IDDayQuartZ01Service dDayQuartZ01Service,
            IDMonthQuartZ01Service dMonthQuartZ01Service,
            ISws_ConsumpSettingService sws_ConsumpSettingService,
            IWebHostEnvironment webHostEnvironment)
        {
            _Sws_DeviceInfo01 = sws_DeviceInfo01Service;
            _RTUInfoService = sws_RTUInfoService;
            _userService = sysUserService;
            _dHourQuartZ01Service = dHourQuartZ01Service;
            _dDayQuartZ01Service = dDayQuartZ01Service;
            _DMonthQuartZ01Service = dMonthQuartZ01Service;
            _webHostEnvironment = webHostEnvironment;
            _ConsumpSettingService = sws_ConsumpSettingService;
        }
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult Index()
        {
            
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = _userService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
            var datalist = _ConsumpSettingService.GetDeviceBySetting(user.UserId, 5).ToList();//能耗对比5
            long deviceid1 = 0, deviceid2 = 0;
            string deviceName1 = "", deviceName2 = "";
            if (datalist.Count > 0)
            {
                deviceid1 = datalist.FirstOrDefault().DeviceID;
                deviceName1 = datalist.FirstOrDefault().DeviceName;
                deviceid2 = datalist.LastOrDefault().DeviceID;
                deviceName2 = datalist.LastOrDefault().DeviceName;
            }
            else
            {
                var device_init = _ConsumpSettingService.GetDevice_initFilter(user.UserId, user.IsAdmin).ToList();
                if (device_init.Count > 0)
                {
                    deviceid1 = device_init.FirstOrDefault().DeviceID;
                    deviceName1 = device_init.FirstOrDefault().DeviceName;
                    deviceid2 = device_init.LastOrDefault().DeviceID;
                    deviceName2 = device_init.LastOrDefault().DeviceName;
                }
            }

            ViewBag.deviceid1 = deviceid1;
            ViewBag.deviceid2 = deviceid2;
            ViewBag.devicename1 = deviceName1;
            ViewBag.devicename2 = deviceName2;

            //if (user.IsAdmin)
            //{
            //var deviceid1 = 1584383568956;
            //    var model1 = _Sws_DeviceInfo01.Find<SwsDeviceInfo01>(deviceid1);
            //    var devicename1 = model1 == null ? "" : model1.DeviceName;
            //    var deviceid2 = 1585608948208;
            //    var model2 = _Sws_DeviceInfo01.Find<SwsDeviceInfo01>(deviceid2);
            //    var devicename2 = model2 == null ? "" : model2.DeviceName;
            //    ViewBag.deviceid1 = deviceid1;
            //    ViewBag.deviceid2 = deviceid2;
            //    ViewBag.devicename1 = devicename1;
            //    ViewBag.devicename2 = devicename2;
            //}
            //else
            //{
            //    var nodes = _RTUInfoService.GetDeviceTree_Big("", user.UserId, user.IsAdmin);
            //    if (nodes.Count() > 0)
            //    {
            //        var nodelist = nodes.Where(r => r.type == 1).ToList();
            //        if (nodelist.Count >= 2)
            //        {
            //            ViewBag.deviceid1 = nodelist[0].ID;
            //            ViewBag.deviceid2 = nodelist[1].ID;
            //            ViewBag.devicename1 = nodelist[0].extraName;
            //            ViewBag.devicename2 = nodelist[1].extraName;
            //        }
            //        else if (nodelist.Count == 1)
            //        {
            //            ViewBag.deviceid1 = nodelist[0].ID;
            //            ViewBag.deviceid2 = nodelist[0].ID;
            //            ViewBag.devicename1 = nodelist[0].extraName;
            //            ViewBag.devicename2 = nodelist[0].extraName;
            //        }
            //        else
            //        {
            //            ViewBag.deviceid1 = 0;
            //            ViewBag.deviceid2 = 0;
            //            ViewBag.devicename1 = "";
            //            ViewBag.devicename2 = "";
            //        }
            //    }
            //}

            var path = _webHostEnvironment.ContentRootPath;
            JsonFileHelper jsonFileHelper = new JsonFileHelper(path + "/Utility/ProjectJson/project.json");
            SystemInfo st = jsonFileHelper.Read<SystemInfo>("SystemInfo");
          
            ViewBag.sysName = st.SystemName;
            ViewBag.logo = st.Logo;
            return View();
        }
        #region 设备配置
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult SelectDevices()
        {
            return View();
        }
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult SelectDevice_single()
        {
            ViewBag.TreeNodes = new HtmlString(GetDeviceTree(""));
            return View();
        }

        public string GetDeviceTree(string stationName)
        {
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = _userService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
            var nodes = _RTUInfoService.GetDeviceTree_BigFilter(stationName, user.UserId, user.IsAdmin);
            var result = "[]";
            if (nodes.Count() > 0)
            {
                var treeNode = nodes.Select(r => new
                {
                    id = r.ID,
                    pId = r.Parent,
                    name = (r.type == 0 ? "<em class='iconfont icon-bengfang'></em>" : "") + r.extraName,
                    extraName = r.extraName,
                    nocheck = r.type == 0 ? true : false
                });
                result = Newtonsoft.Json.JsonConvert.SerializeObject(treeNode);
            }
            return result;

        }
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult SearchTree(string stationName)
        {
            return Content(GetDeviceTree(stationName));
        }
        //设备配置写入数据库
        public IActionResult SettingDevice(long deviceid1, long deviceid2)
        {
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = _userService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
            List<SwsConsumpSetting> setting = new List<SwsConsumpSetting>() { };
            var idlist = new List<long>() { deviceid1, deviceid2 };
            string str = "";
            foreach (var item in idlist)
            {
                SwsConsumpSetting s = new SwsConsumpSetting();
                s.DeviceId = item;
                s.Type = 5;//能耗对比
                s.UserId = userID;
                setting.Add(s);
            }
            try
            {
                _ConsumpSettingService.DeviceSetting(userID, 5, setting);

                str = "ok";

            }
            catch (Exception e)
            {
                str = "no";
            }

            return Content(str);
        }
        #endregion
        #region 数据查询
        //监控数据
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult GetEnergyJkData(long deviceid1, long deviceid2)
        {
            var devideList = _Sws_DeviceInfo01.Query<SwsDeviceInfo01>(r => r.DeviceId == deviceid1 || r.DeviceId == deviceid2);
            var devicelist_hasRtu = devideList.Where(r => r.Rtuid != null);
            Dictionary<long, List<double>> dic = new Dictionary<long, List<double>>() { };
            if (devicelist_hasRtu.Count() > 0)
            {

                List<int> dataid = new List<int> { 2000, 2001, 2002, 2038 };//进水压力，出水压力，设定压力，设定流量
                List<int> InstantFlow = new List<int> { 2030, 2032, 2034, 2036 };//瞬时流量，累计流量+1
                List<int> TotalPower = new List<int> { 2023, 2024, 2025, 2026 };//1#耗电，2#耗电，3#耗电，4#耗电

                var rtuidList = devideList.Select(r => r.Rtuid).Distinct();
                var rtuids = string.Join(",", rtuidList);
                string macJson = "{\"RTUID\":{'$in':[" + rtuids + "]}}";
                var collections = new MongoDBHelper<RtuJKInfo>(DateTime.Now.Year.ToString());
                var jkdata = collections.Find("Sws_DeviceJKInfo", macJson, "UpdateTime");
                if (jkdata.Count() > 0)
                {
                    foreach (var item in devideList)
                    {
                        var dataresult = new List<double>() { };
                        var partition = item.Partition;
                        var jk = jkdata.Where(r => r.RTUID == item.Rtuid).FirstOrDefault();
                        if (jk != null)
                        {
                            foreach (var it in dataid)
                            {
                                var tid = it + (partition - 1) * 500 + "";
                                double tt = jk.AnalogValues.ContainsKey(tid) ? (double)jk.AnalogValues[tid] : 0;
                                dataresult.Add(Math.Round(tt, 2));
                            }
                            //瞬时流量
                            var instantflow = 0.0;
                            foreach (var it in InstantFlow)
                            {
                                var tid = it + (partition - 1) * 500 + "";
                                double tt = jk.AnalogValues.ContainsKey(tid) ? (double)jk.AnalogValues[tid] : 0;
                                instantflow += tt;
                            }
                            dataresult.Add(Math.Round(instantflow, 2));

                            var totalflow = 0.0;
                            foreach (var it in InstantFlow)
                            {
                                var tid = it + (partition - 1) * 500 + 1 + "";
                                double tt = jk.AnalogValues.ContainsKey(tid) ? (double)jk.AnalogValues[tid] : 0;
                                totalflow += tt;
                            }
                            dataresult.Add(Math.Round(totalflow, 2));

                            //累计电量
                            var totalpowerdata = 0.0;

                            foreach (var it in TotalPower)
                            {
                                var tid = it + (partition - 1) * 500 + "";
                                double tt = jk.AnalogValues.ContainsKey(tid) ? (double)jk.AnalogValues[tid] : 0;
                                totalpowerdata += tt;
                            }
                            if (totalpowerdata == 0.0)
                            {
                                double totalPower = 0.0;
                                var tid = 2085 + (partition - 1) * 500 + "";
                                totalPower = jk.AnalogValues.ContainsKey(tid) ? (double)jk.AnalogValues[tid] : 0;
                                totalpowerdata = totalPower;
                            }
                            dataresult.Add(Math.Round(totalpowerdata, 2));


                        }
                        else
                        {
                            dataresult = new List<double>() { 0, 0, 0, 0, 0, 0, 0 };
                        }
                        dic.Add(item.DeviceId, dataresult);

                    }
                }
                else
                {
                    var dataresult = new List<double>() { 0, 0, 0, 0, 0, 0, 0 };
                    dic.Add(deviceid1, dataresult);
                    dic.Add(deviceid2, dataresult);
                }
            }
            else
            {
                var dataresult = new List<double>() { 0, 0, 0, 0, 0, 0, 0 };
                dic.Add(deviceid1, dataresult);
                dic.Add(deviceid2, dataresult);
            }

            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(dic));
        }
        //获取今日流量，能耗
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult GetTodayData(long deviceid1, long deviceid2)
        {
            var devideList = _Sws_DeviceInfo01.Query<SwsDeviceInfo01>(r => r.DeviceId == deviceid1 || r.DeviceId == deviceid2);
            List<string> axias = new List<string>() { };
            List<DateTime> dateList = new List<DateTime>() { };
            List<EneryFlowData> result = new List<EneryFlowData>() { };
            var beginDate = DateTime.Now.Date;
            var endDate = DateTime.Now.AddHours(-1);
            var datalist = _dHourQuartZ01Service.Query<DhourQuartZ01>(r => (r.DeviceId == deviceid1 || r.DeviceId == deviceid2) && r.UpdateTime >= beginDate && r.UpdateTime <= endDate).ToList();
            while (beginDate <= endDate)
            {
                axias.Add(beginDate.ToString("HH:mm"));
                dateList.Add(beginDate);
                beginDate = beginDate.AddHours(1);
            }
            if (axias.Count == 0)
            {
                axias.Add("00:00");
                dateList.Add(DateTime.Now.Date);
            }

            List<long> deviceids = new List<long>() { deviceid1, deviceid2 };
            foreach (var item in deviceids)
            {
                EneryFlowData p = new EneryFlowData();
                p.EberyData = new List<decimal>() { };
                p.FlowData = new List<decimal>() { };
                if (datalist.Count() > 0)
                {
                    var list = datalist.Where(r => r.DeviceId == item)?.OrderBy(r => r.UpdateTime);
                    if (list.Count() > 0)
                    {
                        foreach (var it in dateList)
                        {
                            var datat = list.Where(r => r.UpdateTime == it).FirstOrDefault();
                            if (datat != null)
                            {
                                p.EberyData.Add(Math.Round(datat.EnergyCon, 2));
                                p.FlowData.Add(Math.Round(datat.FlowCon, 2));
                            }
                            else
                            {
                                p.EberyData.Add(0);
                                p.FlowData.Add(0);
                            }
                        }
                    }
                    else
                    {
                        foreach (var it in dateList)
                        {
                            p.EberyData.Add(0);
                            p.FlowData.Add(0);

                        }
                    }
                }
                else
                {
                    foreach (var it in dateList)
                    {
                        p.EberyData.Add(0);
                        p.FlowData.Add(0);

                    }
                }
                result.Add(p);
            }
           
            return Json(new
            {
                axias = axias,
                result = result
            });

        }
        //获取昨日流量，能耗
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult GetYestDayData(long deviceid1, long deviceid2)
        {
            var devideList = _Sws_DeviceInfo01.Query<SwsDeviceInfo01>(r => r.DeviceId == deviceid1 || r.DeviceId == deviceid2);
            List<string> axias = new List<string>() { };
            List<DateTime> dateList = new List<DateTime>() { };
            List<EneryFlowData> result = new List<EneryFlowData>() { };
            var beginDate = DateTime.Now.Date.AddDays(-1);
            var endDate = DateTime.Now.Date;
            var datalist = _dHourQuartZ01Service.Query<DhourQuartZ01>(r => (r.DeviceId == deviceid1 || r.DeviceId == deviceid2) && r.UpdateTime >= beginDate && r.UpdateTime < endDate).ToList();
            while (beginDate < endDate)
            {
                axias.Add(beginDate.ToString("HH:mm"));
                dateList.Add(beginDate);
                beginDate = beginDate.AddHours(1);
            }

            List<long> deviceids = new List<long>() { deviceid1, deviceid2 };
            foreach (var item in deviceids)
            {
                EneryFlowData p = new EneryFlowData();
                p.EberyData = new List<decimal>() { };
                p.FlowData = new List<decimal>() { };
                if (datalist.Count() > 0)
                {
                    var list = datalist.Where(r => r.DeviceId == item)?.OrderBy(r => r.UpdateTime);
                    if (list.Count() > 0)
                    {
                        foreach (var it in dateList)
                        {
                            var datat = list.Where(r => r.UpdateTime == it).FirstOrDefault();
                            if (datat != null)
                            {
                                p.EberyData.Add(Math.Round(datat.EnergyCon, 2));
                                p.FlowData.Add(Math.Round(datat.FlowCon, 2));
                            }
                            else
                            {
                                p.EberyData.Add(0);
                                p.FlowData.Add(0);
                            }
                        }
                    }
                    else
                    {
                        foreach (var it in dateList)
                        {
                            p.EberyData.Add(0);
                            p.FlowData.Add(0);

                        }
                    }
                }
                else
                {
                    foreach (var it in dateList)
                    {
                        p.EberyData.Add(0);
                        p.FlowData.Add(0);

                    }
                }
                result.Add(p);
            }
            return Json(new
            {
                axias = axias,
                result = result
            });
        }
        //获取本周
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult GetThisWeekData(long deviceid1, long deviceid2)
        {
            var zhouji = DateTime.Now.DayOfWeek;
            var num = (int)System.Enum.Parse(typeof(WeekDay), zhouji.ToString());
            if (num == 1)
            {
                num = 8;
            }
            var endDate = DateTime.Now.Date;
            var beginDate = DateTime.Now.AddDays(2 - num).Date;
            var devideList = _Sws_DeviceInfo01.Query<SwsDeviceInfo01>(r => r.DeviceId == deviceid1 || r.DeviceId == deviceid2);
            List<string> axias = new List<string>() { };
            List<DateTime> dateList = new List<DateTime>() { };
            List<EneryFlowData> result = new List<EneryFlowData>() { };
            var datalist = _dDayQuartZ01Service.Query<DdayQuartZ01>(r => (r.DeviceId == deviceid1 || r.DeviceId == deviceid2) && r.UpdateTime >= beginDate && r.UpdateTime < endDate).ToList();
            string deviceid_string = deviceid1 + "," + deviceid2;
           
            var todaylist = _RTUInfoService.GetThisDayCompData(deviceid_string,endDate.ToString(),DateTime.Now.ToString());//今日能耗数据
            datalist = datalist.Union(todaylist).ToList();
            while (beginDate <= endDate)
            {
                axias.Add(beginDate.ToString("MM-dd"));
                dateList.Add(beginDate);
                beginDate = beginDate.AddDays(1);
            }

            List<long> deviceids = new List<long>() { deviceid1, deviceid2 };
            foreach (var item in deviceids)
            {
                EneryFlowData p = new EneryFlowData();
                p.EberyData = new List<decimal>() { };
                p.FlowData = new List<decimal>() { };
                if (datalist.Count() > 0)
                {
                    var list = datalist.Where(r => r.DeviceId == item)?.OrderBy(r => r.UpdateTime);
                    if (list.Count() > 0)
                    {
                        foreach (var it in dateList)
                        {
                            var datat = list.Where(r => r.UpdateTime == it).FirstOrDefault();
                            if (datat != null)
                            {
                                p.EberyData.Add(Math.Round(datat.EnergyCon, 2));
                                p.FlowData.Add(Math.Round(datat.FlowCon, 2));
                            }
                            else
                            {
                                p.EberyData.Add(0);
                                p.FlowData.Add(0);
                            }
                        }
                    }
                    else
                    {
                        foreach (var it in dateList)
                        {
                            p.EberyData.Add(0);
                            p.FlowData.Add(0);

                        }
                    }
                }
                else
                {
                    foreach (var it in dateList)
                    {
                        p.EberyData.Add(0);
                        p.FlowData.Add(0);

                    }
                }
                result.Add(p);
            }
            //当天的单吨能耗
            decimal device_comp1 = 0;//设备1的单吨能耗
            decimal device_comp2 = 0;//设备2的单吨能耗
            if (todaylist.Count() > 0)
            {
                var device1_data = todaylist.Where(r => r.DeviceId == deviceid1).FirstOrDefault();
                if (device1_data != null)
                {
                    if (device1_data.FlowCon == 0)
                    {
                        device_comp1 = 0;
                    }
                    else
                    {
                        device_comp1 = Math.Round(device1_data.EnergyCon/ device1_data.FlowCon,2);
                    }
                }
                 
                var data2= todaylist.Where(r => r.DeviceId == deviceid2).FirstOrDefault();
                if (data2 != null)
                {
                    if (data2.FlowCon == 0)
                    {
                        device_comp2 = 0;
                    }
                    else
                    {
                        device_comp2 = Math.Round(data2.EnergyCon / data2.FlowCon, 2);
                    }
                }

            }

            return Json(new
            {
                axias = axias,
                result = result,
                device_comp1= device_comp1,
                device_comp2= device_comp2
            });
        }
        //获取近一周
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult GetAlmostWeekData(long deviceid1, long deviceid2)
        {
            var endDate = DateTime.Now.Date;
            var beginDate = DateTime.Now.AddDays(-7).Date;
            List<string> axias = new List<string>() { };
            List<DateTime> dateList = new List<DateTime>() { };
            List<EneryFlowData> result = new List<EneryFlowData>() { };
            var datalist = _dDayQuartZ01Service.Query<DdayQuartZ01>(r => (r.DeviceId == deviceid1 || r.DeviceId == deviceid2) && r.UpdateTime >= beginDate && r.UpdateTime < endDate).ToList();
            string deviceid_string = deviceid1 + "," + deviceid2;

            var todaylist = _RTUInfoService.GetThisDayCompData(deviceid_string, endDate.ToString(), DateTime.Now.ToString());//今日能耗数据
          
            while (beginDate < endDate)
            {
                axias.Add(beginDate.ToString("MM-dd"));
                dateList.Add(beginDate);
                beginDate = beginDate.AddDays(1);
            }

            List<long> deviceids = new List<long>() { deviceid1, deviceid2 };
            foreach (var item in deviceids)
            {
                EneryFlowData p = new EneryFlowData();
                p.EberyData = new List<decimal>() { };
                p.FlowData = new List<decimal>() { };
                if (datalist.Count() > 0)
                {
                    var list = datalist.Where(r => r.DeviceId == item)?.OrderBy(r => r.UpdateTime);
                    if (list.Count() > 0)
                    {
                        foreach (var it in dateList)
                        {
                            var datat = list.Where(r => r.UpdateTime == it).FirstOrDefault();
                            if (datat != null)
                            {
                                p.EberyData.Add(Math.Round(datat.EnergyCon, 2));
                                p.FlowData.Add(Math.Round(datat.FlowCon, 2));
                            }
                            else
                            {
                                p.EberyData.Add(0);
                                p.FlowData.Add(0);
                            }
                        }
                    }
                    else
                    {
                        foreach (var it in dateList)
                        {
                            p.EberyData.Add(0);
                            p.FlowData.Add(0);

                        }
                    }
                }
                else
                {
                    foreach (var it in dateList)
                    {
                        p.EberyData.Add(0);
                        p.FlowData.Add(0);

                    }
                }
                result.Add(p);
            }

            //当天的单吨能耗
            decimal device_comp1 = 0;//设备1的单吨能耗
            decimal device_comp2 = 0;//设备2的单吨能耗
            if (todaylist.Count() > 0)
            {
                var device1_data = todaylist.Where(r => r.DeviceId == deviceid1).FirstOrDefault();
                if (device1_data != null)
                {
                    if (device1_data.FlowCon == 0)
                    {
                        device_comp1 = 0;
                    }
                    else
                    {
                        device_comp1 = Math.Round(device1_data.EnergyCon / device1_data.FlowCon, 2);
                    }
                }

                var data2 = todaylist.Where(r => r.DeviceId == deviceid2).FirstOrDefault();
                if (data2 != null)
                {
                    if (data2.FlowCon == 0)
                    {
                        device_comp2 = 0;
                    }
                    else
                    {
                        device_comp2 = Math.Round(data2.EnergyCon / data2.FlowCon, 2);
                    }
                }

            }
            return Json(new
            {
                axias = axias,
                result = result,
                device_comp1 = device_comp1,
                device_comp2 = device_comp2

            });
        }
        //获取近15天的数据
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult Get15DayData(long deviceid1, long deviceid2)
        {
            var beginDate = DateTime.Now.Date.AddDays(-15);
            var endDate = DateTime.Now.Date;
            //var devideList = _Sws_DeviceInfo01.Query<SwsDeviceInfo01>(r => r.DeviceId == deviceid1 || r.DeviceId == deviceid2);
            List<string> axias = new List<string>() { };
            List<DateTime> dateList = new List<DateTime>() { };
            List<EneryFlowData> result = new List<EneryFlowData>() { };
            var datalist = _dDayQuartZ01Service.Query<DdayQuartZ01>(r => (r.DeviceId == deviceid1 || r.DeviceId == deviceid2) && r.UpdateTime >= beginDate && r.UpdateTime < endDate).ToList();
            while (beginDate < endDate)
            {
                axias.Add(beginDate.ToString("MM-dd"));
                dateList.Add(beginDate);
                beginDate = beginDate.AddDays(1);
            }
            string deviceid_string = deviceid1 + "," + deviceid2;
           
            List<long> deviceids = new List<long>() { deviceid1, deviceid2 };
            foreach (var item in deviceids)
            {
                EneryFlowData p = new EneryFlowData();
                p.EberyData = new List<decimal>() { };
                p.FlowData = new List<decimal>() { };
                if (datalist.Count() > 0)
                {
                    var list = datalist.Where(r => r.DeviceId == item)?.OrderBy(r => r.UpdateTime);
                    if (list.Count() > 0)
                    {
                        foreach (var it in dateList)
                        {
                            var datat = list.Where(r => r.UpdateTime == it).FirstOrDefault();
                            if (datat != null)
                            {
                                p.EberyData.Add(Math.Round(datat.EnergyCon, 2));
                                p.FlowData.Add(Math.Round(datat.FlowCon, 2));
                            }
                            else
                            {
                                p.EberyData.Add(0);
                                p.FlowData.Add(0);
                            }
                        }
                    }
                    else
                    {
                        foreach (var it in dateList)
                        {
                            p.EberyData.Add(0);
                            p.FlowData.Add(0);

                        }
                    }
                }
                else
                {
                    foreach (var it in dateList)
                    {
                        p.EberyData.Add(0);
                        p.FlowData.Add(0);

                    }
                }
                result.Add(p);
            }
            return Json(new
            {
                axias = axias,
                result = result
            });
        }
        //获取本月
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult GetThisMonthData(long deviceid1, long deviceid2)
        {


            var beginDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM") + "-01");
            var endDate = DateTime.Now.Date;
            var devideList = _Sws_DeviceInfo01.Query<SwsDeviceInfo01>(r => r.DeviceId == deviceid1 || r.DeviceId == deviceid2);
            List<string> axias = new List<string>() { };
            List<DateTime> dateList = new List<DateTime>() { };
            List<EneryFlowData> result = new List<EneryFlowData>() { };
            var datalist = _dDayQuartZ01Service.Query<DdayQuartZ01>(r => (r.DeviceId == deviceid1 || r.DeviceId == deviceid2) && r.UpdateTime >= beginDate && r.UpdateTime < endDate).ToList();
            while (beginDate <= endDate)
            {
                axias.Add(beginDate.ToString("MM-dd"));
                dateList.Add(beginDate);
                beginDate = beginDate.AddDays(1);
            }
            string deviceid_string = deviceid1 + "," + deviceid2;
            var todaylist = _RTUInfoService.GetThisDayCompData(deviceid_string, endDate.ToString(), DateTime.Now.ToString());//今天的数据
            datalist = datalist.Union(todaylist).ToList();
            List<long> deviceids = new List<long>() { deviceid1, deviceid2 };
            foreach (var item in deviceids)
            {
                EneryFlowData p = new EneryFlowData();
                p.EberyData = new List<decimal>() { };
                p.FlowData = new List<decimal>() { };
                if (datalist.Count() > 0)
                {
                    var list = datalist.Where(r => r.DeviceId == item)?.OrderBy(r => r.UpdateTime);
                    if (list.Count() > 0)
                    {
                        foreach (var it in dateList)
                        {
                            var datat = list.Where(r => r.UpdateTime == it).FirstOrDefault();
                            if (datat != null)
                            {
                                p.EberyData.Add(Math.Round(datat.EnergyCon, 2));
                                p.FlowData.Add(Math.Round(datat.FlowCon, 2));
                            }
                            else
                            {
                                p.EberyData.Add(0);
                                p.FlowData.Add(0);
                            }
                        }
                    }
                    else
                    {
                        foreach (var it in dateList)
                        {
                            p.EberyData.Add(0);
                            p.FlowData.Add(0);

                        }
                    }
                }
                else
                {
                    foreach (var it in dateList)
                    {
                        p.EberyData.Add(0);
                        p.FlowData.Add(0);

                    }
                }
                result.Add(p);
            }
            return Json(new
            {
                axias = axias,
                result = result
            });
        }
        //获取本年
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult GetThisYearData(long deviceid1, long deviceid2)
        {


            var beginDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy") + "-01-01");
            var endDate = DateTime.Now.Date;
            var devideList = _Sws_DeviceInfo01.Query<SwsDeviceInfo01>(r => r.DeviceId == deviceid1 || r.DeviceId == deviceid2);
            List<string> axias = new List<string>() { };
            List<DateTime> dateList = new List<DateTime>() { };
            List<EneryFlowData> result = new List<EneryFlowData>() { };
            var datalist = _DMonthQuartZ01Service.Query<DmonthQuartZ01>(r => (r.DeviceId == deviceid1 || r.DeviceId == deviceid2) && r.UpdateTime >= beginDate && r.UpdateTime < endDate).ToList();
            var enddateFlag = Convert.ToDateTime(DateTime.Now.Date.ToString("yyyy-MM") + "-01");
            while (beginDate < enddateFlag)
            {
                axias.Add(beginDate.Month + "月");
                dateList.Add(beginDate);
                beginDate = beginDate.AddMonths(1);
            }
            //本月
            var beginDate1 = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM") + "-01");
            var endDate1 = DateTime.Now.Date;
            var datamonthList = _dDayQuartZ01Service.Query<DdayQuartZ01>(r => (r.DeviceId == deviceid1 || r.DeviceId == deviceid2) && r.UpdateTime >= beginDate1 && r.UpdateTime < endDate1).ToList();
            List<long> deviceids = new List<long>() { deviceid1, deviceid2 };
            foreach (var item in deviceids)
            {
                EneryFlowData p = new EneryFlowData();
                p.EberyData = new List<decimal>() { };
                p.FlowData = new List<decimal>() { };
                if (datalist.Count() > 0)
                {
                    var list = datalist.Where(r => r.DeviceId == item)?.OrderBy(r => r.UpdateTime);
                    if (list.Count() > 0)
                    {
                        foreach (var it in dateList)
                        {
                            var datat = list.Where(r => r.UpdateTime == it).FirstOrDefault();
                            if (datat != null)
                            {
                                p.EberyData.Add(Math.Round(datat.EnergyCon, 2));
                                p.FlowData.Add(Math.Round(datat.FlowCon, 2));
                            }
                            else
                            {
                                p.EberyData.Add(0);
                                p.FlowData.Add(0);
                            }
                        }
                    }
                    else
                    {
                        foreach (var it in dateList)
                        {
                            p.EberyData.Add(0);
                            p.FlowData.Add(0);

                        }
                    }
                }
                else
                {
                    foreach (var it in dateList)
                    {
                        p.EberyData.Add(0);
                        p.FlowData.Add(0);

                    }
                }
                var device_thisMonth = datamonthList.Where(r => r.DeviceId == item);
                if (device_thisMonth.Count() > 0)
                {
                    p.EberyData.Add(Math.Round(device_thisMonth.Select(r => r.EnergyCon).Sum(), 2));
                    p.FlowData.Add(Math.Round(device_thisMonth.Select(r => r.FlowCon).Sum(), 2));
                }
                else
                {
                    p.EberyData.Add(0);
                    p.FlowData.Add(0);
                }
                result.Add(p);
            }
            axias.Add(DateTime.Now.Month + "月");
            return Json(new
            {
                axias = axias,
                result = result
            });
        }
        public class EneryFlowData
        {
            public List<decimal> FlowData { get; set; }
            public List<decimal> EberyData { get; set; }
        }
        #endregion
    }
}