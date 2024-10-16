using System;
using System.Collections.Generic;
using System.Text;
using WNMS.Model.DataModels;

namespace WNMS.IService
{
    public partial interface IGD_InspectionService : IBaseService
    {
        IEnumerable<dynamic> GetInspectionTable(bool isadmin, int pageindex, int pagesize, string orderby, string filter, ref int Totalcount);
        IEnumerable<dynamic> GetTeamUserTree();
        int AddEventIn(GdInspection gdInspection, GdEvents gdEvents);
        int AddEventMa(GdMaintain gdMaintain, GdEvents gdEvents);
        int AddEventRe(GdRepair gdRepair, GdEvents gdEvents);
        int DeleteXJFeedBack(List<int> XJIDs);
        IEnumerable<dynamic> GetRepairTable(bool isadmin, int pageindex, int pagesize, string orderby, string filter, ref int Totalcount);
        IEnumerable<dynamic> GetDeviceList_ByStationID(string tablename, int stationid, long deviceid);
        int UpdateRepairInfo(GdRepair p, List<GdResource> resourceList);
        int DeleteWXFeedBack(List<long> WXIDs, string RootPath);
        IEnumerable<dynamic> GetMainTainTable(bool isadmin, int pageindex, int pagesize, string orderby, string filter, ref int Totalcount);
        int UpdateMainTainInfo(GdMaintain p, List<GdResource> resourceList);
        int DeleteBYFeedBack(List<long> WXIDs, string RootPath);
        IEnumerable<dynamic> GetGD_SelectStationTable(bool isadmin, int stationid, int f_typeItemID, int startindex, int pageSize, string orderItems, string filterString, ref int Totalcount);
        #region 业务反馈相关接口
        GdEvents GetGdEventByWorkID(long workid);
        IEnumerable<dynamic> GetModelByID(int type, int id);
        #endregion
    }
}
