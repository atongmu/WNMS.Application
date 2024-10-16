using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using WNMS.Application.Utility.Filters;
using WNMS.IService;
using WNMS.Model.CustomizedClass;
using WNMS.Model.DataModels;

namespace WNMS.Application.Areas.Sws.Controllers
{
    [Area("Sws")]
    public class Sws_TemplateController : Controller
    {
        private ISws_TemplateService _sws_TemplateService;
        private ISysUserService _UserService = null;
        private readonly ISws_DeviceInfo01Service _sws_DeviceInfo01Service;
        private ISws_RTUInfoService _RTUInfoService = null;
        private ISws_StationService _sws_StationService = null;
        private ISws_DeviceTemplateService _sws_DeviceTemplateService = null;
        private readonly ISws_DataInfoService _sws_DataInfoService;
        public Sws_TemplateController(ISws_TemplateService sws_TemplateService,
            ISysUserService sysUserService, ISws_DataInfoService sws_DataInfoService,
            ISws_DeviceInfo01Service sws_DeviceInfo01Service,
            ISws_RTUInfoService sws_RTUInfoService,
            ISws_StationService sws_StationService,
            ISws_DeviceTemplateService sws_DeviceTemplateService)
        {
            _RTUInfoService = sws_RTUInfoService;
            _sws_TemplateService = sws_TemplateService;
            _sws_DataInfoService = sws_DataInfoService;
            _UserService = sysUserService;
            _sws_DeviceInfo01Service = sws_DeviceInfo01Service;
            _sws_StationService = sws_StationService;
            _sws_DeviceTemplateService = sws_DeviceTemplateService;
        }
        public IActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 查询信息
        /// </summary>
        /// <param name="pageSize">分页</param>
        /// <param name="pageIndex"></param>
        /// <param name="sort">排序字段</param>
        /// <param name="sortOrder">排序方式</param>
        /// <param name="templateName">模板名称</param>
        /// <param name="templateType">模板类型</param>
        /// <returns></returns>
        [TypeFilter(typeof(IgonreActionFilter))]
        public ActionResult LoadInfoList(int pageSize, int pageIndex, string sort, string sortOrder, string templateName, int templateType)
        {
            Expression<Func<SwsTemplate, bool>> funcWhere = r => true;
            bool flag = true;
            //查询条件
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = _UserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
            if (user.IsAdmin == true)
            {
                if (!string.IsNullOrEmpty(templateName))
                {
                    funcWhere = r => r.TemplateName.Contains(templateName) && r.DeviceType == templateType;
                }
                else
                {
                    funcWhere = r => r.DeviceType == templateType;
                }

                if (sortOrder == "desc")
                {
                    flag = false;
                }
                var infoList = _sws_TemplateService.QueryPage(funcWhere, pageSize, pageIndex, sort, flag);
                return Json(new { total = infoList.TotalCount, rows = Newtonsoft.Json.JsonConvert.SerializeObject(infoList.DataList.ToList()) });

            }
            else
            {
                if (!string.IsNullOrEmpty(templateName))
                {
                    funcWhere = r => r.TemplateName.Contains(templateName) && r.UserId == user.UserId && r.DeviceType == templateType;
                }
                else
                {
                    funcWhere = r => r.UserId == user.UserId && r.DeviceType == templateType;
                }


                if (sortOrder == "desc")
                {
                    flag = false;
                }
                var infoList = _sws_TemplateService.QueryPage(funcWhere, pageSize, pageIndex, sort, flag);
                return Json(new { total = infoList.TotalCount, rows = Newtonsoft.Json.JsonConvert.SerializeObject(infoList.DataList.ToList()) });
            }

        }
        //分配设备
        [TypeFilter(typeof(IgonreActionFilter))]
        public ActionResult AllowRtu(int id)
        {
            //查询泵房
            //树形
            //int userID = int.Parse(User.FindFirstValue("UserID"));
            //var user = _UserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
            //var treeNodes = TreesOfStationDevice(user, "");
            //if (!string.IsNullOrEmpty(treeNodes))
            //{
            //    ViewBag.TreeNodes = new HtmlString(treeNodes);
            //}
            //else
            //{
            //    ViewBag.TreeNodes = "[]";
            //} 
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = _UserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
            var nodes = GetStationTree(userID, user.IsAdmin, "");
            ViewBag.TreeNodes = new HtmlString(nodes);
            ViewBag.stationid = 0;
            ViewBag.rtuid = 0;
            ViewBag.deviceids = 0;
            ViewBag.temid = id;
            return View();
        }
        //左侧的泵房树
        [TypeFilter(typeof(IgonreActionFilter))]
        private string GetStationTree(int userid, bool isadmin, string stationName)
        {
            var datalist = _sws_DeviceInfo01Service.GetStationTree(userid, isadmin, stationName);
            if (datalist.Count() > 0)
            {
                var treenode = datalist.Select(r => new TreeAction
                {
                    id = r.StationID,
                    pId = 0,
                    name = GetIcon(r.InType) + " " + r.StationName,


                });
                return Newtonsoft.Json.JsonConvert.SerializeObject(treenode);
            }
            else
            {
                return "[]";
            }
        }
        //左侧树查询
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult SearchTree(string stationName)
        {
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = _UserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
            return Content(GetStationTree(userID, user.IsAdmin, stationName));
        }
        [TypeFilter(typeof(IgonreActionFilter))]
        private string GetIcon(int type)
        {
            string icon = "";
            if (type == 1)//供水泵房
            {
                icon = "<em class='iconfont icon-bengfang' style='color:blue'></em>";
            }
            else if (type == 2)//直饮水泵房
            {
                icon = "<em class='iconfont icon-bengfang' style='color:green'></em>";
            }
            return icon;
        }
        //右侧设备列表
        [TypeFilter(typeof(IgonreActionFilter))]
        public ActionResult GetDeviceInfo(int stationid, int temid, string deviceids)
        {
            var stationmodel = _sws_StationService.Query<SwsStation>(r => r.StationId == stationid).FirstOrDefault();
            string tablename = "Sws_DeviceInfo01";

            if (stationmodel.InType == 1)//供水泵房
            {
                tablename = "Sws_DeviceInfo01";
            }
            else if (stationmodel.InType == 2)//直饮水泵站
            {
                tablename = "Sws_DeviceInfo02";
            }
            var f_itemid = (int)Model.CustomizedClass.Enum.设备分区;
            if (string.IsNullOrEmpty(deviceids))
            {
                deviceids = "0";
            }
            var hsdeviceid = _sws_DeviceTemplateService.Query<SwsDeviceTemplate>(r => r.TemplateId == temid).Select(r => r.DeviceId).Distinct().ToList();
            var ids = string.Join(',', hsdeviceid);
            var datalist = _sws_DeviceInfo01Service.GetStationDevice(stationid, temid, tablename, f_itemid, ids);
            return Json(new
            {
                data = datalist
            });
        }
        //更新设备表
        [TypeFilter(typeof(IgonreActionFilter))]
        [HttpPost]
        public IActionResult AddTempDevice(string deviceids, int temid, string key)
        {
            try
            {
                List<long> detailids = new List<string>(deviceids.Split(',')).ConvertAll(r => long.Parse(r));
                if (detailids.Count > 0)
                {
                    if (key == "Add")
                    {
                        foreach (var it in detailids)
                        {
                            //查询模板中是否已存在设备模板，存在则删除
                            var infohas = _sws_DeviceTemplateService.Query<SwsDeviceTemplate>(r => r.DeviceId == it).ToList();
                            if (infohas.Count > 0)
                            {
                                foreach (var i in infohas)
                                {
                                    _sws_DeviceTemplateService.Delete(i);
                                }

                            }
                            SwsDeviceTemplate swsDeviceTemplate = new SwsDeviceTemplate();
                            swsDeviceTemplate.DeviceId = it;
                            swsDeviceTemplate.TemplateId = temid;
                            _sws_DeviceTemplateService.Insert(swsDeviceTemplate);
                        }
                    }
                    else
                    {
                        var infoList = _sws_DeviceTemplateService.Query<SwsDeviceTemplate>(r => detailids.Contains(r.DeviceId)).ToList();
                        foreach (var item in infoList)
                        {
                            _sws_DeviceTemplateService.Delete(item);
                        }
                    }
                }
                return Content("ok");
            }
            catch (Exception ex)
            {
                return Content("no");
            }
        }
        //泵房树形
        [TypeFilter(typeof(IgonreActionFilter))]
        public string TreesOfStationDevice(SysUser sysUser, string stationName)
        {
            List<StationAndDevice> treelist = _sws_DeviceInfo01Service.QueryZtreeInfo(sysUser, stationName).ToList();

            IEnumerable<TreeAction> ztreeStation = treelist.Select(t => new TreeAction
            {
                id = t.StationId,
                pId = 0,
                name = "<em class='iconfont icon-bengfang'></em>" + t.StationName,
                @checked = false,
                isDevice = false
                //icon = "../../Content/zTree/css/zTreeStyle/area.png"
            });

            IEnumerable<TreeAction> ztreeDevice = treelist.Select(t => new TreeAction
            {
                id = t.DeviceId,
                pId = t.StationId,
                name = t.DeviceName,
                @checked = false,
                isDevice = true
                //icon = "../../Content/zTree/css/zTreeStyle/area.png"
            });
            var treeList = ztreeStation.Union<TreeAction>(ztreeDevice).Distinct(new TreeAreaCompare());
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(treeList);
            return json;
        }

        #region 分配泵房
        [TypeFilter(typeof(IgonreActionFilter))]
        public ActionResult AllowStation(int id)
        {
            ViewBag.temid = id;
            return View();
        }
        //右侧设备列表
        [TypeFilter(typeof(IgonreActionFilter))]
        public ActionResult GetStationInfo(int temid, string deviceids)
        {
            if (string.IsNullOrEmpty(deviceids))
            {
                deviceids = "0";
            }
            var hsdeviceid = _sws_DeviceTemplateService.Query<SwsDeviceTemplate>(r => r.TemplateId == temid).Select(r => r.DeviceId).Distinct().ToList();
            var ids = string.Join(',', hsdeviceid);
            var datalist = _sws_DeviceInfo01Service.GetStationInfo(temid, ids);
            return Json(new
            {
                data = datalist
            });
        }
        //更新设备表
        [TypeFilter(typeof(IgonreActionFilter))]
        [HttpPost]
        public IActionResult AddTempStation(string deviceids, int temid, string key)
        {
            try
            {
                List<long> detailids = new List<string>(deviceids.Split(',')).ConvertAll(r => long.Parse(r));
                if (detailids.Count > 0)
                {
                    if (key == "Add")
                    {
                        foreach (var it in detailids)
                        {
                            //查询模板中是否已存在设备模板，存在则删除
                            var infohas = _sws_DeviceTemplateService.Query<SwsDeviceTemplate>(r => r.DeviceId == it && r.DeviceType == 1).ToList();
                            if (infohas.Count > 0)
                            {
                                foreach (var i in infohas)
                                {
                                    _sws_DeviceTemplateService.Delete(i);
                                }

                            }
                            SwsDeviceTemplate swsDeviceTemplate = new SwsDeviceTemplate();
                            swsDeviceTemplate.DeviceId = it;
                            swsDeviceTemplate.TemplateId = temid;
                            swsDeviceTemplate.DeviceType = 1;
                            _sws_DeviceTemplateService.Insert(swsDeviceTemplate);
                        }
                    }
                    else
                    {
                        var infoList = _sws_DeviceTemplateService.Query<SwsDeviceTemplate>(r => detailids.Contains(r.DeviceId)).ToList();
                        foreach (var item in infoList)
                        {
                            _sws_DeviceTemplateService.Delete(item);
                        }
                    }
                }
                return Content("ok");
            }
            catch (Exception ex)
            {
                return Content("no");
            }
        }
        #endregion


        #region 配置模板
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult SetTemplate(string type)
        {
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = _UserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
            int types = Convert.ToInt32(type);
            List<long> dataid = new List<long>() { 1000, 1001 };
            //根据用户信息获取用户模板
            if (user.IsAdmin)
            {
                var temInfoList = _sws_TemplateService.Query<SwsTemplate>(r => r.DeviceType == types && r.Classify == 1).OrderByDescending(r => r.FocusOn).ToList();
                ViewBag.Template = temInfoList;
            }
            else
            {
                var temInfoList = _sws_TemplateService.Query<SwsTemplate>(r => r.UserId == user.UserId && r.DeviceType == types && r.Classify == 1).OrderByDescending(r => r.FocusOn).ToList();
                ViewBag.Template = temInfoList;
            }

            var mo = _sws_TemplateService.Query<SwsTemplate>(r => r.UserId == user.UserId && r.DeviceType == types && r.Classify == 1 && r.FocusOn == true).FirstOrDefault();
            if (mo != null)
            {
                ViewBag.tempid = mo.Id;
            }
            else
            {
                ViewBag.tempid = 0;
            }
            ViewBag.TypeID = type;
            return View();
        }
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult LoadTempData(int id, int type)
        {
            if (id == 0)
            {
                List<SwsDataInfo> endhasData = new List<SwsDataInfo>();
                List<SwsDataInfo> nohasData = new List<SwsDataInfo>();
                var nodata = _sws_DataInfoService.Query<SwsDataInfo>(r => r.DataId < 2500 && r.DeviceType == type && r.IsShow == true).OrderBy(r => r.Enname).ToList();
                if (type == 3)
                {
                    nodata = _sws_DataInfoService.Query<SwsDataInfo>(r => r.DataId >= 4500 && r.DataId < 4739 && r.DeviceType == 1 && r.IsShow == true).OrderBy(r => r.DataId).ToList();
                }
                foreach (var item in nodata)
                {
                    if (type == 1)
                    {
                        item.Cnname = item.Cnname.Substring(2, item.Cnname.Length - 2);
                    }
                    if (nohasData.Where(r => r.Cnname == item.Cnname).Count() > 0)
                    {

                    }
                    else
                    {
                        nohasData.Add(item);
                    }

                }
                var rel = new
                {
                    endhasData,
                    nohasData
                };
                return Content(Newtonsoft.Json.JsonConvert.SerializeObject(rel));
            }
            else
            {
                var tempInfo = _sws_TemplateService.Query<SwsTemplate>(r => r.Id == id).FirstOrDefault();
                string dataid = tempInfo.DataId;
                string[] listdata = dataid.Split(',');
                //查询已存在的datdid
                List<int> ids = new List<int>();
                foreach (var item in listdata)
                {
                    ids.Add(int.Parse(item));
                }
                List<SwsDataInfo> endhasData = new List<SwsDataInfo>();
                List<SwsDataInfo> nohasData = new List<SwsDataInfo>();
                //已存在的
                var hasData = _sws_DataInfoService.Query<SwsDataInfo>(r => ids.Contains(r.DataId) && r.DataId < 2500 && r.DeviceType == type && r.IsShow == true).ToList();
                if (type == 3)
                {
                    hasData = _sws_DataInfoService.Query<SwsDataInfo>(r => ids.Contains(r.DataId) && r.DataId >= 4500 && r.DataId < 4739 && r.DeviceType == 1 && r.IsShow == true).ToList();
                }

                foreach (var item in hasData)
                {
                    if (type == 1)
                    {
                        item.Cnname = item.Cnname.Substring(2, item.Cnname.Length - 2);
                    }
                    if (endhasData.Where(r => r.Cnname == item.Cnname).Count() > 0)
                    {

                    }
                    else
                    {
                        endhasData.Add(item);
                    }

                }
                var nodata = _sws_DataInfoService.Query<SwsDataInfo>(r => !ids.Contains(r.DataId) && r.DataId < 2500 && r.DeviceType == type && r.IsShow == true).ToList();
                if (type == 3)
                {
                    nodata = _sws_DataInfoService.Query<SwsDataInfo>(r => !ids.Contains(r.DataId) && r.DataId >= 4500 && r.DataId < 4739 && r.DeviceType == 1 && r.IsShow == true).ToList();
                }
                foreach (var item in nodata)
                {
                    if (type == 1)
                    {
                        item.Cnname = item.Cnname.Substring(2, item.Cnname.Length - 2);
                    }
                    if (nohasData.Where(r => r.Cnname == item.Cnname).Count() > 0)
                    {

                    }
                    else
                    {
                        nohasData.Add(item);
                    }

                }
                var rel = new
                {
                    endhasData,
                    nohasData
                };
                return Content(Newtonsoft.Json.JsonConvert.SerializeObject(rel));
            }

        }
        //修改模板信息
        [HttpPost]
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult UpdateTep(string dataArr, int tempid, byte type, string temName)
        {
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = _UserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
            if (tempid == 0)
            {
                if (string.IsNullOrEmpty(dataArr))
                {
                    return Content("nonull");
                }
                SwsTemplate tempInfo = new SwsTemplate();
                tempInfo.Id = _UserService.QueryID("Id", "Sws_Template");
                tempInfo.UserId = userID;
                if (dataArr.Length > 0)
                {
                    tempInfo.DataId = dataArr;
                }
                if (!string.IsNullOrEmpty(temName))
                {
                    tempInfo.TemplateName = temName;
                }
                tempInfo.DeviceType = type;
                tempInfo.Classify = 1;
                if (_sws_TemplateService.Insert<SwsTemplate>(tempInfo) != null)
                {
                    return Content("ok");
                }
                else
                {
                    return Content("no");
                }
            }
            else
            {
                var tempInfo = _sws_TemplateService.Query<SwsTemplate>(r => r.Id == tempid).FirstOrDefault();
                if (dataArr.Length > 0)
                {
                    tempInfo.DataId = dataArr;
                }
                if (!string.IsNullOrEmpty(temName))
                {
                    tempInfo.TemplateName = temName;
                }
                if (_sws_TemplateService.Update<SwsTemplate>(tempInfo))
                {
                    return Content("ok");
                }
                else
                {
                    return Content("no");
                }
            }

        }
        //删除模板
        [HttpPost]
        [TypeFilter(typeof(IgonreActionFilter))]
        public ActionResult Deletetemp(int id)
        {
            var templates = _sws_TemplateService.Query<SwsTemplate>(t => t.Id == id).FirstOrDefault();
            if (_sws_TemplateService.Delete(templates))
            {
                var userdeivce = _sws_DeviceTemplateService.Query<SwsDeviceTemplate>(r => r.TemplateId == id).ToList();
                foreach (var item in userdeivce)
                {
                    _sws_DeviceTemplateService.Delete(item);
                }
                return Content("ok");
            }
            else
            {
                return Content("no");
            }
        }
        #endregion
    }
}