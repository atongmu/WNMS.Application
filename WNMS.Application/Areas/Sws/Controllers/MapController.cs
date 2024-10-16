using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDBHelper;
using NPOI.SS.Formula.Functions;
using WNMS.Application.Utility.Filters;
using WNMS.Application.Utility.JsonHelper;
using WNMS.IService;
using WNMS.Model.CustomizedClass;
using WNMS.Model.DataModels;
using WNMS.Utility;

namespace WNMS.Application.Areas.Sws.Controllers
{
    [Area("Sws")]
    public class MapController : Controller
    {
        private ISws_StationService _StationService = null;
        private ISysUserService _UserService = null;
        private ISws_EventInfoService _EventInfoService = null;
        private readonly IWebHostEnvironment _webHostEnvironment;
        int timevalue = int.Parse(StaticConstraint.TimeValue);
        public MapController(ISws_StationService sws_StationService,
            ISysUserService sysUserService, ISws_EventInfoService sws_EventInfoService,
             IWebHostEnvironment webHostEnvironment
           )
        {
            _StationService = sws_StationService;
            _UserService = sysUserService;
            _EventInfoService = sws_EventInfoService;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            var path = _webHostEnvironment.ContentRootPath;
            JsonFileHelper jsonFileHelper = new JsonFileHelper(path + "/Utility/ProjectJson/project.json");
            SystemInfo st = jsonFileHelper.Read<SystemInfo>("SystemInfo");
            ViewBag.lng = st.Lng;
            ViewBag.lat = st.Lat;
            ViewBag.zoom = st.MapLevel;
            ViewBag.AreaName = st.AreaName;
            return View();
        }

        //泵房数量数据
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult QueryStationNum()
        {
            //参数定义
            string alarmSids = "", onlinertuIds = "";
            int allNum = 0, alarmNum = 0, onlineNum = 0, offLineNum = 0, attentionNum = 0, deviceNum = 0, onlineNum1 = 0;
            //用户
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = _UserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
            #region 各个数量
            //GetAllALarmStaion(user, ref alarmSids);//获取报警泵房相关数据
            //GetOnlineRtuID(user.IsAdmin, user.UserId, ref onlinertuIds);//在线rtuids
            //GetOnlineRtuIDAndAlarmSID(user, ref alarmSids, 1, ref onlinertuIds);
            #region 在线设备查询
            GetOnlineRtuID(user.IsAdmin, user.UserId, ref onlinertuIds);//在线rtuids
            var ids_rtuonline = new List<string>(onlinertuIds.Split(",")).ConvertAll(r => int.Parse(r));
            DataTable tvpDt = new DataTable();
            tvpDt.Columns.Add("RTUID", typeof(int));
            foreach (var item in ids_rtuonline)
            {
                tvpDt.Rows.Add(item);
            }
            var query = _StationService.GetSationNumMany_tvp(tvpDt, "", userID, user.IsAdmin, 1).FirstOrDefault();
            var zquery = _StationService.GetSationNumMany_tvp(tvpDt, "", userID, user.IsAdmin, 2).FirstOrDefault();
            #endregion

            //var query = _StationService.GetSationNumMap(alarmSids, onlinertuIds, userID, user.IsAdmin).FirstOrDefault();
            allNum = query.allNum + zquery.allNum;
            alarmNum = query.alarmNum + zquery.alarmNum;
            onlineNum = query.onlineNum + zquery.onlineNum;
            //onlineNum1 = query.onlineNum1;
            offLineNum = allNum - alarmNum - onlineNum;
            //attentionNum = query.attentionNum;
            var deviceNum1 = _StationService.GetDeviceNumMap(user.IsAdmin, user.UserId).FirstOrDefault();
            deviceNum = deviceNum1.ty1 + deviceNum1.ty2;
            #endregion
            return Json(new
            {
                allNum = allNum,
                alarmNum = alarmNum,
                onlineNum = onlineNum + onlineNum1,
                offLineNum = offLineNum,
                deviceNum = deviceNum,

            });
        }

        //获取在线rtid，报警泵房id
        public void GetOnlineRtuIDAndAlarmSID(SysUser u, ref string alarmSids, int stationtype, ref string onlinertuIds)
        {
            var offtime = DateTime.Now.AddMinutes(-timevalue);//掉线时间
            string rtuids = "";
            if (u.IsAdmin == false)
            {
                var rtuidlist = _StationService.GetRtuIDByUserIDMap(u.UserId);
                if (rtuidlist.Count() > 0)
                {
                    rtuids = string.Join(",", rtuidlist.Select(r => r.RTUID));
                }
                else
                {
                    rtuids = "0";
                }
            }
            string macJson = "";
            if (rtuids != "")
            {
                macJson = "{'RTUID':{'$in':[" + rtuids + "]},'DigitalValues.1001':true,'DigitalValues.1000':true,'UpdateTime':{'$gte':ISODate('" + offtime.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'") + "')}}";
            }
            else
            {
                macJson = "{'DigitalValues.1001':true,'DigitalValues.1000':true,'UpdateTime':{'$gte':ISODate('" + offtime.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'") + "')}}";
            }
            var collections = new MongoDBHelper<RtuJKInfo>(DateTime.Now.Year.ToString());
            var datalist = collections.Find("Sws_DeviceJKInfo", macJson, "UpdateTime").Select(r => r.RTUID).ToList();

            onlinertuIds = datalist.Count > 0 ? string.Join(",", datalist) : "";//在线的rtuid字符串
            //测试onlinertuIds=“”；

            var datalist_Alarm = _StationService.GetAlarmStion_New(u.IsAdmin, u.UserId, 1, onlinertuIds);
            var datalist_ZAlarm = _StationService.GetAlarmZStion_New(u.IsAdmin, u.UserId, 2, onlinertuIds);
            List<dynamic> ids = new List<dynamic>();
            var eid = datalist_Alarm.Select(r => r.StationID).ToList();
            var zid = datalist_ZAlarm.Select(r => r.StationID).ToList();
            ids = eid.Union(zid).ToList();
            if (ids.Count() > 0)
            {
                alarmSids = string.Join(",", ids);
            }
            //if (datalist.Count() > 0)
            //{
            //    alarmSids = string.Join(",", datalist_Alarm.Select(r => r.StationID));
            //}
        }

        #region 二供泵房数据
        //泵房数据
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult QueryTableData(string typecheck, string stationName)
        {
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = _UserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
            string alarmSids = "", onlinertuIds = "";
            if (stationName == null)
            {
                stationName = "";
            }
            //跳转第一个坐标点
            double toLat = 0, toLng = 0;
            #region
            //GetALarmStaion(user, ref alarmSids);//获取报警泵房相关数据
            //GetOnlineRtuID(user.IsAdmin, user.UserId, ref onlinertuIds);//在线rtuids
            //GetOnlineRtuIDAndAlarmSID(user, ref alarmSids, 1, ref onlinertuIds);
            GetOnlineRtuIDNew(user.IsAdmin, user.UserId, 1, ref onlinertuIds);
            var ids_rtuonline = new List<string>(onlinertuIds.Split(",")).ConvertAll(r => int.Parse(r));
            DataTable tvpDt = new DataTable();
            tvpDt.Columns.Add("RTUID", typeof(int));
            foreach (var item in ids_rtuonline)
            {
                tvpDt.Rows.Add(item);
            }
            #endregion
            //var dataList = _StationService.GetStaionDataMap("all", onlinertuIds, alarmSids, user.IsAdmin, user.UserId, stationName);
            var dataList = _StationService.GetStaionDataByMap_tvp(tvpDt, 1, "all", stationName, user.IsAdmin, user.UserId);

            List<StationMapState> result = new List<StationMapState>() { };
            if (dataList.Count() > 0)
            {
                var rtuidlist = dataList.Where(r => r.RTUID.ToString() != "").Select(r => r.RTUID).Distinct();
                var rtuids = string.Join(",", rtuidlist);
                string macJson = "{\"RTUID\":{'$in':[" + rtuids + "]}}";
                var collections = new MongoDBHelper<RtuJKInfo>(DateTime.Now.Year.ToString());
                var jklist = collections.Find("Sws_DeviceJKInfo", macJson, "UpdateTime");
                var stationdata = dataList.GroupBy(r => new { r.StationID, r.StationName, r.FocusOn, r.CameraMonitor, r.ControlMonitor, r.Lng, r.Lat }).OrderBy(r => r.Key.StationName);
                List<SwsEventInfo> eventlist_all = new List<SwsEventInfo>();
                if (rtuids != "")
                {
                    var rtuidlist_int = new List<string>(rtuids.Split(',')).ConvertAll(r => int.Parse(r));
                    eventlist_all = _EventInfoService.Query<SwsEventInfo>(r => rtuidlist_int.Contains(r.Rtuid) && r.EventLevel != 0).ToList(); ;//1代表供水datainfo
                }
                foreach (var item in stationdata)
                {
                    StationMapState jk = new StationMapState();
                    jk.StationName = item.Key.StationName;
                    jk.StationID = item.Key.StationID;
                    jk.deviceJKs = new List<mapmontorJK>() { };
                    jk.Lng = item.Key.Lng;
                    jk.Lat = item.Key.Lat;

                    toLat = item.Key.Lat;
                    toLng = item.Key.Lng;
                    double allInFlow = 0;
                    List<string> deviceState = new List<string>();
                    //所有设备
                    var devicelist = dataList.Where(r => r.StationID == item.Key.StationID && r.Partition != 6);
                    //var sortedAddresses = devicelist.OrderBy(address => Convert.ToInt32(address.Substring(0, 2))).ToList();
                    foreach (var it in devicelist)
                    {
                        if (it.RTUID.ToString() != "")
                        {
                            double timpan = 0;
                            int Rtuidd = Convert.ToInt32(it.RTUID);
                            var partion = Convert.ToInt32(it.Partition.ToString());
                            if (partion == 6)
                            {

                            }
                            mapmontorJK jkdata = new mapmontorJK();
                            var datajk = jklist.Where(r => r.RTUID == Rtuidd).FirstOrDefault();
                            #region 状态
                            var state = "离线";
                            var eventdata = eventlist_all.Where(r => r.Rtuid == Rtuidd).ToList();
                            if (datajk != null && datajk.DigitalValues != null)
                            {
                                bool zstate = datajk.DigitalValues.Keys.Contains("1001") ? Convert.ToBoolean(datajk.DigitalValues["1001"]) : false;
                                var tstate = datajk.DigitalValues.Keys.Contains("1000") ? Convert.ToBoolean(datajk.DigitalValues["1000"]) : false;
                                if (zstate == false || tstate == false || (DateTime.Now - Convert.ToDateTime(datajk.UpdateTime)).TotalMinutes > timevalue)
                                {
                                    state = "离线";
                                }
                                else
                                {
                                    if (zstate == true && tstate == true && (DateTime.Now - Convert.ToDateTime(datajk.UpdateTime)).TotalMinutes <= timevalue && eventdata.Count() == 0)
                                    {
                                        jk.UpdateTime = datajk.UpdateTime.ToString();
                                        state = "正常";
                                    }
                                    else
                                    {
                                        jk.UpdateTime = datajk.UpdateTime.ToString();
                                        state = "故障";
                                        eventdata = eventdata.Union(eventdata).ToList();
                                    }

                                }

                            }
                            else
                            {
                                state = "离线";
                            }
                            deviceState.Add(state);
                            if (datajk != null)
                            {
                                var updatetime = datajk.UpdateTime;
                                var timeSpan = (updatetime - DateTime.Now).TotalMinutes;
                                if (timeSpan < timpan)
                                {
                                    //jk.UpdateTime = updatetime.ToString();
                                    timpan = timeSpan;
                                }
                                jkdata.Partition = it.Partition;
                                //出水压力
                                int pressout = 2001 + (partion - 1) * 500;
                                jkdata.PressOut = new KeyValuePair<string, bool>(datajk.AnalogValues.ContainsKey(pressout.ToString()) ? datajk.AnalogValues[pressout.ToString()].ToString() : "--",
                                eventdata.Count > 0 ? (eventdata.Where(r => r.EventSource == pressout && r.EventLevel != 0).Count() > 0 ? true : false) : false);

                                #endregion
                                //瞬时流量
                                var infdata1 = datajk.AnalogValues.ContainsKey((2030 + (partion - 1) * 500).ToString()) ? datajk.AnalogValues[(2030 + (partion - 1) * 500).ToString()] : null;
                                var infdata2 = datajk.AnalogValues.ContainsKey((2032 + (partion - 1) * 500).ToString()) ? datajk.AnalogValues[(2032 + (partion - 1) * 500).ToString()] : null;
                                var infdata3 = datajk.AnalogValues.ContainsKey((2034 + (partion - 1) * 500).ToString()) ? datajk.AnalogValues[(2034 + (partion - 1) * 500).ToString()] : null;
                                var infdata4 = datajk.AnalogValues.ContainsKey((2036 + (partion - 1) * 500).ToString()) ? datajk.AnalogValues[(2036 + (partion - 1) * 500).ToString()] : null;
                                if (infdata1 == null && infdata2 == null && infdata3 == null && infdata4 == null)
                                {

                                }
                                else
                                {
                                    var inflowtemp = Math.Round(Convert.ToDouble(infdata1) + Convert.ToDouble(infdata2) + Convert.ToDouble(infdata3) + Convert.ToDouble(infdata4), 2);

                                    allInFlow += inflowtemp;
                                }
                            }
                            else
                            {
                                jkdata.datetime = "--";//更新时间
                                jkdata.RtuID = Rtuidd;
                                jkdata.State = state;
                                jkdata.PressIN = new KeyValuePair<string, bool>("--", false);
                                jkdata.PressOut = new KeyValuePair<string, bool>("--", false);
                                jkdata.inflow = "--";
                            }

                            jk.deviceJKs.Add(jkdata);
                        }
                    }
                    jk.allInFlow = Math.Round(allInFlow, 2);
                    if (deviceState.Contains("故障"))
                    {
                        jk.State = "故障";//泵房状态 
                    }
                    else if (!deviceState.Contains("故障") && deviceState.Contains("正常"))
                    {
                        jk.State = "正常";//泵房状态 
                    }
                    else
                    {
                        jk.State = "离线";//泵房状态 
                    }
                    jk.UpdateTime = jk.UpdateTime == null ? "--" : jk.UpdateTime;//更新时间 
                    result.Add(jk);
                }

            }
            result = result.Where(r => typecheck.Contains(r.State)).OrderByDescending(t => t.UpdateTime).ToList();
            
            return Json(new
            {
                dataTable = result,
                toLat,
                toLng
            });
        }
        //在线rtuid
        public void GetOnlineRtuIDNew(bool isadmin, int userid, int stationtype, ref string onlinertuIds)
        {

            var offtime = DateTime.Now.AddMinutes(-timevalue);//掉线时间
            string macJson = "";
            macJson = "{'DigitalValues.1001':true,'DigitalValues.1000':true,'UpdateTime':{'$gte':ISODate('" + offtime.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'") + "')}}";

            var collections = new MongoDBHelper<RtuJKInfo>(DateTime.Now.Year.ToString());
            var datalist = collections.Find("Sws_DeviceJKInfo", macJson, "UpdateTime").Select(r => r.RTUID).ToList();
            if (isadmin == false)
            {
                var rtuidlist = _StationService.GetRtuIDByUserID(userid, stationtype).Select(r => r.RTUID);
                if (rtuidlist.Count() > 0)
                {
                    var rtuids = string.Join(",", rtuidlist);
                    var rtuid_List = new List<string>(rtuids.Split(",")).ConvertAll(r => int.Parse(r));
                    datalist = datalist.Intersect(rtuid_List).ToList();
                }
                else
                {
                    datalist = new List<int>() { };
                }

            }
            onlinertuIds = datalist.Count > 0 ? string.Join(",", datalist) : "-1";//在线的rtuid字符串
        }
        //全部报警泵房id(泵房下只要有一个设备报警该泵房就是报警泵房)
        [TypeFilter(typeof(IgonreActionFilter))]
        public void GetAllALarmStaion(SysUser u, ref string alarmSids)
        {
            var datalist = _StationService.GetAllAlarmStionMap(u.IsAdmin, u.UserId);
            if (datalist.Count() > 0)
            {
                alarmSids = string.Join(",", datalist.Select(r => r.StationID));
            }
        }
        #region 泵房各状态数量
        //报警泵房id(泵房下只要有一个设备报警该泵房就是报警泵房)
        public void GetALarmStaion(SysUser u, ref string alarmSids)
        {
            var datalist = _StationService.GetAlarmStionMap(u.IsAdmin, u.UserId);
            if (datalist.Count() > 0)
            {
                alarmSids = string.Join(",", datalist.Select(r => r.StationID));
            }
        }

        //在线rtuid
        public void GetOnlineRtuID(bool isadmin, int userid, ref string onlinertuIds)
        {
            var offtime = DateTime.Now.AddMinutes(-timevalue);//掉线时间
            string macJson = "";
            macJson = "{'DigitalValues.1001':true,'DigitalValues.1000':true,'UpdateTime':{'$gte':ISODate('" + offtime.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'") + "')}}";

            var collections = new MongoDBHelper<RtuJKInfo>(DateTime.Now.Year.ToString());
            var datalist = collections.Find("Sws_DeviceJKInfo", macJson, "UpdateTime").Select(r => r.RTUID).ToList();
            if (isadmin == false)
            {
                var rtuidlist = _StationService.GetRtuIDByUserIDMap(userid).Select(r => r.RTUID);
                if (rtuidlist.Count() > 0)
                {
                    var rtuids = string.Join(",", rtuidlist);
                    var rtuid_List = new List<string>(rtuids.Split(",")).ConvertAll(r => int.Parse(r));
                    datalist = datalist.Intersect(rtuid_List).ToList();
                }
                else
                {
                    datalist = new List<int>() { };
                }

            }
            onlinertuIds = datalist.Count > 0 ? string.Join(",", datalist) : "-1";//在线的rtuid字符串
            //var offtime = DateTime.Now.AddMinutes(-timevalue);//掉线时间
            //string rtuids = "";
            //if (isadmin == false)
            //{
            //    var rtuidlist = _StationService.GetRtuIDByUserIDMap(userid).Distinct();
            //    if (rtuidlist.Count() > 0)
            //    {
            //        rtuids = string.Join(",", rtuidlist.Select(r => r.RTUID));
            //    }
            //    else
            //    {
            //        rtuids = "0";
            //    }
            //}
            //string macJson = "";
            //if (rtuids != "")
            //{
            //    macJson = "{'RTUID':{'$in':[" + rtuids + "]},'DigitalValues.1001':true,'DigitalValues.1000':true,'EventState':false,'UpdateTime':{'$gte':ISODate('" + offtime.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'") + "')}}";
            //}
            //else
            //{
            //    macJson = "{'DigitalValues.1001':true,'DigitalValues.1000':true,'EventState':false,'UpdateTime':{'$gte':ISODate('" + offtime.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'") + "')}}";
            //}
            //var collections = new MongoDBHelper<RtuJKInfo>(DateTime.Now.Year.ToString());
            //var datalist = collections.Find("Sws_DeviceJKInfo", macJson, "UpdateTime").Select(r => r.RTUID).ToList();
            //onlinertuIds = datalist.Count > 0 ? string.Join(",", datalist) : "";//在线的rtuid字符串
        }
        #endregion

        //获取单个泵房的监控信息
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult LoadDataByStationID(int id)
        {
            //获取泵房RTUID
            var dataList = _StationService.GetStaionById("", id);
            StationMarkerData jk = new StationMarkerData();
            List<string> deviceState = new List<string>();
            jk.deviceJKs = new List<mapmontorJK>() { };
            if (dataList.Count() > 0)
            {
                var rtuidlist = dataList.Where(r => r.RTUID.ToString() != "").Select(r => r.RTUID).Distinct();
                var rtuids = string.Join(",", rtuidlist);
                string macJson = "{\"RTUID\":{'$in':[" + rtuids + "]}}";
                var collections = new MongoDBHelper<RtuJKInfo>(DateTime.Now.Year.ToString());
                var jklist = collections.Find("Sws_DeviceJKInfo", macJson, "UpdateTime");
                var stationdata = dataList.GroupBy(r => new { r.StationID, r.StationName, r.Lng, r.Lat }).OrderBy(r => r.Key.StationName).FirstOrDefault();
                jk.StationName = stationdata.Key.StationName;
                jk.StationID = stationdata.Key.StationID;
                jk.Lat = stationdata.Key.Lat;
                jk.Lng = stationdata.Key.Lng;
                //所有设备
                var devicelist = stationdata.Where(r => r.StationID == id && r.Partition != 6);
                foreach (var it in devicelist)
                //foreach (var it in stationdata)
                {

                    if (it.RTUID.ToString() != "")
                    {
                        int Rtuidd = Convert.ToInt32(it.RTUID);
                        mapmontorJK jkdata = new mapmontorJK();
                        var partion = Convert.ToInt32(it.Partition.ToString());
                        jkdata.devicename = it.DeviceName.ToString();
                        jkdata.DeviceID = long.Parse(it.DeviceID.ToString());
                        var datajk = jklist.Where(r => r.RTUID == Rtuidd).FirstOrDefault();
                        #region 状态
                        var state = "离线";
                        var eventdata = _EventInfoService.Query<SwsEventInfo>(r => r.Rtuid == Rtuidd && r.EventLevel != 0).ToList();
                        if (datajk != null && datajk.DigitalValues != null)
                        {
                            bool zstate = datajk.DigitalValues.Keys.Contains("1001") ? Convert.ToBoolean(datajk.DigitalValues["1001"]) : false;
                            var tstate = datajk.DigitalValues.Keys.Contains("1000") ? Convert.ToBoolean(datajk.DigitalValues["1000"]) : false;
                            if (zstate == false || tstate == false || (DateTime.Now - Convert.ToDateTime(datajk.UpdateTime)).TotalMinutes > timevalue)
                            {
                                state = "离线";
                            }
                            else
                            {
                                if (zstate == true && tstate == true && (DateTime.Now - Convert.ToDateTime(datajk.UpdateTime)).TotalMinutes <= timevalue && eventdata.Count() == 0)
                                {
                                    state = "正常";
                                }
                                else
                                {
                                    state = "故障";
                                    eventdata = eventdata.Union(eventdata).ToList();
                                }

                            }

                        }
                        else
                        {
                            state = "离线";
                        }
                        //if (eventdata.Count() > 0)
                        //{
                        //    state = "故障";
                        //}
                        //else
                        //{
                        //    if (datajk != null && datajk.DigitalValues != null)
                        //    {
                        //        bool zstate = datajk.DigitalValues.Keys.Contains("1001") ? Convert.ToBoolean(datajk.DigitalValues["1001"]) : false;
                        //        var tstate = datajk.DigitalValues.Keys.Contains("1000") ? Convert.ToBoolean(datajk.DigitalValues["1000"]) : false;

                        //        if (zstate == true && tstate == true && (DateTime.Now - Convert.ToDateTime(datajk.UpdateTime)).TotalMinutes <= timevalue)
                        //        {
                        //            state = "正常";
                        //        }
                        //        else
                        //        {
                        //            state = "离线";
                        //        }
                        //    }
                        //}
                        deviceState.Add(state);
                        #endregion 
                        jkdata.Partition = it.Partition;
                        if (datajk != null)
                        {
                            var updatetime = datajk.UpdateTime;
                            jkdata.datetime = updatetime.ToString();//更新时间
                            jkdata.DeviceID = it.DeviceID;
                            jkdata.RtuID = Rtuidd;
                            jkdata.State = state;
                            //jkdata.Partition = it.Partition;
                            //进水压力
                            int pressinid = 2000 + (partion - 1) * 500;
                            jkdata.PressIN = new KeyValuePair<string, bool>(datajk.AnalogValues.ContainsKey(pressinid.ToString()) ? datajk.AnalogValues[pressinid.ToString()].ToString() : "--",
                             eventdata.Count > 0 ? (eventdata.Where(r => r.EventSource == pressinid && r.EventLevel != 0).Count() > 0 ? true : false) : false);
                            //出水压力
                            int pressout = 2001 + (partion - 1) * 500;
                            jkdata.PressOut = new KeyValuePair<string, bool>(datajk.AnalogValues.ContainsKey(pressout.ToString()) ? datajk.AnalogValues[pressout.ToString()].ToString() : "--",
                            eventdata.Count > 0 ? (eventdata.Where(r => r.EventSource == pressout && r.EventLevel != 0).Count() > 0 ? true : false) : false);
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
                            jkdata.datetime = "--";//更新时间
                            jkdata.RtuID = Rtuidd;
                            jkdata.State = state;
                            jkdata.PressIN = new KeyValuePair<string, bool>("--", false);
                            jkdata.PressOut = new KeyValuePair<string, bool>("--", false);
                            jkdata.inflow = "--";
                        }
                        jk.deviceJKs.Add(jkdata);
                    }
                    else
                    {
                        mapmontorJK jkdata = new mapmontorJK();
                        jkdata.devicename = it.DeviceName.ToString();
                        jkdata.DeviceID = long.Parse(it.DeviceID.ToString()); ;
                        jkdata.datetime = "--";//更新时间
                        jkdata.RtuID = 0;
                        jkdata.State = "离线";
                        jkdata.PressIN = new KeyValuePair<string, bool>("--", false);
                        jkdata.PressOut = new KeyValuePair<string, bool>("--", false);
                        jkdata.inflow = "--";
                        jkdata.Partition = it.Partition;
                        jk.deviceJKs.Add(jkdata);

                    }
                }
                if (deviceState.Contains("故障"))
                {
                    jk.State = "故障";//泵房状态 
                }
                else if (!deviceState.Contains("故障") && deviceState.Contains("正常"))
                {
                    jk.State = "正常";//泵房状态 
                }
                else
                {
                    jk.State = "离线";//泵房状态 
                }
            }
            return Json(new
            {
                jk

            });
        }
        #endregion
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult QueryZTableData(string typecheck, string stationName)
        {
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = _UserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
            string alarmSids = "", onlinertuIds = "";
            //跳转第一个坐标点
            double toLat = 0, toLng = 0;
            if (stationName == null)
            {
                stationName = "";
            }
            #region
            //GetAlarmZStion(user, ref alarmSids, 2);//获取报警泵房相关数据
            //GetZOnlineRtuID(user.IsAdmin, user.UserId, 2, ref onlinertuIds);//在线rtuids
            //GetOnlineRtuIDAndAlarmSID(user, ref alarmSids, 1, ref onlinertuIds);
            GetZOnlineRtuID(user.IsAdmin, user.UserId, 2, ref onlinertuIds);//在线rtuids
            var ids_rtuonline = new List<string>(onlinertuIds.Split(",")).ConvertAll(r => int.Parse(r));
            DataTable tvpDt = new DataTable();
            tvpDt.Columns.Add("RTUID", typeof(int));
            foreach (var item in ids_rtuonline)
            {
                tvpDt.Rows.Add(item);
            }
            #endregion
            //var dataList = _StationService.GetZStaionDataMap("all", onlinertuIds, alarmSids, user.IsAdmin, user.UserId, stationName);
            var dataList = _StationService.GetStaionDataByMap_tvp(tvpDt, 2, "all", stationName, user.IsAdmin, user.UserId);
            List<ZStationMapJkInfo> result = new List<ZStationMapJkInfo>() { };
            if (dataList.Count() > 0)
            {
                var rtuidlist = dataList.Where(r => r.RTUID.ToString() != "").Select(r => r.RTUID).Distinct();
                var rtuids = string.Join(",", rtuidlist);
                string macJson = "{\"RTUID\":{'$in':[" + rtuids + "]}}";
                var collections = new MongoDBHelper<RtuJKInfo>(DateTime.Now.Year.ToString());
                var jklist = collections.Find("Sws_DeviceJKInfo", macJson, "UpdateTime");
                var stationdata = dataList.GroupBy(r => new { r.StationID, r.StationName, r.FocusOn, r.Lng, r.Lat }).OrderBy(r => r.Key.StationName);
                List<SwsEventInfo> eventlist_all = new List<SwsEventInfo>();
                if (rtuids != "")
                {
                    var rtuidlist_int = new List<string>(rtuids.Split(',')).ConvertAll(r => int.Parse(r));
                    eventlist_all = _EventInfoService.Query<SwsEventInfo>(r => rtuidlist_int.Contains(r.Rtuid) && r.EventLevel != 0).ToList(); ;//1代表供水datainfo
                }
                foreach (var item in stationdata)
                {
                    ZStationMapJkInfo jk = new ZStationMapJkInfo();
                    jk.StationName = item.Key.StationName;
                    jk.StationID = item.Key.StationID;
                    jk.deviceJKs = new List<zmapmontorJK>() { };
                    var StationState = "离线";//泵房状态
                    jk.Lng = item.Key.Lng;
                    jk.Lat = item.Key.Lat;
                    toLat = item.Key.Lat;
                    toLng = item.Key.Lng;
                    List<SwsEventInfo> eventlist = new List<SwsEventInfo>();
                    //所有设备
                    var devicelist = dataList.Where(r => r.StationID == item.Key.StationID);
                    foreach (var it in devicelist)
                    {
                        var partion = Convert.ToInt32(it.Partition.ToString());
                        if (it.RTUID.ToString() != "")
                        {
                            int Rtuidd = Convert.ToInt32(it.RTUID);

                            zmapmontorJK jkdata = new zmapmontorJK();
                            jkdata.Partition = partion;
                            jkdata.devicename = it.DeviceName.ToString();
                            var datajk = jklist.Where(r => r.RTUID == Rtuidd).FirstOrDefault();
                            #region 状态
                            var state = "离线";
                            double timpan = 0;
                            var eventdata = eventlist_all.Where(r => r.Rtuid == Rtuidd);
                            if (datajk != null && datajk.DigitalValues != null)
                            {
                                bool zstate = datajk.DigitalValues.Keys.Contains("1001") ? Convert.ToBoolean(datajk.DigitalValues["1001"]) : false;
                                var tstate = datajk.DigitalValues.Keys.Contains("1000") ? Convert.ToBoolean(datajk.DigitalValues["1000"]) : false;
                                if (zstate == false || tstate == false || (DateTime.Now - Convert.ToDateTime(datajk.UpdateTime)).TotalMinutes > timevalue)
                                {
                                    state = "离线";
                                }
                                else
                                {
                                    if (zstate == true && tstate == true && (DateTime.Now - Convert.ToDateTime(datajk.UpdateTime)).TotalMinutes <= timevalue && eventdata.Count() == 0)
                                    {
                                        state = "正常";
                                    }
                                    else
                                    {
                                        state = "故障";
                                        eventdata = eventdata.Union(eventdata).ToList();
                                    }

                                }

                            }
                            else
                            {
                                state = "离线";
                            }
                            //if (eventdata.Count() > 0)
                            //{
                            //    state = "故障";
                            //    eventlist = eventlist.Union(eventdata).ToList();


                            //}
                            //else
                            //{
                            //    if (datajk != null && datajk.DigitalValues != null)
                            //    {
                            //        bool zstate = datajk.DigitalValues.Keys.Contains("1001") ? Convert.ToBoolean(datajk.DigitalValues["1001"]) : false;
                            //        var tstate = datajk.DigitalValues.Keys.Contains("1000") ? Convert.ToBoolean(datajk.DigitalValues["1000"]) : false;

                            //        if (zstate == true && tstate == true && (DateTime.Now - Convert.ToDateTime(datajk.UpdateTime)).TotalMinutes <= timevalue)
                            //        {
                            //            state = "正常";
                            //        }
                            //        else
                            //        {
                            //            state = "离线";
                            //        }

                            //    }
                            //}
                            #endregion
                            if (datajk != null)
                            {
                                var updatetime = datajk.UpdateTime;
                                var timeSpan = (updatetime - DateTime.Now).TotalMinutes;
                                if (timeSpan < timpan)
                                {
                                    jk.UpdateTime = updatetime.ToString();
                                    timpan = timeSpan;
                                }
                                jkdata.datetime = updatetime.ToString();//更新时间  
                                jkdata.RtuID = Rtuidd;
                                jkdata.State = state;
                                //var partion = Convert.ToInt32(it.Partition.ToString());
                                //净水出水压力
                                int pressinid = 2014 + (partion - 1) * 500;
                                jkdata.JPressOut = new KeyValuePair<string, bool>(datajk.AnalogValues.ContainsKey(pressinid.ToString()) ? datajk.AnalogValues[pressinid.ToString()].ToString() : "--",
                                 eventlist.Count > 0 ? (eventlist.Where(r => r.EventSource == pressinid && r.EventLevel != 0 && r.Rtuid == Rtuidd).Count() > 0 ? true : false) : false);
                                //原水出水压力
                                int pressout = 2015 + (partion - 1) * 500;
                                jkdata.YPressOut = new KeyValuePair<string, bool>(datajk.AnalogValues.ContainsKey(pressout.ToString()) ? datajk.AnalogValues[pressout.ToString()].ToString() : "--",
                                eventlist.Count > 0 ? (eventlist.Where(r => r.EventSource == pressout && r.EventLevel != 0 && r.Rtuid == Rtuidd).Count() > 0 ? true : false) : false);
                                //浊度
                                int turid = 2034;
                                jkdata.Turbidity = datajk.AnalogValues.ContainsKey(turid.ToString()) ? datajk.AnalogValues[turid.ToString()].ToString() : "--";
                                //余氯
                                int clid = 2033;
                                jkdata.CL = datajk.AnalogValues.ContainsKey(clid.ToString()) ? datajk.AnalogValues[clid.ToString()].ToString() : "--";
                                //ph
                                int tphid = 2032;
                                jkdata.PH = datajk.AnalogValues.ContainsKey(tphid.ToString()) ? datajk.AnalogValues[tphid.ToString()].ToString() : "--";

                            }
                            else
                            {
                                jkdata.JPressOut = new KeyValuePair<string, bool>("--", false);
                                jkdata.State = "离线";
                                jkdata.Partition = partion;
                                jkdata.CL = "--";
                                jkdata.PH = "--";
                                jkdata.Turbidity = "--";
                                jk.UpdateTime = jk.UpdateTime ?? "--";
                            }
                            jk.deviceJKs.Add(jkdata);
                        }
                        else
                        {
                            jk.UpdateTime = "--";
                            zmapmontorJK jkdata = new zmapmontorJK();
                            jkdata.JPressOut = new KeyValuePair<string, bool>("--", false);
                            jkdata.Partition = partion;
                            jkdata.State = "离线";
                            jkdata.CL = "--";
                            jkdata.PH = "--";
                            jkdata.Turbidity = "--";
                            jk.deviceJKs.Add(jkdata);
                        }
                    }
                    if (jk.deviceJKs.Where(r => r.State == "故障").Count() > 0)
                    {
                        StationState = "故障";
                    }
                    else if (jk.deviceJKs.Where(r => r.State == "正常").Count() > 0)
                    {
                        StationState = "正常";
                    }
                    jk.State = StationState;//泵房状态
                    result.Add(jk);
                }

            }
            result = result.Where(r => typecheck.Contains(r.State)).OrderByDescending(t => t.UpdateTime).ToList();
            return Json(new
            {
                dataTable = result,
                toLat,
                toLng
            });
        }
        //获取单个泵房的监控信息
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult LoadDataByZStationID(int id)
        {
            //获取泵房RTUID
            var dataList = _StationService.GetZStaionById("", id);
            ZStationMapJkInfo jk = new ZStationMapJkInfo();
            List<string> deviceState = new List<string>();
            jk.deviceJKs = new List<zmapmontorJK>() { };
            if (dataList.Count() > 0)
            {
                var rtuidlist = dataList.Where(r => r.RTUID.ToString() != "").Select(r => r.RTUID).Distinct();
                var rtuids = string.Join(",", rtuidlist);
                string macJson = "{\"RTUID\":{'$in':[" + rtuids + "]}}";
                var collections = new MongoDBHelper<RtuJKInfo>(DateTime.Now.Year.ToString());
                var jklist = collections.Find("Sws_DeviceJKInfo", macJson, "UpdateTime");
                var stationdata = dataList.GroupBy(r => new { r.StationID, r.StationName, r.Lng, r.Lat }).OrderBy(r => r.Key.StationName).FirstOrDefault();
                jk.StationName = stationdata.Key.StationName;
                jk.StationID = stationdata.Key.StationID;
                jk.Lat = stationdata.Key.Lat;
                jk.Lng = stationdata.Key.Lng;
                //所有设备
                var devicelist = stationdata.Where(r => r.StationID == id);
                foreach (var it in stationdata)
                {
                    var partion = Convert.ToInt32(it.Partition.ToString());
                    if (it.RTUID.ToString() != "")
                    {
                        int Rtuidd = Convert.ToInt32(it.RTUID);
                        zmapmontorJK jkdata = new zmapmontorJK();

                        jkdata.devicename = it.DeviceName.ToString();
                        var datajk = jklist.Where(r => r.RTUID == Rtuidd).FirstOrDefault();
                        #region 状态
                        var state = "离线";
                        var eventdata = _EventInfoService.Query<SwsEventInfo>(r => r.Rtuid == Rtuidd && r.EventLevel != 0).ToList();
                        if (datajk != null && datajk.DigitalValues != null)
                        {
                            bool zstate = datajk.DigitalValues.Keys.Contains("1001") ? Convert.ToBoolean(datajk.DigitalValues["1001"]) : false;
                            var tstate = datajk.DigitalValues.Keys.Contains("1000") ? Convert.ToBoolean(datajk.DigitalValues["1000"]) : false;
                            if (zstate == false || tstate == false || (DateTime.Now - Convert.ToDateTime(datajk.UpdateTime)).TotalMinutes > timevalue)
                            {
                                state = "离线";
                            }
                            else
                            {
                                if (zstate == true && tstate == true && (DateTime.Now - Convert.ToDateTime(datajk.UpdateTime)).TotalMinutes <= timevalue && eventdata.Count() == 0)
                                {
                                    state = "正常";
                                }
                                else
                                {
                                    state = "故障";
                                    eventdata = eventdata.Union(eventdata).ToList();
                                }

                            }

                        }
                        else
                        {
                            state = "离线";
                        }
                        //var state = "离线";
                        //var eventdata = _EventInfoService.Query<SwsEventInfo>(r => r.Rtuid == Rtuidd).ToList();
                        //if (datajk != null && datajk.DigitalValues != null)
                        //{
                        //    bool zstate = datajk.DigitalValues.Keys.Contains("1001") ? Convert.ToBoolean(datajk.DigitalValues["1001"]) : false;
                        //    var tstate = datajk.DigitalValues.Keys.Contains("1000") ? Convert.ToBoolean(datajk.DigitalValues["1000"]) : false;
                        //    if (zstate == false || tstate == false || (DateTime.Now - Convert.ToDateTime(datajk.UpdateTime)).TotalMinutes > timevalue)
                        //    {
                        //        state = "离线";
                        //    }
                        //    else
                        //    {
                        //        if (zstate == true && tstate == true && (DateTime.Now - Convert.ToDateTime(datajk.UpdateTime)).TotalMinutes <= timevalue && eventdata.Count() == 0)
                        //        {
                        //            state = "正常";
                        //        }
                        //        else
                        //        {
                        //            state = "故障";
                        //            eventdata = eventdata.Union(eventdata).ToList();
                        //        }

                        //    }

                        //}
                        //else
                        //{
                        //    state = "离线";
                        //}
                        //if (eventdata.Count() > 0)
                        //{
                        //    state = "故障";
                        //}
                        //else
                        //{
                        //    if (datajk != null && datajk.DigitalValues != null)
                        //    {
                        //        bool zstate = datajk.DigitalValues.Keys.Contains("1001") ? Convert.ToBoolean(datajk.DigitalValues["1001"]) : false;
                        //        var tstate = datajk.DigitalValues.Keys.Contains("1000") ? Convert.ToBoolean(datajk.DigitalValues["1000"]) : false;

                        //        if (zstate == true && tstate == true && (DateTime.Now - Convert.ToDateTime(datajk.UpdateTime)).TotalMinutes <= timevalue)
                        //        {
                        //            state = "正常";
                        //        }
                        //        else
                        //        {
                        //            state = "离线";
                        //        }
                        //    }
                        //}
                        deviceState.Add(state);
                        #endregion 
                        jkdata.Partition = it.Partition;
                        if (datajk != null)
                        {
                            var updatetime = datajk.UpdateTime;
                            jkdata.datetime = updatetime.ToString();//更新时间
                            jkdata.RtuID = Rtuidd;
                            jkdata.State = state;
                            //jkdata.Partition = it.Partition;
                            //var partion = Convert.ToInt32(it.Partition.ToString());
                            //净水出水压力
                            int pressinid = 2014 + (partion - 1) * 500;
                            jkdata.JPressOut = new KeyValuePair<string, bool>(datajk.AnalogValues.ContainsKey(pressinid.ToString()) ? datajk.AnalogValues[pressinid.ToString()].ToString() : "--",
                             eventdata.Count > 0 ? (eventdata.Where(r => r.EventSource == pressinid && r.EventLevel != 0 && r.Rtuid == Rtuidd).Count() > 0 ? true : false) : false);
                            //原水出水压力
                            int pressout = 2015 + (partion - 1) * 500;
                            jkdata.YPressOut = new KeyValuePair<string, bool>(datajk.AnalogValues.ContainsKey(pressout.ToString()) ? datajk.AnalogValues[pressout.ToString()].ToString() : "--",
                            eventdata.Count > 0 ? (eventdata.Where(r => r.EventSource == pressout && r.EventLevel != 0 && r.Rtuid == Rtuidd).Count() > 0 ? true : false) : false);
                            //浊度
                            int turid = 2034;
                            jkdata.Turbidity = datajk.AnalogValues.ContainsKey(turid.ToString()) ? datajk.AnalogValues[turid.ToString()].ToString() : "--";
                            //余氯
                            int clid = 2033;
                            jkdata.CL = datajk.AnalogValues.ContainsKey(clid.ToString()) ? datajk.AnalogValues[clid.ToString()].ToString() : "--";
                            //ph
                            int tphid = 2032;
                            jkdata.PH = datajk.AnalogValues.ContainsKey(tphid.ToString()) ? datajk.AnalogValues[tphid.ToString()].ToString() : "--";
                        }
                        else
                        {
                            jkdata.JPressOut = new KeyValuePair<string, bool>("--", false);
                            jkdata.Partition = partion;
                            jkdata.CL = "--";
                            jkdata.PH = "--";
                            jkdata.Turbidity = "--";


                            jkdata.datetime = "--";//更新时间
                            jkdata.RtuID = Rtuidd;
                            jkdata.State = state;
                        }
                        jk.deviceJKs.Add(jkdata);
                    }
                    else
                    {
                        zmapmontorJK jkdata = new zmapmontorJK();
                        jkdata.JPressOut = new KeyValuePair<string, bool>("--", false);
                        jkdata.Partition = partion;
                        jkdata.State = "离线";
                        jkdata.CL = "--";
                        jkdata.PH = "--";
                        jkdata.Turbidity = "--";
                        jkdata.datetime = "--";//更新时间
                        jk.deviceJKs.Add(jkdata);
                        jk.deviceJKs.Add(jkdata);

                    }
                }
                if (deviceState.Contains("故障"))
                {
                    jk.State = "故障";//泵房状态 
                }
                else if (!deviceState.Contains("故障") && deviceState.Contains("正常"))
                {
                    jk.State = "正常";//泵房状态 
                }
                else
                {
                    jk.State = "离线";//泵房状态 
                }
            }
            return Json(new
            {
                jk

            });
        }
        #region 直饮水泵房各状态数量
        //报警泵房id(泵房下只要有一个设备报警该泵房就是报警泵房)
        [TypeFilter(typeof(IgonreActionFilter))]
        public void GetAlarmZStion(SysUser u, ref string alarmSids, int stationtype)
        {
            var datalist = _StationService.GetAlarmZStion(u.IsAdmin, u.UserId, stationtype);
            if (datalist.Count() > 0)
            {
                alarmSids = string.Join(",", datalist.Select(r => r.StationID));
            }
        }

        //在线rtuid
        [TypeFilter(typeof(IgonreActionFilter))]
        public void GetZOnlineRtuID(bool isadmin, int userid, int stationtype, ref string onlinertuIds)
        {
            var offtime = DateTime.Now.AddMinutes(-timevalue);//掉线时间
            string macJson = "";
            macJson = "{'DigitalValues.1001':true,'DigitalValues.1000':true,'UpdateTime':{'$gte':ISODate('" + offtime.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'") + "')}}";

            var collections = new MongoDBHelper<RtuJKInfo>(DateTime.Now.Year.ToString());
            var datalist = collections.Find("Sws_DeviceJKInfo", macJson, "UpdateTime").Select(r => r.RTUID).ToList();
            if (isadmin == false)
            {
                var rtuidlist = _StationService.GetZRtuIDByUserID(userid, stationtype).Select(r => r.RTUID);
                if (rtuidlist.Count() > 0)
                {
                    var rtuids = string.Join(",", rtuidlist);
                    var rtuid_List = new List<string>(rtuids.Split(",")).ConvertAll(r => int.Parse(r));
                    datalist = datalist.Intersect(rtuid_List).ToList();
                }
                else
                {
                    datalist = new List<int>() { };
                }

            }
            onlinertuIds = datalist.Count > 0 ? string.Join(",", datalist) : "0";//在线的rtuid字符串
            //var offtime = DateTime.Now.AddMinutes(-timevalue);//掉线时间
            //string rtuids = "";
            //if (isadmin == false)
            //{
            //    var rtuidlist = _StationService.GetZRtuIDByUserID(userid, stationtype);
            //    if (rtuidlist.Count() > 0)
            //    {
            //        rtuids = string.Join(",", rtuidlist.Select(r => r.RTUID));
            //    }
            //    else
            //    {
            //        rtuids = "0";
            //    }
            //}
            //string macJson = "";
            //if (rtuids != "")
            //{
            //    macJson = "{'RTUID':{'$in':[" + rtuids + "]},'DigitalValues.1001':true,'DigitalValues.1000':true,'EventState':false,'UpdateTime':{'$gte':ISODate('" + offtime.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'") + "')}}";
            //}
            //else
            //{
            //    macJson = "{'DigitalValues.1001':true,'DigitalValues.1000':true,'EventState':false,'UpdateTime':{'$gte':ISODate('" + offtime.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'") + "')}}";
            //}
            //var collections = new MongoDBHelper<RtuJKInfo>(DateTime.Now.Year.ToString());
            //var datalist = collections.Find("Sws_DeviceJKInfo", macJson, "UpdateTime").Select(r => r.RTUID).ToList();
            //onlinertuIds = datalist.Count > 0 ? string.Join(",", datalist) : "";//在线的rtuid字符串
        }
        #endregion
        #region 直饮水泵房数据

        #endregion

    }
}