using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using WNMS.Application.Utility.Filters;
using WNMS.IService;
using WNMS.Model.CustomizedClass;
using WNMS.Model.DataModels;

namespace WNMS.Application.Areas.Sws.Controllers
{
    [Area("Sws")]
    [TypeFilter(typeof(IgonreActionFilter))]
    public class Sws_DataSameRadioController : Controller
    {
        private ISws_StationService stationService = null;
        private ISws_DeviceInfo01Service device01Service = null;
        private ISws_DeviceInfo02Service device02Service = null;
        private ISws_RTUInfoService rtuService = null;
        private ISws_DataInfoService dataInfoService = null;
        public Sws_DataSameRadioController(ISws_DeviceInfo01Service sws_DeviceInfo01Service, ISws_DeviceInfo02Service sws_DeviceInfo02Service,
            ISws_RTUInfoService sws_RTUInfoService, ISws_StationService sws_StationService, ISws_DataInfoService sws_DataInfoservice) 
        {
            device01Service = sws_DeviceInfo01Service;
            device02Service = sws_DeviceInfo02Service;
            rtuService = sws_RTUInfoService;
            stationService = sws_StationService;
            dataInfoService = sws_DataInfoservice;

        }
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
            var aa = DateTime.Now;
            ViewBag.Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            ViewBag.Date= DateTime.Now.ToString("yyyy-MM");
            return View();
        }
        //获取数据监测的树
        public string GetStationTree(string stationName)
        {
            int userID = int.Parse(User.FindFirstValue("UserID"));
            List<TreeAction> stationTree = new List<TreeAction>();

            #region 获取泵房树
            List<int> stationIds = new List<int>();
            List<StationData> stationInfo = this.stationService.GetStationInfo(userID, stationName).OrderBy(r => r.StationName).ToList();
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
                }).OrderBy(r => r.name).ToList();
            }

            #endregion

            #region 获取设备树
            List<TreeAction> device01Tree = new List<TreeAction>(); //设备类型1 树
            List<DeviceInfo01Info> device01 = this.device01Service.LoadInfoList(null, 10000, 1, "DeviceName", true).DataList.Where(r => stationIds.Contains(r.StationId)).ToList();
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
                stationTree = stationTree.Concat<TreeAction>(device01Tree).Distinct(new TreeAreaCompare()).OrderBy(r=>r.name).ToList();
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
    }
}