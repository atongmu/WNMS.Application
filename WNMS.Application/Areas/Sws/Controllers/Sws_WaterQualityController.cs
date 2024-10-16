using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using WNMS.Application.Utility;
using WNMS.Application.Utility.Filters;
using WNMS.Application.Utility.JsonHelper;
using WNMS.IService;
using WNMS.Model.CustomizedClass;
using WNMS.Model.DataModels;

namespace WNMS.Application.Areas.Sws.Controllers
{
    [Area("Sws")]
    public class Sws_WaterQualityController : Controller
    {
        public readonly ISws_DeviceInfo02Service _sws_DeviceInfo02Service = null;
        private readonly ISws_StationService _sws_StationService;
        private readonly ISws_DataInfoService _sws_DataInfoService;
        private readonly IDDayQuartZ02Service ddayquartz02Service;
        private readonly ISysUserService _sysUserService;
        public ISysUserService _ISys_UserService = null;
        public ISws_UserStationService _sws_UserStationService = null;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public Sws_WaterQualityController(ISws_DeviceInfo02Service sws_DeviceInfo02Service,
            ISws_StationService sws_StationService,
            ISws_DataInfoService sws_DataInfoService,
            IDDayQuartZ02Service sws_DayQuartZ02,
            ISysUserService sysUserService,
            ISysUserService _userservice,
            ISws_UserStationService sws_UserStationService,
                 IWebHostEnvironment webHostEnvironment)
        {
            _sws_DeviceInfo02Service = sws_DeviceInfo02Service;
            _sws_StationService = sws_StationService;
            _sws_DataInfoService = sws_DataInfoService;
            ddayquartz02Service = sws_DayQuartZ02;
            _sysUserService = sysUserService;
            _ISys_UserService = _userservice;
            _sws_UserStationService = sws_UserStationService;
            _webHostEnvironment = webHostEnvironment;
        }

        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult Index()
        {
            var path = _webHostEnvironment.ContentRootPath;
            JsonFileHelper jsonFileHelper = new JsonFileHelper(path + "/Utility/ProjectJson/project.json");
            SystemInfo st = jsonFileHelper.Read<SystemInfo>("SystemInfo");

            ViewBag.sysName = st.SystemName;
            ViewBag.logo = st.Logo;

            return View();
        }

        
        [TypeFilter(typeof(IgonreActionFilter))]
        [HttpPost]
        public IActionResult GetJKQualityData(long deviceID)
        {
            SwsDeviceInfo02 device02 = _sws_DeviceInfo02Service.Query<SwsDeviceInfo02>(r => r.DeviceId == deviceID).FirstOrDefault();
            RtuJKInfo rtudata = new RtuJKInfo();
            if (device02 != null&&device02.Rtuid!=null)
            {
                rtudata = _sws_DeviceInfo02Service.GetJKWaterQuality((int)device02.Rtuid).FirstOrDefault();
            }
            return Json(rtudata);
        }

        [TypeFilter(typeof(IgonreActionFilter))]
        [HttpPost]
        public async Task<IActionResult> GetAllJKData()
        {
            List<SwsDeviceInfo02> device02 = _sws_DeviceInfo02Service.Query<SwsDeviceInfo02>(r =>true).ToList();
            List<RtuJKInfo> rtulist = new List<RtuJKInfo>();

            StringBuilder sb = new StringBuilder();
            if (device02 != null && device02.Count > 0)
            {
                foreach(var item in device02)
                {
                    sb.Append(item.Rtuid + ",");
                }
                sb.Remove(sb.Length - 1, 1);
                rtulist = _sws_DeviceInfo02Service.GetALLJKWaterQuality(sb.ToString()).ToList();
            }

            List<JKWaterData> jklist = new List<JKWaterData>();
            if (rtulist.Count() > 0)
            {
                foreach(var item in rtulist)
                {
                    JKWaterData jk = new JKWaterData();
                    jk.DeviceName = device02.Where(r => r.Rtuid == item.RTUID).FirstOrDefault()?.DeviceName;
                    jk.PhAver = item.AnalogValues.ContainsKey("2032") ? Math.Round(double.Parse(item.AnalogValues["2032"].ToString()), 2).ToString() : "--";
                    jk.ClAver = item.AnalogValues.ContainsKey("2033") ? Math.Round(double.Parse(item.AnalogValues["2033"].ToString()), 2).ToString() : "--";
                    jk.TurbidityAver = item.AnalogValues.ContainsKey("2034") ? Math.Round(double.Parse(item.AnalogValues["2034"].ToString()), 2).ToString() : "--";
                    jk.OrpAver = item.AnalogValues.ContainsKey("2035") ? Math.Round(double.Parse(item.AnalogValues["2035"].ToString()), 2).ToString() : "--";
                    jk.SalinityAver = item.AnalogValues.ContainsKey("2036") ? Math.Round(double.Parse(item.AnalogValues["2036"].ToString()), 2).ToString() : "--";
                    jk.OxygenAver = item.AnalogValues.ContainsKey("2037") ? Math.Round(double.Parse(item.AnalogValues["2037"].ToString()), 2).ToString() : "--";
                    //jk.ClAver = item.AnalogValues.ContainsKey("2038") ? Math.Round(double.Parse(item.AnalogValues["2038"].ToString()), 2).ToString() : "--";
                    jk.ConductivityAver = item.AnalogValues.ContainsKey("2023") ? Math.Round(double.Parse(item.AnalogValues["2023"].ToString()), 2).ToString() : "--";
                    jklist.Add(jk);
                }
            }

            PartialView("_DataTable", jklist);
            string dataTable = await ViewToString.RenderPartialViewToString(this, "_DataTable");
            return Json(new
            {
                dataTable = dataTable
            });

        }

        [TypeFilter(typeof(IgonreActionFilter))]
        //查询曲线数据
        public IActionResult GetChartData(long deviceID)
        {
            DateTime endDate = DateTime.Now;
            DateTime beginDate = DateTime.Now.AddDays(-8);
            List<DdayQuartZ02> daylist = ddayquartz02Service.Query<DdayQuartZ02>(r => r.DeviceId == deviceID&&r.UpdateTime>beginDate&&r.UpdateTime<endDate).OrderBy(r=>r.UpdateTime).ToList();
            List<decimal?> oxdata = daylist.Select(r => r.OxygenAver).ToList();  //溶解氧
            List<decimal?> orpdata = daylist.Select(r => r.OrpAver).ToList();
            List<decimal?> cudata = daylist.Select(r => r.ConductivityAver).ToList();
            List<decimal?> sadata = daylist.Select(r => r.SalinityAver).ToList();
            List<decimal?> harddata = daylist.Select(r => r.HardnessAver).ToList();
            List<string> xAxis = daylist.Select(r => r.UpdateTime.ToString("MM-dd")).ToList();
            return Json(new
            {
                oxdata,
                orpdata,
                cudata,
                sadata,
                harddata,
                xAxis
            });
        }

        [TypeFilter(typeof(IgonreActionFilter))]
        //查询水质对比曲线
        public IActionResult GetWaterChart()
        {
            List<WaterQualityDatas> list = _sws_DeviceInfo02Service.QueryWaterQuality().ToList();
            List<string> xAxis = new List<string>();
            List<double> ph = new List<double>();
            List<double> cl = new List<double>();
            List<double> tr = new List<double>();
            if (list != null && list.Count() > 0)
            {
                foreach(var item in list)
                {
                    xAxis.Add(item.DeviceName);
                    ph.Add(item.PhAver == null ? 0.0 : Math.Round((double)item.PhAver,2));
                    cl.Add(item.ClAver == null ? 0.0 : Math.Round((double)item.ClAver,2));
                    tr.Add(item.TurbidityAver == null ? 0.0 : Math.Round((double)item.TurbidityAver,2));
                }
            }
            return Json(new
            {
                xAxis,
                ph,
                cl,
                tr
            }) ;
        }
    }
    public class JKWaterData
    {
        public string DeviceName { get; set; }
        public string PhAver { get; set; }
        public string ClAver { get; set; }
        public string TurbidityAver { get; set; }
        public string OrpAver { get; set; }
        public string SalinityAver { get; set; }
        public string OxygenAver { get; set; }
        public string ConductivityAver { get; set; }
        public string HardnessAver { get; set; }
    }
}