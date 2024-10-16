using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WNMS.Application.Utility.Filters;
using WNMS.IService;
using WNMS.Model.DataModels;
using WNMS.Utility;
using HttpUtil;
using WNMS.Model.CustomizedClass;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WNMS.Application.Utility;

namespace WNMS.Application.Areas.Sws
{
    [Area("Sws")]
    public class Sws_CameraPlaybackController : Controller
    {

        #region 属性 构造函数
        private ISws_AccessControlService _AccessControlService = null;
        private ISysUserService _UserService = null;
        private ISws_CameraService cameraService = null;
        int timevalue = int.Parse(StaticConstraint.TimeValue);
        string cameraIP = StaticConstraint.CameraService;
        string cameraLivePort = StaticConstraint.CameraLivePort;
        string cameraControlPort = StaticConstraint.CameraControlPort;
        public Sws_CameraPlaybackController(ISws_AccessControlService sws_AccessControlService,
            ISysUserService sysUserService,
            ISws_CameraService sws_cameraServiceService)
        {
            _AccessControlService = sws_AccessControlService;
            _UserService = sysUserService;
            cameraService = sws_cameraServiceService;
        }
        #endregion
        public IActionResult Index()
        {
            ViewBag.treenodes = new HtmlString(GetStationTree(""));
            ViewBag.BeginTime = DateTime.Now.AddDays(-3).ToString("yyyy-MM-dd HH:mm:ss");
            ViewBag.EndTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            return View();
        }
        public IActionResult videoplay(int cameraId, string beginTime, string endTime)
        {
            SwsCamera ca = cameraService.Query<SwsCamera>(r => r.CameraId == cameraId).FirstOrDefault();
            if (ca != null)
            {
                ViewBag.IP = ca.Ip;
                ViewBag.AppKey = ca.AppKey;
                ViewBag.AppSecret = ca.AppSecret;
                ViewBag.Port = ca.Port;
                ViewBag.beginTime = beginTime;
                ViewBag.endTime = endTime;
                ViewBag.Num = ca.SerialNum;
            }
            else
            {
                ViewBag.beginTime = "";
            }
            return View();
        }
        #region 树查询
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult SearchTree(string name)
        {
            return Content(GetStationTree(name));
        }
        private string GetStationTree(string name)
        {
            int userID = int.Parse(User.FindFirstValue("UserID"));
            List<CameraTree> camreaTree = new List<CameraTree>();

            var user = _UserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
            var data = cameraService.GetCameraStationData(user.IsAdmin, userID, name, 1);
            if (data.Count() > 0)
            {
                var num = data.Select(r => r.StationID).ToList();
                camreaTree = data.Select(r => new CameraTree
                {
                    id = r.StationID + "泵房",
                    pId = "0",
                    name = "<em class='iconfont icon-bengfang'></em>" + r.StationName,
                    isDevice = false,
                    icon = ""
                }).ToList();

                var camrea = camreaTree.Distinct();
                var clist = data.Select(r => new CameraTree
                {
                    id = r.CameraID.ToString(),
                    pId = r.StationID + "泵房",
                    name = "<em class='iconfont icon-bengfang'></em>" + r.CameraName,
                    isDevice = true,
                    icon = ""
                });
                camreaTree = camreaTree.Concat<CameraTree>(clist).Distinct(new CameraTreeCompare()).ToList();
                return Newtonsoft.Json.JsonConvert.SerializeObject(camreaTree);
            }
            else
            {
                return "[]";
            }
        }
        #endregion

        #region
        [HttpPost]
        [TypeFilter(typeof(IgonreActionFilter))]
        public async Task<IActionResult> GetPlayBackTable(int cameraId, string beginTime, string endTime)
        {
            string dataTable = "";
            List<Cdata> cdata = new List<Cdata>();
            //时间出出力
            TimeSpan ts = Convert.ToDateTime(endTime) - Convert.ToDateTime(beginTime);

            string beginDate = Convert.ToDateTime(beginTime).ToString("yyyy-MM-ddThh:mm:ss.fff+08:00");
            string endDate = Convert.ToDateTime(endTime).ToString("yyyy-MM-ddThh:mm:ss.fff+08:00");
            var cr = cameraService.Query<SwsCamera>(r => r.CameraId == cameraId)?.FirstOrDefault();
            if (cr != null && ts.TotalDays <= 3)
            {
                //
                HttpUtillib.SetPlatformInfo(cr.AppKey, cr.AppSecret, cr.Ip, (int)cr.Port, true);

                //组装body
                string body = "{\"cameraIndexCode\": \"" + cr.SerialNum + "\",\"recordLocation\":1,\"protocol\": \"rtsp\",\"transmode\": 1, \"beginTime\": \"" + beginDate + "\",\"endTime\":\"" + endDate + "\"}";

                // 填充URL
                string uri = "/artemis/api/video/v2/cameras/playbackURLs";

                // step3：发起POST请求，超时时间15秒，返回响应字节数组
                byte[] result = HttpUtillib.HttpPost(uri, body, 15);

                if (result != null)
                {
                    string tmp = System.Text.Encoding.UTF8.GetString(result);
                    JObject obj = null;
                    try
                    {
                        obj = (JObject)JsonConvert.DeserializeObject(tmp);
                        string url = obj["data"]["url"].ToString();
                        string surl = url.Substring(0, url.IndexOf("begin"));
                        var list = obj["data"]["list"];
                        for (int i = 0; i < list.Count(); i++)
                        {
                            Cdata cd = new Cdata();
                            string btime = list[i]["beginTime"].ToString();
                            string etime = list[i]["endTime"].ToString();
                            string newurl = surl + "beginTime=" + Convert.ToDateTime(btime).ToString("yyyyMMddTHHmmss") + "&endTime=" + Convert.ToDateTime(etime).ToString("yyyyMMddTHHmmss") + "";
                            double size = Math.Round(((double)list[i]["size"]) / 1024 / 1024, 2);
                            cd.beginTime = btime;
                            cd.endTime = etime;
                            cd.url = newurl;
                            cd.size = size;
                            cdata.Add(cd);
                        }
                        PartialView("_cameraTable", cdata);
                        dataTable = await ViewToString.RenderPartialViewToString(this, "_cameraTable");
                    }
                    catch (Exception e)
                    {
                        dataTable = "";
                    }
                }
                else
                {
                    dataTable = "";
                }
            }
            else
            {
                if (ts.TotalDays > 3)
                {
                    dataTable = "error";
                }
                else
                {
                    dataTable = "";
                }
            }
            return Json(new
            {
                dataTable = dataTable
            });
        }
        #endregion
    }

    public class Cdata
    {
        public string url { get; set; }
        public string beginTime { get; set; }
        public string endTime { get; set; }
        public double size { get; set; }
    }

    public class CameraTree
    {
        public string id { get; set; }
        public string pId { get; set; }
        public string name { get; set; }
        public string icon { get; set; }
        //是否隐藏节点
        public bool isDevice { get; set; }

    }

    public class CameraTreeCompare : IEqualityComparer<CameraTree>
    {
        public bool Equals(CameraTree x, CameraTree y)
        {
            if (x == null)
                return y == null;
            return x.id == y.id;
        }

        public int GetHashCode(CameraTree obj)
        {
            if (obj == null)
                return 0;
            return obj.id.GetHashCode();
        }
    }
}
