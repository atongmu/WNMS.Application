using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using WNMS.Application.Utility.JsonHelper;
using WNMS.Model.CustomizedClass;

namespace WNMS.Application.Areas.Sws.Controllers
{
    [Area("Sws")]
    public class Sws_DataViewController : Controller
    {
      
        private readonly IWebHostEnvironment _webHostEnvironment;
        public Sws_DataViewController( 
             IWebHostEnvironment webHostEnvironment 
           )
        { 
            _webHostEnvironment = webHostEnvironment; 
        }
        public IActionResult Index()
        {
            var path = _webHostEnvironment.ContentRootPath;
            JsonFileHelper jsonFileHelper = new JsonFileHelper(path + "/Utility/ProjectJson/project.json");
            SystemInfo st = jsonFileHelper.Read<SystemInfo>("SystemInfo");
            ViewBag.lng = st.Lng;
            ViewBag.lat = st.Lat;
            ViewBag.zoom = st.MapLevel;
            ViewBag.sysName = st.SystemName;
            ViewBag.logo = st.Logo;
            ViewBag.AreaName = st.AreaName;
            return View();
        }
    }
}