using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using WNMS.IService;
using WNMS.Model.DataModels;
using WNMS.Utility;
using WNMS.Model.CustomizedClass;
using WNMS.Application.Utility;
using Newtonsoft.Json;
using System.Security.Claims;
using WNMS.Application.Utility.Filters;

namespace WNMS.Application.Areas.Sws.Controllers
{
    [Area("Sws")]
    public class Sws_DeviceInfo02Controller : Controller
    {
        private ISws_DeviceInfo02Service deviceInfo02Service = null;
        private ISws_StationService stationService = null;
        private ISys_DataItemService dataItemService = null;
        private ISys_DataItemDetailService dataItemDetailServices = null;
        private ISws_GUIInfoService guiInfoService = null;
        public Sws_DeviceInfo02Controller(ISws_DeviceInfo02Service sws_DeviceInfo02Service, ISws_StationService sws_StationService,
            ISys_DataItemDetailService sys_DataItemDetailService, ISys_DataItemService sys_DataItemService, ISws_GUIInfoService sws_GUIInfoService)
        {
            deviceInfo02Service = sws_DeviceInfo02Service;
            stationService = sws_StationService;
            dataItemService = sys_DataItemService;
            dataItemDetailServices = sys_DataItemDetailService;
            guiInfoService = sws_GUIInfoService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [TypeFilter(typeof(IgonreActionFilter))]
        public async Task<IActionResult> LoadDevice02Info(string sortName, string sortOrder, string deviceName, int pageSize = 10, int pageIndex = 1)
        {
            int userID = int.Parse(User.FindFirstValue("UserID"));
            #region 查询条件
            Expression<Func<DeviceInfo02Info, bool>> funcWhere = null;
            if (!string.IsNullOrWhiteSpace(deviceName))
            {
                funcWhere = funcWhere.And(r => r.DeviceName.Contains(deviceName));
            }
            #endregion

            #region  排序
            bool flag = true;
            if (sortOrder == "desc") flag = false;
            string sort = string.IsNullOrWhiteSpace(sortName) ? "DeviceID" : sortName;
            #endregion

            PageResult<DeviceInfo02Info> deviceList = this.deviceInfo02Service.LoadInfoList(funcWhere, pageSize, pageIndex, sort, userID, true, flag);
            PartialView("_Device02Table", deviceList.DataList);
            string dataTable = await ViewToString.RenderPartialViewToString(this, "_Device02Table");
            return Json(new
            {
                total = deviceList.TotalCount,
                pageIndex = deviceList.PageIndex,
                pageSize = deviceList.PageSize,
                order = sortOrder,
                sortName = sortName ?? "",
                dataTable = dataTable
            });
        }

        #region 直饮水设备编辑
        public IActionResult AddDevicePage()
        {
            List<int> ids = new List<int> { 7, 8, 14 };
            //所有类型
            var allType = dataItemDetailServices.Query<SysDataItemDetail>(r => ids.Contains(r.FItemId) && r.IsEnable==true).ToList();

            //直饮水设备类型
            int devicetypeid = (int)WNMS.Model.CustomizedClass.Enum.直饮水设备类型;
            var deviceType = allType.Where(r => r.FItemId == devicetypeid).ToList();
            ViewBag.deviceType = deviceType;

            //获取工艺图
            var guiinfo = guiInfoService.Query<SwsGuiinfo>(r => r.DeviceType == 2).ToList();
            ViewBag.guiinfo = guiinfo;

            //设备品牌
            int Manufacturerid = (int)WNMS.Model.CustomizedClass.Enum.设备品牌;
            var Manufacturerinfo = allType.Where(r => r.FItemId == Manufacturerid).ToList();
            ViewBag.Manufacturerinfo = Manufacturerinfo;

            //设备分区
            int Partitionid = (int)WNMS.Model.CustomizedClass.Enum.设备分区;
            var Partitionfo = allType.Where(r => r.FItemId == Partitionid).ToList();
            ViewBag.Partitionfo = Partitionfo;

            //图片上传需要的预添加设备ID
            SwsDeviceInfo02 swsDeviceInfo02 = new SwsDeviceInfo02();
            swsDeviceInfo02.DeviceId = ConvertDateTimeInt(DateTime.Now);

            ViewBag.StationName = "";
            ViewBag.Key = 0;
            return View("SetDevice", swsDeviceInfo02);
        }

        public IActionResult EditDevicePage(long id)
        {
            List<int> ids = new List<int> { 7, 8, 14 };
            //所有类型
            var allType = dataItemDetailServices.Query<SysDataItemDetail>(r => ids.Contains(r.FItemId) && r.IsEnable == true).ToList();

            //直饮水设备类型
            int devicetypeid = (int)WNMS.Model.CustomizedClass.Enum.直饮水设备类型;
            var deviceType = allType.Where(r => r.FItemId == devicetypeid).ToList();
            ViewBag.deviceType = deviceType;

            //获取工艺图
            var guiinfo = guiInfoService.Query<SwsGuiinfo>(r => r.DeviceType == 2).ToList();
            ViewBag.guiinfo = guiinfo;

            //设备品牌
            int Manufacturerid = (int)WNMS.Model.CustomizedClass.Enum.设备品牌;
            var Manufacturerinfo = allType.Where(r => r.FItemId == Manufacturerid).ToList();
            ViewBag.Manufacturerinfo = Manufacturerinfo;

            //设备分区
            int Partitionid = (int)WNMS.Model.CustomizedClass.Enum.设备分区;
            var Partitionfo = allType.Where(r => r.FItemId == Partitionid).ToList();
            ViewBag.Partitionfo = Partitionfo;

            SwsDeviceInfo02 swsDeviceInfo02 = deviceInfo02Service.Find<SwsDeviceInfo02>(id);
            var swsStation = stationService.Find<SwsStation>(swsDeviceInfo02.StationId);
            if (swsStation != null)
            {
                ViewBag.StationName = swsStation.StationName;
            }
            else
            {
                ViewBag.StationName = "";
            }
            ViewBag.Key = 1;
            return View("SetDevice", swsDeviceInfo02);
        }

        [HttpPost]
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult SetDeviceInfo(string device,string key)
        {
            SwsDeviceInfo02 deviceData = string.IsNullOrEmpty(device) ? null : JsonConvert.DeserializeObject<SwsDeviceInfo02>(device);
            List<SwsDeviceInfo02> device02 = this.deviceInfo02Service.Query<SwsDeviceInfo02>(r => r.StationId == deviceData.StationId &&
            r.Partition == deviceData.Partition && r.DeviceId != deviceData.DeviceId).ToList();
            if (device02.Count > 0)
            {
                return Content("false");
            }
            else
            {
                if (key == "0")    //插入
                {
                    deviceData.CreateTime = DateTime.Now;
                    int newdevice = deviceInfo02Service.InsertData(deviceData);
                    if (newdevice > 0)
                    {
                        return Content("ok");
                    }
                    else
                    {
                        return Content("no");
                    }
                }
                else    //修改
                {
                    if (deviceInfo02Service.Update(deviceData))
                    {
                        return Content("ok");
                    }
                    else
                    {
                        return Content("no");
                    }
                }
            }            
        }

        [HttpPost]
        public IActionResult DeleteDevice(string deviceId)
        {
            List<long> detailids = new List<string>(deviceId.Split(',')).ConvertAll(r => long.Parse(r));
            List<SwsDeviceInfo02> deleteDetail = deviceInfo02Service.Query<SwsDeviceInfo02>(r => detailids.Contains(r.DeviceId)).ToList();

            if (this.deviceInfo02Service.DeleteDevice(deleteDetail)>0)
            {              
                return Content("ok");
            }
            else
            {
                return Content("no");
            }
        }
        #endregion
        //时间戳
        public static long ConvertDateTimeInt(System.DateTime time)
        {
            System.DateTime startTime = System.TimeZoneInfo.ConvertTimeToUtc(new System.DateTime(1970, 1, 1));
            return (long)(time - startTime).TotalMilliseconds;
        }

        #region 导入导出
        //导出泵站信息
        [TypeFilter(typeof(IgonreActionFilter))]
        public ActionResult ExportData(string deviceName, string sort, string sortOrder)
        {
            int userID = int.Parse(User.FindFirstValue("UserID"));
            #region 查询条件
            Expression<Func<WNMS.Model.CustomizedClass.DeviceInfo02Info, bool>> funcWhere = null;
            if (!string.IsNullOrWhiteSpace(deviceName))
            {
                funcWhere = funcWhere.And(r => r.DeviceName.Contains(deviceName));
            }
            bool flag = true;
            if (sortOrder == "desc")
            {
                flag = false;
            }
            #endregion

            #region  排序
            Expression<Func<WNMS.Model.CustomizedClass.DeviceInfo02Info, string>> funcOrderby = r => r.DeviceName;
            #endregion

            var list = this.deviceInfo02Service.LoadInfoList(funcWhere, 1, 1, sort, userID, false,flag).DataList;


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
            dataRow.CreateCell(0).SetCellValue("设备名称");
            dataRow.CreateCell(1).SetCellValue("设备编号");
            dataRow.CreateCell(2).SetCellValue("所属泵房");
            dataRow.CreateCell(3).SetCellValue("分区");
            dataRow.CreateCell(4).SetCellValue("设备类型");
            dataRow.CreateCell(5).SetCellValue("厂商");
            dataRow.CreateCell(6).SetCellValue("生产日期");
            for (int i = 0; i < 10; i++)
            {
                sheet.SetColumnWidth(i, 30 * 256);
            }
            for (int s = 0; s < 7; s++)
            {
                sheet.SetColumnWidth(s, 25 * 256);
                dataRow.Cells[s].CellStyle = style1;
            }
            int j = 1;
            foreach (var item in list)
            {
                dataRow = sheet.CreateRow(j);
                dataRow.Height = 100 * 4;
                dataRow.CreateCell(0).SetCellValue(item.DeviceName?.ToString());
                dataRow.CreateCell(1).SetCellValue(item.DeviceNum?.ToString());
                dataRow.CreateCell(2).SetCellValue(item.StationName?.ToString());
                dataRow.CreateCell(3).SetCellValue(item.PartitionName?.ToString());
                dataRow.CreateCell(4).SetCellValue(item.DeviceTypeName?.ToString());
                dataRow.CreateCell(5).SetCellValue(item.ManufacturerName?.ToString());
                dataRow.CreateCell(6).SetCellValue(item.ProductionDate==null?"":Convert.ToDateTime(item.ProductionDate).ToString("yyyy-MM-dd"));
                j++;
            }

            for (int ro = 1; ro < list.Count + 1; ro++)
            {
                sheet.GetRow(ro).Height = 20 * 20;
                for (int s = 0; s < 7; s++)
                {
                    sheet.GetRow(ro).Cells[s].CellStyle = style;
                }
            }
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            workbook.Write(ms);
            workbook = null;
            ms.Close();
            ms.Dispose();
            return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "设备数据.xls");
        }
        #region 导入设备信息 
        [HttpPost]
        //public async Task<int> Import(IFormFile excelfile)
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
            List<SwsDeviceInfo02> list = new List<SwsDeviceInfo02> { };

            //设备编号
            List<string> DeviceNums = new List<string>() { };
            //泵房id
            List<int> stationid = new List<int>() { };
            List<StationPartion> keyValuePairs = new List<StationPartion>();
            StationPartion stationPartion = new StationPartion();

            MemoryStream ms = new MemoryStream();
            excelfile.CopyTo(ms);
            ms.Seek(0, SeekOrigin.Begin);
            List<int> ids = new List<int> { 7, 8, 14 };
            //所有类型
            var allType = dataItemDetailServices.Query<SysDataItemDetail>(r => ids.Contains(r.FItemId)).ToList();

            Random random = new Random();
            int raInt = random.Next(1, 10000);
            long startId = ConvertDateTimeInt(DateTime.Now) + raInt;
            //IWorkbook workbook = new XSSFWorkbook(ms);
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
            for (int i = 1; i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);
                SwsDeviceInfo02 obj = new SwsDeviceInfo02();
                if (row.GetCell(0) != null)
                {

                    obj.DeviceId = startId + i;
                    obj.StationId = int.Parse(row.GetCell(0).ToString());
                    stationid.Add(int.Parse(row.GetCell(0).ToString()));
                    obj.DeviceName = row.GetCell(3)?.ToString();
                    obj.DeviceNum = row.GetCell(4)?.ToString();
                    DeviceNums.Add(row.GetCell(4)?.ToString());

                    var sreDevice = row.GetCell(5)?.ToString();
                    var deviceType = allType.Where(r => r.ItemName.Contains(sreDevice)).FirstOrDefault();
                    if (deviceType != null)
                    {
                        obj.DeviceType = int.Parse(deviceType.ItemValue);
                    }
                    else
                    {
                        obj.DeviceType = 1;
                    }
                    //var bp = row.GetCell(6)?.ToString();
                    //if (bp == "单变频")
                    //{
                    //    obj.Frequency = 1;
                    //}
                    //else
                    //{
                    //    obj.Frequency = 2;
                    //}
                    var par = row.GetCell(6)?.ToString();
                    switch (par)
                    {
                        case "低区":
                            obj.Partition = 1;
                            break;
                        case "中区":
                            obj.Partition = 2;
                            break;
                        case "高区":
                            obj.Partition = 3;
                            break;
                        case "超高区":
                            obj.Partition = 4;
                            break;
                        case "超超高区":
                            obj.Partition = 5;
                            break;
                        default:
                            obj.Partition = 1;
                            break;
                    }
                    obj.ProductionDate = DateTime.Now;
                    stationPartion.stationid = obj.StationId;
                    stationPartion.partion = obj.Partition;
                    keyValuePairs.Add(stationPartion);
                    var strManufacturer = row.GetCell(7)?.ToString();
                    var Manufacturer = allType.Where(r => r.ItemName.Contains(strManufacturer)).FirstOrDefault();
                    if (Manufacturer != null)
                    {
                        obj.Manufacturer = int.Parse(Manufacturer.ItemValue);
                    }
                    else
                    {
                        obj.Manufacturer = 1;
                    }
                    obj.Gui = 1;
                }
                else
                {
                    continue;
                }
                list.Add(obj);
            }

            #region 判断设备编码是否有重复
            if (DeviceNums.Count > 0)
            {
                //判断添加进去的是否有重复
                if (DeviceNums.Count != DeviceNums.Distinct().Count())
                {
                    return Content("文件中有重复的设备编码");
                }
                var hasDevice = deviceInfo02Service.Query<SwsDeviceInfo02>(r => DeviceNums.Contains(r.DeviceNum)).Select(r => r.DeviceNum).ToList();
                if (hasDevice.Count > 0)
                {
                    var hasDeviceNum = string.Join(',', hasDevice);
                    return Content("文件中的设备编码与数据库相同，" + hasDeviceNum + "");
                }
            }
            #endregion
            #region  判断泵房类型
            if (stationid.Count > 0)
            {
                var stationlist = stationService.Query<SwsStation>(r => stationid.Contains(r.StationId) && r.InType != 2 && r.InType != 0).ToList();
                if (stationlist.Count > 0)
                {
                    var hasDevice = string.Join(',', stationlist.Select(r => r.StationName).ToList());
                    return Content("文件中的泵房类型存在非直饮水泵房," + hasDevice + "");
                }
            }
            #endregion

            #region  判断泵房下是否有重复的分区
            List<IGrouping<int, StationPartion>> group = keyValuePairs.GroupBy(r => r.stationid).ToList();
            if (group.Count > 0)
            {
                foreach(var item in group)
                {
                    List<byte> partion = deviceInfo02Service.Query<SwsDeviceInfo02>(r => r.StationId == item.Key)?.Select(r => r.Partition).ToList();
                    List<int> part = item.Select(r => r.partion).ToList();
                    var count = partion.Count() + part.Count();
                    var countnew = partion.Distinct().Count() + part.Distinct().Count();
                    if (count != countnew && partion.Count != 0)
                    {
                        return Content("泵房ID为" + item.Key + "的泵房下存在相同的分区，请重新编辑");
                    }
                }
            }
            #endregion

            if (deviceInfo02Service.DeviceImport(list) > 0)
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }
            // _sws_DeviceInfo01Service.DeviceImport(list);
        }
        #endregion
        #endregion
    }
}