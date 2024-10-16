using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using WNMS.IService;
using WNMS.Service;
using WNMS.Model.DataModels;
using System.Globalization;
using MongoDBHelper;
using WNMS.Model.CustomizedClass;
using System.Security.Claims;
using WNMS.Application.Utility.Filters;
using Microsoft.AspNetCore.Hosting;
using WNMS.Application.Utility.JsonHelper;

namespace WNMS.Application.Areas.Sws.Controllers
{
    [Area("Sws")]
    public class BigScreen_ConsumpController : Controller
    {
        private ISws_RTUInfoService _RTUInfoService = null;
        private IDHourQuartZ01Service _dHourQuartZ01Service = null;
        private IDDayQuartZ01Service _dDayQuartZ01Service = null;
        private IDMonthQuartZ01Service _DMonthQuartZ01Service = null;
        private ISws_DeviceInfo01Service _DeviceInfo01Service = null;
        private ISysUserService _userService = null;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private ISws_ConsumpSettingService _ConsumpSettingService = null;
        public BigScreen_ConsumpController(ISws_RTUInfoService sws_RTUInfoService,
            IDHourQuartZ01Service dHourQuartZ01Service,
            IDDayQuartZ01Service dDayQuartZ01Service,
            IDMonthQuartZ01Service dMonthQuartZ01Service,
            ISws_DeviceInfo01Service sws_DeviceInfo01Service,
            ISysUserService sysUserService,
            ISws_ConsumpSettingService sws_ConsumpSettingService,
            IWebHostEnvironment webHostEnvironment)
        {
            _RTUInfoService = sws_RTUInfoService;
            _dHourQuartZ01Service=dHourQuartZ01Service;
            _dDayQuartZ01Service = dDayQuartZ01Service;
            _DMonthQuartZ01Service = dMonthQuartZ01Service;
            _DeviceInfo01Service = sws_DeviceInfo01Service;
            _userService = sysUserService;
            _webHostEnvironment = webHostEnvironment;
            _ConsumpSettingService = sws_ConsumpSettingService;
        }
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult Index()
        {
            ViewBag.TreeNodes = new HtmlString(GetDeviceTree(""));
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = _userService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
            ViewBag.deviceid = _ConsumpSettingService.GetDevice_init(user.UserId, user.IsAdmin).FirstOrDefault().DeviceID;

            var path = _webHostEnvironment.ContentRootPath;
            JsonFileHelper jsonFileHelper = new JsonFileHelper(path + "/Utility/ProjectJson/project.json");
            SystemInfo st = jsonFileHelper.Read<SystemInfo>("SystemInfo");

            ViewBag.sysName = st.SystemName;
            ViewBag.logo = st.Logo;

            return View();
        }
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult SearchTree(string stationName)
        {
            return Content(GetDeviceTree(stationName));
        }
        public string GetDeviceTree(string stationName)
        {
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = _userService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
            var nodes = _RTUInfoService.GetDeviceTree_Big(stationName,user.UserId,user.IsAdmin);
            var result = "[]";
            if (nodes.Count() > 0)
            {
                var treeNode = nodes.Select(r => new
                {
                    id = r.ID,
                    pId = r.Parent,
                    name =(r.type==0? "<em class='iconfont icon-bengfang'></em>":"")+ r.Name,
                   
                    nocheck =r.type==0? true : false
                });
                result= Newtonsoft.Json.JsonConvert.SerializeObject(treeNode);
            }
            return result;

        }
        #region 今日，昨日，本周，本月
        //今日，本周能耗
        [TypeFilter(typeof(IgonreActionFilter))]
        #region 保存
        //public IActionResult GetThisCompData(long deviceid)
        //{
        //    //今日
        //    var beginDate_today = DateTime.Now.Date;
        //    var endDate_today = DateTime.Now;
        //    decimal todayData = 0, thisWeekData = 0;//返回值
        //    #region 今日能耗
        //    decimal today_enery = 0;//累加值
        //    List<string> today_axis = new List<string>();
        //    List<decimal> today_data = new List<decimal>();
        //    var todaylist = _dHourQuartZ01Service.Query<DhourQuartZ01>(r => r.DeviceId == deviceid && r.UpdateTime >= beginDate_today && r.UpdateTime <= endDate_today).OrderBy(r=>r.UpdateTime).ToList();
        //    if (todaylist.Count > 0)
        //    {
        //        foreach (var item in todaylist)
        //        {
        //            today_data.Add(Math.Round(item.EnergyCon,2));
        //            today_axis.Add(item.UpdateTime.ToString("HH:mm"));

        //            today_enery += item.EnergyCon;
        //        }
        //        todayData =Math.Round(today_enery,2);
        //    }
        //    #endregion
        //    #region 本周能耗
        //    List<string> week_axis = new List<string>();
        //    List<decimal> week_data = new List<decimal>();
        //    decimal week_enery = 0;//累加值
        //    var zhouji = DateTime.Now.DayOfWeek;
        //    var num = (int)System.Enum.Parse(typeof(WeekDay), zhouji.ToString());
        //    if (num == 1)
        //    {
        //        num = 8;
        //    }
        //    var enddate_week = DateTime.Now.Date;
        //    var begindate_week = DateTime.Now.AddDays(2 - num).Date;
        //    var weekDataList = _dDayQuartZ01Service.Query<DdayQuartZ01>(r => r.DeviceId == deviceid && r.UpdateTime >= begindate_week && r.UpdateTime <= enddate_week).OrderBy(r => r.UpdateTime).ToList();
        //    if (weekDataList.Count > 0)
        //    {
        //        foreach (var item in weekDataList)
        //        {

        //             week_data.Add(Math.Round(item.FlowCon,2));
        //            week_axis.Add(item.UpdateTime.ToString("MM-dd"));

        //            week_enery += item.EnergyCon;
        //        }
        //    }
        //    if (todaylist.Count > 0)
        //    {
        //        week_data.Add(todayData);
        //        week_axis.Add(DateTime.Now.ToString("MM-dd"));

        //        week_enery += today_enery;
        //        thisWeekData =Math.Round(week_enery,2);
        //    }

        //    #endregion
        //    return Json(new
        //    {
        //        todayData= todayData,
        //        today_axis= today_axis,
        //        today_data= today_data,
        //        thisWeekData= thisWeekData,
        //        week_axis= week_axis,
        //        week_data = week_data
        //    });
        //}
        #endregion
        public IActionResult GetThisCompData(long deviceid)
        {
            //今日
            var beginDate_today = DateTime.Now.Date;
            var endDate_today = DateTime.Now;
            decimal todayData = 0, thisWeekData = 0;//返回值
            #region 今日能耗
            decimal today_enery = 0;//累加值
           
            var todaylist = _dHourQuartZ01Service.Query<DhourQuartZ01>(r => r.DeviceId == deviceid && r.UpdateTime >= beginDate_today && r.UpdateTime <= endDate_today).OrderBy(r => r.UpdateTime).ToList();
            if (todaylist.Count > 0)
            {
               
                today_enery = todaylist.Select(r => r.EnergyCon).Sum();
                todayData = Math.Round(today_enery, 2);
            }
            #endregion
            #region 本周能耗
           
            decimal week_enery = 0;//累加值
            var zhouji = DateTime.Now.DayOfWeek;
            var num = (int)System.Enum.Parse(typeof(WeekDay), zhouji.ToString());
            if (num == 1)
            {
                num = 8;
            }
            var enddate_week = DateTime.Now.Date;
            var begindate_week = DateTime.Now.AddDays(2 - num).Date;
            var weekDataList = _dDayQuartZ01Service.Query<DdayQuartZ01>(r => r.DeviceId == deviceid && r.UpdateTime >= begindate_week && r.UpdateTime <= enddate_week).OrderBy(r => r.UpdateTime).ToList();
            if (weekDataList.Count > 0)
            {

                week_enery = weekDataList.Select(r => r.EnergyCon).Sum();
            }
            if (todaylist.Count > 0)
            {
                week_enery += today_enery;
               
            }
            thisWeekData = Math.Round(week_enery, 2);
            #endregion
            //昨日能耗
            var yestday_date = DateTime.Now.Date.AddDays(-1);
            var yest_data = _dDayQuartZ01Service.Query<DdayQuartZ01>(r => r.DeviceId == deviceid&&r.UpdateTime == yestday_date).FirstOrDefault();
            var yestData = yest_data==null?0:Math.Round(yest_data.EnergyCon,2);
            
            return Json(new
            {
                todayData = todayData,
                thisWeekData = thisWeekData,
                yestData= yestData
            });
        }
        //昨日能耗
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult GetYestdayData(long deviceid)
        {
            var beginDate_yestday = DateTime.Now.AddDays(-1).Date;
            var endDate_yestday = DateTime.Now.Date;
            decimal yestdayData = 0;//返回值
            List<string> yestday_axis = new List<string>();
            List<decimal> yestday_data = new List<decimal>();
            decimal  yestday_enery = 0;//累加值
            var yestlist = _dHourQuartZ01Service.Query<DhourQuartZ01>(r => r.DeviceId == deviceid && r.UpdateTime >= beginDate_yestday && r.UpdateTime <endDate_yestday).OrderBy(r => r.UpdateTime).ToList();
            if (yestlist.Count > 0)
            {
                foreach (var item in yestlist)
                {
                    
                    yestday_data.Add(Math.Round(item.EnergyCon,2));
                    yestday_axis.Add(item.UpdateTime.ToString("HH:mm"));
                   
                    yestday_enery += item.EnergyCon;
                }
                yestdayData = Math.Round(yestday_enery, 2);
            }
            return Json(new
            {
                yestdayData= yestdayData,
                yestday_axis= yestday_axis,
                yestday_data= yestday_data
            });
        }
        //本月能耗
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult GetThisMonthData(long deviceid)
        {
            var beginDate_month = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM") + "-01");
            var endDate_month = DateTime.Now.Date;
            decimal monthData = 0;//返回值
            List<string> month_axis = new List<string>();
            List<decimal> month_data = new List<decimal>();
            decimal month_enery = 0;//累加值
            var yestlist = _dDayQuartZ01Service.Query<DdayQuartZ01>(r => r.DeviceId == deviceid && r.UpdateTime >= beginDate_month && r.UpdateTime < endDate_month).OrderBy(r => r.UpdateTime).ToList();
            if (yestlist.Count > 0)
            {
                foreach (var item in yestlist)
                {

                    month_data.Add(Math.Round(item.EnergyCon, 2));

                    month_axis.Add(item.UpdateTime.ToString("MM-dd"));

                    month_enery += item.EnergyCon;
                }
                monthData = Math.Round(month_enery, 2);
            }
            return Json(new
            {
                monthData = monthData,
                month_axis = month_axis,
                month_data = month_data
            });
        }
       

        #endregion
        #region 本年度能耗统计
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult GetYearData(long deviceid)
        {
            var axis = new List<string>() { };//x轴坐标
            var datalist = new List<decimal>() { };//数据集合
            var beginTime = Convert.ToDateTime(DateTime.Now.Year + "-01-01");
            var endTime = DateTime.Now.Date;
            List<DateTime> dateList = new List<DateTime>() { };

            var yeardataList = _DMonthQuartZ01Service.Query<DmonthQuartZ01>(r=>r.DeviceId==deviceid&& r.UpdateTime>= beginTime&&r.UpdateTime< endTime).OrderBy(r=>r.UpdateTime).ToList();
            //查询当前月能耗数据
            var bengin1 = DateTime.Now.ToString("yyyy-MM") + "-01";
            var end1= DateTime.Now.Date.ToString();
            var thismonth = _RTUInfoService.GetTodayCompData(deviceid, bengin1, end1);
            yeardataList = yeardataList.Concat(thismonth).ToList();
           
            if (yeardataList.Count > 0)
            {
                while (beginTime <= endTime)
                {
                    axis.Add(beginTime.Month + "月");
                    dateList.Add(beginTime);
                    beginTime = beginTime.AddMonths(1);
                }
                foreach (var it in dateList)
                {
                    var datat = yeardataList.Where(r => r.UpdateTime == it).FirstOrDefault();
                    if (datat != null)
                    {
                        datalist.Add(Math.Round(datat.EnergyCon, 2));
                    }
                    else
                    {
                        datalist.Add(0);
                    }
                }
            }
            decimal thisMonthData = 0;
            if (thismonth.Count() > 0)
            {
                thisMonthData =Math.Round(thismonth.FirstOrDefault().EnergyCon,2);
            }
            return Json(new
            {
                axis= axis,
                datalist= datalist,
                thisMonthData= thisMonthData
            });
          
        }
        #endregion
        #region 近7天能耗统计
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult GetEnergy7Day(long deviceid)
        {
            var beginDate_month = DateTime.Now.AddDays(-7).Date;
            var endDate_month = DateTime.Now.Date;
           
            List<string> axis = new List<string>();
            List<decimal> Energydata = new List<decimal>() { };
            List<decimal> compdata = new List<decimal>() { };
            
            var yestlist = _dDayQuartZ01Service.Query<DdayQuartZ01>(r => r.DeviceId == deviceid && r.UpdateTime >= beginDate_month && r.UpdateTime < endDate_month).OrderBy(r => r.UpdateTime).ToList();
            if (yestlist.Count > 0)
            {
                foreach (var item in yestlist)
                {
                    if (item.FlowCon == 0)
                    {
                        compdata.Add(0);

                    }
                    else
                    {
                        compdata.Add(Math.Round(item.EnergyCon / item.FlowCon, 2));
                    }
                    axis.Add(item.UpdateTime.ToString("MM-dd"));
                    Energydata.Add(Math.Round(item.EnergyCon,2));
                    
                }
               
            }
            return Json(new
            {
                axis = axis,
                Energydata = Energydata,
                compdata = compdata
            });
        }
        #endregion
        #region 总累计电量，总累计流量
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult GetDataAll(long deviceid)
        {
            double totalflow = 0, totalenery = 0;
            var model = _DeviceInfo01Service.Find<SwsDeviceInfo01>(deviceid);
            var rtuid = model.Rtuid == null ? 0 : model.Rtuid;
            string macJson = "{\"RTUID\":{'$in':[" + rtuid + "]}}";
            var collections = new MongoDBHelper<RtuJKInfo>(DateTime.Now.Year.ToString());
            var jklist = collections.Find("Sws_DeviceJKInfo", macJson, "UpdateTime").FirstOrDefault();
            if (jklist != null)
            {
                if (model.Partition != 6)
                {
                    var flowidlist = new List<int>() { 2031+(model.Partition-1)*500, 2033 + (model.Partition - 1) * 500 ,
                2035+(model.Partition-1)*500,2037+(model.Partition-1)*500};
                    var eneryidList = new List<int>() { 2023+ (model.Partition - 1) * 500, 2024+ (model.Partition - 1) * 500 ,
                2025+(model.Partition - 1) * 500,2026+(model.Partition - 1) * 500};

                    foreach (var item in flowidlist)
                    {
                        totalflow += jklist.AnalogValues.Keys.Contains(item.ToString()) ? (double)jklist.AnalogValues[item.ToString()] : 0;
                    }
                    foreach (var item in eneryidList)
                    {
                        totalenery += jklist.AnalogValues.Keys.Contains(item.ToString()) ? (double)jklist.AnalogValues[item.ToString()] : 0;
                    }
                    if (totalenery == 0)
                    {
                        var eallid = 2085 + (model.Partition - 1) * 500;
                        var data_enery = jklist.AnalogValues.Keys.Contains(eallid.ToString()) ? (double)jklist.AnalogValues[eallid.ToString()] : 0;
                        totalenery = data_enery;
                    }
                }
                else
                {
                    totalflow = jklist.AnalogValues.Keys.Contains("4511") ? (double)jklist.AnalogValues["4511"] : 0;
                    totalenery= jklist.AnalogValues.Keys.Contains("4512") ? (double)jklist.AnalogValues["4512"] : 0;
                }
            }
            return Json(new
            {
                totalflow = Math.Round(totalflow,2),
                totalpower = Math.Round(totalenery,2)
            });
        }
        #endregion
        #region 进一个月各泵能耗
        [TypeFilter(typeof(IgonreActionFilter))]
        public ActionResult GetPowerDataByMonth(long deviceid)
        {
            string beginDate = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
            string endDate = DateTime.Now.ToString();
            string year = Convert.ToDateTime(beginDate).Year.ToString();
           
            double data1 = 0.0, data2 = 0.0, data3 = 0.0, data4 = 0.0;
            var model = _DeviceInfo01Service.Find<SwsDeviceInfo01>(deviceid);
            var rtuid = model.Rtuid == null ? 0 : model.Rtuid;
            //数据查询参数处理
            string[] sstr = { 2023+ (model.Partition - 1) * 500+"", 2024+ (model.Partition - 1) * 500+"", 2025+ (model.Partition - 1) * 500+"", 2026+ (model.Partition - 1) * 500+"", 2085+ (model.Partition - 1) * 500+"" };
           
            string group = "{$group:{'_id':{'$dateToString':{'format':'%Y-%m-%d','date':'$UpdateTime'}}," +
                      "'Mindata':{$min:'$AnalogValues." + sstr[0] + "'},'Mindata2':{$min:'$AnalogValues." + sstr[1] + "'}," +
                   "'Mindata3':{$min:'$AnalogValues." + sstr[2] + "'},'Mindata4':{$min:'$AnalogValues." + sstr[3] + "'}," +
                   "'Mindataall':{$min:'$AnalogValues." + sstr[4] + "'}," +
                   "}}";
            string project = "{$project:{'_id': 0,'Data1':{$ifNull:['$Mindata', 0.0]},'Data2':{$ifNull:['$Mindata2', 0.0]}," +
                    "'Data3':{$ifNull:['$Mindata3', 0.0]},'Data4':{$ifNull:['$Mindata4', 0.0]},'Data':'$Mindataall'," +
                    "'Time':'$_id'}}";

            //数据查询
            List<DetailFlowData> dataList = new List<DetailFlowData>();
            dataList = _RTUInfoService.GetDetailFlowData(year, beginDate, endDate, group, project, rtuid.ToString()).ToList();

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
            List<powertable> reult = new List<powertable>();
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
            powertable pp = new powertable();
            pp.datalist = new List<string>();
            pp.datalist.Add("平均值");
            pp.datalist.Add(data.Average(r => r.Data1) == null ? "--" : Math.Round((double)data.Average(r => r.Data1), 2).ToString());
            pp.datalist.Add(data.Average(r => r.Data2) == null ? "--" : Math.Round((double)data.Average(r => r.Data2), 2).ToString());
            pp.datalist.Add(data.Average(r => r.Data3) == null ? "--" : Math.Round((double)data.Average(r => r.Data3), 2).ToString());
            pp.datalist.Add(data.Average(r => r.Data4) == null ? "--" : Math.Round((double)data.Average(r => r.Data4), 2).ToString());
            pp.datalist.Add(data.Average(r => r.Data) == null ? "--" : Math.Round((double)data.Average(r => r.Data), 2).ToString());
            reult.Add(pp);
            powertable pp2 = new powertable();
            pp2.datalist = new List<string>();
            pp2.datalist.Add("最大时间");
            pp2.datalist.Add(data.Where(r => r.Data1 == data.Max(a => a.Data1)).FirstOrDefault() == null ? "--" : data.Where(r => r.Data1 == data.Max(a => a.Data1)).FirstOrDefault().Time.ToString("yyyy-MM-dd"));
            pp2.datalist.Add(data.Where(r => r.Data2 == data.Max(a => a.Data2)).FirstOrDefault() == null ? "--" : data.Where(r => r.Data2 == data.Max(a => a.Data2)).FirstOrDefault().Time.ToString("yyyy-MM-dd"));
            pp2.datalist.Add(data.Where(r => r.Data3 == data.Max(a => a.Data3)).FirstOrDefault() == null ? "--" : data.Where(r => r.Data3 == data.Max(a => a.Data3)).FirstOrDefault().Time.ToString("yyyy-MM-dd"));
            pp2.datalist.Add(data.Where(r => r.Data4 == data.Max(a => a.Data4)).FirstOrDefault() == null ? "--" : data.Where(r => r.Data4 == data.Max(a => a.Data4)).FirstOrDefault().Time.ToString("yyyy-MM-dd"));
            pp2.datalist.Add(data.Where(r => r.Data == data.Max(a => a.Data)).FirstOrDefault() == null ? "--" : data.Where(r => r.Data == data.Max(a => a.Data)).FirstOrDefault().Time.ToString("yyyy-MM-dd"));
            reult.Add(pp2);
            powertable pp3 = new powertable();
            pp3.datalist = new List<string>();
            pp3.datalist.Add("最大值");
            
            pp3.datalist.Add(data.Max(r => r.Data1) == null ? "--" :Math.Round((double)data.Max(r => r.Data1),2).ToString());
            pp3.datalist.Add(data.Max(r => r.Data2) == null ? "--" :Math.Round((double)data.Max(r => r.Data2),2).ToString());
            pp3.datalist.Add(data.Max(r => r.Data3) == null ? "--" :Math.Round((double)data.Max(r => r.Data3),2).ToString());
            pp3.datalist.Add(data.Max(r => r.Data4) == null ? "--" :Math.Round((double)data.Max(r => r.Data4),2).ToString());
            pp3.datalist.Add(data.Max(r => r.Data) == null ? "--" :Math.Round((double)data.Max(r => r.Data),2).ToString());
            reult.Add(pp3);
            powertable pp4 = new powertable();
            pp4.datalist = new List<string>();
            pp4.datalist.Add("最小时间");
           
            pp4.datalist.Add(data.Where(r => r.Data1 == data.Min(a => a.Data1)).FirstOrDefault() == null ? "--" : data.Where(r => r.Data1 == data.Min(a => a.Data1)).FirstOrDefault().Time.ToString("yyyy-MM-dd"));
            pp4.datalist.Add(data.Where(r => r.Data2 == data.Min(a => a.Data2)).FirstOrDefault() == null ? "--" : data.Where(r => r.Data2 == data.Min(a => a.Data2)).FirstOrDefault().Time.ToString("yyyy-MM-dd"));
            pp4.datalist.Add(data.Where(r => r.Data3 == data.Min(a => a.Data3)).FirstOrDefault() == null ? "--" : data.Where(r => r.Data3 == data.Min(a => a.Data3)).FirstOrDefault().Time.ToString("yyyy-MM-dd"));
            pp4.datalist.Add(data.Where(r => r.Data4 == data.Min(a => a.Data4)).FirstOrDefault() == null ? "--" : data.Where(r => r.Data4 == data.Min(a => a.Data4)).FirstOrDefault().Time.ToString("yyyy-MM-dd"));
            pp4.datalist.Add(data.Where(r => r.Data == data.Min(a => a.Data)).FirstOrDefault() == null ? "--" : data.Where(r => r.Data == data.Min(a => a.Data)).FirstOrDefault().Time.ToString("yyyy-MM-dd"));
            reult.Add(pp4);
            powertable pp5 = new powertable();
            pp5.datalist = new List<string>();
            pp5.datalist.Add("最小值");
           
            pp5.datalist.Add(data.Min(r => r.Data1) == null ? "--" :Math.Round((double)data.Min(r => r.Data1),2).ToString());
            pp5.datalist.Add(data.Min(r => r.Data2) == null ? "--" : Math.Round((double)data.Min(r => r.Data2),2).ToString());
            pp5.datalist.Add(data.Min(r => r.Data3) == null ? "--" : Math.Round((double)data.Min(r => r.Data3),2).ToString());
            pp5.datalist.Add(data.Min(r => r.Data4) == null ? "--" : Math.Round((double)data.Min(r => r.Data4),2).ToString());
            pp5.datalist.Add(data.Min(r => r.Data) == null ? "--" : Math.Round((double)data.Min(r => r.Data),2).ToString());
            reult.Add(pp5);
            data1 = Math.Round((double)data.Sum(r => r.Data1),2);
            data2 = Math.Round((double)data.Sum(r => r.Data2),2);
            data3 = Math.Round((double)data.Sum(r => r.Data3),2);
            data4 = Math.Round((double)data.Sum(r => r.Data4),2);

          

            return Json(new
            {
                tabledata = reult,
                data1 = data1,
                data2 = data2,
                data3 = data3,
                data4 = data4
               
            });
        }
        #endregion
        #region 本月能耗数据
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult GetYestMonthEnery(long deviceid)
        {
            var time= Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM") + "-01"); 
            var beginDate_month = time.AddMonths(-1);
            var endDate_month = time;
            
            List<string> month_axis = new List<string>();
            List<decimal> month_data = new List<decimal>();
           
            var yestlist = _dDayQuartZ01Service.Query<DdayQuartZ01>(r => r.DeviceId == deviceid && r.UpdateTime >= beginDate_month && r.UpdateTime < endDate_month).OrderBy(r => r.UpdateTime).ToList();
            if (yestlist.Count > 0)
            {
                foreach (var item in yestlist)
                {

                    month_data.Add(Math.Round(item.EnergyCon, 2));

                    month_axis.Add(item.UpdateTime.ToString("dd"));
                }
               
            }
            return Json(new
            {
                
                month_axis = month_axis,
                month_data = month_data
            });
        }
        #endregion
        #region 近一个月单吨能耗数据
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult GetThisMonthComp(long deviceid)
        {
            var beginDate_month = DateTime.Now.Date.AddMonths(-1);
            var endDate_month = DateTime.Now.Date;

            List<string> month_axis = new List<string>();
            List<decimal> month_data = new List<decimal>();
            
            var yestlist = _dDayQuartZ01Service.Query<DdayQuartZ01>(r => r.DeviceId == deviceid && r.UpdateTime >= beginDate_month && r.UpdateTime < endDate_month).OrderBy(r => r.UpdateTime).ToList();
            if (yestlist.Count > 0)
            {
                foreach (var item in yestlist)
                {
                    if (item.FlowCon == 0)
                    {
                        month_data.Add(0);
                    }
                    else
                    {
                        month_data.Add(Math.Round(item.EnergyCon/item.FlowCon, 2));
                    }
                    
                    month_axis.Add(item.UpdateTime.ToString("MM-dd"));
   
                }

            }
            return Json(new
            {
                month_axis = month_axis,
                month_data = month_data
            });
        }
        #endregion
        public class powertable
        {
            public List<string> datalist { get; set; }
        }
        enum WeekDay
        {
            Monday = 2,
            Tuesday = 3,
            Wednesday = 4,
            Thursday = 5,
            Friday = 6,
            Saturday = 7,
            Sunday = 1,

        }
        private int Getweek()
        {
            GregorianCalendar gc = new GregorianCalendar();
            int weekOfYear = gc.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
            
            return weekOfYear;

        }
    }
}