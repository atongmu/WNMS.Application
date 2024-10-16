using DPC.CmdType;
using DPC.Config.Resource;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using WNMS.Application.Utility.Filters;
using WNMS.IService;
using WNMS.Model.CustomizedClass;
using WNMS.Model.DataModels;

namespace WNMS.Application.Areas.Sws.Controllers
{
    [Area("Sws")]
    public class Sws_EventSchemeController : Controller
    {
        private readonly ISws_DeviceInfo01Service _sws_DeviceInfo01Service;
        private readonly ISws_StationService _sws_StationService;
        private readonly ISws_DataInfoService _sws_DataInfoService;
        private readonly ISysUserService _sysUserService;
        public ISws_UserStationService _sws_UserStationService = null;
        private readonly ISws_RTUInfoService _sws_RTUInfoService;
        public ISws_EventSchemeService _sws_EventSchemeService = null;
        string[] array = { "System.Byte", "System.Int32", "System.Nullable`1[System.Double]", "System.Double" };
        public Sws_EventSchemeController(ISws_DeviceInfo01Service sws_DeviceInfo01Service,
            ISws_StationService sws_StationService,
            ISws_DataInfoService sws_DataInfoService,
            ISysUserService sysUserService,
            ISws_UserStationService sws_UserStationService,
            ISws_RTUInfoService sws_RTUInfoService,
            ISws_EventSchemeService sws_EventSchemeService)
        {
            _sws_DeviceInfo01Service = sws_DeviceInfo01Service;
            _sws_StationService = sws_StationService;
            _sws_DataInfoService = sws_DataInfoService;
            _sysUserService = sysUserService;
            _sws_UserStationService = sws_UserStationService;
            _sws_RTUInfoService = sws_RTUInfoService;
            _sws_EventSchemeService = sws_EventSchemeService;
        }
        public IActionResult Index()
        {
            //泵房树形
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = _sysUserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
            var treeNodes = TreesOfStationDevice(user, "");
            if (!string.IsNullOrEmpty(treeNodes))
            {
                ViewBag.TreeNodes = new HtmlString(treeNodes);
            }
            else
            {
                ViewBag.TreeNodes = "[]";
            }
            return View();
        }
        //设置DPC信息
        public IActionResult GetDPCInfo(int id)
        {
            DPC.Config.DPCConfig dpc = new DPC.Config.DPCConfig();
            var rtuinfo = _sws_RTUInfoService.Query<SwsRtuinfo>(r => r.Rtuid == id).FirstOrDefault();

            if (rtuinfo != null && rtuinfo.PluginFile != null)
            {
                dpc = DPC.Config.DPCConfig.FromBinary(rtuinfo.PluginFile);
            }
            else
            {
            }
            List<Node> nodes = DpcToNodes(dpc);
            var dtreeNodes = new HtmlString(Newtonsoft.Json.JsonConvert.SerializeObject(nodes));
            return Content(dtreeNodes.ToString());
        }
        //获取已存在的信息
        public string GetExist(int id)
        {
            var existdata = _sws_EventSchemeService.Query<SwsEventScheme>(r => r.Rtuid == id).ToList();
            return Newtonsoft.Json.JsonConvert.SerializeObject(existdata);
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
                isDevice = true,
                Partition = t.Partition,
                rtuid = t.RTUID
                //icon = "../../Content/zTree/css/zTreeStyle/area.png"
            });
            var treeList = ztreeStation.Union<TreeAction>(ztreeDevice).Distinct(new TreeAreaCompare());
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(treeList);
            return json;

        }
        //树形查询
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult SelectTree(string stationName)
        {
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var sysUser = _sysUserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();

            var json = TreesOfStationDevice(sysUser, stationName);
            var str = new HtmlString(json);
            return Content(str.ToString());

        }
        #region 提交data报警
        public IActionResult AddScheme(string arrays, int rtuid, int Partition)
        {
            List<datajs> js = Newtonsoft.Json.JsonConvert.DeserializeObject<List<datajs>>(arrays);
            List<SwsEventScheme> ls = new List<SwsEventScheme>();
            //short satrtIndex = 0;
            //if (Partition == 1)
            //{
            //    satrtIndex = 2000;
            //}
            //else if (Partition == 2)
            //{
            //    satrtIndex = 2500;
            //}
            //else if (Partition == 3)
            //{
            //    satrtIndex = 3000;
            //}
            //else if (Partition == 4)
            //{
            //    satrtIndex = 3500;
            //}
            //else if (Partition == 5)
            //{
            //    satrtIndex = 4000;
            //}
            //else if (Partition == 6)
            //{
            //    satrtIndex = 4500;
            //}
            var exitData = _sws_EventSchemeService.Query<SwsEventScheme>(r => r.Rtuid == rtuid);
            if (_sws_EventSchemeService.Delete<SwsEventScheme>(exitData))
            {
                foreach (var item in js)
                {
                    SwsEventScheme swsEventScheme = new SwsEventScheme();
                    //swsEventScheme.DataId = (short)(satrtIndex + item.dataid);
                    swsEventScheme.DataId = item.dataid;
                    swsEventScheme.SchemeName = item.dataname;
                    swsEventScheme.Rtuid = rtuid;
                    ls.Add(swsEventScheme);
                }
                if (_sws_EventSchemeService.Insert<SwsEventScheme>(ls) != null)
                {
                    return Content("提交成功");
                }
                else
                {
                    return Content("提交失败");
                }

            }
            else
            {
                return Content("提交失败");
            }
            

        }
        #endregion
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

            //nodes.Add(new Node
            //{
            //    id = 3,
            //    name = "指令查询指令集",
            //    pId = 1
            //});

            //nodes.Add(new Node
            //{
            //    id = 4,
            //    name = "远程控制指令集",
            //    pId = 1
            //});
            #endregion

            #region 指令集节点
            ///指令集
            ///CommonQuery  状态查询
            ///CommonParameterQuery  参数查询     

            //状态查询指令集
            List<InstructionsRes> queryList = dpc.Instructions.Where(i => i.CmdType == DpcCmdType.CommonQuery).ToList();
            ////指令查询指令集
            //List<InstructionsRes> parQueryList = dpc.Instructions.Where(i => i.CmdType == DpcCmdType.CommonParameterQuery).ToList();
            ////远程控制指令集
            //List<InstructionsRes> teleList = dpc.Instructions.Where(i => i.CmdType != DpcCmdType.CommonParameterQuery && i.CmdType != DpcCmdType.CommonQuery).ToList();

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


            ////指令查询集合
            //int parQueryLength = parQueryList.Count;
            //if (parQueryLength > 0)
            //{
            //    for (int i = 0; i < parQueryLength; i++)
            //    {
            //        int parQueryInsID = 200 + i;
            //        nodes.Add(new Node
            //        {
            //            id = parQueryInsID,
            //            name = parQueryList[i].Name,
            //            pId = 3,
            //            key = parQueryList[i].Key,
            //            property = SetProperty(parQueryList[i])
            //        });

            //        Node anaConst = new Node();
            //        anaConst.id = (parQueryInsID * 10) + 1;
            //        anaConst.name = "模拟量";
            //        anaConst.key = "1";
            //        anaConst.pId = parQueryInsID;
            //        nodes.Add(anaConst);

            //        Node digConst = new Node();
            //        digConst.id = (parQueryInsID * 10) + 2;
            //        digConst.name = "开关量";
            //        digConst.key = "2";
            //        digConst.pId = parQueryInsID;
            //        nodes.Add(digConst);

            //        addAnalogsNode(parQueryList[i].Analogs, anaConst.id, nodes);
            //        addDigitalsNode(parQueryList[i].Digitals, digConst.id, nodes);
            //    }
            //}

            ////远程控制指令集
            //int teleListLength = teleList.Count;
            //if (teleListLength > 0)
            //{
            //    for (int i = 0; i < teleListLength; i++)
            //    {
            //        int teleInsID = 300 + i;
            //        nodes.Add(new Node
            //        {
            //            id = teleInsID,
            //            name = teleList[i].Name,
            //            pId = 4,
            //            key = teleList[i].Key,
            //            property = SetProperty(teleList[i])
            //        });

            //    }
            //}
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
                        isdata = true,
                        datatype = 1,
                        //key = analogRes[i].Key,
                        //property = Newtonsoft.Json.JsonConvert.SerializeObject(analogRes[i])
                        //property = SetProperty(analogRes[i])  
                        dataid = int.Parse(analogRes[i].Name.Contains("预留") ? "0" : analogRes[i].Key)
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
                        isdata = true,
                        datatype = 2,
                        //key = digitalRes[i].Key,
                        //property = Newtonsoft.Json.JsonConvert.SerializeObject(digitalRes[i])
                        dataid = int.Parse(digitalRes[i].Name.Contains("预留") ? "0" : digitalRes[i].Key)
                        //property = SetProperty(digitalRes[i])
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
            public int? dataid
            {
                get;
                set;
            }
            public bool isdata
            {
                get;
                set;
            }
            public int? datatype
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

        public class datajs
        {
            public short dataid;
            public string dataname;

        }
        #endregion
    }
}
