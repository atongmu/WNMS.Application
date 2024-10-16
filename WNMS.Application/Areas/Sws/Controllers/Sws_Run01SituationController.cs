using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WNMS.Model.DataModels;
using WNMS.IService;
using WNMS.Model.CustomizedClass;
using System.Linq.Expressions;
using WNMS.Utility;
using Microsoft.AspNetCore.Http;
using System.Text;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.Security.Claims;
using WNMS.Application.Utility.Filters;

namespace WNMS.Application.Areas.Sws.Controllers
{
    [Area("Sws")]
    public class Sws_Run01SituationController : Controller
    {
        private IHourQuartZService hourService = null;
        private IDayQuartZService dayService = null;
        private IMonthQuartZService monthService = null;

        private ISws_DeviceInfo01Service deviceInfo01Service = null;
        private ISysUserService userService = null;
        public IActionResult Index()
        {
            ViewBag.DateTime = DateTime.Now.ToString("yyyy-MM-dd");
            return View();
        }
        public Sws_Run01SituationController(IHourQuartZService sws_hourService, ISws_DeviceInfo01Service sws_DeviceInfo01Service,
            ISysUserService sys_UserService, IDayQuartZService dayQuartZService, IMonthQuartZService monthQuartZService)
        {
            hourService = sws_hourService;
            deviceInfo01Service = sws_DeviceInfo01Service;
            userService = sys_UserService;
            dayService = dayQuartZService;
            monthService = monthQuartZService;
        }

        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult GetStationRunData(string stationName, int datetype, string datetime, string order, string sortName, int pageSize = 10, int pageIndex = 1)
        {
            //获取登录用户信息
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = userService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();

            //时间及获取数据库名称处理
            string beginDate = "";
            string endDate = "";
            string datebaseName = "";
            GetDate(datetime, datetype, ref beginDate, ref endDate, ref datebaseName);

            //查询条件处理
            Expression<Func<Device01Data, bool>> funcWhere = null;
            if (!string.IsNullOrWhiteSpace(stationName))
            {
                funcWhere = funcWhere.And(c => c.StationName.Contains(stationName));
            }

            //排序
            bool flag = true;
            if (order == "desc")
            {
                flag = false;
            }
            string sortname = sortName;
            if (string.IsNullOrEmpty(sortName))
            {
                sortname = "StationName";
            }

            //获取数据信息
            PageResult<Device01Data> dataList = this.deviceInfo01Service.GetSituationData(funcWhere, datebaseName, user, beginDate, endDate, pageSize, pageIndex, sortname, flag);

            List<HourQuartZ01> list = new List<HourQuartZ01>();
            List<string> exAxis = new List<string>();
            List<string> fxAxis = new List<string>();
            List<string> eyAxis = new List<string>();
            List<string> fyAxis = new List<string>();

            //获取曲线数据
            if (dataList.DataList != null && dataList.DataList.Count > 0)
            {
                DateTime beginTime = Convert.ToDateTime(beginDate);
                DateTime endTime = Convert.ToDateTime(endDate);
                List<int> stationIDs = dataList.DataList.Select(r => r.StationID).ToList();
                if (datetype == 1)
                {
                    list = hourService.Query<HourQuartZ01>(r => stationIDs.Contains((int)r.StationId) && r.UpdateTime > beginTime && r.UpdateTime <= endTime).ToList();
                }
                if (datetype == 2)
                {
                    list = dayService.Query<DayQuartZ01>(r => stationIDs.Contains((int)r.StationId) && r.UpdateTime > beginTime && r.UpdateTime <= endTime)?.Select(r => new HourQuartZ01
                    {
                        StationId = r.StationId,
                        DataKey = r.DataKey,
                        DataValue = r.DataValue,
                        UpdateTime = r.UpdateTime,
                        Id = r.Id
                    }).ToList();
                }
                if (datetype == 3)
                {
                    list = dayService.Query<MonthQuartZ01>(r => stationIDs.Contains((int)r.StationId) && r.UpdateTime > beginTime && r.UpdateTime <= endTime)?.Select(r => new HourQuartZ01
                    {
                        StationId = r.StationId,
                        DataKey = r.DataKey,
                        DataValue = r.DataValue,
                        UpdateTime = r.UpdateTime,
                        Id = r.Id
                    }).ToList();
                }

                if (list.Count > 0)
                {
                    foreach (var item in stationIDs)
                    {
                        List<HourQuartZ01> listenergy = list.Where(r => r.StationId == item&&r.DataKey== "EnergyCon").OrderBy(r => r.UpdateTime).ToList();
                        List<HourQuartZ01> listflow = list.Where(r => r.StationId == item && r.DataKey == "FlowCon").OrderBy(r => r.UpdateTime).ToList();
                        StringBuilder fsb = new StringBuilder();
                        StringBuilder fxsb = new StringBuilder();
                        StringBuilder exsb = new StringBuilder();
                        StringBuilder esb = new StringBuilder();
                        if (listenergy != null && listenergy.Count > 0)   //能耗曲线
                        {
                            foreach (var data in listenergy)
                            {
                                if (data.DataValue >= 0)
                                {
                                    esb.Append((data.DataValue == null ? "null" : Math.Round((double)data.DataValue, 2).ToString()) + ",");
                                    if (datetype == 1)
                                    {
                                        exsb.Append("'" + Convert.ToDateTime(data.UpdateTime).ToString("HH:mm") + "',");
                                    }
                                    else
                                    {
                                        exsb.Append("'" + Convert.ToDateTime(data.UpdateTime).ToString("yyyy-MM-dd") + "',");
                                    }
                                }                              
                            }
                            exAxis.Add("[" + exsb.ToString().Substring(0, exsb.ToString().Length - 1) + "]");
                            eyAxis.Add("[" + esb.ToString().Substring(0, esb.ToString().Length - 1) + "]");
                        }
                        else
                        {
                            exAxis.Add("[]");
                            eyAxis.Add("[]");
                        }
                        if (listflow != null && listflow.Count > 0)   //流量曲线
                        {
                            foreach (var data in listflow)
                            {
                                if (data.DataValue >= 0)
                                {
                                    fsb.Append((data.DataValue == null ? "null" : Math.Round((double)data.DataValue, 2).ToString()) + ",");
                                    if (datetype == 1)
                                    {
                                        fxsb.Append("'" + Convert.ToDateTime(data.UpdateTime).ToString("HH:mm") + "',");
                                    }
                                    else
                                    {
                                        fxsb.Append("'" + Convert.ToDateTime(data.UpdateTime).ToString("yyyy-MM-dd") + "',");
                                    }
                                }
                            }
                            fxAxis.Add("[" + fxsb.ToString().Substring(0, fxsb.ToString().Length - 1) + "]");
                            fyAxis.Add("[" + fsb.ToString().Substring(0, fsb.ToString().Length - 1) + "]");
                        }
                        else
                        {
                            fxAxis.Add("[]");
                            fyAxis.Add("[]");
                        }
                    }
                }
            }

            return Json(new
            {
                total = dataList.TotalCount,
                rows = Newtonsoft.Json.JsonConvert.SerializeObject(dataList.DataList.ToList()),
                sortor = order,
                sortname = sortName,
                exs = exAxis,
                ey = eyAxis,
                fxs = fxAxis,
                fy = fyAxis
            });
        }

        //导出数据
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult ExportData(string datetime, int datetype, string stationName)
        {
            //获取登录用户信息
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = userService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();

            //时间及获取数据库名称处理
            string beginDate = "";
            string endDate = "";
            string datebaseName = "";
            GetDate(datetime, datetype, ref beginDate, ref endDate, ref datebaseName);

            //查询条件处理
            Expression<Func<Device01Data, bool>> funcWhere = null;
            if (!string.IsNullOrWhiteSpace(stationName))
            {
                funcWhere = funcWhere.And(c => c.StationName.Contains(stationName));
            }

            //排序
            bool flag = true;
            string sortname = "StationName";

            //获取数据信息
            PageResult<Device01Data> dataList = this.deviceInfo01Service.GetSituationData(funcWhere, datebaseName, user, beginDate, endDate, 10000, 1, sortname, flag);
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
            dataRow.CreateCell(0).SetCellValue("泵站名称");
            dataRow.CreateCell(1).SetCellValue("总供水量（m³）");
            dataRow.CreateCell(2).SetCellValue("总耗电量（kW·h）");
            dataRow.CreateCell(3).SetCellValue("单吨能耗（kW·h/m³）");
            dataRow.CreateCell(4).SetCellValue("报警次数");
            dataRow.Height = 20 * 20;
            for (int s = 0; s < 5; s++)
            {
                sheet.SetColumnWidth(s, 25 * 256);
                dataRow.Cells[s].CellStyle = style1;
            }

            List<Device01Data> list = dataList.DataList;
            //填充内容
            for (int j = 0; j < list.Count(); j++)
            {
                dataRow = sheet.CreateRow(j + 1);
                dataRow.CreateCell(0).SetCellValue(list[j].StationName.ToString());
                dataRow.CreateCell(1).SetCellValue(list[j].FlowCon==null?"--":Math.Round((double)list[j].FlowCon,2).ToString());
                dataRow.CreateCell(2).SetCellValue(list[j].EnergyCon==null?"--":Math.Round((double)list[j].EnergyCon,2).ToString());
                if(list[j].FlowCon == null|| list[j].EnergyCon == null|| list[j].FlowCon == 0)
                {
                    dataRow.CreateCell(3).SetCellValue("--");
                }
                else
                {
                    dataRow.CreateCell(3).SetCellValue(Math.Round((double)(list[j].EnergyCon/ list[j].FlowCon),2).ToString());
                }
                dataRow.CreateCell(4).SetCellValue(list[j].Num == null ? "--" : list[j].Num.ToString());
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
            return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "运行概况数据" + datetime + ".xls");
        }

        //时间处理方法，根据不同的时间类型，获取时间
        public void GetDate(string dateTime, int dateType, ref string beginDate, ref string endDate, ref string databaseName)
        {
            beginDate = DateTime.Now.ToString("yyyy-MM-dd");
            endDate = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            databaseName = "HourQuartZ01";
            if (dateType == 1)
            {
                beginDate = Convert.ToDateTime(dateTime).ToString("yyyy-MM-dd");
                endDate = Convert.ToDateTime(dateTime).AddDays(1).ToString("yyyy-MM-dd");
                databaseName = "HourQuartZ01";
            }
            if (dateType == 2)
            {
                beginDate = dateTime + "-01";
                endDate = Convert.ToDateTime(beginDate).AddMonths(1).ToString("yyyy-MM-dd");
                databaseName = "DayQuartZ01";
            }
            if (dateType == 3)
            {
                beginDate = dateTime + "-01-01";
                endDate = Convert.ToDateTime(beginDate).AddYears(1).ToString("yyyy-MM-dd");
                databaseName = "MonthQuartZ01";
            }
        }
    }
}