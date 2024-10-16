using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
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
    public class Sws_Device02RunLogController : Controller
    {
        public  readonly ISws_DeviceInfo02Service _sws_DeviceInfo02Service=null;
        private readonly ISws_StationService _sws_StationService;
        private readonly ISws_DataInfoService _sws_DataInfoService;
        private readonly ISws_TemplateService _sws_TemplateService;
        private readonly ISysUserService _sysUserService;
        public ISysUserService _ISys_UserService = null;
        public ISws_UserStationService _sws_UserStationService = null;

        public Sws_Device02RunLogController(ISws_DeviceInfo02Service sws_DeviceInfo02Service,
            ISws_StationService sws_StationService,
            ISws_DataInfoService sws_DataInfoService,
            ISws_TemplateService sws_TemplateService,
            ISysUserService sysUserService,
            ISysUserService _userservice,
            ISws_UserStationService sws_UserStationService)
        {
            _sws_DeviceInfo02Service = sws_DeviceInfo02Service;
            _sws_StationService = sws_StationService;
            _sws_DataInfoService = sws_DataInfoService;
            _sws_TemplateService = sws_TemplateService;
            _sysUserService = sysUserService;
            _ISys_UserService = _userservice;
            _sws_UserStationService = sws_UserStationService;
        }
        #region
        public IActionResult Index()
        {
            //初始化时间
            ViewBag.beginDate = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now.AddDays(-1));
            ViewBag.endDate = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now);
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
            //模板信息
            if (user.IsAdmin == true)
            {
                var tempList = _sws_TemplateService.Query<SwsTemplate>(r => r.DeviceType == 2).ToList();
                ViewBag.tempList = tempList;
            }
            else
            {
                var tempList = _sws_TemplateService.Query<SwsTemplate>(r => r.UserId == user.UserId && r.DeviceType == 2).OrderByDescending(r => r.FocusOn).ToList();
                ViewBag.tempList = tempList;
            }

            return View();
        }

        //泵房树形
        public  string TreesOfStationDevice(SysUser sysUser, string stationName)
        {
            List<StationAndDevice> treelist = _sws_DeviceInfo02Service.QueryZtreeInfo(sysUser, stationName).ToList();

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
        }
        //树形查询
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult SelectTree(string stationName)
        {
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var sysUser = _sysUserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();

            var json = TreesOfStationDevice(sysUser, stationName);
            var str = new HtmlString(json);
            return Content(str.ToString());
        }

        //分部视图  查询历史数据
        [TypeFilter(typeof(IgonreActionFilter))]
        public async Task<IActionResult> GetHistoryData(string beginDate, string endDate, string equID, int template, int pageSize = 20, int pageIndex = 1, string order = "UpdateTime", string sort = "")
        {
            //定义变量
            string year = Convert.ToDateTime(beginDate).Year.ToString();
            int totalCount = 0;
            long equipmentID = Convert.ToInt64(equID);
            if (string.IsNullOrEmpty(order))
            {
                order = "UpdateTime";
            }
            //获取设备信息
            var deviceInfo = _sws_DeviceInfo02Service.Find<SwsDeviceInfo02>(equipmentID);
            //获取需要查询的字段 模板信息 
            var swsdataInfoList = GetTempByID(template.ToString(), 2, deviceInfo.Partition);

            List<HistoryJKData> dataList = new List<HistoryJKData>();
            if (deviceInfo.Rtuid != null)
            {
                 dataList = _sws_DeviceInfo02Service.GetMongoHistoryData(year, deviceInfo.Rtuid.ToString(), beginDate, endDate, order, sort, pageIndex, pageSize, ref totalCount).ToList();
            }            
            ViewBag.DataInfo = swsdataInfoList;
            PartialView("_DeviceJKHistoryTable", dataList);
            string dataTable = await ViewToString.RenderPartialViewToString(this, "_DeviceJKHistoryTable");
            return Json(new
            {
                total = totalCount,
                pageIndex = pageIndex,
                pageSize = pageSize,
                order = sort,
                sortName = "UpdateTime",
                dataTable = dataTable
            });
        }
        #endregion

        #region 用户模板信息
        List<SwsDataInfo> GetTempByID(string id, int type, int partion)
        {
            List<SwsDataInfo> enddatalist = new List<SwsDataInfo>();
            List<SwsDataInfo> datalist = new List<SwsDataInfo>();
            var tid = int.Parse(id);
            var temp = _sws_TemplateService.Query<SwsTemplate>(t => t.Id == tid && t.DeviceType == type).FirstOrDefault();
            if (temp == null)
            {
                temp = _sws_TemplateService.Query<SwsTemplate>(t => t.DeviceType == type).FirstOrDefault();
            }
            List<int> DataId = new List<int>();
            if (temp != null)
            {
                var Datastring = temp.DataId.Split(',');
                short enditem = 0;
                foreach (var item in Datastring)
                {
                    switch (partion)
                    {
                        case 2:
                            enditem = (short)(short.Parse(item) + 500);
                            break;
                        case 3:
                            enditem = (short)(short.Parse(item) + 1000);
                            break;
                        case 4:
                            enditem = (short)(short.Parse(item) + 1500);
                            break;
                        case 5:
                            enditem = (short)(short.Parse(item) + 2000);
                            break;
                        default:
                            enditem = short.Parse(item);
                            break;
                    }
                    DataId.Add(enditem);
                }
            }
            datalist = _sws_DataInfoService.Query<SwsDataInfo>(r => DataId.Contains(r.DataId) && r.DeviceType == type && r.IsShow == true).ToList();
            return datalist;
        }
        #endregion

        #region 矫正数据
        #region 矫正页面加载
        [TypeFilter(typeof(IgonreActionFilter))]
        public ActionResult QueryDataIDInfo(string EquipmentId, int template)
        {
            List<SwsDataInfo> data = new List<SwsDataInfo>();
            long equID = Convert.ToInt64(EquipmentId);
            //获取设备信息
            var deviceInfo = _sws_DeviceInfo02Service.Find<SwsDeviceInfo02>(equID);
            //获取模板信息
            List<SwsDataInfo> dataInfo = GetTempByID(template.ToString(), 2, deviceInfo.Partition).Where(r => r.DataType == 1).ToList();
            ViewBag.datemin = string.Format("{0:yyyy-MM-dd 00:00:00}", DateTime.Now);
            ViewBag.datemax = string.Format("{0:yyyy-MM-dd HH:MM:ss}", DateTime.Now);
            ViewBag.Sws_DataInfo = dataInfo;
            ViewBag.EquipmentId = EquipmentId;
            return View("FilterPage");
        }
        #endregion
        [TypeFilter(typeof(IgonreActionFilter))]
        public async Task<IActionResult> SetData(string BeginDate, string EndDate, string EquipmentId, string DataID)
        {
            DateTime dateTime = Convert.ToDateTime(BeginDate);
            string year = dateTime.Year.ToString();
            long Equipment = long.Parse(EquipmentId);
            //获取设备信息
            var deviceInfo = _sws_DeviceInfo02Service.Find<SwsDeviceInfo02>(Equipment);

            long DataIDint = long.Parse(DataID);

            SwsDataInfo dataInfo = _sws_DataInfoService.Query<SwsDataInfo>(r => r.DataId == DataIDint && r.DeviceType == 2).FirstOrDefault();
            ViewBag.DataInfo = dataInfo;
            var rTUDtails = _sws_DeviceInfo02Service.GetCorrectData(year, deviceInfo.Rtuid.ToString(), BeginDate, EndDate, DataID).ToList();
            ViewBag.EquipmentId = EquipmentId;
            PartialView("_DHTable", rTUDtails);
            string dataTable = await ViewToString.RenderPartialViewToString(this, "_DHTable");
            return Json(new
            {
                dataTable = dataTable
            });
        }
        #region 点击矫正
        [TypeFilter(typeof(IgonreActionFilter))]
        public ActionResult Update(string Time, string id, string Value, string EquipmentID, string DataID)
        {
            long Equipment = long.Parse(EquipmentID);
            //获取设备信息
            var deviceInfo = _sws_DeviceInfo02Service.Find<SwsDeviceInfo02>(Equipment);
            ViewBag.id = id;
            ViewBag.Value = Value;
            ViewBag.EquipmentID = deviceInfo.Rtuid;
            ViewBag.DataID = DataID;
            ViewBag.Time = Time;
            return View();
        }
        #region 更新矫正数据
        [TypeFilter(typeof(IgonreActionFilter))]
        public ActionResult UpdateData(string Time, string Value, string EquipmentID, string DataID, string id)
        {
            //long equID = long.Parse(EquipmentID); 
            ////获取设备信息
            //var deviceInfo = _sws_DeviceInfo02Service.Find<SwsDeviceInfo02>(equID);
            double value = double.Parse(Value);
            string year = Convert.ToDateTime(Time).Year.ToString();
            string dataName = "AnalogValues." + DataID;
            long number = _sws_DeviceInfo02Service.UpdateCorrectData(year, EquipmentID, id, dataName, value);
            if (number == 1)
            {
                return Content("ok");
            }
            return Content("no");
        }
        #endregion
        #endregion
        #endregion

        #region 模板配置
        ////配置模板按钮点击，加载配置模板页面
        //public ActionResult SetTemplate(string type)
        //{
        //    int userID = int.Parse(User.FindFirstValue("UserID"));
        //    var user = _sysUserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();

        //    int types = Convert.ToInt32(type);
        //    List<long> dataid = new List<long>() { 1000, 1001 };

        //    //根据用户信息获取用户模板
        //    var temInfoList = _sws_TemplateService.Query<SwsTemplate>(r => r.UserId == user.UserId).OrderByDescending(r => r.FocusOn).ToList();
        //    ViewBag.Template = temInfoList;
        //    var mo = _sws_TemplateService.Query<SwsTemplate>(r => r.UserId == user.UserId && r.FocusOn == true).FirstOrDefault();
        //    if (mo != null)
        //    {
        //        ViewBag.tempid = mo.Id;
        //    }
        //    else
        //    {
        //        //ViewBag.tempid = temInfoList.FirstOrDefault().Id;
        //        ViewBag.tempid = 0;
        //    }

        //    //获取设备类型
        //    //var EquType = service_Type.LoadEntities(s => true).ToList();
        //    //ViewBag.EquType = EquType;
        //    ViewBag.TypeID = type;
        //    return View();
        //}
        //#region  查询模板内容信息
        //public IActionResult LoadTempData(int id)
        //{
        //    if (id == 0)
        //    {
        //        return Content("");
        //    }
        //    var tempInfo = _sws_TemplateService.Query<SwsTemplate>(r => r.Id == id).FirstOrDefault();
        //    string dataid = tempInfo.DataId;
        //    string[] listdata = dataid.Split(',');
        //    //查询已存在的datdid
        //    List<int> ids = new List<int>();
        //    foreach (var item in listdata)
        //    {
        //        ids.Add(int.Parse(item));
        //    }
        //    List<SwsDataInfo> endhasData = new List<SwsDataInfo>();
        //    List<SwsDataInfo> nohasData = new List<SwsDataInfo>();
        //    //已存在的
        //    var hasData = _sws_DataInfoService.Query<SwsDataInfo>(r => ids.Contains(r.DataId) && r.DataId < 2500).ToList();
        //    foreach (var item in hasData)
        //    {
        //        item.Cnname = item.Cnname.Substring(2, item.Cnname.Length - 2);
        //        if (endhasData.Where(r => r.Cnname == item.Cnname).Count() > 0)
        //        {

        //        }
        //        else
        //        {
        //            endhasData.Add(item);
        //        }

        //    }
        //    var nodata = _sws_DataInfoService.Query<SwsDataInfo>(r => !ids.Contains(r.DataId) && r.DataId < 2500).ToList();
        //    foreach (var item in nodata)
        //    {
        //        item.Cnname = item.Cnname.Substring(2, item.Cnname.Length - 2);
        //        if (nohasData.Where(r => r.Cnname == item.Cnname).Count() > 0)
        //        {

        //        }
        //        else
        //        {
        //            nohasData.Add(item);
        //        }

        //    }
        //    var rel = new
        //    {
        //        endhasData,
        //        nohasData
        //    };
        //    return Content(Newtonsoft.Json.JsonConvert.SerializeObject(rel));
        //}
        ////修改模板信息
        //public IActionResult UpdateTep(string dataArr, int tempid)
        //{
        //    var tempInfo = _sws_TemplateService.Query<SwsTemplate>(r => r.Id == tempid).FirstOrDefault();
        //    if (dataArr.Length > 0)
        //    {
        //        tempInfo.DataId = dataArr;
        //    }
        //    if (_sws_TemplateService.Update<SwsTemplate>(tempInfo))
        //    {
        //        return Content("ok");
        //    }
        //    else
        //    {
        //        return Content("no");
        //    }
        //}
        ////修改关注
        //public IActionResult UpdateFocusOn(int id, bool focusOn)
        //{
        //    var tempInfo = _sws_TemplateService.Query<SwsTemplate>(r => r.Id == id).FirstOrDefault();
        //    int userID = int.Parse(User.FindFirstValue("UserID"));
        //    var user = _sysUserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
        //    if (_sws_DeviceInfo02Service.UpdateFocusOn(id, focusOn, userID) > 0)
        //    {
        //        return Content("ok");
        //    }
        //    else
        //    {
        //        return Content("no");
        //    }

        //}
        ////加载模板数据
        //public IActionResult LoadTemp()
        //{
        //    int userID = int.Parse(User.FindFirstValue("UserID"));
        //    var user = _sysUserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
        //    //根据用户信息获取用户模板
        //    var temInfoList = _sws_TemplateService.Query<SwsTemplate>(r => r.UserId == user.UserId).OrderByDescending(r => r.FocusOn).ToList();
        //    return Content(Newtonsoft.Json.JsonConvert.SerializeObject(temInfoList));
        //}
        //#endregion
        ////提交表单信息
        //public ActionResult TemplateIndex(string templatename, string ids, int flag, string type)
        //{
        //    int userID = int.Parse(User.FindFirstValue("UserID"));
        //    var user = _sysUserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
        //    SwsTemplate tempinfo = new SwsTemplate();
        //    tempinfo.TemplateName = templatename;
        //    tempinfo.DataId = ids;
        //    tempinfo.UserId = user.UserId;
        //    tempinfo.Classify = 1;
        //    tempinfo.DeviceType = Convert.ToByte(type);
        //    if (flag == 0)
        //    {
        //        tempinfo.Id = _ISys_UserService.QueryID("ID", "Sws_Template");
        //        if (_sws_TemplateService.Insert(tempinfo) != null)
        //        {
        //            return Content("ok");
        //        }
        //        else
        //        {
        //            return Content("no");
        //        }
        //    }
        //    else
        //    {
        //        tempinfo.Id = flag;
        //        if (_sws_TemplateService.Update(tempinfo))
        //        {
        //            return Content("ok");
        //        }
        //        else
        //        {
        //            return Content("no");
        //        }
        //    }
        //}
        ////修改模板
        //[HttpPost]
        //public ActionResult edittemplate(string id)
        //{
        //    var idint = Convert.ToInt32(id);
        //    var templates = _sws_TemplateService.Query<SwsTemplate>(r => r.Id == idint).FirstOrDefault();
        //    var dataid = templates.DataId;
        //    string[] listdata = dataid.Split(',');
        //    return Content(Newtonsoft.Json.JsonConvert.SerializeObject(listdata));
        //}
        ////刷新模板集合
        //[HttpPost]
        //public ActionResult datarefresh(string type)
        //{
        //    int userID = int.Parse(User.FindFirstValue("UserID"));
        //    var user = _sysUserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
        //    int types = Convert.ToInt32(type);
        //    var templates = _sws_TemplateService.Query<SwsTemplate>(t => t.UserId == user.UserId & t.Classify == 1 && t.DeviceType == types).ToList();
        //    var datacount = templates.Count();
        //    var rel = new
        //    {
        //        Templates = templates,
        //        Datacount = datacount
        //    };
        //    return Content(Newtonsoft.Json.JsonConvert.SerializeObject(rel));

        //}
        ////删除模板
        //[HttpPost]
        //public ActionResult Deletetemp(int id)
        //{
        //    var templates = _sws_TemplateService.Query<SwsTemplate>(t => t.Id == id).FirstOrDefault();
        //    if (_sws_TemplateService.Delete(templates))
        //    {
        //        return Content("ok");
        //    }
        //    else
        //    {
        //        return Content("no");
        //    }
        //}

        #endregion

        #region 导出历史数据
        [TypeFilter(typeof(IgonreActionFilter))]
        public ActionResult ExportHistoryData(string equID, string beginDate, string endDate, int templateID)
        {

            //变量处理
            long equipmentID = Convert.ToInt64(equID);
            string year = Convert.ToDateTime(beginDate).Year.ToString();

            //获取设备类型（Equipmenttype）
            var rtu = _sws_DeviceInfo02Service.Query<SwsDeviceInfo02>(r => r.DeviceId == equipmentID).FirstOrDefault();
            int type = 2;
            string name = "";
            if (rtu != null)
            {
                name = rtu.DeviceName;
            }
            //获取模板信息
            List<SwsDataInfo> dataInfo = GetTempByID(templateID.ToString(), type, rtu.Partition);

            List<HistoryJKData> list = new List<HistoryJKData>();
            //获取历史数据信息
            if (rtu.Rtuid != null)
            {
                list = _sws_DeviceInfo02Service.GetExportHistoryData(year, rtu.Rtuid.ToString(), beginDate, endDate).ToList();
            }
            

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
            dataRow.CreateCell(0).SetCellValue("更新时间");
            dataRow.Height = 20 * 20;
            int i = 1;
            foreach (var item in dataInfo)
            {
                dataRow.CreateCell(i).SetCellValue(item.Cnname);
                i++;
            }
            for (int s = 0; s < dataInfo.Count + 1; s++)
            {
                sheet.SetColumnWidth(s, 25 * 256);
                dataRow.Cells[s].CellStyle = style1;
            }
            //填充内容
            for (int j = 0; j < list.Count(); j++)
            {
                dataRow = sheet.CreateRow(j + 1);
                dataRow.CreateCell(0).SetCellValue(list[j].UpdateTime.ToString());
                int k = 1;
                foreach (var item in dataInfo)
                {
                    if (item.DataType == 2)
                    {
                        if (list[j].DigitalValues.Keys.Contains(item.DataId.ToString()))
                        {
                            dataRow.CreateCell(k).SetCellValue(list[j].DigitalValues[item.DataId.ToString()].ToString());
                        }
                        else
                        {
                            dataRow.CreateCell(k).SetCellValue("--");
                        }

                    }
                    else
                    {
                        if (list[j].AnalogValues.Keys.Contains(item.DataId.ToString()))
                        {
                            dataRow.CreateCell(k).SetCellValue(list[j].AnalogValues[item.DataId.ToString()].ToString());
                        }
                        else
                        {
                            dataRow.CreateCell(k).SetCellValue("--");
                        }

                    }
                    k++;
                }
            }
            for (int ro = 1; ro < list.Count + 1; ro++)
            {
                sheet.GetRow(ro).Height = 20 * 20;
                for (int s = 0; s < dataInfo.Count + 1; s++)
                {
                    sheet.GetRow(ro).Cells[s].CellStyle = style;
                }
            }
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            workbook.Write(ms);
            workbook = null;
            ms.Close();
            ms.Dispose();
            return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", name + "历史数据" + beginDate.Substring(0, 10) + ".xls");
        }
        #endregion



    }
}