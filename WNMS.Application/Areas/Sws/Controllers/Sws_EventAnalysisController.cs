using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WNMS.Model.DataModels;
using WNMS.IService;
using Microsoft.AspNetCore.Http;
using System.Collections;
using WNMS.Model.CustomizedClass;
using WNMS.Application.Utility;
using System.Security.Claims;
using WNMS.Application.Utility.Filters;

namespace WNMS.Application.Areas.Sws.Controllers
{
    [Area("Sws")]
    public class Sws_EventAnalysisController : Controller
    {
        private ISysUserService userService = null;
        private ISws_EventHistoryService eventHistoryService = null;
        private ISws_RTUInfoService rtuService = null;
        private ISws_StationService stationService = null;
        private ISws_UserStationService userstationService = null;
        public Sws_EventAnalysisController(ISysUserService sys_userService, ISws_EventHistoryService sws_EventHistoryService,
            ISws_RTUInfoService sws_RTUInfoService, ISws_StationService sws_StationService, ISws_UserStationService sws_UserStationService)
        {
            userService = sys_userService;
            eventHistoryService = sws_EventHistoryService;
            rtuService = sws_RTUInfoService;
            stationService = sws_StationService;
            userstationService = sws_UserStationService;
        }
        public IActionResult Index()
        {
            ViewBag.BeginDate = DateTime.Now.AddDays(-3).ToString("yyyy-MM-dd");
            ViewBag.EndDate = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            return View();
        }

        [HttpPost]
        [TypeFilter(typeof(IgonreActionFilter))]
        //品牌分类报警分析
        public IActionResult GetAlarmPinPaiData(string beginDate, string endDate, string type)
        {
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = userService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
            if (type == "year")
            {
                beginDate = beginDate + "-01-01";
                endDate = endDate + "-01-01";
            }

            //时间及获取数据库名称处理
            DateTime begindate = Convert.ToDateTime(beginDate);
            DateTime enddate = Convert.ToDateTime(endDate);

            List<int> rtuIds = GetRtuID(user);
            var itemid = (int)Model.CustomizedClass.Enum.设备品牌;

            List<Eventmau> eventList = new List<Eventmau>();
            eventList = eventHistoryService.GetPinPaiData(begindate, enddate, rtuIds, itemid).ToList();

            List<string> xAxis = new List<string>();
            List<int> data = new List<int>();
            if (eventList.Count > 0)
            {
                foreach (var info in eventList)
                {
                    xAxis.Add(info.Name == null ? "未知品牌设备" : info.Name);
                    data.Add(info.Count);
                }
            }

            //数据返回
            var rel = new
            {
                xAxis = xAxis,
                data = data
            };
            string result = Newtonsoft.Json.JsonConvert.SerializeObject(rel);
            return Content(result);
        }

        [HttpPost]
        [TypeFilter(typeof(IgonreActionFilter))]
        //设备类型报警数据获取
        public IActionResult GetAlarmDeviceTypeData(string beginDate, string endDate,string type)
        {
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = userService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
            if (type == "year")
            {
                beginDate = beginDate + "-01-01";
                endDate = endDate + "-01-01";
            }
             
            //时间及获取数据库名称处理
            DateTime begindate = Convert.ToDateTime(beginDate);
            DateTime enddate = Convert.ToDateTime(endDate);

            List<int> rtuIds = GetRtuID(user);
            var itemid = (int)Model.CustomizedClass.Enum.设备类型;

            List<Eventmau> eventList = new List<Eventmau>();
            eventList = eventHistoryService.GetDeviceTypeData(begindate, enddate, rtuIds, itemid).ToList();

            List<EventRank> rank = new List<EventRank>();

            if (eventList.Count > 0)
            {
                foreach (var info in eventList)
                {
                    EventRank ek = new EventRank();
                    ek.name = info.Name == null ? "未知类型设备" : info.Name;
                    ek.value = info.Count;
                    rank.Add(ek);
                }
            }

            //数据返回
            string result = Newtonsoft.Json.JsonConvert.SerializeObject(rank);
            return Content(result);
        }

        //获取报警频度排名
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult GetEventMessageData(string beginDate, string endDate, string type, int limit = 10, int page = 1)
        {
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = userService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
            if (type == "year")
            {
                beginDate = beginDate + "-01-01";
                endDate = endDate + "-01-01";
            }

            //时间及获取数据库名称处理
            DateTime begindate = Convert.ToDateTime(beginDate);
            DateTime enddate = Convert.ToDateTime(endDate);

            List<int> rtuIds = GetRtuID(user);
            List<IGrouping<string, SwsEventHistory>> countList = new List<IGrouping<string, SwsEventHistory>>();


            List<EventTotal> eventcount = new List<EventTotal>();
            var events = eventHistoryService.Query<SwsEventHistory>(r => r.EventTime > begindate && r.EventTime < enddate && rtuIds.Contains(r.Rtuid)).GroupBy(r => r.EventMessage).Select(r => new EventTotal
            {
                Name = r.Key,
                Count = r.Count()
            });
            //foreach (var info in countList)
            //{
            //    EventTotal hs = new EventTotal();
            //    hs.Name = info.Key.ToString();
            //    hs.Count = info.Count();
            //    eventcount.Add(hs);
            //}
            int totalCount = events.Count();
            eventcount = events.OrderByDescending(r => r.Count).Skip((page - 1) * limit).Take(limit).ToList();

            return Json(new { code = "0", msg = "", count = totalCount, data = eventcount });
        }



        //获取table数据 报警详情
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult GetEventTableData(string beginDate, string endDate, string type, int limit = 1, int page = 1)
        {
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = userService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
            if (type == "year")
            {
                beginDate = beginDate + "-01-01";
                endDate = endDate + "-01-01";
            }

            //时间及获取数据库名称处理
            DateTime begindate = Convert.ToDateTime(beginDate);
            DateTime enddate = Convert.ToDateTime(endDate);

            List<int> rtuIds = GetRtuID(user);

            int totalCount = 0;
            totalCount = eventHistoryService.Query<SwsEventHistory>(e => e.EventTime > begindate && e.EventTime < enddate && rtuIds.Contains(e.Rtuid)).Count();
            List<EventStation> eventList = eventHistoryService.GetEventDetailData(begindate, enddate, rtuIds, limit, page).ToList();

            return Json(new { code = "0", msg = "", count = totalCount, data = eventList });
        }

        //获取排行榜数据
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult GetRankingData(string beginDate, string endDate, string type, int limit = 10, int page = 1)
        {
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = userService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
            if (type == "year")
            {
                beginDate = beginDate + "-01-01";
                endDate = endDate + "-01-01";
            }
            //时间及获取数据库名称处理
            DateTime begindate = Convert.ToDateTime(beginDate);
            DateTime enddate = Convert.ToDateTime(endDate);
            int totalCount = 0;

            List<int> rtuIds = GetRtuID(user);
            List<EventRanking> list = eventHistoryService.GetRankingData(begindate, enddate, rtuIds, limit, page, ref totalCount).ToList();
            return Json(new { code = "0", msg = "", count = totalCount, data = list });
        }


        #region   获取报警详情
        //添加页面
        public IActionResult detailPage(string beginDate, string endDate,string type, int detailType, string content)
        {
            ViewBag.beginDate = beginDate;
            ViewBag.endDate = endDate;
            ViewBag.type = detailType;
            ViewBag.DateType = type;
            ViewBag.content = content;
            return View();
        }

        /// <summary>
        /// 获取报警详情数据
        /// </summary>
        /// <param name="beginDate">报警开始时间</param>
        /// <param name="endDate">报警结束时间</param>
        ///  <param name="type">时间类型</param>
        /// <param name="detailType">详情的类型（1报警内容排序详情，2设备排序详情）</param>
        /// <param name="content">筛选条件（1报警内容，2rtuid）</param>
        /// <param name="limit">每页条数</param>
        /// <param name="page">页数</param>
        /// <returns></returns>
        [TypeFilter(typeof(IgonreActionFilter))]
        [HttpPost]
        public IActionResult GetEventDetailData(string beginDate, string endDate, string type, int detailType, string content, int limit = 10, int page = 1)
        {
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = userService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
            if (type == "year")
            {
                beginDate = beginDate + "-01-01";
                endDate = endDate + "-01-01";
            }

            //时间及获取数据库名称处理
            DateTime begindate = Convert.ToDateTime(beginDate);
            DateTime enddate = Convert.ToDateTime(endDate);
            List<int> rtuIds = GetRtuID(user);

            //获取报警数据
            List<EventStation> eventlist = new List<EventStation>();
            int totalCount = 0;
            eventlist = eventHistoryService.GetDetailData(begindate, enddate, rtuIds, limit, page, detailType, content, ref totalCount).ToList();

            return Json(new { code = "0", msg = "", count = totalCount, data = eventlist });
        }
        #endregion

        //时间处理方法，根据不同的时间类型，获取时间
        public void GetDate(string dateTime, int dateType, ref string beginDate, ref string endDate, ref string str)
        {
            beginDate = DateTime.Now.ToString("yyyy-MM-dd");
            endDate = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            if (dateType == 1)
            {
                beginDate = Convert.ToDateTime(dateTime).ToString("yyyy-MM-dd");
                endDate = Convert.ToDateTime(dateTime).AddDays(1).ToString("yyyy-MM-dd");
                str = "yyyy-MM-dd HH:00:00";
            }
            if (dateType == 2)
            {
                beginDate = dateTime + "-01";
                endDate = Convert.ToDateTime(beginDate).AddMonths(1).ToString("yyyy-MM-dd");
                str = "yyyy-MM-dd";
            }
            if (dateType == 3)
            {
                beginDate = dateTime + "-01-01";
                endDate = Convert.ToDateTime(beginDate).AddYears(1).ToString("yyyy-MM-dd");
                str = "yyyy-MM";
            }
        }

        public List<int> GetRtuID(SysUser user)
        {
            List<int> rtuIds = new List<int>();
            if (user.IsAdmin)
            {
                rtuIds = rtuService.Query<SwsRtuinfo>(r => true)?.Select(r => r.Rtuid).ToList();
            }
            else
            {
                List<int> sids = userstationService.Query<SwsUserStation>(r => r.UserId == user.UserId).Select(r => r.StationId).ToList();
                if (sids != null)
                {
                    rtuIds = rtuService.Query<SwsRtuinfo>(r => sids.Contains(r.StationId))?.Select(r => r.Rtuid).ToList();
                }
            }
            return rtuIds;
        }
    }
    public class EventRank
    {
        public string name { get; set; }
        public int value { get; set; }
    }
    public class EventTotal
    {
        public string Name { get; set; }
        public int Count { get; set; }
    }
}
