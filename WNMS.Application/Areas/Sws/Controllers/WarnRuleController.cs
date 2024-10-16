using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WNMS.Application.Utility;
using WNMS.Application.Utility.Filters;
using WNMS.IService;
using WNMS.Model.CustomizedClass;
using WNMS.Model.DataModels;
namespace WNMS.Application.Areas.Sws.Controllers
{
    [Area("Sws")]
    public class WarnRuleController : Controller
    {
        private IWarnRuleService _WarnRuleService = null;
        private IWarnRule_DetailService _WarnRule_DetailService = null;
        private ISys_DataItemDetailService _DataItemDetailService = null;
        private ISws_DataInfoService _DataInfoService = null;
        private ISysUserService _sysUserService = null;
        
        public WarnRuleController(IWarnRuleService warnRuleService,
            IWarnRule_DetailService warnRule_DetailService,
            ISys_DataItemDetailService sys_DataItemDetailService,
            ISws_DataInfoService sws_DataInfoService,
            ISysUserService sysUserService) {

            _WarnRuleService = warnRuleService;
            _WarnRule_DetailService = warnRule_DetailService;
            _DataItemDetailService = sys_DataItemDetailService;
            _DataInfoService = sws_DataInfoService;
            _sysUserService = sysUserService;
        }
        public IActionResult Index()
        {
            return View();
        }
        #region 预警规则
        [HttpPost]
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult QueryTable(int pagesize,int pageindex,string SearchText,string order,string sort)
        {
            Expression<Func<WarnRule, bool>> funcWhere = r => true;
            bool flag = true;
            //查询条件

            if (!string.IsNullOrEmpty(SearchText))
            {
                funcWhere = r => r.RuleText.Contains(SearchText);
            }
            if (order == "desc")
            {
                flag = false;
            }
            var infoList = _WarnRuleService.QueryPage(funcWhere, pagesize, pageindex, sort, flag);
            return Json(new { total = infoList.TotalCount, rows = Newtonsoft.Json.JsonConvert.SerializeObject(infoList.DataList.ToList()) });
        }
        public IActionResult AllotRule(int id)
        {
            ViewBag.ruleid = id;
            var compare_fitem = (int)WarnRule_enum.比较符号;
            var relate_fitem = (int)WarnRule_enum.关系符号;

            var DataItemDetail = _DataItemDetailService.Query<SysDataItemDetail>(r => (r.FItemId == compare_fitem || r.FItemId == relate_fitem) && r.IsEnable == true);
            ViewBag.DataItemDetail = DataItemDetail;
            var partition = 0;
            if (id != 0)
            {
                partition = (int)_WarnRuleService.Query<WarnRule>(r => r.RuleId == id).FirstOrDefault()?.Partition;
            }
            ViewBag.partition = partition;
            return View();
        }
        //获取规则详情
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult GetRuleDetailData(int RuleId)
        {
            var datalist = _WarnRuleService.GetDetailRuleByID(RuleId).ToList();
            
            return Json(new
            {
                datalist
            });

        }
        //编辑规则表单,编辑子规则
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult EditeRuleDetail(int detailID)
        {
            var datalist = _WarnRuleService.GetRuleByDetailID(detailID).FirstOrDefault();

            return Json(new
            {
                data= datalist
            });
        }
        //模拟量选择
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult SelectAnalog(int id,int partition)
        {
            IQueryable<SwsDataInfo> Analogs;
            if (partition != 0)
            {
                Analogs = _DataInfoService.Query<SwsDataInfo>(r => r.DeviceType == 1 && r.DataType == 1 &&(r.Region== partition||r.Region==0));//region==0是共用
            }
            else
            {
                Analogs = _DataInfoService.Query<SwsDataInfo>(r => r.DeviceType == 1 && r.DataType == 1);
            }
            ViewBag.dataid = id;
            ViewBag.analogs = Analogs;
            ViewBag.partition = partition;
            return View();
        }
        //模拟量查询
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult SearchAnalog(string searchText,int partition)
        {
            IQueryable<SwsDataInfo> Analogs;
            if (partition != 0)
            {
                Analogs = _DataInfoService.Query<SwsDataInfo>(r => r.DeviceType == 1 && r.DataType == 1 && r.Cnname.Contains(searchText)&& (r.Region == partition || r.Region == 0));
            }
            else
            {
                Analogs = _DataInfoService.Query<SwsDataInfo>(r => r.DeviceType == 1 && r.DataType == 1 && r.Cnname.Contains(searchText));
            }
            return Json(new
            {
                Analogs
            });
        }
        //提交表单，提交规则
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult SetRule(string info, int RuleId,byte partition)
        {
            try
            {
                 
                WarnRuleDetail rd = Newtonsoft.Json.JsonConvert.DeserializeObject<WarnRuleDetail>(info);

                var IDIsChange = false;
                if (RuleId == 0)
                {
                    IDIsChange = true;
                    RuleId = _sysUserService.QueryID("RuleID", "WarnRule");
                }
                rd.ParentId = RuleId;
                //判断子规则编号是否存在
                var oldRuleDetail = _WarnRule_DetailService.Query<WarnRuleDetail>(r => r.Num == rd.Num && r.ParentId == rd.ParentId && r.Id != rd.Id).FirstOrDefault();
                if (oldRuleDetail != null)
                {
                    return Json(new
                    {
                        str = "编号重复，请重新输入"
                    });
                }
                
                if (rd.Id == 0)
                {
                    rd.Id = _sysUserService.QueryID("ID", "WarnRule_Detail");
                }
                var str = "";
                
                if (_WarnRuleService.SetRuleInfo(rd, RuleId,partition) > 0)
                {
                    str = "ok";
                }
                else
                {
                    str = "no";
                }
                return Json(new
                {
                    str,
                    IDIsChange,
                    RuleId
                });
            }
            catch (Exception e)
            {
                return Content("");
            }
        }
        //删除子规则
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult DeleteDetailRule(int detailID,int ruleID)
        {
            if (_WarnRuleService.Del_DetailRule(detailID, ruleID) > 0)
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }
        }
        //删除规则
        public IActionResult DeleteRules(string ids)
        {
            List<int> ruleIDs = new List<string>(ids.Split(',')).ConvertAll(r => int.Parse(r));
            if (_WarnRuleService.DeleteRules(ruleIDs) > 0)
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }
        }
        #endregion
        #region 预警报告
        public IActionResult WarnReport()
        {
            return View();
        }
        //获取预警数据
        [HttpPost]
        [TypeFilter(typeof(IgonreActionFilter))]
        public async Task<IActionResult> GetWarnReportData(int pageindex,int pagesize,string searchTxt)
        {
            try
            {
                 int userID = int.Parse(User.FindFirstValue("UserID"));
                var user = _sysUserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
                int user_id = user.UserId;
                if (user.IsAdmin)
                {
                    user_id = 0;
                }
                Expression<Func<warnReport, bool>> funcWhere = null;
                if (!string.IsNullOrEmpty(searchTxt))
                {
                    funcWhere = r => r.DeviceName.Contains(searchTxt) || r.RuleText.Contains(searchTxt);
                }
                PageResult<warnReport> dataList = _WarnRuleService.LoadWarnDataList(user_id, funcWhere, pagesize, pageindex, "DeviceName", true);
                PartialView("_warnReportTable", dataList.DataList);
                string dataTable = await ViewToString.RenderPartialViewToString(this, "_warnReportTable");
                return Json(new
                {
                    dataTable,
                    TotalPage = Math.Ceiling((float)dataList.TotalCount / (float)pagesize),


                });
            }
            catch (Exception e)
            {
                return Content("");

            }
        }
        //查询当前账号下是否有预警信息
        [HttpPost]
        [TypeFilter(typeof(IgonreActionFilter))]
        public IActionResult HaveWarninfo()
        {
            int userID = int.Parse(User.FindFirstValue("UserID"));
            var user = _sysUserService.Query<SysUser>(u => u.UserId == userID).FirstOrDefault();
            int user_id = user.UserId;
            if (user.IsAdmin)
            {
                user_id = 0;
            }
            var num = _WarnRuleService.GetWarnCount(user_id);
            return Json(new
            {
                num
            });
        }
        #endregion
    }
}