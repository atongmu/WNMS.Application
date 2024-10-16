using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using WNMS.Application.Utility.Filters;
using WNMS.IService;
using WNMS.Model.CustomizedClass;
using WNMS.Model.DataModels;

namespace WNMS.Application.Areas.Sys.Controllers
{
    [Area("Sys")]
    public class Sys_RoleController : Controller
    {
        public readonly ISys_RoleService _sys_RoleService = null;
        public readonly ISys_RoleModuleService _sys_RoleModuleService = null;
        public readonly ISys_ModuleService _sys_ModuleService = null;
        public readonly ISysUserService _sysUserService = null;
        public readonly ISys_ModuleButtonService _sys_ModuleButtonService = null;
        public readonly ISys_DepartMentService _sys_DepartMentService = null;
        public readonly ISys_UserRoleService _sys_UserRoleService = null;
        public Sys_RoleController(ISys_RoleService sys_RoleService,
            ISys_RoleModuleService sys_RoleModuleService,
            ISys_ModuleService sys_ModuleService,
            ISysUserService sysUserService,
            ISys_ModuleButtonService sys_ModuleButtonService,
            ISys_DepartMentService sys_DepartMentService,
            ISys_UserRoleService sys_UserRoleService
            )
        {
            _sys_RoleService = sys_RoleService;
            _sys_RoleModuleService = sys_RoleModuleService;
            _sys_ModuleService = sys_ModuleService;
            _sysUserService = sysUserService;
            _sys_ModuleButtonService = sys_ModuleButtonService;
            _sys_DepartMentService = sys_DepartMentService;
            _sys_UserRoleService = sys_UserRoleService;
        }
        #region 数据查询 
        public IActionResult Index()
        {
            //Expression<Func<SysRole, bool>> funcWhere = r => true;
            //var infoList = _sys_RoleService.Query(funcWhere);
            //ViewBag.info = infoList;
            return View();
        }
        /// <summary>
        /// 查询信息
        /// </summary>
        /// <param name="pageSize">分页</param>
        /// <param name="pageIndex"></param>
        /// <param name="sort">排序字段</param>
        /// <param name="sortOrder">排序方式</param>
        /// <param name="roleName">角色名称</param>
        /// <returns></returns>
        [TypeFilter(typeof(IgonreActionFilter))]
        public ActionResult LoadInfoList(int pageSize, int pageIndex, string sort, string sortOrder, string roleName)
        {
            Expression<Func<SysRole, bool>> funcWhere = r => true;
            bool flag = true;
            //查询条件

            if (!string.IsNullOrEmpty(roleName))
            {
                funcWhere = r => r.RoleName.Contains(roleName);
            }
            if (sortOrder == "desc")
            {
                flag = false;
            }
            var infoList = _sys_RoleService.QueryPage(funcWhere, pageSize, pageIndex, sort, flag);
            return Json(new { total = infoList.TotalCount, rows = Newtonsoft.Json.JsonConvert.SerializeObject(infoList.DataList.ToList()) });

        }
        /// <summary>
        /// 修改角色是否可用
        /// </summary>
        /// <param name="roleid">角色id</param>
        /// <param name="status">状态</param>
        /// <returns></returns>
        [HttpPost]
        [TypeFilter(typeof(IgonreActionFilter))]
        public ActionResult UpdateStatus(int roleid, bool status)
        {
            var info = _sys_RoleService.Find<SysRole>(roleid);
            if (info != null)
            {
                info.IsEnable = status;
                if (_sys_RoleService.Update<SysRole>(info))
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

        #region 角色增删改 

        //添加角色页面
        public IActionResult AddPage()
        {
            return View("SetRole", new SysRole());
        }
        //编辑角色页面
        public IActionResult EditPage(int id)
        {
            SysRole roleInfo = _sys_RoleService.Find<SysRole>(id);
            return View("SetRole", roleInfo);
        }
        //角色表单提交
        [HttpPost]
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult SetRoleInfo(SysRole ro, int id_Role)
        {
            if (id_Role == 0)
            {
                var usedinfo = _sys_RoleService.Query<SysRole>(r => r.Sort == ro.Sort).ToList();
                if (usedinfo.Count > 0)
                {
                    return Content("hassort");
                }
                ro.RoleId = _sysUserService.QueryID("RoleID", "Sys_Role");
                ro.IsEnable = true;
                if (_sys_RoleService.Find<SysRole>(ro.RoleId) != null)
                {
                    return Content("false");
                }
                else
                {
                    if (_sys_RoleService.Insert<SysRole>(ro) != null)
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
                ro.RoleId = id_Role;
                var oldRole = _sys_RoleService.Find<SysRole>(ro.RoleId);
                ro.IsEnable = oldRole.IsEnable;
                ro.IsEnable = true;
                oldRole.RoleName = ro.RoleName;
                oldRole.Sort = ro.Sort;
                oldRole.Remark = ro.Remark;
                if (_sys_RoleService.Update<SysRole>(oldRole))
                {
                    return Content("ok");
                }
                else
                {
                    return Content("no");
                }
            }
        }

        //批量删除角色
        [HttpPost]
        public ActionResult DeleteRole(string requestRoleids)
        {
            string[] role_Ids = requestRoleids.Split(',');
            List<string> roleIdsList = new List<string>();
            List<int> roleIds = new List<int>();
            foreach (var item in role_Ids)
            {
                roleIds.Add(int.Parse(item));
            }
            if (_sys_RoleService.DeleteRoles(roleIds, ref roleIdsList) > 0)
            {
                string message = "";
                if (roleIdsList.Count() > 0)            //判断并循环未被删除的项                             
                {
                    foreach (var dpt in roleIdsList)
                    {
                        message += dpt.ToString() + ",";
                    }
                    message = message.Substring(0, message.Length - 1);
                    return Content("角色" + message + "被占用，无法删除！其它删除成功！");     //有未被删除的项时返回提示信息
                }
                else
                {
                    return Content("ok");
                }
            }
            else
            {
                if (roleIdsList.Count() > 0)
                {
                    return Content("false");
                }
                else
                {
                    return Content("no");
                }
            }

        }
        #endregion

        #region 角色权限
        /// <summary>
        /// 获取第一步的权限 加载视图
        /// </summary>
        /// <param name="id">角色ID</param>
        /// <returns></returns>
        public IActionResult RoleAction(int id)
        {
            ViewBag.RoleID = id;
            //已有权限
            var ids = _sys_RoleModuleService.Query<SysRoleModule>(r => r.RoleId == id && r.Type == 1).Select(r => r.ModuleId).ToList();
            IEnumerable<SysModule> saveActions = _sys_ModuleService.Query<SysModule>(r => ids.Contains(r.ModuleNum));
            //全部权限
            //IEnumerable<Model.Action> allActions = service_Action.LoadEntities(a => true);
            IEnumerable<SysModule> allActions = _sys_ModuleService.Query<SysModule>(r => true);
            //差集权限
            IEnumerable<SysModule> exceptActions = allActions.Except(saveActions, new Model.CustomizedClass.ActionCompare());
            //IEnumerable<Model.Action> exceptActions = (from a in allActions
            //                                           select a).Except(from b in saveActions select b);


            //已有权限-菜单权限设置
            IEnumerable<Model.CustomizedClass.TreeAction> saveMenus = saveActions.Select(t => new Model.CustomizedClass.TreeAction
            {
                id = t.ModuleNum,
                pId = t.Pnum,
                name = "<i class='" + t.Icon + "'></i>" + t.ModuleName,
                @checked = true,
                isHidden = false,
                type = 1
                //icon = "../../Content/zTree/css/zTreeStyle/actions.png"
            });

            ////已有权限-执行权限设置
            //IEnumerable<Model.CustomizedClass.TreeAction > saveBtns = saveActions.Where(a => a.ActionTypeEnum == 3).Select(t => new Model.CustomizedClass.TreeAction
            //{
            //    id = t.Num,
            //    pId = t.PNum,
            //    name = t.Name,
            //    @checked = false,
            //    isHidden = true,
            //    isSave = true,
            //    icon = "../../Content/zTree/css/zTreeStyle/actions.png"
            //});
            ////已有权限-水泵命令权限
            //IEnumerable<Model.CustomizedClass.TreeAction> savePumpBtns = saveActions.Where(a => a.ActionTypeEnum == 4).Select(t => new Model.CustomizedClass.TreeAction
            //{
            //    id = t.Num,
            //    pId = t.PNum,
            //    name = t.Name,
            //    @checked = true,
            //    isHidden = false,
            //    icon = "../../Content/zTree/css/zTreeStyle/actions.png"
            //    //  isSave = true
            //});
            ////已有权限-数据大屏权限
            //IEnumerable<Model.CustomizedClass.TreeAction> saveScreens = saveActions.Where(a => a.ActionTypeEnum == 5).Select(t => new Model.CustomizedClass.TreeAction
            //{
            //    id = t.Num,
            //    pId = t.PNum,
            //    name = t.Name,
            //    @checked = true,
            //    isHidden = false,
            //    icon = "../../Content/zTree/css/zTreeStyle/actions.png"
            //    //  isSave = true
            //});

            //差集权限-菜单权限设置
            IEnumerable<Model.CustomizedClass.TreeAction> exceptMenus = exceptActions.Select(t => new Model.CustomizedClass.TreeAction
            {
                id = t.ModuleNum,
                pId = t.Pnum,
                name = "<i class='" + t.Icon + "'></i>" + t.ModuleName,
                @checked = false,
                isHidden = false,
                type = 1
                //icon = "../../Content/zTree/css/zTreeStyle/actions.png"
            });

            ////差集权限-执行权限设置
            //IEnumerable<Model.CustomizedClass.TreeAction> exceptBtns = exceptActions.Where(a => a.ActionTypeEnum == 3).Select(t => new Model.CustomizedClass.TreeAction
            //{
            //    id = t.Num,
            //    pId = t.PNum,
            //    name = t.Name,
            //    @checked = false,
            //    isHidden = true,
            //    isSave = false,
            //    icon = "../../Content/zTree/css/zTreeStyle/actions.png"
            //});

            ////差集权限-水泵命令权限
            //IEnumerable<Model.CustomizedClass.TreeAction> exceptPumpBtns = exceptActions.Where(a => a.ActionTypeEnum == 4).Select(t => new Model.CustomizedClass.TreeAction
            //{
            //    id = t.Num,
            //    pId = t.PNum,
            //    name = t.Name,
            //    @checked = false,
            //    isHidden = false,
            //    icon = "../../Content/zTree/css/zTreeStyle/actions.png"
            //});
            ////差集权限-数据大屏权限
            //IEnumerable<Model.CustomizedClass.TreeAction> exceptScreens = exceptActions.Where(a => a.ActionTypeEnum == 5).Select(t => new Model.CustomizedClass.TreeAction
            //{
            //    id = t.Num,
            //    pId = t.PNum,
            //    name = t.Name,
            //    @checked = false,
            //    isHidden = false,
            //    icon = "../../Content/zTree/css/zTreeStyle/actions.png"
            //});

            //IEnumerable<Model.CustomizedClass.TreeAction> allAction = saveMenus.Union<Model.CustomizedClass.TreeAction>(saveBtns).Union<Model.CustomizedClass.TreeAction>(exceptMenus).Union<Model.CustomizedClass.TreeAction>(exceptBtns).Union<Model.CustomizedClass.TreeAction>(exceptPumpBtns).Union<Model.CustomizedClass.TreeAction>(savePumpBtns).Union<Model.CustomizedClass.TreeAction>(exceptScreens).Union<Model.CustomizedClass.TreeAction>(saveScreens);
            IEnumerable<Model.CustomizedClass.TreeAction> allAction = saveMenus.Union<Model.CustomizedClass.TreeAction>(exceptMenus);
            ViewBag.TreeNodes = new HtmlString(Newtonsoft.Json.JsonConvert.SerializeObject(allAction));
            return View();
        }
        //根据第一步的权限 获取第二步的权限
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult LoadButAction(int roleId, string actionids)
        {
            if (string.IsNullOrEmpty(actionids))
            {
                return Content("");
            }
            //已有按钮权限
            var ids = _sys_RoleModuleService.Query<SysRoleModule>(r => r.RoleId == roleId && r.Type == 2).Select(r => r.ModuleId).ToList();
            //IEnumerable<SysModule> saveActions = _sys_ModuleService.Query<SysModule>(r => ids.Contains(r.ModuleNum));
            IEnumerable<SysModuleButton> saveActions = _sys_ModuleButtonService.Query<SysModuleButton>(r => ids.Contains(r.ModuleButtonId));
            List<int> list = null;
            if (!string.IsNullOrEmpty(actionids))
            {
                list = new List<int>(new List<string>(actionids.Split(',')).Select(a => int.Parse(a)));
            }
            //所选的全部权限
            //IEnumerable<SysModule> allActions = _sys_ModuleService.Query<SysModule>(r => list.Contains(r.ModuleNum));
            IEnumerable<SysModuleButton> allActions = _sys_ModuleButtonService.Query<SysModuleButton>(r => list.Contains(r.ModuleId));
            //差集权限
            IEnumerable<SysModuleButton> exceptActions = allActions.Except(saveActions, new Model.CustomizedClass.ActionCompareBtn());
            //已有权限-菜单权限设置
            IEnumerable<Model.CustomizedClass.TreeAction> saveMenus = saveActions.Select(t => new Model.CustomizedClass.TreeAction
            {
                id = t.ModuleButtonId,
                pId = t.ModuleId,
                name = "<i class='" + t.ButtionIcon + "'></i>" + t.ButtonName,
                @checked = true,
                isHidden = false,
                type = 2
                //icon = "../../Content/zTree/css/zTreeStyle/actions.png"
            });
            //差集权限-菜单权限设置
            IEnumerable<Model.CustomizedClass.TreeAction> exceptMenus = exceptActions.Select(t => new Model.CustomizedClass.TreeAction
            {
                id = t.ModuleButtonId,
                pId = t.ModuleId,
                name = "<i class='" + t.ButtionIcon + "'></i>" + t.ButtonName,
                @checked = false,
                isHidden = false,
                type = 2
                //icon = "../../Content/zTree/css/zTreeStyle/actions.png"
            });
            //查询所选的数据 
            IEnumerable<SysModule> PallActions = _sys_ModuleService.Query<SysModule>(r => list.Contains(r.ModuleNum));
            IEnumerable<Model.CustomizedClass.TreeAction> PallActionsSelect = PallActions.Select(t => new Model.CustomizedClass.TreeAction
            {
                id = t.ModuleNum,
                pId = t.Pnum,
                name = "<i class='" + t.Icon + "'></i>" + t.ModuleName,
                @checked = false,
                isHidden = false,
                type = 1
                //icon = "../../Content/zTree/css/zTreeStyle/actions.png"
            });
            IEnumerable<Model.CustomizedClass.TreeAction> allAction = saveMenus.Union<Model.CustomizedClass.TreeAction>(exceptMenus).Union<Model.CustomizedClass.TreeAction>(PallActionsSelect);
            var TreeNodes = new HtmlString(Newtonsoft.Json.JsonConvert.SerializeObject(allAction));
            return Content(TreeNodes.ToString());
        }
        /// <summary>
        /// 提交选中的权限数据
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <param name="actionids">模块权限</param>
        /// <param name="btnActions">按钮权限</param>
        /// <returns></returns>
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult SetRoleAction(int roleId, string actionids, string btnActions)
        {
            //获取原有权限
            List<SysRoleModule> actionlist = _sys_RoleModuleService.Query<SysRoleModule>(r => r.RoleId == roleId).ToList();
            //type=1 权限
            List<int> list = null;
            if (!string.IsNullOrEmpty(actionids))
            {
                list = new List<int>(new List<string>(actionids.Split(',')).Select(a => int.Parse(a)));
            }
            //List<SysRoleModule> srmList = new List<SysRoleModule>();
            //type=2 权限
            List<int> btnlist = null;
            if (!string.IsNullOrEmpty(btnActions))
            {
                btnlist = new List<int>(new List<string>(btnActions.Split(',')).Select(a => int.Parse(a)));
            }
            List<SysRoleModule> module = new List<SysRoleModule>();
            if (list != null)
            {
                foreach (var item in list)
                {
                    SysRoleModule sysRoleModule = new SysRoleModule();
                    sysRoleModule.RoleId = roleId;
                    sysRoleModule.ModuleId = item;
                    sysRoleModule.Type = 1;
                    module.Add(sysRoleModule);
                }
            }
            if (btnlist != null)
            {
                foreach (var item in btnlist)
                {
                    SysRoleModule sysRoleModule = new SysRoleModule();
                    sysRoleModule.RoleId = roleId;
                    sysRoleModule.ModuleId = item;
                    sysRoleModule.Type = 2;
                    module.Add(sysRoleModule);
                }
            }
            if(actionlist==null&& string.IsNullOrEmpty(btnActions) && string.IsNullOrEmpty(actionids))
            {
                return Content("false");
            }
            else
            {
                if (_sys_RoleService.SetModule(module, roleId) > 0)
                {
                    return Content("ok");
                }
                else
                {
                    return Content("no");
                }
            }
        }

        //测试ztree
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult TestZtree()
        {
            int id = 1;
            ViewBag.RoleID = 1;
            //已有权限
            var ids = _sys_RoleModuleService.Query<SysRoleModule>(r => r.RoleId == id && r.Type == 1).Select(r => r.ModuleId).ToList();
            IEnumerable<SysModule> saveActions = _sys_ModuleService.Query<SysModule>(r => ids.Contains(r.ModuleNum));
            //全部权限
            //IEnumerable<Model.Action> allActions = service_Action.LoadEntities(a => true);
            IEnumerable<SysModule> allActions = _sys_ModuleService.Query<SysModule>(r => true);
            //差集权限
            IEnumerable<SysModule> exceptActions = allActions.Except(saveActions, new Model.CustomizedClass.ActionCompare());
            //IEnumerable<Model.Action> exceptActions = (from a in allActions
            //                                           select a).Except(from b in saveActions select b);


            //已有权限-菜单权限设置
            IEnumerable<Model.CustomizedClass.TreeAction> saveMenus = saveActions.Select(t => new Model.CustomizedClass.TreeAction
            {
                id = t.ModuleNum,
                pId = t.Pnum,
                name = t.ModuleName,
                @checked = true,
                isHidden = false,
                type = 1
                //icon = "../../Content/zTree/css/zTreeStyle/actions.png"
            });

            ////已有权限-执行权限设置
            //IEnumerable<Model.CustomizedClass.TreeAction > saveBtns = saveActions.Where(a => a.ActionTypeEnum == 3).Select(t => new Model.CustomizedClass.TreeAction
            //{
            //    id = t.Num,
            //    pId = t.PNum,
            //    name = t.Name,
            //    @checked = false,
            //    isHidden = true,
            //    isSave = true,
            //    icon = "../../Content/zTree/css/zTreeStyle/actions.png"
            //});
            ////已有权限-水泵命令权限
            //IEnumerable<Model.CustomizedClass.TreeAction> savePumpBtns = saveActions.Where(a => a.ActionTypeEnum == 4).Select(t => new Model.CustomizedClass.TreeAction
            //{
            //    id = t.Num,
            //    pId = t.PNum,
            //    name = t.Name,
            //    @checked = true,
            //    isHidden = false,
            //    icon = "../../Content/zTree/css/zTreeStyle/actions.png"
            //    //  isSave = true
            //});
            ////已有权限-数据大屏权限
            //IEnumerable<Model.CustomizedClass.TreeAction> saveScreens = saveActions.Where(a => a.ActionTypeEnum == 5).Select(t => new Model.CustomizedClass.TreeAction
            //{
            //    id = t.Num,
            //    pId = t.PNum,
            //    name = t.Name,
            //    @checked = true,
            //    isHidden = false,
            //    icon = "../../Content/zTree/css/zTreeStyle/actions.png"
            //    //  isSave = true
            //});

            //差集权限-菜单权限设置
            IEnumerable<Model.CustomizedClass.TreeAction> exceptMenus = exceptActions.Select(t => new Model.CustomizedClass.TreeAction
            {
                id = t.ModuleNum,
                pId = t.Pnum,
                name = t.ModuleName,
                @checked = false,
                isHidden = false,
                type = 1
                //icon = "../../Content/zTree/css/zTreeStyle/actions.png"
            });

            ////差集权限-执行权限设置
            //IEnumerable<Model.CustomizedClass.TreeAction> exceptBtns = exceptActions.Where(a => a.ActionTypeEnum == 3).Select(t => new Model.CustomizedClass.TreeAction
            //{
            //    id = t.Num,
            //    pId = t.PNum,
            //    name = t.Name,
            //    @checked = false,
            //    isHidden = true,
            //    isSave = false,
            //    icon = "../../Content/zTree/css/zTreeStyle/actions.png"
            //});

            ////差集权限-水泵命令权限
            //IEnumerable<Model.CustomizedClass.TreeAction> exceptPumpBtns = exceptActions.Where(a => a.ActionTypeEnum == 4).Select(t => new Model.CustomizedClass.TreeAction
            //{
            //    id = t.Num,
            //    pId = t.PNum,
            //    name = t.Name,
            //    @checked = false,
            //    isHidden = false,
            //    icon = "../../Content/zTree/css/zTreeStyle/actions.png"
            //});
            ////差集权限-数据大屏权限
            //IEnumerable<Model.CustomizedClass.TreeAction> exceptScreens = exceptActions.Where(a => a.ActionTypeEnum == 5).Select(t => new Model.CustomizedClass.TreeAction
            //{
            //    id = t.Num,
            //    pId = t.PNum,
            //    name = t.Name,
            //    @checked = false,
            //    isHidden = false,
            //    icon = "../../Content/zTree/css/zTreeStyle/actions.png"
            //});

            //IEnumerable<Model.CustomizedClass.TreeAction> allAction = saveMenus.Union<Model.CustomizedClass.TreeAction>(saveBtns).Union<Model.CustomizedClass.TreeAction>(exceptMenus).Union<Model.CustomizedClass.TreeAction>(exceptBtns).Union<Model.CustomizedClass.TreeAction>(exceptPumpBtns).Union<Model.CustomizedClass.TreeAction>(savePumpBtns).Union<Model.CustomizedClass.TreeAction>(exceptScreens).Union<Model.CustomizedClass.TreeAction>(saveScreens);
            IEnumerable<Model.CustomizedClass.TreeAction> allAction = saveMenus.Union<Model.CustomizedClass.TreeAction>(exceptMenus);
            ViewBag.TreeNodes = new HtmlString(Newtonsoft.Json.JsonConvert.SerializeObject(allAction));
            return View();
        }
        #endregion

        #region 分配用户信息
        public IActionResult AllotUserRole(int id)
        {
            //部门信息
            var depInfo = _sys_DepartMentService.Query<SysDepartMent>(r => true).ToList();
            var list = _sys_DepartMentService.Query<SysDepartMent>(t => true).OrderBy(r => r.Sort);
            List<dynamic> listTree = new List<dynamic>() { };
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
            ViewBag.RoleID = id;
            //List<AllotUserRole> allList = this.userService.GetStation(id).ToList();
            //ViewBag.Station = allList;
            //ViewBag.UserID = id;
            return View();
        }
        //根据部门获取用户
        [HttpPost]
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult LoadUserByDep(int id, int roleid)
        {
            var userindo = _sys_RoleService.GetUserByDep(id, roleid).ToList();
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(userindo));
        }
        //分配用户
        [TypeFilter(typeof(IgonreActionFilter))]
        public ActionResult SaveAllotUser(int RoleID, string strUserID, string key)
        {
            var UserIDlist = strUserID.Split(',');

            try
            {
                foreach (var item in UserIDlist)
                {
                    int suID = int.Parse(item);
                    SysUserRole sysUserRole = _sys_UserRoleService.Query<SysUserRole>(r => r.UserId == suID && r.Role == RoleID).FirstOrDefault();
                    if (key == "Add")
                    {
                        if (sysUserRole == null)
                        {
                            SysUserRole sysUserRole1 = new SysUserRole();
                            sysUserRole1.Role = RoleID;
                            sysUserRole1.UserId = suID;
                            _sys_UserRoleService.Insert<SysUserRole>(sysUserRole1);
                            if (RoleID == 2)
                            {
                                _sys_RoleService.InsertPosition(suID);
                            }
                        }
                    }
                    else
                    {
                        if (sysUserRole != null)
                        {
                            _sys_UserRoleService.Delete<SysUserRole>(sysUserRole);
                        }
                    }
                }
                return Content("ok");
            }
            catch (Exception e)
            {
                return Content("no");
            }
        }
        #endregion
    }
}