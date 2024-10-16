using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.Extensions.Logging;

namespace WNMS.Application.Utility.Filters
{
    public class GlobalActionFilter : IActionFilter
    {

        private Cache.ICache cache;
        private readonly ILogger<GlobalActionFilter> logger = null;
        public GlobalActionFilter(Cache.ICache myCache, ILogger<GlobalActionFilter> myLogger)
        {
            logger = myLogger;
            cache = myCache;
        }
        void IActionFilter.OnActionExecuting(ActionExecutingContext context)
        {
            // logger.LogInformation("GlobalActionFilter");
            var ignoreLogin = context.ActionDescriptor.FilterDescriptors.Select(f => f.Filter)
              .OfType<TypeFilterAttribute>()
              .Any(f => f.ImplementationType.Equals(typeof(IgonreLoginFilter)));

            if (!ignoreLogin)
            {
                string userID = context.HttpContext.User.FindFirstValue("UserID");
                var isAdmin = Convert.ToBoolean(context.HttpContext.User.FindFirstValue("IsAdmin"));

                if (userID == null)
                {
                    context.Result = new ContentResult
                    {
                        Content = "<script type='text/javascript'>window.top.location='/Login/Index';</script>",
                        ContentType = "text/html",
                    };

                    return;
                }
                else
                {

                    if (!isAdmin)
                    {
                        var ignoreAction = context.ActionDescriptor.FilterDescriptors.Select(f => f.Filter)
                                          .OfType<TypeFilterAttribute>()
                                          .Any(f => f.ImplementationType.Equals(typeof(IgonreActionFilter)));

                        if (!ignoreAction)
                        {
                            var httpContent = context.HttpContext;

                            var area = context.ActionDescriptor.RouteValues["area"];
                            var controllerName = context.ActionDescriptor.RouteValues["controller"];
                            var actionName = context.ActionDescriptor.RouteValues["action"].ToString();

                            string url = "";

                            if (area == null)
                            {
                                url = $"/{controllerName.ToLower()}/{actionName.ToLower()}";
                            }
                            else
                            {
                                url = $"/{area.ToLower()}/{controllerName.ToLower()}/{actionName.ToLower()}";
                            }

                            //string url = httpContent.Request.Path.ToString().ToLower();
                            string httpMethod = httpContent.Request.Method.ToLower();

                            var cacheModules = cache.Get<List<Model.CustomizedClass.CacheModules>>("menu" + userID);
                            var model = cacheModules.Where(c => c.URL.ToLower().Trim() == url && c.HttpMethod.ToLower().Trim() == httpMethod).FirstOrDefault();

                            if (model == null)
                            {
                                throw new Exception(url + "暂无权限");
                            }
                        }
                    }


                }
            }
        }

        void IActionFilter.OnActionExecuted(ActionExecutedContext context)
        {
            //throw new NotImplementedException();
        }
    }
}
