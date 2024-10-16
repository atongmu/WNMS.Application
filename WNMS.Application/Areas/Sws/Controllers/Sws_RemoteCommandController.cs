using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDBHelper;
using WNMS.Application.Utility.Filters;
using WNMS.IService;
using WNMS.Model.CustomizedClass;
using WNMS.Model.DataModels;
using WNMS.Utility;

namespace WNMS.Application.Areas.Sws.Controllers
{
    [Area("Sws")]
    public class Sws_RemoteCommandController : Controller
    {
        private ISysUserService _UserService = null;
        private ISws_DeviceInfo01Service _sws_DeviceInfo01Service = null;
        private ISws_AccessControlService _AccessControlService = null;
        private readonly ILogger<Sws_RemoteCommandController> logger = null;
        public ISysUserService _ISys_UserService = null;
        Command.HttpHelper hp = new Command.HttpHelper();
        string service_ip = StaticConstraint.Service_Ip;
        public Sws_RemoteCommandController(ISysUserService sysUserService,
            ISws_DeviceInfo01Service sws_DeviceInfo01Service,
            ISws_AccessControlService sws_AccessControlService,
            ISysUserService _userservice,
            ILogger<Sws_RemoteCommandController> myLogger
            )
        {
            _UserService = sysUserService;
            _sws_DeviceInfo01Service = sws_DeviceInfo01Service;
            _AccessControlService = sws_AccessControlService;
            _ISys_UserService = _userservice;
            logger = myLogger;
        }
        public IActionResult Index()
        {
            return View();
        }
        [TypeFilter(typeof(IgonreActionFilter))]
        public ActionResult CheckUser()
        {
            return View();
        }
        [HttpPost]
        [TypeFilter(typeof(IgonreActionFilter))]
        //验证用户名密码
        public ActionResult CheckUsername(string username, string password, string Short)
        {
            //const string VendorCodeString = "LA8JG4yEkEuefG2G9uBBcYyGy/dFZCOYNEgHCBR7CR48PQRZO/GGua3y5fBUIKDhIAqfukDgZ8pF45v5V5+D+4kEr+N5nKbuNnayAikbSyuHXYHN7HaPx/ObHXrXD/8jzNZ11rL990zsIDTXzGsMOSaR7N7mZiUINDRBB+aUrEFUiqvAIiJ2gwFZIRWiTZsV/ubfpqIvmevEhI4LrLQoe8H0nOafJjGUMD5Xqy6v71rIHyE0YPnnhrQmLWA3vOUUJPkLQjPUY8wcCyONGY6ee9FV5akJLDiwYcUad4DLIw9o+lz5yWq7aLUgV2gFE/5fNIK01R93Yi5zm/gQXWVcwza78zrtSVurCYKsxFFi4wttNtxphw98NPVYgik3QirP6UNn7DZACqvOM40S3W08qq6F8MMJneyhTMEIJga6taUpkT+EN+mdZgWcw+kOryleMPcM6s5WxJpqmFbA+6/Ow7rfxCq25yQWepVTZsp/vLgLUDDSKz+yrrUftq1kNAu38/Juapj0GAIkjUHQ30UcFJD2cxKRBb0TIY3fN81HdO9PpBvxza+7HT+TZEtGDrLJB0Ay8zTwJx4xzSxx8TTPR/vMmYzn2qApJylsXFhiAq4S+3uuTxx1qMgYIfkSwq9oJ3EeihCALRxfa6cgmx8Yrkt4ttAay14oSJOyn2OrUB4FzGHBlHF9zH8d8FGj7te3Kd2lpR4bl1YB/E1MBsUa62TZa03Bf5T3bSKzQiela3jiAO/go60Rf2rzNgTwcfjp8ielYFd4OQ81dICJtSZ9UdqwOZNZj7zBCi/8+oOR4mZHbhGFBdMwLjyZLZhfaJRKt5NZSMbQPa0YVK57Qx+BUqvYHB2ymo+YnV3p+gG7ltoJ4J4dgbSBv0G49HjwbfMne8YfVlWQxVXONqAxuOmZhpxd2cMB2Ia5xgu3XKI2a0+9L0OGEGwooCo3FDldevNiaMAgwAX6xx+K7qySjGZUwg==";
            //SuperDog.DogFeature dogFeature = new SuperDog.DogFeature();
            //SuperDog.Dog dog = new SuperDog.Dog(dogFeature);
            //SuperDog.DogStatus dogStatus = dog.Login(VendorCodeString);
            //string data = "no";
            //if (dogStatus == SuperDog.DogStatus.StatusOk)
            //{
            //    dogStatus = dog.Logout();
            //    //验证输入账号是否为管理员 
            //    string name = WNMS.Utility.Encrypt.Encrypt.MD5Encoding(username, "WNMS@Standard");
            //    var user = _UserService.Query<SysUser>(u => u.Account == name).FirstOrDefault();
            //    string pword = WNMS.Utility.Encrypt.Encrypt.MD5Encoding(password, "WNMS@Standard");
            //    if (user != null && pword == user.Password)
            //    {
            //        if (user.IsAdmin)
            //        {
            //            data = "ok";
            //        }
            //        else
            //        {
            //            data = "noAuthority";
            //        }

            //    }
            //    else
            //    {
            //        data = "no";
            //    }
            //    return Content(data);
            //}
            //else
            //{
            //    return Content("NotFoundDog");
            //}

            //验证输入账号是否为管理员 
            string data = "";
            string name = WNMS.Utility.Encrypt.Encrypt.MD5Encoding(username, "WNMS@Standard");
            var user = _UserService.Query<SysUser>(u => u.Account == name).FirstOrDefault();
            string pword = WNMS.Utility.Encrypt.Encrypt.MD5Encoding(password, "WNMS@Standard");
            if (user != null && pword == user.Password)
            {
                if (user.IsAdmin)
                {
                    data = "ok";
                }
                else
                {
                    data = "noAuthority";
                }

            }
            else
            {
                data = "no";
            }
            return Content(data);


            //string data = "";
            //int userID = int.Parse(User.FindFirstValue("UserID"));
            //var user = _UserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
            //string pword = WNMS.Utility.Encrypt.Encrypt.MD5Encoding(password, "WNMS@Standard");
            //if (username == user.Account & pword == user.Password)
            //{
            //    if (user.IsAdmin)
            //    {
            //        data = "ok";
            //    }
            //    else
            //    {
            //        data = "noAuthority";
            //    }

            //}
            //else
            //{
            //    data = "no";
            //}
            //return Content(data);

        }
        //启动
        [HttpPost]
        public async Task<IActionResult> LoadTurnOn(int sid, int paid)
        {
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = _ISys_UserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
            var deviceinfo = _sws_DeviceInfo01Service.Query<SwsDeviceInfo01>(r => r.StationId == sid && r.Partition == paid).FirstOrDefault();
            string CmdGuid = Guid.NewGuid().ToString();
            string aa = "ok";
            try
            {

                //Task tsak = Task.Run(() =>
                //{
                //    aa = hp..HttpGet(service_ip + "/WnmsWebService/DeviceCtrl?guid=CF27D377-8073-4966-BA86-061A5E246E19&RTUID=" + deviceinfo.Rtuid + "&cmdType=0", "");
                //    return Content("\"{\\\"Message\\\":\\\"" + "通知成功" + "\\\"}\"");
                //});
                //return Content("\"{\\\"Message\\\":\\\"" + "发送成功" + "\\\"}\"");
                aa = await hp.HttpGetAsync(service_ip + "/WnmsWebService/DeviceCtrl?guid=CF27D377-8073-4966-BA86-061A5E246E19&RTUID=" + deviceinfo.Rtuid + "&cmdType=0", "");
                logger.LogInformation(user.NickName + " 启动设备-" + deviceinfo.DeviceName);
                return Content("\"{\\\"Message\\\":\\\"" + "发送成功" + "\\\"}\"");
            }
            catch (Exception e)
            {
                return Content("\"{\\\"Message\\\":\\\"" + e.Message + "\\\"}\"");
            }
        }
        //停止
        public async Task<IActionResult> LoadTurnOff(int sid, int paid)
        {
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = _ISys_UserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
            string CmdGuid = Guid.NewGuid().ToString();
            string aa = "";
            var deviceinfo = _sws_DeviceInfo01Service.Query<SwsDeviceInfo01>(r => r.StationId == sid && r.Partition == paid).FirstOrDefault();
            try
            {
                //aa = hp.HttpGet(service_ip + "/WnmsWebService/DeviceCtrl?guid=CF27D377-8073-4966-BA86-061A5E246E19&equipmentId=" + id + "&cmdType=1", "");
                //Task tsak = Task.Run(() =>
                //{
                //    aa = hp.HttpGet(service_ip + "/WnmsWebService/DeviceCtrl?guid=CF27D377-8073-4966-BA86-061A5E246E19&RTUID=" + deviceinfo.Rtuid + "&cmdType=1", "");
                //    return Content("\"{\\\"Message\\\":\\\"" + "通知成功" + "\\\"}\"");
                //});
                aa = await hp.HttpGetAsync(service_ip + "/WnmsWebService/DeviceCtrl?guid=CF27D377-8073-4966-BA86-061A5E246E19&RTUID=" + deviceinfo.Rtuid + "&cmdType=1", "");
                logger.LogInformation(user.NickName + " 停止设备-" + deviceinfo.DeviceName);
                return Content("\"{\\\"Message\\\":\\\"" + "发送成功" + "\\\"}\"");

            }
            catch (Exception e)
            {
                return Content("\"{\\\"Message\\\":\\\"" + e.Message + "\\\"}\"");
            }



        }
        //设定压力
        public ActionResult SetPressure(int sid, int paid)
        {
            //获取当前压力值
            var deviceInfo01 = _sws_DeviceInfo01Service.Query<SwsDeviceInfo01>(r => r.StationId == sid && r.Partition == paid).FirstOrDefault();
            if (deviceInfo01 != null)
            {
                var rtuID = deviceInfo01.Rtuid;
                string macJson = "{\"RTUID\":{'$in':[" + rtuID + "]}}";
                var collections = new MongoDBHelper<RtuJKInfo>(DateTime.Now.Year.ToString());
                var jklist = collections.Find("Sws_DeviceJKInfo", macJson, "UpdateTime").FirstOrDefault();
                ViewBag.PressureSet = jklist.AnalogValues.ContainsKey((2002 + (paid - 1) * 500).ToString()) ? jklist.AnalogValues[(2002 + (paid - 1) * 500).ToString()] : "";
            }
            else
            {
                ViewBag.PressureSet = "";
            }
            //ViewBag.PressureSet = 0.6;
            ViewBag.sid = sid;
            ViewBag.paid = paid;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> LoadSetPressure(int sid, int paid, double Pressure)
        {
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = _ISys_UserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
            var deviceinfo = _sws_DeviceInfo01Service.Query<SwsDeviceInfo01>(r => r.StationId == sid && r.Partition == paid).FirstOrDefault();
            string CmdGuid = Guid.NewGuid().ToString();
            string aa = "";
            try
            {
                //double pre = Pressure;
                //Task tsak = Task.Run(() =>
                //{
                //    aa = hp.HttpGet(service_ip + "/WnmsWebService/DeviceCtrl?guid=CF27D377-8073-4966-BA86-061A5E246E19&RTUID=" + deviceinfo.Rtuid + "&cmdType=8&value=" + pre + "", "");
                //    return Content("\"{\\\"Message\\\":\\\"" + "通知成功" + "\\\"}\"");
                //});
                string b = service_ip + "/WnmsWebService/DeviceCtrl?guid=CF27D377-8073-4966-BA86-061A5E246E19&RTUID=" + deviceinfo.Rtuid + "&cmdType=8&value=" + Pressure + "";
                aa = await hp.HttpGetAsync(service_ip + "/WnmsWebService/DeviceCtrl?guid=CF27D377-8073-4966-BA86-061A5E246E19&RTUID=" + deviceinfo.Rtuid + "&cmdType=8&value=" + Pressure + "", "");
                logger.LogInformation(user.NickName + " 修改" + deviceinfo.DeviceName + "设定压力");
                return Content("\"{\\\"Message\\\":\\\"" + "发送成功" + "\\\"}\"");


            }
            catch (Exception e)
            {

                return Content("\"{\\\"Message\\\":\\\"" + e.Message + "\\\"}\"");
            }

        }
        //变频复位
        [HttpPost]
        public async Task<IActionResult> LoadVFDReset(int sid, int paid)
        {
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = _ISys_UserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
            string CmdGuid = Guid.NewGuid().ToString();
            string aa = "";
            var deviceinfo = _sws_DeviceInfo01Service.Query<SwsDeviceInfo01>(r => r.StationId == sid && r.Partition == paid).FirstOrDefault();
            try
            {
                //Task tsak = Task.Run(() =>
                //{
                //    aa = hp.HttpGet("http://192.168.61.16:8733/WnmsWebService/DeviceCtrl?guid=CF27D377-8073-4966-BA86-061A5E246E19&RTUID=" + deviceinfo.Rtuid + "&cmdType=0", "");
                //    return Content("\"{\\\"Message\\\":\\\"" + "通知成功" + "\\\"}\"");
                //});
                aa = await hp.HttpGetAsync(service_ip + "/WnmsWebService/DeviceCtrl?guid=CF27D377-8073-4966-BA86-061A5E246E19&RTUID=" + deviceinfo.Rtuid + "&cmdType=0", "");
                logger.LogInformation(user.NickName + " 变频复位-" + deviceinfo.DeviceName);
                return Content("\"{\\\"Message\\\":\\\"" + "发送成功" + "\\\"}\"");
            }
            catch (Exception e)
            {
                return Content("\"{\\\"Message\\\":\\\"" + e.Message + "\\\"}\"");
            }

        }

        //开门
        public async Task<IActionResult> LoadOpenDoor(int sid, int paid)
        {
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = _ISys_UserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
            string CmdGuid = Guid.NewGuid().ToString();
            string aa = "";
            //修改为获取门禁信息  http://127.0.0.1:8733/WnmsWebService/AccessControl?guid=CF27D377-8073-4966-BA86-061A5E246E19&uType=1&iGateWayIndex=-1&sLoginName=admin&sPwd=sanli1234&sIP=172.26.128.5&uPort=8000
            var model = _AccessControlService.Query<SwsAccessControl>(r => r.StationId == sid).FirstOrDefault();
            if (model != null)
            {
                //  Task tsak = Task.Run(() =>
                //{
                //    aa = hp.HttpGet(service_ip + "/WnmsWebService/AccessControl?guid=CF27D377-8073-4966-BA86-061A5E246E19&uType=1&iGateWayIndex=-1&sLoginName=" + model.UserName + "&sPwd=" + model.PassWord + "&sIP=" + model.Ip + "&uPort=" + model.Port, "");
                //    return Content("\"{\\\"Message\\\":\\\"" + "通知成功" + "\\\"}\"");

                //});
                aa = await hp.HttpGetAsync(service_ip + "/WnmsWebService/AccessControl?guid=CF27D377-8073-4966-BA86-061A5E246E19&uType=1&iGateWayIndex=-1&sLoginName=" + model.UserName + "&sPwd=" + model.PassWord + "&sIP=" + model.Ip + "&uPort=" + model.Port, "");
                logger.LogInformation(user.NickName + " 开门-" + model.AccessName);
                return Content("\"{\\\"Message\\\":\\\"" + "发送成功" + "\\\"}\"");
            }
            else
            {
                return Content("\"{\\\"Message\\\":\\\"" + "暂无门禁信息" + "\\\"}\"");
            }
            //var deviceinfo = _sws_DeviceInfo01Service.Query<SwsDeviceInfo01>(r => r.StationId == sid && r.Partition == paid).FirstOrDefault();
            //try
            //{
            //    Task tsak = Task.Run(() =>
            //  {
            //      aa = hp.HttpGet(service_ip + "/WnmsWebService/DeviceCtrl?guid=CF27D377-8073-4966-BA86-061A5E246E19&RTUID=" + deviceinfo.Rtuid + "&cmdType=10", "");
            //      return Content("\"{\\\"Message\\\":\\\"" + "通知成功" + "\\\"}\"");

            //  });
            //    return Content("\"{\\\"Message\\\":\\\"" + "发送成功" + "\\\"}\"");

            //}
            //catch (Exception e)
            //{
            //    return Content("\"{\\\"Message\\\":\\\"" + e.Message + "\\\"}\"");
            //}



        }
        //开灯
        public async Task<IActionResult> LoadOpenligth(int sid, int paid,int value)
        {
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = _ISys_UserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
            string CmdGuid = Guid.NewGuid().ToString();
            string aa = "";
            var deviceinfo = _sws_DeviceInfo01Service.Query<SwsDeviceInfo01>(r => r.StationId == sid && r.Partition == paid).FirstOrDefault();
            try
            {
                // Task tsak = Task.Run(() =>
                //{
                //    aa = hp.HttpGet(service_ip + "/WnmsWebService/DeviceCtrl?guid=CF27D377-8073-4966-BA86-061A5E246E19&RTUID=" + deviceinfo.Rtuid + "&cmdType=12", "");
                //    return Content("\"{\\\"Message\\\":\\\"" + "通知成功" + "\\\"}\"");
                //});

                string a = service_ip + "/WnmsWebService/DeviceCtrl?guid=CF27D377-8073-4966-BA86-061A5E246E19&RTUID=" + deviceinfo.Rtuid + "&cmdType="+ value;
                aa = await hp.HttpGetAsync(service_ip + "/WnmsWebService/DeviceCtrl?guid=CF27D377-8073-4966-BA86-061A5E246E19&RTUID=" + deviceinfo.Rtuid + "&cmdType="+value, "");
                logger.LogInformation(user.NickName + " 开灯-" + deviceinfo.DeviceName);
                return Content("\"{\\\"Message\\\":\\\"" + "发送成功" + "\\\"}\"");
            }
            catch (Exception e)
            {
                return Content("\"{\\\"Message\\\":\\\"" + e.Message + "\\\"}\"");
            }
        }

        //开除湿器
        public async Task<IActionResult> LoadOpenDehumidifier(int sid, int paid, int value)
        {
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = _ISys_UserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
            string CmdGuid = Guid.NewGuid().ToString();
            string aa = "";
            var deviceinfo = _sws_DeviceInfo01Service.Query<SwsDeviceInfo01>(r => r.StationId == sid && r.Partition == paid).FirstOrDefault();
            try
            {
                // Task tsak = Task.Run(() =>
                //{
                //    aa = hp.HttpGet(service_ip + "/WnmsWebService/DeviceCtrl?guid=CF27D377-8073-4966-BA86-061A5E246E19&RTUID=" + deviceinfo.Rtuid + "&cmdType=12", "");
                //    return Content("\"{\\\"Message\\\":\\\"" + "通知成功" + "\\\"}\"");
                //});

                string a = service_ip + "/WnmsWebService/DeviceCtrl?guid=CF27D377-8073-4966-BA86-061A5E246E19&RTUID=" + deviceinfo.Rtuid + "&cmdType="+value;
                aa = await hp.HttpGetAsync(service_ip + "/WnmsWebService/DeviceCtrl?guid=CF27D377-8073-4966-BA86-061A5E246E19&RTUID=" + deviceinfo.Rtuid + "&cmdType=" + value, "");
                logger.LogInformation(user.NickName + " 开灯-" + deviceinfo.DeviceName);
                return Content("\"{\\\"Message\\\":\\\"" + "发送成功" + "\\\"}\"");
            }
            catch (Exception e)
            {
                return Content("\"{\\\"Message\\\":\\\"" + e.Message + "\\\"}\"");
            }
        }
        ////开阀门
        //public async Task<IActionResult> LoadOpenFa(int sid, int paid)
        //{
        //    string CmdGuid = Guid.NewGuid().ToString();
        //    string aa = "";
        //    var deviceinfo = _sws_DeviceInfo01Service.Query<SwsDeviceInfo01>(r => r.StationId == sid && r.Partition == paid).FirstOrDefault();
        //    try
        //    {
        //        Task tsak = Task.Run(() =>
        //        {
        //            aa = hp.HttpGet(service_ip + "/WnmsWebService/DeviceCtrl?guid=CF27D377-8073-4966-BA86-061A5E246E19&RTUID=" + deviceinfo.Rtuid + "&cmdType=13", "");
        //            return Content("\"{\\\"Message\\\":\\\"" + "通知成功" + "\\\"}\"");
        //        });
        //        return Content("\"{\\\"Message\\\":\\\"" + "发送成功" + "\\\"}\"");
        //    }
        //    catch (Exception e)
        //    {
        //        return Content("\"{\\\"Message\\\":\\\"" + e.Message + "\\\"}\"");
        //    }
        //}
        //开阀门
        public async Task<IActionResult> LoadOpenFa(int id, int valveNum)
        {
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = _ISys_UserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
            string CmdGuid = Guid.NewGuid().ToString();
            string aa = "";
            int cmdType = 0;
            if (valveNum == 1)
            {
                cmdType = 13;
            }
            else if (valveNum == 2)
            {
                cmdType = 15;
            }
            else if (valveNum == 3)
            {
                cmdType = 17;
            }
            else if (valveNum == 4)
            {
                cmdType = 19;
            }
            else if (valveNum == 5)
            {
                cmdType = 21;
            }
            try
            {
                //Task tsak = Task.Run(() =>
                //{
                //    aa = hp.HttpGet(service_ip + "/WnmsWebService/DeviceCtrl?guid=CF27D377-8073-4966-BA86-061A5E246E19&RTUID=" + id + "&cmdType=" + cmdType + "", "");
                //    return Content("\"{\\\"Message\\\":\\\"" + "通知成功" + "\\\"}\"");
                //});
                aa = await hp.HttpGetAsync(service_ip + "/WnmsWebService/DeviceCtrl?guid=CF27D377-8073-4966-BA86-061A5E246E19&RTUID=" + id + "&cmdType=" + cmdType + "", "");
                logger.LogInformation(user.NickName + " 开启阀门");
                return Content("\"{\\\"Message\\\":\\\"" + "发送成功" + "\\\"}\"");
            }
            catch (Exception e)
            {
                return Content("\"{\\\"Message\\\":\\\"" + e.Message + "\\\"}\"");
            }
        }
        //关阀门
        public async Task<IActionResult> LoadCloseFa(int id, int valveNum)
        {
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = _ISys_UserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
            string CmdGuid = Guid.NewGuid().ToString();
            string aa = "";
            int cmdType = 0;
            if (valveNum == 1)
            {
                cmdType = 14;
            }
            else if (valveNum == 2)
            {
                cmdType = 16;
            }
            else if (valveNum == 3)
            {
                cmdType = 18;
            }
            else if (valveNum == 4)
            {
                cmdType = 20;
            }
            else if (valveNum == 5)
            {
                cmdType = 22;
            }
            try
            {
                //Task tsak = Task.Run(() =>
                //{
                //    aa = hp.HttpGet(service_ip + "/WnmsWebService/DeviceCtrl?guid=CF27D377-8073-4966-BA86-061A5E246E19&RTUID=" + id + "&cmdType=" + cmdType + "", "");
                //    return Content("\"{\\\"Message\\\":\\\"" + "通知成功" + "\\\"}\"");
                //});

                aa = await hp.HttpGetAsync(service_ip + "/WnmsWebService/DeviceCtrl?guid=CF27D377-8073-4966-BA86-061A5E246E19&RTUID=" + id + "&cmdType=" + cmdType + "", "");
                logger.LogInformation(user.NickName + " 关闭阀门");
                return Content("\"{\\\"Message\\\":\\\"" + "发送成功" + "\\\"}\"");
            }
            catch (Exception e)
            {
                return Content("\"{\\\"Message\\\":\\\"" + e.Message + "\\\"}\"");
            }
        }


        //开启使能
        public async Task<IActionResult> LoadOpenSn(int id)
        {
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = _ISys_UserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
            string CmdGuid = Guid.NewGuid().ToString();
            string aa = "";
            int cmdType = 26;

            try
            {
                //Task tsak = Task.Run(() =>
                //{
                //    aa = hp.HttpGet(service_ip + "/WnmsWebService/DeviceCtrl?guid=CF27D377-8073-4966-BA86-061A5E246E19&RTUID=" + id + "&cmdType=" + cmdType + "", "");
                //    return Content("\"{\\\"Message\\\":\\\"" + "通知成功" + "\\\"}\"");
                //});
                aa = await hp.HttpGetAsync(service_ip + "/WnmsWebService/DeviceCtrl?guid=CF27D377-8073-4966-BA86-061A5E246E19&RTUID=" + id + "&cmdType=" + cmdType + "", "");
                logger.LogInformation(user.NickName + " 开启使能");
                return Content("\"{\\\"Message\\\":\\\"" + "发送成功" + "\\\"}\"");
            }
            catch (Exception e)
            {
                return Content("\"{\\\"Message\\\":\\\"" + e.Message + "\\\"}\"");
            }
        }
        //关闭使能
        public async Task<IActionResult> LoadCloseSn(int id)
        {
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = _ISys_UserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
            string CmdGuid = Guid.NewGuid().ToString();
            string aa = "";
            int cmdType = 25;

            try
            {
                //Task tsak = Task.Run(() =>
                //{
                //    aa = hp.HttpGet(service_ip + "/WnmsWebService/DeviceCtrl?guid=CF27D377-8073-4966-BA86-061A5E246E19&RTUID=" + id + "&cmdType=" + cmdType + "", "");
                //    return Content("\"{\\\"Message\\\":\\\"" + "通知成功" + "\\\"}\"");
                //});
                aa = await hp.HttpGetAsync(service_ip + "/WnmsWebService/DeviceCtrl?guid=CF27D377-8073-4966-BA86-061A5E246E19&RTUID=" + id + "&cmdType=" + cmdType + "", "");
                logger.LogInformation(user.NickName + " 关闭使能");
                return Content("\"{\\\"Message\\\":\\\"" + "发送成功" + "\\\"}\"");
            }
            catch (Exception e)
            {
                return Content("\"{\\\"Message\\\":\\\"" + e.Message + "\\\"}\"");
            }
        }

        #region 7泵远程控制
        [HttpPost]
        public async Task<IActionResult> LoadRemote(int sid, int paid, string cmtype)
        {
            var deviceinfo = _sws_DeviceInfo01Service.Query<SwsDeviceInfo01>(r => r.StationId == sid && r.Partition == paid).FirstOrDefault();
            string CmdGuid = Guid.NewGuid().ToString();
            long reid = 0;//远程控制id
            switch (cmtype)
            {
                //远程使能
                case "AutoTb2":
                    reid = 105;
                    break;
                case "TeleTb2":
                    reid = 106;
                    break;
                case "AutoTb":
                    reid = 25;
                    break;
                case "TeleTb":
                    reid = 26;
                    break;
                //1-7泵启停
                case "ValueUp1":
                    reid = 32;
                    break;
                case "ValueOff1":
                    reid = 33;
                    break;
                case "ValueUp2":
                    reid = 34;
                    break;
                case "ValueOff2":
                    reid = 35;
                    break;
                case "ValueUp3":
                    reid = 36;
                    break;
                case "ValueOff3":
                    reid = 37;
                    break;
                case "ValueUp4":
                    reid = 38;
                    break;
                case "ValueOff4":
                    reid = 39;
                    break;
                case "ValueUp5":
                    reid = 92;
                    break;
                case "ValueOff5":
                    reid = 93;
                    break;
                case "ValueUp6":
                    reid = 94;
                    break;
                case "ValueOff6":
                    reid = 95;
                    break;
                case "ValueUp7":
                    reid = 96;
                    break;
                case "ValueOff7":
                    reid = 97;
                    break;
                case "Start":
                    reid = 0;
                    break;
                case "Stop":
                    reid = 1;
                    break;
                case "Start2":
                    reid = 89;
                    break;
                case "Stop2":
                    reid = 90;
                    break;
                //电动阀启停
                case "ValveUp1":
                    reid = 13;
                    break;
                case "ValveOff1":
                    reid = 14;
                    break;
                case "ValveUp2":
                    reid = 15;
                    break;
                case "ValveOff2":
                    reid = 16;
                    break;
                case "ValveUp3":
                    reid = 17;
                    break;
                case "ValveOff3":
                    reid = 18;
                    break;
                case "ValveUp4":
                    reid = 19;
                    break;
                case "ValveOff4":
                    reid = 20;
                    break;
                case "ValveUp5":
                    reid = 21;
                    break;
                case "ValveOff5":
                    reid = 22;
                    break;
                case "ValveUp6":
                    reid = 101;
                    break;
                case "ValveOff6":
                    reid = 102;
                    break;
                case "ValveUp7":
                    reid = 103;
                    break;
                case "ValveOff7":
                    reid = 104;
                    break;
            }


            string aa = "";
            try
            {
                Task tsak = Task.Run(() =>
                {
                    aa = hp.HttpGet(service_ip + "/WnmsWebService/DeviceCtrl?guid=CF27D377-8073-4966-BA86-061A5E246E19&RTUID=" + deviceinfo.Rtuid + "&cmdType=" + reid + "", "");
                    return Content("\"{\\\"Message\\\":\\\"" + "通知成功" + "\\\"}\"");
                });
                return Content("\"{\\\"Message\\\":\\\"" + "发送成功" + "\\\"}\"");
            }
            catch (Exception e)
            {

                return Content("\"{\\\"Message\\\":\\\"" + e.Message + "\\\"}\"");
            }

        }

        //设定频率、开度、压力
        public ActionResult SetAllValue(int sid, int paid, string cmtype)
        {
            ViewBag.sid = sid;
            ViewBag.paid = paid;
            ViewBag.cmtype = cmtype;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> LoadSetValue(int sid, int paid, string cmtype, double Pressure)
        {
            var deviceinfo = _sws_DeviceInfo01Service.Query<SwsDeviceInfo01>(r => r.StationId == sid && r.Partition == paid).FirstOrDefault();
            string CmdGuid = Guid.NewGuid().ToString();
            string aa = "";
            long reid = 0;//远程控制id
            switch (cmtype)
            {

                case "PressureSet":
                    reid = 8;
                    break;
                case "PressureSet2":
                    reid = 91;
                    break;
                case "FrequencySet1":
                    reid = 40;
                    break;
                case "FrequencySet2":
                    reid = 41;
                    break;
                case "FrequencySet3":
                    reid = 42;
                    break;
                case "FrequencySet4":
                    reid = 43;
                    break;
                case "FrequencySet5":
                    reid = 98;
                    break;
                case "FrequencySet6":
                    reid = 99;
                    break;
                case "FrequencySet7":
                    reid = 100;
                    break;
                case "ValveOpening1":
                    reid = 72;
                    break;
                case "ValveOpening2":
                    reid = 73;
                    break;
                case "ValveOpening3":
                    reid = 74;
                    break;
            }
            try
            {
                Task tsak = Task.Run(() =>
                {
                    aa = hp.HttpGet(service_ip + "/WnmsWebService/DeviceCtrl?guid=CF27D377-8073-4966-BA86-061A5E246E19&RTUID=" + deviceinfo.Rtuid + "&cmdType=" + reid + "&value=" + Pressure + "", "");
                    return Content("\"{\\\"Message\\\":\\\"" + "通知成功" + "\\\"}\"");
                });
                return Content("\"{\\\"Message\\\":\\\"" + "发送成功" + "\\\"}\"");

            }
            catch (Exception e)
            {

                return Content("\"{\\\"Message\\\":\\\"" + e.Message + "\\\"}\"");
            }

        }
        #endregion
    }
}