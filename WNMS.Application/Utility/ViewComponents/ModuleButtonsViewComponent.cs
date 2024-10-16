using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WNMS.Application.Utility.ViewComponents
{
    public class ModuleButtonsViewComponent : ViewComponent
    {
        private IService.ISys_ModuleService moduleService;
        public ModuleButtonsViewComponent(IService.ISys_ModuleService sys_ModuleService)
        {
            moduleService = sys_ModuleService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            string area = RouteData.Values["area"].ToString().ToLower();
            string controllerName = RouteData.Values["controller"].ToString().ToLower();
            string actionName = RouteData.Values["action"].ToString().ToLower();

            string url = $"/{area}/{controllerName}/{actionName}";
            string method = Request.Method.ToString().ToLower();

            //int userID = UserClaimsPrincipal.FindFirstValue("UserID"));
            //获取
            //int userID = 1;
            int userID = int.Parse(UserClaimsPrincipal.FindFirstValue("UserID"));

            var buttons = moduleService.QueryUserButtons(userID, url, method).ToList();


            return View(buttons);
        }
    }
}
