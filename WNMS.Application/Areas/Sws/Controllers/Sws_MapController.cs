using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDBHelper;
using WNMS.IService;
using WNMS.Model.CustomizedClass;
using WNMS.Model.DataModels;
using WNMS.Utility;

namespace WNMS.Application.Areas.Sws.Controllers
{
    [Area("Sws")]
    public class Sws_MapController : Controller
    {
        private ISws_StationService _StationService = null;
        private ISysUserService _UserService = null;
        private ISws_EventInfoService _EventInfoService = null;
        int timevalue = int.Parse(StaticConstraint.TimeValue);
        public Sws_MapController(ISws_StationService sws_StationService,
              ISysUserService sysUserService, ISws_EventInfoService sws_EventInfoService
             )
        {
            _StationService = sws_StationService;
            _UserService = sysUserService;
            _EventInfoService = sws_EventInfoService;
        }
        public IActionResult Index()
        {
            return View();
        }
        //泵房数量数据
        public IActionResult QueryStationNum()
        {
            //参数定义
            string alarmSids = "", onlinertuIds = "";
            int allNum = 0, alarmNum = 0, onlineNum = 0, offLineNum = 0, attentionNum = 0, deviceNum = 0, onlineNum1 = 0;
            //用户
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = _UserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
            #region 各个数量
            GetAllALarmStaion(user, ref alarmSids);//获取报警泵房相关数据
            GetOnlineRtuID(user.IsAdmin, user.UserId, ref onlinertuIds);//在线rtuids

            var query = _StationService.GetSationNumMap(alarmSids, onlinertuIds, userID, user.IsAdmin).FirstOrDefault();
            allNum = query.allNum;
            alarmNum = query.alarmNum;
            onlineNum = query.onlineNum;
            onlineNum1 = query.onlineNum1;
            offLineNum = query.allNum - query.alarmNum - query.onlineNum;
            attentionNum = query.attentionNum;
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
        //全部报警泵房id(泵房下只要有一个设备报警该泵房就是报警泵房)
        public void GetAllALarmStaion(SysUser u, ref string alarmSids)
        {
            var datalist = _StationService.GetAllAlarmStionMap(u.IsAdmin, u.UserId);
            if (datalist.Count() > 0)
            {
                alarmSids = string.Join(",", datalist.Select(r => r.StationID));
            }
        }
        //在线rtuid
        public void GetOnlineRtuID(bool isadmin, int userid, ref string onlinertuIds)
        {

            var offtime = DateTime.Now.AddMinutes(-timevalue);//掉线时间
            string rtuids = "";
            if (isadmin == false)
            {
                var rtuidlist = _StationService.GetRtuIDByUserIDMap(userid).Distinct();
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
                macJson = "{'RTUID':{'$in':[" + rtuids + "]},'DigitalValues.1001':true,'DigitalValues.1000':true,'EventState':false,'UpdateTime':{'$gte':ISODate('" + offtime.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'") + "')}}";
            }
            else
            {
                macJson = "{'DigitalValues.1001':true,'DigitalValues.1000':true,'EventState':false,'UpdateTime':{'$gte':ISODate('" + offtime.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'") + "')}}";
            }
            var collections = new MongoDBHelper<RtuJKInfo>(DateTime.Now.Year.ToString());
            var datalist = collections.Find("Sws_DeviceJKInfo", macJson, "UpdateTime").Select(r => r.RTUID).ToList();
            onlinertuIds = datalist.Count > 0 ? string.Join(",", datalist) : "";//在线的rtuid字符串
        }
        #region 查询泵房监控数据
        public IActionResult QueryTableData(string typecheck, string stationName)
        {
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = _UserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
            string alarmSids = "", onlinertuIds = "";
            #region
            GetAllALarmStaion(user, ref alarmSids);//获取报警泵房相关数据
            GetOnlineRtuID(user.IsAdmin, user.UserId, ref onlinertuIds);//在线rtuids

            #endregion
            var dataList = _StationService.GetStaionAllDataMap(onlinertuIds, alarmSids, user.IsAdmin, user.UserId, stationName);
            List<StationMapState> result = new List<StationMapState>() { };
            if (dataList.Count() > 0)
            {
                var rtuidlist = dataList.Where(r => r.RTUID.ToString() != "").Select(r => r.RTUID).Distinct();
                var rtuids = string.Join(",", rtuidlist);
                string macJson = "{\"RTUID\":{'$in':[" + rtuids + "]}}";
                var collections = new MongoDBHelper<RtuJKInfo>(DateTime.Now.Year.ToString());
                var jklist = collections.Find("Sws_DeviceJKInfo", macJson, "UpdateTime");
                var stationdata = dataList.GroupBy(r => new { r.StationID, r.StationName, r.Lng, r.Lat }).OrderBy(r => r.Key.StationName);
                List<SwsEventInfo> eventlist_all = new List<SwsEventInfo>();
                if (rtuids != "")
                {
                    var rtuidlist_int = new List<string>(rtuids.Split(',')).ConvertAll(r => int.Parse(r));
                    eventlist_all = _EventInfoService.Query<SwsEventInfo>(r => rtuidlist_int.Contains(r.Rtuid)).ToList(); ;//1代表供水datainfo
                }
                foreach (var item in stationdata)
                {
                    StationMapState jk = new StationMapState();
                    jk.StationName = item.Key.StationName;
                    jk.StationID = item.Key.StationID;
                    jk.deviceJKs = new List<mapmontorJK>() { };
                    jk.Lng = item.Key.Lng;
                    jk.Lat = item.Key.Lat;
                    double allInFlow = 0;
                    List<string> deviceState = new List<string>();
                    //所有设备
                    var devicelist = dataList.Where(r => r.StationID == item.Key.StationID);
                    foreach (var it in devicelist)
                    {
                        if (it.RTUID.ToString() != "")
                        {
                            double timpan = 0;
                            int Rtuidd = Convert.ToInt32(it.RTUID);
                            var partion = Convert.ToInt32(it.Partition.ToString());
                            mapmontorJK jkdata = new mapmontorJK();
                            var datajk = jklist.Where(r => r.RTUID == Rtuidd).FirstOrDefault();
                            #region 状态
                            var state = "离线";
                            var eventdata = eventlist_all.Where(r => r.Rtuid == Rtuidd).ToList();
                            if (eventdata.Count() > 0)
                            {
                                state = "故障";
                            }
                            else
                            {
                                if (datajk != null && datajk.DigitalValues != null)
                                {
                                    bool zstate = datajk.DigitalValues.Keys.Contains("1001") ? Convert.ToBoolean(datajk.DigitalValues["1001"]) : false;
                                    var tstate = datajk.DigitalValues.Keys.Contains("1000") ? Convert.ToBoolean(datajk.DigitalValues["1000"]) : false;

                                    if (zstate == true && tstate == true && (DateTime.Now - Convert.ToDateTime(datajk.UpdateTime)).TotalMinutes <= timevalue)
                                    {
                                        state = "正常";
                                    }
                                    else
                                    {
                                        state = "离线";
                                    }

                                }
                            }
                            deviceState.Add(state);
                            if (datajk != null)
                            {
                                var updatetime = datajk.UpdateTime;
                                var timeSpan = (updatetime - DateTime.Now).TotalMinutes;
                                if (timeSpan < timpan)
                                {
                                    jk.UpdateTime = updatetime.ToString();
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
            });
        }
        #endregion
    }
}