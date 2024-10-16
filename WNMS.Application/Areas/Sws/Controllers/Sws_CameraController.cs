using Command;
using Microsoft.AspNetCore.Mvc;
using MongoDBHelper;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WNMS.Application.Utility.Filters;
using WNMS.IService;
using WNMS.Model.CustomizedClass;
using WNMS.Model.DataModels;
using WNMS.Utility;

namespace WNMS.Application.Areas.Sws.Controllers
{
    [Area("Sws")]
    public class Sws_CameraController : Controller
    {
        #region 属性 构造函数
        private ISws_AccessControlService _AccessControlService = null;
        private ISysUserService _UserService = null;
        private ISws_CameraService cameraService = null;
        int timevalue = int.Parse(StaticConstraint.TimeValue);
        string cameraIP = StaticConstraint.CameraService;
        string cameraLivePort = StaticConstraint.CameraLivePort;
        string cameraControlPort = StaticConstraint.CameraControlPort;
        public Sws_CameraController(ISws_AccessControlService sws_AccessControlService,
            ISysUserService sysUserService,
            ISws_CameraService sws_cameraServiceService)
        {
            _AccessControlService = sws_AccessControlService;
            _UserService = sysUserService;
            cameraService = sws_cameraServiceService;
        }
        #endregion

        #region 页面加载
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult Index()
        {
            var data = GetStationList("");
            ViewBag.stationState = data;
            ViewBag.endTime = DateTime.Now.ToString("yyyy-MM-dd");
            ViewBag.beginTime = DateTime.Now.AddDays(-3).ToString("yyyy-MM-dd");
            return View();
        }
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult Preview(int cameraId)
        {
            SwsCamera ca = cameraService.Query<SwsCamera>(r => r.CameraId == cameraId).FirstOrDefault();
            if (ca != null)
            {
                ViewBag.IP = ca.Ip;
                ViewBag.AppKey = ca.AppKey;
                ViewBag.AppSecret = ca.AppSecret;
                ViewBag.Port = ca.Port;
                ViewBag.Num = ca.SerialNum;
                ViewBag.url = ca.Url;


            }
            else
            {
                ViewBag.Num = "";
            }
            return View();
        }
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult PlayBack(int cameraId, string beginTime, string endTime)
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
                ViewBag.cameraIp = cameraIP + ":" + cameraLivePort;
                //cameraIp = camera.Ip +":"+ camera.Port,
                ViewBag.type = ca.CameraType;
            }
            else
            {
                ViewBag.Num = "";
            }
            return View();
        }
        #endregion

        #region 左侧泵房状态
        [TypeFilter(typeof(IgonreActionFilter))]
        private IEnumerable<dynamic> GetStationList(string stationName)
        {
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = _UserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
            var offtime = DateTime.Now.AddMinutes(-timevalue);//掉线时间
            string macJson = "{'DigitalValues.1001':true,'DigitalValues.1000':true,'UpdateTime':{'$gte':ISODate('" + offtime.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'") + "')}}";

            var collections = new MongoDBHelper<RtuJKInfo>(DateTime.Now.Year.ToString());
            var datalist = collections.Find("Sws_DeviceJKInfo", macJson, "UpdateTime").Select(r => r.RTUID).ToList();
            string onlinertuIds = datalist.Count > 0 ? string.Join(",", datalist) : "";//在线的rtuid字符串
            IEnumerable<dynamic> query = null;
            //if (datalist.Count > 0)
            //{
            query = _AccessControlService.GetStationTreeOfState(onlinertuIds, user.IsAdmin, user.UserId, stationName == null ? "" : stationName);
            //}
            return query;
        }

        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult SearchStationList(string stationName)
        {
            var query = GetStationList(stationName);
            return Json(new
            {
                data = query
            });
        }
        #endregion

        #region 摄像头列表获取
        /// <summary>
        /// 点击泵房后，获取泵房下的摄像头列表
        /// </summary>
        /// <param name="stationID"></param>
        /// <returns></returns>
        [TypeFilter(typeof(IgonreActionFilter))]
        public ActionResult GetCameraList(int stationID)
        {
            List<SwsCamera> cameraList = cameraService.Query<SwsCamera>(r => r.StationId == stationID).ToList();
            return Json(new
            {
                list = cameraList
            });
        }

        /// <summary>
        /// 获取摄像头信息，返回获取视频流url
        /// </summary>
        /// <param name="cameraID">摄像头ID</param>
        /// <returns></returns>
        [TypeFilter(typeof(IgonreActionFilter))]
        public ActionResult GetCameraDetail(int cameraID)
        {
            SwsCamera camera = cameraService.Find<SwsCamera>(cameraID);
            string url = "";
            if (camera != null)
            {
                //if (camera.CameraType == 1)
                //{
                //    url= "rtsp://"+camera.UserName+":"+WNMS.Encrypt.Decode(camera.PassWord)+"@"+camera.Ip+":554/cam/realmonitor?channel="+camera.ChannelNum+"&subtype=1";
                //}
                //if (camera.CameraType == 2)
                //{
                //    url= "rtsp://" + camera.UserName + ":" + WNMS.Encrypt.Decode(camera.PassWord) + "@" + camera.Ip + ":554/h264/ch"+camera.ChannelNum+"/sub/av_stream";
                //}
                //if (camera.CameraType == 3)
                //{
                //    url = camera.AppSecret;
                //}
                url = camera.Url;
            }
            return Json(new
            {
                url = url,
                cameraIp = cameraIP + ":" + cameraLivePort,
                //cameraIp = camera.Ip +":"+ camera.Port,
                type = camera.CameraType
            });
        }
        #endregion

        #region 单个泵房视频详情页
        //加载页面
        [TypeFilter(typeof(IgonreActionFilter))]
        public ActionResult detailCamera(int id)
        {
            List<SwsCamera> cameraList = cameraService.Query<SwsCamera>(r => r.StationId == id).ToList();
            ViewBag.CameraList = cameraList;
            return View();
        }
        #endregion

        #region 视频控制
        //0开始 1停止
        //0-上，1-下，2-左，3-右，4-左上，5-左下，6-右上，7-右下，8-放大，9-缩小，10-停止
        public IActionResult CameraMoveControl(int direction, int start, int cameraID, string step)
        {
            HttpHelper hp = new HttpHelper();
            string result = "";
            SwsCamera cam = this.cameraService.Find<SwsCamera>(cameraID);
            if (cam != null)
            {
                //if (cam.CameraType == 1 || cam.CameraType == 0)    //大华海康
                //{
                result = HKMoveControl(direction, start, step, cam);
                //}
                //else
                //{
                //    if (cam.CameraType == 2)       //萤石
                //    {
                //        result = YSCameraControl(direction, start, cameraID,step, cam);
                //    }
                //    else
                //    {
                //        if (cam.CameraType == 3)    //乐橙
                //        {
                //            if (direction < 10)
                //            {
                //                result = LCCameraControl(direction, step, cam);
                //            }
                //        }
                //        else
                //        {
                //            result = "false";
                //        }
                //    }
                //}
            }
            else
            {
                result = "noid";
            }

            return Content(result);
        }

        //海康大华视频控制   8-放大，9-缩小，11-焦距放大，12-焦距缩小，13-光圈放大，14-光圈缩小，
        public string HKMoveControl(int direction, int start, string step, SwsCamera cam)
        {
            HttpHelper hp = new HttpHelper();
            string result = "ok";
            string method = "";
            switch (direction)
            {
                case 0:
                    if (start == 0) { method = "UpBegin"; } else { method = "UpEnd"; }
                    break;
                case 1:
                    if (start == 0) { method = "DownBegin"; } else { method = "DownEnd"; }
                    break;
                case 2:
                    if (start == 0) { method = "LeftBegin"; } else { method = "LeftEnd"; }
                    break;
                case 3:
                    if (start == 0) { method = "RightBegin"; } else { method = "RightEnd"; }
                    break;
                case 4:
                    if (start == 0) { method = "LeftUpBegin"; } else { method = "LeftUpEnd"; }
                    break;
                case 5:
                    if (start == 0) { method = "LeftDownBegin"; } else { method = "LeftDownEnd"; }
                    break;
                case 6:
                    if (start == 0) { method = "RightUpBegin"; } else { method = "RightUpEnd"; }
                    break;
                case 7:
                    if (start == 0) { method = "RightDownBegin"; } else { method = "RightDownEnd"; }
                    break;
                case 8:
                    if (start == 0) { method = "ZOOM_ADDBegin"; } else { method = "ZOOM_ADDEnd"; }
                    break;
                case 9:
                    if (start == 0) { method = "ZOOM_DECBegin"; } else { method = "ZOOM_DECEnd"; }
                    break;
                case 11:
                    if (start == 0) { method = "FOCUS_ADDBegin"; } else { method = "FOCUS_ADDEnd"; }
                    break;
                case 12:
                    if (start == 0) { method = "FOCUS_DECBegin"; } else { method = "FOCUS_DECEnd"; }
                    break;
                case 13:
                    if (start == 0) { method = "APERTURE_ADDBegin"; } else { method = "APERTURE_ADDEnd"; }
                    break;
                case 14:
                    if (start == 0) { method = "APERTURE_DECBegin"; } else { method = "APERTURE_DECEnd"; }
                    break;
                default:
                    if (start == 0) { method = "UpBegin"; } else { method = "UpEnd"; }
                    break;
            }
            string aa = "操作失败";
            string bb = "";
            string dd = "";
            try
            {
                bb = cameraIP + ":" + cameraControlPort + "/api/PTZ/GetInstance?cameraBrand=" + cam.CameraType + "&ip=" + cam.Ip + "&port=" + cam.Port + "&userName=" + cam.UserName + "&passWord=" + WNMS.Encrypt.Decode(cam.PassWord) + "";

                var login = hp.HttpGet(cameraIP + ":" + cameraControlPort + "/api/PTZ/GetInstance?cameraBrand=" + cam.CameraType + "&ip=" + cam.Ip + "&port=" + cam.Port + "&userName=" + cam.UserName + "&passWord=" + WNMS.Encrypt.Decode(cam.PassWord) + "", "");
                aa = "登录成功";
                JObject json = (JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(login);
                var key = json["key"].ToString();
                if (json["code"].ToString() != "0")
                {
                    result = json["msg"].ToString();
                }
                dd = cameraIP + ":" + cameraControlPort + "/api/PTZ/" + method + "?key=" + key + "&channelNum=" + cam.ChannelNum + "&step=" + step + "";
                var control = hp.HttpGet(cameraIP + ":" + cameraControlPort + "/api/PTZ/" + method + "?key=" + key + "&channelNum=" + cam.ChannelNum + "&step=" + step + "", "");
                JObject jsonobj = (JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(control);
                if (jsonobj["code"].ToString() != "0")
                {
                    result = json["msg"].ToString();
                }
            }
            catch (Exception e)
            {
                result = e.Message + aa + bb + "--" + dd;
            }

            return result;
        }

        //萤石视频控制
        public string YSCameraControl(int direction, int start, int cameraID, string step, SwsCamera cam)
        {
            HttpHelper hp = new HttpHelper();
            string result = "ok";
            string method = "start";
            string parm = "speed=1";
            if (start == 1)
            {
                method = "stop";
                parm = "";
            }
            try
            {
                var login = hp.HttpPost("https://open.ys7.com/api/lapp/token/get?appKey=" + cam.AppKey.Trim() + "&appSecret=" + cam.AppSecret.Trim() + "", "");
                JObject json = (JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(login);
                var token = json["data"]["accessToken"].ToString();
                if (json["code"].ToString() != "200")
                {
                    result = json["msg"].ToString(); ;
                }
                var control = hp.HttpPost("https://open.ys7.com/api/lapp/device/ptz/" + method + "?accessToken=" + token + "&deviceSerial=" + cam.SerialNum.Trim() + "&channelNo=" + cam.ChannelNum + "&direction=" + direction + "&" + parm + "", "");
                JObject jsonobj = (JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(control);
                if (jsonobj["code"].ToString() != "200")
                {
                    result = jsonobj["msg"].ToString();
                }
            }
            catch (Exception e)
            {
                result = "no";
            }
            return result;
        }

        //乐橙视频控制
        public string LCCameraControl(int direction, string step, SwsCamera cam)
        {
            string result = "ok";
            int stepint = Convert.ToInt32(step);
            try
            {
                string str = "params\": {}";
                var login = LCApi("accessToken", str, cam.AppKey, cam.AppSecret, cam.CameraId.ToString());
                JObject json = (JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(login);
                string token = json["result"]["data"]["accessToken"].ToString();
                if (json["result"]["code"].ToString() != "0")
                {
                    result = "false";
                }

                string parstr = "params\": {\"token\":\"" + token + "\",\"deviceId\":\"" + cam.SerialNum + "\",\"channelId\":\"" + cam.ChannelNum + "\",\"operation\":" + direction + ",\"duration\":\"" + stepint * 1000 / 2 + "\"}";
                var control = LCApi("controlMovePTZ", parstr, cam.AppKey, cam.AppSecret, cam.CameraId.ToString());
                JObject jsonobj = (JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(control);
                if (jsonobj["result"]["code"].ToString() != "0")
                {
                    result = "no";
                }
            }
            catch (Exception e)
            {
                result = "no";
            }
            return result;
        }

        //乐橙接口视频控制 controlMovePTZ
        public string LCApi(string method, string parstr, string appIds, string appSecrets, string id)
        {
            string url = "https://openapi.lechange.cn:443/openapi/" + method + "";
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "POST";
            request.ContentType = "application/json";


            //当前的UTC时间戳
            DateTime dateTime = DateTime.Now;
            long time = Convert.ToInt64((dateTime - TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1))).TotalSeconds);

            //设置随机数
            string nonce = Guid.NewGuid().ToString();

            //授权信息之appid（注：如果还未获取appid，请登录open.lechange.com，开发者服务模块中创建应用后获取）
            //string appId = "lcf16bb7c6271e40e7";
            string appId = appIds.Trim();

            //授权信息之appSecret（注：如果还未获取appSecret，请登录open.lechange.com，开发者服务模块中创建应用后获取）
            //string appSecret = "3310b5f52b2b45be8bae488011e54b";
            string appSecret = appSecrets.Trim();


            //拼接计算“签名原始串”
            string signStr = "time:" + time + ",nonce:" + nonce + ",appSecret:" + appSecret;

            //计算摘要 sign
            MD5 md5 = MD5.Create();

            string sign = MD5Encrypt32(signStr).ToLower();

            StringBuilder data = new StringBuilder();
            data.Append("{\"system\": {");
            data.Append("\"ver\": \"1.0\",");
            data.AppendFormat("\"sign\": \"{0}\",", sign);
            data.AppendFormat("\"appId\": \"{0}\",", appId);
            data.AppendFormat("\"time\": \"{0}\",", time);
            data.AppendFormat(" \"nonce\": \"{0}\"", nonce);
            data.Append("},\"" + parstr.Trim() + ",\"id\": \"" + id + "\"}");


            byte[] byteData = UTF8Encoding.UTF8.GetBytes(data.ToString());
            request.ContentLength = byteData.Length;

            using (Stream postStream = request.GetRequestStream())
            {
                postStream.Write(byteData, 0, byteData.Length);
            }

            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());
                return reader.ReadToEnd();
            }
        }

        private static string MD5Encrypt32(string password)
        {

            string cl = password;
            string pwd = "";
            MD5 md5 = MD5.Create(); //实例化一个md5对像
            // 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择　
            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(cl));
            // 通过使用循环，将字节类型的数组转换为字符串，此字符串是常规字符格式化所得
            for (int i = 0; i < s.Length; i++)
            {
                // 将得到的字符串使用十六进制类型格式。格式后的字符是小写的字母，如果使用大写（X）则格式后的字符是大写字符 
                pwd = pwd + s[i].ToString("X2");
            }
            return pwd;

        }
        #endregion

        #region 大华DSS
        public ActionResult DSSCamera()
        {
            var data = GetStationList("");
            ViewBag.stationState = data;
            return View();
        }
        /// <summary>
        /// 点击泵房后，获取泵房下的摄像头列表
        /// </summary>
        /// <param name="stationID"></param>
        /// <returns></returns>
        [TypeFilter(typeof(IgonreActionFilter))]
        public ActionResult GetDSSCameraList(int stationID)
        {
            List<SwsCamera> cameraList = cameraService.Query<SwsCamera>(r => r.StationId == stationID).ToList();
            return Json(new
            {
                list = cameraList
            });
        }
        /// <summary>
        /// 获取摄像头信息，返回获取视频流url
        /// </summary>
        /// <param name="cameraID">摄像头ID</param>
        /// <returns></returns>
        [TypeFilter(typeof(IgonreActionFilter))]
        public ActionResult GetDSSCameraDetail(int cameraID)
        {
            SwsCamera camera = cameraService.Find<SwsCamera>(cameraID);
            string url = "";
            if (camera != null)
            {
                url = camera.Numbering + "$1$0$" + camera.ChannelNum;
            }
            return Json(new
            {
                url = url,
                cameraIp = cameraIP + ":" + cameraLivePort,
                type = camera.CameraType
            });
        }
        #endregion
    }
}