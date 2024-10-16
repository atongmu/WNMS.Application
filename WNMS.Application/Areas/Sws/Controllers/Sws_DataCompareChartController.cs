using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WNMS.Application.Utility.Filters;
using WNMS.IService;
using WNMS.Model.CustomizedClass;
using WNMS.Model.DataModels;

namespace WNMS.Application.Areas.Sws.Controllers
{
    [Area("Sws")]
    public class Sws_DataCompareChartController : Controller
    {
        #region 属性 构造函数
        private ISws_StationService stationService = null;
        private ISws_DeviceInfo01Service device01Service = null;
        private ISws_DeviceInfo02Service device02Service = null;
        private ISws_RTUInfoService rtuService = null;
        private ISws_DataInfoService dataInfoService = null;

        public Sws_DataCompareChartController(ISws_DeviceInfo01Service sws_DeviceInfo01Service, ISws_DeviceInfo02Service sws_DeviceInfo02Service,
            ISws_RTUInfoService sws_RTUInfoService, ISws_StationService sws_StationService, ISws_DataInfoService sws_DataInfoservice)
        {
            device01Service = sws_DeviceInfo01Service;
            device02Service = sws_DeviceInfo02Service;
            rtuService = sws_RTUInfoService;
            stationService = sws_StationService;
            dataInfoService = sws_DataInfoservice;
        }
        #endregion

        #region 初始化页面及树查询
        public IActionResult Index()
        {
            var trees = GetStationTree("");
            if (!string.IsNullOrWhiteSpace(trees))
            {
                ViewBag.TreeNodes = new HtmlString(trees);
            }
            else
            {
                ViewBag.TreeNodes = "[]";
            }

            ViewBag.DateTime = DateTime.Now.ToString("yyyy-MM-dd");
            ViewBag.Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            return View();
        }

        //获取数据监测的树
        public string GetStationTree(string stationName)
        {
            int userID = int.Parse(User.FindFirstValue("UserID"));
            List<TreeAction> stationTree = new List<TreeAction>();

            #region 获取泵房树
            List<int> stationIds = new List<int>();
            List<StationData> stationInfo = this.stationService.GetStationInfo(userID, stationName).ToList();
            if (stationInfo.Count > 0)
            {
                stationIds = stationInfo.Select(r => r.StationId).ToList();
                stationTree = stationInfo.Select(r => new TreeAction
                {
                    id = r.StationId,
                    pId = 0,
                    name = "<em class='iconfont icon-bengfang'></em>" + r.StationName,
                    icon = "",
                    @checked = false,
                    nocheck = true
                }).ToList();
            }

            #endregion

            #region 获取设备树
            List<TreeAction> device01Tree = new List<TreeAction>(); //设备类型1 树
            List<DeviceInfo01Info> device01 = this.device01Service.LoadInfoList(null, 10000, 1, "DeviceName", true).DataList.Where(r => stationIds.Contains(r.StationId)).OrderBy(r => r.Partition).ToList();
            if (device01.Count > 0)
            {
                device01Tree = device01.Select(r => new TreeAction
                {
                    id = r.DeviceId,
                    pId = r.StationId,
                    name = r.StationName + "#" + r.PartitionName,
                    icon = "",
                    @checked = false,
                    nocheck = true
                }).ToList();
                stationTree = stationTree.Concat<TreeAction>(device01Tree).Distinct(new TreeAreaCompare()).ToList();
            }
            #endregion

            #region 获取字典树
            List<SwsRtuinfo> rtuinfos = this.rtuService.Query<SwsRtuinfo>(r => stationIds.Contains(r.StationId)).ToList();
            foreach (var item in device01)
            {
                SwsRtuinfo rtu = rtuinfos.Where(r => r.Rtuid == item.RTUID).FirstOrDefault();
                if (rtu != null)
                {
                    //转成DataInfo
                    DPC.Config.DPCConfig dpc = new DPC.Config.DPCConfig();
                    if (rtu.PluginFile != null)
                    {
                        dpc = DPC.Config.DPCConfig.FromBinary(rtu.PluginFile);
                        var dpcitem = dpc.Instructions.Where(r => r.CmdType == DPC.CmdType.DpcCmdType.CommonQuery).ToList();

                        System.Text.RegularExpressions.Regex searchTerm = new System.Text.RegularExpressions.Regex(@"^-?[0-9]\d*$");
                        List<int> dataIDlist = new List<int>();
                        foreach (var dpi in dpcitem)
                        {
                            var datalist = dpi.Analogs.Where(r => searchTerm.IsMatch(r.Key)).Select(r => r.Key).ToList().ConvertAll(r => Convert.ToInt32(r));
                            string partition = dpi.Partition;
                            if (partition == "0")
                            {
                                foreach (var num in datalist)
                                {
                                    int id = num + 2000 + (item.Partition - 1) * 500;
                                    dataIDlist.Add(id);
                                }
                            }
                            else
                            {
                                string[] strPartition = partition.Split(',');
                                if (int.Parse(strPartition[0]) == (item.Partition - 1) * 500 + 2000)
                                {
                                    foreach (var num in datalist)
                                    {
                                        int id = num + 2000 + (item.Partition - 1) * 500;
                                        dataIDlist.Add(id);
                                    }
                                }
                            }
                        }
                        List<SwsDataInfo> sws_DataInfo = new List<SwsDataInfo>();
                        sws_DataInfo = dataInfoService.Query<SwsDataInfo>(r => dataIDlist.Contains(r.DataId) && r.DeviceType == 1 && !r.Cnname.Contains("风机")).ToList();
                        if (sws_DataInfo.Count > 0)
                        {
                            List<TreeAction> list = sws_DataInfo.Select(r => new TreeAction
                            {
                                id = r.DataId,
                                pId = item.DeviceId,
                                name = r.Cnname.Remove(0, r.Cnname.IndexOf("区") + 1),
                                @checked = false,
                                icon = ""
                            }).ToList();
                            stationTree = stationTree.Concat<TreeAction>(list).OrderBy(r => r.name).ToList();
                        }
                    }
                }
            }
            #endregion
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(stationTree);
            return json;
        }

        //设备树查询
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult SearchTree(string stationName)
        {
            var trees = GetStationTree(stationName);
            if (!string.IsNullOrWhiteSpace(trees))
            {
                var str = new HtmlString(trees);
                return Content(str.ToString());
            }
            else
            {
                return Content("[]");
            }
        }
        #endregion

        #region  监测点叠加查询
        /// <summary>
        /// 历史曲线点叠加数据查询（mongodb）
        /// </summary>
        /// <param name="date">区分小时，日值，月值</param>
        /// <param name="beginDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <param name="dataIDs">参数ID</param>
        /// <param name="equID">设备ID</param>
        /// <returns></returns>
        [TypeFilter(typeof(IgonreActionFilter))]
        public ActionResult GetMongoDBHistorychart(string date, string beginDate, string endDate, string deviceID)
        {
            List<DeviceDataID> devlist = new List<DeviceDataID>();
            //数据处理
            Dictionary<string, string> dic = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(deviceID))
            {
                devlist = JsonConvert.DeserializeObject<List<DeviceDataID>>(deviceID);
            }
            List<string> IDs = new List<string>();
            List<string> dataName = new List<string>();
            List<string> unitName = new List<string>();
            if (devlist.Count > 0)
            {
                List<IGrouping<long, DeviceDataID>> devgroup = devlist.GroupBy(r => r.DeviceID).ToList();
                List<string> keylist = new List<string>();
                foreach (var dev in devgroup)
                {
                    Dictionary<string, string[]> dicc = new Dictionary<string, string[]>();
                    long deviceId = dev.Key;
                    string stationName = "";
                    List<int> dataId = dev.Select(r => r.DataID).ToList();

                    int? stationID = device01Service.Query<SwsDeviceInfo01>(r => r.DeviceId == deviceId)?.Select(r => r.StationId).FirstOrDefault();
                    if (stationID != null)
                    {
                        SwsStation st = stationService.Query<SwsStation>(r => r.StationId == (int)stationID).FirstOrDefault();
                        stationName = st == null ? "" : st.StationName;
                    }

                    string year = Convert.ToDateTime(beginDate).Year.ToString();
                    //小时，日值，月值区分
                    string _id = "";
                    string time = "";
                    DateTime enddateflag = Convert.ToDateTime(endDate);
                    if (date == "小时")
                    {
                        _id = "{$subtract:[{$subtract:['$UpdateTime',new Date('1970-01-01')]},{$mod:[{$subtract:['$UpdateTime',new Date('1970-01-01')]}," +
                           "3600000]}]}";
                        time = "{$add:[new Date(-28800000),'$_id']}";
                    }
                    else if (date == "分钟")
                    {
                        _id = "{$subtract:['$UpdateTime',new Date('1970-01-01')]}";
                        time = "{$add:[new Date(-28800000),'$_id']}";
                    }
                    else
                    {
                        if (date == "日值")
                        {
                            endDate = Convert.ToDateTime(endDate).AddDays(2).ToString();
                            _id = "{'$dateToString':{'format':'%Y-%m-%d','date':'$UpdateTime'}}";
                            time = "'$_id'";
                        }
                        else
                        {
                            beginDate = beginDate + "-01";
                            enddateflag = Convert.ToDateTime(endDate + "-01");
                            if (enddateflag > DateTime.Now)
                            {
                                enddateflag = Convert.ToDateTime(DateTime.Now.Date.ToString("yyyy-MM") + "-01");
                            }
                            endDate = Convert.ToDateTime(endDate + "-01").AddMonths(2).ToString();
                            _id = "{'$dateToString':{'format':'%Y-%m','date':'$UpdateTime'}}";
                            time = "{$concat:['$_id','-01']}";
                        }
                    }

                    //查询模拟量字段处理
                    string _group = "";
                    string _project = "";
                    //long deviceId = Convert.ToInt64(deviceID);
                    SwsDeviceInfo01 deviceinfo = this.device01Service.Query<SwsDeviceInfo01>(r => r.DeviceId == deviceId).FirstOrDefault();


                    foreach (var item in dataId)
                    {
                        int dataID = Convert.ToInt32(item);
                        string datakey = item.ToString() + deviceId.ToString();
                        SwsDataInfo dataInfo = dataInfoService.Query<SwsDataInfo>(r => r.DataId == item && r.DataType == 1 && r.DeviceType == 1).FirstOrDefault();

                        if (dataInfo != null && (dataInfo.IsCumulation == false || dataInfo.IsCumulation == null))
                        {
                            string[] temp = new string[] { "0", stationName + dataInfo.Cnname, dataInfo.Unit, dataInfo.DataId.ToString() };//是否是累计量(否)，中文名称，单位
                            dicc.Add(datakey, temp);
                            _group += "'" + item + "':{$avg:'$AnalogValues." + item + "'},";
                            _project += "'" + item + "':1,";
                        }
                        if (dataInfo != null && dataInfo.IsCumulation == true)
                        {
                            string[] temp = new string[] { "1", stationName + dataInfo.Cnname, dataInfo.Unit, dataInfo.DataId.ToString() };//是否是累计量(是)，中文名称，单位
                            dicc.Add(datakey, temp);
                            _group += "'min" + item + "':{$min:'$AnalogValues." + item + "'},'max" + item + "':{$max:'$AnalogValues." + item + "'},";
                            _project += "'" + item + "':'$min" + item + "',";
                        }
                    }
                    string group = "{$group:{'_id':" + _id + "," + _group + "}}";
                    string project = "{$project:{'_id': 0," + _project + "'Time':" + time + "}}";

                    //数据查询
                    List<dynamic> chartsData = device01Service.GetHistoryChartData(year, beginDate, endDate, group, project, deviceinfo.Rtuid.ToString()).ToList();
                    if (Convert.ToDateTime(beginDate).Year != Convert.ToDateTime(endDate).Year)
                    {
                        string yearappend = Convert.ToDateTime(endDate).Year.ToString();
                        var dataappend = device01Service.GetHistoryChartData(yearappend, beginDate, endDate, group, project, deviceinfo.Rtuid.ToString()).ToList();
                        if (dataappend.Count > 0)
                        {
                            chartsData = chartsData.Union(dataappend).ToList();
                        }
                    }
                    if (date == "月值")
                    {
                        if (chartsData.Count > 0 && (Convert.ToDateTime(chartsData[chartsData.Count - 1].Time) < Convert.ToDateTime(endDate).AddMonths(-1)))
                        {
                            beginDate = chartsData[chartsData.Count - 1].Time;
                            _id = "{'$dateToString':{'format':'%Y-%m-%d','date':'$UpdateTime'}}";
                            time = "'$_id'";
                            group = "{$group:{'_id':" + _id + "," + _group + "}}";
                            project = "{$project:{'_id': 0," + _project + "'Time':" + time + "}}";
                            var chartsData1 = device01Service.GetHistoryChartData(Convert.ToDateTime(endDate).AddMonths(-2).Year.ToString(), beginDate, endDate, group, project, deviceinfo.Rtuid.ToString()).LastOrDefault();
                            if (chartsData1 != null)
                            {
                                chartsData.Add(chartsData1);
                            }
                        }
                    }

                    foreach (var item in dataId)
                    {
                        dic.Add(item.ToString() + deviceId.ToString(), "");
                        keylist.Add(item.ToString() + deviceId.ToString());
                    }

                    foreach (var item in dicc)
                    {
                        var idname = item.Value[3];
                        if (item.Value[0] == "1")//累计值
                        {
                            for (var i = 0; i < chartsData.Count - 1; i++)
                            {
                                var analog1 = (IDictionary<string, object>)(chartsData[i]);
                                var analog2 = (IDictionary<string, object>)(chartsData[i + 1]);
                                var data1 = analog1.Keys.Contains(idname) ? Convert.ToDouble(analog1[idname]) : 0.0;
                                var data2 = analog2.Keys.Contains(idname) ? Convert.ToDouble(analog2[idname]) : 0.0;
                                string timet = "";
                                if (date == "小时")
                                {
                                    timet = analog2.Keys.Contains("Time") ? Convert.ToString(analog2["Time"]) : "";
                                }
                                else
                                {
                                    timet = analog1.Keys.Contains("Time") ? Convert.ToString(analog1["Time"]) : "";
                                }
                                double cdata = data2 - data1;
                                double data = Math.Round((cdata < 0 ? 0.0 : cdata), 2);
                                dic[item.Key] += "{name:'" + timet + "',value:['" + timet + "'," + data + "]},";
                            }
                        }
                        else//瞬时值
                        {
                            for (var i = 0; i < chartsData.Count - 1; i++)
                            {
                                //2019-11-26 掉线添加空数据
                                var chart = Newtonsoft.Json.JsonConvert.SerializeObject(chartsData[i]);
                                var charts = Newtonsoft.Json.JsonConvert.DeserializeObject(chart);

                                var chart2 = Newtonsoft.Json.JsonConvert.SerializeObject(chartsData[i + 1]);
                                var charts2 = Newtonsoft.Json.JsonConvert.DeserializeObject(chart2);

                                if (Convert.ToDateTime(charts["Time"]) <= enddateflag)
                                {
                                    double? data = (charts[idname] == null || charts[idname] < 0) ? 0.0 : Math.Round((double)charts[idname], 2);
                                    dic[item.Key] += "{name:'" + charts["Time"] + "',value:['" + charts["Time"] + "'," + data + "]},";
                                    //TimeSpan interval = Convert.ToDateTime(charts2["Time"]) - Convert.ToDateTime(charts["Time"]);
                                    //var mm = interval.TotalSeconds;//两个日期的差值总秒数
                                }
                            }
                        }
                    }

                    foreach (var item in dicc)
                    {
                        IDs.Add(item.Key);
                        dataName.Add(item.Value[1] + "(" + item.Value[2] + ")");
                        unitName.Add(item.Value[2]);
                    }
                }
                foreach (var item in keylist)
                {
                    string key = item;
                    if (dic[key].Length > 1)
                    {
                        dic[key] = "[" + dic[key].Substring(0, dic[key].Length - 1) + "]";
                    }
                }
            }

            var rel = new
            {
                DataID = IDs,
                DataName = dataName,
                UnitName = unitName,
                data = dic
            };
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(rel));
        }

        //单条曲线
        [TypeFilter(typeof(IgonreActionFilter))]
        public ActionResult GetMongoDBSingle(string date, string beginDate, string endDate, string dataIDs, string deviceID)
        {
            long deviceId = Convert.ToInt64(deviceID);
            string stationName = "";
            int? stationID = device01Service.Query<SwsDeviceInfo01>(r => r.DeviceId == deviceId)?.Select(r => r.StationId).FirstOrDefault();
            if (stationID != null)
            {
                SwsStation st = stationService.Query<SwsStation>(r => r.StationId == (int)stationID).FirstOrDefault();
                stationName = st == null ? "" : st.StationName;
            }

            Dictionary<string, string[]> dicc = new Dictionary<string, string[]>();
            string[] dataId = dataIDs.Split(',');
            string year = Convert.ToDateTime(beginDate).Year.ToString();
            //小时，日值，月值区分
            string _id = "";
            string time = "";
            DateTime enddateflag = Convert.ToDateTime(endDate);
            if (date == "小时")
            {
                _id = "{$subtract:[{$subtract:['$UpdateTime',new Date('1970-01-01')]},{$mod:[{$subtract:['$UpdateTime',new Date('1970-01-01')]}," +
                   "3600000]}]}";
                time = "{$add:[new Date(-28800000),'$_id']}";
            }
            else if (date == "分钟")
            {
                //_id = "{$subtract:[{$subtract:['$UpdateTime',new Date('1970-01-01')]},{$mod:[{$subtract:['$UpdateTime',new Date('1970-01-01')]}," +
                //   "60000]}]}";
                _id = "{$subtract:['$UpdateTime',new Date('1970-01-01')]}";
                time = "{$add:[new Date(-28800000),'$_id']}";
            }
            else
            {
                if (date == "日值")
                {
                    endDate = Convert.ToDateTime(endDate).AddDays(2).ToString();
                    _id = "{'$dateToString':{'format':'%Y-%m-%d','date':'$UpdateTime'}}";
                    time = "'$_id'";
                }
                else
                {
                    beginDate = beginDate + "-01";
                    enddateflag = Convert.ToDateTime(endDate + "-01");
                    if (enddateflag > DateTime.Now)
                    {
                        enddateflag = Convert.ToDateTime(DateTime.Now.Date.ToString("yyyy-MM") + "-01");
                    }
                    endDate = Convert.ToDateTime(endDate + "-01").AddMonths(2).ToString();
                    _id = "{'$dateToString':{'format':'%Y-%m','date':'$UpdateTime'}}";
                    time = "{$concat:['$_id','-01']}";
                }
            }

            //查询模拟量字段处理
            string _group = "";
            string _project = "";

            SwsDeviceInfo01 deviceinfo = this.device01Service.Query<SwsDeviceInfo01>(r => r.DeviceId == deviceId).FirstOrDefault();


            foreach (var item in dataId)
            {
                int dataID = Convert.ToInt32(item);
                SwsDataInfo dataInfo = dataInfoService.Query<SwsDataInfo>(r => r.DataId == dataID && r.DataType == 1 && r.DeviceType == 1).FirstOrDefault();

                if (dataInfo != null && (dataInfo.IsCumulation == false || dataInfo.IsCumulation == null))
                {
                    string[] temp = new string[] { "0", stationName + dataInfo.Cnname, dataInfo.Unit, dataInfo.DataId.ToString() };//是否是累计量(否)，中文名称，单位
                    dicc.Add(dataInfo.DataId.ToString() + deviceID, temp);
                    _group += "'" + item + "':{$avg:'$AnalogValues." + item + "'},";
                    _project += "'" + item + "':1,";
                }
                if (dataInfo != null && dataInfo.IsCumulation == true)
                {
                    string[] temp = new string[] { "1", stationName + dataInfo.Cnname, dataInfo.Unit, dataInfo.DataId.ToString() };//是否是累计量(是)，中文名称，单位
                    dicc.Add(dataInfo.DataId.ToString() + deviceID, temp);
                    _group += "'min" + item + "':{$min:'$AnalogValues." + item + "'},'max" + item + "':{$max:'$AnalogValues." + item + "'},";
                    _project += "'" + item + "':'$min" + item + "',";
                }
            }
            string group = "{$group:{'_id':" + _id + "," + _group + "}}";
            string project = "{$project:{'_id': 0," + _project + "'Time':" + time + "}}";

            //数据查询
            List<dynamic> chartsData = device01Service.GetHistoryChartData(year, beginDate, endDate, group, project, deviceinfo.Rtuid.ToString()).ToList();
            if (Convert.ToDateTime(beginDate).Year != Convert.ToDateTime(endDate).Year)
            {
                string yearappend = Convert.ToDateTime(endDate).Year.ToString();
                var dataappend = device01Service.GetHistoryChartData(yearappend, beginDate, endDate, group, project, deviceinfo.Rtuid.ToString()).ToList();
                if (dataappend.Count > 0)
                {
                    chartsData = chartsData.Union(dataappend).ToList();
                }
            }
            if (date == "月值")
            {
                if (chartsData.Count > 0 && (Convert.ToDateTime(chartsData[chartsData.Count - 1].Time) < Convert.ToDateTime(endDate).AddMonths(-1)))
                {
                    beginDate = chartsData[chartsData.Count - 1].Time;
                    _id = "{'$dateToString':{'format':'%Y-%m-%d','date':'$UpdateTime'}}";
                    time = "'$_id'";
                    group = "{$group:{'_id':" + _id + "," + _group + "}}";
                    project = "{$project:{'_id': 0," + _project + "'Time':" + time + "}}";
                    var chartsData1 = device01Service.GetHistoryChartData(Convert.ToDateTime(endDate).AddMonths(-2).Year.ToString(), beginDate, endDate, group, project, deviceinfo.Rtuid.ToString()).LastOrDefault();
                    if (chartsData1 != null)
                    {
                        chartsData.Add(chartsData1);
                    }
                }
            }
            //数据处理
            Dictionary<string, string> dic = new Dictionary<string, string>();
            foreach (var item in dataId)
            {
                dic.Add(item + deviceID, "");
            }

            foreach (var item in dicc)
            {
                var idname = item.Value[3];
                if (item.Value[0] == "1")//累计值
                {
                    for (var i = 0; i < chartsData.Count - 1; i++)
                    {
                        var analog1 = (IDictionary<string, object>)(chartsData[i]);
                        var analog2 = (IDictionary<string, object>)(chartsData[i + 1]);
                        var data1 = analog1.Keys.Contains(idname) ? Convert.ToDouble(analog1[idname]) : 0.0;
                        var data2 = analog2.Keys.Contains(idname) ? Convert.ToDouble(analog2[idname]) : 0.0;
                        string timet = "";
                        if (date == "小时")
                        {
                            timet = analog2.Keys.Contains("Time") ? Convert.ToString(analog2["Time"]) : "";
                        }
                        else
                        {
                            timet = analog1.Keys.Contains("Time") ? Convert.ToString(analog1["Time"]) : "";
                        }
                        double cdata = data2 - data1;
                        double data = Math.Round((cdata < 0 ? 0.0 : cdata), 2);
                        dic[item.Key] += "{name:'" + timet + "',value:['" + timet + "'," + data + "]},";
                    }
                }
                else//瞬时值
                {
                    for (var i = 0; i < chartsData.Count - 1; i++)
                    {
                        //2019-11-26 掉线添加空数据
                        var chart = Newtonsoft.Json.JsonConvert.SerializeObject(chartsData[i]);
                        var charts = Newtonsoft.Json.JsonConvert.DeserializeObject(chart);

                        var chart2 = Newtonsoft.Json.JsonConvert.SerializeObject(chartsData[i + 1]);
                        var charts2 = Newtonsoft.Json.JsonConvert.DeserializeObject(chart2);

                        if (Convert.ToDateTime(charts["Time"]) <= enddateflag)
                        {
                            double? data = (charts[idname] == null || charts[idname] < 0) ? 0.0 : Math.Round((double)charts[idname], 2);
                            dic[item.Key] += "{name:'" + charts["Time"] + "',value:['" + charts["Time"] + "'," + data + "]},";
                        }
                    }
                }
            }
            foreach (var item in dataId)
            {
                string key = item + deviceID;
                if (dic[key].Length > 1)
                {
                    dic[key] = "[" + dic[key].Substring(0, dic[key].Length - 1) + "]";
                }
            }

            List<string> IDs = new List<string>();
            List<string> dataName = new List<string>();
            List<string> unitName = new List<string>();
            foreach (var item in dicc)
            {
                IDs.Add(item.Key);
                dataName.Add(item.Value[1] + "(" + item.Value[2] + ")");
                unitName.Add(item.Value[2]);
            }

            var rel = new
            {
                DataID = IDs,
                DataName = dataName,
                UnitName = unitName,
                data = dic
            };
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(rel));
        }
        #endregion
        
    }
}
