using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using WNMS.Model.CustomizedClass;
using WNMS.Model.DataModels;

namespace WNMS.IService
{
    public partial interface IGD_EventsService:IBaseService
    {
        PageResult<SEvents> LoadEventInfoList(Expression<Func<SEvents, bool>> funcWhere, string state, int pageSize, int pageIndex, string funcOrderby, int userID, bool ispage = true, bool isAsc = true);
        IEnumerable<SEvents> GetIncidentByID(long id);
        IEnumerable<dynamic> GetTreateTaskData(string eventID);
        IEnumerable<TeamUser> GetTeamInfo();
        int AddWorkerOrder(GdWorkOrder order, GdWooperation wo, GdEvents e);
        int EditWorkOrderAudit(GdWorkOrder w, GdWooperation woo);
    }
}
