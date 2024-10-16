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
using Microsoft.EntityFrameworkCore;
using WNMS.Application.Utility.Filters;
using WNMS.IService;
using WNMS.Model.CustomizedClass;
using WNMS.Model.DataModels;
using WNMS.Utility;

namespace WNMS.Application.Areas.Sws.Controllers
{
    [Area("Sws")]
    public class Sws_StationAccessController : Controller
    {
        private ISws_StationService _StationService = null;
        private ISws_AccessControlService _AccessControlService = null;
        private ISys_DataItemDetailService _DataItemDetailService = null;
        private ISysUserService _UserService = null;
        private ISws_CameraService cameraService = null;
        private ISws_RTUInfoService _RTUInfoService = null;
        private ISws_ComTypesService _ComTypesService = null;
        private ISws_DPCInfoService _DPCInfoService = null;
        private ISws_DeviceInfo01Service _DeviceInfo01Service = null;
        private ISws_DeviceInfo02Service _DeviceInfo02Service = null;
        private ISws_GUIInfoService _GUIInfoService = null;
        string[] array = { "System.Byte", "System.Int32", "System.Nullable`1[System.Double]", "System.Double" };
        public Sws_StationAccessController(ISws_StationService sws_StationService,
            ISws_AccessControlService sws_AccessControlService,
            ISys_DataItemDetailService sys_DataItemDetailService,
            ISysUserService sysUserService, ISws_CameraService sws_CameraService,
            ISws_RTUInfoService sws_RTUInfoService,
            ISws_ComTypesService sws_ComTypesService,
            ISws_DPCInfoService sws_DPCInfoService,
            ISws_DeviceInfo01Service sws_DeviceInfo01Service,
            ISws_DeviceInfo02Service sws_DeviceInfo02Service,
            ISws_GUIInfoService sws_GUIInfoService)
        {
            _StationService = sws_StationService;
            _AccessControlService = sws_AccessControlService;
            _DataItemDetailService = sys_DataItemDetailService;
            _UserService = sysUserService;
            cameraService = sws_CameraService;
            _RTUInfoService = sws_RTUInfoService;
            _ComTypesService = sws_ComTypesService;
            _DPCInfoService = sws_DPCInfoService;
            _DeviceInfo01Service = sws_DeviceInfo01Service;
            _DeviceInfo02Service = sws_DeviceInfo02Service;
            _GUIInfoService = sws_GUIInfoService;
        }
        public IActionResult Index()
        {
            return View();
        }
        //泵房接入表格统计
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult QueryStationAccess(int pagesize, int pageindex, string searchText, string order, string sort)
        {
            string filter = "1=1";
            if (!string.IsNullOrEmpty(searchText))
            {
                filter = " StationNum like '%" + searchText + "%' or StationName like '%" + searchText + "%'";
            }
            string orderby = sort + " " + order;
            int Totalcount = 0;
            int F_itemid = (int)Model.CustomizedClass.Enum.泵房类型;
            var query = _StationService.QueryStationAccessTable(pageindex, pagesize, F_itemid, orderby, filter, ref Totalcount);
            return Json(new { total = Totalcount, rows = Newtonsoft.Json.JsonConvert.SerializeObject(query) });
        }
        #region 门禁配置
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult setDoorPage(int id)
        {
            ViewBag.stationid = id;
            var f_itemid = (int)Model.CustomizedClass.Enum.门禁品牌;
            var brandList = _DataItemDetailService.Query<SysDataItemDetail>(r => r.FItemId == f_itemid && r.IsEnable == true);
            ViewBag.brandList = brandList;
            return View();
        }
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult QueryDoorTable(int stationid, int pageindex, int pagesize, string searchtext)
        {
            int Totalcount = 0;
            int F_itemid = (int)Model.CustomizedClass.Enum.门禁品牌;
            var query = _StationService.GetAccessByStationid(pageindex, pagesize, stationid, F_itemid, searchtext, ref Totalcount);
            return Json(new { total = Totalcount, rows = Newtonsoft.Json.JsonConvert.SerializeObject(query) });
        }
        //根据门禁id获取门禁信息
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult GetDoorInfoByDoorID(int doorid)
        {
            var model = _AccessControlService.Query<SwsAccessControl>(r => r.DoorId == doorid).FirstOrDefault();
            //if (model != null)
            //{
            //    model.PassWord = WNMS.Encrypt.Decode(model.PassWord);
            //}
            return Json(new
            {
                data = model
            });
        }
        //门禁表单提交
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult SetAccessInfo(string aInfo, int stationid)
        {
            SwsAccessControl accessInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<SwsAccessControl>(aInfo);
            //string passWord = WNMS.Encrypt.Encode(accessInfo.PassWord);   //加密;
            if (accessInfo.DoorId == 0)//添加
            {
                //判断门禁编号是否重复
                var hasmodel = _AccessControlService.Query<SwsAccessControl>(r => r.Num == accessInfo.Num).FirstOrDefault();
                if (hasmodel != null)
                {
                    return Content("has");
                }
                accessInfo.DoorId = _UserService.QueryID("DoorID", "Sws_AccessControl");
                accessInfo.StationId = stationid;
                //accessInfo.PassWord = passWord;
                var operatenum = _AccessControlService.InsertAccessControl(accessInfo);
                if (operatenum > 0)
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
                //判断门禁编号是否重复
                var hasmodel = _AccessControlService.Query<SwsAccessControl>(r => r.Num == accessInfo.Num && r.DoorId != accessInfo.DoorId).FirstOrDefault();
                if (hasmodel != null)
                {
                    return Content("has");
                }

                try
                {
                    //accessInfo.PassWord = passWord;
                    _AccessControlService.Update<SwsAccessControl>(accessInfo);
                    return Content("ok");
                }
                catch (Exception e)
                {
                    return Content("no");
                }
            }
        }
        //门禁挂接提交
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult SetStationAccess_Door(string doorids, int stationid)
        {
            try
            {
                var oprateNum = _StationService.SetStationAccess_Door(doorids, stationid);
                return Content("ok");
            }
            catch (Exception e)
            {
                return Content("no");

            }

        }
        #endregion
        #region 通讯配置
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult setCommiPage(int id)
        {
            var sttaionmodel = _StationService.Query<SwsStation>(r => r.StationId == id).FirstOrDefault();
            var type = sttaionmodel.InType;
            string tablename = "Sws_DeviceInfo01";
            if (type == 2)
            {
                tablename = "Sws_DeviceInfo02";
            }
            var stationDevice = _StationService.GetDeviceByStationID(id, tablename);
            ViewBag.stationDevice = stationDevice;
            ViewBag.stationid = id;
            ViewBag.intype = type;
            return View();
        }
        //获取左侧设备分区以及分区对应的通讯信息
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult GetDeviceAndRtu(int stationid, int intype)
        {
            string tablename = "Sws_DeviceInfo01";
            if (intype == 2)
            {
                tablename = "Sws_DeviceInfo02";
            }
            var stationDevice = _StationService.GetDeviceByStationID(stationid, tablename);
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(stationDevice));
        }
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult QuerySwsRtuTable_Access(int pagesize, int pageindex, string deviceid, string order, string sort, int stationid)
        {
            string filter = "  (r.StationID=0 or r.StationID=" + stationid + ")";
            if (!string.IsNullOrEmpty(deviceid))//查询条件
            {
                filter += " and DeviceID like '%" + deviceid + "%'";
            }
            string orderby = sort + " " + order;
            int Totalcount = 0;
            var datalist = _RTUInfoService.GetSwsRtuInfoTable(pageindex, pagesize, orderby, filter, ref Totalcount);
            return Json(new { total = Totalcount, rows = Newtonsoft.Json.JsonConvert.SerializeObject(datalist) });

        }
        //设备取消通讯
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult CancleRtu(string Deviceid, int Intype)
        {
            var operate = _StationService.CancleRtu_Access(Deviceid, Intype);
            if (operate > 0)
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }
        }
        //设备通讯挂接
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult SetDevice_Rtu(int rtuid, string deviceID, int intype)
        {
            var operate = _StationService.SetDeviceRtu_Access(rtuid, deviceID, intype);
            if (operate > 0)
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }
        }
        //添加通讯页面
        [TypeFilter(typeof(IgonreActionFilter))]
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
            //ViewBag.deviceid = "";//设备id
            //ViewBag.StationDeviceName = "";
            //ViewBag.SationName = "";
            return View("_SetRtuInfo", model);
        }
        //修改通讯页面
        [TypeFilter(typeof(IgonreActionFilter))]
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
            return View("_SetRtuInfo", model);
        }

        //表单提交
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult SetRtuControl(SwsRtuinfo r)
        {


            if (r.Rtuid == 0)//添加
            {
                //先判断通讯编号和通讯地址是否重复
                var hasmodel = _RTUInfoService.Query<SwsRtuinfo>(a => a.ComAddress == r.ComAddress && a.DeviceId == r.DeviceId).ToList();
                if (hasmodel.Count > 0)
                {

                    return Json(new
                    {
                        flag = "have",
                        data = ""
                    });
                }

                r.Rtuid = _UserService.QueryID("RTUID", "Sws_RTUInfo");
                var dpcinfo = _DPCInfoService.Query<SwsDpcinfo>(a => a.Dpcname == r.DeviceType).FirstOrDefault();
                r.PluginFile = dpcinfo.PluginFile;
                try
                {
                    _RTUInfoService.Insert<SwsRtuinfo>(r);
                    return Json(new
                    {
                        flag = "ok",
                        data = r.Rtuid
                    });

                }
                catch (Exception e)
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
                var hasmodel = _RTUInfoService.Query<SwsRtuinfo>(a => a.ComAddress == r.ComAddress && a.DeviceId == r.DeviceId && a.Rtuid != r.Rtuid).ToList();
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
                try
                {
                    _RTUInfoService.Update<SwsRtuinfo>(r);
                    return Json(new
                    {
                        flag = "ok",
                        data = r.Rtuid
                    });

                }
                catch (Exception e)
                {
                    return Json(new
                    {
                        flag = "no",
                        data = ""
                    });

                }

            }
        }
        //dpc修改
        [TypeFilter(typeof(IgonreActionFilter))]
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
        #region 视频接入
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult SetCameraInfo(int id)
        {
            ViewBag.StationID = id;
            return View();
        }

        //查询已接入视频
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult GetAllotCamera(int id, string sortName, string sortOrder, string cameraName, int pageSize = 10, int pageIndex = 1)
        {
            int userID = int.Parse(User.FindFirstValue("UserID"));
            #region 查询条件
            Expression<Func<CameraInfo, bool>> funcWhere = null;
            funcWhere = funcWhere.And(r => r.StationID == id || r.StationID == 0);
            if (!string.IsNullOrWhiteSpace(cameraName))
            {
                funcWhere = funcWhere.And(r => r.CameraName.Contains(cameraName));
            }
            #endregion

            #region  排序
            bool flag = true;
            if (sortOrder == "desc") flag = false;
            string sort = string.IsNullOrWhiteSpace(sortName) ? "CameraName" : sortName;
            #endregion

            PageResult<CameraInfo> cameraList = this.cameraService.LoadInfoList(funcWhere, pageSize, pageIndex, sort, userID, true, flag);
            return Json(new
            {
                total = cameraList.TotalCount,
                rows = Newtonsoft.Json.JsonConvert.SerializeObject(cameraList.DataList)
            });
        }

        //查询未接入视频
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult GetNotAllotCamera(int id)
        {
            int userID = int.Parse(User.FindFirstValue("UserID"));
            string str = @" and c.StationID=0";
            List<CameraInfo> camera = cameraService.LoadCameraInfo(str, userID).ToList();
            return Json(new
            {
                rows = Newtonsoft.Json.JsonConvert.SerializeObject(camera)
            });
        }

        //分配摄像头
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult AllotCamera(string ids, int id)
        {
            List<int> listids = new List<int>();
            if (!string.IsNullOrEmpty(ids))
            {
                string[] cid = ids.Split(",");
                foreach (var item in cid)
                {
                    listids.Add(int.Parse(item));
                }
            }
            List<SwsCamera> listcamera = cameraService.Query<SwsCamera>(r => listids.Contains(r.CameraId)).AsNoTracking().ToList();
            if (listcamera != null)
            {
                foreach (var item in listcamera)
                {
                    item.StationId = id;
                }
                if (cameraService.EditCameraEntitys(listcamera, id) > 0)
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
                return Content("false");
            }
        }

        //取消分配
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult RemoveAllot(int camid, int stationid)
        {
            SwsCamera camera = cameraService.Find<SwsCamera>(camid);
            if (camera != null)
            {
                camera.StationId = 0;
                if (cameraService.Update<SwsCamera>(camera))
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
                return Content("false");
            }
        }
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult GetCameraInfoByCameraID(int cameraid)
        {
            var model = cameraService.Query<SwsCamera>(r => r.CameraId == cameraid).FirstOrDefault();
            if (model != null)
            {
                model.PassWord = WNMS.Encrypt.Decode(model.PassWord);
            }
            return Json(new
            {
                data = model
            });
        }
        #endregion
        #region 工艺图原则
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult SelectGuiByStationID(int id)
        {
            var stationModel = _StationService.Query<SwsStation>(r => r.StationId == id).FirstOrDefault();
            var intype = stationModel.InType;
            var pumpNum = 0;
            if (intype == 1)
            {
                var deviceList = _DeviceInfo01Service.Query<SwsDeviceInfo01>(r => r.StationId == id && r.Partition != 6);
                foreach (var item in deviceList)
                {
                    pumpNum += item.PumpNum;
                }
                var guiList = _GUIInfoService.Query<SwsGuiinfo>(r => r.PumpNum == pumpNum && r.DeviceType == 1 && r.IsSum == 1);
                ViewBag.guiList = guiList;

            }
            else
            {
                var deviceList = _DeviceInfo02Service.Query<SwsDeviceInfo02>(r => r.StationId == id);
                pumpNum = deviceList.Count() * 2;
                var guiList = _GUIInfoService.Query<SwsGuiinfo>(r => r.PumpNum == pumpNum && r.DeviceType == 2 && r.IsSum == 1);
                ViewBag.guiList = guiList;
            }
            ViewBag.guiid = stationModel.GuiNum;
            ViewBag.stationid = id;
            return View("SelectGUIPage");
        }
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult SetGuiOfStation(int stationid, int guinum)
        {
            var stationModel = _StationService.Query<SwsStation>(r => r.StationId == stationid).FirstOrDefault();
            stationModel.GuiNum = guinum;
            try
            {
                _StationService.Update<SwsStation>(stationModel);
                return Content("ok");
            }
            catch (Exception e)
            {
                return Content("no");
            }
        }
        #endregion
        #region 3D工艺图接入
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult Select3DGuiByStationID(int id)
        {
            var stationModel = _StationService.Query<SwsStation>(r => r.StationId == id).FirstOrDefault();
            var intype = stationModel.InType;
            var pumpNum = 0;
            if (intype == 1)
            {
                var deviceList = _DeviceInfo01Service.Query<SwsDeviceInfo01>(r => r.StationId == id && r.Partition != 6);
                foreach (var item in deviceList)
                {
                    pumpNum += item.PumpNum;
                }
                var guiList = _GUIInfoService.Query<SwsGuiinfo>(r => r.PumpNum == pumpNum && r.DeviceType == 1 && r.IsSum == 3);
                ViewBag.guiList = guiList;

            }
            else
            {
                var deviceList = _DeviceInfo02Service.Query<SwsDeviceInfo02>(r => r.StationId == id);
                pumpNum = deviceList.Count() * 2;
                var guiList = _GUIInfoService.Query<SwsGuiinfo>(r => r.PumpNum == pumpNum && r.DeviceType == 2 && r.IsSum == 3);
                ViewBag.guiList = guiList;
            }
            ViewBag.guiid = stationModel.Gui3dnum;
            ViewBag.stationid = id;
            return View("Select3DGuiPage");
        }
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult Set3DGuiOfStation(int stationid, int guinum)
        {
            var stationModel = _StationService.Query<SwsStation>(r => r.StationId == stationid).FirstOrDefault();
            stationModel.Gui3dnum = guinum;
            try
            {
                _StationService.Update<SwsStation>(stationModel);
                return Content("ok");
            }
            catch (Exception e)
            {
                return Content("no");
            }
        }
        #endregion
        #region 控制以及水质接入
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult ChangeAccess(string para, string operate, int stationid)//para为水质或者控制
        {
            var stationmodel = _StationService.Query<SwsStation>(r => r.StationId == stationid).FirstOrDefault();
            if (operate == "add")
            {
                if (para == "水质")
                {
                    stationmodel.WaterQualityMonitor = true;
                }
                else if(para == "控制")
                {
                    stationmodel.ControlMonitor = true;
                }
                else 
                {
                    stationmodel.ControlMonitor_bengfang = true;
                }
            }
            else
            {
                if (para == "水质")
                {
                    stationmodel.WaterQualityMonitor = false;
                }
                else if(para == "控制")
                {
                    stationmodel.ControlMonitor = false;
                }
                else 
                {
                    stationmodel.ControlMonitor_bengfang = false;
                }
            }
            try
            {
                _StationService.Update(stationmodel);
                return Content("ok");
            }
            catch (Exception e)
            {
                return Content("no");
            }
        }
        #endregion
    }
}