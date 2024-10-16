using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common;
using Common.WebApiActions;
using Microsoft.AspNetCore.Mvc;
using MongoDBHelper;
using WNMS.Application.Utility.Filters;
using WNMS.IService;
using WNMS.Model.CustomizedClass;
using WNMS.Model.DataModels;
using WNMS.Utility;

namespace WNMS.Application.Areas.Sws.Controllers
{
    [Area("Sws")]
    public class Sws_DetailController : Controller
    {
        private ISws_StationService stationService;
        private ISws_DeviceInfo01Service _sws_DeviceInfo01Service;
        private ISws_GUIInfoService _sws_GUIInfoService;
        private IMonthQuartZ01Service _monthQuartZ01Service;
        private IDayQuartZ01Service _dayQuartZ01Service;
        private ISws_EventInfoService _EventInfoService = null;
        private ISys_DataItemDetailService _DataItemDetailService = null;
        private IAttachmentService _attachmentService = null;
        private ISws_DataInfoService _sws_DataInfoService = null;
        private ISws_CameraService cameraService = null;
        private ISws_DeviceInfo02Service _sws_DeviceInfo02Service = null;
        private ISws_RTUInfoService _sws_RTUInfoService = null;
        private IDDayQuartZ01Service _dDayQuartZ01Service = null;
        private IDDayQuartZ02Service _dDayQuartZ02Service = null;
        private readonly ISws_ValveWith01Service valveService;
        private ISws_DeviceTemplateService _sws_DeviceTemplateService = null;
        private ISws_TemplateService _sws_TemplateService = null;
        int timevalue = int.Parse(StaticConstraint.TimeValue);
        string signalrHubs = StaticConstraint.SignalrHubs;
        public Sws_DetailController(ISws_StationService sws_StationService,
            ISws_DeviceInfo01Service sws_DeviceInfo01Service,
            ISws_GUIInfoService sws_GUIInfoService,
            IMonthQuartZ01Service monthQuartZ01Service,
            ISws_EventInfoService sws_EventInfoService,
            ISys_DataItemDetailService sys_DataItemDetailService,
            IAttachmentService attachmentService,
            ISws_DataInfoService sws_DataInfoService,
            ISws_CameraService sws_cameraServiceService,
            IDayQuartZ01Service dayQuartZ01Service,
            ISws_DeviceInfo02Service sws_DeviceInfo02Service,
            ISws_RTUInfoService sws_RTUInfoService,
            IDDayQuartZ01Service dDayQuartZ01Service,
            ISws_ValveWith01Service sws_ValveWith01Service,
            IDDayQuartZ02Service dDayQuartZ02Service,
            ISws_DeviceTemplateService sws_DeviceTemplateService,
            ISws_TemplateService sws_TemplateService

            )
        {
            stationService = sws_StationService;
            _sws_DeviceInfo01Service = sws_DeviceInfo01Service;
            _sws_GUIInfoService = sws_GUIInfoService;
            _monthQuartZ01Service = monthQuartZ01Service;
            _EventInfoService = sws_EventInfoService;
            _DataItemDetailService = sys_DataItemDetailService;
            _attachmentService = attachmentService;
            _sws_DataInfoService = sws_DataInfoService;
            cameraService = sws_cameraServiceService;
            _dayQuartZ01Service = dayQuartZ01Service;
            _sws_DeviceInfo02Service = sws_DeviceInfo02Service;
            _sws_RTUInfoService = sws_RTUInfoService;
            _dDayQuartZ01Service = dDayQuartZ01Service;
            valveService = sws_ValveWith01Service;
            _dDayQuartZ02Service = dDayQuartZ02Service;
            _sws_DeviceTemplateService = sws_DeviceTemplateService;
            _sws_TemplateService = sws_TemplateService;
        }
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult Index(int id)
        {
            var model = stationService.Query<Model.DataModels.SwsStation>(a => a.StationId == id).FirstOrDefault();

            if (model.InType == 1)
                return RedirectToAction("Detail01", "Sws_Detail", new
                {
                    Area = "Sws",
                    sid = model.StationId,
                    name = model.StationName,
                    lng = model.Lng,
                    lat = model.Lat,
                    showTab = 1
                });
            if (model.InType == 2)
                return RedirectToAction("Detail02", "Sws_Detail", new
                {
                    Area = "Sws",
                    sid = model.StationId,
                    name = model.StationName,
                    showTab = 1
                });
            else
                return RedirectToAction("Detail01", "Sws_Detail", new
                {
                    Area = "Sws",
                    sid = model.StationId,
                    name = model.StationName,
                    lng = model.Lng,
                    lat = model.Lat,
                    showTab = 1
                });
        }
        /// <summary>
        /// showTab=1显示泵站详情、showTab=2显示报警监控、showTab=3远程控制
        /// /// </summary>
        /// <param name="id"></param>
        /// <param name="showTab"></param>
        /// <returns></returns>
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult ShowDetail(int id, int showTab)
        {
            var model = stationService.Query<Model.DataModels.SwsStation>(a => a.StationId == id).FirstOrDefault();

            if (model.InType == 1)
                return RedirectToAction("Detail01", "Sws_Detail", new
                {
                    Area = "Sws",
                    sid = model.StationId,
                    name = model.StationName,
                    lng = model.Lng,
                    lat = model.Lat,
                    showTab = showTab
                });
            if (model.InType == 2)
                return RedirectToAction("Detail02", "Sws_Detail", new
                {
                    Area = "Sws",
                    sid = model.StationId,
                    name = model.StationName,
                    showTab = showTab
                });
            else
                return RedirectToAction("Detail01", "Sws_Detail", new
                {
                    Area = "Sws",
                    sid = model.StationId,
                    name = model.StationName,
                    lng = model.Lng,
                    lat = model.Lat,
                    showTab = showTab
                });
        }
        #region 二供详情页方法
        //详情页主界面
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult Detail01(int sid, string name, double lng, double lat, int showTab)
        {
            ViewBag.StationName = name;
            ViewBag.ShowTab = showTab;
            //分区信息
            var painfo = _sws_DeviceInfo01Service.Query<SwsDeviceInfo01>(r => r.StationId == sid).OrderBy(r => r.Partition).ToList();
            var partitionList = painfo.Select(r => r.Partition).ToList();
            ViewBag.sid = sid;
            //加载设备分区对应表

            if (partitionList.Count > 0)
            {
                ViewBag.parid = partitionList.FirstOrDefault();
            }
            else
            {
                ViewBag.parid = 1;
            }
            
            ViewBag.partitionList = partitionList;
            //报警信息
            var rtuid = _sws_DeviceInfo01Service.Query<SwsDeviceInfo01>(r => r.StationId == sid && r.Rtuid != null).Select(r => r.Rtuid).ToList();
            //var eventInfo = _EventInfoService.Query<SwsEventInfo>(r => rtuid.Contains(r.Rtuid)).ToList();
            List<dynamic> eventInfo = new List<dynamic>();
            if (rtuid.Count > 0)
            {
                var rtuids = string.Join(",", rtuid);
                eventInfo = _EventInfoService.LoadEventList(rtuids).ToList();
            }
            ViewBag.eventInfo = eventInfo;
            //泵房样貌
            List<string> suff = new List<string> { ".jpg", ".png", "jpeg" };
            var imgList = _attachmentService.Query<Attachment>(r => r.Affiliation == sid && suff.Contains(r.Suffix)).ToList();
            ViewBag.imgList = imgList;
            //总工艺图信息
            var guidInfo = stationService.Query<SwsStation>(r => r.StationId == sid).FirstOrDefault();
            var iscontoroller = guidInfo?.ControlMonitor;
            var iscontoroller_bengfang = guidInfo?.ControlMonitor_bengfang;
            ViewBag.iscontoroller = iscontoroller;
            ViewBag.iscontoroller_bengfang = iscontoroller_bengfang;
            if (guidInfo != null)
            {
                if (guidInfo.GuiNum != 0)
                {
                    var gui = _sws_GUIInfoService.Query<SwsGuiinfo>(r => r.Num == guidInfo.GuiNum).FirstOrDefault();
                    if (gui != null)
                    {
                        ViewBag.guiPage = gui.PageUrl;
                    }
                    else
                    {
                        var firgui = _sws_GUIInfoService.Query<SwsGuiinfo>(r => r.Num == painfo.FirstOrDefault().Gui).FirstOrDefault();
                        if (firgui != null)
                        {
                            ViewBag.guiPage = firgui.PageUrl;

                        }
                        else
                        {
                            ViewBag.guiPage = "";
                        }
                    }
                }
                else
                {
                    var firgui = _sws_GUIInfoService.Query<SwsGuiinfo>(r => r.Num == painfo.FirstOrDefault().Gui).FirstOrDefault();
                    if (firgui != null)
                    {
                        ViewBag.guiPage = firgui.PageUrl;

                    }
                    else
                    {
                        ViewBag.guiPage = "";
                    }
                }
            }
            else
            {
                //第一个分区的工艺图
                var firgui = _sws_GUIInfoService.Query<SwsGuiinfo>(r => r.Num == painfo.FirstOrDefault().Gui).FirstOrDefault();
                if (firgui != null)
                {
                    ViewBag.guiPage = firgui.PageUrl;

                }
                else
                {
                    ViewBag.guiPage = "";
                }

            }
            ViewBag.signalrHubs = signalrHubs;
            ViewBag.lng = lng;
            ViewBag.lat = lat;
            ViewBag.ArcgisUrl = StaticConstraint.ArcgisUrl;
            ViewBag.Threed = StaticConstraint.Threed;
            return View();
        }
        //查询泵房下所属的设备的泵数量，加载总工艺图
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult LadPartitionNum(int sid)
        {
            var deviceList = _sws_DeviceInfo01Service.Query<SwsDeviceInfo01>(r => r.StationId == sid && r.Partition != 6).OrderByDescending(r => r.PumpNum).ToList();
            return Json(new
            {
                deviceList
            });
        }
        //加载报警信息
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult LoadEvent(int sid)
        {
            //报警信息
            var rtuid = _sws_DeviceInfo01Service.Query<SwsDeviceInfo01>(r => r.StationId == sid && r.Rtuid != null).Select(r => r.Rtuid).ToList();
            //var eventInfo = _EventInfoService.Query<SwsEventInfo>(r => rtuid.Contains(r.Rtuid)).ToList();
            List<dynamic> eventInfo = new List<dynamic>();
            if (rtuid.Count > 0)
            {
                var rtuids = string.Join(",", rtuid);
                eventInfo = _EventInfoService.LoadEventList(rtuids).ToList();
            }
            return Json(new
            {
                eventInfo
            });
        }
        //加载报警信息
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult LoadZEvent(int sid)
        {
            //报警信息
            var rtuid = _sws_DeviceInfo02Service.Query<SwsDeviceInfo02>(r => r.StationId == sid && r.Rtuid != null).Select(r => r.Rtuid).ToList();
            //var eventInfo = _EventInfoService.Query<SwsEventInfo>(r => rtuid.Contains(r.Rtuid)).ToList();
            List<dynamic> eventInfo = new List<dynamic>();
            if (rtuid.Count > 0)
            {
                var rtuids = string.Join(",", rtuid);
                eventInfo = _EventInfoService.LoadEventList(rtuids).ToList();
            }
            return Json(new
            {
                eventInfo
            });
        }
        //根据分区获取详情页的工艺图
        [TypeFilter(typeof(IgonreActionFilter))]
        [HttpPost]
        public IActionResult LoadGuiInfo(int sid, int partionid)
        {
            var deviceInfo = _sws_DeviceInfo01Service.Query<SwsDeviceInfo01>(r => r.StationId == sid && r.Partition == partionid).FirstOrDefault();
            if (deviceInfo != null)
            {
                var guiPage = _sws_GUIInfoService.Query<SwsGuiinfo>(r => r.Num == deviceInfo.Gui).FirstOrDefault();
                if (guiPage != null)
                {
                    return Content(guiPage.PageUrl);
                }
                else
                {
                    return Content("");
                }
            }
            else
            {
                return Content("");
            }


        }
        //获取泵房的统计数据
        [TypeFilter(typeof(IgonreActionFilter))]
        [HttpPost]
        public IActionResult LoadStationStatistics(int sid)
        {
            #region 近7天数据
            ////近七天的总电量、总流量、吨水电耗
            //DateTime startTime = DateTime.Parse(DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd 00:00:00"));
            //DateTime endTime = DateTime.Parse(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd 23:59:59"));
            //decimal TonWater = 0;
            ////decimal allEnergyCon = 0;
            ////decimal allFlowCon = 0; 
            //var ids = _sws_DeviceInfo01Service.Query<SwsDeviceInfo01>(r => r.StationId == sid).Select(r => r.DeviceId).ToList();
            ////总电量 kWh 
            ////var EnergyCona = _dDayQuartZ01Service.Query<DdayQuartZ01>(r => ids.Contains(r.DeviceId) && r.UpdateTime >= startTime && r.UpdateTime <= endTime).Select(r=>r.EnergyCon).Sum();

            //var EnergyCon = _dDayQuartZ01Service.Query<DdayQuartZ01>(r => ids.Contains(r.DeviceId) && r.UpdateTime >= startTime && r.UpdateTime <= endTime).Sum(r => r.EnergyCon);
            ////foreach (var i in EnergyCon)
            ////{
            ////    allEnergyCon += i.EnergyCon;
            ////}
            ////总水量m³
            //var FlowCon = _dDayQuartZ01Service.Query<DdayQuartZ01>(r => ids.Contains(r.DeviceId) && r.UpdateTime >= startTime && r.UpdateTime <= endTime).Sum(r => r.FlowCon);
            ////foreach (var i in FlowCon)
            ////{
            ////    allFlowCon += i.FlowCon;
            ////}
            ////吨水电耗
            ////if (allFlowCon != 0)
            ////{
            ////    TonWater = Math.Round((allEnergyCon / allFlowCon), 2);
            ////}
            //if (EnergyCon != 0 && FlowCon != 0)
            //{
            //    TonWater = Math.Round((EnergyCon / FlowCon), 2);
            //}
            //return Json(new
            //{
            //    EnergyCon,
            //    FlowCon,
            //    TonWater
            //});
            #endregion
            #region 所有数据  
            decimal TonWater = 0;
            var ids = _sws_DeviceInfo01Service.Query<SwsDeviceInfo01>(r => r.StationId == sid).Select(r => r.DeviceId).ToList();

            var EnergyCon = _dDayQuartZ01Service.Query<DdayQuartZ01>(r => ids.Contains(r.DeviceId)).Sum(r => r.EnergyCon);

            var FlowCon = _dDayQuartZ01Service.Query<DdayQuartZ01>(r => ids.Contains(r.DeviceId)).Sum(r => r.FlowCon);

            if (EnergyCon != 0 && FlowCon != 0)
            {
                TonWater = Math.Round((EnergyCon / FlowCon), 2);
            }
            return Json(new
            {
                EnergyCon,
                FlowCon,
                TonWater
            });
            #endregion
        }
        //获取泵房的统计数据
        [TypeFilter(typeof(IgonreActionFilter))]
        [HttpPost]
        public IActionResult LoadZStationStatistics(int sid)
        {
            //近七天的总电量、总流量、吨水电耗
            //DateTime startTime = DateTime.Now.AddDays(-7);
            //DateTime endTime = DateTime.Now;
            DateTime startTime = DateTime.Parse(DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd 00:00:00"));
            DateTime endTime = DateTime.Parse(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd 23:59:59"));
            decimal TonWater = 0;
            //decimal allEnergyCon = 0;
            //decimal allFlowCon = 0;
            //应急方案，数据类型不匹配
            var ids = _sws_DeviceInfo02Service.Query<SwsDeviceInfo02>(r => r.StationId == sid).Select(r => r.DeviceId).ToList();
            //总电量 kWh

            var EnergyCon = _dDayQuartZ02Service.Query<DdayQuartZ02>(r => ids.Contains(r.DeviceId) && r.UpdateTime >= startTime && r.UpdateTime <= endTime).Sum(r => r.EnergyCon);

            //总水量m³
            var FlowCon = _dDayQuartZ02Service.Query<DdayQuartZ02>(r => ids.Contains(r.DeviceId) && r.UpdateTime >= startTime && r.UpdateTime <= endTime).Sum(r => r.FlowCon);

            if (EnergyCon != 0 && FlowCon != 0)
            {
                TonWater = Math.Round((EnergyCon / FlowCon), 2);
            }
            return Json(new
            {
                EnergyCon,
                FlowCon,
                TonWater
            });
        }
        //查询泵房下的分区设备信息
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult LoadStationJKPartuion(int sid, int partionid)
        {
            var enumfrequency = (int)Model.CustomizedClass.Enum.变频分类;
            var ftype = _DataItemDetailService.Query<SysDataItemDetail>(r => r.FItemId == enumfrequency).ToList();
            var deviceInfo = _sws_DeviceInfo01Service.Query<SwsDeviceInfo01>(r => r.StationId == sid && r.Partition == partionid).FirstOrDefault();
            if (deviceInfo == null)
            {
                montorGuiJK jkdata1 = new montorGuiJK();
                jkdata1.datetime = "--";//更新时间
                jkdata1.RtuID = 0;
                jkdata1.State = "离线";
                jkdata1.PressIN = new KeyValuePair<string, bool>("--", false);
                jkdata1.PressOut = new KeyValuePair<string, bool>("--", false);
                jkdata1.pumpState = new List<string>();
                jkdata1.pumpdatas = new List<PumpData>();
                jkdata1.totalflow = "--";
                jkdata1.totalpower = "--";
                jkdata1.inflow = "--";
                jkdata1.Temperature = "--";
                for (var i = 0; i < 5; i++)
                {
                    PumpData p = new PumpData();
                    p.eletric = new KeyValuePair<string, bool>("--", false);
                    p.frequency = new KeyValuePair<string, bool>("--", false);
                    p.runtime = new KeyValuePair<string, bool>("--", false);
                    jkdata1.pumpdatas.Add(p);
                    jkdata1.pumpState.Add("停止");
                }
                return Json(new
                {

                    jkdata = jkdata1

                });
            }
            var rtuID = deviceInfo.Rtuid;
            double allInFlow = 0;//累计流量
            int PumpNum = deviceInfo.PumpNum;//泵数量
            montorGuiJK jkdata = new montorGuiJK();
            if (rtuID != null)
            {
                string macJson = "{\"RTUID\":{'$in':[" + rtuID + "]}}";
                var collections = new MongoDBHelper<RtuJKInfo>(DateTime.Now.Year.ToString());
                var jklist = collections.Find("Sws_DeviceJKInfo", macJson, "UpdateTime").FirstOrDefault();
                List<SwsEventInfo> eventlist_all = new List<SwsEventInfo>();
                eventlist_all = _EventInfoService.Query<SwsEventInfo>(r => r.Rtuid == rtuID).ToList();
                var datajk = jklist;
                #region 状态
                var state = "离线";
                //if (eventlist_all.Count() > 0)
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
                        if (zstate == true && tstate == true && (DateTime.Now - Convert.ToDateTime(datajk.UpdateTime)).TotalMinutes <= timevalue && eventlist_all.Count() == 0)
                        {
                            state = "正常";
                        }
                        else
                        {
                            state = "故障";
                        }

                    }

                }
                else
                {
                    state = "离线";
                }
                if (datajk != null)
                {
                    var updatetime = datajk.UpdateTime;
                    jkdata.datetime = updatetime.ToString();//更新时间 
                    jkdata.RtuID = (int)rtuID;
                    jkdata.State = state;
                    var partion = partionid;
                    //进水压力
                    int pressinid = 2000 + (partion - 1) * 500;
                    jkdata.PressIN = new KeyValuePair<string, bool>(datajk.AnalogValues.ContainsKey(pressinid.ToString()) ? datajk.AnalogValues[pressinid.ToString()].ToString() : "--",
                     eventlist_all.Count > 0 ? (eventlist_all.Where(r => r.EventSource == pressinid && r.EventLevel != 0 && r.Rtuid == rtuID).Count() > 0 ? true : false) : false);
                    //出水压力
                    int pressout = 2001 + (partion - 1) * 500;
                    jkdata.PressOut = new KeyValuePair<string, bool>(datajk.AnalogValues.ContainsKey(pressout.ToString()) ? datajk.AnalogValues[pressout.ToString()].ToString() : "--",
                    eventlist_all.Count > 0 ? (eventlist_all.Where(r => r.EventSource == pressout && r.EventLevel != 0 && r.Rtuid == rtuID).Count() > 0 ? true : false) : false);
                    //水泵数据
                    #region 水泵数据
                    var fixfrequencyvalue = 2003 + (partion - 1) * 500;//频率
                    var frequencyvalue = 2004 + (partion - 1) * 500;//频率
                    var electricvalue = 2010 + (partion - 1) * 500;//电流
                    var pumpstatevalue = 5000 + (partion - 1) * 500;//方式
                    var guzhangvalue = 5003 + (partion - 1) * 500;//状态
                    var bianpinvalue = 5001 + (partion - 1) * 500;//变频
                    var hengpinvalue = 5127 + (partion - 1) * 500;//恒频
                    var gongpinvalue = 5002 + (partion - 1) * 500;//工频
                    var yunxingTime = 2017 + (partion - 1) * 500;//运行时间//datainfo 和2017重复

                    List<string> pumpState = new List<string>();//泵状态        
                    List<PumpData> pumpdataList = new List<PumpData>();//泵频率,电流、运行时间

                    var frequencyflg = deviceInfo.Frequency.ToString();
                    int j = 0;
                    var fname = ftype.Count() > 0 ? (ftype.Where(r => r.ItemValue == frequencyflg).FirstOrDefault()?.ItemName) : "单变频";
                    for (var i = 0; i < 4; i++)
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
                        if (state != "离线")
                        {
                            //if ((pump.PFault == "变频" || pump.PFault == "恒频") && pump.Frequency != "0")
                            //{
                            //    pumpState.Add("变频");

                            //}
                            if ((pump.PFault == "变频") && pump.Frequency != "0")
                            {
                                pumpState.Add("变频");

                            }
                            else if (pump.PFault == "恒频" && pump.Frequency != "0")
                            {
                                pumpState.Add("恒频");
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
                                pdatlist.frequency = new KeyValuePair<string, bool>(pump.Frequency, eventlist_all.Count > 0 ? (eventlist_all.Where(r => r.EventSource == fixfrequencyvalue && r.EventLevel != 0 && r.Rtuid == rtuID).Count() > 0 ? true : false) : false);
                            }
                            else
                            {
                                pdatlist.frequency = new KeyValuePair<string, bool>(pump.Frequency, false);
                            }
                        }
                        else
                        {
                            var fid = frequencyvalue + i;
                            pdatlist.frequency = new KeyValuePair<string, bool>(pump.Frequency, eventlist_all.Count > 0 ? (eventlist_all.Where(r => r.EventSource == fid && r.EventLevel != 0 && r.Rtuid == rtuID).Count() > 0 ? true : false) : false);
                        }
                        var runtimeid = yunxingTime + i;
                        pdatlist.runtime = new KeyValuePair<string, bool>(pump.RunHour.ToString(), eventlist_all.Count > 0 ? (eventlist_all.Where(r => r.EventSource == runtimeid && r.EventLevel != 0 && r.Rtuid == rtuID).Count() > 0 ? true : false) : false);
                        var eltricid = electricvalue + i;
                        pdatlist.eletric = new KeyValuePair<string, bool>(pump.Electric.ToString(), eventlist_all.Count > 0 ? (eventlist_all.Where(r => r.EventSource == eltricid && r.EventLevel != 0 && r.Rtuid == rtuID).Count() > 0 ? true : false) : false);
                        pumpdataList.Add(pdatlist);

                    }
                    //5泵
                    {
                        fixfrequencyvalue = 2003 + (partion - 1) * 500;//频率
                        frequencyvalue = 2008 + (partion - 1) * 500;//频率
                        electricvalue = 2014 + (partion - 1) * 500;//电流
                        pumpstatevalue = 5016 + (partion - 1) * 500;//方式
                        guzhangvalue = 5019 + (partion - 1) * 500;//状态
                        bianpinvalue = 5017 + (partion - 1) * 500;//变频
                        hengpinvalue = 5131 + (partion - 1) * 500;//恒频
                        gongpinvalue = 5018 + (partion - 1) * 500;//工频
                        yunxingTime = 2021 + (partion - 1) * 500;//运行时间//datainfo 和2017重复


                        PumpData pdatlist = new PumpData();

                        List<int> dataids = new List<int>();
                        if (fname.Contains("单变频"))
                        {
                            dataids = new List<int>() { pumpstatevalue, fixfrequencyvalue, electricvalue, guzhangvalue, bianpinvalue, hengpinvalue, gongpinvalue, yunxingTime };

                        }
                        else
                        {
                            dataids = new List<int>() { pumpstatevalue, frequencyvalue, electricvalue, guzhangvalue, bianpinvalue, hengpinvalue, gongpinvalue, yunxingTime };

                        }
                        Pump pump = GetDataByDataID(datajk, dataids);
                        if (state != "离线")
                        {
                            if ((pump.PFault == "变频") && pump.Frequency != "0")
                            {
                                pumpState.Add("变频");

                            }
                            else if (pump.PFault == "恒频" && pump.Frequency != "0")
                            {
                                pumpState.Add("恒频");
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
                            if (pumpState[4] == "变频" || pumpState[4] == "工频")
                            {
                                pdatlist.frequency = new KeyValuePair<string, bool>(pump.Frequency, eventlist_all.Count > 0 ? (eventlist_all.Where(r => r.EventSource == fixfrequencyvalue && r.EventLevel != 0 && r.Rtuid == rtuID).Count() > 0 ? true : false) : false);
                            }
                            else
                            {
                                pdatlist.frequency = new KeyValuePair<string, bool>(pump.Frequency, false);
                            }
                        }
                        else
                        {
                            var fid = frequencyvalue;
                            pdatlist.frequency = new KeyValuePair<string, bool>(pump.Frequency, eventlist_all.Count > 0 ? (eventlist_all.Where(r => r.EventSource == fid && r.EventLevel != 0 && r.Rtuid == rtuID).Count() > 0 ? true : false) : false);
                        }
                        var runtimeid = yunxingTime;
                        pdatlist.runtime = new KeyValuePair<string, bool>(pump.RunHour.ToString(), eventlist_all.Count > 0 ? (eventlist_all.Where(r => r.EventSource == runtimeid && r.EventLevel != 0 && r.Rtuid == rtuID).Count() > 0 ? true : false) : false);
                        var eltricid = electricvalue;
                        pdatlist.eletric = new KeyValuePair<string, bool>(pump.Electric.ToString(), eventlist_all.Count > 0 ? (eventlist_all.Where(r => r.EventSource == eltricid && r.EventLevel != 0 && r.Rtuid == rtuID).Count() > 0 ? true : false) : false);
                        pumpdataList.Add(pdatlist);


                    }
                    //6泵
                    {
                        fixfrequencyvalue = 2003 + (partion - 1) * 500;//频率
                        frequencyvalue = 2009 + (partion - 1) * 500;//频率
                        electricvalue = 2015 + (partion - 1) * 500;//电流
                        pumpstatevalue = 5020 + (partion - 1) * 500;//方式
                        guzhangvalue = 5023 + (partion - 1) * 500;//状态
                        bianpinvalue = 5021 + (partion - 1) * 500;//变频
                        hengpinvalue = 5251 + (partion - 1) * 500;//恒频
                        gongpinvalue = 5022 + (partion - 1) * 500;//工频
                        yunxingTime = 2022 + (partion - 1) * 500;//运行时间//datainfo 和2017重复


                        PumpData pdatlist = new PumpData();

                        List<int> dataids = new List<int>();
                        if (fname.Contains("单变频"))
                        {
                            dataids = new List<int>() { pumpstatevalue, fixfrequencyvalue, electricvalue, guzhangvalue, bianpinvalue, hengpinvalue, gongpinvalue, yunxingTime };

                        }
                        else
                        {
                            dataids = new List<int>() { pumpstatevalue, frequencyvalue, electricvalue, guzhangvalue, bianpinvalue, hengpinvalue, gongpinvalue, yunxingTime };

                        }
                        Pump pump = GetDataByDataID(datajk, dataids);
                        if (state != "离线")
                        {
                            if ((pump.PFault == "变频") && pump.Frequency != "0")
                            {
                                pumpState.Add("变频");

                            }
                            else if (pump.PFault == "恒频" && pump.Frequency != "0")
                            {
                                pumpState.Add("恒频");
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
                            if (pumpState[5] == "变频" || pumpState[5] == "工频")
                            {
                                pdatlist.frequency = new KeyValuePair<string, bool>(pump.Frequency, eventlist_all.Count > 0 ? (eventlist_all.Where(r => r.EventSource == fixfrequencyvalue && r.EventLevel != 0 && r.Rtuid == rtuID).Count() > 0 ? true : false) : false);
                            }
                            else
                            {
                                pdatlist.frequency = new KeyValuePair<string, bool>(pump.Frequency, false);
                            }
                        }
                        else
                        {
                            var fid = frequencyvalue;
                            pdatlist.frequency = new KeyValuePair<string, bool>(pump.Frequency, eventlist_all.Count > 0 ? (eventlist_all.Where(r => r.EventSource == fid && r.EventLevel != 0 && r.Rtuid == rtuID).Count() > 0 ? true : false) : false);
                        }
                        var runtimeid = yunxingTime;
                        pdatlist.runtime = new KeyValuePair<string, bool>(pump.RunHour.ToString(), eventlist_all.Count > 0 ? (eventlist_all.Where(r => r.EventSource == runtimeid && r.EventLevel != 0 && r.Rtuid == rtuID).Count() > 0 ? true : false) : false);
                        var eltricid = electricvalue;
                        pdatlist.eletric = new KeyValuePair<string, bool>(pump.Electric.ToString(), eventlist_all.Count > 0 ? (eventlist_all.Where(r => r.EventSource == eltricid && r.EventLevel != 0 && r.Rtuid == rtuID).Count() > 0 ? true : false) : false);
                        pumpdataList.Add(pdatlist);
                    }
                    //7泵
                    {
                        fixfrequencyvalue = 2003 + (partion - 1) * 500;//频率
                        frequencyvalue = 2164 + (partion - 1) * 500;//频率
                        electricvalue = 2154 + (partion - 1) * 500;//电流
                        pumpstatevalue = 2150 + (partion - 1) * 500;//方式  低区7#泵状态
                        guzhangvalue = 5249 + (partion - 1) * 500;// 故障
                        bianpinvalue = 5247 + (partion - 1) * 500;//变频
                        hengpinvalue = 5252 + (partion - 1) * 500;//恒频
                        gongpinvalue = 5248 + (partion - 1) * 500;//工频
                        yunxingTime = 2159 + (partion - 1) * 500;//运行时间//datainfo 和2017重复


                        PumpData pdatlist = new PumpData();

                        List<int> dataids = new List<int>();
                        if (fname.Contains("单变频"))
                        {
                            dataids = new List<int>() { pumpstatevalue, fixfrequencyvalue, electricvalue, guzhangvalue, bianpinvalue, hengpinvalue, gongpinvalue, yunxingTime };

                        }
                        else
                        {
                            dataids = new List<int>() { pumpstatevalue, frequencyvalue, electricvalue, guzhangvalue, bianpinvalue, hengpinvalue, gongpinvalue, yunxingTime };

                        }
                        Pump pump = GetDataByDataID(datajk, dataids);
                        if (state != "离线")
                        {
                            if ((pump.PFault == "变频") && pump.Frequency != "0")
                            {
                                pumpState.Add("变频");

                            }
                            else if (pump.PFault == "恒频" && pump.Frequency != "0")
                            {
                                pumpState.Add("恒频");
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
                            if (pumpState[6] == "变频" || pumpState[6] == "工频")
                            {
                                pdatlist.frequency = new KeyValuePair<string, bool>(pump.Frequency, eventlist_all.Count > 0 ? (eventlist_all.Where(r => r.EventSource == fixfrequencyvalue && r.EventLevel != 0 && r.Rtuid == rtuID).Count() > 0 ? true : false) : false);
                            }
                            else
                            {
                                pdatlist.frequency = new KeyValuePair<string, bool>(pump.Frequency, false);
                            }
                        }
                        else
                        {
                            var fid = frequencyvalue;
                            pdatlist.frequency = new KeyValuePair<string, bool>(pump.Frequency, eventlist_all.Count > 0 ? (eventlist_all.Where(r => r.EventSource == fid && r.EventLevel != 0 && r.Rtuid == rtuID).Count() > 0 ? true : false) : false);
                        }
                        var runtimeid = yunxingTime;
                        pdatlist.runtime = new KeyValuePair<string, bool>(pump.RunHour.ToString(), eventlist_all.Count > 0 ? (eventlist_all.Where(r => r.EventSource == runtimeid && r.EventLevel != 0 && r.Rtuid == rtuID).Count() > 0 ? true : false) : false);
                        var eltricid = electricvalue;
                        pdatlist.eletric = new KeyValuePair<string, bool>(pump.Electric.ToString(), eventlist_all.Count > 0 ? (eventlist_all.Where(r => r.EventSource == eltricid && r.EventLevel != 0 && r.Rtuid == rtuID).Count() > 0 ? true : false) : false);
                        pumpdataList.Add(pdatlist);
                    }
                    #endregion

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
                        allInFlow += inflowtemp;
                    }
                    //累计流量
                    var tfdata1 = datajk.AnalogValues.ContainsKey((2031 + (partion - 1) * 500).ToString()) ? datajk.AnalogValues[(2031 + (partion - 1) * 500).ToString()] : null;
                    var tfdata2 = datajk.AnalogValues.ContainsKey((2033 + (partion - 1) * 500).ToString()) ? datajk.AnalogValues[(2033 + (partion - 1) * 500).ToString()] : null;
                    var tfdata3 = datajk.AnalogValues.ContainsKey((2035 + (partion - 1) * 500).ToString()) ? datajk.AnalogValues[(2035 + (partion - 1) * 500).ToString()] : null;
                    var tfdata4 = datajk.AnalogValues.ContainsKey((2037 + (partion - 1) * 500).ToString()) ? datajk.AnalogValues[(2037 + (partion - 1) * 500).ToString()] : null;
                    if (tfdata1 == null && tfdata2 == null && tfdata3 == null && tfdata4 == null)
                    {
                        jkdata.totalflow = "--";
                    }
                    else
                    {
                        jkdata.totalflow = (Convert.ToDouble(tfdata1) + Convert.ToDouble(tfdata2) + Convert.ToDouble(tfdata3) + Convert.ToDouble(tfdata4)).ToString();
                    }
                    //累计电量
                    var pall = datajk.AnalogValues.ContainsKey((2085 + (partion - 1) * 500).ToString()) ? datajk.AnalogValues[(2085 + (partion - 1) * 500).ToString()] : null;
                    var pdata1 = datajk.AnalogValues.ContainsKey((2023 + (partion - 1) * 500).ToString()) ? datajk.AnalogValues[(2023 + (partion - 1) * 500).ToString()] : null;
                    var pdata2 = datajk.AnalogValues.ContainsKey((2024 + (partion - 1) * 500).ToString()) ? datajk.AnalogValues[(2024 + (partion - 1) * 500).ToString()] : null;
                    var pdata3 = datajk.AnalogValues.ContainsKey((2025 + (partion - 1) * 500).ToString()) ? datajk.AnalogValues[(2025 + (partion - 1) * 500).ToString()] : null;
                    var pdata4 = datajk.AnalogValues.ContainsKey((2026 + (partion - 1) * 500).ToString()) ? datajk.AnalogValues[(2026 + (partion - 1) * 500).ToString()] : null;
                    var pdata = Convert.ToDouble(pdata1) + Convert.ToDouble(pdata2) + Convert.ToDouble(pdata3) + Convert.ToDouble(pdata4);
                    jkdata.totalpower = (pdata == 0.0 ? (Convert.ToDouble(pall)).ToString() : pdata.ToString());
                    //浊度
                    var Turbidity = datajk.AnalogValues.ContainsKey((2051 + (partion - 1) * 500).ToString()) ? (datajk.AnalogValues[(2051 + (partion - 1) * 500).ToString()]).ToString() : "";
                    if (Turbidity == "")
                    {
                        Turbidity = datajk.AnalogValues.ContainsKey((4551).ToString()) ? datajk.AnalogValues[(4551).ToString()].ToString() : "--";
                    }
                    jkdata.Turbidity = Turbidity;
                    //余氯
                    var CL = datajk.AnalogValues.ContainsKey((2050 + (partion - 1) * 500).ToString()) ? (datajk.AnalogValues[(2050 + (partion - 1) * 500).ToString()]).ToString() : "";
                    if (CL == "")
                    {
                        CL = datajk.AnalogValues.ContainsKey((4550).ToString()) ? datajk.AnalogValues[(4550).ToString()].ToString() : "--";
                    }
                    jkdata.CL = CL;
                    //PH
                    var PH = datajk.AnalogValues.ContainsKey((2049 + (partion - 1) * 500).ToString()) ? (datajk.AnalogValues[(2049 + (partion - 1) * 500).ToString()]).ToString() : "";
                    if (PH == "")
                    {
                        PH = datajk.AnalogValues.ContainsKey((4549).ToString()) ? datajk.AnalogValues[(4549).ToString()].ToString() : "--";
                    }
                    jkdata.PH = PH;
                    //湿度
                    var Humidity = datajk.AnalogValues.ContainsKey((2053 + (partion - 1) * 500).ToString()) ? datajk.AnalogValues[(2053 + (partion - 1) * 500).ToString()] : "";
                    jkdata.Humidity = Humidity.ToString();
                    //温度
                    var Temperature = datajk.AnalogValues.ContainsKey((2052 + (partion - 1) * 500).ToString()) ? datajk.AnalogValues[(2052 + (partion - 1) * 500).ToString()] : "--";
                    jkdata.Temperature = Temperature.ToString();
                    //噪音
                    var Noise = datajk.AnalogValues.ContainsKey((2054 + (partion - 1) * 500).ToString()) ? datajk.AnalogValues[(2054 + (partion - 1) * 500).ToString()] : "";
                    jkdata.Noise = Noise.ToString();
                    //设定压力
                    var PressureSet = datajk.AnalogValues.ContainsKey((2002 + (partion - 1) * 500).ToString()) ? datajk.AnalogValues[(2002 + (partion - 1) * 500).ToString()] : "";
                    jkdata.PressureSet = PressureSet.ToString();
                    //液位高度
                    var LiquidHight = datajk.AnalogValues.ContainsKey((2055 + (partion - 1) * 500).ToString()) ? datajk.AnalogValues[(2055 + (partion - 1) * 500).ToString()] : "";
                    jkdata.LiquidHight = LiquidHight.ToString();
                    //保温层外温度
                    var BWCWWD1 = datajk.AnalogValues.ContainsKey((2109 + (partion - 1) * 500).ToString()) ? datajk.AnalogValues[(2109 + (partion - 1) * 500).ToString()] : "";
                    jkdata.BWCWWD1 = BWCWWD1.ToString();
                    //保温层内温度
                    var BWCNWD2 = datajk.AnalogValues.ContainsKey((2110 + (partion - 1) * 500).ToString()) ? datajk.AnalogValues[(2110 + (partion - 1) * 500).ToString()] : "";
                    jkdata.BWCNWD2 = BWCNWD2.ToString();
                    //弱电箱内温度
                    var RDXNWD = datajk.AnalogValues.ContainsKey((2111 + (partion - 1) * 500).ToString()) ? datajk.AnalogValues[(2111 + (partion - 1) * 500).ToString()] : "";
                    jkdata.RDXNWD = RDXNWD.ToString();

                    jkdata.pumpState = pumpState;
                    jkdata.pumpdatas = pumpdataList;
                }
                else
                {

                    jkdata.datetime = "--";//更新时间
                    jkdata.RtuID = (int)rtuID;
                    jkdata.State = state;
                    jkdata.PressIN = new KeyValuePair<string, bool>("--", false);
                    jkdata.PressOut = new KeyValuePair<string, bool>("--", false);
                    jkdata.pumpState = new List<string>();
                    jkdata.pumpdatas = new List<PumpData>();
                    jkdata.totalflow = "--";
                    jkdata.totalpower = "--";
                    jkdata.inflow = "--";
                    jkdata.Temperature = "--";
                    for (var i = 0; i < PumpNum; i++)
                    {
                        PumpData p = new PumpData();
                        p.eletric = new KeyValuePair<string, bool>("--", false);
                        p.frequency = new KeyValuePair<string, bool>("--", false);
                        p.runtime = new KeyValuePair<string, bool>("--", false);
                        jkdata.pumpdatas.Add(p);
                        jkdata.pumpState.Add("停止");
                    }
                }
                #endregion
            }
            else
            {
                montorGuiJK jkdata1 = new montorGuiJK();
                jkdata1.datetime = "--";//更新时间
                jkdata1.RtuID = 0;
                jkdata1.State = "离线";
                jkdata1.PressIN = new KeyValuePair<string, bool>("--", false);
                jkdata1.PressOut = new KeyValuePair<string, bool>("--", false);
                jkdata1.pumpState = new List<string>();
                jkdata1.pumpdatas = new List<PumpData>();
                jkdata1.totalflow = "--";
                jkdata1.totalpower = "--";
                jkdata1.inflow = "--";
                for (var i = 0; i < 5; i++)
                {
                    PumpData p = new PumpData();
                    p.eletric = new KeyValuePair<string, bool>("--", false);
                    p.frequency = new KeyValuePair<string, bool>("--", false);
                    p.runtime = new KeyValuePair<string, bool>("--", false);
                    jkdata1.pumpdatas.Add(p);
                    jkdata1.pumpState.Add("停止");
                }
                return Json(new
                {

                    jkdata = jkdata1

                });
            }
            return Json(new
            {

                jkdata

            });
        }


        //查询泵房下的分区设备信息 只查询分区信息
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult LoadStationJKPartuionNew(int sid, int partionid)
        {
            var enumfrequency = (int)Model.CustomizedClass.Enum.变频分类;
            var ftype = _DataItemDetailService.Query<SysDataItemDetail>(r => r.FItemId == enumfrequency).ToList();
            var deviceInfo = _sws_DeviceInfo01Service.Query<SwsDeviceInfo01>(r => r.StationId == sid && r.Partition == partionid).FirstOrDefault();
            List<EquipmentData> Equipmentstate = new List<EquipmentData>();
            if (deviceInfo == null)
            {
                montorGuiJK jkdata1 = new montorGuiJK();
                jkdata1.datetime = "--";//更新时间
                jkdata1.RtuID = 0;
                jkdata1.State = "离线";
                jkdata1.PressIN = new KeyValuePair<string, bool>("--", false);
                jkdata1.PressOut = new KeyValuePair<string, bool>("--", false);
                jkdata1.pumpState = new List<string>();
                jkdata1.pumpdatas = new List<PumpData>();
                jkdata1.totalflow = "--";
                jkdata1.totalpower = "--";
                jkdata1.inflow = "--";
                jkdata1.Temperature = "--";
                for (var i = 0; i < 5; i++)
                {
                    PumpData p = new PumpData();
                    p.eletric = new KeyValuePair<string, bool>("--", false);
                    p.frequency = new KeyValuePair<string, bool>("--", false);
                    p.runtime = new KeyValuePair<string, bool>("--", false);
                    jkdata1.pumpdatas.Add(p);
                    jkdata1.pumpState.Add("停止");
                }
                return Json(new
                {

                    jkdata = jkdata1,
                    Equipmentstate

                });
            }
            var rtuID = deviceInfo.Rtuid;
            double allInFlow = 0;//累计流量
            int PumpNum = deviceInfo.PumpNum;//泵数量
            montorGuiJK jkdata = new montorGuiJK();
            if (rtuID != null)
            {
                string macJson = "{\"RTUID\":{'$in':[" + rtuID + "]}}";
                var collections = new MongoDBHelper<RtuJKInfo>(DateTime.Now.Year.ToString());
                var jklist = collections.Find("Sws_DeviceJKInfo", macJson, "UpdateTime").FirstOrDefault();
                List<SwsEventInfo> eventlist_all = new List<SwsEventInfo>();
                eventlist_all = _EventInfoService.Query<SwsEventInfo>(r => r.Rtuid == rtuID).ToList();
                var datajk = jklist;
                #region 状态
                var state = "离线";
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
                        if (zstate == true && tstate == true && (DateTime.Now - Convert.ToDateTime(datajk.UpdateTime)).TotalMinutes <= timevalue && eventlist_all.Count() == 0)
                        {
                            state = "正常";
                        }
                        else
                        {
                            state = "故障";
                        }

                    }

                }
                else
                {
                    state = "离线";
                }
                if (datajk != null)
                {
                    var updatetime = datajk.UpdateTime;
                    jkdata.datetime = updatetime.ToString();//更新时间 
                    jkdata.RtuID = (int)rtuID;
                    jkdata.State = state;
                    var partion = partionid;
                    //进水压力
                    int pressinid = 2000 + (partion - 1) * 500;
                    jkdata.PressIN = new KeyValuePair<string, bool>(datajk.AnalogValues.ContainsKey(pressinid.ToString()) ? datajk.AnalogValues[pressinid.ToString()].ToString() : "--",
                     eventlist_all.Count > 0 ? (eventlist_all.Where(r => r.EventSource == pressinid && r.EventLevel != 0 && r.Rtuid == rtuID).Count() > 0 ? true : false) : false);
                    //出水压力
                    int pressout = 2001 + (partion - 1) * 500;
                    jkdata.PressOut = new KeyValuePair<string, bool>(datajk.AnalogValues.ContainsKey(pressout.ToString()) ? datajk.AnalogValues[pressout.ToString()].ToString() : "--",
                    eventlist_all.Count > 0 ? (eventlist_all.Where(r => r.EventSource == pressout && r.EventLevel != 0 && r.Rtuid == rtuID).Count() > 0 ? true : false) : false);
                    //水泵数据
                    #region 水泵数据
                    var fixfrequencyvalue = 2003 + (partion - 1) * 500;//频率
                    var frequencyvalue = 2004 + (partion - 1) * 500;//频率
                    var electricvalue = 2010 + (partion - 1) * 500;//电流
                    var pumpstatevalue = 5000 + (partion - 1) * 500;//方式
                    var guzhangvalue = 5003 + (partion - 1) * 500;//状态
                    var bianpinvalue = 5001 + (partion - 1) * 500;//变频
                    var hengpinvalue = 5127 + (partion - 1) * 500;//恒频
                    var gongpinvalue = 5002 + (partion - 1) * 500;//工频
                    var yunxingTime = 2017 + (partion - 1) * 500;//运行时间//datainfo 和2017重复

                    List<string> pumpState = new List<string>();//泵状态        
                    List<PumpData> pumpdataList = new List<PumpData>();//泵频率,电流、运行时间

                    var frequencyflg = deviceInfo.Frequency.ToString();
                    int j = 0;
                    var fname = ftype.Count() > 0 ? (ftype.Where(r => r.ItemValue == frequencyflg).FirstOrDefault()?.ItemName) : "单变频";
                    for (var i = 0; i < 4; i++)
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
                        if (state != "离线")
                        {
                            //if ((pump.PFault == "变频" || pump.PFault == "恒频") && pump.Frequency != "0")
                            //{
                            //    pumpState.Add("变频");

                            //}
                            if ((pump.PFault == "变频") && pump.Frequency != "0")
                            {
                                pumpState.Add("变频");

                            }
                            else if (pump.PFault == "恒频" && pump.Frequency != "0")
                            {
                                pumpState.Add("恒频");
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
                                pdatlist.frequency = new KeyValuePair<string, bool>(pump.Frequency, eventlist_all.Count > 0 ? (eventlist_all.Where(r => r.EventSource == fixfrequencyvalue && r.EventLevel != 0 && r.Rtuid == rtuID).Count() > 0 ? true : false) : false);
                            }
                            else
                            {
                                pdatlist.frequency = new KeyValuePair<string, bool>(pump.Frequency, false);
                            }
                        }
                        else
                        {
                            var fid = frequencyvalue + i;
                            pdatlist.frequency = new KeyValuePair<string, bool>(pump.Frequency, eventlist_all.Count > 0 ? (eventlist_all.Where(r => r.EventSource == fid && r.EventLevel != 0 && r.Rtuid == rtuID).Count() > 0 ? true : false) : false);
                        }
                        var runtimeid = yunxingTime + i;
                        pdatlist.runtime = new KeyValuePair<string, bool>(pump.RunHour.ToString(), eventlist_all.Count > 0 ? (eventlist_all.Where(r => r.EventSource == runtimeid && r.EventLevel != 0 && r.Rtuid == rtuID).Count() > 0 ? true : false) : false);
                        var eltricid = electricvalue + i;
                        pdatlist.eletric = new KeyValuePair<string, bool>(pump.Electric.ToString(), eventlist_all.Count > 0 ? (eventlist_all.Where(r => r.EventSource == eltricid && r.EventLevel != 0 && r.Rtuid == rtuID).Count() > 0 ? true : false) : false);
                        pumpdataList.Add(pdatlist);

                    }
                    //5泵
                    {
                        fixfrequencyvalue = 2003 + (partion - 1) * 500;//频率
                        frequencyvalue = 2008 + (partion - 1) * 500;//频率
                        electricvalue = 2014 + (partion - 1) * 500;//电流
                        pumpstatevalue = 5016 + (partion - 1) * 500;//方式
                        guzhangvalue = 5019 + (partion - 1) * 500;//状态
                        bianpinvalue = 5017 + (partion - 1) * 500;//变频
                        hengpinvalue = 5131 + (partion - 1) * 500;//恒频
                        gongpinvalue = 5018 + (partion - 1) * 500;//工频
                        yunxingTime = 2021 + (partion - 1) * 500;//运行时间//datainfo 和2017重复


                        PumpData pdatlist = new PumpData();

                        List<int> dataids = new List<int>();
                        if (fname.Contains("单变频"))
                        {
                            dataids = new List<int>() { pumpstatevalue, fixfrequencyvalue, electricvalue, guzhangvalue, bianpinvalue, hengpinvalue, gongpinvalue, yunxingTime };

                        }
                        else
                        {
                            dataids = new List<int>() { pumpstatevalue, frequencyvalue, electricvalue, guzhangvalue, bianpinvalue, hengpinvalue, gongpinvalue, yunxingTime };

                        }
                        Pump pump = GetDataByDataID(datajk, dataids);
                        if (state != "离线")
                        {
                            if ((pump.PFault == "变频") && pump.Frequency != "0")
                            {
                                pumpState.Add("变频");

                            }
                            else if (pump.PFault == "恒频" && pump.Frequency != "0")
                            {
                                pumpState.Add("恒频");
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
                            if (pumpState[4] == "变频" || pumpState[4] == "工频")
                            {
                                pdatlist.frequency = new KeyValuePair<string, bool>(pump.Frequency, eventlist_all.Count > 0 ? (eventlist_all.Where(r => r.EventSource == fixfrequencyvalue && r.EventLevel != 0 && r.Rtuid == rtuID).Count() > 0 ? true : false) : false);
                            }
                            else
                            {
                                pdatlist.frequency = new KeyValuePair<string, bool>(pump.Frequency, false);
                            }
                        }
                        else
                        {
                            var fid = frequencyvalue;
                            pdatlist.frequency = new KeyValuePair<string, bool>(pump.Frequency, eventlist_all.Count > 0 ? (eventlist_all.Where(r => r.EventSource == fid && r.EventLevel != 0 && r.Rtuid == rtuID).Count() > 0 ? true : false) : false);
                        }
                        var runtimeid = yunxingTime;
                        pdatlist.runtime = new KeyValuePair<string, bool>(pump.RunHour.ToString(), eventlist_all.Count > 0 ? (eventlist_all.Where(r => r.EventSource == runtimeid && r.EventLevel != 0 && r.Rtuid == rtuID).Count() > 0 ? true : false) : false);
                        var eltricid = electricvalue;
                        pdatlist.eletric = new KeyValuePair<string, bool>(pump.Electric.ToString(), eventlist_all.Count > 0 ? (eventlist_all.Where(r => r.EventSource == eltricid && r.EventLevel != 0 && r.Rtuid == rtuID).Count() > 0 ? true : false) : false);
                        pumpdataList.Add(pdatlist);


                    }
                    //6泵
                    {
                        fixfrequencyvalue = 2003 + (partion - 1) * 500;//频率
                        frequencyvalue = 2009 + (partion - 1) * 500;//频率
                        electricvalue = 2015 + (partion - 1) * 500;//电流
                        pumpstatevalue = 5020 + (partion - 1) * 500;//方式
                        guzhangvalue = 5023 + (partion - 1) * 500;//状态
                        bianpinvalue = 5021 + (partion - 1) * 500;//变频
                        hengpinvalue = 5251 + (partion - 1) * 500;//恒频
                        gongpinvalue = 5022 + (partion - 1) * 500;//工频
                        yunxingTime = 2022 + (partion - 1) * 500;//运行时间//datainfo 和2017重复


                        PumpData pdatlist = new PumpData();

                        List<int> dataids = new List<int>();
                        if (fname.Contains("单变频"))
                        {
                            dataids = new List<int>() { pumpstatevalue, fixfrequencyvalue, electricvalue, guzhangvalue, bianpinvalue, hengpinvalue, gongpinvalue, yunxingTime };

                        }
                        else
                        {
                            dataids = new List<int>() { pumpstatevalue, frequencyvalue, electricvalue, guzhangvalue, bianpinvalue, hengpinvalue, gongpinvalue, yunxingTime };

                        }
                        Pump pump = GetDataByDataID(datajk, dataids);
                        if (state != "离线")
                        {
                            if ((pump.PFault == "变频") && pump.Frequency != "0")
                            {
                                pumpState.Add("变频");

                            }
                            else if (pump.PFault == "恒频" && pump.Frequency != "0")
                            {
                                pumpState.Add("恒频");
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
                            if (pumpState[5] == "变频" || pumpState[5] == "工频")
                            {
                                pdatlist.frequency = new KeyValuePair<string, bool>(pump.Frequency, eventlist_all.Count > 0 ? (eventlist_all.Where(r => r.EventSource == fixfrequencyvalue && r.EventLevel != 0 && r.Rtuid == rtuID).Count() > 0 ? true : false) : false);
                            }
                            else
                            {
                                pdatlist.frequency = new KeyValuePair<string, bool>(pump.Frequency, false);
                            }
                        }
                        else
                        {
                            var fid = frequencyvalue;
                            pdatlist.frequency = new KeyValuePair<string, bool>(pump.Frequency, eventlist_all.Count > 0 ? (eventlist_all.Where(r => r.EventSource == fid && r.EventLevel != 0 && r.Rtuid == rtuID).Count() > 0 ? true : false) : false);
                        }
                        var runtimeid = yunxingTime;
                        pdatlist.runtime = new KeyValuePair<string, bool>(pump.RunHour.ToString(), eventlist_all.Count > 0 ? (eventlist_all.Where(r => r.EventSource == runtimeid && r.EventLevel != 0 && r.Rtuid == rtuID).Count() > 0 ? true : false) : false);
                        var eltricid = electricvalue;
                        pdatlist.eletric = new KeyValuePair<string, bool>(pump.Electric.ToString(), eventlist_all.Count > 0 ? (eventlist_all.Where(r => r.EventSource == eltricid && r.EventLevel != 0 && r.Rtuid == rtuID).Count() > 0 ? true : false) : false);
                        pumpdataList.Add(pdatlist);
                    }
                    //7泵
                    {
                        fixfrequencyvalue = 2003 + (partion - 1) * 500;//频率
                        frequencyvalue = 2164 + (partion - 1) * 500;//频率
                        electricvalue = 2154 + (partion - 1) * 500;//电流
                        pumpstatevalue = 2150 + (partion - 1) * 500;//方式  低区7#泵状态
                        guzhangvalue = 5249 + (partion - 1) * 500;// 故障
                        bianpinvalue = 5247 + (partion - 1) * 500;//变频
                        hengpinvalue = 5252 + (partion - 1) * 500;//恒频
                        gongpinvalue = 5248 + (partion - 1) * 500;//工频
                        yunxingTime = 2159 + (partion - 1) * 500;//运行时间//datainfo 和2017重复


                        PumpData pdatlist = new PumpData();

                        List<int> dataids = new List<int>();
                        if (fname.Contains("单变频"))
                        {
                            dataids = new List<int>() { pumpstatevalue, fixfrequencyvalue, electricvalue, guzhangvalue, bianpinvalue, hengpinvalue, gongpinvalue, yunxingTime };

                        }
                        else
                        {
                            dataids = new List<int>() { pumpstatevalue, frequencyvalue, electricvalue, guzhangvalue, bianpinvalue, hengpinvalue, gongpinvalue, yunxingTime };

                        }
                        Pump pump = GetDataByDataID(datajk, dataids);
                        if (state != "离线")
                        {
                            if ((pump.PFault == "变频") && pump.Frequency != "0")
                            {
                                pumpState.Add("变频");

                            }
                            else if (pump.PFault == "恒频" && pump.Frequency != "0")
                            {
                                pumpState.Add("恒频");
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
                            if (pumpState[6] == "变频" || pumpState[6] == "工频")
                            {
                                pdatlist.frequency = new KeyValuePair<string, bool>(pump.Frequency, eventlist_all.Count > 0 ? (eventlist_all.Where(r => r.EventSource == fixfrequencyvalue && r.EventLevel != 0 && r.Rtuid == rtuID).Count() > 0 ? true : false) : false);
                            }
                            else
                            {
                                pdatlist.frequency = new KeyValuePair<string, bool>(pump.Frequency, false);
                            }
                        }
                        else
                        {
                            var fid = frequencyvalue;
                            pdatlist.frequency = new KeyValuePair<string, bool>(pump.Frequency, eventlist_all.Count > 0 ? (eventlist_all.Where(r => r.EventSource == fid && r.EventLevel != 0 && r.Rtuid == rtuID).Count() > 0 ? true : false) : false);
                        }
                        var runtimeid = yunxingTime;
                        pdatlist.runtime = new KeyValuePair<string, bool>(pump.RunHour.ToString(), eventlist_all.Count > 0 ? (eventlist_all.Where(r => r.EventSource == runtimeid && r.EventLevel != 0 && r.Rtuid == rtuID).Count() > 0 ? true : false) : false);
                        var eltricid = electricvalue;
                        pdatlist.eletric = new KeyValuePair<string, bool>(pump.Electric.ToString(), eventlist_all.Count > 0 ? (eventlist_all.Where(r => r.EventSource == eltricid && r.EventLevel != 0 && r.Rtuid == rtuID).Count() > 0 ? true : false) : false);
                        pumpdataList.Add(pdatlist);
                    }
                    #endregion

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
                        allInFlow += inflowtemp;
                    }
                    //累计流量
                    var tfdata1 = datajk.AnalogValues.ContainsKey((2031 + (partion - 1) * 500).ToString()) ? datajk.AnalogValues[(2031 + (partion - 1) * 500).ToString()] : null;
                    var tfdata2 = datajk.AnalogValues.ContainsKey((2033 + (partion - 1) * 500).ToString()) ? datajk.AnalogValues[(2033 + (partion - 1) * 500).ToString()] : null;
                    var tfdata3 = datajk.AnalogValues.ContainsKey((2035 + (partion - 1) * 500).ToString()) ? datajk.AnalogValues[(2035 + (partion - 1) * 500).ToString()] : null;
                    var tfdata4 = datajk.AnalogValues.ContainsKey((2037 + (partion - 1) * 500).ToString()) ? datajk.AnalogValues[(2037 + (partion - 1) * 500).ToString()] : null;
                    if (tfdata1 == null && tfdata2 == null && tfdata3 == null && tfdata4 == null)
                    {
                        jkdata.totalflow = "--";
                    }
                    else
                    {
                        jkdata.totalflow = (Convert.ToDouble(tfdata1) + Convert.ToDouble(tfdata2) + Convert.ToDouble(tfdata3) + Convert.ToDouble(tfdata4)).ToString();
                    }
                    //累计电量
                    var pall = datajk.AnalogValues.ContainsKey((2085 + (partion - 1) * 500).ToString()) ? datajk.AnalogValues[(2085 + (partion - 1) * 500).ToString()] : null;
                    var pdata1 = datajk.AnalogValues.ContainsKey((2023 + (partion - 1) * 500).ToString()) ? datajk.AnalogValues[(2023 + (partion - 1) * 500).ToString()] : null;
                    var pdata2 = datajk.AnalogValues.ContainsKey((2024 + (partion - 1) * 500).ToString()) ? datajk.AnalogValues[(2024 + (partion - 1) * 500).ToString()] : null;
                    var pdata3 = datajk.AnalogValues.ContainsKey((2025 + (partion - 1) * 500).ToString()) ? datajk.AnalogValues[(2025 + (partion - 1) * 500).ToString()] : null;
                    var pdata4 = datajk.AnalogValues.ContainsKey((2026 + (partion - 1) * 500).ToString()) ? datajk.AnalogValues[(2026 + (partion - 1) * 500).ToString()] : null;
                    var pdata = Convert.ToDouble(pdata1) + Convert.ToDouble(pdata2) + Convert.ToDouble(pdata3) + Convert.ToDouble(pdata4);
                    jkdata.totalpower = (pdata == 0.0 ? (Convert.ToDouble(pall)).ToString() : pdata.ToString());
                    //浊度
                    var Turbidity = datajk.AnalogValues.ContainsKey((2051 + (partion - 1) * 500).ToString()) ? datajk.AnalogValues[(2051 + (partion - 1) * 500).ToString()] : "";
                    jkdata.Turbidity = Turbidity.ToString();
                    //余氯
                    var CL = datajk.AnalogValues.ContainsKey((2050 + (partion - 1) * 500).ToString()) ? datajk.AnalogValues[(2050 + (partion - 1) * 500).ToString()] : "";
                    jkdata.CL = CL.ToString();
                    //PH
                    var PH = datajk.AnalogValues.ContainsKey((2049 + (partion - 1) * 500).ToString()) ? datajk.AnalogValues[(2049 + (partion - 1) * 500).ToString()] : "";
                    jkdata.PH = PH.ToString();
                    //湿度
                    var Humidity = datajk.AnalogValues.ContainsKey((2053 + (partion - 1) * 500).ToString()) ? datajk.AnalogValues[(2053 + (partion - 1) * 500).ToString()] : "";
                    jkdata.Humidity = Humidity.ToString();
                    //温度
                    var Temperature = datajk.AnalogValues.ContainsKey((2052 + (partion - 1) * 500).ToString()) ? datajk.AnalogValues[(2052 + (partion - 1) * 500).ToString()] : "--";
                    jkdata.Temperature = Temperature.ToString();
                    //噪音
                    var Noise = datajk.AnalogValues.ContainsKey((2054 + (partion - 1) * 500).ToString()) ? datajk.AnalogValues[(2054 + (partion - 1) * 500).ToString()] : "";
                    jkdata.Noise = Noise.ToString();
                    //设定压力
                    var PressureSet = datajk.AnalogValues.ContainsKey((2002 + (partion - 1) * 500).ToString()) ? datajk.AnalogValues[(2002 + (partion - 1) * 500).ToString()] : "";
                    jkdata.PressureSet = PressureSet.ToString();
                    //液位高度
                    var LiquidHight = datajk.AnalogValues.ContainsKey((2055 + (partion - 1) * 500).ToString()) ? datajk.AnalogValues[(2055 + (partion - 1) * 500).ToString()] : "";
                    jkdata.LiquidHight = LiquidHight.ToString();
                    //保温层外温度
                    var BWCWWD1 = datajk.AnalogValues.ContainsKey((2109 + (partion - 1) * 500).ToString()) ? datajk.AnalogValues[(2109 + (partion - 1) * 500).ToString()] : "";
                    jkdata.BWCWWD1 = BWCWWD1.ToString();
                    //保温层内温度
                    var BWCNWD2 = datajk.AnalogValues.ContainsKey((2110 + (partion - 1) * 500).ToString()) ? datajk.AnalogValues[(2110 + (partion - 1) * 500).ToString()] : "";
                    jkdata.BWCNWD2 = BWCNWD2.ToString();
                    //弱电箱内温度
                    var RDXNWD = datajk.AnalogValues.ContainsKey((2111 + (partion - 1) * 500).ToString()) ? datajk.AnalogValues[(2111 + (partion - 1) * 500).ToString()] : "";
                    jkdata.RDXNWD = RDXNWD.ToString();

                    jkdata.pumpState = pumpState;
                    jkdata.pumpdatas = pumpdataList;


                    //模板数据 
                    //var mongoJKInfo = collections.Find("Sws_DeviceJKInfo", macJson, "UpdateTime").FirstOrDefault();
                    List<int> DataId = new List<int>();
                    if (deviceInfo != null)
                    {
                        List<SwsDataInfo> datainfo = new List<SwsDataInfo>();
                        SwsTemplate swsTemplate = new SwsTemplate();
                        var dyinfo = _sws_DeviceTemplateService.Query<SwsDeviceTemplate>(r => r.DeviceId == deviceInfo.DeviceId && r.DeviceType == 0).FirstOrDefault();
                        if (dyinfo != null)
                        {
                            swsTemplate = _sws_TemplateService.Query<SwsTemplate>(r => r.Id == dyinfo.TemplateId).FirstOrDefault();
                        }
                        else
                        {
                            swsTemplate.DataId = "1000,1001,2000,2001,2002,2004,2005,2006,2007,2030,2031,2038,2049,2050,2051,2085,2003,2010,2011,2012,2013";

                        }
                        var Datastring = swsTemplate.DataId.Split(',');
                        short enditem = 0;
                        foreach (var item in Datastring)
                        {
                            if (item == "1000" || item == "1001")
                            {
                                enditem = short.Parse(item);
                            }
                            else
                            {
                                switch (partionid)
                                {
                                    case 2:
                                        enditem = (short)(short.Parse(item) + 500);
                                        break;
                                    case 3:
                                        enditem = (short)(short.Parse(item) + 1000);
                                        break;
                                    case 4:
                                        enditem = (short)(short.Parse(item) + 1500);
                                        break;
                                    case 5:
                                        enditem = (short)(short.Parse(item) + 2000);
                                        break;
                                    default:
                                        enditem = short.Parse(item);
                                        break;
                                }
                            }

                            DataId.Add(enditem);
                        }
                        datainfo = _sws_DataInfoService.Query<SwsDataInfo>(r => DataId.Contains(r.DataId) && r.DeviceType == 1 && r.DataType == 1).ToList();
                        List<SwsDataInfo> dataInfosless = datainfo.Where(r => r.IsCumulation != true).ToList();
                        var Sws_DataInfo = dataInfosless;
                        List<SwsDataInfo> dataInfosmany = datainfo.Where(r => r.IsCumulation == true).ToList();
                        Sws_DataInfo.AddRange(dataInfosmany);


                        //电动阀状态
                        Dictionary<int, bool> dicValve = new Dictionary<int, bool>();//阀门对应dataid以及是否可调
                        if (Sws_DataInfo.Where(r => r.Cnname.Contains("电动阀")).Count() > 0)
                        {
                            var valves = valveService.Query<SwsValveWith01>(r => r.DeviceId == deviceInfo.DeviceId).ToList();
                            if (valves.Count > 0)
                            {
                                foreach (var item in valves)
                                {
                                    int dataidValve = 2056 + ((int)item.ValveNum - 1);
                                    dicValve.Add(dataidValve, item.IsAdjusted);

                                }
                            }
                        }
                        //dataid读取数据
                        if (jklist != null)
                        {
                            foreach (var item in Sws_DataInfo)
                            {
                                double? keyValue = 0;
                                if (item.DataType == 1)
                                {
                                    if (jklist.AnalogValues != null)
                                    {
                                        keyValue = jklist.AnalogValues.Keys.Contains(item.DataId.ToString()) ? double.Parse(jklist.AnalogValues[item.DataId.ToString()].ToString()) : 0;
                                    }
                                    if (item.Cnname.IndexOf("电动阀") > -1)
                                    {
                                        var avalue = "0";
                                        #region 电动阀

                                        if (dicValve.ContainsKey(item.DataId) && dicValve[item.DataId] == true)//可调节
                                        {
                                            if (keyValue == null)
                                            {
                                                avalue = "0%";
                                            }
                                            else
                                            {
                                                avalue = keyValue + "%";
                                            }
                                        }
                                        else
                                        {
                                            if (keyValue == 0)
                                            {

                                                avalue = "全开";
                                            }
                                            else if (keyValue == 1)
                                            {

                                                avalue = "全关";
                                            }
                                            else if (keyValue == 2)
                                            {
                                                avalue = "故障";

                                            }
                                            else if (keyValue == 3)
                                            {
                                                avalue = "开启中";

                                            }
                                            else if (keyValue == 4)
                                            {
                                                avalue = "关闭中";

                                            }
                                            else
                                            {
                                                avalue = "全关";
                                            }
                                        }

                                        EquipmentData equipment = new EquipmentData();
                                        equipment.AnalogValue = avalue;
                                        equipment.IsAlert = false;
                                        Equipmentstate.Add(equipment);

                                        #endregion
                                    }
                                    else
                                    {
                                        EquipmentData equipment = new EquipmentData();

                                        equipment.AnalogValue = keyValue.ToString();

                                        if (keyValue == null)
                                        {
                                            equipment.IsAlert = false;
                                        }

                                        Equipmentstate.Add(equipment);
                                    }
                                }
                                else//数字量
                                {
                                    if (jklist.DigitalValues != null)
                                    {
                                        keyValue = Convert.ToDouble(jklist.DigitalValues.Keys.Contains(item.DataId.ToString()) ? jklist.DigitalValues[item.DataId.ToString()] : null);
                                    }
                                    if (keyValue == null)
                                    {
                                        EquipmentData equipment = new EquipmentData();
                                        equipment.AnalogValue = "否";
                                        equipment.IsAlert = false;
                                        Equipmentstate.Add(equipment);
                                    }
                                    else
                                    {
                                        EquipmentData equipment = new EquipmentData();
                                        equipment.AnalogValue = keyValue.ToString() == "1" ? "是" : "否";
                                        equipment.IsAlert = false;
                                        Equipmentstate.Add(equipment);

                                    }
                                }

                            }
                        }


                    }
                    else
                    {

                    }
                }
                else
                {

                    jkdata.datetime = "--";//更新时间
                    jkdata.RtuID = (int)rtuID;
                    jkdata.State = state;
                    jkdata.PressIN = new KeyValuePair<string, bool>("--", false);
                    jkdata.PressOut = new KeyValuePair<string, bool>("--", false);
                    jkdata.pumpState = new List<string>();
                    jkdata.pumpdatas = new List<PumpData>();
                    jkdata.totalflow = "--";
                    jkdata.totalpower = "--";
                    jkdata.inflow = "--";
                    jkdata.Temperature = "--";
                    for (var i = 0; i < PumpNum; i++)
                    {
                        PumpData p = new PumpData();
                        p.eletric = new KeyValuePair<string, bool>("--", false);
                        p.frequency = new KeyValuePair<string, bool>("--", false);
                        p.runtime = new KeyValuePair<string, bool>("--", false);
                        jkdata.pumpdatas.Add(p);
                        jkdata.pumpState.Add("停止");
                    }
                }
                #endregion
            }
            else
            {
                montorGuiJK jkdata1 = new montorGuiJK();
                jkdata1.datetime = "--";//更新时间
                jkdata1.RtuID = 0;
                jkdata1.State = "离线";
                jkdata1.PressIN = new KeyValuePair<string, bool>("--", false);
                jkdata1.PressOut = new KeyValuePair<string, bool>("--", false);
                jkdata1.pumpState = new List<string>();
                jkdata1.pumpdatas = new List<PumpData>();
                jkdata1.totalflow = "--";
                jkdata1.totalpower = "--";
                jkdata1.inflow = "--";
                for (var i = 0; i < 5; i++)
                {
                    PumpData p = new PumpData();
                    p.eletric = new KeyValuePair<string, bool>("--", false);
                    p.frequency = new KeyValuePair<string, bool>("--", false);
                    p.runtime = new KeyValuePair<string, bool>("--", false);
                    jkdata1.pumpdatas.Add(p);
                    jkdata1.pumpState.Add("停止");
                }
                return Json(new
                {

                    jkdata = jkdata1

                });
            }
            return Json(new
            {

                jkdata,
                Equipmentstate
            });
        }
        //获取泵的状态LoadStationJKPartuion
        [TypeFilter(typeof(IgonreActionFilter))]
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
                pump.Electric = listdevicejkinfo.AnalogValues.Keys.Contains(dataid[2].ToString()) ? double.Parse(listdevicejkinfo.AnalogValues[dataid[2].ToString()].ToString()) : 0;
                pump.Frequency = listdevicejkinfo.AnalogValues.Keys.Contains(dataid[1].ToString()) ? listdevicejkinfo.AnalogValues[dataid[1].ToString()].ToString() : "0";
                pump.RunHour = listdevicejkinfo.AnalogValues.Keys.Contains(dataid[7].ToString()) ? double.Parse(listdevicejkinfo.AnalogValues[dataid[7].ToString()].ToString()) : 0;
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

        //加载模板信息
        public IActionResult LoadTemInfo(int sid, int partionid)
        {
            if (partionid == 0)
            {
                //泵房环境数据
                List<long> dataid = new List<long>();
                var dyinfo = _sws_DeviceTemplateService.Query<SwsDeviceTemplate>(r => r.DeviceId == sid && r.DeviceType == 1).FirstOrDefault();
                if (dyinfo != null)
                {
                    dataid = _sws_TemplateService.Query<SwsTemplate>(r => r.Id == dyinfo.TemplateId).FirstOrDefault().DataId.Split(',').ToList().ConvertAll(s => long.Parse(s));
                }
                else
                {
                    dataid = new List<long> { 4500, 4501, 4502, 4503, 4504, 4505, 4508, 4509, 4510, 4511, 4512 };
                }
                List<SwsDataInfo> datainfo = new List<SwsDataInfo>();
                datainfo = _sws_DataInfoService.Query<SwsDataInfo>(r => dataid.Contains(r.DataId) && r.DeviceType == 1 && r.DataType == 1).ToList();
                List<SwsDataInfo> dataInfosmany = datainfo.Where(r => r.IsCumulation == true).ToList();
                List<SwsDataInfo> dataInfosless = datainfo.Where(r => r.IsCumulation != true).ToList();
                var rel = new
                {
                    dataInfosmany,
                    dataInfosless
                };
                return Content(Newtonsoft.Json.JsonConvert.SerializeObject(rel));
            }
            else
            {
                //设备数据
                var deviceInfo = _sws_DeviceInfo01Service.Query<SwsDeviceInfo01>(r => r.StationId == sid && r.Partition == partionid).FirstOrDefault();
                List<int> DataId = new List<int>();
                if (deviceInfo != null)
                {
                    List<SwsDataInfo> datainfo = new List<SwsDataInfo>();
                    SwsTemplate swsTemplate = new SwsTemplate();
                    var dyinfo = _sws_DeviceTemplateService.Query<SwsDeviceTemplate>(r => r.DeviceId == deviceInfo.DeviceId && r.DeviceType == 0).FirstOrDefault();
                    if (dyinfo != null)
                    {
                        swsTemplate = _sws_TemplateService.Query<SwsTemplate>(r => r.Id == dyinfo.TemplateId).FirstOrDefault();
                    }
                    else
                    {
                        swsTemplate.DataId = "1000,1001,2000,2001,2002,2004,2005,2006,2007,2030,2031,2038,2049,2050,2051,2085,2003,2010,2011,2012,2013";

                    }
                    var Datastring = swsTemplate.DataId.Split(',');
                    short enditem = 0;
                    foreach (var item in Datastring)
                    {
                        if (item == "1000" || item == "1001")
                        {
                            enditem = short.Parse(item);
                        }
                        else
                        {
                            switch (partionid)
                            {
                                case 2:
                                    enditem = (short)(short.Parse(item) + 500);
                                    break;
                                case 3:
                                    enditem = (short)(short.Parse(item) + 1000);
                                    break;
                                case 4:
                                    enditem = (short)(short.Parse(item) + 1500);
                                    break;
                                case 5:
                                    enditem = (short)(short.Parse(item) + 2000);
                                    break;
                                default:
                                    enditem = short.Parse(item);
                                    break;
                            }
                        }

                        DataId.Add(enditem);
                    }
                    datainfo = _sws_DataInfoService.Query<SwsDataInfo>(r => DataId.Contains(r.DataId) && r.DeviceType == 1 && r.DataType == 1).ToList();
                    List<SwsDataInfo> dataInfosmany = datainfo.Where(r => r.IsCumulation == true).ToList();
                    List<SwsDataInfo> dataInfosless = datainfo.Where(r => r.IsCumulation != true).ToList();
                    var rel = new
                    {
                        dataInfosmany,
                        dataInfosless
                    };
                    return Content(Newtonsoft.Json.JsonConvert.SerializeObject(rel));
                }
                else
                {
                    return Content("");
                }

            }
        }
        //获取模板数据信息
        public ActionResult GetJKData(int sid, int partionid)
        {
            if (partionid == 0)
            {
                var deviceInfo = _sws_DeviceInfo01Service.Query<SwsDeviceInfo01>(r => r.StationId == sid && r.Partition == 6).FirstOrDefault();

                //查询mongo数据
                string macJson = "{\"RTUID\":{'$in':[" + deviceInfo?.Rtuid + "]}}";
                var collections = new MongoDBHelper<RtuJKInfo>(DateTime.Now.Year.ToString());
                var mongoJKInfo = collections.Find("Sws_DeviceJKInfo", macJson, "UpdateTime").FirstOrDefault();


                //泵房环境数据
                List<long> dataid = new List<long>();
                var dyinfo = _sws_DeviceTemplateService.Query<SwsDeviceTemplate>(r => r.DeviceId == sid && r.DeviceType == 1).FirstOrDefault();
                if (dyinfo != null)
                {
                    dataid = _sws_TemplateService.Query<SwsTemplate>(r => r.Id == dyinfo.TemplateId).FirstOrDefault().DataId.Split(',').ToList().ConvertAll(s => long.Parse(s));
                }
                else
                {
                    dataid = new List<long> { 4500, 4501, 4502, 4503, 4504, 4505, 4508, 4509, 4510, 4511, 4512 };
                }
                List<SwsDataInfo> datainfo = new List<SwsDataInfo>();
                datainfo = _sws_DataInfoService.Query<SwsDataInfo>(r => dataid.Contains(r.DataId) && r.DeviceType == 1 && r.DataType == 1).ToList();
                List<SwsDataInfo> dataInfosless = datainfo.Where(r => r.IsCumulation != true).ToList();
                var Sws_DataInfo = dataInfosless;
                List<SwsDataInfo> dataInfosmany = datainfo.Where(r => r.IsCumulation == true).ToList();
                Sws_DataInfo.AddRange(dataInfosmany);
                List<EquipmentData> Equipmentstate = new List<EquipmentData>();
                //电动阀状态
                Dictionary<int, bool> dicValve = new Dictionary<int, bool>();//阀门对应dataid以及是否可调
                if (Sws_DataInfo.Where(r => r.Cnname.Contains("电动阀")).Count() > 0)
                {
                    var valves = valveService.Query<SwsValveWith01>(r => r.DeviceId == deviceInfo.DeviceId).ToList();
                    if (valves.Count > 0)
                    {
                        foreach (var item in valves)
                        {
                            int dataidValve = 2056 + ((int)item.ValveNum - 1);
                            dicValve.Add(dataidValve, item.IsAdjusted);

                        }
                    }
                }
                //dataid读取数据
                if (mongoJKInfo != null)
                {
                    foreach (var item in Sws_DataInfo)
                    {
                        double? keyValue = 0;
                        if (item.DataType == 1)
                        {
                            if (mongoJKInfo.AnalogValues != null)
                            {
                                keyValue = mongoJKInfo.AnalogValues.Keys.Contains(item.DataId.ToString()) ? double.Parse(mongoJKInfo.AnalogValues[item.DataId.ToString()].ToString()) : 0;
                            }
                            if (item.Cnname.IndexOf("电动阀") > -1)
                            {
                                var avalue = "0";
                                #region 电动阀

                                if (dicValve.ContainsKey(item.DataId) && dicValve[item.DataId] == true)//可调节
                                {
                                    if (keyValue == null)
                                    {
                                        avalue = "0%";
                                    }
                                    else
                                    {
                                        avalue = keyValue + "%";
                                    }
                                }
                                else
                                {
                                    if (keyValue == 0)
                                    {

                                        avalue = "全开";
                                    }
                                    else if (keyValue == 1)
                                    {

                                        avalue = "全关";
                                    }
                                    else if (keyValue == 2)
                                    {
                                        avalue = "故障";

                                    }
                                    else if (keyValue == 3)
                                    {
                                        avalue = "开启中";

                                    }
                                    else if (keyValue == 4)
                                    {
                                        avalue = "关闭中";

                                    }
                                    else
                                    {
                                        avalue = "全关";
                                    }
                                }

                                EquipmentData equipment = new EquipmentData();
                                equipment.AnalogValue = avalue;
                                equipment.IsAlert = false;
                                Equipmentstate.Add(equipment);

                                #endregion
                            }
                            else
                            {
                                EquipmentData equipment = new EquipmentData();

                                equipment.AnalogValue = keyValue.ToString();

                                if (keyValue == null)
                                {
                                    equipment.IsAlert = false;
                                }

                                Equipmentstate.Add(equipment);
                            }
                        }
                        else//数字量
                        {
                            if (mongoJKInfo.DigitalValues != null)
                            {
                                keyValue = Convert.ToDouble(mongoJKInfo.DigitalValues.Keys.Contains(item.DataId.ToString()) ? mongoJKInfo.DigitalValues[item.DataId.ToString()] : null);
                            }
                            if (keyValue == null)
                            {
                                EquipmentData equipment = new EquipmentData();
                                equipment.AnalogValue = "否";
                                equipment.IsAlert = false;
                                Equipmentstate.Add(equipment);
                            }
                            else
                            {
                                EquipmentData equipment = new EquipmentData();
                                equipment.AnalogValue = keyValue.ToString() == "1" ? "是" : "否";
                                equipment.IsAlert = false;
                                Equipmentstate.Add(equipment);

                            }
                        }

                    }
                    //foreach (var item in Sws_DataInfo)
                    //{
                    //    double? keyValue = 0;
                    //    if (item.DataType == 1)
                    //    {
                    //        if (mongoJKInfo.AnalogValues != null)
                    //        {
                    //            keyValue = mongoJKInfo.AnalogValues.Keys.Contains(item.DataId.ToString()) ? double.Parse(mongoJKInfo.AnalogValues[item.DataId.ToString()].ToString()) : 0;
                    //        }

                    //        EquipmentData equipment = new EquipmentData();

                    //        equipment.AnalogValue = keyValue.ToString();

                    //        if (keyValue == null)
                    //        {
                    //            equipment.IsAlert = false;
                    //        }

                    //        Equipmentstate.Add(equipment);

                    //    }
                    //    else//数字量
                    //    {
                    //        if (mongoJKInfo.DigitalValues != null)
                    //        {
                    //            keyValue = Convert.ToDouble(mongoJKInfo.DigitalValues.Keys.Contains(item.DataId.ToString()) ? mongoJKInfo.DigitalValues[item.DataId.ToString()] : null);
                    //        }
                    //        if (keyValue == null)
                    //        {
                    //            EquipmentData equipment = new EquipmentData();
                    //            equipment.AnalogValue = "否";
                    //            equipment.IsAlert = false;
                    //            Equipmentstate.Add(equipment);
                    //        }
                    //        else
                    //        {
                    //            EquipmentData equipment = new EquipmentData();
                    //            equipment.AnalogValue = keyValue.ToString() == "1" ? "是" : "否";
                    //            equipment.IsAlert = false;
                    //            Equipmentstate.Add(equipment);

                    //        }
                    //    }

                    //}
                }


                var rel = new
                {
                    Equipmentstate
                };
                return Content(Newtonsoft.Json.JsonConvert.SerializeObject(rel));
            }
            else
            {
                //设备数据
                var deviceInfo = _sws_DeviceInfo01Service.Query<SwsDeviceInfo01>(r => r.StationId == sid && r.Partition == partionid).FirstOrDefault();
                //查询mongo数据
                string macJson = "{\"RTUID\":{'$in':[" + deviceInfo.Rtuid + "]}}";
                var collections = new MongoDBHelper<RtuJKInfo>(DateTime.Now.Year.ToString());
                var mongoJKInfo = collections.Find("Sws_DeviceJKInfo", macJson, "UpdateTime").FirstOrDefault();
                List<int> DataId = new List<int>();
                if (deviceInfo != null)
                {
                    List<SwsDataInfo> datainfo = new List<SwsDataInfo>();
                    SwsTemplate swsTemplate = new SwsTemplate();
                    var dyinfo = _sws_DeviceTemplateService.Query<SwsDeviceTemplate>(r => r.DeviceId == deviceInfo.DeviceId && r.DeviceType == 0).FirstOrDefault();
                    if (dyinfo != null)
                    {
                        swsTemplate = _sws_TemplateService.Query<SwsTemplate>(r => r.Id == dyinfo.TemplateId).FirstOrDefault();
                    }
                    else
                    {
                        swsTemplate.DataId = "1000,1001,2000,2001,2002,2004,2005,2006,2007,2030,2031,2038,2049,2050,2051,2085,2003,2010,2011,2012,2013";

                    }
                    var Datastring = swsTemplate.DataId.Split(',');
                    short enditem = 0;
                    foreach (var item in Datastring)
                    {
                        if (item == "1000" || item == "1001")
                        {
                            enditem = short.Parse(item);
                        }
                        else
                        {
                            switch (partionid)
                            {
                                case 2:
                                    enditem = (short)(short.Parse(item) + 500);
                                    break;
                                case 3:
                                    enditem = (short)(short.Parse(item) + 1000);
                                    break;
                                case 4:
                                    enditem = (short)(short.Parse(item) + 1500);
                                    break;
                                case 5:
                                    enditem = (short)(short.Parse(item) + 2000);
                                    break;
                                default:
                                    enditem = short.Parse(item);
                                    break;
                            }
                        }

                        DataId.Add(enditem);
                    }
                    datainfo = _sws_DataInfoService.Query<SwsDataInfo>(r => DataId.Contains(r.DataId) && r.DeviceType == 1 && r.DataType == 1).ToList();
                    List<SwsDataInfo> dataInfosless = datainfo.Where(r => r.IsCumulation != true).ToList();
                    var Sws_DataInfo = dataInfosless;
                    List<SwsDataInfo> dataInfosmany = datainfo.Where(r => r.IsCumulation == true).ToList();
                    Sws_DataInfo.AddRange(dataInfosmany);

                    List<EquipmentData> Equipmentstate = new List<EquipmentData>();
                    //电动阀状态
                    Dictionary<int, bool> dicValve = new Dictionary<int, bool>();//阀门对应dataid以及是否可调
                    if (Sws_DataInfo.Where(r => r.Cnname.Contains("电动阀")).Count() > 0)
                    {
                        var valves = valveService.Query<SwsValveWith01>(r => r.DeviceId == deviceInfo.DeviceId).ToList();
                        if (valves.Count > 0)
                        {
                            foreach (var item in valves)
                            {
                                int dataidValve = 2056 + ((int)item.ValveNum - 1);
                                dicValve.Add(dataidValve, item.IsAdjusted);

                            }
                        }
                    }
                    //dataid读取数据
                    if (mongoJKInfo != null)
                    {
                        foreach (var item in Sws_DataInfo)
                        {
                            double? keyValue = 0;
                            if (item.DataType == 1)
                            {
                                if (mongoJKInfo.AnalogValues != null)
                                {
                                    keyValue = mongoJKInfo.AnalogValues.Keys.Contains(item.DataId.ToString()) ? double.Parse(mongoJKInfo.AnalogValues[item.DataId.ToString()].ToString()) : 0;
                                }
                                if (item.Cnname.IndexOf("电动阀") > -1)
                                {
                                    var avalue = "0";
                                    #region 电动阀

                                    if (dicValve.ContainsKey(item.DataId) && dicValve[item.DataId] == true)//可调节
                                    {
                                        if (keyValue == null)
                                        {
                                            avalue = "0%";
                                        }
                                        else
                                        {
                                            avalue = keyValue + "%";
                                        }
                                    }
                                    else
                                    {
                                        if (keyValue == 0)
                                        {

                                            avalue = "全开";
                                        }
                                        else if (keyValue == 1)
                                        {

                                            avalue = "全关";
                                        }
                                        else if (keyValue == 2)
                                        {
                                            avalue = "故障";

                                        }
                                        else if (keyValue == 3)
                                        {
                                            avalue = "开启中";

                                        }
                                        else if (keyValue == 4)
                                        {
                                            avalue = "关闭中";

                                        }
                                        else
                                        {
                                            avalue = "全关";
                                        }
                                    }

                                    EquipmentData equipment = new EquipmentData();
                                    equipment.AnalogValue = avalue;
                                    equipment.IsAlert = false;
                                    Equipmentstate.Add(equipment);

                                    #endregion
                                }
                                else
                                {
                                    EquipmentData equipment = new EquipmentData();

                                    equipment.AnalogValue = keyValue.ToString();

                                    if (keyValue == null)
                                    {
                                        equipment.IsAlert = false;
                                    }

                                    Equipmentstate.Add(equipment);
                                }
                            }
                            else//数字量
                            {
                                if (mongoJKInfo.DigitalValues != null)
                                {
                                    keyValue = Convert.ToDouble(mongoJKInfo.DigitalValues.Keys.Contains(item.DataId.ToString()) ? mongoJKInfo.DigitalValues[item.DataId.ToString()] : null);
                                }
                                if (keyValue == null)
                                {
                                    EquipmentData equipment = new EquipmentData();
                                    equipment.AnalogValue = "否";
                                    equipment.IsAlert = false;
                                    Equipmentstate.Add(equipment);
                                }
                                else
                                {
                                    EquipmentData equipment = new EquipmentData();
                                    equipment.AnalogValue = keyValue.ToString() == "1" ? "是" : "否";
                                    equipment.IsAlert = false;
                                    Equipmentstate.Add(equipment);

                                }
                            }

                        }
                    }

                    var rel = new
                    {
                        Equipmentstate
                    };
                    return Content(Newtonsoft.Json.JsonConvert.SerializeObject(rel));
                }
                else
                {
                    return Content("");
                }

            }
        }



        #region 泵的数据实体
        [TypeFilter(typeof(IgonreActionFilter))]
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
        //查询泵房的字段信息  PH CL 浊度 
        //public IActionResult LoadStationJK(int sid)
        //{


        //}
        //泵房更多数据查询
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult LoadMoreData(int id)
        {
            List<PartitionData> data = new List<PartitionData>();
            //获取泵房RTUID
            var dataList = stationService.GetStaionById("", id);
            var alldataInfo = _sws_DataInfoService.Query<SwsDataInfo>(r => r.DeviceType == 1).ToList();
            var rtuidlist = dataList.Where(r => r.RTUID.ToString() != "").Select(r => r.RTUID).Distinct();
            var rtuids = string.Join(",", rtuidlist);
            string macJson = "{\"RTUID\":{'$in':[" + rtuids + "]}}";
            var collections = new MongoDBHelper<RtuJKInfo>(DateTime.Now.Year.ToString());
            var jklist = collections.Find("Sws_DeviceJKInfo", macJson, "UpdateTime");
            var stationdata = dataList.GroupBy(r => new { r.StationID, r.StationName, r.Lng, r.Lat }).OrderBy(r => r.Key.StationName).FirstOrDefault();
            List<string> partitionName = new List<string>();
            foreach (var it in stationdata)
            {
                PartitionData partitionData = new PartitionData();
                if (it.RTUID.ToString() != "")
                {
                    int Rtuidd = Convert.ToInt32(it.RTUID);
                    var partion = Convert.ToInt32(it.Partition.ToString());
                    var datajk = jklist.Where(r => r.RTUID == Rtuidd).FirstOrDefault();
                    if (partion == 1)
                    {
                        partitionData.partition = "低区";
                    }
                    else if (partion == 2)
                    {
                        partitionData.partition = "中区";
                    }
                    else if (partion == 3)
                    {
                        partitionData.partition = "高区";
                    }
                    else if (partion == 4)
                    {
                        partitionData.partition = "超高区";
                    }
                    else if (partion == 5)
                    {
                        partitionData.partition = "超超高区";
                    }
                    else if (partion == 6)
                    {
                        partitionData.partition = "泵房环境";
                    }
                    partitionName.Add(partitionData.partition);
                    int startid = 2000;//开始dataid
                    int endid = 2500;//结束dataid 模拟量
                    int tstartid = 5000;//开关量
                    int tendid = 5500;
                    List<KeyValuePair<string, object>> alist = new List<KeyValuePair<string, object>>();
                    List<PartitionDatdid> pdList = new List<PartitionDatdid>();
                    if (datajk != null && datajk.AnalogValues != null)
                    {
                        foreach (var ite in datajk.AnalogValues)
                        {
                            var dataInfo = alldataInfo.Where(r => r.DataId == int.Parse(ite.Key) && r.DeviceType == 1).FirstOrDefault();
                            if (dataInfo != null)
                            {
                                if ((startid + (partion - 1) * 500) <= int.Parse(ite.Key) && int.Parse(ite.Key) < (endid + (partion - 1) * 500))
                                {
                                    //KeyValuePair<string, object> keyValuePair = new KeyValuePair<string, object>(dataInfo.Cnname, ite.Value + dataInfo.Unit);
                                    PartitionDatdid partitionDatdid = new PartitionDatdid()
                                    {
                                        keyValue = ite.Value.ToString(),
                                        Unit = dataInfo.Unit,
                                        Cnname = dataInfo.Cnname
                                    };
                                    //alist.Add(keyValuePair);
                                    pdList.Add(partitionDatdid);
                                }
                            }


                        }
                    }
                    if (datajk != null && datajk.DigitalValues != null)
                    {
                        foreach (var ite in datajk.DigitalValues)
                        {
                            var dataInfo = alldataInfo.Where(r => r.DataId == int.Parse(ite.Key) && r.DeviceType == 1).FirstOrDefault();
                            if (dataInfo != null)
                            {
                                if ((tstartid + (partion - 1) * 500) <= int.Parse(ite.Key) && int.Parse(ite.Key) < (tendid + (partion - 1) * 500))
                                {
                                    //KeyValuePair<string, object> keyValuePair = new KeyValuePair<string, object>(dataInfo.Cnname, ite.Value);
                                    //alist.Add(keyValuePair);
                                    PartitionDatdid partitionDatdid = new PartitionDatdid()
                                    {
                                        keyValue = ite.Value.ToString(),
                                        Unit = dataInfo.Unit,
                                        Cnname = dataInfo.Cnname
                                    };
                                    pdList.Add(partitionDatdid);
                                }
                            }
                        }
                    }
                    //partitionData.akeyValuePairs = alist;
                    partitionData.partitionDatdids = pdList;
                    data.Add(partitionData);
                }
                else
                {
                }
            }
            ViewBag.partitionName = partitionName;
            ViewBag.data = data;
            return View();
        }
        //低中高区实体
        [TypeFilter(typeof(IgonreActionFilter))]
        public class PartitionData
        {
            public string partition { get; set; }
            //public List<KeyValuePair<string, object>> akeyValuePairs { get; set; }
            public List<PartitionDatdid> partitionDatdids { get; set; }
        }
        [TypeFilter(typeof(IgonreActionFilter))]
        public class PartitionDatdid
        {
            public string keyValue { get; set; }
            public string Cnname { get; set; }
            public string Unit { get; set; }
        }
        public class EquipmentData
        {
            public string AnalogValue { get; set; }
            public bool IsAlert { get; set; }
        }
        //查询泵房所有数据
        public IActionResult LoadAllData(int sid)
        {
            //获取泵房RTUID
            var dataList = stationService.GetStaionById("", sid);
            StationMarkerData jk = new StationMarkerData();
            List<string> deviceState = new List<string>();
            jk.PH = "0";
            jk.CL = "0";
            jk.Turbidity = "0";
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
                double allInFlow = 0;
                //所有设备
                var devicelist = stationdata.Where(r => r.StationID == sid);




                foreach (var it in stationdata)
                {

                    if (it.RTUID.ToString() != "")
                    {
                        int Rtuidd = Convert.ToInt32(it.RTUID);
                        AllDatdid jkdata = new AllDatdid();
                        var partion = Convert.ToInt32(it.Partition.ToString());
                        var datajk = jklist.Where(r => r.RTUID == Rtuidd).FirstOrDefault();
                        #region 状态
                        var state = "离线";
                        var eventdata = _EventInfoService.Query<SwsEventInfo>(r => r.Rtuid == Rtuidd && r.EventLevel != 0).ToList();
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
                        #endregion  
                        if (datajk != null)
                        {
                            var updatetime = datajk.UpdateTime;
                            #region 进水压力
                            int[] piids = { 2000, 2500, 3000, 3500 };
                            //低区进水压力 
                            jkdata.lPressIN = new KeyValuePair<string, bool>(datajk.AnalogValues.ContainsKey(piids[0].ToString()) ? datajk.AnalogValues[piids[0].ToString()].ToString() : "--",
                             eventdata.Count > 0 ? (eventdata.Where(r => r.EventSource == piids[0] && r.EventLevel != 0).Count() > 0 ? true : false) : false);
                            //中区进水压力 
                            jkdata.mPressIN = new KeyValuePair<string, bool>(datajk.AnalogValues.ContainsKey(piids[1].ToString()) ? datajk.AnalogValues[piids[1].ToString()].ToString() : "--",
                            eventdata.Count > 0 ? (eventdata.Where(r => r.EventSource == piids[1] && r.EventLevel != 0).Count() > 0 ? true : false) : false);
                            //高区进水压力 
                            jkdata.gPressIN = new KeyValuePair<string, bool>(datajk.AnalogValues.ContainsKey(piids[2].ToString()) ? datajk.AnalogValues[piids[2].ToString()].ToString() : "--",
                            eventdata.Count > 0 ? (eventdata.Where(r => r.EventSource == piids[2] && r.EventLevel != 0).Count() > 0 ? true : false) : false);
                            //超高区进水压力 
                            jkdata.cPressIN = new KeyValuePair<string, bool>(datajk.AnalogValues.ContainsKey(piids[3].ToString()) ? datajk.AnalogValues[piids[3].ToString()].ToString() : "--",
                            eventdata.Count > 0 ? (eventdata.Where(r => r.EventSource == piids[3] && r.EventLevel != 0).Count() > 0 ? true : false) : false);
                            #endregion

                            #region 出水压力
                            int[] poids = { 2001, 2501, 3001, 3501 };
                            jkdata.lPressOut = new KeyValuePair<string, bool>(datajk.AnalogValues.ContainsKey(poids[0].ToString()) ? datajk.AnalogValues[poids[0].ToString()].ToString() : "--",
                             eventdata.Count > 0 ? (eventdata.Where(r => r.EventSource == poids[0] && r.EventLevel != 0).Count() > 0 ? true : false) : false);
                            jkdata.mPressOut = new KeyValuePair<string, bool>(datajk.AnalogValues.ContainsKey(poids[1].ToString()) ? datajk.AnalogValues[poids[1].ToString()].ToString() : "--",
                            eventdata.Count > 0 ? (eventdata.Where(r => r.EventSource == poids[1] && r.EventLevel != 0).Count() > 0 ? true : false) : false);
                            jkdata.gPressOut = new KeyValuePair<string, bool>(datajk.AnalogValues.ContainsKey(poids[2].ToString()) ? datajk.AnalogValues[poids[2].ToString()].ToString() : "--",
                            eventdata.Count > 0 ? (eventdata.Where(r => r.EventSource == poids[2] && r.EventLevel != 0).Count() > 0 ? true : false) : false);
                            jkdata.cPressOut = new KeyValuePair<string, bool>(datajk.AnalogValues.ContainsKey(poids[3].ToString()) ? datajk.AnalogValues[poids[3].ToString()].ToString() : "--",
                            eventdata.Count > 0 ? (eventdata.Where(r => r.EventSource == poids[3] && r.EventLevel != 0).Count() > 0 ? true : false) : false);
                            #endregion

                            #region 设定压力
                            int[] psids = { 2002, 2502, 3002, 3502 };
                            jkdata.lPressSet = datajk.AnalogValues.ContainsKey(psids[0].ToString()) ? double.Parse((datajk.AnalogValues[psids[0].ToString()]).ToString()) : 0;
                            jkdata.mPressSet = datajk.AnalogValues.ContainsKey(psids[1].ToString()) ? double.Parse((datajk.AnalogValues[psids[1].ToString()]).ToString()) : 0;
                            jkdata.gPressSet = datajk.AnalogValues.ContainsKey(psids[2].ToString()) ? double.Parse((datajk.AnalogValues[psids[2].ToString()]).ToString()) : 0;
                            jkdata.cPressSet = datajk.AnalogValues.ContainsKey(psids[3].ToString()) ? double.Parse((datajk.AnalogValues[psids[3].ToString()]).ToString()) : 0;
                            #endregion



                            //瞬时流量
                            var infdata1 = datajk.AnalogValues.ContainsKey((2030 + (partion - 1) * 500).ToString()) ? datajk.AnalogValues[(2030 + (partion - 1) * 500).ToString()] : null;
                            var infdata2 = datajk.AnalogValues.ContainsKey((2032 + (partion - 1) * 500).ToString()) ? datajk.AnalogValues[(2032 + (partion - 1) * 500).ToString()] : null;
                            var infdata3 = datajk.AnalogValues.ContainsKey((2034 + (partion - 1) * 500).ToString()) ? datajk.AnalogValues[(2034 + (partion - 1) * 500).ToString()] : null;
                            var infdata4 = datajk.AnalogValues.ContainsKey((2036 + (partion - 1) * 500).ToString()) ? datajk.AnalogValues[(2036 + (partion - 1) * 500).ToString()] : null;
                            if (infdata1 == null && infdata2 == null && infdata3 == null && infdata4 == null)
                            {
                                jkdata.linflow = 0;
                            }
                            else
                            {
                                var inflowtemp = Math.Round(Convert.ToDouble(infdata1) + Convert.ToDouble(infdata2) + Convert.ToDouble(infdata3) + Convert.ToDouble(infdata4), 2);
                                jkdata.linflow = inflowtemp.ToString();
                            }

                            //累计电量
                            var pall = datajk.AnalogValues.ContainsKey((2085 + (partion - 1) * 500).ToString()) ? datajk.AnalogValues[(2085 + (partion - 1) * 500).ToString()] : null;
                            var pdata1 = datajk.AnalogValues.ContainsKey((2023 + (partion - 1) * 500).ToString()) ? datajk.AnalogValues[(2023 + (partion - 1) * 500).ToString()] : null;
                            var pdata2 = datajk.AnalogValues.ContainsKey((2024 + (partion - 1) * 500).ToString()) ? datajk.AnalogValues[(2024 + (partion - 1) * 500).ToString()] : null;
                            var pdata3 = datajk.AnalogValues.ContainsKey((2025 + (partion - 1) * 500).ToString()) ? datajk.AnalogValues[(2025 + (partion - 1) * 500).ToString()] : null;
                            var pdata4 = datajk.AnalogValues.ContainsKey((2026 + (partion - 1) * 500).ToString()) ? datajk.AnalogValues[(2026 + (partion - 1) * 500).ToString()] : null;
                            var pdata = Convert.ToDouble(pdata1) + Convert.ToDouble(pdata2) + Convert.ToDouble(pdata3) + Convert.ToDouble(pdata4);
                            allInFlow += pdata;
                        }
                        else
                        {
                        }

                    }
                    else
                    {
                        mapmontorJK jkdata = new mapmontorJK();
                        jkdata.devicename = "";
                        jkdata.DeviceID = 0;
                        jkdata.datetime = "--";//更新时间
                        jkdata.RtuID = 0;
                        jkdata.State = "离线";
                        jkdata.PressIN = new KeyValuePair<string, bool>("--", false);
                        jkdata.PressOut = new KeyValuePair<string, bool>("--", false);
                        jkdata.inflow = "--";
                        jkdata.Partition = 1;
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
                jk.allInFlow = Math.Round(allInFlow, 2);
            }
            return Json(new
            {
                jk

            });
        }
        //查询泵房所有数据
        public IActionResult LoadAllDataByPar(int sid)
        {
            //获取泵房的3D工艺图
            var stationInfo = stationService.Query<SwsStation>(r => r.StationId == sid).FirstOrDefault();
            var guiInfo = _sws_GUIInfoService.Query<SwsGuiinfo>(r => r.Num == stationInfo.Gui3dnum).FirstOrDefault();
            List<PartitionData> data = new List<PartitionData>();
            //获取泵房RTUID
            var dataList = stationService.GetStaionById("", sid);
            var alldataInfo = _sws_DataInfoService.Query<SwsDataInfo>(r => r.DeviceType == 1).ToList();
            var rtuidlist = dataList.Where(r => r.RTUID.ToString() != "").Select(r => r.RTUID).Distinct();
            var rtuids = string.Join(",", rtuidlist);
            string macJson = "{\"RTUID\":{'$in':[" + rtuids + "]}}";
            var collections = new MongoDBHelper<RtuJKInfo>(DateTime.Now.Year.ToString());
            var jklist = collections.Find("Sws_DeviceJKInfo", macJson, "UpdateTime");
            var stationdata = dataList.GroupBy(r => new { r.StationID, r.StationName, r.Lng, r.Lat }).OrderBy(r => r.Key.StationName).FirstOrDefault();
            List<string> partitionName = new List<string>();
            foreach (var it in stationdata)
            {
                PartitionData partitionData = new PartitionData();
                if (it.RTUID.ToString() != "")
                {
                    int Rtuidd = Convert.ToInt32(it.RTUID);
                    var partion = Convert.ToInt32(it.Partition.ToString());
                    var datajk = jklist.Where(r => r.RTUID == Rtuidd).FirstOrDefault();
                    if (partion == 1)
                    {
                        partitionData.partition = "低区";
                    }
                    else if (partion == 2)
                    {
                        partitionData.partition = "中区";
                    }
                    else if (partion == 3)
                    {
                        partitionData.partition = "高区";
                    }
                    else if (partion == 4)
                    {
                        partitionData.partition = "超区";
                    }
                    else if (partion == 5)
                    {
                        partitionData.partition = "超超高区";
                    }
                    else if (partion == 6)
                    {
                        partitionData.partition = "智能泵房";
                    }
                    partitionName.Add(partitionData.partition);
                    List<KeyValuePair<string, object>> alist = new List<KeyValuePair<string, object>>();
                    List<PartitionDatdid> pdList = new List<PartitionDatdid>();
                    //需要查询的数据
                    if (partion == 6)
                    {
                        string[] strArr = { "4500", "4501", "4502" };
                        int startid = 4500;//开始dataid
                        int endid = 4503;//结束dataid 模拟量
                        if (datajk != null && datajk.AnalogValues != null)
                        {
                            foreach (var ite in datajk.AnalogValues)
                            {
                                var dataInfo = alldataInfo.Where(r => r.DataId == int.Parse(ite.Key) && r.DeviceType == 1).FirstOrDefault();
                                if (dataInfo != null)
                                {
                                    if ((startid + (partion - 1) * 500) <= int.Parse(ite.Key) && int.Parse(ite.Key) < (endid + (partion - 1) * 500))
                                    {
                                        PartitionDatdid partitionDatdid = new PartitionDatdid()
                                        {
                                            keyValue = ite.Value.ToString(),
                                            Unit = dataInfo.Unit,
                                            Cnname = dataInfo.Cnname.Substring(2, dataInfo.Cnname.Length - 2)
                                        };
                                        pdList.Add(partitionDatdid);
                                    }
                                }
                            }
                            ////瞬时流量为多个泵的加值 
                            //PartitionDatdid partitionDatdid1 = new PartitionDatdid();

                            //var infdata1 = datajk.AnalogValues.ContainsKey((4510 + (partion - 1) * 500).ToString()) ? datajk.AnalogValues[(4510 + (partion - 1) * 500).ToString()] : null;
                            //var infdata2 = datajk.AnalogValues.ContainsKey((2032 + (partion - 1) * 500).ToString()) ? datajk.AnalogValues[(2032 + (partion - 1) * 500).ToString()] : null;
                            //var infdata3 = datajk.AnalogValues.ContainsKey((2034 + (partion - 1) * 500).ToString()) ? datajk.AnalogValues[(2034 + (partion - 1) * 500).ToString()] : null;
                            //var infdata4 = datajk.AnalogValues.ContainsKey((2036 + (partion - 1) * 500).ToString()) ? datajk.AnalogValues[(2036 + (partion - 1) * 500).ToString()] : null;
                            //if (infdata1 == null && infdata2 == null && infdata3 == null && infdata4 == null)
                            //{
                            //    partitionDatdid1.keyValue = "0";
                            //}
                            //else
                            //{
                            //    var inflowtemp = Math.Round(Convert.ToDouble(infdata1) + Convert.ToDouble(infdata2) + Convert.ToDouble(infdata3) + Convert.ToDouble(infdata4), 2);
                            //    partitionDatdid1.keyValue = inflowtemp.ToString();
                            //}
                            //partitionDatdid1.Cnname = "瞬时流量";
                            //partitionDatdid1.Unit = "m³/h";
                            //pdList.Add(partitionDatdid1);
                        }



                        partitionData.partitionDatdids = pdList;
                        data.Add(partitionData);
                    }
                    else
                    {
                        string[] strArr = { "2000", "2001", "2002" };
                        int startid = 2000;//开始dataid
                        int endid = 2003;//结束dataid 模拟量
                        if (datajk != null && datajk.AnalogValues != null)
                        {
                            foreach (var ite in datajk.AnalogValues)
                            {
                                var dataInfo = alldataInfo.Where(r => r.DataId == int.Parse(ite.Key) && r.DeviceType == 1).FirstOrDefault();
                                if (dataInfo != null)
                                {
                                    if ((startid + (partion - 1) * 500) <= int.Parse(ite.Key) && int.Parse(ite.Key) < (endid + (partion - 1) * 500))
                                    {
                                        PartitionDatdid partitionDatdid = new PartitionDatdid()
                                        {
                                            keyValue = ite.Value.ToString(),
                                            Unit = dataInfo.Unit,
                                            Cnname = dataInfo.Cnname.Substring(2, dataInfo.Cnname.Length - 2)
                                        };
                                        pdList.Add(partitionDatdid);
                                    }
                                }
                            }
                            //瞬时流量为多个泵的加值 
                            PartitionDatdid partitionDatdid1 = new PartitionDatdid();

                            var infdata1 = datajk.AnalogValues.ContainsKey((2030 + (partion - 1) * 500).ToString()) ? datajk.AnalogValues[(2030 + (partion - 1) * 500).ToString()] : null;
                            var infdata2 = datajk.AnalogValues.ContainsKey((2032 + (partion - 1) * 500).ToString()) ? datajk.AnalogValues[(2032 + (partion - 1) * 500).ToString()] : null;
                            var infdata3 = datajk.AnalogValues.ContainsKey((2034 + (partion - 1) * 500).ToString()) ? datajk.AnalogValues[(2034 + (partion - 1) * 500).ToString()] : null;
                            var infdata4 = datajk.AnalogValues.ContainsKey((2036 + (partion - 1) * 500).ToString()) ? datajk.AnalogValues[(2036 + (partion - 1) * 500).ToString()] : null;
                            if (infdata1 == null && infdata2 == null && infdata3 == null && infdata4 == null)
                            {
                                partitionDatdid1.keyValue = "0";
                            }
                            else
                            {
                                var inflowtemp = Math.Round(Convert.ToDouble(infdata1) + Convert.ToDouble(infdata2) + Convert.ToDouble(infdata3) + Convert.ToDouble(infdata4), 2);
                                partitionDatdid1.keyValue = inflowtemp.ToString();
                            }
                            partitionDatdid1.Cnname = "瞬时流量";
                            partitionDatdid1.Unit = "m³/h";
                            pdList.Add(partitionDatdid1);
                        }



                        partitionData.partitionDatdids = pdList;
                        data.Add(partitionData);
                    }

                }
                else
                {
                }
            }
            return Json(new
            {
                partitionName,
                data,
                guiInfo = guiInfo?.ImageUrl

            });
        }

        public class AllDatdid
        {
            //进水压力 
            public KeyValuePair<string, bool> lPressIN { get; set; }
            public KeyValuePair<string, bool> mPressIN { get; set; }
            public KeyValuePair<string, bool> gPressIN { get; set; }
            public KeyValuePair<string, bool> cPressIN { get; set; }
            //出水压力
            public KeyValuePair<string, bool> lPressOut { get; set; }
            public KeyValuePair<string, bool> mPressOut { get; set; }
            public KeyValuePair<string, bool> gPressOut { get; set; }
            public KeyValuePair<string, bool> cPressOut { get; set; }
            //设定压力
            public double lPressSet { get; set; }
            public double mPressSet { get; set; }
            public double gPressSet { get; set; }
            public double cPressSet { get; set; }
            //瞬时流量
            public double linflow { get; set; }
            public double minflow { get; set; }
            public double ginflow { get; set; }
            public double cinflow { get; set; }
            //累计流量
            public double lTotalFlow { get; set; }
            public double mTotalFlow { get; set; }
            public double gTotalFlow { get; set; }
            public double cTotalFlow { get; set; }
        }
        //查询泵房的实时报警数据
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult LoadEventInfo(int sid)
        {
            var rtuid = _sws_DeviceInfo01Service.Query<SwsDeviceInfo01>(r => r.StationId == sid && r.Rtuid != null).Select(r => r.Rtuid).ToList();
            var eventInfo = _EventInfoService.Query<SwsEventInfo>(r => rtuid.Contains(r.Rtuid)).ToList();
            return Json(new
            {
                eventInfo
            });
        }
        //获取地图鹰眼数据
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult LoadMapInfo(int sid)
        {
            //获取泵房RTUID
            var dataList = stationService.GetStaionById("", sid);
            StationMarkerData jk = new StationMarkerData();
            List<string> deviceState = new List<string>();
            jk.PH = "0";
            jk.CL = "0";
            jk.Turbidity = "0";
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
                double allInFlow = 0;
                //所有设备
                var devicelist = stationdata.Where(r => r.StationID == sid);
                foreach (var it in stationdata)
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
                            //PH
                            var cph = datajk.AnalogValues.ContainsKey((2049 + (partion - 1) * 500).ToString()) ? datajk.AnalogValues[(2049 + (partion - 1) * 500).ToString()] : 0;
                            if (cph != 0)
                            {
                                jk.PH = cph.ToString();
                            }
                            //cl
                            var ccl = datajk.AnalogValues.ContainsKey((2050 + (partion - 1) * 500).ToString()) ? datajk.AnalogValues[(2050 + (partion - 1) * 500).ToString()] : 0;
                            if (ccl != 0)
                            {
                                jk.CL = ccl.ToString();
                            }
                            //PH
                            var ctb = datajk.AnalogValues.ContainsKey((2051 + (partion - 1) * 500).ToString()) ? datajk.AnalogValues[(2051 + (partion - 1) * 500).ToString()] : 0;
                            if (ctb != 0)
                            {
                                jk.Turbidity = ctb.ToString();
                            }

                            if (it.Partition == 6)
                            {
                                jk.Turbidity = datajk.AnalogValues.ContainsKey((4551).ToString()) ? datajk.AnalogValues[(4551).ToString()].ToString() : "--";
                                jk.CL = datajk.AnalogValues.ContainsKey((4550).ToString()) ? datajk.AnalogValues[(4550).ToString()].ToString() : "--";
                                jk.PH = datajk.AnalogValues.ContainsKey((4549).ToString()) ? datajk.AnalogValues[(4549).ToString()].ToString() : "--";
                                jk.allInFlow = datajk.AnalogValues.ContainsKey((4512).ToString()) ? double.Parse(datajk.AnalogValues[(4512).ToString()].ToString()) : 0;
                            }

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

                            //累计电量
                            var pall = datajk.AnalogValues.ContainsKey((2085 + (partion - 1) * 500).ToString()) ? datajk.AnalogValues[(2085 + (partion - 1) * 500).ToString()] : null;
                            var pdata1 = datajk.AnalogValues.ContainsKey((2023 + (partion - 1) * 500).ToString()) ? datajk.AnalogValues[(2023 + (partion - 1) * 500).ToString()] : null;
                            var pdata2 = datajk.AnalogValues.ContainsKey((2024 + (partion - 1) * 500).ToString()) ? datajk.AnalogValues[(2024 + (partion - 1) * 500).ToString()] : null;
                            var pdata3 = datajk.AnalogValues.ContainsKey((2025 + (partion - 1) * 500).ToString()) ? datajk.AnalogValues[(2025 + (partion - 1) * 500).ToString()] : null;
                            var pdata4 = datajk.AnalogValues.ContainsKey((2026 + (partion - 1) * 500).ToString()) ? datajk.AnalogValues[(2026 + (partion - 1) * 500).ToString()] : null;
                            var pdata = Convert.ToDouble(pdata1) + Convert.ToDouble(pdata2) + Convert.ToDouble(pdata3) + Convert.ToDouble(pdata4);
                            allInFlow += pdata;
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
                        jkdata.devicename = "";
                        jkdata.DeviceID = 0;
                        jkdata.datetime = "--";//更新时间
                        jkdata.RtuID = 0;
                        jkdata.State = "离线";
                        jkdata.PressIN = new KeyValuePair<string, bool>("--", false);
                        jkdata.PressOut = new KeyValuePair<string, bool>("--", false);
                        jkdata.inflow = "--";
                        jkdata.Partition = 1;
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
                if (jk.allInFlow == 0)
                {
                    jk.allInFlow = Math.Round(allInFlow, 2);
                }

            }
            return Json(new
            {
                jk

            });
        }

        //获取水箱数据
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult LoadshuixiangInfo(int sid, int partionid = 6)
        {
            var enumfrequency = (int)Model.CustomizedClass.Enum.变频分类;
            var ftype = _DataItemDetailService.Query<SysDataItemDetail>(r => r.FItemId == enumfrequency).ToList();
            var deviceInfo = _sws_DeviceInfo01Service.Query<SwsDeviceInfo01>(r => r.StationId == sid && r.Partition == partionid).FirstOrDefault();
            if (deviceInfo == null)
            {
                shuixiangjk jkdata1 = new shuixiangjk();


            }
            var rtuID = deviceInfo.Rtuid;

            shuixiangjk jkdata = new shuixiangjk();
            if (rtuID != null)
            {
                string macJson = "{\"RTUID\":{'$in':[" + rtuID + "]}}";
                var collections = new MongoDBHelper<RtuJKInfo>(DateTime.Now.Year.ToString());
                var jklist = collections.Find("Sws_DeviceJKInfo", macJson, "UpdateTime").FirstOrDefault();
                List<SwsEventInfo> eventlist_all = new List<SwsEventInfo>();
                eventlist_all = _EventInfoService.Query<SwsEventInfo>(r => r.Rtuid == rtuID).ToList();
                var datajk = jklist;
                #region 状态 
                if (datajk != null)
                {
                    //累计电量
                    jkdata.sx1 = datajk.AnalogValues.ContainsKey((4555).ToString()) ? double.Parse(datajk.AnalogValues[(4555).ToString()].ToString()) : 0.0;
                    jkdata.sx2 = datajk.AnalogValues.ContainsKey((4556).ToString()) ? double.Parse(datajk.AnalogValues[(4556).ToString()].ToString()) : 0.0;
                    jkdata.sx3 = datajk.AnalogValues.ContainsKey((4557).ToString()) ? double.Parse(datajk.AnalogValues[(4557).ToString()].ToString()) : 0.0;
                    jkdata.sx4 = datajk.AnalogValues.ContainsKey((4558).ToString()) ? double.Parse(datajk.AnalogValues[(4558).ToString()].ToString()) : 0.0;
                    jkdata.sx5 = datajk.AnalogValues.ContainsKey((4559).ToString()) ? double.Parse(datajk.AnalogValues[(4559).ToString()].ToString()) : 0.0;

                }
                else
                {

                }
                #endregion
            }
            else
            {

            }
            return Json(new
            {

                jkdata

            });
        }
        #endregion
        #region 监控
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult CameraDetail(int sid)
        {
            var cameraInfo = cameraService.Query<SwsCamera>(r => r.StationId == sid).FirstOrDefault();
            if (cameraInfo != null)
            {
                ViewBag.cameraid = cameraInfo.CameraId;
            }
            else
            {
                ViewBag.cameraid = 0;
            }
            return View();

        }
        #endregion 
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult Detail02(int sid, string name, int showTab)
        {
            ViewBag.StationName = name;
            ViewBag.ShowTab = showTab;
            var info = _sws_DeviceInfo02Service.Query<SwsDeviceInfo02>(r => r.StationId == sid).ToList();
            //分区信息
            var partitionList = info.OrderBy(r => r.Partition).Select(r => r.Partition).ToList();
            ViewBag.sid = sid;

            //泵房工艺图地址
            var station = stationService.GetGUIImg(sid).FirstOrDefault();
            if (station != null && !string.IsNullOrEmpty(station.PageURL))
            {
                ViewBag.GUIUrl = station.PageURL;
            }
            else
            {
                ViewBag.GUIUrl = "../../GUI/zhiyinshui/index-zys.html";
            }

            if (partitionList.Count > 0)
            {
                ViewBag.parid = partitionList.FirstOrDefault();
            }
            else
            {
                ViewBag.parid = 1;
            }
            //报警信息
            var rtuid = info.Select(r => r.Rtuid).ToList();
            List<dynamic> eventInfo = new List<dynamic>();
            if (rtuid.Count > 0)
            {
                var rtuids = string.Join(",", rtuid);
                eventInfo = _EventInfoService.LoadEventList(rtuids).ToList();
            }
            ViewBag.eventInfo = eventInfo;
            //泵房样貌
            List<string> suff = new List<string> { ".jpg", ".png", "jpeg" };
            var imgList = _attachmentService.Query<Attachment>(r => r.Affiliation == sid && suff.Contains(r.Suffix)).ToList();
            ViewBag.imgList = imgList;

            //总工艺图信息
            var guidInfo = stationService.Query<SwsStation>(r => r.StationId == sid).FirstOrDefault();
            var iscontoroller = guidInfo?.ControlMonitor;
            ViewBag.iscontoroller = iscontoroller;
            return View();
        }
        #region 直饮水数据
        //查询泵房下的分区设备信息
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult LoadZStationJKPartuion(int sid, int partionid)
        {
            var enumfrequency = (int)Model.CustomizedClass.Enum.变频分类;
            var ftype = _DataItemDetailService.Query<SysDataItemDetail>(r => r.FItemId == enumfrequency).ToList();
            var deviceInfo = _sws_DeviceInfo02Service.Query<SwsDeviceInfo02>(r => r.StationId == sid && r.Partition == partionid).FirstOrDefault();
            var rtuID = deviceInfo.Rtuid;
            montorZGuiJK jkdata = new montorZGuiJK();
            if (rtuID != null)
            {
                string macJson = "{\"RTUID\":{'$in':[" + rtuID + "]}}";
                var collections = new MongoDBHelper<RtuJKInfo>(DateTime.Now.Year.ToString());
                var jklist = collections.Find("Sws_DeviceJKInfo", macJson, "UpdateTime").FirstOrDefault();
                List<SwsEventInfo> eventlist_all = new List<SwsEventInfo>();
                eventlist_all = _EventInfoService.Query<SwsEventInfo>(r => r.Rtuid == rtuID).ToList();
                var datajk = jklist;
                #region 状态
                var state = "离线";
                if (eventlist_all.Count() > 0)
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
                if (datajk != null)
                {
                    var updatetime = datajk.UpdateTime;
                    jkdata.datetime = updatetime.ToString();//更新时间 
                    jkdata.RtuID = (int)rtuID;
                    jkdata.State = state;
                    var partion = partionid;
                    //净水出水压力
                    int pressinid = 2014 + (partion - 1) * 500;
                    jkdata.JPressOut = new KeyValuePair<string, bool>(datajk.AnalogValues.ContainsKey(pressinid.ToString()) ? datajk.AnalogValues[pressinid.ToString()].ToString() : "--",
                     eventlist_all.Count > 0 ? (eventlist_all.Where(r => r.EventSource == pressinid && r.EventLevel != 0 && r.Rtuid == rtuID).Count() > 0 ? true : false) : false);
                    //原水出水压力
                    int pressout = 2015 + (partion - 1) * 500;
                    jkdata.YPressOut = new KeyValuePair<string, bool>(datajk.AnalogValues.ContainsKey(pressout.ToString()) ? datajk.AnalogValues[pressout.ToString()].ToString() : "--",
                    eventlist_all.Count > 0 ? (eventlist_all.Where(r => r.EventSource == pressout && r.EventLevel != 0 && r.Rtuid == rtuID).Count() > 0 ? true : false) : false);
                    //水泵数据
                    #region 水泵数据
                    //泵的信息


                    #endregion

                    ////瞬时流量
                    //var infdata1 = datajk.AnalogValues.ContainsKey((2030 + (partion - 1) * 500).ToString()) ? datajk.AnalogValues[(2030 + (partion - 1) * 500).ToString()] : null;
                    //var infdata2 = datajk.AnalogValues.ContainsKey((2032 + (partion - 1) * 500).ToString()) ? datajk.AnalogValues[(2032 + (partion - 1) * 500).ToString()] : null;
                    //var infdata3 = datajk.AnalogValues.ContainsKey((2034 + (partion - 1) * 500).ToString()) ? datajk.AnalogValues[(2034 + (partion - 1) * 500).ToString()] : null;
                    //var infdata4 = datajk.AnalogValues.ContainsKey((2036 + (partion - 1) * 500).ToString()) ? datajk.AnalogValues[(2036 + (partion - 1) * 500).ToString()] : null;
                    //if (infdata1 == null && infdata2 == null && infdata3 == null && infdata4 == null)
                    //{
                    //    jkdata.inflow = "--";
                    //}
                    //else
                    //{
                    //    var inflowtemp = Math.Round(Convert.ToDouble(infdata1) + Convert.ToDouble(infdata2) + Convert.ToDouble(infdata3) + Convert.ToDouble(infdata4), 2);
                    //    jkdata.inflow = inflowtemp.ToString();
                    //    allInFlow += inflowtemp;
                    //}
                    ////累计流量
                    //var tfdata1 = datajk.AnalogValues.ContainsKey((2031 + (partion - 1) * 500).ToString()) ? datajk.AnalogValues[(2031 + (partion - 1) * 500).ToString()] : null;
                    //var tfdata2 = datajk.AnalogValues.ContainsKey((2033 + (partion - 1) * 500).ToString()) ? datajk.AnalogValues[(2033 + (partion - 1) * 500).ToString()] : null;
                    //var tfdata3 = datajk.AnalogValues.ContainsKey((2035 + (partion - 1) * 500).ToString()) ? datajk.AnalogValues[(2035 + (partion - 1) * 500).ToString()] : null;
                    //var tfdata4 = datajk.AnalogValues.ContainsKey((2037 + (partion - 1) * 500).ToString()) ? datajk.AnalogValues[(2037 + (partion - 1) * 500).ToString()] : null;
                    //if (tfdata1 == null && tfdata2 == null && tfdata3 == null && tfdata4 == null)
                    //{
                    //    jkdata.totalflow = "--";
                    //}
                    //else
                    //{
                    //    jkdata.totalflow = (Convert.ToDouble(tfdata1) + Convert.ToDouble(tfdata2) + Convert.ToDouble(tfdata3) + Convert.ToDouble(tfdata4)).ToString();
                    //}
                    ////累计电量
                    //var pall = datajk.AnalogValues.ContainsKey((2085 + (partion - 1) * 500).ToString()) ? datajk.AnalogValues[(2085 + (partion - 1) * 500).ToString()] : null;
                    //var pdata1 = datajk.AnalogValues.ContainsKey((2023 + (partion - 1) * 500).ToString()) ? datajk.AnalogValues[(2023 + (partion - 1) * 500).ToString()] : null;
                    //var pdata2 = datajk.AnalogValues.ContainsKey((2024 + (partion - 1) * 500).ToString()) ? datajk.AnalogValues[(2024 + (partion - 1) * 500).ToString()] : null;
                    //var pdata3 = datajk.AnalogValues.ContainsKey((2025 + (partion - 1) * 500).ToString()) ? datajk.AnalogValues[(2025 + (partion - 1) * 500).ToString()] : null;
                    //var pdata4 = datajk.AnalogValues.ContainsKey((2026 + (partion - 1) * 500).ToString()) ? datajk.AnalogValues[(2026 + (partion - 1) * 500).ToString()] : null;
                    //var pdata = Convert.ToDouble(pdata1) + Convert.ToDouble(pdata2) + Convert.ToDouble(pdata3) + Convert.ToDouble(pdata4);
                    //jkdata.totalpower = (pdata == 0.0 ? (Convert.ToDouble(pall)).ToString() : pdata.ToString());
                    //浊度
                    //浊度
                    int turid = 2034;
                    jkdata.Turbidity = datajk.AnalogValues.ContainsKey(turid.ToString()) ? datajk.AnalogValues[turid.ToString()].ToString() : "--";
                    //余氯
                    int clid = 2033;
                    jkdata.CL = datajk.AnalogValues.ContainsKey(clid.ToString()) ? datajk.AnalogValues[clid.ToString()].ToString() : "--";
                    //ph
                    int tphid = 2032;
                    jkdata.PH = datajk.AnalogValues.ContainsKey(tphid.ToString()) ? datajk.AnalogValues[tphid.ToString()].ToString() : "--";
                    //原水箱液位
                    int ylevelid = 2016;
                    jkdata.YLevel = datajk.AnalogValues.ContainsKey(ylevelid.ToString()) ? datajk.AnalogValues[ylevelid.ToString()].ToString() : "--";
                    //净水箱液位
                    int jlevelid = 2017;
                    jkdata.JLevel = datajk.AnalogValues.ContainsKey(jlevelid.ToString()) ? datajk.AnalogValues[jlevelid.ToString()].ToString() : "--";
                    //净水设定
                    int jsid = 2011;
                    jkdata.JSetpressure = datajk.AnalogValues.ContainsKey(jsid.ToString()) ? datajk.AnalogValues[jsid.ToString()].ToString() : "--";
                    //电导率
                    int cdid = 2023;
                    jkdata.Conductivity = datajk.AnalogValues.ContainsKey(cdid.ToString()) ? datajk.AnalogValues[cdid.ToString()].ToString() : "--";
                    // 2035
                    int orpid = 2035;
                    jkdata.Orp = datajk.AnalogValues.ContainsKey(orpid.ToString()) ? datajk.AnalogValues[orpid.ToString()].ToString() : "--";
                    int slid = 2036;
                    jkdata.Salinity = datajk.AnalogValues.ContainsKey(slid.ToString()) ? datajk.AnalogValues[slid.ToString()].ToString() : "--";
                    int oxid = 2037;
                    jkdata.Oxygen = datajk.AnalogValues.ContainsKey(oxid.ToString()) ? datajk.AnalogValues[oxid.ToString()].ToString() : "--";
                    //硬度
                    int hardnessid = 2039;
                    jkdata.Hardness = datajk.AnalogValues.ContainsKey(hardnessid.ToString()) ? datajk.AnalogValues[hardnessid.ToString()].ToString() : "--";
                    //jkdata.pumpState = pumpState;
                    //jkdata.pumpdatas = pumpdataList;
                }
                else
                {

                    //jkdata.datetime = "--";//更新时间
                    //jkdata.RtuID = (int)rtuID;
                    //jkdata.State = state;
                    //jkdata.PressIN = new KeyValuePair<string, bool>("--", false);
                    //jkdata.PressOut = new KeyValuePair<string, bool>("--", false);
                    //jkdata.pumpState = new List<string>();
                    //jkdata.pumpdatas = new List<PumpData>();
                    //jkdata.totalflow = "--";
                    //jkdata.totalpower = "--";
                    //jkdata.inflow = "--";
                    //for (var i = 0; i < PumpNum; i++)
                    //{
                    //    PumpData p = new PumpData();
                    //    p.eletric = new KeyValuePair<string, bool>("--", false);
                    //    p.frequency = new KeyValuePair<string, bool>("--", false);
                    //    p.runtime = new KeyValuePair<string, bool>("--", false);
                    //    jkdata.pumpdatas.Add(p);
                    //    jkdata.pumpState.Add("停止");
                    //}
                }
                #endregion
            }
            return Json(new
            {

                jkdata

            });
        }

        //新工艺图数据
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult LoadZStationJK(int sid, string partionid)
        {
            var item = _sws_RTUInfoService.LoadDeviceAndRtuinfo(sid).Where(e => e.Partition == "低区").FirstOrDefault();
            List<WNMS.Model.CustomizedClass.Pump> lstPumps = new List<WNMS.Model.CustomizedClass.Pump>();
            //获取实时监控数据
            var listResulr = _sws_RTUInfoService.GetMongoJKData(item.Rtuid);
            string Section = SimpleFactory.GetParameters(item.Partition.ToString());
            string[] strSection = Section.Split(',');

            DeviceJK_SS_ZYS deviceJK = new DeviceJK_SS_ZYS();
            deviceJK.EquipID = item.EquipID;
            deviceJK.RTUID = item.Rtuid;
            deviceJK.EquipmentType = item.EquipmentType;
            deviceJK.DeviceType = item.DeviceType;
            deviceJK.Partition = item.Partition;
            deviceJK.DeviceName = item.DeviceName;
            deviceJK.IsOnline = "离线";
            List<SwsEventInfo> eventlist_all = new List<SwsEventInfo>();
            eventlist_all = _EventInfoService.Query<SwsEventInfo>(r => r.Rtuid == item.Rtuid).ToList();
            if (listResulr != null)
            {
                deviceJK.UpdateTime = Convert.ToDateTime(listResulr.UpdateTime).ToString("yyyy-MM-dd HH:mm:ss");

                if (strSection.Length == 2)
                {
                    int iAnalog = int.Parse(strSection[0]);
                    int iDigial = int.Parse(strSection[1]);
                    List<int> dataid = new List<int>() { iAnalog + 16, iAnalog + 17, iAnalog + 15, iAnalog + 11, iAnalog + 14, iAnalog + 32, iAnalog + 33, iAnalog + 34, iAnalog + 35, iAnalog + 36, iAnalog + 37, iAnalog + 23, iAnalog + 4, iAnalog + 5,
                        iAnalog + 6, iAnalog + 7, iAnalog + 8, iAnalog + 9, iAnalog + 10, 1001, iAnalog+40,iAnalog+41};
                    deviceJK.YLevel = listResulr.AnalogValues.ContainsKey(dataid[0].ToString()) ? Math.Round(double.Parse((listResulr.AnalogValues[dataid[0].ToString()]).ToString()), 2) : 0;
                    deviceJK.JLevel = listResulr.AnalogValues.ContainsKey(dataid[1].ToString()) ? Math.Round(double.Parse((listResulr.AnalogValues[dataid[1].ToString()]).ToString()), 2) : 0;
                    deviceJK.YOutPressure = listResulr.AnalogValues.ContainsKey(dataid[2].ToString()) ? Math.Round(double.Parse((listResulr.AnalogValues[dataid[2].ToString()]).ToString()), 2) : 0;  //原水出水压力
                    deviceJK.JSetpressure = listResulr.AnalogValues.ContainsKey(dataid[3].ToString()) ? Math.Round(double.Parse((listResulr.AnalogValues[dataid[3].ToString()]).ToString()), 2) : 0;  //净水设定压力
                    deviceJK.JOutPressure = listResulr.AnalogValues.ContainsKey(dataid[4].ToString()) ? Math.Round(double.Parse((listResulr.AnalogValues[dataid[4].ToString()]).ToString()), 2) : 0;   //净水出水压力

                    deviceJK.PH = listResulr.AnalogValues.ContainsKey(dataid[5].ToString()) ? double.Parse((listResulr.AnalogValues[dataid[5].ToString()]).ToString()) : 0;
                    deviceJK.CL = listResulr.AnalogValues.ContainsKey(dataid[6].ToString()) ? double.Parse((listResulr.AnalogValues[dataid[6].ToString()]).ToString()) : 0;
                    deviceJK.Turbidity = listResulr.AnalogValues.ContainsKey(dataid[7].ToString()) ? double.Parse((listResulr.AnalogValues[dataid[7].ToString()]).ToString()) : 0;
                    deviceJK.ORP = listResulr.AnalogValues.ContainsKey(dataid[8].ToString()) ? Math.Round(double.Parse((listResulr.AnalogValues[dataid[8].ToString()]).ToString()), 2) : 0;
                    deviceJK.Salinity = listResulr.AnalogValues.ContainsKey(dataid[9].ToString()) ? Math.Round(double.Parse((listResulr.AnalogValues[dataid[9].ToString()]).ToString()), 2) : 0;
                    deviceJK.Oxygen = listResulr.AnalogValues.ContainsKey(dataid[10].ToString()) ? Math.Round(double.Parse((listResulr.AnalogValues[dataid[10].ToString()]).ToString()), 2) : 0;
                    deviceJK.Conductivity = listResulr.AnalogValues.ContainsKey(dataid[11].ToString()) ? Math.Round(double.Parse((listResulr.AnalogValues[dataid[11].ToString()]).ToString()), 2) : 0;
                    deviceJK.Ssll = listResulr.AnalogValues.ContainsKey(dataid[20].ToString()) ? Math.Round(double.Parse((listResulr.AnalogValues[dataid[20].ToString()]).ToString()), 2) : 0;
                    deviceJK.Ljll = listResulr.AnalogValues.ContainsKey(dataid[21].ToString()) ? Math.Round(double.Parse((listResulr.AnalogValues[dataid[21].ToString()]).ToString()), 2) : 0;
                    int hardnessid = 2039;
                    deviceJK.Hardness = listResulr.AnalogValues.ContainsKey(hardnessid.ToString()) ? Math.Round(double.Parse((listResulr.AnalogValues[hardnessid.ToString()]).ToString()), 2) : 0;

                    deviceJK.GPumpState = listResulr.AnalogValues.ContainsKey(dataid[12].ToString()) ? (double.Parse((listResulr.AnalogValues[dataid[12].ToString()]).ToString()) == 0 ? "停止" : "启动") : "停止";  //高压泵状态
                    deviceJK.QPumpState = listResulr.AnalogValues.ContainsKey(dataid[13].ToString()) ? (double.Parse((listResulr.AnalogValues[dataid[13].ToString()]).ToString()) == 0 ? "停止" : "启动") : "停止";  //清洗泵状态
                    deviceJK.QValveState = listResulr.AnalogValues.ContainsKey(dataid[14].ToString()) ? (double.Parse((listResulr.AnalogValues[dataid[14].ToString()]).ToString()) == 0 ? "关闭" : "打开") : "关闭";  //进水阀状态
                    deviceJK.PValveState = listResulr.AnalogValues.ContainsKey(dataid[15].ToString()) ? (double.Parse((listResulr.AnalogValues[dataid[15].ToString()]).ToString()) == 0 ? "关闭" : "打开") : "关闭";  //旁通阀状态
                    deviceJK.GValveState = listResulr.AnalogValues.ContainsKey(dataid[16].ToString()) ? (double.Parse((listResulr.AnalogValues[dataid[16].ToString()]).ToString()) == 0 ? "关闭" : "打开") : "关闭";  //高压阀状态
                    deviceJK.NValveState = listResulr.AnalogValues.ContainsKey(dataid[17].ToString()) ? (double.Parse((listResulr.AnalogValues[dataid[17].ToString()]).ToString()) == 0 ? "关闭" : "打开") : "关闭";  //浓水阀状态
                    deviceJK.HValveState = listResulr.AnalogValues.ContainsKey(dataid[18].ToString()) ? (double.Parse((listResulr.AnalogValues[dataid[18].ToString()]).ToString()) == 0 ? "关闭" : "打开") : "关闭";  //回水阀状态
                    deviceJK.JJValveState = listResulr.AnalogValues.ContainsKey("2076") ? (double.Parse((listResulr.AnalogValues["2076"]).ToString()) == 0 ? "关闭" : "打开") : "关闭";  //净水箱进水电磁阀状态
                    deviceJK.MQValveState = listResulr.AnalogValues.ContainsKey("2051") ? (double.Parse((listResulr.AnalogValues["2051"]).ToString()) == 0 ? "关闭" : "打开") : "关闭";  //膜前电磁阀状态
                    deviceJK.FJValveState = listResulr.AnalogValues.ContainsKey("2052") ? (double.Parse((listResulr.AnalogValues["2052"]).ToString()) == 0 ? "关闭" : "打开") : "关闭";  //反洗进水电磁阀状态

                    //判断水质是否合格
                    deviceJK.ConHighPolice = listResulr.AnalogValues.ContainsKey("5036") ? (double.Parse((listResulr.AnalogValues["5036"]).ToString()) == 0 ? "合格" : "不合格") : "合格";  //电导率
                    deviceJK.PHBHG = listResulr.AnalogValues.ContainsKey("5037") ? (double.Parse((listResulr.AnalogValues["5037"]).ToString()) == 0 ? "合格" : "不合格") : "合格";  //ph
                    deviceJK.YLCBBJ = listResulr.AnalogValues.ContainsKey("5038") ? (double.Parse((listResulr.AnalogValues["5038"]).ToString()) == 0 ? "合格" : "不合格") : "合格";  //余氯
                    deviceJK.ZDCBBJ = listResulr.AnalogValues.ContainsKey("5039") ? (double.Parse((listResulr.AnalogValues["5039"]).ToString()) == 0 ? "合格" : "不合格") : "合格";  //浊度
                    deviceJK.RYCBBJ = listResulr.AnalogValues.ContainsKey("5040") ? (double.Parse((listResulr.AnalogValues["5040"]).ToString()) == 0 ? "合格" : "不合格") : "合格";  //溶氧
                    deviceJK.ORPCBBJ = listResulr.AnalogValues.ContainsKey("5041") ? (double.Parse((listResulr.AnalogValues["5041"]).ToString()) == 0 ? "合格" : "不合格") : "合格";  //ORP
                    deviceJK.YANDCBBJ = listResulr.AnalogValues.ContainsKey("5042") ? (double.Parse((listResulr.AnalogValues["5042"]).ToString()) == 0 ? "合格" : "不合格") : "合格";  //盐度
                    deviceJK.CODCBBJ = listResulr.AnalogValues.ContainsKey("5043") ? (double.Parse((listResulr.AnalogValues["5043"]).ToString()) == 0 ? "合格" : "不合格") : "合格";  //Cod
                    deviceJK.YINGDCBBJ = listResulr.AnalogValues.ContainsKey("5044") ? (double.Parse((listResulr.AnalogValues["5044"]).ToString()) == 0 ? "合格" : "不合格") : "合格";  //硬度

                    //判断在线离线
                    if (listResulr != null && listResulr.DigitalValues != null)
                    {
                        bool zstate = listResulr.DigitalValues.Keys.Contains("1001") ? Convert.ToBoolean(listResulr.DigitalValues["1001"]) : false;
                        var tstate = listResulr.DigitalValues.Keys.Contains("1000") ? Convert.ToBoolean(listResulr.DigitalValues["1000"]) : false;

                        if (zstate == true && tstate == true && (DateTime.Now - Convert.ToDateTime(listResulr.UpdateTime)).TotalMinutes <= timevalue)
                        {
                            if (eventlist_all.Count() > 0)
                            {
                                deviceJK.IsOnline = "故障";
                            }
                            else
                            {
                                deviceJK.IsOnline = "正常";
                            }
                        }
                        else
                        {
                            deviceJK.IsOnline = "离线";
                        }
                    }

                    //泵的信息
                    List<int> y1dataid = new List<int>() { iAnalog, iDigial + 27, iAnalog + 18 };  //原水1泵状态
                    lstPumps.Add(GetYDataMongo(listResulr, y1dataid, deviceJK.IsOnline));
                    List<int> y2dataid = new List<int>() { iAnalog + 1, iDigial + 28, iAnalog + 19 };   //原水2泵
                    lstPumps.Add(GetYDataMongo(listResulr, y2dataid, deviceJK.IsOnline));
                    List<int> gdataid = new List<int>() { iAnalog + 4, iDigial + 29, iAnalog + 22 };  //高压泵
                    lstPumps.Add(GetYDataMongo(listResulr, gdataid, deviceJK.IsOnline));
                    List<int> sumbers = new List<int>() { iAnalog + 2, iDigial + 30, iAnalog + 12, iAnalog + 20 };  //净水1泵
                    lstPumps.Add(GetYDataMongo(listResulr, sumbers, deviceJK.IsOnline));
                    List<int> sumbers2 = new List<int>() { iAnalog + 3, iDigial + 31, iAnalog + 13, iAnalog + 21 };  //净水2泵
                    lstPumps.Add(GetYDataMongo(listResulr, sumbers2, deviceJK.IsOnline));
                    List<int> qxbeng = new List<int>() { iAnalog + 5, iDigial + 32 };     //清洗泵
                    lstPumps.Add(GetYDataMongo(listResulr, qxbeng, deviceJK.IsOnline));



                    for (int i = 0; i < 6; i++)
                    {
                        List<int> sumbers3 = new List<int>() { iAnalog + i + 40, iDigial + i + +45 };     //净水3-8泵
                        lstPumps.Add(GetYDataMongo(listResulr, sumbers3, deviceJK.IsOnline));
                    }

                    List<int> fxbeng = new List<int>() { iDigial + 69, iDigial + 70 };     //反洗泵
                    lstPumps.Add(GetYDataMongo(listResulr, fxbeng, deviceJK.IsOnline));
                    deviceJK.lstPumps = lstPumps;
                }
                else
                {

                }
            }

            else
            {

            }
            return Json(new
            {
                deviceJK
            });
        }
        //wzg分离方法
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult LoadWzgZStationJK(int sid, string partionid)
        {
            var item = _sws_RTUInfoService.LoadDeviceAndRtuinfo(sid).Where(e => e.Partition == "低区").FirstOrDefault();
            List<WNMS.Model.CustomizedClass.Pump> lstPumps = new List<WNMS.Model.CustomizedClass.Pump>();
            //获取实时监控数据
            var listResulr = _sws_RTUInfoService.GetMongoJKData(item.Rtuid);
            string Section = SimpleFactory.GetParameters(item.Partition.ToString());
            string[] strSection = Section.Split(',');

            DeviceJK_SS_ZYS deviceJK = new DeviceJK_SS_ZYS();
            deviceJK.EquipID = item.EquipID;
            deviceJK.RTUID = item.Rtuid;
            deviceJK.EquipmentType = item.EquipmentType;
            deviceJK.DeviceType = item.DeviceType;
            deviceJK.Partition = item.Partition;
            deviceJK.DeviceName = item.DeviceName;
            deviceJK.IsOnline = "离线";
            List<SwsEventInfo> eventlist_all = new List<SwsEventInfo>();
            eventlist_all = _EventInfoService.Query<SwsEventInfo>(r => r.Rtuid == item.Rtuid).ToList();
            if (listResulr != null)
            {
                deviceJK.UpdateTime = Convert.ToDateTime(listResulr.UpdateTime).ToString("yyyy-MM-dd HH:mm:ss");

                if (strSection.Length == 2)
                {
                    int iAnalog = int.Parse(strSection[0]);
                    int iDigial = int.Parse(strSection[1]);
                    List<int> dataid = new List<int>() { iAnalog + 16, iAnalog + 17, iAnalog + 15, iAnalog + 11, iAnalog + 14, iAnalog + 32, iAnalog + 33, iAnalog + 34, iAnalog + 35, iAnalog + 36, iAnalog + 37, iAnalog + 23, iAnalog + 4, iAnalog + 5,
                        iAnalog + 6, iAnalog + 7, iAnalog + 8, iAnalog + 9, iAnalog + 10, 1001, iAnalog+40,iAnalog+41};
                    deviceJK.YLevel = listResulr.AnalogValues.ContainsKey(dataid[0].ToString()) ? Math.Round(double.Parse((listResulr.AnalogValues[dataid[0].ToString()]).ToString()), 2) : 0;
                    deviceJK.JLevel = listResulr.AnalogValues.ContainsKey(dataid[1].ToString()) ? Math.Round(double.Parse((listResulr.AnalogValues[dataid[1].ToString()]).ToString()), 2) : 0;
                    deviceJK.YOutPressure = listResulr.AnalogValues.ContainsKey(dataid[2].ToString()) ? Math.Round(double.Parse((listResulr.AnalogValues[dataid[2].ToString()]).ToString()), 2) : 0;  //原水出水压力
                    deviceJK.JSetpressure = listResulr.AnalogValues.ContainsKey(dataid[3].ToString()) ? Math.Round(double.Parse((listResulr.AnalogValues[dataid[3].ToString()]).ToString()), 2) : 0;  //净水设定压力
                    deviceJK.JOutPressure = listResulr.AnalogValues.ContainsKey(dataid[4].ToString()) ? Math.Round(double.Parse((listResulr.AnalogValues[dataid[4].ToString()]).ToString()), 2) : 0;   //净水出水压力

                    deviceJK.PH = listResulr.AnalogValues.ContainsKey(dataid[5].ToString()) ? double.Parse((listResulr.AnalogValues[dataid[5].ToString()]).ToString()) : 0;
                    deviceJK.CL = listResulr.AnalogValues.ContainsKey(dataid[6].ToString()) ? double.Parse((listResulr.AnalogValues[dataid[6].ToString()]).ToString()) : 0;
                    deviceJK.Turbidity = listResulr.AnalogValues.ContainsKey(dataid[7].ToString()) ? double.Parse((listResulr.AnalogValues[dataid[7].ToString()]).ToString()) : 0;
                    deviceJK.ORP = listResulr.AnalogValues.ContainsKey(dataid[8].ToString()) ? Math.Round(double.Parse((listResulr.AnalogValues[dataid[8].ToString()]).ToString()), 2) : 0;
                    deviceJK.Salinity = listResulr.AnalogValues.ContainsKey(dataid[9].ToString()) ? Math.Round(double.Parse((listResulr.AnalogValues[dataid[9].ToString()]).ToString()), 2) : 0;
                    deviceJK.Oxygen = listResulr.AnalogValues.ContainsKey(dataid[10].ToString()) ? Math.Round(double.Parse((listResulr.AnalogValues[dataid[10].ToString()]).ToString()), 2) : 0;
                    deviceJK.Conductivity = listResulr.AnalogValues.ContainsKey(dataid[11].ToString()) ? Math.Round(double.Parse((listResulr.AnalogValues[dataid[11].ToString()]).ToString()), 2) : 0;
                    deviceJK.Ssll = listResulr.AnalogValues.ContainsKey(dataid[20].ToString()) ? Math.Round(double.Parse((listResulr.AnalogValues[dataid[20].ToString()]).ToString()), 2) : 0;
                    deviceJK.Ljll = listResulr.AnalogValues.ContainsKey(dataid[21].ToString()) ? Math.Round(double.Parse((listResulr.AnalogValues[dataid[21].ToString()]).ToString()), 2) : 0;
                    int hardnessid = 2039;
                    deviceJK.Hardness = listResulr.AnalogValues.ContainsKey(hardnessid.ToString()) ? Math.Round(double.Parse((listResulr.AnalogValues[hardnessid.ToString()]).ToString()), 2) : 0;

                    deviceJK.GPumpState = listResulr.AnalogValues.ContainsKey(dataid[12].ToString()) ? (double.Parse((listResulr.AnalogValues[dataid[12].ToString()]).ToString()) == 0 ? "停止" : "启动") : "停止";  //高压泵状态
                    deviceJK.QPumpState = listResulr.AnalogValues.ContainsKey(dataid[13].ToString()) ? (double.Parse((listResulr.AnalogValues[dataid[13].ToString()]).ToString()) == 0 ? "停止" : "启动") : "停止";  //清洗泵状态
                    deviceJK.QValveState = listResulr.AnalogValues.ContainsKey(dataid[14].ToString()) ? (double.Parse((listResulr.AnalogValues[dataid[14].ToString()]).ToString()) == 0 ? "关闭" : "打开") : "关闭";  //进水阀状态
                    deviceJK.PValveState = listResulr.AnalogValues.ContainsKey(dataid[15].ToString()) ? (double.Parse((listResulr.AnalogValues[dataid[15].ToString()]).ToString()) == 0 ? "关闭" : "打开") : "关闭";  //旁通阀状态
                    deviceJK.GValveState = listResulr.AnalogValues.ContainsKey(dataid[16].ToString()) ? (double.Parse((listResulr.AnalogValues[dataid[16].ToString()]).ToString()) == 0 ? "关闭" : "打开") : "关闭";  //高压阀状态
                    deviceJK.NValveState = listResulr.AnalogValues.ContainsKey(dataid[17].ToString()) ? (double.Parse((listResulr.AnalogValues[dataid[17].ToString()]).ToString()) == 0 ? "关闭" : "打开") : "关闭";  //浓水阀状态
                    deviceJK.HValveState = listResulr.AnalogValues.ContainsKey(dataid[18].ToString()) ? (double.Parse((listResulr.AnalogValues[dataid[18].ToString()]).ToString()) == 0 ? "关闭" : "打开") : "关闭";  //回水阀状态

                    //判断水质是否合格
                    deviceJK.ConHighPolice = listResulr.AnalogValues.ContainsKey("5036") ? (double.Parse((listResulr.AnalogValues["5036"]).ToString()) == 0 ? "合格" : "不合格") : "合格";  //电导率
                    deviceJK.PHBHG = listResulr.AnalogValues.ContainsKey("5037") ? (double.Parse((listResulr.AnalogValues["5037"]).ToString()) == 0 ? "合格" : "不合格") : "合格";  //ph
                    deviceJK.YLCBBJ = listResulr.AnalogValues.ContainsKey("5038") ? (double.Parse((listResulr.AnalogValues["5038"]).ToString()) == 0 ? "合格" : "不合格") : "合格";  //余氯
                    deviceJK.ZDCBBJ = listResulr.AnalogValues.ContainsKey("5039") ? (double.Parse((listResulr.AnalogValues["5039"]).ToString()) == 0 ? "合格" : "不合格") : "合格";  //浊度
                    deviceJK.RYCBBJ = listResulr.AnalogValues.ContainsKey("5040") ? (double.Parse((listResulr.AnalogValues["5040"]).ToString()) == 0 ? "合格" : "不合格") : "合格";  //溶氧
                    deviceJK.ORPCBBJ = listResulr.AnalogValues.ContainsKey("5041") ? (double.Parse((listResulr.AnalogValues["5041"]).ToString()) == 0 ? "合格" : "不合格") : "合格";  //ORP
                    deviceJK.YANDCBBJ = listResulr.AnalogValues.ContainsKey("5042") ? (double.Parse((listResulr.AnalogValues["5042"]).ToString()) == 0 ? "合格" : "不合格") : "合格";  //盐度
                    deviceJK.CODCBBJ = listResulr.AnalogValues.ContainsKey("5043") ? (double.Parse((listResulr.AnalogValues["5043"]).ToString()) == 0 ? "合格" : "不合格") : "合格";  //Cod
                    deviceJK.YINGDCBBJ = listResulr.AnalogValues.ContainsKey("5044") ? (double.Parse((listResulr.AnalogValues["5044"]).ToString()) == 0 ? "合格" : "不合格") : "合格";  //硬度

                    //判断在线离线
                    if (listResulr != null && listResulr.DigitalValues != null)
                    {
                        bool zstate = listResulr.DigitalValues.Keys.Contains("1001") ? Convert.ToBoolean(listResulr.DigitalValues["1001"]) : false;
                        var tstate = listResulr.DigitalValues.Keys.Contains("1000") ? Convert.ToBoolean(listResulr.DigitalValues["1000"]) : false;

                        if (zstate == true && tstate == true && (DateTime.Now - Convert.ToDateTime(listResulr.UpdateTime)).TotalMinutes <= timevalue)
                        {
                            if (eventlist_all.Count() > 0)
                            {
                                deviceJK.IsOnline = "故障";
                            }
                            else
                            {
                                deviceJK.IsOnline = "正常";
                            }
                        }
                        else
                        {
                            deviceJK.IsOnline = "离线";
                        }
                    }

                    //泵的信息
                    List<int> y1dataid = new List<int>() { iAnalog, iDigial + 27, iAnalog + 18 };  //原水1泵状态
                    lstPumps.Add(GetYDataMongo(listResulr, y1dataid, deviceJK.IsOnline));
                    List<int> y2dataid = new List<int>() { iAnalog + 1, iDigial + 28, iAnalog + 19 };   //原水2泵
                    lstPumps.Add(GetYDataMongo(listResulr, y2dataid, deviceJK.IsOnline));
                    List<int> gdataid = new List<int>() { iAnalog + 4, iDigial + 29, iAnalog + 22 };  //高压泵
                    lstPumps.Add(GetYDataMongo(listResulr, gdataid, deviceJK.IsOnline));
                    List<int> sumbers = new List<int>() { iAnalog + 2, iDigial + 30, iAnalog + 12, iAnalog + 20 };  //净水1泵
                    lstPumps.Add(GetYDataMongo(listResulr, sumbers, deviceJK.IsOnline));
                    List<int> sumbers2 = new List<int>() { iAnalog + 3, iDigial + 31, iAnalog + 13, iAnalog + 21 };  //净水2泵
                    lstPumps.Add(GetYDataMongo(listResulr, sumbers2, deviceJK.IsOnline));
                    List<int> qxbeng = new List<int>() { iAnalog + 5, iDigial + 32 };     //清洗泵
                    lstPumps.Add(GetYDataMongo(listResulr, qxbeng, deviceJK.IsOnline));



                    for (int i = 0; i < 7; i++)
                    {
                        List<int> sumbers3 = new List<int>() { iAnalog + i + 40, iDigial + i + +45 };     //净水3-8泵
                        lstPumps.Add(GetYDataMongo(listResulr, sumbers3, deviceJK.IsOnline));
                    }

                    List<int> fxbeng = new List<int>() { iDigial + 69, iDigial + 70 };     //反洗泵
                    lstPumps.Add(GetYDataMongo(listResulr, fxbeng, deviceJK.IsOnline));
                    deviceJK.lstPumps = lstPumps;
                }
                else
                {

                }
            }

            else
            {

            }
            return Json(new
            {
                deviceJK
            });
        }


        //超滤分离方法
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult LoadZUFStationJK(int sid, string partionid)
        {
            var item = _sws_RTUInfoService.LoadDeviceAndRtuinfo(sid).Where(e => e.Partition == "低区").FirstOrDefault();
            List<WNMS.Model.CustomizedClass.Pump> lstPumps = new List<WNMS.Model.CustomizedClass.Pump>();
            //获取实时监控数据
            var listResulr = _sws_RTUInfoService.GetMongoJKData(item.Rtuid);
            string Section = SimpleFactory.GetParameters(item.Partition.ToString());
            string[] strSection = Section.Split(',');

            DeviceJK_SS_ZYS deviceJK = new DeviceJK_SS_ZYS();
            deviceJK.EquipID = item.EquipID;
            deviceJK.RTUID = item.Rtuid;
            deviceJK.EquipmentType = item.EquipmentType;
            deviceJK.DeviceType = item.DeviceType;
            deviceJK.Partition = item.Partition;
            deviceJK.DeviceName = item.DeviceName;
            deviceJK.IsOnline = "离线";
            List<SwsEventInfo> eventlist_all = new List<SwsEventInfo>();
            eventlist_all = _EventInfoService.Query<SwsEventInfo>(r => r.Rtuid == item.Rtuid).ToList();
            if (listResulr != null)
            {
                deviceJK.UpdateTime = Convert.ToDateTime(listResulr.UpdateTime).ToString("yyyy-MM-dd HH:mm:ss");

                if (strSection.Length == 2)
                {
                    int iAnalog = int.Parse(strSection[0]);
                    int iDigial = int.Parse(strSection[1]);
                    List<int> dataid = new List<int>() { iAnalog + 16, iAnalog + 17, iAnalog + 15, iAnalog + 11, iAnalog + 14, iAnalog + 32, iAnalog + 33, iAnalog + 34, iAnalog + 35, iAnalog + 36, iAnalog + 37, iAnalog + 23, iAnalog + 4, iAnalog + 5,
                        iAnalog + 6, iAnalog + 7, iAnalog + 8, iAnalog + 9, iAnalog + 10, 1001, iAnalog+40,iAnalog+41};
                    deviceJK.YLevel = listResulr.AnalogValues.ContainsKey(dataid[0].ToString()) ? Math.Round(double.Parse((listResulr.AnalogValues[dataid[0].ToString()]).ToString()), 2) : 0;
                    deviceJK.JLevel = listResulr.AnalogValues.ContainsKey(dataid[1].ToString()) ? Math.Round(double.Parse((listResulr.AnalogValues[dataid[1].ToString()]).ToString()), 2) : 0;
                    deviceJK.YOutPressure = listResulr.AnalogValues.ContainsKey(dataid[2].ToString()) ? Math.Round(double.Parse((listResulr.AnalogValues[dataid[2].ToString()]).ToString()), 2) : 0;  //原水出水压力
                    deviceJK.JSetpressure = listResulr.AnalogValues.ContainsKey(dataid[3].ToString()) ? Math.Round(double.Parse((listResulr.AnalogValues[dataid[3].ToString()]).ToString()), 2) : 0;  //净水设定压力
                    deviceJK.JOutPressure = listResulr.AnalogValues.ContainsKey(dataid[4].ToString()) ? Math.Round(double.Parse((listResulr.AnalogValues[dataid[4].ToString()]).ToString()), 2) : 0;   //净水出水压力

                    deviceJK.PH = listResulr.AnalogValues.ContainsKey(dataid[5].ToString()) ? double.Parse((listResulr.AnalogValues[dataid[5].ToString()]).ToString()) : 0;
                    deviceJK.CL = listResulr.AnalogValues.ContainsKey(dataid[6].ToString()) ? double.Parse((listResulr.AnalogValues[dataid[6].ToString()]).ToString()) : 0;
                    deviceJK.Turbidity = listResulr.AnalogValues.ContainsKey(dataid[7].ToString()) ? double.Parse((listResulr.AnalogValues[dataid[7].ToString()]).ToString()) : 0;
                    deviceJK.ORP = listResulr.AnalogValues.ContainsKey(dataid[8].ToString()) ? Math.Round(double.Parse((listResulr.AnalogValues[dataid[8].ToString()]).ToString()), 2) : 0;
                    deviceJK.Salinity = listResulr.AnalogValues.ContainsKey(dataid[9].ToString()) ? Math.Round(double.Parse((listResulr.AnalogValues[dataid[9].ToString()]).ToString()), 2) : 0;
                    deviceJK.Oxygen = listResulr.AnalogValues.ContainsKey(dataid[10].ToString()) ? Math.Round(double.Parse((listResulr.AnalogValues[dataid[10].ToString()]).ToString()), 2) : 0;
                    deviceJK.Conductivity = listResulr.AnalogValues.ContainsKey(dataid[11].ToString()) ? Math.Round(double.Parse((listResulr.AnalogValues[dataid[11].ToString()]).ToString()), 2) : 0;
                    deviceJK.Ssll = listResulr.AnalogValues.ContainsKey(dataid[20].ToString()) ? Math.Round(double.Parse((listResulr.AnalogValues[dataid[20].ToString()]).ToString()), 2) : 0;
                    deviceJK.Ljll = listResulr.AnalogValues.ContainsKey(dataid[21].ToString()) ? Math.Round(double.Parse((listResulr.AnalogValues[dataid[21].ToString()]).ToString()), 2) : 0;
                    int hardnessid = 2039;
                    deviceJK.Hardness = listResulr.AnalogValues.ContainsKey(hardnessid.ToString()) ? Math.Round(double.Parse((listResulr.AnalogValues[hardnessid.ToString()]).ToString()), 2) : 0;

                    deviceJK.GPumpState = listResulr.AnalogValues.ContainsKey(dataid[12].ToString()) ? (double.Parse((listResulr.AnalogValues[dataid[12].ToString()]).ToString()) == 0 ? "停止" : "启动") : "停止";  //高压泵状态
                    deviceJK.QPumpState = listResulr.AnalogValues.ContainsKey(dataid[13].ToString()) ? (double.Parse((listResulr.AnalogValues[dataid[13].ToString()]).ToString()) == 0 ? "停止" : "启动") : "停止";  //清洗泵状态
                    deviceJK.QValveState = listResulr.AnalogValues.ContainsKey(dataid[14].ToString()) ? (double.Parse((listResulr.AnalogValues[dataid[14].ToString()]).ToString()) == 0 ? "关闭" : "打开") : "关闭";  //进水阀状态
                    deviceJK.PValveState = listResulr.AnalogValues.ContainsKey(dataid[15].ToString()) ? (double.Parse((listResulr.AnalogValues[dataid[15].ToString()]).ToString()) == 0 ? "关闭" : "打开") : "关闭";  //旁通阀状态
                    deviceJK.GValveState = listResulr.AnalogValues.ContainsKey(dataid[16].ToString()) ? (double.Parse((listResulr.AnalogValues[dataid[16].ToString()]).ToString()) == 0 ? "关闭" : "打开") : "关闭";  //高压阀状态
                    deviceJK.NValveState = listResulr.AnalogValues.ContainsKey(dataid[17].ToString()) ? (double.Parse((listResulr.AnalogValues[dataid[17].ToString()]).ToString()) == 0 ? "关闭" : "打开") : "关闭";  //浓水阀状态
                    deviceJK.HValveState = listResulr.AnalogValues.ContainsKey(dataid[18].ToString()) ? (double.Parse((listResulr.AnalogValues[dataid[18].ToString()]).ToString()) == 0 ? "关闭" : "打开") : "关闭";  //回水阀状态
                    deviceJK.JJValveState = listResulr.AnalogValues.ContainsKey("2076") ? (double.Parse((listResulr.AnalogValues["2076"]).ToString()) == 0 ? "关闭" : "打开") : "关闭";  //净水箱进水电磁阀状态
                    deviceJK.MQValveState = listResulr.AnalogValues.ContainsKey("2051") ? (double.Parse((listResulr.AnalogValues["2051"]).ToString()) == 0 ? "关闭" : "打开") : "关闭";  //膜前电磁阀状态
                    deviceJK.FJValveState = listResulr.AnalogValues.ContainsKey("2052") ? (double.Parse((listResulr.AnalogValues["2052"]).ToString()) == 0 ? "关闭" : "打开") : "关闭";  //反洗进水电磁阀状态

                    //判断水质是否合格
                    deviceJK.ConHighPolice = listResulr.AnalogValues.ContainsKey("5036") ? (double.Parse((listResulr.AnalogValues["5036"]).ToString()) == 0 ? "合格" : "不合格") : "合格";  //电导率
                    deviceJK.PHBHG = listResulr.AnalogValues.ContainsKey("5037") ? (double.Parse((listResulr.AnalogValues["5037"]).ToString()) == 0 ? "合格" : "不合格") : "合格";  //ph
                    deviceJK.YLCBBJ = listResulr.AnalogValues.ContainsKey("5038") ? (double.Parse((listResulr.AnalogValues["5038"]).ToString()) == 0 ? "合格" : "不合格") : "合格";  //余氯
                    deviceJK.ZDCBBJ = listResulr.AnalogValues.ContainsKey("5039") ? (double.Parse((listResulr.AnalogValues["5039"]).ToString()) == 0 ? "合格" : "不合格") : "合格";  //浊度
                    deviceJK.RYCBBJ = listResulr.AnalogValues.ContainsKey("5040") ? (double.Parse((listResulr.AnalogValues["5040"]).ToString()) == 0 ? "合格" : "不合格") : "合格";  //溶氧
                    deviceJK.ORPCBBJ = listResulr.AnalogValues.ContainsKey("5041") ? (double.Parse((listResulr.AnalogValues["5041"]).ToString()) == 0 ? "合格" : "不合格") : "合格";  //ORP
                    deviceJK.YANDCBBJ = listResulr.AnalogValues.ContainsKey("5042") ? (double.Parse((listResulr.AnalogValues["5042"]).ToString()) == 0 ? "合格" : "不合格") : "合格";  //盐度
                    deviceJK.CODCBBJ = listResulr.AnalogValues.ContainsKey("5043") ? (double.Parse((listResulr.AnalogValues["5043"]).ToString()) == 0 ? "合格" : "不合格") : "合格";  //Cod
                    deviceJK.YINGDCBBJ = listResulr.AnalogValues.ContainsKey("5044") ? (double.Parse((listResulr.AnalogValues["5044"]).ToString()) == 0 ? "合格" : "不合格") : "合格";  //硬度

                    //判断在线离线
                    if (listResulr != null && listResulr.DigitalValues != null)
                    {
                        bool zstate = listResulr.DigitalValues.Keys.Contains("1001") ? Convert.ToBoolean(listResulr.DigitalValues["1001"]) : false;
                        var tstate = listResulr.DigitalValues.Keys.Contains("1000") ? Convert.ToBoolean(listResulr.DigitalValues["1000"]) : false;

                        if (zstate == true && tstate == true && (DateTime.Now - Convert.ToDateTime(listResulr.UpdateTime)).TotalMinutes <= timevalue)
                        {
                            if (eventlist_all.Count() > 0)
                            {
                                deviceJK.IsOnline = "故障";
                            }
                            else
                            {
                                deviceJK.IsOnline = "正常";
                            }
                        }
                        else
                        {
                            deviceJK.IsOnline = "离线";
                        }
                    }

                    //泵的信息
                    List<int> y1dataid = new List<int>() { 5055, 5067, 5056 };  //原水1泵状态
                    lstPumps.Add(GetYUFDataMongo(listResulr, y1dataid, deviceJK.IsOnline, "1#原水泵"));


                    List<int> y2dataid = new List<int>() { 5057, 5068, 5058 };   //原水2泵
                    lstPumps.Add(GetYUFDataMongo(listResulr, y2dataid, deviceJK.IsOnline, "2#原水泵"));
                    //List<int> gdataid = new List<int>() { iAnalog + 4, iDigial + 29, iAnalog + 22 };  //高压泵
                    //lstPumps.Add(GetYDataMongo(listResulr, gdataid, deviceJK.IsOnline));


                    List<int> gdataid = new List<int>() { 5059, 5094, 5060 };  //高压泵
                    lstPumps.Add(GetYUFDataMongo(listResulr, gdataid, deviceJK.IsOnline, "高压泵"));


                    //List<int> sumbers = new List<int>() { iAnalog + 2, iDigial + 30, iAnalog + 12, iAnalog + 20 };  //净水1泵
                    //lstPumps.Add(GetYDataMongo(listResulr, sumbers, deviceJK.IsOnline));
                    List<int> sumbers = new List<int>() { 5061, 5062 };  //净水1泵
                    lstPumps.Add(GetJSYUFDataMongo(listResulr, sumbers, deviceJK.IsOnline, "净水1泵"));


                    //List<int> sumbers2 = new List<int>() { iAnalog + 3, iDigial + 31, iAnalog + 13, iAnalog + 21 };  //净水2泵
                    //lstPumps.Add(GetYDataMongo(listResulr, sumbers2, deviceJK.IsOnline));
                    List<int> sumbers2 = new List<int>() { 5063, 5064 };  //净水2泵
                    lstPumps.Add(GetJSYUFDataMongo(listResulr, sumbers2, deviceJK.IsOnline, "净水2泵"));


                    List<int> qxbeng = new List<int>() { iAnalog + 5, iDigial + 32 };     //清洗泵
                    lstPumps.Add(GetYDataMongo(listResulr, qxbeng, deviceJK.IsOnline));



                    for (int i = 0; i < 6; i++)
                    {
                        List<int> sumbers3 = new List<int>() { iAnalog + i + 40, iDigial + i + +45 };     //净水3-8泵
                        lstPumps.Add(GetYDataMongo(listResulr, sumbers3, deviceJK.IsOnline));
                    }

                    List<int> fxbeng = new List<int>() { 5091, 5070 };     //反洗泵
                    lstPumps.Add(GetYDataMongo(listResulr, fxbeng, deviceJK.IsOnline));
                    deviceJK.lstPumps = lstPumps;
                }
                else
                {

                }
            }

            else
            {

            }
            return Json(new
            {
                deviceJK
            });
        }

        //查询状态 分离方法
        [TypeFilter(typeof(IgonreActionFilter))]
        public WNMS.Model.CustomizedClass.Pump GetYUFDataMongo(DeviceJK deviceJK, List<int> dataid, string state, string name)
        {
            Model.CustomizedClass.Pump pump = new Model.CustomizedClass.Pump();
            pump.PumpName = name;

            bool GPDeviceJKInfo = deviceJK.DigitalValues.ContainsKey(dataid[0].ToString()) ? bool.Parse((deviceJK.DigitalValues[dataid[0].ToString()]).ToString()) : false;
            bool BPDeviceJKInfo = deviceJK.DigitalValues.ContainsKey(dataid[1].ToString()) ? bool.Parse((deviceJK.DigitalValues[dataid[1].ToString()]).ToString()) : false;

            bool GDeviceJKInfo = deviceJK.DigitalValues.ContainsKey(dataid[2].ToString()) ? bool.Parse((deviceJK.DigitalValues[dataid[2].ToString()]).ToString()) : false;
            pump.PFault = "停止";
            pump.Frequency = 50;
            pump.PState = "无";
            if (state == "离线")
            {
                pump.PFault = "停止";
            }
            else
            {
                if (GDeviceJKInfo == true)
                {
                    pump.PFault = "故障";
                }
                else if (GPDeviceJKInfo == true)
                {
                    pump.Frequency = 50;
                    pump.PFault = "工频";
                }
                else if (BPDeviceJKInfo == true)
                {
                    pump.PFault = "变频";
                }
                else
                {
                    pump.Frequency = 0;
                    pump.PFault = "停止";
                }
            }
            pump.Electric = 0;
            return pump;
        }
        //查询状态 井水泵 分离方法
        [TypeFilter(typeof(IgonreActionFilter))]
        public WNMS.Model.CustomizedClass.Pump GetJSYUFDataMongo(DeviceJK deviceJK, List<int> dataid, string state, string name)
        {
            Model.CustomizedClass.Pump pump = new Model.CustomizedClass.Pump();
            pump.PumpName = name;

            //bool GPDeviceJKInfo = deviceJK.DigitalValues.ContainsKey(dataid[0].ToString()) ? bool.Parse((deviceJK.DigitalValues[dataid[0].ToString()]).ToString()) : false;
            bool BPDeviceJKInfo = deviceJK.DigitalValues.ContainsKey(dataid[0].ToString()) ? bool.Parse((deviceJK.DigitalValues[dataid[0].ToString()]).ToString()) : false;

            bool GDeviceJKInfo = deviceJK.DigitalValues.ContainsKey(dataid[1].ToString()) ? bool.Parse((deviceJK.DigitalValues[dataid[1].ToString()]).ToString()) : false;
            pump.PFault = "停止";
            pump.Frequency = 50;
            pump.PState = "无";
            if (state == "离线")
            {
                pump.PFault = "停止";
            }
            else
            {
                if (GDeviceJKInfo == true)
                {
                    pump.PFault = "故障";
                }

                else if (BPDeviceJKInfo == true)
                {
                    pump.PFault = "变频";
                }
                else
                {
                    pump.Frequency = 0;
                    pump.PFault = "停止";
                }
            }
            pump.Electric = 0;
            return pump;
        }

        //查询状态
        [TypeFilter(typeof(IgonreActionFilter))]
        public WNMS.Model.CustomizedClass.Pump GetYDataMongo(DeviceJK deviceJK, List<int> dataid, string state)
        {
            Model.CustomizedClass.Pump pump = new Model.CustomizedClass.Pump();
            if (dataid[0] == 2000)
            {
                pump.PumpName = "1#原水泵";
            }
            if (dataid[0] == 2001)
            {
                pump.PumpName = "2#原水泵";
            }
            if (dataid[0] == 2002)
            {
                pump.PumpName = "1#净水泵";
            }
            if (dataid[0] == 2003)
            {
                pump.PumpName = "2#净水泵";
            }
            if (dataid[0] == 2004)
            {
                pump.PumpName = "高压泵";
            }
            if (dataid[0] == 2005)
            {
                pump.PumpName = "清洗泵";
            }
            bool GDeviceJKInfo = deviceJK.DigitalValues.ContainsKey(dataid[1].ToString()) ? bool.Parse((deviceJK.DigitalValues[dataid[1].ToString()]).ToString()) : false;
            double ZDeviceJKInfo = deviceJK.AnalogValues.ContainsKey(dataid[0].ToString()) ? double.Parse((deviceJK.AnalogValues[dataid[0].ToString()]).ToString()) : 0;
            pump.PFault = "停止";
            pump.Frequency = 50;
            pump.PState = "无";
            if (state == "离线")
            {
                pump.PFault = "停止";
            }
            else
            {
                if (GDeviceJKInfo == true)
                {
                    pump.PFault = "故障";
                }
                else if (ZDeviceJKInfo == 1)
                {
                    pump.PFault = "正常";
                }
                else if (ZDeviceJKInfo == 0)
                {
                    pump.Frequency = 0;
                    pump.PFault = "停止";
                }
                else if (ZDeviceJKInfo == 2)
                {
                    pump.PFault = "工频";
                }
                else
                {
                    pump.Frequency = 0;
                    pump.PFault = "停止";
                }
            }
            pump.Electric = 0;
            return pump;
        }
        //高压泵状态
        [TypeFilter(typeof(IgonreActionFilter))]
        public WNMS.Model.CustomizedClass.Pump GetGYDataMongo(DeviceJK deviceJK, List<int> dataid, string state)
        {
            Model.CustomizedClass.Pump pump = new Model.CustomizedClass.Pump();
            if (dataid[0] == 2000)
            {
                pump.PumpName = "1#原水泵";
            }
            if (dataid[0] == 2001)
            {
                pump.PumpName = "2#原水泵";
            }
            if (dataid[0] == 2002)
            {
                pump.PumpName = "1#净水泵";
            }
            if (dataid[0] == 2003)
            {
                pump.PumpName = "2#净水泵";
            }
            if (dataid[0] == 2004)
            {
                pump.PumpName = "高压泵";
            }
            if (dataid[0] == 2005)
            {
                pump.PumpName = "清洗泵";
            }
            bool GDeviceJKInfo = deviceJK.DigitalValues.ContainsKey(dataid[1].ToString()) ? bool.Parse((deviceJK.DigitalValues[dataid[1].ToString()]).ToString()) : false;
            double ZDeviceJKInfo = deviceJK.AnalogValues.ContainsKey(dataid[0].ToString()) ? double.Parse((deviceJK.AnalogValues[dataid[0].ToString()]).ToString()) : 0;
            pump.PFault = "停止";
            pump.Frequency = 50;
            pump.PState = "无";
            if (state == "离线")
            {
                pump.PFault = "停止";
            }
            else
            {
                if (GDeviceJKInfo == true)
                {
                    pump.PFault = "故障";
                }
                else if (ZDeviceJKInfo == 1)
                {
                    pump.PFault = "正常";
                }
                else if (ZDeviceJKInfo == 0)
                {
                    pump.Frequency = 0;
                    pump.PFault = "停止";
                }
                else if (ZDeviceJKInfo == 2)
                {
                    pump.PFault = "工频";
                }
                else
                {
                    pump.Frequency = 0;
                    pump.PFault = "停止";
                }
            }
            pump.Electric = 0;
            return pump;
        }
        //直饮水更多数据
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult LoadMoreZData(int id)
        {
            List<PartitionData> data = new List<PartitionData>();
            //获取泵房RTUID
            var dataList = stationService.GetStaionById("2", id);
            var alldataInfo = _sws_DataInfoService.Query<SwsDataInfo>(r => r.DeviceType == 2).ToList();
            var rtuidlist = dataList.Where(r => r.RTUID.ToString() != "").Select(r => r.RTUID).Distinct();
            var rtuids = string.Join(",", rtuidlist);
            string macJson = "{\"RTUID\":{'$in':[" + rtuids + "]}}";
            var collections = new MongoDBHelper<RtuJKInfo>(DateTime.Now.Year.ToString());
            var jklist = collections.Find("Sws_DeviceJKInfo", macJson, "UpdateTime");
            var stationdata = dataList.GroupBy(r => new { r.StationID, r.StationName, r.Lng, r.Lat }).OrderBy(r => r.Key.StationName).FirstOrDefault();
            List<string> partitionName = new List<string>();
            foreach (var it in stationdata)
            {
                PartitionData partitionData = new PartitionData();
                if (it.RTUID.ToString() != "")
                {
                    int Rtuidd = Convert.ToInt32(it.RTUID);
                    var partion = Convert.ToInt32(it.Partition.ToString());
                    var datajk = jklist.Where(r => r.RTUID == Rtuidd).FirstOrDefault();
                    if (partion == 1)
                    {
                        partitionData.partition = "低区";
                    }
                    else if (partion == 2)
                    {
                        partitionData.partition = "中区";
                    }
                    else if (partion == 3)
                    {
                        partitionData.partition = "高区";
                    }
                    else if (partion == 4)
                    {
                        partitionData.partition = "超区";
                    }
                    else if (partion == 5)
                    {
                        partitionData.partition = "超超高区";
                    }
                    partitionName.Add(partitionData.partition);
                    int startid = 2000;//开始dataid
                    int endid = 2500;//结束dataid 模拟量
                    int tstartid = 5000;//开关量
                    int tendid = 5500;
                    List<KeyValuePair<string, object>> alist = new List<KeyValuePair<string, object>>();
                    List<PartitionDatdid> pdList = new List<PartitionDatdid>();
                    if (datajk != null && datajk.AnalogValues != null)
                    {
                        foreach (var ite in datajk.AnalogValues)
                        {
                            var dataInfo = alldataInfo.Where(r => r.DataId == int.Parse(ite.Key) && r.DeviceType == 2).FirstOrDefault();
                            if (dataInfo != null)
                            {
                                if ((startid + (partion - 1) * 500) <= int.Parse(ite.Key) && int.Parse(ite.Key) < (endid + (partion - 1) * 500))
                                {
                                    //KeyValuePair<string, object> keyValuePair = new KeyValuePair<string, object>(dataInfo.Cnname, ite.Value + dataInfo.Unit);
                                    PartitionDatdid partitionDatdid = new PartitionDatdid()
                                    {
                                        keyValue = ite.Value.ToString(),
                                        Unit = dataInfo.Unit,
                                        Cnname = dataInfo.Cnname
                                    };
                                    //alist.Add(keyValuePair);
                                    pdList.Add(partitionDatdid);
                                }
                            }


                        }
                    }
                    if (datajk != null && datajk.DigitalValues != null)
                    {
                        foreach (var ite in datajk.DigitalValues)
                        {
                            var dataInfo = alldataInfo.Where(r => r.DataId == int.Parse(ite.Key) && r.DeviceType == 2).FirstOrDefault();
                            if (dataInfo != null)
                            {
                                if ((tstartid + (partion - 1) * 500) <= int.Parse(ite.Key) && int.Parse(ite.Key) < (tendid + (partion - 1) * 500))
                                {
                                    //KeyValuePair<string, object> keyValuePair = new KeyValuePair<string, object>(dataInfo.Cnname, ite.Value);
                                    //alist.Add(keyValuePair); 
                                    PartitionDatdid partitionDatdid = new PartitionDatdid()
                                    {
                                        keyValue = ite.Value.ToString(),
                                        Unit = dataInfo.Unit,
                                        Cnname = dataInfo.Cnname
                                    };
                                    pdList.Add(partitionDatdid);
                                }
                            }


                        }
                    }
                    //partitionData.akeyValuePairs = alist;
                    partitionData.partitionDatdids = pdList;
                    data.Add(partitionData);
                }
                else
                {
                }
            }
            ViewBag.partitionName = partitionName;
            ViewBag.data = data;
            return View();
        }

        //获取地图鹰眼数据
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult LoadZMapInfo(int sid)
        {
            //获取泵房RTUID
            var dataList = stationService.GetZStaionById("", sid);
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
                var devicelist = stationdata.Where(r => r.StationID == sid);
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
                            jk.Turbidity = jkdata.Turbidity;
                            //余氯
                            int clid = 2033;
                            jkdata.CL = datajk.AnalogValues.ContainsKey(clid.ToString()) ? datajk.AnalogValues[clid.ToString()].ToString() : "--";
                            jk.CL = jkdata.CL;
                            //ph
                            int tphid = 2032;
                            jkdata.PH = datajk.AnalogValues.ContainsKey(tphid.ToString()) ? datajk.AnalogValues[tphid.ToString()].ToString() : "--";
                            jk.PH = jkdata.PH;
                            //PH
                            var cph = datajk.AnalogValues.ContainsKey(tphid.ToString()) ? datajk.AnalogValues[tphid.ToString()] : 0;

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
        #endregion
        #region  阀门页面
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult LoadFa(int id, int type)
        {
            //获取泵房设备信息
            var valuelist = _sws_DeviceInfo01Service.QueryFaInfo(id, type);
            ViewBag.valuelist = valuelist;
            //var deviceInfo = _sws_DeviceInfo01Service.Query<SwsDeviceInfo01>(r => r.StationId == sid).ToList();
            //if(deviceInfo.Count > 0)
            //{
            //    var ids = deviceInfo.Select(r => r.DeviceId).ToList();
            //    var valuelist = valveService.Query<SwsValveWith01>(r => ids.Contains(r.DeviceId)).ToList();
            //    ViewBag.valuelist = valuelist;
            //}else
            //{
            //    ViewBag.valuelist = "";
            //}
            return View();
        }
        #endregion

        #region 7泵设备信息
        //查询7泵设备信息
        [TypeFilter(typeof(IgonreActionFilter))]
        [TypeFilter(typeof(IgonreLoginFilter))]
        public IActionResult Load7StationJKPartuion(int sid, int partionid)
        {
            var enumfrequency = (int)Model.CustomizedClass.Enum.变频分类;
            var ftype = _DataItemDetailService.Query<SysDataItemDetail>(r => r.FItemId == enumfrequency).ToList();
            var deviceInfo = _sws_DeviceInfo01Service.Query<SwsDeviceInfo01>(r => r.StationId == sid && r.Partition == partionid).FirstOrDefault();
            if (deviceInfo == null)
            {
                montorGuiJK jkdata1 = new montorGuiJK();
                jkdata1.datetime = "--";//更新时间
                jkdata1.RtuID = 0;
                jkdata1.State = "离线";
                jkdata1.PressIN = new KeyValuePair<string, bool>("--", false);
                jkdata1.PressOut = new KeyValuePair<string, bool>("--", false);
                jkdata1.pumpState = new List<string>();
                jkdata1.pumpdatas = new List<PumpData>();
                jkdata1.ValveFault = new List<string>();
                jkdata1.totalflow = "--";
                jkdata1.totalpower = "--";
                jkdata1.inflow = "--";
                jkdata1.Temperature = "--";
                for (var i = 0; i < 7; i++)
                {
                    PumpData p = new PumpData();
                    p.eletric = new KeyValuePair<string, bool>("--", false);
                    p.frequency = new KeyValuePair<string, bool>("--", false);
                    p.runtime = new KeyValuePair<string, bool>("--", false);
                    jkdata1.pumpdatas.Add(p);
                    jkdata1.pumpState.Add("停止");
                }
                for (var i = 0; i < 7; i++)
                {
                    string state = "待确定";
                    jkdata1.ValveFault.Add(state);
                }
                return Json(new
                {

                    jkdata = jkdata1

                });
            }
            var rtuID = deviceInfo.Rtuid;
            //int PumpNum = deviceInfo.PumpNum;//泵数量
            int PumpNum = deviceInfo.PumpNum;//泵数量
            montorGuiJK jkdata = new montorGuiJK();
            if (rtuID != null)
            {
                string macJson = "{\"RTUID\":{'$in':[" + rtuID + "]}}";
                var collections = new MongoDBHelper<RtuJKInfo>(DateTime.Now.Year.ToString());
                var jklist = collections.Find("Sws_DeviceJKInfo", macJson, "UpdateTime").FirstOrDefault();
                List<SwsEventInfo> eventlist_all = new List<SwsEventInfo>();
                eventlist_all = _EventInfoService.Query<SwsEventInfo>(r => r.Rtuid == rtuID).ToList();
                var datajk = jklist;
                #region 状态
                var state = "离线";
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
                        if (zstate == true && tstate == true && (DateTime.Now - Convert.ToDateTime(datajk.UpdateTime)).TotalMinutes <= timevalue && eventlist_all.Count() == 0)
                        {
                            state = "正常";
                        }
                        else
                        {
                            state = "故障";
                        }

                    }

                }
                else
                {
                    state = "离线";
                }
                if (datajk != null)
                {
                    var updatetime = datajk.UpdateTime;
                    jkdata.datetime = updatetime.ToString();//更新时间 
                    jkdata.RtuID = (int)rtuID;
                    jkdata.State = state;
                    var partion = partionid;
                    //进水压力
                    int pressinid = 2000 + (partion - 1) * 500;
                    jkdata.PressIN = new KeyValuePair<string, bool>(datajk.AnalogValues.ContainsKey(pressinid.ToString()) ? datajk.AnalogValues[pressinid.ToString()].ToString() : "--",
                     eventlist_all.Count > 0 ? (eventlist_all.Where(r => r.EventSource == pressinid && r.EventLevel != 0 && r.Rtuid == rtuID).Count() > 0 ? true : false) : false);
                    //出水压力
                    int pressout = 2001 + (partion - 1) * 500;
                    jkdata.PressOut = new KeyValuePair<string, bool>(datajk.AnalogValues.ContainsKey(pressout.ToString()) ? datajk.AnalogValues[pressout.ToString()].ToString() : "--",
                    eventlist_all.Count > 0 ? (eventlist_all.Where(r => r.EventSource == pressout && r.EventLevel != 0 && r.Rtuid == rtuID).Count() > 0 ? true : false) : false);

                    //水泵数据
                    #region 水泵数据
                    var fixfrequencyvalue = 2003 + (partion - 1) * 500;//频率
                    var frequencyvalue = 2004 + (partion - 1) * 500;//频率
                    var electricvalue = 2010 + (partion - 1) * 500;//电流
                    var pumpstatevalue = 5000 + (partion - 1) * 500;//方式
                    var guzhangvalue = 5003 + (partion - 1) * 500;//状态
                    var bianpinvalue = 5001 + (partion - 1) * 500;//变频
                    var hengpinvalue = 5127 + (partion - 1) * 500;//恒频
                    var gongpinvalue = 5002 + (partion - 1) * 500;//工频
                    var yunxingTime = 2017 + (partion - 1) * 500;//运行时间//datainfo 和2017重复


                    List<string> pumpState = new List<string>();//泵状态        
                    List<PumpData> pumpdataList = new List<PumpData>();//泵频率,电流、运行时间

                    var frequencyflg = deviceInfo.Frequency.ToString();
                    int j = 0;
                    var fname = ftype.Count() > 0 ? (ftype.Where(r => r.ItemValue == frequencyflg).FirstOrDefault()?.ItemName) : "单变频";

                    for (var i = 0; i < 4; i++)
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
                        if (state != "离线")
                        {
                            //if ((pump.PFault == "变频" || pump.PFault == "恒频") && pump.Frequency != "0")
                            //{
                            //    pumpState.Add("变频");

                            //}
                            if ((pump.PFault == "变频") && pump.Frequency != "0")
                            {
                                pumpState.Add("变频");

                            }
                            else if (pump.PFault == "恒频" && pump.Frequency != "0")
                            {
                                pumpState.Add("恒频");
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
                                pdatlist.frequency = new KeyValuePair<string, bool>(pump.Frequency, eventlist_all.Count > 0 ? (eventlist_all.Where(r => r.EventSource == fixfrequencyvalue && r.EventLevel != 0 && r.Rtuid == rtuID).Count() > 0 ? true : false) : false);
                            }
                            else
                            {
                                pdatlist.frequency = new KeyValuePair<string, bool>(pump.Frequency, false);
                            }
                        }
                        else
                        {
                            var fid = frequencyvalue + i;
                            pdatlist.frequency = new KeyValuePair<string, bool>(pump.Frequency, eventlist_all.Count > 0 ? (eventlist_all.Where(r => r.EventSource == fid && r.EventLevel != 0 && r.Rtuid == rtuID).Count() > 0 ? true : false) : false);
                        }
                        var runtimeid = yunxingTime + i;
                        pdatlist.runtime = new KeyValuePair<string, bool>(pump.RunHour.ToString(), eventlist_all.Count > 0 ? (eventlist_all.Where(r => r.EventSource == runtimeid && r.EventLevel != 0 && r.Rtuid == rtuID).Count() > 0 ? true : false) : false);
                        var eltricid = electricvalue + i;
                        pdatlist.eletric = new KeyValuePair<string, bool>(pump.Electric.ToString(), eventlist_all.Count > 0 ? (eventlist_all.Where(r => r.EventSource == eltricid && r.EventLevel != 0 && r.Rtuid == rtuID).Count() > 0 ? true : false) : false);
                        pumpdataList.Add(pdatlist);

                    }
                    //5泵
                    {
                        fixfrequencyvalue = 2003 + (partion - 1) * 500;//频率
                        frequencyvalue = 2008 + (partion - 1) * 500;//频率
                        electricvalue = 2014 + (partion - 1) * 500;//电流
                        pumpstatevalue = 5016 + (partion - 1) * 500;//方式
                        guzhangvalue = 5019 + (partion - 1) * 500;//状态
                        bianpinvalue = 5017 + (partion - 1) * 500;//变频
                        hengpinvalue = 5131 + (partion - 1) * 500;//恒频
                        gongpinvalue = 5018 + (partion - 1) * 500;//工频
                        yunxingTime = 2021 + (partion - 1) * 500;//运行时间//datainfo 和2017重复


                        PumpData pdatlist = new PumpData();

                        List<int> dataids = new List<int>();
                        if (fname.Contains("单变频"))
                        {
                            dataids = new List<int>() { pumpstatevalue, fixfrequencyvalue, electricvalue, guzhangvalue, bianpinvalue, hengpinvalue, gongpinvalue, yunxingTime };

                        }
                        else
                        {
                            dataids = new List<int>() { pumpstatevalue, frequencyvalue, electricvalue, guzhangvalue, bianpinvalue, hengpinvalue, gongpinvalue, yunxingTime };

                        }
                        Pump pump = GetDataByDataID(datajk, dataids);
                        if (state != "离线")
                        {
                            if ((pump.PFault == "变频") && pump.Frequency != "0")
                            {
                                pumpState.Add("变频");

                            }
                            else if (pump.PFault == "恒频" && pump.Frequency != "0")
                            {
                                pumpState.Add("恒频");
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
                            if (pumpState[4] == "变频" || pumpState[4] == "工频")
                            {
                                pdatlist.frequency = new KeyValuePair<string, bool>(pump.Frequency, eventlist_all.Count > 0 ? (eventlist_all.Where(r => r.EventSource == fixfrequencyvalue && r.EventLevel != 0 && r.Rtuid == rtuID).Count() > 0 ? true : false) : false);
                            }
                            else
                            {
                                pdatlist.frequency = new KeyValuePair<string, bool>(pump.Frequency, false);
                            }
                        }
                        else
                        {
                            var fid = frequencyvalue;
                            pdatlist.frequency = new KeyValuePair<string, bool>(pump.Frequency, eventlist_all.Count > 0 ? (eventlist_all.Where(r => r.EventSource == fid && r.EventLevel != 0 && r.Rtuid == rtuID).Count() > 0 ? true : false) : false);
                        }
                        var runtimeid = yunxingTime;
                        pdatlist.runtime = new KeyValuePair<string, bool>(pump.RunHour.ToString(), eventlist_all.Count > 0 ? (eventlist_all.Where(r => r.EventSource == runtimeid && r.EventLevel != 0 && r.Rtuid == rtuID).Count() > 0 ? true : false) : false);
                        var eltricid = electricvalue;
                        pdatlist.eletric = new KeyValuePair<string, bool>(pump.Electric.ToString(), eventlist_all.Count > 0 ? (eventlist_all.Where(r => r.EventSource == eltricid && r.EventLevel != 0 && r.Rtuid == rtuID).Count() > 0 ? true : false) : false);
                        pumpdataList.Add(pdatlist);


                    }
                    //6泵
                    {
                        fixfrequencyvalue = 2003 + (partion - 1) * 500;//频率
                        frequencyvalue = 2009 + (partion - 1) * 500;//频率
                        electricvalue = 2015 + (partion - 1) * 500;//电流
                        pumpstatevalue = 5020 + (partion - 1) * 500;//方式
                        guzhangvalue = 5023 + (partion - 1) * 500;//状态
                        bianpinvalue = 5021 + (partion - 1) * 500;//变频
                        hengpinvalue = 5251 + (partion - 1) * 500;//恒频
                        gongpinvalue = 5022 + (partion - 1) * 500;//工频
                        yunxingTime = 2022 + (partion - 1) * 500;//运行时间//datainfo 和2017重复


                        PumpData pdatlist = new PumpData();

                        List<int> dataids = new List<int>();
                        if (fname.Contains("单变频"))
                        {
                            dataids = new List<int>() { pumpstatevalue, fixfrequencyvalue, electricvalue, guzhangvalue, bianpinvalue, hengpinvalue, gongpinvalue, yunxingTime };

                        }
                        else
                        {
                            dataids = new List<int>() { pumpstatevalue, frequencyvalue, electricvalue, guzhangvalue, bianpinvalue, hengpinvalue, gongpinvalue, yunxingTime };

                        }
                        Pump pump = GetDataByDataID(datajk, dataids);
                        if (state != "离线")
                        {
                            if ((pump.PFault == "变频") && pump.Frequency != "0")
                            {
                                pumpState.Add("变频");

                            }
                            else if (pump.PFault == "恒频" && pump.Frequency != "0")
                            {
                                pumpState.Add("恒频");
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
                            if (pumpState[5] == "变频" || pumpState[5] == "工频")
                            {
                                pdatlist.frequency = new KeyValuePair<string, bool>(pump.Frequency, eventlist_all.Count > 0 ? (eventlist_all.Where(r => r.EventSource == fixfrequencyvalue && r.EventLevel != 0 && r.Rtuid == rtuID).Count() > 0 ? true : false) : false);
                            }
                            else
                            {
                                pdatlist.frequency = new KeyValuePair<string, bool>(pump.Frequency, false);
                            }
                        }
                        else
                        {
                            var fid = frequencyvalue;
                            pdatlist.frequency = new KeyValuePair<string, bool>(pump.Frequency, eventlist_all.Count > 0 ? (eventlist_all.Where(r => r.EventSource == fid && r.EventLevel != 0 && r.Rtuid == rtuID).Count() > 0 ? true : false) : false);
                        }
                        var runtimeid = yunxingTime;
                        pdatlist.runtime = new KeyValuePair<string, bool>(pump.RunHour.ToString(), eventlist_all.Count > 0 ? (eventlist_all.Where(r => r.EventSource == runtimeid && r.EventLevel != 0 && r.Rtuid == rtuID).Count() > 0 ? true : false) : false);
                        var eltricid = electricvalue;
                        pdatlist.eletric = new KeyValuePair<string, bool>(pump.Electric.ToString(), eventlist_all.Count > 0 ? (eventlist_all.Where(r => r.EventSource == eltricid && r.EventLevel != 0 && r.Rtuid == rtuID).Count() > 0 ? true : false) : false);
                        pumpdataList.Add(pdatlist);
                    }
                    //7泵
                    {
                        fixfrequencyvalue = 2003 + (partion - 1) * 500;//频率
                        frequencyvalue = 2164 + (partion - 1) * 500;//频率
                        electricvalue = 2154 + (partion - 1) * 500;//电流
                        pumpstatevalue = 2150 + (partion - 1) * 500;//方式  低区7#泵状态
                        guzhangvalue = 5249 + (partion - 1) * 500;// 故障
                        bianpinvalue = 5247 + (partion - 1) * 500;//变频
                        hengpinvalue = 5252 + (partion - 1) * 500;//恒频
                        gongpinvalue = 5248 + (partion - 1) * 500;//工频
                        yunxingTime = 2159 + (partion - 1) * 500;//运行时间//datainfo 和2017重复


                        PumpData pdatlist = new PumpData();

                        List<int> dataids = new List<int>();
                        if (fname.Contains("单变频"))
                        {
                            dataids = new List<int>() { pumpstatevalue, fixfrequencyvalue, electricvalue, guzhangvalue, bianpinvalue, hengpinvalue, gongpinvalue, yunxingTime };

                        }
                        else
                        {
                            dataids = new List<int>() { pumpstatevalue, frequencyvalue, electricvalue, guzhangvalue, bianpinvalue, hengpinvalue, gongpinvalue, yunxingTime };

                        }
                        Pump pump = GetDataByDataID(datajk, dataids);
                        if (state != "离线")
                        {
                            if ((pump.PFault == "变频") && pump.Frequency != "0")
                            {
                                pumpState.Add("变频");

                            }
                            else if (pump.PFault == "恒频" && pump.Frequency != "0")
                            {
                                pumpState.Add("恒频");
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
                            if (pumpState[6] == "变频" || pumpState[6] == "工频")
                            {
                                pdatlist.frequency = new KeyValuePair<string, bool>(pump.Frequency, eventlist_all.Count > 0 ? (eventlist_all.Where(r => r.EventSource == fixfrequencyvalue && r.EventLevel != 0 && r.Rtuid == rtuID).Count() > 0 ? true : false) : false);
                            }
                            else
                            {
                                pdatlist.frequency = new KeyValuePair<string, bool>(pump.Frequency, false);
                            }
                        }
                        else
                        {
                            var fid = frequencyvalue;
                            pdatlist.frequency = new KeyValuePair<string, bool>(pump.Frequency, eventlist_all.Count > 0 ? (eventlist_all.Where(r => r.EventSource == fid && r.EventLevel != 0 && r.Rtuid == rtuID).Count() > 0 ? true : false) : false);
                        }
                        var runtimeid = yunxingTime;
                        pdatlist.runtime = new KeyValuePair<string, bool>(pump.RunHour.ToString(), eventlist_all.Count > 0 ? (eventlist_all.Where(r => r.EventSource == runtimeid && r.EventLevel != 0 && r.Rtuid == rtuID).Count() > 0 ? true : false) : false);
                        var eltricid = electricvalue;
                        pdatlist.eletric = new KeyValuePair<string, bool>(pump.Electric.ToString(), eventlist_all.Count > 0 ? (eventlist_all.Where(r => r.EventSource == eltricid && r.EventLevel != 0 && r.Rtuid == rtuID).Count() > 0 ? true : false) : false);
                        pumpdataList.Add(pdatlist);
                    }
                    //for (var i = 0; i < PumpNum; i++)
                    //{
                    //    PumpData pdatlist = new PumpData();

                    //    List<int> dataids = new List<int>();
                    //    if (fname.Contains("单变频"))
                    //    {
                    //        dataids = new List<int>() { pumpstatevalue + j, fixfrequencyvalue, electricvalue + i, guzhangvalue + j, bianpinvalue + j, hengpinvalue + i, gongpinvalue + j, yunxingTime + i };
                    //        j += 4;
                    //    }
                    //    else
                    //    {
                    //        dataids = new List<int>() { pumpstatevalue + j, frequencyvalue + i, electricvalue + i, guzhangvalue + j, bianpinvalue + j, hengpinvalue + i, gongpinvalue + j, yunxingTime + i };
                    //        j += 4;
                    //    }
                    //    Pump pump = GetDataByDataID(datajk, dataids);
                    //    if (state != "离线")
                    //    {
                    //        if ((pump.PFault == "变频" || pump.PFault == "恒频") && pump.Frequency != "0")
                    //        {
                    //            pumpState.Add("变频");

                    //        }
                    //        else if (pump.PFault == "工频" && pump.Frequency != "0")
                    //        {
                    //            pumpState.Add("工频");
                    //        }
                    //        else if (pump.PFault == "故障")
                    //        {
                    //            pumpState.Add("故障");
                    //        }
                    //        else
                    //        {
                    //            pumpState.Add("停止");
                    //        }

                    //    }
                    //    else
                    //    {
                    //        pumpState.Add("停止");

                    //    }
                    //    if (fname.Contains("单变频"))
                    //    {
                    //        if (pumpState[i] == "变频" || pumpState[i] == "工频")
                    //        {
                    //            pdatlist.frequency = new KeyValuePair<string, bool>(pump.Frequency, eventlist_all.Count > 0 ? (eventlist_all.Where(r => r.EventSource == fixfrequencyvalue && r.EventLevel != 0 && r.Rtuid == rtuID).Count() > 0 ? true : false) : false);
                    //        }
                    //        else
                    //        {
                    //            pdatlist.frequency = new KeyValuePair<string, bool>(pump.Frequency, false);
                    //        }
                    //    }
                    //    else
                    //    {
                    //        var fid = frequencyvalue + i;
                    //        pdatlist.frequency = new KeyValuePair<string, bool>(pump.Frequency, eventlist_all.Count > 0 ? (eventlist_all.Where(r => r.EventSource == fid && r.EventLevel != 0 && r.Rtuid == rtuID).Count() > 0 ? true : false) : false);
                    //    }
                    //    var runtimeid = yunxingTime + i;
                    //    pdatlist.runtime = new KeyValuePair<string, bool>(pump.RunHour.ToString(), eventlist_all.Count > 0 ? (eventlist_all.Where(r => r.EventSource == runtimeid && r.EventLevel != 0 && r.Rtuid == rtuID).Count() > 0 ? true : false) : false);
                    //    var eltricid = electricvalue + i;
                    //    pdatlist.eletric = new KeyValuePair<string, bool>(pump.Electric.ToString(), eventlist_all.Count > 0 ? (eventlist_all.Where(r => r.EventSource == eltricid && r.EventLevel != 0 && r.Rtuid == rtuID).Count() > 0 ? true : false) : false);
                    //    pumpdataList.Add(pdatlist);

                    //}


                    #endregion


                    #region 电动阀状态
                    //电动阀状态
                    List<string> valveFaultList = new List<string>();//电动阀状态
                    List<string> valveValueList = new List<string>();//电动阀开度
                    int faultvalue = 2136;//电动阀开关
                    for (int i = 0; i < 7; i++)
                    {

                        var valve = datajk.AnalogValues.ContainsKey((faultvalue + i + (partion - 1) * 500).ToString()) ? double.Parse(datajk.AnalogValues[(faultvalue + i + (partion - 1) * 500).ToString()].ToString()) : 0;
                        if (valve == 0)
                        {
                            valveFaultList.Add("关");
                        }
                        else if (valve == 1)
                        {
                            valveFaultList.Add("开");
                        }
                        else if (valve == 2)
                        {
                            valveFaultList.Add("故障");
                        }
                        else
                        {
                            valveFaultList.Add("开");
                        }
                    }

                    {
                        //中站电动阀 
                        var valve = datajk.DigitalValues.ContainsKey((5240 + (partion - 1) * 500).ToString()) ? bool.Parse(datajk.DigitalValues[(5240 + (partion - 1) * 500).ToString()].ToString()) : false;
                        if (valve == false)
                        {
                            var infdata = datajk.AnalogValues.ContainsKey((2133 + (partion - 1) * 500).ToString()) ? double.Parse(datajk.AnalogValues[(2133 + (partion - 1) * 500).ToString()].ToString()) : 0;

                            valveValueList.Add(infdata.ToString());

                            if (infdata == 0)
                            {
                                valveFaultList.Add("关");
                            }
                            else
                            {
                                valveFaultList.Add("开");
                            }
                        }
                        else
                        {
                            valveFaultList.Add("故障");
                        }
                    }
                    {
                        //分割电动阀 
                        var valve = datajk.DigitalValues.ContainsKey((5242 + (partion - 1) * 500).ToString()) ? bool.Parse(datajk.DigitalValues[(5242 + (partion - 1) * 500).ToString()].ToString()) : false;
                        if (valve == false)
                        {
                            var infdata = datajk.AnalogValues.ContainsKey((2135 + (partion - 1) * 500).ToString()) ? double.Parse(datajk.AnalogValues[(2135 + (partion - 1) * 500).ToString()].ToString()) : 0;
                            valveValueList.Add(infdata.ToString());

                            if (infdata == 0)
                            {
                                valveFaultList.Add("关");
                            }
                            else
                            {
                                valveFaultList.Add("开");
                            }
                        }
                        else
                        {
                            valveFaultList.Add("故障");
                        }
                    }
                    {
                        //峰林电动阀 
                        var valve = datajk.DigitalValues.ContainsKey((5241 + (partion - 1) * 500).ToString()) ? bool.Parse(datajk.DigitalValues[(5241 + (partion - 1) * 500).ToString()].ToString()) : false;
                        if (valve == false)
                        {
                            var infdata = datajk.AnalogValues.ContainsKey((2134 + (partion - 1) * 500).ToString()) ? double.Parse(datajk.AnalogValues[(2134 + (partion - 1) * 500).ToString()].ToString()) : 0;
                            valveValueList.Add(infdata.ToString());

                            if (infdata == 0)
                            {
                                valveFaultList.Add("关");
                            }
                            else
                            {
                                valveFaultList.Add("开");
                            }
                        }
                        else
                        {
                            valveFaultList.Add("故障");
                        }
                    }


                    //for (int i = 0; i < 3; i++)
                    //{


                    //    var valve = datajk.DigitalValues.ContainsKey((zvalgz + i + (partion - 1) * 500).ToString()) ? bool.Parse(datajk.DigitalValues[(zvalgz + i + (partion - 1) * 500).ToString()].ToString()) : false;
                    //    if (valve == false)
                    //    {
                    //        var infdata = datajk.AnalogValues.ContainsKey((zvalue + i + (partion - 1) * 500).ToString()) ? double.Parse(datajk.AnalogValues[(zvalue + i + (partion - 1) * 500).ToString()].ToString()) : 0;
                    //        if (infdata == 0)
                    //        {
                    //            valveFaultList.Add("关");
                    //        }
                    //        else if (infdata == 1)
                    //        {
                    //            valveFaultList.Add("开");
                    //        }
                    //        else if (infdata == 2)
                    //        {
                    //            valveFaultList.Add("故障");
                    //        }
                    //    }
                    //    else
                    //    {
                    //        valveFaultList.Add("关");
                    //    }
                    //}



                    #endregion 
                    //总泵的累计流量
                    jkdata.totalflow = datajk.AnalogValues.ContainsKey((2156).ToString()) ? datajk.AnalogValues[(2156).ToString()].ToString() : "0";

                    //总泵的瞬时流量
                    jkdata.inflow = datajk.AnalogValues.ContainsKey((2155).ToString()) ? datajk.AnalogValues[(2155).ToString()].ToString() : "0";

                    //峰林进出压力 
                    int fpressinid = 2152;
                    jkdata.FPressIN = new KeyValuePair<string, bool>(datajk.AnalogValues.ContainsKey(fpressinid.ToString()) ? datajk.AnalogValues[fpressinid.ToString()].ToString() : "--",
                     eventlist_all.Count > 0 ? (eventlist_all.Where(r => r.EventSource == fpressinid && r.EventLevel != 0 && r.Rtuid == rtuID).Count() > 0 ? true : false) : false);

                    int fpressout = 2153;
                    jkdata.FPressOut = new KeyValuePair<string, bool>(datajk.AnalogValues.ContainsKey(fpressout.ToString()) ? datajk.AnalogValues[fpressout.ToString()].ToString() : "--",
                    eventlist_all.Count > 0 ? (eventlist_all.Where(r => r.EventSource == fpressout && r.EventLevel != 0 && r.Rtuid == rtuID).Count() > 0 ? true : false) : false);
                    jkdata.FPressSet = datajk.AnalogValues.ContainsKey((2151).ToString()) ? datajk.AnalogValues[(2151).ToString()].ToString() : "0";
                    //设定压力
                    var PressureSet = datajk.AnalogValues.ContainsKey((2002 + (partion - 1) * 500).ToString()) ? datajk.AnalogValues[(2002 + (partion - 1) * 500).ToString()] : "";
                    jkdata.PressureSet = PressureSet.ToString();
                    //每台泵的电压 暂无

                    //底下两个流量计
                    //中站累计流量
                    jkdata.ztotalflow = datajk.AnalogValues.ContainsKey((2033).ToString()) ? datajk.AnalogValues[(2033).ToString()].ToString() : "0";

                    //中站的瞬时流量
                    jkdata.zinflow = datajk.AnalogValues.ContainsKey((2032).ToString()) ? datajk.AnalogValues[(2032).ToString()].ToString() : "0";
                    //峰林的累计流量
                    jkdata.ftotalflow = datajk.AnalogValues.ContainsKey((2158).ToString()) ? datajk.AnalogValues[(2158).ToString()].ToString() : "0";

                    //峰林的瞬时流量
                    jkdata.finflow = datajk.AnalogValues.ContainsKey((2157).ToString()) ? datajk.AnalogValues[(2157).ToString()].ToString() : "0";
                    //电压2073 - 2076和 2160 - 2162
                    List<string> voltageList = new List<string>();
                    int voltageid = 2073;
                    string voltagevalue = "0";
                    for (var i = 0; i < 4; i++)
                    {
                        voltagevalue = datajk.AnalogValues.ContainsKey((voltageid + i).ToString()) ? datajk.AnalogValues[(voltageid + i).ToString()].ToString() : "0";
                        voltageList.Add(voltagevalue);
                    }

                    int voltageid1 = 2160;
                    for (var i = 0; i < 3; i++)
                    {
                        voltagevalue = datajk.AnalogValues.ContainsKey((voltageid1 + i).ToString()) ? datajk.AnalogValues[(voltageid1 + i).ToString()].ToString() : "0";
                        voltageList.Add(voltagevalue);
                    }


                    jkdata.pumpState = pumpState;
                    jkdata.pumpdatas = pumpdataList;
                    jkdata.ValveFault = valveFaultList;
                    jkdata.ValveValue = valveValueList;
                    jkdata.voltage = voltageList;
                }
                else
                {

                    jkdata.datetime = "--";//更新时间
                    jkdata.RtuID = (int)rtuID;
                    jkdata.State = state;
                    jkdata.PressIN = new KeyValuePair<string, bool>("--", false);
                    jkdata.PressOut = new KeyValuePair<string, bool>("--", false);
                    jkdata.pumpState = new List<string>();
                    jkdata.pumpdatas = new List<PumpData>();
                    jkdata.totalflow = "--";
                    jkdata.totalpower = "--";
                    jkdata.inflow = "--";
                    jkdata.Temperature = "--";
                    for (var i = 0; i < PumpNum; i++)
                    {
                        PumpData p = new PumpData();
                        p.eletric = new KeyValuePair<string, bool>("--", false);
                        p.frequency = new KeyValuePair<string, bool>("--", false);
                        p.runtime = new KeyValuePair<string, bool>("--", false);
                        jkdata.pumpdatas.Add(p);
                        jkdata.pumpState.Add("停止");
                    }
                }
                #endregion
            }
            else
            {
                montorGuiJK jkdata1 = new montorGuiJK();
                jkdata1.datetime = "--";//更新时间
                jkdata1.RtuID = 0;
                jkdata1.State = "离线";
                jkdata1.PressIN = new KeyValuePair<string, bool>("--", false);
                jkdata1.PressOut = new KeyValuePair<string, bool>("--", false);
                jkdata1.pumpState = new List<string>();
                jkdata1.pumpdatas = new List<PumpData>();
                jkdata1.ValveFault = new List<string>();
                jkdata1.totalflow = "--";
                jkdata1.totalpower = "--";
                jkdata1.inflow = "--";
                for (var i = 0; i < 5; i++)
                {
                    PumpData p = new PumpData();
                    p.eletric = new KeyValuePair<string, bool>("--", false);
                    p.frequency = new KeyValuePair<string, bool>("--", false);
                    p.runtime = new KeyValuePair<string, bool>("--", false);
                    jkdata1.pumpdatas.Add(p);
                    jkdata1.pumpState.Add("停止");
                }
                for (var i = 0; i < 7; i++)
                {
                    string state = "待确定";
                    jkdata1.ValveFault.Add(state);
                }
                for (var i = 0; i < 7; i++)
                {
                    string state = "0";
                    jkdata1.voltage.Add(state);
                }
                return Json(new
                {

                    jkdata = jkdata1

                });
            }
            return Json(new
            {

                jkdata

            });
        }
        #endregion
    }
}