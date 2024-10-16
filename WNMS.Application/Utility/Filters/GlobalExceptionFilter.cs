using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WNMS.Utility.Log;

namespace WNMS.Application.Utility.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {


        private readonly IModelMetadataProvider _moprovider;
        private readonly ILogger<GlobalExceptionFilter> logger = null;
        public GlobalExceptionFilter(IModelMetadataProvider moprovider, ILogger<GlobalExceptionFilter> myLogger)
        {
            this._moprovider = moprovider;
            logger = myLogger;

        }

        void IExceptionFilter.OnException(ExceptionContext context)
        {
            //if (context.Exception.GetType() == typeof(BusException))
            //{
            //    //如果是自定义异常，则不做处理
            //}
            //else
            //{

            //}


            Exception ex = context.Exception;

            LogUnit.ErrorLog(context.Exception.Message, ex);
            // logger.LogInformation(context.Exception.Message);
            ViewResult view = new ViewResult();

            view.ViewName = "Error";

            view.ViewData = new ViewDataDictionary(_moprovider, context.ModelState);
            view.ViewData.Add("Msg", ex.Message);
            view.ViewData.Add("Stack", ex.StackTrace);
            view.ViewData.Add("Code", context.HttpContext.Response.StatusCode = 500);


            context.Result = view;
            context.ExceptionHandled = true;
        }
    }
}
