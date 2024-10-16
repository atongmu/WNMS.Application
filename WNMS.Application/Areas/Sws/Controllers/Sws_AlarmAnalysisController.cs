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
    public class Sws_AlarmAnalysisController : Controller
    {
        private ISysUserService userService = null;
        private ISws_EventHistoryService eventHistoryService = null;
        private ISws_RTUInfoService rtuService = null;
        private ISws_StationService stationService = null;
        private ISws_UserStationService userstationService = null;
        public Sws_AlarmAnalysisController(ISysUserService sys_userService, ISws_EventHistoryService sws_EventHistoryService,
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
            ViewBag.DateTime = DateTime.Now.ToString("yyyy-MM-dd");
            return View();
        }

        [HttpPost]
        [TypeFilter(typeof(IgonreActionFilter))]
        //曲线图数据获取
        public IActionResult GetAlarmLineData(string dateTime, int dateType)
        {
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = userService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();

            //时间及获取数据库名称处理
            string beginDate = "";
            string endDate = "";
            string str = "";
            GetDate(dateTime, dateType, ref beginDate, ref endDate, ref str);
            DateTime begindate = Convert.ToDateTime(beginDate);
            DateTime enddate = Convert.ToDateTime(endDate);

            List<int> rtuIds = GetRtuID(user);
            List<IGrouping<string, SwsEventHistory>> eventList = new List<IGrouping<string, SwsEventHistory>>();
            eventList = eventHistoryService.Query<SwsEventHistory>(r => r.EventTime > begindate && r.EventTime < enddate && rtuIds.Contains(r.Rtuid)).AsEnumerable().GroupBy(r => r.EventTime.ToString(str)).ToList();

            List<string> xAxis = new List<string>();
            List<int> data = new List<int>();
            if (eventList.Count > 0)
            {
                foreach (var info in eventList)
                {
                    xAxis.Add(info.Key);
                    data.Add(info.Count());
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

        //获取饼图数据
        [TypeFilter(typeof(IgonreActionFilter))]
        public async Task<IActionResult> GetpieData(int dateType, string dateTime)
        {
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = userService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();

            //时间及获取数据库名称处理
            string beginDate = "";
            string endDate = "";
            string str = "";
            GetDate(dateTime, dateType, ref beginDate, ref endDate, ref str);
            DateTime begindate = Convert.ToDateTime(beginDate);
            DateTime enddate = Convert.ToDateTime(endDate);

            List<int> rtuIds = GetRtuID(user);
            List<IGrouping<byte, SwsEventHistory>> eventList = new List<IGrouping<byte, SwsEventHistory>>();
            eventList = eventHistoryService.Query<SwsEventHistory>(r => r.EventTime > begindate && r.EventTime < enddate && rtuIds.Contains(r.Rtuid)).AsEnumerable().GroupBy(r => r.EventLevel).ToList();

            List<Hashtable> eventdata = new List<Hashtable>();
            foreach (var info in eventList)
            {
                Hashtable edata = new Hashtable();
                if (info.Key == 1)
                {
                    edata.Add("name", "紧急报警");
                    edata.Add("value", info.Count());
                }
                if (info.Key == 2)
                {
                    edata.Add("name", "一般报警");
                    edata.Add("value", info.Count());
                }
                if (info.Key == 3)
                {
                    edata.Add("name", "提示报警");
                    edata.Add("value", info.Count());
                }
                eventdata.Add(edata);
            }

            List<IGrouping<string, SwsEventHistory>> countList = new List<IGrouping<string, SwsEventHistory>>();
            countList = eventHistoryService.Query<SwsEventHistory>(r => r.EventTime > begindate && r.EventTime < enddate && rtuIds.Contains(r.Rtuid)).AsEnumerable().GroupBy(r => r.EventMessage).ToList();

            List<EventTotal> eventcount = new List<EventTotal>();
            foreach (var info in countList)
            {
                EventTotal hs = new EventTotal();
                hs.Name=info.Key.ToString();
                hs.Count=info.Count();
                eventcount.Add(hs);
            }
            PartialView("_CountTable", eventcount);
            string dataTable = await ViewToString.RenderPartialViewToString(this, "_CountTable");
            int totalCount = countList.Count();
            //数据返回
            var rel = new
            {
                data = eventdata,
                count= eventcount,
                dataTable=dataTable,
                totalCount=totalCount
            };
            string result = Newtonsoft.Json.JsonConvert.SerializeObject(rel);
            return Content(result);
        }

        //获取排行榜数据
        //[TypeFilter(typeof(IgonreActionFilter))]
        //public async Task<IActionResult> GetRankingData(string dateTime, int dateType)
        //{
        //    int userID = int.Parse(User.FindFirstValue("UserID"));
        //    var user = userService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();

        //    //时间及获取数据库名称处理
        //    string beginDate = "";
        //    string endDate = "";
        //    string str = "";
        //    GetDate(dateTime, dateType, ref beginDate, ref endDate, ref str);
        //    DateTime begindate = Convert.ToDateTime(beginDate);
        //    DateTime enddate = Convert.ToDateTime(endDate);

        //    List<int> rtuIds = GetRtuID(user);
        //    List<EventRanking> list = eventHistoryService.GetRankingData(begindate, enddate, rtuIds).ToList();
        //    PartialView("_RankingTable", list);
        //    string dataTable = await ViewToString.RenderPartialViewToString(this, "_RankingTable");
        //    return Json(new
        //    {
        //        dataTable = dataTable
        //    });
        //}

        //获取table数据
        [TypeFilter(typeof(IgonreActionFilter))]
        public async Task<IActionResult> GetEventTableData(string dateTime, int dateType, int pageSize = 6, int pageIndex=1)
        {
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = userService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();

            //时间及获取数据库名称处理
            string beginDate = "";
            string endDate = "";
            string str = "";
            GetDate(dateTime, dateType, ref beginDate, ref endDate, ref str);
            DateTime begindate = Convert.ToDateTime(beginDate);
            DateTime enddate = Convert.ToDateTime(endDate);

            List<int> rtuIds = GetRtuID(user);

            int totalCount = 0;
            List<EventStation> eventList = eventHistoryService.GetEventTableData(begindate, enddate, rtuIds, pageSize, pageIndex, ref totalCount).ToList();
            PartialView("_EventHistoryTable", eventList);
            string dataTable = await ViewToString.RenderPartialViewToString(this, "_EventHistoryTable");
            double totalPage = Math.Ceiling((double)totalCount / pageSize);
            return Json(new
            {
                total = totalCount,
                totalPage = totalPage,
                pageIndex = pageIndex,
                pageSize = pageSize,
                dataTable = dataTable
            }); 
        }
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

}