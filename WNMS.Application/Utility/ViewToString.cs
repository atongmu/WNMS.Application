using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WNMS.Application.Utility
{
    public class ViewToString
    {
        public static async Task<string> RenderPartialViewToString(Controller controller, string partialViewName)
        {          
            IViewEngine viewEngine = controller.HttpContext.RequestServices.GetService(typeof(ICompositeViewEngine)) as ICompositeViewEngine;
            IView view = viewEngine.FindView(controller.ControllerContext, partialViewName,false).View;
            using (StringWriter writer = new StringWriter())
            {
                ViewContext viewContext = new ViewContext(controller.ControllerContext, view, controller.ViewData, controller.TempData, writer,new HtmlHelperOptions());
                await viewContext.View.RenderAsync(viewContext);
                //return writer.ToString();
                return writer.GetStringBuilder().ToString();
            }
        }
    }
}
