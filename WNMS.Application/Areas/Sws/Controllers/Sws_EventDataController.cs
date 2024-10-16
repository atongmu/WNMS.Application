using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using WNMS.Application.Utility.Filters;
using WNMS.Application.Utility.JsonHelper;
using WNMS.IService;
using WNMS.Model.CustomizedClass;
using WNMS.Model.DataModels;
using WNMS.Utility;

namespace WNMS.Application.Areas.Sws.Controllers
{
    [Area("Sws")]
    public class Sws_EventDataController : Controller
    {
        private ISysUserService userService = null;
        private ISws_EventHistoryService eventHistoryService = null;
        private ISws_RTUInfoService rtuService = null;
        private ISws_StationService stationService = null;
        private ISws_UserStationService userstationService = null;
        private ISws_EventInfoService _sws_EventInfoService = null;
        private ISys_DataItemDetailService _sys_DataItemDetailService = null;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public Sws_EventDataController(ISysUserService sys_userService, ISws_EventHistoryService sws_EventHistoryService,
            ISws_RTUInfoService sws_RTUInfoService, ISws_StationService sws_StationService, ISws_UserStationService sws_UserStationService,
            ISws_EventInfoService sws_EventInfoService, ISys_DataItemDetailService sys_DataItemDetailService, IWebHostEnvironment webHostEnvironment
            )
        {
            userService = sys_userService;
            eventHistoryService = sws_EventHistoryService;
            rtuService = sws_RTUInfoService;
            stationService = sws_StationService;
            userstationService = sws_UserStationService;
            _sws_EventInfoService = sws_EventInfoService;
            _sys_DataItemDetailService = sys_DataItemDetailService;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }
        //高德地图
        public IActionResult GDMap()
        {
            var path = _webHostEnvironment.ContentRootPath;
            JsonFileHelper jsonFileHelper = new JsonFileHelper(path + "/Utility/ProjectJson/project.json");
            SystemInfo st = jsonFileHelper.Read<SystemInfo>("SystemInfo");
            ViewBag.lng = st.Lng;
            ViewBag.lat = st.Lat;
            ViewBag.zoom = st.MapLevel;
            ViewBag.sysName = st.SystemName;
            ViewBag.logo = st.Logo;
            ViewBag.adcode = st.AreaName;
            ViewBag.ArcgisUrl = StaticConstraint.ArcgisUrl;
            ViewBag.PipeUrl = StaticConstraint.PipeUrl;
            return View();
        }
        //获取报警泵房
        public IActionResult LoadGZrtu()
        {
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = userService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
            List<int> rtuIds = GetRtuID(user);
            //显示报警泵房
            var stationList = _sws_EventInfoService.GetPumData(rtuIds).Distinct();
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(stationList));
        }
        //获取泵房的报警信息
        public IActionResult LoadEventMap(int id)
        {
            var ids = rtuService.Query<SwsRtuinfo>(r => r.StationId == id).Select(r => r.Rtuid).ToList();
            var info = _sws_EventInfoService.LoadEventsInfo(string.Join(",", ids)).ToList();
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(info));
        }
        /// <summary>
        /// 报警数量
        /// </summary>
        /// <returns></returns>
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult LoadEventCount()
        {
            //当前报警数量
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = userService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
            List<int> rtuIds = GetRtuID(user);
            //实时报警数量
            var eventcount = _sws_EventInfoService.Query<SwsEventInfo>(r => rtuIds.Contains(r.Rtuid) && r.EventLevel != 0).Count();
            //本月报警数量
            var beginTime = GetTimeStartByType("Month", DateTime.Now);
            var monthCount = eventHistoryService.Query<SwsEventHistory>(r => rtuIds.Contains(r.Rtuid) && r.EventTime >= beginTime && r.EventLevel != 0).Count();
            var rel = new
            {
                ecount = eventcount,
                mCount = monthCount
            };
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(rel));
        }
        /// <summary>
        /// 报警信息占比分析
        /// </summary>
        /// <returns></returns>
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult LoadEventPre()
        {

            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = userService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();


            List<int> rtuIds = GetRtuID(user);
            List<IGrouping<string, SwsEventInfo>> countList = new List<IGrouping<string, SwsEventInfo>>();
            countList = _sws_EventInfoService.Query<SwsEventInfo>(r => rtuIds.Contains(r.Rtuid) && r.EventLevel != 0).AsEnumerable().GroupBy(r => r.EventMessage).ToList();

            List<EventChart> eventcount = new List<EventChart>();
            foreach (var info in countList)
            {
                EventChart hs = new EventChart();
                hs.name = info.Key.ToString();
                hs.value = info.Count();
                eventcount.Add(hs);
            }
            var endcount = eventcount.OrderBy(r => r.value).Take(6);
            var rel = new
            {
                endcount
            };
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(rel));
        }
        /// <summary>
        /// 各厂家报警占比
        /// </summary>
        /// <returns></returns>
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult LoadVendorEvent()
        {
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = userService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();

            //时间及获取数据库名称处理
            DateTime begindate = (DateTime)GetTimeStartByType("Month", DateTime.Now);
            DateTime enddate = (DateTime)GetTimeEndByType("Month", DateTime.Now);

            DateTime nmbegindate = begindate.AddMonths(-1);
            DateTime lmenddate = enddate.AddMonths(-1);
            List<int> rtuIds = GetRtuID(user);
            var itemid = (int)Model.CustomizedClass.Enum.设备品牌;
            //var csInfo = _sys_DataItemDetailService.Query<SysDataItemDetail>(r => r.FItemId == itemid).Select(r => r.ItemName).ToList();
            List<Eventmau> nmeventList = eventHistoryService.GetPinPaiData(begindate, enddate, rtuIds, itemid).ToList();//本月报警数量
            List<Eventmau> lmeventList = eventHistoryService.GetPinPaiData(nmbegindate, lmenddate, rtuIds, itemid).ToList();//上月报警数量
            List<string> vendorName = new List<string>();
            //List<string> vendorName = _sys_DataItemDetailService.Query<SysDataItemDetail>(r => r.FItemId == itemid).Select(r => r.ItemName).ToList();
            List<int?> nmdata = new List<int?>();
            List<int?> lmdata = new List<int?>();
            //获取厂商的名字

            if (nmeventList.Count > 0)
            {
                foreach (var info in nmeventList)
                {
                    if (info.Name != null)
                    {
                        vendorName.Add(info.Name);
                    }

                }
            }
            if (lmeventList.Count > 0)
            {
                foreach (var info in lmeventList)
                {
                    if (info.Name != null && !vendorName.Contains(info.Name))
                    {
                        vendorName.Add(info.Name);
                    }

                }
            }
            foreach (var it in vendorName)
            {
                nmdata.Add(nmeventList.Where(r => r.Name == it).FirstOrDefault() == null ? 0 : nmeventList.Where(r => r.Name == it).FirstOrDefault().Count);
                lmdata.Add(lmeventList.Where(r => r.Name == it).FirstOrDefault() == null ? 0 : lmeventList.Where(r => r.Name == it).FirstOrDefault().Count);
            }
            //int countAll = lmeventList.Count() > 0 ? lmeventList.Max(r => r.Count) : 0;
            int countAll = lmeventList.Count() > 0 ? lmeventList.Max(r => r.Count) : 0 + nmeventList.Count() > 0 ? nmeventList.Max(r => r.Count) : 0;
            //数据返回
            var rel = new
            {
                vendorName,
                nmdata,
                lmdata,
                countAll
            };
            string result = Newtonsoft.Json.JsonConvert.SerializeObject(rel);
            return Content(result);
        }
        /// <summary>
        /// 设备报警排名
        /// </summary>
        /// <returns></returns>
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult LoadEqEvent()
        {
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = userService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();



            DateTime begindate = (DateTime)GetTimeStartByType("Month", DateTime.Now);
            DateTime enddate = (DateTime)GetTimeEndByType("Month", DateTime.Now);

            List<int> rtuIds = GetRtuID(user);
            string rtuIdstr = "";
            foreach (var i in rtuIds)
            {
                rtuIdstr += i.ToString() + ',';
            }
            rtuIdstr = rtuIdstr.Substring(0, rtuIdstr.Length - 1);
            //List<EventRanking> list = eventHistoryService.GetRankingData(begindate, enddate, rtuIds, 5, 1, ref totalCount).OrderBy(r => r.Count).ToList();
            List<StationEventRank> list = eventHistoryService.LoadStationRank(begindate, enddate, rtuIdstr).OrderBy(r => r.EventCount).Take(5).ToList();
            List<string> axix = new List<string>();
            List<int> eventcount = new List<int>();
            foreach (var item in list)
            {
                axix.Add(item.StationName);
                eventcount.Add(item.EventCount);
            }
            //数据返回
            var rel = new
            {
                axix,
                eventcount
            };
            string result = Newtonsoft.Json.JsonConvert.SerializeObject(rel);
            return Content(result);

        }

        /// <summary>
        /// 获取报警信息频度排名
        /// </summary>
        /// <returns></returns>
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult GetEventData()
        {
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = userService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();

            //时间及获取数据库名称处理
            DateTime begindate = (DateTime)GetTimeStartByType("Month", DateTime.Now);
            DateTime enddate = (DateTime)GetTimeEndByType("Month", DateTime.Now);

            List<int> rtuIds = GetRtuID(user);
            //List<IGrouping<string, SwsEventHistory>> countList = new List<IGrouping<string, SwsEventHistory>>();
            //countList = eventHistoryService.Query<SwsEventHistory>(r => r.EventTime > begindate && r.EventTime < enddate && rtuIds.Contains(r.Rtuid)).AsEnumerable().GroupBy(r => r.EventMessage).ToList();

            //List<EventTotal> eventcount = new List<EventTotal>();
            //foreach (var info in countList)
            //{
            //    EventTotal hs = new EventTotal();
            //    hs.Name = info.Key.ToString();
            //    hs.Count = info.Count();
            //    eventcount.Add(hs);
            //}

            var eventcount = eventHistoryService.Query<SwsEventHistory>
                (r => r.EventTime > begindate && r.EventTime < enddate && rtuIds.Contains(r.Rtuid) && r.EventLevel != 0)
                .GroupBy(r => r.EventMessage)
                .Select(group => new
                {
                    Name = group.Key,
                    Count = group.Count()
                });
            int totalCount = eventcount.Count();
            eventcount = eventcount.OrderByDescending(r => r.Count).Take(10);

            //数据返回
            var rel = new
            {
                eventcount
            };
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(rel));
        }
        /// <summary>
        /// 各厂家报警排名
        /// </summary>
        /// <returns></returns>
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult LoadVendorRank()
        {
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = userService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();

            //时间及获取数据库名称处理
            DateTime begindate = (DateTime)GetTimeStartByType("Month", DateTime.Now);
            DateTime enddate = (DateTime)GetTimeEndByType("Month", DateTime.Now);

            List<int> rtuIds = GetRtuID(user);
            var itemid = (int)Model.CustomizedClass.Enum.设备品牌;
            List<Eventmau> nmeventList = eventHistoryService.GetPinPaiData(begindate, enddate, rtuIds, itemid).OrderByDescending(r => r.Count).ToList();//本月报警数量
            List<string> vendorName = new List<string>();
            List<int?> nmdata = new List<int?>();
            //获取数据 
            if (nmeventList.Count > 0)
            {
                foreach (var info in nmeventList)
                {
                    vendorName.Add(info.Name == null ? "未知品牌设备" : info.Name);
                    nmdata.Add(info.Count);
                }
            }
            //数据返回
            var rel = new
            {
                vendorName,
                nmdata

            };
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(rel));
        }
        /// <summary>
        /// 获取用户下的设备
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
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
                var ids = userstationService.Query<SwsUserStation>(r => r.UserId == user.UserId).Select(r => r.StationId).ToList();
                rtuIds = rtuService.Query<SwsRtuinfo>(r => ids.Contains(r.StationId))?.Select(r => r.Rtuid).ToList();
            }
            return rtuIds;
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

        public class EventChart
        {
            public string name { get; set; }
            public int value { get; set; }
        }
    }
}