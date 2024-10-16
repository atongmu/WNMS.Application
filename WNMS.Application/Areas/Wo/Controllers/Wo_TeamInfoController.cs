using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using cn.jpush.api.report;
using Microsoft.AspNetCore.Html;
using WNMS.Application.Utility;
using WNMS.Application.Utility.Filters;
using WNMS.IService;
using WNMS.Model.CustomizedClass;
using WNMS.Model.DataModels;

namespace WNMS.Application.Areas.Wo
{
    [Area("Wo")]
    [TypeFilter(typeof(IgonreActionFilter))]
    public class Wo_TeamInfoController : Controller
    {

    
        IWO_TeamInfoService _woTeamInfoService = null;
        IWO_AreaInfoService _woAreaInfoService =null;
        public ISysUserService _sysUserService = null;

        public Wo_TeamInfoController(
            IWO_AreaInfoService wo_AreaInfoService,
            IWO_TeamInfoService wo_TeamInfoService,
            ISysUserService sysUserService

        )
        {

            _woAreaInfoService = wo_AreaInfoService;
            _woTeamInfoService = wo_TeamInfoService;
            _sysUserService = sysUserService;
        }
        public ActionResult Index()
        {
            var teamtreeData = _woTeamInfoService.GetTeamTreeNode();
            if (teamtreeData.Count() > 0)
            {
                var treenode = teamtreeData.Select(r => new TreeAreaString
                {
                    id = r.IsTeam == 1 ? r.ID.ToString() + "t" : r.ID.ToString(),
                    pId = r.PID.ToString(),
                    name = r.IsTeam != 0 ? ("<i class='iconfont icon-yonghu'></i>" + r.AreaName):("<i class='fa fa-globe'></i>" + r.AreaName),
                    @checked = false,
                   // icon = r.IsTeam != 0 ? "../../../Ionicons/png/512/android-user-menu.png" : "../../../Ionicons/png/512/earth.png",
                    type = r.IsTeam.ToString()
                });
                ViewBag.TreeNodes = new HtmlString(Newtonsoft.Json.JsonConvert.SerializeObject(treenode));
            }
            else
            {
                ViewBag.TreeNodes = new HtmlString("[]");
            }

            return View();
        }


        public ActionResult AddPage()
        {
            var regionlist = _woAreaInfoService.Query<WoAreaInfo>(r => true).ToList();
            ViewBag.regionlist = regionlist;
            ViewBag.userids = "";
            ViewBag.usernames = "";
            return View("SetTeamInfo", new WoTeamInfo());
        }
        public ActionResult EditePage(int id)
        {
            var regionlist = _woAreaInfoService.Query<WoAreaInfo>(r => true);
            ViewBag.regionlist = regionlist;
            var teammodel = _woTeamInfoService.Query<WoTeamInfo>(r => r.TeamId == id).FirstOrDefault();
            var datas = _woTeamInfoService.GetUserByTeamID(id).ToList();
            string ids = "", names = "";
            if (datas.Count > 0)
            {
                var userids = datas.Select(r => r.UserID.ToString()).ToList();
                var nickNames = datas.Select(r => r.NickName).ToList();
                ids = string.Join(",", userids);
                names = string.Join(",", nickNames);
               
            }
            ViewBag.userids = ids;
            ViewBag.usernames = names;
            return View("SetTeamInfo", teammodel);
        }

        [HttpPost]

        public ActionResult SetTeamInfo(WoTeamInfo t, string UserIDs)
        {
            var flag = t.TeamId;
            if (t.TeamId == 0)
            {
                t.TeamId = _sysUserService.QueryID("TeamID", "WO_TeamInfo");

            }
            List<int> userlist = new List<int>();
            if (!string.IsNullOrEmpty(UserIDs))
            {
                userlist = new List<string>(UserIDs.Split(',')).ConvertAll(r => int.Parse(r));
            }

            List<WoTeamUser> module = new List<WoTeamUser>();
            if (userlist.Count > 0)
            {

                foreach (var item in userlist)
                {
                    WoTeamUser tuser = new WoTeamUser();

                    tuser.TeamId = t.TeamId;
                    tuser.UserId = item;
                    module.Add(tuser);
                }
            }
            if (flag == 0)//添加
            {
                if (_woTeamInfoService.AddTeamInfo(t, module) > 0)
                {
                    return Content("ok");
                }
                else
                {
                    return Content("no");
                }
            }
            else//修改
            {
                if (_woTeamInfoService.EditeTeamInfo(t, module) > 0)
                {
                    return Content("ok");
                }
                else
                {
                    return Content("no");
                }
            }
           

        }

        public static long ConvertDateTimeInt(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (long)(time - startTime).TotalMilliseconds;
        }
        [HttpPost]
        public ActionResult DeleteInfos(string teamids)
        {

            string[] ids = teamids.Split(',');
            List<long> team_ids = new List<long>();
            foreach (string item in ids)
            {
                team_ids.Add(long.Parse(item));
            }

            if (_woTeamInfoService.DeleteTeamInfos(team_ids)>0)
            {
                return Content("ok");

            }
            else
            {
                return Content("no");
            }
        }
        #region 获取班组table数据
   
        public async Task<IActionResult> GetTableData(string username, int teamid, string sort, string order, int pageSize, int pageIndex)
        {
           
                int totalcount = 0;
                var data = _woTeamInfoService.GetTeamUserInfo(teamid, username, sort, order, pageSize, pageIndex, ref totalcount).ToList();
                var tempdata = "";
                if (data.Count > 0)
                {
                    tempdata = Newtonsoft.Json.JsonConvert.SerializeObject(data);
                }
                PartialView("TeamTable", tempdata);
                string htmlTable = await ViewToString.RenderPartialViewToString(this, "TeamTable");
                double TotalPage = Math.Ceiling((float)totalcount / (float)pageSize);
                return Json(new
                {
                    table = htmlTable,
                    TotalPage = TotalPage
                });
           
          
        }
        #endregion
        #region 人员分配
  
        public ActionResult AllotUser(int teamid,string UserIDs)
        {
            List<int> useridList = new List<int>();
            if (!string.IsNullOrEmpty(UserIDs))
            {
                useridList = new List<string>(UserIDs.Split(",")).ConvertAll(r => int.Parse(r));
            }
            var userlist = _woTeamInfoService.GetUnallotUser(teamid);
            var departlist = userlist.Select(r => new treestring
            {
                id = r.DepartmentID.ToString(),
                pId = r.PID.ToString(),
                name = "<i class='fa fa-sitemap'></i>"+r.DptName,
                @checked = false,
                isUser = false
            }).ToList().Distinct(new treestringCompare());
            var ulist = userlist.Select(r => new treestring
            {
                id = r.UserID.ToString() + "u",
                pId = r.DepartmentID.ToString(),
                name = "<i class='iconfont  icon-yonghu'></i>"+ r.NickName.ToString(),
                @checked = useridList.Count>0?(useridList.Contains(r.UserID)?true:false) :false,
                isUser = true,
                inspectName= r.NickName.ToString()
            });
            var data = departlist.Concat(ulist);
            if (data.Count() > 0)
            {
                ViewBag.ztreenode = new HtmlString(Newtonsoft.Json.JsonConvert.SerializeObject(data));
            }
            else
            {
                ViewBag.ztreenode = new HtmlString("[]");
            }
            ViewBag.allotUser = userlist.Where(r=> useridList.Contains(r.UserID));

            ViewBag.TeamID = teamid;
            return View();
        }
        //提交人员分配数据
   
        public ActionResult SetAllotUser(int teamid, string userids)
        {
      
         var tumodel=   _woTeamInfoService.Find<WoTeamInfo>(teamid);
            var userlist = new List<string>(userids.Split(',')).ConvertAll(r => int.Parse(r));
            List<WoTeamUser> module = new List<WoTeamUser>();
            if (userlist.Count > 0)
            {
                foreach (var item in userlist)
                {
                    WoTeamUser tuser = new WoTeamUser();

                    tuser.TeamId =  teamid;
                    tuser.UserId = item;
                    module.Add(tuser);
                }
            }
            if (_woTeamInfoService.SetTeamUser(tumodel, module) == 0)
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }
        }
        //部门人员树查询
   
        public ActionResult GetTreeNodes(string searchtxt, int teamid, string userids)
        {
            HtmlString result;
            List<int> useridList = new List<int>();
            if (!string.IsNullOrEmpty(userids))
            {
                useridList = new List<string>(userids.Split(",")).ConvertAll(r => int.Parse(r));
            }
            if (!string.IsNullOrEmpty(searchtxt))
            {
                var list = _woTeamInfoService.GetUserTree(searchtxt, teamid);
                var department1 = list.Select(r => new treestring
                {
                    id = r.DepartmentID.ToString(),
                    pId = r.PID.ToString(),
                    name = "<i class='fa fa-sitemap'></i>" + r.DptName,
                    @checked = false,
                    isUser = false
                });
                var userlist = list.Where(r => r.type == 2).Select(r => new treestring
                {
                    id = r.UserID.ToString() + "u",
                    pId = r.DepartmentID.ToString(),
                    name = "<i class='iconfont  icon-yonghu'></i>" + r.NickName.ToString(),
                    @checked = useridList.Count > 0 ? (useridList.Contains(r.UserID) ? true : false) : false,
                    isUser = true,
                    inspectName = r.NickName.ToString()
                });
                var data = department1.Concat(userlist).Distinct(new treestringCompare());
                if (data.Count() > 0)
                {
                    result = new HtmlString(Newtonsoft.Json.JsonConvert.SerializeObject(data));
                }
                else
                {
                    result = new HtmlString("[]");
                }
            }
            else
            {
                var userlist = _woTeamInfoService.GetUnallotUser(teamid);
                var departlist = userlist.Select(r => new treestring
                {
                    id = r.DepartmentID.ToString(),
                    pId = r.PID.ToString(),
                    name = "<i class='fa fa-sitemap'></i>" + r.DptName,
                    @checked = false,
                    isUser = false
                }).ToList().Distinct(new treestringCompare());
                var ulist = userlist.Select(r => new treestring
                {
                    id = r.UserID.ToString() + "u",
                    pId = r.DepartmentID.ToString(),
                    name = "<i class='iconfont  icon-yonghu'></i>" + r.NickName.ToString(),
                    @checked = useridList.Count > 0 ? (useridList.Contains(r.UserID) ? true : false) : false,
                    isUser = true,
                    inspectName = r.NickName.ToString()
                });
                var data = departlist.Concat(ulist);
                if (data.Count() > 0)
                {
                    result = new HtmlString(Newtonsoft.Json.JsonConvert.SerializeObject(data));
                }
                else
                {
                    result = new HtmlString("[]");
                }
            }
            return Content(result.ToString());
        }
        public class treestring
        {
            public string id { get; set; }
            public string pId { get; set; }
            public string name { get; set; }
            public bool @checked { get; set; }
            public string icon { get; set; }
            public bool isUser { get; set; }
            public string inspectName { get; set; }
        }
        public class treestringCompare : IEqualityComparer<treestring>
        {
            public bool Equals(treestring x, treestring y)
            {
                if (x == null)
                    return y == null;
                return x.id == y.id;
            }

            public int GetHashCode(treestring obj)
            {
                if (obj == null)
                    return 0;
                return obj.id.GetHashCode();
            }
        }
        #endregion
        #region 区域-班组树查询
     
        public ActionResult SearchTeamTree(string teamname)
        {
            HtmlString treenode = null;
            if (!string.IsNullOrEmpty(teamname))
            {
               
                    var datalist = _woTeamInfoService.SearchTreeTeam(teamname);
                    if (datalist.Count() > 0)
                    {
                        var data = datalist.Select(r => new TreeAreaString
                        {
                            id = r.IsTeam == 1 ? r.ID.ToString() + "t" : r.ID.ToString(),
                            pId = r.PID.ToString(),
                            name = r.IsTeam != 0 ? ("<i class='iconfont icon-yonghu'></i>" + r.AreaName) : ("<i class='fa fa-globe'></i>" + r.AreaName),
                            @checked = false,

                            type = r.IsTeam.ToString()
                        });
                        treenode = new HtmlString(Newtonsoft.Json.JsonConvert.SerializeObject(data));
                    }
                    else
                    {
                        treenode = new HtmlString("[]");
                    }
               
            }
            else
            {
                var teamtreeData = _woTeamInfoService.GetTeamTreeNode();
                if (teamtreeData.Count() > 0)
                {
                    var treenodes = teamtreeData.Select(r => new TreeAreaString
                    {
                        id = r.IsTeam == 1 ? r.ID.ToString() + "t" : r.ID.ToString(),
                        pId = r.PID.ToString(),
                        name = r.IsTeam != 0 ? ("<i class='iconfont icon-yonghu'></i>" + r.AreaName) : ("<i class='fa fa-globe'></i>" + r.AreaName),
                        @checked = false,
                       
                        type = r.IsTeam.ToString()
                    });
                    treenode = new HtmlString(Newtonsoft.Json.JsonConvert.SerializeObject(treenodes));
                }
                else
                {
                    treenode = new HtmlString("[]");
                }
            }
            return Content(treenode.ToString());
        }
        #endregion
    }
}
