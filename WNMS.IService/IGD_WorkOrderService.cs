using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using WNMS.Model.DataModels;

namespace WNMS.IService
{
    public partial interface IGD_WorkOrderService : IBaseService
    {
        //查询处理工单
        IEnumerable<dynamic> GetWorkTable(string beginDate, string endDate, string State, int pageSize, int pageIndex, string field, string order, string eventsType, long UserID, string message, ref int totalCount);
        //退单
        int EditWo(GdWorkOrder gdWorkOrder, GdWooperation gdWooperation);
        //事件详情
        dynamic GetWO_EventData(long WOID);
        //获取工单操作历史 
        List<dynamic> GetWO_OpData(long WOID, int oType);
        //获取工单延期信息
        IEnumerable<dynamic> LoadExtensionData(long woid);
        //移交工单
        int AddWOTransfer(GdWoextension gD_WOReview, GdWorkOrder newWo, GdWooperation wo, GdWorkOrder oldWo, GdWooperation newwoop);
        //延期申请
        IEnumerable<dynamic> GetWoEInfoTable(int pageindex, int pagesize, string orderby, string filter, ref int Totalcount);
        //审核延期申请通过
        int EditWoExtension(GdWoextension gdWoextension, GdWorkOrder gdWorkOrder, GdWooperation gdWooperation);
        //审核延期申请未通过
        int EditNoWoExtension(GdWoextension gdWoextension, GdWooperation gdWooperation);
        //移交申请
        IEnumerable<dynamic> GetWoReInfoTable(int pageindex, int pagesize, string orderby, string filter, ref int Totalcount);
        //审核退单申请通过
        int EditWoOkReview(GdWoreview gdWoreview, GdWorkOrder gdWorkOrder, GdWooperation gdWooperation);
        //审核退单申请未通过
        int EditWoNoReview(GdWoreview gdWoreview, GdWooperation gdWooperation);
        //审核移交申请通过
        int AddWOTransfer(GdWoreview gdWoreview, GdWorkOrder gdWorkOrder, GdWooperation woop, GdWorkOrder woInfo, GdWooperation newwoop);
        //审核移交申请未通过
        int AddWONoTransfer(GdWoreview gdWoreview, GdWooperation gdWooperation);
        #region 工单处理接口
        //获取工单事件信息
        IEnumerable<dynamic> LoadWoEvent(long userId);
        IEnumerable<dynamic> LoadWoById(long Id);
        //添加工单操作步骤
        int AddWoOp(GdWooperation gd_WOOperation, GdWorkOrder gdWorkOrder, GdEvents gdEvents);
        #endregion
    }
}
