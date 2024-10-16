using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WNMS.Application.Services
{
    /// <summary>
    /// API权限验证
    /// </summary>
    public class BasicAuthorize : Attribute, IActionFilter
    {
        private ILogger<BasicAuthorize> _logger = null;
        public BasicAuthorize(ILogger<BasicAuthorize> logger)
        {
            this._logger = logger;
        }
        public void OnActionExecuting(ActionExecutingContext context)
        {
            this._logger.LogDebug("CustomAuthorityActionFilterAttribute Executing!");
            string userString = context.HttpContext.Session.GetString("CurrentUser");
            if (!string.IsNullOrWhiteSpace(userString))
            {
                CurrentUser currentUser = Newtonsoft.Json.JsonConvert.DeserializeObject<CurrentUser>(userString);
                this._logger.LogDebug($"CustomAuthorityActionFilterAttribute 权限检查通过了 {currentUser.Name}登陆了系统!");
            }
            else
            {
                context.Result = new RedirectResult("~/Fourth/Login");//短路器
            }
        }


        public void OnActionExecuted(ActionExecutedContext context)
        {
            this._logger.LogDebug("CustomAuthorityActionFilterAttribute Executed!");
        }


        //public override void OnAuthorization(AuthorizationFilterContext actionContext)
        //{
        //    var authorization = actionContext.HttpContext.Authorization;

        //    if (actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>(true).Count != 0
        //        || actionContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes<AllowAnonymousAttribute>(true).Count != 0)
        //    {   //如果有Anonymous  就不检查
        //        base.OnAuthorization(actionContext);
        //    }
        //    else if (authorization != null && authorization.Parameter != null)
        //    {
        //        //用户验证逻辑
        //        if (ValidateTicket(authorization.Parameter))
        //        {
        //            base.IsAuthorized(actionContext);
        //        }
        //        else
        //        {
        //            this.HandleUnauthorizedRequest(actionContext);
        //        }
        //    }
        //    else
        //    {
        //        this.HandleUnauthorizedRequest(actionContext);
        //    }
        //}


        //private bool ValidateTicket(string encryptTicket)
        //{
        //    //解密Ticket
        //    var strTicket = FormsAuthentication.Decrypt(encryptTicket).UserData;
        //    return string.Equals(strTicket, string.Format("{0}&{1}", "Admin", "123456"));//应该分拆后去数据库验证
        //}
    }
}
