using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
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
    public class Sws_CandlestickChartController : Controller
    {
        #region 属性 构造函数
        private ISws_StationService stationService = null;
        private ISws_DeviceInfo01Service device01Service = null;
        private ISws_RTUInfoService rtuService = null;
        private ISws_DataInfoService dataInfoService = null;

        public Sws_CandlestickChartController(ISws_DeviceInfo01Service sws_DeviceInfo01Service, 
            ISws_RTUInfoService sws_RTUInfoService, ISws_StationService sws_StationService, ISws_DataInfoService sws_DataInfoservice)
        {
            device01Service = sws_DeviceInfo01Service;
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

            ViewBag.BeginTime = DateTime.Now.AddDays(-20).ToString("yyyy-MM-dd");
            ViewBag.EndTime = DateTime.Now.ToString("yyyy-MM-dd");
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
                    isDevice = true
                }).OrderBy(r => r.name).ToList();
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
                    isDevice = true
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
                        sws_DataInfo = dataInfoService.Query<SwsDataInfo>(r => dataIDlist.Contains(r.DataId) && r.DeviceType == 1 && !r.Cnname.Contains("风机")&&r.IsCumulation==false).ToList();
                        if (sws_DataInfo.Count > 0)
                        {
                            List<TreeAction> list = sws_DataInfo.Select(r => new TreeAction
                            {
                                id = r.DataId,
                                pId = item.DeviceId,
                                name = r.Cnname.Remove(0, r.Cnname.IndexOf("区") + 1),
                                isDevice=false,
                                icon = "",
                                value=r.Unit
                            }).ToList();
                            stationTree = stationTree.Concat<TreeAction>(list).ToList();
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

        //曲线数据获取
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult GetCandlestickData(string beginDate, string endDate,int dataId, string deviceID)
        {
            //定义变量
            List<string> timelist = new List<string>();
            List<string> energylist = new List<string>();
            List<List<string>> seriesList = new List<List<string>>();

            //时间处理
            DateTime beginTime = DateTime.Now.AddDays(-20).Date;   //如果时间为空  默认查七天内数据
            DateTime endTime = DateTime.Now.AddDays(1).Date;
            if (!string.IsNullOrEmpty(beginDate) && !string.IsNullOrEmpty(endDate))     //如果时间不为空，查时间段内数据
            {
                beginTime = Convert.ToDateTime(beginDate);
                endTime = Convert.ToDateTime(endDate);
            }

            //设备id处理
            long deviceId = 0;
            if (!string.IsNullOrEmpty(deviceID))
            {
                deviceId = Convert.ToInt64(deviceID);
            }
            SwsDeviceInfo01 deviceinfo = this.device01Service.Query<SwsDeviceInfo01>(r => r.DeviceId == deviceId).FirstOrDefault();

            //数据查询及处理
            if (deviceinfo != null)
            {
                var deviceList = device01Service.GetDeviceJKData(beginTime,endTime,dataId,deviceinfo.Rtuid.ToString()).ToList();
                if (deviceList.Count > 0)
                {
                    foreach(var item in deviceList)
                    {
                        timelist.Add(item.datetime);
                        List<string> st = new List<string>();
                        st.Add((item.firstdata??0.0).ToString());
                        st.Add((item.lastdata??0.0).ToString());
                        st.Add((item.mindata??0.0).ToString());
                        st.Add((item.maxdata??0.0).ToString());
                        seriesList.Add(st);
                    }
                }
            }

            return Json(new
            {
                timelist = timelist,
                seriesList = seriesList
            });
        }
    }
}
