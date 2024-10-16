using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Castle.Core.Configuration;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using WNMS.Application.Utility.Filters;
using WNMS.IService;
using WNMS.Model.DataModels;

namespace WNMS.Application.Areas.Sys.Controllers
{
    [Area("Sys")]
    public class Sys_DataItemController : Controller
    {

        public ISys_DataItemDetailService _ISys_DataItemDetailService = null;
        public ISys_DataItemService _ISys_DataItemService = null;
        public ISysUserService _ISys_UserService = null;
        public Sys_DataItemController(ISys_DataItemDetailService _sys_DataDetail, ISys_DataItemService _DataItemService, ISysUserService _userservice)
        {

            _ISys_DataItemDetailService = _sys_DataDetail;
            _ISys_DataItemService = _DataItemService;
            _ISys_UserService = _userservice;
        }
        public IActionResult Index()
        {

            var list = _ISys_DataItemService.Query<SysDataItem>(t => true).OrderBy(r => r.Sort);
            if (list.Count() > 0)
            {
                var treedata = list.Select(r => new
                {
                    id = r.ItemId,
                    pId = r.Pid,
                    name = "<i class='fa fa-bookmark-o' aria-hidden='true'></i>" + r.ItemName
                });
                ViewBag.treenode = new HtmlString(Newtonsoft.Json.JsonConvert.SerializeObject(treedata));
            }
            else
            {
                ViewBag.treenode = new HtmlString("[]");
            }
            return View();
        }
        //左侧树刷新
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult GetClassifyTree()
        {
            var list = _ISys_DataItemService.Query<SysDataItem>(t => true).OrderBy(r => r.Sort);
            if (list.Count() > 0)
            {
                var treedata = list.Select(r => new
                {
                    id = r.ItemId,
                    pId = r.Pid,
                    name = "<i class='fa fa-bookmark-o' aria-hidden='true'></i>" + r.ItemName
                   
                });
                return Content(Newtonsoft.Json.JsonConvert.SerializeObject(treedata));
            }
            else
            {
                return Content("[]");
                
            }

        }

        [HttpPost]
        [TypeFilter(typeof(IgonreActionFilter))]
        //public IActionResult QueryTable([FromBody] searchclass data)
        //{
        //    string name = data.ItemName;
        //    int pid = data.pid;
        //    Expression<Func<SysDataItemDetail, bool>> funcWhere = (s => s.ItemName.Contains(name) && s.FItemId == pid);
        //    var datalist = _ISys_DataItemDetailService.QueryPage(funcWhere,10,1,s=>s.ItemName,true);
        //    return Content("");
        //}
        public JsonResult QueryTable(int pageindex, int pagesize, string ItemName, int pid, string order, string sort)
        {
            //int pageindex = int.Parse(base.HttpContext.Request.Form["pageindex"]);
            //int pagesize= int.Parse(base.HttpContext.Request.Form["pagesize"]);
            //dynamic data1 = data;
            string name = ItemName ?? "";
            //int pid = data1.pid;
            Expression<Func<SysDataItemDetail, bool>> funcWhere = (s => s.ItemName.Contains(name) && s.FItemId == pid);
            bool flagsort = false;
            if (order == "asc")
            {
                flagsort = true;
            }

            var datalist = _ISys_DataItemDetailService.QueryPage(funcWhere, pagesize, pageindex, sort, flagsort);

            return Json(new { total = datalist.TotalCount, rows = Newtonsoft.Json.JsonConvert.SerializeObject(datalist.DataList.ToList()) });
        }
        #region 页面操作（增删改）
        //添加
        [HttpGet]
        public IActionResult AddPage(int id)
        {
            var model = new SysDataItemDetail();
            model.FItemId = id;
            return View("_SetDataItemDetail", model);
        }
        //编辑
        [HttpGet]
        public IActionResult EditePage(int id)
        {
            var hasDetail = _ISys_DataItemDetailService.Query<SysDataItemDetail>(r => r.ItemDetailId == id).FirstOrDefault();
            return View("_SetDataItemDetail", hasDetail);
        }
        //提交表单（字典详情表）
        [HttpPost]
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult SetDataDetail([FromForm]SysDataItemDetail a)
        {
            if (a.ItemDetailId == 0)//添加
            {
                var hasDetail = _ISys_DataItemDetailService.Query<SysDataItemDetail>(r => r.FItemId == a.FItemId && r.ItemValue == a.ItemValue).Count();
                if (hasDetail > 0)
                {
                    return Content("have");
                }
                var num = _ISys_UserService.QueryID("ItemDetailID", "Sys_DataItemDetail");
                a.ItemDetailId = num;
                if (_ISys_DataItemDetailService.Insert(a) != null)
                {
                    return Content("ok");
                }
                else
                {
                    return Content("ok");
                }
            }
            else//修改
            {
                var hasDetail = _ISys_DataItemDetailService.Query<SysDataItemDetail>(r => r.FItemId == a.FItemId && r.ItemValue == a.ItemValue && r.ItemDetailId != a.ItemDetailId).Count();
                if (hasDetail > 0)
                {
                    return Content("have");
                }
                _ISys_DataItemDetailService.Update(a);
                return Content("ok");
            }

        }
        //删除
        [HttpPost]
        public IActionResult DeleteItemDetail(string id)
        {
            List<int> detailids = new List<string>(id.Split(',')).ConvertAll(r => int.Parse(r));
            IEnumerable<SysDataItemDetail> deleteDetail = _ISys_DataItemDetailService.Query<SysDataItemDetail>(r => detailids.Contains(r.ItemDetailId));
            try
            {
                _ISys_DataItemDetailService.Delete(deleteDetail);
                return Content("ok");
            }
            catch (Exception e)
            {
                return Content("no");
            }
        }
        #endregion
        #region 字典分类
        public IActionResult ClassifyPage()
        {
            return View();
        }
        //查询表格
        [HttpPost]
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult QueryDataItemTable(string ItemName)
        {
            string name = ItemName ?? "";

            Expression<Func<SysDataItem, int>> func = s => s.Sort;
            List<SysDataItem> datalist = null;
            if (name == "")
            {
                datalist = _ISys_DataItemService.Query<SysDataItem>(null).OrderBy(r => r.Sort).ToList();
            }
            else
            {

                datalist = _ISys_DataItemService.Query<SysDataItem>(s => s.ItemName.Contains(name)).OrderBy(r => r.Sort).Select(r => new SysDataItem
                {

                    ItemId = r.ItemId,
                    Pid = 0,
                    ItemName = r.ItemName,
                    ItemValue = r.ItemValue,
                    Sort = r.Sort,
                    Reamrk = r.Reamrk
                }).ToList();
            }

            //return Json(new { total = datalist.TotalCount, rows = Newtonsoft.Json.JsonConvert.SerializeObject(datalist.DataList.ToList()) });
            return Json(new { data = Newtonsoft.Json.JsonConvert.SerializeObject(datalist) });
        }
        //添加
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult AddClassifyPage()
        {
            List<dynamic> listTree = new List<dynamic>() { };
            var addclass = new
            {
                id = -1,
                pId = 0,
                name = "==请选择==",

            };
            listTree.Add(addclass);
            var list = _ISys_DataItemService.Query<SysDataItem>(t => true).OrderBy(r => r.Sort);
            if (list.Count() > 0)
            {
                var treedata = list.Select(r => new
                {
                    id = r.ItemId,
                    pId = r.Pid,
                    name = r.ItemName
                });
                listTree = listTree.Concat(treedata).ToList();

            }
            ViewBag.treenodes = new HtmlString(Newtonsoft.Json.JsonConvert.SerializeObject(listTree));
            var classmodel = new SysDataItem();
            return View("_SetDataClassify", classmodel);
        }
        //编辑
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult EditeClassifyPage(int id)
        {
            List<dynamic> listTree = new List<dynamic>() { };
            var addclass = new
            {
                id = -1,
                pId = 0,
                name = "==请选择==",

            };
            listTree.Add(addclass);
            var list = _ISys_DataItemService.QueyExtentSelfAndChirldren(id);
            if (list.Count() > 0)
            {
                var treedata = list.Select(r => new
                {
                    id = r.ItemId,
                    pId = r.Pid,
                    name = r.ItemName
                });
                listTree = listTree.Concat(treedata).ToList();

            }
            ViewBag.treenodes = new HtmlString(Newtonsoft.Json.JsonConvert.SerializeObject(listTree));
            var hasmodel = _ISys_DataItemService.Query<SysDataItem>(r => r.ItemId == id).FirstOrDefault();
            if (hasmodel.Pid != 0)
            {
                ViewBag.ItemName = _ISys_DataItemService.Query<SysDataItem>(r => r.ItemId == hasmodel.Pid).FirstOrDefault().ItemName;
            }
            return View("_SetDataClassify", hasmodel);
        }
        //表单提交
        [HttpPost]
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult SetDataClassify([FromForm] SysDataItem a)
        {
            if (a.ItemId == 0)//添加
            {
                try
                {
                    var num = _ISys_UserService.QueryID("ItemID", "Sys_DataItem");
                    a.ItemId = num;
                    a.ItemValue = "0";
                    _ISys_DataItemService.Insert(a);
                    return Content("ok");
                }
                catch (Exception e)
                {
                    return Content("no");
                }

            }
            else//修改
            {
                try
                {
                    if (a.ItemId == a.Pid)
                    {
                        return Content("same");
                    }
                    _ISys_DataItemService.Update(a);
                    return Content("ok");
                }
                catch (Exception e)
                {
                    return Content("no");
                }
            }
        }
        //删除
        [HttpPost]
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult DeleteClassify(string id)
        {
            var entities = _ISys_DataItemService.QuerySelfAndChirldren(id);
            try
            {
                var pid_f = entities.Select(r => r.ItemId);
                IEnumerable<SysDataItemDetail> deleteDetail = _ISys_DataItemDetailService.Query<SysDataItemDetail>(r => pid_f.Contains(r.FItemId));
                if (deleteDetail.Count() > 0)
                {
                    _ISys_DataItemDetailService.Delete(deleteDetail);
                }
                _ISys_DataItemService.Delete(entities);
                return Content("ok");
            }
            catch (Exception e)
            {
                return Content("no");
            }

        }
        #endregion
        public class searchclass
        {
            public string ItemName { get; set; }
            public int pid { get; set; }
        }
    }
}