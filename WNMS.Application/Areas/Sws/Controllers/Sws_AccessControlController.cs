using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WNMS.Application.Utility.Filters;
using WNMS.IService;
using WNMS.Model.DataModels;
namespace WNMS.Application.Areas.Sws.Controllers
{
    [Area("Sws")]
    public class Sws_AccessControlController : Controller
    {
        private ISws_AccessControlService _AccessControlService = null;
        private ISys_DataItemDetailService _DataItemDetailService = null;
        private ISysUserService _UserService = null;
        private ISws_StationService _StationService = null;
        
        public Sws_AccessControlController(ISws_AccessControlService sws_AccessControlService,
            ISys_DataItemDetailService sys_DataItemDetailService,
            ISysUserService sysUserService,
            ISws_StationService sws_StationService)
        {
            _AccessControlService = sws_AccessControlService;
            _DataItemDetailService = sys_DataItemDetailService;
            _UserService = sysUserService;
            _StationService = sws_StationService;
        }
        public IActionResult Index()
        {
            return View();
        }
        #region 表格数据查询
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult QueryAcessControlTable(int pagesize,int pageindex,string accessName,string order,string sort)
        {
            int Totalcount = 0;
            string name = accessName ?? "";
            string filter = " AccessName like '%" + name + "%' ";
            string orderby = sort+ " "+ order;
            var brandItemID = (int)Model.CustomizedClass.Enum.门禁品牌;
            var datalist = _AccessControlService.GetAcessControlTable(brandItemID,pageindex,pagesize , orderby, filter,ref Totalcount);

            return Json(new { total = Totalcount, rows = Newtonsoft.Json.JsonConvert.SerializeObject(datalist) });
        }
        #endregion
        #region 页面操作
        //添加
        public IActionResult AddAccessControlPage()
        {
            SwsAccessControl model = new SwsAccessControl();
            //门禁品牌集合
            var f_brandID = (int)Model.CustomizedClass.Enum.门禁品牌;
            ViewBag.brandList = _DataItemDetailService.Query<SysDataItemDetail>(r => r.FItemId == f_brandID && r.IsEnable == true);
            ViewBag.StationName = "";
          
            return View("_SetAccessControl", model);
        }
        //修改
        
        public IActionResult EditeAccessControlPage(int id)
        {
            var model = _AccessControlService.Query<SwsAccessControl>(r => r.DoorId == id).FirstOrDefault();
            //model.PassWord = WNMS.Encrypt.Decode(model.PassWord);
            //门禁品牌集合
            var f_brandID = (int)Model.CustomizedClass.Enum.门禁品牌;
            ViewBag.brandList = _DataItemDetailService.Query<SysDataItemDetail>(r => r.FItemId == f_brandID && r.IsEnable == true);
            var station = _StationService.Query<SwsStation>(r => r.StationId == model.StationId).FirstOrDefault();
            if (station != null)
            {
                ViewBag.StationName = station.StationName;
                
            }
            else
            {
                ViewBag.StationName ="";
                
            }

            return View("_SetAccessControl", model);
        }
        //表单提交
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult SetAccessControl(SwsAccessControl a)
        {
            //加密
            //string passWord =WNMS.Encrypt.Encode(a.PassWord);   //加密;
            //a.PassWord = passWord;
            if (a.DoorId == 0)//添加
            {
                a.DoorId = _UserService.QueryID("DoorID", "Sws_AccessControl");
                //判断门禁编号是否重复
                var hasmodel = _AccessControlService.Query<SwsAccessControl>(r => r.Num == a.Num).FirstOrDefault();
                if (hasmodel != null)
                {
                    return Content("has");
                }
                //a.DoorId =1;//测试
                var operatenum = _AccessControlService.InsertAccessControl(a);
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
                var hasmodel = _AccessControlService.Query<SwsAccessControl>(r => r.Num == a.Num && r.DoorId!=a.DoorId).FirstOrDefault();
                if (hasmodel != null)
                {
                    return Content("has");
                }

                var operatenum = _AccessControlService.UpdateAccessControl(a);
                if (operatenum > 0)
                {
                    return Content("ok");
                }
                else
                {
                    return Content("no");
                }
            }
        }
        //删除门禁
        public IActionResult DeleteAccessControl(string ids)
        {
            List<int> doorids = new List<string>(ids.Split(",")).ConvertAll(r => int.Parse(r));
            
            var operateNum = _AccessControlService.DeleteAccessControl(doorids);
          
                if (operateNum > 0)
                {
                    return Content("ok");
                }
                else
                {
                    return Content("no");
                }
            
        }
        #endregion
        #region 泵房选择
        [TypeFilter(typeof(IgonreActionFilter))]
        public ActionResult SelectStationInfo()
        {
            return View();
        }

        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult LoadStationList(int pageSize,int pageIndex,string sort,string sortOrder,string searchText,int stationid)
        {
            int Totalcount = 0;
            //string filter = " (a.DoorID is null";
            //if (doorID != 0)
            //{
            //    filter += " or a.DoorID=" + doorID + ")";
            //}
            //else
            //{
            //    filter += ")";
            //}
            string filter = " 1=1";
            if (!string.IsNullOrEmpty(searchText))
            {
                filter += " and (StationNum like '%"+ searchText + "%' or StationName like '%"+ searchText + "%')";
            }
            string orderby = sort + " " + sortOrder;
            var f_stationTypeID = (int)Model.CustomizedClass.Enum.泵房类型;
            var datalist = _AccessControlService.GetAC_StationTable(stationid,f_stationTypeID, pageIndex, pageSize, orderby,filter,ref Totalcount);
            return Json(new { total = Totalcount, rows = Newtonsoft.Json.JsonConvert.SerializeObject(datalist) });
        }
        #endregion
    }
}