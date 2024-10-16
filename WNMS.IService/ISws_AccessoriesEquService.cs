using System;
using System.Collections.Generic;
using System.Text;
using WNMS.Model.DataModels;

namespace WNMS.IService
{
    public partial interface ISws_AccessoriesEquService:IBaseService
    {
       IEnumerable<dynamic> QueryAccessoryEquTable(bool IsAdmin, int UserID, int pageindex, int pagesize, string order, string filter, ref int Totalcount);
       IEnumerable<dynamic> GetAccessoryTable(String conponentid, int startindex, int pageSize, string orderItems, string filterString, ref int Totalcount);
       IEnumerable<dynamic> GetAcce_DeviceTable(bool IsAdmin, int UserID,int startindex, int pageSize, string orderItems, string filterString, ref int Totalcount);
       int AddAccessoryEqu(SwsAccessoriesEqu ae, SwsAccessories a);
       int EditeAccessoryEqu(SwsAccessoriesEqu ae_new, SwsAccessories a, int ae_oldQuantity);
       int DeleteAccessoryEqu(List<long> A_Equids);
        #region 器件地图
        IEnumerable<dynamic> GetStationByAccessory(bool IsAdmin, int UserID, string searchText);
        IEnumerable<dynamic> GetAccessBySId(int stationid,int f_itemid);
        #endregion
    }
}
