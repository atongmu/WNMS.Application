using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WNMS.IService;
using WNMS.Model.DataModels;
using WNMS.Application.Utility;
using WNMS.Model.CustomizedClass;
using System.Linq.Expressions;
using WNMS.Utility;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using WNMS.Application.Utility.Filters;

namespace WNMS.Application.Areas.Sws.Controllers
{
    [Area("Sws")]
    public class Sws_GUIInfoController : Controller
    {
        #region 属性 构造函数
        private ISws_GUIInfoService guiService = null;
        private ISys_DataItemDetailService dataItemDetailService = null;
        private ISws_StationService stationService = null;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public Sws_GUIInfoController(ISws_GUIInfoService sws_GUIInfoService, ISys_DataItemDetailService sys_DataItemDetailService,
             ISws_StationService sws_StationService, IWebHostEnvironment webHostEnvironment)
        {
            guiService = sws_GUIInfoService;
            dataItemDetailService = sys_DataItemDetailService;
            _webHostEnvironment = webHostEnvironment;
            stationService = sws_StationService;
        }
        #endregion

        #region 数据查询
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [TypeFilter(typeof(IgonreActionFilter))]
        public async Task<IActionResult> GetData(string name, string order, string sortName, int? pageSize, int? pageIndex)
        {
            #region 查询条件
            Expression<Func<GUIInfo, bool>> funcWhere = null;
            if (!string.IsNullOrWhiteSpace(name))
            {
                funcWhere = funcWhere.And(c => c.GUIName.Contains(name));
            }
            #endregion

            int size = pageSize ?? 10;
            int index = pageIndex ?? 1;

            PageResult<GUIInfo> guiList = this.guiService.GetGUIData(funcWhere, size, index, sortName, order);
            PartialView("_GUITable", guiList.DataList);
            string dataTable = await ViewToString.RenderPartialViewToString(this, "_GUITable");
            return Json(new
            {
                total = guiList.TotalCount,
                pageIndex = guiList.PageIndex,
                pageSize = guiList.PageSize,
                order = order,
                sortName = sortName ?? "",
                dataTable = dataTable
            });
        }
        #endregion

        #region 工艺图编辑
        public IActionResult AddGUIPage()
        {
            //设备类型列表
            List<SysDataItemDetail> detailList = this.dataItemDetailService.Query<SysDataItemDetail>(r => r.FItemId == 5).ToList();
            ViewBag.Detail = detailList;
            var GuiType = dataItemDetailService.Query<SysDataItemDetail>(r => r.FItemId == 30).ToList();
            ViewBag.GuiType = GuiType;
            return View("SetGUI", new SwsGuiinfo());
        }

        public IActionResult EditGUIPage(int? id)
        {
            //根据id获取工艺图信息
            SwsGuiinfo gui = new SwsGuiinfo();
            if (id != null)
            {
                gui = this.guiService.Find<SwsGuiinfo>((int)id) ?? new SwsGuiinfo();
            }

            //获取设备类型列表
            List<SysDataItemDetail> detailList = new List<SysDataItemDetail>();
            if (gui.DeviceType == 1)
            {
                detailList = this.dataItemDetailService.Query<SysDataItemDetail>(r => r.FItemId == 5).ToList();
            }
            if (gui.DeviceType == 2)
            {
                detailList = this.dataItemDetailService.Query<SysDataItemDetail>(r => r.FItemId == 14).ToList();
            }
            ViewBag.Detail = detailList;
            var GuiType = dataItemDetailService.Query<SysDataItemDetail>(r => r.FItemId == 30).ToList();
            ViewBag.GuiType = GuiType;
            return View("SetGUI", gui);
        }

        //获取设备类型
        [HttpPost]
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult GetType(int devicetype)
        {
            List<SysDataItemDetail> list = new List<SysDataItemDetail>();
            if (devicetype == 1)
            {
                list = this.dataItemDetailService.Query<SysDataItemDetail>(r => r.FItemId == 5).ToList();
            }
            if (devicetype == 2)
            {
                list = this.dataItemDetailService.Query<SysDataItemDetail>(r => r.FItemId == 14).ToList();
            }
            return Json(new
            {
                list = list
            });
        }

        //图片上传 
        [TypeFilter(typeof(IgonreActionFilter))]
        public ActionResult UpLoadImg(IFormFile file)
        {
            string imgPath = "\\UploadImg\\GuiPicture\\";
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

        //提交表单
        [HttpPost]
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult SetGUIInfo(SwsGuiinfo gui)
        {
            if (gui.Num == 0)    //插入
            {
                List<SwsGuiinfo> guiList = this.guiService.Query<SwsGuiinfo>(u => u.DeviceType == gui.DeviceType && u.EquipType == gui.EquipType && u.PumpNum == gui.PumpNum && u.IsSum == 2).ToList();
                if (guiList.Count > 0 && gui.IsSum == 2)
                {
                    return Content("false");
                }
                else
                {
                    //构造guiID
                    int? uId = this.guiService.Query<SwsGuiinfo>(c => true)?.Max(c => c.Num);
                    gui.Num = uId + 1 ?? 1;
                    SwsGuiinfo newgui = this.guiService.Insert(gui);
                    if (newgui != null)
                    {
                        return Content("ok");
                    }
                    else
                    {
                        return Content("no");
                    }
                }
            }
            else    //修改
            {
                SwsGuiinfo guiDB = this.guiService.Query<SwsGuiinfo>(r => r.Num == gui.Num).AsNoTracking().FirstOrDefault();
                if (guiDB == null)
                {
                    return Content("false");
                }
                else
                {
                    if (this.guiService.Update(gui))
                    {
                        return Content("ok");
                    }
                    else
                    {
                        return Content("no");
                    }
                }
            }
        }

        [HttpPost]
        public IActionResult DeleteGUI(int? guiId)
        {
            if (this.guiService.DeleteGUI((int)guiId) > 0)
            {
                return Content("ok");
            }
            else
            {
                if (this.guiService.DeleteGUI((int)guiId) == 0)
                {
                    return Content("false");
                }
                else
                {
                    return Content("no");
                }
            }
        }
        #endregion
    }
}