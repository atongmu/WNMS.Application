using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cn.jpush.api.report;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Html;
using WNMS.Application.Utility;
using WNMS.Application.Utility.Filters;
using WNMS.Application.Utility.JsonHelper;
using WNMS.IService;
using WNMS.Model.CustomizedClass;
using WNMS.Model.DataModels;
using WNMS.Service;

namespace WNMS.Application.Areas.Wo
{
    [Area("Wo")]
    [TypeFilter(typeof(IgonreActionFilter))]
    public class Wo_AreaInfoController : Controller
    {
        public IWO_AreaInfoService _wo_AreaInfoService = null;
       public IWO_TeamInfoService _wo_TeamInfoService = null;
       public ISysUserService _sysUserService = null;
        //I_wo_AreaInfoService _wo_AreaInfoService = new _wo_AreaInfoService();
        public Wo_AreaInfoController(
            IWO_AreaInfoService wo_AreaInfoService,
            IWO_TeamInfoService wo_TeamInfoService,
            ISysUserService sysUserService
            )
        {
         
            _wo_AreaInfoService = wo_AreaInfoService;
            _wo_TeamInfoService = wo_TeamInfoService;
            _sysUserService = sysUserService;
        }

        public IActionResult Index()
        {

            //var path = _webHostEnvironment.ContentRootPath;
            //JsonFileHelper jsonFileHelper = new JsonFileHelper(path + "/Utility/ProjectJson/project.json");
            //SystemInfo st = jsonFileHelper.Read<SystemInfo>("SystemInfo");
            //ViewBag.lng = st.Lng;
            //ViewBag.lat = st.Lat;
            //ViewBag.zoom = st.MapLevel;
            //ViewBag.AreaName = st.AreaName;
            var treenode = _wo_AreaInfoService.Query<WoAreaInfo>(w => true).Select(r => new
            {
                id = r.Id,
                name = r.AreaName,
                pId = r.Pid,
                icon = "../../images/open.png"
            }).ToList();


            treenode.Add(new
            {
                id = 0,
                name = "所有区域",
                pId = -1,
                icon = "../../images/open.png"
            }
            );
            ViewBag.TreeNodes = new HtmlString(Newtonsoft.Json.JsonConvert.SerializeObject(treenode));
            return View();
        }



        //提交表单

        public ActionResult SetGD_areaInfo(int areaID, int pid, string areaName, string fillColor, string points)
        {
            var type = 1;//0代表添加，1代表修改
            if (areaID == 0)//添加
            {

                areaID = _sysUserService.QueryID("ID", "WO_AreaInfo");
                type = 0;
            }
            var query = _wo_AreaInfoService.SetAreaInfo(areaID, pid, areaName, fillColor, points, type);
            if (query > 0)
            {
                return Json(new
                {
                    result = "ok",
                    data = areaID
                });
            }
            else
            {
                return Json(new
                {
                    result = "no",
                    data = areaID
                });
            }
     
        }
        //获取区域的信息

        public ActionResult GetAreaInfo(string IDs)
        {
            var query = _wo_AreaInfoService.GetAreaInfo(IDs);
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(query));

        }
        //获取区域的设备
 
        public async Task<IActionResult> DisplayRtu(int areaID, string sort, int pageSize, string deviceName)
        {
            double totalPage = 0;
            int TotalCount = 0;
            string dataTable = "";
            var data = _wo_AreaInfoService.GetRtuInfoOfArea(areaID,deviceName);
            IEnumerable<dynamic> tabledata;
            string result = "";
            if (data.Count() > 0)
            {
                if (sort == "asc")
                {
                    tabledata = data.OrderBy(r => r.DeviceName);
                }
                else
                {
                    tabledata = data.OrderByDescending(r => r.DeviceName);
                }
                tabledata = tabledata.Take(pageSize);
                result = Newtonsoft.Json.JsonConvert.SerializeObject(data);
                TotalCount = data.Count();
                totalPage = Math.Ceiling((float)TotalCount / (float)pageSize);
                ViewBag.initnum = 1;
                PartialView("RtuTable", tabledata);
                dataTable = await ViewToString.RenderPartialViewToString(this, "RtuTable");
               
                //double totalPage = Math.Ceiling((double)totalCount / pageSize);
                //return Json(new
                //{
                //    totalPage = totalPage,
                //    //pageIndex = pageIndex,
                //    pageSize = pageSize,
                //    dataTable = dataTable
                //});
            }
            else
            {
                tabledata = null;
            }

            return Json(new
            {
                Rtudata = result,
                dataTable = dataTable,
                TotalCount = TotalCount,
                totalPage = totalPage

            });
        }
        //设备分页

        public async Task<IActionResult> GetRtuByPage(int areaID, string sort, int pageSize, string devicename, int pageIndex)
        {
            int TotalCount = 0;
            var data = _wo_AreaInfoService.GetRtuInfoOfAreaPage(areaID, sort, pageSize, pageIndex, devicename, ref TotalCount);
            string dataTable = "";
            if (data.Count() > 0)
            {
                ViewBag.initnum = (pageIndex - 1) * pageSize + 1;
                PartialView("RtuTable", data);
                dataTable = await ViewToString.RenderPartialViewToString(this, "RtuTable");
            }
            return Json(new
            {

                dataTable = dataTable,
                TotalCount = TotalCount,
            });
        }
        //删除区域
 
        public ActionResult DeleteAreaInfo(int areaID)
        {
            List<string> areanames = new List<string>();
            string data = "ok";
            if (_wo_AreaInfoService.DeleteAreaInfo(areaID, ref areanames))
            {
                if (areanames.Count() > 0)
                {
                    string names = "";
                    foreach (var item in areanames)
                    {
                        names += item + ",";
                    }
                    names = names.Substring(0, names.Length - 1);
                    data = "区域" + names + "被占用，无法删除";
                }
                else
                {
                    data = "ok";
                }

            }
            else
            {
                if (areanames.Count() > 0)
                {
                    data = "区域被占用无法删除";
                }
                else
                {
                    data = "no";
                }
            }
            return Content(data);
        }
        //区域查询

        public ActionResult SearchTree(string areaName)
        {
            var arealist = _wo_AreaInfoService.SearchAreaTree(areaName).Select(r => new
            {
                id = r.ID,
                name = r.AreaName,
                pId = r.PID,
                icon = "../../images/open.png"
            }).ToList();
            arealist.Add(new
            {
                id = (dynamic)0,
                name = (dynamic)"所有区域",
                pId = (dynamic)(-1),
                icon = "../../images/open.png"
            }
             );
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(arealist));
        }
        #region 班组数据

        public ActionResult GetTeamUser(int areaID)
        {
            int pageindex = 1;
            int pagesize = 20;
            var teamlist = _wo_TeamInfoService.Query<WoTeamInfo>(r => r.RegionId == areaID).OrderBy(r => r.TeamName);
            int TotalCount = 0;
            double totalPage = 1;
            List<SysUser> tuserlist = new List<SysUser>();
            if (teamlist.Count() > 0)
            {
               
                tuserlist = _wo_TeamInfoService.GetUserByTeamIDOfPage(teamlist.FirstOrDefault().TeamId, pageindex, pagesize, ref TotalCount).ToList();
                if (tuserlist.Count > 0)
                {
                    totalPage = Math.Ceiling((float)TotalCount / (float)pagesize);
                }
            }
            return Json(new
            {
                teamlist = teamlist,
                tuserlist = tuserlist,
                totalPage = totalPage

            });
        }
  
        public ActionResult GetSomeTeamUser(long teamid, int pageindex)
        {

            int pagesize = 20;

            int TotalCount = 0;
            double totalPage = 1;
            List<SysUser> tuserlist = new List<SysUser>();
            tuserlist = _wo_TeamInfoService.GetUserByTeamIDOfPage(teamid, pageindex, pagesize, ref TotalCount).ToList();
            if (tuserlist.Count > 0)
            {
                totalPage = Math.Ceiling((float)TotalCount / (float)pagesize);
            }
            return Json(new
            {

                tuserlist = tuserlist,
                totalPage = totalPage

            });
        }
        #endregion


    }
}
