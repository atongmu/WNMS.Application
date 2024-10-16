using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using WNMS.Application.Utility.Filters;
using WNMS.IService;
using WNMS.Model.DataModels;

namespace WNMS.Application.Areas.Sws.Controllers
{
    [Area("Sws")]
    public class Sws_ConsumptionAnalysisController : Controller
    {
        private ISws_StationService _StationService = null;
        private ISysUserService _userService = null;
        private IDDayQuartZ01Service _dDayQuartZ01Service = null;
        public Sws_ConsumptionAnalysisController(ISws_StationService sws_StationService, ISysUserService sysUserService,
            IDDayQuartZ01Service dDayQuartZ01Service)
        {
            _dDayQuartZ01Service = dDayQuartZ01Service;
            _StationService = sws_StationService;
            _userService = sysUserService;
        }
        public IActionResult Index()
        {
            //设置默认数据
            ViewBag.treenodes = new HtmlString(GetStationTree(""));
            /*
             * 	var stationId = 4, 					// 默认选中的泵房id
				stationName = '李庄嘉园小区', 	// 默认选中的泵房名称
				dateBegin = '2021-06-23', 		// 日期开始时间
				dateEnd = '2021-06-29', 		// 日期结束时间
				weekDateBegin = '2021-07-01', 	// 近7日开始时间
				weekDateEnd = '2021-07-07',		// 近7日结束时间
				monthDateBegin = '2021-07-01', 	// 月份开始时间
				monthDateEnd = '2021-07-30', 	// 月份结束时间
				totalDateBegin = dateBegin,		// 累计统计开始时间
				totalDateEnd = dateEnd; 		// 累计统计结束时间
             * 
             * */

            int userID = int.Parse(User.FindFirstValue("UserID"));

            var user = _userService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
            var data = _StationService.GetStation_consumption(user.IsAdmin, userID, "", 1).ToList().FirstOrDefault();
            ViewBag.stationId = data?.StationID;                 // 默认选中的泵房id
            ViewBag.stationName = data?.StationName;     // 默认选中的泵房名称 

            ViewBag.dateBegin = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 00:00:00")).ToString("HH:00:00");     // 日期开始时间
            ViewBag.dateEnd = DateTime.Now.ToString("HH:00:00");         // 日期结束时间

            ViewBag.weekDateBegin = DateTime.Now.AddDays(-8).Date.ToString("yyyy-MM-dd");   // 近7日开始时间
            ViewBag.weekDateEnd = DateTime.Now.AddDays(-1).Date.ToString("yyyy-MM-dd");     // 近7日结束时间
            ViewBag.monthDateBegin = DateTime.Now.AddMonths(-1).AddDays(-1).Date.ToString("yyyy-MM-dd");  // 月份开始时间
            ViewBag.monthDateEnd = DateTime.Now.AddDays(-1).Date.ToString("yyyy-MM-dd");    // 月份结束时间

            return View();
        }
        /// <summary>
        /// 近七天以及月能耗分析
        /// </summary>
        /// <param name="type">七天或是一个月</param>
        /// <param name="stationid">泵房ID</param>
        /// <returns></returns>
        public IActionResult ConsumptionAnalysis(string type, int stationid)
        {
            DateTime beginTime = new DateTime(), endTime = new DateTime();
            List<string> xix = new List<string>();//横坐标
            List<decimal> flow = new List<decimal>();//流量
            List<decimal> energyCon = new List<decimal>();//电量
            List<decimal> nh = new List<decimal>();//能耗 
            //查询泵房设备                              
            List<long> ids = new List<long>();
            decimal flowInit, energyConInit;
            var deviceList = _StationService.GetDeviceNameOfCon(stationid).ToList();
            if (deviceList.Count > 0)
            {
                if (type == "day")
                {
                    //beginTime = DateTime.Now.AddDays(-7).Date;
                    //endTime = DateTime.Now.AddDays(1).Date;
                    beginTime = DateTime.Now.AddDays(-8).Date;
                    endTime = DateTime.Now.Date;
                }
                else
                {
                    beginTime = DateTime.Now.AddMonths(-1).AddDays(-1).Date;
                    endTime = DateTime.Now.AddDays(-1).Date;
                }
                ids = deviceList.Select(r => (long)r.DeviceID).ToList();
                var datalist = _dDayQuartZ01Service.Query<DdayQuartZ01>(r => r.UpdateTime >= beginTime && r.UpdateTime < endTime && ids.Contains(r.DeviceId)).ToList();
                var tdata = datalist.GroupBy(r => r.UpdateTime).OrderBy(r => r.Key).ToList();
                foreach (var item in tdata)
                {
                    xix.Add(item.Key.ToString("yyyy-MM-dd"));
                    flowInit = datalist.Where(r => r.UpdateTime == item.Key).Sum(r => r.FlowCon);
                    flow.Add(flowInit);
                    energyConInit = datalist.Where(r => r.UpdateTime == item.Key).Sum(r => r.EnergyCon);
                    energyCon.Add(energyConInit);
                    if (flowInit != 0)
                    {
                        //EnergyCon/FlowCon
                        nh.Add(Math.Round((energyConInit / flowInit), 2));

                    }
                    else
                    {
                        nh.Add(0);
                    }
                }
            }

            return Json(new
            {
                xix,
                flow,
                energyCon,
                nh,
                beginTime = beginTime.ToString("yyyy-MM-dd"),
                endTime = endTime.ToString("yyyy-MM-dd")
            });
        }
        /// <summary>
        /// 单吨能耗表 日周月
        /// </summary>
        /// <param name="type">月周日</param>
        /// <param name="stationid">泵房ID</param>
        /// <returns></returns>
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult ConsumptionDate(string type, int stationid)
        {
            DateTime beginTime = new DateTime(), endTime = new DateTime();
            List<string> xix = new List<string>();//横坐标
            List<decimal> flow = new List<decimal>();//流量
            List<decimal> energyCon = new List<decimal>();//电量
            List<decimal> nh = new List<decimal>();//能耗 
            //查询泵房设备                              
            List<long> ids = new List<long>();
            decimal flowInit, energyConInit;
            var deviceList = _StationService.GetDeviceNameOfCon(stationid).ToList();
            if (deviceList.Count > 0)
            {
                if (type == "day")
                {
                    //本日
                    beginTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 00:00:00"));
                    endTime = DateTime.Now;
                    //beginTime = DateTime.Parse("2021-06-23 00:00:00");
                    //endTime = DateTime.Parse(DateTime.Now.ToString("2021-06-23 23:59:59"));
                    ids = deviceList.Select(r => (long)r.DeviceID).ToList();
                    var ddatalist = _dDayQuartZ01Service.Query<DhourQuartZ01>(r => r.UpdateTime >= beginTime && r.UpdateTime < endTime && ids.Contains(r.DeviceId)).ToList();
                    var dtdata = ddatalist.GroupBy(r => r.UpdateTime).OrderBy(r => r.Key).ToList();
                    foreach (var item in dtdata)
                    {
                        xix.Add(item.Key.ToString("HH:mm:ss"));
                        flowInit = ddatalist.Where(r => r.UpdateTime == item.Key).Sum(r => r.FlowCon);
                        flow.Add(flowInit);
                        energyConInit = ddatalist.Where(r => r.UpdateTime == item.Key).Sum(r => r.EnergyCon);
                        energyCon.Add(energyConInit);
                        if (flowInit != 0)
                        {
                            //EnergyCon/FlowCon
                            nh.Add(Math.Round((energyConInit / flowInit), 2));

                        }
                        else
                        {
                            nh.Add(0);
                        }
                    }
                    return Json(new
                    {
                        xix,
                        flow,
                        energyCon,
                        nh,
                        beginTime = beginTime.ToString("HH:00:00"),
                        endTime = endTime.ToString("HH:00:00")
                    });
                }
                else if (type == "week")//本周
                {
                    //本周
                    var dayofWeek1 = (int)DateTime.Now.DayOfWeek == 0 ? 7 : (int)DateTime.Now.DayOfWeek;
                    var BeginTime = string.Format("{0:yyyy-MM-dd}", DateTime.Now.AddDays(-(dayofWeek1 - 1)));
                    beginTime = DateTime.Parse(BeginTime);
                    endTime = DateTime.Now;
                }
                else
                {
                    //本月
                    beginTime = DateTime.Parse(string.Format("{0:yyyy-MM}", DateTime.Now) + "-01");
                    endTime = DateTime.Now.AddMonths(1).AddSeconds(-1);
                }
                ids = deviceList.Select(r => (long)r.DeviceID).ToList();
                var datalist = _dDayQuartZ01Service.Query<DdayQuartZ01>(r => r.UpdateTime >= beginTime && r.UpdateTime < endTime && ids.Contains(r.DeviceId)).ToList();
                var tdata = datalist.GroupBy(r => r.UpdateTime).OrderBy(r => r.Key).ToList();
                foreach (var item in tdata)
                {
                    xix.Add(item.Key.ToString("yyyy-MM-dd"));
                    flowInit = datalist.Where(r => r.UpdateTime == item.Key).Sum(r => r.FlowCon);
                    flow.Add(flowInit);
                    energyConInit = datalist.Where(r => r.UpdateTime == item.Key).Sum(r => r.EnergyCon);
                    energyCon.Add(energyConInit);
                    if (flowInit != 0)
                    {
                        //EnergyCon/FlowCon
                        nh.Add(Math.Round((energyConInit / flowInit), 2));

                    }
                    else
                    {
                        nh.Add(0);
                    }
                }
            }

            return Json(new
            {
                xix,
                flow,
                energyCon,
                nh,
                beginTime = beginTime.ToString("yyyy-MM-dd"),
                endTime = endTime.ToString("yyyy-MM-dd")
            });
        }

        /// <summary>
        /// 能耗表单
        /// </summary>
        /// <param name="beginTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="stationid">泵房ID</param>
        /// <returns></returns>
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult ConsumptionByDate(string beginTime, string endTime, int stationid)
        {
            //查询泵房设备                              
            List<long> ids = new List<long>();
            decimal flowInit, energyConInit;
            var deviceList = _StationService.GetDeviceNameOfCon(stationid).ToList();
            List<EconInfo> econInfos = new List<EconInfo>();

            if (deviceList.Count > 0)
            {
                //DateTime bTime = DateTime.Parse(beginTime);
                //DateTime eTime = DateTime.Parse(endTime);
                DateTime bTime = new DateTime();
                DateTime eTime = new DateTime();
                if (beginTime == null)
                {
                    bTime = DateTime.Parse(DateTime.Now.AddMonths(-1).AddDays(-1).Date.ToString("yyyy-MM-dd"));  // 月份开始时间
                    eTime = DateTime.Parse(DateTime.Now.AddDays(-1).Date.ToString("yyyy-MM-dd"));    // 月份结束时间
                }
                else
                {
                    bTime = DateTime.Parse(beginTime);
                    eTime = DateTime.Parse(endTime);
                }

                ids = deviceList.Select(r => (long)r.DeviceID).ToList();
                var datalist = _dDayQuartZ01Service.Query<DdayQuartZ01>(r => r.UpdateTime >= bTime && r.UpdateTime <= eTime && ids.Contains(r.DeviceId)).ToList();
                var tdata = datalist.GroupBy(r => r.UpdateTime).OrderBy(r => r.Key).ToList();
                foreach (var item in tdata)
                {
                    EconInfo econInfo = new EconInfo();
                    econInfo.dateTime = item.Key.ToString("yyyy-MM-dd");
                    flowInit = datalist.Where(r => r.UpdateTime == item.Key).Sum(r => r.FlowCon);
                    energyConInit = datalist.Where(r => r.UpdateTime == item.Key).Sum(r => r.EnergyCon);
                    econInfo.dateTime = item.Key.ToString("yyyy-MM-dd");
                    econInfo.flow = flowInit;
                    econInfo.energyCon = energyConInit;
                    if (flowInit != 0)
                    {
                        //EnergyCon/FlowCon 
                        econInfo.nh = Math.Round((energyConInit / flowInit), 2);
                    }
                    else
                    {
                        econInfo.nh = 0;
                    }
                    econInfos.Add(econInfo);
                }
            }
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(econInfos));
        }

        class EconInfo
        {
            public string dateTime;
            public decimal flow;
            public decimal energyCon;
            public decimal nh;
        }
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
            var data = _StationService.GetStation_consumption(user.IsAdmin, userID, stationname, 1);
            if (data.Count() > 0)
            {
                var list = data.Select(r => new
                {
                    id = r.StationID,
                    pId = 0,
                    name = "<em class='iconfont icon-bengfang'></em>" + r.StationName,
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

        //近七天以及月能耗分析
        //public IActionResult ConsumptionAnalysis(string type, int stationid)
        //{
        //    DateTime beginTime = new DateTime(), endTime = new DateTime();
        //    var deciceList = _StationService.GetDeviceNameOfCon(stationid);
        //    string tablename = "";
        //    string timeformate = "";
        //    List<string> xix = new List<string>();//横坐标
        //    List<string> flow = new List<string>();//流量
        //    List<string> energyCon = new List<string>();//流量
        //    List<string> nh = new List<string>();//能耗
        //    if (deciceList.Count() > 0)
        //    {
        //        var deviceidList = deciceList.Select(r => r.DeviceID);
        //        string deviceids = string.Join(",", deviceidList);
        //        if (type == "day")
        //        {
        //            beginTime = DateTime.Now.AddDays(-7);
        //            endTime = DateTime.Now;
        //            tablename = "DDayQuartZ01";
        //            timeformate = "MM-dd";
        //        }
        //        else if (type == "month")
        //        {
        //            beginTime = DateTime.Now.AddMonths(-1);
        //            endTime = DateTime.Now;
        //            tablename = "DDayQuartZ01";
        //            timeformate = "MM-dd";
        //        }
        //        var datalist = _StationService.GetConDataByDeviceID("2021-06-22", "2021-06-29", deviceids, tablename);

        //        foreach (var item in datalist)
        //        {
        //            xix.Add(item.UpdateTime.ToString(timeformate));
        //            flow.Add(item.FlowCon);
        //            energyCon.Add(item.EnergyCon);
        //            nh.Add(item.consump);
        //        }
        //        return Json(new
        //        {
        //            xix,
        //            flow,
        //            energyCon,
        //            nh,
        //            beginTime,
        //            endTime
        //        });
        //    }
        //    return Content("");
        //}
    }
}