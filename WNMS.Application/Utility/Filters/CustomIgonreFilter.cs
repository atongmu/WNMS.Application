using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WNMS.Application.Utility.Filters
{
    /// <summary>
    /// 忽略登录 Filter
    /// </summary>
    public class IgonreLoginFilter : IActionFilter
    {

        private readonly ILogger<IgonreLoginFilter> logger = null;
        public IgonreLoginFilter(ILogger<IgonreLoginFilter> myLogger)
        {

            logger = myLogger;

        }
        void IActionFilter.OnActionExecuted(ActionExecutedContext context)
        {
            //logger.LogInformation(context.ActionDescriptor.ToString());
        }

        void IActionFilter.OnActionExecuting(ActionExecutingContext context)
        {
            //logger.LogInformation(context.ActionDescriptor.ToString());
        }
    }

    /// <summary>
    /// 忽略 Action Filter
    /// </summary>
    public class IgonreActionFilter : IActionFilter
    {
        private readonly ILogger<IgonreActionFilter> logger = null;
        public IgonreActionFilter(ILogger<IgonreActionFilter> myLogger)
        {

            logger = myLogger;

        }
        void IActionFilter.OnActionExecuted(ActionExecutedContext context)
        {
            //logger.LogInformation(context.ActionDescriptor.ToString());
        }

        void IActionFilter.OnActionExecuting(ActionExecutingContext context)
        {
            //logger.LogInformation(context.ActionDescriptor.ToString());
        }
    }

}
