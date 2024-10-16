using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using WNMS.Model.CustomizedClass;
using WNMS.Model.DataModels;

namespace WNMS.IService
{
    public partial interface IWO_AssignmentPlanService : IBaseService
    {
        IEnumerable<dynamic> GetCreaterAndInspector();
        PageResult<Assignment> LoadAssignmentList(Expression<Func<Assignment, bool>> funcWhere, int pageSize, int pageIndex, string funcOrderby, bool isAsc = true);
        IEnumerable<dynamic> GetAssignmentPlanInfo(long id);
        IEnumerable<dynamic> GetUserInfoList(long DMAID);
        int AddPlan(WoAssignmentPlan plan, List<WoPlanInspectO> planInspectO);
        int EditePlan(WoAssignmentPlan plan, List<WoPlanInspectO> planInspectO);
        int DeletePlan(string planIDs);
        IEnumerable<dynamic> GetWatchData(long planID);
        IEnumerable<dynamic> GetWatchDetailData(long planID, int type);
        IEnumerable<dynamic> GetFeedDataInfo(long planID);
        IEnumerable<dynamic> GetDeviceData(long deviceID);
        int AssignPlan(string planIDs);
        string CreateSchedule(long id);
        string EditSchedule(WoInspectionPlan isp);
        void DeleteSchedule(List<long> planids);
        IEnumerable<dynamic> GetTemplateDetailData(long planID);
        /// <summary>
        /// 更新上传反馈
        /// </summary>
        /// <param name="woInspectPlanCheck">反馈信息</param>
        /// <param name="woAssignmentPlan">巡检信息</param>
        /// <param name="woPlanInspectO">巡检设备信息</param>
        /// <returns></returns>
        int AddInspectPlanCheckInfo(WoInspectPlanCheck woInspectPlanCheck, WoAssignmentPlan woAssignmentPlan, WoPlanInspectO woPlanInspectO);
        /// <summary>
        /// 审核巡检工单
        /// </summary>
        /// <param name="woAssignmentPlan">巡检单信息</param>
        /// <param name="woPlanInspectOs">巡检设备信息</param>
        /// <param name="woInsExtension">巡检延期申请信息</param>
        /// <returns></returns>
        int AuditAss(WoAssignmentPlan woAssignmentPlan, List<WoPlanInspectO> woPlanInspectOs, List<WoInsExtension> woInsExtension);
        /// <summary>
        /// 审核反馈设备
        /// </summary>
        /// <param name="woPlanInspectO">设备表</param>
        /// <param name="woInspectPlanCheck">设备反馈表</param>
        /// <returns></returns>
        int AuditEq(WoPlanInspectO woPlanInspectO, WoInspectPlanCheck woInspectPlanCheck);
    }
}
