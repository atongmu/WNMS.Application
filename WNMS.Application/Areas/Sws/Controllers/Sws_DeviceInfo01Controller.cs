using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using WNMS.Application.Utility;
using WNMS.Application.Utility.Filters;
using WNMS.IService;
using WNMS.Model.DataModels;
using WNMS.Utility;

namespace WNMS.Application.Areas.Sws.Controllers
{
    [Area("Sws")]
    public class Sws_DeviceInfo01Controller : Controller
    {
        private readonly ISws_DeviceInfo01Service _sws_DeviceInfo01Service;
        private readonly ISws_StationService _sws_StationService;
        private readonly ISys_DataItemService _sys_DataItemService;
        private readonly ISys_DataItemDetailService _sys_DataItemDetailServices;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ISws_GUIInfoService _sws_GUIInfoService;
        private readonly ISysUserService _sysUserService;
        private readonly ISws_UserStationService _swsUserStationService;
        private readonly ISws_ValveWith01Service valveService;
        private readonly ILogger<Sws_DeviceInfo01Controller> logger = null;

        Command.HttpHelper hp = new Command.HttpHelper();

        public Sws_DeviceInfo01Controller(ISws_DeviceInfo01Service sws_DeviceInfo01Service,
            ISws_StationService sws_StationService,
            ISys_DataItemService sys_DataItemService,
            ISys_DataItemDetailService sys_DataItemDetailServices,
            IWebHostEnvironment webHostEnvironment,
            ISws_GUIInfoService sws_GUIInfoService,
            ISysUserService sysUserService,
            ISws_UserStationService swsUserStationService,
            ISws_ValveWith01Service sws_ValveWith01Service,
            ILogger<Sws_DeviceInfo01Controller> myLogger
            )
        {
            _sws_DeviceInfo01Service = sws_DeviceInfo01Service;
            _sws_StationService = sws_StationService;
            _sys_DataItemService = sys_DataItemService;
            _sys_DataItemDetailServices = sys_DataItemDetailServices;
            _webHostEnvironment = webHostEnvironment;
            _sws_GUIInfoService = sws_GUIInfoService;
            _sysUserService = sysUserService;
            _swsUserStationService = swsUserStationService;
            valveService = sws_ValveWith01Service;
            logger = myLogger; 
        }
        #region 数据查询
        public IActionResult Index()
        {
            return View();
        }
        //数据查询
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult LoadInfoList(int pageSize, int pageIndex, string sort, string sortOrder, string deviceName)
        {
            #region 查询条件
            Expression<Func<WNMS.Model.CustomizedClass.DeviceInfo01Info, bool>> funcWhere = null;
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = _sysUserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
            if (user.IsAdmin != true)
            {
                var ids = _swsUserStationService.Query<SwsUserStation>(r => r.UserId == user.UserId).Select(r => r.StationId).ToList();
                funcWhere = funcWhere.And(r => ids.Contains(r.StationId));
            }
            if (!string.IsNullOrWhiteSpace(deviceName))
            {
                funcWhere = funcWhere.And(r => r.DeviceName.ToLower().Contains(deviceName.ToLower()));
                //funcWhere = funcWhere.Or(r =>  r.RDeviceID.Contains(deviceName));
            }
            bool flag = true;
            if (sortOrder == "desc")
            {
                flag = false;
            }
            string functwoOrderby = "Partition";
            #endregion

            #region  排序
            //Expression<Func<WNMS.Model.CustomizedClass.DeviceInfo01Info, string>> funcOrderby = r => r.DeviceName;
            #endregion

            PageResult<WNMS.Model.CustomizedClass.DeviceInfo01Info> dataList = this._sws_DeviceInfo01Service.LoadorderInfoList(funcWhere, pageSize, pageIndex, sort, functwoOrderby, flag);

            return Json(new { total = dataList.TotalCount, rows = Newtonsoft.Json.JsonConvert.SerializeObject(dataList.DataList.ToList()), sortor = sortOrder, sortname = sort });
        }

        #endregion

        #region 信息增删改查

        /// <summary>
        /// 添加
        /// </summary>
        /// <returns></returns>
        public IActionResult AddPage()
        {
            List<int> ids = new List<int> { 5, 6, 7, 8 };
            //所有类型
            var allType = _sys_DataItemDetailServices.Query<SysDataItemDetail>(r => ids.Contains(r.FItemId)).ToList();
            //设备类型
            int devicetypeid = (int)WNMS.Model.CustomizedClass.Enum.设备类型;
            var deviceType = allType.Where(r => r.FItemId == devicetypeid).ToList();
            ViewBag.deviceType = deviceType;
            //变频器类型
            int frequencyid = (int)WNMS.Model.CustomizedClass.Enum.变频分类;
            var frequencytype = allType.Where(r => r.FItemId == frequencyid).ToList();
            ViewBag.frequencytype = frequencytype;
            //图片上传需要的预添加设备ID
            var idflag = ConvertDateTimeInt(DateTime.Now);
            ViewBag.idflag = idflag;
            //获取工艺图
            var guiinfo = _sws_GUIInfoService.Query<SwsGuiinfo>(r => r.DeviceType == 1 && r.PumpNum == 2 && r.IsSum == 2).ToList();
            //var guiinfo = _sws_GUIInfoService.Query<SwsGuiinfo>(r => r.DeviceType == 1).ToList();
            ViewBag.guiinfo = guiinfo;
            //设备品牌
            int Manufacturerid = (int)WNMS.Model.CustomizedClass.Enum.设备品牌;
            var Manufacturerinfo = allType.Where(r => r.FItemId == Manufacturerid).ToList();
            ViewBag.Manufacturerinfo = Manufacturerinfo;
            //设备分区
            int Partitionid = (int)WNMS.Model.CustomizedClass.Enum.设备分区;
            var Partitionfo = allType.Where(r => r.FItemId == Partitionid).ToList();
            ViewBag.Partitionfo = Partitionfo;
            SwsDeviceInfo01 swsDeviceInfo01 = new SwsDeviceInfo01();
            ViewBag.StationName = "";
            return View("SetInfo", swsDeviceInfo01);
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="id">设备ID</param>
        /// <returns></returns>
        public IActionResult EditPage(long id)
        {
            SwsDeviceInfo01 swsDeviceInfo01 = _sws_DeviceInfo01Service.Find<SwsDeviceInfo01>(id);
            List<int> ids = new List<int> { 5, 6, 7, 8 };
            //所有类型
            var allType = _sys_DataItemDetailServices.Query<SysDataItemDetail>(r => ids.Contains(r.FItemId)).ToList();
            //设备类型
            int devicetypeid = (int)WNMS.Model.CustomizedClass.Enum.设备类型;
            //var deviceType = _sys_DataItemDetailServices.Query<SysDataItemDetail>(r => r.FItemId == devicetypeid).ToList();
            var deviceType = allType.Where(r => r.FItemId == devicetypeid).ToList();
            ViewBag.deviceType = deviceType;
            //变频器类型
            int frequencyid = (int)WNMS.Model.CustomizedClass.Enum.变频分类;
            var frequencytype = allType.Where(r => r.FItemId == frequencyid).ToList();
            ViewBag.frequencytype = frequencytype;
            //获取工艺图
            var guiinfo = _sws_GUIInfoService.Query<SwsGuiinfo>(r => r.DeviceType == 1 && r.EquipType == swsDeviceInfo01.DeviceType && r.PumpNum == swsDeviceInfo01.PumpNum && r.IsSum == 2).ToList();
            //var guiinfo = _sws_GUIInfoService.Query<SwsGuiinfo>(r => r.DeviceType == 1).ToList();
            ViewBag.guiinfo = guiinfo;
            //设备品牌
            int Manufacturerid = (int)WNMS.Model.CustomizedClass.Enum.设备品牌;
            var Manufacturerinfo = allType.Where(r => r.FItemId == Manufacturerid).ToList();
            ViewBag.Manufacturerinfo = Manufacturerinfo;
            //设备分区
            int Partitionid = (int)WNMS.Model.CustomizedClass.Enum.设备分区;
            var Partitionfo = allType.Where(r => r.FItemId == Partitionid).ToList();
            ViewBag.Partitionfo = Partitionfo;

            ViewBag.idflag = swsDeviceInfo01.DeviceId;
            var swsStation = _sws_StationService.Find<SwsStation>(swsDeviceInfo01.StationId);
            if (swsStation != null)
            {
                ViewBag.StationName = swsStation.StationName;
            }
            else
            {
                ViewBag.StationName = "";
            }
            return View("SetInfo", swsDeviceInfo01);
        }

        [TypeFilter(typeof(IgonreActionFilter))]
        public async Task<IActionResult> SetInfo(string baseinfo, string requinfo, long deviceid, long idflag)
        {
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = _sysUserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
            SwsDeviceInfo01 baseInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<SwsDeviceInfo01>(baseinfo);
            SwsDeviceInfo01 deviceInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<SwsDeviceInfo01>(requinfo);

            if (string.IsNullOrWhiteSpace(baseInfo.DeviceNum))
            {

            }
            baseInfo.PumpNum = deviceInfo.PumpNum;
            baseInfo.PumpType = deviceInfo.PumpType;
            baseInfo.Frequency = deviceInfo.Frequency;
            baseInfo.ImageUrl = deviceInfo.ImageUrl;
            baseInfo.Gui = deviceInfo.Gui;
            //添加
            if (deviceid == 0)
            {
                //判断是否存在泵房分区重复数据
                var deciceidlist = _sws_DeviceInfo01Service.Query<SwsDeviceInfo01>(r => r.StationId == baseInfo.StationId).Select(r => r.Partition).ToList();
                if (deciceidlist.Contains(baseInfo.Partition))
                {
                    return Content("has");
                }
                //baseInfo.DeviceId = ConvertDateTimeInt(DateTime.Now);
                baseInfo.DeviceId = idflag;
                baseInfo.CreateTime = DateTime.Now;

                if (_sws_DeviceInfo01Service.DeviceInsert(baseInfo) > 0)
                {
                    if (_sws_DeviceInfo01Service.SetAreaInfo(baseInfo.DeviceId) >= 0)
                    {
                        logger.LogInformation(user.NickName + " 添加设备-" + baseInfo.DeviceName);
                        return Content("ok");
                    }
                    else
                    {
                        return Content("no");
                    }
                    //Task task = Task.Run(() =>
                    //{
                    //    string aa = hp.HttpGet("http://192.168.61.16:8733/WnmsWebService/DeviceCtrl?guid=CF27D377-8073-4966-BA86-061A5E246E19&RTUID=" + idflag + "&cmdType=0", "");
                    //});

                    //TaskFactory taskFactory = new TaskFactory();
                    //Task task = taskFactory.StartNew(()=> {
                    //    string aa = hp.HttpGet("http://192.168.61.16:8733/WnmsWebService/DeviceCtrl?guid=CF27D377-8073-4966-BA86-061A5E246E19&RTUID="+ idflag + "&cmdType=0", "");
                    //});
                    // return Content("ok");
                }
                else
                {
                    return Content("no");
                }
            }
            else
            {
                //修改
                baseInfo.DeviceId = deviceid;
                var deciceidlist = _sws_DeviceInfo01Service.Query<SwsDeviceInfo01>(r => r.StationId == baseInfo.StationId && r.DeviceId != deviceid).Select(r => r.Partition).ToList();
                if (deciceidlist.Contains(baseInfo.Partition))
                {
                    return Content("has");
                }
                if (_sws_DeviceInfo01Service.Update<SwsDeviceInfo01>(baseInfo))
                {
                    if (_sws_DeviceInfo01Service.SetAreaInfo(baseInfo.DeviceId) >= 0)
                    {
                        logger.LogInformation($"{user.NickName} 修改设备-{baseInfo.DeviceName}");
                        return Content("ok");
                    }
                    else
                    {
                        return Content("no");
                    }
                }
                else
                {
                    return Content("no");
                }
            }
        }
        //获取工艺图信息
        public ActionResult GetGUI(string DeviceType, string PumpNum)
        {

            int DevType = DeviceType == "" ? 0 : int.Parse(DeviceType);
            int pumpnum = int.Parse(PumpNum);
            var gui = _sws_GUIInfoService.Query<SwsGuiinfo>(r => r.DeviceType == 1 && r.PumpNum == pumpnum && r.EquipType == DevType && r.IsSum == 2).ToList();

            //if (gui != null)
            //{
            //    guino = gui.Num;
            //}
            //else
            //{
            //    guino = 0;
            //}

            return Json(new
            {
                gui
            });
        }
        //图片上传 

        [TypeFilter(typeof(IgonreActionFilter))]
        public ActionResult UpLoadImg(IFormFile file, string deviceid)
        {

            //if (Request.Form.Files.Count <= 0)
            //{
            //    return Content("请选择图片");
            //}
            //string name = Request.Query["address"];
            string imgPath = "\\UploadImg\\" + deviceid + "\\";
            string dicPath = _webHostEnvironment.WebRootPath + imgPath;
            if (!Directory.Exists(dicPath))
            {
                Directory.CreateDirectory(dicPath);
            }
            var img = file;
            if (img == null)
            {
                return Content("上传失败");
            }
            string ext = Path.GetExtension(img.FileName);
            //判断后缀是否是图片
            const string fileFilt = ".jpg|.jpeg|.png|";
            if (ext == null)
            {
                return Content("上传的文件没有后缀");
            }
            if (fileFilt.IndexOf(ext.ToLower(), StringComparison.Ordinal) <= -1)
            {
                return Content("上传的文件不是图片");
            }
            string fileName = Guid.NewGuid().ToString() + ext;
            string filePath = Path.Combine(dicPath, fileName);
            using (FileStream fs = System.IO.File.Create(filePath))
            {
                img.CopyTo(fs);
                fs.Flush();
            }
            return Content(imgPath + fileName);

        }
        //时间戳
        public static long ConvertDateTimeInt(System.DateTime time)
        {
            System.DateTime startTime = System.TimeZoneInfo.ConvertTimeToUtc(new System.DateTime(1970, 1, 1));
            return (long)(time - startTime).TotalMilliseconds;
        }

        //删除
        [HttpPost]
        public IActionResult DeleteList(string id)
        {
            List<long> detailids = new List<string>(id.Split(',')).ConvertAll(r => long.Parse(r));
            var deleteDetail = _sws_DeviceInfo01Service.Query<SwsDeviceInfo01>(r => detailids.Contains(r.DeviceId)).ToList();
            string deleteNameStr = "";
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = _sysUserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
            try
            {
                //_sws_DeviceInfo01Service.Delete(deleteDetail);
                if (_sws_DeviceInfo01Service.DeleteDevice(deleteDetail, ref deleteNameStr) > 0)
                {
                    if (_sws_DeviceInfo01Service.DelAreaInfo(id) > 0)
                    {
                        logger.LogInformation(user.NickName + " 删除设备-" + deleteNameStr.Substring(0, deleteNameStr.Length - 1));
                        return Content("ok");
                    }
                    else
                    {
                        return Content("no");
                    }
                }
                else
                {
                    return Content("no");
                }

            }
            catch (Exception e)
            {
                return Content("no");
            }
        }
        #endregion

        #region 选择泵房
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult StationInfo(int type)
        {
            ViewBag.type = type;
            return View();
        }

        [TypeFilter(typeof(IgonreActionFilter))]
        public ActionResult LoadStationList(int pageSize, int pageIndex, string sort, string sortOrder, string stationName, int type)
        {
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = _sysUserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
            Expression<Func<SwsStation, bool>> funcWhere = null;
            if (type != 0)
            {
                funcWhere = funcWhere.And(r => r.InType == type || r.InType == 0);
            }
            if (user.IsAdmin != true)
            {
                var id = _swsUserStationService.Query<SwsUserStation>(r => r.UserId == user.UserId).Select(r => r.StationId).ToList();
                funcWhere = funcWhere.And(r => id.Contains(r.StationId));
            }
            bool flag = true;
            //查询条件 
            if (!string.IsNullOrEmpty(stationName))
            {
                funcWhere = funcWhere.And(r => r.StationName.Contains(stationName) || r.StationNum.Contains(stationName));
            }
            if (sortOrder == "desc")
            {
                flag = false;
            }
            var infoList = _sws_StationService.QueryPage(funcWhere, pageSize, pageIndex, sort, flag);
            return Json(new { total = infoList.TotalCount, rows = Newtonsoft.Json.JsonConvert.SerializeObject(infoList.DataList.ToList()) });

        }
        #endregion

        #region 导入导出
        //导出泵站信息

        [TypeFilter(typeof(IgonreActionFilter))]
        public ActionResult ExportData(string deviceName, string sort = "DeviceId", string sortOrder = "desc")
        {
            #region 查询条件
            Expression<Func<WNMS.Model.CustomizedClass.DeviceInfo01Info, bool>> funcWhere = null;
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
            Expression<Func<WNMS.Model.CustomizedClass.DeviceInfo01Info, string>> funcOrderby = r => r.DeviceName;
            #endregion

            var list = this._sws_DeviceInfo01Service.LoadorderInfoList(funcWhere, 100000, 1, sort, "Partition", flag).DataList;


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
            dataRow.CreateCell(0).SetCellValue("泵房名称");
            dataRow.CreateCell(1).SetCellValue("设备名称");
            dataRow.CreateCell(2).SetCellValue("设备编号");
            dataRow.CreateCell(3).SetCellValue("分区");
            dataRow.CreateCell(4).SetCellValue("设备类型");
            dataRow.CreateCell(5).SetCellValue("变频分类");
            dataRow.CreateCell(6).SetCellValue("厂商");
            dataRow.CreateCell(7).SetCellValue("泵数量");
            dataRow.CreateCell(8).SetCellValue("泵类型");
            dataRow.CreateCell(9).SetCellValue("进口DN");
            dataRow.CreateCell(10).SetCellValue("出口DN");
            dataRow.CreateCell(11).SetCellValue("出厂日期");

            for (int i = 0; i < 12; i++)
            {
                sheet.SetColumnWidth(i, 30 * 256);
            }
            for (int s = 0; s < 12; s++)
            {
                sheet.SetColumnWidth(s, 25 * 256);
                dataRow.Cells[s].CellStyle = style1;
            }
            int j = 1;
            foreach (var item in list)
            {
                dataRow = sheet.CreateRow(j);
                dataRow.Height = 100 * 4;
                dataRow.CreateCell(0).SetCellValue(item.StationName?.ToString());
                dataRow.CreateCell(1).SetCellValue(item.DeviceName?.ToString());
                dataRow.CreateCell(2).SetCellValue(item.DeviceNum?.ToString());
                dataRow.CreateCell(3).SetCellValue(item.PartitionName?.ToString());
                dataRow.CreateCell(4).SetCellValue(item.DeviceTypeName?.ToString());
                dataRow.CreateCell(5).SetCellValue(item.FrequencyName?.ToString());
                dataRow.CreateCell(6).SetCellValue(item.ManufacturerName?.ToString());
                dataRow.CreateCell(7).SetCellValue(item.PumpNum.ToString());
                dataRow.CreateCell(8).SetCellValue(item.PumpType.ToString());
                dataRow.CreateCell(9).SetCellValue(item.ImportDn?.ToString());
                dataRow.CreateCell(10).SetCellValue(item.ExportDn?.ToString());
                dataRow.CreateCell(11).SetCellValue(item.ManufactureDate?.ToString());
                j++;
            }

            for (int ro = 1; ro < list.Count + 1; ro++)
            {
                sheet.GetRow(ro).Height = 20 * 20;
                for (int s = 0; s < 12; s++)
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
            List<SwsDeviceInfo01> list = new List<SwsDeviceInfo01> { };
            //设备编号
            List<string> DeviceNums = new List<string>() { };
            //泵房id
            List<int> stationid = new List<int>() { };
            List<StationPartion> keyValuePairs = new List<StationPartion>();
            StationPartion stationPartion = new StationPartion();

            MemoryStream ms = new MemoryStream();
            excelfile.CopyTo(ms);
            ms.Seek(0, SeekOrigin.Begin);
            List<int> ids = new List<int> { 5, 6, 7, 8 };
            //所有类型
            var allType = _sys_DataItemDetailServices.Query<SysDataItemDetail>(r => ids.Contains(r.FItemId)).ToList();
            Random random = new Random();
            int raInt = random.Next(1, 10000);
            long startId = ConvertDateTimeInt(DateTime.Now) + raInt;
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
                SwsDeviceInfo01 obj = new SwsDeviceInfo01();
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
                    var bp = row.GetCell(6)?.ToString();
                    if (bp == "单变频")
                    {
                        obj.Frequency = 1;
                    }
                    else
                    {
                        obj.Frequency = 2;
                    }
                    var par = row.GetCell(7)?.ToString();
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
                    stationPartion.stationid = obj.StationId;
                    stationPartion.partion = obj.Partition;
                    keyValuePairs.Add(stationPartion);
                    obj.PumpNum = int.Parse(row.GetCell(8)?.ToString());
                    obj.PumpType = row.GetCell(9)?.ToString();
                    obj.ImportDn = row.GetCell(10)?.ToString();
                    obj.ExportDn = row.GetCell(11)?.ToString();
                    obj.ManufactureDate = DateTime.Now;
                    var strManufacturer = row.GetCell(12)?.ToString();
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
                var hasDevice = _sws_DeviceInfo01Service.Query<SwsDeviceInfo01>(r => DeviceNums.Contains(r.DeviceNum)).Select(r => r.DeviceNum).ToList();
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
                var stationlist = _sws_StationService.Query<SwsStation>(r => stationid.Contains(r.StationId) && r.InType != 1 && r.InType != 0).ToList();
                if (stationlist.Count > 0)
                {
                    ms.Dispose();
                    var hasDevice = string.Join(',', stationlist.Select(r => r.StationName).ToList());
                    return Content("文件中的泵房类型存在非二供泵房," + hasDevice + "");
                }
            }
            #endregion

            #region  判断泵房下是否有重复的分区
            List<IGrouping<int, StationPartion>> group = keyValuePairs.GroupBy(r => r.stationid).ToList();
            if (group.Count > 0)
            {
                foreach (var item in group)
                {
                    List<byte> partion = _sws_DeviceInfo01Service.Query<SwsDeviceInfo01>(r => r.StationId == item.Key)?.Select(r => r.Partition).ToList();
                    List<int> part = item.Select(r => r.partion).ToList();
                    var count = partion.Count() + part.Count();
                    var countnew = partion.Distinct().Count() + part.Distinct().Count();
                    if (count != countnew && partion.Count != 0)
                    {
                        ms.Dispose();
                        return Content("泵房ID为" + item.Key + "的泵房下存在相同的分区，请重新编辑");
                    }
                }
            }
            #endregion

            ms.Dispose();
            if (_sws_DeviceInfo01Service.DeviceImport(list) > 0)
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

        #region 阀门配置
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult AllotValve(long id)
        {
            ViewBag.DeviceID = id;
            return View();
        }

        //获取阀门数据列表
        [TypeFilter(typeof(IgonreActionFilter))]
        [HttpPost]
        public async Task<IActionResult> GetValueData(long deviceID)
        {
            List<SwsValveWith01> valve = new List<SwsValveWith01>();
            valve = valveService.Query<SwsValveWith01>(r => r.DeviceId == deviceID).ToList();
            PartialView("_ValveTable", valve);
            string dataTable = await ViewToString.RenderPartialViewToString(this, "_ValveTable");
            return Json(new
            {
                dataTable = dataTable
            });
        }

        //获取表单数据
        [TypeFilter(typeof(IgonreActionFilter))]
        [HttpPost]
        public async Task<IActionResult> EditValve(long deviceID, int vID)
        {
            SwsValveWith01 valve = new SwsValveWith01();
            if (vID != 0)
            {
                valve = valveService.Query<SwsValveWith01>(r => r.ValveId == vID && r.DeviceId == deviceID).FirstOrDefault();
            }
            else
            {
                valve.DeviceId = deviceID;
            }
            //ViewBag.EquipmentType = rtuservice.LoadEntities(r => r.EquipmentID == equID).FirstOrDefault().EquipmentType;
            //ViewBag.hardlist = sws_RtuHardInfoService.LoadEntities(r => r.EquipmentID == equID);
            PartialView("_ValveForm", valve);
            string forms = await ViewToString.RenderPartialViewToString(this, "_ValveForm");
            return Json(new
            {
                Forms = forms
            });
        }
        //提交
        [TypeFilter(typeof(IgonreActionFilter))]
        public ActionResult SetValve(SwsValveWith01 val, long id_Valve)
        {
            if (id_Valve == 0)
            {
                val.ValveId = ConvertDateTime(DateTime.Now);
                if (valveService.Query<SwsValveWith01>(r => r.DeviceId == val.DeviceId && r.ValveNum == val.ValveNum).ToList().Count() > 0)
                {
                    return Content("false");
                }
                else
                {
                    if (valveService.Insert(val) != null)
                    {
                        return Content("ok");
                    }
                    else
                    {
                        return Content("no");
                    }
                }
            }
            else
            {
                if (valveService.Query<SwsValveWith01>(r => r.ValveNum == val.ValveNum && r.DeviceId == val.DeviceId && r.ValveId != val.ValveId).ToList().Count() > 0)
                {
                    return Content("false");
                }
                else
                {
                    if (valveService.Update<SwsValveWith01>(val))
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

        //删除阀门
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult DeleteValve(string ids, long deviceID)
        {
            string[] valIDs = ids.Split(',');
            List<long> valveIDList = new List<long>();
            foreach (var item in valIDs)
            {
                valveIDList.Add(long.Parse(item));
            }
            IEnumerable<SwsValveWith01> ValveList = new List<SwsValveWith01>();
            ValveList = valveService.Query<SwsValveWith01>(r => valveIDList.Contains(r.ValveId) && r.DeviceId == deviceID).ToList();
            if (valveService.Delete(ValveList))
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }
        }

        //时间戳
        public static int ConvertDateTime(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (int)(time - startTime).TotalSeconds;
        }
        #endregion

    }
    public class StationPartion
    {
        public int stationid { get; set; }
        public int partion { get; set; }
    }
}