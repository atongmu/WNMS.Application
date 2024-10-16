using System;
using System.Collections.Generic;


namespace WNMS.IService
{
    public partial interface IWO_AreaInfoService : IBaseService
    {
        IEnumerable<dynamic> GetAreaInfo(string areaIDs);
        IEnumerable<dynamic> GetRtuInfoOfArea(int areaID, string deviceName);
        IEnumerable<dynamic> GetRtuInfoOfAreaPage(int areaID, string sort, int pageSize, int pageIndex, string devicename, ref int TotalCount);
        bool DeleteAreaInfo(int id, ref List<string> areanames);
        IEnumerable<dynamic> SearchAreaTree(string areaName);
        int SetAreaInfo(int areaID, int pid, string areaName, string fillColor, string points, int type);
    }
}
