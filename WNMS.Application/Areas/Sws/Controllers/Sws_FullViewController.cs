using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using MongoDBHelper;
using WNMS.Application.Utility.Filters;
using WNMS.Application.Utility.JsonHelper;
using WNMS.IService;
using WNMS.Model.CustomizedClass;
using WNMS.Model.DataModels;
using WNMS.Utility;

namespace WNMS.Application.Areas.Sws.Controllers
{
    [Area("Sws")]
    public class Sws_FullViewController : Controller
    {
        private ISws_StationService _StationService = null;
        private ISysUserService _UserService = null;
        private ISws_EventInfoService _EventInfoService = null;
        private readonly ISws_DeviceInfo01Service _sws_DeviceInfo01Service;
        private readonly ISys_DataItemDetailService _sys_DataItemDetailServices;
        private readonly ISws_DeviceInfo02Service _sws_DeviceInfo02Service;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ISws_UserStationService _sws_UserStationService;
        private ISws_RTUInfoService rtuService = null;
        private ISws_EventHistoryService eventHistoryService = null;
        int timevalue = int.Parse(StaticConstraint.TimeValue);
        public Sws_FullViewController(ISws_StationService sws_StationService,
            ISysUserService sysUserService, ISws_EventInfoService sws_EventInfoService,
             IWebHostEnvironment webHostEnvironment,
             ISws_DeviceInfo01Service sws_DeviceInfo01Service,
            ISys_DataItemDetailService sys_DataItemDetailServices,
            ISws_UserStationService sws_UserStationService,
            ISws_RTUInfoService sws_RTUInfoService,
             ISws_EventHistoryService sws_EventHistoryService,
             ISws_DeviceInfo02Service sws_DeviceInfo02Service
           )
        {
            _StationService = sws_StationService;
            _UserService = sysUserService;
            _EventInfoService = sws_EventInfoService;
            _webHostEnvironment = webHostEnvironment;
            _sws_DeviceInfo01Service = sws_DeviceInfo01Service;
            _sys_DataItemDetailServices = sys_DataItemDetailServices;
            _sws_DeviceInfo02Service = sws_DeviceInfo02Service;
            _sws_UserStationService = sws_UserStationService;
            rtuService = sws_RTUInfoService;
            eventHistoryService = sws_EventHistoryService;
        }
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult Index()
        {
            var path = _webHostEnvironment.ContentRootPath;
            JsonFileHelper jsonFileHelper = new JsonFileHelper(path + "/Utility/ProjectJson/project.json");
            SystemInfo st = jsonFileHelper.Read<SystemInfo>("SystemInfo");
            ViewBag.lng = st.Lng;
            ViewBag.lat = st.Lat;
            ViewBag.zoom = st.MapLevel;
            ViewBag.sysName = st.SystemName;
            ViewBag.logo = st.Logo;
            ViewBag.AreaName = st.AreaName;
            ViewBag.ArcgisUrl = StaticConstraint.ArcgisUrl;
            return View();
        }

        //机组相关数据
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult LoadDeviceChart()
        {
            //用户
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = _UserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
            //获取机组信息 
            var deviceInfo = _sws_DeviceInfo01Service.Query<SwsDeviceInfo01>(r => true).ToList();
            var zdeviceInfo = _sws_DeviceInfo02Service.Query<SwsDeviceInfo02>(r => true).ToList();
            if (user.IsAdmin != true)
            {
                var statioids = _sws_UserStationService.Query<SwsUserStation>(r => r.UserId == user.UserId).Select(r => r.StationId).ToList();
                deviceInfo = deviceInfo.Where(r => statioids.Contains(r.StationId)).ToList();
                zdeviceInfo = zdeviceInfo.Where(r => statioids.Contains(r.StationId)).ToList();
            }
            //机组类型品牌
            List<int> ids = new List<int> { 5, 7, 8 };
            var allType = _sys_DataItemDetailServices.Query<SysDataItemDetail>(r => ids.Contains(r.FItemId)).ToList();
            //机组类型统计 
            int devicetypeid = (int)WNMS.Model.CustomizedClass.Enum.设备类型;
            var deviceType = allType.Where(r => r.FItemId == devicetypeid).ToList();
            List<string> devicetypeNatList = new List<string>();
            List<int> devicecount = new List<int>();
            foreach (var item in deviceType)
            {
                var stcCount = deviceInfo.Where(r => r.DeviceType == int.Parse(item.ItemValue)).Count();
                int zcount = 0;
                if (item.ItemValue == "5")
                {
                    zcount = zdeviceInfo.Count();
                }
                var allcount = stcCount + zcount;
                devicecount.Add(allcount);
                devicetypeNatList.Add(item.ItemName.ToString());
            }
            var rel = new
            {
                devicecount,
                devicetypeNatList,
            };
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(rel));
        }
        //机组相关数据
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult LoadDeviceNewChart()
        {
            //用户
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = _UserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
            //获取机组信息 
            var deviceInfo = _sws_DeviceInfo01Service.Query<SwsDeviceInfo01>(r => true).ToList();
            var zdeviceInfo = _sws_DeviceInfo02Service.Query<SwsDeviceInfo02>(r => true).ToList();
            if (user.IsAdmin != true)
            {
                var statioids = _sws_UserStationService.Query<SwsUserStation>(r => r.UserId == user.UserId).Select(r => r.StationId).ToList();
                deviceInfo = deviceInfo.Where(r => statioids.Contains(r.StationId)).ToList();
                zdeviceInfo = zdeviceInfo.Where(r => statioids.Contains(r.StationId)).ToList();
            }
            //机组类型品牌
            List<int> ids = new List<int> { 5, 7, 8 };
            var allType = _sys_DataItemDetailServices.Query<SysDataItemDetail>(r => ids.Contains(r.FItemId)).ToList();
            //机组类型统计 
            int devicetypeid = (int)WNMS.Model.CustomizedClass.Enum.设备类型;
            var deviceType = allType.Where(r => r.FItemId == devicetypeid).ToList();
            List<RtuCount> devicecount = new List<RtuCount>();
            foreach (var item in deviceType)
            {
                RtuCount rtuCount = new RtuCount();
                var stcCount = deviceInfo.Where(r => r.DeviceType == int.Parse(item.ItemValue)).Count();
                int zcount = 0;
                if (item.ItemName.Contains("直饮水"))
                {
                    zcount = zdeviceInfo.Count();
                }
                var allcount = stcCount + zcount;
                rtuCount.CountRtu = allcount;
                rtuCount.DeviceTypeName = item.ItemName.ToString();
                devicecount.Add(rtuCount);
            }
            var rel = new
            {
                devicecount,
            };
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(rel));
        }
        //获取报警信息
        [TypeFilter(typeof(IgonreActionFilter))]
        public async Task<IActionResult> GetpieData(int dateType, string dateTime)
        {
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = _UserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();

            List<int> rtuIds = GetRtuID(user);
            //var eventalllist = eventHistoryService.Query<SwsEventHistory>(r => rtuIds.Contains(r.Rtuid)).ToList();

            //List<IGrouping<byte, SwsEventHistory>> eventList = new List<IGrouping<byte, SwsEventHistory>>();
            //eventList = eventHistoryService.Query<SwsEventHistory>(r => rtuIds.Contains(r.Rtuid)).AsEnumerable().GroupBy(r => r.EventLevel).ToList();

            //List<IGrouping<byte, SwsEventHistory>> eventList = new List<IGrouping<byte, SwsEventHistory>>();
            //eventList = eventalllist.Where(r => rtuIds.Contains(r.Rtuid)).AsEnumerable().GroupBy(r => r.EventLevel).ToList();

            var eventdata = eventHistoryService.Query<SwsEventHistory>(r => rtuIds.Contains(r.Rtuid))
               .GroupBy(r => r.EventLevel)
               .Select(group => new
               {
                   name = group.Key == 1 ? "紧急报警" : (group.Key == 2 ? "一般报警" : "提示报警"),
                   value = group.Count()
               });
            int level1 = 0, level2 = 0, level3 = 0;

            var level1Model = eventdata.Where(a => a.name == "紧急报警").FirstOrDefault();
            if (level1Model != null)
                level1 = level1Model.value;

            var level2Model = eventdata.Where(a => a.name == "一般报警").FirstOrDefault();
            if (level2Model != null)
                level2 = level2Model.value;

            var level3Model = eventdata.Where(a => a.name == "提示报警").FirstOrDefault();
            if (level3Model != null)
                level3 = level3Model.value;
            //List<Hashtable> eventdata = new List<Hashtable>();
            //foreach (var info in eventList)
            //{
            //    Hashtable edata = new Hashtable();
            //    if (info.Key == 1)
            //    {
            //        edata.Add("name", "紧急报警");
            //        edata.Add("value", info.Count());
            //        level1 = info.Count();
            //    }
            //    if (info.Key == 2)
            //    {
            //        edata.Add("name", "一般报警");
            //        edata.Add("value", info.Count());
            //        level2 = info.Count();
            //    }
            //    if (info.Key == 3)
            //    {
            //        edata.Add("name", "提示报警");
            //        edata.Add("value", info.Count());
            //        level3 = info.Count();
            //    }
            //    eventdata.Add(edata);
            //}
            //本周报警Top5
            //List<Hashtable> eventsourcelist = new List<Hashtable>();
            //Hashtable sourcedata = new Hashtable();
            var dt = DateTime.Now;
            var dayofWeek1 = (int)dt.DayOfWeek == 0 ? 7 : (int)dt.DayOfWeek;
            var startTime = DateTime.Parse(string.Format("{0:yyyy-MM-dd}", dt.AddDays(-(dayofWeek1 - 1))));
            var endTime = DateTime.Parse(string.Format("{0:yyyy-MM-dd}", dt.AddDays(-(dayofWeek1) + 7)));

            var eventsourcelist = eventHistoryService.Query<SwsEventHistory>(r => r.EventTime >= startTime && r.EventTime <= endTime && r.State == 0 && rtuIds.Contains(r.Rtuid))
                .GroupBy(r => r.EventMessage)
                .Select(group => new
                {
                    name = group.Key,
                    value = group.Count()
                }).OrderByDescending(a => a.value).Take(5);

            if (eventsourcelist != null)
                eventsourcelist = eventsourcelist.OrderBy(a => a.value);

            string[] name = eventsourcelist.Select(a => a.name).ToArray();
            //IEnumerable<IGrouping<string, SwsEventHistory>> Eventlist = eventalllist.Where(r => r.EventTime >= startTime && r.EventTime <= endTime && r.State == 0 && rtuIds.Contains(r.Rtuid)).ToList().GroupBy(r => r.EventMessage);

            //Eventlist = Eventlist.ToList().OrderBy(delegate (IGrouping<string, SwsEventHistory> p1)
            //{
            //    return p1.Count();
            //});
            //Eventlist = Eventlist.Skip(Eventlist.Count() - 5).ToList();
            //string[] name = new string[Eventlist.Count()];
            //int number = 0;
            //foreach (IGrouping<string, SwsEventHistory> info in Eventlist)
            //{
            //    sourcedata = new Hashtable();
            //    sourcedata.Add("value", info.Count());
            //    sourcedata.Add("name", info.Key);
            //    eventsourcelist.Add(sourcedata);
            //    name[number] = info.Key;
            //    number++;
            //}




            //数据返回
            var rel = new
            {
                eventdata,
                level1,
                level2,
                level3,
                eventsourcelist,
                name
            };
            string result = Newtonsoft.Json.JsonConvert.SerializeObject(rel);
            return Content(result);
        }
        //实时报警数量
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult LoadEventCount()
        {
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = _UserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
            List<int> rtuIds = GetRtuID(user);
            decimal pre = 0;
            //实时报警数量
            var eventcount = _EventInfoService.Query<SwsEventInfo>(r => rtuIds.Contains(r.Rtuid) && r.EventLevel != 0).ToList();
            //var eventcount = _sws_DeviceInfo01Service.QueryEventDevice().ToList();
            //设备总数量
            var ids = _sws_UserStationService.Query<SwsUserStation>(r => r.UserId == user.UserId).Select(r => r.StationId).ToList();
            List<SwsDeviceInfo01> edevicecount = new List<SwsDeviceInfo01>();
            List<SwsDeviceInfo02> zdevicecount = new List<SwsDeviceInfo02>();
            if (user.IsAdmin)
            {
                edevicecount = _sws_DeviceInfo01Service.Query<SwsDeviceInfo01>(r => true).ToList();
                zdevicecount = _sws_DeviceInfo02Service.Query<SwsDeviceInfo02>(r => true).ToList();
            }
            else
            {
                edevicecount = _sws_DeviceInfo01Service.Query<SwsDeviceInfo01>(r => ids.Contains(r.StationId)).ToList();
                zdevicecount = _sws_DeviceInfo02Service.Query<SwsDeviceInfo02>(r => ids.Contains(r.StationId)).ToList();
            }

            var alldevice = edevicecount.Count() + zdevicecount.Count();
            List<int?> eventrtuid = eventcount.Select(r => r?.Rtuid).ToList();
            var bj01 = edevicecount.Where(r => eventrtuid.Contains(r.Rtuid)).Count();
            var bj02 = zdevicecount.Where(r => eventrtuid.Contains(r.Rtuid)).Count();
            var allcount = bj01 + bj02;
            //报警百分比
            List<decimal> prelist = new List<decimal>();
            if (allcount != 0 && alldevice != 0)
            {
                var beg = decimal.Parse(allcount.ToString()) / decimal.Parse(alldevice.ToString());
                pre = Math.Round(beg, 2);
            }
            prelist.Add(pre);
            //数据返回
            var rel = new
            {
                allcount,
                alldevice,
                prelist
            };
            string result = Newtonsoft.Json.JsonConvert.SerializeObject(rel);
            return Content(result);
        }
        [TypeFilter(typeof(IgonreActionFilter))]
        public List<int> GetRtuID(SysUser user)
        {
            List<int> rtuIds = new List<int>();
            if (user.IsAdmin)
            {
                rtuIds = rtuService.Query<SwsRtuinfo>(r => true)?.Select(r => r.Rtuid).ToList();
            }
            else
            {
                var ids = _sws_UserStationService.Query<SwsUserStation>(r => r.UserId == user.UserId).Select(r => r.StationId).ToList();
                rtuIds = rtuService.Query<SwsRtuinfo>(r => ids.Contains(r.StationId))?.Select(r => r.Rtuid).ToList();
            }
            return rtuIds;
        }
        //本周报警排名
        [TypeFilter(typeof(IgonreActionFilter))]
        public ActionResult LoadEventRank()
        {
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = _UserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
            List<int> rtuIds = GetRtuID(user);
            //报警源分析
            //List<Hashtable> eventsourcelist = new List<Hashtable>();
            //Hashtable sourcedata = new Hashtable();
            var dt = DateTime.Now;
            var dayofWeek1 = (int)dt.DayOfWeek == 0 ? 7 : (int)dt.DayOfWeek;
            var startTime = DateTime.Parse(string.Format("{0:yyyy-MM-dd}", dt.AddDays(-(dayofWeek1 - 1))));
            var endTime = DateTime.Parse(string.Format("{0:yyyy-MM-dd}", dt.AddDays(-(dayofWeek1) + 7)));
            //IEnumerable<IGrouping<string, SwsEventHistory>> Eventlist = eventHistoryService.Query<SwsEventHistory>(r => r.EventTime >= startTime && r.EventTime <= endTime && r.State == 0 && rtuIds.Contains(r.Rtuid)).ToList().GroupBy(r => r.EventMessage);
            //Eventlist.ToList().Sort(delegate (IGrouping<string, SwsEventHistory> p1, IGrouping<string, SwsEventHistory> p2)
            //{
            //    return p1.Count().CompareTo(p2.Count());

            //});
            //Eventlist = Eventlist.ToList().OrderBy(delegate (IGrouping<string, SwsEventHistory> p1)
            //{
            //    return p1.Count();
            //});
            //Eventlist = Eventlist.Skip(Eventlist.Count() - 5).ToList();
            //string[] name = new string[Eventlist.Count()];
            //int number = 0;
            //foreach (IGrouping<string, SwsEventHistory> info in Eventlist)
            //{
            //    sourcedata = new Hashtable();
            //    sourcedata.Add("value", info.Count());
            //    sourcedata.Add("name", info.Key);
            //    eventsourcelist.Add(sourcedata);
            //    name[number] = info.Key;
            //    number++;
            //}
            var eventsourcelist = eventHistoryService.Query<SwsEventHistory>(r => r.EventTime >= startTime && r.EventTime <= endTime && r.State == 0 && rtuIds.Contains(r.Rtuid))
                .GroupBy(r => r.EventMessage)
                .Select(a => new
                {
                    name = a.Key,
                    value = a.Count()
                }).OrderByDescending(a => a.value).Take(5);

            var name = eventsourcelist.Select(a => a.name).ToArray();

            var rel = new
            {
                eventsourcelist,
                name
            };
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(rel));
        }
        #region 获取 本周、本月、本季度、本年 的开始时间或结束时间
        /// <summary>
        /// 获取结束时间
        /// </summary>
        /// <param name="TimeType">Week、Month、Season、Year</param>
        /// <param name="now"></param>
        /// <returns></returns>
        public static DateTime? GetTimeStartByType(string TimeType, DateTime now)
        {
            switch (TimeType)
            {
                case "Week":
                    return now.AddDays(-(int)now.DayOfWeek + 1);
                case "Month":
                    return now.AddDays(-now.Day + 1);
                case "Season":
                    var time = now.AddMonths(0 - ((now.Month - 1) % 3));
                    return time.AddDays(-time.Day + 1);
                case "Year":
                    return now.AddDays(-now.DayOfYear + 1);
                default:
                    return null;
            }
        }

        /// <summary>
        /// 获取结束时间
        /// </summary>
        /// <param name="TimeType">Week、Month、Season、Year</param>
        /// <param name="now"></param>
        /// <returns></returns>
        public static DateTime? GetTimeEndByType(string TimeType, DateTime now)
        {
            switch (TimeType)
            {
                case "Week":
                    return now.AddDays(7 - (int)now.DayOfWeek);
                case "Month":
                    return now.AddMonths(1).AddDays(-now.AddMonths(1).Day + 1).AddDays(-1);
                case "Season":
                    var time = now.AddMonths((3 - ((now.Month - 1) % 3) - 1));
                    return time.AddMonths(1).AddDays(-time.AddMonths(1).Day + 1).AddDays(-1);
                case "Year":
                    var time2 = now.AddYears(1);
                    return time2.AddDays(-time2.DayOfYear);
                default:
                    return null;
            }
        }
        #endregion

        //设备增长表
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult loadDeviceUp()
        {
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = _UserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
            var startTime = GetTimeStartByType("Year", DateTime.Now);
            var endTime = GetTimeEndByType("Year", DateTime.Now);
            var device = _sws_DeviceInfo01Service.loadDeviceUp(user, startTime, endTime);
            List<string> xAxis = new List<string>();
            List<double> datas = new List<double>();
            foreach (var item in device)
            {
                xAxis.Add(item.CreateTime.ToString() + "月");
                datas.Add(double.Parse(item.CountRtu.ToString()));
            }
            var rel = new
            {
                xAxis,
                datas
            };
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(rel));
        }

        //设备实时监控数据
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult LoadDeviceJK()
        {
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = _UserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
            List<SwsDeviceInfo01> devicelist = new List<SwsDeviceInfo01>();
            if (user.IsAdmin)
            {
                devicelist = _sws_DeviceInfo01Service.Query<SwsDeviceInfo01>(r => r.Rtuid != null && r.Partition != 6).Take(20).ToList();
            }
            else
            {
                var ids = _sws_UserStationService.Query<SwsUserStation>(r => r.UserId == user.UserId).Select(r => r.StationId).ToList();
                //devicelist = _sws_DeviceInfo01Service.Query<SwsDeviceInfo01>(r => _sws_UserStationService.Query<SwsUserStation>(r => r.UserId == user.UserId).Select(r => r.StationId).Contains(r.StationId) && r.Rtuid != null).Take(20).ToList();
                devicelist = _sws_DeviceInfo01Service.Query<SwsDeviceInfo01>(r => ids.Contains(r.StationId) && r.Rtuid != null && r.Partition != 6).Take(20).ToList();
            }
            var rtuids = string.Join(",", devicelist.Select(r => r.Rtuid).ToList());
            string macJson = "{\"RTUID\":{'$in':[" + rtuids + "]}}";
            var collections = new MongoDBHelper<RtuJKInfo>(DateTime.Now.Year.ToString());
            var jklist = collections.Find("Sws_DeviceJKInfo", macJson, "UpdateTime");
            List<mapmontorJK> dataList = new List<mapmontorJK>();
            foreach (var it in devicelist)
            {
                if (it.Rtuid.ToString() != "")
                {
                    var partion = it.Partition;
                    mapmontorJK jkdata = new mapmontorJK();
                    var datajk = jklist.Where(r => r.RTUID == it.Rtuid).FirstOrDefault();
                    if (datajk != null)
                    {
                        jkdata.devicename = it.DeviceName;
                        int pressout = 2001 + (partion - 1) * 500;
                        jkdata.PressOut = new KeyValuePair<string, bool>(datajk.AnalogValues.ContainsKey(pressout.ToString()) ? datajk.AnalogValues[pressout.ToString()].ToString() : "--", false);
                        //进水压力
                        int pressinid = 2000 + (partion - 1) * 500;
                        jkdata.PressIN = new KeyValuePair<string, bool>(datajk.AnalogValues.ContainsKey(pressinid.ToString()) ? datajk.AnalogValues[pressinid.ToString()].ToString() : "--", false);
                        //瞬时流量
                        var infdata1 = datajk.AnalogValues.ContainsKey((2030 + (partion - 1) * 500).ToString()) ? datajk.AnalogValues[(2030 + (partion - 1) * 500).ToString()] : null;
                        var infdata2 = datajk.AnalogValues.ContainsKey((2032 + (partion - 1) * 500).ToString()) ? datajk.AnalogValues[(2032 + (partion - 1) * 500).ToString()] : null;
                        var infdata3 = datajk.AnalogValues.ContainsKey((2034 + (partion - 1) * 500).ToString()) ? datajk.AnalogValues[(2034 + (partion - 1) * 500).ToString()] : null;
                        var infdata4 = datajk.AnalogValues.ContainsKey((2036 + (partion - 1) * 500).ToString()) ? datajk.AnalogValues[(2036 + (partion - 1) * 500).ToString()] : null;
                        if (infdata1 == null && infdata2 == null && infdata3 == null && infdata4 == null)
                        {
                            jkdata.inflow = "--";
                        }
                        else
                        {
                            var inflowtemp = Math.Round(Convert.ToDouble(infdata1) + Convert.ToDouble(infdata2) + Convert.ToDouble(infdata3) + Convert.ToDouble(infdata4), 2);
                            jkdata.inflow = inflowtemp.ToString();
                        }

                    }
                    else
                    {
                        jkdata.devicename = it.DeviceName;
                        jkdata.PressOut = new KeyValuePair<string, bool>("--", false);
                        //进水压力 
                        jkdata.PressIN = new KeyValuePair<string, bool>("--", false);
                        //瞬时流量 
                        jkdata.inflow = "--";
                    }
                    dataList.Add(jkdata);
                }
                else
                {
                    var partion = it.Partition;
                    mapmontorJK jkdata = new mapmontorJK();
                    jkdata.devicename = it.DeviceName;
                    jkdata.PressOut = new KeyValuePair<string, bool>("--", false);
                    //进水压力 
                    jkdata.PressIN = new KeyValuePair<string, bool>("--", false);
                    //瞬时流量 
                    jkdata.inflow = "--";
                    dataList.Add(jkdata);
                }
            }
            var rel = new
            {
                dataList
            };
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(rel));
        }
    }
}