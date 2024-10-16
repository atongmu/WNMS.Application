using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using DPC.CmdType;
using DPC.Config.Resource;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using WNMS.Application.Utility.Filters;
using WNMS.IService;
using WNMS.Model.DataModels;

namespace WNMS.Application.Areas.Sws.Controllers
{
    [Area("Sws")]
    public class Sws_DPCInfoController : Controller
    {
        private readonly ISws_DPCInfoService _sws_DPCInfoService;
        private readonly ISysUserService _sysUserService;
        string[] array = { "System.Byte", "System.Int32", "System.Nullable`1[System.Double]", "System.Double" };

        public Sws_DPCInfoController(ISws_DPCInfoService sws_DPCInfoService,ISysUserService _userservice)
        {
            _sws_DPCInfoService = sws_DPCInfoService;
            _sysUserService = _userservice; 
        }
        public IActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 查询DPC数据
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="sort"></param>
        /// <param name="sortOrder"></param>
        /// <param name="dpcName"></param>
        /// <returns></returns>
        [TypeFilter(typeof(IgonreActionFilter))]
        public ActionResult LoadInfoList(int pageSize, int pageIndex, string sort, string sortOrder, string dpcName)
        {
            Expression<Func<SwsDpcinfo, bool>> funcWhere = r => true;
            bool flag = true;
            //查询条件

            if (!string.IsNullOrEmpty(dpcName))
            {
                funcWhere = r => r.Dpcname.Contains(dpcName);
            }
            if (sortOrder == "desc")
            {
                flag = false;
            }
            var infoList = _sws_DPCInfoService.QueryPage(funcWhere, pageSize, pageIndex, sort, flag);
            return Json(new { total = infoList.TotalCount, rows = Newtonsoft.Json.JsonConvert.SerializeObject(infoList.DataList.ToList()) });

        }
        //设置DPC信息
        public IActionResult SetDPCInfo(string id)
        {

            DPC.Config.DPCConfig dpc = new DPC.Config.DPCConfig();
             
            var docInfo = _sws_DPCInfoService.Query<SwsDpcinfo>(r => r.Dpcname == id).FirstOrDefault();
            if (docInfo != null && docInfo.PluginFile != null)
            {
                dpc = DPC.Config.DPCConfig.FromBinary(docInfo.PluginFile);
            }
            else
            {
                 

            } 
            List<Node> nodes = DpcToNodes(dpc);
            ViewBag.id = id;
            ViewBag.treeNodes = new HtmlString(Newtonsoft.Json.JsonConvert.SerializeObject(nodes));
            return View();
        }
        //提交
        [HttpPost]
        [TypeFilter(typeof(IgonreActionFilter))]
        public ActionResult Sws_Setting(string strnodes, string id, string begindate, string enddate)
        {
            try
            {
                int userID = int.Parse(User.FindFirstValue("UserID"));
                var user = _sysUserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
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
                    var docInfo = _sws_DPCInfoService.Query<SwsDpcinfo>(r => r.Dpcname == id).FirstOrDefault();
                    docInfo.PluginFile = bytes;

                    bool IsExecute = false;
                    if (begindate != "" && enddate != "")
                    {
                        //删除监控历史记录
                        //IsExecute = rtuservice.EditEntity(rtu) && deviceJKHistoryService.DeleteDevHistoryInfo(begindate, enddate, dbName, id) >= 0;

                    }
                    //else
                    //{
                    //    IsExecute = rtuservice.EditEntity(rtu);
                    //}
                    IsExecute = _sws_DPCInfoService.Update(docInfo);
                    if (IsExecute)
                    {
                        try
                        {
                            //DPC日志记录
                            //string IP = IPHelper1.IPAddress;
                            //string Address = IpHelper.GetIPCitys(IP);
                            //myLogger.Info(new LogContent(IP, user.UserName, "修改DPC-" + rtu.DeviceName, "修改成功", Address, rtu.EquipmentID.ToString()));
                            //string aa = hp.HttpGet(service_ip + "/WnmsWebService/DeviceManage?guid=aaa&EquipmentID=" + rtu.EquipmentID + "&CmdFlag=2", "");
                        }
                        catch
                        {
                            return Content("error");
                        } 
                        return Content("ok");
                    }
                    else
                    {
                        return Content("no");
                    }
                }
                else
                {
                    return Content("no");
                }
            }
            catch (Exception e)
            {
                return Content("错误：" + e.Message);
            }
        }
        //删除
        [HttpPost]
        public ActionResult DeleteDpc(string requestids)
        {
            var dpc_Ids = requestids.Split(',').ToList();
            List<string> dpcIdsList = new List<string>();
             
            if (_sws_DPCInfoService.DeleteDpcs(dpc_Ids, ref dpcIdsList) > 0)
            {
                string message = "";
                if (dpcIdsList.Count() > 0)            //判断并循环未被删除的项                             
                {
                    foreach (var dpt in dpcIdsList)
                    {
                        message += dpt.ToString() + ",";
                    }
                    message = message.Substring(0, message.Length - 1);
                    return Content("DPC" + message + "被占用，无法删除！其它删除成功！");     //有未被删除的项时返回提示信息
                }
                else
                {
                    return Content("ok");
                }
            }
            else
            {
                if (dpcIdsList.Count() > 0)
                {
                    return Content("false");
                }
                else
                {
                    return Content("no");
                }
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
                    object ovalue = Enum.Parse(typeof(DPC.CmdType.DpcCmdType), value);
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

    }
}