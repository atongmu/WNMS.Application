using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDBHelper;
using WNMS.Application.Utility;
using WNMS.Application.Utility.Filters;
using WNMS.IService;
using WNMS.Model.CustomizedClass;
using WNMS.Model.DataModels;
using WNMS.Utility;

namespace WNMS.Application.Areas.Sws.Controllers
{
    [Area("Sws")]
    public class Sws_ZStationJKController : Controller
    {
        private ISws_StationService _StationService = null;
        private ISysUserService _UserService = null;
        private ISws_EventInfoService _EventInfoService = null;
        private ISws_DataInfoService _DataInfoService = null;
        private ISys_DataItemDetailService _DataItemDetailService = null;
        private ISws_UserStationService _UserStationService = null;
        int timevalue = int.Parse(StaticConstraint.TimeValue);
        public Sws_ZStationJKController(ISws_StationService sws_StationService, ISysUserService sysUserService, ISws_EventInfoService sws_EventInfoService,
           ISws_DataInfoService sws_DataInfoService, ISys_DataItemDetailService sys_DataItemDetailService,
           ISws_UserStationService sws_UserStationService)
        {
            _UserStationService = sws_UserStationService;
            _StationService = sws_StationService;
            _UserService = sysUserService;
            _EventInfoService = sws_EventInfoService;
            _DataInfoService = sws_DataInfoService;
            _DataItemDetailService = sys_DataItemDetailService;

        }
        public IActionResult Index()
        {
            return View();
        }
        #region 泵房监控表格模式
        [TypeFilter(typeof(IgonreActionFilter))]
        public async Task<IActionResult> QuerySJKTableList(int pageindex, int pagesize, string searchText, string type)
        {
            var enumfrequency = (int)Model.CustomizedClass.Enum.变频分类;
            var ftype = _DataItemDetailService.Query<SysDataItemDetail>(r => r.FItemId == enumfrequency).ToList();
            int userID = int.Parse(User.FindFirstValue("UserID"));
            int totalcount = 0;
            var user = _UserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
            string  onlinertuIds = "";
            int allNum = 0, alarmNum = 0, onlineNum = 0, offLineNum = 0, attentionNum = 0;
            //获取供水泵房对应的innertype
            int stationtype = 0;
            var intypeid = (int)Model.CustomizedClass.Enum.泵房内置类型;
            var typemodel = _DataItemDetailService.Query<SysDataItemDetail>(r => r.FItemId == intypeid && r.ItemName.Contains("直饮水泵房")).FirstOrDefault();
            if (typemodel != null)
            {
                stationtype = int.Parse(typemodel.ItemValue);
            }
            else
            {
                stationtype = 2;
            }
            #region
            
            GetZOnlineRtuID(user.IsAdmin, user.UserId, stationtype, ref onlinertuIds);//在线rtuids
            var ids_rtuonline = new List<string>(onlinertuIds.Split(",")).ConvertAll(r => int.Parse(r));
            DataTable tvpDt = new DataTable();
            tvpDt.Columns.Add("RTUID", typeof(int));
            foreach (var item in ids_rtuonline)
            {
                tvpDt.Rows.Add(item);
            }

            var query = _StationService.GetSationNumMany_StationJK(tvpDt, searchText == null ? "" : searchText, userID, user.IsAdmin, stationtype).FirstOrDefault();
            allNum = query.allNum;
            alarmNum = query.alarmNum;
            onlineNum = query.onlineNum;
            offLineNum = query.allNum - query.alarmNum - query.onlineNum;
            attentionNum = query.attentionNum;
            #endregion
            var dataList = _StationService.GetStaionDataByPage_tvp(tvpDt, stationtype, type, searchText == null ? "" : searchText, pageindex, pagesize, user.IsAdmin, user.UserId, ref totalcount).ToList();
            List<ZStationJkInfo> result = new List<ZStationJkInfo>() { };
            if (dataList.Count() > 0)
            {
                var rtuidlist = dataList.Where(r => r.RTUID.ToString() != "").Select(r => r.RTUID).Distinct();
                var rtuids = string.Join(",", rtuidlist);
                string macJson = "{\"RTUID\":{'$in':[" + rtuids + "]}}";
                var collections = new MongoDBHelper<RtuJKInfo>(DateTime.Now.Year.ToString());
                var jklist = collections.Find("Sws_DeviceJKInfo", macJson, "UpdateTime");
                var stationdata = dataList.GroupBy(r => new { r.StationID, r.StationName, r.FocusOn, r.CameraMonitor, r.ControlMonitor }).OrderBy(r => r.Key.StationName);
                List<dynamic> eventlist_all = new List<dynamic>();
                if (rtuids != "")
                {
                   
                    eventlist_all = _EventInfoService.QueryEventByRtuID(rtuids, 2).ToList(); ;//1代表二供，2代表直饮水
                }
                foreach (var item in stationdata)
                {
                    ZStationJkInfo jk = new ZStationJkInfo();
                    jk.StationName = item.Key.StationName;
                    jk.StationID = item.Key.StationID;
                    jk.CameraMonitor = item.Key.CameraMonitor;
                    jk.ControlMonitor = item.Key.ControlMonitor;
                    jk.deviceJKs = new List<zmontorJK>() { };
                     var StationState = "离线";//泵房状态
                    List<dynamic> eventlist = new List<dynamic>();
                    //所有设备
                    var devicelist = dataList.Where(r => r.StationID == item.Key.StationID);
                    foreach (var it in devicelist)
                    {
                        
                        if (it.RTUID.ToString() != "")
                        {
                            int Rtuidd = Convert.ToInt32(it.RTUID);

                            zmontorJK jkdata = new zmontorJK();

                            jkdata.devicename = it.DeviceName.ToString();
                            jkdata.DeviceID = long.Parse(it.DeviceID.ToString());
                            var datajk = jklist.Where(r => r.RTUID == Rtuidd).FirstOrDefault();
                            #region 状态
                            var state = "离线";

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
                                        eventlist = eventlist.Union(eventdata).ToList();
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
                                jkdata.datetime = updatetime.ToString();//更新时间
                                jkdata.DeviceID = it.DeviceID;
                                jkdata.RtuID = Rtuidd;
                                jkdata.State = state;
                                var partion = Convert.ToInt32(it.Partition.ToString());
                                //净水出水压力
                                int pressinid = 2014 + (partion - 1) * 500;
                                jkdata.JPressOut = new KeyValuePair<string, bool>(datajk.AnalogValues.ContainsKey(pressinid.ToString()) ? datajk.AnalogValues[pressinid.ToString()].ToString() : "--",
                                 eventlist.Count > 0 ? (eventlist.Where(r => r.EventSource == pressinid  && r.Rtuid == Rtuidd).Count() > 0 ? true : false) : false);
                                //原水出水压力
                                int pressout = 2015 + (partion - 1) * 500;
                                jkdata.YPressOut = new KeyValuePair<string, bool>(datajk.AnalogValues.ContainsKey(pressout.ToString()) ? datajk.AnalogValues[pressout.ToString()].ToString() : "--",
                                eventlist.Count > 0 ? (eventlist.Where(r => r.EventSource == pressout  && r.Rtuid == Rtuidd).Count() > 0 ? true : false) : false);
                                //浊度
                                int turid = 2034;
                                jkdata.Turbidity = datajk.AnalogValues.ContainsKey(turid.ToString()) ? datajk.AnalogValues[turid.ToString()].ToString() : "--";
                                //余氯
                                int clid = 2033;
                                jkdata.CL = datajk.AnalogValues.ContainsKey(clid.ToString()) ? datajk.AnalogValues[clid.ToString()].ToString() : "--";
                                //ph
                                int tphid = 2032;
                                jkdata.PH = datajk.AnalogValues.ContainsKey(tphid.ToString()) ? datajk.AnalogValues[tphid.ToString()].ToString() : "--";
                                ////高压泵状态
                                //int gybid = 2008;
                                //jkdata.GPumpState = datajk.AnalogValues.ContainsKey(gybid.ToString()) ? datajk.AnalogValues[gybid.ToString()].ToString() : "--";
                                //原水箱液位
                                int ylevelid = 2016;
                                jkdata.YLevel = datajk.AnalogValues.ContainsKey(ylevelid.ToString()) ? datajk.AnalogValues[ylevelid.ToString()].ToString() : "--";
                                int jlevelid = 2017;
                                jkdata.JLevel = datajk.AnalogValues.ContainsKey(jlevelid.ToString()) ? datajk.AnalogValues[jlevelid.ToString()].ToString() : "--";
                                int jsid = 2011;
                                jkdata.JSetpressure = datajk.AnalogValues.ContainsKey(jsid.ToString()) ? datajk.AnalogValues[jsid.ToString()].ToString() : "--";
                                int cdid = 2023;
                                jkdata.Conductivity = datajk.AnalogValues.ContainsKey(cdid.ToString()) ? datajk.AnalogValues[cdid.ToString()].ToString() : "--";
                                int orpid = 2035;
                                jkdata.Orp = datajk.AnalogValues.ContainsKey(orpid.ToString()) ? datajk.AnalogValues[orpid.ToString()].ToString() : "--";
                                int slid = 2036;
                                jkdata.Salinity = datajk.AnalogValues.ContainsKey(slid.ToString()) ? datajk.AnalogValues[slid.ToString()].ToString() : "--";
                                int oxid = 2037;
                                jkdata.Oxygen = datajk.AnalogValues.ContainsKey(oxid.ToString()) ? datajk.AnalogValues[oxid.ToString()].ToString() : "--"; 
                            }
                            else
                            {
                                jkdata.datetime = "--";//更新时间
                                jkdata.RtuID = Rtuidd;
                                jkdata.State = state;
                                jkdata.JPressOut = new KeyValuePair<string, bool>("--", false);
                                jkdata.YPressOut = new KeyValuePair<string, bool>("--", false);
                                //浊度
                                jkdata.Turbidity = "--";
                                //余氯
                                jkdata.CL = "--";
                                //ph
                                jkdata.PH = "--";
                                jkdata.YLevel = "--";
                                jkdata.JLevel =  "--";
                                jkdata.JSetpressure =  "--";
                                jkdata.Conductivity =  "--";
                                jkdata.Orp = "--";
                                jkdata.Salinity =  "--";
                                jkdata.Oxygen = "--";
                                //jkdata.pumpdatas = new List<ZPumpData>();
                            }
                            jk.deviceJKs.Add(jkdata);
                        }
                        else
                        { 
                            zmontorJK jkdata = new zmontorJK();
                            jkdata.devicename = it.DeviceName.ToString();
                            jkdata.DeviceID = long.Parse(it.DeviceID.ToString());
                            jkdata.datetime = "--";//更新时间
                            jkdata.RtuID = 0;
                            jkdata.State = "离线";
                            jkdata.JPressOut = new KeyValuePair<string, bool>("--", false);
                            jkdata.YPressOut = new KeyValuePair<string, bool>("--", false);
                            //浊度
                            jkdata.Turbidity = "--";
                            //余氯
                            jkdata.CL = "--";
                            //ph
                            jkdata.PH = "--";
                          
                            jkdata.YLevel = "--";
                            jkdata.JLevel = "--";
                            jkdata.JSetpressure = "--";
                            jkdata.Conductivity = "--";
                            jkdata.Orp = "--";
                            jkdata.Salinity = "--";
                            jkdata.Oxygen = "--";
                            //jkdata.pumpdatas = new List<ZPumpData>();
                        
                            
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

                    jk.eventlist = eventlist.Select(r => new Eventinfo
                    {
                        ID = r.ID,
                        Rtuid = r.Rtuid,
                        EventMessage = r.EventMessage,
                        EventTime = r.EventTime,
                        CurrentValue = r.CurrentValue,
                        LimitValue = r.LimitValue,
                        Unit = r.Unit,
                        DataType = r.DataType,
                        RegionName = r.RegionName
                    }).Distinct(new eventdataCompare()).ToList();//事件集合去重
                    jk.State = StationState;//泵房状态
                    jk.Attention = Convert.ToBoolean(item.Key.FocusOn);//是否关注 
                    result.Add(jk);
                }

            }
            double TotalPage = Math.Ceiling((float)totalcount / (float)pagesize);

            PartialView("_ZStationJKTable_List", result.OrderByDescending(r=>r.Attention).ThenBy(r=>r.StationName));
            string dataTable = await ViewToString.RenderPartialViewToString(this, "_ZStationJKTable_List");

            return Json(new
            {
                TotalCount = totalcount,
                TotalPage = TotalPage,
                dataTable = dataTable,
                allNum = allNum,
                alarmNum = alarmNum,
                onlineNum = onlineNum,
                offLineNum = offLineNum,
                attentionNum = attentionNum,

            });
        }
        #endregion
        #region 泵房监控card模式
        [TypeFilter(typeof(IgonreActionFilter))]
        public async Task<IActionResult> QuerySJKTable_card(int pageindex, int pagesize, string searchText, string type)
        {
            var enumfrequency = (int)Model.CustomizedClass.Enum.变频分类;
            var ftype = _DataItemDetailService.Query<SysDataItemDetail>(r => r.FItemId == enumfrequency).ToList();
            int userID = int.Parse(User.FindFirstValue("UserID"));
            int totalcount = 0;
            var user = _UserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
            string  onlinertuIds = "";
            int allNum = 0, alarmNum = 0, onlineNum = 0, offLineNum = 0, attentionNum = 0;
            //获取供水泵房对应的innertype
            int stationtype = 0;
            var intypeid = (int)Model.CustomizedClass.Enum.泵房内置类型;
            var typemodel = _DataItemDetailService.Query<SysDataItemDetail>(r => r.FItemId == intypeid && r.ItemName.Contains("直饮水泵房")).FirstOrDefault();
            if (typemodel != null)
            {
                stationtype = int.Parse(typemodel.ItemValue);
            }
            else
            {
                stationtype = 2;
            }
            #region
           
            GetZOnlineRtuID(user.IsAdmin, user.UserId, stationtype, ref onlinertuIds);//在线rtuids
            var ids_rtuonline = new List<string>(onlinertuIds.Split(",")).ConvertAll(r => int.Parse(r));
            DataTable tvpDt = new DataTable();
            tvpDt.Columns.Add("RTUID", typeof(int));
            foreach (var item in ids_rtuonline)
            {
                tvpDt.Rows.Add(item);
            }

            var query = _StationService.GetSationNumMany_StationJK(tvpDt, searchText == null ? "" : searchText, userID, user.IsAdmin, stationtype).FirstOrDefault();
            allNum = query.allNum;
            alarmNum = query.alarmNum;
            onlineNum = query.onlineNum;
            offLineNum = query.allNum - query.alarmNum - query.onlineNum;
            attentionNum = query.attentionNum;
            #endregion

            var dataList = _StationService.GetStaionDataByPage_tvp(tvpDt, stationtype, type, searchText == null ? "" : searchText, pageindex, pagesize, user.IsAdmin, user.UserId, ref totalcount).ToList();
            List<ZStationJkInfo> result = new List<ZStationJkInfo>() { };
            if (dataList.Count() > 0)
            {
                var rtuidlist = dataList.Where(r => r.RTUID.ToString() != "").Select(r => r.RTUID).Distinct();
                var rtuids = string.Join(",", rtuidlist);

                string macJson = "{\"RTUID\":{'$in':[" + rtuids + "]}}";
                var collections = new MongoDBHelper<RtuJKInfo>(DateTime.Now.Year.ToString());
                var jklist = collections.Find("Sws_DeviceJKInfo", macJson, "UpdateTime");
                var stationdata = dataList.GroupBy(r => new { r.StationID, r.StationName, r.FocusOn, r.CameraMonitor, r.ControlMonitor }).OrderBy(r => r.Key.StationName);
                List<dynamic> eventlist_all = new List<dynamic>();
                if (rtuids != "")
                {
                    
                    eventlist_all = _EventInfoService.QueryEventByRtuID(rtuids, 2).ToList(); ;//1代表二供，2代表直饮水
                }
                foreach (var item in stationdata)
                {
                    ZStationJkInfo jk = new ZStationJkInfo();
                    jk.StationName = item.Key.StationName;
                    jk.StationID = item.Key.StationID;
                    jk.CameraMonitor = item.Key.CameraMonitor;
                    jk.ControlMonitor = item.Key.ControlMonitor;
                    jk.deviceJKs = new List<zmontorJK>() { };
                    var StationState = "离线";//泵房状态
                    List<dynamic> eventlist = new List<dynamic>();
                    //所有设备
                    var devicelist = dataList.Where(r => r.StationID == item.Key.StationID);
                    foreach (var it in devicelist)
                    {
                        
                        if (it.RTUID.ToString() != "")
                        {
                            int Rtuidd = Convert.ToInt32(it.RTUID);

                            zmontorJK jkdata = new zmontorJK();

                            jkdata.devicename = it.DeviceName.ToString();
                            jkdata.DeviceID = long.Parse(it.DeviceID.ToString());
                            var datajk = jklist.Where(r => r.RTUID == Rtuidd).FirstOrDefault();
                            #region 状态
                            var state = "离线";

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
                                        eventlist = eventlist.Union(eventdata).ToList();
                                    }

                                }

                            }
                            else
                            {
                                state = "离线";
                            }
                            #endregion
                            if (datajk != null)
                            {
                                var updatetime = datajk.UpdateTime;
                                jkdata.datetime = updatetime.ToString();//更新时间
                                jkdata.DeviceID = it.DeviceID;
                                jkdata.RtuID = Rtuidd;
                                jkdata.State = state;
                                var partion = Convert.ToInt32(it.Partition.ToString());
                                //净水出水压力
                                int pressinid = 2014 + (partion - 1) * 500;
                                jkdata.JPressOut = new KeyValuePair<string, bool>(datajk.AnalogValues.ContainsKey(pressinid.ToString()) ? datajk.AnalogValues[pressinid.ToString()].ToString() : "--",
                                 eventlist.Count > 0 ? (eventlist.Where(r => r.EventSource == pressinid && r.Rtuid == Rtuidd).Count() > 0 ? true : false) : false);
                                //原水出水压力
                                int pressout = 2015 + (partion - 1) * 500;
                                jkdata.YPressOut = new KeyValuePair<string, bool>(datajk.AnalogValues.ContainsKey(pressout.ToString()) ? datajk.AnalogValues[pressout.ToString()].ToString() : "--",
                                eventlist.Count > 0 ? (eventlist.Where(r => r.EventSource == pressout && r.Rtuid == Rtuidd).Count() > 0 ? true : false) : false);
                                //浊度
                                int turid = 2034;
                                jkdata.Turbidity = datajk.AnalogValues.ContainsKey(turid.ToString()) ? datajk.AnalogValues[turid.ToString()].ToString() : "--";
                                //余氯
                                int clid = 2033;
                                jkdata.CL = datajk.AnalogValues.ContainsKey(clid.ToString()) ? datajk.AnalogValues[clid.ToString()].ToString() : "--";
                                //ph
                                int tphid = 2032;
                                jkdata.PH = datajk.AnalogValues.ContainsKey(tphid.ToString()) ? datajk.AnalogValues[tphid.ToString()].ToString() : "--";
                                ////高压泵状态
                                //int gybid = 2008;
                                //jkdata.GPumpState = datajk.AnalogValues.ContainsKey(gybid.ToString()) ? datajk.AnalogValues[gybid.ToString()].ToString() : "--";
                                //原水箱液位
                                int ylevelid = 2016;
                                jkdata.YLevel = datajk.AnalogValues.ContainsKey(ylevelid.ToString()) ? datajk.AnalogValues[ylevelid.ToString()].ToString() : "--";
                                int jlevelid = 2017;
                                jkdata.JLevel = datajk.AnalogValues.ContainsKey(jlevelid.ToString()) ? datajk.AnalogValues[jlevelid.ToString()].ToString() : "--";
                                int jsid = 2011;
                                jkdata.JSetpressure = datajk.AnalogValues.ContainsKey(jsid.ToString()) ? datajk.AnalogValues[jsid.ToString()].ToString() : "--";
                                int cdid = 2023;
                                jkdata.Conductivity = datajk.AnalogValues.ContainsKey(cdid.ToString()) ? datajk.AnalogValues[cdid.ToString()].ToString() : "--";
                                int orpid = 2035;
                                jkdata.Orp = datajk.AnalogValues.ContainsKey(orpid.ToString()) ? datajk.AnalogValues[orpid.ToString()].ToString() : "--";
                                int slid = 2036;
                                jkdata.Salinity = datajk.AnalogValues.ContainsKey(slid.ToString()) ? datajk.AnalogValues[slid.ToString()].ToString() : "--";
                                int oxid = 2037;
                                jkdata.Oxygen = datajk.AnalogValues.ContainsKey(oxid.ToString()) ? datajk.AnalogValues[oxid.ToString()].ToString() : "--"; 
                            }
                            else
                            {

                                jkdata.datetime = "--";//更新时间
                                jkdata.RtuID = Rtuidd;
                                jkdata.State = state;
                                //浊度
                                jkdata.Turbidity = "--";
                                //余氯
                                jkdata.CL = "--";
                                //ph
                                jkdata.PH = "--";
                                //高压泵状态 
                                jkdata.GPumpState = "--";
                                jkdata.pumpState = new List<string>();
                                jkdata.pumpdatas = new List<ZPumpData>();
                                jkdata.YLevel = "--";
                                jkdata.JLevel = "--";
                                jkdata.JSetpressure = "--";
                                jkdata.Conductivity = "--";
                                jkdata.Orp = "--";
                                jkdata.Salinity = "--";
                                jkdata.Oxygen = "--";
                                for (var i = 0; i < 4; i++)
                                {

                                    jkdata.pumpState.Add("停止");
                                }
                            }
                            jk.deviceJKs.Add(jkdata);
                        }
                        else
                        {


                            zmontorJK jkdata = new zmontorJK();
                            jkdata.devicename = it.DeviceName.ToString();
                            jkdata.DeviceID = long.Parse(it.DeviceID.ToString());
                            jkdata.datetime = "--";//更新时间
                            jkdata.RtuID = 0;
                            jkdata.State = "离线";
                            jkdata.JPressOut = new KeyValuePair<string, bool>("--", false);
                            jkdata.YPressOut = new KeyValuePair<string, bool>("--", false);
                            //浊度
                            jkdata.Turbidity = "--";
                            //余氯
                            jkdata.CL = "--";
                            //ph
                            jkdata.PH = "--";
                            //高压泵状态 
                            jkdata.GPumpState = "--";
                            jkdata.YLevel = "--";
                            jkdata.JLevel = "--";
                            jkdata.JSetpressure = "--";
                            jkdata.Conductivity = "--";
                            jkdata.Orp = "--";
                            jkdata.Salinity = "--";
                            jkdata.Oxygen = "--";
                            //jkdata.pumpState = new List<string>();
                            //jkdata.pumpdatas = new List<ZPumpData>();

                            //for (var i = 0; i < 4; i++)
                            //{

                            //    jkdata.pumpState.Add("停止");
                            //}
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
                    jk.eventlist = eventlist.Select(r => new Eventinfo
                    {
                        ID = r.ID,
                        Rtuid = r.Rtuid,
                        EventMessage = r.EventMessage,
                        EventTime = r.EventTime,
                        CurrentValue = r.CurrentValue,
                        LimitValue = r.LimitValue,
                        Unit = r.Unit,
                        DataType = r.DataType,
                        RegionName = r.RegionName


                    }).Distinct(new eventdataCompare()).ToList();//事件集合去重
                    jk.State = StationState;//泵房状态
                    jk.Attention = Convert.ToBoolean(item.Key.FocusOn);//是否关注
                    result.Add(jk);
                }

            }
            double TotalPage = Math.Ceiling((float)totalcount / (float)pagesize);

            PartialView("_ZStationJKTable_card", result.OrderByDescending(r => r.Attention).ThenBy(r => r.StationName));
            string dataTable = await ViewToString.RenderPartialViewToString(this, "_ZStationJKTable_card");

            return Json(new
            {
                TotalCount = totalcount,
                TotalPage = TotalPage,
                dataTable = dataTable,
                allNum = allNum,
                alarmNum = alarmNum,
                onlineNum = onlineNum,
                offLineNum = offLineNum,
                attentionNum = attentionNum,

            });
        }
        #endregion
        #region 泵房监控精简模式
        [TypeFilter(typeof(IgonreActionFilter))]
        public async Task<IActionResult> QuerySJKTable_simple(int pageindex, int pagesize, string searchText, string type)
        {

            var enumfrequency = (int)Model.CustomizedClass.Enum.变频分类;
            var ftype = _DataItemDetailService.Query<SysDataItemDetail>(r => r.FItemId == enumfrequency).ToList();
            int userID = int.Parse(User.FindFirstValue("UserID"));
            int totalcount = 0;
            var user = _UserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
            string alarmSids = "", onlinertuIds = "";
            int allNum = 0, alarmNum = 0, onlineNum = 0, offLineNum = 0, attentionNum = 0;
            //获取供水泵房对应的innertype
            int stationtype = 0;
            var intypeid = (int)Model.CustomizedClass.Enum.泵房内置类型;
            var typemodel = _DataItemDetailService.Query<SysDataItemDetail>(r => r.FItemId == intypeid && r.ItemName.Contains("直饮水泵房")).FirstOrDefault();
            if (typemodel != null)
            {
                stationtype = int.Parse(typemodel.ItemValue);
            }
            else
            {
                stationtype = 2;
            }
            #region
            //GetAlarmZStion(user, ref alarmSids, stationtype);//获取报警泵房相关数据
            GetZOnlineRtuID(user.IsAdmin, user.UserId, stationtype, ref onlinertuIds);//在线rtuids
            var ids_rtuonline = new List<string>(onlinertuIds.Split(",")).ConvertAll(r => int.Parse(r));
            DataTable tvpDt = new DataTable();
            tvpDt.Columns.Add("RTUID", typeof(int));
            foreach (var item in ids_rtuonline)
            {
                tvpDt.Rows.Add(item);
            }

            var query = _StationService.GetSationNumMany_StationJK(tvpDt, searchText == null ? "" : searchText, userID, user.IsAdmin, stationtype).FirstOrDefault();
            allNum = query.allNum;
            alarmNum = query.alarmNum;
            onlineNum = query.onlineNum;
            offLineNum = query.allNum - query.alarmNum - query.onlineNum;
            attentionNum = query.attentionNum;
            #endregion

            var dataList = _StationService.GetStaionDataByPage_simple_tvp(tvpDt, stationtype, type, searchText == null ? "" : searchText, pageindex, pagesize, user.IsAdmin, user.UserId, ref totalcount);
            List<StationJkInfo_simple> result = new List<StationJkInfo_simple>() { };
            if (dataList.Count() > 0)
            {
                result = dataList.Select(r => new StationJkInfo_simple
                {
                    StationID = r.StationID,
                    StationName = r.StationName,
                    State = r.type,
                    Attention = Convert.ToBoolean(r.FocusOn)

                }).ToList(); 
            } 
            double TotalPage = Math.Ceiling((float)totalcount / (float)pagesize);

            PartialView("_ZStationJKTable_simple", result.OrderByDescending(r => r.Attention).ThenBy(r => r.StationName));
            string dataTable = await ViewToString.RenderPartialViewToString(this, "_ZStationJKTable_simple");

            return Json(new
            {
                TotalCount = totalcount,
                TotalPage = TotalPage,
                dataTable = dataTable,
                allNum = allNum,
                alarmNum = alarmNum,
                onlineNum = onlineNum,
                offLineNum = offLineNum,
                attentionNum = attentionNum,

            });
        }
        #endregion
        #region 泵房各状态数量
        //报警泵房id(泵房下只要有一个设备报警该泵房就是报警泵房)
        public void GetAlarmZStion(SysUser u, ref string alarmSids, int stationtype)
        {
            var datalist = _StationService.GetAlarmZStion(u.IsAdmin, u.UserId, stationtype);
            if (datalist.Count() > 0)
            {
                alarmSids = string.Join(",", datalist.Select(r => r.StationID));
            }
        }

        //在线rtuid
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

        }
        //获取在线rtid，报警泵房id
        public void GetOnlineRtuIDAndAlarmSID(SysUser u, ref string alarmSids, int stationtype, ref string onlinertuIds)
        {
            var offtime = DateTime.Now.AddMinutes(-timevalue);//掉线时间
            string rtuids = "";
            if (u.IsAdmin == false)
            {
                var rtuidlist = _StationService.GetZRtuIDByUserID(u.UserId, stationtype);
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

            var datalist_Alarm = _StationService.GetAlarmStion_New(u.IsAdmin, u.UserId, stationtype, onlinertuIds);
            if (datalist.Count() > 0)
            {
                alarmSids = string.Join(",", datalist_Alarm.Select(r => r.StationID));
            }
        }
        #endregion
        #region 泵的数据实体
        public class Pump
        {
            /// <summary>
            /// 运行方式
            /// </summary>
            public string PState { get; set; }
            /// <summary>
            /// 运行状态
            /// </summary>
            public string PFault { get; set; }
            /// <summary>
            /// 电流
            /// </summary>
            public double Electric { get; set; }

            /// <summary>
            /// 频率
            /// </summary>
            public string Frequency { get; set; }
            /// <summary>
            /// 泵的名称
            /// </summary>
            public string PumpName { get; set; }
            /// <summary>
            /// 运行时间
            /// </summary>
            public double RunHour { get; set; }
        }
        #endregion
        public Pump GetDataByDataID(RtuJKInfo listdevicejkinfo, List<int> dataid)
        {
            Pump pump = new Pump();




            #region 方式
            var psate = listdevicejkinfo.DigitalValues.Keys.Contains(dataid[0].ToString()) ? listdevicejkinfo.DigitalValues[dataid[0].ToString()].ToString() : null;
            pump.PState = "自动";
            if (psate != null)
            {
                pump.PState = psate == "True" ? "手动" : "自动";
            }

            #endregion
            #region 状态
            //普通无负压设备
            var PFault = listdevicejkinfo.DigitalValues.Keys.Contains(dataid[3].ToString()) ? listdevicejkinfo.DigitalValues[dataid[3].ToString()].ToString() : null;
            var DeviceJKInfoB = listdevicejkinfo.DigitalValues.Keys.Contains(dataid[4].ToString()) ? listdevicejkinfo.DigitalValues[dataid[4].ToString()].ToString() : null;
            var DeviceJKInfoH = listdevicejkinfo.DigitalValues.Keys.Contains(dataid[5].ToString()) ? listdevicejkinfo.DigitalValues[dataid[5].ToString()].ToString() : null;
            var DeviceJKInfoG = listdevicejkinfo.DigitalValues.Keys.Contains(dataid[6].ToString()) ? listdevicejkinfo.DigitalValues[dataid[6].ToString()].ToString() : null;


            if (listdevicejkinfo.AnalogValues != null)
            {
                pump.Electric = listdevicejkinfo.AnalogValues.Keys.Contains(dataid[2].ToString()) ? (double)listdevicejkinfo.AnalogValues[dataid[2].ToString()] : 0;
                pump.Frequency = listdevicejkinfo.AnalogValues.Keys.Contains(dataid[1].ToString()) ? listdevicejkinfo.AnalogValues[dataid[1].ToString()].ToString() : "0";
                pump.RunHour = listdevicejkinfo.AnalogValues.Keys.Contains(dataid[7].ToString()) ? (double)listdevicejkinfo.AnalogValues[dataid[7].ToString()] : 0;
            }
            else
            {
                pump.Electric = 0;
                pump.Frequency = "0";
                pump.RunHour = 0;
            }
            if (pump.PState == "自动")
            {
                if (PFault != null && PFault == "True")
                {
                    pump.PFault = "故障";
                    if ((DeviceJKInfoB != null && DeviceJKInfoB == "True") || (DeviceJKInfoH != null && DeviceJKInfoH == "True") || (DeviceJKInfoG != null && DeviceJKInfoG == "True"))
                    {

                    }
                    else
                    {
                        pump.Frequency = "0";
                    }
                }
                else if (DeviceJKInfoB != null && DeviceJKInfoB == "True")
                {
                    pump.PFault = "变频";
                }
                else if (DeviceJKInfoH != null && DeviceJKInfoH == "True")
                {
                    pump.PFault = "恒频";
                }
                else if (DeviceJKInfoG != null && DeviceJKInfoG == "True")
                {
                    pump.PFault = "工频";
                    pump.Frequency = "50";
                }
                else
                {
                    pump.PFault = "停止";
                    pump.Frequency = "0";
                }
            }
            else if (pump.PState == "手动")
            {
                if (PFault != null && PFault == "True")
                {
                    pump.PFault = "故障";
                    if (pump.Electric == 0)
                    {
                        pump.Frequency = "0";
                    }
                }
                else if (pump.Electric != 0)
                {
                    pump.PFault = "工频";
                    pump.Frequency = "50";
                }
                else
                {
                    pump.PFault = "停止";
                    pump.Frequency = "0";
                }
            }
            #endregion
            return pump;
        }
        public class eventdataCompare : IEqualityComparer<Eventinfo>
        {
            public bool Equals(Eventinfo x, Eventinfo y)
            {
                if (x == null)
                    return y == null;
                return x.ID == y.ID;
            }

            public int GetHashCode(Eventinfo obj)
            {
                if (obj == null)
                    return 0;
                return obj.ID.GetHashCode();
            }
        }
    }
}