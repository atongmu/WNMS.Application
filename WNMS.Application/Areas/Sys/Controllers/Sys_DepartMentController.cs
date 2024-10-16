using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WNMS.Model.DataModels;
using WNMS.IService;
using Microsoft.AspNetCore.Html;
using WNMS.Application.Utility.Filters;

namespace WNMS.Application.Areas.Sys.Controllers
{
    [Area("Sys")]
    public class Sys_DepartMentController : Controller
    {
        private ISys_DepartMentService _ISys_DepartMentService = null;
        public ISysUserService _ISys_UserService = null;
        public ISys_DataItemDetailService _ISys_DataItemDetailService = null;
        public Sys_DepartMentController(ISys_DepartMentService _DepartMentService, ISysUserService _userservice,ISys_DataItemDetailService _sys_DataDetail)
        {

            _ISys_DepartMentService = _DepartMentService;
            _ISys_UserService = _userservice;
            _ISys_DataItemDetailService = _sys_DataDetail;
        }
        public IActionResult Index()
        {
           
            return View();
        }
        //获取表格数据
        [HttpPost]
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult QueryDepartmentTable(string departmentName)
        {
            string name = departmentName ?? "";

            var departmentType = (int)Model.CustomizedClass.Enum.部门类型;
            
           
            var datalist = _ISys_DepartMentService.QueryDepartmentTable(name, departmentType).OrderBy(r => r.Sort).ToList();
            
            return Json(new { data = Newtonsoft.Json.JsonConvert.SerializeObject(datalist) });
        }
        #region 页面操作
        //添加
        public IActionResult AddDepartPage()
        {
            List<dynamic> listTree = new List<dynamic>(){};
            var addclass = new 
            {
                id = -1,
                pId = 0,
                name = "==请选择==",
               
            };
            listTree.Add(addclass);
            var list = _ISys_DepartMentService.Query<SysDepartMent>(t => true).OrderBy(r => r.Sort);
            if (list.Count() > 0)
            {
                var treedata = list.Select(r => new
                {
                    id = r.DepartmentId,
                    pId = r.Pid,
                    name = r.DptName
                });

                listTree = listTree.Concat(treedata).ToList();
            }
            ViewBag.treenodes = new HtmlString(Newtonsoft.Json.JsonConvert.SerializeObject(listTree));
            //部门性质
            var departmentType = (int)Model.CustomizedClass.Enum.部门类型;
            var departTypeList = _ISys_DataItemDetailService.Query<SysDataItemDetail>(r => r.FItemId == departmentType&&r.IsEnable==true);
            ViewBag.departTypeList = departTypeList;
            return View("_SetSys_DepartMent",new SysDepartMent());
        }
        //修改
        public IActionResult EditDepartPage(int id)
        {
            List<dynamic> listTree = new List<dynamic>() { };
            var addclass = new
            {
                id = -1,
                pId = 0,
                name = "==请选择==",

            };
            listTree.Add(addclass);
            var list = _ISys_DepartMentService.QueyExtentSelfAndChirldren_Depart(id);
            if (list.Count() > 0)
            {
                var treedata = list.Select(r => new
                {
                    id = r.DepartmentId,
                    pId = r.Pid,
                    name = r.DptName
                });
                listTree = listTree.Concat(treedata).ToList();
            }
            ViewBag.treenodes = new HtmlString(Newtonsoft.Json.JsonConvert.SerializeObject(listTree));
            var hasmodel = _ISys_DepartMentService.Query<SysDepartMent>(r => r.DepartmentId == id).FirstOrDefault();
            if (hasmodel.Pid != 0)
            {
                ViewBag.PIDName = _ISys_DepartMentService.Query<SysDepartMent>(r => r.DepartmentId == hasmodel.Pid).FirstOrDefault().DptName;
            }
            //部门性质
            var departmentType = (int)Model.CustomizedClass.Enum.部门类型;
            var departTypeList = _ISys_DataItemDetailService.Query<SysDataItemDetail>(r => r.FItemId == departmentType&&r.IsEnable==true);
            ViewBag.departTypeList = departTypeList;
            return View("_SetSys_DepartMent", hasmodel);
        }
        //提交表单
        [HttpPost]
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult SetDepart([FromForm] SysDepartMent d)
        {
            if (d.DepartmentId == 0)//添加
            {
                try
                {
                    var num = _ISys_UserService.QueryID("DepartmentID", "Sys_DepartMent");
                    d.DepartmentId = num;
                    _ISys_DepartMentService.Insert(d);
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
                    _ISys_DepartMentService.Update(d);
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
        public IActionResult DeleteDepart(string ids)
        {
            string contentUse = "部门";
            var idlist = new List<string>(ids.Split(','));
            List<SysDepartMent> delteDepart = new List<SysDepartMent>() { };
            foreach (var item in idlist)
            {
                var selfChild = _ISys_DepartMentService.QuerySelfAndChirldren_Depart(item);//本身以及子集部门集合
                var departIdList = selfChild.Select(r => r.DepartmentId).ToList();
                var userList = _ISys_UserService.Query<SysUser>(r => departIdList.Contains(r.Department));//查询占用部门的用户集合
                if (userList.Count() > 0)
                {
                    var useDid = int.Parse(item);
                    var useDmodel = selfChild.Where(r => r.DepartmentId == useDid).FirstOrDefault();
                    contentUse += useDmodel.DptName+",";
                }
                else
                {
                    delteDepart = delteDepart.Concat(selfChild).ToList();
                }
            }
            delteDepart = delteDepart.Distinct(new DepartCompare()).ToList();
            if (delteDepart.Count > 0)
            {
                try {
                    _ISys_DepartMentService.Delete<SysDepartMent>(delteDepart);
                }
                catch (Exception e) {
                    return Content("no");
                }
            }
            if (contentUse != "部门")
            {
                contentUse = contentUse.Substring(0, contentUse.Length - 1);
                contentUse = contentUse + "被占用，无法删除";
                return Content(contentUse);
            }
            return Content("ok");
        }
        #endregion
        public class DepartCompare : IEqualityComparer<SysDepartMent>
        {
            public bool Equals(SysDepartMent x, SysDepartMent y)
            {
                if (x == null)
                    return y == null;
                return x.DepartmentId == y.DepartmentId;
            }

            public int GetHashCode(SysDepartMent obj)
            {
                if (obj == null)
                    return 0;
                return obj.DepartmentId.GetHashCode();
            }
        }
    }
}