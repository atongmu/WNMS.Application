using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NLog;
using WNMS.Application.Models;
using WNMS.Application.Utility.Filters;
using WNMS.Application.Utility.JsonHelper;
using WNMS.IService;
using WNMS.Model.CustomizedClass;
using WNMS.Model.DataModels;
using WNMS.Utility;
using static cn.jpush.api.report.UsersResult;

namespace WNMS.Application.Controllers
{
    [TypeFilter(typeof(IgonreActionFilter))]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> myLog;
        private IService.ISysUserService userService = null;
        private IService.ISys_ModuleService moduleService = null;
        private IService.ISws_EventInfoService _EventInfoService = null;
        private IService.ISys_DepartMentService dptService = null;
        private IService.ISys_DepartMentService _DepartMentService = null;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private ISws_AccessoriesMaintenanceService _sws_AccessoriesMaintenanceService = null;
        private ISws_ConsumpSettingService _ConsumpSettingService = null;
        private ISws_CameraService _CameraService = null;
        private IWO_WorkOrderService _wO_WorkOrderService = null;
        private IWO_WOExtensionService _wO_WOExtensionService = null;
        private IWO_ForwardService _wO_ForwardService = null;
        private IWO_InsExtensionService _wO_InsExtensionService = null;
        private IWO_InsForwardService _wO_InsForwardService = null;
        string signalrHubs = StaticConstraint.SignalrHubs;
        string webspeech = StaticConstraint.WebSpeech;
        public HomeController(
            ILogger<HomeController> logger,
            ISysUserService sysUserService,
            ISys_ModuleService sysModuleService,
            ISws_EventInfoService sws_EventInfoService,
            ISys_DepartMentService departMentService,
            ISys_DepartMentService sys_DepartMentService,
            ISws_AccessoriesMaintenanceService sws_AccessoriesMaintenanceService,
            IWebHostEnvironment webHostEnvironment,
            ISws_ConsumpSettingService sws_ConsumpSettingService,
            ISws_CameraService sws_CameraService, IWO_WorkOrderService wO_WorkOrderService,
            IWO_WOExtensionService wO_WOExtensionService, IWO_ForwardService wO_ForwardService,
            IWO_InsExtensionService wO_InsExtensionService,
            IWO_InsForwardService wO_InsForwardService

            )
        {
            myLog = logger;
            userService = sysUserService;
            moduleService = sysModuleService;
            dptService = departMentService;
            _EventInfoService = sws_EventInfoService;
            _DepartMentService = sys_DepartMentService;
            _webHostEnvironment = webHostEnvironment;
            _sws_AccessoriesMaintenanceService = sws_AccessoriesMaintenanceService;
            _ConsumpSettingService = sws_ConsumpSettingService;
            _CameraService = sws_CameraService;
            _wO_WorkOrderService = wO_WorkOrderService;
            _wO_WOExtensionService = wO_WOExtensionService;
            _wO_ForwardService = wO_ForwardService;
            _wO_InsExtensionService = wO_InsExtensionService;
            _wO_InsForwardService = wO_InsForwardService;
        }

        public IActionResult Index()
        {
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = userService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
            var dpt = dptService.Query<SysDepartMent>(d => d.DepartmentId == user.Department).FirstOrDefault();
            if (dpt != null)
                ViewBag.DptName = dpt.DptName;
            else
                ViewBag.DptName = "暂无部门";

            var modules = moduleService.QueryUserModules(userID);
            var menus = new Models.Modules.ModuleHelper(modules).ConvertToWebModule();


            ViewBag.AddTab = modules.Where(m => m.HttpMethod.ToLower() == "get" && m.Target == 2 && m.Url != "#").FirstOrDefault();

            ViewBag.Menus = new HtmlString(Newtonsoft.Json.JsonConvert.SerializeObject(menus));
            ViewBag.alramCount = _EventInfoService.GetAlarmCount(userID, user.IsAdmin);
            ViewBag.userimge = string.IsNullOrEmpty(user.HeadIcon) ? "../lib/img/UserIcon.png" : user.HeadIcon;
            ViewBag.MaintenanceCount = _sws_AccessoriesMaintenanceService.GetMaintenanceRtCount(userID, user.IsAdmin);
            short custate = (short)Model.CustomizedClass.WoState.未分派;
            ViewBag.WoCount = _wO_WorkOrderService.Query<WoWorkOrder>(r => r.CurrentState == custate).Count();//未分派工单数量

            ViewBag.WoExCount = _wO_WOExtensionService.Query<WoWoextension>(r => r.State == 3).Count();//工单延期数量
            var woFall = _wO_ForwardService.Query<WoForward>(r => r.State == 3);
            ViewBag.WoFor = woFall.Where(r => r.Type == 2).Count();//工单转发申请
            ViewBag.WoTurn = woFall.Where(r => r.Type == 1).Count();//工单退单申请

            var inFall = _wO_InsForwardService.Query<WoInsForward>(r => r.State == 3);
            ViewBag.InFor = inFall.Where(r => r.Type == 2).Count();//巡检转发申请
            ViewBag.InTurn = inFall.Where(r => r.Type == 1).Count();//巡检退单申请
            ViewBag.InExCount = _wO_InsExtensionService.Query<WoInsExtension>(r => r.State == 3).Count();

            var allcount = ViewBag.WoCount + ViewBag.WoExCount + ViewBag.WoFor + ViewBag.WoTurn + ViewBag.InFor + ViewBag.InTurn + ViewBag.InExCount + ViewBag.MaintenanceCount;
            ViewBag.Allcount = allcount;
            var path = _webHostEnvironment.ContentRootPath;
            JsonFileHelper jsonFileHelper = new JsonFileHelper(path + "/Utility/ProjectJson/project.json");
            SystemInfo st = jsonFileHelper.Read<SystemInfo>("SystemInfo");
            string name = st.SystemName == "" ? "智慧水务管理平台" : st.SystemName;
            string ename = st.SystemEngName == "" ? "smart water system platform" : st.SystemEngName;

            ViewBag.SystemName = name;
            ViewBag.SystenEName = ename;
            ViewBag.Logo = st.Logo;
            ViewBag.Logo2 = st.Logo2;
            ViewBag.signalrHubs = signalrHubs;
            #region 订阅事件
            if (user.IsAdmin)
            {
                ViewBag.SerialNum = "";
            }
            else
            {
                var SerialNums = _ConsumpSettingService.GetSerialNum(user.UserId).ToList();
                var cameraNum = "";
                if (SerialNums.Count() > 0)
                {
                    cameraNum = string.Join(",", SerialNums.Select(r => r.SerialNum));
                }
                ViewBag.SerialNum = cameraNum;
            }

            ////事件类型枚举
            //Dictionary<int, string> dic = new Dictionary<int, string>();
            //dic.Add(131329, "视频丢失");
            //dic.Add(131330, "视频遮挡");
            //dic.Add(131331, "移动侦测");
            //dic.Add(131612, "场景变更");
            //dic.Add(131588, "区域入侵");
            //ViewData["EventEnum"] = dic;

            #endregion
            ViewBag.IsAdmin = user.IsAdmin;
            ViewBag.userid = user.UserId;
            ViewBag.WebSpeech = webspeech;

            //myLog.LogInformation("登录成功");
            return View(user);
        }

        public async Task<IActionResult> CheckLoginLog()
        {
            
            return Content("ok");
        }

            [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult EditPassWord()
        {
            return View();
        }


        [HttpPost]
        public IActionResult SavePassWord(string oldPassWord, string myPassWord, string newPassWord)
        {
            int userID = int.Parse(User.FindFirstValue("UserID"));
            string old = WNMS.Utility.Encrypt.Encrypt.MD5Encoding(oldPassWord, "WNMS@Standard");
            var user = userService.Query<SysUser>(u => u.UserId == userID && u.Password == old).FirstOrDefault();
            if (user == null)
            {
                return Content("error");
            }
            else
            {
                user.Password = WNMS.Utility.Encrypt.Encrypt.MD5Encoding(myPassWord, "WNMS@Standard");
                var flag = userService.Update(user);
                if (flag)
                {
                    return Content("ok");
                }
                else
                {
                    return Content("no");
                }

            }
        }
        #region 登陆人员信息修改
        public IActionResult EditUserInfo()
        {
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = userService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
            //部门列表
            List<SysDepartMent> departmentList = this._DepartMentService.Query<SysDepartMent>(c => true).ToList();
            ViewBag.Department = departmentList;
            return View("EditUserInfo", user);
        }
        //图片上传 
        public ActionResult UpLoadImg(IFormFile file, string userid)
        {

            //if (Request.Form.Files.Count <= 0)
            //{
            //    return Content("请选择图片");
            //}
            //string name = Request.Query["address"];
            string imgPath = "\\UploadImg\\HeadIcon\\" + userid + "\\";
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

        #endregion
        #region 视频预览
        public IActionResult GetCameraDetail(string srcIndex)
        {
            var cameraInfo = _CameraService.Query<SwsCamera>(r => r.SerialNum == srcIndex).FirstOrDefault();
            return View("PreviewVideo", cameraInfo);
        }
        #endregion
    }
}
