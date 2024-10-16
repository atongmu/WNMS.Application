using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using WNMS.Application.Utility;
using WNMS.Application.Utility.Filters;
using WNMS.IService;
using WNMS.Model.CustomizedClass;
using WNMS.Model.DataModels;

namespace WNMS.Application.Areas.Sws.Controllers
{
    [Area("Sws")]
    public class Sws_RunEvaluationController : Controller
    {
        private readonly ISws_StationService _sws_StationService;
        private readonly ISws_DeviceInfo01Service _sws_DeviceInfo01Service;
        private readonly ISysUserService _sysUserService;
        private readonly ISws_UserStationService _sws_UserStationService;
        public Sws_RunEvaluationController(
            ISws_StationService sws_StationService,
            ISws_DeviceInfo01Service sws_DeviceInfo01Service, ISws_UserStationService sws_UserStationService,
            ISysUserService sysUserService
            )
        {
            _sws_StationService = sws_StationService;
            _sws_DeviceInfo01Service = sws_DeviceInfo01Service;
            _sysUserService = sysUserService; _sws_UserStationService = sws_UserStationService;
        }
        public IActionResult Index()
        {
            ViewBag.datemin = string.Format("{0:yyyy-MM-dd}", DateTime.Now);
            ViewBag.datemax = string.Format("{0:yyyy-MM-dd}", DateTime.Now);
            return View();
        }
        //查询table
        [TypeFilter(typeof(IgonreActionFilter))]
        public async Task<IActionResult> LoadRunEvaluationData(int? pageindex, int? pagesize, string beginTime, string endTime, string sortName = "EnergyCon", string order = "desc", string type = "1")
        {
            if (type != "4" && !string.IsNullOrEmpty(type))
            {
                GetBeginDate(type, beginTime, ref beginTime, ref endTime);
            }
            else
            {

            }

            int Totalcount = 0;
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var sysUser = _sysUserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
            int size = pagesize ?? 15;
            int index = pageindex ?? 1;
            var dataList = _sws_DeviceInfo01Service.QueryStationTable(sysUser.IsAdmin, index, size, beginTime, endTime, type, sortName, order, sysUser.UserId.ToString(), ref Totalcount);
            PartialView("_RunEvaluationTable", dataList);
            string dataTable = await ViewToString.RenderPartialViewToString(this, "_RunEvaluationTable");
            return Json(new
            {
                total = Totalcount,
                pageIndex = index,
                pageSize = size,
                order = order,
                sortName = sortName ?? "",
                dataTable = dataTable
            });
        }
        //查询设备table
        [TypeFilter(typeof(IgonreActionFilter))]
        public async Task<IActionResult> LoadDeviceRunEvaluationData(int? pageindex, int? pagesize, string beginTime, string endTime, string sortName = "EnergyCon", string order = "desc", string type = "1")
        {
            string TableName = "";
            if (type != "4" && !string.IsNullOrEmpty(type))
            {
                GetBeginDate(type, beginTime, ref beginTime, ref endTime);
            }
            else
            {

            }
            if (type == "3")
            {
                TableName = "DMonthQuartZ01";
            }
            else if (type == "4")
            {
                TableName = "DHourQuartZ01";
            }
            else if (type == "1")
            {
                TableName = "DHourQuartZ01";
            }
            else
            {
                TableName = "DDayQuartZ01";
            }

            int Totalcount = 0;
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var sysUser = _sysUserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
            int size = pagesize ?? 15;
            int index = pageindex ?? 1;
            var dataList = _sws_DeviceInfo01Service.QueryDeviceStationTable(sysUser.IsAdmin, index, size, beginTime, endTime, TableName, sortName, order, sysUser.UserId.ToString(), ref Totalcount);
            PartialView("_RunEvaluationTable", dataList);
            string dataTable = await ViewToString.RenderPartialViewToString(this, "_RunEvaluationTable");
            return Json(new
            {
                total = Totalcount,
                pageIndex = index,
                pageSize = size,
                order = order,
                sortName = sortName ?? "",
                dataTable = dataTable
            });
        }
        //查询同比排名table
        [TypeFilter(typeof(IgonreActionFilter))]
        public async Task<IActionResult> LoadDeviceRunPreData(int? pageindex, int? pagesize, string beginTime, string endTime, string sortName = "EnergyCon", string order = "desc", string type = "1")
        {
            string TableName = "";
            string tBeginTime = ""; string tEndTime = "";
            if (type != "4" && !string.IsNullOrEmpty(type))
            {
                thGetBeginDate(type, beginTime, ref beginTime, ref endTime, ref tBeginTime, ref tEndTime);
            }
            else
            {
                tBeginTime = DateTime.Parse(beginTime).AddYears(-1).ToString("yyyy-MM-dd");
                tEndTime = DateTime.Parse(endTime).AddYears(-1).ToString("yyyy-MM-dd");
            }
            if (type == "3")
            {
                TableName = "DMonthQuartZ01";
            }
            else if (type == "4")
            {
                TableName = "DHourQuartZ01";
            }
            else
            {
                TableName = "DDayQuartZ01";
            }
            int Totalcount = 0;
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var sysUser = _sysUserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
            int size = pagesize ?? 10;
            int index = pageindex ?? 1;
            var dataList = _sws_DeviceInfo01Service.QueryDevicePreTable(sysUser.IsAdmin, 1, 5, beginTime, endTime, TableName, tBeginTime, tEndTime, sysUser.UserId.ToString(), ref Totalcount);
            PartialView("_RunPreTable", dataList);
            string dataTable = await ViewToString.RenderPartialViewToString(this, "_RunPreTable");
            return Json(new
            {
                total = Totalcount,
                pageIndex = index,
                pageSize = size,
                order = order,
                sortName = sortName ?? "",
                dataTable = dataTable
            });
        }

        //查询同比排名table
        [TypeFilter(typeof(IgonreActionFilter))]
        public async Task<IActionResult> LoadRunPreData(int? pageindex, int? pagesize, string beginTime, string endTime, string sortName = "EnergyCon", string order = "desc", string type = "1")
        {
            string tBeginTime = ""; string tEndTime = "";
            if (type != "4" && !string.IsNullOrEmpty(type))
            {
                thGetBeginDate(type, beginTime, ref beginTime, ref endTime, ref tBeginTime, ref tEndTime);
            }
            else
            {
                tBeginTime = DateTime.Parse(beginTime).AddYears(-1).ToString("yyyy-MM-dd");
                tEndTime = DateTime.Parse(endTime).AddYears(-1).ToString("yyyy-MM-dd");
            }

            int Totalcount = 0;
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var sysUser = _sysUserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
            int size = pagesize ?? 10;
            int index = pageindex ?? 1;
            var dataList = _sws_DeviceInfo01Service.QueryPreTable(sysUser.IsAdmin, 1, 5, beginTime, endTime, type, tBeginTime, tEndTime, sysUser.UserId, ref Totalcount);
            PartialView("_RunPreTable", dataList);
            string dataTable = await ViewToString.RenderPartialViewToString(this, "_RunPreTable");
            return Json(new
            {
                total = Totalcount,
                pageIndex = index,
                pageSize = size,
                order = order,
                sortName = sortName ?? "",
                dataTable = dataTable
            });
        }
        //查询同比排名
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult LoadData()
        {
            //查询数据集合
            List<StationEvaluation> stationEvaluations = new List<StationEvaluation>();
            //查询所有泵房信息
            var stationInfo = _sws_StationService.Query<SwsStation>(r => true).ToList();
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var sysUser = _sysUserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
            if (sysUser.IsAdmin != true)
            {
                var statioids = _sws_UserStationService.Query<SwsUserStation>(r => r.UserId == sysUser.UserId).Select(r => r.StationId).ToList();
                stationInfo = stationInfo.Where(r => statioids.Contains(r.StationId)).ToList();
            }
            foreach (var item in stationInfo)
            {
                StationEvaluation stationEvaluation = new StationEvaluation();
                stationEvaluation.StationId = item.StationId;
                stationEvaluation.StationName = item.StationName;
                //查询泵房下的rtuid
                var deviceList = _sws_DeviceInfo01Service.Query<SwsDeviceInfo01>(r => r.StationId == item.StationId && r.Rtuid != null).ToList();
                foreach (var devitem in deviceList)
                {
                    //获取历史数据信息
                    var list = _sws_DeviceInfo01Service.GetExportHistoryData("2020", devitem.Rtuid.ToString(), "2020-02-03", "2020-02-05").ToList();
                    var partion = Convert.ToInt32(devitem.Partition.ToString());
                    foreach (var dataitem in list)
                    {
                        //累计流量
                        var tfdata1 = dataitem.AnalogValues.ContainsKey((2031 + (partion - 1) * 500).ToString()) ? dataitem.AnalogValues[(2031 + (partion - 1) * 500).ToString()] : null;
                        var tfdata2 = dataitem.AnalogValues.ContainsKey((2033 + (partion - 1) * 500).ToString()) ? dataitem.AnalogValues[(2033 + (partion - 1) * 500).ToString()] : null;
                        var tfdata3 = dataitem.AnalogValues.ContainsKey((2035 + (partion - 1) * 500).ToString()) ? dataitem.AnalogValues[(2035 + (partion - 1) * 500).ToString()] : null;
                        var tfdata4 = dataitem.AnalogValues.ContainsKey((2037 + (partion - 1) * 500).ToString()) ? dataitem.AnalogValues[(2037 + (partion - 1) * 500).ToString()] : null;
                        stationEvaluation.SupplyWater += (Convert.ToDouble(tfdata1) + Convert.ToDouble(tfdata2) + Convert.ToDouble(tfdata3) + Convert.ToDouble(tfdata4));

                        //累计电量
                        var pall = dataitem.AnalogValues.ContainsKey((2085 + (partion - 1) * 500).ToString()) ? dataitem.AnalogValues[(2085 + (partion - 1) * 500).ToString()] : null;
                        var pdata1 = dataitem.AnalogValues.ContainsKey((2023 + (partion - 1) * 500).ToString()) ? dataitem.AnalogValues[(2023 + (partion - 1) * 500).ToString()] : null;
                        var pdata2 = dataitem.AnalogValues.ContainsKey((2024 + (partion - 1) * 500).ToString()) ? dataitem.AnalogValues[(2024 + (partion - 1) * 500).ToString()] : null;
                        var pdata3 = dataitem.AnalogValues.ContainsKey((2025 + (partion - 1) * 500).ToString()) ? dataitem.AnalogValues[(2025 + (partion - 1) * 500).ToString()] : null;
                        var pdata4 = dataitem.AnalogValues.ContainsKey((2026 + (partion - 1) * 500).ToString()) ? dataitem.AnalogValues[(2026 + (partion - 1) * 500).ToString()] : null;
                        var pdata = Convert.ToDouble(pdata1) + Convert.ToDouble(pdata2) + Convert.ToDouble(pdata3) + Convert.ToDouble(pdata4);
                        stationEvaluation.SupplyPower += (pdata == 0.0 ? (Convert.ToDouble(pall)) : pdata);
                    }
                }
                stationEvaluation.TonsWater = stationEvaluation.SupplyPower / stationEvaluation.SupplyWater;
                stationEvaluations.Add(stationEvaluation);
            }

            var aa = stationEvaluations.ToList();
            return Content("");
        }
        //查询设备echart数据
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult LoadDeviceRunEvaluationCharts(string beginTime, string endTime, string type = "1")
        {
            string TableName = "";
            if (type != "4" && !string.IsNullOrEmpty(type))
            {
                //thGetBeginDate(type, beginTime, ref beginTime, ref endTime, ref  tBeginTime, ref  tEndTime, ref  hBeginTime, ref  hEndTime);
                GetBeginDate(type, beginTime, ref beginTime, ref endTime);
            }
            else
            {

            }
            if (type == "3")
            {
                TableName = "DMonthQuartZ01";
            }
            else if (type == "4")
            {
                TableName = "DHourQuartZ01";
            }
            else
            {
                TableName = "DDayQuartZ01";
            }
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var sysUser = _sysUserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
            var dataList = _sws_DeviceInfo01Service.GetDeviceConsumptionSumNew(sysUser.IsAdmin, sysUser.UserId.ToString(), beginTime, endTime, TableName).FirstOrDefault();
            var tonwater = double.Parse(dataList.FlowCon.ToString()) == 0.0 ? 0.0 : Math.Round((double.Parse(dataList.EnergyCon.ToString()) / double.Parse(dataList.FlowCon.ToString())), 2);
            var rel = new
            {
                EnergyCon = Math.Round(dataList.EnergyCon, 2),
                FlowCon = Math.Round(dataList.FlowCon, 2),
                tonwater = tonwater
            };
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(rel));
        }
        //查询echart数据
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult LoadRunEvaluationCharts(string beginTime, string endTime, string type = "1")
        {
            string tBeginTime = "", tEndTime = "", hBeginTime = "", hEndTime = "";
            if (type != "4" && !string.IsNullOrEmpty(type))
            {
                //thGetBeginDate(type, beginTime, ref beginTime, ref endTime, ref  tBeginTime, ref  tEndTime, ref  hBeginTime, ref  hEndTime);
                GetBeginDate(type, beginTime, ref beginTime, ref endTime);
            }
            else
            {

            }
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var sysUser = _sysUserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
            var dataList = _sws_DeviceInfo01Service.GetConsumptionSumNew(sysUser.IsAdmin, sysUser.UserId, beginTime, endTime, type).FirstOrDefault();
            var tonwater = double.Parse(dataList.FlowCon.ToString()) == 0.0 ? 0.0 : Math.Round((double.Parse(dataList.EnergyCon.ToString()) / double.Parse(dataList.FlowCon.ToString())), 2);
            var rel = new
            {
                EnergyCon = Math.Round(dataList.EnergyCon, 2),
                FlowCon = Math.Round(dataList.FlowCon, 2),
                tonwater = tonwater
            };
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(rel));
        }
        //获取查询开始日期
        void GetBeginDate(string executeTime, string Begindate, ref string BeginTime, ref string EndTime)
        {
            DateTime dt = DateTime.Now;
            switch (executeTime)
            {
                case "1":
                    dt = DateTime.Parse(Begindate);

                    ////weekrange(DateTime.Parse( Begindate) ,ref BeginTime, ref EndTime); 
                    //DateTime startWeek = dt.AddDays(1 - Convert.ToInt32(dt.DayOfWeek.ToString("d")));  //本周周一
                    //DateTime endWeek = startWeek.AddDays(6);  //本周周日
                    //BeginTime = startWeek.ToString("yyyy-MM-dd");
                    //EndTime = endWeek.ToString("yyyy-MM-dd");

                    var dayofWeek1 = (int)dt.DayOfWeek == 0 ? 7 : (int)dt.DayOfWeek;
                    BeginTime = string.Format("{0:yyyy-MM-dd}", dt.AddDays(-(dayofWeek1 - 1)));
                    EndTime = string.Format("{0:yyyy-MM-dd}", dt.AddDays(-(dayofWeek1) + 7));

                    break;
                case "2":
                    dt = DateTime.Parse(Begindate + "-01");
                    DateTime startMonth = dt.AddDays(1 - dt.Day);  //本月月初
                    DateTime endMonth = startMonth.AddMonths(1).AddDays(-1);  //本月月末//
                    BeginTime = startMonth.ToString("yyyy-MM-dd");
                    EndTime = endMonth.ToString("yyyy-MM-dd");

                    break;
                case "3":
                    dt = DateTime.Parse(Begindate + "-01" + "-01");
                    DateTime startYear = new DateTime(dt.Year, 1, 1);  //本年年初
                    DateTime endYear = new DateTime(dt.Year, 12, 31);  //本年年末至于昨天、明天、上周、上月、上季度、上年度等等，
                    BeginTime = startYear.ToString("yyyy-MM-dd");
                    EndTime = endYear.ToString("yyyy-MM-dd");

                    break;
                case "下周":
                    var dayofWeek2 = (int)DateTime.Now.DayOfWeek == 0 ? 7 : (int)DateTime.Now.DayOfWeek;
                    BeginTime = string.Format("{0:yyyy-MM-dd}", DateTime.Now.AddDays(-(dayofWeek2 - 1) + 7));
                    EndTime = string.Format("{0:yyyy-MM-dd}", DateTime.Now.AddDays(-(dayofWeek2 - 1) + 13));
                    break;
                case "上月":
                    BeginTime = string.Format("{0:yyyy-MM}", DateTime.Now.AddMonths(-1)) + "-01";
                    EndTime = string.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(string.Format("{0:yyyy-MM-01}", DateTime.Now)).AddDays(-1));
                    break;
                case "本月":
                    BeginTime = string.Format("{0:yyyy-MM}", DateTime.Now) + "-01";
                    EndTime = string.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(string.Format("{0:yyyy-MM-01}", DateTime.Now.AddMonths(1))).AddDays(-1));
                    break;
                case "下月":
                    BeginTime = string.Format("{0:yyyy-MM}", DateTime.Now.AddMonths(1)) + "-01";
                    EndTime = string.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(string.Format("{0:yyyy-MM-01}", DateTime.Now.AddMonths(2))).AddDays(-1));
                    break;
                case "自定义":

                    break;
                default:
                    break;

            }

        }
        //获取查询开始日期
        void thGetBeginDate(string executeTime, string Begindate, ref string BeginTime, ref string EndTime, ref string tBeginTime, ref string tEndTime)
        {
            DateTime dt = DateTime.Now;
            switch (executeTime)
            {
                case "1":
                    dt = DateTime.Parse(Begindate);
                    //weekrange(DateTime.Parse( Begindate) ,ref BeginTime, ref EndTime); 
                    DateTime startWeek = dt.AddDays(1 - Convert.ToInt32(dt.DayOfWeek.ToString("d")));  //本周周一
                    DateTime endWeek = startWeek.AddDays(6);  //本周周日
                    BeginTime = startWeek.ToString("yyyy-MM-dd");
                    EndTime = endWeek.ToString("yyyy-MM-dd");
                    // 同比日期
                    tBeginTime = startWeek.AddMonths(-1).ToString("yyyy-MM-dd");
                    tEndTime = endWeek.AddMonths(-1).ToString("yyyy-MM-dd");
                    ////环比日期
                    //hBeginTime = startWeek.AddDays(-7).ToString("yyyy-MM-dd");
                    //hBeginTime = endWeek.AddMonths(-7).ToString("yyyy-MM-dd");
                    break;
                case "2":
                    dt = DateTime.Parse(Begindate + "-01");
                    DateTime startMonth = dt.AddDays(1 - dt.Day);  //本月月初
                    DateTime endMonth = startMonth.AddMonths(1).AddDays(-1);  //本月月末//
                    BeginTime = startMonth.ToString("yyyy-MM-dd");
                    EndTime = endMonth.ToString("yyyy-MM-dd");
                    // 同比日期
                    tBeginTime = startMonth.AddYears(-1).ToString("yyyy-MM-dd");
                    tEndTime = endMonth.AddYears(-1).ToString("yyyy-MM-dd");
                    //环比日期
                    //hBeginTime = startMonth.AddMonths(-1).ToString("yyyy-MM-dd");
                    //hEndTime = endMonth.AddMonths(-1).ToString("yyyy-MM-dd");
                    break;
                case "3":
                    dt = DateTime.Parse(Begindate + "-01" + "-01");
                    DateTime startYear = new DateTime(dt.Year, 1, 1);  //本年年初
                    DateTime endYear = new DateTime(dt.Year, 12, 31);  //本年年末至于昨天、明天、上周、上月、上季度、上年度等等，
                    BeginTime = startYear.ToString("yyyy-MM-dd");
                    EndTime = endYear.ToString("yyyy-MM-dd");
                    // 同比日期
                    tBeginTime = startYear.AddYears(-1).ToString("yyyy-MM-dd");
                    tEndTime = endYear.AddYears(-1).ToString("yyyy-MM-dd");
                    //环比日期
                    //hBeginTime = startYear.AddYears(-1).ToString("yyyy-MM-dd");
                    //hEndTime = endYear.AddYears(-1).ToString("yyyy-MM-dd");
                    break;
                case "下周":
                    var dayofWeek2 = (int)DateTime.Now.DayOfWeek == 0 ? 7 : (int)DateTime.Now.DayOfWeek;
                    BeginTime = string.Format("{0:yyyy-MM-dd}", DateTime.Now.AddDays(-(dayofWeek2 - 1) + 7));
                    EndTime = string.Format("{0:yyyy-MM-dd}", DateTime.Now.AddDays(-(dayofWeek2 - 1) + 13));
                    break;
                case "上月":
                    BeginTime = string.Format("{0:yyyy-MM}", DateTime.Now.AddMonths(-1)) + "-01";
                    EndTime = string.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(string.Format("{0:yyyy-MM-01}", DateTime.Now)).AddDays(-1));
                    break;
                case "本月":
                    BeginTime = string.Format("{0:yyyy-MM}", DateTime.Now) + "-01";
                    EndTime = string.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(string.Format("{0:yyyy-MM-01}", DateTime.Now.AddMonths(1))).AddDays(-1));
                    break;
                case "下月":
                    BeginTime = string.Format("{0:yyyy-MM}", DateTime.Now.AddMonths(1)) + "-01";
                    EndTime = string.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(string.Format("{0:yyyy-MM-01}", DateTime.Now.AddMonths(2))).AddDays(-1));
                    break;
                case "自定义":

                    break;
                default:
                    break;

            }

        }
        /// <summary>
        /// 本周起止时间
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private void weekrange(System.DateTime dt, ref string BeginTime, ref string EndTime)
        {
            int weeknow = Convert.ToInt32(dt.DayOfWeek);
            int daydiff = (-1) * weeknow;
            int dayadd = 6 - weeknow;
            BeginTime = dt.AddDays(daydiff).ToString("yyyy-MM-dd");
            EndTime = dt.AddDays(dayadd).ToString("yyyy-MM-dd");
        }
        #region 导出

        [TypeFilter(typeof(IgonreActionFilter))]
        public ActionResult ExprotEvaluationData(int? pageindex, int? pagesize, string beginTime, string endTime, string sortName = "EnergyCon", string order = "desc", string type = "1")
        {
            if (type != "4" && !string.IsNullOrEmpty(type))
            {
                GetBeginDate(type, beginTime, ref beginTime, ref endTime);
            }
            else
            {

            }
            string TableName = "";
            if (type == "3")
            {
                TableName = "DMonthQuartZ01";
            }
            else if (type == "4")
            {
                TableName = "DHourQuartZ01";
            }
            else
            {
                TableName = "DDayQuartZ01";
            }
            int Totalcount = 0;
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var sysUser = _sysUserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
            int size = pagesize ?? 10;
            int index = pageindex ?? 1;
            //var list = _sws_DeviceInfo01Service.QueryStationTable(sysUser.IsAdmin, index, size, beginTime, endTime, type, sortName, order, sysUser.UserId.ToString(), ref Totalcount).ToList();
            var list = _sws_DeviceInfo01Service.QueryDeviceStationTable(sysUser.IsAdmin, index, size, beginTime, endTime, TableName, sortName, order, sysUser.UserId.ToString(), ref Totalcount).ToList();
            //数据导出
            HSSFWorkbook workbook = new HSSFWorkbook();
            ISheet sheet = workbook.CreateSheet();
            #region 内容样式
            IFont font1 = workbook.CreateFont(); //创建一个字体样式对象
            font1.FontName = "Microsoft YaHei"; //和excel里面的字体对应
                                                //font1.Boldweight = short.MaxValue;//字体加粗
            font1.FontHeightInPoints = 12;//字体大小
            ICellStyle style = workbook.CreateCellStyle();//创建样式对象
            style.BorderBottom = BorderStyle.Thin;
            style.BorderLeft = BorderStyle.Thin;
            style.BorderRight = BorderStyle.Thin;
            style.BorderTop = BorderStyle.Thin;
            style.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
            style.VerticalAlignment = VerticalAlignment.Center;
            style.SetFont(font1); //将字体样式赋给样式对象 
            #endregion

            #region 标题样式
            IFont font = workbook.CreateFont(); //创建一个字体样式对象
            font.FontName = "Microsoft YaHei"; //和excel里面的字体对应
            font.Boldweight = (short)FontBoldWeight.Bold;//字体加粗
            font.FontHeightInPoints = 12;//字体大小
            ICellStyle style1 = workbook.CreateCellStyle();//创建样式对象
            style1.BorderBottom = BorderStyle.Thin;
            style1.BorderLeft = BorderStyle.Thin;
            style1.BorderRight = BorderStyle.Thin;
            style1.BorderTop = BorderStyle.Thin;
            style1.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
            style1.VerticalAlignment = VerticalAlignment.Center;
            style1.SetFont(font); //将字体样式赋给样式对象 
            #endregion

            //填充表头
            IRow dataRow = sheet.CreateRow(0);
            dataRow.Height = 20 * 20;
            dataRow.CreateCell(0).SetCellValue("排名");
            dataRow.CreateCell(1).SetCellValue("设备名称");
            dataRow.CreateCell(2).SetCellValue("吨水电耗");
            dataRow.CreateCell(3).SetCellValue("用电量");
            dataRow.CreateCell(4).SetCellValue("供水量");

            for (int i = 0; i < 5; i++)
            {
                sheet.SetColumnWidth(i, 30 * 256);
            }
            for (int s = 0; s < 5; s++)
            {
                sheet.SetColumnWidth(s, 25 * 256);
                dataRow.Cells[s].CellStyle = style1;
            }
            int j = 1;
            foreach (var item in list)
            {
                dataRow = sheet.CreateRow(j);
                dataRow.Height = 100 * 4;
                dataRow.CreateCell(0).SetCellValue(item.rownumber?.ToString());
                dataRow.CreateCell(1).SetCellValue(item.DeviceName?.ToString());
                dataRow.CreateCell(2).SetCellValue(Math.Round(item.tonwater, 2)?.ToString());
                dataRow.CreateCell(3).SetCellValue(item.EnergyCon?.ToString());
                dataRow.CreateCell(4).SetCellValue(item.FlowCon?.ToString());
                j++;
            }

            for (int ro = 1; ro < list.Count + 1; ro++)
            {
                sheet.GetRow(ro).Height = 20 * 20;
                for (int s = 0; s < 5; s++)
                {
                    sheet.GetRow(ro).Cells[s].CellStyle = style;
                }
            }
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            workbook.Write(ms);
            workbook = null;
            ms.Close();
            ms.Dispose();
            return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "运行分析.xls");
        }
        #endregion
    }
}