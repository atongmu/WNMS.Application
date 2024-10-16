using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WNMS.Application.Utility.Filters;
using WNMS.IService;
using WNMS.Model.CustomizedClass;
using WNMS.Model.DataModels;

namespace WNMS.Application.Areas.Com.Controllers
{
    [Area("Com")]
    public class AccessoriesChartController : Controller
    {
        #region 属性 构造函数
        ISws_AcccessoriesChartService accessoriesChartService = null;
        ISysUserService _sysUserService = null;
        ISws_AccessoriesEquService accEquService = null;
        public AccessoriesChartController(ISws_AcccessoriesChartService sws_AccessoriesChartService, ISysUserService userservice,ISws_AccessoriesEquService _accEquService)
        {
            accessoriesChartService = sws_AccessoriesChartService;
            _sysUserService = userservice;
            accEquService = _accEquService;
        }
        #endregion

        #region 页面初始化及树查询
        public IActionResult Index()
        {
            //获取设备树信心并返回
            var treeNodes = GetStationTree("");
            if (!string.IsNullOrEmpty(treeNodes))
            {
                ViewBag.TreeNodes = new HtmlString(treeNodes);
            }
            else
            {
                ViewBag.TreeNodes = "[]";
            }
            return View();
        }

        //获取器件设备树
        public string GetStationTree(string deviceName)
        {
            //获取登录用户信息
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = _sysUserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();

            //获取设备树信息
            List<StationDevice> treelist = new List<StationDevice>();
            treelist = accessoriesChartService.LoadZtreeInfo(user, deviceName).ToList();

            //初始化树信息
            IEnumerable<TreeAction> ztreeStation = treelist.Select(t => new TreeAction
            {
                id = t.StationId,
                pId = 0,
                name = "<em class='iconfont icon-bengfang'></em>" + t.StationName,
                @checked = false,
                isDevice = false,
                type=t.EquType,
                isSave=false,
                isParent=true
            });
            
            IEnumerable<TreeAction> ztreeDevice = treelist.Select(t => new TreeAction
            {
                id = t.DeviceId,
                pId = t.StationId,
                name = t.DeviceName+"("+t.Total+")",
                @checked = false,
                isDevice = true,
                type = t.EquType,
                isSave = false,
                isParent = t.Total>0?true:false
            });
            var treeList = ztreeStation.Union<TreeAction>(ztreeDevice).Distinct(new TreeAreaCompare());

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(treeList);
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


        //获取设备下的器件
        [HttpPost]
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult LoadAccessoriesTree(long id,byte type)
        {
            //获取器件
            List<AccessEquInfo> treelist = new List<AccessEquInfo>();
            treelist = accessoriesChartService.LoadAccZtree(id, type).ToList();
            IEnumerable<TreeAction> ztreeDevice = treelist.Select(t => new TreeAction
            {
                id = t.ID,
                pId = t.EquipmentID,
                name = t.Name,
                @checked = false,
                isDevice = false,
                isSave = true
            });
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(ztreeDevice);
            if (!string.IsNullOrEmpty(json))
            {
                return Content(json);
            }
            else
            {
                return Content("[]");
            }
        }
        #endregion

        #region 查询器件生命周期曲线数据
        [HttpPost]
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult LoadChartData(long accid)
        {
            string date = "";
            string data = "";
            string symbol = "";        //曲线标注  到期时警示
            string value = "";         //曲线标注的值
            List<string> timeList = new List<string>();  //x轴数据
            List<double> datas = new List<double>();     //y轴数据
            List<int> nowdatas = new List<int>();        //天数  中间使用
            int days = 0;       //已运行天数
            //查询曲线数据
            AccChartData accList = new AccChartData();
            accList = accessoriesChartService.LoadAccChartData(accid).FirstOrDefault();
            if (accList != null)
            {
                data = accList.ChartData;
            }

            //查询使用日期数据
            SwsAccessoriesEqu accequ = new SwsAccessoriesEqu();
            accequ = accEquService.Query<SwsAccessoriesEqu>(r => r.Id == accid).FirstOrDefault();
            if (accequ != null)
            {
                date = accequ.DeliveryDate.ToString("yyyy-MM-dd");
                if (DateTime.Now > accequ.DeliveryDate)
                {
                    TimeSpan ts = DateTime.Now - accequ.DeliveryDate;
                    days = ts.Days;
                }
            }
            if (data != null &&days>0)
            {
                //曲线点处理
                double tdata = 0.0;
                var obj = Newtonsoft.Json.JsonConvert.DeserializeObject(data);
                Newtonsoft.Json.Linq.JArray ar = (Newtonsoft.Json.Linq.JArray)obj;
                for (int i = 0; i < ar.Count; i++)
                {
                    var item = ar[i];
                    int s = Convert.ToInt32(item[0]);
                    double val = Convert.ToDouble(item[1]);
                    string xdata = accequ.DeliveryDate.AddDays(s).ToString("yyyy-MM-dd");
                    int k = i;

                    //如果没有（0，1）点，添加这个点
                    if (i == 0 && s > 0)
                    {
                        timeList.Add(date);
                        datas.Add(1);
                        nowdatas.Add(0);
                    }
                    //添加当前天
                    if (i == 0 && days < s)
                    {
                        timeList.Add(DateTime.Now.ToString("yyy-MM-dd"));
                        //tdata = Math.Round(1.0 - (1.0 - val) * ((double)days / s), 3);  //按照比例分配
                        tdata = Math.Round((1.0 + val) / 2, 3);         //取中间值      按照比例分配合理，但是曲线x不是time类型，添加的点间隔相等，在两点中间，所以去中间值
                        datas.Add(tdata);
                        nowdatas.Add(days);
                    }
                    else
                    {
                        if ((int)ar[0][0] > 0)
                        {
                            k = i + 1;
                        }
                        if (days > nowdatas[k - 1] && days < s)
                        {
                            timeList.Add(DateTime.Now.ToString("yyy-MM-dd"));
                            //tdata = Math.Round(datas[k - 1] - (datas[k - 1] - val) * ((double)(days - nowdatas[k - 1]) / (s - nowdatas[k - 1])), 3);  //按照比例分配
                            tdata = Math.Round((datas[k - 1] + val) / 2, 3);
                            datas.Add(tdata);
                            nowdatas.Add(days);
                        }
                    }

                    //添加点
                    timeList.Add(xdata);
                    nowdatas.Add(s);
                    datas.Add(val);
                }
                DateTime dt = Convert.ToDateTime(timeList.Last()).AddMonths(-6);
                int count2 = nowdatas.Count() - nowdatas.Where(r => r > (nowdatas.Last() - 180)).ToList().Count();   //分段最后一段的分隔点

                //往曲线上添加当前时间点
                var nd = nowdatas.Where(r => r <= days).ToList();

                //计算即将到期时间点 曲线markpoint用
                if (DateTime.Now > dt)
                {
                    symbol = "image://../../images/bbz.png";
                    value = "报废预警";
                }
                else
                {
                    symbol = "pin";
                    value = tdata.ToString();
                }

                return Json(new
                {
                    xAxis = timeList,
                    yAxis = datas,
                    days,
                    date,
                    count = nd.Count - 1,
                    symbol,
                    tdata,
                    count2,
                    value
                });
            }
            else
            {
                return Json(new
                {
                    xAxis = timeList,
                    yAxis = datas,
                    days,
                    date
                });
            }
        }
        #endregion

        #region 导入 导出模板
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult Import(IFormFile excelfile)
        {
            string ExcelKzm = ".xls|.xlsx";
            string OldName = string.Empty;
            OldName = "." + excelfile.FileName.Split('.')[1];
            if (ExcelKzm.IndexOf(OldName) < 0)
            {
                return Content("typeno");
            }
            List<SwsAcccessoriesChart> list = new List<SwsAcccessoriesChart> { };

            MemoryStream ms = new MemoryStream();
            excelfile.CopyTo(ms);
            ms.Seek(0, SeekOrigin.Begin);
            IWorkbook workbook = null;
            if (OldName == ".xls")
            {
                workbook = new HSSFWorkbook(ms);

            }
            else
            {
                workbook = new XSSFWorkbook(ms);

            }
            ISheet sheet = workbook.GetSheetAt(0);
            try
            {
                for (int i = 1; i <= sheet.LastRowNum; i++)
                {
                    IRow row = sheet.GetRow(i);
                    SwsAcccessoriesChart obj = new SwsAcccessoriesChart();
                    if (row.GetCell(0) != null&& row.GetCell(1).ToString() != "")
                    {
                        obj.AccessoriesId = row.GetCell(0)?.ToString();
                        obj.ChartData = row.GetCell(1)?.ToString();
                    }
                    else
                    {
                        continue;
                    }
                    list.Add(obj);
                }
                ms.Dispose();
                if (accessoriesChartService.ChartDataImport(list) > 0)
                {
                    return Content("ok");
                }
                else
                {
                    return Content("no");
                }
            }
            catch (Exception ex)
            {
                return Content("exception");
            }

        }

        /// <summary>
        /// 导出器件生命周期数据导入模板
        /// </summary>
        /// <returns></returns>
        [TypeFilter(typeof(IgonreActionFilter))]
        public ActionResult CycleImportExport()
        {
            var datalist = accessoriesChartService.ImportChartsData().ToList();

            //数据导出
            HSSFWorkbook workbook = new HSSFWorkbook();
            ISheet sheet = workbook.CreateSheet();


            //填充表头
            IRow dataRow = sheet.CreateRow(0);
            dataRow.Height = 20 * 20;
            dataRow.CreateCell(0).SetCellValue("器件ID");
            dataRow.CreateCell(1).SetCellValue("器件生命周期趋势数据");
            dataRow.CreateCell(2).SetCellValue("器件名称");
            for (int i = 0; i < 3; i++)
            {
                sheet.SetColumnWidth(i, 15 * 256);
            }
            //for (int s = 0; s < 3; s++)
            //{
            //    sheet.SetColumnWidth(s, 25 * 256);
            //    dataRow.Cells[s].CellStyle = style1;
            //}
            int j = 1;
            foreach (var item in datalist)
            {
                dataRow = sheet.CreateRow(j);
                dataRow.Height = 100 * 4;
                dataRow.CreateCell(0).SetCellValue(item.Id?.ToString());
                dataRow.CreateCell(1).SetCellValue(item.ChartData??"");
                dataRow.CreateCell(2).SetCellValue(item.Name?.ToString());
                j++;
            }

            ////for (int ro = 1; ro < list.Count + 1; ro++)
            ////{
            ////    sheet.GetRow(ro).Height = 20 * 20;
            ////    for (int s = 0; s < 3; s++)
            ////    {
            ////        sheet.GetRow(ro).Cells[s].CellStyle = style;
            ////    }
            ////}
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            workbook.Write(ms);
            workbook = null;
            ms.Close();
            ms.Dispose();
            return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "器件生命周期数据模板.xls");
        }
        #endregion
    }
}
