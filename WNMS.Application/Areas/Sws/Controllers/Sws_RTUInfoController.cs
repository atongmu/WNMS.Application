using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DPC.CmdType;
using DPC.Config.Resource;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WNMS.Application.Utility.Filters;
using WNMS.IService;
using WNMS.Model.CustomizedClass;
using WNMS.Model.DataModels;
using Command;
using WNMS.Utility;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace WNMS.Application.Areas.Sws.Controllers
{
    [Area("Sws")]
    public class Sws_RTUInfoController : Controller
    {
        private ISws_RTUInfoService _RTUInfoService = null;
        private ISws_ComTypesService _ComTypesService = null;
        private ISws_DPCInfoService _DPCInfoService = null;
        private ISws_StationService _StationService = null;
        private ISysUserService _UserService = null;
        private ISws_DeviceInfo01Service _DeviceInfo01Service = null;
        private ISws_DeviceInfo02Service _DeviceInfo02Service = null;
        HttpHelper hp = new HttpHelper();
        string service_ip = StaticConstraint.Service_Ip;
        string[] array = { "System.Byte", "System.Int32", "System.Nullable`1[System.Double]", "System.Double" };
        public Sws_RTUInfoController(ISws_RTUInfoService sws_RTUInfoService,
            ISws_ComTypesService sws_ComTypesService,
            ISws_DPCInfoService sws_DPCInfoService,
            ISws_StationService sws_StationService,
            ISysUserService sysUserService,
            ISws_DeviceInfo01Service sws_DeviceInfo01Service,
            ISws_DeviceInfo02Service sws_DeviceInfo02Service
           )
        {
            _RTUInfoService = sws_RTUInfoService;
            _ComTypesService = sws_ComTypesService;
            _DPCInfoService = sws_DPCInfoService;
            _StationService = sws_StationService;
            _UserService = sysUserService;
            _DeviceInfo01Service = sws_DeviceInfo01Service;
            _DeviceInfo02Service = sws_DeviceInfo02Service;
        }
        public IActionResult Index()
        {
           
            return View();
        }
        //通讯信息表格数据查询
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult QuerySwsRtuTable(int pagesize,int pageindex,string deviceid,string order,string sort)
        {
            string filter = "";
            if (!string.IsNullOrEmpty(deviceid))//查询条件
            {
                filter = " DeviceID like '%"+ deviceid + "%'";
            }
            string orderby = sort + " " + order;
            int Totalcount = 0;
            var datalist = _RTUInfoService.GetSwsRtuInfoTable(pageindex,pagesize, orderby, filter,ref Totalcount);
            return Json(new { total = Totalcount, rows = Newtonsoft.Json.JsonConvert.SerializeObject(datalist) });

        }
        #region 页面操作
        //添加
        public IActionResult AddRtuPage()
        {
            SwsRtuinfo model = new SwsRtuinfo();
            //通讯类型集合
            ViewBag.comtypes = _ComTypesService.Query<SwsComTypes>(r => true);
            //设备类型集合
            var deviceTypes = _DPCInfoService.Query<SwsDpcinfo>(r => true);
            if (deviceTypes.Count() > 0)
            {
                ViewBag.deviceTypes = _DPCInfoService.Query<SwsDpcinfo>(r => true).Select(r => r.Dpcname);
            }
            else
            {
                ViewBag.deviceTypes = null;
            }
            model.ComAddress = 2;//默认值
            model.ActivelySent = true;//默认值
            ViewBag.deviceid = "";//设备id
            ViewBag.StationDeviceName = "";
            ViewBag.SationName = "";
            return View("_SetRtuInfo", model);
        }
        //修改
        public IActionResult EditeRtuPage(int id)
        {
            SwsRtuinfo model = _RTUInfoService.Query<SwsRtuinfo>(r => r.Rtuid == id).FirstOrDefault();
            //通讯类型集合
            ViewBag.comtypes = _ComTypesService.Query<SwsComTypes>(r => true);
            //设备类型集合
            var deviceTypes = _DPCInfoService.Query<SwsDpcinfo>(r => true);
            if (deviceTypes.Count() > 0)
            {
                ViewBag.deviceTypes = _DPCInfoService.Query<SwsDpcinfo>(r => true).Select(r => r.Dpcname);
            }
            else
            {
                ViewBag.deviceTypes = null;
            }
            var station = _StationService.Query<SwsStation>(r => r.StationId == model.StationId).FirstOrDefault();
            
            if (station != null)
            {
                var deviceids = "";
                var StationDeviceName = "";
                ViewBag.SationName = station.StationName;
                string tablename = "Sws_DeviceInfo01";
                if (station.InType == 1)
                {
                    tablename = "Sws_DeviceInfo01";

                }
                else if (station.InType == 2)
                {
                    tablename = "Sws_DeviceInfo02";
                }
                var f_itemid = (int)Model.CustomizedClass.Enum.设备分区;
                var devicelist = _RTUInfoService.GetDeviceByRtuId(id, tablename, f_itemid);
                if (devicelist.Count() > 0)
                {
                    deviceids = string.Join(",", devicelist.Select(r => r.DeviceID));
                    StationDeviceName = station.StationName + ":" + string.Join(",", devicelist.Select(r => r.partionname));
                }
                ViewBag.deviceid = deviceids;//设备id
                ViewBag.StationDeviceName = StationDeviceName;
            }
            else
            {
                ViewBag.deviceid = "";//设备id
                ViewBag.StationDeviceName = "";
                ViewBag.SationName = "";
                
            }
            return View("_SetRtuInfo", model);
        }
        //表单提交
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult SetRtuControl(SwsRtuinfo r,string deviceids)
        {
            
             List<long>   deviceidList = new List<long>() { };
            if (!string.IsNullOrEmpty(deviceids))
            {

                deviceidList = new List<string>(deviceids.Split(',')).ConvertAll(r => long.Parse(r));
            }
            if (r.Rtuid == 0)//添加
            {
                //先判断通讯编号和通讯地址是否重复
                var hasmodel = _RTUInfoService.Query<SwsRtuinfo>(a => a.ComAddress == r.ComAddress && a.DeviceId == r.DeviceId).ToList();
                if (hasmodel.Count>0)
                {
                    return Json(new
                    {
                        flag = "have",
                        data = ""
                    });
                }

                r.Rtuid= _UserService.QueryID("RTUID", "Sws_RTUInfo");
                var dpcinfo = _DPCInfoService.Query<SwsDpcinfo>(a => a.Dpcname == r.DeviceType).FirstOrDefault();
                r.PluginFile = dpcinfo.PluginFile;
                var operatenum = _RTUInfoService.AddRtuInfo(r, deviceidList);
                if (operatenum > 0)
                {
                    //Response.ContentType = "text/javascript";
                    //Response.WriteAsync("Noticetest(" + r.Rtuid + ")");
                    return Json(new
                    {
                        flag="ok",
                        data= r.Rtuid
                    });
                 }
                else
                {
                    return Json(new
                    {
                        flag = "no",
                        data = ""
                    });
                }
            }
            else//修改
            {
                //先判断通讯编号和通讯地址是否重复
                var hasmodel = _RTUInfoService.Query<SwsRtuinfo>(a => a.ComAddress == r.ComAddress && a.DeviceId == r.DeviceId&&a.Rtuid!=r.Rtuid).ToList();
                if (hasmodel.Count > 0)
                {
                    return Json(new
                    {
                        flag = "have",
                        data = ""
                    });
                }

                //先判断设备类型是否更改，若更改则改变PluginFile，否则不变动
                var oldmodel = _RTUInfoService.Query<SwsRtuinfo>(a => a.Rtuid == r.Rtuid).AsNoTracking().FirstOrDefault();
                if (oldmodel.DeviceType == r.DeviceType)
                {
                    r.PluginFile = oldmodel.PluginFile;
                }
                else
                {
                    var dpcinfo = _DPCInfoService.Query<SwsDpcinfo>(a => a.Dpcname == r.DeviceType).FirstOrDefault();
                    r.PluginFile = dpcinfo.PluginFile;
                }
                var operatenum = _RTUInfoService.EditeRtuInfo(r, deviceidList);
                if (operatenum > 0)
                {
                    //Response.ContentType = "text/javascript";
                    //Response.WriteAsync("Noticetest("+ r.Rtuid + ")");
                    #region 暂存
                    //try
                    //{
                    //    //var aa1= hp.HttpGet("http://47.93.6.250:10041/Service/C_WNMS_API.asmx/LoadUserInfo?userID=1", "");

                    //    Task.Run<string>(() =>
                    //    {
                    //        try
                    //        {
                    //            string aa = hp.HttpGet("http://" + service_ip + "/WnmsWebService/DeviceManage?guid=aaa&RTUID=" + r.Rtuid + "&CmdFlag=2", "");

                    //            return "ok";
                    //        }
                    //        catch (Exception e)
                    //        {

                    //            return "no";
                    //        }
                    //    }).ContinueWith(tInt =>
                    //    {
                    //        if (tInt.Result == "ok")
                    //        {

                    //            //Response.ContentType = "text/javascript";
                    //            //Response.WriteAsync("alert('通知成功')");
                    //        }
                    //        else
                    //        {

                    //            //Response.ContentType = "text/javascript";
                    //            //Response.WriteAsync("alert('通知失败')");
                    //        }

                    //    });

                    //    return Content("ok");

                    //}
                    //catch (Exception e)
                    //{

                    //    return Content("no");
                    //}
                    #endregion
                    return Json(new
                    {
                        flag = "ok",
                        data = r.Rtuid
                    });

                }
                else
                {
                    return Json(new
                    {
                        flag = "no",
                        data = ""
                    });
                }
            }
        }
        //通知方法
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult GetNotice(string rtuid,string deviceids,string comadress)
        {
            string result = "";
            List<int> rtuids = new List<string>(rtuid.Split(",")).ConvertAll(r => int.Parse(r));
            if (string.IsNullOrEmpty(deviceids))
            {
                try
                {
                    
                    string aa = hp.HttpGet(service_ip + "/WnmsWebService/DeviceManage?guid=CF27D377-8073-4966-BA86-061A5E246E19&RTUID=" + rtuids[0] + "&CmdFlag=2", "");

                    result = "通知成功";
                }
                catch (Exception e)
                {

                    result = "通知失败";
                }
            }
            else
            {
                var deviceid = deviceids.Split(",");
                var ComAddress = comadress.Split(",");
               for(var i=0;i< rtuids.Count;i++)//删除
                {
                    
                    
                    try
                    {
                        string aa = hp.HttpGet(service_ip + "/WnmsWebService/DeviceManage?guid=CF27D377-8073-4966-BA86-061A5E246E19&RTUID=" + rtuids[i] + "&CmdFlag=3", "");

                        result += "通讯编号" + deviceid[i] + "通讯地址" + ComAddress[i] + ",通知成功;";
                    }
                    catch (Exception e)
                    {

                        result += "通讯编号" + deviceid[i] + "通讯地址" + ComAddress[i] + ",通知失败;";
                    }

                }
            
            }
            return Content(result);
 
        }
        //删除
        public IActionResult DeleteRtu(string ids)
        {
            List<int> rtuids = new List<string>(ids.Split(",")).ConvertAll(r => int.Parse(r));
            rtuids.Sort();
           
            var idds = string.Join(",", rtuids);
            var rtulist = _RTUInfoService.Query<SwsRtuinfo>(r => rtuids.Contains(r.Rtuid)).OrderBy(r=>r.Rtuid);
            string deviceids= string.Join(",", rtulist.Select(r=>r.DeviceId));
            string comadress= string.Join(",", rtulist.Select(r => r.ComAddress));
            if (_RTUInfoService.DeleteRtuInfo(rtuids) > 0)
            {
                //Response.ContentType = "text/javascript";
                //Response.WriteAsync("GetNotice('" + idds + "','"+ deviceids + "','"+ comadress + "')");
                return Json(new
                {
                    flag = "ok",
                    data = idds,
                    deviceids= deviceids,
                    comadress= comadress
                });
            }
            else
            {
                return Json(new
                {
                    flag = "no",
                    data = "",
                    deviceids = "",
                    comadress = ""
                });
            }

        }
        #endregion
        #region 选择设备
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult SelectDeviceInfo( int stationid,int rtuid,string deviceid)//泵房id
        {
            var nodes= GetStationTree(stationid,"");
            ViewBag.TreeNodes = new HtmlString(nodes);
            ViewBag.stationid = stationid;
            ViewBag.rtuid = rtuid;
            ViewBag.deviceids = deviceid;
            return View();
        }
        //左侧的泵房树
        private string GetStationTree(int stationid,string stationName)
        {
            var datalist = _RTUInfoService.GetRtu_StationTree(stationid, stationName);
            if (datalist.Count() > 0)
            {
                var treenode = datalist.Select(r => new TreeAction
                {
                    id = r.StationID,
                    pId = 0,
                    name = GetIcon(r.InType)+" "+ r.StationName,
                    @checked=Convert.ToBoolean(r.check1)

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
        public IActionResult SearchTree(int stationid, string stationName)
        {
            return Content(GetStationTree(stationid,stationName));
        }
        private string GetIcon(int type)
        {
            string icon = "";
            if (type == 1)//供水泵房
            {
                icon = "<em class='iconfont icon-bengfang' style='color:blue'></em>";
            }
            else if (type==2)//直饮水泵房
            {
                icon = "<em class='iconfont icon-bengfang' style='color:green'></em>";
            }
            return icon;
        }
        //右侧设备列表
        [TypeFilter(typeof(IgonreActionFilter))]
        public ActionResult GetDeviceInfo(int stationid,int rtuid,string deviceids)
        {
            var stationmodel = _StationService.Query<SwsStation>(r => r.StationId == stationid).FirstOrDefault();
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
            var datalist = _RTUInfoService.GetRtu_StationDevice(stationid,rtuid, tablename, f_itemid, deviceids);
            return Json(new
            {
                data= datalist
            });
        }
        #endregion
        #region 通讯单独dpc配置
        //删除数据
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult DelTimespan()
        {
            ViewBag.datemin = System.DateTime.Now.AddDays(-30);
            ViewBag.datemax = System.DateTime.Now;
            return View();
        }
        public IActionResult DPCInfoPage(int id)
        {
            DPC.Config.DPCConfig dpc = new DPC.Config.DPCConfig();
            var rtuinfo = _RTUInfoService.Query<SwsRtuinfo>(r => r.Rtuid == id).FirstOrDefault();
         
            if (rtuinfo != null && rtuinfo.PluginFile != null)
            {
                dpc = DPC.Config.DPCConfig.FromBinary(rtuinfo.PluginFile);
            }
            else
            {


            }
            List<Node> nodes = DpcToNodes(dpc);
            ViewBag.rtuid = id;
            ViewBag.treeNodes = new HtmlString(Newtonsoft.Json.JsonConvert.SerializeObject(nodes));
            return View();
        }
        [TypeFilter(typeof(IgonreActionFilter))]
        public ActionResult Sws_Setting(string strnodes, int RTUID, string begindate, string enddate)
        {
            try
            {
                //int userID = int.Parse(User.FindFirstValue("UserID"));
                //var user = _sysUserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
                string str = strnodes;
                List<Node> nodes = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Node>>(str);
                DPC.Config.DPCConfig dpc = new DPC.Config.DPCConfig();


                string dpcproperty = nodes.Where(r => r.id == 1).FirstOrDefault().property;

                dpc = Newtonsoft.Json.JsonConvert.DeserializeObject<DPC.Config.DPCConfig>(nodes.Where(r => r.id == 1).FirstOrDefault().property);
                //dpc.Parity = DPC.Config.ParityType.CRC;
                //查询指令集
                List<Node> query = nodes.Where(n => n.id >= 100 && n.id <= 399).ToList();

                if (query.Count > 0)
                {
                    for (int i = 0; i < query.Count; i++)
                    {
                        //指令集条目
                        InstructionsRes ins = Newtonsoft.Json.JsonConvert.DeserializeObject<InstructionsRes>(query[i].property);

                        //模拟量、开关量分类
                        var fourLevelNodes = nodes.Where(n => n.pId == query[i].id).ToList();
                        //具体参数
                        if (fourLevelNodes.Count > 0)
                        {
                            for (int j = 0; j < fourLevelNodes.Count; j++)
                            {
                                List<string> strList = nodes.Where(m => m.pId == fourLevelNodes[j].id).Select(m => m.property).ToList();
                                if ((fourLevelNodes[j].id) % 10 == 1)
                                {
                                    List<AnalogRes> anaList = strList.Select(s => Newtonsoft.Json.JsonConvert.DeserializeObject<AnalogRes>(s)).ToList();
                                    ins.Analogs = anaList;
                                }
                                else if ((fourLevelNodes[j].id) % 10 == 2)
                                {
                                    List<DigitalRes> digList = strList.Select(s => Newtonsoft.Json.JsonConvert.DeserializeObject<DigitalRes>(s)).ToList();
                                    ins.Digitals = digList;
                                }
                            }
                            dpc.Instructions.Add(ins);
                        }
                        else
                        {
                            dpc.Instructions.Add(ins);
                        }
                    }

                    var bytes = dpc.Serialize();
                    var rtumodel = _RTUInfoService.Query<SwsRtuinfo>(r => r.Rtuid == RTUID).FirstOrDefault();
                    rtumodel.PluginFile = bytes;

                    bool IsExecute = false;
                    if (!string.IsNullOrEmpty(begindate) && !string.IsNullOrEmpty(enddate))
                    {
                        try
                        {
                            //删除监控历史记录
                            _RTUInfoService.Update(rtumodel);
                            _RTUInfoService.DeleteDevHistoryInfo(begindate, enddate, RTUID);
                            IsExecute = true;
                        }
                        catch (Exception e)
                        {
                            IsExecute = false;
                        }

                    }
                    else
                    {
                        IsExecute = _RTUInfoService.Update(rtumodel);
                    }
                    
                    if (IsExecute)
                    {
                        //Response.ContentType = "text/javascript";
                        //Response.WriteAsync("Noticetest(" + RTUID + ")");
                        return Json(new
                        {
                            flag = "ok",
                            data = RTUID
                            
                        });
                    }
                    else
                    {
                        return Json(new
                        {
                            flag = "no",
                            data = ""

                        });
                    }
                }
                else
                {
                    return Json(new
                    {
                        flag = "no",
                        data = ""

                    });
                }
            }
            catch (Exception e)
            {
                return Content("错误：" + e.Message);
            }
        }
        #region dpc-node方法
        /// <summary>
        /// dpc转ztree节点
        /// </summary>
        /// <param name="dpc">dpc实体类</param>
        /// <returns></returns>
        public List<Node> DpcToNodes(DPC.Config.DPCConfig dpc)
        {
            #region 初始化节点
            //树的总节点
            List<Node> nodes = new List<Node>();
            nodes.Add(new Node
            {
                id = 1,
                name = dpc.Controller,
                pId = 0,
                property = SetProperty(dpc)
            });
            nodes.Add(new Node
            {
                id = 2,
                name = "状态查询指令集",
                pId = 1
            });

            nodes.Add(new Node
            {
                id = 3,
                name = "指令查询指令集",
                pId = 1
            });

            nodes.Add(new Node
            {
                id = 4,
                name = "远程控制指令集",
                pId = 1
            });
            #endregion

            #region 指令集节点
            ///指令集
            ///CommonQuery  状态查询
            ///CommonParameterQuery  参数查询     

            //状态查询指令集
            List<InstructionsRes> queryList = dpc.Instructions.Where(i => i.CmdType == DpcCmdType.CommonQuery).ToList();
            //指令查询指令集
            List<InstructionsRes> parQueryList = dpc.Instructions.Where(i => i.CmdType == DpcCmdType.CommonParameterQuery).ToList();
            //远程控制指令集
            List<InstructionsRes> teleList = dpc.Instructions.Where(i => i.CmdType != DpcCmdType.CommonParameterQuery && i.CmdType != DpcCmdType.CommonQuery).ToList();

            //状态查询集合
            int queryLength = queryList.Count;
            if (queryLength > 0)
            {
                for (int i = 0; i < queryLength; i++)
                {
                    int queryInsID = 100 + i;
                    nodes.Add(new Node
                    {
                        id = queryInsID,
                        name = queryList[i].Name,
                        pId = 2,
                        key = queryList[i].Key,
                        property = SetProperty(queryList[i])
                    });
                    Node anaConst = new Node();
                    anaConst.id = (queryInsID * 10) + 1;
                    anaConst.name = "模拟量";
                    anaConst.key = "1";
                    anaConst.pId = queryInsID;
                    nodes.Add(anaConst);

                    Node digConst = new Node();
                    digConst.id = (queryInsID * 10) + 2;
                    digConst.name = "开关量";
                    digConst.key = "2";
                    digConst.pId = queryInsID;
                    nodes.Add(digConst);

                    addAnalogsNode(queryList[i].Analogs, anaConst.id, nodes);
                    addDigitalsNode(queryList[i].Digitals, digConst.id, nodes);
                }
            }


            //指令查询集合
            int parQueryLength = parQueryList.Count;
            if (parQueryLength > 0)
            {
                for (int i = 0; i < parQueryLength; i++)
                {
                    int parQueryInsID = 200 + i;
                    nodes.Add(new Node
                    {
                        id = parQueryInsID,
                        name = parQueryList[i].Name,
                        pId = 3,
                        key = parQueryList[i].Key,
                        property = SetProperty(parQueryList[i])
                    });

                    Node anaConst = new Node();
                    anaConst.id = (parQueryInsID * 10) + 1;
                    anaConst.name = "模拟量";
                    anaConst.key = "1";
                    anaConst.pId = parQueryInsID;
                    nodes.Add(anaConst);

                    Node digConst = new Node();
                    digConst.id = (parQueryInsID * 10) + 2;
                    digConst.name = "开关量";
                    digConst.key = "2";
                    digConst.pId = parQueryInsID;
                    nodes.Add(digConst);

                    addAnalogsNode(parQueryList[i].Analogs, anaConst.id, nodes);
                    addDigitalsNode(parQueryList[i].Digitals, digConst.id, nodes);
                }
            }

            //远程控制指令集
            int teleListLength = teleList.Count;
            if (teleListLength > 0)
            {
                for (int i = 0; i < teleListLength; i++)
                {
                    int teleInsID = 300 + i;
                    nodes.Add(new Node
                    {
                        id = teleInsID,
                        name = teleList[i].Name,
                        pId = 4,
                        key = teleList[i].Key,
                        property = SetProperty(teleList[i])
                    });

                }
            }
            #endregion

            return nodes;
        }


        /// <summary>
        /// 添加X指令集下的模拟量
        /// </summary>
        /// <param name="analogRes">X指令集下模拟量集合</param>
        /// <param name="queryInsID">X指令集ID</param>
        /// <param name="nodes">总节点集合</param>
        public void addAnalogsNode(List<AnalogRes> analogRes, int queryInsID, List<Node> nodes)
        {
            int length = analogRes.Count;
            if (length > 0)
            {
                for (int i = 0; i < length; i++)
                {
                    nodes.Add(new Node
                    {
                        id = (1000 * queryInsID) + i,
                        name = analogRes[i].Name,
                        pId = queryInsID,
                        //key = analogRes[i].Key,
                        //property = Newtonsoft.Json.JsonConvert.SerializeObject(analogRes[i])
                        property = SetProperty(analogRes[i])
                    });
                }
            }
        }

        /// <summary>
        /// 添加X指令集下的开关量
        /// </summary>
        /// <param name="digitalRes">X指令集下开关量集合</param>
        /// <param name="parQueryInsID">X指令集ID</param>
        /// <param name="nodes">总节点集合</param>
        public void addDigitalsNode(List<DigitalRes> digitalRes, int parQueryInsID, List<Node> nodes)
        {
            int length = digitalRes.Count;
            if (length > 0)
            {
                for (int i = 0; i < length; i++)
                {
                    nodes.Add(new Node
                    {
                        id = (1000 * parQueryInsID) + i,
                        name = digitalRes[i].Name,
                        pId = parQueryInsID,
                        //key = digitalRes[i].Key,
                        //property = Newtonsoft.Json.JsonConvert.SerializeObject(digitalRes[i])
                        property = SetProperty(digitalRes[i])
                    });
                }
            }
        }

        #endregion

        #region 配置设备类
        public class Node
        {
            public int id
            {
                get;
                set;
            }

            public string name
            {
                get;
                set;
            }

            public int? pId
            {
                get;
                set;
            }

            public string key
            {
                get;
                set;
            }
            public string property
            {
                get;
                set;
            }
        }
        public string SetProperty<T>(T model)
        {
            string result = string.Empty;

            PropertyInfo[] propertys = model.GetType().GetProperties();
            int len = propertys.Length;
            foreach (var item in propertys)
            {
                string value = item.GetValue(model, null) == null ? "" : item.GetValue(model, null).ToString();
                string name = item.Name;
                if (name == "CmdType")
                {
                    object ovalue = System.Enum.Parse(typeof(DPC.CmdType.DpcCmdType), value);
                    value = ((int)(DPC.CmdType.DpcCmdType)ovalue).ToString();
                }


                string type = item.PropertyType.ToString();
                object[] str = item.GetCustomAttributes(typeof(DescriptionAttribute), true);
                string des = "获取或设置资源的节点";
                if (str.Length != 0)
                {
                    des = str[0].GetType().GetProperty("Description").GetValue(str[0]).ToString();
                }
                string editor = "\"text\"";
                if (array.Contains(type))
                {
                    if (type == "System.Nullable`1[System.Double]" || type == "System.Double")
                    {
                        editor = "{\"type\": \"numberbox\",\"options\": {\"precision\": \"2\"}}";
                    }
                    else
                    {
                        editor = "\"numberbox\"";
                    }

                }
                else if (type == "System.Boolean")
                {
                    editor = "{\"type\": \"combobox\",\"options\": {\"data\": [ { \"value\": \"True\", \"text\": \"True\" }, { \"value\": \"False\", \"text\": \"False\" } ],\"panelHeight\": \"auto\"}}";
                }
                else if (type == "DPC.Config.ParityType")
                {
                    editor = "{\"type\": \"combobox\",\"options\": {\"data\": [ { \"value\": \"1\", \"text\": \"CRC\" }, { \"value\": \"0\", \"text\": \"None\" } ],\"panelHeight\": \"auto\"}}";
                }

                else
                {
                    if (type != "System.String" && type != "DPC.CmdType.DpcCmdType")
                    {
                        len--;
                        continue;
                    }
                }
                result += "{\"name\":\"" + name + "\",\"value\":\"" + value + "\",\"group\":\"" + des + "\",\"editor\":" + editor + "},";
            }

            result = "{\"total\": " + len + ",\"rows\":[" + result.Substring(0, result.Length - 1) + "]}";
            result = result.Replace("\r\n", "");
            return result;
        }
        #endregion
        #endregion
    }
}