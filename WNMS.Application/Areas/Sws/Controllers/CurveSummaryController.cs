using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using MongoDBHelper;
using WNMS.Application.Utility;
using WNMS.Application.Utility.Filters;
using WNMS.IService;
using WNMS.Model.CustomizedClass;
using WNMS.Model.DataModels;

namespace WNMS.Application.Areas.Sws.Controllers
{
    [Area("Sws")]
    [TypeFilter(typeof(IgonreActionFilter))]
    public class CurveSummaryController : Controller
    {
        private ISysUserService _userService = null;
        private ISws_ConsumpSettingService _ConsumpSettingService = null;
        private ISws_DeviceInfo01Service _DeviceInfo01Service = null;
        public CurveSummaryController(ISysUserService sysUserService,
            ISws_ConsumpSettingService sws_ConsumpSettingService,
            ISws_DeviceInfo01Service sws_DeviceInfo01Service)
        {
            _userService = sysUserService;
            _ConsumpSettingService = sws_ConsumpSettingService;
            _DeviceInfo01Service = sws_DeviceInfo01Service;
        }
        public IActionResult Index()
        {
            long deviceid1 = 0;
            long deviceid2 = 0;
            string deviceName1 = "", deviceName2="";

            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = _userService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
            var datalist = _ConsumpSettingService.GetDeviceBySetting(user.UserId,4).ToList();
           
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
            return View();
        }
        #region 配置设备
        public IActionResult SelectDevices()
        {
            return View();
        }
        public IActionResult SelectDevice_single(long id)
        {
            ViewBag.TreeNodes = new HtmlString(GetDataDeviceTree("", id));
            var deviceModel = _DeviceInfo01Service.Query<SwsDeviceInfo01>(r => r.DeviceId == id).FirstOrDefault();
            ViewBag.deviceid = id;
            ViewBag.deviceName = deviceModel?.DeviceName;
            return View();
        }
        public string GetDataDeviceTree(string searchtxt, long deviceid)
        {
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = _userService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
            var datalist = _ConsumpSettingService.QueryDeviceTree(searchtxt,user.UserId,user.IsAdmin);
            var result = "[]";
            if (datalist.Count() > 0)
            {
              
                IEnumerable<dynamic> stationNode = datalist.GroupBy(r => new { r.StationID, r.StationName }).Select(r => new
                {
                    id = r.Key.StationID + "s",
                    pId = 0,
                    name = "<em class='iconfont icon-bengfang'></em>" + r.Key.StationName,
                    nocheck = true
                }).OrderBy(r => r.name);
                var deviceNode = datalist.OrderBy(r => r.Partition).Select(r => new
                {
                    id = r.DeviceID,
                    pId = r.StationID + "s",
                    name = r.DeviceName,
                    nocheck = false,
                    @checked = r.DeviceID == deviceid ? true : false
                });
               
                var treeNode = stationNode.Union(deviceNode);
                result = Newtonsoft.Json.JsonConvert.SerializeObject(treeNode);
            }
            return result;
        }
        public IActionResult SearchTree(string searchtxt,long deviceid)
        {
            return Content(GetDataDeviceTree(searchtxt, deviceid));
        }
        //设备配置写入数据库
        public IActionResult SettingDevice(long deviceid1,long deviceid2)
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
                s.Type = 4;
                s.UserId = userID;
                setting.Add(s);
            }
            try
            {
                _ConsumpSettingService.DeviceSetting(userID, 4, setting);

                str = "ok";

            }
            catch (Exception e)
            {
                str = "no";
            }

            return Content(str);
        }
        #endregion
        #region 实时数据
        //实时数据
        public IActionResult GetJKDatas(long EquipmentID1,long EquipmentID2)
        {
            var baseinfo = GetBaseInfo(EquipmentID1, EquipmentID2);
            var rtuidlist = baseinfo.Select(r => r.RTUID).Distinct();
            var rtuids = string.Join(",", rtuidlist);
            string macJson = "{\"RTUID\":{'$in':[" + rtuids + "]}}";
            var collections = new MongoDBHelper<RtuJKInfo>(DateTime.Now.Year.ToString());
            var jklist = collections.Find("Sws_DeviceJKInfo", macJson, "UpdateTime");
            List<PressClass> presslist = new List<PressClass>() { };
           
            foreach (var item in baseinfo)
            {
                if (jklist.Count > 0)
                {
                    var dataSingle = jklist.Where(r => r.RTUID == item.RTUID).FirstOrDefault();
                    if (dataSingle != null)
                    {
                        var pressIN = 2000+(item.Partition-1)*500+"";
                        var pressOut = 2001+ (item.Partition - 1) * 500+"";
                        var pressSet = 2002+ (item.Partition - 1) * 500+"";
                        PressClass p = new PressClass();
                        if (item.Partition == 6)//没找到设定dataid,用排污泵设定
                        {
                            pressIN = "4507";
                            pressOut = "4508";
                            pressSet = "4700";
                            p.pressSet = 0;
                        }
                        else
                        {
                            p.pressSet = dataSingle.AnalogValues.ContainsKey(pressSet) ? (double)dataSingle.AnalogValues[pressSet] : 0;
                        }
                        p.pressIN = dataSingle.AnalogValues.ContainsKey(pressIN)? (double)dataSingle.AnalogValues[pressIN]:0;
                        p.pressOut = dataSingle.AnalogValues.ContainsKey(pressOut) ? (double)dataSingle.AnalogValues[pressOut] : 0;
                        
                        presslist.Add(p);

                    }
                    else
                    {
                        PressClass p = new PressClass();
                        p.pressIN = 0;
                        p.pressOut = 0;
                        p.pressSet = 0;
                        presslist.Add(p);
                    }
                }
                else
                {
                    PressClass p = new PressClass();
                    p.pressIN = 0;
                    p.pressOut = 0;
                    p.pressSet = 0;
                    presslist.Add(p);
                }
            }
            return Json(new
            {
                presslist
            });
        }
        public List<RtuPartition> GetBaseInfo(long EquipmentID1, long EquipmentID2)
        {
            var idlist = new List<long>() { EquipmentID1, EquipmentID2 };
            var devicelist = _DeviceInfo01Service.Query<SwsDeviceInfo01>(r => idlist.Contains(r.DeviceId));
            if (EquipmentID1 > EquipmentID2)
            {
                devicelist = devicelist.OrderByDescending(r => r.DeviceId);
            }
            else
            {
                devicelist = devicelist.OrderBy(r => r.DeviceId);
            }
            List<RtuPartition> res = new List<RtuPartition>() { };
            foreach (var item in devicelist)
            {
                RtuPartition tt = new RtuPartition();
                tt.RTUID = (int)item.Rtuid;
                tt.Partition = item.Partition;
                tt.Frequency = item.Frequency;
                tt.PumpNum= item.PumpNum;
                res.Add(tt);
            }
            return res;
        }
        public class RtuPartition { 
            public int RTUID { get; set; }
            public byte Partition { get; set; }
            public int Frequency { get; set; }

            public int PumpNum { get; set; }
        }
        public class PressClass
        {
            public double pressIN { get; set; }
            public double pressSet { get; set; }
            public double pressOut { get; set; }

        }
        #endregion
        #region 能耗、单吨能耗
        public IActionResult GetNHData(long EquipmentID1,long EquipmentID2)
        {
            var begindate = Convert.ToDateTime(DateTime.Now.Year + "-01-01");
            var endate = DateTime.Now;
            var equipmentids = EquipmentID1 + "," + EquipmentID2;
            List<long> equlist = new List<long>() { EquipmentID1, EquipmentID2 };
            var dataOfMonth = _ConsumpSettingService.GetMonthNHData(equipmentids, begindate.ToString(), endate.ToString()).ToList();
            var begindate1 = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-01"));
            var thismonth = _ConsumpSettingService.GetThisMonthNH(equipmentids,begindate1.ToString(), endate.ToString());
            if (thismonth.Count() > 0)
            {
                dataOfMonth = dataOfMonth.Union(thismonth).ToList();

            }
            var endtime = begindate1;
            List<string> axias = new List<string>() { };

            List<DateTime> times = new List<DateTime>() { };
            List<WaterData> result = new List<WaterData>() { };
            while (begindate <= endtime)
            {
                axias.Add(begindate.ToString("MM") + "月");
                times.Add(begindate);
                begindate = begindate.AddMonths(1);
            }
            if (dataOfMonth.Count() > 0)
            {
                foreach (var item in equlist)
                {
                    WaterData w = new WaterData();
                    w.equipid = item;
                    w.eneryAvg = new List<double>() { };
                    w.eneryData = new List<double>() { };
                    w.flowData = new List<double>() { };
                    foreach (var ii in times)
                    {
                        var model = dataOfMonth.Where(r => r.DeviceID == item && r.UpdateTime == ii).FirstOrDefault();
                        if (model != null)
                        {

                            w.eneryData.Add(Math.Round((double)model.EnergyCon, 2));
                            w.flowData.Add(Math.Round((double)model.FlowCon, 2));
                            if (model.FlowCon != 0)
                            {
                                w.eneryAvg.Add(Math.Round((double)model.EnergyCon / (double)model.FlowCon, 2));
                            }
                            else
                            {
                                w.eneryAvg.Add(0);
                            }
                        }
                        else
                        {
                            w.eneryAvg.Add(0);
                            w.eneryData.Add(0);
                            w.flowData.Add(0);
                        }
                    }
                    result.Add(w);
                }
            }
            else
            {

                foreach (var item in equlist)
                {
                    WaterData w = new WaterData();
                    w.equipid = item;
                    w.eneryAvg = new List<double>() { };
                    w.eneryData = new List<double>() { };
                    w.flowData = new List<double>() { };
                    foreach (var it in times)
                    {
                        w.eneryAvg.Add(0);
                        w.eneryData.Add(0);
                        w.flowData.Add(0);
                    }
                    result.Add(w);
                }
            }
            return Json(new
            {
                datas = result,
                axias = axias
            });
        }
        public class WaterData
        {
            public long equipid { get; set; }
            public List<double> flowData { get; set; }
            public List<double> eneryData { get; set; }
            public List<double> eneryAvg { get; set; }
        }
        #endregion
        #region 频率压力数据
        public IActionResult FrePressData(long EquipmentID1, long EquipmentID2)
        {
            var baseinfo = GetBaseInfo(EquipmentID1, EquipmentID2);
            var begindate = DateTime.Now.Date;
            var endate = DateTime.Now;
            //判断时间间隔
            var intervalTime = (endate - begindate).TotalMinutes;
            if (intervalTime < 30)
            {
                begindate = endate.AddMinutes(-30);
            }
            List<FrequencyData> result = new List<FrequencyData>() { };
            foreach (var item in baseinfo)
            {
                FrequencyData f = new FrequencyData();
                string project = "";
                string pressOut = 2001 + (item.Partition - 1) * 500 + "";
                if (item.Partition == 6)
                {
                    pressOut = "4508";
                }
                if (item.Frequency == 1)//单变频
                {
                    var fredataid = 2003+(item.Partition - 1) * 500 + "";
                    project = @"{'$project':{
                'UpdateTime':1,
                  'fdata':{$ifNull:['$AnalogValues."+ fredataid + "',0]},"+
                  @"'pdata':{$ifNull:['$AnalogValues."+ pressOut + "',0]}}}";
                }
                else//多变频
                {
                    var fault = 2004+ (item.Partition - 1) * 500;
                    string ss = "";
                    for (var i = 0; i < item.PumpNum; i++)
                    {
                        var tkey = fault + i + "";
                        ss += "{$ifNull:['$AnalogValues." + tkey + "',0]},";
                        //ss += "'$AnalogValues." + tkey + "',";
                    }
                    ss = ss.Substring(0, ss.Length - 1);
                    project = @"{'$project':{
                'UpdateTime':1,
                  'fdata':{'$avg':{'$sum':[" + ss + "]}}," +
         @"'pdata':{$ifNull:['$AnalogValues." + pressOut + "',0]}}}";
                }
                var datalist = _ConsumpSettingService.HistoryChartData(begindate, endate, project,item.RTUID.ToString());
                if (begindate.Year != endate.Year)
                {
                    var begindate_append =Convert.ToDateTime(endate.Year + "-01-01");
                    var appenddata= _ConsumpSettingService.HistoryChartData(begindate_append, endate, project, item.RTUID.ToString());
                    if (appenddata.Count() > 0)
                    {
                        datalist = datalist.Union(appenddata);
                    }
                }
                if (datalist.Count() > 0)
                {
                    StringBuilder datastring = new StringBuilder();//频率
                    StringBuilder Pressstring = new StringBuilder();//出水压力
                    foreach (var ii in datalist)
                    {
                        //频率
                        var fdata = Math.Round((double)ii.fdata, 2);
                        string str = "{name:'" + ii.UpdateTime.ToString("yyyy-MM-dd HH:mm:ss") + "',value:['" + ii.UpdateTime.ToString("yyyy-MM-dd HH:mm:ss") + "'," + Math.Round(fdata, 2) + "]},";
                        datastring.Append(str);

                        //出水压力
                        var pdata = Math.Round((double)ii.pdata, 2);
                        string strp = "{name:'" + ii.UpdateTime.ToString("yyyy-MM-dd HH:mm:ss") + "',value:['" + ii.UpdateTime.ToString("yyyy-MM-dd HH:mm:ss") + "'," + Math.Round(pdata, 2) + "]},";
                        Pressstring.Append(strp);
                    }
                    if (datastring.ToString() != "")
                    {

                        f.datalist = "[" + datastring.ToString().Substring(0, datastring.ToString().Length - 1) + "]";
                        f.outPress = "[" + Pressstring.ToString().Substring(0, Pressstring.ToString().Length - 1) + "]";
                    }
                    result.Add(f);
                }
                else
                {
                    f.datalist = "";
                    f.outPress = "";
                    result.Add(f);
                }

            }
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(result));
        }
        //频率、压力实时更新
        public IActionResult UpdateLineData(long EquipmentID1, long EquipmentID2)
        {
            var baseinfo = GetBaseInfo(EquipmentID1, EquipmentID2);
           
            var rtuidlist = baseinfo.Select(r => r.RTUID).Distinct();
            var rtuids = string.Join(",", rtuidlist);
            string macJson = "{\"RTUID\":{'$in':[" + rtuids + "]}}";
            var collections = new MongoDBHelper<RtuJKInfo>(DateTime.Now.Year.ToString());
            var jklist = collections.Find("Sws_DeviceJKInfo", macJson, "UpdateTime");
            List<FrequencyData> result = new List<FrequencyData>() { };
            if (jklist.Count > 0)
            {
                foreach (var item in baseinfo)
                {
                    FrequencyData f = new FrequencyData();
                    var datas = jklist.Where(r => r.RTUID == item.RTUID).FirstOrDefault();
                    if (datas != null)
                    {
                        StringBuilder datastring = new StringBuilder();//频率
                        StringBuilder Pressstring = new StringBuilder();//出水压力
                        string pressOut = 2001 + (item.Partition - 1) * 500 + "";
                        if (item.Partition == 6)
                        {
                            pressOut = "4508";
                        }
                        if (item.Frequency == 1)//单变频
                        {
                            var fredataid = 2003 + (item.Partition - 1) * 500 + "";
                            //频率
                            var fdata = datas.AnalogValues.ContainsKey(fredataid) ? (double)datas.AnalogValues[fredataid] : 0;
                            string str = "{name:'" + datas.UpdateTime.ToString("yyyy-MM-dd HH:mm:ss") + "',value:['" + datas.UpdateTime.ToString("yyyy-MM-dd HH:mm:ss") + "'," + Math.Round(fdata, 2) + "]},";
                            datastring.Append(str);

                            //出水压力
                            var pdata = datas.AnalogValues.ContainsKey(pressOut) ? (double)datas.AnalogValues[pressOut] : 0;
                            string strp = "{name:'" + datas.UpdateTime.ToString("yyyy-MM-dd HH:mm:ss") + "',value:['" + datas.UpdateTime.ToString("yyyy-MM-dd HH:mm:ss") + "'," + Math.Round(pdata, 2) + "]},";
                            Pressstring.Append(strp);
                        }
                        else
                        {
                            var fault = 2004 + (item.Partition - 1) * 500;
                            var Numflag = 0;
                            double value = 0;
                            for (var i = 0; i < item.PumpNum; i++)
                            {
                                var tkey = fault + i + "";
                                if (datas.AnalogValues.ContainsKey(tkey) && (double)datas.AnalogValues[tkey] > 0)
                                {
                                    value += (double)datas.AnalogValues[tkey];
                                    Numflag++;
                                }
                            }

                            var fdata = Numflag > 0 ? (Math.Round(value / Numflag, 2)) : 0;

                            string str = "{name:'" + datas.UpdateTime.ToString("yyyy-MM-dd HH:mm:ss") + "',value:['" + datas.UpdateTime.ToString("yyyy-MM-dd HH:mm:ss") + "'," + Math.Round(fdata, 2) + "]},";
                            datastring.Append(str);


                            //出水压力
                            var pdata = datas.AnalogValues.ContainsKey(pressOut) ? (double)datas.AnalogValues[pressOut] : 0;
                            string strp = "{name:'" + datas.UpdateTime.ToString("yyyy-MM-dd HH:mm:ss") + "',value:['" + datas.UpdateTime.ToString("yyyy-MM-dd HH:mm:ss") + "'," + Math.Round(pdata, 2) + "]},";
                            Pressstring.Append(strp);

                        }
                        if (datastring.ToString() != "")
                        {

                            f.datalist = "[" + datastring.ToString().Substring(0, datastring.ToString().Length - 1) + "]";
                            f.outPress = "[" + Pressstring.ToString().Substring(0, Pressstring.ToString().Length - 1) + "]";
                        }
                        result.Add(f);
                    }
                    else
                    {
                        f.datalist = "";
                        f.outPress = "";
                        result.Add(f);

                    }
                }

            }
            else
            {
                FrequencyData f = new FrequencyData();
                f.datalist = "";
                f.outPress = "";
                result.Add(f);
                result.Add(f);

            }
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(result));
        }
        public class FrequencyData
        {
            public string datalist { get; set; }//频率
            public string outPress { get; set; }//出水压力

        }
        #endregion
        #region 压力波动分析（按月汇总）
        public IActionResult GetFlucNumber(long EquipmentID1, long EquipmentID2, double diff)
        {
            string project = "";
            string match = "";
            string group = "";
            var baseinfo = GetBaseInfo(EquipmentID1, EquipmentID2);
            var begindate = Convert.ToDateTime(DateTime.Now.Year + "-01-01");
            var enddate = DateTime.Now;
            List<string> axias = new List<string>() { };
            List<string> times = new List<string>() { };
            List<FlucNum> result = new List<FlucNum>() { };
            while (begindate <= enddate)
            {
                axias.Add(begindate.Month + "月");
                times.Add(begindate.ToString("yyyy-MM"));
                begindate = begindate.AddMonths(1);
            }
            foreach (var item in baseinfo)
            {
                FlucNum f = new FlucNum();
                f.datalist = new List<double>() { };
                var outPress = 2001 + (item.Partition - 1) * 500+"";
                var setPress= 2002 + (item.Partition - 1) * 500 + "";
                project = @"{$project:{'data1':{$abs:{$subtract:[{$ifNull:['$AnalogValues."+ outPress + "',0.0]},{$ifNull:['$AnalogValues."+ setPress + "',0.0]}]}},'datetime':'$UpdateTime'}}";
                match = @"{'$match':{'data1':{'$gte':" + diff + "}}}";
                group = @"{'$group':{
                 '_id':{'$dateToString':{'format':'%Y-%m','date':'$datetime'}},
                  'count':{'$sum':1}    
                }}";
                var data = _ConsumpSettingService.GetPressFlucNum(begindate.Year.ToString(), begindate.ToString(),enddate.ToString(),match,group,project,item.RTUID.ToString());
                if (data.Count() > 0)
                {
                    foreach (var ii in times)
                    {
                        var tt = data.Where(r => r._id == ii).FirstOrDefault();
                        if (tt != null)
                        {
                            f.datalist.Add(tt.count);
                        }
                        else
                        {
                            f.datalist.Add(0);
                        }
                    }
                }
                else
                {
                    foreach (var ii in times)
                    {
                        f.datalist.Add(0);
                    }
                }
                result.Add(f);
            }
            return Json(new
            {
                axias = axias,
                result = result
            });
        }
        public class FlucNum
        {
            public List<double> datalist { get; set; }
        }
        #endregion
        #region 设备对比结论
        public async Task<IActionResult> GetCompareData(long EquipmentID1, long EquipmentID2, double diff)
        {
            try
            {
                List<long> equipmentids = new List<long>() { EquipmentID1, EquipmentID2 };
                var begindate = DateTime.Now.Date;
                var endate = DateTime.Now;
                var baseinfo = GetBaseInfo(EquipmentID1, EquipmentID2);
                #region 压力波动
                List<double> dataFluc = new List<double>() { };

                foreach (var item in baseinfo)
                {
                    var outPress = 2001 + (item.Partition - 1) * 500 + "";
                    var setPress = 2002 + (item.Partition - 1) * 500 + "";
                    string project = @"{$project:{'data1':{$abs:{$subtract:[{$ifNull:['$AnalogValues." + outPress + "',0.0]},{$ifNull:['$AnalogValues." + setPress + "',0.0]}]}},'datetime':'1'}}";
                    var match = @"{'$match':{'data1':{'$gte':" + diff + "}}}";

                    var group = @"{'$group':{
                 '_id':'$datetime',
                  'count':{'$sum':1}    
                }}";
                    var data = _ConsumpSettingService.GetPressFlucNum_today(project,match,group,item.RTUID.ToString(), begindate, endate).ToList();
                    if (data.Count > 0)
                    {
                        dataFluc.Add(data.FirstOrDefault().count);
                    }
                    else
                    {
                        dataFluc.Add(0);
                    }
                }
                #endregion
                #region 最大值最小值
                List<MaxValue> result = new List<MaxValue>() { };
                string eids = EquipmentID1 + "," + EquipmentID2;
                var datalist = _ConsumpSettingService.GetMaxWaterData(begindate, endate, eids).ToList();
                if (datalist.Count > 0)
                {
                    foreach (var item in equipmentids)
                    {
                        var model = datalist.Where(r => r.DeviceID == item).FirstOrDefault();
                        if (model != null)
                        {
                            MaxValue m = new MaxValue();
                            m.consump = (double)model.consump;
                            m.maxFlow = (double)model.maxFlow;
                            m.maxPower = (double)model.maxEnergy;
                            m.minFlow = (double)model.minFlow;
                            m.minPower = (double)model.minEnergy;
                            result.Add(m);
                        }
                        else
                        {
                            MaxValue m = new MaxValue();
                            m.consump = 0;
                            m.maxFlow = 0;
                            m.maxPower = 0;
                            m.minFlow = 0;
                            m.minPower = 0;
                            result.Add(m);
                        }

                    }
                }
                else
                {
                    foreach (var item in equipmentids)
                    {
                        MaxValue m = new MaxValue();
                        m.consump = 0;
                        m.maxFlow = 0;
                        m.maxPower = 0;
                        m.minFlow = 0;
                        m.minPower = 0;
                        result.Add(m);
                    }

                }
                #endregion
                #region 高低频
                List<RunTime> resulthigh = new List<RunTime>() { };
                foreach (var item in baseinfo)
                {
                    RunTime f = new RunTime();
                    double totalseconde_H = 0;//高频运行
                    double totalseconde_L = 0;//低频运行
                    string project = "";
                    List<int> keys = new List<int>() { };
                    if (item.Frequency == 1)//单变频
                    {
                        var fredataid = 2003 + (item.Partition - 1) * 500 + "";
                        project = "{$project:{" +
                       "'_id': 0," +
                       "'data1':{$ifNull:['$AnalogValues." + fredataid + "',0.0]}," +
                      "'UpdateTime':1" +
              " }}";
                        keys.Add(1);
                    }
                    else
                    {
                        var fault = 2004 + (item.Partition - 1) * 500;
                        string ss = "";
                        for (var i = 0; i < item.PumpNum; i++)
                        {
                            var tkey = fault + i + "";
                            var num = i + 1;
                            ss += "'data" + num + "':{$ifNull:['$AnalogValues." + tkey + "',0.0]},";
                            keys.Add(num);
                        }

                        project = "{$project:{" +
                      "'_id': 0," +
                      "" + ss + "" +
                     "'UpdateTime':1" +
             " }}";
                    }
                    QueryRunSecond(begindate, endate, item.RTUID.ToString(), keys, project, ref totalseconde_H, ref totalseconde_L);
                    f.HighHour = Math.Round(totalseconde_H / 3600, 2);
                    f.LowHour = Math.Round(totalseconde_L / 3600, 2);
                    resulthigh.Add(f);
                }
                #endregion
                result[0].FlucNum = dataFluc[0];
                result[0].HighHour = resulthigh[0].HighHour;
                result[0].LowHour = resulthigh[0].LowHour;
                result[1].HighHour = resulthigh[1].HighHour;
                result[1].LowHour = resulthigh[1].LowHour;
                result[1].FlucNum = dataFluc[1];
                PartialView("TodayFreshData", result);
                string dataTable = await ViewToString.RenderPartialViewToString(this, "TodayFreshData");
                return Json(new
                {

                    dataTable = dataTable
                });
            }
            catch (Exception e)
            {
                return Content("");
            }
        }
        public void QueryRunSecond(DateTime begindate, DateTime endate, string rtuid, List<int> keys,string project, ref double totalseconde_H, ref double totalseconde_L)
        {
            var highF = 45;
            var lowF = 30;
           
            var datas = _ConsumpSettingService.HistoryChartData(begindate, endate, project, rtuid).ToList();
            if (datas.Count > 0)
            {
                foreach (var item in keys)
                {
                    var flag = "";//高或者低
                    double fdata = 0;
                    if (item == 1)
                    {
                        fdata = datas[0].data1;
                    }
                    else if (item == 2)
                    {
                        fdata = datas[0].data2;
                    }
                    else if (item == 3)
                    {
                        fdata = datas[0].data3;
                    }
                    else if (item == 4)
                    {
                        fdata = datas[0].data4;
                    }
                    else if (item == 5)
                    {
                        fdata = datas[0].data5;
                    }
                    else if (item == 6)
                    {
                        fdata = datas[0].data6;
                    }
                    if (fdata < lowF && fdata > 0)
                    {
                        flag = "低";
                    }
                    if (fdata > highF)
                    {
                        flag = "高";
                    }
                    var begintime = datas[0].UpdateTime;
                    var endtime = datas.LastOrDefault().UpdateTime;
                    while (begintime < endtime)
                    {
                        if (flag == "低")
                        {
                            dynamic tt= datas[0];
                            if (item == 1)
                            {
                                tt = datas.Where(r => ((r.data1 >= lowF) || r.data1 == 0) && r.UpdateTime > begintime).FirstOrDefault();
                                if (tt != null)
                                {
                                    totalseconde_L += (tt.UpdateTime - begintime).TotalSeconds;//总秒数
                                    begintime = tt.UpdateTime;
                                    if (tt.data1 > highF)
                                    {
                                        flag = "高";
                                    }
                                    else
                                    {
                                        flag = "";
                                    }
                                }
                                else
                                {
                                    begintime = endtime;
                                }
                            }
                            else if (item == 2)
                            {
                                tt = datas.Where(r => ((r.data2 >= lowF) || r.data2 == 0) && r.UpdateTime > begintime).FirstOrDefault();
                                if (tt != null)
                                {
                                    totalseconde_L += (tt.UpdateTime - begintime).TotalSeconds;//总秒数
                                    begintime = tt.UpdateTime;
                                    if (tt.data2 > highF)
                                    {
                                        flag = "高";
                                    }
                                    else
                                    {
                                        flag = "";
                                    }
                                }
                                else
                                {
                                    begintime = endtime;
                                }
                            }
                            else if (item == 3)
                            {
                                tt = datas.Where(r => ((r.data3 >= lowF) || r.data3== 0) && r.UpdateTime > begintime).FirstOrDefault();
                                if (tt != null)
                                {
                                    totalseconde_L += (tt.UpdateTime - begintime).TotalSeconds;//总秒数
                                    begintime = tt.UpdateTime;
                                    if (tt.data3 > highF)
                                    {
                                        flag = "高";
                                    }
                                    else
                                    {
                                        flag = "";
                                    }
                                }
                                else
                                {
                                    begintime = endtime;
                                }
                            }
                            else if (item == 4)
                            {
                                tt = datas.Where(r => ((r.data4 >= lowF) || r.data4 == 0) && r.UpdateTime > begintime).FirstOrDefault();
                                if (tt != null)
                                {
                                    totalseconde_L += (tt.UpdateTime - begintime).TotalSeconds;//总秒数
                                    begintime = tt.UpdateTime;
                                    if (tt.data4 > highF)
                                    {
                                        flag = "高";
                                    }
                                    else
                                    {
                                        flag = "";
                                    }
                                }
                                else
                                {
                                    begintime = endtime;
                                }
                            }
                            else if (item == 5)
                            {
                                tt = datas.Where(r => ((r.data5 >= lowF) || r.data5 == 0) && r.UpdateTime > begintime).FirstOrDefault();
                                if (tt != null)
                                {
                                    totalseconde_L += (tt.UpdateTime - begintime).TotalSeconds;//总秒数
                                    begintime = tt.UpdateTime;
                                    if (tt.data5 > highF)
                                    {
                                        flag = "高";
                                    }
                                    else
                                    {
                                        flag = "";
                                    }
                                }
                                else
                                {
                                    begintime = endtime;
                                }
                            }
                            else if (item == 6)
                            {
                                tt = datas.Where(r => ((r.data6 >= lowF) || r.data6 == 0) && r.UpdateTime > begintime).FirstOrDefault();
                                if (tt != null)
                                {
                                    totalseconde_L += (tt.UpdateTime - begintime).TotalSeconds;//总秒数
                                    begintime = tt.UpdateTime;
                                    if (tt.data6 > highF)
                                    {
                                        flag = "高";
                                    }
                                    else
                                    {
                                        flag = "";
                                    }
                                }
                                else
                                {
                                    begintime = endtime;
                                }
                            }
                            
                        }
                        else if (flag == "高")
                        {
                            var tt = datas[0];
                            if (item == 1)
                            {
                                tt = datas.Where(r => r.data1 <= highF && r.UpdateTime > begintime).FirstOrDefault();
                                if (tt != null)
                                {
                                    totalseconde_H += (tt.UpdateTime - begintime).TotalSeconds;//总秒数
                                    begintime = tt.UpdateTime;
                                    if (tt.data1 < lowF && tt.data1 > 0)
                                    {
                                        flag = "低";
                                    }
                                    else
                                    {
                                        flag = "";
                                    }
                                }
                                else
                                {
                                    begintime = endtime;
                                }
                            }
                            else if (item == 2)
                            {
                                tt = datas.Where(r => r.data2 <= highF && r.UpdateTime > begintime).FirstOrDefault();
                                if (tt != null)
                                {
                                    totalseconde_H += (tt.UpdateTime - begintime).TotalSeconds;//总秒数
                                    begintime = tt.UpdateTime;
                                    if (tt.data2 < lowF && tt.data2 > 0)
                                    {
                                        flag = "低";
                                    }
                                    else
                                    {
                                        flag = "";
                                    }
                                }
                                else
                                {
                                    begintime = endtime;
                                }
                            }
                            else if (item == 3)
                            {
                                tt = datas.Where(r => r.data3 <= highF && r.UpdateTime > begintime).FirstOrDefault();
                                if (tt != null)
                                {
                                    totalseconde_H += (tt.UpdateTime - begintime).TotalSeconds;//总秒数
                                    begintime = tt.UpdateTime;
                                    if (tt.data3 < lowF && tt.data3 > 0)
                                    {
                                        flag = "低";
                                    }
                                    else
                                    {
                                        flag = "";
                                    }
                                }
                                else
                                {
                                    begintime = endtime;
                                }
                            }
                            else if (item == 4)
                            {
                                tt = datas.Where(r => r.data4 <= highF && r.UpdateTime > begintime).FirstOrDefault();
                                if (tt != null)
                                {
                                    totalseconde_H += (tt.UpdateTime - begintime).TotalSeconds;//总秒数
                                    begintime = tt.UpdateTime;
                                    if (tt.data4 < lowF && tt.data4 > 0)
                                    {
                                        flag = "低";
                                    }
                                    else
                                    {
                                        flag = "";
                                    }
                                }
                                else
                                {
                                    begintime = endtime;
                                }
                            }
                            else if (item == 5)
                            {
                                tt = datas.Where(r => r.data5 <= highF && r.UpdateTime > begintime).FirstOrDefault();
                                if (tt != null)
                                {
                                    totalseconde_H += (tt.UpdateTime - begintime).TotalSeconds;//总秒数
                                    begintime = tt.UpdateTime;
                                    if (tt.data5 < lowF && tt.data5 > 0)
                                    {
                                        flag = "低";
                                    }
                                    else
                                    {
                                        flag = "";
                                    }
                                }
                                else
                                {
                                    begintime = endtime;
                                }
                            }
                            else if (item == 6)
                            {
                                tt = datas.Where(r => r.data6 <= highF && r.UpdateTime > begintime).FirstOrDefault();
                                if (tt != null)
                                {
                                    totalseconde_H += (tt.UpdateTime - begintime).TotalSeconds;//总秒数
                                    begintime = tt.UpdateTime;
                                    if (tt.data6 < lowF && tt.data6 > 0)
                                    {
                                        flag = "低";
                                    }
                                    else
                                    {
                                        flag = "";
                                    }
                                }
                                else
                                {
                                    begintime = endtime;
                                }
                            }
                            
                        }
                        else
                        {
                            if (item == 1)
                            {
                                var tt = datas.Where(r => (r.data1 > highF || (r.data1 > 0 && r.data1 < lowF)) && r.UpdateTime > begintime).FirstOrDefault();
                                if (tt != null)
                                {

                                    begintime = tt.UpdateTime;
                                    if (tt.data1 < lowF && tt.data1 > 0)
                                    {
                                        flag = "低";
                                    }
                                    else if (tt.data1 > highF)
                                    {
                                        flag = "高";
                                    }
                                    else
                                    {
                                        flag = "";
                                    }
                                }
                                else
                                {
                                    begintime = endtime;
                                }
                            }
                            else if (item == 2)
                            {
                                var tt = datas.Where(r => (r.data2 > highF || (r.data2 > 0 && r.data2 < lowF)) && r.UpdateTime > begintime).FirstOrDefault();
                                if (tt != null)
                                {

                                    begintime = tt.UpdateTime;
                                    if (tt.data2 < lowF && tt.data2 > 0)
                                    {
                                        flag = "低";
                                    }
                                    else if (tt.data2 > highF)
                                    {
                                        flag = "高";
                                    }
                                    else
                                    {
                                        flag = "";
                                    }
                                }
                                else
                                {
                                    begintime = endtime;
                                }
                            }
                            else if (item == 3)
                            {
                                var tt = datas.Where(r => (r.data3 > highF || (r.data3 > 0 && r.data3 < lowF)) && r.UpdateTime > begintime).FirstOrDefault();
                                if (tt != null)
                                {

                                    begintime = tt.UpdateTime;
                                    if (tt.data3 < lowF && tt.data3 > 0)
                                    {
                                        flag = "低";
                                    }
                                    else if (tt.data3 > highF)
                                    {
                                        flag = "高";
                                    }
                                    else
                                    {
                                        flag = "";
                                    }
                                }
                                else
                                {
                                    begintime = endtime;
                                }
                            }
                            else if (item == 4)
                            {
                                var tt = datas.Where(r => (r.data4 > highF || (r.data4 > 0 && r.data4 < lowF)) && r.UpdateTime > begintime).FirstOrDefault();
                                if (tt != null)
                                {

                                    begintime = tt.UpdateTime;
                                    if (tt.data4 < lowF && tt.data4 > 0)
                                    {
                                        flag = "低";
                                    }
                                    else if (tt.data4 > highF)
                                    {
                                        flag = "高";
                                    }
                                    else
                                    {
                                        flag = "";
                                    }
                                }
                                else
                                {
                                    begintime = endtime;
                                }
                            }
                            else if (item == 5)
                            {
                                var tt = datas.Where(r => (r.data5 > highF || (r.data5 > 0 && r.data5 < lowF)) && r.UpdateTime > begintime).FirstOrDefault();
                                if (tt != null)
                                {

                                    begintime = tt.UpdateTime;
                                    if (tt.data5 < lowF && tt.data5 > 0)
                                    {
                                        flag = "低";
                                    }
                                    else if (tt.data5 > highF)
                                    {
                                        flag = "高";
                                    }
                                    else
                                    {
                                        flag = "";
                                    }
                                }
                                else
                                {
                                    begintime = endtime;
                                }
                            }
                            else if (item == 6)
                            {
                                var tt = datas.Where(r => (r.data6 > highF || (r.data6 > 0 && r.data6 < lowF)) && r.UpdateTime > begintime).FirstOrDefault();
                                if (tt != null)
                                {

                                    begintime = tt.UpdateTime;
                                    if (tt.data6 < lowF && tt.data6 > 0)
                                    {
                                        flag = "低";
                                    }
                                    else if (tt.data6 > highF)
                                    {
                                        flag = "高";
                                    }
                                    else
                                    {
                                        flag = "";
                                    }
                                }
                                else
                                {
                                    begintime = endtime;
                                }
                            }
                        }
                    }
                }
            }
        }
        public class RunTime
        {
            public double HighHour { get; set; }//高频运行时间
            public double LowHour { get; set; }//低频运行时间

        }
        #endregion
        #region 周波动次数
        public IActionResult GetFlucNum_WeekMonth(long EquipmentID1, long EquipmentID2, double diff, string key)
        {
            var begindate = DateTime.Now.Date;
            var endate = DateTime.Now;
            if (key == "周")
            {
                begindate = DateTime.Now.Date.AddDays(-7);
            }
            else if (key == "月")
            {
                begindate = DateTime.Now.Date.AddDays(-30);
            }
            List<double> result = new List<double>() { };
           
            var baseinfo = GetBaseInfo(EquipmentID1, EquipmentID2);
            foreach (var item in baseinfo)
            {
                var outPress = 2001 + (item.Partition - 1) * 500 + "";
                var setPress = 2002 + (item.Partition - 1) * 500 + "";
                string project = @"{$project:{'data1':{$abs:{$subtract:[{$ifNull:['$AnalogValues." + outPress + "',0.0]},{$ifNull:['$AnalogValues." + setPress + "',0.0]}]}},'datetime':'1'}}";
                var match = @"{'$match':{'data1':{'$gte':" + diff + "}}}";
                
                var group = @"{'$group':{
                 '_id':'$datetime',
                  'count':{'$sum':1}    
                }}";
                var data = _ConsumpSettingService.GetPressFlucNum_today(project,match,group,item.RTUID.ToString(), begindate, endate).ToList();
                var num = 0;
                if (data.Count > 0)
                {
                    num=data.FirstOrDefault().count;
                }
               
                if (begindate.Year != endate.Year)
                {
                    var begindate1 =Convert.ToDateTime(endate.Year + "-01-01");
                    var data2 = _ConsumpSettingService.GetPressFlucNum_today(project, match, group, item.RTUID.ToString(), begindate1, endate).ToList();
                    if (data2.Count > 0)
                    {
                        num += data2.FirstOrDefault().count;
                    }
                }
               
                result.Add(num);
               
            }
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(result));
        }
        #endregion
    }
}