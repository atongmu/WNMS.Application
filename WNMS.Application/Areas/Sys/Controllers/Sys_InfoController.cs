using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WNMS.Application.Utility.JsonHelper;
using WNMS.Model.CustomizedClass;

namespace WNMS.Application.Areas.Sys.Controllers
{
    [Area("sys")]
    public class Sys_InfoController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public Sys_InfoController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            var path = _webHostEnvironment.ContentRootPath;
            JsonFileHelper jsonFileHelper = new JsonFileHelper(path + "/Utility/ProjectJson/project.json");
            SystemInfo st = jsonFileHelper.Read<SystemInfo>("SystemInfo");
            ViewBag.SystemInfo = st;
            return View();
        }

        public IActionResult EditPage()
        {
            var path = _webHostEnvironment.ContentRootPath;
            JsonFileHelper jsonFileHelper = new JsonFileHelper(path + "/Utility/ProjectJson/project.json");
            SystemInfo st = new SystemInfo();
            st = jsonFileHelper.Read<SystemInfo>("SystemInfo");
            if (st == null)
            {
                return View(new SystemInfo());
            }
            else
            {
                return View(st);
            }
        }

        //提交
        [HttpPost]
        public IActionResult SetSystemInfo(SystemInfo st)
        {
            var path = _webHostEnvironment.ContentRootPath;
            JsonFileHelper jsonFileHelper = new JsonFileHelper(path + "/Utility/ProjectJson/project.json");
            jsonFileHelper.Remove("SystemInfo");
            if(jsonFileHelper.Write<SystemInfo>("SystemInfo", st))
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }           
        }


        //图片上传 
        public ActionResult UpLoadImg(IFormFile file)
        {
            string imgPath = "\\UploadImg\\Logo\\";
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
            string fileName = file.FileName;
            string filePath = Path.Combine(dicPath, fileName);
            using (FileStream fs = System.IO.File.Create(filePath))
            {
                img.CopyTo(fs);
                fs.Flush();
            }
            return Content(imgPath + fileName);
        }
    }
}