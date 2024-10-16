using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDBHelper;
using WNMS.Application.Utility;
using WNMS.IService;
using WNMS.Model.CustomizedClass;
using WNMS.Model.DataModels;
using WNMS.Utility;

namespace WNMS.Application.Areas.Sws.Controllers
{
    [Area("Sws")]
    public class Sws_StationJKController : Controller
    {
        
        private ISws_StationService _StationService = null;
        private ISysUserService _UserService = null;
        private ISws_EventInfoService _EventInfoService = null;
        private ISws_DataInfoService _DataInfoService = null;
        private ISys_DataItemDetailService _DataItemDetailService = null;

        int timevalue = int.Parse(StaticConstraint.TimeValue);
        public Sws_StationJKController(ISws_StationService sws_StationService,ISysUserService sysUserService,ISws_EventInfoService sws_EventInfoService,
            ISws_DataInfoService sws_DataInfoService,ISys_DataItemDetailService sys_DataItemDetailService) {
           
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
        public async Task<IActionResult> QuerySJKTableList(int pageindex,int pagesize,string searchText,string type)
        {
            var enumfrequency = (int)Model.CustomizedClass.Enum.变频分类;
            var ftype = _DataItemDetailService.Query<SysDataItemDetail>(r => r.FItemId == enumfrequency).ToList();
            int userID = (int)HttpContext.Session.GetInt32("user");
            int totalcount = 0;
            var user = _UserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
            string alarmSids = "", onlinertuIds = "";
            int allNum = 0, alarmNum = 0, onlineNum = 0, offLineNum = 0, attentionNum = 0;
            //获取供水泵房对应的innertype
            int stationtype = 0;
            var intypeid = (int)Model.CustomizedClass.Enum.泵房内置类型;
            var typemodel = _DataItemDetailService.Query<SysDataItemDetail>(r => r.FItemId == intypeid && r.ItemName.Contains("供水泵房")).FirstOrDefault();
            if (typemodel != null)
            {
                stationtype = int.Parse(typemodel.ItemValue);
            }
            else
            {
                stationtype = 1;
            }
            #region
            GetALarmStaion(user, ref alarmSids, stationtype);//获取报警泵房相关数据
            GetOnlineRtuID(user.IsAdmin, user.UserId, stationtype, ref onlinertuIds);//在线rtuids
            
            var query = _StationService.GetSationNumMany(alarmSids, onlinertuIds, searchText == null ? "" : searchText, userID, user.IsAdmin,stationtype).FirstOrDefault();
            allNum = query.allNum;
            alarmNum = query.alarmNum;
            onlineNum = query.onlineNum;
            offLineNum = query.allNum - query.alarmNum - query.onlineNum;
            attentionNum = query.attentionNum;
            #endregion
            var dataList = _StationService.GetStaionDataByPage(stationtype, type, onlinertuIds, alarmSids,searchText==null?"": searchText, pageindex,pagesize,user.IsAdmin,user.UserId,ref totalcount);
            List<StationJkInfo> result = new List<StationJkInfo>() { };
            if (dataList.Count() > 0)
            {
                var rtuidlist = dataList.Where(r => r.RTUID.ToString() != "").Select(r => r.RTUID).Distinct();
                var rtuids = string.Join(",", rtuidlist);
                string macJson = "{\"RTUID\":{'$in':[" + rtuids + "]}}";
                var collections = new MongoDBHelper<RtuJKInfo>(DateTime.Now.Year.ToString());
                var jklist = collections.Find("Sws_DeviceJKInfo", macJson, "UpdateTime");
                var stationdata = dataList.GroupBy(r => new { r.StationID, r.StationName, r.FocusOn }).OrderBy(r => r.Key.StationName);
                foreach (var item in stationdata)
                {
                    StationJkInfo jk = new StationJkInfo();
                    jk.StationName= item.Key.StationName;
                    jk.StationID = item.Key.StationID;
                    jk.deviceJKs = new List<montorJK>() { };
                    var StationState = "离线";//泵房状态
                    List<SwsEventInfo> eventlist = new List<SwsEventInfo>();
                    //所有设备
                    var devicelist = dataList.Where(r => r.StationID == item.Key.StationID);
                    foreach (var it in devicelist)
                    {
                        int PumpNum = int.Parse(it.PumpNum.ToString());
                        if (it.RTUID.ToString() != "")
                        {
                            int Rtuidd = Convert.ToInt32(it.RTUID);
                            
                            montorJK jkdata = new montorJK();
                            
                            jkdata.devicename = it.DeviceName.ToString();
                            jkdata.DeviceID = long.Parse(it.DeviceID.ToString());
                            var datajk = jklist.Where(r => r.RTUID == Rtuidd).FirstOrDefault();
                            #region 状态
                            var state = "离线";
                           
                            var eventdata = _EventInfoService.Query<SwsEventInfo>(r => r.Rtuid == Rtuidd);
                            if (eventdata.Count() > 0)
                            {
                                state = "故障";
                                eventlist = eventlist.Union(eventdata).ToList();


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
                            #endregion
                            if (datajk != null)
                            {
                                var updatetime = datajk.UpdateTime;
                                jkdata.datetime = updatetime.ToString();//更新时间
                                jkdata.DeviceID = it.DeviceID;
                                jkdata.RtuID = Rtuidd;
                                jkdata.State = state;
                                var partion = Convert.ToInt32(it.Partition.ToString());
                                //进水压力
                                int pressinid = 2000 + (partion-1)*500;
                                jkdata.PressIN=new KeyValuePair<string, bool>(datajk.AnalogValues.ContainsKey(pressinid.ToString())? datajk.AnalogValues[pressinid.ToString()].ToString():"--", 
                                 eventlist.Count>0?(eventlist.Where(r=>r.EventSource== pressinid&&r.EventLevel!=0).Count()>0?true:false):false);
                                //出水压力
                                int pressout= 2001+ (partion - 1) * 500;
                                jkdata.PressOut = new KeyValuePair<string, bool>(datajk.AnalogValues.ContainsKey(pressout.ToString()) ? datajk.AnalogValues[pressout.ToString()].ToString() : "--",
                                eventlist.Count > 0 ? (eventlist.Where(r => r.EventSource == pressout && r.EventLevel != 0).Count() > 0 ? true : false) : false);
                                //水泵数据
                                #region 水泵数据
                                var fixfrequencyvalue = 2003;//频率
                                var frequencyvalue = 2004;//频率
                                var electricvalue = 2010;//电流
                                var pumpstatevalue = 5000;//方式
                                var guzhangvalue = 5003;//状态
                                var bianpinvalue = 5001;//变频
                                var hengpinvalue = 5127;//恒频
                                var gongpinvalue = 5002;//工频
                                var yunxingTime = 2077;//运行时间//datainfo 和2017重复
                                
                                List<string> pumpState = new List<string>();//泵状态        
                                List<PumpData> pumpdataList = new List<PumpData>();//泵频率,电流、运行时间
                              
                                var frequencyflg = it.Frequency.ToString();
                                int j = 0;
                                var fname = ftype.Count() > 0 ? (ftype.Where(r=>r.ItemValue==frequencyflg).FirstOrDefault()?.ItemName) : "单变频";
                                for (var i = 0; i < PumpNum; i++)
                                {
                                    PumpData pdatlist = new PumpData();
                                    
                                    List<int> dataids = new List<int>();
                                    if (fname.Contains("单变频"))
                                    {
                                        dataids = new List<int>() { pumpstatevalue + j, fixfrequencyvalue, electricvalue + i, guzhangvalue + j, bianpinvalue + j, hengpinvalue + i, gongpinvalue + j, yunxingTime + i };
                                        j += 4;
                                    }
                                    else
                                    {
                                        dataids = new List<int>() { pumpstatevalue + j, frequencyvalue + i, electricvalue + i, guzhangvalue + j, bianpinvalue + j, hengpinvalue + i, gongpinvalue + j, yunxingTime + i };
                                        j += 4;
                                    }
                                    Pump pump = GetDataByDataID(datajk, dataids);
                                    if (state == "正常")
                                    {
                                        if ((pump.PFault == "变频" || pump.PFault == "恒频") && pump.Frequency != "0")
                                        {
                                            pumpState.Add("变频");

                                        }
                                        else if (pump.PFault == "工频" && pump.Frequency != "0")
                                        {
                                            pumpState.Add("工频");
                                        }
                                        else if (pump.PFault == "故障")
                                        {
                                            pumpState.Add("故障");
                                        }
                                        else
                                        {
                                            pumpState.Add("停止");
                                        }
                                        
                                    }
                                    else
                                    {
                                        pumpState.Add("停止");
                                       
                                    }
                                    if (fname.Contains("单变频"))
                                    {
                                        if (pumpState[i] == "变频" || pumpState[i] == "工频")
                                        {
                                            pdatlist.frequency = new KeyValuePair<string, bool>(pump.Frequency, eventlist.Count > 0 ? (eventlist.Where(r => r.EventSource == fixfrequencyvalue && r.EventLevel != 0).Count() > 0 ? true : false) : false);
                                        }
                                        else
                                        {
                                            pdatlist.frequency = new KeyValuePair<string, bool>(pump.Frequency,false);
                                        }
                                    }
                                    else
                                    {
                                        var fid = frequencyvalue + i;
                                            pdatlist.frequency = new KeyValuePair<string, bool>(pump.Frequency, eventlist.Count > 0 ? (eventlist.Where(r => r.EventSource == fid && r.EventLevel != 0).Count() > 0 ? true : false) : false);
                                    }
                                    var runtimeid = yunxingTime + i;
                                    pdatlist.runtime = new KeyValuePair<string, bool>(pump.RunHour.ToString(), eventlist.Count > 0 ? (eventlist.Where(r => r.EventSource == runtimeid && r.EventLevel != 0).Count() > 0 ? true : false) : false);
                                    var eltricid = electricvalue + i;
                                    pdatlist.eletric= new KeyValuePair<string, bool>(pump.Electric.ToString(), eventlist.Count > 0 ? (eventlist.Where(r => r.EventSource == eltricid && r.EventLevel != 0).Count() > 0 ? true : false) : false);
                                    pumpdataList.Add(pdatlist);
                                       

                                }
                                #endregion

                                //瞬时流量
                                var infdata1 = datajk.AnalogValues.ContainsKey((2030 + (partion - 1) * 500).ToString()) ? datajk.AnalogValues[(2030 + (partion - 1) * 500).ToString()] : null;
                                var infdata2= datajk.AnalogValues.ContainsKey((2032 + (partion - 1) * 500).ToString()) ? datajk.AnalogValues[(2032 + (partion - 1) * 500).ToString()] : null;
                                var infdata3= datajk.AnalogValues.ContainsKey((2034 + (partion - 1) * 500).ToString()) ? datajk.AnalogValues[(2034 + (partion - 1) * 500).ToString()] : null;
                                var infdata4 = datajk.AnalogValues.ContainsKey((2036 + (partion - 1) * 500).ToString()) ? datajk.AnalogValues[(2036 + (partion - 1) * 500).ToString()] : null;
                                if (infdata1 == null && infdata2 == null && infdata3 == null && infdata4 == null)
                                {
                                    jkdata.inflow = "";
                                }
                                else
                                {
                                    jkdata.inflow = (Convert.ToDouble(infdata1)+ Convert.ToDouble(infdata2)+ Convert.ToDouble(infdata3)+ Convert.ToDouble(infdata4)).ToString();
                                }
                                //累计流量
                                var tfdata1= datajk.AnalogValues.ContainsKey((2031 + (partion - 1) * 500).ToString()) ? datajk.AnalogValues[(2031 + (partion - 1) * 500).ToString()] : null;
                                var tfdata2 = datajk.AnalogValues.ContainsKey((2033 + (partion - 1) * 500).ToString()) ? datajk.AnalogValues[(2033 + (partion - 1) * 500).ToString()] : null;
                                var tfdata3 = datajk.AnalogValues.ContainsKey((2035 + (partion - 1) * 500).ToString()) ? datajk.AnalogValues[(2035 + (partion - 1) * 500).ToString()] : null;
                                var tfdata4 = datajk.AnalogValues.ContainsKey((2037 + (partion - 1) * 500).ToString()) ? datajk.AnalogValues[(2037 + (partion - 1) * 500).ToString()] : null;
                                if (tfdata1 == null && tfdata2 == null && tfdata3 == null && tfdata4 == null)
                                {
                                    jkdata.totalflow = "";
                                }
                                else
                                {
                                    jkdata.totalflow = (Convert.ToDouble(tfdata1) + Convert.ToDouble(tfdata2) + Convert.ToDouble(tfdata3) + Convert.ToDouble(tfdata4)).ToString();
                                }
                                //累计电量
                                var pall= datajk.AnalogValues.ContainsKey((2085 + (partion - 1) * 500).ToString()) ? datajk.AnalogValues[(2085 + (partion - 1) * 500).ToString()] : null;
                                var pdata1= datajk.AnalogValues.ContainsKey((2023 + (partion - 1) * 500).ToString()) ? datajk.AnalogValues[(2023 + (partion - 1) * 500).ToString()] : null;
                                var pdata2= datajk.AnalogValues.ContainsKey((2024 + (partion - 1) * 500).ToString()) ? datajk.AnalogValues[(2024 + (partion - 1) * 500).ToString()] : null;
                                var pdata3 = datajk.AnalogValues.ContainsKey((2025 + (partion - 1) * 500).ToString()) ? datajk.AnalogValues[(2025 + (partion - 1) * 500).ToString()] : null;
                                var pdata4 = datajk.AnalogValues.ContainsKey((2026 + (partion - 1) * 500).ToString()) ? datajk.AnalogValues[(2026 + (partion - 1) * 500).ToString()] : null;
                                var pdata = Convert.ToDouble(pdata1) + Convert.ToDouble(pdata2) + Convert.ToDouble(pdata3) + Convert.ToDouble(pdata4);
                                jkdata.totalpower = (pdata == 0.0 ? (Convert.ToDouble(pall)).ToString() : pdata.ToString());
                                jkdata.pumpState = pumpState;
                                jkdata.pumpdatas = pumpdataList;
                            }
                            else
                            {
                                
                                jkdata.datetime = "";//更新时间
                                jkdata.RtuID = Rtuidd;
                                jkdata.State = state;
                                jkdata.PressIN = new KeyValuePair<string, bool>("",false);
                                jkdata.PressOut = new KeyValuePair<string, bool>("",false);
                                jkdata.pumpState = new List<string>();
                                jkdata.pumpdatas = new List<PumpData>();
                                jkdata.totalflow = "";
                                jkdata.totalpower = "";
                                for (var i = 0; i < PumpNum; i++)
                                {
                                    PumpData p = new PumpData();
                                    p.eletric = new KeyValuePair<string, bool>("", false);
                                    p.frequency = new KeyValuePair<string, bool>("", false);
                                    p.runtime = new KeyValuePair<string, bool>("", false);
                                    jkdata.pumpdatas.Add(p);
                                    jkdata.pumpState.Add("停止");
                                }
                            }
                            jk.deviceJKs.Add(jkdata);
                        }
                        else
                        {

                            
                            montorJK jkdata = new montorJK();
                            jkdata.devicename = it.DeviceName.ToString();
                            jkdata.DeviceID = long.Parse(it.DeviceID.ToString());
                            jkdata.datetime = "";//更新时间
                            jkdata.RtuID = 0;
                            jkdata.State = "离线";
                            jkdata.PressIN = new KeyValuePair<string, bool>("",false);
                            jkdata.PressOut = new KeyValuePair<string, bool>("",false);
                            jkdata.pumpState = new List<string>();
                            jkdata.pumpdatas = new List<PumpData>();
                            jkdata.totalflow = "";
                            jkdata.totalpower = "";
                            for (var i = 0; i < PumpNum; i++)
                            {
                                PumpData p = new PumpData();
                                p.eletric = new KeyValuePair<string, bool>("",false);
                                p.frequency= new KeyValuePair<string, bool>("", false);
                                p.runtime= new KeyValuePair<string, bool>("", false);
                                jkdata.pumpdatas.Add(p);
                                jkdata.pumpState.Add("停止");
                            }
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
                    jk.eventlist = eventlist.Distinct(new eventdataCompare()).ToList();//事件集合去重
                    jk.State = StationState;//泵房状态
                    jk.Attention = Convert.ToBoolean(item.Key.FocusOn);//是否关注
                    result.Add(jk);
                }

            }
            double TotalPage = Math.Ceiling((float)totalcount / (float)pagesize);
            
            PartialView("_StationJKTable_List", result);
            string dataTable = await ViewToString.RenderPartialViewToString(this, "_StationJKTable_List");

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
                }
                else if (pump.Electric != 0)
                {
                    pump.PFault = "工频";
                }
                else
                {
                    pump.PFault = "停止";
                }
            }
            #endregion
            return pump;
        }
        #region 泵房各状态数量
        //报警泵房id(泵房下只要有一个设备报警该泵房就是报警泵房)
        public void GetALarmStaion(SysUser u,ref string alarmSids,int stationtype)
        {
            var datalist = _StationService.GetAlarmStion(u.IsAdmin,u.UserId, stationtype);
            if (datalist.Count() > 0)
            {
                alarmSids = string.Join(",", datalist.Select(r=>r.StationID));
            }
        }
        
        //在线rtuid
        public void GetOnlineRtuID(bool isadmin,int userid,int stationtype,ref string onlinertuIds )
        {
            
            var offtime = DateTime.Now.AddMinutes(-timevalue);//掉线时间
            string rtuids = "";
            if (isadmin == false)
            {
                var rtuidlist = _StationService.GetRtuIDByUserID(userid, stationtype);
                if (rtuidlist.Count() > 0)
                {
                    rtuids = string.Join(",", rtuidlist.Select(r=>r.RTUID));
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
            //var onlineIDList = datalist.Select(r => r.RtuID).ToList();//在线的rtuid集合
            //var aa = collections.FindByFields("Sws_DeviceJKInfo", macJson, new FieldsDocument("RtuID",1), "UpdateTime").Select(r=>r["RtuID"]).ToList();
            onlinertuIds = datalist.Count > 0 ? string.Join(",", datalist) : "";//在线的rtuid字符串
        }
        #endregion
        #region 多线程
        //public void test()
        //{
        //    TaskFactory taskFactory = new TaskFactory();
        //    List<Task> taskList = new List<Task>() { };

        //    taskList.Add(taskFactory.StartNew(() => this.GetALarmStaion(, "Portal")));
        //    taskList.Add(taskFactory.StartNew(() => this.Coding("随心随缘", "  DBA ")));
        //    taskList.Add(taskFactory.StartNew(() => this.Coding("心如迷醉", "Client")));
        //    taskList.Add(taskFactory.StartNew(() => this.Coding(" 千年虫", "BackService")));
        //    taskList.Add(taskFactory.StartNew(() => this.Coding("简单生活", "Wechat")));
        //}
        //private long Coding(string name, string projectName)
        //{

        //    long lResult = 0;
        //    for (int i = 0; i < 1_000_000_000; i++)
        //    {
        //        lResult += i;
        //    }

        //    return lResult;
        //}
        #endregion
        public class eventdataCompare : IEqualityComparer<SwsEventInfo>
        {
            public bool Equals(SwsEventInfo x, SwsEventInfo y)
            {
                if (x == null)
                    return y == null;
                return x.Id == y.Id;
            }

            public int GetHashCode(SwsEventInfo obj)
            {
                if (obj == null)
                    return 0;
                return obj.Id.GetHashCode();
            }
        }
    }
}