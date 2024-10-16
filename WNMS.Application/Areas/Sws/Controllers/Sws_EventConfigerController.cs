using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HttpUtil;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WNMS.Application.Utility.Filters;

namespace WNMS.Application.Areas.Sws.Controllers
{
    [TypeFilter(typeof(IgonreLoginFilter))]
    [TypeFilter(typeof(IgonreActionFilter))]
    [Area("Sws")]
    public class Sws_EventConfigerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        //查询
        [HttpPost]
        public string QueryEvent()
        {
            try
            {
                HttpUtillib.SetPlatformInfo("24365906", "XlaLwOqHVWVE3CBa8GOC", "222.173.103.226", 10133, true);
                string uri = "/artemis/api/eventService/v1/eventSubscriptionView";
                byte[] result = HttpUtillib.HttpPost(uri, "", 15);
                string tmp = System.Text.Encoding.UTF8.GetString(result);
                var obj = (JObject)JsonConvert.DeserializeObject(tmp);
                return tmp;
            }
            catch (Exception e)
            {
                return "";
            }
        }
        //取消
        [HttpPost]
        public IActionResult CancelEvent(string eventtype)
        {
            HttpUtillib.SetPlatformInfo("24365906", "XlaLwOqHVWVE3CBa8GOC", "222.173.103.226", 10133, true);
            string uri = "/artemis/api/eventService/v1/eventUnSubscriptionByEventTypes";
            //var para = "{\"eventTypes\": [131592,131331,131329,131612,131613,131585,131586,131330]}";
            var para = "{\"eventTypes\": ["+ eventtype + "]}";
            byte[] result = HttpUtillib.HttpPost(uri, para, 15);
            string tmp = System.Text.Encoding.UTF8.GetString(result);
            var obj = (JObject)JsonConvert.DeserializeObject(tmp);
            return Content("ok");
        }
        //事件订阅
        [HttpPost]
        public IActionResult SetEvent(string eventtype,int subType,string eventLvl,string ipport)
        {
            HttpUtillib.SetPlatformInfo("24365906", "XlaLwOqHVWVE3CBa8GOC", "222.173.103.226", 10133, true);
            string uri = "/artemis/api/eventService/v1/eventSubscriptionByEventTypes";
            var para = "{\"eventTypes\": ["+ eventtype + "], \"eventDest\": \"https://"+ ipport + "/api/WMNS_API/DoorEventRcv\",\"subType\": " + subType + ",\"eventLvl\": ["+ eventLvl + "]}";
            byte[] result = HttpUtillib.HttpPost(uri, para, 15);
            string tmp = System.Text.Encoding.UTF8.GetString(result);
            var obj = (JObject)JsonConvert.DeserializeObject(tmp);
            return Content("ok");
        }
        //查询预置点
        [HttpPost]
        public IActionResult QueryDot(string cameraIndexCode)
        {
            HttpUtillib.SetPlatformInfo("24365906", "XlaLwOqHVWVE3CBa8GOC", "222.173.103.226", 10133, true);
            string uri = "/artemis/api/video/v1/presets/searches";
            var para = "{\"cameraIndexCode\":\""+ cameraIndexCode + "\"}";
            byte[] result = HttpUtillib.HttpPost(uri, para, 15);
            string tmp = System.Text.Encoding.UTF8.GetString(result);
            return Content(tmp);

        }
        //操作云台转到预置点
        [HttpPost]
        public IActionResult ToThisDot(string cameraIndexCode,int presetIndex)
        {
            HttpUtillib.SetPlatformInfo("24365906", "XlaLwOqHVWVE3CBa8GOC", "222.173.103.226", 10133, true);
            string uri = "/artemis/api/video/v1/ptzs/controlling";
            var para = "{\"cameraIndexCode\":\"" + cameraIndexCode + "\",\"action\":0,\"command\":\"GOTO_PRESET\",\"speed\":50,\"presetIndex\":"+ presetIndex + "}";
            byte[] result = HttpUtillib.HttpPost(uri, para, 15);
            string tmp = System.Text.Encoding.UTF8.GetString(result);
            return Content(tmp);
        }
        //门禁操作
        [HttpPost]
        public IActionResult OperateDoor(string doorIndexCodes,int controlType)
        {
            HttpUtillib.SetPlatformInfo("24365906", "XlaLwOqHVWVE3CBa8GOC", "222.173.103.226", 10133, true);
            string uri = "/artemis/api/acs/v1/door/doControl";
            var para = "{\"doorIndexCodes\":[\"" + doorIndexCodes + "\"],\"controlType\":"+ controlType + "}";
            byte[] result = HttpUtillib.HttpPost(uri, para, 15);
            string tmp = System.Text.Encoding.UTF8.GetString(result);
            return Content(tmp);
        }
        //获取门禁
        [HttpPost]
        public IActionResult QueryDoorList()
        {
            HttpUtillib.SetPlatformInfo("24365906", "XlaLwOqHVWVE3CBa8GOC", "222.173.103.226", 10133, true);
            string uri = "/artemis/api/resource/v2/door/search";
            var para = "{\"name\": \"\",\"regionIndexCodes\": [],\"isSubRegion\": true,\"pageNo\": 1,\"pageSize\": 100,\"authCodes\": [\"view\"]," +
   "\"expressions\": [],\"orderBy\": \"name\",\"orderType\": \"desc\"}";
            byte[] result = HttpUtillib.HttpPost(uri, para, 15);
            string tmp = System.Text.Encoding.UTF8.GetString(result);
            return Content(tmp);
        }
    }
}