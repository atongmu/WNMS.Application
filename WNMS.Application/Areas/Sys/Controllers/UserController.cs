using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
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
using WNMS.Application.Utility.Filters;
using Microsoft.EntityFrameworkCore;
using WNMS.Model.GPRSModels;

namespace WNMS.Application.Areas.Sys.Controllers
{
    [Area("Sys")]
    public class UserController : Controller
    {
        #region 属性 构造函数
        private ISysUserService userService = null;
        private ISys_DepartMentService departmentService = null;
        private ISys_UserModuleService usermoduleService = null;
        private ISys_ModuleService moduleService = null;
        private ISys_ModuleButtonService moduleButtonService = null;
        private ISws_StationService stationService = null;
        private ISws_UserStationService userStationService = null;

        public UserController(ISysUserService sys_UserService, ISys_DepartMentService sys_DepartmentService,
            ISys_UserModuleService sys_UserModuleService, ISys_ModuleService sys_ModuleService,
            ISys_ModuleButtonService sys_ModuleButtonService, ISws_StationService sws_StationService,
            ISws_UserStationService sws_UserStationService)
        {
            userService = sys_UserService;
            departmentService = sys_DepartmentService;
            usermoduleService = sys_UserModuleService;
            moduleService = sys_ModuleService;
            moduleButtonService = sys_ModuleButtonService;
            stationService = sws_StationService;
            userStationService = sws_UserStationService;
        }
        #endregion

        #region 数据查询
        public IActionResult Index()
        {
            //部门列表
            List<SysDepartMent> departmentList = this.departmentService.Query<SysDepartMent>(c => true).ToList();
            ViewBag.Department = departmentList;
            return View();
        }

        [HttpPost]
        [TypeFilter(typeof(IgonreActionFilter))]
        public async Task<IActionResult> GetDate(string name, int department, string order, string sortName, int? pageSize, int? pageIndex)
        {
            #region 查询条件
            Expression<Func<DptUser, bool>> funcWhere = null;
            if (!string.IsNullOrWhiteSpace(name))
            {
                funcWhere = funcWhere.And(c => c.NickName.Contains(name));
            }
            if (department != 0)
            {
                funcWhere = funcWhere.And(c => c.Department == department);
            }
            #endregion

            int size = pageSize ?? 20;
            int index = pageIndex ?? 1;

            PageResult<DptUser> userList = this.userService.GetUserData(funcWhere, size, index, sortName, order);
            PartialView("_UserTable", userList.DataList);
            string dataTable = await ViewToString.RenderPartialViewToString(this, "_UserTable");
            return Json(new
            {
                total = userList.TotalCount,
                pageIndex = userList.PageIndex,
                pageSize = userList.PageSize,
                order = order,
                sortName = sortName ?? "",
                dataTable = dataTable
            });
        }

        [HttpPost]
        [TypeFilter(typeof(IgonreActionFilter))]
        public ActionResult UpdateStatus(int userId, bool status)
        {
            var info = userService.Find<SysUser>(userId);
            if (info != null)
            {
                info.IsEnable = status;
                if (userService.Update<SysUser>(info))
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

        #region 用户编辑
        public IActionResult AddUserPage()
        {
            //部门列表
            List<SysDepartMent> departmentList = this.departmentService.Query<SysDepartMent>(c => true).ToList();
            ViewBag.Department = departmentList;

            return View("SetUser", new SysUser());
        }

        public IActionResult EditUserPage(int? id)
        {
            //部门列表
            List<SysDepartMent> departmentList = this.departmentService.Query<SysDepartMent>(c => true).ToList();
            ViewBag.Department = departmentList;

            SysUser user = new SysUser();
            if (id != null)
            {
                user = this.userService.Find<SysUser>((int)id) ?? new SysUser();
            }
            ViewBag.UserId = id;
            return View("SetUser", user);
        }

        [HttpPost]
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult SetUserInfo(SysUser user)
        {
            if (user.UserId == 0)    //插入
            {
                List<SysUser> userList = this.userService.Query<SysUser>(u => u.Account == user.Account).ToList();
                if (userList.Count > 0)
                {
                    return Content("false");
                }
                else
                {
                    //加密
                    string passWord = WNMS.Utility.Encrypt.Encrypt.MD5Encoding("123456", "WNMS@Standard");   //加密;
                    //构造userID
                    int? uId = this.userService.Query<SysUser>(c => true)?.Max(c => c.UserId);
                    user.Account = WNMS.Utility.Encrypt.Encrypt.MD5Encoding(user.Account, "WNMS@Standard");  //用户名加密 
                    user.UserId = uId + 1 ?? 1;
                    user.Password = passWord;
                    user.LitmitTime = 0;
                    user.CreateTime = DateTime.Now;
                    user.ErrorTimes = 0;
                    user.HeadIcon = @"\lib\img\UserIcon.png";
                    user.IsAdmin = false;
                    user.IsEnable = true;
                    user.IsLock = false;
                    user.Imei = "666";
                    SysUser newuser = this.userService.Insert(user);
                    if (newuser != null)
                    {
                        return Content("ok");
                    }
                    else
                    {
                        return Content("no");
                    }
                }
            }
            else    //修改
            {
                SysUser userDB = this.userService.Find<SysUser>(user.UserId);
                if (userDB == null)
                {
                    return Content("false");
                }
                else
                {
                    userDB.Account = user.Account;
                    userDB.Department = user.Department;
                    userDB.Email = user.Email;
                    userDB.Gender = user.Gender;
                    userDB.NickName = user.NickName;
                    userDB.Phone = user.Phone;
                    userDB.Remark = user.Remark;
                    userDB.HeadIcon = user.HeadIcon;
                    userDB.Imei = user.Imei;
                    userDB.WeChatKey = user.WeChatKey; 
                    if (this.userService.Update(userDB))
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
        public IActionResult DeleteUser(int? userId)
        {
            if (userId == null)
            {
                return Content("false");
            }
            else
            {
                if (this.userService.DeleteUser((int)userId) > 0)
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

        #region 分配用户权限
        /// <summary>
        /// 获取第一步的权限 加载视图
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <returns></returns>
        public IActionResult UserAction(int id)
        {
            ViewBag.UserID = id;
            //已有权限
            var ids = usermoduleService.Query<SysUserModule>(r => r.UserId == id && r.Type == 1).Select(r => r.ModuleId).ToList();
            IEnumerable<SysModule> saveActions = moduleService.Query<SysModule>(r => ids.Contains(r.ModuleNum));
            //全部权限
            IEnumerable<SysModule> allActions = moduleService.Query<SysModule>(r => r.IsEnable == true);
            //差集权限
            IEnumerable<SysModule> exceptActions = allActions.Except(saveActions, new Model.CustomizedClass.ActionCompare());


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


            IEnumerable<Model.CustomizedClass.TreeAction> allAction = saveMenus.Union<Model.CustomizedClass.TreeAction>(exceptMenus);
            ViewBag.TreeNodes = new HtmlString(Newtonsoft.Json.JsonConvert.SerializeObject(allAction));
            return View();
        }
        //根据第一步的权限 获取第二步的权限
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult LoadButAction(int userId, string actionids)
        {
            if (string.IsNullOrEmpty(actionids))
            {
                return Content("");
            }

            //已有按钮权限
            var ids = usermoduleService.Query<SysUserModule>(r => r.UserId == userId && r.Type == 2).Select(r => r.ModuleId).ToList();
            IEnumerable<SysModuleButton> saveActions = moduleButtonService.Query<SysModuleButton>(r => ids.Contains(r.ModuleButtonId));
            List<int> list = null;
            if (!string.IsNullOrEmpty(actionids))
            {
                list = new List<int>(new List<string>(actionids.Split(',')).Select(a => int.Parse(a)));
            }

            //所选的全部权限           
            IEnumerable<SysModuleButton> allActions = moduleButtonService.Query<SysModuleButton>(r => list.Contains(r.ModuleId));
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
            IEnumerable<SysModule> PallActions = moduleService.Query<SysModule>(r => list.Contains(r.ModuleNum));
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
        /// <param name="userId">用户ID</param>
        /// <param name="actionids">模块权限</param>
        /// <param name="btnActions">按钮权限</param>
        /// <returns></returns>
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult SetUserAction(int userId, string actionids, string btnActions)
        {
            List<SysUserModule> actionlist = usermoduleService.Query<SysUserModule>(r => r.UserId == userId).AsNoTracking().ToList();
            //type=1 权限
            List<int> list = null;
            if (!string.IsNullOrEmpty(actionids))
            {
                list = new List<int>(new List<string>(actionids.Split(',')).Select(a => int.Parse(a)));
            }

            //type=2 权限
            List<int> btnlist = null;
            if (!string.IsNullOrEmpty(btnActions))
            {
                btnlist = new List<int>(new List<string>(btnActions.Split(',')).Select(a => int.Parse(a)));
            }
            List<SysUserModule> module = new List<SysUserModule>();
            if (list != null)
            {
                foreach (var item in list)
                {
                    SysUserModule sysUserModule = new SysUserModule();
                    sysUserModule.UserId = userId;
                    sysUserModule.ModuleId = item;
                    sysUserModule.Type = 1;
                    module.Add(sysUserModule);
                }
            }
            if (btnlist != null)
            {
                foreach (var item in btnlist)
                {
                    SysUserModule sysUserModule = new SysUserModule();
                    sysUserModule.UserId = userId;
                    sysUserModule.ModuleId = item;
                    sysUserModule.Type = 2;
                    module.Add(sysUserModule);
                }
            }
            if ((actionlist == null || actionlist.Count() == 0) && string.IsNullOrEmpty(btnActions) && string.IsNullOrEmpty(actionids))
            {
                return Content("false");
            }
            else
            {
                if (userService.SetModule(module, userId) >= 0)
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

        #region  分配泵房
        public IActionResult AllotStation(int id)
        {

            List<AllotStation> allList = this.userService.GetStation(id).ToList();
            ViewBag.Station = allList;
            ViewBag.UserID = id;
            return View();
        }
        [TypeFilter(typeof(IgonreActionFilter))]
        public ActionResult SaveAllotRTU(int UserID, string StationID, string key)   //分配设备提交
        {
            var StationIDlist = StationID.Split(',');

            try
            {
                foreach (var item in StationIDlist)
                {
                    int stationID = Convert.ToInt32(item);
                    SwsUserStation userStations = this.userStationService.Query<SwsUserStation>(r => r.StationId == stationID && r.UserId == UserID).FirstOrDefault();
                    if (key == "Add")
                    {
                        if (userStations == null)
                        {
                            SwsUserStation userStation = new SwsUserStation();
                            userStation.StationId = stationID;
                            userStation.UserId = UserID;
                            userStation.FocusOn = false;
                            userStationService.Insert<SwsUserStation>(userStation);
                        }
                    }
                    else
                    {
                        if (userStations != null)
                        {
                            userStationService.Delete<SwsUserStation>(userStations);
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