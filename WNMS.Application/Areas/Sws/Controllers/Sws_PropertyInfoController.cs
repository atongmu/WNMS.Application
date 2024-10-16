using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using WNMS.Application.Utility.Filters;
using WNMS.IService;
using WNMS.Model.DataModels;
using WNMS.Utility;

namespace WNMS.Application.Areas.Sws.Controllers
{
    [Area("Sws")]
    public class Sws_PropertyInfoController : Controller
    {
        private ISysUserService _ISys_UserService = null;
        private ISws_PropertyInfoService _PropertyInfoService = null;
        private ISys_DataItemDetailService _DataItemDetailService = null;
        private ISws_StationService _StationService = null;
        private ISws_UserStationService _UserStationService = null;
        public Sws_PropertyInfoController(ISysUserService _userservice,
            ISws_PropertyInfoService sws_PropertyInfoService,
            ISys_DataItemDetailService sys_DataItemDetailService,
            ISws_StationService sws_StationService,
            ISws_UserStationService sws_UserStationService) {
            _ISys_UserService = _userservice;
            _PropertyInfoService = sws_PropertyInfoService;
            _DataItemDetailService = sys_DataItemDetailService;
            _StationService = sws_StationService;
            _UserStationService = sws_UserStationService;
        }


        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult QueryPropertyTable(int pagesize, int pageindex, string SearchText, string order, string sort, string time, string beginDate, string endDate)
        {
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = _ISys_UserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();

            int Totalcount = 0;
            string ordertems = sort + " " + order;
            string filter = "1=1";

            var propertyid = (int)Model.CustomizedClass.Enum.资产类型;
            
            if (SearchText != null)
            {
                filter += " and (Name like '%" + SearchText + "%' or StationName like '%" + SearchText + "%')";
            }

            if (!string.IsNullOrEmpty(time))
            {
                GetBeginDate(time, ref beginDate, ref endDate);
                if (!string.IsNullOrEmpty(beginDate) || !string.IsNullOrEmpty(endDate))
                {

                    if (!string.IsNullOrEmpty(beginDate))
                    {
                        filter += " and  BuyDate>='" + beginDate + "'";
                    }
                    if (!string.IsNullOrEmpty(endDate))
                    {
                        filter += " and  BuyDate<'" + endDate + "'";
                    }
                }

            }
            if (user.IsAdmin == false)//非管理员
            {
                filter += "and UserID= " + userID + "";
            }
            
            var datalist = _PropertyInfoService.QueryPropertyTable(user.IsAdmin, pageindex, pagesize, propertyid, ordertems, filter, ref Totalcount);
             return Json(new { total = Totalcount, rows =Newtonsoft.Json.JsonConvert.SerializeObject(datalist) });
        }
        //获取查询开始日期
        void GetBeginDate(string executeTime, ref string BeginTime, ref string EndTime)
        {
            switch (executeTime)
            {
                case "昨天":
                    BeginTime = string.Format("{0:yyyy-MM-dd}", DateTime.Now.AddDays(-1));
                    EndTime = string.Format("{0:yyyy-MM-dd}", DateTime.Now);
                    break;
                case "上周":
                    var dayofWeek = (int)DateTime.Now.DayOfWeek == 0 ? 7 : (int)DateTime.Now.DayOfWeek;
                    BeginTime = string.Format("{0:yyyy-MM-dd}", DateTime.Now.AddDays(-(dayofWeek - 1) - 7));
                    EndTime = string.Format("{0:yyyy-MM-dd}", DateTime.Now.AddDays(-(dayofWeek - 1)));
                    break;
                case "本周":
                    var dayofWeek1 = (int)DateTime.Now.DayOfWeek == 0 ? 7 : (int)DateTime.Now.DayOfWeek;
                    BeginTime = string.Format("{0:yyyy-MM-dd}", DateTime.Now.AddDays(-(dayofWeek1 - 1)));
                    EndTime = string.Format("{0:yyyy-MM-dd}", DateTime.Now.AddDays(-(dayofWeek1 - 1) + 7));
                    break;
                case "下周":
                    var dayofWeek2 = (int)DateTime.Now.DayOfWeek == 0 ? 7 : (int)DateTime.Now.DayOfWeek;
                    BeginTime = string.Format("{0:yyyy-MM-dd}", DateTime.Now.AddDays(-(dayofWeek2 - 1) + 7));
                    EndTime = string.Format("{0:yyyy-MM-dd}", DateTime.Now.AddDays(-(dayofWeek2 - 1) + 14));
                    break;
                case "上月":
                    BeginTime = string.Format("{0:yyyy-MM}", DateTime.Now.AddMonths(-1)) + "-01";
                    EndTime = string.Format("{0:yyyy-MM}", DateTime.Now) + "-01";
                    break;
                case "本月":
                    BeginTime = string.Format("{0:yyyy-MM}", DateTime.Now) + "-01";
                    EndTime = string.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(string.Format("{0:yyyy-MM-01}", DateTime.Now.AddMonths(1))));
                    break;
                case "下月":
                    BeginTime = string.Format("{0:yyyy-MM}", DateTime.Now.AddMonths(1)) + "-01";
                    EndTime = string.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(string.Format("{0:yyyy-MM-01}", DateTime.Now.AddMonths(2))));
                    break;
                case "自定义":
                    if (EndTime != null)
                    {
                        EndTime = string.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(EndTime).AddDays(1));
                    }
                    break;
                default:
                    break;

            }
        }
        #region 页面操作
        //添加
        public IActionResult AddPropertyPage()
        {
            var pf_itemid = (int)Model.CustomizedClass.Enum.资产类型;
            var typelist = _DataItemDetailService.Query<SysDataItemDetail>(r => r.FItemId == pf_itemid && r.IsEnable == true);
            ViewBag.typelist = typelist;

            return View("_SetPropertyInfo",new SwsPropertyInfo());
        }
        //修改
        public IActionResult EditePropertyPage(int id)
        {
            var pf_itemid = (int)Model.CustomizedClass.Enum.资产类型;
            var typelist = _DataItemDetailService.Query<SysDataItemDetail>(r => r.FItemId == pf_itemid && r.IsEnable == true);
            ViewBag.typelist = typelist;
            var pmodel = _PropertyInfoService.Query<SwsPropertyInfo>(r => r.PropertyId == id).FirstOrDefault();
            var stationmodel = _StationService.Query<SwsStation>(r => r.StationId == pmodel.StationId).FirstOrDefault();
            ViewBag.StationName = stationmodel?.StationName;
            return View("_SetPropertyInfo", pmodel);

        }
        //删除
        [HttpPost]
        public IActionResult DeletePropertys(string id)
        {
            List<int> propertyids = new List<string>(id.Split(',')).ConvertAll(r => int.Parse(r));
            var plist = _PropertyInfoService.Query<SwsPropertyInfo>(r => propertyids.Contains(r.PropertyId));
            try
            {
                _PropertyInfoService.Delete<SwsPropertyInfo>(plist);
                return Content("ok");
            }
            catch (Exception e)
            {
                return Content("no");
            }
        }
        //提交表单
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult SetPropertyInfo(SwsPropertyInfo p)
        {
            if (p.PropertyId == 0)//添加
            {
                p.PropertyId= _ISys_UserService.QueryID("PropertyID", "Sws_PropertyInfo");
                try
                {
                    _PropertyInfoService.Insert<SwsPropertyInfo>(p);
                    return Content("ok");
                }
                catch (Exception e) {
                    return Content("no");
                }
            }
            else
            {
                try
                {
                    _PropertyInfoService.Update<SwsPropertyInfo>(p);
                    return Content("ok");
                }
                catch (Exception e)
                {
                    return Content("no");
                }
            }
        }
        #endregion
        #region 导入导出
        //导出表格
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult PropertyTableExport(string SearchText, string order, string sort, string time, string beginDate, string endDate)
        {
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = _ISys_UserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();

            int Totalcount = 0;
            string ordertems = sort + " " + order;
            string filter = "1=1";

            var propertyid = (int)Model.CustomizedClass.Enum.资产类型;

            if (SearchText != null)
            {
                filter += " and (Name like '%" + SearchText + "%' or StationName like '%" + SearchText + "%')";
            }

            if (!string.IsNullOrEmpty(time))
            {
                GetBeginDate(time, ref beginDate, ref endDate);
                if (!string.IsNullOrEmpty(beginDate) || !string.IsNullOrEmpty(endDate))
                {

                    if (!string.IsNullOrEmpty(beginDate))
                    {
                        filter += " and  BuyDate>='" + beginDate + "'";
                    }
                    if (!string.IsNullOrEmpty(endDate))
                    {
                        filter += " and  BuyDate<'" + endDate + "'";
                    }
                }

            }
            if (user.IsAdmin == false)//非管理员
            {
                filter += "and UserID= " + userID + "";
            }

            var list = _PropertyInfoService.QueryPropertyTable(user.IsAdmin, 0, 0, propertyid, ordertems, filter, ref Totalcount);
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
            dataRow.CreateCell(0).SetCellValue("资产名称");
            dataRow.CreateCell(1).SetCellValue("资产类型");
            dataRow.CreateCell(2).SetCellValue("泵房类型");
            dataRow.CreateCell(3).SetCellValue("品牌");
            dataRow.CreateCell(4).SetCellValue("购买日期");
            dataRow.CreateCell(5).SetCellValue("购买金额(￥)");

            dataRow.CreateCell(6).SetCellValue("保管人");
            dataRow.CreateCell(7).SetCellValue("质保期");
            
            for (int i = 0; i < 8; i++)
            {
                sheet.SetColumnWidth(i, 30 * 256);
            }
            for (int s = 0; s < 8; s++)
            {
                sheet.SetColumnWidth(s, 25 * 256);
                dataRow.Cells[s].CellStyle = style1;
            }
            int j = 1;
            foreach (var item in list)
            {
                dataRow = sheet.CreateRow(j);
                dataRow.Height = 100 * 4;
                dataRow.CreateCell(0).SetCellValue(item.Name?.ToString());
                dataRow.CreateCell(1).SetCellValue(item.PropertyTypeName?.ToString());
                dataRow.CreateCell(2).SetCellValue(item.StationName?.ToString());
                dataRow.CreateCell(3).SetCellValue(item.BrandName?.ToString());
                dataRow.CreateCell(4).SetCellValue(item.BuyDate?.ToString());
                dataRow.CreateCell(5).SetCellValue(item.BuyMonery?.ToString());

                dataRow.CreateCell(6).SetCellValue(item.Custodian?.ToString());
                
                dataRow.CreateCell(7).SetCellValue(item.WarrantyPeriod?.ToString());
                
                j++;
            }

            for (int ro = 1; ro < Totalcount + 1; ro++)
            {
                sheet.GetRow(ro).Height = 20 * 20;
                for (int s = 0; s < 8; s++)
                {
                    sheet.GetRow(ro).Cells[s].CellStyle = style;
                }
            }
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            workbook.Write(ms);
            workbook = null;
            ms.Close();
            ms.Dispose();
            return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "资产信息.xls");

        }
        //导入
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult PropertyImport(IFormFile excelfile)
        {
            List<SwsPropertyInfo> list = new List<SwsPropertyInfo> { };
            try
            {
                MemoryStream ms = new MemoryStream();
                excelfile.CopyTo(ms);
                ms.Seek(0, SeekOrigin.Begin);
                
                HSSFWorkbook workbook = new HSSFWorkbook(ms);
               
                List<int> ids = new List<int> { (int)Model.CustomizedClass.Enum.资产类型 };
                //所有类型
                var propertyType = _DataItemDetailService.Query<SysDataItemDetail>(r => ids.Contains(r.FItemId) && r.IsEnable == true).ToList();
                var propertymodel = _PropertyInfoService.Query<SwsPropertyInfo>(r => true).OrderBy(r => r.PropertyId).LastOrDefault();
                int maxid = 0;
                if (propertymodel == null)
                {
                    maxid = 0;
                }
                else
                {
                    maxid = propertymodel.PropertyId;
                }

                List<int> stationNums = new List<int>() { };
                ISheet sheet = workbook.GetSheetAt(0);

                for (int i = 1; i <= sheet.LastRowNum; i++)
                {
                    IRow row = sheet.GetRow(i);
                    SwsPropertyInfo obj = new SwsPropertyInfo();
                    if (row.GetCell(0) != null && row.GetCell(0).ToString() != "")
                    {

                        obj.PropertyId = maxid + i;
                        obj.StationId =int.Parse(row.GetCell(0).ToString());
                        stationNums.Add( int.Parse(row.GetCell(0).ToString()));
                        obj.Name = row.GetCell(3)?.ToString();
                        string typename = row.GetCell(4)?.ToString();
                        var pTypeName = propertyType.Where(r => r.ItemName.Contains(typename) && r.FItemId == ids[0]).FirstOrDefault();
                        if (pTypeName != null)
                        {
                            obj.Type = byte.Parse(pTypeName.ItemValue);
                        }
                        else
                        {
                            obj.Type = 1;
                        }
                      
                        obj.BuyDate =string.IsNullOrEmpty(row.GetCell(5).ToString())?DateTime.Now:row.GetCell(5).DateCellValue;
                        obj.BuyMonery = decimal.Parse(row.GetCell(6)?.ToString());
                        obj.Custodian= row.GetCell(7)?.ToString();
                        obj.Manufacturer= row.GetCell(8)?.ToString();
                        obj.BrandName= row.GetCell(9)?.ToString();
                        obj.Size = row.GetCell(10)?.ToString();
                        obj.WarrantyPeriod= row.GetCell(11)?.ToString();
                        obj.StorePosition= row.GetCell(12)?.ToString();
                        list.Add(obj);

                    }


                }
                #region 判断泵房id是否有重复
                if (stationNums.Count > 0)
                {
                    //判断添加进去的是否有重复
                    if (stationNums.Count != stationNums.Distinct().Count())
                    {
                        return Content("文件中有重复的泵房id");
                    }
                    int userID = int.Parse(User.FindFirstValue("UserID"));
                    var user = _ISys_UserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
                    //判断是否有数据库未有的id
                    List<int> unhasStation = new List<int>() { };
                    if (user.IsAdmin == true)
                    {
                       var  hasStation = _StationService.Query<SwsStation>(r => stationNums.Contains(r.StationId)).Select(r => r.StationId).ToList();
                        unhasStation = stationNums.Except(hasStation).Distinct().ToList();
                        if (unhasStation.Count > 0)
                        {
                            var unhasStationNum = string.Join(',', unhasStation);
                            return Content("文件中的泵房id，" + unhasStationNum + ",数据库中不存在");
                        }
                    }
                    else
                    {
                        List<int> userStationids = _UserStationService.Query<SwsUserStation>(r => r.UserId == userID).Select(r => r.StationId).ToList();//普通用户分配的权限
                        unhasStation = stationNums.Except(userStationids).Distinct().ToList();
                        if (unhasStation.Count > 0)
                        {
                            var unhasStationNum = string.Join(',', unhasStation);
                            return Content("文件中的泵房id，" + unhasStationNum + ",数据库中不存在或没有此泵房的权限");
                        }

                    }
                   

                }
                #endregion
            }
            catch (Exception e)
            {
                if (e.Source == "NPOI")
                {
                    return Content("文件类型不对");
                }
                else
                {
                    if (e.Message.Contains("DateTime"))
                    {
                        return Content("日期不对");
                    }
                    else
                    {
                        return Content("请填写必填信息");
                    }
                }

            }
            try
            {
                if (list.Count > 0)
                {
                    _PropertyInfoService.Insert<SwsPropertyInfo>(list);
                    return Content("导入成功");
                }
                else
                {
                    return Content("空文件");
                }

            }
            catch (Exception e)
            {
                return Content("导入失败");
            }
        }
        #endregion
        #region 选择泵房
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult SelectStationInfo()
        {
            return View();
        }
        [TypeFilter(typeof(IgonreActionFilter))]
        public ActionResult LoadStationList(int pageSize, int pageIndex, string sort, string sortOrder, string stationName)
        {
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = _ISys_UserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
            Expression<Func<SwsStation, bool>> funcWhere = r => true;
            if (user.IsAdmin != true)
            {
                var id = _UserStationService.Query<SwsUserStation>(r => r.UserId == user.UserId).Select(r => r.StationId).ToList();
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
            var infoList = _StationService.QueryPage(funcWhere, pageSize, pageIndex, sort, flag);
            return Json(new { total = infoList.TotalCount, rows = Newtonsoft.Json.JsonConvert.SerializeObject(infoList.DataList.ToList()) });

        }
        #endregion
    }
}