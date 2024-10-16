using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WNMS.Application.Utility.Filters;
using WNMS.IService;
using WNMS.Model.CustomizedClass;
using WNMS.Model.DataModels;

namespace WNMS.Application.Areas.Sws.Controllers
{
    [Area("Sws")]
    public class WaterAnalysisController : Controller
    {
        #region 属性 构造函数
        private ISws_StationService stationService = null;
        private ISws_DeviceInfo01Service device01Service = null;
        private ISws_DeviceInfo02Service device02Service = null;
        private ISws_RTUInfoService rtuService = null;
        private ISws_DataInfoService dataInfoService = null;

        public WaterAnalysisController(ISws_DeviceInfo01Service sws_DeviceInfo01Service, ISws_DeviceInfo02Service sws_DeviceInfo02Service,
            ISws_RTUInfoService sws_RTUInfoService, ISws_StationService sws_StationService, ISws_DataInfoService sws_DataInfoservice)
        {
            device01Service = sws_DeviceInfo01Service;
            device02Service = sws_DeviceInfo02Service;
            rtuService = sws_RTUInfoService;
            stationService = sws_StationService;
            dataInfoService = sws_DataInfoservice;
        }
        #endregion
        public IActionResult Index(string id)//水质分析
        {
            id = "1629395354586";
            ViewBag.did = id;
          
            return View();
        }


        #region 单个模拟量数据查询
        [TypeFilter(typeof(IgonreActionFilter))]
        public ActionResult GetOverLayTimeHistory(string Cnname, string beginDate, string endDate, string deviceID)
        {
            List<historyChar> result = new List<historyChar>();
           
            if (string.IsNullOrEmpty(deviceID))
            {
                return Json(new
                {
                    result = result,
                    datainfo = ""

                });
            }
            var deviceId = Convert.ToInt64(deviceID);
        
            var datainfo = dataInfoService.Query<SwsDataInfo>(r => r.Cnname == Cnname && r.DeviceType == 1).FirstOrDefault();
            var deviceInfo = device01Service.Query<SwsDeviceInfo01>(r => r.DeviceId == deviceId).FirstOrDefault();
            DateTime Bdate = Convert.ToDateTime(beginDate);
            DateTime Edate = Convert.ToDateTime(endDate);
            TimeSpan cha = Edate - Bdate;

            if (cha.TotalHours <= 0)
            {
                return Json(new
                {
                    result = result,
                    datainfo = ""

                });
            }
           else if (cha.TotalHours<=24)
            {
             result = QueryDayOver(datainfo.DataId, beginDate, endDate, deviceInfo.Rtuid.ToString(), datainfo.IsCumulation);
                return Json(new
                {
                    result = result,
                    datainfo = datainfo.Unit

                });
            }
            else
            {
            
                        result = QueryMonthOver(datainfo.DataId, beginDate,endDate, deviceInfo.Rtuid.ToString(), datainfo.IsCumulation);
                        return Json(new
                        {
                            result = result,
                            IsCumulation = datainfo.IsCumulation,
                            datainfo = datainfo.Unit
                        });
              
                
            }

        }
        public List<historyChar> QueryDayOver(int DataID, string BeginDate,string EndDate, string rtuId, bool? IsCumulation)
        {
            double interval = 1000 * 60 * 60;
            List<dynamic> resultlist = new List<dynamic>();

            if (IsCumulation == true)
            {
              
                  var list = device01Service.GetWaterAns( interval, BeginDate, EndDate,DataID, IsCumulation, rtuId).ToList();
                if (list.Count() > 1)
                {
                    for (var i = 0; i < list.Count() - 1; i++)
                    {
                        Cumulation cc = new Cumulation();
                        cc.data = ((list[i + 1].data ?? 0.0) - (list[i].data ?? 0.0)) < 0 ? 0.0 : ((list[i + 1].data ?? 0.0) - (list[i].data ?? 0.0));
                        cc.datetime = list[i + 1].datetime;
                        cc.group = string.Format("{0:yyyy-MM-dd}", list[i + 1].datetime);
                        resultlist.Add(cc);
                    }
                }
            }
            else
            {
                interval = 1000 * 60 * 10;
                var list = device01Service.GetWaterAns(interval, BeginDate, EndDate, DataID, IsCumulation, rtuId).ToList();
                if (list.Count > 1)
                {
                    for (var i = 0; i < list.Count() - 1; i++)
                    {
                     

                        Cumulation cc = new Cumulation();
                        cc.data = (list[i].data == null || list[i].data < 0) ? 0.0 : list[i].data;
                        cc.datetime = list[i].datetime;
                        cc.group = string.Format("{0:yyyy-MM-dd}", list[i].datetime);
                        resultlist.Add(cc);
                    }
                    Cumulation cc2 = new Cumulation();
                    cc2.data = (list[list.Count - 1].data ?? 0.0);
                    cc2.datetime = list[list.Count - 1].datetime;
                    cc2.group = string.Format("{0:yyyy-MM-dd}", list[list.Count - 1].datetime);
                    resultlist.Add(cc2);

                }
                else if (list.Count == 1)
                {
                    Cumulation cc1 = new Cumulation();
                    cc1.data = (list[0].data == null || list[0].data < 0) ? 0.0 : list[0].data;
                    cc1.datetime = list[0].datetime;
                    cc1.group = string.Format("{0:yyyy-MM-dd}", list[0].datetime);
                    resultlist.Add(cc1);
                }
            }
            List<historyChar> result = new List<historyChar>();
            if (resultlist.Count > 0)
            {
                var datelist = resultlist.GroupBy(r => r.group).Select(r => r.Key).ToList();

                DateTime dtUTC = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                foreach (var item in datelist)
                {
                    var dataitem = resultlist.Where(r => r.group == item);
                    StringBuilder datastring = new StringBuilder();
                    historyChar chardata = new historyChar();
                    chardata.name = item;
                    foreach (var it in dataitem)
                    {
                        var dataT = it.data == null ? 0.0 : (double?)(Math.Round((double)it.data, 2));
                        string strPIN = "{name:'2000-01-01 " + it.datetime.ToString("HH:mm:ss") + "',value:['2000-01-01 " + it.datetime.ToString("HH:mm:ss") + "'," + dataT + "]},";
                        datastring.Append(strPIN);

                    }
                    if (datastring.ToString() != "")
                    {
                        chardata.data = "[" + datastring.ToString().Substring(0, datastring.ToString().Length - 1) + "]"; ;
                    }
                    result.Add(chardata);
                }
            }
            return result;
        }


        //月时间叠加数据
        //public List<historyChar> QueryMonthOver(int DataID, string BeginDate, string EndDate, string rtuId, bool? IsCumulation)
        //{
        //    double interval = 1000 * 60 * 60;
        //    var lists = device01Service.GetWaterAns(interval, BeginDate, EndDate, DataID, IsCumulation, rtuId).ToList();

        //    List<Cumulation> list = new List<Cumulation>() { };
        //    if (IsCumulation == true)
        //    {
        //        if (lists.Count > 1)
        //        {
        //            for (var i = 0; i < lists.Count - 1; i++)
        //            {
        //                Cumulation cc = new Cumulation();
        //                cc.data = ((lists[i + 1].data ?? 0.0) - (lists[i].data ?? 0.0)) < 0 ? 0.0 : ((lists[i + 1].data ?? 0.0) - (lists[i].data ?? 0.0));
        //                cc.datetime = Convert.ToDateTime(lists[i].datetime);
        //                cc.group = Convert.ToDateTime(lists[i].datetime).ToString("yyyy-MM-dd");
        //                list.Add(cc);
        //            }
        //        }
        //    }
        //    else
        //    {
        //        if (lists.Count > 1)
        //        {
        //            for (var i = 0; i < lists.Count - 1; i++)
        //            {

        //                Cumulation cc = new Cumulation();
        //                cc.data = (lists[i].data == null || lists[i].data < 0) ? 0.0 : lists[i].data;
        //                cc.datetime = Convert.ToDateTime(lists[i].datetime);
        //                cc.group = Convert.ToDateTime(lists[i].datetime).ToString("yyyy-MM-dd");
        //                list.Add(cc);
        //            }
        //            Cumulation cc2 = new Cumulation();
        //            cc2.data = (lists[lists.Count - 1].data ?? 0.0);
        //            cc2.datetime = Convert.ToDateTime(lists[lists.Count - 1].datetime);
        //            cc2.group = Convert.ToDateTime(lists[lists.Count - 1].datetime).ToString("yyyy-MM-dd");
        //            list.Add(cc2);
        //        }
        //        else if (lists.Count == 1)
        //        {
        //            Cumulation cc = new Cumulation();
        //            cc.data = (lists[0].data == null || lists[0].data < 0) ? 0.0 : lists[0].data;
        //            cc.datetime = Convert.ToDateTime(lists[0].datetime);
        //            cc.group = Convert.ToDateTime(lists[0].datetime).ToString("yyyy-MM-dd");
        //            list.Add(cc);
        //        }
        //    }
        //    var datelist = list.GroupBy(r => r.group).Select(r => r.Key).ToList();
        //    List<historyChar> result = new List<historyChar>();
        //    DateTime dtUTC = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        //    foreach (var item in datelist)
        //    {
        //        var dataitem = list.Where(r => r.group == item);
        //        StringBuilder datastring = new StringBuilder();
        //        historyChar chardata = new historyChar();
        //        chardata.name = item.ToString();
        //        foreach (var it in dataitem)
        //        {
        //            var time =  it.datetime.Day.ToString();
        //            var dataT = it.data == null ? 0.0 : (double?)(Math.Round((double)it.data, 2));
        //            string strPIN = "{name:' " + time + "',value:['" + time + "'," + dataT + "]},";
        //            datastring.Append(strPIN);
        //        }
        //        if (datastring.ToString() != "")
        //        {
        //            chardata.data = "[" + datastring.ToString().Substring(0, datastring.ToString().Length - 1) + "]"; ;
        //        }
        //        result.Add(chardata);
        //    }
        //    return result;

        //}

        //月时间叠加数据
        public List<historyChar> QueryMonthOver(int DataID, string BeginDate, string EndDate, string rtuId, bool? IsCumulation)
        {
            double interval = 1000 * 60 * 60;
            var lists = device01Service.GetWaterAns(interval, BeginDate, EndDate, DataID, IsCumulation, rtuId).ToList();

            List<Cumulation> list = new List<Cumulation>() { };
            if (IsCumulation == true)
            {
                if (lists.Count > 1)
                {
                    for (var i = 0; i < lists.Count - 1; i++)
                    {
                        Cumulation cc = new Cumulation();
                        cc.data = ((lists[i + 1].data ?? 0.0) - (lists[i].data ?? 0.0)) < 0 ? 0.0 : ((lists[i + 1].data ?? 0.0) - (lists[i].data ?? 0.0));
                        cc.datetime = Convert.ToDateTime(lists[i].datetime);
                        cc.group = Convert.ToDateTime(lists[i].datetime).ToString("yyyy-MM-dd");
                        list.Add(cc);
                    }
                }
            }
            else
            {
                if (lists.Count > 1)
                {
                    for (var i = 0; i < lists.Count - 1; i++)
                    {

                        Cumulation cc = new Cumulation();
                        cc.data = (lists[i].data == null || lists[i].data < 0) ? 0.0 : lists[i].data;
                        cc.datetime = Convert.ToDateTime(lists[i].datetime);
                        cc.group = Convert.ToDateTime(lists[i].datetime).ToString("yyyy-MM-dd");
                        list.Add(cc);
                    }
                    Cumulation cc2 = new Cumulation();
                    cc2.data = (lists[lists.Count - 1].data ?? 0.0);
                    cc2.datetime = Convert.ToDateTime(lists[lists.Count - 1].datetime);
                    cc2.group = Convert.ToDateTime(lists[lists.Count - 1].datetime).ToString("yyyy-MM-dd");
                    list.Add(cc2);
                }
                else if (lists.Count == 1)
                {
                    Cumulation cc = new Cumulation();
                    cc.data = (lists[0].data == null || lists[0].data < 0) ? 0.0 : lists[0].data;
                    cc.datetime = Convert.ToDateTime(lists[0].datetime);
                    cc.group = Convert.ToDateTime(lists[0].datetime).ToString("yyyy-MM-dd");
                    list.Add(cc);
                }
            }
            var datelist = list.GroupBy(r => r.group).Select(r => r.Key).ToList();
            List<historyChar> result = new List<historyChar>();
            DateTime dtUTC = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            foreach (var item in datelist)
            {
                var dataitem = list.Where(r => r.group == item);
               StringBuilder datastring = new StringBuilder();
                historyChar chardata = new historyChar();
                chardata.name = item.ToString();
                foreach (var it in dataitem)
                {
                    var time = it.datetime.Day.ToString();
                    var dataT = it.data == null ? 0.0 : (double?)(Math.Round((double)it.data, 2));
                    string strPIN = "{name:' " + time + "',value:['" + time + "'," + dataT + "]},";
                    datastring.Append(strPIN);
                }
                if (datastring.ToString() != "")
                {
                    chardata.data = "[" + datastring.ToString().Substring(0, datastring.ToString().Length - 1) + "]"; ;
                }
                result.Add(chardata);
            }
            return result;

        }
        #endregion

        public class historyChar
        {
            public string name { get; set; }
            public string data { get; set; }
        }
        public class historyChars
        {
            public List<string> name { get; set; }
            public List<string> data { get; set; }
        }
        public class Cumulation
        {
            public DateTime datetime { get; set; }
            public double? data { get; set; }
            public string group { get; set; }
        }
    }
}
