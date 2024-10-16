using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using Command;
using Common;
using Common.WebApiActions;
using HttpUtil;
using Jiguang.JPush.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WNMS.Application.Utility.Filters;
using WNMS.Application.Utility.SignalRChat.Hubs;
using WNMS.IService;
using WNMS.Model.CustomizedClass;
using WNMS.Model.DataModels;
using WNMS.Utility;
using WNMS.Utility.Jpush;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WNMS.Application.Services
{
    //[TypeFilter(typeof(IgonreLoginFilter))]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]/[action]"), ApiController]
    [TypeFilter(typeof(IgonreLoginFilter))]
    public class WMNS_APIController : Controller
    {

        #region MyRegion
        private ISysUserService _ISysUserService = null;
        private ISws_StationService _ISws_StationService = null;
        private ISws_RTUInfoService _ISws_RTUInfoService = null;
        private ISws_UserStationService _ISws_UserStationService = null;
        private ISws_DeviceInfo01Service _ISws_DeviceInfo01Service = null;
        private ISws_DeviceInfo02Service _ISws_DeviceInfo02Service = null;
        private ISws_DeviceInfo03Service _ISws_DeviceInfo03Service = null;
        private ISws_EventHistoryService _ISws_EventHistoryService = null;
        private ISws_EventInfoService _ISws_EventInfoService = null;
        private ISws_CutOffMessageService _ISwsCutOffMessageService = null;
        private IView_DeviceInfoService _IView_DeviceInfoService = null;
        private IGD_InspectionService _InspectionService = null;
        private ISys_DataItemDetailService _DataItemDetailService = null;

        private readonly IGD_RepairService _gD_RepairService;
        private readonly IGD_EventsService _gD_EventsService;
        private IGD_InspectionService _gD_InspectionService;
        private readonly ISws_UserStationService _swsUserStationService;
        private IGD_WorkOrderService _gD_WorkOrderService = null;
        private IGD_ResourceService _gD_ResourceService = null;
        private IGD_WOExtensionService _gD_WOExtensionService = null;
        private IGD_WOReviewService _gD_WOReviewService = null;
        private IGD_TeamUserService _gD_TeamUserService = null;
        private ISws_DataInfoService _DataInfoService = null;

        private IWO_WorkOrderService _wO_WorkOrderService = null;
        private IWO_EventsService _wO_EventsService = null;
        private IWO_ResourceService _wO_ResourceService = null;
        private IWO_WOExtensionService _wO_WOExtensionService = null;
        private IWO_ForwardService _wO_ForwardService = null;
        private IWO_InspectionPlanService _InspectionPlanService = null;
        private IWO_AssignmentPlanService _wO_AssignmentPlanService = null;
        private IWO_InspectPlanCheckService _wO_InspectPlanCheckService = null;
        private IWO_TeamUserService _wO_TeamUserService = null;
        private ISys_EarlyWarningPlanService _sys_EarlyWarningPlanService = null;
        private IWO_PlanInspectOService _wO_PlanInspectOService = null;
        private IWO_InsForwardService _wO_InsForwardService = null;
        private ISws_ConsumpSettingService _ConsumpSettingService = null;
        private IWO_WOOperationService _wO_WOOperationService = null;
        private IWO_InsExtensionService _wO_InsExtensionService = null;
        private ISws_GPSModuleService _sws_GPSModuleService = null;
        IHubContext<ChatHub> _chatHubContext;
        HttpHelper hp = new HttpHelper();
        string service_ip = StaticConstraint.Service_Ip;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public WMNS_APIController(ISysUserService sysuserService, ISws_StationService sws_StationService,
               ISws_RTUInfoService sws_RTUInfoService, ISws_UserStationService sws_UserStationService,
               ISws_DeviceInfo01Service sws_DeviceInfo01Service, ISws_DeviceInfo02Service sws_DeviceInfo02Service,
               ISws_DeviceInfo03Service sws_DeviceInfo03Service, ISws_EventHistoryService sws_EventHistoryService,
               ISws_EventInfoService sws_EventInfoService, ISws_CutOffMessageService swsCutOffMessageService,
               IView_DeviceInfoService view_DeviceInfoService,
               IHubContext<ChatHub> hubcontext,
               IGD_InspectionService gD_InspectionService,
               ISys_DataItemDetailService sys_DataItemDetailService,
               IWebHostEnvironment webHostEnvironment,
               IGD_RepairService gD_RepairService,
               IGD_EventsService gD_EventsService,
               ISws_UserStationService swsUserStationService,
               IGD_WorkOrderService gD_WorkOrderService,
               IGD_ResourceService gD_ResourceService,
               IGD_WOExtensionService gD_WOExtensionService,
               IGD_WOReviewService gD_WOReviewService,
               IGD_TeamUserService gD_TeamUserService,
               ISws_DataInfoService DataInfoService,
               IWO_WorkOrderService wO_WorkOrderService,
               IWO_EventsService wO_EventsService,
               IWO_ResourceService wO_ResourceService,
               IWO_WOExtensionService wO_WOExtensionService,
               IWO_InspectionPlanService wO_InspectionPlanService,
               IWO_ForwardService wO_ForwardService,
               IWO_AssignmentPlanService wO_AssignmentPlanService,
               IWO_InspectPlanCheckService wO_InspectPlanCheckService,
               IWO_TeamUserService wO_TeamUserService,
               ISys_EarlyWarningPlanService sys_EarlyWarningPlanService,
               IWO_PlanInspectOService wO_PlanInspectOService,
               IWO_InsForwardService wO_InsForwardService,
               ISws_ConsumpSettingService sws_ConsumpSettingService,
               IWO_WOOperationService wO_WOOperationService,
               IWO_InsExtensionService wO_InsExtensionService,
               ISws_GPSModuleService sws_GPSModuleService
              )
        {
            this._ISysUserService = sysuserService;
            this._ISws_StationService = sws_StationService;
            this._ISws_RTUInfoService = sws_RTUInfoService;
            this._ISws_UserStationService = sws_UserStationService;
            this._ISws_DeviceInfo01Service = sws_DeviceInfo01Service;
            this._ISws_DeviceInfo02Service = sws_DeviceInfo02Service;
            this._ISws_DeviceInfo03Service = sws_DeviceInfo03Service;
            this._ISws_EventHistoryService = sws_EventHistoryService;
            this._ISws_EventInfoService = sws_EventInfoService;
            this._ISwsCutOffMessageService = swsCutOffMessageService;
            this._IView_DeviceInfoService = view_DeviceInfoService;
            _chatHubContext = hubcontext;
            _InspectionService = gD_InspectionService;
            _DataItemDetailService = sys_DataItemDetailService;
            _webHostEnvironment = webHostEnvironment;
            _gD_RepairService = gD_RepairService;
            _gD_EventsService = gD_EventsService;
            _gD_InspectionService = gD_InspectionService;
            _swsUserStationService = swsUserStationService;
            _gD_WorkOrderService = gD_WorkOrderService;
            _gD_ResourceService = gD_ResourceService;
            _gD_WOExtensionService = gD_WOExtensionService;
            _gD_WOReviewService = gD_WOReviewService;
            _gD_TeamUserService = gD_TeamUserService;

            _DataInfoService = DataInfoService;

            _webHostEnvironment = webHostEnvironment;

            _wO_WorkOrderService = wO_WorkOrderService;
            _wO_EventsService = wO_EventsService;
            _wO_ResourceService = wO_ResourceService;
            _wO_WOExtensionService = wO_WOExtensionService;
            _wO_ForwardService = wO_ForwardService;
            _InspectionPlanService = wO_InspectionPlanService;
            _wO_AssignmentPlanService = wO_AssignmentPlanService;
            _wO_InspectPlanCheckService = wO_InspectPlanCheckService;
            _wO_TeamUserService = wO_TeamUserService;
            _sys_EarlyWarningPlanService = sys_EarlyWarningPlanService;
            _wO_PlanInspectOService = wO_PlanInspectOService;
            _wO_InsForwardService = wO_InsForwardService;
            _ConsumpSettingService = sws_ConsumpSettingService;
            _wO_WOOperationService = wO_WOOperationService;
            _wO_InsExtensionService = wO_InsExtensionService;
            _sws_GPSModuleService = sws_GPSModuleService;
        }
        #endregion

        #region 案例
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
        #endregion

        #region 用户信息模块
        #region 登录
        /// <summary>
        /// 获取Token
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public string GetToken()
        {

            JWTTokenOptions jwtTokenOptions = new JWTTokenOptions();

            //创建用户身份标识
            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Role, "user"),
            };

            //创建令牌
            var token = new JwtSecurityToken(
                issuer: jwtTokenOptions.Issuer,
                audience: jwtTokenOptions.Audience,
                claims: claims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: jwtTokenOptions.Credentials
                );
            var rel = new
            {
                Code = 1,
                Token = new JwtSecurityTokenHandler().WriteToken(token)

            };

            return JsonConvert.SerializeObject(rel);
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="Name">用户名</param>
        /// <param name="Password">密码</param>
        /// <param name="IMEI">手机IMEI</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public string LogIn(string Name, string Password, string IMEI)
        {
            string enConde = WNMS.Utility.Encrypt.Encrypt.MD5Encoding(Password, "WNMS@Standard");
            string naConde = WNMS.Utility.Encrypt.Encrypt.MD5Encoding(Name, "WNMS@Standard");
            var user = _ISysUserService.Query<SysUser>(r => r.Account == naConde && r.Password == enConde && r.IsLock == false && r.IsEnable == true).FirstOrDefault();
            if (user != null)
            {
                if (string.IsNullOrEmpty(user.Imei))
                {
                    user.Imei = IMEI;
                    if (_ISysUserService.Update(user))
                    {
                        var rel = new
                        {
                            Code = 2,
                            Data = user,
                            Message = "用户正在审核，请稍候..."

                        };

                        return JsonConvert.SerializeObject(rel);


                    }
                    else
                    {
                        var rel = new
                        {
                            Code = 0,
                            Message = "登录失败"
                        };
                        return JsonConvert.SerializeObject(rel);
                    }
                }
                else
                {
                    if (user.Imei == IMEI || user.Imei == "666")
                    {
                        //返回Token
                        JWTTokenOptions jwtTokenOptions = new JWTTokenOptions();

                        //创建用户身份标识
                        var claims = new Claim[]
                        {
                          new Claim("UserID",user.UserId.ToString()),
                          new Claim("UserName",user.Account.ToString())
                        };

                        //创建令牌
                        var token = new JwtSecurityToken(
                            issuer: jwtTokenOptions.Issuer,
                            audience: jwtTokenOptions.Audience,
                            claims: claims,
                            notBefore: DateTime.Now,
                            expires: DateTime.Now.AddMinutes(30),
                            signingCredentials: jwtTokenOptions.Credentials
                            );

                        string Token = new JwtSecurityTokenHandler().WriteToken(token);
                        //更新token
                        user.Token = Token;
                        if (_ISysUserService.Update<SysUser>(user))
                        {
                            var rel = new
                            {
                                Code = 1,
                                Message = "用户登录成功",
                                Data = user,
                                Token
                            };
                            return JsonConvert.SerializeObject(rel);
                        }
                        else
                        {
                            var rel = new
                            {
                                Code = 4,
                                Message = "更新用户信息失败"
                            };
                            return JsonConvert.SerializeObject(rel);
                        }

                    }
                    else
                    {
                        var rel = new
                        {
                            Code = 3,
                            Message = "当前IMEI与已存储的IMEI不匹配！"
                        };
                        return JsonConvert.SerializeObject(rel);
                    }
                }


            }
            else
            {
                var rel = new
                {
                    Code = 0,
                    Message = "登录失败"
                };
                return JsonConvert.SerializeObject(rel);
            }

        }
        #endregion

        #region 用户管理
        #region 修改用户密码
        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <param name="UserID">用户ID</param>
        /// <param name="OldPwd">旧密码</param>
        /// <param name="NewPwd">新密码</param>
        /// <param name="ConfirmPwd">确认密码</param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public string EditPassWord(int UserID, string OldPwd, string NewPwd, string ConfirmPwd)
        {
            //获取用户信息
            SysUser user = _ISysUserService.Query<SysUser>(r => r.UserId == UserID).FirstOrDefault();
            string pwd = WNMS.Utility.Encrypt.Encrypt.MD5Encoding(OldPwd, "WNMS@Standard");
            //WNMS.Encrypt.Decode(user.Password);

            //判断原密码是否正确
            if (user.Password != pwd)
            {
                var rel = new
                {
                    Code = 6,
                    Message = "原密码输入不正确"
                };
                return JsonConvert.SerializeObject(rel);
            }
            else
            {
                //判断两次输入密码是否一致
                if (NewPwd != ConfirmPwd)
                {
                    var rel = new
                    {
                        Code = 7,
                        Message = "两次输入的新密码不同"
                    };
                    return JsonConvert.SerializeObject(rel);
                }
                else
                {
                    //密码转换
                    string encodePwd = WNMS.Utility.Encrypt.Encrypt.MD5Encoding(NewPwd, "WNMS@Standard");
                    //string encodePwd = WNMS.Encrypt.Encode(NewPwd);
                    user.Password = encodePwd;
                    //判断修改是否成功
                    if (_ISysUserService.Update(user))
                    {
                        var rel = new
                        {
                            Code = 1,
                            Message = "修改密码成功"
                        };
                        return JsonConvert.SerializeObject(rel);
                    }
                    else
                    {
                        var rel = new
                        {
                            Code = 2,
                            Message = "修改密码失败"
                        };
                        return JsonConvert.SerializeObject(rel);
                    }
                }
            }
        }
        #endregion
        #region 加载个人信息
        /// <summary>
        /// 加载个人信息
        /// </summary>
        /// <param name="UserID">用户ID</param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public string LoadUserInfo(int UserID)
        {
            //获取用户信息
            SysUser user = _ISysUserService.Query<SysUser>(r => r.UserId == UserID).FirstOrDefault();
            if (user != null)
            {
                var rel = new
                {
                    Code = 1,
                    Message = "获取用户信息成功",
                    Data = user
                };
                return JsonConvert.SerializeObject(rel);
            }
            else
            {
                var rel = new
                {
                    Code = 2,
                    Message = "不存在该用户"
                };
                return JsonConvert.SerializeObject(rel);
            }
        }
        #endregion
        #region 编辑个人信息
        /// <summary>
        /// 编辑个人信息
        /// </summary>
        /// <param name="UserID">用户ID</param>
        /// <param name="UserFullName">用户全名</param>
        /// <param name="Phone">手机号</param>
        /// <param name="WeChatID">微信号</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public string EditUserInfo(int UserID, string UserFullName, string Phone, string WeChatID)
        {
            ////验证身份是否过期
            //if (!CheckGuid(guid))
            //{
            //    Context.Response.Write("{\"Code\": \"0\",\"Message\":\"身份验证过期,请重新登陆!\"}");
            //    Context.Response.End();
            //    return;
            //}
            //获取用户信息
            SysUser user = _ISysUserService.Query<SysUser>(r => r.UserId == UserID).FirstOrDefault();
            user.NickName = UserFullName;
            user.Phone = Phone;
            user.WeChatKey = WeChatID;

            //判断修改是否成功
            if (_ISysUserService.Update(user))
            {
                var rel = new
                {
                    Code = 1,
                    Message = "个人信息修改成功"
                };
                return JsonConvert.SerializeObject(rel);
            }
            else
            {
                var rel = new
                {
                    Code = 2,
                    Message = "个人信息修改失败"
                };
                return JsonConvert.SerializeObject(rel);
            }
        }

        #endregion
        #endregion
        #endregion

        #region 远程控制
        /// <summary>
        /// 是否是管理员
        /// </summary>
        /// <param name="UserName">用户名</param>
        /// <param name="UserPwd">密码</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public string IsAdmin(string UserName, string UserPwd)
        {
            //if (!CheckGuid(guid))
            //{
            //    Context.Response.Write("{\"Code\": \"0\",\"Message\":\"身份验证过期,请重新登陆!\"}");
            //    Context.Response.End();
            //    return;
            //}
            string enConde = WNMS.Utility.Encrypt.Encrypt.MD5Encoding(UserPwd, "WNMS@Standard");
            //string enConde = WNMS.Encrypt.Encode(UserPwd);
            SysUser user = _ISysUserService.Query<SysUser>(u => u.Account == UserName && u.Password == enConde).FirstOrDefault();
            if (user != null)
            {
                if (user.IsAdmin == true)
                {
                    return "{\"Code\": \"1\",\"Message\":\"管理员验证通过\"}";
                }
                else
                {
                    return "{\"Code\": \"2\",\"Message\":\"管理员验证不通过\"}";
                }
            }
            else
            {
                return "{\"Code\": \"2\",\"Message\":\"管理员验证不通过\"}";
            }

        }

        /// <summary>
        /// 远程开灯
        /// </summary>
        /// <param name="Id">设备ID</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public string LoadTurnOn(string Id)
        {
            //if (!CheckGuid(guid))
            //{
            //    Context.Response.Write("{\"Code\": \"0\",\"Message\":\"身份验证过期,请重新登陆!\"}");
            //    Context.Response.End();
            //    return;
            //}
            string CmdGuid = Guid.NewGuid().ToString();
            string result = "ok";
            try
            {
                result = hp.HttpGet(service_ip + "/WnmsWebService/DeviceCtrl?guid=CF27D377-8073-4966-BA86-061A5E246E19&RTUID=" + Id + "&cmdType=12", "");
                var rel = new
                {
                    Code = 1,
                    message = "发送成功"
                };
                return JsonConvert.SerializeObject(rel);
            }
            catch (Exception e)
            {
                var rel = new
                {
                    Code = 2,
                    message = e.Message
                };
                return JsonConvert.SerializeObject(rel);
            }
        }

        /// <summary>
        /// 远程关灯
        /// </summary>
        /// <param name="Id">设备ID</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public string LoadTurnOff(string Id)
        {
            //if (!CheckGuid(guid))
            //{
            //    Context.Response.Write("{\"Code\": \"0\",\"Message\":\"身份验证过期,请重新登陆!\"}");
            //    Context.Response.End();
            //    return;
            //}
            string CmdGuid = Guid.NewGuid().ToString();
            string result = "ok";
            try
            {
                result = hp.HttpGet(service_ip + "/WnmsWebService/DeviceCtrl?guid=CF27D377-8073-4966-BA86-061A5E246E19&RTUID=" + Id + "&cmdType=11", "");
                var rel = new
                {
                    Code = 1,
                    message = "发送成功"
                };
                return JsonConvert.SerializeObject(rel);
            }
            catch (Exception e)
            {
                var rel = new
                {
                    Code = 2,
                    message = e.Message
                };
                return JsonConvert.SerializeObject(rel);
            }
        }

        /// <summary>
        /// 远程开门
        /// </summary>
        /// <param name="Id">设备ID</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public string LoadVFDReset(string Id)
        {
            //if (!CheckGuid(guid))
            //{
            //    Context.Response.Write("{\"Code\": \"0\",\"Message\":\"身份验证过期,请重新登陆!\"}");
            //    Context.Response.End();
            //    return;
            //}
            string CmdGuid = Guid.NewGuid().ToString();
            string result = "ok";
            try
            {
                result = hp.HttpGet(service_ip + "/WnmsWebService/DeviceCtrl?guid=CF27D377-8073-4966-BA86-061A5E246E19&RTUID=" + Id + "&cmdType=10", "");
                var rel = new
                {
                    Code = 1,
                    message = "发送成功"
                };
                return JsonConvert.SerializeObject(rel);
            }
            catch (Exception e)
            {
                var rel = new
                {
                    Code = 2,
                    message = e.Message
                };
                return JsonConvert.SerializeObject(rel);
            }
        }

        #region 获取地图显示泵房信息
        /// <summary>
        /// 获取地图显示泵房信息
        /// </summary>
        /// <param name="UserID">用户ID</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]

        public string LoadAllStationsOnMap(int UserID)
        {
            int OnlineDevices = 0, OfflineDevices = 0, AlarmDevices = 0;
            //掉线时长获取
            int timevalue = int.Parse(StaticConstraint.TimeValue);
            var offlinetime = DateTime.Now.AddMinutes(-timevalue);

            var lstSwsUserStation = _ISws_UserStationService.LoadUserStationInfo(UserID).ToList();
            if (lstSwsUserStation.Count() > 0)
            {
                List<StationJK> lstStationJK = new List<StationJK>();
                foreach (var USItem in lstSwsUserStation)
                {
                    StationJK station = new StationJK();
                    station.StationID = USItem.StationID;
                    station.StationName = USItem.StationName;
                    station.FocusOn = USItem.FocusOn;
                    station.Lng = USItem.Lng;
                    station.Lat = USItem.Lat;
                    station.IsAlarm = false;
                    station.IsOffline = false;

                    //查看该泵房是否存在报警设备
                    var lstAlarmDeviceInfo = _ISws_RTUInfoService.LoadAlarmDeviceInfo(USItem.StationID).ToList();
                    if (lstAlarmDeviceInfo.Count > 0)
                    {
                        station.IsAlarm = true;
                        station.IsOffline = false;
                    }

                    //查看该泵房是否掉线
                    List<DeviceJK> lstDeviceJK = new List<DeviceJK>();
                    //设备监控信息
                    var lstSwsRTUInfo = _ISws_RTUInfoService.LoadDeviceAndRtuinfo(USItem.StationID).ToList();
                    if (lstSwsRTUInfo.Count() > 0)
                    {
                        int OfflineCount = 0;
                        foreach (var item in lstSwsRTUInfo)
                        {
                            //获取实时监控数据
                            var listResulr = _ISws_RTUInfoService.GetMongoJKData(item.Rtuid);

                            //如果获取不到或是掉线 掉线个数加1
                            if (listResulr == null || listResulr.UpdateTime < offlinetime)
                            {
                                OfflineCount++;
                            }

                            DeviceJK deviceJK = new DeviceJK();
                            deviceJK.EquipID = item.EquipID;
                            deviceJK.RTUID = item.Rtuid;
                            deviceJK.EquipmentType = item.EquipmentType;
                            deviceJK.Partition = item.Partition;
                            deviceJK.DeviceName = item.DeviceName;

                            string Section = SimpleFactory.GetParameters(item.Partition.ToString());
                            string[] strSection = Section.Split(',');
                            if (strSection.Length == 2)
                            {
                                int iAnalog = int.Parse(strSection[0]);
                                List<int> dataid = new List<int>() { iAnalog, iAnalog + 1, iAnalog + 2 };
                                if (listResulr != null)
                                {
                                    deviceJK.PressureIN = listResulr.AnalogValues.ContainsKey(dataid[0].ToString()) ? double.Parse((listResulr.AnalogValues[dataid[0].ToString()]).ToString()) : 0;
                                    deviceJK.PressureOut = listResulr.AnalogValues.ContainsKey(dataid[1].ToString()) ? double.Parse((listResulr.AnalogValues[dataid[1].ToString()]).ToString()) : 0;
                                    deviceJK.PressureSet = listResulr.AnalogValues.ContainsKey(dataid[2].ToString()) ? double.Parse((listResulr.AnalogValues[dataid[2].ToString()]).ToString()) : 0;
                                    deviceJK.UpdateTime = listResulr.UpdateTime;
                                }
                                else
                                {
                                    deviceJK.PressureIN = 0;
                                    deviceJK.PressureOut = 0;
                                    deviceJK.PressureSet = 0;
                                }
                            }

                            lstDeviceJK.Add(deviceJK);
                        }

                        //如果泵房所有的设备都掉线则标记该泵房掉线
                        if (OfflineCount == lstSwsRTUInfo.Count())
                        {
                            station.IsOffline = true;
                            station.IsAlarm = false;
                        }
                    }

                    station.lstDeviceJK = lstDeviceJK;
                    lstStationJK.Add(station);
                }


                //计算掉线、报警及正常泵房个数
                OnlineDevices = lstStationJK.Where(s => s.IsAlarm == false && s.IsOffline == false).Count();
                OfflineDevices = lstStationJK.Where(s => s.IsAlarm == false && s.IsOffline == true).Count();
                AlarmDevices = lstStationJK.Where(s => s.IsAlarm == true && s.IsOffline == false).Count();

                var rel = new
                {
                    Code = 1,
                    Message = "数据获取成功",
                    Data = lstStationJK,
                    SumCount = lstStationJK.Count(),
                    OnlineDevices = OnlineDevices,
                    OfflineDevices = OfflineDevices,
                    AlarmDevices = AlarmDevices
                };
                return JsonConvert.SerializeObject(rel);
            }
            else
            {
                var rel = new
                {
                    Code = 0,
                    Message = "数据获取失败"
                };
                return JsonConvert.SerializeObject(rel);
            }

        }
        #endregion

        #region 获取带视频监控的泵房配置信息
        /// <summary>
        /// 获取带视频监控的泵房配置信息
        /// </summary>
        /// <param name="UserID">用户ID</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public string LoadStationAndCameraInfo(int UserID)
        {
            var lstSwsUserStation = _ISws_UserStationService.LoadStationAndCameraInfo(UserID).ToList();
            if (lstSwsUserStation.Count > 0)
            {
                var rel = new
                {
                    Code = 1,
                    Message = "数据获取成功",
                    Data = lstSwsUserStation
                };
                return JsonConvert.SerializeObject(rel);
            }
            else
            {
                var rel = new
                {
                    Code = 0,
                    Message = "数据获取失败"
                };
                return JsonConvert.SerializeObject(rel);
            }

        }
        #endregion
        #endregion

        #region 实时数据模块
        #region 获取所有的泵站信息
        /// <summary>
        /// 获取所有的泵站信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public string LoadAllStations()
        {
            var lstSwsStation = _ISws_StationService.Query<SwsStation>(r => true).ToList();
            if (lstSwsStation.Count() > 0)
            {
                var rel = new
                {
                    Code = 1,
                    Message = "数据获取成功",
                    Data = lstSwsStation
                };
                return JsonConvert.SerializeObject(rel);
            }
            else
            {
                var rel = new
                {
                    Code = 0,
                    Message = "数据获取失败"
                };
                return JsonConvert.SerializeObject(rel);
            }

        }
        #endregion

        #region 获取总瞬时流量/昨日同期瞬时流量/增长率/总供水量/总用电量/平均吨水电耗
        /// <summary>
        ///  获取总瞬时流量/昨日同期瞬时流量/增长率/总供水量/总用电量/平均吨水电耗
        /// </summary>
        /// <param name="StationID">泵房ID</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public string LoadRealTimeData(int StationID)
        {
            double TInstantFlow = 0;
            double YInstantFlow = 0;
            string IncreateRate = string.Empty;

            double TTotalFlow = 0;
            double TTotalPower = 0;
            double AverageElectric = 0;

            string YYear = DateTime.Now.AddDays(-1).Year.ToString();

            var lstSwsRTUInfo = _ISws_RTUInfoService.LoadDeviceAndRtuinfo(StationID).ToList();
            if (lstSwsRTUInfo.Count() > 0)
            {
                foreach (var rtu in lstSwsRTUInfo)
                {
                    //获取实时监控数据
                    var listResulr = _ISws_RTUInfoService.GetMongoJKData(rtu.Rtuid);

                    if (listResulr != null)
                    {
                        //查询昨日同期总瞬时流量
                        string beginDate = listResulr.UpdateTime?.AddDays(-1).AddMinutes(-10).ToString();
                        string endDate = listResulr.UpdateTime?.AddDays(-1).AddMinutes(10).ToString();
                        var YlistResulr = _ISws_RTUInfoService.GetMongoHistoryJKData(YYear, beginDate, endDate, rtu.Rtuid.ToString());


                        string Section = SimpleFactory.GetParameters(rtu.Partition.ToString());
                        string[] strSection = Section.Split(',');
                        if (strSection.Length == 2)
                        {
                            int iAnalog = int.Parse(strSection[0]);

                            //计算总瞬时流量
                            List<int> dataid = new List<int>() { iAnalog + 30, iAnalog + 32, iAnalog + 34, iAnalog + 36 };
                            if (listResulr != null)
                            {
                                TInstantFlow = Math.Round(TInstantFlow + WebApiActions.GetSumData(listResulr, dataid), 2);

                                //计算总累计流量
                                dataid = new List<int>() { iAnalog + 31, iAnalog + 33, iAnalog + 35, iAnalog + 37 };
                                TTotalFlow = Math.Round(TTotalFlow + WebApiActions.GetSumData(listResulr, dataid), 2);
                                //计算总累计电量 ---2021-09-16修改
                                //dataid = new List<int>() { iAnalog + 23, iAnalog + 24, iAnalog + 25, iAnalog + 26 };
                                //TTotalPower = Math.Round(TTotalPower + WebApiActions.GetSumData(listResulr, dataid), 2);

                                dataid = new List<int>() { iAnalog + 23, iAnalog + 24, iAnalog + 25, iAnalog + 26 };
                                var TTotalPowerfen = WebApiActions.GetSumData(listResulr, dataid);

                                dataid = new List<int>() { iAnalog + 85 };
                                var TTotalPowerall = WebApiActions.GetSumData(listResulr, dataid);
                                TTotalPower = Math.Round(TTotalPowerall == 0.0 ? TTotalPower + TTotalPowerfen : TTotalPower + TTotalPowerall, 2);
                            }
                            if (YlistResulr != null)
                            {
                                dataid = new List<int>() { iAnalog + 30, iAnalog + 32, iAnalog + 34, iAnalog + 36 };
                                YInstantFlow = Math.Round(YInstantFlow + WebApiActions.GetSumData(YlistResulr, dataid), 2);
                            }

                        }
                        //else
                        //{
                        //    var p = new
                        //    {
                        //        Code = 0,
                        //        Message = "暂无数据"
                        //    };
                        //    return JsonConvert.SerializeObject(p);
                        //}
                    }

                }

                //今日比昨日总瞬时流量增长率
                if (YInstantFlow != 0)
                {
                    IncreateRate = (Math.Round((TInstantFlow - YInstantFlow) / YInstantFlow, 2)) * 100 + "%";
                }
                else
                {
                    IncreateRate = "0.0%";
                }

                //平均吨水电耗
                if (TTotalPower != 0)
                {
                    AverageElectric = Math.Round(TTotalFlow / TTotalPower, 2);
                }

                var rel = new
                {
                    Code = 1,
                    Message = "数据获取成功",
                    TInstantFlow = TInstantFlow,
                    YInstantFlow = YInstantFlow,
                    IncreateRate = IncreateRate,
                    TTotalFlow = TTotalFlow,
                    TTotalPower = TTotalPower,
                    AverageElectric = AverageElectric
                };
                return JsonConvert.SerializeObject(rel);
            }
            else
            {
                var rel = new
                {
                    Code = 0,
                    Message = "暂无数据"
                };
                return JsonConvert.SerializeObject(rel);
            }

        }
        #endregion

        #region 获取水质信息
        /// <summary>
        /// 获取水质信息---泵房实时数据模块
        /// </summary>
        /// <param name="StationID">泵房ID</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public string LoadWaterQualityData(int StationID)
        {
            var SwsRTUInfo = _ISws_RTUInfoService.LoadDeviceAndRtuinfo(StationID).Where(r => r.Partition == "低区").FirstOrDefault();
            if (SwsRTUInfo != null)
            {
                //获取实时监控数据
                var listResulr = _ISws_RTUInfoService.GetMongoJKData(SwsRTUInfo.Rtuid);
                string Section = SimpleFactory.GetParameters(SwsRTUInfo.Partition.ToString());
                string[] strSection = Section.Split(',');
                if (strSection.Length == 2)
                {
                    int iAnalog = int.Parse(strSection[0]);
                    WaterQuality waterQuality = new WaterQuality();

                    //获取水质信息
                    List<int> dataid = new List<int>() { iAnalog + 49, iAnalog + 50, iAnalog + 51, iAnalog + 52, iAnalog + 53, iAnalog + 54 };

                    if (listResulr != null)
                    {
                        waterQuality = WebApiActions.GetWaterQualityData(listResulr, dataid);
                    }

                    var rel = new
                    {
                        Code = 1,
                        Message = "数据获取成功",
                        waterQuality = waterQuality
                    };
                    return JsonConvert.SerializeObject(rel);
                }
                else
                {
                    var rel = new
                    {
                        Code = 0,
                        Message = "数据获取失败"
                    };
                    return JsonConvert.SerializeObject(rel);
                }
            }
            else
            {
                var rel = new
                {
                    Code = 0,
                    Message = "数据获取失败"
                };
                return JsonConvert.SerializeObject(rel);
            }
        }

        /// <summary>
        /// 获取智能泵房水质信息 
        /// </summary>
        /// <param name="StationID">泵房ID</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public string LoadZNStationData(int StationID)
        {
            var SwsRTUInfo = _ISws_RTUInfoService.LoadDeviceAndRtuinfo(StationID).Where(r => r.Partition == "智能泵房").FirstOrDefault();
            if (SwsRTUInfo != null)
            {
                //获取实时监控数据
                var listResulr = _ISws_RTUInfoService.GetMongoJKData(SwsRTUInfo.Rtuid);
                //string Section = SimpleFactory.GetParameters(SwsRTUInfo.Partition.ToString());
                //string[] strSection = Section.Split(',');
                //string[] strSection = { "4500", "5000" };
                string[] strSection = { "4549", "5000" };
                if (strSection.Length == 2)
                {
                    int iAnalog = int.Parse(strSection[0]);
                    WaterQuality waterQuality = new WaterQuality();

                    //获取水质信息 ph  cl 浊度温度 湿度噪音 
                    List<int> dataid = new List<int>() { iAnalog, iAnalog + 1, iAnalog + 2, iAnalog + 3, iAnalog + 4, iAnalog + 5 };

                    if (listResulr != null)
                    {
                        waterQuality = WebApiActions.GetWaterQualityData(listResulr, dataid);
                    }

                    var rel = new
                    {
                        Code = 1,
                        Message = "数据获取成功",
                        waterQuality = waterQuality
                    };
                    return JsonConvert.SerializeObject(rel);
                }
                else
                {
                    var rel = new
                    {
                        Code = 0,
                        Message = "暂无数据"
                    };
                    return JsonConvert.SerializeObject(rel);
                }
            }
            else
            {
                var rel = new
                {
                    Code = 0,
                    Message = "暂无数据"
                };
                return JsonConvert.SerializeObject(rel);
            }
        }
        #endregion

        #region 获取设备实时数据信息
        /// <summary>
        /// 获取设备实时数据信息---实时数据模块
        /// </summary>
        /// <param name="StationID"></param>
        /// <param name="EquipID"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public string LoadDeviceJKDataByStationID(int StationID, string EquipID)
        {
            long lEquipID = Convert.ToInt64(EquipID);
            var item = _ISws_RTUInfoService.LoadDeviceAndRtuinfo(StationID).Where(e => e.EquipID == lEquipID).FirstOrDefault();
            if (item != null)
            {
                List<Pump> lstPumps = new List<Pump>();
                //获取实时监控数据
                var listResulr = _ISws_RTUInfoService.GetMongoJKData(item.Rtuid);
                string Section = SimpleFactory.GetParameters(item.Partition.ToString());
                string[] strSection = Section.Split(',');

                #region 供水设备处理
                if (item.iEquipmentType == 1)
                {
                    DeviceJK_SS deviceJK = new DeviceJK_SS();
                    deviceJK.EquipID = item.EquipID;
                    deviceJK.RTUID = item.Rtuid;
                    deviceJK.EquipmentType = item.EquipmentType;
                    deviceJK.DeviceType = item.DeviceType;
                    deviceJK.Partition = item.Partition;
                    deviceJK.DeviceName = item.DeviceName;
                    deviceJK.IsOnline = "离线";
                    if (listResulr != null)
                    {
                        deviceJK.UpdateTime = Convert.ToDateTime(listResulr.UpdateTime).ToString("yyyy-MM-dd HH:mm:ss");

                        int PumpNum = 0;
                        if (!string.IsNullOrEmpty(item.PumpNum.ToString()))
                        {
                            PumpNum = item.PumpNum;
                        }

                        if (strSection.Length == 2)
                        {
                            int iAnalog = int.Parse(strSection[0]);
                            int iDigial = int.Parse(strSection[1]);

                            //获取进水、出水、设定压力、瞬时流量、累计电量及累计流量
                            List<int> dataid = new List<int>() { iAnalog, iAnalog + 1, iAnalog + 2, iAnalog + 30, iAnalog + 23, iAnalog + 31, iAnalog + 38, 1001 };
                            if (listResulr != null)
                            {
                                deviceJK.PressureIN = listResulr.AnalogValues.ContainsKey(dataid[0].ToString()) ? Math.Round(double.Parse((listResulr.AnalogValues[dataid[0].ToString()]).ToString()), 2) : 0;
                                deviceJK.PressureOut = listResulr.AnalogValues.ContainsKey(dataid[1].ToString()) ? Math.Round(double.Parse((listResulr.AnalogValues[dataid[1].ToString()]).ToString()), 2) : 0;
                                deviceJK.PressureSet = listResulr.AnalogValues.ContainsKey(dataid[2].ToString()) ? Math.Round(double.Parse((listResulr.AnalogValues[dataid[2].ToString()]).ToString()), 2) : 0;
                                deviceJK.InstantFlow = listResulr.AnalogValues.ContainsKey(dataid[3].ToString()) ? Math.Round(double.Parse((listResulr.AnalogValues[dataid[3].ToString()]).ToString()), 2) : 0;
                                deviceJK.TotalPower = listResulr.AnalogValues.ContainsKey(dataid[4].ToString()) ? Math.Round(double.Parse((listResulr.AnalogValues[dataid[4].ToString()]).ToString()), 2) : 0;
                                deviceJK.TotalFlow = listResulr.AnalogValues.ContainsKey(dataid[5].ToString()) ? Math.Round(double.Parse((listResulr.AnalogValues[dataid[5].ToString()]).ToString()), 2) : 0;
                                deviceJK.SetFlow = listResulr.AnalogValues.ContainsKey(dataid[6].ToString()) ? Math.Round(double.Parse((listResulr.AnalogValues[dataid[6].ToString()]).ToString()), 2) : 0;

                                //在线离线
                                bool IsOnline = listResulr.DigitalValues.ContainsKey(dataid[7].ToString()) ? bool.Parse((listResulr.DigitalValues[dataid[7].ToString()]).ToString()) : false;
                                if (IsOnline)
                                {
                                    deviceJK.IsOnline = "在线";
                                }
                                else
                                {
                                    deviceJK.IsOnline = "离线";
                                }

                            }
                            else
                            {
                                deviceJK.PressureIN = 0;
                                deviceJK.PressureOut = 0;
                                deviceJK.PressureSet = 0;
                                deviceJK.InstantFlow = 0;
                                deviceJK.TotalPower = 0;
                                deviceJK.TotalFlow = 0;
                            }



                            //获取水泵信息
                            var fixfrequencyvalue = iAnalog + 3;
                            var frequencyvalue = iAnalog + 4;//频率
                            var electricvalue = iAnalog + 10;//电流
                            var pumpstatevalue = iDigial;//方式
                            var guzhangvalue = iDigial + 3;//状态
                            var bianpinvalue = iDigial + 1;//变频
                            var gongpinvalue = iDigial + 2;//工频
                            var hengpinvalue = iDigial + +127;//恒频
                            int j = 0;
                            for (var i = 0; i < PumpNum; i++)
                            {
                                //单变频
                                if (item.Frequency == 1)
                                {
                                    dataid = new List<int>() { pumpstatevalue + j, fixfrequencyvalue, electricvalue + i, guzhangvalue + j, bianpinvalue + j, hengpinvalue + i, gongpinvalue + j };

                                    //6# 7#采集值是写死的
                                    if (i == 5)
                                    {
                                        dataid = new List<int>() { pumpstatevalue + j, fixfrequencyvalue, electricvalue + i, guzhangvalue + j, bianpinvalue + j, iDigial + 251, gongpinvalue + j };
                                    }
                                    if (i == 6)
                                    {
                                        dataid = new List<int>() { iDigial + 246, fixfrequencyvalue, iAnalog + 154, iDigial + 249, iDigial + 247, iDigial + 252, iDigial + 248 };
                                    }

                                    Pump pump = WebApiActions.GetDataByDataIDMongo(listResulr, dataid);
                                    pump.PumpName = (i + 1) + "#泵";
                                    lstPumps.Add(pump);
                                }
                                //多变频
                                else
                                {
                                    dataid = new List<int>() { pumpstatevalue + j, frequencyvalue + i, electricvalue + i, guzhangvalue + j, bianpinvalue + j, hengpinvalue + i, gongpinvalue + j };

                                    //6# 7#采集值是写死的
                                    if (i == 5)
                                    {
                                        dataid = new List<int>() { pumpstatevalue + j, frequencyvalue + i, electricvalue + i, guzhangvalue + j, bianpinvalue + j, iDigial + 251, gongpinvalue + j };
                                    }
                                    if (i == 6)
                                    {
                                        dataid = new List<int>() { iDigial + 246, iAnalog + 164, iAnalog + 154, iDigial + 249, iDigial + 247, iDigial + 252, iDigial + 248 };
                                    }
                                    Pump pump = WebApiActions.GetDataByDataIDMongo(listResulr, dataid);
                                    pump.PumpName = (i + 1) + "#泵";
                                    lstPumps.Add(pump);
                                }
                                j += 4;
                            }

                            deviceJK.lstPumps = lstPumps;



                            var rel = new
                            {
                                Code = 1,
                                Message = "数据获取成功",
                                deviceJK = deviceJK
                            };
                            return JsonConvert.SerializeObject(rel);
                        }

                        else
                        {
                            var p = new
                            {
                                Code = 1,
                                Message = "设备所属区域（低/中/高/超高/超超高）未填写"
                            };
                            return JsonConvert.SerializeObject(p);
                        }
                    }
                    else
                    {
                        var rel = new
                        {
                            Code = 0,
                            Message = "数据获取失败"
                        };
                        return JsonConvert.SerializeObject(rel);
                    }
                }

                #endregion

                #region 直饮水设备处理
                if (item.iEquipmentType == 2)
                {
                    DeviceJK_SS_ZYS deviceJK = new DeviceJK_SS_ZYS();
                    deviceJK.EquipID = item.EquipID;
                    deviceJK.RTUID = item.Rtuid;
                    deviceJK.EquipmentType = item.EquipmentType;
                    deviceJK.DeviceType = item.DeviceType;
                    deviceJK.Partition = item.Partition;
                    deviceJK.DeviceName = item.DeviceName;
                    deviceJK.IsOnline = "离线";
                    if (listResulr != null)
                    {
                        deviceJK.UpdateTime = Convert.ToDateTime(listResulr.UpdateTime).ToString("yyyy-MM-dd HH:mm:ss");

                        if (strSection.Length == 2)
                        {
                            int iAnalog = int.Parse(strSection[0]);
                            int iDigial = int.Parse(strSection[1]);
                            List<int> dataid = new List<int>() { iAnalog + 16, iAnalog + 17, iAnalog + 15, iAnalog + 11, iAnalog + 14, iAnalog + 32, iAnalog + 33, iAnalog + 34, iAnalog + 35, iAnalog + 36, iAnalog + 37, iAnalog + 23, iAnalog + 4, iAnalog + 5, iAnalog + 6, iAnalog + 7, iAnalog + 8, iAnalog + 9, iAnalog + 10, 1001 };
                            deviceJK.YLevel = listResulr.AnalogValues.ContainsKey(dataid[0].ToString()) ? Math.Round(double.Parse((listResulr.AnalogValues[dataid[0].ToString()]).ToString()), 2) : 0;
                            deviceJK.JLevel = listResulr.AnalogValues.ContainsKey(dataid[1].ToString()) ? Math.Round(double.Parse((listResulr.AnalogValues[dataid[1].ToString()]).ToString()), 2) : 0;
                            deviceJK.YOutPressure = listResulr.AnalogValues.ContainsKey(dataid[2].ToString()) ? Math.Round(double.Parse((listResulr.AnalogValues[dataid[2].ToString()]).ToString()), 2) : 0;
                            deviceJK.JSetpressure = listResulr.AnalogValues.ContainsKey(dataid[3].ToString()) ? Math.Round(double.Parse((listResulr.AnalogValues[dataid[3].ToString()]).ToString()), 2) : 0;
                            deviceJK.JOutPressure = listResulr.AnalogValues.ContainsKey(dataid[4].ToString()) ? Math.Round(double.Parse((listResulr.AnalogValues[dataid[4].ToString()]).ToString()), 2) : 0;

                            deviceJK.PH = listResulr.AnalogValues.ContainsKey(dataid[5].ToString()) ? Math.Round(double.Parse((listResulr.AnalogValues[dataid[5].ToString()]).ToString()), 2) : 0;
                            deviceJK.CL = listResulr.AnalogValues.ContainsKey(dataid[6].ToString()) ? Math.Round(double.Parse((listResulr.AnalogValues[dataid[6].ToString()]).ToString()), 2) : 0;
                            deviceJK.Turbidity = listResulr.AnalogValues.ContainsKey(dataid[7].ToString()) ? Math.Round(double.Parse((listResulr.AnalogValues[dataid[7].ToString()]).ToString()), 2) : 0;
                            deviceJK.ORP = listResulr.AnalogValues.ContainsKey(dataid[8].ToString()) ? Math.Round(double.Parse((listResulr.AnalogValues[dataid[8].ToString()]).ToString()), 2) : 0;
                            deviceJK.Salinity = listResulr.AnalogValues.ContainsKey(dataid[9].ToString()) ? Math.Round(double.Parse((listResulr.AnalogValues[dataid[9].ToString()]).ToString()), 2) : 0;
                            deviceJK.Oxygen = listResulr.AnalogValues.ContainsKey(dataid[10].ToString()) ? Math.Round(double.Parse((listResulr.AnalogValues[dataid[10].ToString()]).ToString()), 2) : 0;
                            deviceJK.Conductivity = listResulr.AnalogValues.ContainsKey(dataid[11].ToString()) ? Math.Round(double.Parse((listResulr.AnalogValues[dataid[11].ToString()]).ToString()), 2) : 0;


                            deviceJK.GPumpState = listResulr.AnalogValues.ContainsKey(dataid[12].ToString()) ? (double.Parse((listResulr.AnalogValues[dataid[12].ToString()]).ToString()) == 0 ? "停止" : "启动") : "停止";
                            deviceJK.QPumpState = listResulr.AnalogValues.ContainsKey(dataid[13].ToString()) ? (double.Parse((listResulr.AnalogValues[dataid[13].ToString()]).ToString()) == 0 ? "停止" : "启动") : "停止";
                            deviceJK.QValveState = listResulr.AnalogValues.ContainsKey(dataid[14].ToString()) ? (double.Parse((listResulr.AnalogValues[dataid[14].ToString()]).ToString()) == 0 ? "关闭" : "打开") : "关闭";
                            deviceJK.PValveState = listResulr.AnalogValues.ContainsKey(dataid[15].ToString()) ? (double.Parse((listResulr.AnalogValues[dataid[15].ToString()]).ToString()) == 0 ? "关闭" : "打开") : "关闭";
                            deviceJK.GValveState = listResulr.AnalogValues.ContainsKey(dataid[16].ToString()) ? (double.Parse((listResulr.AnalogValues[dataid[16].ToString()]).ToString()) == 0 ? "关闭" : "打开") : "关闭";
                            deviceJK.NValveState = listResulr.AnalogValues.ContainsKey(dataid[17].ToString()) ? (double.Parse((listResulr.AnalogValues[dataid[17].ToString()]).ToString()) == 0 ? "关闭" : "打开") : "关闭";
                            deviceJK.HValveState = listResulr.AnalogValues.ContainsKey(dataid[18].ToString()) ? (double.Parse((listResulr.AnalogValues[dataid[18].ToString()]).ToString()) == 0 ? "关闭" : "打开") : "关闭";

                            //在线离线
                            bool IsOnline = listResulr.DigitalValues.ContainsKey(dataid[19].ToString()) ? bool.Parse((listResulr.DigitalValues[dataid[19].ToString()]).ToString()) : false;
                            if (IsOnline)
                            {
                                deviceJK.IsOnline = "在线";
                            }
                            else
                            {
                                deviceJK.IsOnline = "离线";
                            }
                            //List<dynamic> list = new List<dynamic>();
                            //Dictionary<string, object> dataDic = new Dictionary<string, object>();
                            //JObject jObject = new JObject();
                            //foreach (var analogitem in listResulr.AnalogValues)
                            //{
                            //    string a = LoadADataName(int.Parse(analogitem.Key), 2);
                            //    //dataDic.Add(a, item.Value);
                            //    jObject.Add(a, analogitem.Value.ToString());
                            //}
                            //foreach (var digitalitem in listResulr.DigitalValues)
                            //{
                            //    string a = LoadDDataName(int.Parse(digitalitem.Key), 2);
                            //    // dataDic.Add(a, item.Value);
                            //    jObject.Add(a, digitalitem.Value.ToString());
                            //}
                            //list.Add(jObject);

                            //泵的信息
                            List<int> y1dataid = new List<int>() { iAnalog, iDigial + 27, iAnalog + 18 };
                            lstPumps.Add(WebApiActions.GetYDataMongo(listResulr, y1dataid));
                            List<int> y2dataid = new List<int>() { iAnalog + 1, iDigial + 28, iAnalog + 19 };
                            lstPumps.Add(WebApiActions.GetYDataMongo(listResulr, y2dataid));
                            List<int> gdataid = new List<int>() { iAnalog + 4, iDigial + 29, iAnalog + 22 };
                            lstPumps.Add(WebApiActions.GetYDataMongo(listResulr, gdataid));
                            List<int> sumbers = new List<int>() { iAnalog + 2, iDigial + 30, iAnalog + 12, iAnalog + 20 };
                            lstPumps.Add(WebApiActions.GetYDataMongo(listResulr, sumbers));
                            if (item.IsSingle == 1)
                            {
                                List<int> sumbers2 = new List<int>() { iAnalog + 3, iDigial + 31, iAnalog + 12, iAnalog + 21 };
                                lstPumps.Add(WebApiActions.GetYDataMongo(listResulr, sumbers2));
                            }
                            else
                            {
                                List<int> sumbers2 = new List<int>() { iAnalog + 3, iDigial + 31, iAnalog + 13, iAnalog + 21 };
                                lstPumps.Add(WebApiActions.GetYDataMongo(listResulr, sumbers2));
                            }



                            deviceJK.lstPumps = lstPumps;


                            var rel = new
                            {
                                Code = 1,
                                Message = "数据获取成功",
                                deviceJK = deviceJK
                            };
                            return JsonConvert.SerializeObject(rel);
                        }

                        else
                        {
                            var p = new
                            {
                                Code = 1,
                                Message = "设备所属区域（低/中/高/超高/超超高）未填写"
                            };
                            return JsonConvert.SerializeObject(p);
                        }

                    }

                    else
                    {
                        var rel = new
                        {
                            Code = 0,
                            Message = "数据获取失败"
                        };
                        return JsonConvert.SerializeObject(rel);
                    }
                }
                #endregion
                else
                {
                    var rel = new
                    {
                        Code = 0,
                        Message = "数据获取失败"
                    };
                    return JsonConvert.SerializeObject(rel);
                }

            }
            else
            {
                var rel = new
                {
                    Code = 0,
                    Message = "数据获取失败"
                };
                return JsonConvert.SerializeObject(rel);
            }
        }


        /// <summary>
        /// 获取设备实时数据信息---实时数据模块
        /// </summary>
        /// <param name="StationID"></param>
        /// <param name="EquipID"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public string LoadDeviceJKDataByStationID_Z(int StationID, string EquipID)
        {
            long lEquipID = Convert.ToInt64(EquipID);
            var item = _ISws_RTUInfoService.LoadDeviceAndRtuinfo(StationID).Where(e => e.EquipID == lEquipID).FirstOrDefault();
            if (item != null)
            {
                List<Pump> lstPumps = new List<Pump>();
                //获取实时监控数据
                var listResulr = _ISws_RTUInfoService.GetMongoJKData(item.Rtuid);
                string Section = SimpleFactory.GetParameters(item.Partition.ToString());
                string[] strSection = Section.Split(',');

                #region 供水设备处理
                if (item.iEquipmentType == 1)
                {
                    DeviceJK_SS deviceJK = new DeviceJK_SS();
                    deviceJK.EquipID = item.EquipID;
                    deviceJK.RTUID = item.Rtuid;
                    deviceJK.EquipmentType = item.EquipmentType;
                    deviceJK.DeviceType = item.DeviceType;
                    deviceJK.Partition = item.Partition;
                    deviceJK.DeviceName = item.DeviceName;
                    deviceJK.IsOnline = "离线";
                    if (listResulr != null)
                    {
                        deviceJK.UpdateTime = Convert.ToDateTime(listResulr.UpdateTime).ToString("yyyy-MM-dd HH:mm:ss");

                        int PumpNum = 0;
                        if (!string.IsNullOrEmpty(item.PumpNum.ToString()))
                        {
                            PumpNum = item.PumpNum;
                        }

                        if (strSection.Length == 2)
                        {
                            int iAnalog = int.Parse(strSection[0]);
                            int iDigial = int.Parse(strSection[1]);

                            //获取进水、出水、设定压力、瞬时流量、累计电量及累计流量
                            List<int> dataid = new List<int>() { iAnalog, iAnalog + 1, iAnalog + 2, iAnalog + 30, iAnalog + 23, iAnalog + 31, iAnalog + 38, 1001 };
                            if (listResulr != null)
                            {
                                deviceJK.PressureIN = listResulr.AnalogValues.ContainsKey(dataid[0].ToString()) ? Math.Round(double.Parse((listResulr.AnalogValues[dataid[0].ToString()]).ToString()), 2) : 0;
                                deviceJK.PressureOut = listResulr.AnalogValues.ContainsKey(dataid[1].ToString()) ? Math.Round(double.Parse((listResulr.AnalogValues[dataid[1].ToString()]).ToString()), 2) : 0;
                                deviceJK.PressureSet = listResulr.AnalogValues.ContainsKey(dataid[2].ToString()) ? Math.Round(double.Parse((listResulr.AnalogValues[dataid[2].ToString()]).ToString()), 2) : 0;
                                deviceJK.InstantFlow = listResulr.AnalogValues.ContainsKey(dataid[3].ToString()) ? Math.Round(double.Parse((listResulr.AnalogValues[dataid[3].ToString()]).ToString()), 2) : 0;
                                deviceJK.TotalPower = listResulr.AnalogValues.ContainsKey(dataid[4].ToString()) ? Math.Round(double.Parse((listResulr.AnalogValues[dataid[4].ToString()]).ToString()), 2) : 0;
                                deviceJK.TotalFlow = listResulr.AnalogValues.ContainsKey(dataid[5].ToString()) ? Math.Round(double.Parse((listResulr.AnalogValues[dataid[5].ToString()]).ToString()), 2) : 0;
                                deviceJK.SetFlow = listResulr.AnalogValues.ContainsKey(dataid[6].ToString()) ? Math.Round(double.Parse((listResulr.AnalogValues[dataid[6].ToString()]).ToString()), 2) : 0;

                                //在线离线
                                bool IsOnline = listResulr.DigitalValues.ContainsKey(dataid[7].ToString()) ? bool.Parse((listResulr.DigitalValues[dataid[7].ToString()]).ToString()) : false;
                                if (IsOnline)
                                {
                                    deviceJK.IsOnline = "在线";
                                }
                                else
                                {
                                    deviceJK.IsOnline = "离线";
                                }

                            }
                            else
                            {
                                deviceJK.PressureIN = 0;
                                deviceJK.PressureOut = 0;
                                deviceJK.PressureSet = 0;
                                deviceJK.InstantFlow = 0;
                                deviceJK.TotalPower = 0;
                                deviceJK.TotalFlow = 0;
                            }



                            //获取模拟量
                            //string Analogs = JoinAnalogs(1);
                            Dictionary<string, object> dataDic = new Dictionary<string, object>();
                            List<dynamic> list = new List<dynamic>();
                            JObject jObject = new JObject();
                            foreach (var analog in listResulr.AnalogValues)
                            {
                                string a = LoadADataName(int.Parse(analog.Key), item.iEquipmentType);
                                if (a == "")
                                {
                                    jObject.Add(analog.Key, analog.Value.ToString());
                                }
                                else
                                {
                                    jObject.Add(a, analog.Value.ToString());
                                }
                            }
                            foreach (var digital in listResulr.DigitalValues)
                            {
                                string a = LoadDDataName(int.Parse(digital.Key), item.iEquipmentType);
                                //dataDic.Add(a, item.Value);
                                if (a == "")
                                {
                                    jObject.Add(digital.Key, digital.Value.ToString());
                                }
                                else
                                {
                                    jObject.Add(a, digital.Value.ToString());
                                }

                            }
                            list.Add(jObject);
                            deviceJK.list = list;



                            //获取水泵信息
                            var fixfrequencyvalue = iAnalog + 3;
                            var frequencyvalue = iAnalog + 4;//频率
                            var electricvalue = iAnalog + 10;//电流
                            var pumpstatevalue = iDigial;//方式
                            var guzhangvalue = iDigial + 3;//状态
                            var bianpinvalue = iDigial + 1;//变频
                            var gongpinvalue = iDigial + 2;//工频
                            var hengpinvalue = iDigial + +127;//恒频
                            int j = 0;
                            for (var i = 0; i < PumpNum; i++)
                            {
                                //单变频
                                if (item.Frequency == 1)
                                {
                                    //List<int> dataid = new List<int>() { pumpstatevalue + j, fixfrequencyvalue, electricvalue + i, guzhangvalue + j, bianpinvalue + j, hengpinvalue + i, gongpinvalue + j };
                                    dataid = new List<int>() { pumpstatevalue + j, fixfrequencyvalue, electricvalue + i, guzhangvalue + j, bianpinvalue + j, hengpinvalue + i, gongpinvalue + j };
                                    Pump pump = WebApiActions.GetDataByDataIDMongo(listResulr, dataid);
                                    pump.PumpName = (i + 1) + "#泵";
                                    lstPumps.Add(pump);
                                }
                                //多变频
                                else
                                {
                                    //List<int> dataid = new List<int>() { pumpstatevalue + j, frequencyvalue + i, electricvalue + i, guzhangvalue + j, bianpinvalue + j, hengpinvalue + i, gongpinvalue + j };
                                    //new List<int>() { pumpstatevalue + j, frequencyvalue + i, electricvalue + i, guzhangvalue + j, bianpinvalue + j, hengpinvalue + i, gongpinvalue + j };
                                    dataid = new List<int>() { pumpstatevalue + j, frequencyvalue + i, electricvalue + i, guzhangvalue + j, bianpinvalue + j, hengpinvalue + i, gongpinvalue + j };
                                    Pump pump = WebApiActions.GetDataByDataIDMongo(listResulr, dataid);
                                    pump.PumpName = (i + 1) + "#泵";
                                    lstPumps.Add(pump);
                                }
                                j += 4;
                            }

                            deviceJK.lstPumps = lstPumps;



                            var rel = new
                            {
                                Code = 1,
                                Message = "数据获取成功",
                                deviceJK = deviceJK
                            };
                            return JsonConvert.SerializeObject(rel);
                        }

                        else
                        {
                            var p = new
                            {
                                Code = 2,
                                Message = "设备所属区域（低/中/高/超高/超超高）未填写"
                            };
                            return JsonConvert.SerializeObject(p);
                        }
                    }
                    else
                    {
                        var rel = new
                        {
                            Code = 0,
                            Message = "数据获取失败"
                        };
                        return JsonConvert.SerializeObject(rel);
                    }
                }

                #endregion

                #region 直饮水设备处理
                if (item.iEquipmentType == 2)
                {
                    DeviceJK_SS_ZYS deviceJK = new DeviceJK_SS_ZYS();
                    deviceJK.EquipID = item.EquipID;
                    deviceJK.RTUID = item.Rtuid;
                    deviceJK.EquipmentType = item.EquipmentType;
                    deviceJK.DeviceType = item.DeviceType;
                    deviceJK.Partition = item.Partition;
                    deviceJK.DeviceName = item.DeviceName;
                    deviceJK.IsOnline = "离线";
                    if (listResulr != null)
                    {
                        deviceJK.UpdateTime = Convert.ToDateTime(listResulr.UpdateTime).ToString("yyyy-MM-dd HH:mm:ss");

                        if (strSection.Length == 2)
                        {
                            int iAnalog = int.Parse(strSection[0]);
                            int iDigial = int.Parse(strSection[1]);
                            List<int> dataid = new List<int>() { iAnalog + 16, iAnalog + 17, iAnalog + 15, iAnalog + 11, iAnalog + 14, iAnalog + 32, iAnalog + 33, iAnalog + 34, iAnalog + 35, iAnalog + 36, iAnalog + 37, iAnalog + 23, iAnalog + 4, iAnalog + 5, iAnalog + 6, iAnalog + 7, iAnalog + 8, iAnalog + 9, iAnalog + 10, 1001 };
                            deviceJK.YLevel = listResulr.AnalogValues.ContainsKey(dataid[0].ToString()) ? Math.Round(double.Parse((listResulr.AnalogValues[dataid[0].ToString()]).ToString()), 2) : 0;
                            deviceJK.JLevel = listResulr.AnalogValues.ContainsKey(dataid[1].ToString()) ? Math.Round(double.Parse((listResulr.AnalogValues[dataid[1].ToString()]).ToString()), 2) : 0;
                            deviceJK.YOutPressure = listResulr.AnalogValues.ContainsKey(dataid[2].ToString()) ? Math.Round(double.Parse((listResulr.AnalogValues[dataid[2].ToString()]).ToString()), 2) : 0;
                            deviceJK.JSetpressure = listResulr.AnalogValues.ContainsKey(dataid[3].ToString()) ? Math.Round(double.Parse((listResulr.AnalogValues[dataid[3].ToString()]).ToString()), 2) : 0;
                            deviceJK.JOutPressure = listResulr.AnalogValues.ContainsKey(dataid[4].ToString()) ? Math.Round(double.Parse((listResulr.AnalogValues[dataid[4].ToString()]).ToString()), 2) : 0;

                            deviceJK.PH = listResulr.AnalogValues.ContainsKey(dataid[5].ToString()) ? Math.Round(double.Parse((listResulr.AnalogValues[dataid[5].ToString()]).ToString()), 2) : 0;
                            deviceJK.CL = listResulr.AnalogValues.ContainsKey(dataid[6].ToString()) ? Math.Round(double.Parse((listResulr.AnalogValues[dataid[6].ToString()]).ToString()), 2) : 0;
                            deviceJK.Turbidity = listResulr.AnalogValues.ContainsKey(dataid[7].ToString()) ? Math.Round(double.Parse((listResulr.AnalogValues[dataid[7].ToString()]).ToString()), 2) : 0;
                            deviceJK.ORP = listResulr.AnalogValues.ContainsKey(dataid[8].ToString()) ? Math.Round(double.Parse((listResulr.AnalogValues[dataid[8].ToString()]).ToString()), 2) : 0;
                            deviceJK.Salinity = listResulr.AnalogValues.ContainsKey(dataid[9].ToString()) ? Math.Round(double.Parse((listResulr.AnalogValues[dataid[9].ToString()]).ToString()), 2) : 0;
                            deviceJK.Oxygen = listResulr.AnalogValues.ContainsKey(dataid[10].ToString()) ? Math.Round(double.Parse((listResulr.AnalogValues[dataid[10].ToString()]).ToString()), 2) : 0;
                            deviceJK.Conductivity = listResulr.AnalogValues.ContainsKey(dataid[11].ToString()) ? Math.Round(double.Parse((listResulr.AnalogValues[dataid[11].ToString()]).ToString()), 2) : 0;


                            deviceJK.GPumpState = listResulr.AnalogValues.ContainsKey(dataid[12].ToString()) ? (double.Parse((listResulr.AnalogValues[dataid[12].ToString()]).ToString()) == 0 ? "停止" : "启动") : "停止";
                            deviceJK.QPumpState = listResulr.AnalogValues.ContainsKey(dataid[13].ToString()) ? (double.Parse((listResulr.AnalogValues[dataid[13].ToString()]).ToString()) == 0 ? "停止" : "启动") : "停止";
                            deviceJK.QValveState = listResulr.AnalogValues.ContainsKey(dataid[14].ToString()) ? (double.Parse((listResulr.AnalogValues[dataid[14].ToString()]).ToString()) == 0 ? "关闭" : "打开") : "关闭";
                            deviceJK.PValveState = listResulr.AnalogValues.ContainsKey(dataid[15].ToString()) ? (double.Parse((listResulr.AnalogValues[dataid[15].ToString()]).ToString()) == 0 ? "关闭" : "打开") : "关闭";
                            deviceJK.GValveState = listResulr.AnalogValues.ContainsKey(dataid[16].ToString()) ? (double.Parse((listResulr.AnalogValues[dataid[16].ToString()]).ToString()) == 0 ? "关闭" : "打开") : "关闭";
                            deviceJK.NValveState = listResulr.AnalogValues.ContainsKey(dataid[17].ToString()) ? (double.Parse((listResulr.AnalogValues[dataid[17].ToString()]).ToString()) == 0 ? "关闭" : "打开") : "关闭";
                            deviceJK.HValveState = listResulr.AnalogValues.ContainsKey(dataid[18].ToString()) ? (double.Parse((listResulr.AnalogValues[dataid[18].ToString()]).ToString()) == 0 ? "关闭" : "打开") : "关闭";

                            //在线离线
                            bool IsOnline = listResulr.DigitalValues.ContainsKey(dataid[19].ToString()) ? bool.Parse((listResulr.DigitalValues[dataid[19].ToString()]).ToString()) : false;
                            if (IsOnline)
                            {
                                deviceJK.IsOnline = "在线";
                            }
                            else
                            {
                                deviceJK.IsOnline = "离线";
                            }
                            //List<dynamic> list = new List<dynamic>();
                            //Dictionary<string, object> dataDic = new Dictionary<string, object>();
                            //JObject jObject = new JObject();
                            //foreach (var analogitem in listResulr.AnalogValues)
                            //{
                            //    string a = LoadADataName(int.Parse(analogitem.Key), 2);
                            //    //dataDic.Add(a, item.Value);
                            //    jObject.Add(a, analogitem.Value.ToString());
                            //}
                            //foreach (var digitalitem in listResulr.DigitalValues)
                            //{
                            //    string a = LoadDDataName(int.Parse(digitalitem.Key), 2);
                            //    // dataDic.Add(a, item.Value);
                            //    jObject.Add(a, digitalitem.Value.ToString());
                            //}
                            //list.Add(jObject);

                            //泵的信息
                            List<int> y1dataid = new List<int>() { iAnalog, iDigial + 27, iAnalog + 18 };
                            lstPumps.Add(WebApiActions.GetYDataMongo(listResulr, y1dataid));
                            List<int> y2dataid = new List<int>() { iAnalog + 1, iDigial + 28, iAnalog + 19 };
                            lstPumps.Add(WebApiActions.GetYDataMongo(listResulr, y2dataid));
                            List<int> gdataid = new List<int>() { iAnalog + 4, iDigial + 29, iAnalog + 22 };
                            lstPumps.Add(WebApiActions.GetYDataMongo(listResulr, gdataid));
                            List<int> sumbers = new List<int>() { iAnalog + 2, iDigial + 30, iAnalog + 12, iAnalog + 20 };
                            lstPumps.Add(WebApiActions.GetYDataMongo(listResulr, sumbers));
                            if (item.IsSingle == 1)
                            {
                                List<int> sumbers2 = new List<int>() { iAnalog + 3, iDigial + 31, iAnalog + 12, iAnalog + 21 };
                                lstPumps.Add(WebApiActions.GetYDataMongo(listResulr, sumbers2));
                            }
                            else
                            {
                                List<int> sumbers2 = new List<int>() { iAnalog + 3, iDigial + 31, iAnalog + 13, iAnalog + 21 };
                                lstPumps.Add(WebApiActions.GetYDataMongo(listResulr, sumbers2));
                            }



                            deviceJK.lstPumps = lstPumps;


                            var rel = new
                            {
                                Code = 1,
                                Message = "数据获取成功",
                                deviceJK = deviceJK
                            };
                            return JsonConvert.SerializeObject(rel);
                        }

                        else
                        {
                            var p = new
                            {
                                Code = 1,
                                Message = "设备所属区域（低/中/高/超高/超超高）未填写"
                            };
                            return JsonConvert.SerializeObject(p);
                        }

                    }

                    else
                    {
                        var rel = new
                        {
                            Code = 0,
                            Message = "数据获取失败"
                        };
                        return JsonConvert.SerializeObject(rel);
                    }
                }
                #endregion
                else
                {
                    var rel = new
                    {
                        Code = 0,
                        Message = "数据获取失败"
                    };
                    return JsonConvert.SerializeObject(rel);
                }

            }
            else
            {
                var rel = new
                {
                    Code = 0,
                    Message = "数据获取失败"
                };
                return JsonConvert.SerializeObject(rel);
            }
        }


        #region 根据Data信息拼接模拟量开关量 Mongodb
        //模拟量
        private string LoadADataName(int dataid, int EquipmentType)
        {
            var info = _DataInfoService.Query<SwsDataInfo>(r => true && r.DataId == dataid && r.DeviceType == EquipmentType && r.DataType == 1).FirstOrDefault(); ;
            //拼接字符串  模拟量
            string str = "";
            if (info != null)
            {
                str = info.Enname;
            }
            return str;
        }
        //数字量
        private string LoadDDataName(int dataid, int EquipmentType)
        {
            var info = _DataInfoService.Query<SwsDataInfo>(r => true && r.DataId == dataid && r.DeviceType == EquipmentType && r.DataType == 2).FirstOrDefault(); ;
            //拼接字符串  模拟量
            string str = "";
            if (info != null)
            {
                str = info.Enname;
            }
            return str;
        }
        #endregion
        #endregion

        #region 加载手机端模拟量显示模板
        /// <summary>
        /// 加载手机端模拟量显示模板
        /// </summary>
        /// <param name="EquipmentType">设备类型</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public string LoadShowDataInfo(int EquipmentType)
        {
            var dataInfo = _DataInfoService.Query<SwsDataInfo>(r => true && r.DeviceType == EquipmentType && r.IsShow == true && r.DataType == 1);
            if (dataInfo.Count() != 0)
            {
                var rel = new
                {
                    Code = 1,
                    Message = "获取成功",
                    Data = dataInfo
                };
                return JsonConvert.SerializeObject(rel);
            }
            else
            {
                var rel = new
                {
                    Code = 2,
                    Message = "无数据"
                };
                return JsonConvert.SerializeObject(rel);
            }
        }
        #endregion

        #region 获取每日用水量数据列表
        /// <summary>
        /// 获取每日用水量数据列表
        /// </summary>
        /// <param name="StationID">泵房ID</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public string LoadDayReportData(int StationID)
        {
            //每间隔1小时查询一次数据
            int time = 60 * 60 * 1000;
            var beginDate = Convert.ToDateTime(DateTime.Now.ToShortDateString());
            var endDate = beginDate.AddDays(1);
            //数据列表
            List<FLowReport_Last> flowDataList = new List<FLowReport_Last>();
            double MaxValue = 0.0;
            string MaxDate = string.Empty, MinDate = string.Empty;
            double MinValue = 0.0;
            double AverValue = 0.0;

            var lstSwsRTUInfo = _ISws_RTUInfoService.LoadDeviceAndRtuinfo(StationID).ToList();
            if (lstSwsRTUInfo.Count() > 0)
            {
                foreach (var rtu in lstSwsRTUInfo)
                {
                    string group = "";
                    string project = "";
                    //数据查询
                    List<FLowReport> dataList = new List<FLowReport>();

                    //获取所属区域
                    string Section = SimpleFactory.GetParameters(rtu.Partition.ToString());
                    string[] strSection = Section.Split(',');
                    if (strSection.Length == 2)
                    {
                        int iAnalog = int.Parse(strSection[0]);

                        //计算总瞬时流量
                        List<int> sstr = new List<int>() { iAnalog + 31, iAnalog + 33, iAnalog + 35, iAnalog + 37 };
                        group = "{$group:{'_id':{$subtract:[{$subtract:['$UpdateTime',new Date('1970-01-01')]},{$mod:[{$subtract:['$UpdateTime',new Date('1970-01-01')]}," +
                               "" + time + "]}]},'Mindata':{$min:'$AnalogValues." + sstr[0] + "'},'Maxdata':{$max:'$AnalogValues." + sstr[0] + "'},'Mindata2':{$min:'$AnalogValues." + sstr[1] + "'},'Maxdata2':{$max:'$AnalogValues." + sstr[1] + "'}" +
                               ",'Mindata3':{$min:'$AnalogValues." + sstr[2] + "'},'Maxdata3':{$max:'$AnalogValues." + sstr[2] + "'},'Mindata4':{$min:'$AnalogValues." + sstr[3] + "'},'Maxdata4':{$max:'$AnalogValues." + sstr[3] + "'}}}";

                        project = "{$project:{'_id': 0,'Mindata':{$add:[{$ifNull:['$Mindata',0.0]},{$ifNull:['$Mindata2',0.0]},{$ifNull:['$Mindata3',0.0]},{$ifNull:['$Mindata4',0.0]}]}," +
                            "'Maxdata': {$add:[{$ifNull:['$Maxdata',0.0]},{$ifNull:['$Maxdata2',0.0]},{$ifNull:['$Maxdata3',0.0]},{$ifNull:['$Maxdata4',0.0]}]}," +
                          "'Time':{$add: [new Date(-28800000), '$_id']}}}";

                        dataList = _ISws_RTUInfoService.GetFlowReportData(beginDate.Year.ToString(), beginDate.ToString(), endDate.ToString(), group, project, rtu.Rtuid.ToString()).ToList();

                        if (dataList.Count > 0)
                        {
                            for (var i = 1; i < dataList.Count(); i++)
                            {
                                FLowReport_Last fLowReport = new FLowReport_Last();
                                fLowReport.Data = dataList[i].Mindata - dataList[i - 1].Mindata;
                                fLowReport.Data = Math.Round(double.Parse(fLowReport.Data.ToString()), 2);
                                fLowReport.Time = dataList[i].Time;

                                //查询是否已经添加相同时间段的数据，如果已经添加就在原先基础上新增（即将低区、中区、高区等相同时间段获取的用水量相加）
                                var QueryflowData = flowDataList.Where(f => f.Time == fLowReport.Time).FirstOrDefault();
                                if (QueryflowData != null)
                                {
                                    flowDataList.Remove(QueryflowData);
                                    QueryflowData.Data = QueryflowData.Data + fLowReport.Data;

                                    flowDataList.Add(QueryflowData);
                                }
                                else
                                {
                                    flowDataList.Add(fLowReport);
                                }

                            }
                        }

                    }
                    //else
                    //{
                    //    var p = new
                    //    {
                    //        Code = 0,
                    //        Message = "数据获取失败"
                    //    };
                    //    return JsonConvert.SerializeObject(p);
                    //}

                }


                //计算最大值，最小值及平均值
                if (flowDataList.Count > 0)
                {
                    var MaxData = flowDataList.Max(r => r.Data);
                    MaxValue = Math.Round((double)MaxData, 2);
                    MaxDate = flowDataList.Where(r => r.Data == MaxData).FirstOrDefault().Time.ToString();

                    var MinData = flowDataList.Min(r => r.Data);
                    MinValue = Math.Round((double)MinData, 2);
                    MinDate = flowDataList.Where(r => r.Data == MinData).FirstOrDefault().Time.ToString();

                    AverValue = Math.Round((double)flowDataList.Average(r => r.Data), 2);
                }



                var rel = new
                {
                    Code = 1,
                    Message = "数据获取成功",
                    Data = flowDataList,
                    MaxValue = MaxValue.ToString(),
                    MaxDate = MaxDate.ToString(),
                    MinValue = MinValue.ToString(),
                    MinDate = MinDate.ToString(),
                    AverValue = AverValue.ToString()
                };
                return JsonConvert.SerializeObject(rel);
            }
            else
            {
                var rel = new
                {
                    Code = 0,
                    Message = "暂无数据"
                };
                return JsonConvert.SerializeObject(rel);
            }

        }
        #endregion

        #region 获取每周用水量数据列表
        /// <summary>
        /// 获取每周用水量数据列表
        /// </summary>
        /// <param name="StationID">泵房ID</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public string LoadWeekReportData(int StationID)
        {
            var zhouji = DateTime.Now.DayOfWeek;
            var num = (int)System.Enum.Parse(typeof(WeekDay), zhouji.ToString());
            if (num == 1)
            {
                num = 8;
            }

            var beginDate = Convert.ToDateTime(string.Format("{0:yyyy-MM-dd}", DateTime.Now.AddDays(2 - num)));
            var endDate = Convert.ToDateTime(string.Format("{0:yyyy-MM-dd}", DateTime.Now.AddDays(9 - num)));

            //数据列表
            List<FLowReport_Last> flowDataList = new List<FLowReport_Last>();
            double MaxValue = 0.0;
            string MaxDate = string.Empty, MinDate = string.Empty;
            double MinValue = 0.0;
            double AverValue = 0.0;

            var lstSwsRTUInfo = _ISws_RTUInfoService.LoadDeviceAndRtuinfo(StationID).ToList();
            if (lstSwsRTUInfo.Count() > 0)
            {
                foreach (var rtu in lstSwsRTUInfo)
                {
                    string group = "";
                    string project = "";
                    //数据查询
                    List<FLowReport> dataList = new List<FLowReport>();

                    //获取所属区域
                    string Section = SimpleFactory.GetParameters(rtu.Partition.ToString());
                    string[] strSection = Section.Split(',');
                    if (strSection.Length == 2)
                    {
                        int iAnalog = int.Parse(strSection[0]);

                        //计算总瞬时流量
                        List<int> sstr = new List<int>() { iAnalog + 31, iAnalog + 33, iAnalog + 35, iAnalog + 37 };
                        group = "{$group:{'_id':{'$dateToString':{'format':'%Y-%m-%d','date':'$UpdateTime'}}," +
                                   "'Mindata':{$min:'$AnalogValues." + sstr[0] + "'},'Maxdata':{$max:'$AnalogValues." + sstr[0] + "'},'Mindata2':{$min:'$AnalogValues." + sstr[1] + "'},'Maxdata2':{$max:'$AnalogValues." + sstr[1] + "'}" +
                                   ",'Mindata3':{$min:'$AnalogValues." + sstr[2] + "'},'Maxdata3':{$max:'$AnalogValues." + sstr[2] + "'},'Mindata4':{$min:'$AnalogValues." + sstr[3] + "'},'Maxdata4':{$max:'$AnalogValues." + sstr[3] + "'}}}";

                        project = "{$project:{'_id': 0,'Mindata':{$add:[{$ifNull:['$Mindata',0.0]},{$ifNull:['$Mindata2',0.0]},{$ifNull:['$Mindata3',0.0]},{$ifNull:['$Mindata4',0.0]}]}," +
                              "'Maxdata': {$add:[{$ifNull:['$Maxdata',0.0]},{$ifNull:['$Maxdata2',0.0]},{$ifNull:['$Maxdata3',0.0]},{$ifNull:['$Maxdata4',0.0]}]}," +
                            "'Time':'$_id'}}";

                        dataList = _ISws_RTUInfoService.GetFlowReportData(beginDate.Year.ToString(), beginDate.ToString(), endDate.ToString(), group, project, rtu.Rtuid.ToString()).ToList();
                        if (beginDate.Year != endDate.Year)
                        {
                            var dataappend = _ISws_RTUInfoService.GetFlowReportData(endDate.Year.ToString(), beginDate.ToString(), endDate.ToString(), group, project, rtu.Rtuid.ToString()).ToList();
                            if (dataappend.Count > 0)
                            {
                                dataList = dataList.Concat(dataappend).ToList();
                            }
                        }

                        if (dataList.Count > 0)
                        {
                            for (var i = 1; i < dataList.Count(); i++)
                            {
                                FLowReport_Last fLowReport = new FLowReport_Last();
                                fLowReport.Data = dataList[i].Mindata - dataList[i - 1].Mindata;
                                fLowReport.Data = Math.Round(double.Parse(fLowReport.Data.ToString()), 2);
                                fLowReport.Time = dataList[i].Time;

                                //查询是否已经添加相同时间段的数据，如果已经添加就在原先基础上新增（即将低区、中区、高区等相同时间段获取的用水量相加）
                                var QueryflowData = flowDataList.Where(f => f.Time == fLowReport.Time).FirstOrDefault();
                                if (QueryflowData != null)
                                {
                                    flowDataList.Remove(QueryflowData);
                                    QueryflowData.Data = QueryflowData.Data + fLowReport.Data;
                                    flowDataList.Add(QueryflowData);
                                }
                                else
                                {
                                    flowDataList.Add(fLowReport);
                                }

                            }
                        }

                    }
                    //else
                    //{
                    //    var p = new
                    //    {
                    //        Code = 0,
                    //        Message = "数据获取失败"
                    //    };
                    //    return JsonConvert.SerializeObject(p);
                    //}

                }


                //计算最大值，最小值及平均值
                if (flowDataList.Count > 0)
                {
                    var MaxData = flowDataList.Max(r => r.Data);
                    MaxValue = Math.Round((double)MaxData, 2);
                    MaxDate = flowDataList.Where(r => r.Data == MaxData).FirstOrDefault().Time.ToString();

                    var MinData = flowDataList.Min(r => r.Data);
                    MinValue = Math.Round((double)MinData, 2);
                    MinDate = flowDataList.Where(r => r.Data == MinData).FirstOrDefault().Time.ToString();

                    AverValue = Math.Round((double)flowDataList.Average(r => r.Data), 2);
                }



                var rel = new
                {
                    Code = 1,
                    Message = "数据获取成功",
                    Data = flowDataList,
                    MaxValue = MaxValue.ToString(),
                    MaxDate = MaxDate.ToString(),
                    MinValue = MinValue.ToString(),
                    MinDate = MinDate.ToString(),
                    AverValue = AverValue.ToString()
                };
                return JsonConvert.SerializeObject(rel);
            }
            else
            {
                var rel = new
                {
                    Code = 0,
                    Message = "暂无数据"
                };
                return JsonConvert.SerializeObject(rel);
            }

        }
        #endregion

        #region 获取每月用水量数据列表
        /// <summary>
        /// 获取每月用水量数据列表
        /// </summary>
        /// <param name="StationID">泵房ID</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public string LoadMonthReportData(int StationID)
        {
            string beginTime = DateTime.Now.Year + "-" + DateTime.Now.Month + "-01 00:00:00";
            var beginDate = Convert.ToDateTime(beginTime);
            var endDate = beginDate.AddMonths(1).AddDays(1).Date;
            //数据列表
            List<FLowReport_Last> flowDataList = new List<FLowReport_Last>();
            double MaxValue = 0.0;
            string MaxDate = string.Empty, MinDate = string.Empty;
            double MinValue = 0.0;
            double AverValue = 0.0;

            var lstSwsRTUInfo = _ISws_RTUInfoService.LoadDeviceAndRtuinfo(StationID).ToList();
            if (lstSwsRTUInfo.Count() > 0)
            {
                foreach (var rtu in lstSwsRTUInfo)
                {
                    string group = "";
                    string project = "";
                    //数据查询
                    List<FLowReport> dataList = new List<FLowReport>();

                    //获取所属区域
                    string Section = SimpleFactory.GetParameters(rtu.Partition.ToString());
                    string[] strSection = Section.Split(',');
                    if (strSection.Length == 2)
                    {
                        int iAnalog = int.Parse(strSection[0]);

                        //计算总瞬时流量
                        List<int> sstr = new List<int>() { iAnalog + 31, iAnalog + 33, iAnalog + 35, iAnalog + 37 };
                        group = "{$group:{'_id':{'$dateToString':{'format':'%Y-%m-%d','date':'$UpdateTime'}}," +
                                   "'Mindata':{$min:'$AnalogValues." + sstr[0] + "'},'Maxdata':{$max:'$AnalogValues." + sstr[0] + "'},'Mindata2':{$min:'$AnalogValues." + sstr[1] + "'},'Maxdata2':{$max:'$AnalogValues." + sstr[1] + "'}" +
                                   ",'Mindata3':{$min:'$AnalogValues." + sstr[2] + "'},'Maxdata3':{$max:'$AnalogValues." + sstr[2] + "'},'Mindata4':{$min:'$AnalogValues." + sstr[3] + "'},'Maxdata4':{$max:'$AnalogValues." + sstr[3] + "'}}}";

                        project = "{$project:{'_id': 0,'Mindata':{$add:[{$ifNull:['$Mindata',0.0]},{$ifNull:['$Mindata2',0.0]},{$ifNull:['$Mindata3',0.0]},{$ifNull:['$Mindata4',0.0]}]}," +
                              "'Maxdata': {$add:[{$ifNull:['$Maxdata',0.0]},{$ifNull:['$Maxdata2',0.0]},{$ifNull:['$Maxdata3',0.0]},{$ifNull:['$Maxdata4',0.0]}]}," +
                            "'Time':'$_id'}}";

                        dataList = _ISws_RTUInfoService.GetFlowReportData(beginDate.Year.ToString(), beginDate.ToString(), endDate.ToString(), group, project, rtu.Rtuid.ToString()).ToList();
                        if (beginDate.Year != endDate.Year)
                        {
                            var dataappend = _ISws_RTUInfoService.GetFlowReportData(endDate.Year.ToString(), beginDate.ToString(), endDate.ToString(), group, project, rtu.Rtuid.ToString()).ToList();
                            if (dataappend.Count > 0)
                            {
                                dataList = dataList.Concat(dataappend).ToList();
                            }
                        }

                        if (dataList.Count > 0)
                        {
                            for (var i = 1; i < dataList.Count(); i++)
                            {
                                FLowReport_Last fLowReport = new FLowReport_Last();
                                fLowReport.Data = dataList[i].Mindata - dataList[i - 1].Mindata;
                                fLowReport.Data = Math.Round(double.Parse(fLowReport.Data.ToString()), 2);
                                fLowReport.Time = dataList[i].Time;

                                //查询是否已经添加相同时间段的数据，如果已经添加就在原先基础上新增（即将低区、中区、高区等相同时间段获取的用水量相加）
                                var QueryflowData = flowDataList.Where(f => f.Time == fLowReport.Time).FirstOrDefault();
                                if (QueryflowData != null)
                                {
                                    flowDataList.Remove(QueryflowData);
                                    QueryflowData.Data = QueryflowData.Data + fLowReport.Data;
                                    flowDataList.Add(QueryflowData);
                                }
                                else
                                {
                                    flowDataList.Add(fLowReport);
                                }

                            }
                        }

                    }
                    //else
                    //{
                    //    var p = new
                    //    {
                    //        Code = 0,
                    //        Message = "数据获取失败"
                    //    };
                    //    return JsonConvert.SerializeObject(p);
                    //}

                }


                //计算最大值，最小值及平均值
                if (flowDataList.Count > 0)
                {
                    var MaxData = flowDataList.Max(r => r.Data);
                    MaxValue = Math.Round((double)MaxData, 2);
                    MaxDate = flowDataList.Where(r => r.Data == MaxData).FirstOrDefault().Time.ToString();

                    var MinData = flowDataList.Min(r => r.Data);
                    MinValue = Math.Round((double)MinData, 2);
                    MinDate = flowDataList.Where(r => r.Data == MinData).FirstOrDefault().Time.ToString();

                    AverValue = Math.Round((double)flowDataList.Average(r => r.Data), 2);
                }



                var rel = new
                {
                    Code = 1,
                    Message = "数据获取成功",
                    Data = flowDataList,
                    MaxValue = MaxValue.ToString(),
                    MaxDate = MaxDate.ToString(),
                    MinValue = MinValue.ToString(),
                    MinDate = MinDate.ToString(),
                    AverValue = AverValue.ToString()
                };
                return JsonConvert.SerializeObject(rel);
            }
            else
            {
                var rel = new
                {
                    Code = 0,
                    Message = "暂无数据"
                };
                return JsonConvert.SerializeObject(rel);
            }

        }
        #endregion

        #endregion

        #region 历史数据模板
        #region 获取设备某一时间段内的历史数据
        /// <summary>
        /// 获取设备某一时间段内的历史数据
        /// </summary>
        /// <param name="StationID">泵房ID</param>
        /// <param name="EquipID">设备ID</param>
        /// <param name="BeginDate">查询开始时间</param>
        /// <param name="EndDate">查询结束时间</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public string LoadHistoryDataByFilter(int StationID, string EquipID, DateTime BeginDate, DateTime EndDate)
        {
            long lEquipID = Convert.ToInt64(EquipID);
            var viewDeviceInfo = _ISws_RTUInfoService.LoadDeviceAndRtuinfo(StationID).Where(r => r.EquipID == lEquipID).FirstOrDefault();
            if (viewDeviceInfo != null)
            {
                List<DeviceJK01> dataList01 = new List<DeviceJK01>();
                List<DeviceJK02> dataList02 = new List<DeviceJK02>();
                //数据查询
                List<DeviceJK> dataList = new List<DeviceJK>();

                //获取所属区域
                string Section = SimpleFactory.GetParameters(viewDeviceInfo.Partition.ToString());
                string[] strSection = Section.Split(',');

                if (strSection.Length == 2)
                {
                    dataList = _ISws_RTUInfoService.GetMoreMongoHistoryJKData(BeginDate.Year.ToString(), BeginDate.ToString(), EndDate.ToString(), viewDeviceInfo.Rtuid.ToString()).ToList();
                    if (BeginDate.Year != EndDate.Year)
                    {
                        var dataappend = _ISws_RTUInfoService.GetMoreMongoHistoryJKData(EndDate.Year.ToString(), BeginDate.ToString(), EndDate.ToString(), viewDeviceInfo.Rtuid.ToString()).ToList();
                        if (dataappend.Count > 0)
                        {
                            dataList = dataList.Concat(dataappend).ToList();
                        }
                    }

                    if (dataList.Count > 0)
                    {
                        int iAnalog = int.Parse(strSection[0]);
                        int iDigial = int.Parse(strSection[1]);

                        //供水设备
                        if (viewDeviceInfo.iEquipmentType == 1)
                        {
                            //获取进水、出水、设定压力、瞬时流量、累计电量及累计流量
                            List<int> dataid = new List<int>() { iAnalog, iAnalog + 1, iAnalog + 2, iAnalog + 30, iAnalog + 23, iAnalog + 31, iAnalog + 49, iAnalog + 50, iAnalog + 51 };



                            dataList = dataList.OrderByDescending(r => r.UpdateTime).ToList();
                            foreach (var item in dataList)
                            {
                                DeviceJK01 deviceJK01 = new DeviceJK01();
                                deviceJK01.EquipID = item.EquipID;
                                deviceJK01.PressureIN = item.AnalogValues.ContainsKey(dataid[0].ToString()) ? Math.Round(double.Parse((item.AnalogValues[dataid[0].ToString()]).ToString()), 2) : 0;
                                deviceJK01.PressureOut = item.AnalogValues.ContainsKey(dataid[1].ToString()) ? Math.Round(double.Parse((item.AnalogValues[dataid[1].ToString()]).ToString()), 2) : 0;
                                deviceJK01.PressureSet = item.AnalogValues.ContainsKey(dataid[2].ToString()) ? Math.Round(double.Parse((item.AnalogValues[dataid[2].ToString()]).ToString()), 2) : 0;
                                deviceJK01.InstantFlow = item.AnalogValues.ContainsKey(dataid[3].ToString()) ? Math.Round(double.Parse((item.AnalogValues[dataid[3].ToString()]).ToString()), 2) : 0;
                                deviceJK01.TotalPower = item.AnalogValues.ContainsKey(dataid[4].ToString()) ? Math.Round(double.Parse((item.AnalogValues[dataid[4].ToString()]).ToString()), 2) : 0;
                                deviceJK01.TotalFlow = item.AnalogValues.ContainsKey(dataid[5].ToString()) ? Math.Round(double.Parse((item.AnalogValues[dataid[5].ToString()]).ToString()), 2) : 0;
                                deviceJK01.PH = item.AnalogValues.ContainsKey(dataid[6].ToString()) ? Math.Round(double.Parse((item.AnalogValues[dataid[6].ToString()]).ToString()), 2) : 0;
                                deviceJK01.CL = item.AnalogValues.ContainsKey(dataid[7].ToString()) ? Math.Round(double.Parse((item.AnalogValues[dataid[7].ToString()]).ToString()), 2) : 0;
                                deviceJK01.Turbidity = item.AnalogValues.ContainsKey(dataid[8].ToString()) ? Math.Round(double.Parse((item.AnalogValues[dataid[8].ToString()]).ToString()), 2) : 0;
                                deviceJK01.UpdateTime = Convert.ToDateTime(item.UpdateTime).ToString("yyyy-MM-dd HH:mm:ss");
                                dataList01.Add(deviceJK01);
                            }

                            var rel = new
                            {
                                Code = 1,
                                Message = "数据获取成功",
                                lstDeviceJK = dataList01
                            };
                            return JsonConvert.SerializeObject(rel);
                        }

                        //直饮水设备
                        if (viewDeviceInfo.iEquipmentType == 2)
                        {
                            //获取水质相关参数
                            List<int> dataid = new List<int>() { iAnalog + 32, iAnalog + 33, iAnalog + 34, iAnalog + 35, iAnalog + 36, iAnalog + 37, iAnalog + 23 };

                            dataList = dataList.OrderByDescending(r => r.UpdateTime).ToList();
                            foreach (var item in dataList)
                            {
                                DeviceJK02 deviceJK02 = new DeviceJK02();
                                deviceJK02.EquipID = item.EquipID;
                                deviceJK02.PH = item.AnalogValues.ContainsKey(dataid[0].ToString()) ? Math.Round(double.Parse((item.AnalogValues[dataid[0].ToString()]).ToString()), 2) : 0;
                                deviceJK02.CL = item.AnalogValues.ContainsKey(dataid[1].ToString()) ? Math.Round(double.Parse((item.AnalogValues[dataid[1].ToString()]).ToString()), 2) : 0;
                                deviceJK02.Turbidity = item.AnalogValues.ContainsKey(dataid[2].ToString()) ? Math.Round(double.Parse((item.AnalogValues[dataid[2].ToString()]).ToString()), 2) : 0;
                                deviceJK02.ORP = item.AnalogValues.ContainsKey(dataid[3].ToString()) ? Math.Round(double.Parse((item.AnalogValues[dataid[3].ToString()]).ToString()), 2) : 0;
                                deviceJK02.Salinity = item.AnalogValues.ContainsKey(dataid[4].ToString()) ? Math.Round(double.Parse((item.AnalogValues[dataid[4].ToString()]).ToString()), 2) : 0;
                                deviceJK02.Oxygen = item.AnalogValues.ContainsKey(dataid[5].ToString()) ? Math.Round(double.Parse((item.AnalogValues[dataid[5].ToString()]).ToString()), 2) : 0;
                                deviceJK02.Conductivity = item.AnalogValues.ContainsKey(dataid[6].ToString()) ? Math.Round(double.Parse((item.AnalogValues[dataid[6].ToString()]).ToString()), 2) : 0;

                                deviceJK02.UpdateTime = Convert.ToDateTime(item.UpdateTime).ToString("yyyy-MM-dd HH:mm:ss");
                                dataList02.Add(deviceJK02);
                            }

                            var rel = new
                            {
                                Code = 1,
                                Message = "数据获取成功",
                                lstDeviceJK = dataList02
                            };
                            return JsonConvert.SerializeObject(rel);
                        }
                        else
                        {
                            var p = new
                            {
                                Code = 0,
                                Message = "数据获取失败"
                            };
                            return JsonConvert.SerializeObject(p);
                        }

                    }
                    else
                    {
                        var p = new
                        {
                            Code = 0,
                            Message = "数据获取失败"
                        };
                        return JsonConvert.SerializeObject(p);
                    }

                }
                else
                {
                    var p = new
                    {
                        Code = 0,
                        Message = "数据获取失败"
                    };
                    return JsonConvert.SerializeObject(p);
                }

            }

            else
            {
                var rel = new
                {
                    Code = 0,
                    Message = "数据获取失败"
                };
                return JsonConvert.SerializeObject(rel);
            }

        }
        #endregion
        #endregion

        #region 首页
        #region 添加泵房关注及取消关注
        /// <summary>
        /// 添加泵房关注及取消关注
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="StationID"></param>
        /// <param name="IsAttention"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public string IsFocusOnStation(int UserID, int StationID, bool IsAttention)
        {
            var user = _ISysUserService.Query<SysUser>(u => u.UserId == UserID).FirstOrDefault();
            if (IsAttention == true)//关注
            {
                SwsUserStation a = new SwsUserStation();
                a.StationId = StationID;
                a.UserId = UserID;
                a.FocusOn = true;
                if (user.IsAdmin == true)
                {

                    try
                    {
                        _ISws_UserStationService.Insert<SwsUserStation>(a);
                        var rel = new
                        {
                            Code = 1,
                            Message = "数据更新成功"
                        };
                        return JsonConvert.SerializeObject(rel);
                    }
                    catch (Exception e)
                    {
                        var rel = new
                        {
                            Code = 0,
                            Message = "数据更新失败"
                        };
                        return JsonConvert.SerializeObject(rel);
                    }
                }
                else
                {
                    try
                    {
                        _ISws_UserStationService.Update<SwsUserStation>(a);
                        var rel = new
                        {
                            Code = 1,
                            Message = "数据更新成功"
                        };
                        return JsonConvert.SerializeObject(rel);

                    }
                    catch (Exception)
                    {
                        var rel = new
                        {
                            Code = 0,
                            Message = "数据更新失败"
                        };
                        return JsonConvert.SerializeObject(rel);
                    }
                }
            }
            else
            {//取消
                if (user.IsAdmin == true)
                {
                    SwsUserStation a = new SwsUserStation();
                    a.StationId = StationID;
                    a.UserId = UserID;
                    a.FocusOn = true;
                    try
                    {
                        _ISws_UserStationService.Delete<SwsUserStation>(a);
                        var rel = new
                        {
                            Code = 1,
                            Message = "数据更新成功"
                        };
                        return JsonConvert.SerializeObject(rel);
                    }
                    catch (Exception e)
                    {
                        var rel = new
                        {
                            Code = 0,
                            Message = "数据更新失败"
                        };
                        return JsonConvert.SerializeObject(rel);
                    }
                }
                else
                {
                    SwsUserStation a = new SwsUserStation();
                    a.StationId = StationID;
                    a.UserId = UserID;
                    a.FocusOn = false;
                    try
                    {
                        _ISws_UserStationService.Update<SwsUserStation>(a);
                        var rel = new
                        {
                            Code = 1,
                            Message = "数据更新成功"
                        };
                        return JsonConvert.SerializeObject(rel);
                    }
                    catch (Exception e)
                    {
                        var rel = new
                        {
                            Code = 0,
                            Message = "数据更新失败"
                        };
                        return JsonConvert.SerializeObject(rel);
                    }

                }
            }

        }
        #endregion

        #region 获取该用户下所有的泵站信息
        /// <summary>
        /// 获取该用户下所有的泵站信息
        /// </summary>
        /// <param name="UserID">用户ID</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public string LoadAllStationsByUserID(int UserID)
        {
            var lstSwsUserStation = _ISws_UserStationService.LoadUserStationInfo(UserID).ToList();
            if (lstSwsUserStation.Count() > 0)
            {
                List<StationJK_SY> lstStationJK = new List<StationJK_SY>();
                foreach (var USItem in lstSwsUserStation)
                {
                    StationJK_SY station = new StationJK_SY();
                    station.StationID = USItem.StationID;
                    station.StationName = USItem.StationName;
                    station.FocusOn = USItem.FocusOn;

                    List<DeviceJK_SY> lstDeviceJK = new List<DeviceJK_SY>();
                    //设备监控信息
                    var lstSwsRTUInfo = _ISws_RTUInfoService.LoadDeviceAndRtuinfo(USItem.StationID).ToList();
                    if (lstSwsRTUInfo.Count() > 0)
                    {
                        foreach (var item in lstSwsRTUInfo)
                        {
                            //获取实时监控数据
                            var listResulr = _ISws_RTUInfoService.GetMongoJKData(item.Rtuid);

                            DeviceJK_SY deviceJK = new DeviceJK_SY();
                            deviceJK.EquipID = item.EquipID;
                            deviceJK.RTUID = item.Rtuid;
                            deviceJK.EquipmentType = item.EquipmentType;
                            deviceJK.DeviceType = item.DeviceType;
                            deviceJK.DeviceType_ZYS = item.DeviceType_ZYS;
                            deviceJK.Partition = item.Partition;
                            deviceJK.DeviceName = item.DeviceName;

                            string Section = SimpleFactory.GetParameters(item.Partition.ToString());
                            string[] strSection = Section.Split(',');
                            if (strSection.Length == 2)
                            {
                                int iAnalog = int.Parse(strSection[0]);
                                List<int> dataid = new List<int>() { iAnalog, iAnalog + 1, iAnalog + 2, iAnalog + 30 };
                                if (listResulr != null)
                                {
                                    deviceJK.PressureIN = listResulr.AnalogValues.ContainsKey(dataid[0].ToString()) ? Math.Round(double.Parse((listResulr.AnalogValues[dataid[0].ToString()]).ToString()), 2) : 0;
                                    deviceJK.PressureOut = listResulr.AnalogValues.ContainsKey(dataid[1].ToString()) ? Math.Round(double.Parse((listResulr.AnalogValues[dataid[1].ToString()]).ToString()), 2) : 0;
                                    deviceJK.PressureSet = listResulr.AnalogValues.ContainsKey(dataid[2].ToString()) ? Math.Round(double.Parse((listResulr.AnalogValues[dataid[2].ToString()]).ToString()), 2) : 0;
                                    deviceJK.InstantFlow = listResulr.AnalogValues.ContainsKey(dataid[3].ToString()) ? Math.Round(double.Parse((listResulr.AnalogValues[dataid[3].ToString()]).ToString()), 2) : 0;
                                    deviceJK.UpdateTime = Convert.ToDateTime(listResulr.UpdateTime).ToString("yyyy-MM-dd HH:mm:ss");
                                }
                                else
                                {
                                    deviceJK.PressureIN = 0;
                                    deviceJK.PressureOut = 0;
                                    deviceJK.PressureSet = 0;
                                    deviceJK.InstantFlow = 0;
                                    deviceJK.UpdateTime = "---";
                                }


                            }

                            lstDeviceJK.Add(deviceJK);
                        }
                    }

                    station.lstDeviceJK = lstDeviceJK;
                    lstStationJK.Add(station);

                }


                lstStationJK = lstStationJK.OrderByDescending(r => r.FocusOn).ToList();


                var rel = new
                {
                    Code = 1,
                    Message = "数据获取成功",
                    Data = lstStationJK,
                    Count = lstStationJK.Count()
                };
                return JsonConvert.SerializeObject(rel);
            }
            else
            {
                var rel = new
                {
                    Code = 0,
                    Message = "数据获取失败"
                };
                return JsonConvert.SerializeObject(rel);
            }

        }
        //获取泵房下所有设备的监控信息
        [HttpGet]
        [Authorize]
        public string LoadStationsDataByID(int StationID, int UserID)
        {
            var lstSwsUserStation = _ISws_UserStationService.LoadUserStationInfo(UserID).ToList().Where(r => r.StationID == StationID).FirstOrDefault();
            StationJK_SY station = new StationJK_SY();
            station.StationID = StationID;
            station.StationName = lstSwsUserStation.StationName;
            station.FocusOn = lstSwsUserStation.FocusOn;

            List<DeviceJK_SY> lstDeviceJK = new List<DeviceJK_SY>();
            //设备监控信息
            var lstSwsRTUInfo = _ISws_RTUInfoService.LoadDeviceAndRtuinfo(StationID).ToList();
            if (lstSwsRTUInfo.Count() > 0)
            {
                foreach (var item in lstSwsRTUInfo)
                {
                    //获取实时监控数据
                    var listResulr = _ISws_RTUInfoService.GetMongoJKData(item.Rtuid);

                    DeviceJK_SY deviceJK = new DeviceJK_SY();
                    deviceJK.EquipID = item.EquipID;
                    deviceJK.RTUID = item.Rtuid;
                    deviceJK.EquipmentType = item.EquipmentType;
                    deviceJK.DeviceType = item.DeviceType;
                    deviceJK.DeviceType_ZYS = item.DeviceType_ZYS;
                    deviceJK.Partition = item.Partition;
                    deviceJK.DeviceName = item.DeviceName;

                    string Section = SimpleFactory.GetParameters(item.Partition.ToString());
                    string[] strSection = Section.Split(',');
                    if (strSection.Length == 2)
                    {
                        int iAnalog = int.Parse(strSection[0]);
                        List<int> dataid = new List<int>() { iAnalog, iAnalog + 1, iAnalog + 2, iAnalog + 30 };
                        if (listResulr != null)
                        {
                            deviceJK.PressureIN = listResulr.AnalogValues.ContainsKey(dataid[0].ToString()) ? Math.Round(double.Parse((listResulr.AnalogValues[dataid[0].ToString()]).ToString()), 2) : 0;
                            deviceJK.PressureOut = listResulr.AnalogValues.ContainsKey(dataid[1].ToString()) ? Math.Round(double.Parse((listResulr.AnalogValues[dataid[1].ToString()]).ToString()), 2) : 0;
                            deviceJK.PressureSet = listResulr.AnalogValues.ContainsKey(dataid[2].ToString()) ? Math.Round(double.Parse((listResulr.AnalogValues[dataid[2].ToString()]).ToString()), 2) : 0;
                            deviceJK.InstantFlow = listResulr.AnalogValues.ContainsKey(dataid[3].ToString()) ? Math.Round(double.Parse((listResulr.AnalogValues[dataid[3].ToString()]).ToString()), 2) : 0;
                            deviceJK.UpdateTime = Convert.ToDateTime(listResulr.UpdateTime).ToString("yyyy-MM-dd HH:mm:ss");
                        }
                        else
                        {
                            deviceJK.PressureIN = 0;
                            deviceJK.PressureOut = 0;
                            deviceJK.PressureSet = 0;
                            deviceJK.InstantFlow = 0;
                            deviceJK.UpdateTime = "---";
                        }


                    }

                    lstDeviceJK.Add(deviceJK);
                }
                station.lstDeviceJK = lstDeviceJK;
                var rel = new
                {
                    Code = 1,
                    Message = "数据获取成功",
                    Data = station
                };
                return JsonConvert.SerializeObject(rel);
            }
            else
            {
                var rel = new
                {
                    Code = 0,
                    Message = "数据获取失败",
                    Data = station
                };
                return JsonConvert.SerializeObject(rel);
            }

        }
        #endregion

        #region 获取该用户下所有关注的泵站信息
        /// <summary>
        /// 获取该用户下所有关注的泵站信息
        /// </summary>
        /// <param name="UserID">用户ID</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public string LoadAllFocusOnStations(int UserID)
        {
            var lstSwsUserStation = _ISws_UserStationService.LoadUserStationInfo(UserID).Where(r => r.FocusOn == true).ToList();
            if (lstSwsUserStation.Count() > 0)
            {
                List<StationJK_SY> lstStationJK = new List<StationJK_SY>();
                foreach (var USItem in lstSwsUserStation)
                {
                    StationJK_SY station = new StationJK_SY();
                    station.StationID = USItem.StationID;
                    station.StationName = USItem.StationName;
                    station.FocusOn = USItem.FocusOn;

                    List<DeviceJK_SY> lstDeviceJK = new List<DeviceJK_SY>();
                    //设备监控信息
                    var lstSwsRTUInfo = _ISws_RTUInfoService.LoadDeviceAndRtuinfo(USItem.StationID).ToList();
                    if (lstSwsRTUInfo.Count() > 0)
                    {
                        foreach (var item in lstSwsRTUInfo)
                        {
                            //获取实时监控数据
                            var listResulr = _ISws_RTUInfoService.GetMongoJKData(item.Rtuid);

                            DeviceJK_SY deviceJK = new DeviceJK_SY();
                            deviceJK.EquipID = item.EquipID;
                            deviceJK.RTUID = item.Rtuid;
                            deviceJK.EquipmentType = item.EquipmentType;
                            deviceJK.DeviceType = item.DeviceType;
                            deviceJK.Partition = item.Partition;
                            deviceJK.DeviceName = item.DeviceName;

                            string Section = SimpleFactory.GetParameters(item.Partition.ToString());
                            string[] strSection = Section.Split(',');
                            if (strSection.Length == 2)
                            {
                                int iAnalog = int.Parse(strSection[0]);
                                List<int> dataid = new List<int>() { iAnalog, iAnalog + 1, iAnalog + 2, iAnalog + 30 };
                                if (listResulr != null)
                                {
                                    deviceJK.PressureIN = listResulr.AnalogValues.ContainsKey(dataid[0].ToString()) ? Math.Round(double.Parse((listResulr.AnalogValues[dataid[0].ToString()]).ToString()), 2) : 0;
                                    deviceJK.PressureOut = listResulr.AnalogValues.ContainsKey(dataid[1].ToString()) ? Math.Round(double.Parse((listResulr.AnalogValues[dataid[1].ToString()]).ToString()), 2) : 0;
                                    deviceJK.PressureSet = listResulr.AnalogValues.ContainsKey(dataid[2].ToString()) ? Math.Round(double.Parse((listResulr.AnalogValues[dataid[2].ToString()]).ToString()), 2) : 0;
                                    deviceJK.InstantFlow = listResulr.AnalogValues.ContainsKey(dataid[3].ToString()) ? Math.Round(double.Parse((listResulr.AnalogValues[dataid[3].ToString()]).ToString()), 2) : 0;
                                    deviceJK.UpdateTime = Convert.ToDateTime(listResulr.UpdateTime).ToString("yyyy-MM-dd HH:mm:ss");
                                }
                                else
                                {
                                    deviceJK.PressureIN = 0;
                                    deviceJK.PressureOut = 0;
                                    deviceJK.PressureSet = 0;
                                    deviceJK.InstantFlow = 0;
                                    deviceJK.UpdateTime = "---";
                                }
                            }

                            lstDeviceJK.Add(deviceJK);
                        }
                    }

                    station.lstDeviceJK = lstDeviceJK;
                    lstStationJK.Add(station);
                }

                var rel = new
                {
                    Code = 1,
                    Message = "数据获取成功",
                    Data = lstStationJK,
                    Count = lstStationJK.Count()
                };
                return JsonConvert.SerializeObject(rel);
            }
            else
            {
                var rel = new
                {
                    Code = 0,
                    Message = "数据获取失败"
                };
                return JsonConvert.SerializeObject(rel);
            }

        }
        #endregion

        #region 获取该用户下所有的掉线设备信息
        /// <summary>
        /// 获取该用户下所有的掉线设备信息
        /// </summary>
        /// <param name="UserID">用户ID</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public string LoadAllOfflineStations(int UserID)
        {
            //掉线时长获取
            int timevalue = int.Parse(StaticConstraint.TimeValue);
            var offlinetime = DateTime.Now.AddMinutes(-timevalue);

            var lstSwsUserStation = _ISws_UserStationService.LoadUserStationInfo(UserID).ToList();
            if (lstSwsUserStation.Count() > 0)
            {
                List<StationJK_SY> lstStationJK = new List<StationJK_SY>();
                foreach (var USItem in lstSwsUserStation)
                {
                    StationJK_SY station = new StationJK_SY();
                    station.StationID = USItem.StationID;
                    station.StationName = USItem.StationName;
                    station.FocusOn = USItem.FocusOn;

                    List<DeviceJK_SY> lstDeviceJK = new List<DeviceJK_SY>();
                    //设备监控信息
                    var lstSwsRTUInfo = _ISws_RTUInfoService.LoadDeviceAndRtuinfo(USItem.StationID).ToList();
                    if (lstSwsRTUInfo.Count() > 0)
                    {
                        foreach (var item in lstSwsRTUInfo)
                        {
                            //获取实时监控数据
                            var listResulr = _ISws_RTUInfoService.GetMongoJKData(item.Rtuid);
                            if (listResulr == null || listResulr.UpdateTime < offlinetime)
                            {
                                DeviceJK_SY deviceJK = new DeviceJK_SY();
                                deviceJK.EquipID = item.EquipID;
                                deviceJK.RTUID = item.Rtuid;
                                deviceJK.EquipmentType = item.EquipmentType;
                                deviceJK.DeviceType = item.DeviceType;
                                deviceJK.Partition = item.Partition;
                                deviceJK.DeviceName = item.DeviceName;

                                string Section = SimpleFactory.GetParameters(item.Partition.ToString());
                                string[] strSection = Section.Split(',');
                                if (strSection.Length == 2)
                                {
                                    int iAnalog = int.Parse(strSection[0]);
                                    List<int> dataid = new List<int>() { iAnalog, iAnalog + 1, iAnalog + 2, iAnalog + 30 };
                                    if (listResulr != null)
                                    {
                                        deviceJK.PressureIN = listResulr.AnalogValues.ContainsKey(dataid[0].ToString()) ? Math.Round(double.Parse((listResulr.AnalogValues[dataid[0].ToString()]).ToString()), 2) : 0;
                                        deviceJK.PressureOut = listResulr.AnalogValues.ContainsKey(dataid[1].ToString()) ? Math.Round(double.Parse((listResulr.AnalogValues[dataid[1].ToString()]).ToString()), 2) : 0;
                                        deviceJK.PressureSet = listResulr.AnalogValues.ContainsKey(dataid[2].ToString()) ? Math.Round(double.Parse((listResulr.AnalogValues[dataid[2].ToString()]).ToString()), 2) : 0;
                                        deviceJK.InstantFlow = listResulr.AnalogValues.ContainsKey(dataid[3].ToString()) ? Math.Round(double.Parse((listResulr.AnalogValues[dataid[3].ToString()]).ToString()), 2) : 0;
                                        deviceJK.UpdateTime = Convert.ToDateTime(listResulr.UpdateTime).ToString("yyyy-MM-dd HH:mm:ss");


                                    }
                                    else
                                    {
                                        deviceJK.PressureIN = 0;
                                        deviceJK.PressureOut = 0;
                                        deviceJK.PressureSet = 0;
                                        deviceJK.InstantFlow = 0;
                                        deviceJK.UpdateTime = "---";
                                    }
                                }

                                lstDeviceJK.Add(deviceJK);
                            }
                        }
                    }

                    station.lstDeviceJK = lstDeviceJK;
                    if (lstDeviceJK.Count > 0)
                    {
                        lstStationJK.Add(station);
                    }

                }

                var rel = new
                {
                    Code = 1,
                    Message = "数据获取成功",
                    Data = lstStationJK,
                    Count = lstStationJK.Count()
                };
                return JsonConvert.SerializeObject(rel);
            }
            else
            {
                var rel = new
                {
                    Code = 0,
                    Message = "数据获取失败"
                };
                return JsonConvert.SerializeObject(rel);
            }

        }
        #endregion

        #region 根据设备名称/通讯编号/泵房名称查询相关泵房信息
        /// <summary>
        /// 根据设备名称/通讯编号/泵房名称查询相关泵房信息
        /// </summary>
        /// <param name="UserID">用户ID</param>
        /// <param name="StrFilter">查询条件</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public string LoadAllFilterStations(int UserID, string StrFilter)
        {
            var lstSwsStation = _ISws_UserStationService.LoadUserStationAndDeviceInfo(UserID).ToList();
            if (lstSwsStation.Count() > 0)
            {
                var lstSwsUserStation = _ISws_UserStationService.LoadUserStationAndDeviceInfo(UserID).
                    Where(r => (r.DeviceName != null && r.DeviceName.Contains(StrFilter)) || (r.DeviceID != null && r.DeviceID.Contains(StrFilter)) || (r.StationName != null && r.StationName.Contains(StrFilter))).
                    GroupBy(r => new { r.StationID, r.StationName, r.FocusOn }).Select(g => g.First()).ToList();
                if (lstSwsUserStation.Count() > 0)
                {
                    List<StationJK_SY> lstStationJK = new List<StationJK_SY>();
                    foreach (var USItem in lstSwsUserStation)
                    {
                        StationJK_SY station = new StationJK_SY();
                        station.StationID = USItem.StationID;
                        station.StationName = USItem.StationName;
                        station.FocusOn = USItem.FocusOn;

                        List<DeviceJK_SY> lstDeviceJK = new List<DeviceJK_SY>();
                        //设备监控信息
                        var lstSwsRTUInfo = _ISws_RTUInfoService.LoadDeviceAndRtuinfo(USItem.StationID).ToList();
                        if (lstSwsRTUInfo.Count() > 0)
                        {
                            foreach (var item in lstSwsRTUInfo)
                            {
                                //获取实时监控数据
                                var listResulr = _ISws_RTUInfoService.GetMongoJKData(item.Rtuid);

                                DeviceJK_SY deviceJK = new DeviceJK_SY();
                                deviceJK.EquipID = item.EquipID;
                                deviceJK.RTUID = item.Rtuid;
                                deviceJK.EquipmentType = item.EquipmentType;
                                deviceJK.DeviceType = item.DeviceType;
                                deviceJK.Partition = item.Partition;
                                deviceJK.DeviceName = item.DeviceName;

                                string Section = SimpleFactory.GetParameters(item.Partition.ToString());
                                string[] strSection = Section.Split(',');
                                if (strSection.Length == 2)
                                {
                                    int iAnalog = int.Parse(strSection[0]);
                                    List<int> dataid = new List<int>() { iAnalog, iAnalog + 1, iAnalog + 2, iAnalog + 30 };
                                    if (listResulr != null)
                                    {
                                        deviceJK.PressureIN = listResulr.AnalogValues.ContainsKey(dataid[0].ToString()) ? Math.Round(double.Parse((listResulr.AnalogValues[dataid[0].ToString()]).ToString()), 2) : 0;
                                        deviceJK.PressureOut = listResulr.AnalogValues.ContainsKey(dataid[1].ToString()) ? Math.Round(double.Parse((listResulr.AnalogValues[dataid[1].ToString()]).ToString()), 2) : 0;
                                        deviceJK.PressureSet = listResulr.AnalogValues.ContainsKey(dataid[2].ToString()) ? Math.Round(double.Parse((listResulr.AnalogValues[dataid[2].ToString()]).ToString()), 2) : 0;
                                        deviceJK.InstantFlow = listResulr.AnalogValues.ContainsKey(dataid[3].ToString()) ? Math.Round(double.Parse((listResulr.AnalogValues[dataid[3].ToString()]).ToString()), 2) : 0;
                                        deviceJK.UpdateTime = Convert.ToDateTime(listResulr.UpdateTime).ToString("yyyy-MM-dd HH:mm:ss");
                                    }
                                    else
                                    {
                                        deviceJK.PressureIN = 0;
                                        deviceJK.PressureOut = 0;
                                        deviceJK.PressureSet = 0;
                                        deviceJK.InstantFlow = 0;
                                        deviceJK.UpdateTime = "---";
                                    }


                                }
                                lstDeviceJK.Add(deviceJK);
                            }
                        }

                        station.lstDeviceJK = lstDeviceJK;
                        lstStationJK.Add(station);
                    }

                    var rel = new
                    {
                        Code = 1,
                        Message = "数据获取成功",
                        Data = lstStationJK,
                        Count = lstStationJK.Count()
                    };
                    return JsonConvert.SerializeObject(rel);
                }
                else
                {
                    var rel = new
                    {
                        Code = 0,
                        Message = "数据获取失败"
                    };
                    return JsonConvert.SerializeObject(rel);
                }
            }
            else
            {
                var rel = new
                {
                    Code = 0,
                    Message = "数据获取失败"
                };
                return JsonConvert.SerializeObject(rel);
            }

        }
        #endregion
        #endregion

        #region 设备及泵房详情模块
        #region 设备详情
        /// <summary>
        /// 设备详情
        /// </summary>
        /// <param name="EquipID">设备ID</param>
        /// <param name="EquipmentType">设备类型</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public string LoadDeviceInfoByID(string EquipID, int EquipmentType)
        {
            long lEquipID = Convert.ToInt64(EquipID);

            #region 获取供水设备详情
            if (EquipmentType == 1)
            {
                var swsDeviceInfo01 = _ISws_DeviceInfo01Service.LoadSwsDeviceInfo01Info(lEquipID).FirstOrDefault();
                if (swsDeviceInfo01 != null)
                {
                    var rel = new
                    {
                        Code = 1,
                        Message = "数据获取成功",
                        Data = swsDeviceInfo01
                    };
                    return JsonConvert.SerializeObject(rel);
                }
                else
                {
                    var rel = new
                    {
                        Code = 0,
                        Message = "数据获取失败"
                    };
                    return JsonConvert.SerializeObject(rel);
                }

            }
            #endregion

            #region 获取直饮水设备详情
            else if (EquipmentType == 2)
            {
                var swsDeviceInfo02 = _ISws_DeviceInfo02Service.LoadSwsDeviceInfo02Info(lEquipID).FirstOrDefault();
                if (swsDeviceInfo02 != null)
                {
                    var rel = new
                    {
                        Code = 1,
                        Message = "数据获取成功",
                        Data = swsDeviceInfo02
                    };
                    return JsonConvert.SerializeObject(rel);
                }
                else
                {
                    var rel = new
                    {
                        Code = 0,
                        Message = "数据获取失败"
                    };
                    return JsonConvert.SerializeObject(rel);
                }

            }
            else
            {
                var rel = new
                {
                    Code = 0,
                    Message = "数据获取失败"
                };
                return JsonConvert.SerializeObject(rel);
            }
            #endregion

        }
        #endregion

        #region 泵房详情
        /// <summary>
        /// 泵房详情
        /// </summary>
        /// <param name="StationID">泵房ID</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public string LoadStationInfoByID(int StationID)
        {
            var swsStation = _ISws_StationService.LoadStationInfo(StationID).FirstOrDefault();
            if (swsStation != null)
            {
                StationInfo stationInfo = new StationInfo();
                stationInfo.StationId = swsStation.StationID;
                stationInfo.StationNum = swsStation.StationNum;
                stationInfo.StationName = swsStation.StationName;
                stationInfo.StaitonTypeName = swsStation.StaitonTypeName;
                stationInfo.Lng_Lat = swsStation.Lng + "," + swsStation.Lat;
                //stationInfo.StationPostion = swsStation.StationPostion;
                stationInfo.InstallationDate = swsStation.InstallationDate;
                stationInfo.AcceptanceDate = swsStation.AcceptanceDate;
                stationInfo.QualityEndDate = swsStation.QualityEndDate;
                stationInfo.InspectionCycle = swsStation.InspectionCycle;
                stationInfo.MaintenanceCycle = swsStation.MaintenanceCycle;
                stationInfo.CleaningCycle = swsStation.CleaningCycle;
                stationInfo.WaterTankNum = swsStation.WaterTankNum;
                stationInfo.WaterTankPublic = swsStation.WaterTankNum == 1 ? "是" : "否";

                var rel = new
                {
                    Code = 1,
                    Message = "数据获取成功",
                    Data = stationInfo
                };
                return JsonConvert.SerializeObject(rel);
            }
            else
            {
                var rel = new
                {
                    Code = 0,
                    Message = "数据获取失败"
                };
                return JsonConvert.SerializeObject(rel);
            }

        }
        #endregion
        #endregion

        #region 报警模块

        #region 获取某套设备的报警历史记录
        /// <summary>
        /// 获取某套设备的报警历史记录
        /// </summary>
        /// <param name="EquipID">设备ID</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public string LoadEventHistoryInfoByEquipID(string EquipID)
        {
            long lEquipID = Convert.ToInt64(EquipID);
            List<EventHistoryInfo> lsteventHistoryInfo = new List<EventHistoryInfo>();
            var lstAlarmDeviceInfo = _ISws_EventHistoryService.LoadEventHistoryInfo(lEquipID).ToList();
            if (lstAlarmDeviceInfo.Count() > 0)
            {
                var data = lstAlarmDeviceInfo.Select(x => new
                {
                    x.EquipID,
                    x.DeviceID,
                    x.DeviceName,
                    x.EventSource,
                    x.EventMessage,
                    x.EventLevel,
                    BeginTime = x.EventTime,
                    EndTime = x.EndDate,
                });
                ////循环其他报警信息
                //foreach (var item in lstAlarmDeviceInfo)
                //{
                //    EventHistoryInfo eventHistoryInfo = new EventHistoryInfo();
                //    eventHistoryInfo.EquipID = item.EquipID;
                //    eventHistoryInfo.DeviceID = item.DeviceID;
                //    eventHistoryInfo.DeviceName = item.DeviceName;
                //    eventHistoryInfo.EventSource = item.EventSource;
                //    eventHistoryInfo.EventMessage = item.EventMessage;
                //    eventHistoryInfo.EventLevel = item.EventLevel;
                //    eventHistoryInfo.BeginTime = item.EventTime;
                //    eventHistoryInfo.EndTime = item.EndDate.ToString();

                //    lsteventHistoryInfo.Add(eventHistoryInfo);
                //}


                var rel = new
                {
                    Code = 1,
                    Message = "数据获取成功",
                    Data = data
                };
                return JsonConvert.SerializeObject(rel);
            }
            else
            {
                var rel = new
                {
                    Code = 0,
                    Message = "暂无数据"
                };
                return JsonConvert.SerializeObject(rel);
            }

        }

        /// <summary>
        /// 获取某套设备的报警历史记录分页
        /// </summary>
        /// <param name="EquipID">设备ID</param>
        /// <param name="pageIndex">分页</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public string LoadEventHistoryInfoByPage(string EquipID, int pageIndex)
        {
            long lEquipID = Convert.ToInt64(EquipID);
            List<EventHistoryInfo> lsteventHistoryInfo = new List<EventHistoryInfo>();
            int Totalcount = 0;
            var lstAlarmDeviceInfo = _ISws_EventHistoryService.LoadEventHistoryInfoByPage(lEquipID, pageIndex, 100, ref Totalcount).ToList();
            if (lstAlarmDeviceInfo.Count() > 0)
            {
                var data = lstAlarmDeviceInfo.Select(x => new
                {
                    x.EquipID,
                    x.DeviceID,
                    x.DeviceName,
                    x.EventSource,
                    x.EventMessage,
                    x.EventLevel,
                    BeginTime = x.EventTime,
                    EndTime = x.EndDate,
                });

                ////循环其他报警信息
                //foreach (var item in lstAlarmDeviceInfo)
                //{
                //    EventHistoryInfo eventHistoryInfo = new EventHistoryInfo();
                //    eventHistoryInfo.EquipID = item.EquipID;
                //    eventHistoryInfo.DeviceID = item.DeviceID;
                //    eventHistoryInfo.DeviceName = item.DeviceName;
                //    eventHistoryInfo.EventSource = item.EventSource;
                //    eventHistoryInfo.EventMessage = item.EventMessage;
                //    eventHistoryInfo.EventLevel = item.EventLevel;
                //    eventHistoryInfo.BeginTime = item.EventTime;
                //    eventHistoryInfo.EndTime = item.EndDate.ToString();

                //    lsteventHistoryInfo.Add(eventHistoryInfo);
                //}


                var rel = new
                {
                    Code = 1,
                    Message = "数据获取成功",
                    Data = data
                };
                return JsonConvert.SerializeObject(rel);
            }
            else
            {
                var rel = new
                {
                    Code = 0,
                    Message = "暂无数据"
                };
                return JsonConvert.SerializeObject(rel);
            }

        }

        //public string LoadEventHistoryInfoByEquipID(string EquipID)
        //{
        //    long lEquipID = Convert.ToInt64(EquipID);
        //    List<EventHistoryInfo> lsteventHistoryInfo = new List<EventHistoryInfo>();
        //    var lstAlarmDeviceInfo = _ISws_EventHistoryService.LoadEventHistoryInfo(lEquipID).ToList();
        //    if (lstAlarmDeviceInfo.Count()>0)
        //    {
        //        //先获取第一条报警信息
        //        EventHistoryInfo eventHistoryInfo = new EventHistoryInfo();
        //        var AlarmDeviceInfo_First = lstAlarmDeviceInfo.First();

        //        eventHistoryInfo.EquipID = AlarmDeviceInfo_First.EquipID;
        //        eventHistoryInfo.DeviceID = AlarmDeviceInfo_First.DeviceID;
        //        eventHistoryInfo.DeviceName = AlarmDeviceInfo_First.DeviceName;
        //        eventHistoryInfo.EventSource = AlarmDeviceInfo_First.EventSource;
        //        eventHistoryInfo.EventMessage = AlarmDeviceInfo_First.EventMessage;
        //        //eventHistoryInfo.EventLevel = System.Enum.GetName(typeof(EventLevelEnum), int.Parse(AlarmDeviceInfo_First.EventLevel));
        //        eventHistoryInfo.EventLevel = AlarmDeviceInfo_First.EventLevel;

        //        if (AlarmDeviceInfo_First.State==1)
        //        {
        //            eventHistoryInfo.BeginTime = AlarmDeviceInfo_First.EventTime;
        //        }
        //        else
        //        {
        //            eventHistoryInfo.EndTime = AlarmDeviceInfo_First.EventTime;
        //        }

        //         lstAlarmDeviceInfo.Remove(AlarmDeviceInfo_First);
        //        //循环其他报警信息
        //        foreach (var item in lstAlarmDeviceInfo)
        //        {
        //            if(item.EventSource== eventHistoryInfo.EventSource)
        //            {
        //                if (item.State == 0)
        //                {
        //                    eventHistoryInfo.EndTime = item.EventTime;
        //                    lsteventHistoryInfo.Add(eventHistoryInfo);
        //                }
        //                else
        //                {
        //                    eventHistoryInfo = new EventHistoryInfo();
        //                    eventHistoryInfo.EquipID = item.EquipID;
        //                    eventHistoryInfo.DeviceID = item.DeviceID;
        //                    eventHistoryInfo.DeviceName = item.DeviceName;
        //                    eventHistoryInfo.EventSource = item.EventSource;
        //                    eventHistoryInfo.EventMessage = item.EventMessage;
        //                    eventHistoryInfo.EventLevel = item.EventLevel;

        //                    if (item.State == 1)
        //                    {
        //                        eventHistoryInfo.BeginTime = item.EventTime;
        //                    }
        //                    else
        //                    {
        //                        eventHistoryInfo.EndTime = item.EventTime;
        //                    }
        //                }

        //            }
        //            else
        //            {
        //                eventHistoryInfo = new EventHistoryInfo();
        //                eventHistoryInfo.EquipID = item.EquipID;
        //                eventHistoryInfo.DeviceID = item.DeviceID;
        //                eventHistoryInfo.DeviceName = item.DeviceName;
        //                eventHistoryInfo.EventSource = item.EventSource;
        //                eventHistoryInfo.EventMessage = item.EventMessage;
        //                eventHistoryInfo.EventLevel = item.EventLevel;

        //                if (item.State == 1)
        //                {
        //                    eventHistoryInfo.BeginTime = item.EventTime;
        //                }
        //                else
        //                {
        //                    eventHistoryInfo.EndTime = item.EventTime;
        //                }
        //            }

        //        }


        //        var rel = new
        //        {
        //            Code = 1,
        //            Message = "数据获取成功",
        //            Data = lsteventHistoryInfo
        //        };
        //        return JsonConvert.SerializeObject(rel);
        //    }
        //    else
        //    {
        //        var rel = new
        //        {
        //            Code = 0,
        //            Message = "数据获取失败"
        //        };
        //        return JsonConvert.SerializeObject(rel);
        //    }

        //}
        #endregion

        #region 获取某个时间段内设备的报警历史记录
        /// <summary>
        /// 获取某个时间段内设备的报警历史记录
        /// </summary>
        /// <param name="EquipID">设备ID</param>
        /// <param name="BeginDate">查询开始时间</param>
        /// <param name="EndDate">查询结束时间</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public string LoadEventHistoryInfoByFilter(string EquipID, string BeginDate, string EndDate)
        {
            long lEquipID = Convert.ToInt64(EquipID);
            DateTime dat_BeginDate = Convert.ToDateTime(BeginDate);
            DateTime dat_EndDate = Convert.ToDateTime(EndDate);
            List<EventHistoryInfo> lsteventHistoryInfo = new List<EventHistoryInfo>();
            var lstAlarmDeviceInfo = _ISws_EventHistoryService.LoadEventHistoryInfo(lEquipID).Where(r => r.EventTime >= dat_BeginDate && r.EventTime <= dat_EndDate).ToList();
            if (lstAlarmDeviceInfo.Count() > 0)
            {
                var data = lstAlarmDeviceInfo.Select(x => new
                {
                    x.EquipID,
                    x.DeviceID,
                    x.DeviceName,
                    x.EventSource,
                    x.EventMessage,
                    x.EventLevel,
                    BeginTime = x.EventTime,
                    EndTime = x.EndDate
                });
                ////循环其他报警信息
                //foreach (var item in lstAlarmDeviceInfo)
                //{
                //    EventHistoryInfo eventHistoryInfo = new EventHistoryInfo();
                //    eventHistoryInfo.EquipID = item.EquipID;
                //    eventHistoryInfo.DeviceID = item.DeviceID;
                //    eventHistoryInfo.DeviceName = item.DeviceName;
                //    eventHistoryInfo.EventSource = item.EventSource;
                //    eventHistoryInfo.EventMessage = item.EventMessage;
                //    eventHistoryInfo.EventLevel = item.EventLevel;
                //    eventHistoryInfo.BeginTime = item.EventTime;
                //    eventHistoryInfo.EndTime = item.EndDate.ToString();

                //    lsteventHistoryInfo.Add(eventHistoryInfo);
                //}


                var rel = new
                {
                    Code = 1,
                    Message = "数据获取成功",
                    Data = data
                };
                return JsonConvert.SerializeObject(rel);
            }
            else
            {
                var rel = new
                {
                    Code = 0,
                    Message = "暂无数据"
                };
                return JsonConvert.SerializeObject(rel);
            }

        }

        #endregion

        #region 获取某套设备某个时间段内的报警次数及报警级别所在百分比
        /// <summary>
        /// 获取某套设备某个时间段内的报警次数及报警级别所在百分比
        /// </summary>
        /// <param name="EquipID">设备ID</param>
        /// <param name="BeginDate">查询开始时间</param>
        /// <param name="EndDate">查询结束时间</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public string LoadEventCountByEquipID(string EquipID, DateTime BeginDate, DateTime EndDate)
        {
            long lEquipID = Convert.ToInt64(EquipID);
            int DiffDay = (EndDate - BeginDate).Days;
            var lstEventLevelCounts = _ISws_EventHistoryService.LoadEventLevelCountsByEquipID(lEquipID, BeginDate, EndDate).ToList();

            //如果查询时间跨年，则按照年统计报警数量
            if (EndDate.Year != BeginDate.Year)
            {
                var lstEventYearCounts = _ISws_EventHistoryService.LoadEventYearCountsByEquipID(lEquipID, BeginDate, EndDate).ToList();
                if (lstEventYearCounts != null && lstEventYearCounts != null)
                {
                    var rel = new
                    {
                        Code = 1,
                        Message = "数据获取成功",
                        lstEventDateCounts = lstEventYearCounts,
                        lstEventLevelCounts = lstEventLevelCounts
                    };
                    return JsonConvert.SerializeObject(rel);
                }
                else
                {
                    var rel = new
                    {
                        Code = 0,
                        Message = "数据获取失败"
                    };
                    return JsonConvert.SerializeObject(rel);
                }
            }

            //如果查询时间跨月且时间差超过30天，则按照月统计报警数量
            else if (DiffDay > 30 && EndDate.Month != BeginDate.Month)
            {
                var lstEventMonthCounts = _ISws_EventHistoryService.LoadEventMonthCountsByEquipID(lEquipID, BeginDate, EndDate).ToList();
                if (lstEventMonthCounts != null && lstEventMonthCounts != null)
                {
                    var rel = new
                    {
                        Code = 1,
                        Message = "数据获取成功",
                        lstEventDateCounts = lstEventMonthCounts,
                        lstEventLevelCounts = lstEventLevelCounts
                    };
                    return JsonConvert.SerializeObject(rel);
                }
                else
                {
                    var rel = new
                    {
                        Code = 0,
                        Message = "数据获取失败"
                    };
                    return JsonConvert.SerializeObject(rel);
                }
            }
            else
            {
                var lstEventDateCounts = _ISws_EventHistoryService.LoadEventDateCountsByEquipID(lEquipID, BeginDate, EndDate).ToList();
                if (lstEventDateCounts != null && lstEventLevelCounts != null)
                {
                    var rel = new
                    {
                        Code = 1,
                        Message = "数据获取成功",
                        lstEventDateCounts = lstEventDateCounts,
                        lstEventLevelCounts = lstEventLevelCounts
                    };
                    return JsonConvert.SerializeObject(rel);
                }
                else
                {
                    var rel = new
                    {
                        Code = 0,
                        Message = "数据获取失败"
                    };
                    return JsonConvert.SerializeObject(rel);
                }
            }

        }
        #endregion

        #endregion

        #region 消息模块

        #region 获取该用户下所有发生报警的设备信息
        /// <summary>
        /// 获取该用户下所有发生报警的设备信息
        /// </summary>
        /// <param name="UserID">用户ID</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public string LoadAllAlarmDevices(int UserID)
        {
            var lstSwsUserStation = _ISws_UserStationService.LoadUserStationInfo(UserID).ToList();
            if (lstSwsUserStation.Count() > 0)
            {
                List<StationJK_Alarm> lstStationJK = new List<StationJK_Alarm>();
                foreach (var USItem in lstSwsUserStation)
                {
                    StationJK_Alarm station = new StationJK_Alarm();
                    station.StationID = USItem.StationID;
                    station.StationName = USItem.StationName;
                    station.FocusOn = USItem.FocusOn;

                    List<AlarmDeviceInfo_SS> lstAlarmDeviceInfo = new List<AlarmDeviceInfo_SS>();
                    //设备监控信息
                    var lstSwsRTUInfo = _ISws_RTUInfoService.LoadAlarmDeviceInfo(USItem.StationID).ToList();
                    if (lstSwsRTUInfo.Count() > 0)
                    {
                        foreach (var item in lstSwsRTUInfo)
                        {
                            //获取实时监控数据
                            //var listResulr = _ISws_RTUInfoService.GetMongoJKData(item.RTUID);

                            AlarmDeviceInfo_SS alarmDeviceInfo = new AlarmDeviceInfo_SS();
                            alarmDeviceInfo.RTUID = item.RTUID;
                            alarmDeviceInfo.Partition = item.Partition;
                            alarmDeviceInfo.DeviceName = item.DeviceName;
                            alarmDeviceInfo.EventSource = item.EventSource;
                            alarmDeviceInfo.EventMessage = item.EventMessage;
                            alarmDeviceInfo.EventLevel = item.EventLevel;
                            alarmDeviceInfo.EventTime = item.EventTime;
                            lstAlarmDeviceInfo.Add(alarmDeviceInfo);
                        }


                        station.lstAlarmDeviceInfo = lstAlarmDeviceInfo;
                        lstStationJK.Add(station);
                    }


                }

                var rel = new
                {
                    Code = 1,
                    Message = "数据获取成功",
                    Data = lstStationJK,
                    Count = lstStationJK.Count()
                };
                return JsonConvert.SerializeObject(rel);
            }
            else
            {
                var rel = new
                {
                    Code = 0,
                    Message = "数据获取失败"
                };
                return JsonConvert.SerializeObject(rel);
            }

        }
        #endregion

        #region 获取所有存在报警的设备信息
        /// <summary>
        /// 获取所有存在报警的设备信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public string LoadCurrentEventInfo()
        {
            var lstAlarmDeviceInfo = _ISws_EventInfoService.LoadCurrentEventInfo().ToList();
            if (lstAlarmDeviceInfo.Count() > 0)
            {
                var rel = new
                {
                    Code = 1,
                    Message = "数据获取成功",
                    Data = lstAlarmDeviceInfo
                };
                return JsonConvert.SerializeObject(rel);
            }
            else
            {
                var rel = new
                {
                    Code = 0,
                    Message = "数据获取失败"
                };
                return JsonConvert.SerializeObject(rel);
            }

        }
        #endregion

        #region 获取停水消息
        /// <summary>
        /// 获取停水消息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public string LoadCutOffMessageInfo()
        {
            var lstSysCutOffMessage = _ISwsCutOffMessageService.Query<SwsCutOffMessage>(r => true).ToList();
            if (lstSysCutOffMessage.Count() > 0)
            {
                var rel = new
                {
                    Code = 1,
                    Message = "数据获取成功",
                    Data = lstSysCutOffMessage
                };
                return JsonConvert.SerializeObject(rel);
            }
            else
            {
                var rel = new
                {
                    Code = 0,
                    Message = "数据获取失败"
                };
                return JsonConvert.SerializeObject(rel);
            }

        }
        #endregion
        #endregion

        #region 设备动画模块
        /// <summary>
        /// 设备动画模块
        /// </summary>
        /// <param name="StationID"></param>
        /// <param name="EquipID"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public string LoadPumpRealTimeData(int StationID, string EquipID)
        {
            long lEquipID = Convert.ToInt64(EquipID);
            var SwsRTUInfo = _ISws_RTUInfoService.LoadDeviceAndRtuinfo(StationID).Where(e => e.EquipID == lEquipID).FirstOrDefault();
            if (SwsRTUInfo != null)
            {
                List<Pump> lstPumps = new List<Pump>();
                //获取实时监控数据
                var listResulr = _ISws_RTUInfoService.GetMongoJKData(SwsRTUInfo.Rtuid);
                string Section = SimpleFactory.GetParameters(SwsRTUInfo.Partition.ToString());
                string[] strSection = Section.Split(',');

                DeviceJK_SS deviceJK = new DeviceJK_SS();
                deviceJK.EquipID = SwsRTUInfo.EquipID;
                deviceJK.RTUID = SwsRTUInfo.Rtuid;
                deviceJK.EquipmentType = SwsRTUInfo.EquipmentType;
                deviceJK.DeviceType = SwsRTUInfo.DeviceType;
                deviceJK.Partition = SwsRTUInfo.Partition;
                deviceJK.DeviceName = SwsRTUInfo.DeviceName;
                //是否在线
                deviceJK.IsOnline = "离线";

                if (listResulr != null)
                {
                    deviceJK.UpdateTime = Convert.ToDateTime(listResulr.UpdateTime).ToString("yyyy-MM-dd HH:mm:ss");


                    #region 供水设备处理
                    if (SwsRTUInfo.iEquipmentType == 1)
                    {
                        int PumpNum = 0;
                        if (!string.IsNullOrEmpty(SwsRTUInfo.PumpNum.ToString()))
                        {
                            PumpNum = SwsRTUInfo.PumpNum;
                        }

                        if (strSection.Length == 2)
                        {
                            int iAnalog = int.Parse(strSection[0]);
                            int iDigial = int.Parse(strSection[1]);


                            //获取进水、出水、设定压力、瞬时流量、累计电量及累计流量、液位高度
                            List<int> dataid = new List<int>() { iAnalog, iAnalog + 1, iAnalog + 2, iAnalog + 30, iAnalog + 23, iAnalog + 31, iAnalog + 38, 1001, iAnalog + 55 };
                            if (listResulr != null)
                            {
                                deviceJK.PressureIN = listResulr.AnalogValues.ContainsKey(dataid[0].ToString()) ? Math.Round(double.Parse((listResulr.AnalogValues[dataid[0].ToString()]).ToString()), 2) : 0;
                                deviceJK.PressureOut = listResulr.AnalogValues.ContainsKey(dataid[1].ToString()) ? Math.Round(double.Parse((listResulr.AnalogValues[dataid[1].ToString()]).ToString()), 2) : 0;
                                deviceJK.PressureSet = listResulr.AnalogValues.ContainsKey(dataid[2].ToString()) ? Math.Round(double.Parse((listResulr.AnalogValues[dataid[2].ToString()]).ToString()), 2) : 0;
                                deviceJK.InstantFlow = listResulr.AnalogValues.ContainsKey(dataid[3].ToString()) ? Math.Round(double.Parse((listResulr.AnalogValues[dataid[3].ToString()]).ToString()), 2) : 0;
                                deviceJK.TotalPower = listResulr.AnalogValues.ContainsKey(dataid[4].ToString()) ? Math.Round(double.Parse((listResulr.AnalogValues[dataid[4].ToString()]).ToString()), 2) : 0;
                                deviceJK.TotalFlow = listResulr.AnalogValues.ContainsKey(dataid[5].ToString()) ? Math.Round(double.Parse((listResulr.AnalogValues[dataid[5].ToString()]).ToString()), 2) : 0;
                                deviceJK.SetFlow = listResulr.AnalogValues.ContainsKey(dataid[6].ToString()) ? Math.Round(double.Parse((listResulr.AnalogValues[dataid[6].ToString()]).ToString()), 2) : 0;
                                deviceJK.LiquidHight = listResulr.AnalogValues.ContainsKey(dataid[8].ToString()) ? Math.Round(double.Parse((listResulr.AnalogValues[dataid[8].ToString()]).ToString()), 2) : 0;
                                //在线离线
                                bool IsOnline = listResulr.DigitalValues.ContainsKey(dataid[7].ToString()) ? bool.Parse((listResulr.DigitalValues[dataid[7].ToString()]).ToString()) : false;
                                if (IsOnline)
                                {
                                    deviceJK.IsOnline = "在线";
                                }
                                else
                                {
                                    deviceJK.IsOnline = "离线";
                                }

                            }
                            else
                            {
                                deviceJK.PressureIN = 0;
                                deviceJK.PressureOut = 0;
                                deviceJK.PressureSet = 0;
                                deviceJK.InstantFlow = 0;
                                deviceJK.TotalPower = 0;
                                deviceJK.TotalFlow = 0;
                                deviceJK.SetFlow = 0;
                            }



                            //获取水泵信息
                            var fixfrequencyvalue = iAnalog + 3;
                            var frequencyvalue = iAnalog + 4;//频率
                            var electricvalue = iAnalog + 10;//电流
                            var pumpstatevalue = iDigial;//方式
                            var guzhangvalue = iDigial + 3;//状态
                            var bianpinvalue = iDigial + 1;//变频
                            var gongpinvalue = iDigial + 2;//工频
                            var hengpinvalue = iDigial + +127;//恒频
                            int j = 0;
                            for (var i = 0; i < PumpNum; i++)
                            {
                                //单变频
                                if (SwsRTUInfo.Frequency == 1)
                                {
                                    dataid = new List<int>() { pumpstatevalue + j, fixfrequencyvalue, electricvalue + i, guzhangvalue + j, bianpinvalue + j, hengpinvalue + i, gongpinvalue + j };
                                    Pump pump = WebApiActions.GetDataByDataIDMongo(listResulr, dataid);
                                    pump.PumpName = (i + 1) + "#泵";
                                    lstPumps.Add(pump);
                                }
                                //多变频
                                else
                                {
                                    dataid = new List<int>() { pumpstatevalue + j, frequencyvalue + i, electricvalue + i, guzhangvalue + j, bianpinvalue + j, hengpinvalue + i, gongpinvalue + j };
                                    Pump pump = WebApiActions.GetDataByDataIDMongo(listResulr, dataid);
                                    pump.PumpName = (i + 1) + "#泵";
                                    lstPumps.Add(pump);
                                }
                                j += 4;
                            }

                            deviceJK.lstPumps = lstPumps;
                        }

                        else
                        {
                            var p = new
                            {
                                Code = 1,
                                Message = "设备所属区域（低/中/高/超高/超超高）未填写"
                            };
                            return JsonConvert.SerializeObject(p);
                        }
                    }

                    #endregion

                    #region 直饮水设备处理
                    if (SwsRTUInfo.iEquipmentType == 2)
                    {
                        if (strSection.Length == 2)
                        {
                            int iAnalog = int.Parse(strSection[0]);
                            int iDigial = int.Parse(strSection[1]);

                            //在线离线
                            bool IsOnline = listResulr.DigitalValues.ContainsKey("1001") ? bool.Parse(listResulr.DigitalValues["1001"].ToString()) : false;
                            if (IsOnline)
                            {
                                deviceJK.IsOnline = "在线";
                            }
                            else
                            {
                                deviceJK.IsOnline = "离线";
                            }

                            deviceJK.PressureIN = 0;
                            deviceJK.PressureOut = 0;
                            deviceJK.PressureSet = 0;
                            deviceJK.InstantFlow = 0;
                            deviceJK.TotalPower = 0;
                            deviceJK.TotalFlow = 0;
                            deviceJK.SetFlow = 0;

                            List<int> y1dataid = new List<int>() { iAnalog, iDigial + 27, iAnalog + 18 };
                            lstPumps.Add(WebApiActions.GetYDataMongo(listResulr, y1dataid));
                            List<int> y2dataid = new List<int>() { iAnalog + 1, iDigial + 28, iAnalog + 19 };
                            lstPumps.Add(WebApiActions.GetYDataMongo(listResulr, y2dataid));
                            List<int> gdataid = new List<int>() { iAnalog + 4, iDigial + 29, iAnalog + 22 };
                            lstPumps.Add(WebApiActions.GetYDataMongo(listResulr, gdataid));
                            List<int> sumbers = new List<int>() { iAnalog + 2, iDigial + 30, iAnalog + 12, iAnalog + 20 };
                            lstPumps.Add(WebApiActions.GetYDataMongo(listResulr, sumbers));
                            List<int> sumbers2 = new List<int>() { iAnalog + 3, iDigial + 31, iAnalog + 13, iAnalog + 21 };
                            lstPumps.Add(WebApiActions.GetYDataMongo(listResulr, sumbers2));

                            deviceJK.lstPumps = lstPumps;

                        }

                        else
                        {
                            var p = new
                            {
                                Code = 1,
                                Message = "设备所属区域（低/中/高/超高/超超高）未填写"
                            };
                            return JsonConvert.SerializeObject(p);
                        }

                    }
                    #endregion
                }



                var rel = new
                {
                    Code = 1,
                    Message = "数据获取成功",
                    Data = deviceJK
                };
                return JsonConvert.SerializeObject(rel);
            }
            else
            {
                var rel = new
                {
                    Code = 0,
                    Message = "数据获取失败"
                };
                return JsonConvert.SerializeObject(rel);
            }
        }
        #endregion

        #region 报警推送接口
        [AllowAnonymous]
        [HttpGet]
        [TypeFilter(typeof(IgonreLoginFilter))]
        public string PushDevieAlarm(string ID, bool Cancle)
        {
            try
            {
                _chatHubContext.Clients.All.SendAsync("ReceiveMessage", ID, Cancle);
                var rel = new
                {
                    Code = 1,
                    message = "发送成功"
                };
                return JsonConvert.SerializeObject(rel);
            }
            catch (Exception e)
            {
                var rel = new
                {
                    Code = 2,
                    message = e.Message
                };
                return JsonConvert.SerializeObject(rel);
            }

        }
        #endregion

        #region 9000新工单  
        /// <summary>
        /// 时间戳
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static long ConvertDateTimeInt(System.DateTime time)
        {
            System.DateTime startTime = System.TimeZoneInfo.ConvertTimeToUtc(new System.DateTime(1970, 1, 1));
            return (long)(time - startTime).TotalMilliseconds;
        }
        /// <summary>
        /// 获取用户需要处理的工单
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public string LoadWosByUserID(long userId)
        {
            var workList = _wO_WorkOrderService.LoadWoListByUserID(userId).ToList();
            if (workList.Count > 0)
            {
                var rel = new
                {
                    Code = 1,
                    Message = "获取信息成功",
                    Data = workList
                };
                return JsonConvert.SerializeObject(rel);
            }
            else
            {
                var rel = new
                {
                    Code = 2,
                    Message = "暂无数据"
                };
                return JsonConvert.SerializeObject(rel);
            }
        }
        /// <summary>
        /// 根据工单ID获取用户需要处理的工单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public string LoadWoByID(long id)
        {
            var woInfo = _wO_WorkOrderService.LoadWoById(id).ToList().FirstOrDefault();
            if (woInfo != null)
            {
                var rel = new
                {
                    Code = 1,
                    Message = "获取信息成功",
                    Data = woInfo
                };
                return JsonConvert.SerializeObject(rel);
            }
            else
            {
                var rel = new
                {
                    Code = 2,
                    Message = "暂无数据"
                };
                return JsonConvert.SerializeObject(rel);
            }
        }
        /// <summary>
        /// 获取工单事件的图片，音频
        /// </summary>
        /// <param name="eventId">事件ID</param>
        /// <param name="type">类型（1图片，2音频，3视频）</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public string LoadResource(long eventId, int type)
        {
            var eventInfo = _wO_EventsService.Query<WoEvents>(R => R.IncidentId == eventId).FirstOrDefault();
            var rouseInfo = _wO_ResourceService.Query<WoResource>(r => r.Pid == eventInfo.IncidentId && r.ResourceType == eventInfo.IncidentType && r.Type == type).ToList();
            if (rouseInfo.Count > 0)
            {
                var rel = new
                {
                    Code = 1,
                    Message = "获取信息成功",
                    Data = rouseInfo
                };
                return JsonConvert.SerializeObject(rel);
            }
            else
            {
                var rel = new
                {
                    Code = 2,
                    Message = "暂无数据"
                };
                return JsonConvert.SerializeObject(rel);
            }
        }
        /// <summary>
        /// 添加工单操作信息
        /// </summary>
        /// <param name="info">工单操作信息</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public string AddOpInfo(string info)
        {
            //info = "{\"Id\":20,\"UserId\":3,\"OperationTime\":\"2020-08-24T11:51:54\",\"Type\":4,\"State\":0,\"Description\":\"wancheng\",\"Pid\":10101011}";
            string endinfo = info.Replace("\\", "");
            WoWooperation wo_WOOperation = new WoWooperation();
            //JObject jObject = JsonConvert.DeserializeObject(info) as JObject;
            //wo_WOOperation.Id = long.Parse(jObject["Id"].ToString());
            //wo_WOOperation.UserId = int.Parse(jObject["UserId"].ToString());
            //wo_WOOperation.OperationTime = DateTime.Parse(jObject["OperationTime"].ToString());
            //wo_WOOperation.Type = short.Parse(jObject["Type"].ToString());
            //wo_WOOperation.State = short.Parse(jObject["State"].ToString());
            //wo_WOOperation.Description = jObject["Description"].ToString();
            //wo_WOOperation.Pid = long.Parse(jObject["Pid"].ToString()); 
            //操作步骤信息
            wo_WOOperation = JsonConvert.DeserializeObject<WoWooperation>(endinfo);
            //工单信息
            WoWorkOrder woWorkOrder = new WoWorkOrder();
            woWorkOrder = _wO_WorkOrderService.Query<WoWorkOrder>(r => r.Woid == wo_WOOperation.Pid).FirstOrDefault();
            //事件信息
            WoEvents woEvents = new WoEvents();
            //更新工单状态、添加步骤信息
            if (wo_WOOperation.Type == (int)WNMS.Model.CustomizedClass.WOOperationType.维修完工)
            {
                woEvents = _wO_EventsService.Query<WoEvents>(R => R.IncidentId == woWorkOrder.EventId).FirstOrDefault();
                woEvents.DisposeState = (int)WNMS.Model.CustomizedClass.DisposeState.处理完成;
            }
            switch (wo_WOOperation.Type)
            {
                case 1:
                    woWorkOrder.CurrentState = (int)WNMS.Model.CustomizedClass.WoState.已接收;
                    break;
                case 2:
                    woWorkOrder.CurrentState = (int)WNMS.Model.CustomizedClass.WoState.已到场;
                    break;
                case 3:
                    woWorkOrder.CurrentState = (int)WNMS.Model.CustomizedClass.WoState.处理中;
                    break;
                case 4:
                    woWorkOrder.CurrentState = (int)WNMS.Model.CustomizedClass.WoState.已完工;
                    break;
            };
            if (_wO_WorkOrderService.AddWoOpInfo(wo_WOOperation, woWorkOrder, woEvents) > 0)
            {
                var rel = new
                {
                    Code = 1,
                    Message = "操作成功"
                };
                return JsonConvert.SerializeObject(rel);
            }
            else
            {
                var rel = new
                {
                    Code = 2,
                    Message = "操作失败"
                };
                return JsonConvert.SerializeObject(rel);
            }
        }
        /// <summary>
        /// 延期申请
        /// </summary>
        /// <param name="info">申请信息</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public string AddWOExtensionInfo(string info)
        {
            string endinfo = info.Replace("\"", "");
            WoWoextension wo_WOExtension = JsonConvert.DeserializeObject<WoWoextension>(endinfo);
            if (_wO_WOExtensionService.Insert(wo_WOExtension) != null)
            {
                var rel = new
                {
                    Code = 1,
                    Message = "操作成功"
                };
                return JsonConvert.SerializeObject(rel);
            }
            else
            {
                var rel = new
                {
                    Code = 2,
                    Message = "操作失败"
                };
                return JsonConvert.SerializeObject(rel);
            }
        }
        /// <summary>
        /// 延期、转发、退单申请
        /// </summary>
        /// <param name="info">申请信息</param>
        /// <param name="type">申请类型 1退单2转发3延期</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public string AddApplyInfo(string info, string type)
        {
            string endinfo = info.Replace("\\", ""); ;
            if (type == "3")
            {
                WoWoextension wo_WOExtension = JsonConvert.DeserializeObject<WoWoextension>(endinfo);
                if (_wO_WOExtensionService.Insert(wo_WOExtension) != null)
                {
                    var rel = new
                    {
                        Code = 1,
                        Message = "操作成功"
                    };
                    return JsonConvert.SerializeObject(rel);
                }
                else
                {
                    var rel = new
                    {
                        Code = 2,
                        Message = "操作失败"
                    };
                    return JsonConvert.SerializeObject(rel);
                }
            }
            else
            {
                WoForward woForward = JsonConvert.DeserializeObject<WoForward>(endinfo);
                woForward.ForwardTime = (DateTime)woForward.ExtensionTime;
                if (_wO_ForwardService.Insert(woForward) != null)
                {
                    var rel = new
                    {
                        Code = 1,
                        Message = "操作成功"
                    };
                    return JsonConvert.SerializeObject(rel);
                }
                else
                {
                    var rel = new
                    {
                        Code = 2,
                        Message = "操作失败"
                    };
                    return JsonConvert.SerializeObject(rel);
                }
            }

        }
        /// <summary>
        /// 我的延期申请
        /// </summary>
        /// <param name="userId">申请人</param> 
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public string LoadWOExtensionByUserID(long userId)
        {
            var infoList = _wO_WOExtensionService.Query<WoWoextension>(r => r.UserId == userId).ToList();
            if (infoList.Count > 0)
            {
                var rel = new
                {
                    Code = 1,
                    Message = "获取成功",
                    Data = infoList
                };
                return JsonConvert.SerializeObject(rel);
            }
            else
            {
                var rel = new
                {
                    Code = 2,
                    Message = "暂无数据"
                };
                return JsonConvert.SerializeObject(rel);
            }
        }
        /// <summary>
        /// 转发工单  查询已转发次数
        /// </summary>
        /// <param name="id">工单ID</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public string LoadWOForwardCount(long id)
        {
            var infoList = _wO_WorkOrderService.GetWOForwardCount(id).ToList();
            if (infoList.Count < 2)
            {
                var rel = new
                {
                    Code = 1,
                    Message = "可以转发"
                };
                return JsonConvert.SerializeObject(rel);
            }
            else
            {
                var rel = new
                {
                    Code = 2,
                    Message = "转发已超过两次"
                };
                return JsonConvert.SerializeObject(rel);
            }
        }
        /// <summary>
        /// 我的转发
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public string LoadMyForwardWO(int userId)
        {
            var foWo = _wO_ForwardService.Query<WoForward>(r => r.UserId == userId).ToList();
            if (foWo.Count > 0)
            {
                var rel = new
                {
                    Code = 1,
                    Message = "获取成功",
                    Data = foWo
                };
                return JsonConvert.SerializeObject(rel);
            }
            else
            {
                var rel = new
                {
                    Code = 2,
                    Message = "暂无数据"
                };
                return JsonConvert.SerializeObject(rel);
            }
        }

        /// <summary>
        /// 转发或退单申请
        /// </summary>
        /// <param name="woForward">申请信息</param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public string ForwardWO(string woForward)
        {
            WoForward woForward1 = JsonConvert.DeserializeObject<WoForward>(woForward);
            if (_wO_ForwardService.Insert(woForward1) != null)
            {
                var rel = new
                {
                    Code = 1,
                    Message = "申请成功"
                };
                return JsonConvert.SerializeObject(rel);
            }
            else
            {
                var rel = new
                {
                    Code = 2,
                    Message = "申请失败"
                };
                return JsonConvert.SerializeObject(rel);
            }
        }
        /// <summary>
        /// 同意/不同意接受转发工单
        /// </summary>
        /// <param name="id">转发申请的id</param>
        /// <param name="isOk">是否同意</param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public string ForwardWOOk(long id, int isOk)
        {
            if (isOk == 1)
            {
                //获取需要转发的工单信息
                //随机数
                Random random = new Random();
                var fwoinfo = _wO_ForwardService.Query<WoForward>(r => r.Woid == id).FirstOrDefault();
                var woInfo = _wO_WorkOrderService.Query<WoWorkOrder>(r => r.Woid == fwoinfo.Woid).FirstOrDefault();
                var newInfo = woInfo;
                newInfo.ReceiveUser = fwoinfo.RecipientId;
                newInfo.ReleaseTime = DateTime.Now;
                newInfo.Woid = ConvertDateTimeInt(DateTime.Now) + random.Next(1, 10000);
                //修改当前工单状态
                woInfo.CurrentState = (short)Model.CustomizedClass.WoState.移交;
                woInfo.CompleteTime = DateTime.Now;

                //工单历史数据
                WoWooperation woop = new WoWooperation();
                woop.Description = "工单转发";
                woop.Id = ConvertDateTimeInt(DateTime.Now) + random.Next(1, 10000);
                woop.OperationTime = DateTime.Now;
                woop.Pid = woInfo.Woid;
                woop.State = 0;
                woop.Type = (short)Model.CustomizedClass.WOOperationType.移交;
                woop.UserId = (int)woInfo.ReceiveUser;
                //新工单历史操作表
                WoWooperation newwoop = new WoWooperation();
                newwoop.Description = "";
                newwoop.Id = ConvertDateTimeInt(DateTime.Now) + random.Next(1, 10000);
                newwoop.OperationTime = DateTime.Now;
                newwoop.Pid = newInfo.Woid;
                newwoop.State = 0;
                newwoop.Type = (short)Model.CustomizedClass.WOOperationType.派发;
                newwoop.UserId = (int)woInfo.ReceiveUser;
                if (_wO_WorkOrderService.AddWOForward(newInfo, woInfo, newwoop, woop) > 0)
                {
                    var rel = new
                    {
                        Code = 1,
                        Message = "接收成功"
                    };
                    return JsonConvert.SerializeObject(rel);
                }
                else
                {
                    var rel = new
                    {
                        Code = 1,
                        Message = "接收失败"
                    };
                    return JsonConvert.SerializeObject(rel);
                }
            }
            else
            {
                var fwoinfo = _wO_ForwardService.Query<WoForward>(r => r.Woid == id).FirstOrDefault();
                fwoinfo.State = 2;
                if (_wO_ForwardService.Update(fwoinfo))
                {
                    var rel = new
                    {
                        Code = 1,
                        Message = "拒收成功"
                    };
                    return JsonConvert.SerializeObject(rel);
                }
                else
                {
                    var rel = new
                    {
                        Code = 1,
                        Message = "拒收失败，请重新操作"
                    };
                    return JsonConvert.SerializeObject(rel);
                }

            }

        }

        /// <summary>
        /// 转发接收人员
        /// </summary>
        /// <param name="userId">转发者</param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public string LoadUserTeamByUserID(int userId)
        {
            //获取客服信息 已存在班组中的人员
            var userInfo = _wO_TeamUserService.Query<WoTeamUser>(r => r.UserId == userId).FirstOrDefault();
            var userids = _wO_TeamUserService.Query<WoTeamUser>(r => r.TeamId == userInfo.TeamId).Select(r => r.UserId).ToList();
            var wbUser = _ISysUserService.Query<SysUser>(t => userids.Contains(t.UserId) && t.UserId != userId).ToList();
            if (wbUser.Count > 0)
            {
                var rel = new
                {
                    Code = 1,
                    Message = "获取成功",
                    Data = wbUser
                };
                return JsonConvert.SerializeObject(rel);
            }
            else
            {
                var rel = new
                {
                    Code = 2,
                    Message = "暂无数据"
                };
                return JsonConvert.SerializeObject(rel);
            }
        }

        /// <summary>
        /// 获取所有处理工单
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public string LoadAllWoByUserId(long userId)
        {
            //维修工单
            var workList = _wO_WorkOrderService.LoadWoListByUserID(userId).ToList();
            //巡检工单 
            var Travel_fitemid = (int)WNMS.Model.CustomizedClass.WorkOrder_enum.行走方式;
            var Cycle_fitemid = (int)WNMS.Model.CustomizedClass.WorkOrder_enum.巡检周期;
            //var filter = "State=1 and (IsFinish =0 or IsFinish is null) and Inspector=" + userId + "";
            var assignList = _InspectionPlanService.GetAssignList(Travel_fitemid, Cycle_fitemid, userId).ToList();
            if (workList.Count > 0 || assignList.Count > 0)
            {
                var rel = new
                {
                    Code = 1,
                    Message = "获取成功",
                    WoData = workList,
                    AsData = assignList
                };
                return JsonConvert.SerializeObject(rel);
            }
            else
            {
                var rel = new
                {
                    Code = 2,
                    Message = "暂无数据"
                };
                return JsonConvert.SerializeObject(rel);
            }

        }
        /// <summary>
        /// 获取历史工单
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public string LoadHisWoByUserId(long userId)
        {
            //维修历史工单 查询近三个月工单
            var beginDate = DateTime.Now.AddMonths(-3).ToString("yyyy-MM-dd");
            var workList = _wO_WorkOrderService.LoadHisWoListByUserID(userId, beginDate).ToList();
            //巡检工单
            var Travel_fitemid = (int)WNMS.Model.CustomizedClass.WorkOrder_enum.行走方式;
            var Cycle_fitemid = (int)WNMS.Model.CustomizedClass.WorkOrder_enum.巡检周期;
            //var filter = "IsFinish = 1 and EndDate >'" + beginDate + "' and Inspector=" + userId + "";
            var assignList = _InspectionPlanService.GetAssignListHistory(Travel_fitemid, Cycle_fitemid, beginDate, userId).ToList();
            if (workList.Count > 0 || assignList.Count > 0)
            {
                var rel = new
                {
                    Code = 1,
                    Message = "获取成功",
                    WoData = workList,
                    AsData = assignList
                };
                return JsonConvert.SerializeObject(rel);
            }
            else
            {
                var rel = new
                {
                    Code = 2,
                    Message = "暂无数据"
                };
                return JsonConvert.SerializeObject(rel);
            }
        }

        /// <summary>
        /// 根据工单ID获取用户需要处理的工单
        /// </summary>
        /// <param name="id">工单ID</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public string LoadWoByWOID(long id)//new
        {
            var woInfo = _wO_WorkOrderService.LoadWoById(id).ToList().FirstOrDefault();
            var eventInfo = _wO_EventsService.Query<WoEvents>(R => R.IncidentId == woInfo.EventID).FirstOrDefault();
            List<WoResource> rouseInfo = new List<WoResource>();
            if (eventInfo != null)
            {
                rouseInfo = _wO_ResourceService.Query<WoResource>(r => r.Pid == eventInfo.IncidentId && r.ResourceType == 5).ToList();
            }
            if (woInfo != null)
            {
                var rel = new
                {
                    Code = 1,
                    Message = "获取信息成功",
                    WoData = woInfo,
                    RseourceData = rouseInfo
                };
                return JsonConvert.SerializeObject(rel);
            }
            else
            {
                var rel = new
                {
                    Code = 2,
                    Message = "暂无数据"
                };
                return JsonConvert.SerializeObject(rel);
            }
        }
        /// <summary>
        /// 根据工单ID 获取工单反馈信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public string LoadWoOpByWOID(long id)
        {
            var info = _wO_WOOperationService.Query<WoWooperation>(r => r.Type > 1 && r.Type < 5 && r.Pid == id).ToList();
            List<RepairInfo> lists = new List<RepairInfo>();
            foreach (var it in info)
            {
                RepairInfo repairInfo = new RepairInfo();
                repairInfo.WoWooperation = it;
                repairInfo.woResources = _wO_ResourceService.Query<WoResource>(r => r.Pid == it.Id).ToList();
                lists.Add(repairInfo);
            }
            if (lists.Count > 0)
            {
                var rel = new
                {
                    Code = 1,
                    Message = "获取信息成功",
                    Data = lists
                };
                return JsonConvert.SerializeObject(rel);
            }
            else
            {
                var rel = new
                {
                    Code = 2,
                    Message = "暂无数据"
                };
                return JsonConvert.SerializeObject(rel);
            }
        }
        class RepairInfo
        {
            public WoWooperation WoWooperation;
            public List<WoResource> woResources;
        }
        #endregion

        #region 9000新工单事件管理
        /// <summary>
        /// 事件上报
        /// </summary>
        /// <param name="eventInfo">事件信息</param>
        /// <param name="woInfo">工单信息</param>
        /// <param name="resourceInfo">资源信息</param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public string AddEventInfo(string eventInfo, string woInfo, string resourceInfo)
        {
            // eventInfo = "{ \"Description\": \"描述\",\"Lat\": 0,\"EquipmentId\": 1583346221267,\"IncidentSource\": 0,\"EventID\": \"\", \"ReportUser\": 3,\"IncidentContent\": \"水井漏水\",\"IncidentType\": 2,\"ReportTime\": \"2021-07-27 09: 24: 22\",\"Lng\": 0,\"Address\": \"长安街\",\"StationID\": 0,\"Type\": 1,\"IncidentID\": 202107270924226670,\"IncidentState\": 1}";
            //woInfo = "{\"AuditingContent\": \"未审核\",\"ReceiveUser\": 3,\"Degree\": 1,\"WOID\": 202107270924226670,\"IsAuditing\": 3,\"EventID\": 202107270923562780,\"CurrentState\": 0,\"Num\": \"wx_num_10101\",\"CompleteTime\": \"2021-08-12 12:00:01\",\"HandleLevel\": \"2\",\"ReleaseUser\": 3,\"PID\": \"\",\"ReleaseTime\": \"2021-08-12 12:00:01\"}";
            var jsoneventInfo = eventInfo.Replace("\\", "");
            var jsonwoInfo = woInfo.Replace("\\", "");
            Random random = new Random();
            WoEvents woEvents = JsonConvert.DeserializeObject<WoEvents>(jsoneventInfo);
            //事件内容
            woEvents.IncidentId = ConvertDateTimeInt(DateTime.Now) + random.Next(1, 10000);
            woEvents.IncidentType = 2;
            int evCount = _wO_EventsService.Query<WoEvents>(r => true).Count() + 1;
            woEvents.IncidentNum = "EGWX" + "-" + DateTime.Now.Year + "-" + evCount.ToString().PadLeft(7, '0');
            woEvents.IncidentSource = 0;
            woEvents.DisposeState = 1;
            if (woEvents.Lat == 0 || woEvents.Lat == null)
            {
                if (woEvents.StationId != 0)
                {
                    var info = _ISws_StationService.Query<SwsStation>(r => r.StationId == woEvents.StationId).FirstOrDefault();
                    woEvents.Lat = (decimal)info?.Lat;
                    woEvents.Lng = (decimal)info?.Lng;
                }
            }
            //生成未处理工单 
            WoWorkOrder woWorkOrder = JsonConvert.DeserializeObject<WoWorkOrder>(jsonwoInfo);
            woWorkOrder.Woid = ConvertDateTimeInt(DateTime.Now) + random.Next(1, 10000);
            woWorkOrder.CurrentState = (short)WoState.未分派;
            woWorkOrder.Num = "WO_WX_" + woWorkOrder.Woid;
            woWorkOrder.IsAuditing = (byte)WOExtensionReview.未审核;

            //资源信息
            string jsonreSourceInfo = resourceInfo.Replace("\\", "");
            WoResource woResource = JsonConvert.DeserializeObject<WoResource>(jsonreSourceInfo);
            if (_wO_WorkOrderService.AddWOAndEvent(woWorkOrder, woEvents, woResource) > 0)
            {
                var rel = new
                {
                    Code = 1,
                    Message = "上报成功",
                };
                return JsonConvert.SerializeObject(rel);
            }
            else
            {
                var rel = new
                {
                    Code = 2,
                    Message = "上报失败",
                };
                return JsonConvert.SerializeObject(rel);
            }

        }

        /// <summary>
        /// 事件上报
        /// </summary>
        /// <param name="eventInfo">事件信息</param>
        /// <param name="Degree">紧急程度</param>
        /// <param name="HandleLevel">处理级别</param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public string AddEventsInfo(string eventInfo, string Degree, string HandleLevel)
        {
            // eventInfo = "{ \"Description\": \"描述\",\"Lat\": 0,\"EquipmentId\": 1583346221267,\"IncidentSource\": 0,\"EventID\": \"\", \"ReportUser\": 3,\"IncidentContent\": \"水井漏水\",\"IncidentType\": 2,\"ReportTime\": \"2021-07-27 09: 24: 22\",\"Lng\": 0,\"Address\": \"长安街\",\"StationID\": 0,\"Type\": 1,\"IncidentID\": 202107270924226670,\"IncidentState\": 1}";
            //woInfo = "{\"AuditingContent\": \"未审核\",\"ReceiveUser\": 3,\"Degree\": 1,\"WOID\": 202107270924226670,\"IsAuditing\": 3,\"EventID\": 202107270923562780,\"CurrentState\": 0,\"Num\": \"wx_num_10101\",\"CompleteTime\": \"2021-08-12 12:00:01\",\"HandleLevel\": \"2\",\"ReleaseUser\": 3,\"PID\": \"\",\"ReleaseTime\": \"2021-08-12 12:00:01\"}";
            var jsoneventInfo = eventInfo.Replace("\\", "");
            WoEvents woEvents = JsonConvert.DeserializeObject<WoEvents>(jsoneventInfo);
            //事件内容 
            //woEvents.IncidentType = 2;
            int evCount = _wO_EventsService.Query<WoEvents>(r => true).Count() + 1;
            woEvents.IncidentNum = "BXWX" + "_" + DateTime.Now.Year + "_" + evCount.ToString().PadLeft(7, '0');
            woEvents.IncidentSource = (int)Model.CustomizedClass.IncidentSource.移动端;
            woEvents.DisposeState = 1;
            if (woEvents.Lat == 0 || woEvents.Lat == null)
            {
                if (woEvents.StationId != 0)
                {
                    var info = _ISws_StationService.Query<SwsStation>(r => r.StationId == woEvents.StationId).FirstOrDefault();
                    woEvents.Lat = (decimal)info?.Lat;
                    woEvents.Lng = (decimal)info?.Lng;
                }
            }
            //生成未处理工单
            Random random = new Random();
            WoWorkOrder woWorkOrder = new WoWorkOrder();
            woWorkOrder.Woid = ConvertDateTimeInt(DateTime.Now) + random.Next(1, 10000);
            woWorkOrder.Degree = short.Parse(Degree);
            woWorkOrder.HandleLevel = short.Parse(HandleLevel);
            switch (HandleLevel)
            {
                case "2":
                    woWorkOrder.CompleteTime = woEvents.ReportTime.AddHours(2);
                    break;
                case "12":
                    woWorkOrder.CompleteTime = woEvents.ReportTime.AddHours(12);
                    break;
                case "24":
                    woWorkOrder.CompleteTime = woEvents.ReportTime.AddHours(24);
                    break;
                case "48":
                    woWorkOrder.CompleteTime = woEvents.ReportTime.AddHours(48);
                    break;
                case "72":
                    woWorkOrder.CompleteTime = woEvents.ReportTime.AddHours(72);
                    break;
                case "162":
                    woWorkOrder.CompleteTime = woEvents.ReportTime.AddHours(162);
                    break;
                case "6":
                    woWorkOrder.CompleteTime = woEvents.ReportTime.AddDays(7);
                    break;
            }
            woWorkOrder.ReleaseTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            woWorkOrder.EventId = woEvents.IncidentId;
            woWorkOrder.CurrentState = (short)WoState.未分派;
            //woWorkOrder.Num = "WO_WX_" + woWorkOrder.Woid;

            var beginTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 00:00:00"));
            var endTime = DateTime.Parse(DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00"));
            var woCount = _wO_WorkOrderService.Query<WoWorkOrder>(r => r.ReleaseTime >= beginTime && r.ReleaseTime <= endTime).ToList().Count();
            string num = "0";
            if (woCount == 0)
            {
                num = "001";
            }
            else if (woCount < 10)
            {
                int tenCount = woCount + 1;
                num = "00" + tenCount;
            }
            else if (woCount >= 10 && woCount < 100)
            {
                num = "0" + woCount;
            }
            woWorkOrder.Num = "WO_WX_" + DateTime.Now.ToString("yyyyMMdd") + num;

            woWorkOrder.IsAuditing = (byte)WOExtensionReview.未审核;

            if (_wO_WorkOrderService.AddWOAndEvent(woWorkOrder, woEvents) > 0)
            {
                var rel = new
                {
                    Code = 1,
                    Message = "上报成功",
                };
                return JsonConvert.SerializeObject(rel);
            }
            else
            {
                var rel = new
                {
                    Code = 2,
                    Message = "上报失败",
                };
                return JsonConvert.SerializeObject(rel);
            }

        }
        /// <summary>
        /// 事件上报的资源（图片，音频） 弃
        /// </summary>
        /// <param name="info">资源信息</param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public string AddResourceInfo(string info)
        {
            try
            {
                string endinfo = info.Replace("\\", "");
                WoResource wo_Resource = JsonConvert.DeserializeObject<WoResource>(endinfo);
                if (_wO_ResourceService.Insert(wo_Resource) != null)
                {
                    var rel = new
                    {
                        Code = 1,
                        Message = "添加成功"
                    };
                    return JsonConvert.SerializeObject(rel);
                }
                else
                {
                    var rel = new
                    {
                        Code = 2,
                        Message = "添加失败"
                    };
                    return JsonConvert.SerializeObject(rel);
                }
            }
            catch (Exception ex)
            {
                var rel = new
                {
                    Code = 3,
                    Message = ex.Message
                };
                return JsonConvert.SerializeObject(rel);
            }

        }
        /// <summary>
        /// 获取泵房信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public string LoadStationInfo(long userId)
        {
            var user = _ISysUserService.Query<SysUser>(u => u.UserId == userId).FirstOrDefault();
            List<PumStation> psList = new List<PumStation>();
            if (user.IsAdmin != true)
            {
                var id = _swsUserStationService.Query<SwsUserStation>(r => r.UserId == user.UserId).Select(r => r.StationId).ToList();
                var stationList = _ISws_StationService.Query<SwsStation>(r => id.Contains(r.StationId)).ToList();

                if (stationList.Count > 0)
                {
                    foreach (var item in stationList)
                    {
                        List<PumEqu> pumEqus = new List<PumEqu>();
                        PumStation pumStation = new PumStation();
                        pumStation.StationID = item.StationId;
                        pumStation.StationName = item.StationName;
                        pumStation.Lng = item.Lng;
                        pumStation.Lat = item.Lat;
                        var deviceList = _ISws_DeviceInfo01Service.Query<SwsDeviceInfo01>(r => r.StationId == item.StationId).ToList();
                        foreach (var it in deviceList)
                        {
                            PumEqu pumEqu = new PumEqu();
                            pumEqu.DeviceID = it.DeviceId;
                            pumEqu.DeviceName = it.DeviceName;
                            pumEqus.Add(pumEqu);
                        }
                        pumStation.pumEqus = pumEqus;
                        psList.Add(pumStation);
                    }
                    var rel = new
                    {
                        Code = 1,
                        Message = "获取成功",
                        Data = psList
                    };
                    return JsonConvert.SerializeObject(rel);
                }
                else
                {
                    var rel = new
                    {
                        Code = 2,
                        Message = "暂无数据"
                    };
                    return JsonConvert.SerializeObject(rel);
                }
            }
            else
            {
                var stationList = _ISws_StationService.Query<SwsStation>(r => true).ToList();
                if (stationList.Count > 0)
                {
                    foreach (var item in stationList)
                    {
                        List<PumEqu> pumEqus = new List<PumEqu>();
                        PumStation pumStation = new PumStation();
                        pumStation.StationID = item.StationId;
                        pumStation.StationName = item.StationName;
                        pumStation.Lng = item.Lng;
                        pumStation.Lat = item.Lat;
                        var deviceList = _ISws_DeviceInfo01Service.Query<SwsDeviceInfo01>(r => r.StationId == item.StationId).ToList();
                        foreach (var it in deviceList)
                        {
                            PumEqu pumEqu = new PumEqu();
                            pumEqu.DeviceID = it.DeviceId;
                            pumEqu.DeviceName = it.DeviceName;
                            pumEqus.Add(pumEqu);
                        }
                        pumStation.pumEqus = pumEqus;
                        psList.Add(pumStation);
                    }
                    var rel = new
                    {
                        Code = 1,
                        Message = "获取成功",
                        //Data = stationList
                        Data = psList
                    };
                    return JsonConvert.SerializeObject(rel);
                }
                else
                {
                    var rel = new
                    {
                        Code = 2,
                        Message = "暂无数据"
                    };
                    return JsonConvert.SerializeObject(rel);
                }
            }
        }
        /// <summary>
        /// 文件上传
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public string UploadFileInfo(string id)
        {
            try
            {
                var imgPath = "";
                var fileName = "";
                string path = "";
                var files = Request.Form.Files;
                var ids = Request.Form.FirstOrDefault().Value;
                var idList = Request.Form.ToList();
                foreach (var item in idList)
                {
                    var aa = item.Key;
                }
                //var id1 = ids["id"];
                foreach (var file in files)
                {
                    imgPath = "/UploadImg/Wo_Event/" + id + "/";
                    string dicPath = _webHostEnvironment.WebRootPath + imgPath;
                    if (!Directory.Exists(dicPath))
                    {
                        Directory.CreateDirectory(dicPath);
                    }
                    string ext = Path.GetExtension(file.FileName);
                    fileName = Guid.NewGuid().ToString() + ext;
                    string filePath = Path.Combine(dicPath, fileName);
                    using (FileStream fs = System.IO.File.Create(filePath))
                    {
                        file.CopyTo(fs);
                        fs.Flush();
                    }
                    path += imgPath + fileName + ",";
                }
                path = path.Substring(0, path.Length - 1);
                var rel = new
                {
                    Code = 1,
                    Message = "上传成功",
                    Data = path
                };
                return JsonConvert.SerializeObject(rel);
            }
            catch (Exception ex)
            {
                var rel = new
                {
                    Code = 2,
                    Message = "上传失败",
                    Data = ex.Message
                };
                return JsonConvert.SerializeObject(rel);
            }

        }
        /// <summary>
        /// 文件上传
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public string UploadFiles()
        {
            try
            {
                var imgPath = "";
                var fileName = "";
                string path = "";
                var files = Request.Form.Files;
                var idList = Request.Form.ToList();
                string id = "";
                string type = "";
                foreach (var item in idList)
                {
                    if (item.Key == "id")
                    {
                        id = item.Value;
                    }
                    if (item.Key == "type")
                    {
                        type = item.Value;
                    }
                }
                if (type == "1" || type == "3" || type == "5" || type == "6")
                {
                    foreach (var file in files)
                    {
                        imgPath = "/UploadImg/Wo_Event/" + id + "/";
                        string dicPath = _webHostEnvironment.WebRootPath + imgPath;
                        if (!Directory.Exists(dicPath))
                        {
                            Directory.CreateDirectory(dicPath);
                        }
                        string ext = Path.GetExtension(file.FileName);
                        fileName = Guid.NewGuid().ToString() + ext;
                        string filePath = Path.Combine(dicPath, fileName);
                        using (FileStream fs = System.IO.File.Create(filePath))
                        {
                            file.CopyTo(fs);
                            fs.Flush();
                        }

                        WoResource woResource = new WoResource();
                        Random random = new Random();
                        woResource.Id = ConvertDateTimeInt(DateTime.Now) + random.Next(1, 10000);
                        woResource.Pid = long.Parse(id);
                        if (ext == ".mp3")
                        {
                            woResource.Type = 2;
                        }
                        else
                        {
                            woResource.Type = 1;
                        }
                        woResource.Path = imgPath + fileName;
                        woResource.ResourceType = short.Parse(type);
                        woResource.Suffix = ext;
                        woResource.FileName = fileName;
                        _wO_ResourceService.Insert(woResource);
                    }
                    var rel = new
                    {
                        Code = 1,
                        Message = "上传成功"
                    };
                    return JsonConvert.SerializeObject(rel);
                }
                else
                {
                    foreach (var file in files)
                    {
                        imgPath = "/UploadImg/设备巡检/";
                        string dicPath = _webHostEnvironment.WebRootPath + imgPath;
                        if (!Directory.Exists(dicPath))
                        {
                            Directory.CreateDirectory(dicPath);
                        }
                        string ext = Path.GetExtension(file.FileName);
                        fileName = Guid.NewGuid().ToString() + ext;
                        string filePath = Path.Combine(dicPath, fileName);
                        using (FileStream fs = System.IO.File.Create(filePath))
                        {
                            file.CopyTo(fs);
                            fs.Flush();
                        }
                        path += fileName + ",";
                    }
                    path = path.Substring(0, path.Length - 1);
                    var rel = new
                    {
                        Code = 1,
                        Message = "上传成功",
                        Data = path
                    };
                    return JsonConvert.SerializeObject(rel);
                }

            }
            catch (Exception ex)
            {
                var rel = new
                {
                    Code = 2,
                    Message = "上传失败",
                    Data = ex.Message
                };
                return JsonConvert.SerializeObject(rel);
            }

        }
        /// <summary>
        /// 获取解决方案
        /// </summary>
        /// <param name="id">工单中解决方案字段</param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public string EarlyWarningPlan(string id)
        {
            List<long> ids = new List<string>(id.Split(",")).ConvertAll(r => long.Parse(r));
            var info = _sys_EarlyWarningPlanService.Query<SysEarlyWarningPlan>(r => ids.Contains(r.Id)).ToList();
            if (info.Count > 0)
            {
                var rel = new
                {
                    Code = 1,
                    Message = "获取成功",
                    Data = info
                };
                return JsonConvert.SerializeObject(rel);
            }
            else
            {
                var rel = new
                {
                    Code = 2,
                    Message = "暂无数据"
                };
                return JsonConvert.SerializeObject(rel);
            }


        }

        /// <summary>
        /// 设备历史维修记录
        /// </summary>
        /// <param name="eqId">设备ID</param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public string LoadHisByEqId(long eqId)
        {
            List<WoHistory> woHistories = new List<WoHistory>();
            var eventList = _wO_EventsService.Query<WoEvents>(r => r.EquipmentId == eqId).ToList();
            foreach (var item in eventList)
            {
                WoHistory woHistory = new WoHistory();
                woHistory.woEvents = item;
                var woList = _wO_WorkOrderService.Query<WoWorkOrder>(r => r.EventId == item.IncidentId).FirstOrDefault();
                woHistory.woWooperations = _wO_WOOperationService.Query<WoWooperation>(r => r.Pid == woList.Woid && r.Type == (short)Model.CustomizedClass.WOOperationType.维修完工).OrderByDescending(r => r.OperationTime).FirstOrDefault();
                woHistories.Add(woHistory);
            }
            if (woHistories.Count > 0)
            {
                var rel = new
                {
                    Code = 1,
                    Message = "获取成功",
                    Data = woHistories
                };
                return JsonConvert.SerializeObject(rel);
            }
            else
            {
                var rel = new
                {
                    Code = 2,
                    Message = "暂无数据",
                };
                return JsonConvert.SerializeObject(rel);
            }
        }
        #endregion

        #region 9000新工单巡检管理

        /// <summary>
        /// 根据巡检任务ID，获取巡检项内容
        /// </summary>
        /// <param name="planID">巡检任务ID</param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public string LoadTempLateByID(long planID)
        {
            var tempList = _wO_AssignmentPlanService.GetTemplateDetailData(planID).ToList();
            if (tempList.Count > 0)
            {
                var rel = new
                {
                    Code = 1,
                    Message = "获取成功",
                    Data = tempList
                };
                return JsonConvert.SerializeObject(rel);
            }
            else
            {
                var rel = new
                {
                    Code = 2,
                    Message = "暂无数据"
                };
                return JsonConvert.SerializeObject(rel);
            }
        }

        /// <summary>
        /// 添加反馈信息
        /// </summary>
        /// <param name="info">json字符串</param>
        /// <param name="isFinish">是否完成</param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public string AddInspectPlanCheckInfo(string info, bool isFinish)
        {

            // info = "{\"Id\":20,\"PlanId\":5,\"EquipmentId\":1483687370867,\"ModifyTime\":\"2020-08-24T11:51:54\",\"ReachState\":false,\"FeedBackState\":true,\"Gislocation\":null,\"InspectImage\":\"IMG_20200821_114707.jpg,IMG_20200821_114745.jpg,IMG_20200821_114754.jpg,IMG_20200821_114836.jpg,IMG_20200821_114845.jpg,IMG_20200821_115202.jpg,IMG_20200821_115209.jpg,IMG_20200821_115214.jpg,IMG_20200821_115227.jpg\",\"DetailContent\":\"{\\\"监控是否连接正常\\\":\\\"666\\\",\\\"实际压力值\\\":\\\"666\\\",\\\"巡检人\\\":\\\"666\\\",\\\"设备型号\\\":\\\"fgh\\\"}\",\"ObjectType\":0}";
            try
            {
                //string endinfo = info.Replace("\"", "");
                WoInspectPlanCheck wo_check = JsonConvert.DeserializeObject<WoInspectPlanCheck>(info);
                var assInfo = _wO_AssignmentPlanService.Query<WoAssignmentPlan>(r => r.PlanId == wo_check.PlanId).FirstOrDefault();
                if (isFinish == true)
                {
                    //0是未完成1是待审核2是已审核
                    assInfo.IsFinish = 1;
                }
                //更新一下设备的审核状态
                var eqInfo = _wO_PlanInspectOService.Query<WoPlanInspectO>(r => r.PlanId == wo_check.PlanId && r.InspectObject == wo_check.EquipmentId).FirstOrDefault();
                eqInfo.AuditUser = null;
                eqInfo.IsAuditing = null;
                if (_wO_AssignmentPlanService.AddInspectPlanCheckInfo(wo_check, assInfo, eqInfo) > 0)
                {
                    var rel = new
                    {
                        Code = 1,
                        Message = "添加成功"
                    };
                    return JsonConvert.SerializeObject(rel);
                }
                else
                {
                    var rel = new
                    {
                        Code = 2,
                        Message = "添加失败"
                    };
                    return JsonConvert.SerializeObject(rel);
                }
            }
            catch (Exception ex)
            {
                var rel = new
                {
                    Code = 3,
                    Message = ex.Message
                };
                return JsonConvert.SerializeObject(rel);
            }

        }
        /// <summary>
        /// 查询巡检情况
        /// </summary>
        /// <param name="planID">巡检ID</param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public string LoadConditionByID(long planID)
        {
            var tempList = _wO_AssignmentPlanService.GetTemplateDetailData(planID).ToList();
            if (tempList.Count > 0)
            {
                var rel = new
                {
                    Code = 1,
                    Message = "获取成功",
                    Data = tempList
                };
                return JsonConvert.SerializeObject(rel);
            }
            else
            {
                var rel = new
                {
                    Code = 2,
                    Message = "暂无数据"
                };
                return JsonConvert.SerializeObject(rel);
            }
        }
        #endregion

        #region 巡检列表
        /// <summary>
        /// 根据用户id查询巡检列表
        /// </summary>
        /// <param name="userid">用户id</param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public string LoadAssignList(int userid)
        {
            var Travel_fitemid = (int)WNMS.Model.CustomizedClass.WorkOrder_enum.行走方式;
            var Cycle_fitemid = (int)WNMS.Model.CustomizedClass.WorkOrder_enum.巡检周期;
            //var filter = "State=1 and (IsFinish =0 or IsFinish is null) and Inspector=" + userid + "";
            var data = _InspectionPlanService.GetAssignList(Travel_fitemid, Cycle_fitemid, userid).ToList();
            if (data.Count > 0)
            {
                var rel = new
                {
                    Code = 1,
                    Message = "获取成功",
                    Data = data
                };
                return JsonConvert.SerializeObject(rel);
            }
            else
            {
                var rel = new
                {
                    Code = 2,
                    Message = "暂无数据"
                };
                return JsonConvert.SerializeObject(rel);
            }
        }
        /// <summary>
        /// 根据巡检ID获取巡检详情
        /// </summary>
        /// <param name="planid">巡检详情</param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public string LoadAssinByPlanid(long planid)
        {
            //var Travel_fitemid = (int)WNMS.Model.CustomizedClass.WorkOrder_enum.行走方式;
            //var Cycle_fitemid = (int)WNMS.Model.CustomizedClass.WorkOrder_enum.巡检周期;
            //var filter = " PlanID=" + planid + "";
            //var data = _InspectionPlanService.GetAssignList(Travel_fitemid, Cycle_fitemid, filter).ToList();
            var data = _InspectionPlanService.GetEqui_Assign(planid).ToList();
            var Travel_fitemid = (int)WNMS.Model.CustomizedClass.WorkOrder_enum.行走方式;
            var Cycle_fitemid = (int)WNMS.Model.CustomizedClass.WorkOrder_enum.巡检周期;
            //var filter = "PlanID = " + planid + "";
            var assData = _InspectionPlanService.GetAssignListByID(Travel_fitemid, Cycle_fitemid, planid).ToList();
            if (data.Count > 0)
            {
                var rel = new
                {
                    Code = 1,
                    Message = "获取成功",
                    Data = data,
                    assData
                    //Data = new
                    //{
                    //    assignData = data,
                    //    EquipList = _InspectionPlanService.GetEqui_Assign(planid)
                    //}
                };
                return JsonConvert.SerializeObject(rel);
            }
            else
            {
                var rel = new
                {
                    Code = 2,
                    Message = "暂无数据"
                };
                return JsonConvert.SerializeObject(rel);
            }
        }
        /// <summary>
        /// 巡检转发
        /// </summary>
        /// <param name="planid">移交的ID</param>
        /// <param name="eqids">移交的设备</param>
        /// <param name="userid">移交接收人</param>
        /// <param name="content">移交原因</param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public string InsForward(long planid, string eqids, int userid, string content)
        {
            Random random = new Random();
            var oldAssplan = _wO_AssignmentPlanService.Query<WoAssignmentPlan>(r => r.PlanId == planid).FirstOrDefault();
            var userInfo = _ISysUserService.Find<SysUser>(oldAssplan.Inspector);
            if (oldAssplan != null)
            {
                List<WoPlanInspectO> newObj = new List<WoPlanInspectO>();
                List<WoPlanInspectO> oldObj = new List<WoPlanInspectO>();
                WoAssignmentPlan woAssignmentPlan = new WoAssignmentPlan();
                woAssignmentPlan.PlanId = ConvertDateTimeInt(DateTime.Now) + random.Next(1, 10000);
                woAssignmentPlan.Inspector = userid;
                woAssignmentPlan.BeginDate = oldAssplan.BeginDate;
                woAssignmentPlan.EndDate = oldAssplan.EndDate;
                woAssignmentPlan.PlanName = userInfo.NickName + "移交的" + oldAssplan.PlanName;
                woAssignmentPlan.InspectCycle = oldAssplan.InspectCycle;
                woAssignmentPlan.Creater = oldAssplan.Creater;
                woAssignmentPlan.Dmaid = oldAssplan.Dmaid;
                woAssignmentPlan.Travel = oldAssplan.Travel;
                woAssignmentPlan.State = false;
                woAssignmentPlan.Remark = oldAssplan.Remark;
                woAssignmentPlan.PlanType = oldAssplan.PlanType;
                woAssignmentPlan.CreateTime = DateTime.Now;
                woAssignmentPlan.TemplateId = oldAssplan.TemplateId;
                woAssignmentPlan.Type = oldAssplan.Type;
                woAssignmentPlan.Gis = oldAssplan.Gis;
                woAssignmentPlan.TemplatePlanId = oldAssplan.TemplatePlanId;
                woAssignmentPlan.IsFinish = 0;//---biaoji yixia 
                woAssignmentPlan.IsForward = 1;
                List<long> ids = new List<string>(eqids.Split(',')).ConvertAll(r => long.Parse(r));//获取需要移交的设备ID
                //获取转发之前工单的巡检设备信息 ForwardState 标识转发状态，NULL 为未转发，1为正在转发，2为已经转发
                var oldEq = _wO_PlanInspectOService.Query<WoPlanInspectO>(r => r.PlanId == oldAssplan.PlanId && r.ForwardState == null).ToList();
                foreach (var item in ids)
                {
                    var oldInfo = oldEq.Where(r => r.PlanId == planid && r.InspectObject == item).FirstOrDefault();
                    WoPlanInspectO inspectO = new WoPlanInspectO();
                    //生成新的巡检设备  更新之前的巡检设备状态
                    inspectO.PlanId = woAssignmentPlan.PlanId;
                    inspectO.InspectObject = oldInfo.InspectObject;
                    inspectO.IsTemplate = oldInfo.IsTemplate;//是计划模板
                    inspectO.TemplateId = oldInfo.TemplateId;
                    inspectO.ObjectName = oldInfo.ObjectName;
                    inspectO.PumpStationId = oldInfo.PumpStationId;
                    inspectO.ForwardCount = oldInfo.ForwardCount + 1;
                    oldInfo.ForwardState = 1;
                    newObj.Add(inspectO);
                    oldObj.Add(oldInfo);
                }
                //添加申请信息
                WoInsForward woInsForward = new WoInsForward();
                woInsForward.Id = ConvertDateTimeInt(DateTime.Now) + random.Next(1, 10000);
                woInsForward.PlanId = woAssignmentPlan.PlanId;
                woInsForward.UserId = oldAssplan.Inspector;
                woInsForward.RecipientId = userid;
                woInsForward.ExtensionTime = DateTime.Now;
                woInsForward.CompleteTime = woAssignmentPlan.EndDate;
                woInsForward.State = 3;
                woInsForward.Type = 2;//转发
                woInsForward.ForwardTime = DateTime.Now;
                woInsForward.IsReceive = false;
                woInsForward.OldPlanId = oldAssplan.PlanId;
                woInsForward.Remake = content;
                if (_InspectionPlanService.EditObjectForward(woAssignmentPlan, newObj, oldObj, woInsForward) > 0)
                {
                    var rel = new
                    {
                        Code = 1,
                        Message = "申请成功"
                    };
                    return JsonConvert.SerializeObject(rel);
                }
                else
                {
                    var rel = new
                    {
                        Code = 2,
                        Message = "申请失败"
                    };
                    return JsonConvert.SerializeObject(rel);
                }
            }
            else
            {
                var rel = new
                {
                    Code = 3,
                    Message = "无转发数据"
                };
                return JsonConvert.SerializeObject(rel);
            }
        }
        /// <summary>
        /// 巡检退单申请
        /// </summary>
        /// <param name="planinfo">申请信息</param>
        /// <param name="type">申请类型1-退单 2-延期</param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public string PlanTurn(string planinfo, string type)
        {
            
            if (type == "1")
            {
                WoInsForward woInsForward = JsonConvert.DeserializeObject<WoInsForward>(planinfo);
                //WoForward woForward = JsonConvert.DeserializeObject<WoForward>(planinfo);
                woInsForward.RecipientId = null;
                woInsForward.CompleteTime = null;
                woInsForward.State = 3;
               // woForward.RecipientId = null;
              //  woForward.RecipientId = null;
                if (_wO_InsForwardService.Insert(woInsForward) != null)
                {
                    var rel = new
                    {
                        Code = 1,
                        Message = "申请成功"
                    };
                    return JsonConvert.SerializeObject(rel);
                }
                else
                {
                    var rel = new
                    {
                        Code = 2,
                        Message = "申请失败"
                    };
                    return JsonConvert.SerializeObject(rel);
                }
            }
            else
            {
                WoInsExtension woInsExtension = JsonConvert.DeserializeObject<WoInsExtension>(planinfo);
                if (_wO_InsExtensionService.Insert(woInsExtension) != null)
                {
                    var rel = new
                    {
                        Code = 1,
                        Message = "申请成功"
                    };
                    return JsonConvert.SerializeObject(rel);
                }
                else
                {
                    var rel = new
                    {
                        Code = 2,
                        Message = "申请失败"
                    };
                    return JsonConvert.SerializeObject(rel);
                }
            }

        }
        /// <summary>
        /// 撤销反馈
        /// </summary>
        /// <param name="planid">巡检单ID</param>
        /// <param name="eqid">巡检设备ID</param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public string RevokeFB(long planid, long eqid)
        {
            var fbinfo = _wO_InspectPlanCheckService.Query<WoInspectPlanCheck>(r => r.PlanId == planid && r.EquipmentId == eqid).FirstOrDefault();
            if (fbinfo != null)
            {
                if (_wO_InspectPlanCheckService.Delete(fbinfo))
                {
                    var rel = new
                    {
                        Code = 1,
                        Message = "操作成功"
                    };
                    return JsonConvert.SerializeObject(rel);
                }
                else
                {
                    var rel = new
                    {
                        Code = 2,
                        Message = "操作失败"
                    };
                    return JsonConvert.SerializeObject(rel);
                }
            }
            else
            {
                var rel = new
                {
                    Code = 3,
                    Message = "暂无数据"
                };
                return JsonConvert.SerializeObject(rel);
            }
        }
        #endregion

        #region 预警接口 
        /// <summary>
        /// 预警接口
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [TypeFilter(typeof(IgonreLoginFilter))]
        public string PushWarnInfo()
        {
            try
            {
                _chatHubContext.Clients.All.SendAsync("ReceiveWarn");
                var rel = new
                {
                    Code = 1,
                    message = "发送成功"
                };
                return JsonConvert.SerializeObject(rel);
            }
            catch (Exception e)
            {
                var rel = new
                {
                    Code = 2,
                    message = e.Message
                };
                return JsonConvert.SerializeObject(rel);
            }
        }
        #endregion

        //极光推送
        [HttpGet]
        [AllowAnonymous]
        public void PushTask(int userId)
        {
            Jpush jpush = new Jpush();
            try
            {

                PushPayload pushPayload = new PushPayload()
                {
                    Platform = new List<string> { "android", "ios" },
                    Audience = new Audience
                    {
                        Alias = new List<string> { userId.ToString() }
                    },
                    Message = new Message
                    {
                        Title = "ceshi",
                        Content = "ceshi",
                    }
                };
                jpush.ExcutePush(pushPayload);

            }
            catch (Exception)
            {
                throw;
            }
        }

        #region 小工具  JSON转换
        [HttpGet]
        [AllowAnonymous]
        public string JsoToData()
        {
            //            {
            //                "Id": 20,
            //    "UserId": 3,
            //    "OperationTime": 2020 - 08 - 24T11: 51: 54,
            //    "Description": "完成",
            //    "Pid": 10101011,
            //    "Type": 3,
            //    "State": 0
            //}

            WoWooperation woWooperation = new WoWooperation();
            woWooperation.Id = 10;
            woWooperation.UserId = 3;
            woWooperation.OperationTime = DateTime.Now;
            woWooperation.Description = "wancheng";
            woWooperation.Pid = 10101011;
            woWooperation.Type = 3;
            woWooperation.State = 0;

            return JsonConvert.SerializeObject(woWooperation);
        }
        #endregion
        #region 服务事件回调接口
        [HttpPost]
        [AllowAnonymous]
        [TypeFilter(typeof(IgonreLoginFilter))]
        public string EventRcv([FromBody] rcvEvents e)
        {


            List<pushEevent> pushData = new List<pushEevent>();
            var serialNums = _ConsumpSettingService.Query<SwsCamera>(r => r.SerialNum != null && r.SerialNum != "").Select(r => r.SerialNum).ToList();
            if (serialNums.Count > 0)
            {
                foreach (var item in e.@params.events)
                {
                    if (serialNums.Contains(item.srcIndex))
                    {
                        pushEevent pp = new pushEevent();
                        var eventtypeName = System.Enum.GetName(typeof(WNMS.Model.CustomizedClass.eventType_enum), item.eventType);
                        pp.eventType = (int)item.eventType;
                        pp.eventTypeName = !string.IsNullOrEmpty(eventtypeName) ? eventtypeName : item.eventType.ToString();
                        pp.happenTime = Convert.ToDateTime(item.happenTime.Substring(0, item.happenTime.IndexOf("+")).Replace("T", " ")).ToString("yyyy-MM-dd HH:mm:ss");
                        pp.srcIndex = item.srcIndex;
                        pp.srcName = item.srcName;
                        pushData.Add(pp);
                    }
                }
                //var length=e["params"]["events"].Count();
                //var srcIndex = "";
                //for (var i=0;i< length;i++)
                //{
                //    srcIndex += e["params"]["events"][i]["srcIndex"]+",";
                //}
                if (pushData.Count > 0)
                {
                    var data = Newtonsoft.Json.JsonConvert.SerializeObject(pushData);
                    _chatHubContext.Clients.All.SendAsync("EventRcv", data);
                }
            }

            return "ok";
        }

        public class rcvEvents
        {
            public string method { get; set; }
            public eventPara @params { get; set; }

        }
        public class eventPara
        {
            public string sendTime { get; set; }
            public string ability { get; set; }
            public List<events> events { get; set; }
        }
        public class events
        {
            public string eventId { get; set; }
            public string srcIndex { get; set; }
            public string srcType { get; set; }
            public string srcName { get; set; }
            public int? eventType { get; set; }
            public int? status { get; set; }
            public int? eventLvl { get; set; }
            public int? timeout { get; set; }
            public string happenTime { get; set; }
            // public string srcParentIndex { get; set; }
            //public eventDetail eventDetails {get;set;}
        }
        public class pushEevent
        {
            public string srcIndex { get; set; }//设备编号
            public int eventType { get; set; }//事件类型
            public string eventTypeName { get; set; }//事件类型名称
            public string happenTime { get; set; }//发生事件
            public string srcName { get; set; }//事件源名称

        }
        public class eventDetail
        {
            public int eventType { get; set; }
            public string ability { get; set; }
            public string srcIndex { get; set; }
            public string srcType { get; set; }
            public string srcName { get; set; }
            public string regionIndexCode { get; set; }
            public string regionName { get; set; }
            public string locationIndexCode { get; set; }

            public string locationName { get; set; }

        }
        #region 事件联动
        //public class eventDetail
        //{
        //    public int eventType { get; set; }
        //    public string ability { get; set; }
        //    public string srcIndex { get; set; }
        //    public string srcType { get; set; }
        //    public string srcName { get; set; }
        //    public string regionIndexCode { get; set; }
        //    public string regionName { get; set; }
        //    public string locationIndexCode { get; set; }

        //    public string locationName { get; set; }
        //    public object? data { get; set; }
        //}
        //public class events
        //{
        //    public string eventId { get; set; }
        //    public string srcIndex { get; set; }
        //    public string srcType { get; set; }
        //    public string srcName { get; set; }
        //    public int eventType { get; set; }
        //    public int status { get; set; }

        //    public int timeout { get; set; }
        //    public string eventName { get; set; }
        //    public string happenTime { get; set; }
        //    public string stopTime { get; set; }
        //    public string remark { get; set; }

        //    public eventDetail eventDetails { get; set; }
        //}
        //public class eventPara { 
        //    public string sendTime { get; set; }
        //    public string ability { get; set; }
        //    public String[] uids { get; set; }
        //    public String[] clients { get; set; }
        //    public List<events> events { get; set; }
        //}
        //public class rcvEvents
        //{
        //    public string method { get; set; }
        //    public eventPara @params{get;set;}

        // }
        #endregion
        //门禁事件回调
        [HttpPost]
        [AllowAnonymous]
        [TypeFilter(typeof(IgonreLoginFilter))]
        public string DoorEventRcv([FromBody] rcvEvents e)
        {
            var data = Newtonsoft.Json.JsonConvert.SerializeObject(e);
            _chatHubContext.Clients.All.SendAsync("EventRcv_test", data);
            //CallThisDot("a5e75140e8b84da1aa1e4b477af086f4", 2);//调预置点
            //门禁
            //foreach (var item in e.@params.events)
            //{

            //    //List<int> doorEvents = new List<int>() { 198657, 198913, 198916, 198919 };
            //    //var eventType_item = (int)item.eventType;
            //    //if (doorEvents.Contains(eventType_item))
            //    //{
            //    //    CallThisDot("a5e75140e8b84da1aa1e4b477af086f4",2);//调预置点
            //       //var data = Newtonsoft.Json.JsonConvert.SerializeObject(item);
            //       // _chatHubContext.Clients.All.SendAsync("EventRcv", data);
            //    //}
            //}
            return "ok";
        }
        //调预置点
        [HttpPost]
        [AllowAnonymous]
        [TypeFilter(typeof(IgonreLoginFilter))]
        public string CallThisDot(string cameraIndexCode, int presetIndex)
        {
            HttpUtillib.SetPlatformInfo("24365906", "XlaLwOqHVWVE3CBa8GOC", "222.173.103.226", 10133, true);
            string uri = "/artemis/api/video/v1/ptzs/controlling";
            var para = "{\"cameraIndexCode\":\"" + cameraIndexCode + "\",\"action\":0,\"command\":\"GOTO_PRESET\",\"speed\":50,\"presetIndex\":" + presetIndex + "}";
            byte[] result = HttpUtillib.HttpPost(uri, para, 15);
            string tmp = System.Text.Encoding.UTF8.GetString(result);
            return tmp;
        }
        #endregion

        #region 门禁信息
        //调预置点
        [HttpPost]
        [AllowAnonymous]
        [TypeFilter(typeof(IgonreLoginFilter))]
        public string MJInfoList()
        {
            int pageNo = 1;
            int pageSize = 10;
            string startTime = "2021-08-05T11:30:08+08:00";
            string endTime = "2021-08-22T17:30:08+08:00";
            string receiveStartTime = "2021-08-05T17:30:08+08:00";
            string receiveEndTime = "2021-08-22T17:30:08+08:00";
            HttpUtillib.SetPlatformInfo("24365906", "XlaLwOqHVWVE3CBa8GOC", "222.173.103.226", 10133, true);
            string uri = "/artemis/api/acs/v2/door/events";
            var para = "{\"pageNo\":" + pageNo + ",\"pageSize\":" + pageSize + ",\"startTime\":\"" + startTime + "\",\"endTime\":\"" + endTime + "\",\"receiveStartTime\":\"" + receiveStartTime + "\",\"receiveEndTime\":\"" + receiveEndTime + "\"}";
            var result = HttpUtillib.HttpPost(uri, para, 15);
            string tmp = System.Text.Encoding.UTF8.GetString(result);
            return tmp;
        }
        #endregion

        #region 详情页3D数据接口
        [AllowAnonymous]
        [HttpGet]
        [TypeFilter(typeof(IgonreLoginFilter))]
        public string LoadDataInfo_3D(string sid)
        {
            int StationID = Convert.ToInt32(sid);
            var pationList = _ISws_StationService.GetStationOfRtu(StationID).ToList();
            List<DeviceData> result = new List<DeviceData>() { };
            if (pationList.Count > 0)
            {
                var rtuids = "";
                List<RtuJKInfo> jklist = new List<RtuJKInfo>();
                var rtuidlist = pationList.Where(r => r.RTUID.ToString() != "").Select(r => r.RTUID).Distinct();
                if (rtuidlist.Count() > 0)
                {

                    rtuids = string.Join(",", rtuidlist);
                    jklist = _ISws_DeviceInfo02Service.GetALLJKWaterQuality(rtuids).ToList();
                }
                int[] strArr = { 2000, 2001, 2002 };
                string[] NameArr = { "进水压力", "出水压力", "设定压力" };

                if (jklist.Count > 0)
                {

                    foreach (var item_partion in pationList)
                    {

                        var rtuid = Convert.ToInt32(item_partion.RTUID);
                        var jk = jklist.Where(r => r.RTUID == rtuid).FirstOrDefault();
                        if (jk != null)
                        {
                            DeviceData item = new DeviceData();
                            item.partitionName = item_partion.ItemName;
                            item.datas = new List<DataDetail>();
                            if (item_partion.Partition == 6)
                            {
                                string[] strArr6 = { "4500", "4501", "4502" };
                                string[] NameArr6 = { "PH", "余氯", "浊度" };
                                string[] UnitArr6 = { "", "mg/L", "NTU" };
                                for (var i = 0; i < strArr6.Length; i++)
                                {
                                    DataDetail dd = new DataDetail();
                                    dd.name = NameArr6[i];
                                    dd.unit = UnitArr6[i];
                                    dd.value = jk.AnalogValues.ContainsKey(strArr6[i]) ? string.Format("{0:N2}", Convert.ToDouble(jk.AnalogValues[strArr6[i]])) : "0.00";
                                    item.datas.Add(dd);
                                }
                            }
                            else
                            {
                                for (var i = 0; i < strArr.Length; i++)
                                {
                                    DataDetail dd = new DataDetail();
                                    dd.name = NameArr[i];
                                    dd.unit = "MPa";
                                    var dataid = strArr[i] + (item_partion.Partition - 1) * 500 + "";
                                    dd.value = jk.AnalogValues.ContainsKey(dataid) ? string.Format("{0:N2}", Convert.ToDouble(jk.AnalogValues[dataid])) : "0.00";
                                    item.datas.Add(dd);
                                }
                                //瞬时流量为多个泵的加值 
                                var flow = "0.00";
                                var infdata1 = jk.AnalogValues.ContainsKey((2030 + (item_partion.Partition - 1) * 500).ToString()) ? jk.AnalogValues[(2030 + (item_partion.Partition - 1) * 500).ToString()] : null;
                                var infdata2 = jk.AnalogValues.ContainsKey((2032 + (item_partion.Partition - 1) * 500).ToString()) ? jk.AnalogValues[(2032 + (item_partion.Partition - 1) * 500).ToString()] : null;
                                var infdata3 = jk.AnalogValues.ContainsKey((2034 + (item_partion.Partition - 1) * 500).ToString()) ? jk.AnalogValues[(2034 + (item_partion.Partition - 1) * 500).ToString()] : null;
                                var infdata4 = jk.AnalogValues.ContainsKey((2036 + (item_partion.Partition - 1) * 500).ToString()) ? jk.AnalogValues[(2036 + (item_partion.Partition - 1) * 500).ToString()] : null;
                                if (infdata1 == null && infdata2 == null && infdata3 == null && infdata4 == null)
                                {

                                }
                                else
                                {
                                    var flow_1 = Convert.ToDouble(infdata1) + Convert.ToDouble(infdata2) + Convert.ToDouble(infdata3) + Convert.ToDouble(infdata4);
                                    flow = string.Format("{0:N2}", flow_1);
                                }
                                DataDetail dd1 = new DataDetail();
                                dd1.name = "瞬时流量";
                                dd1.unit = "m³/h";
                                dd1.value = flow;
                                item.datas.Add(dd1);
                            }
                            result.Add(item);
                        }
                    }

                }

            }
            if (result.Count > 0)
            {
                var rel = new
                {
                    Code = 1,
                    Message = "数据获取成功",
                    DataList = result
                };
                return JsonConvert.SerializeObject(rel);
            }
            else
            {
                var rel = new
                {
                    Code = 0,
                    Message = "数据获取失败"
                };
                return JsonConvert.SerializeObject(rel);
            }
        }
        #endregion

        #region 移动端GPS定位
        /// <summary>
        /// 上传坐标
        /// </summary>
        /// <param name="uerid">用户ID</param>
        /// <param name="lat">纬度</param>
        /// <param name="lng">经度</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public string UpdatePosition(long uerid, double lat, double lng)
        {
            //更新数据
            var result = _sws_GPSModuleService.UpdatePosition(uerid, lng, lat);
            if (result > 0)
            {
                var rel = new
                {
                    Code = 1,
                    Message = "上传成功"
                };
                return JsonConvert.SerializeObject(rel);
            }
            return JsonConvert.SerializeObject(new { Code = 2, Message = "上传失败" });
        }
        #endregion


    }
    class PumStation
    {
        public int StationID;
        public string StationName;
        public List<PumEqu> pumEqus;
        public double Lng;
        public double Lat;
    }

    class PumEqu
    {
        public long DeviceID;
        public string DeviceName;
    }

    class WoHistory
    {
        public WoEvents woEvents;
        public WoWooperation woWooperations;
    }
    class DeviceData
    {
        public string partitionName { get; set; }
        public List<DataDetail> datas { get; set; }
    }
    class DataDetail
    {
        public string value { get; set; }
        public string name { get; set; }
        public string unit { get; set; }
    }
}
