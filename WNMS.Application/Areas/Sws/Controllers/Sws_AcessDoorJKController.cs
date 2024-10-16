using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using HttpUtil;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDBHelper;
using Newtonsoft.Json.Linq;
using WNMS.Application.Utility.Filters;
using WNMS.IService;
using WNMS.Model.CustomizedClass;
using WNMS.Model.DataModels;
using WNMS.Utility;

namespace WNMS.Application.Areas.Sws.Controllers
{
    [Area("Sws")]
    public class Sws_AcessDoorJKController : Controller
    {
        private ISws_AccessControlService _AccessControlService = null;
        private ISysUserService _UserService = null;
        private ISws_AccessHistoryService _AccessHistoryService = null;
        int timevalue = int.Parse(StaticConstraint.TimeValue);
        public Sws_AcessDoorJKController(ISws_AccessControlService sws_AccessControlService,
            ISysUserService sysUserService,
            ISws_AccessHistoryService sws_AccessHistoryService)
        {
            _AccessControlService = sws_AccessControlService;
            _UserService = sysUserService;
            _AccessHistoryService = sws_AccessHistoryService;
        }
        public IActionResult Index()
        {
            var data = GetStationList("");
            ViewBag.stationState = data;
            ViewBag.beginTime = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
            ViewBag.endTime = DateTime.Now.ToString("yyyy-MM-dd");
            return View();
        }
        #region 左侧泵房状态
        private IEnumerable<dynamic> GetStationList(string stationName)
        {
            int userID = int.Parse(User.FindFirstValue("UserID"));

            var user = _UserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
            var offtime = DateTime.Now.AddMinutes(-timevalue);//掉线时间
            string macJson = "{'DigitalValues.1001':true,'DigitalValues.1000':true,'UpdateTime':{'$gte':ISODate('" + offtime.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'") + "')}}";

            var collections = new MongoDBHelper<RtuJKInfo>(DateTime.Now.Year.ToString());
            var datalist = collections.Find("Sws_DeviceJKInfo", macJson, "UpdateTime").Select(r => r.RTUID).ToList();
            string onlinertuIds = datalist.Count > 0 ? string.Join(",", datalist) : "0";//在线的rtuid字符串

            var query = _AccessControlService.GetStationTreeOfState(onlinertuIds, user.IsAdmin, user.UserId, stationName == null ? "" : stationName);
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
        #region 数据查询
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult QueryAcessTable(int stationid, int pagesize, int pageindex, string SearchText, string order, string sort, string time, string beginDate, string endDate)
        {
            #region 查询条件

            string filter = " 1=1";
            if (!string.IsNullOrEmpty(time))
            {
                GetBeginDate(time, ref beginDate, ref endDate);
                if (!string.IsNullOrEmpty(beginDate))
                {
                    var datebign = Convert.ToDateTime(beginDate);
                    filter += " and PoliceTime>='" + datebign + "' ";

                }
                if (!string.IsNullOrEmpty(endDate))
                {
                    var dateEnd = Convert.ToDateTime(endDate);

                    filter += "  and PoliceTime<'" + dateEnd + "'";
                }
            }

            if (!string.IsNullOrWhiteSpace(SearchText))
            {

                filter += " and OperatingUser like '%" + SearchText + "%'";
            }

            string orderby = sort + " " + order;
            #endregion
            int Totalcount = 0;
            var dataList = _AccessControlService.GetAccessHistory(stationid, pageindex, pagesize, filter, orderby, ref Totalcount);
            return Json(new { total = Totalcount, rows = Newtonsoft.Json.JsonConvert.SerializeObject(dataList) });

        }

        public IActionResult QueryAcessTableByHK(int stationid, int pagesize, int pageindex, string SearchText, string order, string sort, string time, string beginDate, string endDate)
        {
            #region 查询条件 
            JosnInfo josnInfo = new JosnInfo();
            josnInfo.pageNo = pageindex;
            josnInfo.pageSize = pagesize;
            josnInfo.startTime = beginDate + "T00:00:00+08:00";
            josnInfo.endTime = endDate + "T23:59:59+08:00";
            josnInfo.receiveStartTime = beginDate + "T00:00:00+08:00";
            josnInfo.receiveEndTime = endDate + "T23:59:59+08:00";
            josnInfo.order = order;
            josnInfo.sort = sort;
            if (!string.IsNullOrEmpty(SearchText))
            {
                josnInfo.personName = SearchText;
            }
            #endregion 
            //查询泵房门禁信息
            var stationInfo = _AccessControlService.Query<SwsAccessControl>(r => r.StationId == stationid).FirstOrDefault();
            string para = Newtonsoft.Json.JsonConvert.SerializeObject(josnInfo);
            //HttpUtillib.SetPlatformInfo("24365906", "XlaLwOqHVWVE3CBa8GOC", "222.173.103.226", 10133, true);
            if (stationInfo != null && stationInfo.AppKey != null)
            {
                HttpUtillib.SetPlatformInfo(stationInfo?.AppKey, stationInfo?.Secret, stationInfo?.Ip, stationInfo.Port, true);
                string uri = "/artemis/api/acs/v2/door/events";
                var result = HttpUtillib.HttpPost(uri, para, 15);
                if (result != null)
                {
                    string tmp = System.Text.Encoding.UTF8.GetString(result);
                    JosnAllInfo josnAllInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<JosnAllInfo>(tmp);
                    //JosnOneInfo josnOneInfo  = ConvertObject<JosnOneInfo>(josnAllInfo.data);
                    var json = Newtonsoft.Json.JsonConvert.DeserializeObject<JosnOneInfo>(josnAllInfo.data.ToString());
                    return Json(new { total = json.total, rows = Newtonsoft.Json.JsonConvert.SerializeObject(json.list) });
                }
                else
                {
                    List<dynamic> list = new List<dynamic>();
                    return Json(new { total = 0, rows = Newtonsoft.Json.JsonConvert.SerializeObject(list) });
                }

            }
            else
            {
                List<dynamic> list = new List<dynamic>();
                return Json(new { total = 0, rows = Newtonsoft.Json.JsonConvert.SerializeObject(list) });
            }
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
        /// <summary>
        /// 将object对象转换为实体对象
        /// </summary>
        /// <typeparam name="T">实体对象类名</typeparam>
        /// <param name="asObject">object对象</param>
        /// <returns></returns>
        private T ConvertObject<T>(object asObject) where T : new()
        {
            //创建实体对象实例
            var t = Activator.CreateInstance<T>();
            if (asObject != null)
            {
                Type type = asObject.GetType();
                //遍历实体对象属性
                foreach (var info in typeof(T).GetProperties())
                {
                    object obj = null;
                    //取得object对象中此属性的值
                    var val = type.GetProperty(info.Name)?.GetValue(asObject);
                    if (val != null)
                    {
                        //非泛型
                        if (!info.PropertyType.IsGenericType)
                            obj = Convert.ChangeType(val, info.PropertyType);
                        else//泛型Nullable<>
                        {
                            Type genericTypeDefinition = info.PropertyType.GetGenericTypeDefinition();
                            if (genericTypeDefinition == typeof(Nullable<>))
                            {
                                obj = Convert.ChangeType(val, Nullable.GetUnderlyingType(info.PropertyType));
                            }
                            else
                            {
                                obj = Convert.ChangeType(val, info.PropertyType);
                            }
                        }
                        info.SetValue(t, obj, null);
                    }
                }
            }
            return t;
        }
        #endregion
    }
    class JosnInfo
    {
        public int pageNo;
        public int pageSize;
        public string startTime;
        public string endTime;
        public string receiveStartTime;
        public string receiveEndTime;
        public string personName;
        public string sort;
        public string order;
    }
    class JosnAllInfo
    {
        public string code;
        public string msg;
        public object data;
        public int total;
        public int totalPage;
        public int pageNo;
        public string personName;
        public string sort;
        public string order;
    }
    class JosnOneInfo
    {
        public int pageSize;
        public object list;
        public int total;
        public int totalPage;
        public int pageNo;
    }
    class JosnOneOneInfo
    {
        public string eventId;
        public string eventName;
        public string eventTime;
        public string personId;
        public string cardNo;
        public string personName;
        public string orgIndexCode;
        public string orgName;
        public string doorName;
        public string doorIndexCode;
        public string doorRegionIndexCode;
        public string picUri;
        public string svrIndexCode;
        public string eventType;
        public string inAndOutType;
        public string readerDevIndexCode;
        public string readerDevName;
        public string devIndexCode;
        public string devName;
        public string identityCardUri;
        public string receiveTime;
        public string jobNo;
        public string studentId;
        public string certNo;
    }
}