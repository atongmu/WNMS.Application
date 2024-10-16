using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using WNMS.Application.Utility.Filters;
using WNMS.IService;
using WNMS.Model.DataModels;
namespace WNMS.Application.Areas.Sws.Controllers
{
    [Area("Sws")]
    [TypeFilter(typeof(IgonreActionFilter))]
    public class Sws_Consump_itemController : Controller
    {
        private ISysUserService _userService = null;
        private ISws_ConsumpSettingService _ConsumpSettingService = null;
       
        public Sws_Consump_itemController(ISysUserService sysUserService,
            ISws_ConsumpSettingService sws_ConsumpSettingService) {
            _userService = sysUserService;
            _ConsumpSettingService = sws_ConsumpSettingService;
        }
        public IActionResult Index()
        {
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = _userService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
            var selectDevices = "";
            var deviceid_pre = "";//单吨能耗id
            var deviceid_ds = "";//吨水能耗id
            var deviceid_st = "";//泵房能耗id
            var datalist = _ConsumpSettingService.Query<SwsConsumpSetting>(r => r.UserId == user.UserId);
            if (datalist.Count() > 0)
            {
                var device1 = datalist.Where(r => r.Type == 1).ToList();
                var device2 = datalist.Where(r => r.Type == 2).ToList();
                var device3 = datalist.Where(r => r.Type == 3).ToList();
                if (device1.Count == 0 || device2.Count == 0 || device3.Count == 0)
                {
                    selectDevices = QueryDeviceID_init(user.UserId, user.IsAdmin);
                }
                if (device1.Count > 0)
                {
                    deviceid_pre = string.Join(",", device1.Select(r => r.DeviceId));
                }
                else
                {
                    deviceid_pre = selectDevices;
                }
                if (device2.Count > 0)
                {
                    deviceid_ds = string.Join(",", device2.Select(r => r.DeviceId));
                }
                else
                {
                    deviceid_ds = selectDevices;
                }
                if (device3.Count > 0)
                {
                    deviceid_st = string.Join(",", device3.Select(r => r.DeviceId));
                }
                else
                {
                    deviceid_st = selectDevices;
                }
            }
            else
            {
                selectDevices = QueryDeviceID_init(user.UserId, user.IsAdmin);
                deviceid_pre = selectDevices;
                deviceid_ds = selectDevices;
                deviceid_st = selectDevices;
            }
            ViewBag.deviceid_pre = deviceid_pre;
            ViewBag.deviceid_ds = deviceid_ds;
            ViewBag.deviceid_st = deviceid_st;
            ViewBag.beginDate = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd");
            ViewBag.endDate= DateTime.Now.Date.ToString("yyyy-MM-dd");

            return View();
        }
        public string QueryDeviceID_init(int userid, bool isadmin)
        {
            string deviceids = "";
            var deviceidList = _ConsumpSettingService.GetDeviceid_init(userid, isadmin);
            if (deviceidList.Count() > 0)
            {
                deviceids = string.Join(",", deviceidList.Select(r => r.DeviceID));
            }
            return deviceids;
        }
        #region 设备配置
        public IActionResult SelectDevices(short type)//1单吨能耗，2吨水能耗 3泵房能耗
        {
            ViewBag.TreeNodes = new HtmlString(GetDataDeviceTree("", type));
            ViewBag.type = type;
            return View();
        }
        public string GetDataDeviceTree(string searchtxt, short type)
        {
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = _userService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
            var datalist = _ConsumpSettingService.GetDeviceTree(user.IsAdmin, user.UserId, searchtxt, type);
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
                IEnumerable<dynamic> deviceNode=null;
                if (type == 2)
                {
                    deviceNode = datalist.Select(r => new
                    {
                        id = r.DeviceID,
                        pId = r.StationID + "s",
                        name = r.DeviceName+"("+r.ManufacturerName + ")",
                        nocheck = false,
                        @checked = r.ischeck == 1 ? true : false
                    }).OrderBy(r => r.name);
                }
                else
                {
                     deviceNode = datalist.Select(r => new
                    {
                        id = r.DeviceID,
                        pId = r.StationID + "s",
                        name = r.DeviceName,
                        nocheck = false,
                        @checked = r.ischeck == 1 ? true : false
                    }).OrderBy(r => r.name);
                }
                var treeNode = stationNode.Union(deviceNode);
                result = Newtonsoft.Json.JsonConvert.SerializeObject(treeNode);
            }
            return result;
        }
        public IActionResult SearchTree(string searchtxt, short type)
        {
            return Content(GetDataDeviceTree(searchtxt, type));
        }
        #endregion
        //设备更新
        public IActionResult SetDevice_search(string datas, short type, DateTime begindate, DateTime endate)
        {
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = _userService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
            List<SwsConsumpSetting> setting = new List<SwsConsumpSetting>() { };
            string deviceIDs = datas;
            var arrayData = datas.Split(",");
            string str = "";
            foreach (var item in arrayData)
            {
                SwsConsumpSetting s = new SwsConsumpSetting();
                var temp_data = item.Split(",");
                s.DeviceId = long.Parse(item);
                //deviceIDs += s.DeviceId + ",";

                s.Type = type;
                s.UserId = userID;
                setting.Add(s);
            }
            try
            {
                _ConsumpSettingService.DeviceSetting(userID, type, setting);

                str = "ok";

            }
            catch (Exception e)
            {
                str = "no";
            }
           
           

            return Content(str);
        }
        //查询已选设备
        public IActionResult SelectedDevice(string deviceIDs)
        {
            ////获取已选择设备  代写
            var selectDevices = _ConsumpSettingService.QueryAreaDevice(deviceIDs).ToList();
            List<selectDevice> selectdatas = new List<selectDevice>() { };
            var areas = selectDevices.GroupBy(r => new { r.StationID, r.StationName });
            foreach (var item in areas)
            {
                selectDevice ss = new selectDevice();
                ss.StationID = item.Key.StationID;
                ss.StationName = item.Key.StationName;
                ss.DataList = selectDevices.Where(r => r.StationID == item.Key.StationID).Select(r => new DeviceData
                {
                    DeviceID = r.DeviceID,
                    DeviceName = r.DeviceName
                }).ToList();
                selectdatas.Add(ss);
            }
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(selectdatas);
            return Content(json);
        }
        public class selectDevice{
            public int StationID { get; set; }
            public string StationName { get; set; }
            public List<DeviceData> DataList { get; set; }
        }
        public class DeviceData { 
            public long DeviceID { get; set; }
            public string DeviceName { get; set; }
        }
        #region 单吨能耗
        public IActionResult GetDeviceNH_pre(string deviceIDs,DateTime begindate,DateTime enddate)
        {

            string tablename = "DHourQuartZ01";
            enddate = enddate.AddDays(1);
            var timediff = (enddate - begindate).TotalDays;
            if (timediff > 3 && timediff < 32)
            {
                tablename = "DDayQuartZ01";
            }
            else if (timediff >= 32)
            {
                tablename = "DMonthQuartZ01";
            }

            var list = _ConsumpSettingService.QueryNH_pre(deviceIDs, tablename, begindate, enddate).ToList();
            var axias = new List<string>() { };
            var dataList = new List<deviceNH>() { };
            if (list.Count > 0)
            {

                var datelist = list.GroupBy(r => r.UpdateTime).OrderBy(r => r.Key);
                foreach (var item in datelist)
                {

                    axias.Add(item.Key.ToString("yyyy-MM-dd HH:mm"));
                }
                var deviceArray = list.GroupBy(r => new { r.DeviceID, r.DeviceName });
                foreach (var item in deviceArray)
                {
                    deviceNH nhpre = new deviceNH();
                    nhpre.deviceName = item.Key.DeviceName;
                    nhpre.datas = new List<string>() { };
                    var dataOfDevice = list.Where(r => r.DeviceID == item.Key.DeviceID);
                    foreach (var it in datelist)
                    {
                        var temp = dataOfDevice.Where(r => r.UpdateTime == it.Key).FirstOrDefault();
                        if (temp != null)
                        {
                            if (temp.FlowCon == 0)
                            {
                                nhpre.datas.Add("0");
                            }
                            else
                            {
                                var tt = Math.Round((double)temp.EnergyCon / (double)temp.FlowCon, 2);
                                nhpre.datas.Add(tt.ToString());
                            }
                        }
                        else
                        {
                            nhpre.datas.Add("");
                        }
                    }
                    dataList.Add(nhpre);
                }
            }
            return Json(new
            {
                tablename,
                axias,
                dataList
            });

        }

       
        public class deviceNH { 
             public string deviceName { get; set; }
             public List<String> datas { get; set; }
        }
        #endregion
        #region 吨水能耗
        public IActionResult GetManufacterNH_ds(string deviceIDs, DateTime begindate, DateTime enddate)
        {
            var datalist = _ConsumpSettingService.QueryNH_manufacter(deviceIDs, begindate,enddate).ToList();
            var names = new List<string>() { };
            var datas = new List<string>() { };
            if (datalist.Count > 0)
            {
                datalist = datalist.OrderBy(r => r.nh).ToList();
                
                foreach (var item in datalist)
                {
                    names.Add(item.ManufacturerName);
                    datas.Add(Math.Round(item.nh,2).ToString());
                }
            }
            return Json(new {
                names,
                datas
            });
        }

        #endregion
        #region 泵房能耗总量
        public IActionResult GetStationNH(string deviceIDs, DateTime begindate, DateTime enddate)
        {
            var datalist = _ConsumpSettingService.QueryNH_station(deviceIDs, begindate, enddate).ToList();
            var names = new List<string>() { };
            var datas = new List<string>() { };
            if (datalist.Count> 0)
            {
                foreach (var item in datalist)
                {
                    names.Add(item.StationName);
                    datas.Add(Math.Round(item.nh, 2).ToString());
                }
            }
            return Json(new {
                names,
                datas
            });
        }
        #endregion
        //厂商设备类型能耗排名
        public IActionResult MaDeviceTypeRate(string begindate,string enddate)
        {
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = _userService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
            var datalist = _ConsumpSettingService.GetMaDeviceRate(user.UserId, user.IsAdmin, begindate, enddate);
            return Json(new
            {
                datalist
            });
        }
        //千吨水百米扬程能耗
        //public IActionResult GetMilliLiftNH(string begindate, string enddate)
        //{
        //    int userID = int.Parse(User.FindFirstValue("UserID"));
        //    var user = _userService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
        //    var datalist = _ConsumpSettingService.GetMilliLiftNH(user.UserId, user.IsAdmin, begindate, enddate);
        //    return Json(new
        //    {
        //        datalist
        //    });
        //}
        //千吨水百米扬程能耗
        public IActionResult GetMilliLiftNH()
        {
            var data = _ConsumpSettingService.GetMilliLiftNH_base();
            if (data.Count()>0)
            {
                return Content(Newtonsoft.Json.JsonConvert.SerializeObject(data));
            }
            else
            {
                return Content("");
            }
        }
    }
}