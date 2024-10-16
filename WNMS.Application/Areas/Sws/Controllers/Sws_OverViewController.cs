using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WNMS.Application.Utility.Filters;
using WNMS.IService;
using WNMS.Model.DataModels;

namespace WNMS.Application.Areas.Sws.Controllers
{
    [Area("Sws")]
    public class Sws_OverViewController : Controller
    {
        private readonly ISws_StationService _sws_StationService;
        private readonly ISws_DeviceInfo01Service _sws_DeviceInfo01Service;
        private readonly ISys_DataItemDetailService _sys_DataItemDetailServices;
        private readonly ISws_DeviceInfo02Service _sws_DeviceInfo02Service;
        private ISysUserService _UserService = null;
        private readonly ISws_UserStationService _sws_UserStationService;
        public Sws_OverViewController(ISws_StationService sws_StationService,
            ISws_DeviceInfo01Service sws_DeviceInfo01Service,
            ISys_DataItemDetailService sys_DataItemDetailServices,
            ISysUserService sysUserService,
            ISws_UserStationService sws_UserStationService,
            ISws_DeviceInfo02Service sws_DeviceInfo02Service)
        {
            _sws_StationService = sws_StationService;
            _sws_DeviceInfo01Service = sws_DeviceInfo01Service;
            _sys_DataItemDetailServices = sys_DataItemDetailServices;
            _sws_DeviceInfo02Service = sws_DeviceInfo02Service;
            _UserService = sysUserService;
            _sws_UserStationService = sws_UserStationService;
        }
        public IActionResult Index()
        {
            return View();
        }
        //泵房个数 机组个数 门禁视频安装率 水箱情况 水质情况
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult LoadCount()
        {
            //获取泵房信息
            var stationInfo = _sws_StationService.Query<SwsStation>(r => true).ToList();
            //用户
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = _UserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
            if (user.IsAdmin != true)
            {
                var statioids = _sws_UserStationService.Query<SwsUserStation>(r => r.UserId == user.UserId).Select(r => r.StationId).ToList();
                stationInfo = stationInfo.Where(r => statioids.Contains(r.StationId)).ToList();
            }
            //泵房数量
            double stationCount = stationInfo.Count;
            //门禁安装率
            double mjCount = stationInfo.Where(r => r.DoorInsert == true).ToList().Count;
            double mjRate = Math.Round(mjCount / stationCount * 100, 2);
            //视频安装率
            double cameraCount = stationInfo.Where(r => r.CameraMonitor == true).ToList().Count;
            double cameraRate = Math.Round(cameraCount / stationCount * 100, 2);
            //水箱个数
            double waterCount = stationInfo.Sum(r => r.WaterTankNum);
            //水质检测仪个数及安装率
            double disinfectionCount = stationInfo.Where(r => r.WaterQualityMonitor == true).ToList().Count;
            double disinfectionRate = Math.Round(disinfectionCount / stationCount * 100, 2);
            #region 泵房类型统计
            List<int> ids = new List<int> { 7, 10 };
            var allType = _sys_DataItemDetailServices.Query<SysDataItemDetail>(r => ids.Contains(r.FItemId)).ToList();
            //泵房类型 
            int stationtypeid = (int)WNMS.Model.CustomizedClass.Enum.泵房类型;
            var stationType = allType.Where(r => r.FItemId == stationtypeid && r.IsEnable == true).ToList();
            List<Hashtable> stalist = new List<Hashtable>();
            Hashtable stadata = new Hashtable();
            List<string> staNametList = new List<string>();
            foreach (var item in stationType)
            {
                stadata = new Hashtable();
                var stcCount = stationInfo.Where(r => r.StaitonType == byte.Parse(item.ItemValue)).Count();
                stadata.Add("value", stcCount);
                stadata.Add("name", item.ItemName);
                stalist.Add(stadata);
                staNametList.Add(item.ItemName.ToString());
            }
            //泵房厂商 暂无泵房厂商数据
            //int stationtypeid = (int)WNMS.Model.CustomizedClass.Enum.泵房类型;
            //var stationType = allType.Where(r => r.FItemId == stationtypeid).ToList();
            //泵房使用年限
            DateTime begTime = new DateTime();
            DateTime endTime = new DateTime();
            List<int> yearInt = new List<int>();
            //--一年内
            begTime = DateTime.Now.AddYears(-1);
            var oneyearCount = stationInfo.Where(r => r.InstallationDate > begTime).ToList().Count;
            yearInt.Add(oneyearCount);
            //1-2年
            begTime = DateTime.Now.AddYears(-2);
            endTime = DateTime.Now.AddYears(-1);
            var onetwoCount = stationInfo.Where(r => r.InstallationDate > begTime && r.InstallationDate < endTime).ToList().Count;
            yearInt.Add(onetwoCount);
            //2-5年
            begTime = DateTime.Now.AddYears(-5);
            endTime = DateTime.Now.AddYears(-2);
            var twofiveCount = stationInfo.Where(r => r.InstallationDate > begTime && r.InstallationDate < endTime).ToList().Count;
            yearInt.Add(twofiveCount);
            //5-10年
            begTime = DateTime.Now.AddYears(-10);
            endTime = DateTime.Now.AddYears(-5);
            var fivetenCount = stationInfo.Where(r => r.InstallationDate > begTime && r.InstallationDate < endTime).ToList().Count;
            yearInt.Add(fivetenCount);
            //10年以上
            endTime = DateTime.Now.AddYears(-10);
            var tenCount = stationInfo.Where(r => r.InstallationDate < endTime).ToList().Count;
            yearInt.Add(tenCount);
            #endregion
            //泵房质保统计 
            List<int> zbInt = new List<int>();
            //--质保半年  
            var bts = new TimeSpan(182, 0, 0, 0);//开始
            var ets = new TimeSpan(182, 0, 0, 0);//结束
            var halfdata = stationInfo.Where(r => (r.QualityEndDate - DateTime.Now) < bts && r.QualityEndDate > DateTime.Now).ToList().Count;
            zbInt.Add(halfdata);
            //--质保半年到1年
            bts = new TimeSpan(365, 0, 0, 0);
            ets = new TimeSpan(182, 0, 0, 0);
            var onehalfdata = stationInfo.Where(r => (r.QualityEndDate - DateTime.Now) < bts && (r.QualityEndDate - DateTime.Now) > ets).ToList().Count;
            zbInt.Add(onehalfdata);
            //--质保1到2年
            bts = new TimeSpan(730, 0, 0, 0);
            ets = new TimeSpan(365, 0, 0, 0);
            var onetwodata = stationInfo.Where(r => (r.QualityEndDate - DateTime.Now) < bts && (r.QualityEndDate - DateTime.Now) > ets).ToList().Count;
            zbInt.Add(onetwodata);
            //--质保2到3年
            bts = new TimeSpan(1095, 0, 0, 0);
            ets = new TimeSpan(730, 0, 0, 0);
            var twothreedata = stationInfo.Where(r => (r.QualityEndDate - DateTime.Now) < bts && (r.QualityEndDate - DateTime.Now) > ets).ToList().Count;
            zbInt.Add(twothreedata);
            //--质保3年以上 
            ets = new TimeSpan(1095, 0, 0, 0);
            var threedata = stationInfo.Where(r => (r.QualityEndDate - DateTime.Now) > ets).ToList().Count;
            zbInt.Add(threedata);
            var rel = new
            {
                stationCount,
                mjRate,
                cameraRate,
                waterCount,
                disinfectionRate,
                stalist,
                staNametList,
                yearInt,
                zbInt,
                disinfectionCount
            };
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(rel));
        }
        //机组相关数据
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult LoadDeviceChart()
        {
            //用户
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = _UserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
            //获取机组信息 
            var deviceInfo = _sws_DeviceInfo01Service.Query<SwsDeviceInfo01>(r => true).ToList();
            var zdeviceInfo = _sws_DeviceInfo02Service.Query<SwsDeviceInfo02>(r => true).ToList();
            if (user.IsAdmin != true)
            {
                var statioids = _sws_UserStationService.Query<SwsUserStation>(r => r.UserId == user.UserId).Select(r => r.StationId).ToList();
                deviceInfo = deviceInfo.Where(r => statioids.Contains(r.StationId)).ToList();
                zdeviceInfo = zdeviceInfo.Where(r => statioids.Contains(r.StationId)).ToList();
            }
            //机组类型品牌
            List<int> ids = new List<int> { 5, 7, 8 };
            var allType = _sys_DataItemDetailServices.Query<SysDataItemDetail>(r => ids.Contains(r.FItemId)).ToList();
            //机组品牌
            int devicemanid = (int)WNMS.Model.CustomizedClass.Enum.设备品牌;
            var deviceMan = allType.Where(r => r.FItemId == devicemanid).ToList();
            Hashtable devicedata = new Hashtable();
            List<string> deviceNametList = new List<string>();
            List<int> devicecountList = new List<int>();
            foreach (var item in deviceMan)
            {
                devicedata = new Hashtable();
                var stcCount = deviceInfo.Where(r => r.Manufacturer == int.Parse(item.ItemValue)).Count();
                var stczCount = zdeviceInfo.Where(r => r.Manufacturer == int.Parse(item.ItemValue)).Count();
                var all = stcCount + stczCount;
                devicecountList.Add(all);
                deviceNametList.Add(item.ItemName.ToString());
            }
            //机组类型统计 
            int devicetypeid = (int)WNMS.Model.CustomizedClass.Enum.设备类型;
            var deviceType = allType.Where(r => r.FItemId == devicetypeid && r.IsEnable == true).ToList();
            List<Hashtable> devicetypelist = new List<Hashtable>();
            Hashtable devictypedata = new Hashtable();
            List<string> devicetypeNatList = new List<string>();

            foreach (var item in deviceType)
            {
                devictypedata = new Hashtable();
                var stcCount = deviceInfo.Where(r => r.DeviceType == int.Parse(item.ItemValue)).Count();
                int zcount = 0;
                if (item.ItemValue == "5")
                {
                    zcount = zdeviceInfo.Count();
                }
                var allcount = stcCount + zcount;
                devictypedata.Add("value", allcount);
                devictypedata.Add("name", item.ItemName);
                devicetypelist.Add(devictypedata);
                devicetypeNatList.Add(item.ItemName.ToString());
            }
            //机组分区统计 
            int partitionid = (int)WNMS.Model.CustomizedClass.Enum.设备分区;
            var partition = allType.Where(r => r.FItemId == partitionid).ToList();
            List<Hashtable> partitionlist = new List<Hashtable>();
            Hashtable partitiondata = new Hashtable();
            List<string> partitionNatList = new List<string>();
            foreach (var item in partition)
            {
                partitiondata = new Hashtable();
                var stcCount = deviceInfo.Where(r => r.Partition == int.Parse(item.ItemValue)).Count();
                partitiondata.Add("value", stcCount);
                partitiondata.Add("name", item.ItemName);
                partitionlist.Add(partitiondata);
                partitionNatList.Add(item.ItemName.ToString());
            }
            var rel = new
            {
                deviceCount = deviceInfo.Count + zdeviceInfo.Count,
                devicecountList,
                deviceNametList,
                devicetypeNatList,
                devicetypelist,
                partitionlist,
                partitionNatList
            };
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(rel));
        }
    }
}