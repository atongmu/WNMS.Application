using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDBHelper;
using WNMS.Application.Utility.Filters;
using WNMS.IService;
using WNMS.Model.CustomizedClass;
using WNMS.Model.DataModels;
namespace WNMS.Application.Areas.Sws.Controllers
{
    [Area("Sws")]
    [TypeFilter(typeof(IgonreLoginFilter))]
    [TypeFilter(typeof(IgonreActionFilter))]
    public class Sws_RealLineMobileController : Controller
    {
        private ISws_DeviceInfo01Service _DeviceInfo01Service = null;
        private ISws_ConsumpSettingService _ConsumpSettingService = null;
        private ISysUserService userService = null;
        public Sws_RealLineMobileController(ISws_DeviceInfo01Service sws_DeviceInfo01Service,
            ISws_ConsumpSettingService sws_ConsumpSettingService,
            ISysUserService sysUserService) {
            _DeviceInfo01Service = sws_DeviceInfo01Service;
            _ConsumpSettingService = sws_ConsumpSettingService;
            userService = sysUserService;
        }
        public IActionResult Index(long id,string token)
        {
            var model = _DeviceInfo01Service.Query<SwsDeviceInfo01>(r => r.DeviceId == id).FirstOrDefault();
            var partition = 1;
            var rtuid = 0;
            if (model != null)
            {
                partition=model.Partition;
                rtuid = (int)model.Rtuid;
            }
            ViewBag.partition = partition;
            ViewBag.rtuid = rtuid;
            ViewBag.TimeRange = DateTime.Now.Date.AddDays(-6).ToString("yyyy-MM-dd HH:mm") + " - " + DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            var hasOwn = false;
            var users = userService.Query<SysUser>(r => r.Token == token).FirstOrDefault();
            if (users != null)
            {
                hasOwn = true;
            }
            ViewBag.hasOwn = hasOwn;
            return View();
        }
        #region 实时曲线
        public IActionResult RealHistoryData(string rtuid, int partition)
        {
            var endate = DateTime.Now;
            var begindate = endate.AddHours(-2);
            var pressIN = 2000 + (partition - 1) * 500 + "";
            var pressOut= 2001 + (partition - 1) * 500 + "";
            var pressSet= 2002 + (partition - 1) * 500 + "";
            string resultIN = "", resultOut = "", resultSet = "";
            string project = @"{'$project':{
                'UpdateTime':1,
                  'pIN':{$ifNull:['$AnalogValues." + pressIN + "',0]}," +
                 @"'pOut':{$ifNull:['$AnalogValues." + pressOut + "',0]},'pSet':{$ifNull:['$AnalogValues." + pressSet + "',0]}}}";
            var datalist = _ConsumpSettingService.HistoryChartData(begindate, endate, project, rtuid);
            if (begindate.Year != endate.Year)//测试
            {
                var begindate_append = Convert.ToDateTime(endate.Year + "-01-01");
                var appenddata = _ConsumpSettingService.HistoryChartData(begindate_append, endate, project, rtuid);
                if (appenddata.Count() > 0)
                {
                    datalist = datalist.Union(appenddata);
                }
            }
            if (datalist.Count() > 0)
            {
                StringBuilder dataIn = new StringBuilder();
                StringBuilder dataOut = new StringBuilder();
                StringBuilder dataSet = new StringBuilder();
                foreach (var item in datalist)
                {
                    //进水压力
                    var p_in = item.pIN;
                    string str_in = "{name:'" + item.UpdateTime.ToString("yyyy-MM-dd HH:mm:ss") + "',value:['" + item.UpdateTime.ToString("yyyy-MM-dd HH:mm:ss") + "'," + Math.Round((double)p_in, 2) + "]},";
                    dataIn.Append(str_in);
                    //出水压力
                    var p_out = item.pOut;
                    string str_out = "{name:'" + item.UpdateTime.ToString("yyyy-MM-dd HH:mm:ss") + "',value:['" + item.UpdateTime.ToString("yyyy-MM-dd HH:mm:ss") + "'," + Math.Round((double)p_out, 2) + "]},";
                    dataOut.Append(str_out);
                    //设定压力
                    var p_set = item.pSet;
                    string str_set = "{name:'" + item.UpdateTime.ToString("yyyy-MM-dd HH:mm:ss") + "',value:['" + item.UpdateTime.ToString("yyyy-MM-dd HH:mm:ss") + "'," + Math.Round((double)p_set, 2) + "]},";
                    dataSet.Append(str_set);

                }
                if (dataIn.ToString() != "")
                {

                    resultIN = "[" + dataIn.ToString().Substring(0, dataIn.ToString().Length - 1) + "]";
                    resultOut = "[" + dataOut.ToString().Substring(0, dataOut.ToString().Length - 1) + "]";
                    resultSet = "[" + dataSet.ToString().Substring(0, dataSet.ToString().Length - 1) + "]";
                }
            }
            return Json(new
            {
                resultIN,
                resultOut,
                resultSet
            });
        }
        public IActionResult UpdateRealData(string rtuid, int partition)
        {
            string macJson = "{\"RTUID\":{'$in':[" + rtuid + "]}}";
            var collections = new MongoDBHelper<RtuJKInfo>(DateTime.Now.Year.ToString());
            var jklist = collections.Find("Sws_DeviceJKInfo", macJson, "UpdateTime").FirstOrDefault();
            var pressIN = 2000 + (partition - 1) * 500 + "";
            var pressOut = 2001 + (partition - 1) * 500 + "";
            var pressSet = 2002 + (partition - 1) * 500 + "";
            string resultIN = "", resultOut = "", resultSet = "";
            if (jklist != null)
            {
                //进水压力
                double p_in = jklist.AnalogValues.ContainsKey(pressIN) ? (double)jklist.AnalogValues[pressIN] : 0;
                resultIN = "[{name:'" + jklist.UpdateTime.ToString("yyyy-MM-dd HH:mm:ss") + "',value:['" + jklist.UpdateTime.ToString("yyyy-MM-dd HH:mm:ss") + "'," + Math.Round(p_in, 2) + "]}]";
                
                //出水压力
                double p_out = jklist.AnalogValues.ContainsKey(pressOut) ? (double)jklist.AnalogValues[pressOut] : 0;
                resultOut = "[{name:'" + jklist.UpdateTime.ToString("yyyy-MM-dd HH:mm:ss") + "',value:['" + jklist.UpdateTime.ToString("yyyy-MM-dd HH:mm:ss") + "'," + Math.Round(p_out, 2) + "]}]";
                
                //设定压力
                double p_set = jklist.AnalogValues.ContainsKey(pressSet) ? (double)jklist.AnalogValues[pressSet] : 0;
                resultSet = "[{name:'" + jklist.UpdateTime.ToString("yyyy-MM-dd HH:mm:ss") + "',value:['" + jklist.UpdateTime.ToString("yyyy-MM-dd HH:mm:ss") + "'," + Math.Round(p_set, 2) + "]}]";
                
            }
            return Json(new
            {
                resultIN,
                resultOut,
                resultSet
            });

        }
        #endregion
        #region 历史曲线
        public IActionResult HistoryLine(string rtuid, int partition,DateTime begindate,DateTime enddate)
        {
            var pressIN = 2000 + (partition - 1) * 500 + "";
            var pressOut = 2001 + (partition - 1) * 500 + "";
            var pressSet = 2002 + (partition - 1) * 500 + "";
            string resultIN = "", resultOut = "", resultSet = "";
            var dayNum = (enddate - begindate).TotalDays;
            string type = "1";
            string group = "";
            string project = "";
            string sort = "{'$sort': {'UpdateTime': 1}}";
            if (dayNum <= 7)
            {
                type = "1";
                group = @"{'$group':{
        '_id':{$subtract:['$UpdateTime', new Date('1970-01-01')]},
        'pin':{$first: '$AnalogValues."+ pressIN + "'},'pout':{$first: '$AnalogValues."+ pressOut + "'},'pset':{$first: '$AnalogValues."+ pressSet + "'}}}";
                project = @"{'$project':{
                '_id': 0,
                  'pin':{$ifNull:['$pin',0]},
                   'pout':{$ifNull:['$pout',0]},
                   'pset':{$ifNull:['$pset',0]},
                   'UpdateTime':{$add:[new Date(-28800000),'$_id']}    
               }}";
            }
            else if (dayNum <= 30)
            {
                type = "2";
                group = @"{'$group':{
        '_id':{$subtract:[{$subtract:['$UpdateTime',new Date('1970-01-01')]},{$mod:[{$subtract:['$UpdateTime',new Date('1970-01-01')]},3600000]}]},
        'pin':{$first: '$AnalogValues." + pressIN + "'},'pout':{$first: '$AnalogValues." + pressOut + "'},'pset':{$first: '$AnalogValues." + pressSet + "'}}}";
                project = @"{'$project':{
                '_id': 0,
                  'pin':{$ifNull:['$pin',0]},
                   'pout':{$ifNull:['$pout',0]},
                   'pset':{$ifNull:['$pset',0]},
                   'UpdateTime':{$add:[new Date(-28800000),'$_id']}    
               }}";
            }
            else
            {
                type = "3";
                group = @"{'$group':{
        '_id':{'$dateToString':{'format':'%Y-%m-%d','date':'$UpdateTime'}},
        'pin':{$first: '$AnalogValues." + pressIN + "'},'pout':{$first: '$AnalogValues." + pressOut + "'},'pset':{$first: '$AnalogValues." + pressSet + "'}}}";
                project = @"{'$project':{
                '_id': 0,
                  'pin':{$ifNull:['$pin',0]},
                   'pout':{$ifNull:['$pout',0]},
                   'pset':{$ifNull:['$pset',0]},
                   'UpdateTime':'$_id'    
               }}";
            }
            var datalist = _ConsumpSettingService.HistroyChartData(project,sort,group, rtuid, begindate,enddate);
            if (begindate.Year != enddate.Year)//测试
            {
                var begindate_append = Convert.ToDateTime(enddate.Year + "-01-01");
                var appenddata = _ConsumpSettingService.HistroyChartData(project, sort, group, rtuid, begindate_append, enddate);
                if (appenddata.Count() > 0)
                {
                    datalist = datalist.Union(appenddata);
                }
            }
            if (datalist.Count() > 0)
            {
                StringBuilder dataIn = new StringBuilder();
                StringBuilder dataOut = new StringBuilder();
                StringBuilder dataSet = new StringBuilder();
                foreach (var item in datalist)
                {
                    //进水压力
                    var p_in = item.pin;
                    string str_in = "";
                    if (type == "3")
                    {
                         str_in = "{name:'" + item.UpdateTime + "',value:['" + item.UpdateTime + "'," + Math.Round((double)p_in, 2) + "]},";
                    }
                    else
                    {
                         str_in = "{name:'" + item.UpdateTime.ToString("yyyy-MM-dd HH:mm:ss") + "',value:['" + item.UpdateTime.ToString("yyyy-MM-dd HH:mm:ss") + "'," + Math.Round((double)p_in, 2) + "]},";
                    }
                    dataIn.Append(str_in);
                    //出水压力
                    var p_out = item.pout;
                    string str_out = "";
                    if (type == "3")
                    {
                         str_out = "{name:'" + item.UpdateTime + "',value:['" + item.UpdateTime + "'," + Math.Round((double)p_out, 2) + "]},";
                    }
                    else
                    {
                         str_out = "{name:'" + item.UpdateTime.ToString("yyyy-MM-dd HH:mm:ss") + "',value:['" + item.UpdateTime.ToString("yyyy-MM-dd HH:mm:ss") + "'," + Math.Round((double)p_out, 2) + "]},";
                    }
                    dataOut.Append(str_out);
                    //设定压力
                    var p_set = item.pset;
                    string str_set = "";
                    if (type == "3")
                    {
                         str_set = "{name:'" + item.UpdateTime + "',value:['" + item.UpdateTime + "'," + Math.Round((double)p_set, 2) + "]},";
                    }
                    else
                    {
                         str_set = "{name:'" + item.UpdateTime.ToString("yyyy-MM-dd HH:mm:ss") + "',value:['" + item.UpdateTime.ToString("yyyy-MM-dd HH:mm:ss") + "'," + Math.Round((double)p_set, 2) + "]},";
                    }
                    dataSet.Append(str_set);

                }
                if (dataIn.ToString() != "")
                {

                    resultIN = "[" + dataIn.ToString().Substring(0, dataIn.ToString().Length - 1) + "]";
                    resultOut = "[" + dataOut.ToString().Substring(0, dataOut.ToString().Length - 1) + "]";
                    resultSet = "[" + dataSet.ToString().Substring(0, dataSet.ToString().Length - 1) + "]";
                }
            }
            return Json(new
            {
                resultIN,
                resultOut,
                resultSet,
                type
            });
        }
        #endregion
    }
}