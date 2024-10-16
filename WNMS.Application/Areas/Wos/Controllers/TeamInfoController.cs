using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WNMS.IService;
using WNMS.Service;
using WNMS.Model.DataModels;
using Microsoft.AspNetCore.Html;
using WNMS.Application.Utility.Filters;

namespace WNMS.Application.Areas.Wos.Controllers
{
    [Area("Wos")]
    public class TeamInfoController : Controller
    {
        private IGD_TeamInfoService _GD_TeamInfoService = null;
        private ISysUserService _ISys_UserService = null;
        
        public TeamInfoController(IGD_TeamInfoService gD_TeamInfoService,
            ISysUserService sysUserService
            )
        {
            _GD_TeamInfoService = gD_TeamInfoService;
            _ISys_UserService = sysUserService;
        }
        public IActionResult Index()
        {
            return View();
        }
        //表格数据获取
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult QueryTable(int pagesize,int pageindex,string SearchText,string order,string sort)
        {
            int Totalcount = 0;
            string ordertems = sort + " " + order;
            string filter = "1=1";
            if (!string.IsNullOrEmpty(SearchText))
            {
                filter += " and TeamName like '%"+ SearchText + "%'";
            }

            var datalist = _GD_TeamInfoService.GetTeamInfoTable(pageindex, pagesize, ordertems, filter, ref Totalcount);
            return Json(new { total = Totalcount, rows = Newtonsoft.Json.JsonConvert.SerializeObject(datalist) });
        }
        #region 页面操作
        //添加
        public IActionResult AddPage()
        {
            ViewBag.UserNames = "";
            ViewBag.UserIDs = "";
            return View("_SetTeamInfo",new GdTeamInfo());
        }
        public IActionResult EditePage(int id)
        {
            var teaminfo = _GD_TeamInfoService.Query<GdTeamInfo>(r => r.TeamId == id).FirstOrDefault();
            var teamuser = _GD_TeamInfoService.GetAllotUser(id);
            if (teamuser.Count() > 0)
            {
                var userids = teamuser.Select(r => r.UserID);
                var usernames= teamuser.Select(r => r.Account);
                ViewBag.UserNames =string.Join(",", usernames) ;
                ViewBag.UserIDs = string.Join(",", userids);
            }
            else
            {
                ViewBag.UserNames = "";
                ViewBag.UserIDs = "";
            }
            return View("_SetTeamInfo", teaminfo);

        }
        //表单提交
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult SetTeamInfo(string UserIDs,GdTeamInfo t)
        {
            List<int> userIDList = new List<string>(UserIDs.Split(",")).ConvertAll(r => int.Parse(r));
            if (t.TeamId == 0)
            {
                t.TeamId = _ISys_UserService.QueryID("TeamID", "GD_TeamInfo");
                if (_GD_TeamInfoService.AddTeamInfo(t, userIDList) > 0)
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
                if (_GD_TeamInfoService.EditeTeamInfo(t, userIDList) > 0)
                {
                    return Content("ok");
                }
                else
                {
                    return Content("no");
                }
            }
        }
        //删除
        public IActionResult DeleteTeams(string id)
        {
            if (_GD_TeamInfoService.DeleteTeamInfo(id) > 0)
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }
        }
        #region 人员选择
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult SelectUsers(int id,string selectUserids)
        {
            if (string.IsNullOrEmpty(selectUserids))
            {
                selectUserids = "-1";
            }
            var nodeData = GetDepartUserTree(id,"", selectUserids);
            ViewBag.treenodes =new HtmlString(nodeData);

            List<int> idlist = new List<string>(selectUserids.Split(",")).ConvertAll(r => int.Parse(r));
            ViewBag.allotUser = _ISys_UserService.Query<SysUser>(r=>idlist.Contains(r.UserId));
            ViewBag.teamid = id;
            ViewBag.selectUserids = selectUserids;
            return View();
        }
        //树查询
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult SearchTree(string searchtxt,int teamid,string selectUserids)
        {
            if (string.IsNullOrEmpty(selectUserids))
            {
                selectUserids = "-1";
            }
            var data= GetDepartUserTree(teamid, searchtxt, selectUserids);
            return Content(data);
        }
        private string GetDepartUserTree(int teamid,string searchText,string selectUserids)
        {
            var list = _GD_TeamInfoService.GetDepartUserNode(searchText, teamid, selectUserids);
            if (list.Count() > 0)
            {
                var department1 = list.Select(r => new treestring
                {
                    id = r.DepartmentID.ToString(),
                    pId = r.PID.ToString(),
                    name = "<i class='fa fa-share-alt'></i>" + r.DptName,
                    @checked = false,
                    isUser = false,
                    playname= r.DptName

                });
                var userlist = list.Where(r => r.type == 2).Select(r => new treestring
                {
                    id = r.UserID.ToString() + "u",
                    pId = r.DepartmentID.ToString(),
                    name = "<i class='fa fa-user-circle-o'></i>"+ r.Account.ToString(),
                    @checked = (r.Ischeack.ToString() == "0" ? false : true),
                    isUser = true,
                    playname= r.Account.ToString()

                });
                var data = department1.Concat(userlist).Distinct(new treestringCompare());
                if (data.Count() > 0)
                {
                    return Newtonsoft.Json.JsonConvert.SerializeObject(data);
                }
                else
                {
                    return "[]";
                }
            }
            else
            {
                return "[]";
            }
        }
        public class treestring
        {
            public string id { get; set; }
            public string pId { get; set; }
            public string name { get; set; }
            public bool @checked { get; set; }
            public string icon { get; set; }
            public bool isUser { get; set; }
            public string playname { get; set; }
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
        //详细人员表
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult ShowDetailUser(int id)
        {
            var datalist = _GD_TeamInfoService.GetAllotUser(id);
            ViewBag.datalist = datalist;
            return View();
        }
        #endregion
    }
}