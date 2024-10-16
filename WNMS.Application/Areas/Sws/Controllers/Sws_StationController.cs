using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WNMS.IService;
using WNMS.Model.DataModels;
using System.Web;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Net;
using NPOI.SS.Util;
using System.Security.Claims;
using WNMS.Application.Utility.Filters;
using Microsoft.Extensions.Logging;
using WNMS.Utility;
using WNMS.Application.Utility.JsonHelper;
using WNMS.Model.CustomizedClass;

namespace WNMS.Application.Areas.Sws.Controllers
{
    [Area("Sws")]
    public class Sws_StationController : Controller
    {
        private ISws_StationService _ISws_StationService = null;
        private ISys_DataItemDetailService _IISys_DataItemDetailService = null;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private IAttachmentService attachmentService = null;
        public ISysUserService _ISys_UserService = null;
        private readonly ILogger<Sws_StationController> logger = null;
        string arcgisUrl = StaticConstraint.ArcgisUrl;

        public Sws_StationController(ISws_StationService sws_StationService,
            ISys_DataItemDetailService _sys_DataItemDetailService,
            IWebHostEnvironment webHostEnvironment,
            IAttachmentService _IattachmentService,
            ISysUserService _userservice, ILogger<Sws_StationController> myLogger)
        {
            _ISws_StationService = sws_StationService;
            _IISys_DataItemDetailService = _sys_DataItemDetailService;
            _webHostEnvironment = webHostEnvironment;
            attachmentService = _IattachmentService;
            _ISys_UserService = _userservice;
            logger = myLogger; 
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult QueryStationTable(int pagesize, int pageindex, string SearchText, string order, string sort, string time, string beginDate, string endDate)
        {
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = _ISys_UserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();

            int Totalcount = 0;
            string ordertems = sort + " " + order;
            string filter = "1=1";

            var fitemid = (int)Model.CustomizedClass.Enum.泵房类型;
            var f_StationInstall = (int)Model.CustomizedClass.Enum.泵房安装位置;
            if (SearchText != null)
            {
                filter += " and (StationName like '%" + SearchText + "%' or StationNum like '%" + SearchText + "%')";
            }

            if (!string.IsNullOrEmpty(time))
            {
                GetBeginDate(time, ref beginDate, ref endDate);
                if (!string.IsNullOrEmpty(beginDate) || !string.IsNullOrEmpty(endDate))
                {

                    if (!string.IsNullOrEmpty(beginDate))
                    {
                        filter += " and  CreateTime>='" + beginDate + "'";
                    }
                    if (!string.IsNullOrEmpty(endDate))
                    {
                        filter += " and  CreateTime<'" + endDate + "'";
                    }
                }

            }
            if (user.IsAdmin == false)//非管理员
            {
                filter += "and UserID= " + userID + "";
            }
            var datalist = _ISws_StationService.QueryStationTable(user.IsAdmin, pageindex, pagesize, fitemid, f_StationInstall, ordertems, filter, ref Totalcount);
            return Json(new { total = Totalcount, rows = Newtonsoft.Json.JsonConvert.SerializeObject(datalist) });
        }
        //获取查询开始日期
        void GetBeginDate(string executeTime, ref string BeginTime, ref string EndTime)
        {
            switch (executeTime)
            {
                case "昨天":
                    BeginTime = string.Format("{0:yyyy-MM-dd}", DateTime.Now.AddDays(-1));
                    EndTime = string.Format("{0:yyyy-MM-dd}", DateTime.Now);
                    break;
                case "上周":
                    var dayofWeek = (int)DateTime.Now.DayOfWeek == 0 ? 7 : (int)DateTime.Now.DayOfWeek;
                    BeginTime = string.Format("{0:yyyy-MM-dd}", DateTime.Now.AddDays(-(dayofWeek - 1) - 7));
                    EndTime = string.Format("{0:yyyy-MM-dd}", DateTime.Now.AddDays(-(dayofWeek - 1)));
                    break;
                case "本周":
                    var dayofWeek1 = (int)DateTime.Now.DayOfWeek == 0 ? 7 : (int)DateTime.Now.DayOfWeek;
                    BeginTime = string.Format("{0:yyyy-MM-dd}", DateTime.Now.AddDays(-(dayofWeek1 - 1)));
                    EndTime = string.Format("{0:yyyy-MM-dd}", DateTime.Now.AddDays(-(dayofWeek1 - 1) + 7));
                    break;
                case "下周":
                    var dayofWeek2 = (int)DateTime.Now.DayOfWeek == 0 ? 7 : (int)DateTime.Now.DayOfWeek;
                    BeginTime = string.Format("{0:yyyy-MM-dd}", DateTime.Now.AddDays(-(dayofWeek2 - 1) + 7));
                    EndTime = string.Format("{0:yyyy-MM-dd}", DateTime.Now.AddDays(-(dayofWeek2 - 1) + 14));
                    break;
                case "上月":
                    BeginTime = string.Format("{0:yyyy-MM}", DateTime.Now.AddMonths(-1)) + "-01";
                    EndTime = string.Format("{0:yyyy-MM}", DateTime.Now) + "-01";
                    break;
                case "本月":
                    BeginTime = string.Format("{0:yyyy-MM}", DateTime.Now) + "-01";
                    EndTime = string.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(string.Format("{0:yyyy-MM-01}", DateTime.Now.AddMonths(1))));
                    break;
                case "下月":
                    BeginTime = string.Format("{0:yyyy-MM}", DateTime.Now.AddMonths(1)) + "-01";
                    EndTime = string.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(string.Format("{0:yyyy-MM-01}", DateTime.Now.AddMonths(2))));
                    break;
                case "自定义":
                    if (EndTime != null)
                    {
                        EndTime = string.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(EndTime).AddDays(1));
                    }
                    break;
                default:
                    break;

            }

        }
        #region 页面操作
        //添加
        public IActionResult AddStationPage()
        {
            var model = new SwsStation();
            //默认值
            model.InspectionCycle = 1;
            model.MaintenanceCycle = 1;
            model.CleaningCycle = 1;
            model.WaterTankNum = 0;
            model.SwitchingCycle = 1;

            model.InType = 0;
            List<int> fItemidList = new List<int>() { };
            //泵房类型
            var fitemid = (int)Model.CustomizedClass.Enum.泵房类型;
            fItemidList.Add(fitemid);


            var dataItemDetaiList = _IISys_DataItemDetailService.Query<SysDataItemDetail>(r => fItemidList.Contains(r.FItemId) && r.IsEnable == true);

            var typelist = dataItemDetaiList.Where(r => r.FItemId == fitemid);
            ViewBag.typelist = typelist;//泵房类型

            var userlist = _ISys_UserService.Query<SysUser>(u => u.IsAdmin == false).ToList();
            ViewBag.UserList = userlist;


            return View("_SetStationInfo", model);
        }
        //编辑
        public IActionResult EditeStationPage(int id)
        {
            var model = _ISws_StationService.Query<SwsStation>(r => r.StationId == id).FirstOrDefault();
            List<int> fItemidList = new List<int>() { };
            //泵房类型
            var fitemid = (int)Model.CustomizedClass.Enum.泵房类型;
            fItemidList.Add(fitemid);


            var dataItemDetaiList = _IISys_DataItemDetailService.Query<SysDataItemDetail>(r => fItemidList.Contains(r.FItemId) && r.IsEnable == true);

            var typelist = dataItemDetaiList.Where(r => r.FItemId == fitemid);
            ViewBag.typelist = typelist;//泵房类型

            var userlist = _ISys_UserService.Query<SysUser>(u => u.IsAdmin == false).ToList();
            ViewBag.UserList = userlist;

            return View("_SetStationInfo", model);
        }
        //删除
        [HttpPost]
        public IActionResult DeleteStations(string id)
        {
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = _ISys_UserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
            string useStationName = "";
            string deleteStationName = "";
            string result = "";
            List<int> stationids = new List<string>(id.Split(',')).ConvertAll(r => int.Parse(r));
            var classify = (int)Model.CustomizedClass.AttachmentClassify.泵房;
            string RootPath = _webHostEnvironment.WebRootPath;
            var operateNum = _ISws_StationService.DeleteStation(RootPath, stationids, classify, ref useStationName,ref deleteStationName);
            if (useStationName != "")
            {
                result += useStationName + ",不能删除";
                return Content(result);
            }
            else
            {
                if (operateNum > 0)
                {
                    logger.LogInformation(user.NickName + " 删除泵房-" + deleteStationName);
                    return Content("ok");
                }
                else
                {
                    return Content("no");
                }
            }
        }

        //提交表单
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult SetStationInfo(string baseinfo, string Stationinfo, string deviceInfo, string urls, string sizes, string suffixs, string names, int idFlag)
        {
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = _ISys_UserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
            if (idFlag == 0)
            {
                idFlag = _ISys_UserService.QueryID("StationID", "Sws_Station");
            }
            int id = 0;

            IEnumerable<Attachment> attachment = attachmentService.Query<Attachment>(r => true);
            if (attachment.Count() > 0)
            {
                id = attachment.Select(r => r.FileId).Max();
            }
            SwsStation ss = Newtonsoft.Json.JsonConvert.DeserializeObject<SwsStation>(baseinfo);
            SwsStation StationPart = Newtonsoft.Json.JsonConvert.DeserializeObject<SwsStation>(Stationinfo);
            SwsStation devicedata = Newtonsoft.Json.JsonConvert.DeserializeObject<SwsStation>(deviceInfo);
            if (ss.StationId == 0)//添加
            {
                var datahas = _ISws_StationService.Query<SwsStation>(r => r.StationNum == ss.StationNum);
                if (datahas.Count() > 0)
                {
                    return Content("has");
                }
            }
            else
            {
                var datahas = _ISws_StationService.Query<SwsStation>(r => r.StationNum == ss.StationNum && r.StationId != ss.StationId);
                if (datahas.Count() > 0)
                {
                    return Content("has");
                }
            }
            ss.StaitonType = StationPart.StaitonType;
            ss.InstallationPosition = StationPart.InstallationPosition;
            ss.InspectionCycle = StationPart.InspectionCycle;
            ss.MaintenanceCycle = StationPart.MaintenanceCycle;
            ss.CleaningCycle = StationPart.CleaningCycle;
            ss.Manager = StationPart.Manager;
            ss.ManagerPhone = StationPart.ManagerPhone;
            ss.OccupancyRate = StationPart.OccupancyRate;
            ss.Remark = StationPart.Remark;

            ss.WaterTankNum = devicedata.WaterTankNum;
            ss.SwitchingCycle = devicedata.SwitchingCycle;
            ss.WaterTankPublic = devicedata.WaterTankPublic;
            ss.ControlMonitor = devicedata.ControlMonitor;
            ss.WaterQualityMonitor = devicedata.WaterQualityMonitor;

            List<Attachment> attachList = new List<Attachment>() { };
            if (!string.IsNullOrEmpty(urls))
            {
                List<string> FileUrl = new List<string>(urls.Split('|'));
                List<double> FileSize = new List<string>(sizes.Split(',')).ConvertAll(r => double.Parse(r));
                List<string> Suffix = new List<string>(suffixs.Split(','));
                List<string> FileName = new List<string>(names.Split(','));
                for (var i = 0; i < FileUrl.Count; i++)
                {
                    string fileunit = "";
                    Attachment o = new Attachment();
                    o.FileId = id + i + 1;
                    o.FileSize = FileSizeData(Convert.ToInt32(FileSize[i]), ref fileunit);
                    o.FileUnit = fileunit;
                    o.Suffix = Suffix[i];
                    o.FileUrl = FileUrl[i];
                    o.Affiliation = idFlag;
                    o.UploadTime = DateTime.Now;
                    o.FileType = (byte)GetFileType(Suffix[i]);
                    o.Classify = (int)Model.CustomizedClass.AttachmentClassify.泵房;
                    o.FileName = FileName[i];
                    attachList.Add(o);
                }

            }
            if (ss.StationId == 0)//添加
            {

                ss.StationId = idFlag;

                ss.CreateTime = DateTime.Now;
               
                if (_ISws_StationService.AddStation(ss, attachList, user.IsAdmin, user.UserId) > 0)
                {
                    logger.LogInformation(user.NickName + " 添加泵房-" + ss.StationName);
                    return Content("ok");
                }
                else
                {
                    return Content("no");
                }
            }
            else//修改
            {
                if (_ISws_StationService.EditeStation(ss, attachList, (int)Model.CustomizedClass.AttachmentClassify.泵房) > 0)
                {
                    logger.LogInformation(user.NickName + " 修改泵房-" + ss.StationName);
                    return Content("ok");
                }
                else
                {
                    return Content("no");
                }

            }

        }

        //获取泵房地址
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult GetStationPosition(string lng, string lat,string StationNum,string StationName)
        {
            ViewBag.lng = lng;
            ViewBag.lat = lat;
            ViewBag.StationNum = StationNum;
            ViewBag.StationName = StationName;
            ViewBag.arcgisUrl = arcgisUrl;
            return View();
        }
        #endregion
        //地图页面
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult GetMap()
        {
            //获取地图级别
            var path = _webHostEnvironment.ContentRootPath;
            JsonFileHelper jsonFileHelper = new JsonFileHelper(path + "/Utility/ProjectJson/project.json");
            SystemInfo st = jsonFileHelper.Read<SystemInfo>("SystemInfo");
            string zoom = st.MapLevel == "" ? "11" : st.MapLevel;
            ViewBag.Zoom = Convert.ToInt32(zoom);
            ViewBag.lng = st.Lng;
            ViewBag.lat = st.Lat;
            ViewBag.arcgisUrl = arcgisUrl;
            return View();
        }
        #region 附件处理
        #region 上传文件处理
        /// <summary>
        /// 文件上传（项目中所有文件上传都用这个）
        /// </summary>
        /// <param name="file">需要上传的文件</param>
        /// <param name="paramID">添加页面，模块的ID（例如公告管理模块，为noticeID的值）</param>
        /// <param name="idName">id的名称（例如notice模块，id的值为“NoticeID”）</param>
        /// <param name="ControllerName">模块的名称（例如公告管理模块，值为“OA_Notice”）</param>
        /// <returns></returns>
        [TypeFilter(typeof(IgonreActionFilter))]

        public ActionResult UpLoad(IFormFile file, string paramID, string idName, string ControllerName)
        {
            string filePathName = string.Empty;
            var result = paramID;
            string fileExt = file.FileName.Substring(file.FileName.IndexOf('.')); //文件扩展名，不含“.”
            int index = Convert.ToInt32(base.HttpContext.Request.Form["chunk"]);//当前分块序号
            //若为添加，从数据库获取主键
            if (string.IsNullOrEmpty(paramID) || paramID == "0")
            {
                result = _ISys_UserService.QueryID("StationID", "Sws_Station").ToString();
            }

            ////上传文件是否为空的判断
            if (file.Length == 0)
            {
                return Json(new { jsonrpc = 2.0, error = new { code = 102, message = "保存失败" }, id = "id" });
            }
            string ext = Path.GetExtension(file.FileName);
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
            //路径处理

            string dicPath = _webHostEnvironment.WebRootPath;
            string localPath = Path.Combine(dicPath, "UploadFile/" + ControllerName + "/" + result + "/" + file.FileName);
            if (!System.IO.Directory.Exists(localPath))
            {
                System.IO.Directory.CreateDirectory(localPath);
            }
            //var filePath = localPath + "/" + index + fileExt;
            var pathfile = Path.Combine(localPath, index + fileExt);
            using (FileStream fs = System.IO.File.Create(pathfile))
            {
                file.CopyTo(fs);
                fs.Flush();
            }

            return Json(new
            {
                noticeID = result
            });

        }

        /// <summary>
        /// 合并文件碎片（大文件上传时，先分片上传，临时存储，然后合并文件碎片）
        /// </summary>
        /// <param name="paramID">模块ID，通过公告ID获取文件路径</param>
        /// <param name="fileName">文件名称，通过文件名称获取文件路径</param>
        /// <param name="ControllerName">模块名称</param>
        /// <returns></returns>
        [HttpPost]
        [TypeFilter(typeof(IgonreActionFilter))]
        public ActionResult MergeFile(long paramID, string fileName, string ControllerName)
        {
            string dicPath = _webHostEnvironment.WebRootPath;
            var provider = new PhysicalFileProvider(dicPath);
            string localPath = "wwwroot/UploadFile/" + ControllerName + "/" + paramID + "/" + fileName;
            //var files1 = provider.(localPath);
            var files = Directory.GetFiles(localPath);//获得下面的所有文件
            string paths = Guid.NewGuid().ToString("N");
            string realPath = Path.Combine(dicPath, "UploadFile/" + ControllerName + "/" + paramID + "/" + paths);
            var finalPath = Path.Combine(realPath, fileName);
            if (!System.IO.Directory.Exists(realPath))
            {
                System.IO.Directory.CreateDirectory(realPath);
            }

            var fs = new FileStream(finalPath, FileMode.Create);
            foreach (var part in files.OrderBy(x => x.Length).ThenBy(x => x))//排一下序，保证从0-N Write
            {
                var bytes = System.IO.File.ReadAllBytes(part);
                fs.Write(bytes, 0, bytes.Length);
                bytes = null;
                System.IO.File.Delete(part);//删除分块
            }
            fs.Close();

            System.IO.Directory.Delete(localPath);//删除文件夹           
            return Json(new
            {
                path = paths
            });
        }

        /// <summary>
        /// 获取各功能模块下 上传的文件信息（包括问文件名称、大小、地址、类型等）
        /// </summary>
        /// <param name="id">模块ID</param>
        /// <param name="classifyName">类型名称（枚举获取类型值时使用，例如公告管理模块，类型值为“通知公告”）</param>
        /// <returns></returns>
        [TypeFilter(typeof(IgonreActionFilter))]
        public ActionResult GetFileInfo(int id, string classifyName)
        {
            //获取分类
            var classify = (int)(Model.CustomizedClass.AttachmentClassify)System.Enum.Parse(typeof(Model.CustomizedClass.AttachmentClassify), classifyName);
            //var classify = (int)Model.CustomizedClass.AttachmentClassify.泵房;
            string dicPath = _webHostEnvironment.WebRootPath;
            var file = attachmentService.Query<Attachment>(r => r.Affiliation == id && r.Classify == classify).ToList();
            List<Attachment> att = new List<Attachment>();
            foreach (var item in file)
            {
                string localPath = Path.Combine(dicPath, item.FileUrl.Substring(1).Replace('/', '\\'));
                //删除文件
                if (System.IO.File.Exists(localPath))
                {
                    att.Add(item);
                }
            }
            return Json(new
            {
                data = att
            });
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="fileId">文件ID</param>
        /// <returns></returns>
        [HttpPost]
        [TypeFilter(typeof(IgonreActionFilter))]
        public ActionResult DeleteFile(string fileId)
        {
            string dicPath = _webHostEnvironment.WebRootPath;
            int id = Convert.ToInt32(fileId);
            Attachment fileInfo = attachmentService.Query<Attachment>(r => r.FileId == id).FirstOrDefault();
            if (fileInfo != null)
            {
                attachmentService.Delete(fileInfo);
                string localPath = Path.Combine(dicPath, fileInfo.FileUrl.Substring(1).Replace('/', '\\'));
                //删除文件
                if (System.IO.File.Exists(localPath))
                {
                    System.IO.File.Delete(localPath);
                }
                return Content("ok");
            }
            else
            {
                return Content("no");
            }
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="fileId">文件ID</param>
        /// <returns></returns>
        [TypeFilter(typeof(IgonreActionFilter))]
        public FileStreamResult DownloadFile(int fileId)
        {
            string dicPath = _webHostEnvironment.WebRootPath;
            var file = attachmentService.Query<Attachment>(r => r.FileId == fileId).FirstOrDefault();
            string realPath = Path.Combine(dicPath, file.FileUrl.Substring(1).Replace('/', '\\'));
            FileStream fs = new FileStream(realPath, FileMode.Open);
            return File(fs, "text/plain", file.FileName);
        }
        #endregion
        #region 数据处理方法
        //分解ID
        public List<int> GetIDs(string ids)
        {
            string[] idList = ids.Split(',');
            List<int> nIds = new List<int>();
            foreach (var item in idList)
            {
                nIds.Add(int.Parse(item));
            }
            return nIds;
        }
        //上传文件大小处理
        public static double FileSizeData(int size, ref string fileunit)
        {
            double fileSize = 0.0;
            if (size > 1000)
            {
                fileSize = Math.Round((double)size / 1024, 2);
                fileunit = "KB";
                if (fileSize > 1000)
                {
                    fileSize = Math.Round((double)fileSize / 1024, 2);
                    fileunit = "MB";
                    if (fileSize > 1000)
                    {
                        fileSize = Math.Round((double)fileSize / 1024, 2);
                        fileunit = "GB";
                    }
                }
            }
            else
            {
                fileSize = Math.Round((double)size, 2);
                fileunit = "B";
            }
            return fileSize;
        }

        //图片类型处理
        public static int GetFileType(string suffix)
        {
            if (suffix == ".jpg" || suffix == ".png")
            {
                return (int)Model.CustomizedClass.FileType.图片;
            }
            else
            {
                if (suffix == ".doc" || suffix == ".docx" || suffix == ".wps" || suffix == ".txt")
                {
                    return (int)Model.CustomizedClass.FileType.文本文档;
                }
                else
                {
                    if (suffix == ".zip" || suffix == ".rar")
                    {
                        return (int)Model.CustomizedClass.FileType.压缩文件;
                    }
                    else
                    {
                        if (suffix == ".rmvb" || suffix == ".mp4" || suffix == "flv" || suffix == ".wmv" || suffix == ".mkv" || suffix == ".mov")
                        {
                            return (int)Model.CustomizedClass.FileType.视频文件;
                        }
                        else
                        {
                            if (suffix == ".xls" || suffix == ".xlsx")
                            {
                                return (int)Model.CustomizedClass.FileType.电子表格;
                            }
                            else
                            {
                                return (int)Model.CustomizedClass.FileType.其他;
                            }
                        }
                    }
                }
            }
        }

        #endregion
        #endregion

        //地图测试
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult MapTest()
        {
            return View();
        }
        #region 导入导出
        //导出表格
        [TypeFilter(typeof(IgonreActionFilter))]
        public ActionResult StationTableExport(string SearchText, string order, string sort, string time, string beginDate, string endDate)
        {
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = _ISys_UserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
            int Totalcount = 0;
            string ordertems = sort + " " + order;
            string filter = "1=1";

            var fitemid = (int)Model.CustomizedClass.Enum.泵房类型;
            var f_StationInstall = (int)Model.CustomizedClass.Enum.泵房安装位置;
            if (SearchText != null)
            {
                filter += " and (StationName like '%" + SearchText + "%' or StationNum like '%" + SearchText + "%')";
            }

            if (time != "")
            {
                GetBeginDate(time, ref beginDate, ref endDate);
                if (!string.IsNullOrEmpty(beginDate) || !string.IsNullOrEmpty(endDate))
                {

                    if (!string.IsNullOrEmpty(beginDate))
                    {
                        filter += " and  CreateTime>='" + beginDate + "'";
                    }
                    if (!string.IsNullOrEmpty(endDate))
                    {
                        filter += " and  CreateTime<'" + endDate + "'";
                    }
                }

            }
            if (user.IsAdmin == false)//非管理员
            {
                filter += "and UserID=" + userID + "";
            }
            var list = _ISws_StationService.QueryStationTable(user.IsAdmin, 0, 0, fitemid, f_StationInstall, ordertems, filter, ref Totalcount).ToList();


            //数据导出
            HSSFWorkbook workbook = new HSSFWorkbook();
            ISheet sheet = workbook.CreateSheet();
            #region 内容样式
            IFont font1 = workbook.CreateFont(); //创建一个字体样式对象
            font1.FontName = "Microsoft YaHei"; //和excel里面的字体对应
                                                //font1.Boldweight = short.MaxValue;//字体加粗
            font1.FontHeightInPoints = 12;//字体大小
            ICellStyle style = workbook.CreateCellStyle();//创建样式对象
            style.BorderBottom = BorderStyle.Thin;
            style.BorderLeft = BorderStyle.Thin;
            style.BorderRight = BorderStyle.Thin;
            style.BorderTop = BorderStyle.Thin;
            style.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
            style.VerticalAlignment = VerticalAlignment.Center;
            style.SetFont(font1); //将字体样式赋给样式对象 
            #endregion

            #region 标题样式
            IFont font = workbook.CreateFont(); //创建一个字体样式对象
            font.FontName = "Microsoft YaHei"; //和excel里面的字体对应
            font.Boldweight = (short)FontBoldWeight.Bold;//字体加粗
            font.FontHeightInPoints = 12;//字体大小
            ICellStyle style1 = workbook.CreateCellStyle();//创建样式对象
            style1.BorderBottom = BorderStyle.Thin;
            style1.BorderLeft = BorderStyle.Thin;
            style1.BorderRight = BorderStyle.Thin;
            style1.BorderTop = BorderStyle.Thin;
            style1.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
            style1.VerticalAlignment = VerticalAlignment.Center;
            style1.SetFont(font); //将字体样式赋给样式对象 
            #endregion

            //填充表头
            IRow dataRow = sheet.CreateRow(0);
            dataRow.Height = 20 * 20;
            dataRow.CreateCell(0).SetCellValue("泵房编号");
            dataRow.CreateCell(1).SetCellValue("名称");
            dataRow.CreateCell(2).SetCellValue("类型");
            dataRow.CreateCell(3).SetCellValue("安装日期");
            dataRow.CreateCell(4).SetCellValue("验收日期");
            dataRow.CreateCell(5).SetCellValue("质保日期");

            //dataRow.CreateCell(6).SetCellValue("安装位置");
            dataRow.CreateCell(6).SetCellValue("通讯接入");
            dataRow.CreateCell(7).SetCellValue("水质接入");
            dataRow.CreateCell(8).SetCellValue("门禁接入");
            dataRow.CreateCell(9).SetCellValue("视频接入");
            dataRow.CreateCell(10).SetCellValue("控制接入");
            dataRow.CreateCell(11).SetCellValue("负责人");
            dataRow.CreateCell(12).SetCellValue("负责人电话");
            dataRow.CreateCell(13).SetCellValue("创建日期");
            for (int i = 0; i < 14; i++)
            {
                sheet.SetColumnWidth(i, 30 * 256);
            }
            for (int s = 0; s < 14; s++)
            {
                sheet.SetColumnWidth(s, 25 * 256);
                dataRow.Cells[s].CellStyle = style1;
            }
            int j = 1;
            foreach (var item in list)
            {
                dataRow = sheet.CreateRow(j);
                dataRow.Height = 100 * 4;
                dataRow.CreateCell(0).SetCellValue(item.StationNum?.ToString());
                dataRow.CreateCell(1).SetCellValue(item.StationName?.ToString());
                dataRow.CreateCell(2).SetCellValue(item.StaitonTypeName?.ToString());
                dataRow.CreateCell(3).SetCellValue(item.InstallationDate?.ToString());
                dataRow.CreateCell(4).SetCellValue(item.AcceptanceDate?.ToString());
                dataRow.CreateCell(5).SetCellValue(item.QualityEndDate?.ToString());

                //dataRow.CreateCell(6).SetCellValue(item.StationPostion?.ToString());
                dataRow.CreateCell(6).SetCellValue(item.commi?.ToString());
                dataRow.CreateCell(7).SetCellValue(item.WaterQualityMonitor == true ? "是" : "否");
                dataRow.CreateCell(8).SetCellValue(item.DoorInsert == true ? "是" : "否");
                dataRow.CreateCell(9).SetCellValue(item.CameraMonitor == true ? "是" : "否");
                dataRow.CreateCell(10).SetCellValue(item.ControlMonitor == true ? "是" : "否");
                dataRow.CreateCell(11).SetCellValue(item.Manager?.ToString());
                dataRow.CreateCell(12).SetCellValue(item.ManagerPhone?.ToString());
                dataRow.CreateCell(13).SetCellValue(item.CreateTime.ToString());
                j++;
            }

            for (int ro = 1; ro < list.Count + 1; ro++)
            {
                sheet.GetRow(ro).Height = 20 * 20;
                for (int s = 0; s < 14; s++)
                {
                    sheet.GetRow(ro).Cells[s].CellStyle = style;
                }
            }
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            workbook.Write(ms);
            workbook = null;
            ms.Close();
            ms.Dispose();
            return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "泵房信息.xls");
        }
        //导出关键数据
        [TypeFilter(typeof(IgonreActionFilter))]
        public ActionResult StationImportExport(string SearchText, string order, string sort, string time, string beginDate, string endDate)
        {
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = _ISys_UserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
            int Totalcount = 0;
            string ordertems = sort + " " + order;
            string filter = "1=1";

            var fitemid = (int)Model.CustomizedClass.Enum.泵房类型;
            var f_StationInstall = (int)Model.CustomizedClass.Enum.泵房安装位置;
            if (SearchText != null)
            {
                filter += " and (StationName like '%" + SearchText + "%' or StationNum like '%" + SearchText + "%')";
            }
            filter += " and (InType = 1 or InType = 0) ";
            if (time != "")
            {
                GetBeginDate(time, ref beginDate, ref endDate);
                if (!string.IsNullOrEmpty(beginDate) || !string.IsNullOrEmpty(endDate))
                {

                    if (!string.IsNullOrEmpty(beginDate))
                    {
                        filter += " and  CreateTime>='" + beginDate + "'";
                    }
                    if (!string.IsNullOrEmpty(endDate))
                    {
                        filter += " and  CreateTime<'" + endDate + "'";
                    }
                }

            }
            if (user.IsAdmin == false)//非管理员
            {
                filter += "and UserID=" + userID + "";
            }
            var list = _ISws_StationService.QueryStationTable(user.IsAdmin, 0, 0, fitemid, f_StationInstall, ordertems, filter, ref Totalcount).ToList();
            List<int> ids = new List<int> { 5, 6, 7, 8 };
            var allType = _IISys_DataItemDetailService.Query<SysDataItemDetail>(r => ids.Contains(r.FItemId) && r.IsEnable == true).ToList();

            //数据导出
            HSSFWorkbook workbook = new HSSFWorkbook();
            ISheet sheet = workbook.CreateSheet();
            #region 内容样式
            //IFont font1 = workbook.CreateFont(); //创建一个字体样式对象
            //font1.FontName = "Microsoft YaHei"; //和excel里面的字体对应
            //                                    //font1.Boldweight = short.MaxValue;//字体加粗
            //font1.FontHeightInPoints = 12;//字体大小
            //ICellStyle style = workbook.CreateCellStyle();//创建样式对象
            //style.BorderBottom = BorderStyle.Thin;
            //style.BorderLeft = BorderStyle.Thin;
            //style.BorderRight = BorderStyle.Thin;
            //style.BorderTop = BorderStyle.Thin;
            //style.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
            //style.VerticalAlignment = VerticalAlignment.Center;
            //style.SetFont(font1); //将字体样式赋给样式对象 
            #endregion

            #region 标题样式
            //IFont font = workbook.CreateFont(); //创建一个字体样式对象
            //font.FontName = "Microsoft YaHei"; //和excel里面的字体对应
            //font.Boldweight = (short)FontBoldWeight.Bold;//字体加粗
            //font.FontHeightInPoints = 12;//字体大小
            //ICellStyle style1 = workbook.CreateCellStyle();//创建样式对象
            //style1.BorderBottom = BorderStyle.Thin;
            //style1.BorderLeft = BorderStyle.Thin;
            //style1.BorderRight = BorderStyle.Thin;
            //style1.BorderTop = BorderStyle.Thin;
            //style1.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
            //style1.VerticalAlignment = VerticalAlignment.Center;
            //style1.SetFont(font); //将字体样式赋给样式对象 
            #endregion
            #region
            //设置生成下拉框的行和列  设备类型设置
            CellRangeAddressList cellRegions = new CellRangeAddressList(0, 65535, 5, 5);
            //设置 下拉框内容 
            int devicetypeid = (int)WNMS.Model.CustomizedClass.Enum.设备类型;
            var deviceType = allType.Where(r => r.FItemId == devicetypeid).Select(r => r.ItemName).ToList();
            var strArray = deviceType.ToArray();
            DVConstraint constraint = DVConstraint.CreateExplicitListConstraint(strArray);
            //绑定下拉框和作用区域，并设置错误提示信息
            HSSFDataValidation dataValidate = new HSSFDataValidation(cellRegions, constraint);
            dataValidate.CreateErrorBox("输入不合法", "请输入下拉列表中的值！");
            dataValidate.ShowPromptBox = true;
            sheet.AddValidationData(dataValidate);
            //变频器类型
            int frequencyid = (int)WNMS.Model.CustomizedClass.Enum.变频分类;
            var frequencytype = allType.Where(r => r.FItemId == frequencyid).Select(r => r.ItemName).ToList();
            strArray = frequencytype.ToArray();
            cellRegions = new CellRangeAddressList(0, 65535, 6, 6);
            constraint = DVConstraint.CreateExplicitListConstraint(strArray);
            dataValidate = new HSSFDataValidation(cellRegions, constraint);
            sheet.AddValidationData(dataValidate);
            //分区
            //设备分区
            int Partitionid = (int)WNMS.Model.CustomizedClass.Enum.设备分区;
            var Partitionfo = allType.Where(r => r.FItemId == Partitionid).Select(r => r.ItemName).ToList();
            strArray = Partitionfo.ToArray();
            cellRegions = new CellRangeAddressList(0, 65535, 7, 7);
            constraint = DVConstraint.CreateExplicitListConstraint(strArray);
            dataValidate = new HSSFDataValidation(cellRegions, constraint);
            sheet.AddValidationData(dataValidate);
            //设备品牌
            int Manufacturerid = (int)WNMS.Model.CustomizedClass.Enum.设备品牌;
            var Manufacturerinfo = allType.Where(r => r.FItemId == Manufacturerid).Select(r => r.ItemName).ToList();
            strArray = Manufacturerinfo.ToArray();
            cellRegions = new CellRangeAddressList(0, 65535, 12, 12);
            constraint = DVConstraint.CreateExplicitListConstraint(strArray);
            dataValidate = new HSSFDataValidation(cellRegions, constraint);
            sheet.AddValidationData(dataValidate);
            #endregion

            //填充表头
            IRow dataRow = sheet.CreateRow(0);
            dataRow.Height = 20 * 20;
            dataRow.CreateCell(0).SetCellValue("泵房ID");
            dataRow.CreateCell(1).SetCellValue("泵房编号");
            dataRow.CreateCell(2).SetCellValue("名称");
            dataRow.CreateCell(3).SetCellValue("设备名称");
            dataRow.CreateCell(4).SetCellValue("设备编号");
            dataRow.CreateCell(5).SetCellValue("设备类型");
            dataRow.CreateCell(6).SetCellValue("变频器类型");
            dataRow.CreateCell(7).SetCellValue("分区");
            dataRow.CreateCell(8).SetCellValue("泵数量");
            dataRow.CreateCell(9).SetCellValue("泵类型");
            dataRow.CreateCell(10).SetCellValue("进口DN");
            dataRow.CreateCell(11).SetCellValue("出口DN");
            dataRow.CreateCell(12).SetCellValue("厂商");
            for (int i = 0; i < 13; i++)
            {
                sheet.SetColumnWidth(i, 15 * 256);
            }
            //for (int s = 0; s < 3; s++)
            //{
            //    sheet.SetColumnWidth(s, 25 * 256);
            //    dataRow.Cells[s].CellStyle = style1;
            //}
            int j = 1;
            foreach (var item in list)
            {
                dataRow = sheet.CreateRow(j);
                //dataRow.Height = 100 * 4;
                dataRow.CreateCell(0).SetCellValue(item.StationID?.ToString());
                dataRow.CreateCell(1).SetCellValue(item.StationNum?.ToString());
                dataRow.CreateCell(2).SetCellValue(item.StationName?.ToString());
                j++;
            }

            //for (int ro = 1; ro < list.Count + 1; ro++)
            //{
            //    sheet.GetRow(ro).Height = 20 * 20;
            //    for (int s = 0; s < 3; s++)
            //    {
            //        sheet.GetRow(ro).Cells[s].CellStyle = style;
            //    }
            //}
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            workbook.Write(ms);
            workbook = null;
            ms.Close();
            ms.Dispose();
            return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "导入设备模板.xls");
        }
        //导出直饮水关键数据
        [TypeFilter(typeof(IgonreActionFilter))]
        public ActionResult ZStationImportExport(string SearchText, string order, string sort, string time, string beginDate, string endDate)
        {
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = _ISys_UserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
            int Totalcount = 0;
            string ordertems = sort + " " + order;
            string filter = "1=1";

            var fitemid = (int)Model.CustomizedClass.Enum.泵房类型;
            var f_StationInstall = (int)Model.CustomizedClass.Enum.泵房安装位置;
            if (SearchText != null)
            {
                filter += " and (StationName like '%" + SearchText + "%' or StationNum like '%" + SearchText + "%')";
            }
            filter += " and (InType = 2 or InType = 0) ";
            if (time != "")
            {
                GetBeginDate(time, ref beginDate, ref endDate);
                if (!string.IsNullOrEmpty(beginDate) || !string.IsNullOrEmpty(endDate))
                {

                    if (!string.IsNullOrEmpty(beginDate))
                    {
                        filter += " and  CreateTime>='" + beginDate + "'";
                    }
                    if (!string.IsNullOrEmpty(endDate))
                    {
                        filter += " and  CreateTime<'" + endDate + "'";
                    }
                }

            }
            if (user.IsAdmin == false)//非管理员
            {
                filter += "and UserID=" + userID + "";
            }
            var list = _ISws_StationService.QueryStationTable(user.IsAdmin, 0, 0, fitemid, f_StationInstall, ordertems, filter, ref Totalcount).ToList();
            List<int> ids = new List<int> { 14, 6, 7, 8 };
            var allType = _IISys_DataItemDetailService.Query<SysDataItemDetail>(r => ids.Contains(r.FItemId) && r.IsEnable == true).ToList();

            //数据导出
            HSSFWorkbook workbook = new HSSFWorkbook();
            ISheet sheet = workbook.CreateSheet();

            #region
            //设置生成下拉框的行和列  设备类型设置
            CellRangeAddressList cellRegions = new CellRangeAddressList(0, 65535, 5, 5);
            //设置 下拉框内容 
            int devicetypeid = (int)WNMS.Model.CustomizedClass.Enum.直饮水设备类型;
            var deviceType = allType.Where(r => r.FItemId == devicetypeid).Select(r => r.ItemName).ToList();
            var strArray = deviceType.ToArray();
            DVConstraint constraint = DVConstraint.CreateExplicitListConstraint(strArray);
            //绑定下拉框和作用区域，并设置错误提示信息
            HSSFDataValidation dataValidate = new HSSFDataValidation(cellRegions, constraint);
            dataValidate.CreateErrorBox("输入不合法", "请输入下拉列表中的值！");
            dataValidate.ShowPromptBox = true;
            sheet.AddValidationData(dataValidate);
            //设备分区
            int Partitionid = (int)WNMS.Model.CustomizedClass.Enum.设备分区;
            var Partitionfo = allType.Where(r => r.FItemId == Partitionid).Select(r => r.ItemName).ToList();
            strArray = Partitionfo.ToArray();
            cellRegions = new CellRangeAddressList(0, 65535, 6, 6);
            constraint = DVConstraint.CreateExplicitListConstraint(strArray);
            dataValidate = new HSSFDataValidation(cellRegions, constraint);
            sheet.AddValidationData(dataValidate);
            //设备品牌
            int Manufacturerid = (int)WNMS.Model.CustomizedClass.Enum.设备品牌;
            var Manufacturerinfo = allType.Where(r => r.FItemId == Manufacturerid).Select(r => r.ItemName).ToList();
            strArray = Manufacturerinfo.ToArray();
            cellRegions = new CellRangeAddressList(0, 65535, 7, 7);
            constraint = DVConstraint.CreateExplicitListConstraint(strArray);
            dataValidate = new HSSFDataValidation(cellRegions, constraint);
            sheet.AddValidationData(dataValidate);
            #endregion

            //填充表头
            IRow dataRow = sheet.CreateRow(0);
            dataRow.Height = 20 * 20;
            dataRow.CreateCell(0).SetCellValue("泵房ID");
            dataRow.CreateCell(1).SetCellValue("泵房编号");
            dataRow.CreateCell(2).SetCellValue("名称");
            dataRow.CreateCell(3).SetCellValue("设备名称");
            dataRow.CreateCell(4).SetCellValue("设备编号");
            dataRow.CreateCell(5).SetCellValue("设备类型");
            dataRow.CreateCell(6).SetCellValue("分区");
            dataRow.CreateCell(7).SetCellValue("厂商");
            for (int i = 0; i < 7; i++)
            {
                sheet.SetColumnWidth(i, 15 * 256);
            }
            int j = 1;
            foreach (var item in list)
            {
                dataRow = sheet.CreateRow(j);
                //dataRow.Height = 100 * 4;
                dataRow.CreateCell(0).SetCellValue(item.StationID?.ToString());
                dataRow.CreateCell(1).SetCellValue(item.StationNum?.ToString());
                dataRow.CreateCell(2).SetCellValue(item.StationName?.ToString());
                j++;
            }

            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            workbook.Write(ms);
            workbook = null;
            ms.Close();
            ms.Dispose();
            return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "导入直饮水设备模板.xls");
        }
        //导入 (必填项必须填，否则row.GetCell(x)会报错，因为生成的模板没有create该单元格)

        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult ImportStation(IFormFile excelfile)
        {
            List<SwsStation> list = new List<SwsStation> { };

            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = _ISys_UserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
            try
            {
                MemoryStream ms = new MemoryStream();
                excelfile.CopyTo(ms);
                ms.Seek(0, SeekOrigin.Begin);
                //WebClient webClient = new WebClient();
                //var objj = webClient.DownloadData(excelfile.FileName);//fileName 是远程url地址，可以url直接下载
                //Stream stream = new MemoryStream(objj);
                HSSFWorkbook workbook = new HSSFWorkbook(ms);
                //IWorkbook workbook = new XSSFWorkbook(stream);
                List<int> ids = new List<int> { (int)Model.CustomizedClass.Enum.泵房类型 };
                //所有类型
                var allType = _IISys_DataItemDetailService.Query<SysDataItemDetail>(r => ids.Contains(r.FItemId) && r.IsEnable == true).ToList();
                var stationmodel = _ISws_StationService.Query<SwsStation>(r => true).OrderBy(r => r.StationId).LastOrDefault();
                int maxid = 0;
                if (stationmodel == null)
                {
                    maxid = 0;
                }
                else
                {
                    maxid = stationmodel.StationId;
                }

                List<string> stationNums = new List<string>() { };
                ISheet sheet = workbook.GetSheetAt(0);

                for (int i = 1; i <= sheet.LastRowNum; i++)
                {
                    IRow row = sheet.GetRow(i);
                    SwsStation obj = new SwsStation();
                    if (row!=null&& row.GetCell(0) != null && row.GetCell(0).ToString() != "")
                    {

                        obj.StationId = maxid + i;
                        obj.StationNum = row.GetCell(0).ToString();
                        stationNums.Add(row.GetCell(0).ToString());
                        obj.StationName = row.GetCell(1)?.ToString();
                        string typename = row.GetCell(2)?.ToString();
                        var StaitonTypeName = allType.Where(r => r.ItemName.Contains(typename) && r.FItemId == ids[0]).FirstOrDefault();
                        if (StaitonTypeName != null)
                        {
                            obj.StaitonType = byte.Parse(StaitonTypeName.ItemValue);
                        }
                        else
                        {
                            obj.StaitonType = 1;
                        }
                        var aa = row.GetCell(3).ToString();
                        obj.InstallationDate = string.IsNullOrEmpty(row.GetCell(3).ToString()) ? DateTime.Now : row.GetCell(3).DateCellValue;
                        obj.AcceptanceDate = string.IsNullOrEmpty(row.GetCell(4).ToString()) ? DateTime.Now : row.GetCell(4).DateCellValue;
                        obj.QualityEndDate = string.IsNullOrEmpty(row.GetCell(5).ToString()) ? DateTime.Now : row.GetCell(5).DateCellValue;
                        //string positionname = row.GetCell(6)?.ToString();
                        //var StationPostion= allType.Where(r => r.ItemName.Contains(positionname) && r.FItemId == ids[0]).FirstOrDefault();
                        //obj.InstallationPosition = StationPostion==null?(byte)1:byte.Parse(StationPostion.ItemValue);
                        //obj.WaterQualityMonitor = row.GetCell(6)?.ToString() == "是" ? true : false;
                        //obj.DoorInsert = row.GetCell(7)?.ToString() == "是" ? true : false;
                        //obj.CameraMonitor = row.GetCell(8)?.ToString() == "是" ? true : false;
                        obj.ControlMonitor = row.GetCell(6)?.ToString() == "是" ? true : false;
                        obj.WaterQualityMonitor= row.GetCell(7)?.ToString() == "是" ? true : false;
                        obj.Manager = Convert.ToInt64(row.GetCell(8));
                        obj.ManagerPhone = row.GetCell(9)?.ToString();
                        obj.Lng = string.IsNullOrEmpty(row.GetCell(10)?.ToString()) ? 0.0 : Convert.ToDouble(row.GetCell(10).ToString());

                        obj.Lat = string.IsNullOrEmpty(row.GetCell(11)?.ToString()) ? 0.0 : Convert.ToDouble(row.GetCell(11).ToString());
                        obj.CreateTime = DateTime.Now;
                        obj.InspectionCycle = 1;
                        obj.MaintenanceCycle = 1;
                        obj.CleaningCycle = 1;
                        obj.WaterTankNum = 0;
                        obj.SwitchingCycle = 1;
                        
                        obj.DoorInsert = false;
                        obj.CameraMonitor = false;
                        list.Add(obj);

                    }


                }
                #region 判断泵房编码是否有重复
                if (stationNums.Count > 0)
                {
                    //判断添加进去的是否有重复
                    if (stationNums.Count != stationNums.Distinct().Count())
                    {
                        return Content("文件中有重复的泵房编码");
                    }
                    var hasStation = _ISws_StationService.Query<SwsStation>(r => stationNums.Contains(r.StationNum)).Select(r => r.StationNum).ToList();
                    if (hasStation.Count > 0)
                    {
                        var hasStationNum = string.Join(',', hasStation);
                        return Content("文件中的泵房编码与数据库相同，" + hasStationNum + "");
                    }


                }
                #endregion
            }
            catch (Exception e)
            {
                if (e.Source == "NPOI")
                {
                    return Content("文件类型不对");
                }
                else
                {
                    if (e.Message.Contains("DateTime"))
                    {
                        return Content("日期不对");
                    }
                    else
                    {
                        return Content("请填写必填信息");
                    }
                }

            }
            try
            {
                if (list.Count > 0)
                {
                    if (_ISws_StationService.AddStationList(list, user.IsAdmin, user.UserId) > 0)
                    {
                        return Content("导入成功");
                    }
                    else
                    {
                        return Content("导入失败");
                    }

                }
                else
                {
                    return Content("空文件");
                }

            }
            catch (Exception e)
            {
                return Content("导入失败");
            }

        }
        //导出泵房导入模板

        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult SImportTemplateExport()
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            ISheet sheet = workbook.CreateSheet();
            var fitemid = (int)Model.CustomizedClass.Enum.泵房类型;

            List<int> ids = new List<int> { fitemid };
            var allType = _IISys_DataItemDetailService.Query<SysDataItemDetail>(r => ids.Contains(r.FItemId) && r.IsEnable == true).ToList();
            #region



            //设置生成下拉框的行和列  泵房类型设置
            CellRangeAddressList cellRegions = new CellRangeAddressList(0, 65535, 2, 2);
            //设置 下拉框内容 

            var deviceType = allType.Where(r => r.FItemId == fitemid).Select(r => r.ItemName).ToList();
            var strArray = deviceType.ToArray();
            DVConstraint constraint = DVConstraint.CreateExplicitListConstraint(strArray);
            //绑定下拉框和作用区域，并设置错误提示信息
            HSSFDataValidation dataValidate = new HSSFDataValidation(cellRegions, constraint);
            dataValidate.CreateErrorBox("输入不合法", "请输入下拉列表中的值！");
            dataValidate.ShowPromptBox = true;
            sheet.AddValidationData(dataValidate);
            //安装位置


            //泵房编号 判断唯一性（待写）
            // cellRegions = new CellRangeAddressList(0, 65535, 0, 0);
            // constraint = DVConstraint.CreateCustomFormulaConstraint("=COUNTIF($A$2:A2,A2)=1");
            //dataValidate = new HSSFDataValidation(cellRegions, constraint);
            //dataValidate.CreateErrorBox("输入不合法", "输入值唯一");
            //sheet.AddValidationData(dataValidate);

            //时间约束
            //安装日期
            cellRegions = new CellRangeAddressList(0, 65535, 3, 3);
           
            constraint = DVConstraint.CreateDateConstraint(OperatorType.BETWEEN, "1900-01-01", "9999-12-31", "yyyy-MM-dd");

            dataValidate = new HSSFDataValidation(cellRegions, constraint);
            dataValidate.CreateErrorBox("输入不合法", "输入正确的时间值");
            sheet.AddValidationData(dataValidate);
            //验收日期
            cellRegions = new CellRangeAddressList(0, 65535, 4, 4);
            constraint = DVConstraint.CreateDateConstraint(OperatorType.BETWEEN, "1900-01-01", "9999-12-31", "yyyy-MM-dd");
            dataValidate = new HSSFDataValidation(cellRegions, constraint);
            dataValidate.CreateErrorBox("输入不合法", "输入正确的时间值");
            sheet.AddValidationData(dataValidate);
            //质保日期
            cellRegions = new CellRangeAddressList(0, 65535, 5, 5);
            constraint = DVConstraint.CreateDateConstraint(OperatorType.BETWEEN, "1900-01-01", "9999-12-31", "yyyy-MM-dd");
            dataValidate = new HSSFDataValidation(cellRegions, constraint);
            dataValidate.CreateErrorBox("输入不合法", "输入正确的时间值");
            sheet.AddValidationData(dataValidate);
            //水质是否接入
            string[] boolArray = new string[] { "是", "否" };
            //cellRegions = new CellRangeAddressList(0, 65535, 6, 6);
            //constraint = DVConstraint.CreateExplicitListConstraint(boolArray);
            //dataValidate = new HSSFDataValidation(cellRegions, constraint);
            //dataValidate.CreateErrorBox("输入不合法", "请输入下拉列表中的值");
            //sheet.AddValidationData(dataValidate);
            //门禁接入


            //cellRegions = new CellRangeAddressList(0, 65535, 7, 7);
            //constraint = DVConstraint.CreateExplicitListConstraint(boolArray);
            //dataValidate = new HSSFDataValidation(cellRegions, constraint);
            //dataValidate.CreateErrorBox("输入不合法", "请输入下拉列表中的值");
            //sheet.AddValidationData(dataValidate);

            //视频接入
            //cellRegions = new CellRangeAddressList(0, 65535, 8, 8);
            //constraint = DVConstraint.CreateExplicitListConstraint(boolArray);
            //dataValidate = new HSSFDataValidation(cellRegions, constraint);
            //dataValidate.CreateErrorBox("输入不合法", "请输入下拉列表中的值");
            //sheet.AddValidationData(dataValidate);
            //控制接入
            cellRegions = new CellRangeAddressList(0, 65535, 6, 6);
            constraint = DVConstraint.CreateExplicitListConstraint(boolArray);
            dataValidate = new HSSFDataValidation(cellRegions, constraint);
            dataValidate.CreateErrorBox("输入不合法", "请输入下拉列表中的值");
            sheet.AddValidationData(dataValidate);

            //水质接入
            cellRegions = new CellRangeAddressList(0, 65535, 7, 7);
            constraint = DVConstraint.CreateExplicitListConstraint(boolArray);
            dataValidate = new HSSFDataValidation(cellRegions, constraint);
            dataValidate.CreateErrorBox("输入不合法", "请输入下拉列表中的值");
            sheet.AddValidationData(dataValidate);


            ////泵房编号
            //cellRegions = new CellRangeAddressList(0, 65535, 0, 0);
            //constraint = DVConstraint.CreateNumericConstraint(ValidationType.TEXT_LENGTH, OperatorType.GREATER_THAN, "0", "20");
            //dataValidate = new HSSFDataValidation(cellRegions, constraint);
            //dataValidate.CreateErrorBox("输入不合法", "请输入正确的值");
            //sheet.AddValidationData(dataValidate);
            ////泵房名称
            //cellRegions = new CellRangeAddressList(0, 65535, 1, 1);
            //constraint = DVConstraint.CreateNumericConstraint(ValidationType.TEXT_LENGTH, OperatorType.GREATER_THAN, "0", "50");
            //dataValidate = new HSSFDataValidation(cellRegions, constraint);
            //dataValidate.CreateErrorBox("输入不合法", "请输入正确的值");
            //sheet.AddValidationData(dataValidate);
            //经度
            cellRegions = new CellRangeAddressList(0, 65535, 10, 10);
            constraint = DVConstraint.CreateNumericConstraint(ValidationType.DECIMAL, OperatorType.GREATER_THAN, "0", "0");
            dataValidate = new HSSFDataValidation(cellRegions, constraint);
            dataValidate.CreateErrorBox("输入不合法", "请输入大于0的值");
            sheet.AddValidationData(dataValidate);
            //纬度
            cellRegions = new CellRangeAddressList(0, 65535, 11, 11);
            constraint = DVConstraint.CreateNumericConstraint(ValidationType.DECIMAL, OperatorType.GREATER_THAN, "0", "0");
            dataValidate = new HSSFDataValidation(cellRegions, constraint);
            dataValidate.CreateErrorBox("输入不合法", "请输入大于0的值");
            sheet.AddValidationData(dataValidate);

            //负责人电话
            cellRegions = new CellRangeAddressList(0, 65535, 9, 9);
            constraint = DVConstraint.CreateNumericConstraint(ValidationType.TEXT_LENGTH, OperatorType.BETWEEN, "11", "11");
            dataValidate = new HSSFDataValidation(cellRegions, constraint);
            dataValidate.CreateErrorBox("输入不合法", "请输入正确的电话");
            sheet.AddValidationData(dataValidate);

            #endregion
            //填充表头
            IRow dataRow = sheet.CreateRow(0);
            dataRow.Height = 20 * 20;
            dataRow.CreateCell(0).SetCellValue("泵房编号(*)");
            dataRow.CreateCell(1).SetCellValue("泵房名称(*)");
            dataRow.CreateCell(2).SetCellValue("泵房类型(*)");
            dataRow.CreateCell(3).SetCellValue("安装日期(*)");
            dataRow.CreateCell(4).SetCellValue("验收日期(*)");
            dataRow.CreateCell(5).SetCellValue("质保日期(*)");

            //dataRow.CreateCell(6).SetCellValue("安装位置(*)");
           
            //dataRow.CreateCell(7).SetCellValue("门禁接入");
            //dataRow.CreateCell(8).SetCellValue("视频接入");
            dataRow.CreateCell(6).SetCellValue("控制接入");
            dataRow.CreateCell(7).SetCellValue("水质接入");
            dataRow.CreateCell(8).SetCellValue("负责人");
            dataRow.CreateCell(9).SetCellValue("负责人电话");
            dataRow.CreateCell(10).SetCellValue("经度");
            dataRow.CreateCell(11).SetCellValue("纬度");
            for (int i = 0; i < 12; i++)
            {
                sheet.SetColumnWidth(i, 30 * 256);
            }

            
           

            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            workbook.Write(ms);
            workbook = null;
            ms.Close();
            ms.Dispose();
            return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "导入泵房模板.xls");
        }
        //导出资产信息导入模板

        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult PTemplateExport(string SearchText, string order, string sort, string time, string beginDate, string endDate)
        {

            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = _ISys_UserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
            int Totalcount = 0;
            string ordertems = sort + " " + order;
            string filter = "1=1";

            var fitemid = (int)Model.CustomizedClass.Enum.泵房类型;
            var f_StationInstall = (int)Model.CustomizedClass.Enum.泵房安装位置;
            if (SearchText != null)
            {
                filter += " and (StationName like '%" + SearchText + "%' or StationNum like '%" + SearchText + "%')";
            }

            if (time != "")
            {
                GetBeginDate(time, ref beginDate, ref endDate);
                if (!string.IsNullOrEmpty(beginDate) || !string.IsNullOrEmpty(endDate))
                {

                    if (!string.IsNullOrEmpty(beginDate))
                    {
                        filter += " and  CreateTime>='" + beginDate + "'";
                    }
                    if (!string.IsNullOrEmpty(endDate))
                    {
                        filter += " and  CreateTime<'" + endDate + "'";
                    }
                }

            }
            if (user.IsAdmin == false)//非管理员
            {
                filter += "and UserID=" + userID + "";
            }
            var list = _ISws_StationService.QueryStationTable(user.IsAdmin, 0, 0, fitemid, f_StationInstall, ordertems, filter, ref Totalcount).ToList();
            List<int> ids = new List<int> { (int)Model.CustomizedClass.Enum.资产类型 };
            var PropertyTypes = _IISys_DataItemDetailService.Query<SysDataItemDetail>(r => ids.Contains(r.FItemId) && r.IsEnable == true).Select(r => r.ItemName).ToList();
            //数据导出
            HSSFWorkbook workbook = new HSSFWorkbook();
            ISheet sheet = workbook.CreateSheet();
            #region 内容样式
            IFont font1 = workbook.CreateFont(); //创建一个字体样式对象
            font1.FontName = "Microsoft YaHei"; //和excel里面的字体对应
                                                //font1.Boldweight = short.MaxValue;//字体加粗
            font1.FontHeightInPoints = 12;//字体大小
            ICellStyle style = workbook.CreateCellStyle();//创建样式对象
            style.BorderBottom = BorderStyle.Thin;
            style.BorderLeft = BorderStyle.Thin;
            style.BorderRight = BorderStyle.Thin;
            style.BorderTop = BorderStyle.Thin;
            style.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
            style.VerticalAlignment = VerticalAlignment.Center;
            style.SetFont(font1); //将字体样式赋给样式对象 
            #endregion

            #region 标题样式
            IFont font = workbook.CreateFont(); //创建一个字体样式对象
            font.FontName = "Microsoft YaHei"; //和excel里面的字体对应
            font.Boldweight = (short)FontBoldWeight.Bold;//字体加粗
            font.FontHeightInPoints = 12;//字体大小
            ICellStyle style1 = workbook.CreateCellStyle();//创建样式对象
            style1.BorderBottom = BorderStyle.Thin;
            style1.BorderLeft = BorderStyle.Thin;
            style1.BorderRight = BorderStyle.Thin;
            style1.BorderTop = BorderStyle.Thin;
            style1.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
            style1.VerticalAlignment = VerticalAlignment.Center;
            style1.SetFont(font); //将字体样式赋给样式对象 
            #endregion
            #region 约束
            //设置生成下拉框的行和列  资产类型设置
            CellRangeAddressList cellRegions = new CellRangeAddressList(0, 65535, 4, 4);
            //设置 下拉框内容 
            var strArray = PropertyTypes.ToArray();
            DVConstraint constraint = DVConstraint.CreateExplicitListConstraint(strArray);
            //绑定下拉框和作用区域，并设置错误提示信息
            HSSFDataValidation dataValidate = new HSSFDataValidation(cellRegions, constraint);
            dataValidate.CreateErrorBox("输入不合法", "请输入下拉列表中的值！");
            dataValidate.ShowPromptBox = true;
            sheet.AddValidationData(dataValidate);


            //泵房id
            cellRegions = new CellRangeAddressList(0, 65535, 0, 0);
            constraint = DVConstraint.CreateNumericConstraint(ValidationType.INTEGER, OperatorType.GREATER_THAN, "0", "0");
            dataValidate = new HSSFDataValidation(cellRegions, constraint);
            dataValidate.CreateErrorBox("输入不合法", "请输入正确的值");
            sheet.AddValidationData(dataValidate);

            //购买日期
            cellRegions = new CellRangeAddressList(0, 65535, 5, 5);
            constraint = DVConstraint.CreateDateConstraint(OperatorType.BETWEEN, "1900-01-01", "9999-12-31", "yyyy-MM-dd");

            dataValidate = new HSSFDataValidation(cellRegions, constraint);
            dataValidate.CreateErrorBox("输入不合法", "输入正确的时间值");
            sheet.AddValidationData(dataValidate);
            //购买金额
            cellRegions = new CellRangeAddressList(0, 65535, 6, 6);
            constraint = DVConstraint.CreateNumericConstraint(ValidationType.DECIMAL, OperatorType.GREATER_OR_EQUAL, "0", "0");
            dataValidate = new HSSFDataValidation(cellRegions, constraint);
            dataValidate.CreateErrorBox("输入不合法", "请输入正确的值");
            sheet.AddValidationData(dataValidate);

            #endregion
            //填充表头
            IRow dataRow = sheet.CreateRow(0);
            dataRow.Height = 20 * 20;
            dataRow.CreateCell(0).SetCellValue("泵房ID(*)");
            dataRow.CreateCell(1).SetCellValue("泵房编号");
            dataRow.CreateCell(2).SetCellValue("泵房名称");
            dataRow.CreateCell(3).SetCellValue("资产名称(*)");
            dataRow.CreateCell(4).SetCellValue("资产类型(*)");
            dataRow.CreateCell(5).SetCellValue("购买日期(*)");
            dataRow.CreateCell(6).SetCellValue("购买金额(*)");
            dataRow.CreateCell(7).SetCellValue("保管人(*)");
            dataRow.CreateCell(8).SetCellValue("厂商");
            dataRow.CreateCell(9).SetCellValue("品牌");
            dataRow.CreateCell(10).SetCellValue("规格型号");
            dataRow.CreateCell(11).SetCellValue("质保期");
            dataRow.CreateCell(12).SetCellValue("存放位置");
            for (int i = 0; i < 13; i++)
            {
                sheet.SetColumnWidth(i, 15 * 256);
            }
            for (int s = 0; s < 13; s++)
            {
                sheet.SetColumnWidth(s, 25 * 256);
                dataRow.Cells[s].CellStyle = style1;
            }
            int j = 1;
            foreach (var item in list)
            {
                dataRow = sheet.CreateRow(j);
                //dataRow.Height = 100 * 4;
                dataRow.CreateCell(0).SetCellValue(item.StationID?.ToString());
                dataRow.CreateCell(1).SetCellValue(item.StationNum?.ToString());
                dataRow.CreateCell(2).SetCellValue(item.StationName?.ToString());
                dataRow.CreateCell(3).SetCellValue("");
                dataRow.CreateCell(4).SetCellValue("");
                dataRow.CreateCell(5).SetCellValue("");
                dataRow.CreateCell(6).SetCellValue("");
                dataRow.CreateCell(7).SetCellValue("");
                dataRow.CreateCell(8).SetCellValue("");
                dataRow.CreateCell(9).SetCellValue("");
                dataRow.CreateCell(10).SetCellValue("");
                dataRow.CreateCell(11).SetCellValue("");
                dataRow.CreateCell(12).SetCellValue("");
                j++;
            }

            for (int ro = 1; ro < list.Count + 1; ro++)
            {
                sheet.GetRow(ro).Height = 20 * 20;
                for (int s = 0; s < 13; s++)
                {
                    sheet.GetRow(ro).Cells[s].CellStyle = style;
                }
            }
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            workbook.Write(ms);
            workbook = null;
            ms.Close();
            ms.Dispose();
            return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "资产信息导入模板.xls");
        }
        #endregion
    }
}