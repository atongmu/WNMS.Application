using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using WNMS.Model.CustomizedClass;
using WNMS.Model.DataModels;

namespace WNMS.IService
{
    public partial interface IWO_InspectionPlanService : IBaseService
    {
        #region 模板管理
        IEnumerable<dynamic> GetFeedBackItems(long templateID, int feedback_fitem);
        int AddTemplate(WoTemplateInfo tt, List<WoFbOfTemplate> fbTemplateList);
        int EditeTemplate(WoTemplateInfo tt, List<WoFbOfTemplate> fbTemplateList);
        int DeleteTemplate(long templateid);
        int DeleteFeedBack(int id);
        #endregion
        IEnumerable<dynamic> LoadCreaterAndInspector();
        IEnumerable<dynamic> QueryInspectPlanTable(int pageindex, int pagesize, string ordertems, string filter, string Travel_fitemid, string Cycle_fitemid, ref int Totalcount);
        IEnumerable<dynamic> GetAreaInfoByID(int areaid);
        IEnumerable<AllotDevice> GetAllDeviceByID(int areaid, string searchtext);
        IEnumerable<AllotDevice> GetAllotDevice(DataTable tvpDt);
        int AddInspectPlan(WoInspectionPlan p, List<WoPlanInspectO> planInspectO);
        int EditeInspectPlan(WoInspectionPlan p, List<WoPlanInspectO> planInspectO);
        int DeleteInspectPlan(List<long> planids);

        /// <summary>
        /// 巡检延期申请
        /// </summary>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <param name="orderby"></param>
        /// <param name="filter"></param>
        /// <param name="Totalcount"></param>
        /// <returns></returns>
        IEnumerable<dynamic> GetInsEInfoTable(int pageindex, int pagesize, string orderby, string filter, ref int Totalcount);
        /// <summary>
        /// 审核巡检延期申请
        /// </summary>
        /// <param name="woInsExtension"></param>
        /// <param name="woAssignmentPlan"></param>
        /// <returns></returns>
        int EditInsExtension(WoInsExtension woInsExtension, WoAssignmentPlan woAssignmentPlan);
        /// <summary>
        /// 巡检转发
        /// </summary>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <param name="orderby"></param>
        /// <param name="filter"></param>
        /// <param name="Totalcount"></param>
        /// <returns></returns>
        IEnumerable<dynamic> GetInsForwardTable(int pageindex, int pagesize, string orderby, string filter, ref int Totalcount);
        /// <summary>
        /// 审核巡检转发申请
        /// </summary>
        /// <param name="woInsExtension"></param>
        /// <param name="woAssignmentPlan"></param>
        /// <param name="woPlanInspectOs"></param>
        /// <returns></returns>
        int EditInsForward(WoInsForward woInsForward, WoAssignmentPlan woAssignmentPlan, List<WoPlanInspectO> woPlanInspectOs);
        /// <summary>
        /// 审核驳回巡检转发申请
        /// </summary>
        /// <param name="woInsExtension"></param>
        /// <param name="woAssignmentPlan"></param>
        /// <param name="woPlanInspectOs"></param>
        /// <param name="newwoPlanInspectOs"></param>
        /// <returns></returns>
        int TrunInsForward(WoInsForward woInsForward, WoAssignmentPlan woAssignmentPlan, List<WoPlanInspectO> woPlanInspectOs, List<WoPlanInspectO> newwoPlanInspectOs);
        /// <summary>
        /// 巡检退单
        /// </summary>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <param name="orderby"></param>
        /// <param name="filter"></param>
        /// <param name="Totalcount"></param>
        /// <returns></returns>
        IEnumerable<dynamic> GetInsTurnTable(int pageindex, int pagesize, string orderby, string filter, ref int Totalcount);
        /// <summary>
        /// 审核巡检退单申请
        /// </summary>
        /// <param name="woInsForward"></param>
        /// <param name="woAssignmentPlan"></param>
        /// <param name="woPlanInspectOs"></param>
        /// <returns></returns>
        int EditInsTurn(WoInsForward woInsForward, WoAssignmentPlan woAssignmentPlan, List<WoPlanInspectO> woPlanInspectOs);
        #region 接口
        IEnumerable<dynamic> GetAssignList(int Travel_fitemid, int Cycle_fitemid, long userId);
        /// <summary>
        /// 获取简单的巡检信息 用于列表展示
        /// </summary>
        /// <param name="Travel_fitemid"></param>
        /// <param name="Cycle_fitemid"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> GetSinAssignList(string filter);
        IEnumerable<dynamic> GetEqui_Assign(long planid);
        /// <summary>
        /// 更新巡检转发设备状态
        /// </summary>
        /// <param name="woAssignmentPlan">新巡检信息</param>
        /// <param name="woPlanInspectO">巡检设备</param>
        /// <param name="oldwoPlanInspectO">旧设备信息</param>
        /// <param name="woInsForward">转发申请信息</param>
        /// <returns></returns>
        int EditObjectForward(WoAssignmentPlan woAssignmentPlan, List<WoPlanInspectO> woPlanInspectO, List<WoPlanInspectO> oldwoPlanInspectO, WoInsForward woInsForward);

        IEnumerable<dynamic> GetAssignListByID(int Travel_fitemid, int Cycle_fitemid, long planid);
        IEnumerable<dynamic> GetAssignListHistory(int Travel_fitemid, int Cycle_fitemid, string beginDate, long userId);
        #endregion
    }
}
