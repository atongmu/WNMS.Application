using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WNMS.Application.Utility.Filters;
using WNMS.IService;
using WNMS.Model.DataModels;
using System.Globalization;
using System.IO;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using NPOI.HSSF.UserModel;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using WNMS.Utility.PanGu;
using MongoDB.Bson;
using WNMS.Model.CustomizedClass;

namespace WNMS.Application.Areas.Sys
{
    [Area("Sys")]
    [TypeFilter(typeof(IgonreActionFilter))]
    public class Sys_EarlyWarningPlanController : Controller
    {
        public ISysUserService _sysUserService = null;
        private ISys_DataItemDetailService _DataItemDetailService = null;

        public ISys_EarlyWarningPlanService _Sys_EarlyWarningPlanService = null;


        public Sys_EarlyWarningPlanController(ISysUserService sysUserService,
    ISys_DataItemDetailService sys_DataItemDetailService,
    ISys_EarlyWarningPlanService sys_EarlyWarningPlanService)
        {

            _sysUserService = sysUserService;

            _DataItemDetailService = sys_DataItemDetailService;
            _Sys_EarlyWarningPlanService = sys_EarlyWarningPlanService;
        }
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 数据查询
        /// </summary>
        /// <param name="pagesize">条数</param>
        /// <param name="pageindex">页码</param>
        /// <param name="SearchText">查询条件</param>
        /// <param name="order">排序</param>
        /// <param name="sort"></param>
        /// <returns></returns>
        public IActionResult QueryPlansTable(int pagesize, int pageindex, string SearchText, string order, string sort)
        {


            int Totalcount = 0;
            string ordertems = sort + " " + order;
            string filter = "1=1";
            string[] strList = null;
            if (!string.IsNullOrEmpty(SearchText))
            {
                filter += " and ( ";
                strList = SplitContent.SplitWords(SearchText);
                foreach (var item in strList)
                {
                    filter += " Title like '%" + item + "%' or Contents like '%" + item + "%' or Solution like '%" + item + "%' OR";
                }
                filter = filter.Substring(0, filter.Length - 2);
                filter += " ) ";
            }

            var datalist = _Sys_EarlyWarningPlanService.QueryPlansTable(pageindex, pagesize, ordertems, filter, ref Totalcount);

            if (!string.IsNullOrEmpty(SearchText) && datalist.Count() > 0)
            {
                List<EarlyWarningPlan> listplan
                  = new List<EarlyWarningPlan>();
                foreach (var item in datalist)
                {
                    EarlyWarningPlan plan = new EarlyWarningPlan();
                    plan = SplitContent.SetHighlighter(strList, item);
                    //plan = SplitContent.SetHighlighter(strList, item);
                    listplan.Add(plan);
                }

                return Json(new { total = Totalcount, rows = Newtonsoft.Json.JsonConvert.SerializeObject(listplan) });
            }

            else
            {
                return Json(new { total = Totalcount, rows = Newtonsoft.Json.JsonConvert.SerializeObject(datalist) });
            }





        }

        #region 页面操作
        //添加
        public IActionResult AddPage()
        {

            var slotype = (int)Model.CustomizedClass.Enum.方案类型;
            ViewBag.slotypelist = _DataItemDetailService.Query<SysDataItemDetail>(r => r.FItemId == slotype).ToList();
            return View("_SetInfoPage", new SysEarlyWarningPlan());
        }
        //编辑
        public IActionResult EditePage(long id)
        {
            SysEarlyWarningPlan sysEarlyWarningPlan = _Sys_EarlyWarningPlanService.Query<SysEarlyWarningPlan>(s => s.Id == id).FirstOrDefault();
            var slotype = (int)Model.CustomizedClass.Enum.方案类型;
            ViewBag.slotypelist = _DataItemDetailService.Query<SysDataItemDetail>(r => r.FItemId == slotype).ToList();
            return View("_SetInfoPage", sysEarlyWarningPlan);
        }
        //删除
        [HttpPost]
        public IActionResult DeleteAccessories(string requestids)
        {
            List<string> Ids = requestids.Split(',').ToList();

            int count = _Sys_EarlyWarningPlanService.DeletePlans(Ids);
            if (count > 0)
            {
                return Content("ok");
            }
            else if (count == -1)
            {
                return Content("存在已在用的设备无法删除");
            }
            else
            {
                return Content("no");
            }

        }

        //提交表单
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult SetInfo(SysEarlyWarningPlan sysEarlyWarningPlan)
        { 
            if (sysEarlyWarningPlan.Id == 0)
            {
                sysEarlyWarningPlan.IsEnable = true;
                sysEarlyWarningPlan.Id = _sysUserService.QueryID("ID", "Sys_EarlyWarningPlan");
                if (_Sys_EarlyWarningPlanService.Insert(sysEarlyWarningPlan) != null)
                {
                    return Content("ok");
                }
                else
                {
                    return Content("no");
                }

            }

            else
            {
                sysEarlyWarningPlan.IsEnable = true;
                if (_Sys_EarlyWarningPlanService.Update(sysEarlyWarningPlan))
                {
                    return Content("ok");
                }
                else
                {
                    return Content("no");
                }
            }


        }


        /// <summary>
        /// 修改是否可用
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="status">状态</param>
        /// <returns></returns>
        [HttpPost]
        [TypeFilter(typeof(IgonreActionFilter))]
        public ActionResult UpdateStatus(long id, bool status)
        {
            var info = _Sys_EarlyWarningPlanService.Find<SysEarlyWarningPlan>(id);
            if (info != null)
            {
                info.IsEnable = status;
                if (_Sys_EarlyWarningPlanService.Update<SysEarlyWarningPlan>(info))
                {
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

        #endregion


        //#region 导入
        //[TypeFilter(typeof(IgonreActionFilter))]
        //public IActionResult Import(IFormFile excelfile)
        //{
        //    string ExcelKzm = ".xls|.xlsx";
        //    string OldName = string.Empty;
        //    OldName = "." + excelfile.FileName.Split('.')[1];
        //    if (ExcelKzm.IndexOf(OldName) < 0)
        //    {
        //        return Content("typeno");
        //    }
        //    List<SwsAccessories> list = new List<SwsAccessories> { };

        //    MemoryStream ms = new MemoryStream();
        //    excelfile.CopyTo(ms);
        //    ms.Seek(0, SeekOrigin.Begin);
        //    IWorkbook workbook = null;
        //    if (OldName == ".xls")
        //    {
        //        workbook = new HSSFWorkbook(ms);

        //    }
        //    else
        //    {
        //        workbook = new XSSFWorkbook(ms);

        //    }
        //    ISheet sheet = workbook.GetSheetAt(0);
        //    try
        //    {
        //        for (int i = 1; i <= sheet.LastRowNum; i++)
        //        {
        //            IRow row = sheet.GetRow(i);
        //            SwsAccessories obj = new SwsAccessories();
        //            if (row.GetCell(0) != null)
        //            {
        //                //Regex reg = new Regex(@"[\u4e00-\u9fa5]");
        //                //string md = reg.Replace(row.GetCell(3).ToString(), "");
        //                obj.Id = row.GetCell(0)?.ToString();
        //                obj.Name = row.GetCell(1).ToString();
        //                obj.Type = row.GetCell(2).ToString();
        //                obj.Material = row.GetCell(3).ToString();
        //                obj.Manufacturer = row.GetCell(4).ToString();
        //                obj.MaintenancePeriod = Int32.Parse(row.GetCell(5)?.ToString());
        //                obj.ReplacementCycle = Int32.Parse(row.GetCell(6)?.ToString());
        //                obj.Unit = row.GetCell(7)?.ToString();
        //                obj.Inventory = Int32.Parse(row.GetCell(8)?.ToString());
        //                obj.IsConsumable = row.GetCell(9)?.ToString() == "1" || row.GetCell(9)?.ToString() == "是" ? true : false;
        //            }
        //            else
        //            {
        //                continue;
        //            }
        //            list.Add(obj);
        //        }
        //        ms.Dispose();
        //        if (_Sys_EarlyWarningPlanService.AccessoriesImport(list) > 0)
        //        {
        //            return Content("ok");
        //        }
        //        else
        //        {
        //            return Content("no");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Content("exception");
        //    }

        //}
        //#endregion
    }
}
