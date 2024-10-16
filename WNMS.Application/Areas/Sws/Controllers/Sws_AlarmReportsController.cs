using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WNMS.Application.Utility;
using WNMS.Application.Utility.Filters;
using WNMS.IService;
using WNMS.Model;
using WNMS.Model.CustomizedClass;
using WNMS.Model.DataModels;

namespace WNMS.Application.Areas.Sws.Controllers
{
    [Area("Sws")]
    public class Sws_AlarmReportsController : Controller
    {
        private readonly ISws_EventHistoryService _sws_EventHistoryService;
        private readonly ISws_AlarmReportsService _sws_AlarmReportsService;
        private readonly ISws_DeviceInfo01Service _sws_DeviceInfo01Service;
        private readonly ISysUserService _sysUserService;
        private ISws_RTUInfoService rtuService=null;
        private ISws_UserStationService userstationService = null;
        public Sws_AlarmReportsController(ISws_EventHistoryService sws_EventHistoryService,
    ISws_AlarmReportsService sws_AlarmReportsService,
    ISws_DeviceInfo01Service sws_DeviceInfo01Service,
    ISysUserService sysUserService,ISws_RTUInfoService sws_RTUInfoService, ISws_UserStationService sws_UserStationService)
        {
            _sws_EventHistoryService = sws_EventHistoryService;
            _sws_AlarmReportsService = sws_AlarmReportsService;
            _sws_DeviceInfo01Service = sws_DeviceInfo01Service;
            _sysUserService = sysUserService;
            rtuService = sws_RTUInfoService;
            userstationService = sws_UserStationService;

        }
        public IActionResult Index()
        {
            //树形
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = _sysUserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
            var treeNodes = TreesOfStationDevice(user, "");
            if (!string.IsNullOrEmpty(treeNodes))
            {
                ViewBag.TreeNodes = new HtmlString(treeNodes);
            }
            else
            {
                ViewBag.TreeNodes = "[]";
            }

            ViewBag.BeginTimeDay = DateTime.Now.ToString("yyyy-MM-dd");
            ViewBag.BeginTimeMonth = DateTime.Now.ToString("yyyy-MM");
            ViewBag.BeginTimeYear = DateTime.Now.ToString("yyyy");
            ViewBag.BeginTime = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd");
            ViewBag.EndTime = DateTime.Now.ToString("yyyy-MM-dd");
            return View();
 
        }

        #region 数据查询
        [TypeFilter(typeof(IgonreActionFilter))]
        public async Task<IActionResult> QueryAlarmsData(string type, string begindate, string enddate, string deviceid)
        {
            DateTime beginTime = new DateTime(), endTime = new DateTime();
            string dataTable = "";
            IEnumerable<AlarmRep> datalist;
            string tablename = "";
            string timeformate = "";
          
            if (!string.IsNullOrEmpty(deviceid))
            {
               
                if (type == "day")
                {
                    beginTime = Convert.ToDateTime(begindate);
                    endTime = beginTime.AddDays(1);
                   
                    timeformate = "HH:mm";




                }
                else if (type == "month")
                {
                    beginTime = Convert.ToDateTime(begindate + "-01");
                    endTime = beginTime.AddMonths(1);
                    //tablename = "DDayQuartZ01";
                    timeformate = "yyyy-MM-dd";
                    //datalist = _StationService.GetConDataByDeviceID(beginTime.ToString(), endTime.ToString(), deviceids, tablename);
                }
                else if (type == "year")
                {
                    beginTime = Convert.ToDateTime(begindate + "-01-01");
                    endTime = beginTime.AddYears(1);
                    
                    timeformate = "yyyy-MM";
                    //datalist = _StationService.GetConDataByDeviceID(beginTime.ToString(), endTime.ToString(), deviceids, tablename);

                }
                else//自定义
                {
                    beginTime = Convert.ToDateTime(begindate);
                    endTime = Convert.ToDateTime(enddate).AddDays(1);
                 
                    timeformate = "yyyy-MM-dd";
                    //datalist = _StationService.GetConDataByDeviceID(beginTime.ToString(), endTime.ToString(), deviceids, tablename);
                }
                datalist = _sws_AlarmReportsService.GetAlarmDataByDeviceID(beginTime.ToString(), endTime.ToString(), deviceid,type);
                //if (type == "year")
                //{
                //    //只需要查询本月的数据
                //    if (Convert.ToInt32(begindate) == DateTime.Now.Year)
                //    {
                //        var begintimethis = DateTime.Now.ToString("yyyy-MM") + "-01";
                //        //if (datalist.Count() > 0)
                //        //{
                //        //    begintimethis = datalist.LastOrDefault().UpdateTime.AddMonths(1).ToString();
                //        //}
                //        var endtimethis = Convert.ToDateTime(begintimethis).AddMonths(1).ToString();
                //        //var thismonthData = _sws_AlarmReportsService.GetAlarmDataByDeviceID_thisMonth(begintimethis, endtimethis, deviceid);
                //        //if (thismonthData.Count() > 0)
                //        //{
                //        //    datalist = datalist.Union(thismonthData);
                //        //}
                //    }
                //}
                if (datalist.Count() > 0)
                {
                    ViewBag.timeformate = timeformate;
                    //ViewBag.deviceidList = deciceList;
                    PartialView("_alarmsData_New", datalist);
                    dataTable = await ViewToString.RenderPartialViewToString(this, "_alarmsData_New");
                }
                else
                {
                    dataTable = "";
                }

            }
            else
            {
                dataTable = "";
            }
            return Json(new
            {
                dataTable = dataTable

            });
        }


        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult GetAlarmsData(string deviceid, string itemvalue, string time,string type)
        {
            ViewBag.type = type;
            ViewBag.time = time;
            ViewBag.itemvalue = itemvalue;
            ViewBag.deviceid = deviceid;
            return View();

        }
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult GetEventDetailData(string deviceid, string itemvalue, string time, string type, int limit = 10, int page = 1)
        {    //获取报警数据
            List<AlarmDetail> eventlist = new List<AlarmDetail>();
            int totalCount = 0;
            eventlist = _sws_AlarmReportsService.GetAlarmDatas(deviceid, type, time, itemvalue).ToList();

            totalCount = eventlist.Count();
            eventlist = eventlist.Skip((page - 1) * limit).Take(limit).ToList();

            return Json(new { code = "0", msg = "", count = totalCount, data = eventlist });
        }

            public List<int> GetRtuID(SysUser user)
        {
            List<int> rtuIds = new List<int>();
            if (user.IsAdmin)
            {
                rtuIds = rtuService.Query<SwsRtuinfo>(r => true)?.Select(r => r.Rtuid).ToList();
            }
            else
            {
                List<int> sids = userstationService.Query<SwsUserStation>(r => r.UserId == user.UserId).Select(r => r.StationId).ToList();
                if (sids != null)
                {
                    rtuIds = rtuService.Query<SwsRtuinfo>(r => sids.Contains(r.StationId))?.Select(r => r.Rtuid).ToList();
                }
            }
            return rtuIds;
        }
        /// <summary>
        /// 图表
        /// </summary>
        /// <param name="type"></param>
        /// <param name="begindate"></param>
        /// <param name="enddate"></param>
        /// <param name="deviceid"></param>
        /// <returns></returns>
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult GetEchartData(string type, string begindate, string enddate, string deviceid)
        {
            List<string> timelist = new List<string>();
            List<string> cdatalist = new List<string>();
           
            DateTime beginTime = new DateTime(), endTime = new DateTime();
            IEnumerable<AlarmRep> datalist=null;
            string tablename = "";
            string timeformate = "";

            if (!string.IsNullOrEmpty(deviceid))
            {

                if (type == "day")
                {
                    beginTime = Convert.ToDateTime(begindate);
                    endTime = beginTime.AddDays(1);

                    timeformate = "HH:mm";




                }
                else if (type == "month")
                {
                    beginTime = Convert.ToDateTime(begindate + "-01");
                    endTime = beginTime.AddMonths(1);
                    //tablename = "DDayQuartZ01";
                    timeformate = "yyyy-MM-dd";
                    //datalist = _StationService.GetConDataByDeviceID(beginTime.ToString(), endTime.ToString(), deviceids, tablename);
                }
                else if (type == "year")
                {
                    beginTime = Convert.ToDateTime(begindate + "-01-01");
                    endTime = beginTime.AddYears(1);

                    timeformate = "yyyy-MM";
                    //datalist = _StationService.GetConDataByDeviceID(beginTime.ToString(), endTime.ToString(), deviceids, tablename);

                }
                else//自定义
                {
                    beginTime = Convert.ToDateTime(begindate);
                    endTime = Convert.ToDateTime(enddate).AddDays(1);

                    timeformate = "yyyy-MM-dd";
                    //datalist = _StationService.GetConDataByDeviceID(beginTime.ToString(), endTime.ToString(), deviceids, tablename);
                }
                datalist = _sws_AlarmReportsService.GetAlarmDataByDeviceID(beginTime.ToString(), endTime.ToString(), deviceid, type);
 

            }

            foreach (var item in datalist)
            {
                timelist.Add(item.EventTime);
                cdatalist.Add(item.Num.ToString());
            }
            return Json(new
            {
                xAxis = timelist,
                data = cdatalist



            });
        }
        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="type"></param>
        /// <param name="begindate"></param>
        /// <param name="enddate"></param>
        /// <param name="deviceid"></param>
        /// <returns></returns>
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult ExportTable(string type, string begindate, string enddate, string deviceid)
        {
            DateTime beginTime = new DateTime(), endTime = new DateTime();
            IEnumerable<AlarmRep> list;
            string tablename = "";
            string timeformate = "";
            string filename = "";
         
            if (!string.IsNullOrEmpty(deviceid))
            {
  
                if (type == "day")
                {
                    beginTime = Convert.ToDateTime(begindate);
                    endTime = beginTime.AddDays(1);

                    timeformate = "HH:mm";




                }
                else if (type == "month")
                {
                    beginTime = Convert.ToDateTime(begindate + "-01");
                    endTime = beginTime.AddMonths(1);
                    //tablename = "DDayQuartZ01";
                    timeformate = "yyyy-MM-dd";
                    //datalist = _StationService.GetConDataByDeviceID(beginTime.ToString(), endTime.ToString(), deviceids, tablename);
                }
                else if (type == "year")
                {
                    beginTime = Convert.ToDateTime(begindate + "-01-01");
                    endTime = beginTime.AddYears(1);

                    timeformate = "yyyy-MM";
                    //datalist = _StationService.GetConDataByDeviceID(beginTime.ToString(), endTime.ToString(), deviceids, tablename);

                }
                else//自定义
                {
                    beginTime = Convert.ToDateTime(begindate);
                    endTime = Convert.ToDateTime(enddate).AddDays(1);

                    timeformate = "yyyy-MM-dd";
                    //datalist = _StationService.GetConDataByDeviceID(beginTime.ToString(), endTime.ToString(), deviceids, tablename);
                }
                list = _sws_AlarmReportsService.GetAlarmDataByDeviceID(beginTime.ToString(), endTime.ToString(), deviceid, type);


                //数据导出
                HSSFWorkbook workbook = new HSSFWorkbook();
                ISheet sheet = workbook.CreateSheet();
                sheet.SetColumnWidth(0, 30 * 256);
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
                dataRow.CreateCell(0).SetCellValue("更新时间");
                dataRow.Height = 20 * 20;
                dataRow.Cells[0].CellStyle = style1;
                dataRow.CreateCell(1).SetCellValue("报警设备");
                dataRow.CreateCell(2).SetCellValue("报警类型");
                dataRow.CreateCell(3).SetCellValue("报警次数");
                dataRow.Cells[1].CellStyle = style1;
                dataRow.Cells[2].CellStyle = style1;
              
                //填充内容
                int j = 1;
                foreach (var item in list)
                {
                    dataRow = sheet.CreateRow(j);
                    dataRow.CreateCell(0).SetCellValue(item.EventTime);
                    dataRow.CreateCell(1).SetCellValue(item.DeviceName);
                    dataRow.CreateCell(2).SetCellValue(item.ItemName);
                    dataRow.CreateCell(3).SetCellValue(item.Num);
                    filename = item.DeviceName;
                    sheet.GetRow(j).Height = 20 * 20;
                   
                        sheet.GetRow(j).Cells[0].CellStyle = style;
                    sheet.GetRow(j).Cells[1].CellStyle = style;
                    sheet.GetRow(j).Cells[2].CellStyle = style;
                    sheet.GetRow(j).Cells[3].CellStyle = style;
                    j++;
                }
      
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                workbook.Write(ms);
                workbook = null;
                ms.Close();
                ms.Dispose();
                return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename + "报警次数统计" + beginTime.ToString()+"--"+endTime.ToString() + ".xls");
            }
            else
            {
                HSSFWorkbook workbook = new HSSFWorkbook();
                ISheet sheet = workbook.CreateSheet();
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                workbook.Write(ms);
                workbook = null;
                ms.Close();
                ms.Dispose();
                return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "" + filename + ".xls");
            }
        }
        #endregion
        //泵房树形
        [TypeFilter(typeof(IgonreActionFilter))]
        public string TreesOfStationDevice(SysUser sysUser, string stationName)
        {
            List<StationAndDevice> treelist = _sws_DeviceInfo01Service.QueryZtreeInfo(sysUser, stationName).ToList();

            IEnumerable<TreeAction> ztreeStation = treelist.Select(t => new TreeAction
            {
                id = t.StationId,
                pId = 0,
                name = "<em class='iconfont icon-bengfang'></em>" + t.StationName,
                @checked = false,
                isDevice = false
                //icon = "../../Content/zTree/css/zTreeStyle/area.png"
            });

            IEnumerable<TreeAction> ztreeDevice = treelist.Select(t => new TreeAction
            {
                id = t.DeviceId,
                pId = t.StationId,
                name = t.DeviceName,
                @checked = false,
                isDevice = true
                //icon = "../../Content/zTree/css/zTreeStyle/area.png"
            });
            var treeList = ztreeStation.Union<TreeAction>(ztreeDevice).Distinct(new TreeAreaCompare());
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(treeList);
            return json;
            ////var ztreeInfo = _sws_DeviceInfo01Service.QueryZtreeInfo(sysUser, stationName);
            //List<int> stationId = new List<int>();
            //if (sysUser.IsAdmin != true)
            //{
            //    stationId = _sws_UserStationService.Query<SwsUserStation>(r => r.UserId == sysUser.UserId).Select(r => r.StationId).ToList();
            //}
            //else
            //{
            //    stationId = _sws_StationService.Query<SwsStation>(r => true).Select(r => r.StationId).ToList();
            //}
            ////泵房信息
            //var stationList = _sws_StationService.Query<SwsStation>(r => stationId.Contains(r.StationId) && r.StaitonType == 1).ToList();
            ////设备信息
            //var deviceList = _sws_DeviceInfo01Service.Query<SwsDeviceInfo01>(r => stationId.Contains(r.StationId)).ToList();
            //if (!string.IsNullOrEmpty(stationName))
            //{
            //    stationList = stationList.Where(r => r.StationName.Contains(stationName)).ToList();
            //}
            //IEnumerable<TreeAction> ztreeStation = stationList.Select(t => new TreeAction
            //{
            //    id = t.StationId,
            //    pId = 0,
            //    name = "<em class='iconfont icon-bengfang'></em>" + t.StationName,
            //    @checked = false,
            //    isDevice = false
            //    //icon = "../../Content/zTree/css/zTreeStyle/area.png"

            //});

            //IEnumerable<TreeAction> ztreeDevice = deviceList.Select(t => new TreeAction
            //{
            //    id = t.DeviceId,
            //    pId = t.StationId,
            //    name = t.DeviceName,
            //    @checked = false,
            //    isDevice = true
            //    //icon = "../../Content/zTree/css/zTreeStyle/area.png"

            //});
            ////泵房信息
            //var deviceids = deviceList.Select(r => r.StationId).Distinct().ToList();
            //var stationList = _sws_StationService.Query<SwsStation>(r => deviceids.Contains(r.StationId)).ToList();
            //if (!string.IsNullOrEmpty(stationName))
            //{
            //    stationList = stationList.Where(r => r.StationName.Contains(stationName)).ToList();
            //}
            //IEnumerable<TreeAction> ztreeStation = stationList.Select(t => new TreeAction
            //{
            //    id = t.StationId,
            //    pId = 0,
            //    name = t.StationName,
            //    @checked = false,
            //    isDevice = false
            //    //icon = "../../Content/zTree/css/zTreeStyle/area.png"

            //});
            //var treeList = ztreeStation.Union<TreeAction>(ztreeDevice);
            //var json = Newtonsoft.Json.JsonConvert.SerializeObject(treeList);
            //return json;
        }
        //树形查询
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult SearchTree(string stationName)
        {
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var sysUser = _sysUserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();

            var json = TreesOfStationDevice(sysUser, stationName);
            var str = new HtmlString(json);
            return Content(str.ToString());
            //int userID = int.Parse(User.FindFirstValue("UserID"));
            //var sysUser = _sysUserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
            //List<int> stationId = new List<int>();
            //if (sysUser.IsAdmin != true)
            //{
            //    stationId = _sws_UserStationService.Query<SwsUserStation>(r => r.UserId == sysUser.UserId).Select(r => r.StationId).ToList();
            //}
            //else
            //{
            //    stationId = _sws_StationService.Query<SwsStation>(r => true).Select(r => r.StationId).ToList();
            //}
            ////泵房信息
            //var stationList = _sws_StationService.Query<SwsStation>(r => stationId.Contains(r.StationId) && r.StaitonType == 1).ToList();
            //if (!string.IsNullOrEmpty(stationName))
            //{
            //    stationList = stationList.Where(r => r.StationName.Contains(stationName)).ToList();
            //}
            //IEnumerable<TreeAction> ztreeStation = stationList.Select(t => new TreeAction
            //{
            //    id = t.StationId,
            //    pId = 0,
            //    name = t.StationName,
            //    @checked = false,
            //    isDevice = false
            //    //icon = "../../Content/zTree/css/zTreeStyle/area.png"

            //});
            ////设备信息
            //var deviceList = _sws_DeviceInfo01Service.Query<SwsDeviceInfo01>(r => stationId.Contains(r.StationId)).ToList();
            //IEnumerable<TreeAction> ztreeDevice = deviceList.Select(t => new TreeAction
            //{
            //    id = t.DeviceId,
            //    pId = t.StationId,
            //    name = t.DeviceName,
            //    @checked = false,
            //    isDevice = true
            //    //icon = "../../Content/zTree/css/zTreeStyle/area.png"

            //});
            //var treeList = ztreeStation.Union<TreeAction>(ztreeDevice);
            //var json = Newtonsoft.Json.JsonConvert.SerializeObject(treeList);
            //var str = new HtmlString(json);
            //return Content(str.ToString());
        }
    }
}
