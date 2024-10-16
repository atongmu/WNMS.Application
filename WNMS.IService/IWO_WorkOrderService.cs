using System;
using System.Collections.Generic;
using System.Text;
using WNMS.Model.CustomizedClass;
using WNMS.Model.DataModels;

namespace WNMS.IService
{
    public partial interface IWO_WorkOrderService : IBaseService
    {
        /// <summary>
        /// 查询处理工单
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <param name="State"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="field"></param>
        /// <param name="order"></param>
        /// <param name="eventsType"></param>
        /// <param name="UserID"></param>
        /// <param name="message"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        IEnumerable<dynamic> GetWorkTable(string beginDate, string endDate, string State, int pageSize, int pageIndex, string field, string order, string eventsType, long UserID, string message, ref int totalCount);
        /// <summary>
        /// 查询事件信息
        /// </summary>
        /// <param name="WOID"></param>
        /// <returns></returns>
        dynamic GetWO_EventData(long WOID);
        /// <summary>
        /// 获取工单操作历史
        /// </summary>
        /// <param name="WOID"></param>
        /// <param name="oType"></param>
        /// <returns></returns>
        List<dynamic> GetWO_OpData(long WOID, int oType);
        /// <summary>
        /// 获取工单操作历史
        /// </summary>
        /// <param name="WOID"></param>
        /// <returns></returns>
        List<dynamic> GetWO_OpInfo(long WOID);
        /// <summary>
        /// 获取延期申请信息
        /// </summary>
        /// <param name="woid"></param>
        /// <returns></returns>
        IEnumerable<dynamic> LoadExtensionData(long woid);
        /// <summary>
        /// 获取班组人员信息
        /// </summary>
        /// <returns></returns>
        IEnumerable<TeamUser> GetTeamInfo();
        /// <summary>
        /// 派工单
        /// </summary>
        /// <param name="newWo"></param>
        /// <param name="wo"></param>
        /// <param name="woForward"></param>
        /// <returns></returns>
        int AddDispatchWo(WoWorkOrder newWo, WoWooperation wo, WoForward woForward);
        /// <summary>
        /// 退单
        /// </summary>
        /// <param name="woWorkOrder">工单信息</param>
        /// <param name="woWooperation">工单操作</param>
        /// <returns></returns>
        int ChargebackWO(WoWorkOrder woWorkOrder, WoWooperation woWooperation);
        /// <summary>
        /// 退单申请
        /// </summary>
        /// <param name="woWorkOrder">工单信息</param>
        /// <param name="woWooperation">工单操作</param>
        ///  <param name="woForward">退单申请</param>
        /// <returns></returns>
        int ChargebackWOF(WoWorkOrder woWorkOrder, WoWooperation woWooperation, WoForward woForward);
        /// <summary>
        /// 审核工单
        /// </summary>
        /// <param name="woWorkOrder"></param>
        /// <param name="woWooperation"></param>
        /// <returns></returns>
        int ReviewWO(WoWorkOrder woWorkOrder, WoWooperation woWooperation);
        /// <summary>
        /// 延期申请
        /// </summary>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <param name="orderby"></param>
        /// <param name="filter"></param>
        /// <param name="Totalcount"></param>
        /// <returns></returns>
        IEnumerable<dynamic> GetWoEInfoTable(int pageindex, int pagesize, string orderby, string filter, ref int Totalcount);
        /// <summary>
        /// 审核延期申请通过
        /// </summary>
        /// <param name="woWoextension"></param>
        /// <param name="woWorkOrder"></param>
        /// <param name="woWooperation"></param>
        /// <returns></returns>
        int EditWoExtension(WoWoextension woWoextension, WoWorkOrder woWorkOrder, WoWooperation woWooperation);
        /// <summary>
        /// 审核延期申请未通过
        /// </summary>
        /// <param name="woWoextension"></param>
        /// <param name="woWooperation"></param>
        /// <returns></returns>
        int EditNoWoExtension(WoWoextension woWoextension, WoWooperation woWooperation);
        /// <summary>
        /// 工单转发
        /// </summary>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <param name="orderby"></param>
        /// <param name="filter"></param>
        /// <param name="Totalcount"></param>
        /// <returns></returns>
        IEnumerable<dynamic> GetWoForwardTable(int pageindex, int pagesize, string orderby, string filter, ref int Totalcount);
        /// <summary>
        /// 获取设备负责人信息
        /// </summary>
        /// <param name="rtuid"></param>
        /// <returns></returns>
        IEnumerable<TeamUser> LoadUserInfoByRtuid(long rtuid);
        /// <summary>
        /// 插入事件和工单信息
        /// </summary>
        /// <param name="woWorkOrder"></param>
        /// <param name="woEvents"></param>
        /// <returns></returns>
        int InsertWoandEvent(WoWorkOrder woWorkOrder, WoEvents woEvents);
        /// <summary>
        /// 工单退单
        /// </summary>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <param name="orderby"></param>
        /// <param name="filter"></param>
        /// <param name="Totalcount"></param>
        /// <returns></returns>
        IEnumerable<dynamic> GetWoTrunTable(int pageindex, int pagesize, string orderby, string filter, ref int Totalcount);
        /// <summary>
        /// 审核转发申请通过
        /// </summary>
        /// <param name="woWorkOrder"></param>
        /// <param name="woWooperation"></param>
        /// <param name="woForward"></param>
        /// <returns></returns>
        int EditWoForward(WoWorkOrder woWorkOrder, WoWooperation woWooperation, WoForward woForward, WoWorkOrder woWorkOrder1, WoWooperation woWooperation1);
        /// <summary>
        /// 手动派发工单
        /// </summary>
        /// <param name="woEvents">事件信息</param>
        /// <param name="woWorkOrder">工单信息</param>
        /// <param name="woWooperation">工单操作信息</param>
        /// <returns></returns>
        int AddManualDispatch(WoEvents woEvents, WoWorkOrder woWorkOrder, WoWooperation woWooperation);
        /// <summary>
        /// 网站退单操作
        /// </summary>
        /// <param name="woWorkOrder">工单信息</param>
        /// <param name="woWooperation">工单操作</param>
        /// <param name="woForward">退单申请</param>
        /// <param name="woWoextension">延期申请</param>
        /// <returns></returns>
        int ChargebackWOW(WoWorkOrder woWorkOrder, WoWooperation woWooperation, WoForward woForward, WoWoextension woWoextension);
        #region
        /// <summary>
        /// 根据用户id获取用户处理工单
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        IEnumerable<dynamic> LoadWoListByUserID(long userId);
        /// <summary>
        /// 获取单个工单信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        IEnumerable<Api_WoEvent> LoadWoById(long Id);
        /// <summary>
        /// 添加工单操作信息
        /// </summary>
        /// <param name="woWooperation"></param>
        /// <param name="woWorkOrder"></param>
        /// <param name="woEvents"></param>
        /// <returns></returns>
        int AddWoOpInfo(WoWooperation woWooperation, WoWorkOrder woWorkOrder, WoEvents woEvents);
        /// <summary>
        /// 获取工单转发次数
        /// </summary>
        /// <param name="ID">工单ID</param>
        /// <returns></returns>
        IEnumerable<dynamic> GetWOForwardCount(long ID);
        /// <summary>
        /// 转发工单接口
        /// </summary>
        /// <param name="newWo">新工单信息</param>
        /// <param name="oldWo">转发工单信息</param>
        /// <param name="Newwoop">新工单操作数据</param>
        /// <param name="oldwoop">转发工单操作数据</param>
        /// <returns></returns>
        int AddWOForward(WoWorkOrder newWo, WoWorkOrder oldWo, WoWooperation Newwoop, WoWooperation oldwoop);
        /// <summary>
        /// 生成事件和工单接口
        /// </summary>
        /// <param name="woWorkOrder">工单信息</param>
        /// <param name="woEvents">事件信息</param>
        /// <param name="woResource">资源信息</param>
        /// <returns></returns>
        int AddWOAndEvent(WoWorkOrder woWorkOrder, WoEvents woEvents, WoResource woResource);
        /// <summary>
        /// 生成事件和工单接口
        /// </summary>
        /// <param name="woWorkOrder">工单信息</param>
        /// <param name="woEvents">事件信息</param> 
        /// <returns></returns>
        int AddWOAndEvent(WoWorkOrder woWorkOrder, WoEvents woEvents);
        /// <summary>
        /// 根据用户id获取用户历史工单
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="beginDate"></param>
        /// <returns></returns>
        IEnumerable<dynamic> LoadHisWoListByUserID(long userId, string beginDate);
        #endregion
    }
}
