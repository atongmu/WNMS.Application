using System;
using System.Collections.Generic;
using System.Text;
using WNMS.Model.DataModels;

namespace WNMS.IService
{
    public partial interface ISws_AccessControlService : IBaseService
    {
       IEnumerable<dynamic> GetStationTreeOfState(string goodRtuids, bool isadmin, int Userid, string stationname);
       IEnumerable<dynamic> GetAcessControlTable(int f_BrandItemID, int startindex, int pageSize, string orderItems, string filterString, ref int Totalcount);
       int DeleteAccessControl(List<int> doorids);
       IEnumerable<dynamic> GetAC_StationTable(int stationid,int f_typeItemID, int startindex, int pageSize, string orderItems, string filterString, ref int Totalcount);

       int InsertAccessControl(SwsAccessControl a);
       int UpdateAccessControl(SwsAccessControl a);
       IEnumerable<dynamic> GetAccessHistory(int stationid, int pageindex, int pagesize, string filter, string orderby, ref int Totalcount);
    }
}
