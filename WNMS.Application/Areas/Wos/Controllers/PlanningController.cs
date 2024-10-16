using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WNMS.Application.Utility.Filters;
using WNMS.IService;
using WNMS.Model.DataModels;
using WNMS.Utility;

namespace WNMS.Application.Areas.Wos.Controllers
{
    [Area("Wos")]
    public class PlanningController : Controller
    {
        private IGD_InspectionService _gD_InspectionService;
        private ISysUserService _sysUserService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IGD_ResourceService _gD_ResourceService;
        private readonly IGD_EventsService _gD_EventsService;
        private readonly IGD_MaintainService _gD_MaintainService;
        private readonly IGD_RepairService _gD_RepairService;
        private readonly IGD_TeamUserService _gD_TeamUserService;
        public PlanningController(IGD_InspectionService gD_InspectionService,
            IWebHostEnvironment webHostEnvironment,
            IGD_ResourceService gD_ResourceService,
            ISysUserService sysUserService,
            IGD_EventsService gD_EventsService,
            IGD_MaintainService gD_MaintainService,
            IGD_RepairService gD_RepairService,
            IGD_TeamUserService gD_TeamUserService
            )
        {
            _gD_InspectionService = gD_InspectionService;
            _webHostEnvironment = webHostEnvironment;
            _sysUserService = sysUserService;
            _gD_ResourceService = gD_ResourceService;
            _gD_EventsService = gD_EventsService;
            _gD_MaintainService = gD_MaintainService;
            _gD_RepairService = gD_RepairService;
            _gD_TeamUserService = gD_TeamUserService;
        }
        public IActionResult Index()
        {
            return View();
        }
        //巡检计划制定
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult LoadInspection()
        {
            //获取客服信息 已存在班组中的人员
            var userids = _gD_TeamUserService.Query<GdTeamUser>(r => true).Select(r => r.UserId).ToList();
            var wbUser = _sysUserService.Query<SysUser>(t => userids.Contains(t.UserId)).ToList();
            ViewBag.wbUser = wbUser;
            ViewBag.nowTime = DateTime.Now;
            return View();
        }
        //巡检计划提交
        [TypeFilter(typeof(IgonreActionFilter))]
        [HttpPost]
        public async Task<IActionResult> SetInspectionInfo(string inspectioninfo, string eventinfo)
        {
            GdInspection gdInspection = Newtonsoft.Json.JsonConvert.DeserializeObject<GdInspection>(inspectioninfo);
            GdEvents gdEvents = Newtonsoft.Json.JsonConvert.DeserializeObject<GdEvents>(eventinfo);
            //巡检内容
            gdInspection.InspectionId = _sysUserService.QueryID("InspectionID", "GD_Inspection");
            gdInspection.CreateTime = DateTime.Now;
            gdInspection.InspectionTime = DateTime.Now;
            string year = DateTime.Now.Year.ToString();
            int count = _gD_InspectionService.Query<GdInspection>(r => true).Count() + 1;
            gdInspection.Num = "XJ" + "-" + year + "-" + count.ToString().PadLeft(7, '0');

            //事件内容
            gdEvents.IncidentId = ConvertDateTimeInt(DateTime.Now);
            gdEvents.IncidentType = 0;
            int evCount = _gD_EventsService.Query<GdEvents>(r => true).Count() + 1;
            gdEvents.IncidentNum = "EGXJ" + "-" + year + "-" + evCount.ToString().PadLeft(7, '0');
            //gdEvents.IncidentNum = "SJ" + DateTime.Now.ToString("yyyyMMddHHssmm");
            gdEvents.IncidentSource = 1;
            gdEvents.ReportTime = gdInspection.CreateTime;
            gdEvents.DisposeState = 1;
            gdEvents.AuditState = false;
            gdEvents.TaskId = gdInspection.InspectionId;
            gdEvents.IncidentContent = gdInspection.TaskDescription;
            if (_gD_InspectionService.AddEventIn(gdInspection, gdEvents) > 0)
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }
        }
        //巡检保养制定
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult LoadMaintain()
        {
            //获取客服信息 已存在班组中的人员
            var userids = _gD_TeamUserService.Query<GdTeamUser>(r => true).Select(r => r.UserId).ToList();
            var wbUser = _sysUserService.Query<SysUser>(t => userids.Contains(t.UserId)).ToList();
            ViewBag.wbUser = wbUser;
            ViewBag.nowTime = DateTime.Now;
            return View();
        }
        //保养计划提交
        [TypeFilter(typeof(IgonreActionFilter))]
        [HttpPost]
        public async Task<IActionResult> SetMaintainInfo(string inspectioninfo, string eventinfo)
        {
            GdMaintain gdInspection = Newtonsoft.Json.JsonConvert.DeserializeObject<GdMaintain>(inspectioninfo);
            GdEvents gdEvents = Newtonsoft.Json.JsonConvert.DeserializeObject<GdEvents>(eventinfo);
            //保养内容
            gdInspection.MaintainId = _sysUserService.QueryID("MaintainID", "GD_Maintain");
            gdInspection.CreateTime = DateTime.Now;
            gdInspection.MaintainTime = DateTime.Now;
            gdInspection.IsFeedback = false;
            string year = DateTime.Now.Year.ToString();
            int count = _gD_MaintainService.Query<GdMaintain>(r => true).Count();
            gdInspection.Num = "BY" + "-" + year + "-" + count.ToString().PadLeft(7, '0');
            //事件内容
            gdEvents.IncidentId = ConvertDateTimeInt(DateTime.Now);
            gdEvents.IncidentType = 1;
            int evCount = _gD_EventsService.Query<GdEvents>(r => true).Count() + 1;
            gdEvents.IncidentNum = "EGBY" + "-" + year + "-" + evCount.ToString().PadLeft(7, '0');
            //gdEvents.IncidentNum = "BY" + DateTime.Now.ToString("yyyyMMddHHssmm");
            gdEvents.IncidentSource = 1;
            gdEvents.ReportTime = DateTime.Parse(gdInspection.CreateTime.ToString());
            gdEvents.DisposeState = 1;
            gdEvents.AuditState = false;
            gdEvents.TaskId = gdInspection.MaintainId;
            gdEvents.IncidentContent = gdInspection.TaskDescription;
            if (_gD_InspectionService.AddEventMa(gdInspection, gdEvents) > 0)
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }
        }
        //维修上报
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult LoadRepair()
        {
            //获取客服信息 已存在班组中的人员
            var userids = _gD_TeamUserService.Query<GdTeamUser>(r => true).Select(r => r.UserId).ToList();
            var wbUser = _sysUserService.Query<SysUser>(t => userids.Contains(t.UserId)).ToList();
            ViewBag.wbUser = wbUser;
            ViewBag.nowTime = DateTime.Now;
            ViewBag.idflag = _sysUserService.QueryID("RepairID", "GD_Repair");
            return View();
        }
        //维修提交
        [TypeFilter(typeof(IgonreActionFilter))]
        [HttpPost]
        public async Task<IActionResult> SetRepairInfo(string inspectioninfo, string eventinfo)
        {
            GdRepair gdInspection = Newtonsoft.Json.JsonConvert.DeserializeObject<GdRepair>(inspectioninfo);
            GdEvents gdEvents = Newtonsoft.Json.JsonConvert.DeserializeObject<GdEvents>(eventinfo);
            //维修内容
            gdInspection.CreateTime = DateTime.Now;
            gdInspection.IsFeedback = false;
            gdInspection.ReportTime = gdInspection.CreateTime;
            gdInspection.RepairState = 0;
            gdInspection.RepairUser = 0;
            string year = DateTime.Now.Year.ToString();
            int count = _gD_RepairService.Query<GdRepair>(r => true).Count() + 1;
            gdInspection.Num = "WX" + "-" + year + "-" + count.ToString().PadLeft(7, '0');

            //事件内容
            gdEvents.IncidentId = ConvertDateTimeInt(DateTime.Now);
            gdEvents.IncidentType = 2;
            int evCount = _gD_EventsService.Query<GdEvents>(r => true).Count() + 1;
            gdEvents.IncidentNum = "EGWX" + "-" + year + "-" + evCount.ToString().PadLeft(7, '0');
            //gdEvents.IncidentNum = "BY" + DateTime.Now.ToString("yyyyMMddHHssmm");
            gdEvents.IncidentSource = 1;
            gdEvents.ReportTime = DateTime.Parse(gdInspection.CreateTime.ToString());
            gdEvents.DisposeState = 1;
            gdEvents.AuditState = false;
            gdEvents.TaskId = gdInspection.RepairId;
            gdEvents.IncidentContent = gdInspection.FaultContent;
            if (_gD_InspectionService.AddEventRe(gdInspection, gdEvents) > 0)
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }
        }

        [TypeFilter(typeof(IgonreActionFilter))]
        public ActionResult UpLoadImg(IFormFile file, string reid)
        {

            //if (Request.Form.Files.Count <= 0)
            //{
            //    return Content("请选择图片");
            //}
            //string name = Request.Query["address"];
            string imgPath = "\\UploadImg\\GD_Repair\\" + reid + "\\";
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
                Random random = new Random();
                int raInt = random.Next(1, 10000);
                long startId = ConvertDateTimeInt(DateTime.Now) + raInt;
                GdResource gdResource = new GdResource();
                gdResource.Id = startId;
                gdResource.Type = 1;
                gdResource.ResourceType = 1;
                gdResource.Path = imgPath + fileName;
                gdResource.FileName = fileName;
                if (!string.IsNullOrEmpty(ext))
                {
                    string[] str = ext.Split('.');
                    if (str.Length > 1)
                    {
                        gdResource.Suffix = str[1];
                    }
                    else
                    {
                        gdResource.Suffix = ".jpg";
                    }

                }
                else
                {
                    gdResource.Suffix = ".jpg";
                }
                gdResource.Pid = long.Parse(reid);
                _gD_ResourceService.Insert<GdResource>(gdResource);
            }
            return Content(imgPath + fileName);

        }
        //时间戳
        public static long ConvertDateTimeInt(System.DateTime time)
        {
            System.DateTime startTime = System.TimeZoneInfo.ConvertTimeToUtc(new System.DateTime(1970, 1, 1));
            return (long)(time - startTime).TotalMilliseconds;
        }
    }
}