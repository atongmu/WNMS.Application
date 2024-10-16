using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WNMS.Application.Utility;
using WNMS.IService;
using WNMS.Model.DataModels;
using WNMS.Utility;
using WNMS.Model.CustomizedClass;
using Microsoft.AspNetCore.Html;
using Newtonsoft.Json;
using WNMS.Application.Utility.Filters;

namespace WNMS.Application.Areas.Sys.Controllers
{
    [Area("Sys")]
    public class Sys_ModuleController : Controller
    {
        #region 属性 构造函数
        private IService.ISys_ModuleService moduleService = null;
        private IService.ISys_ModuleButtonService modulebuttonService = null;

        public Sys_ModuleController(ISys_ModuleService sys_Module, ISys_ModuleButtonService sys_ModuleButtonService)
        {
            moduleService = sys_Module;
            modulebuttonService = sys_ModuleButtonService;
        }
        #endregion

        #region 数据查询
        public IActionResult Index()
        {
            //系统功能树
            JsonResult result = ActionTree();
            var moduls = Newtonsoft.Json.JsonConvert.SerializeObject(result.Value);
            if (!string.IsNullOrWhiteSpace(moduls))
            {
                ViewBag.TreeNodes = new HtmlString(moduls);
            }
            else
            {
                ViewBag.TreeNodes = "[]";
            }

            return View();
        }

        [HttpPost]
        [TypeFilter(typeof(IgonreActionFilter))]
        public async Task<IActionResult> GetModuleDate(string name, int? pnum, string order, string sortName, int? pageSize, int? pageIndex)
        {
            #region 查询条件
            Expression<Func<SysModule, bool>> funcWhere = null;
            if (!string.IsNullOrWhiteSpace(name))
            {
                funcWhere = funcWhere.And(c => c.ModuleName.Contains(name));
            }
            if (pnum != null)
            {
                funcWhere = funcWhere.And(c => c.Pnum==pnum);
            }
            
            #endregion

            #region 排序
            bool flag = true;
            if (order == "desc") flag = false;
            string sort = string.IsNullOrWhiteSpace(sortName) ? "ModuleNum" : sortName;
            #endregion

            int size = pageSize ?? 20;
            int index = pageIndex ?? 1;

            PageResult<SysModule> moduleList = this.moduleService.QueryPage<SysModule>(funcWhere, size, index, sort, flag);
            PartialView("_ModuleTable", moduleList.DataList);
            string dataTable = await ViewToString.RenderPartialViewToString(this, "_ModuleTable");
            return Json(new
            {
                total = moduleList.TotalCount,
                pageIndex = moduleList.PageIndex,
                pageSize = moduleList.PageSize,
                order = order,
                sortName = sortName ?? "",
                dataTable = dataTable
            });
        }

        [HttpPost]
        [TypeFilter(typeof(IgonreActionFilter))]
        public ActionResult UpdateStatus(int moduleId, bool status)
        {
            var info = moduleService.Find<SysModule>(moduleId);
            if (info != null)
            {
                info.IsEnable = status;
                if (moduleService.Update<SysModule>(info))
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

        [HttpPost]
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult SelectTree()
        {
            JsonResult result = ActionTree();
            var moduls = Newtonsoft.Json.JsonConvert.SerializeObject(result.Value);
            var str = new HtmlString(moduls);
            return Content(str.ToString());          
        }
        #endregion

        #region 编辑系统功能
        public IActionResult AddModulePage()    //添加页面加载
        {
            //系统功能树
            JsonResult result = ActionTree();
            var moduls = Newtonsoft.Json.JsonConvert.SerializeObject(result.Value);
            if (!string.IsNullOrWhiteSpace(moduls))
            {
                ViewBag.TreeNodes = new HtmlString(moduls);
            }
            else
            {
                ViewBag.TreeNodes = "[]";
            }

            ViewBag.KeyValue = 0;
            return View("SetModule", new SysModule());
        }

        public IActionResult EditModulePage(int? id)  //编辑页面加载
        {
            //系统功能树
            JsonResult result = ActionTree();
            var moduls = Newtonsoft.Json.JsonConvert.SerializeObject(result.Value);
            if (!string.IsNullOrWhiteSpace(moduls))
            {
                ViewBag.TreeNodes = new HtmlString(moduls);
            }
            else
            {
                ViewBag.TreeNodes = "[]";
            }

            SysModule module = new SysModule();
            if (id != null)
            {
                module = this.moduleService.Find<SysModule>((int)id) ?? new SysModule();
            }

            List<SysModuleButton> moduleBtnList = this.modulebuttonService.Query<SysModuleButton>(r => r.ModuleId == id).ToList();
            var aa = JsonConvert.SerializeObject(moduleBtnList);
            ViewBag.ButtonList = JsonConvert.SerializeObject(moduleBtnList);
            ViewBag.KeyValue = 1;
            return View("SetModule", module);
        }

        [HttpPost]
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult CheckID(int id)     //判断编号是否存在
        {
            var allnos = this.moduleService.Find<SysModule>((int)id);
            if (allnos != null)
            {
                return Content("no");//已有该编号
            }
            else
            {
                return Content("ok");
            }
        }

        /// <summary>
        /// 表单提交
        /// </summary>
        /// <param name="module">菜单权限数据  json</param>
        /// <param name="moduleBtn">按钮权限数据 json</param>
        /// <param name="key">判断添加还是编辑 0添加，1编辑</param>
        /// <returns></returns>
        [HttpPost]
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult SetModuleInfo(string module,string moduleBtn,int key)
        {
            List<SysModuleButton> moduleButtonList = string.IsNullOrEmpty(moduleBtn)?null:JsonConvert.DeserializeObject<List<SysModuleButton>>(moduleBtn);
            SysModule moduleData = string.IsNullOrEmpty(module) ?null:JsonConvert.DeserializeObject<SysModule>(module);
            if (key == 0)      //插入
            {
                moduleData.IsEnable = true;
                if (this.moduleService.AddModuleEntity(moduleData, moduleButtonList)>0)
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
                    if (moduleService.EditModuleEntity(moduleData, moduleButtonList) > 0)
                    {
                        return Content("ok");
                    }
                    else
                    {
                        return Content("no");
                    }
                

            }
        }

        [HttpPost]
        public IActionResult DeleteModule(int? moduleId)   //删除数据
        {
            if (moduleId == null)
            {
                return Content("false");
            }
            else
            {
                if (this.moduleService.DeleteModule((int)moduleId) > 0)
                {
                    return Content("ok");
                }
                else
                {
                    return Content("no");
                }
            }
        }
        #endregion

        #region 获取权限列表
        [HttpPost]
        [TypeFilter(typeof(IgonreActionFilter))]
        public JsonResult ActionTree()   //获取权限列表
        {
            var list = moduleService.Query<SysModule>(m => true).Select(m => new
            {
                id = m.ModuleNum,
                name = "<i class='" + m.Icon + "'></i><span>" + m.ModuleName + "</span>",
                pId=m.Pnum
            });
            return Json(list);
        }
        #endregion
    }
}