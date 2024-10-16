using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using WNMS.Application.Utility.Filters;
using WNMS.Application.Utility.JsonHelper;
using WNMS.IService;
using WNMS.Model.CustomizedClass;
using WNMS.Model.DataModels;

namespace WNMS.Application.Areas.Sws.Controllers
{
    [Area("Sws")]
    public class Sws_FlowAnalysisController : Controller
    {
        private ISysUserService userService = null;
        private IDDayQuartZ01Service _dDayQuartZ01Service = null;
        private IDHourQuartZ01Service _dHourQuartZ01Service = null;
        private IDMonthQuartZ01Service _dMonthQuartZ01Service = null;
        private ISws_DeviceInfo01Service _sws_DeviceInfo01Service = null;
        private ISws_UserStationService _sws_UserStationService = null;
        private ISws_RTUInfoService _RTUInfoService = null;
        private ISws_ConsumpSettingService _ConsumpSettingService = null;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public Sws_FlowAnalysisController(ISysUserService sys_userService, IDDayQuartZ01Service dDayQuartZ01Service,
            IDHourQuartZ01Service dHourQuartZ01Service,
            IDMonthQuartZ01Service dMonthQuartZ01Service,
            ISws_RTUInfoService sws_RTUInfoService,
            ISws_DeviceInfo01Service sws_DeviceInfo01Service,
            ISws_UserStationService sws_UserStationService,
            ISws_ConsumpSettingService sws_ConsumpSettingService,
             IWebHostEnvironment webHostEnvironment
            )
        {
            userService = sys_userService;
            _dDayQuartZ01Service = dDayQuartZ01Service;
            _dHourQuartZ01Service = dHourQuartZ01Service;
            _RTUInfoService = sws_RTUInfoService;
            _sws_DeviceInfo01Service = sws_DeviceInfo01Service;
            _sws_UserStationService = sws_UserStationService;
            _dMonthQuartZ01Service = dMonthQuartZ01Service;
            _webHostEnvironment = webHostEnvironment;
            _ConsumpSettingService = sws_ConsumpSettingService;
        }
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult Index()
        {
            ViewBag.TreeNodes = new HtmlString(GetDeviceTree(""));
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = userService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
            ViewBag.deviceid = _ConsumpSettingService.GetDevice_init(user.UserId, user.IsAdmin).FirstOrDefault().DeviceID;

            var path = _webHostEnvironment.ContentRootPath;
            JsonFileHelper jsonFileHelper = new JsonFileHelper(path + "/Utility/ProjectJson/project.json");
            SystemInfo st = jsonFileHelper.Read<SystemInfo>("SystemInfo");

            ViewBag.sysName = st.SystemName;
            ViewBag.logo = st.Logo;

            return View();
        }
        //今日昨日用水量
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult LoadFlowData(long id, int day)
        {
            var daystart = DateTime.Now;
            var dayend = DateTime.Now;
            if (day == 1)//今天
            {
                //今日用水量
                daystart = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00");
                dayend = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59");

            }
            else if (day == 2)
            {
                //昨日用水量
                daystart = DateTime.Parse(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd") + " 00:00:00");
                dayend = DateTime.Parse(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd") + " 23:59:59");
            }
            var dayFlow = _dHourQuartZ01Service.Query<DhourQuartZ01>(r => r.DeviceId == id && r.UpdateTime >= daystart && r.UpdateTime <= dayend).ToList();
            //用水量总和
            var dayFlowcount = dayFlow.Sum(r => r.FlowCon);
            IEnumerable<IGrouping<DateTime, DhourQuartZ01>> dayflowlist = dayFlow.GroupBy(r => r.UpdateTime);
            List<string> timelist = new List<string>();
            List<decimal> datalist = new List<decimal>();
            foreach (var item in dayflowlist)
            {
                timelist.Add(item.Key.Hour.ToString() + ":00");
                datalist.Add(item.FirstOrDefault().FlowCon);
            }
            var rel = new
            {
                timelist,
                datalist,
                dayFlowcount
            };
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(rel));
        }
        //本周用水量
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult LoadWeekFlow(long id, int day)
        {
            var dt = DateTime.Now;
            var dayofWeek1 = (int)dt.DayOfWeek == 0 ? 7 : (int)dt.DayOfWeek;
            var daystart = DateTime.Parse(string.Format("{0:yyyy-MM-dd}", dt.AddDays(-(dayofWeek1 - 1))));
            var dayend = DateTime.Parse(string.Format("{0:yyyy-MM-dd 23:59:59}", dt.AddDays(-(dayofWeek1) + 7)));
            var dayFlow = _dDayQuartZ01Service.Query<DdayQuartZ01>(r => r.DeviceId == id && r.UpdateTime >= daystart && r.UpdateTime <= dayend).ToList();
            //用水量总和 = 今日之前 + 今日
            var dayFlowcount = dayFlow.Sum(r => r.FlowCon);
            //今日用水量
            var tdaystart = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00");
            var tdayend = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59");
            var tdayFlow = _dHourQuartZ01Service.Query<DhourQuartZ01>(r => r.DeviceId == id && r.UpdateTime >= tdaystart && r.UpdateTime <= tdayend).ToList();
            //用水量总和
            var tdayFlowcount = tdayFlow.Sum(r => r.FlowCon);
            dayFlowcount = dayFlowcount + tdayFlowcount;
            IEnumerable<IGrouping<DateTime, DdayQuartZ01>> dayflowlist = dayFlow.GroupBy(r => r.UpdateTime);
            string[] weekdays = { "星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六" };
            List<string> timelist = new List<string>() { };
            List<decimal> datalist = new List<decimal>();
            foreach (var item in dayflowlist)
            {
                //timelist.Add(((int)item.Key.DayOfWeek).ToString());
                timelist.Add(weekdays[Convert.ToInt32(item.Key.DayOfWeek)]);
                datalist.Add(item.FirstOrDefault().FlowCon);
            }
            timelist.Add(weekdays[Convert.ToInt32(DateTime.Now.DayOfWeek)]);
            datalist.Add(tdayFlowcount);
            var rel = new
            {
                timelist,
                datalist,
                dayFlowcount
            };
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(rel));
        }
        //本月用水量
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult LoadMonthFlow(long id, int day)
        {
            var dt = DateTime.Now;
            DateTime daystart = DateTime.Parse(dt.AddDays(1 - dt.Day).ToString("yyyy-MM-dd 00:00:00")).AddSeconds(-1);  //本月月初
            DateTime dayend = daystart.AddMonths(1).AddSeconds(-1);  //本月月末//
            var dayFlow = _dDayQuartZ01Service.Query<DdayQuartZ01>(r => r.DeviceId == id && r.UpdateTime >= daystart && r.UpdateTime <= dayend).ToList();
            //用水量总和
            var dayFlowcount = dayFlow.Sum(r => r.FlowCon);

            //今日用水量
            var tdaystart = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00");
            var tdayend = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59");
            var tdayFlow = _dHourQuartZ01Service.Query<DhourQuartZ01>(r => r.DeviceId == id && r.UpdateTime >= tdaystart && r.UpdateTime <= tdayend).ToList();
            dayFlowcount = dayFlowcount + tdayFlow.Sum(r => r.FlowCon);
            IEnumerable<IGrouping<DateTime, DdayQuartZ01>> dayflowlist = dayFlow.GroupBy(r => r.UpdateTime);
            List<string> timelist = new List<string>() { };
            List<decimal> datalist = new List<decimal>();
            foreach (var item in dayflowlist)
            {
                timelist.Add(item.Key.Day.ToString());
                datalist.Add(item.FirstOrDefault().FlowCon);
            }
            var rel = new
            {
                timelist,
                datalist,
                dayFlowcount
            };
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(rel));
        }
        //年度统计
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult LoadYearFlow(long id)
        {
            var startTime = DateTime.Parse(DateTime.Now.AddDays(-DateTime.Now.DayOfYear + 1).ToString("yyyy-MM-dd 00:00:00"));
            //var time2 = DateTime.Now.AddYears(1);
            //var endTime = time2.AddDays(-time2.DayOfYear); 
            var endTime = startTime.AddYears(1).AddSeconds(-1);
            var dayFlow = _dMonthQuartZ01Service.Query<DmonthQuartZ01>(r => r.DeviceId == id && r.UpdateTime >= startTime && r.UpdateTime <= endTime).ToList();
            //用水量总和
            var dayFlowcount = dayFlow.Sum(r => r.FlowCon);
            IEnumerable<IGrouping<DateTime, DmonthQuartZ01>> dayflowlist = dayFlow.GroupBy(r => r.UpdateTime).OrderBy(t => t.Key);
            List<string> timelist = new List<string>() { };
            List<decimal> dataalist = new List<decimal>();
            List<Hashtable> datalist = new List<Hashtable>();
            Hashtable hdata = new Hashtable();
            foreach (var item in dayflowlist)
            {
                timelist.Add(item.Key.Month.ToString() + "月");
                hdata = new Hashtable();
                hdata.Add("value", item.FirstOrDefault()?.FlowCon);
                hdata.Add("name", item.Key.Month.ToString() + "月");
                datalist.Add(hdata);
                dataalist.Add(item.FirstOrDefault().FlowCon);
            }
            var rel = new
            {
                timelist,
                datalist,
                dayFlowcount,
                dataalist
            };
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(rel));
        }
        //近一周用水量
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult LoadLate7Day(long id)
        {
            var dt = DateTime.Now;
            DateTime daystart = DateTime.Parse(dt.AddDays(-7).ToString("yyyy-MM-dd") + " 00:00:00");
            DateTime dayend = DateTime.Parse(dt.AddDays(-1).ToString("yyyy-MM-dd") + " 23:59:59");
            var dayFlow = _dDayQuartZ01Service.Query<DdayQuartZ01>(r => r.DeviceId == id && r.UpdateTime >= daystart && r.UpdateTime <= dayend).ToList();
            //用水量总和
            var dayFlowcount = dayFlow.Sum(r => r.FlowCon);
            IEnumerable<IGrouping<DateTime, DdayQuartZ01>> dayflowlist = dayFlow.GroupBy(r => r.UpdateTime);
            List<string> timelist = new List<string>() { };
            //List<decimal> datalist = new List<decimal>();
            List<Hashtable> datalist = new List<Hashtable>();
            Hashtable hdata = new Hashtable();
            foreach (var item in dayflowlist)
            {
                timelist.Add(item.Key.Month.ToString() + "-" + item.Key.Day.ToString());
                //datalist.Add(item.FirstOrDefault().FlowCon);
                hdata = new Hashtable();
                hdata.Add("value", item.FirstOrDefault()?.FlowCon);
                hdata.Add("name", item.Key.Month.ToString() + "-" + item.Key.Day.ToString());
                datalist.Add(hdata);
            }
            var rel = new
            {
                timelist,
                datalist,
                dayFlowcount
            };
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(rel));
        }
        //查询本周上周上上周的用水量
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult LoadThreeWeek(long id)
        {
            //本周
            var dt = DateTime.Now;
            var dayofWeek1 = (int)dt.DayOfWeek == 0 ? 7 : (int)dt.DayOfWeek;
            var start_time_current_week = DateTime.Parse(string.Format("{0:yyyy-MM-dd}", dt.AddDays(-(dayofWeek1 - 1))));
            var end_time_current_week = DateTime.Parse(string.Format("{0:yyyy-MM-dd 23:59:59}", dt.AddDays(-(dayofWeek1) + 7)));
            //上周
            var start_time_last_week = start_time_current_week.AddDays(-7);//上周星期一
            var end_time_last_week = end_time_current_week.AddDays(-7);//上周星期天 
            //前周
            var sstart_time_last_week = start_time_current_week.AddDays(-14);//上周星期一
            var send_time_last_week = end_time_last_week.AddDays(-14);//上周星期天 
            //上上上周
            var ssstart_time_last_week = start_time_current_week.AddDays(-21);//上上上周星期一
            var ssend_time_last_week = end_time_last_week.AddDays(-21);//上上上周星期天 
            //定义存储
            List<Hashtable> datalist = new List<Hashtable>();
            Hashtable hdata = new Hashtable();
            decimal sssweek = 0;
            decimal qianweek = 0;
            decimal thisweek = 0;
            decimal lastweek = 0;
            decimal nextweek = 0;
            //获取日期的所有数据
            var dayFlow = _dDayQuartZ01Service.Query<DdayQuartZ01>(r => r.DeviceId == id && r.UpdateTime >= ssstart_time_last_week && r.UpdateTime <= end_time_current_week).ToList();
            //上上上周数据
            var sssdayFlow = dayFlow.Where(r => r.UpdateTime >= ssstart_time_last_week && r.UpdateTime <= ssend_time_last_week);
            sssweek = sssdayFlow.Sum(r => r.FlowCon);
            //上上周数据
            var ssdayFlow = dayFlow.Where(r => r.UpdateTime >= sstart_time_last_week && r.UpdateTime <= send_time_last_week);
            qianweek = ssdayFlow.Sum(r => r.FlowCon);
            hdata.Add("value", qianweek);
            hdata.Add("name", "前周");
            datalist.Add(hdata);

            //上周数据
            var sdayFlow = dayFlow.Where(r => r.UpdateTime >= start_time_last_week && r.UpdateTime <= end_time_last_week);
            hdata = new Hashtable();
            lastweek = sdayFlow.Sum(r => r.FlowCon);
            hdata.Add("value", lastweek);
            hdata.Add("name", "上周");
            datalist.Add(hdata);

            //本周数据
            //今日用水量
            var tdaystart = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00");
            var tdayend = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59");
            var tdayFlow = _dHourQuartZ01Service.Query<DhourQuartZ01>(r => r.DeviceId == id && r.UpdateTime >= tdaystart && r.UpdateTime <= tdayend).ToList();
            //用水量总和
            var tdayFlowcount = tdayFlow.Sum(r => r.FlowCon);
            var bdayFlow = dayFlow.Where(r => r.UpdateTime >= start_time_current_week && r.UpdateTime <= end_time_current_week);
            hdata = new Hashtable();
            thisweek = bdayFlow.Sum(r => r.FlowCon) + tdayFlowcount;
            hdata.Add("value", thisweek);
            hdata.Add("name", "本周");
            datalist.Add(hdata);
            nextweek = Math.Round(((qianweek + sssweek + lastweek) / 3), 2);
            var rel = new
            {
                datalist,
                qianweek,
                thisweek,
                lastweek,
                nextweek
            };
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(rel));
        }
        //获取近十天用水量增比
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult GetFlowTable(long id)
        {
            string beginDate = DateTime.Now.AddDays(-11).ToString("yyyy-MM-dd");
            var beginTime = Convert.ToDateTime(beginDate);
            var endDate = DateTime.Now.AddDays(1);
            //获取日期的所有数据
            var dataList = _dDayQuartZ01Service.Query<DdayQuartZ01>(r => r.DeviceId == id && r.UpdateTime >= beginTime && r.UpdateTime <= endDate).ToList();
            List<flowtable> flowDataList = new List<flowtable>();
            for (var i = 1; i < dataList.Count(); i++)
            {
                flowtable fLowReport = new flowtable();
                fLowReport.Data = dataList[i].FlowCon;
                fLowReport.Time = dataList[i].UpdateTime.ToString("yyyy-MM-dd");
                fLowReport.Num = i;
                var yestdata = dataList[i - 1].FlowCon;
                if (yestdata != 0)
                {
                    decimal data = dataList[i].FlowCon;
                    fLowReport.Percent = (Math.Round((double)(fLowReport.Data - yestdata) * 100 / (double)yestdata, 2)).ToString() + "%";
                }
                else
                {
                    fLowReport.Percent = 0.0 + "%";
                }

                flowDataList.Add(fLowReport);
            }
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(flowDataList));
        }
        //获取本日本月预测用水量
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult LoadFlowPrediction(long id)
        {
            decimal sssday = 0;
            decimal qianday = 0;
            decimal lastday = 0;
            decimal nextday = 0;
            //昨日时间
            var yesdaystart = DateTime.Parse(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd") + " 00:00:00");
            var yesdayend = DateTime.Parse(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd") + " 23:59:59");
            //前日时间
            var qyesdaystart = DateTime.Parse(DateTime.Now.AddDays(-2).ToString("yyyy-MM-dd") + " 00:00:00");
            var qyesdayend = DateTime.Parse(DateTime.Now.AddDays(-2).ToString("yyyy-MM-dd") + " 23:59:59");
            //前前日时间
            var qqyesdaystart = DateTime.Parse(DateTime.Now.AddDays(-3).ToString("yyyy-MM-dd") + " 00:00:00");
            var qqyesdayend = DateTime.Parse(DateTime.Now.AddDays(-4).ToString("yyyy-MM-dd") + " 23:59:59");
            //所有数据
            var dayFlow = _dHourQuartZ01Service.Query<DhourQuartZ01>(r => r.DeviceId == id && r.UpdateTime >= qqyesdaystart && r.UpdateTime <= yesdayend).ToList();
            sssday = dayFlow.Where(r => r.UpdateTime >= qqyesdaystart && r.UpdateTime <= qqyesdayend).Sum(r => r.FlowCon);
            qianday = dayFlow.Where(r => r.UpdateTime >= qyesdaystart && r.UpdateTime <= qyesdayend).Sum(r => r.FlowCon);
            lastday = dayFlow.Where(r => r.UpdateTime >= yesdaystart && r.UpdateTime <= yesdayend).Sum(r => r.FlowCon);
            nextday = Math.Round(((sssday + qianday + lastday) / 3), 2);
            //===月数据
            decimal sssmonth = 0;
            decimal qianmonth = 0;
            decimal lastmonth = 0;
            decimal nextmonth = 0;
            var dt = DateTime.Now;
            DateTime daystart = DateTime.Parse(dt.AddDays(1 - dt.Day).ToString("yyyy-MM-dd") + " 00:00:00");  //本月月初
            DateTime dayend = daystart.AddMonths(1).AddSeconds(-1);  //本月月末//
            //上月
            var lastmonthstart = daystart.AddMonths(-1);
            var lastmonthend = daystart.AddSeconds(-1);
            //上上月
            var slastmonthstart = daystart.AddMonths(-2);
            var slastmonthend = lastmonthstart.AddDays(-1);
            //上上上月
            var sslastmonthstart = daystart.AddMonths(-3);
            var sslastmonthend = sslastmonthstart.AddMonths(1).AddSeconds(-1);
            var monthFlow = _dHourQuartZ01Service.Query<DhourQuartZ01>(r => r.DeviceId == id && r.UpdateTime >= sslastmonthstart && r.UpdateTime <= lastmonthend).ToList();
            sssmonth = monthFlow.Where(r => r.UpdateTime >= sslastmonthstart && r.UpdateTime <= sslastmonthend).Sum(r => r.FlowCon);
            qianmonth = monthFlow.Where(r => r.UpdateTime >= slastmonthstart && r.UpdateTime <= slastmonthend).Sum(r => r.FlowCon);
            lastmonth = monthFlow.Where(r => r.UpdateTime >= lastmonthstart && r.UpdateTime <= lastmonthend).Sum(r => r.FlowCon);
            nextmonth = Math.Round(((sssmonth + qianmonth + lastmonth) / 3), 2);
            var rel = new
            {
                qianday,
                lastday,
                nextday,
                qianmonth,
                lastmonth,
                nextmonth
            };
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(rel));
        }
        //同时段瞬时流量查询
        [TypeFilter(typeof(IgonreActionFilter))]
        public ActionResult LoadInstantFlow(long deviceid)
        {
            string beginDate = DateTime.Now.AddDays(-3).ToString("yyyy-MM-dd");
            string endDate = DateTime.Now.AddDays(1).ToString();
            string year = Convert.ToDateTime(beginDate).Year.ToString();

            double data1 = 0.0, data2 = 0.0, data3 = 0.0, data4 = 0.0;
            var model = _sws_DeviceInfo01Service.Find<SwsDeviceInfo01>(deviceid);
            var rtuid = model.Rtuid == null ? 0 : model.Rtuid;
            //数据查询参数处理
            string[] sstr = { 2030 + (model.Partition - 1) * 500 + "", 2032 + (model.Partition - 1) * 500 + "", 2034 + (model.Partition - 1) * 500 + "", 2036 + (model.Partition - 1) * 500 + "" };

            string group = "{$group:{'_id':{'$dateToString':{'format':'%Y-%m-%d','date':'$UpdateTime'}}," +
                      "'Mindata':{$min:'$AnalogValues." + sstr[0] + "'},'Mindata2':{$min:'$AnalogValues." + sstr[1] + "'}," +
                   "'Mindata3':{$min:'$AnalogValues." + sstr[2] + "'},'Mindata4':{$min:'$AnalogValues." + sstr[3] + "'}," +
                   "}}";
            string project = "{$project:{'_id': 0,'Data1':{$ifNull:['$Mindata', 0.0]},'Data2':{$ifNull:['$Mindata2', 0.0]}," +
                    "'Data3':{$ifNull:['$Mindata3', 0.0]},'Data4':{$ifNull:['$Mindata4', 0.0]}," +
                    "'Time':'$_id'}}";




            //数据查询
            List<DetailFlowData> dataList = new List<DetailFlowData>();
            dataList = _RTUInfoService.GetDetailFlowData(year, beginDate, endDate, group, project, rtuid.ToString()).ToList();
            //var datelist = list.GroupBy(r => r.group).Select(r => r.Key).ToList();

            //跨年数据添加
            if (Convert.ToDateTime(beginDate).Year != DateTime.Now.Year)
            {
                string lastyear = DateTime.Now.Year.ToString();
                List<DetailFlowData> lastData = _RTUInfoService.GetDetailFlowData(lastyear, beginDate, endDate, group, project, rtuid.ToString()).ToList();
                if (lastData.Count() > 0)
                {
                    dataList = dataList.Union(lastData).ToList();
                }
            }
            List<DetailFlowData> data = new List<DetailFlowData>();
            List<historyChar> result = new List<historyChar>();
            if (dataList.Count() > 0)
            {
                int length = dataList.Count - 1;
                DetailFlowData tomorrow = null;
                DetailFlowData day = null;
                for (int i = 0; i < length; i++)
                {
                    DetailFlowData d = new DetailFlowData();
                    tomorrow = dataList[i + 1];
                    day = dataList[i];
                    d.Time = day.Time;
                    d.Data1 = tomorrow.Data1 - day.Data1;
                    d.Data2 = tomorrow.Data2 - day.Data2;
                    d.Data3 = tomorrow.Data3 - day.Data3;
                    d.Data4 = tomorrow.Data4 - day.Data4;
                    double? da = (tomorrow.Data1 - day.Data1 ?? 0.0) + (tomorrow.Data2 - day.Data2 ?? 0.0) + (tomorrow.Data3 - day.Data3 ?? 0.0) + (tomorrow.Data4 - day.Data4 ?? 0.0);
                    var power = da == 0.0 ? (tomorrow.Data - day.Data ?? 0.0) : da;
                    d.Data = power;
                    data.Add(d);

                }


            }
            return Content("");
        }
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult LoadFlow(long deviceid)
        {
            //曲线数据
            string Dev_BeginDate = DateTime.Now.ToShortDateString();
            //double interval = 1000 * 60 * 60;
            double interval = 1000 * 60 * 1;
            var model = _sws_DeviceInfo01Service.Find<SwsDeviceInfo01>(deviceid);
            var rtuid = model.Rtuid == null ? 0 : model.Rtuid;
            var dataid = 2030 + (model.Partition - 1) * 500;
            //数据查询参数处理
            string[] sstrc = new string[] { };
            bool isAblity = true;
            if (model.Partition==6)
            {
                sstrc = new string[] {"4510"};
                isAblity = true;
                
            }
            else
            {
                sstrc = new string[] { 2030 + (model.Partition - 1) * 500 + "", 2032 + (model.Partition - 1) * 500 + "", 2034 + (model.Partition - 1) * 500 + "", 2036 + (model.Partition - 1) * 500 + "" };
                isAblity = false;
            }
            var list = _sws_DeviceInfo01Service.GetOverTimeHiatroryAll("day", 3, interval, Dev_BeginDate, dataid, isAblity, rtuid.ToString(), sstrc);
            //var list = _sws_DeviceInfo01Service.GetOverTimeHiatrory("day", 3, interval, Dev_BeginDate, dataid, false, rtuid.ToString());
            var datelist = list.GroupBy(r => r.group).Select(r => r.Key).ToList();
            List<historyChar> result = new List<historyChar>();
            List<double> intValue = new List<double>();
            DateTime dtUTC = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var flag = 0;//判断是否有值
            foreach (var item in datelist)
            {
                var dataitem = list.Where(r => r.group == item);
                StringBuilder datastring = new StringBuilder();
                historyChar chardata = new historyChar();
                chardata.name = item;
                foreach (var it in dataitem)
                {

                    //DateTime dt = it.datetime.ToUniversalTime();
                    //string dateUTC = Convert.ToInt64((dt - dtUTC).TotalMilliseconds).ToString();

                    //string strPIN = "{name:'" + it.datetime.ToString("yyyy-MM-dd HH:mm:ss") + "',value:['" + it.datetime.ToString("yyyy-MM-dd HH:mm:ss") + "'," + Math.Round(it.data, 2) + "]},";
                    var allflow = it.data1;
                    //intValue.Add(allflow);
                    //if (chardata.name == DateTime.Now.Day.ToString("yyy-MM-dd"))
                    //{
                    //    intValue.Add(allflow);

                    //}
                    string strPIN = "{name:'2000-01-01 " + it.datetime.ToString("HH:mm:ss") + "',value:['2000-01-01 " + it.datetime.ToString("HH:mm:ss") + "'," + Math.Round(allflow, 2) + "]},";
                    datastring.Append(strPIN);
                    flag = flag + 1;

                }
                if (datastring.ToString() != "")
                {

                    chardata.data = "[" + datastring.ToString().Substring(0, datastring.ToString().Length - 1) + "]"; ;
                }
                result.Add(chardata);
            }
            var dat = DateTime.Now.ToString("yyy-MM-dd");
            var dataitema = list.Where(r => r.group == dat);
            foreach (var it in dataitema)
            {
                var allflow = it.data1;
                intValue.Add(allflow);
            }

            ////今日最大值、最小值及平均值
            //string beginDate = DateTime.Now.ToString("yyyy-MM-dd");
            //string endDate = Convert.ToDateTime(beginDate).AddDays(1).Date.ToString();
            //string year = Convert.ToDateTime(beginDate).Year.ToString();
            //string sstr = "2030";
            //string group = "{$group:{'_id':{'$dateToString':{'format':'%Y-%m-%d','date':'$UpdateTime'}}," +
            //          "'mdata1':{$min:'$AnalogValues." + sstrc[0] + "'},'mdata2':{$min:'$AnalogValues." + sstrc[1] + "'}," +
            //          "'mdata3':{$min:'$AnalogValues." + sstrc[2] + "'},'mdata4':{$min:'$AnalogValues." + sstrc[3] + "'}," +
            //          "'xdata1':{$max:'$AnalogValues." + sstrc[0] + "'},'xdata2':{$max:'$AnalogValues." + sstrc[1] + "'}," +
            //          "'xdata3':{$max:'$AnalogValues." + sstrc[2] + "'},'xdata4':{$max:'$AnalogValues." + sstrc[3] + "'}," +
            //           "'Avgdata1':{$avg:'$AnalogValues." + sstrc[0] + "'},'Avgdata2':{$avg:'$AnalogValues." + sstrc[1] + "'}," +
            //           "'Avgdata3':{$avg:'$AnalogValues." + sstrc[2] + "'}," +
            //       "'Avgdata4':{$avg:'$AnalogValues." + sstrc[3] + "'}}}";
            ////string project = "{$project:{'_id': 0,'Mindata':{$add:[{$ifNull:['$xdata1',0.0]},{$ifNull:['$xdata2',0.0]},{$ifNull:['$xdata3',0.0]},{$ifNull:['$xdata4',0.0]},'Maxdata':{$add:[{$ifNull:['$mdata1',0.0]},{$ifNull:['$mdata2',0.0]},{$ifNull:['$mdata3',0.0]},{$ifNull:['$mdata4',0.0]}," +
            ////        "'Avgdata':{$add:[{$ifNull:['$Avgdata1',0.0]},{$ifNull:['$Avgdata2',0.0]},{$ifNull:['$Avgdata3',0.0]},{$ifNull:['$Avgdata',0.0]}";
            ////正确
            ////string project = "{$project:{'_id': 0,'Maxdata':{$add:[{$ifNull:['$xdata1',0.0]},{$ifNull:['$xdata2',0.0]},{$ifNull:['$xdata3',0.0]},{$ifNull:['$xdata4',0.0]}]},'Mindata':{$add:[{$ifNull:['$mdata1',0.0]},{$ifNull:['$mdata2',0.0]},{$ifNull:['$mdata3',0.0]},{$ifNull:['$mdata4',0.0]}]}," +
            ////       "'Avgdata':{$add:[{$ifNull:['$Avgdata1',0.0]},{$ifNull:['$Avgdata2',0.0]},{$ifNull:['$Avgdata3',0.0]},{$ifNull:['$Avgdata4',0.0]}]}}}";
            //string project = "{$project:{'_id': 0,'Maxdata':{$max:[{$ifNull:['$xdata1',0.0]},{$ifNull:['$xdata2',0.0]},{$ifNull:['$xdata3',0.0]},{$ifNull:['$xdata4',0.0]}]},'Mindata':{$min:[{$ifNull:['$mdata1',0.0]},{$ifNull:['$mdata2',0.0]},{$ifNull:['$mdata3',0.0]},{$ifNull:['$mdata4',0.0]}]}," +
            //       "'Avgdata':{$avg:[{$ifNull:['$Avgdata1',0.0]},{$ifNull:['$Avgdata2',0.0]},{$ifNull:['$Avgdata3',0.0]},{$ifNull:['$Avgdata4',0.0]}]}}}";



            ////数据查询
            //DetailFlowData dataList = new DetailFlowData();
            //double Mindata = 0.0, Maxdata = 0.0, Avgdata = 0.0;
            //var adataList = _RTUInfoService.GetDetailFlowData(year, beginDate, endDate, group, project, rtuid.ToString());
            //dataList = adataList.FirstOrDefault();
            //if (dataList != null)
            //{
            //    Mindata = Math.Round(dataList.Mindata ?? 0.0, 2);
            //    Maxdata = Math.Round(dataList.Maxdata ?? 0.0, 2);
            //    Avgdata = Math.Round(dataList.Avgdata ?? 0.0, 2);
            //}
            double Mindata = 0.0, Maxdata = 0.0, Avgdata = 0.0;
            if (intValue.Count > 0)
            {
                Maxdata = Math.Round(intValue.Max(), 2);
                Mindata = Math.Round(intValue.Min(), 2);
                Avgdata = Math.Round(intValue.Average(), 2);
            }

            var rel = new
            {
                result = result,
                Mindata = Mindata,
                Maxdata = Maxdata,
                Avgdata = Avgdata,
                flag = flag
            };
            return Json(rel);
        }
        [TypeFilter(typeof(IgonreActionFilter))]
        private class flowtable
        {
            public int Num { get; set; }
            public string Time { get; set; }
            public decimal Data { get; set; }
            public string Percent { get; set; }
        }
        [TypeFilter(typeof(IgonreActionFilter))]
        public class historyChar
        {
            public string name { get; set; }
            public string data { get; set; }
        }
        //获取用户所属的设备id
        [TypeFilter(typeof(IgonreActionFilter))]
        public List<long> GetDeviceID(SysUser user)
        {
            List<long> deviceid = new List<long>();
            if (user.IsAdmin)
            {
                deviceid = _sws_DeviceInfo01Service.Query<SwsDeviceInfo01>(r => true)?.Select(r => r.DeviceId).ToList();
            }
            else
            {
                deviceid = _sws_DeviceInfo01Service.Query<SwsDeviceInfo01>(r => _sws_UserStationService.Query<SwsUserStation>(r => r.UserId == user.UserId).Select(r => r.StationId).Contains(r.StationId))?.Select(r => r.DeviceId).ToList();
            }
            return deviceid;
        }
        #region 获取 本周、本月、本季度、本年 的开始时间或结束时间
        /// <summary>
        /// 获取结束时间
        /// </summary>
        /// <param name="TimeType">Week、Month、Season、Year</param>
        /// <param name="now"></param>
        /// <returns></returns>
        [TypeFilter(typeof(IgonreActionFilter))]
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
        [TypeFilter(typeof(IgonreActionFilter))]
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
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult SearchTree(string stationName)
        {
            return Content(GetDeviceTree(stationName));
        }
        [TypeFilter(typeof(IgonreActionFilter))]
        public string GetDeviceTree(string stationName)
        {
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = userService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
            var nodes = _RTUInfoService.GetDeviceTree_Big(stationName, user.UserId, user.IsAdmin);
            var result = "[]";
            if (nodes.Count() > 0)
            {
                var treeNode = nodes.Select(r => new
                {
                    id = r.ID,
                    pId = r.Parent,
                    name = (r.type == 0 ? "<em class='iconfont icon-bengfang'></em>" : "") + r.Name,

                    nocheck = r.type == 0 ? true : false
                });
                result = Newtonsoft.Json.JsonConvert.SerializeObject(treeNode);
            }
            return result;

        }
        #endregion 
    }
}