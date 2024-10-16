using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WNMS.Application.Utility.Filters;
using WNMS.IService;
using WNMS.Application.Utility.Cache;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using WNMS.Application.Utility.ValidateCode;
using WNMS.Application.Utility.JsonHelper;
using WNMS.Model.CustomizedClass;
using Microsoft.AspNetCore.Hosting;

namespace WNMS.Application.Controllers
{


    [TypeFilter(typeof(IgonreLoginFilter))]
    public class LoginController : Controller
    {

        private readonly ILogger<LoginController> logger = null;
        private ISysUserService userService = null;
        private ISys_ModuleService moduleService = null;
        private ICache cache = null;
        private readonly IWebHostEnvironment webHostEnvironment;

        public LoginController(
            ILogger<LoginController> myLogger,
            ISysUserService sysUserService,
            ICache _cache,
            ISys_ModuleService _moduleService,
            IWebHostEnvironment _webHostEnvironment
            )
        {
            logger = myLogger;
            userService = sysUserService;
            cache = _cache;
            moduleService = _moduleService;
            webHostEnvironment = _webHostEnvironment;
        }
        public IActionResult Index()
        {
            //logger.LogTrace("开发阶段调试，可能包含敏感程序数据", 1);
            //logger.LogDebug("开发阶段短期内比较有用，对调试有益。");
            //logger.LogInformation("你访问了首页。跟踪程序的一般流程。");
            //logger.LogWarning("警告信息！因程序出现故障或其他不会导致程序停止的流程异常或意外事件。");
            //logger.LogError("错误信息。因某些故障停止工作");
            //logger.LogCritical("程序或系统崩溃、遇到灾难性故障！！！");
            var path = webHostEnvironment.ContentRootPath;
            JsonFileHelper jsonFileHelper = new JsonFileHelper(path+ "/Utility/ProjectJson/project.json");
            SystemInfo st = jsonFileHelper.Read<SystemInfo>("SystemInfo");
            string name = st.SystemName == "" ? "智慧水务管理平台" : st.SystemName;
            string ename = st.SystemEngName == "" ? "smart water system platform" : st.SystemEngName;

            ViewBag.SystemName = name;
            ViewBag.SystenEName = ename;
            return View();
        }

        public async Task<IActionResult> CheckLogin(string userName, string passWord, string code)
        {
            passWord = WNMS.Utility.Encrypt.Encrypt.MD5Encoding(passWord, "WNMS@Standard");
            userName = WNMS.Utility.Encrypt.Encrypt.MD5Encoding(userName, "WNMS@Standard");
            var user = userService.Query<WNMS.Model.DataModels.SysUser>(u => u.Account == userName && u.Password == passWord && u.IsEnable == true).FirstOrDefault();

            //验证码
            string codestr = HttpContext.Session.GetString("validateCode");

            if (user != null)
            {
                if (user.IsLock)
                {
                    return Content("lock");
                }
                else
                {
                    if (code != codestr)
                    {
                        return Content("code");
                    }
                    else
                    {
                        var claims = new List<Claim>
                        {
                            new Claim("UserID",user.UserId.ToString()),
                            new Claim("UserName",user.Account.ToString()),
                            new Claim("NickName", user.NickName==null?user.Account:user.NickName),
                            new Claim("IsAdmin",user.IsAdmin.ToString())
                        };

                        var claimsIdentity = new ClaimsIdentity(
                            claims, CookieAuthenticationDefaults.AuthenticationScheme);

                        var authProperties = new AuthenticationProperties
                        {
                            ExpiresUtc = DateTimeOffset.UtcNow.Add(TimeSpan.FromMinutes(120)),
                            //ExpiresUtc = DateTime.UtcNow.AddMinutes(20),
                            IsPersistent = true,//在浏览器持久化，false的时候走session持久化
                            AllowRefresh = true//动态刷新令牌
                        };

                        await HttpContext.SignInAsync(
                               CookieAuthenticationDefaults.AuthenticationScheme,
                               new ClaimsPrincipal(claimsIdentity),
                               authProperties);

                        //HttpContext.Session.SetInt32("user", user.UserId);
                        HttpContext.Session.SetString("userName", user.Account);
                        logger.LogInformation("登录成功");

                        var userModuleTask = moduleService.UserModules(user.UserId);
                        var roleModuleTask = moduleService.RoleModules(user.UserId);

                        //userModuleTask.Start();
                        //roleModuleTask.Start();

                        //Task.WaitAll(userModuleTask, roleModuleTask);

                        //var list = userModuleTask.Result.Union(roleModuleTask.Result).ToList();

                        var list = userModuleTask.Union(roleModuleTask).ToList();

                        cache.Add($"menu{user.UserId}", list);

                        return Content("ok");
                    }
                    //HttpContext.Items["UserName"] = user.Account.ToString();                 
                }
            }
            else
            {
                return Content("no");
            }
        }


        public async Task<IActionResult> LoginOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("Index");
        }

        //验证码
        public ActionResult ShowValidateCode()
        {
            ValidateCode validateCode = new ValidateCode();
            string code = validateCode.CreateValidateCode(4);
            HttpContext.Session.SetString("validateCode", code);
            byte[] buffer = validateCode.CreateValidateGraphic(code);
            return File(buffer, "image/jpeg");
        }
    }
}